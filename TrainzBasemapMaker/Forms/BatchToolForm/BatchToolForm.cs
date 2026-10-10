
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
    public partial class BatchToolForm : Form, Classes.ILocalizableForm
    {
        // Handles file and directory operations for Trainz assets
        private TrainzFileManager _fileManager = new TrainzFileManager();

        // ToolTip used to provide visual feedback for input validation errors
        private ToolTip warningToolTip = new ToolTip { IsBalloon = true };

        public BatchToolForm()
        {
            InitializeComponent();

            // Set default UI states
            comboBoxEpsg.SelectedIndex = 0;
            textBoxBasemapDate.Text = DateTime.Now.Year.ToString();

            // Bind available map sources to the dropdown list
            comboBoxMapType.DataSource = MapSources.AvailableMaps;
            comboBoxMapType.DisplayMember = "DisplayName";
            comboBoxMapType.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxMapType.DrawItem += ComboBoxMapType_DrawItem;

            comboBoxMapType_SelectedIndexChanged(comboBoxMapType, EventArgs.Empty);
            BasemapFolderListBoxRefresh();
            
            ApplyLocalization();

            TrainzBasemapMaker.Classes.ThemeManager.ApplyTheme(this);
        }

        public void ApplyLocalization()
        {
            this.Text = Localization.Strings.Batch_Title;
            label1.Text = Localization.Strings.Batch_Header;
            groupBox1.Text = Localization.Strings.Batch_Group1;
            label10.Text = Localization.Strings.Batch_YourFolders;
            groupBox3.Text = Localization.Strings.Batch_Group2;
            groupBox2.Text = Localization.Strings.Batch_Group3;
            label15.Text = Localization.Strings.Batch_MapType;
            label14.Text = Localization.Strings.Batch_MapYear;
            label2.Text = Localization.Strings.Batch_Resolution;
            label13.Text = Localization.Strings.Batch_BasemapDesignation;
            label4.Text = Localization.Strings.Batch_DestFolder;
            buttonConfAndDownload.Text = Localization.Strings.Batch_ButtonProcess;
            labelProgress.Text = Localization.Strings.Batch_Processed;
            warningToolTip.ToolTipTitle = Localization.Strings.Common_InputError;

            int currentEpsgIdx = comboBoxEpsg.SelectedIndex;
            comboBoxEpsg.Items.Clear();
            comboBoxEpsg.Items.AddRange(new object[] { Localization.Strings.Common_Epsg2180, Localization.Strings.Common_Epsg3857 });
            comboBoxEpsg.SelectedIndex = currentEpsgIdx >= 0 ? currentEpsgIdx : 0;

            if (comboBoxMapType.DataSource != null)
            {
                var selMap = comboBoxMapType.SelectedItem;
                comboBoxMapType.SelectedIndexChanged -= comboBoxMapType_SelectedIndexChanged;
                comboBoxMapType.DataSource = null;
                comboBoxMapType.DataSource = MapSources.AvailableMaps;
                comboBoxMapType.DisplayMember = "DisplayName";
                if (selMap != null) comboBoxMapType.SelectedItem = selMap;
                comboBoxMapType.SelectedIndexChanged += comboBoxMapType_SelectedIndexChanged;
            }
            comboBoxMapType.Invalidate();
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

            var groupInfo = _fileManager.GetGroupInfo(selectedGroup);
            if (groupInfo != null)
            {
                if (!string.IsNullOrWhiteSpace(groupInfo.Designation))
                {
                    textBoxDesignation.Text = groupInfo.Designation;
                }

                if (groupInfo.Epsg == "EPSG:2180")
                {
                    // comboBoxEpsg.Enabled = true;
                    // comboBoxEpsg.Enabled = true;
                    comboBoxEpsg.SelectedIndex = 0;
                }
                else
                {
                    long ax = groupInfo.AnchorX ?? 0;
                    long ay = groupInfo.AnchorY ?? 0;
                    var (lat, lon) = GeoHelperEPSG3857.Meters3857ToLatLon(ax, ay);
                    bool inPoland = GeoHelperEPSG2180.IsWithinPolandBounds(lat, lon);
                    // comboBoxEpsg.Enabled = inPoland;
                    // comboBoxEpsg.Enabled = true;
                    comboBoxEpsg.SelectedIndex = 1;
                }
                return;
            }

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
                    // comboBoxEpsg.Enabled = true;
                    // comboBoxEpsg.Enabled = true;
                    comboBoxEpsg.SelectedIndex = 0;
                }
                else
                {
                    var (lat, lon) = GeoHelperEPSG3857.Meters3857ToLatLon(tileInfo.X, tileInfo.Y);
                    bool inPoland = GeoHelperEPSG2180.IsWithinPolandBounds(lat, lon);
                    if (inPoland)
                    {
                        // comboBoxEpsg.Enabled = true;
                        // comboBoxEpsg.Enabled = true;
                    }
                    else
                    {
                        // comboBoxEpsg.Enabled = false;
                        // comboBoxEpsg.Enabled = true;
                        comboBoxEpsg.SelectedIndex = 1;
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
                MessageBox.Show(Localization.Strings.Batch_Err_SelectSource, Localization.Strings.Common_Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxDestinationFolder.Text))
            {
                MessageBox.Show(Localization.Strings.Batch_Err_EnterDestFolder, Localization.Strings.Common_Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(textBoxDesignation.Text))
            {
                MessageBox.Show(Localization.Strings.Batch_Err_EnterDesignation, Localization.Strings.Common_Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                MessageBox.Show(Localization.Strings.Batch_Err_DestSameAsSource, Localization.Strings.Common_Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var existingGroups = _fileManager.GetBasemapGroups();
            if (existingGroups.Contains(targetGroup))
            {
                var dialogResult = MessageBox.Show(
                    string.Format(Localization.Strings.Batch_Confirm_Overwrite, targetGroup),
                    Localization.Strings.Common_Warning,
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

                labelProgress.Text = string.Format(Localization.Strings.Batch_ProgressFormat, current, total);
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

                            if ((comboBoxEpsg.SelectedIndex == 0))
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
                            else if ((comboBoxEpsg.SelectedIndex == 1))
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
                            bool created = await Task.Run(() => _fileManager.CreateTrainzFiles(
                                imageBytes,
                                targetGroup,
                                targetX, targetY,
                                targetDesignation,
                                tileInfo.Counter,
                                tileInfo.KuidPart1,
                                tileInfo.KuidPart2,
                                force2D: true
                            ));

                            if (created)
                            {
                                successCount++;
                            }
                            else
                            {
                                failureDetails.Add(string.Format(Localization.Strings.Batch_Err_TileAlreadyExists, folder, targetX, targetY));
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
                        failureDetails.Add(string.Format(Localization.Strings.Batch_Err_InvalidFolderFormat, folder));
                    }

                    // Update UI progress indicators
                    progressBar1.Value = current;
                    labelProgress.Text = string.Format(Localization.Strings.Batch_ProgressFormat, current, total);
                }

                if (successCount > 0)
                {
                    try
                    {
                        var srcInfo = _fileManager.GetGroupInfo(sourceGroup);
                        long? anchorX = srcInfo?.AnchorX;
                        long? anchorY = srcInfo?.AnchorY;
                        double? anchorCosLat = srcInfo?.AnchorCosLat;

                        // Fallback anchor if src didn't have one
                        if (anchorX == null && folders.Count > 0 && TrainzFileManager.TryParseTileFolderName(folders[0], out var firstTile))
                        {
                            bool srcIs2180 = GeoHelperEPSG2180.IsWithin2180Bounds(firstTile.X, firstTile.Y);
                            if ((comboBoxEpsg.SelectedIndex == 0))
                            {
                                if (srcIs2180)
                                {
                                    anchorX = firstTile.X;
                                    anchorY = firstTile.Y;
                                }
                                else
                                {
                                    var (lat, lon) = GeoHelperEPSG3857.Meters3857ToLatLon(firstTile.X, firstTile.Y);
                                    var (tx, ty) = GeoHelperEPSG2180.LatLonToMeters2180(lat, lon);
                                    anchorX = (long)Math.Round(tx);
                                    anchorY = (long)Math.Round(ty);
                                }
                            }
                            else
                            {
                                if (srcIs2180)
                                {
                                    var (lat, lon) = GeoHelperEPSG2180.Meters2180ToLatLon(firstTile.X, firstTile.Y);
                                    var (tx, ty) = GeoHelperEPSG3857.LatLonToMeters3857(lat, lon);
                                    anchorX = (long)Math.Round(tx);
                                    anchorY = (long)Math.Round(ty);
                                }
                                else
                                {
                                    anchorX = firstTile.X;
                                    anchorY = firstTile.Y;
                                }
                            }
                        }

                        if ((comboBoxEpsg.SelectedIndex == 1) && anchorX.HasValue && anchorY.HasValue && anchorCosLat == null)
                        {
                            var (lat, _) = GeoHelperEPSG3857.Meters3857ToLatLon(anchorX.Value, anchorY.Value);
                            anchorCosLat = Math.Cos(lat * Math.PI / 180.0);
                        }

                        var targetInfo = new BasemapGroupInfo
                        {
                            GroupName = targetGroup,
                            Designation = targetDesignation,
                            Epsg = (comboBoxEpsg.SelectedIndex == 0) ? "EPSG:2180" : "EPSG:3857",
                            AnchorX = anchorX,
                            AnchorY = anchorY,
                            AnchorCosLat = anchorCosLat,
                            MapSource = selectedMap.Name,
                            Resolution = res,
                            Year = selectedMap.SupportsTime ? year : null,
                            CreatedAt = DateTime.Now,
                            LastUpdatedAt = DateTime.Now
                        };
                        _fileManager.SaveGroupInfo(targetGroup, targetInfo);
                    }
                    catch (Exception ex)
                    {
                        Debug.WriteLine("Error saving batch group metadata: " + ex.Message);
                    }
                }

                if (failureDetails.Count == 0)
                {
                    MessageBox.Show(string.Format(Localization.Strings.Batch_Success, successCount, total), Localization.Strings.Common_Success, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else if (successCount > 0)
                {
                    string errorsPreview = string.Join("\n", failureDetails.Take(5));
                    if (failureDetails.Count > 5) errorsPreview += $"\n... ({failureDetails.Count - 5})";

                    MessageBox.Show(string.Format(Localization.Strings.Batch_Warnings, successCount, total, failureDetails.Count, errorsPreview), Localization.Strings.Common_Warning, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    string errorsPreview = string.Join("\n", failureDetails.Take(5));
                    if (failureDetails.Count > 5) errorsPreview += $"\n... ({failureDetails.Count - 5})";

                    MessageBox.Show(string.Format(Localization.Strings.Batch_Failed, total, errorsPreview), Localization.Strings.Batch_ProcessingError, MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Localization.Strings.Batch_CriticalError, ex.Message), Localization.Strings.Common_Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
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
                private void comboBoxResolution_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Designer requirement
        }

        private int GetSelectedResolution()
        {
            return (comboBoxResolution.SelectedItem as ResolutionOption)?.Value ?? 2048;
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
        private void comboBoxMapType_SelectedIndexChanged(object? sender, EventArgs e)
        {
            if (comboBoxMapType.SelectedItem is IMapSource selected)
            {
                // Enable or disable the year input based on whether the source supports historical data
                textBoxBasemapDate.Enabled = selected.SupportsTime;

                FormHelpers.UpdateResolutionComboBox(comboBoxResolution, selected);
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

                // Subtle soft pastel yellow highlight for OpenStreetMap and OpenRailwayMap (XYZ tile sources)
                // Use darker yellow in dark mode to keep text visible
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
                    e.Graphics.DrawString(mapSource.DisplayName, e.Font ?? comboBox.Font, textBrush, e.Bounds, sf);
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
                    warningToolTip.Show(Localization.Strings.Common_OnlyDigitsTooltip, textBox, 50, -75, 2000);
                }
            }
        }
    }
}
