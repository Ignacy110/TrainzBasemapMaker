
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
        private ToolTip warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "Input Error" };

        public BatchToolForm()
        {
            InitializeComponent();

            // Set default UI states
            radioButton2048.Checked = true;
            textBoxBasemapDate.Text = DateTime.Now.Year.ToString();

            // Bind available map sources to the dropdown list
            comboBoxMapType.DataSource = WmsSource.availableMaps;
            comboBoxMapType.DisplayMember = "Name";

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
            string sourceGroup = basemapFolderListBox.SelectedItem.ToString();
            string targetGroup = textBoxDestinationFolder.Text;
            string targetDesignation = textBoxDesignation.Text;
            string year = textBoxBasemapDate.Text;
            WmsSource selectedMap = (WmsSource)comboBoxMapType.SelectedItem;
            int res = GetSelectedResolution();

            if (sourceGroup == targetGroup)
            {
                MessageBox.Show("Nazwa docelowego folderu musi być inna nić nazwa folderu źródłowego!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
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

                    // Extract parameters encoded in the folder name (e.g., coordinates, original KUID)
                    string[] parts = folder.Split('_');

                    if (parts.Length >= 7)
                    {
                        try
                        {
                            // Parse data from the old folder name
                            int counter = int.Parse(parts[2]);
                            long x = long.Parse(parts[3]);
                            long y = long.Parse(parts[4]);
                            string kuid1 = parts[5]; // Retain original KUID part 1
                            string kuid2 = parts[6]; // Retain original KUID part 2

                            // Download the new map image based on selected parameters
                            byte[] imageBytes = await selectedMap.GetMapImageAsync(year, x, y, res);

                            // Generate new Trainz files in the target group folder
                            _fileManager.CreateTrainzFiles(
                                imageBytes,
                                targetGroup,
                                x, y,
                                targetDesignation,
                                counter,
                                kuid1,
                                kuid2
                            );
                        }
                        catch (Exception ex)
                        {
                            Debug.WriteLine($"Error processing tile {folder}: {ex.Message}");
                        }
                    }

                    // Update UI progress indicators
                    progressBar1.Value = current;
                    labelProgress.Text = $"Przetworzono: {current} z {total}";
                }

                MessageBox.Show("Przetwarzanie seryjne zakończone pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
        }

        /// <summary>
        /// Adjusts available UI options dynamically based on the capabilities of the selected map source.
        /// </summary>
        private void comboBoxMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMapType.SelectedItem is WmsSource selected)
            {
                // Enable or disable the year input based on whether the source supports historical data
                textBoxBasemapDate.Enabled = selected.SupportsTime;
                label14.Enabled = selected.SupportsTime;

                bool isOrto = selected.Name.Contains("Ortofotomapa");

                // 4096px resolution is restricted to orthophotomaps
                radioButton4096.Enabled = isOrto;

                // Fallback to 2048px if 4096px was selected but is no longer supported
                if (!isOrto && radioButton4096.Checked)
                {
                    radioButton2048.Checked = true;
                }
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
