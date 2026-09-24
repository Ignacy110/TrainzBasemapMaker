// Trainz Basemap Maker
// https://github.com/Ignacy110/TrainzBasemapMaker
//
// Copyright (C) 2026 Ignacy110 (http://github.com/Ignacy110)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, see (http://www.gnu.org/licenses/).

using Microsoft.Web.WebView2.Core;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using TrainzBasemapMaker.Classes;
using TrainzBasemapMaker.Classes.TrainzTerrain;

namespace TrainzBasemapMaker
{
    public partial class TerrainGridToolForm : Form
    {
        private class SelectedTileModel
        {
            public int Order { get; set; }
            public int I { get; set; }
            public int J { get; set; }
            public long X { get; set; }
            public long Y { get; set; }
        }

        private readonly TrainzFileManager _fileManager = new TrainzFileManager();
        private readonly ToolTip _warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "Blad wprowadzania" };
        private readonly List<SelectedTileModel> _selectedTiles = new List<SelectedTileModel>();
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isDownloading = false;
        private long? _currentAnchorX;
        private long? _currentAnchorY;
        private TerrainRouteInfo? _loadedRoute;
        private readonly Dictionary<(int SegmentX, int SegmentY), MapGridPart> _loadedGndBlocks = new Dictionary<(int SegmentX, int SegmentY), MapGridPart>();

        public TerrainGridToolForm()
        {
            InitializeComponent();

            // Set initial control states
            textBoxDestinationFolder.Text = "Nowa_Trasa";
            textBoxKuidPart1.Text = Properties.Settings.Default.DefaultKuidFirstPart ?? "123456";

            radioButtonEpsg2180.Checked = true;
            radioButtonModeClick.Checked = true;
            radioButtonElevationAbsolute.Checked = true;

            buttonLoadRoute.Enabled = false;
            buttonDeleteRoute.Enabled = false;

            UpdateNextFreeKuidPart2();
            toolStripStatusLabel1.Text = "LPM: Kliknij lub przeciagnij pedzlem, aby zaznaczyc | PPM: Przesuwanie mapy";
        }

        private async void TerrainGridToolForm_Load(object? sender, EventArgs e)
        {
            this.Size = Properties.Settings.Default.TerrainGridToolFormSize;
            this.WindowState = Properties.Settings.Default.TerrainGridToolFormState;
            ThemeManager.ApplyTheme(this);

            RoutesListBoxRefresh();

            await InitBrowser();
        }

        private async Task InitBrowser()
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);
                webView21.CoreWebView2.Settings.UserAgent = "TrainzBasemapMaker/v0.5.0 (https://github.com/Ignacy110/TrainzBasemapMaker)";

                string indexPath = Path.Combine(Application.StartupPath, "Forms", "TerrainGridToolForm", "Web", "grid_map.html");
                if (!File.Exists(indexPath))
                {
                    indexPath = Path.Combine(Application.StartupPath, "Forms", "GridToolForm", "Web", "grid_map.html");
                }

                webView21.CoreWebView2.NavigationCompleted += async (sender, args) =>
                {
                    if (args.IsSuccess)
                    {
                        // Trainz baseboards are always 720m x 720m
                        await webView21.CoreWebView2.ExecuteScriptAsync("setTileSize(720);");
                        await webView21.CoreWebView2.ExecuteScriptAsync("setCoordinateSystem('EPSG:2180');");
                    }
                };

                webView21.CoreWebView2.Navigate("file:///" + indexPath);
                webView21.CoreWebView2.WebMessageReceived += WebView21_WebMessageReceived;
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.log", ex.ToString());
                toolStripStatusLabel1.Text = "Blad inicjalizacji mapy!";
                MessageBox.Show("Wystapil blad podczas inicjalizacji mapy:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void RoutesListBoxRefresh()
        {
            listBoxRoutes.Items.Clear();
            var routes = _fileManager.GetTerrainRoutes();
            listBoxRoutes.Items.AddRange(routes.ToArray());
            buttonLoadRoute.Enabled = listBoxRoutes.SelectedItem != null;
            buttonDeleteRoute.Enabled = listBoxRoutes.SelectedItem != null;
        }

        private void listBoxRoutes_SelectedIndexChanged(object? sender, EventArgs e)
        {
            buttonLoadRoute.Enabled = listBoxRoutes.SelectedItem != null;
            buttonDeleteRoute.Enabled = listBoxRoutes.SelectedItem != null;

            if (listBoxRoutes.SelectedItem is TerrainRouteInfo route)
            {
                textBoxDestinationFolder.Text = route.RouteName;
                textBoxKuidPart1.Text = route.KuidPart1;
                textBoxKuidPart2.Text = route.KuidPart2;
            }
        }

        private async void buttonLoadRoute_Click(object? sender, EventArgs e)
        {
            await LoadSelectedRoute();
        }

        private async void listBoxRoutes_DoubleClick(object? sender, EventArgs e)
        {
            await LoadSelectedRoute();
        }

        private async Task LoadSelectedRoute()
        {
            if (listBoxRoutes.SelectedItem is not TerrainRouteInfo selectedRoute)
            {
                MessageBox.Show("Wybierz trasę z listy do wczytania.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (webView21.CoreWebView2 == null)
            {
                MessageBox.Show("Komponent mapy jeszcze się inicjalizuje. Spróbuj ponownie za chwilę.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string gndPath = Path.Combine(selectedRoute.FolderPath, "mapfile.gnd");
            if (!File.Exists(gndPath))
            {
                MessageBox.Show($"W folderze trasy nie znaleziono pliku mapfile.gnd:\n{selectedRoute.FolderPath}", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                byte[] gndData = File.ReadAllBytes(gndPath);
                var parts = GndReader.ReadGndFile(gndData);

                _loadedGndBlocks.Clear();
                foreach (var p in parts)
                {
                    _loadedGndBlocks[(p.SegmentX, p.SegmentY)] = p;
                }

                _loadedRoute = selectedRoute;

                textBoxDestinationFolder.Text = selectedRoute.RouteName;
                textBoxKuidPart1.Text = selectedRoute.KuidPart1;
                textBoxKuidPart2.Text = selectedRoute.KuidPart2;

                if (selectedRoute.Epsg == "EPSG:3857")
                    radioButtonEpsg3857.Checked = true;
                else
                    radioButtonEpsg2180.Checked = true;

                if (selectedRoute.IsRelative)
                    radioButtonElevationRelative.Checked = true;
                else
                    radioButtonElevationAbsolute.Checked = true;

                buttonStartDownload.Text = "Aktualizuj teren (map.gnd)";

                if (selectedRoute.AnchorX.HasValue && selectedRoute.AnchorY.HasValue && selectedRoute.Tiles.Count > 0)
                {
                    _currentAnchorX = selectedRoute.AnchorX.Value;
                    _currentAnchorY = selectedRoute.AnchorY.Value;

                    var payload = new
                    {
                        epsg = selectedRoute.Epsg,
                        anchor = new { x = selectedRoute.AnchorX.Value, y = selectedRoute.AnchorY.Value },
                        tiles = selectedRoute.Tiles.Select(t => new
                        {
                            i = t.I,
                            j = t.J,
                            x = t.X,
                            y = t.Y,
                            counter = t.Order
                        })
                    };

                    string json = JsonSerializer.Serialize(payload);
                    await webView21.CoreWebView2.ExecuteScriptAsync($"loadExistingFolderTiles({json})");
                    toolStripStatusLabel1.Text = $"Wczytano trasę \"{selectedRoute.RouteName}\" ({parts.Count} baseboardów). Możesz zaznaczyć dodatkowe pola i kliknąć Aktualizuj.";
                }
                else
                {
                    toolStripStatusLabel1.Text = $"Wczytano trasę \"{selectedRoute.RouteName}\" ({parts.Count} baseboardów) bez geolokalizacji.";
                    MessageBox.Show($"Wczytano trasę \"{selectedRoute.RouteName}\" ({parts.Count} baseboardów).\n\nTrasa ta nie zawierała danych geolokalizacji punktu bazowego (została wygenerowana we wcześniejszej wersji). Nie można jej wyświetlić na mapie.", "Wczytano trasę", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.log", ex.ToString());
                MessageBox.Show("Błąd podczas wczytywania trasy:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonDeleteRoute_Click(object? sender, EventArgs e)
        {
            if (listBoxRoutes.SelectedItem is not TerrainRouteInfo route)
            {
                MessageBox.Show("Wybierz trasę z listy do usunięcia.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show($"Czy na pewno chcesz usunąć trasę \"{route.RouteName}\"?\nFolder: {route.FolderPath}\n\nOperacji nie można cofnąć!", "Potwierdzenie usunięcia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _fileManager.DeleteRouteFolder(route.FolderPath);
                if (_loadedRoute?.FolderPath == route.FolderPath)
                {
                    _loadedRoute = null;
                    _loadedGndBlocks.Clear();
                    buttonResetAnchor_Click(sender, e);
                }
                RoutesListBoxRefresh();
                toolStripStatusLabel1.Text = $"Usunięto trasę \"{route.RouteName}\".";
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania trasy:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void WebView21_WebMessageReceived(object? sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            if (_isDownloading) return;

            string json = e.WebMessageAsJson;

            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    var root = doc.RootElement;
                    string? type = root.GetProperty("type").GetString();

                    if (type == "js_error")
                    {
                        string msg = root.TryGetProperty("message", out var mElem) ? (mElem.GetString() ?? "") : "";
                        File.WriteAllText("js_error.log", msg);
                        System.Diagnostics.Debug.WriteLine($"[JS Error] {msg}");
                        return;
                    }

                    if (type == "selection_changed")
                    {
                        int count = root.GetProperty("count").GetInt32();
                        int existingCount = root.TryGetProperty("existingCount", out var ec) ? ec.GetInt32() : 0;
                        _selectedTiles.Clear();

                        if (root.TryGetProperty("anchor", out var anchorElem) && anchorElem.ValueKind == JsonValueKind.Object)
                        {
                            if (anchorElem.TryGetProperty("x", out var ax) && anchorElem.TryGetProperty("y", out var ay))
                            {
                                _currentAnchorX = (long)Math.Round(ax.GetDouble());
                                _currentAnchorY = (long)Math.Round(ay.GetDouble());
                            }
                        }
                        else if (count == 0 && existingCount == 0)
                        {
                            _currentAnchorX = null;
                            _currentAnchorY = null;
                        }

                        if (root.TryGetProperty("tiles", out var tilesArray))
                        {
                            foreach (var tileElem in tilesArray.EnumerateArray())
                            {
                                int order = tileElem.GetProperty("order").GetInt32();
                                int i = tileElem.GetProperty("i").GetInt32();
                                int j = tileElem.GetProperty("j").GetInt32();
                                long x = (long)tileElem.GetProperty("x").GetDouble();
                                long y = (long)tileElem.GetProperty("y").GetDouble();

                                _selectedTiles.Add(new SelectedTileModel
                                {
                                    Order = order,
                                    I = i,
                                    J = j,
                                    X = x,
                                    Y = y
                                });
                            }
                        }

                        string tileText = existingCount > 0
                            ? $"Nowe baseboardy: {count} (łącznie: {count + existingCount})"
                            : $"Zaznaczono baseboardów (720m): {count}";
                        // 720m x 720m = 0.5184 km2
                        labelTileCount.Text = tileText;
                        labelArea.Text = $"Powierzchnia: {((count + existingCount) * 0.5184):F2} km²";

                        string statusMsg = count > 0
                            ? $"Zaznaczono {count} nowych baseboardów do wygenerowania."
                            : (existingCount > 0 ? $"Wczytano {existingCount} baseboardów. Kliknij na mapie, aby dodać nowe." : "Zaznacz obszar trasy na mapie.");

                        labelProgress.Text = count > 0 ? $"Zaznaczono {count} baseboardów." : (existingCount > 0 ? $"Wczytano {existingCount} baseboardów." : "Gotowy do zaznaczania.");
                        toolStripStatusLabel1.Text = statusMsg;
                    }
                }
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.log", ex.ToString());
                toolStripStatusLabel1.Text = "Blad!";
                MessageBox.Show("Wystapil blad w komunikacji z mapa (szczegoly w error.log):\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void RadioButtonMode_CheckedChanged(object? sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;

            string mode = radioButtonModeBox.Checked ? "box" : "click";
            await webView21.CoreWebView2.ExecuteScriptAsync($"setSelectionMode('{mode}')");
        }

        private async void RadioButtonEpsg_CheckedChanged(object? sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;

            string epsg = radioButtonEpsg2180.Checked ? "EPSG:2180" : "EPSG:3857";
            await webView21.CoreWebView2.ExecuteScriptAsync($"setCoordinateSystem('{epsg}')");
        }

        private async void buttonClearSelection_Click(object sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;
            await webView21.CoreWebView2.ExecuteScriptAsync("clearAllTiles()");
        }

        private async void buttonResetAnchor_Click(object? sender, EventArgs e)
        {
            _loadedRoute = null;
            _loadedGndBlocks.Clear();
            _selectedTiles.Clear();
            _currentAnchorX = null;
            _currentAnchorY = null;
            buttonStartDownload.Text = "Generuj teren (map.gnd)";
            textBoxDestinationFolder.Text = "Nowa_Trasa";
            UpdateNextFreeKuidPart2();
            labelTileCount.Text = "Zaznaczono baseboardów (720m): 0";
            labelArea.Text = "Powierzchnia: 0.00 km²";
            labelProgress.Text = "Zresetowano trasę. Gotowy do zaznaczania.";
            toolStripStatusLabel1.Text = "Wybierz punkt początkowy na mapie.";

            if (webView21.CoreWebView2 != null)
            {
                await webView21.CoreWebView2.ExecuteScriptAsync("resetGridOrigin()");
            }
        }

        private void textBoxDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            if (_loadedRoute == null)
            {
                UpdateNextFreeKuidPart2();
            }
        }

        private void UpdateNextFreeKuidPart2()
        {
            try
            {
                int nextKuid = _fileManager.GetNextFreeKuidPart2();
                textBoxKuidPart2.Text = nextKuid.ToString();
            }
            catch
            {
                textBoxKuidPart2.Text = "1";
            }
        }

        private async void buttonStartDownload_Click(object sender, EventArgs e)
        {
            if (_selectedTiles.Count == 0)
            {
                MessageBox.Show("Nie zaznaczono żadnych kafli na mapie!", "Brak zaznaczenia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxDestinationFolder.Text))
            {
                MessageBox.Show("Wpisz nazwe trasy!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (radioButtonEpsg3857.Checked)
            {
                var res = MessageBox.Show("Pobieranie wysokosci NMT z Geoportalu wymaga ukladu EPSG:2180 (obszar Polski).\nCzy chcesz automatycznie przelaczyc uklad na EPSG:2180?", "Uklad wspolrzednych", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                {
                    radioButtonEpsg2180.Checked = true;
                }
                else
                {
                    return;
                }
            }

            // Check if coordinates fall within Poland EPSG:2180 bounds
            foreach (var tile in _selectedTiles)
            {
                if (!GeoHelperEPSG2180.IsWithin2180Bounds(tile.X, tile.Y))
                {
                    MessageBox.Show($"Kafel ({tile.I}, {tile.J}) o wspolrzednych ({tile.X}, {tile.Y}) znajduje sie poza obszarem Polski w ukladzie EPSG:2180!\nGeoportal NMT udostepnia dane tylko dla terytorium Polski.", "Poza obszarem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
            }

            string targetGroup = "Trasy";
            string routeName = textBoxDestinationFolder.Text.Trim();
            string kuidPart1 = textBoxKuidPart1.Text.Trim();
            string kuidPart2 = textBoxKuidPart2.Text.Trim();
            bool isRelative = radioButtonElevationRelative.Checked;

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            SetUiDownloadingState(true);

            int total = _selectedTiles.Count;
            int completed = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = total;
            progressBar1.Value = 0;
            labelProgress.Text = $"Pobieranie: 0/{total}";
            toolStripStatusLabel1.Text = "Pobieranie danych wysokosciowych...";

            try
            {
                var wcs = new WcsElevationProvider();
                var downloadedGrids = new ConcurrentDictionary<(int I, int J), float[,]>();

                // Concurrent download using SemaphoreSlim (4 concurrent requests)
                using (var semaphore = new SemaphoreSlim(4, 4))
                {
                    var tasks = _selectedTiles.Select(async tile =>
                    {
                        await semaphore.WaitAsync(token);
                        try
                        {
                            token.ThrowIfCancellationRequested();
                            var grid = await wcs.GetElevationGridAsync(tile.X, tile.Y, token);
                            downloadedGrids[(tile.I, tile.J)] = grid;

                            int c = Interlocked.Increment(ref completed);
                            if (!IsDisposed && IsHandleCreated)
                            {
                                BeginInvoke(() =>
                                {
                                    labelProgress.Text = $"Pobieranie: {c}/{total}";
                                    progressBar1.Value = Math.Min(c, progressBar1.Maximum);
                                    toolStripStatusLabel1.Text = $"Pobrano baseboard ({tile.I}, {tile.J}) [{c}/{total}]";
                                });
                            }
                        }
                        finally
                        {
                            semaphore.Release();
                        }
                    });

                    await Task.WhenAll(tasks);
                }

                token.ThrowIfCancellationRequested();

                // Handle Relative Elevation normalization globally (guarantees NO vertical cliffs between tiles)
                float anchorElevation = 0;
                if (isRelative)
                {
                    if (_loadedRoute?.AnchorElevation != null)
                    {
                        anchorElevation = (float)_loadedRoute.AnchorElevation.Value;
                    }
                    else if (_loadedGndBlocks.Count > 0)
                    {
                        if (_loadedGndBlocks.TryGetValue((0, 0), out var p0))
                            anchorElevation = p0.Heights[38, 38];
                        else
                            anchorElevation = _loadedGndBlocks.Values.First().Heights[38, 38];
                    }
                    else
                    {
                        var anchorTile = _selectedTiles.FirstOrDefault(t => t.I == 0 && t.J == 0)
                                         ?? _selectedTiles.OrderBy(t => t.Order).First();

                        if (downloadedGrids.TryGetValue((anchorTile.I, anchorTile.J), out var anchorGrid))
                        {
                            anchorElevation = anchorGrid[38, 38];
                        }
                    }

                    foreach (var kvp in downloadedGrids)
                    {
                        var grid = kvp.Value;
                        for (int x = 0; x < 76; x++)
                        {
                            for (int y = 0; y < 76; y++)
                            {
                                grid[x, y] -= anchorElevation;
                            }
                        }
                    }
                }

                labelProgress.Text = "Generowanie mapfile.gnd...";
                toolStripStatusLabel1.Text = "Tworzenie struktury mapy Trainz...";

                foreach (var tile in _selectedTiles.OrderBy(t => t.Order))
                {
                    if (downloadedGrids.TryGetValue((tile.I, tile.J), out var grid))
                    {
                        // Trainz baseboard coordinates: SegmentX = -J, SegmentY = I
                        int segX = -tile.J;
                        int segY = tile.I;
                        var part = new MapGridPart(segX, segY, grid);
                        _loadedGndBlocks[(segX, segY)] = part;
                    }
                }

                var blocks = _loadedGndBlocks.Values.ToList();
                var writer = new GndWriter();
                byte[] gndData = writer.CreateGndFile(blocks);

                var routeInfo = _loadedRoute ?? new TerrainRouteInfo();
                routeInfo.RouteName = routeName;
                routeInfo.KuidPart1 = kuidPart1;
                routeInfo.KuidPart2 = kuidPart2;
                routeInfo.Epsg = radioButtonEpsg2180.Checked ? "EPSG:2180" : "EPSG:3857";
                routeInfo.IsRelative = isRelative;
                if (isRelative)
                {
                    routeInfo.AnchorElevation = anchorElevation;
                }
                if (_currentAnchorX.HasValue) routeInfo.AnchorX = _currentAnchorX.Value;
                if (_currentAnchorY.HasValue) routeInfo.AnchorY = _currentAnchorY.Value;

                // Merge tiles list
                var newCoords = _selectedTiles.Select(t => (t.I, t.J)).ToHashSet();
                var updatedTiles = new List<TerrainTileInfo>();
                int orderNum = 1;
                foreach (var existingTile in routeInfo.Tiles)
                {
                    if (!newCoords.Contains((existingTile.I, existingTile.J)))
                    {
                        updatedTiles.Add(new TerrainTileInfo
                        {
                            Order = orderNum++,
                            I = existingTile.I,
                            J = existingTile.J,
                            X = existingTile.X,
                            Y = existingTile.Y
                        });
                    }
                }
                foreach (var tile in _selectedTiles.OrderBy(t => t.Order))
                {
                    updatedTiles.Add(new TerrainTileInfo
                    {
                        Order = orderNum++,
                        I = tile.I,
                        J = tile.J,
                        X = tile.X,
                        Y = tile.Y
                    });
                }
                routeInfo.Tiles = updatedTiles;

                string targetFolder = _fileManager.CreateRouteFiles(routeName, targetGroup, kuidPart1, kuidPart2, gndData, routeInfo);
                routeInfo.FolderPath = targetFolder;
                _loadedRoute = routeInfo;

                buttonStartDownload.Text = "Aktualizuj teren (map.gnd)";
                RoutesListBoxRefresh();

                for (int idx = 0; idx < listBoxRoutes.Items.Count; idx++)
                {
                    if (listBoxRoutes.Items[idx] is TerrainRouteInfo r && r.FolderPath == targetFolder)
                    {
                        listBoxRoutes.SelectedIndex = idx;
                        break;
                    }
                }

                // Update WebView2 map so all generated tiles appear in green
                if (routeInfo.AnchorX.HasValue && routeInfo.AnchorY.HasValue)
                {
                    var payload = new
                    {
                        epsg = routeInfo.Epsg,
                        anchor = new { x = routeInfo.AnchorX.Value, y = routeInfo.AnchorY.Value },
                        tiles = routeInfo.Tiles.Select(t => new
                        {
                            i = t.I,
                            j = t.J,
                            x = t.X,
                            y = t.Y,
                            counter = t.Order
                        })
                    };
                    string jsonPayload = JsonSerializer.Serialize(payload);
                    await webView21.CoreWebView2.ExecuteScriptAsync($"loadExistingFolderTiles({jsonPayload})");
                }

                _selectedTiles.Clear();

                labelProgress.Text = "Gotowe!";
                toolStripStatusLabel1.Text = $"Zapisano trasę ({blocks.Count} baseboardów)!";
                MessageBox.Show($"Pomyślnie zapisano mapę terenu ({blocks.Count} baseboardów) w folderze:\n{targetFolder}", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                labelProgress.Text = "Anulowano.";
                toolStripStatusLabel1.Text = "Pobieranie zostalo anulowane przez uzytkownika.";
                MessageBox.Show("Pobieranie zostalo przerwane.", "Anulowano", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                File.WriteAllText("error.log", ex.ToString());
                labelProgress.Text = "Blad generowania!";
                toolStripStatusLabel1.Text = "Wystapil blad podczas generowania mapy!";
                MessageBox.Show("Wystapil blad podczas generowania mapy (szczegoly w error.log):\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetUiDownloadingState(false);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            buttonCancel.Enabled = false;
            labelProgress.Text = "Anulowanie...";
            toolStripStatusLabel1.Text = "Anulowanie pobierania...";
        }

        private void SetUiDownloadingState(bool downloading)
        {
            _isDownloading = downloading;
            buttonStartDownload.Enabled = !downloading;
            buttonCancel.Enabled = downloading;
            groupBox1Selection.Enabled = !downloading;
            groupBox2CoordSystem.Enabled = !downloading;
            groupBox3Config.Enabled = !downloading;
            groupBox4Download.Enabled = true;
            Cursor = downloading ? Cursors.WaitCursor : Cursors.Default;
        }

        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;

                if (sender is TextBox textBox)
                {
                    _warningToolTip.Hide(textBox);
                    _warningToolTip.Show("Tutaj możesz wpisac tylko cyfry!", textBox, 50, -75, 2000);
                }
            }
        }

        private void TerrainGridToolForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_isDownloading)
            {
                var res = MessageBox.Show("Generowanie terenu jest w toku. Czy na pewno chcesz zamknac okno i przerwac operacje?", "Generowanie w toku", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                _cancellationTokenSource?.Cancel();
            }

            Properties.Settings.Default.TerrainGridToolFormState = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.TerrainGridToolFormSize = this.Size;
            }
            else
            {
                Properties.Settings.Default.TerrainGridToolFormSize = this.RestoreBounds.Size;
            }
            Properties.Settings.Default.Save();
        }
    }
}

