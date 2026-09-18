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
using System.Diagnostics;
using System.Drawing;
using System.Globalization;
using System.Text.Json;
using TrainzBasemapMaker.Classes;

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
        private readonly ToolTip _warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "BÅ‚Ä…d wprowadzania" };
        private readonly List<SelectedTileModel> _selectedTiles = new List<SelectedTileModel>();
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isDownloading = false;
        private long? _currentAnchorX;
        private long? _currentAnchorY;

        public TerrainGridToolForm()
        {
            InitializeComponent();

            // Set initial control states
            textBoxDestinationFolder.Text = "PodkÅ‚ady_obszar";
            textBoxDesignation.Text = "P";
            textBoxBasemapDate.Text = DateTime.Now.Year.ToString();
            textBoxKuidPart1.Text = Properties.Settings.Default.DefaultKuidFirstPart ?? "123456";

            radioButton2048.Checked = true;
            radioButtonEpsg2180.Checked = true;
            radioButtonModeClick.Checked = true;

            // Bind map providers
            comboBoxMapType.DataSource = MapSources.AvailableMaps;
            comboBoxMapType.DisplayMember = "Name";
            comboBoxMapType.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxMapType.DrawItem += ComboBoxMapType_DrawItem;

            comboBoxMapType_SelectedIndexChanged(comboBoxMapType, EventArgs.Empty);

            BasemapFolderListBoxRefresh();
            UpdateNextFreeKuidPart2();
            UpdateNextFreeCounter();
            toolStripStatusLabel1.Text = "LPM: Kliknij lub przeciÄ…gnij pÄ™dzlem, aby zaznaczyÄ‡ | PPM: Przesuwanie mapy";
        }

                private async void TerrainGridToolForm_Load(object? sender, EventArgs e)
        {
            this.Size = Properties.Settings.Default.GridToolFormSize;
            this.WindowState = Properties.Settings.Default.GridToolFormState;
            TrainzBasemapMaker.Classes.ThemeManager.ApplyTheme(this);
            
            // Customize UI for Terrain Generation
            this.Text = "Generator terenu (map.gnd)";
            groupBox3Config.Text = "3. Konfiguracja trasy";
            label4.Text = "Nazwa trasy:";
            textBoxDestinationFolder.Text = "Nowa_Mapa";
            
            // Hide everything else in Config group
            comboBoxMapType.Visible = false;
            label15.Visible = false;
            radioButton4096.Visible = false;
            radioButton2048.Visible = false;
            radioButton1024.Visible = false;
            radioButton512.Visible = false;
            label2.Visible = false;
            label14.Visible = false;
            textBoxBasemapDate.Visible = false;
            
            textBoxDesignation.Visible = false;
            label13.Visible = false;
            textBoxCounter.Visible = false;
            label11.Visible = false;
            label10.Visible = false;
            basemapFolderListBox.Visible = false;
            buttonLoadFolder.Visible = false;
            
            // Move Route Name up
            label4.Location = new System.Drawing.Point(6, 30);
            textBoxDestinationFolder.Location = new System.Drawing.Point(6, 48);
            
            // Move KUID up
            label12.Location = new System.Drawing.Point(6, 80);
            panelTrainzFiles.Location = new System.Drawing.Point(0, 98);

            await InitBrowser();
        }

        private async Task InitBrowser()
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);

                webView21.CoreWebView2.Settings.UserAgent = "TrainzBasemapMaker/v0.5.0-alpha (https://github.com/Ignacy110/TrainzBasemapMaker)";

                string indexPath = Path.Combine(Application.StartupPath, "Forms", "GridToolForm", "Web", "grid_map.html");
                
                webView21.CoreWebView2.NavigationCompleted += async (sender, args) =>
                {
                    if (args.IsSuccess)
                    {
                        await webView21.CoreWebView2.ExecuteScriptAsync($"setTileSize({TrainzBasemapMaker.Classes.MapSourceBase.TileSize});");
                    }
                };
                
                webView21.CoreWebView2.Navigate("file:///" + indexPath);

                webView21.CoreWebView2.WebMessageReceived += WebView21_WebMessageReceived;
            }
                        catch (Exception ex)
            {
                System.IO.File.WriteAllText("error.log", ex.ToString());
                toolStripStatusLabel1.Text = "B³¹d generowania!";
                MessageBox.Show("Wyst¹pi³ b³¹d podczas generowania mapy (szczegó³y w error.log):\n\n" + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BasemapFolderListBoxRefresh()
        {
            basemapFolderListBox.Items.Clear();
            var groups = _fileManager.GetBasemapGroups();
            basemapFolderListBox.Items.AddRange(groups.ToArray());
        }

        private void basemapFolderListBox_Click(object? sender, EventArgs e)
        {
            if (basemapFolderListBox.SelectedItem is string selectedItem)
            {
                textBoxDestinationFolder.Text = selectedItem;
            }
        }

        private async void buttonLoadFolder_Click(object? sender, EventArgs e)
        {
            await LoadExistingTilesFromSelectedFolder();
        }

        private async void basemapFolderListBox_DoubleClick(object? sender, EventArgs e)
        {
            await LoadExistingTilesFromSelectedFolder();
        }

        private async Task LoadExistingTilesFromSelectedFolder()
        {
            if (basemapFolderListBox.SelectedItem is not string selectedGroup)
            {
                MessageBox.Show("Wybierz folder z listy, aby wczytaÄ‡ znajdujÄ…ce siÄ™ w nim podkÅ‚ady.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            if (webView21.CoreWebView2 == null)
            {
                MessageBox.Show("Komponent mapy jeszcze siÄ™ inicjalizuje. SprÃ³buj ponownie za chwilÄ™.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var kuids = _fileManager.GetKuidsInGroup(selectedGroup);
            if (kuids.Count == 0)
            {
                MessageBox.Show($"Folder \"{selectedGroup}\" jest pusty.", "Informacja", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var parsedTiles = new List<TileFolderInfo>();
            foreach (var folderName in kuids)
            {
                if (TrainzFileManager.TryParseTileFolderName(folderName, out var tileInfo))
                {
                    parsedTiles.Add(tileInfo);
                }
            }

            if (parsedTiles.Count == 0)
            {
                MessageBox.Show($"W folderze \"{selectedGroup}\" nie znaleziono podkÅ‚adÃ³w o rozpoznanym formacie nazwy.", "OstrzeÅ¼enie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Check if group_info.json exists to retrieve the original anchor and settings
            var groupInfo = _fileManager.GetGroupInfo(selectedGroup);

            bool isEpsg2180;
            long anchorX, anchorY;

            if (groupInfo != null && groupInfo.AnchorX.HasValue && groupInfo.AnchorY.HasValue)
            {
                isEpsg2180 = groupInfo.Epsg == "EPSG:2180";
                anchorX = groupInfo.AnchorX.Value;
                anchorY = groupInfo.AnchorY.Value;

                if (!string.IsNullOrWhiteSpace(groupInfo.Designation))
                {
                    textBoxDesignation.Text = groupInfo.Designation;
                }

                // Restore map source if available in dropdown
                if (!string.IsNullOrWhiteSpace(groupInfo.MapSource))
                {
                    for (int k = 0; k < comboBoxMapType.Items.Count; k++)
                    {
                        if (comboBoxMapType.Items[k] is IMapSource ms && ms.Name.Equals(groupInfo.MapSource, StringComparison.OrdinalIgnoreCase))
                        {
                            comboBoxMapType.SelectedIndex = k;
                            break;
                        }
                    }
                }
            }
            else
            {
                // Fallback: determine EPSG and anchor from first parsed tile
                isEpsg2180 = GeoHelperEPSG2180.IsWithin2180Bounds(parsedTiles[0].X, parsedTiles[0].Y);
                anchorX = parsedTiles[0].X;
                anchorY = parsedTiles[0].Y;

                if (!string.IsNullOrWhiteSpace(parsedTiles[0].Designation))
                {
                    textBoxDesignation.Text = parsedTiles[0].Designation;
                }
            }

            _currentAnchorX = anchorX;
            _currentAnchorY = anchorY;

            if (isEpsg2180)
            {
                radioButtonEpsg2180.Checked = true;
            }
            else
            {
                radioButtonEpsg3857.Checked = true;
            }

            textBoxDestinationFolder.Text = selectedGroup;

            UpdateNextFreeCounter();
            UpdateNextFreeKuidPart2();

            // Build payload for JS
            var payload = new
            {
                epsg = isEpsg2180 ? "EPSG:2180" : "EPSG:3857",
                anchor = new { x = anchorX, y = anchorY },
                tiles = parsedTiles.Select(t => new
                {
                    x = t.X,
                    y = t.Y,
                    counter = t.Counter,
                    designation = t.Designation,
                    kuid1 = t.KuidPart1,
                    kuid2 = t.KuidPart2
                })
            };

            string json = JsonSerializer.Serialize(payload);
            await webView21.CoreWebView2.ExecuteScriptAsync($"loadExistingFolderTiles({json})");

            string anchorSourceText = groupInfo != null ? " (zapisany punkt bazowy)" : "";
            toolStripStatusLabel1.Text = $"Wczytano {parsedTiles.Count} podkÅ‚adÃ³w z folderu \"{selectedGroup}\"{anchorSourceText} i ustawiono siatkÄ™ lokalnÄ….";
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
                            ? $"Zaznaczono nowych: {count} (Wczytano: {existingCount})"
                            : $"Zaznaczono kafli: {count}";

                        labelTileCount.Text = tileText;
                        labelArea.Text = $"Powierzchnia: {((count + existingCount) * 0.25):F2} kmÂ²";

                        string statusMsg = count > 0
                            ? $"Zaznaczono {count} nowych kafli do pobrania."
                            : (existingCount > 0 ? $"Wczytano {existingCount} istniejÄ…cych kafli. Kliknij na siatce, aby dodaÄ‡ nowe." : "Zaznacz kafle na mapie.");

                        labelProgress.Text = count > 0 ? $"Zaznaczono {count} nowych kafli." : "Gotowy do zaznaczania.";
                        toolStripStatusLabel1.Text = statusMsg;
                    }
                }
            }
                        catch (Exception ex)
            {
                System.IO.File.WriteAllText("error.log", ex.ToString());
                toolStripStatusLabel1.Text = "B³¹d generowania!";
                MessageBox.Show("Wyst¹pi³ b³¹d podczas generowania mapy (szczegó³y w error.log):\n\n" + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

        private async void buttonResetAnchor_Click(object sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;
            _currentAnchorX = null;
            _currentAnchorY = null;
            await webView21.CoreWebView2.ExecuteScriptAsync("resetGridOrigin()");
        }

        private void textBoxDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            UpdateNextFreeCounter();
        }

        private void UpdateNextFreeCounter()
        {
            try
            {
                string group = textBoxDestinationFolder.Text.Trim();
                int nextCounter = _fileManager.GetNextFreeCounter(group);
                textBoxCounter.Text = nextCounter.ToString();
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
                MessageBox.Show("Nie zaznaczono ¿adnych kafli na mapie!", "Brak zaznaczenia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            if (string.IsNullOrWhiteSpace(textBoxDestinationFolder.Text))
            {
                MessageBox.Show("Wpisz nazwê trasy!", "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string targetGroup = "Trasy"; // Just a folder inside Kuids/
            string routeName = textBoxDestinationFolder.Text.Trim();
            string kuidPart1 = textBoxKuidPart1.Text.Trim();
            string kuidPart2 = textBoxKuidPart2.Text.Trim();
            
            _cancellationTokenSource = new CancellationTokenSource();
            SetUiDownloadingState(true);

            int total = _selectedTiles.Count;
            int current = 0;
            progressBar1.Minimum = 0;
            progressBar1.Maximum = total;
            progressBar1.Value = 0;

            try
            {
                var blocks = new List<TrainzBasemapMaker.Classes.TrainzTerrain.MapGridPart>();
                var wcs = new TrainzBasemapMaker.Classes.WcsElevationProvider();
                
                                // Find anchor tile to get reference elevation
                var anchorTile = _selectedTiles.FirstOrDefault(t => t.I == 0 && t.J == 0);
                if (anchorTile == null) {
                    anchorTile = _selectedTiles.First();
                }
                
                float anchorElevation = 0;
                bool anchorFound = false;

                foreach (var tile in _selectedTiles)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested) break;
                    
                    labelProgress.Text = $"Pobieranie: {current + 1}/{total}";
                    toolStripStatusLabel1.Text = $"Pobieranie wysokoœci... Kafel {tile.I}, {tile.J}";
                    
                    var grid = await wcs.GetElevationGridAsync(tile.X, tile.Y);
                    
                    // On first tile (anchor), calculate anchorElevation = center point
                    if (tile.I == anchorTile.I && tile.J == anchorTile.J && !anchorFound) {
                        anchorElevation = grid[38, 38];
                        anchorFound = true;
                    }

                    // Subtract anchorElevation from all vertices
                    for (int x = 0; x < 76; x++)
                    {
                        for (int y = 0; y < 76; y++)
                        {
                            grid[x, y] -= anchorElevation;
                        }
                    }

                                        // Trainz wymusza, by SegmentX rós³ na po³udnie (Northing, odwrócone Y),
                    // a SegmentY rós³ na wschód (Easting, bezpoœrednie X).
                    // W interfejsie webowym: I roœnie na wschód, J roœnie na pó³noc.
                    // Dlatego: SegmentX = -tile.J, SegmentY = tile.I
                    var part = new TrainzBasemapMaker.Classes.TrainzTerrain.MapGridPart(-tile.J, tile.I);
                    part.SetHeights(grid);
                    blocks.Add(part);
                    
                    current++;
                    progressBar1.Value = current;
                }

                if (!_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    labelProgress.Text = "Generowanie map.gnd...";
                    var writer = new TrainzBasemapMaker.Classes.TrainzTerrain.GndWriter();
                    byte[] gndData = writer.CreateGndFile(blocks);
                    
                    _fileManager.CreateRouteFiles(routeName, targetGroup, kuidPart1, kuidPart2, gndData);
                    
                    labelProgress.Text = "Gotowe!";
                    toolStripStatusLabel1.Text = "Wygenerowano trasê!";
                    MessageBox.Show("Pomyœlnie wygenerowano mapê (map.gnd)!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
                        catch (Exception ex)
            {
                System.IO.File.WriteAllText("error.log", ex.ToString());
                toolStripStatusLabel1.Text = "B³¹d generowania!";
                MessageBox.Show("Wyst¹pi³ b³¹d podczas generowania mapy (szczegó³y w error.log):\n\n" + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Error);
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
            groupBox4Download.Enabled = !downloading;
            Cursor = downloading ? Cursors.WaitCursor : Cursors.Default;
        }

        private int GetSelectedResolution()
        {
            if (radioButton4096.Checked) return 4096;
            if (radioButton1024.Checked) return 1024;
            if (radioButton512.Checked) return 512;
            return 2048;
        }

        private void comboBoxMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMapType.SelectedItem is IMapSource selected)
            {
                textBoxBasemapDate.Enabled = selected.SupportsTime;
                label14.Enabled = selected.SupportsTime;

                radioButton4096.Enabled = selected.AllowsHighResolution;
                if (!selected.AllowsHighResolution && radioButton4096.Checked)
                {
                    radioButton2048.Checked = true;
                }

                // Auto-suggest the native EPSG for the selected provider while keeping both options enabled
                if (selected is XyzTileMapSource)
                {
                    radioButtonEpsg3857.Checked = true;
                }
                else
                {
                    radioButtonEpsg2180.Checked = true;
                }
                radioButtonEpsg2180.Enabled = true;
                radioButtonEpsg3857.Enabled = true;
            }
        }

        private void ComboBoxMapType_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var comboBox = (ComboBox?)sender;
            if (comboBox?.Items[e.Index] is IMapSource mapSource)
            {
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                bool isDarkMode = Properties.Settings.Default.DarkMode;
                Color highlightColor = isDarkMode ? Color.FromArgb(120, 120, 0) : Color.FromArgb(255, 255, 204);

                Color backColor = isSelected
                    ? SystemColors.Highlight
                    : (mapSource is XyzTileMapSource ? highlightColor : e.BackColor);

                Color foreColor = isSelected ? SystemColors.HighlightText : e.ForeColor;

                using (var backBrush = new SolidBrush(backColor))
                {
                    e.Graphics.FillRectangle(backBrush, e.Bounds);
                }

                using (var textBrush = new SolidBrush(foreColor))
                using (var sf = new StringFormat { LineAlignment = StringAlignment.Center, Alignment = StringAlignment.Near })
                {
                    e.Graphics.DrawString(mapSource.Name, e.Font ?? comboBox.Font, textBrush, e.Bounds, sf);
                }

                e.DrawFocusRectangle();
            }
        }

        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;

                if (sender is TextBox textBox)
                {
                    _warningToolTip.Hide(textBox);
                    _warningToolTip.Show("Tutaj moÅ¼esz wpisaÄ‡ tylko cyfry!", textBox, 50, -75, 2000);
                }
            }
        }

        private void TerrainGridToolForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            if (_isDownloading)
            {
                var res = MessageBox.Show("Pobieranie podkÅ‚adÃ³w jest w toku. Czy na pewno chcesz zamknÄ…Ä‡ okno i przerwaÄ‡ pobieranie?", "Pobieranie w toku", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                _cancellationTokenSource?.Cancel();
            }

            Properties.Settings.Default.GridToolFormState = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.GridToolFormSize = this.Size;
            }
            else
            {
                Properties.Settings.Default.GridToolFormSize = this.RestoreBounds.Size;
            }
            Properties.Settings.Default.Save();
        }
    }
}






