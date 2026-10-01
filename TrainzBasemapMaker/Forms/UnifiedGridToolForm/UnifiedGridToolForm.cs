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
using TrainzBasemapMaker.Classes;
using TrainzBasemapMaker.Classes.TrainzTerrain;

namespace TrainzBasemapMaker
{
    /// <summary>
    /// Unified tool combining GridToolForm (2D basemap download) and TerrainGridToolForm (3D terrain generation).
    /// Allows selecting any combination of: generating 2D basemaps, placing 2D on map, generating 3D basemaps,
    /// placing 3D on map, and generating 3D terrain (map.gnd).
    /// </summary>
    public partial class UnifiedGridToolForm : Form, TrainzBasemapMaker.Classes.IMainMenuOperations
    {
        // ── Inner models ──────────────────────────────────────────────────────────
        private class SelectedTileModel
        {
            public int Order { get; set; }
            public int I { get; set; }
            public int J { get; set; }
            public long X { get; set; }
            public long Y { get; set; }
        }

        // ── Fields ────────────────────────────────────────────────────────────────
        private readonly TrainzFileManager _fileManager = new TrainzFileManager();
        private readonly ToolTip _warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "Błąd wprowadzania" };
        private readonly List<SelectedTileModel> _selectedTiles = new List<SelectedTileModel>();
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isDownloading = false;
        private long? _currentAnchorX;
        private long? _currentAnchorY;

        // Terrain route state (from TerrainGridToolForm)
        private TerrainRouteInfo? _loadedRoute;
        private readonly Dictionary<(int SegmentX, int SegmentY), MapGridPart> _loadedGndBlocks
            = new Dictionary<(int SegmentX, int SegmentY), MapGridPart>();

        public event Action<string>? StatusUpdate;

        // ── Constructor ───────────────────────────────────────────────────────────
        public UnifiedGridToolForm()
        {
            InitializeComponent();

            // Bind map providers
            comboBoxMapType.DataSource = MapSources.AvailableMaps;
            comboBoxMapType.DisplayMember = "Name";
            comboBoxMapType.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxMapType.DrawItem += FormHelpers.ComboBoxMapType_DrawItem;

            // Initial states
            textBoxBasemapDate.Text = DateTime.Now.Year.ToString();
            textBoxDestinationFolder.Text = "Nowa_Trasa";
            textBoxDesignation.Text = "P";
            textBoxKuidPart1.Text = Properties.Settings.Default.DefaultKuidFirstPart ?? "123456";
            textBoxKuidPart1.TextChanged += (s, e) =>
            {
                if (_loadedRoute == null && Properties.Settings.Default.AutoKuidNumber)
                    UpdateNextFreeKuidPart2();
            };

            comboBoxEpsg.SelectedIndex = 0;
            radioButtonModeClick.Checked = true;
            radioButtonElevationAbsolute.Checked = true;

            buttonLoadRoute.Enabled = false;
            buttonDeleteRoute.Enabled = false;

            // Fire map-type selected to populate resolution combobox
            comboBoxMapType_SelectedIndexChanged(comboBoxMapType, EventArgs.Empty);

            RoutesListBoxRefresh();
            UpdateNextFreeCounter();
            UpdateNextFreeKuidPart2();

            StatusUpdate?.Invoke("LPM: Kliknij lub przeciągnij pędzlem, aby zaznaczyć | PPM: Przesuwanie mapy");
        }

        // ── Load / Init ───────────────────────────────────────────────────────────
        private async void UnifiedGridToolForm_Load(object? sender, EventArgs e)
        {
            ThemeManager.ApplyTheme(this);
            await InitBrowser();
        }

        private async Task InitBrowser()
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);
                webView21.CoreWebView2.Settings.UserAgent = "TrainzBasemapMaker/v0.6.0 (https://github.com/Ignacy110/TrainzBasemapMaker)";

                // Prefer TerrainGridToolForm web map (720m tiles), fall back to GridToolForm web map
                string indexPath = Path.Combine(Application.StartupPath, "Forms", "TerrainGridToolForm", "Web", "grid_map.html");
                if (!File.Exists(indexPath))
                    indexPath = Path.Combine(Application.StartupPath, "Forms", "GridToolForm", "Web", "grid_map.html");

                webView21.CoreWebView2.NavigationCompleted += async (s, args) =>
                {
                    if (args.IsSuccess)
                    {
                        // Trainz baseboard size = 720 m
                        await webView21.CoreWebView2.ExecuteScriptAsync("setTileSize(720);");
                        await webView21.CoreWebView2.ExecuteScriptAsync("setCoordinateSystem('EPSG:2180');");
                    }
                };

                webView21.CoreWebView2.Navigate("file:///" + indexPath);
                webView21.CoreWebView2.WebMessageReceived += WebView21_WebMessageReceived;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd inicjalizacji komponentu mapy:\n\n" + ex.Message,
                    "Błąd WebView2", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── Routes list ───────────────────────────────────────────────────────────
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
                MessageBox.Show("Wybierz trasę z listy do wczytania.", "Informacja",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (webView21.CoreWebView2 == null)
            {
                MessageBox.Show("Komponent mapy jeszcze się inicjalizuje. Spróbuj ponownie za chwilę.", "Informacja",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            string gndPath = Path.Combine(selectedRoute.FolderPath, "mapfile.gnd");
            if (!File.Exists(gndPath))
            {
                MessageBox.Show($"W folderze trasy nie znaleziono pliku mapfile.gnd:\n{selectedRoute.FolderPath}",
                    "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                byte[] gndData = File.ReadAllBytes(gndPath);
                var parts = GndReader.ReadGndFile(gndData);

                _loadedGndBlocks.Clear();
                foreach (var p in parts)
                    _loadedGndBlocks[(p.SegmentX, p.SegmentY)] = p;

                _loadedRoute = selectedRoute;

                textBoxDestinationFolder.Text = selectedRoute.RouteName;
                textBoxKuidPart1.Text = selectedRoute.KuidPart1;
                textBoxKuidPart2.Text = selectedRoute.KuidPart2;

                comboBoxEpsg.SelectedIndex = selectedRoute.Epsg == "EPSG:3857" ? 1 : 0;

                if (selectedRoute.IsRelative)
                    radioButtonElevationRelative.Checked = true;
                else
                    radioButtonElevationAbsolute.Checked = true;

                buttonStartDownload.Text = "Aktualizuj trasę";

                if (selectedRoute.AnchorX.HasValue && selectedRoute.AnchorY.HasValue && selectedRoute.Tiles.Count > 0)
                {
                    _currentAnchorX = selectedRoute.AnchorX.Value;
                    _currentAnchorY = selectedRoute.AnchorY.Value;

                    var payload = new
                    {
                        epsg = selectedRoute.Epsg,
                        anchor = new { x = selectedRoute.AnchorX.Value, y = selectedRoute.AnchorY.Value },
                        tiles = selectedRoute.Tiles.Select(t => new { i = t.I, j = t.J, x = t.X, y = t.Y, counter = t.Order })
                    };

                    string json = JsonSerializer.Serialize(payload);
                    await webView21.CoreWebView2.ExecuteScriptAsync($"loadExistingFolderTiles({json})");
                    StatusUpdate?.Invoke($"Wczytano trasę \"{selectedRoute.RouteName}\" ({parts.Count} baseboardów). Możesz zaznaczyć dodatkowe pola i kliknąć Aktualizuj.");
                }
                else
                {
                    StatusUpdate?.Invoke($"Wczytano trasę \"{selectedRoute.RouteName}\" ({parts.Count} baseboardów) bez geolokalizacji.");
                    MessageBox.Show($"Trasa \"{selectedRoute.RouteName}\" nie zawiera danych geolokalizacji (wygenerowana we wcześniejszej wersji). Nie można jej wyświetlić na mapie.",
                        "Wczytano trasę", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas wczytywania trasy:\n\n" + ex.Message,
                    "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonDeleteRoute_Click(object? sender, EventArgs e)
        {
            if (listBoxRoutes.SelectedItem is not TerrainRouteInfo route)
            {
                MessageBox.Show("Wybierz trasę z listy do usunięcia.", "Informacja",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var confirm = MessageBox.Show(
                $"Czy na pewno chcesz usunąć trasę \"{route.RouteName}\"?\nFolder: {route.FolderPath}\n\nOperacji nie można cofnąć!",
                "Potwierdzenie usunięcia", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (confirm != DialogResult.Yes) return;

            try
            {
                _fileManager.DeleteRouteFolder(route.FolderPath);
                if (_loadedRoute?.FolderPath == route.FolderPath)
                {
                    _loadedRoute = null;
                    _loadedGndBlocks.Clear();
                    await ResetAnchorAsync();
                }
                RoutesListBoxRefresh();
                StatusUpdate?.Invoke($"Usunięto trasę \"{route.RouteName}\".");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd podczas usuwania trasy:\n\n" + ex.Message,
                    "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        // ── WebView messages ──────────────────────────────────────────────────────
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
                        Debug.WriteLine($"[JS Error] {msg}");
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
                                _selectedTiles.Add(new SelectedTileModel
                                {
                                    Order = tileElem.GetProperty("order").GetInt32(),
                                    I = tileElem.GetProperty("i").GetInt32(),
                                    J = tileElem.GetProperty("j").GetInt32(),
                                    X = (long)tileElem.GetProperty("x").GetDouble(),
                                    Y = (long)tileElem.GetProperty("y").GetDouble()
                                });
                            }
                        }

                        string tileText = existingCount > 0
                            ? $"Nowe baseboardy: {count} (łącznie: {count + existingCount})"
                            : $"Zaznaczono baseboardów (720m): {count}";

                        labelTileCount.Text = tileText;
                        // 720m x 720m = 0.5184 km²
                        labelArea.Text = $"Powierzchnia: {((count + existingCount) * 0.5184):F2} km²";

                        string statusMsg = count > 0
                            ? $"Zaznaczono {count} nowych baseboardów."
                            : (existingCount > 0 ? $"Wczytano {existingCount} baseboardów. Kliknij na mapie, aby dodać nowe." : "Zaznacz obszar trasy na mapie.");

                        labelProgress.Text = count > 0 ? $"Zaznaczono {count} baseboardów."
                            : (existingCount > 0 ? $"Wczytano {existingCount} baseboardów." : "Gotowy do zaznaczania.");
                        StatusUpdate?.Invoke(statusMsg);
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error processing web message: " + ex.Message);
            }
        }

        // ── Map controls ──────────────────────────────────────────────────────────
        private async void RadioButtonMode_CheckedChanged(object? sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;
            string mode = radioButtonModeBox.Checked ? "box" : "click";
            await webView21.CoreWebView2.ExecuteScriptAsync($"setSelectionMode('{mode}')");
        }

        private async void ComboBoxEpsg_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;
            string epsg = (comboBoxEpsg.SelectedIndex == 0) ? "EPSG:2180" : "EPSG:3857";
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
            textBoxDestinationFolder.Text = "Nowa_Trasa";
            buttonStartDownload.Text = "Generuj / Pobierz";
            UpdateNextFreeKuidPart2();
            labelTileCount.Text = "Zaznaczono baseboardów (720m): 0";
            labelArea.Text = "Powierzchnia: 0.00 km²";
            labelProgress.Text = "Zresetowano. Gotowy do zaznaczania.";
            StatusUpdate?.Invoke("Wybierz punkt początkowy na mapie.");

            if (webView21.CoreWebView2 != null)
                await webView21.CoreWebView2.ExecuteScriptAsync("resetGridOrigin()");
        }

        private async Task ResetAnchorAsync()
        {
            _currentAnchorX = null;
            _currentAnchorY = null;
            buttonStartDownload.Text = "Generuj / Pobierz";
            if (webView21.CoreWebView2 != null)
                await webView21.CoreWebView2.ExecuteScriptAsync("resetGridOrigin()");
        }

        // ── Checkbox logic ────────────────────────────────────────────────────────
        private void checkBoxGenerate2DBasemaps_CheckedChanged(object? sender, EventArgs e)
        {
            checkBoxPlace2DOnMap.Enabled = checkBoxGenerate2DBasemaps.Checked;
            if (!checkBoxGenerate2DBasemaps.Checked)
                checkBoxPlace2DOnMap.Checked = false;
        }

        private void checkBoxGenerate3DBasemaps_CheckedChanged(object? sender, EventArgs e)
        {
            checkBoxPlace3DOnMap.Enabled = checkBoxGenerate3DBasemaps.Checked;
            if (!checkBoxGenerate3DBasemaps.Checked)
                checkBoxPlace3DOnMap.Checked = false;
        }

        private void checkBoxGenerate3DTerrain_CheckedChanged(object? sender, EventArgs e)
        {
            groupBoxElevation.Enabled = checkBoxGenerate3DTerrain.Checked;
        }

        // ── ComboBox handlers ─────────────────────────────────────────────────────
        private void comboBoxMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMapType.SelectedItem is IMapSource selected)
            {
                textBoxBasemapDate.Enabled = selected.SupportsTime;
                label14.Enabled = selected.SupportsTime;
                FormHelpers.UpdateResolutionComboBox(comboBoxResolution, selected);

                // Auto-suggest native EPSG for the selected provider
                if (selected is XyzTileMapSource)
                    comboBoxEpsg.SelectedIndex = 1;
                else
                    comboBoxEpsg.SelectedIndex = 0;
            }
        }

        private void comboBoxResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Required for designer
        }

        private int GetSelectedResolution()
        {
            return (comboBoxResolution.SelectedItem as ResolutionOption)?.Value ?? 2048;
        }

        // ── KUID / Counter ────────────────────────────────────────────────────────
        private void textBoxDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            if (_loadedRoute == null)
            {
                UpdateNextFreeCounter();
                UpdateNextFreeKuidPart2();
            }
        }

        private void UpdateNextFreeCounter()
        {
            try
            {
                string group = textBoxDestinationFolder.Text.Trim();
                int next = _fileManager.GetNextFreeCounter(group);
                textBoxCounter.Text = next.ToString();
            }
            catch
            {
                textBoxCounter.Text = "1";
            }
        }

        private void UpdateNextFreeKuidPart2()
        {
            try
            {
                int next = _fileManager.GetNextFreeKuidPart2(textBoxKuidPart1.Text.Trim());
                textBoxKuidPart2.Text = next.ToString();
            }
            catch
            {
                textBoxKuidPart2.Text = Math.Max(1, Properties.Settings.Default.MinKuidPart2).ToString();
            }
        }

        // ── Main generation / download ────────────────────────────────────────────
        private async void buttonStartDownload_Click(object sender, EventArgs e)
        {
            // ── Validation ──
            if (_selectedTiles.Count == 0)
            {
                MessageBox.Show("Nie zaznaczono żadnych kafli na mapie!", "Brak zaznaczenia",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxDestinationFolder.Text))
            {
                MessageBox.Show("Wpisz nazwę trasy!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            bool gen2D = checkBoxGenerate2DBasemaps.Checked;
            bool place2D = checkBoxPlace2DOnMap.Checked;
            bool gen3D = checkBoxGenerate3DBasemaps.Checked;
            bool place3D = checkBoxPlace3DOnMap.Checked;
            bool genTerrain = checkBoxGenerate3DTerrain.Checked;

            bool needsRoute = place2D || place3D || genTerrain;

            if (!gen2D && !gen3D && !genTerrain)
            {
                MessageBox.Show("Zaznacz co najmniej jedną opcję w sekcji '5. Pobieranie i generowanie mapy'!",
                    "Brak wyboru", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((gen2D || gen3D) && string.IsNullOrWhiteSpace(textBoxDesignation.Text))
            {
                MessageBox.Show("Wpisz oznaczenie podkładów!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBoxCounter.Text, out int startCounter))
            {
                MessageBox.Show("Niepoprawny numer początkowy podkładu!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!int.TryParse(textBoxKuidPart2.Text, out int startKuid2))
            {
                MessageBox.Show("Niepoprawny numer KUID (część 2)!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if ((gen2D || gen3D) && comboBoxMapType.SelectedItem is not IMapSource)
            {
                MessageBox.Show("Wybierz źródło mapy!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Terrain requires EPSG:2180
            if (genTerrain && comboBoxEpsg.SelectedIndex == 1)
            {
                var res = MessageBox.Show(
                    "Pobieranie wysokości NMT z Geoportalu wymaga układu EPSG:2180 (obszar Polski).\nCzy chcesz automatycznie przełączyć układ na EPSG:2180?",
                    "Układ współrzędnych", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (res == DialogResult.Yes)
                    comboBoxEpsg.SelectedIndex = 0;
                else
                    return;
            }

            if (genTerrain)
            {
                foreach (var tile in _selectedTiles)
                {
                    if (!GeoHelperEPSG2180.IsWithin2180Bounds(tile.X, tile.Y))
                    {
                        MessageBox.Show(
                            $"Kafel ({tile.I}, {tile.J}) o współrzędnych ({tile.X}, {tile.Y}) znajduje się poza obszarem Polski!\nGeoportal NMT udostępnia dane tylko dla terytorium Polski.",
                            "Poza obszarem", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                        return;
                    }
                }
            }

            // ── Collect params ──
            string routeName = textBoxDestinationFolder.Text.Trim();
            string designation = textBoxDesignation.Text.Trim();
            string kuidPart1 = textBoxKuidPart1.Text.Trim();
            string kuidPart2 = textBoxKuidPart2.Text.Trim();
            bool isRelative = radioButtonElevationRelative.Checked;
            IMapSource? selectedMap = comboBoxMapType.SelectedItem as IMapSource;
            string year = (selectedMap?.SupportsTime == true) ? textBoxBasemapDate.Text.Trim() : "";
            int maxResolution = GetSelectedResolution();
            var tilesToProcess = _selectedTiles.ToList();
            int total = tilesToProcess.Count;

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;
            SetUiDownloadingState(true);

            progressBar1.Minimum = 0;
            progressBar1.Value = 0;
            labelProgress.Text = "Uruchamianie...";

            try
            {
                // ──────────────────────────────────────────────────────────────────
                // PHASE 1: Download elevation data (if generating terrain or 3D basemaps)
                // ──────────────────────────────────────────────────────────────────
                ConcurrentDictionary<(int I, int J), float[,]>? downloadedGrids = null;

                if (genTerrain || gen3D)
                {
                    StatusUpdate?.Invoke("Pobieranie danych wysokościowych...");
                    labelProgress.Text = "Pobieranie wysokości...";
                    progressBar1.Maximum = total;

                    downloadedGrids = new ConcurrentDictionary<(int I, int J), float[,]>();
                    int completed = 0;

                    var wcs = new WcsElevationProvider();
                    using (var semaphore = new SemaphoreSlim(4, 4))
                    {
                        var tasks = tilesToProcess.Select(async tile =>
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
                                        labelProgress.Text = $"Wysokości: {c}/{total}";
                                        progressBar1.Value = Math.Min(c, progressBar1.Maximum);
                                        StatusUpdate?.Invoke($"Pobrano wysokości ({tile.I},{tile.J}) [{c}/{total}]");
                                    });
                                }
                            }
                            finally { semaphore.Release(); }
                        });
                        await Task.WhenAll(tasks);
                    }

                    token.ThrowIfCancellationRequested();

                    // ── Relative elevation normalization ──
                    if (genTerrain && isRelative)
                    {
                        float anchorElevation = 0;
                        if (_loadedRoute?.AnchorElevation != null)
                        {
                            anchorElevation = (float)_loadedRoute.AnchorElevation.Value;
                        }
                        else if (_loadedGndBlocks.Count > 0)
                        {
                            anchorElevation = _loadedGndBlocks.TryGetValue((0, 0), out var p0)
                                ? p0.Heights[38, 38]
                                : _loadedGndBlocks.Values.First().Heights[38, 38];
                        }
                        else
                        {
                            var anchorTile = tilesToProcess.FirstOrDefault(t => t.I == 0 && t.J == 0)
                                             ?? tilesToProcess.OrderBy(t => t.Order).First();
                            if (downloadedGrids.TryGetValue((anchorTile.I, anchorTile.J), out var anchorGrid))
                                anchorElevation = anchorGrid[38, 38];
                        }

                        foreach (var kvp in downloadedGrids)
                        {
                            var grid = kvp.Value;
                            for (int x = 0; x < 76; x++)
                                for (int y = 0; y < 76; y++)
                                    grid[x, y] -= anchorElevation;
                        }
                    }
                }

                // ──────────────────────────────────────────────────────────────────
                // PHASE 2: Generate 2D basemaps
                // ──────────────────────────────────────────────────────────────────
                // basemap2DKuids[tileOrder] = (kuid1, kuid2, tile)  — for obs placement later
                var basemap2DInfo = new List<(SelectedTileModel Tile, string Kuid1, string Kuid2)>();

                if (gen2D && selectedMap != null)
                {
                    StatusUpdate?.Invoke("Pobieranie podkładów 2D...");
                    progressBar1.Maximum = total;
                    progressBar1.Value = 0;

                    string groupName2D = $"Podklady_2D_{routeName}";
                    int successCount = 0;
                    int failCount = 0;

                    for (int i = 0; i < tilesToProcess.Count; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        var tile = tilesToProcess[i];
                        int counter = startCounter + i;
                        string kuid2ForTile = (startKuid2 + i).ToString();

                        labelProgress.Text = $"Podkład 2D: {i + 1}/{total}";
                        StatusUpdate?.Invoke($"Pobieranie podkładu 2D {i + 1}/{total}...");

                        try
                        {
                            byte[] imageBytes = await DownloadWithResolutionFallback(selectedMap, year, tile.X, tile.Y, maxResolution, token);

                            bool created = _fileManager.CreateTrainzFiles(
                                imageBytes, groupName2D,
                                tile.X, tile.Y,
                                designation, counter,
                                kuidPart1, kuid2ForTile);

                            if (created)
                            {
                                successCount++;
                                basemap2DInfo.Add((tile, kuidPart1, kuid2ForTile));
                            }
                        }
                        catch (Exception ex)
                        {
                            failCount++;
                            Debug.WriteLine($"Błąd podkładu 2D tile ({tile.I},{tile.J}): {ex.Message}");
                        }

                        progressBar1.Value = i + 1;
                    }

                    StatusUpdate?.Invoke($"Podkłady 2D: utworzono {successCount}, błędów {failCount}.");
                    // Advance KUID range so phase 3 doesn't collide
                    startKuid2 += total;
                    startCounter += total;
                }

                // ──────────────────────────────────────────────────────────────────
                // PHASE 3: Generate 3D basemaps
                // ──────────────────────────────────────────────────────────────────
                var basemap3DInfo = new List<(SelectedTileModel Tile, string Kuid1, string Kuid2, float BaseHeight)>();

                if (gen3D && selectedMap != null)
                {
                    StatusUpdate?.Invoke("Pobieranie podkładów 3D...");
                    progressBar1.Maximum = total;
                    progressBar1.Value = 0;

                    string groupName3D = $"Podklady_3D_{routeName}";
                    int successCount = 0;
                    int failCount = 0;

                    for (int i = 0; i < tilesToProcess.Count; i++)
                    {
                        token.ThrowIfCancellationRequested();
                        var tile = tilesToProcess[i];
                        int counter = startCounter + i;
                        string kuid2ForTile = (startKuid2 + i).ToString();

                        labelProgress.Text = $"Podkład 3D: {i + 1}/{total}";
                        StatusUpdate?.Invoke($"Pobieranie podkładu 3D {i + 1}/{total}...");

                        try
                        {
                            byte[] imageBytes = await DownloadWithResolutionFallback(selectedMap, year, tile.X, tile.Y, maxResolution, token);

                            float baseHeight = 0f;
                            float[,]? grid = null;
                            if (downloadedGrids != null && downloadedGrids.TryGetValue((tile.I, tile.J), out grid))
                                baseHeight = grid[38, 38];

                            bool created = _fileManager.CreateTrainzFiles(
                                imageBytes, groupName3D,
                                tile.X, tile.Y,
                                designation, counter,
                                kuidPart1, kuid2ForTile,
                                grid,
                                0.2f,  // zOffset to prevent Z-fighting
                                baseHeight);

                            if (created)
                            {
                                successCount++;
                                basemap3DInfo.Add((tile, kuidPart1, kuid2ForTile, baseHeight));
                            }
                        }
                        catch (Exception ex)
                        {
                            failCount++;
                            Debug.WriteLine($"Błąd podkładu 3D tile ({tile.I},{tile.J}): {ex.Message}");
                        }

                        progressBar1.Value = i + 1;
                    }

                    StatusUpdate?.Invoke($"Podkłady 3D: utworzono {successCount}, błędów {failCount}.");
                    startKuid2 += total;
                    startCounter += total;
                }

                // ──────────────────────────────────────────────────────────────────
                // PHASE 4: Build terrain GND (or flat terrain if no terrain checkbox)
                // ──────────────────────────────────────────────────────────────────
                byte[]? gndData = null;
                TerrainRouteInfo? routeInfo = null;

                if (needsRoute)
                {
                    labelProgress.Text = "Generowanie mapfile.gnd...";
                    StatusUpdate?.Invoke("Tworzenie struktury mapy Trainz...");

                    if (genTerrain && downloadedGrids != null)
                    {
                        // Build GND from real elevation data
                        foreach (var tile in tilesToProcess.OrderBy(t => t.Order))
                        {
                            if (downloadedGrids.TryGetValue((tile.I, tile.J), out var grid))
                            {
                                int segX = -tile.J;
                                int segY = tile.I;
                                var part = new MapGridPart(segX, segY, grid);
                                _loadedGndBlocks[(segX, segY)] = part;
                            }
                        }
                    }
                    else
                    {
                        // Build flat GND (height = 0) for each tile
                        foreach (var tile in tilesToProcess.OrderBy(t => t.Order))
                        {
                            int segX = -tile.J;
                            int segY = tile.I;
                            // Only add if not already in the loaded blocks
                            if (!_loadedGndBlocks.ContainsKey((segX, segY)))
                            {
                                var flatGrid = new float[76, 76]; // all zeros = sea level
                                var part = new MapGridPart(segX, segY, flatGrid);
                                _loadedGndBlocks[(segX, segY)] = part;
                            }
                        }
                    }

                    var blocks = _loadedGndBlocks.Values.ToList();
                    var writer = new GndWriter();
                    gndData = writer.CreateGndFile(blocks);

                    // Build routeInfo
                    routeInfo = _loadedRoute ?? new TerrainRouteInfo();
                    routeInfo.RouteName = routeName;
                    routeInfo.KuidPart1 = kuidPart1;
                    routeInfo.KuidPart2 = kuidPart2;
                    routeInfo.Epsg = (comboBoxEpsg.SelectedIndex == 0) ? "EPSG:2180" : "EPSG:3857";
                    routeInfo.IsRelative = genTerrain && isRelative;

                    if (_currentAnchorX.HasValue) routeInfo.AnchorX = _currentAnchorX.Value;
                    if (_currentAnchorY.HasValue) routeInfo.AnchorY = _currentAnchorY.Value;

                    // Merge tile lists
                    var newCoords = tilesToProcess.Select(t => (t.I, t.J)).ToHashSet();
                    var updatedTiles = new List<TerrainTileInfo>();
                    int orderNum = 1;
                    foreach (var existingTile in routeInfo.Tiles)
                    {
                        if (!newCoords.Contains((existingTile.I, existingTile.J)))
                        {
                            updatedTiles.Add(new TerrainTileInfo
                            {
                                Order = orderNum++,
                                I = existingTile.I, J = existingTile.J,
                                X = existingTile.X, Y = existingTile.Y
                            });
                        }
                    }
                    foreach (var tile in tilesToProcess.OrderBy(t => t.Order))
                    {
                        updatedTiles.Add(new TerrainTileInfo
                        {
                            Order = orderNum++,
                            I = tile.I, J = tile.J,
                            X = tile.X, Y = tile.Y
                        });
                    }
                    routeInfo.Tiles = updatedTiles;
                }

                // ──────────────────────────────────────────────────────────────────
                // PHASE 5: Build obs objects for placement on route
                // ──────────────────────────────────────────────────────────────────
                var obsObjects = new List<ObsObject>();

                if (place2D)
                {
                    foreach (var (tile, k1, k2) in basemap2DInfo)
                    {
                        obsObjects.Add(new ObsObject
                        {
                            KuidPart1 = int.Parse(k1),
                            KuidPart2 = int.Parse(k2),
                            // Flat height (0 + small offset against z-fighting with terrain)
                            X = (-tile.J) * 720f + 360f,
                            Y = tile.I * 720f + 360f,
                            Z = 0.1f,
                            RotZ = (float)(Math.PI / 2.0)
                        });
                    }
                }

                if (place3D)
                {
                    foreach (var (tile, k1, k2, baseHeight) in basemap3DInfo)
                    {
                        obsObjects.Add(new ObsObject
                        {
                            KuidPart1 = int.Parse(k1),
                            KuidPart2 = int.Parse(k2),
                            X = (-tile.J) * 720f + 360f,
                            Y = tile.I * 720f + 360f,
                            Z = baseHeight,
                            RotZ = (float)(Math.PI / 2.0)
                        });
                    }
                }

                // ──────────────────────────────────────────────────────────────────
                // PHASE 6: Save route files (if a route is needed)
                // ──────────────────────────────────────────────────────────────────
                if (needsRoute && gndData != null && routeInfo != null)
                {
                    string targetFolder = _fileManager.CreateRouteFiles(
                        routeName, "Trasy", kuidPart1, kuidPart2, gndData, routeInfo,
                        obsObjects.Count > 0 ? obsObjects : null);
                    routeInfo.FolderPath = targetFolder;
                    _loadedRoute = routeInfo;

                    buttonStartDownload.Text = "Aktualizuj trasę";
                    RoutesListBoxRefresh();

                    // Select the new/updated route in list
                    for (int idx = 0; idx < listBoxRoutes.Items.Count; idx++)
                    {
                        if (listBoxRoutes.Items[idx] is TerrainRouteInfo r && r.FolderPath == targetFolder)
                        {
                            listBoxRoutes.SelectedIndex = idx;
                            break;
                        }
                    }

                    // Refresh map to show all blocks as green
                    if (routeInfo.AnchorX.HasValue && routeInfo.AnchorY.HasValue && webView21.CoreWebView2 != null)
                    {
                        var payload = new
                        {
                            epsg = routeInfo.Epsg,
                            anchor = new { x = routeInfo.AnchorX.Value, y = routeInfo.AnchorY.Value },
                            tiles = routeInfo.Tiles.Select(t => new { i = t.I, j = t.J, x = t.X, y = t.Y, counter = t.Order })
                        };
                        string jsonPayload = JsonSerializer.Serialize(payload);
                        await webView21.CoreWebView2.ExecuteScriptAsync($"loadExistingFolderTiles({jsonPayload})");
                    }
                }

                _selectedTiles.Clear();
                labelProgress.Text = "Gotowe!";

                // ── Summary message ──
                var summaryParts = new List<string>();
                if (gen2D) summaryParts.Add($"podkłady 2D ({basemap2DInfo.Count})");
                if (gen3D) summaryParts.Add($"podkłady 3D ({basemap3DInfo.Count})");
                if (genTerrain) summaryParts.Add("teren 3D (map.gnd)");
                if (place2D) summaryParts.Add("ułożono 2D na mapie");
                if (place3D) summaryParts.Add("ułożono 3D na mapie");

                string summary = string.Join(", ", summaryParts);
                StatusUpdate?.Invoke($"Zakończono: {summary}.");
                MessageBox.Show($"Pomyślnie zakończono operację!\n\nWygenerowano: {summary}.",
                    "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (OperationCanceledException)
            {
                labelProgress.Text = "Anulowano.";
                StatusUpdate?.Invoke("Operacja anulowana przez użytkownika.");
                MessageBox.Show("Operacja została przerwana.", "Anulowano", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                labelProgress.Text = "Błąd!";
                StatusUpdate?.Invoke("Wystąpił błąd!");
                MessageBox.Show("Błąd podczas generowania:\n\n" + ex.Message,
                    "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetUiDownloadingState(false);
                RoutesListBoxRefresh();
                UpdateNextFreeCounter();
                UpdateNextFreeKuidPart2();
            }
        }

        /// <summary>
        /// Downloads a map image at the requested resolution; if unavailable, falls back to lower
        /// resolutions (never exceeds the requested max).
        /// </summary>
        private static async Task<byte[]> DownloadWithResolutionFallback(
            IMapSource source, string year, long x, long y, int maxResolution,
            CancellationToken token)
        {
            // Build the resolution ladder: start at maxResolution, step down to 512
            int[] resolutions = { 8192, 4096, 2048, 1024, 512 };
            var ladder = resolutions.Where(r => r <= maxResolution).ToArray();
            if (ladder.Length == 0) ladder = new[] { 512 };

            Exception? lastEx = null;
            foreach (int res in ladder)
            {
                try
                {
                    return await source.GetMapImageAsync(year, x, y, res, cancellationToken: token);
                }
                catch (OperationCanceledException) { throw; }
                catch (Exception ex)
                {
                    lastEx = ex;
                    Debug.WriteLine($"Resolution {res} failed for ({x},{y}): {ex.Message}. Trying lower...");
                }
            }
            throw new Exception($"Nie udało się pobrać podkładu dla ({x},{y}) w żadnej dostępnej rozdzielczości.", lastEx);
        }

        // ── Cancel ────────────────────────────────────────────────────────────────
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            buttonCancel.Enabled = false;
            labelProgress.Text = "Anulowanie...";
            StatusUpdate?.Invoke("Anulowanie operacji...");
        }

        // ── UI state ──────────────────────────────────────────────────────────────
        private void SetUiDownloadingState(bool downloading)
        {
            _isDownloading = downloading;
            buttonStartDownload.Enabled = !downloading;
            buttonStartDownload.Visible = !downloading;
            buttonCancel.Enabled = downloading;
            buttonCancel.Visible = downloading;
            groupBox2Selection.Enabled = !downloading;
            groupBox1CoordSystem.Enabled = !downloading;
            groupBox4Configurator.Enabled = !downloading;
            groupBox3BasemapParams.Enabled = !downloading;
            groupBox5Download.Enabled = true; // always accessible so cancel works
            // Keep checkboxes and elevation group accessible state via parent
            checkBoxGenerate2DBasemaps.Enabled = !downloading;
            checkBoxPlace2DOnMap.Enabled = !downloading && checkBoxGenerate2DBasemaps.Checked;
            checkBoxGenerate3DBasemaps.Enabled = !downloading;
            checkBoxPlace3DOnMap.Enabled = !downloading && checkBoxGenerate3DBasemaps.Checked;
            checkBoxGenerate3DTerrain.Enabled = !downloading;
            groupBoxElevation.Enabled = !downloading && checkBoxGenerate3DTerrain.Checked;
            Cursor = downloading ? Cursors.WaitCursor : Cursors.Default;
        }

        // ── Input helpers ─────────────────────────────────────────────────────────
        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
                if (sender is TextBox textBox)
                {
                    _warningToolTip.Hide(textBox);
                    _warningToolTip.Show("Tutaj możesz wpisać tylko cyfry!", textBox, 50, -75, 2000);
                }
            }
        }

        // ── Form close ────────────────────────────────────────────────────────────
        private void UnifiedGridToolForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_isDownloading)
            {
                var res = MessageBox.Show(
                    "Operacja jest w toku. Czy na pewno chcesz zamknąć okno i ją przerwać?",
                    "Operacja w toku", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }
                _cancellationTokenSource?.Cancel();
            }
        }

        // ── IMainMenuOperations ───────────────────────────────────────────────────
        public void FindSmallestFreeBasemapNumber()
        {
            UpdateNextFreeCounter();
            StatusUpdate?.Invoke("Automatycznie dobrano numer podkładu: " + textBoxCounter.Text);
        }

        public void FindFreeKuid()
        {
            UpdateNextFreeKuidPart2();
            StatusUpdate?.Invoke("Automatycznie dobrano numer kuidu (część 2): " + textBoxKuidPart2.Text);
        }

        public void RefreshLists()
        {
            RoutesListBoxRefresh();
            StatusUpdate?.Invoke("Odświeżono listę tras");
        }
    }
}
