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

        private class BasemapJob
        {
            public int Index { get; set; }
            public SelectedTileModel Tile { get; set; } = null!;
            public int Counter { get; set; }
            public string Kuid2 { get; set; } = string.Empty;
        }

        // ── Fields ────────────────────────────────────────────────────────────────
        private const int MaxParallelBasemaps = 4;
        private readonly TrainzFileManager _fileManager = new TrainzFileManager();
        private readonly ToolTip _warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "Błąd wprowadzania" };
        private readonly List<SelectedTileModel> _selectedTiles = new List<SelectedTileModel>();
        private readonly System.Windows.Forms.Timer _kuidCounterDebounceTimer = new System.Windows.Forms.Timer { Interval = 300 };
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
            _kuidCounterDebounceTimer.Tick += (s, e) =>
            {
                _kuidCounterDebounceTimer.Stop();
                if (_loadedRoute == null)
                {
                    UpdateNextFreeCounter();
                    if (Properties.Settings.Default.AutoKuidNumber)
                    {
                        UpdateNextFreeKuidPart2();
                    }
                }
            };

            InitializeComponent();

            // Bind map providers
            checkedListBoxMapType.ItemCheck -= checkedListBoxMapType_ItemCheck;
            checkedListBoxMapType.DisplayMember = "Name";
            checkedListBoxMapType.Items.Clear();
            foreach (var map in MapSources.AvailableMaps)
            {
                bool isDefault = map.Name == "Ortofotomapa WMTS";
                checkedListBoxMapType.Items.Add(map, isDefault);
            }
            checkedListBoxMapType.ItemCheck += checkedListBoxMapType_ItemCheck;

            FormHelpers.PopulateAllResolutions(comboBoxResolution, 2048);
            UpdateBasemapParamsState();

            // Initial states
            textBoxBasemapDate.Text = DateTime.Now.Year.ToString();
            textBoxDestinationFolder.Text = "Nowa_Trasa";
            textBoxDesignation.Text = "P";
            textBoxKuidPart1.Text = Properties.Settings.Default.DefaultKuidFirstPart ?? "123456";

            textBoxKuidPart1.TextChanged += (s, e) =>
            {
                if (_loadedRoute == null && Properties.Settings.Default.AutoKuidNumber)
                {
                    _kuidCounterDebounceTimer.Stop();
                    _kuidCounterDebounceTimer.Start();
                }
            };

            comboBoxEpsg.SelectedIndex = 0;
            radioButtonModeClick.Checked = true;
            radioButtonElevationAbsolute.Checked = true;

            buttonLoadRoute.Enabled = false;
            buttonDeleteRoute.Enabled = false;

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
                webView21.CoreWebView2.ProcessFailed += (s, args) =>
                {
                    Debug.WriteLine($"[WebView2 ProcessFailed] Reason: {args.ProcessFailedKind}, Reason: {args.Reason}");
                };
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

        // ── Map sources & resolution handlers ────────────────────────────────────
        private void checkedListBoxMapType_ItemCheck(object? sender, ItemCheckEventArgs e)
        {
            if (IsHandleCreated)
            {
                BeginInvoke(new Action(UpdateBasemapParamsState));
            }
        }

        private void buttonSelectAllMaps_Click(object? sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxMapType.Items.Count; i++)
            {
                checkedListBoxMapType.SetItemChecked(i, true);
            }
            UpdateBasemapParamsState();
        }

        private void buttonDeselectAllMaps_Click(object? sender, EventArgs e)
        {
            for (int i = 0; i < checkedListBoxMapType.Items.Count; i++)
            {
                checkedListBoxMapType.SetItemChecked(i, false);
            }
            UpdateBasemapParamsState();
        }

        private void UpdateBasemapParamsState()
        {
            var checkedSources = checkedListBoxMapType.CheckedItems.Cast<IMapSource>().ToList();
            bool anySupportsTime = checkedSources.Any(s => s.SupportsTime);
            textBoxBasemapDate.Enabled = anySupportsTime;
            label14.Enabled = anySupportsTime;
            labelMapSelectionCount.Text = $"Wybrano: {checkedSources.Count}";

            // Suggest coordinate system if only XYZ sources are checked
            if (checkedSources.Count > 0 && checkedSources.All(s => s is XyzTileMapSource))
            {
                if (comboBoxEpsg.SelectedIndex != 1)
                    comboBoxEpsg.SelectedIndex = 1;
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
                _kuidCounterDebounceTimer?.Stop();
                _kuidCounterDebounceTimer?.Start();
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

            var selectedMaps = checkedListBoxMapType.CheckedItems.Cast<IMapSource>().ToList();
            if ((gen2D || gen3D) && selectedMaps.Count == 0)
            {
                MessageBox.Show("Zaznacz co najmniej jeden rodzaj podkładu na liście!", "Brak wyboru podkładu",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
            string year = textBoxBasemapDate.Text.Trim();
            int maxResolution = GetSelectedResolution();
            var tilesToProcess = _selectedTiles.OrderBy(t => t.Order).ToList();
            int total = tilesToProcess.Count;

            // When a route is generated, the route itself occupies <kuid:kuidPart1:kuidPart2>.
            // Offset basemaps startKuid2 by +1 so that basemap KUIDs do not collide with the route!
            if (needsRoute)
            {
                startKuid2++;
            }

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
                var workingResolutions = new ConcurrentDictionary<IMapSource, int>();
                var downloadedMapImages = new ConcurrentDictionary<(IMapSource Source, long X, long Y), byte[]>();
                var basemap2DInfo = new List<(IMapSource Source, SelectedTileModel Tile, string Kuid1, string Kuid2)>();

                if (gen2D && selectedMaps.Count > 0)
                {
                    int total2DTasks = selectedMaps.Count * total;
                    int current2DTask = 0;
                    progressBar1.Maximum = Math.Max(1, total2DTasks);
                    progressBar1.Value = 0;

                    foreach (var source in selectedMaps)
                    {
                        token.ThrowIfCancellationRequested();
                        string safeSourceName = string.Join("_", source.Name.Split(Path.GetInvalidFileNameChars()))
                            .Replace(" ", "_").Replace(",", "");
                        string groupName2D = selectedMaps.Count > 1
                            ? $"Podklady_2D_{routeName}_{safeSourceName}"
                            : $"Podklady_2D_{routeName}";

                        string mapYear = source.SupportsTime ? year : "";
                        int doneForSource = 0;

                        // Pre-assign counters/KUIDs so numbering is deterministic regardless of completion order.
                        var jobs = AssignBasemapNumbers(tilesToProcess, ref startCounter, ref startKuid2);
                        StatusUpdate?.Invoke($"Pobieranie 2D ({source.Name})...");

                        var results = await RunThrottledAsync(jobs, MaxParallelBasemaps, async job =>
                        {
                            try
                            {
                                byte[] imageBytes = await GetOrDownloadMapImageAsync(
                                    downloadedMapImages, source, mapYear, job.Tile, maxResolution, token, workingResolutions);

                                // File I/O off the UI thread.
                                bool created = await Task.Run(() => _fileManager.CreateTrainzFiles(
                                    imageBytes, groupName2D,
                                    job.Tile.X, job.Tile.Y,
                                    designation, job.Counter,
                                    kuidPart1, job.Kuid2,
                                    force2D: true,
                                    overwrite: true), token);

                                return (job, created, error: (Exception?)null);
                            }
                            catch (OperationCanceledException) { throw; }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Błąd podkładu 2D {source.Name} tile ({job.Tile.I},{job.Tile.J}): {ex.Message}");
                                return (job, created: false, error: ex);
                            }
                            finally
                            {
                                int c = Interlocked.Increment(ref current2DTask);
                                int d = Interlocked.Increment(ref doneForSource);
                                if (!IsDisposed && IsHandleCreated)
                                {
                                    BeginInvoke(() =>
                                    {
                                        labelProgress.Text = $"2D [{source.Name}]: {d}/{total}";
                                        progressBar1.Value = Math.Min(c, progressBar1.Maximum);
                                    });
                                }
                            }
                        }, token);

                        int successCount = 0;
                        int failCount = 0;
                        foreach (var r in results.OrderBy(r => r.job.Index))
                        {
                            if (r.created)
                            {
                                successCount++;
                                basemap2DInfo.Add((source, r.job.Tile, kuidPart1, r.job.Kuid2));
                            }
                            else if (r.error != null)
                            {
                                failCount++;
                            }
                        }

                        StatusUpdate?.Invoke($"Podkłady 2D ({source.Name}): utworzono {successCount}, błędów {failCount}.");
                    }
                }

                // ──────────────────────────────────────────────────────────────────
                // PHASE 3: Generate 3D basemaps
                // ──────────────────────────────────────────────────────────────────
                var basemap3DInfo = new List<(IMapSource Source, SelectedTileModel Tile, string Kuid1, string Kuid2, float BaseHeight)>();

                if (gen3D && selectedMaps.Count > 0)
                {
                    int total3DTasks = selectedMaps.Count * total;
                    int current3DTask = 0;
                    progressBar1.Maximum = Math.Max(1, total3DTasks);
                    progressBar1.Value = 0;

                    foreach (var source in selectedMaps)
                    {
                        token.ThrowIfCancellationRequested();
                        string safeSourceName = string.Join("_", source.Name.Split(Path.GetInvalidFileNameChars()))
                            .Replace(" ", "_").Replace(",", "");
                        string groupName3D = selectedMaps.Count > 1
                            ? $"Podklady_3D_{routeName}_{safeSourceName}"
                            : $"Podklady_3D_{routeName}";

                        string mapYear = source.SupportsTime ? year : "";
                        int doneForSource = 0;

                        var jobs = AssignBasemapNumbers(tilesToProcess, ref startCounter, ref startKuid2);
                        StatusUpdate?.Invoke($"Pobieranie 3D ({source.Name})...");

                        var results = await RunThrottledAsync(jobs, MaxParallelBasemaps, async job =>
                        {
                            float baseHeight = 0f;
                            try
                            {
                                byte[] imageBytes = await GetOrDownloadMapImageAsync(
                                    downloadedMapImages, source, mapYear, job.Tile, maxResolution, token, workingResolutions);

                                float[,]? grid = null;
                                if (downloadedGrids != null && downloadedGrids.TryGetValue((job.Tile.I, job.Tile.J), out var foundGrid))
                                {
                                    grid = foundGrid;
                                    baseHeight = foundGrid[38, 38];
                                }

                                // Mesh generation (TrainzMeshImporter process) and file I/O off the UI thread.
                                bool created = await Task.Run(() => _fileManager.CreateTrainzFiles(
                                    imageBytes, groupName3D,
                                    job.Tile.X, job.Tile.Y,
                                    designation, job.Counter,
                                    kuidPart1, job.Kuid2,
                                    grid,
                                    0.2f,  // zOffset to prevent Z-fighting
                                    baseHeight,
                                    overwrite: true), token);

                                return (job, created, baseHeight, error: (Exception?)null);
                            }
                            catch (OperationCanceledException) { throw; }
                            catch (Exception ex)
                            {
                                Debug.WriteLine($"Błąd podkładu 3D {source.Name} tile ({job.Tile.I},{job.Tile.J}): {ex.Message}");
                                return (job, created: false, baseHeight, error: ex);
                            }
                            finally
                            {
                                int c = Interlocked.Increment(ref current3DTask);
                                int d = Interlocked.Increment(ref doneForSource);
                                if (!IsDisposed && IsHandleCreated)
                                {
                                    BeginInvoke(() =>
                                    {
                                        labelProgress.Text = $"3D [{source.Name}]: {d}/{total}";
                                        progressBar1.Value = Math.Min(c, progressBar1.Maximum);
                                    });
                                }
                            }
                        }, token);

                        int successCount = 0;
                        int failCount = 0;
                        foreach (var r in results.OrderBy(r => r.job.Index))
                        {
                            if (r.created)
                            {
                                successCount++;
                                basemap3DInfo.Add((source, r.job.Tile, kuidPart1, r.job.Kuid2, r.baseHeight));
                            }
                            else if (r.error != null)
                            {
                                failCount++;
                            }
                        }

                        StatusUpdate?.Invoke($"Podkłady 3D ({source.Name}): utworzono {successCount}, błędów {failCount}.");
                    }
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
                // PHASE 5: Build obs objects and Trainz layers
                // ──────────────────────────────────────────────────────────────────
                var obsObjects = new List<ObsObject>();
                var layers = new List<TrainzLayer>();
                layers.Add(new TrainzLayer(0, "route-layer", 0x01));

                if (place2D && basemap2DInfo.Count > 0)
                {
                    var groupedBySource = basemap2DInfo.GroupBy(b => b.Source);
                    foreach (var group in groupedBySource)
                    {
                        byte layerId = (byte)layers.Count;
                        string layerName = selectedMaps.Count > 1
                            ? $"Podklady 2D - {group.Key.Name}"
                            : "Podklady 2D";
                        layers.Add(new TrainzLayer(layerId, layerName, 0x01));

                        foreach (var (_, tile, k1, k2) in group.OrderBy(b => b.Tile.Order))
                        {
                            float zPos = 0.1f;
                            if (downloadedGrids != null && downloadedGrids.TryGetValue((tile.I, tile.J), out var grid))
                            {
                                zPos = grid[38, 38] + 0.1f;
                            }
                            else if (_loadedGndBlocks.TryGetValue((-tile.J, tile.I), out var block))
                            {
                                zPos = block.Heights[38, 38] + 0.1f;
                            }

                            obsObjects.Add(new ObsObject
                            {
                                KuidPart1 = int.Parse(k1),
                                KuidPart2 = int.Parse(k2),
                                LayerId = layerId,
                                SegX = (short)(-tile.J),
                                SegY = (short)tile.I,
                                X = 360f,
                                Y = 360f,
                                Z = zPos,
                                RotZ = 0f
                            });
                        }
                    }
                }

                if (place3D && basemap3DInfo.Count > 0)
                {
                    var groupedBySource = basemap3DInfo.GroupBy(b => b.Source);
                    foreach (var group in groupedBySource)
                    {
                        byte layerId = (byte)layers.Count;
                        string layerName = selectedMaps.Count > 1
                            ? $"Podklady 3D - {group.Key.Name}"
                            : "Podklady 3D";
                        layers.Add(new TrainzLayer(layerId, layerName, 0x01));

                        foreach (var (_, tile, k1, k2, baseHeight) in group.OrderBy(b => b.Tile.Order))
                        {
                            obsObjects.Add(new ObsObject
                            {
                                KuidPart1 = int.Parse(k1),
                                KuidPart2 = int.Parse(k2),
                                LayerId = layerId,
                                SegX = (short)(-tile.J),
                                SegY = (short)tile.I,
                                X = 360f,
                                Y = 360f,
                                Z = baseHeight,
                                RotZ = (float)(Math.PI / 2.0)
                            });
                        }
                    }
                }

                // ──────────────────────────────────────────────────────────────────
                // PHASE 6: Save route files (if a route is needed)
                // ──────────────────────────────────────────────────────────────────
                if (needsRoute && gndData != null && routeInfo != null)
                {
                    string targetFolder = _fileManager.CreateRouteFiles(
                        routeName, "Trasy", kuidPart1, kuidPart2, gndData, routeInfo,
                        obsObjects.Count > 0 ? obsObjects : null,
                        layers.Count > 1 ? layers : null);
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
                        try
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
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Błąd odświeżania mapy po zapisie: {ex.Message}");
                        }
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
                if (layers.Count > 1) summaryParts.Add($"warstwy Trainz ({layers.Count})");

                string summary = string.Join(", ", summaryParts);
                StatusUpdate?.Invoke($"Zakończono: {summary}.");
                BeginInvoke(new Action(() =>
                {
                    MessageBox.Show($"Pomyślnie zakończono operację!\n\nWygenerowano: {summary}.",
                        "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
            catch (OperationCanceledException)
            {
                labelProgress.Text = "Anulowano.";
                StatusUpdate?.Invoke("Operacja anulowana przez użytkownika.");
                BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Operacja została przerwana.", "Anulowano", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }));
            }
            catch (Exception ex)
            {
                labelProgress.Text = "Błąd!";
                StatusUpdate?.Invoke("Wystąpił błąd!");
                BeginInvoke(new Action(() =>
                {
                    MessageBox.Show("Błąd podczas generowania:\n\n" + ex.Message,
                        "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }));
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
        /// resolutions (never exceeds the requested max nor provider's capabilities).
        /// Remembers the highest working resolution per source to avoid retrying failed higher resolutions.
        /// </summary>
        private static async Task<byte[]> DownloadWithResolutionFallback(
            IMapSource source, string year, long x, long y, int maxResolution,
            CancellationToken token,
            ConcurrentDictionary<IMapSource, int>? workingResolutions = null)
        {
            int sourceMax = FormHelpers.GetMaxSupportedResolution(source);
            int startResolution = Math.Min(maxResolution, sourceMax);

            if (workingResolutions != null && workingResolutions.TryGetValue(source, out int knownWorking))
            {
                startResolution = Math.Min(startResolution, knownWorking);
            }

            // Build the resolution ladder: start at startResolution, step down to 512
            int[] resolutions = { 8192, 4096, 2048, 1024, 512 };
            var ladder = resolutions.Where(r => r <= startResolution).ToArray();
            if (ladder.Length == 0) ladder = new[] { 512 };

            Exception? lastEx = null;
            foreach (int res in ladder)
            {
                try
                {
                    // For speculative higher resolutions in the ladder, don't stall on 3 retries with pauses
                    int retries = (res == ladder.Last()) ? 3 : 1;
                    int delay = (res == ladder.Last()) ? 3 : 1;
                    byte[] bytes = await source.GetMapImageAsync(year, x, y, res, retries, delay, cancellationToken: token);
                    if (workingResolutions != null)
                    {
                        workingResolutions[source] = res;
                    }
                    return bytes;
                }
                catch (OperationCanceledException) { throw; }
                catch (Exception ex)
                {
                    lastEx = ex;
                    Debug.WriteLine($"Resolution {res} failed for {source.Name} ({x},{y}): {ex.Message}. Trying lower...");
                }
            }
            throw new Exception($"Nie udało się pobrać podkładu dla ({x},{y}) w żadnej dostępnej rozdzielczości.", lastEx);
        }

        private static List<BasemapJob> AssignBasemapNumbers(
            List<SelectedTileModel> tiles, ref int startCounter, ref int startKuid2)
        {
            var list = new List<BasemapJob>(tiles.Count);
            for (int i = 0; i < tiles.Count; i++)
            {
                list.Add(new BasemapJob
                {
                    Index = i,
                    Tile = tiles[i],
                    Counter = startCounter++,
                    Kuid2 = (startKuid2++).ToString()
                });
            }
            return list;
        }

        private static async Task<byte[]> GetOrDownloadMapImageAsync(
            ConcurrentDictionary<(IMapSource Source, long X, long Y), byte[]> cache,
            IMapSource source,
            string mapYear,
            SelectedTileModel tile,
            int maxResolution,
            CancellationToken token,
            ConcurrentDictionary<IMapSource, int> workingResolutions)
        {
            if (cache.TryGetValue((source, tile.X, tile.Y), out var cached))
            {
                return cached;
            }

            byte[] bytes = await DownloadWithResolutionFallback(source, mapYear, tile.X, tile.Y, maxResolution, token, workingResolutions).ConfigureAwait(false);
            cache[(source, tile.X, tile.Y)] = bytes;
            return bytes;
        }

        private static async Task<List<TResult>> RunThrottledAsync<TItem, TResult>(
            IEnumerable<TItem> items,
            int maxDegreeOfParallelism,
            Func<TItem, Task<TResult>> processor,
            CancellationToken token)
        {
            using var semaphore = new SemaphoreSlim(maxDegreeOfParallelism, maxDegreeOfParallelism);
            var tasks = items.Select(async item =>
            {
                await semaphore.WaitAsync(token).ConfigureAwait(false);
                try
                {
                    token.ThrowIfCancellationRequested();
                    return await processor(item).ConfigureAwait(false);
                }
                finally
                {
                    semaphore.Release();
                }
            });

            return (await Task.WhenAll(tasks).ConfigureAwait(false)).ToList();
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

            _kuidCounterDebounceTimer.Dispose();
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
