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
    public partial class GridToolForm : Form
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
        private readonly ToolTip _warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "Błąd wprowadzania" };
        private readonly List<SelectedTileModel> _selectedTiles = new List<SelectedTileModel>();
        private CancellationTokenSource? _cancellationTokenSource;
        private bool _isDownloading = false;

        public GridToolForm()
        {
            InitializeComponent();

            // Set initial control states
            textBoxDestinationFolder.Text = "Podkłady_obszar";
            textBoxDesignation.Text = "P";
            textBoxBasemapDate.Text = DateTime.Now.Year.ToString();
            textBoxKuidPart1.Text = Properties.Settings.Default.DefaultKuidFirstPart ?? "123456";

            // Bind map providers
            comboBoxMapType.DataSource = MapSources.AvailableMaps;
            comboBoxMapType.DisplayMember = "Name";
            comboBoxMapType.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxMapType.DrawItem += ComboBoxMapType_DrawItem;

            comboBoxMapType_SelectedIndexChanged(comboBoxMapType, EventArgs.Empty);

            UpdateNextFreeKuidPart2();
            UpdateNextFreeCounter();
        }

        private async void GridToolForm_Load(object sender, EventArgs e)
        {
            await InitBrowser();
        }

        private async Task InitBrowser()
        {
            try
            {
                await webView21.EnsureCoreWebView2Async(null);

                webView21.CoreWebView2.Settings.UserAgent = "TrainzBasemapMaker/v0.5.0-alpha (https://github.com/Ignacy110/TrainzBasemapMaker)";

                string indexPath = Path.Combine(Application.StartupPath, "Forms", "GridToolForm", "Web", "grid_map.html");
                webView21.CoreWebView2.Navigate("file:///" + indexPath);

                webView21.CoreWebView2.WebMessageReceived += WebView21_WebMessageReceived;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd inicjalizacji komponentu mapy:\n\n" + ex.Message, "Błąd WebView2", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

                    if (type == "selection_changed")
                    {
                        int count = root.GetProperty("count").GetInt32();
                        _selectedTiles.Clear();

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

                        labelTileCount.Text = $"Zaznaczono kafli: {count}";
                        labelArea.Text = $"Powierzchnia: {(count * 0.25):F2} km²";

                        labelProgress.Text = count > 0
                            ? $"Gotowy do pobrania {count} kafli."
                            : "Zaznacz kafle na mapie.";
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error processing web message: " + ex.Message);
            }
        }

        private async void RadioButtonEpsg_CheckedChanged(object? sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;

            string epsg = radioButtonEpsg2180.Checked ? "EPSG:2180" : "EPSG:3857";
            await webView21.CoreWebView2.ExecuteScriptAsync($"setCoordinateSystem('{epsg}')");
        }

        private async void buttonSelectViewport_Click(object sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;
            await webView21.CoreWebView2.ExecuteScriptAsync("selectCurrentViewport()");
        }

        private async void buttonClearSelection_Click(object sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;
            await webView21.CoreWebView2.ExecuteScriptAsync("clearAllTiles()");
        }

        private async void buttonResetAnchor_Click(object sender, EventArgs e)
        {
            if (webView21.CoreWebView2 == null) return;
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
                MessageBox.Show("Nie zaznaczono żadnych kafli na mapie!", "Brak zaznaczenia", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxDestinationFolder.Text))
            {
                MessageBox.Show("Wpisz nazwę folderu docelowego!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxDesignation.Text))
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

            if (comboBoxMapType.SelectedItem is not IMapSource selectedMap)
            {
                MessageBox.Show("Wybierz źródło mapy!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string targetGroup = textBoxDestinationFolder.Text.Trim();
            string targetDesignation = textBoxDesignation.Text.Trim();
            string kuidPart1 = textBoxKuidPart1.Text.Trim();
            string year = selectedMap.SupportsTime ? textBoxBasemapDate.Text.Trim() : "";
            int res = GetSelectedResolution();

            var tilesToDownload = _selectedTiles.Select(t => new SelectedTileModel
            {
                Order = t.Order,
                I = t.I,
                J = t.J,
                X = t.X,
                Y = t.Y
            }).ToList();

            _cancellationTokenSource = new CancellationTokenSource();
            SetUiDownloadingState(true);

            int total = tilesToDownload.Count;
            int current = 0;
            int successCount = 0;
            List<string> failureDetails = new List<string>();

            progressBar1.Minimum = 0;
            progressBar1.Maximum = total;
            progressBar1.Value = 0;

            try
            {
                foreach (var tile in tilesToDownload)
                {
                    if (_cancellationTokenSource.Token.IsCancellationRequested)
                    {
                        break;
                    }

                    current++;
                    int currentCounter = startCounter + (current - 1);
                    string currentKuid2 = (startKuid2 + (current - 1)).ToString();

                    labelProgress.Text = $"Pobieranie: {current} z {total} (Kafel {tile.Order})...";
                    if (webView21.CoreWebView2 != null)
                    {
                        await webView21.CoreWebView2.ExecuteScriptAsync($"highlightTile({tile.Order}, 'downloading')");
                    }

                    try
                    {
                        byte[] imageBytes = await selectedMap.GetMapImageAsync(year, tile.X, tile.Y, res);

                        bool created = _fileManager.CreateTrainzFiles(
                            imageBytes,
                            targetGroup,
                            tile.X, tile.Y,
                            targetDesignation,
                            currentCounter,
                            kuidPart1,
                            currentKuid2
                        );

                        if (created)
                        {
                            successCount++;
                            if (webView21.CoreWebView2 != null)
                            {
                                await webView21.CoreWebView2.ExecuteScriptAsync($"highlightTile({tile.Order}, 'done')");
                            }
                        }
                        else
                        {
                            failureDetails.Add($"Kafel {tile.Order} ({tile.X}, {tile.Y}): Podkład o tych współrzędnych już istnieje w folderze.");
                            if (webView21.CoreWebView2 != null)
                            {
                                await webView21.CoreWebView2.ExecuteScriptAsync($"highlightTile({tile.Order}, 'error')");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        failureDetails.Add($"Kafel {tile.Order}: {ex.Message}");
                        if (webView21.CoreWebView2 != null)
                        {
                            await webView21.CoreWebView2.ExecuteScriptAsync($"highlightTile({tile.Order}, 'error')");
                        }
                    }

                    progressBar1.Value = current;
                }

                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    labelProgress.Text = $"Pobieranie anulowane. Utworzono {successCount} z {total} podkładów.";
                    MessageBox.Show($"Pobieranie zostało przerwane przez użytkownika.\n\nPomyślnie utworzono {successCount} z {total} podkładów.", "Pobieranie anulowane", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (failureDetails.Count == 0)
                {
                    labelProgress.Text = $"Zakończono sukcesem! Utworzono {successCount} podkładów.";
                    MessageBox.Show($"Pobieranie obszarowe zakończone sukcesem!\n\nPomyślnie utworzono wszystkie zaznaczone podkłady ({successCount} z {total}).", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (successCount > 0)
                {
                    labelProgress.Text = $"Zakończono z ostrzeżeniami: {successCount} z {total}.";
                    string errorsPreview = string.Join("\n", failureDetails.Take(5));
                    if (failureDetails.Count > 5) errorsPreview += $"\n... i {failureDetails.Count - 5} innych błędów.";
                    MessageBox.Show($"Pobieranie obszarowe zakończone z ostrzeżeniami.\n\nUtworzono podkładów: {successCount} z {total}.\nNiepowodzenia ({failureDetails.Count}):\n{errorsPreview}", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    labelProgress.Text = $"Pobieranie nie powiodło się dla żadnego podkładu.";
                    string errorsPreview = string.Join("\n", failureDetails.Take(5));
                    MessageBox.Show($"Pobieranie nie powiodło się dla żadnego podkładu.\n\nSzczegóły błędów:\n{errorsPreview}", "Błąd pobierania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd krytyczny podczas pobierania:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                SetUiDownloadingState(false);
                UpdateNextFreeCounter();
                UpdateNextFreeKuidPart2();
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            _cancellationTokenSource?.Cancel();
            buttonCancel.Enabled = false;
            labelProgress.Text = "Anulowanie pobierania...";
        }

        private void SetUiDownloadingState(bool downloading)
        {
            _isDownloading = downloading;
            buttonStartDownload.Enabled = !downloading;
            buttonCancel.Enabled = downloading;
            groupBoxCoordSystem.Enabled = !downloading;
            groupBoxParams.Enabled = !downloading;
            groupBoxTrainz.Enabled = !downloading;
            buttonSelectViewport.Enabled = !downloading;
            buttonClearSelection.Enabled = !downloading;
            buttonResetAnchor.Enabled = !downloading;
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
                labelDate.Enabled = selected.SupportsTime;

                radioButton4096.Enabled = selected.AllowsHighResolution;
                if (!selected.AllowsHighResolution && radioButton4096.Checked)
                {
                    radioButton2048.Checked = true;
                }
            }
        }

        private void ComboBoxMapType_DrawItem(object? sender, DrawItemEventArgs e)
        {
            if (e.Index < 0) return;

            var comboBox = (ComboBox?)sender;
            if (comboBox?.Items[e.Index] is IMapSource mapSource)
            {
                bool isSelected = (e.State & DrawItemState.Selected) == DrawItemState.Selected;

                Color backColor = isSelected
                    ? SystemColors.Highlight
                    : (mapSource is XyzTileMapSource ? Color.FromArgb(255, 255, 204) : e.BackColor);

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
                    _warningToolTip.Show("Tutaj możesz wpisać tylko cyfry!", textBox, 50, -75, 2000);
                }
            }
        }

        private void GridToolForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (_isDownloading)
            {
                var res = MessageBox.Show("Pobieranie podkładów jest w toku. Czy na pewno chcesz zamknąć okno i przerwać pobieranie?", "Pobieranie w toku", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                if (res == DialogResult.No)
                {
                    e.Cancel = true;
                    return;
                }

                _cancellationTokenSource?.Cancel();
            }
        }
    }
}
