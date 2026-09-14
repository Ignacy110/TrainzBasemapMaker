
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

using System.Diagnostics;
using TrainzBasemapMaker.Classes;

namespace TrainzBasemapMaker
{
    public partial class BatchToolForm : Form
    {
        // Handles file and directory operations for Trainz assets
        private TrainzFileManager _fileManager = new TrainzFileManager();

        // ToolTip used to provide visual feedback for input validation errors
        private ToolTip warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "Błąd wprowadzania" };

        public BatchToolForm()
        {
            InitializeComponent();

            // Set default UI states
            radioButton2048.Checked = true;
            radioButtonEpsg2180.Checked = true;
            textBoxBasemapDate.Text = DateTime.Now.Year.ToString();

            // Bind available map sources to the dropdown list
            comboBoxMapType.DataSource = MapSources.AvailableMaps;
            comboBoxMapType.DisplayMember = "Name";
            comboBoxMapType.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxMapType.DrawItem += ComboBoxMapType_DrawItem;

            comboBoxMapType_SelectedIndexChanged(comboBoxMapType, EventArgs.Empty);
            BasemapFolderListBoxRefresh();
        }

        /// <summary>
        /// Clears and repopulates the ListBox with available basemap group folders.
        /// </summary>
        private void BasemapFolderListBoxRefresh()
        {
            basemapFolderListBox.Items.Clear();
            var groups = _fileManager.GetBasemapGroups();
            basemapFolderListBox.Items.AddRange(groups.ToArray());
        }

        private void basemapFolderListBox_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (basemapFolderListBox.SelectedItem is not string selectedGroup) return;

            // Auto-suggest destination folder name
            textBoxDestinationFolder.Text = $"{selectedGroup}_nowe";

            UpdateCoordinateSystemAvailabilityForSelectedGroup();
        }

        private void UpdateCoordinateSystemAvailabilityForSelectedGroup()
        {
            if (basemapFolderListBox.SelectedItem is not string selectedGroup) return;

            var folders = _fileManager.GetKuidsInGroup(selectedGroup);
            if (folders.Count == 0) return;

            string firstFolder = folders[0];
            if (TrainzFileManager.TryParseTileFolderName(firstFolder, out var tileInfo))
            {
                if (!string.IsNullOrWhiteSpace(tileInfo.Designation))
                {
                    textBoxDesignation.Text = tileInfo.Designation;
                }

                if (GeoHelperEPSG2180.IsWithin2180Bounds(tileInfo.X, tileInfo.Y))
                {
                    radioButtonEpsg2180.Enabled = true;
                    radioButtonEpsg3857.Enabled = true;
                    radioButtonEpsg2180.Checked = true;
                }
                else
                {
                    var (lat, lon) = GeoHelperEPSG3857.Meters3857ToLatLon(tileInfo.X, tileInfo.Y);
                    bool inPoland = GeoHelperEPSG2180.IsWithinPolandBounds(lat, lon);
                    if (inPoland)
                    {
                        radioButtonEpsg2180.Enabled = true;
                        radioButtonEpsg3857.Enabled = true;
                    }
                    else
                    {
                        radioButtonEpsg2180.Enabled = false;
                        radioButtonEpsg3857.Enabled = true;
                        radioButtonEpsg3857.Checked = true;
                    }
                }
            }
        }

        /// <summary>
        /// Handles the batch processing of basemaps. Validates input, iterates over existing
        /// tiles in the source group, downloads new imagery, and creates new Trainz assets.
        /// </summary>
        private async void buttonConfAndDownload_Click(object sender, EventArgs e)
        {
            // 1. Input Validation
            if (basemapFolderListBox.SelectedItem == null)
            {
                MessageBox.Show("Wybierz źródłową grupę podkładów!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxDestinationFolder.Text))
            {
                MessageBox.Show("Wpisz nazwę folderu docelowego!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxDesignation.Text))
            {
                MessageBox.Show("Wpisz nazwę oznaczenia podkładów!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // 2. Retrieve settings from the UI
            if (basemapFolderListBox.SelectedItem is not string sourceGroup) return;
            if (comboBoxMapType.SelectedItem is not IMapSource selectedMap) return;

            string targetGroup = textBoxDestinationFolder.Text;
            string targetDesignation = textBoxDesignation.Text;
            string year = selectedMap.SupportsTime ? textBoxBasemapDate.Text : "";
            int res = GetSelectedResolution();

            if (sourceGroup == targetGroup)
            {
                MessageBox.Show("Nazwa docelowego folderu musi być inna niż nazwa folderu źródłowego!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existingGroups = _fileManager.GetBasemapGroups();
            if (existingGroups.Contains(targetGroup))
            {
                var dialogResult = MessageBox.Show(
                    $"Folder docelowy \"{targetGroup}\" już istnieje. Czy na pewno chcesz go usunąć i nadpisać wszystkie znajdujące się w nim podkłady?",
                    "Ostrzeżenie o nadpisaniu",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning);

                if (dialogResult != DialogResult.Yes)
                {
                    return;
                }
            }

            // Lock UI during asynchronous processing
            UiEnabled(false);

            try
            {
                // Clean up the target directory if it already exists
                _fileManager.DeleteGroupFolder(targetGroup);

                var folders = _fileManager.GetKuidsInGroup(sourceGroup);
                int total = folders.Count;
                int current = 0;
                int successCount = 0;
                List<string> failureDetails = new List<string>();

                // Initialize progress bar
                progressBar1.Minimum = 0;
                progressBar1.Maximum = total;
                progressBar1.Value = 0;

                labelProgress.Text = $"Przetworzono: {current} z {total}";
                labelProgress.Refresh();
                labelProgress.Visible = true;

                // Process each tile folder found in the source group
                foreach (var folder in folders)
                {
                    current++;

                    if (TrainzFileManager.TryParseTileFolderName(folder, out var tileInfo))
                    {
                        try
                        {
                            long targetX = tileInfo.X;
                            long targetY = tileInfo.Y;

                            bool srcIs2180 = GeoHelperEPSG2180.IsWithin2180Bounds(tileInfo.X, tileInfo.Y);

                            if (radioButtonEpsg2180.Checked)
                            {
                                if (!srcIs2180)
                                {
                                    var (lat, lon) = GeoHelperEPSG3857.Meters3857ToLatLon(tileInfo.X, tileInfo.Y);
                                    if (GeoHelperEPSG2180.IsWithinPolandBounds(lat, lon))
                                    {
                                        var (tx, ty) = GeoHelperEPSG2180.LatLonToMeters2180(lat, lon);
                                        targetX = (long)Math.Round(tx);
                                        targetY = (long)Math.Round(ty);
                                    }
                                }
                            }
                            else if (radioButtonEpsg3857.Checked)
                            {
                                if (srcIs2180)
                                {
                                    var (lat, lon) = GeoHelperEPSG2180.Meters2180ToLatLon(tileInfo.X, tileInfo.Y);
                                    var (tx, ty) = GeoHelperEPSG3857.LatLonToMeters3857(lat, lon);
                                    targetX = (long)Math.Round(tx);
                                    targetY = (long)Math.Round(ty);
                                }
                            }

                            // Download the new map image based on selected parameters
                            byte[] imageBytes = await selectedMap.GetMapImageAsync(year, targetX, targetY, res);

                            // Generate new Trainz files in the target group folder
                            bool created = _fileManager.CreateTrainzFiles(
                                imageBytes,
                                targetGroup,
                                targetX, targetY,
                                targetDesignation,
                                tileInfo.Counter,
                                tileInfo.KuidPart1,
                                tileInfo.KuidPart2
                            );

                            if (created)
                            {
                                successCount++;
                            }
                            else
                            {
                                failureDetails.Add($"Kafel {folder}: Podkład o współrzędnych {targetX}, {targetY} już istnieje.");
                            }
                        }
                        catch (Exception ex)
                        {
                            failureDetails.Add($"Kafel {folder}: {ex.Message}");
                            Debug.WriteLine($"Error processing tile {folder}: {ex.Message}");
                        }
                    }
                    else
                    {
                        failureDetails.Add($"Kafel {folder}: Nieprawidłowy format nazwy folderu podkładu.");
                    }

                    // Update UI progress indicators
                    progressBar1.Value = current;
                    labelProgress.Text = $"Przetworzono: {current} z {total}";
                }

                if (failureDetails.Count == 0)
                {
                    MessageBox.Show($"Przetwarzanie seryjne zakończone pomyślnie!\n\nPomyślnie utworzono podkładów: {successCount} z {total}.", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (successCount > 0)
                {
                    string errorsPreview = string.Join("\n", failureDetails.Take(5));
                    if (failureDetails.Count > 5) errorsPreview += $"\n... i {failureDetails.Count - 5} innych błędów.";

                    MessageBox.Show($"Przetwarzanie seryjne zakończone z ostrzeżeniami.\n\nUtworzono podkładów: {successCount} z {total}.\nNiepowodzenia ({failureDetails.Count}):\n{errorsPreview}", "Ostrzeżenie", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string errorsPreview = string.Join("\n", failureDetails.Take(5));
                    if (failureDetails.Count > 5) errorsPreview += $"\n... i {failureDetails.Count - 5} innych błędów.";

                    MessageBox.Show($"Przetwarzanie seryjne nie powiodło się dla żadnego podkładu (0 z {total}).\n\nSzczegóły błędów:\n{errorsPreview}", "Błąd przetwarzania", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd krytyczny: " + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                // Restore UI interactivity and refresh the folder list to show the newly created group
                UiEnabled(true);
                BasemapFolderListBoxRefresh();
            }
        }

        /// <summary>
        /// Helper method to determine the requested image resolution from the radio buttons.
        /// </summary>
        private int GetSelectedResolution()
        {
            if (radioButton4096.Checked) return 4096;
            if (radioButton1024.Checked) return 1024;
            if (radioButton512.Checked) return 512;
            return 2048; // Default fallback resolution
        }

        /// <summary>
        /// Toggles the interactive state of UI elements and changes the cursor.
        /// </summary>
        private void UiEnabled(bool enabled)
        {
            Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
            buttonConfAndDownload.Enabled = enabled;
            basemapFolderListBox.Enabled = enabled;
            groupBox1.Enabled = enabled;
            groupBox2.Enabled = enabled;
            groupBox3.Enabled = enabled;
        }

        /// <summary>
        /// Adjusts available UI options dynamically based on the capabilities of the selected map source.
        /// </summary>
        private void comboBoxMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMapType.SelectedItem is IMapSource selected)
            {
                // Enable or disable the year input based on whether the source supports historical data
                textBoxBasemapDate.Enabled = selected.SupportsTime;
                label14.Enabled = selected.SupportsTime;

                // 4096px resolution is enabled dynamically based on provider capabilities
                radioButton4096.Enabled = selected.AllowsHighResolution;

                // Fallback to 2048px if 4096px was selected but is no longer supported
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

                // Subtle soft pastel yellow highlight for OpenStreetMap and OpenRailwayMap (XYZ tile sources)
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

        /// <summary>
        /// Validates key presses to ensure only numeric digits and control keys are entered.
        /// Shows a balloon tooltip if an invalid character is pressed.
        /// </summary>
        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;

                if (sender is TextBox textBox)
                {
                    warningToolTip.Hide(textBox);
                    warningToolTip.Show("Tutaj możesz wpisać tylko cyfry!", textBox, 50, -75, 2000);
                }
            }
        }
    }
}
