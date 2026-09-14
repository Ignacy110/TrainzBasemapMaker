
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
using System.Globalization;
using TrainzBasemapMaker.Classes;

namespace TrainzBasemapMaker
{
    public partial class MainForm : Form
    {
        // --------------------------
        //  Declaration of variables
        // --------------------------

        // Current top-left position coordinates in the EPSG:2180 projection
        private long currentX;
        private long currentY;

        // Selected map resolution in pixels (default is 2048x2048)
        private int resolution = 2048;

        // Current basemap sequence number
        private int counter = 1;

        // Target directory and designation prefix for generating basemap files
        string basemapGroup = "Podkłady";
        string basemapGroupDesignation = "P";

        private TrainzFileManager _fileManager = new TrainzFileManager();

        // Custom tooltip for displaying input validation warnings
        private ToolTip warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "Błąd wprowadzania" };


        // --------------------------
        //  Form initialization
        // --------------------------

        public MainForm()
        {
            InitializeComponent();

            // Set sample default geographic coordinates
            textBoxLat.Text = "50.6633297";
            textBoxLon.Text = "17.923025";

            // Apply default UI settings
            radioButton2048.Checked = true;
            checkBoxCreateFiles.Checked = true;
            textBoxDestinationFolder.Text = basemapGroup;
            textBoxDesignation.Text = basemapGroupDesignation;
            textBoxCounter.Text = counter.ToString();
            textBoxKuidPart1.Text = Properties.Settings.Default.DefaultKuidFirstPart;
            textBoxBasemapDate.Text = DateTime.Now.Year.ToString();

            // Bind available map providers to the dropdown list
            comboBoxMapType.DataSource = MapSources.AvailableMaps;
            comboBoxMapType.DisplayMember = "Name";
            comboBoxMapType.DrawMode = DrawMode.OwnerDrawFixed;
            comboBoxMapType.DrawItem += ComboBoxMapType_DrawItem;

            // Configure coordinate system radio buttons
            radioButtonEpsg2180.Checked = true;
            radioButtonEpsg2180.CheckedChanged += RadioButtonEpsg_CheckedChanged;
            radioButtonEpsg3857.CheckedChanged += RadioButtonEpsg_CheckedChanged;
            label7.Text = radioButtonEpsg2180.Checked ? "Współrzędne EPSG:2180" : "Współrzędne EPSG:3857";
            textBoxLat.TextChanged += TextBoxLatLon_TextChanged;
            textBoxLon.TextChanged += TextBoxLatLon_TextChanged;

            // Initialize dynamic data and lists
            UpdateNextFreeKuidPart2();
            KuidsInFolderListBoxRefresh();
            BasemapFolderListBoxRefresh();
        }


        // --------------------------
        //  Auxiliary methods
        // --------------------------

        // Updates the UI text boxes with the current internal values
        private void DataRefresh()
        {
            textBoxX.Text = currentX.ToString();
            textBoxY.Text = currentY.ToString();
            textBoxCounter.Text = counter.ToString();
        }

        // Loads and displays a previously saved basemap image in the picture box
        private void OpenImageFromFile(string folderName)
        {
            string fullPath = _fileManager.GetImagePath(basemapGroup, folderName);

            if (File.Exists(fullPath))
                pictureBox1.ImageLocation = fullPath;
            else
                pictureBox1.Image = null;
        }

        private void KuidsInFolderListBoxRefresh()
        {
            try
            {
                kuidsInFolderListBox.Items.Clear();
                var kuids = _fileManager.GetKuidsInGroup(basemapGroup);
                kuidsInFolderListBox.Items.AddRange(kuids.ToArray());
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd odświeżania listy podkładów:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BasemapFolderListBoxRefresh()
        {
            basemapFolderListBox.Items.Clear();
            var groups = _fileManager.GetBasemapGroups();
            basemapFolderListBox.Items.AddRange(groups.ToArray());
        }

        // Automatically finds and sets the next available KUID part 2 to avoid overwriting existing assets
        private void UpdateNextFreeKuidPart2()
        {
            try
            {
                int nextKuid = _fileManager.GetNextFreeKuidPart2();
                textBoxKuidPart2.Text = nextKuid.ToString();
            }
            catch (Exception ex)
            {
                textBoxKuidPart2.Text = "1";
                toolStripStatusLabel1.Text = "Błąd automatycznego wyznaczania oznaczenia kuidu (część 2)";
                MessageBox.Show("Błąd automatycznego wyznaczania oznaczenia kuidu (część 2):\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Automatically finds and sets the next available sequence number for the current basemap group
        private void UpdateNextFreeCounter()
        {
            try
            {
                string currentGroup = basemapGroup;
                int nextCounter = _fileManager.GetNextFreeCounter(currentGroup);
                textBoxCounter.Text = nextCounter.ToString();
            }
            catch (Exception ex)
            {
                textBoxCounter.Text = "1";
                toolStripStatusLabel1.Text = "Błąd automatycznego wyznaczania numeru podkładu";
                MessageBox.Show("Błąd automatycznego wyznaczania numeru podkładu:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Core method: Downloads the map image from the WMS server and optionally creates Trainz asset files
        private async Task DownloadMap()
        {
            UiEnabled(false);

            try
            {
                toolStripStatusLabel1.Text = $"Pobieranie podkładu...";

                if (comboBoxMapType.SelectedItem is not IMapSource selectedMap) return;
                string year = selectedMap.SupportsTime ? textBoxBasemapDate.Text : "";
                byte[] imageBytes = await selectedMap.GetMapImageAsync(year, currentX, currentY, resolution);

                // Update the preview image and dispose of the old one to prevent memory leaks
                using (var ms = new MemoryStream(imageBytes))
                using (var temp = Image.FromStream(ms))
                {
                    var newImage = new Bitmap(temp);
                    var oldImage = pictureBox1.Image;
                    pictureBox1.Image = newImage;
                    oldImage?.Dispose();
                }

                toolStripStatusLabel1.Text = $"Pobrano podkład do pamięci: {currentX}_{currentY}";

                // Proceed with file creation if the user enabled this option
                if (checkBoxCreateFiles.Checked)
                {
                    try
                    {
                        TrainzFileManager fileManager = new TrainzFileManager();

                        bool success = fileManager.CreateTrainzFiles(
                            imageBytes,
                            basemapGroup,
                            currentX,
                            currentY,
                            basemapGroupDesignation,
                            counter,
                            textBoxKuidPart1.Text,
                            textBoxKuidPart2.Text
                        );

                        if (success)
                        {
                            // Auto-increment counters based on user preferences
                            if (Properties.Settings.Default.AutoCounterNumber)
                            {
                                counter++;
                            }
                            if (Properties.Settings.Default.AutoKuidNumber)
                            {
                                UpdateNextFreeKuidPart2();
                            }
                            DataRefresh();
                            toolStripStatusLabel1.Text = $"Pobrano podkład i utworzono pliki dla Trainz: {currentX}, {currentY}";
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Podkład już istnieje – pominięto.";
                        }
                    }
                    catch (Exception ex)
                    {
                        toolStripStatusLabel1.Text = "Błąd zapisu plików!";
                        MessageBox.Show("Błąd zapisu plików:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    KuidsInFolderListBoxRefresh();
                    BasemapFolderListBoxRefresh();
                }
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = $"Błąd pobierania mapy: {currentX}_{currentY}";
                MessageBox.Show("Błąd pobierania mapy:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            finally
            {
                UiEnabled(true);
            }
        }

        // Toggles UI elements to prevent user interactions during asynchronous background operations
        private void UiEnabled(bool enabled)
        {
            Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
            groupBox1Converter.Enabled = enabled;
            groupBox2Configurator.Enabled = enabled;
            groupBox3Navigator.Enabled = enabled;
            groupBox4KuidList.Enabled = enabled;
        }

        // Updates EPSG radio buttons based on geographic coordinates (in Poland both 2180 and 3857 are allowed; outside Poland only 3857)
        private void UpdateCoordinateSystemAvailability()
        {
            string latText = textBoxLat.Text.Replace(',', '.');
            string lonText = textBoxLon.Text.Replace(',', '.');

            if (double.TryParse(latText, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat) &&
                double.TryParse(lonText, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon))
            {
                bool inPoland = GeoHelperEPSG2180.IsWithinPolandBounds(lat, lon);
                if (inPoland)
                {
                    radioButtonEpsg2180.Enabled = true;
                }
                else
                {
                    radioButtonEpsg2180.Enabled = false;
                    radioButtonEpsg3857.Checked = true;
                }
            }
        }

        private void TextBoxLatLon_TextChanged(object? sender, EventArgs e)
        {
            UpdateCoordinateSystemAvailability();
        }

        private void RadioButtonEpsg_CheckedChanged(object? sender, EventArgs e)
        {
            buttonConvert.Text = radioButtonEpsg2180.Checked ? "Konwertuj na EPSG:2180" : "Konwertuj na EPSG:3857";
            label7.Text = radioButtonEpsg2180.Checked ? "Współrzędne EPSG:2180" : "Współrzędne EPSG:3857";
            PerformConversion();
        }

        // Converts standard geographic coordinates (Lat/Lon) to metric projection (EPSG:2180 or EPSG:3857)
        private void PerformConversion()
        {
            string latText = textBoxLat.Text.Replace(',', '.');
            string lonText = textBoxLon.Text.Replace(',', '.');

            if (double.TryParse(latText, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat) &&
                double.TryParse(lonText, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon))
            {
                bool inPoland = GeoHelperEPSG2180.IsWithinPolandBounds(lat, lon);

                if (!inPoland)
                {
                    radioButtonEpsg2180.Enabled = false;
                    radioButtonEpsg3857.Checked = true;
                }
                else
                {
                    radioButtonEpsg2180.Enabled = true;
                }

                if (radioButtonEpsg2180.Checked && inPoland)
                {
                    var (x, y) = GeoHelperEPSG2180.LatLonToMeters2180(lat, lon);
                    currentX = (long)Math.Round(x);
                    currentY = (long)Math.Round(y);

                    DataRefresh();
                    toolStripStatusLabel1.Text = $"Przekonwertowano: {latText}, {lonText} na EPSG:2180: {currentX}, {currentY}";
                }
                else
                {
                    var (x, y) = GeoHelperEPSG3857.LatLonToMeters3857(lat, lon);
                    currentX = (long)Math.Round(x);
                    currentY = (long)Math.Round(y);

                    DataRefresh();
                    string suffix = inPoland ? " (Polska)" : " (Global)";
                    toolStripStatusLabel1.Text = $"Przekonwertowano: {latText}, {lonText} na EPSG:3857{suffix}: {currentX}, {currentY}";
                }
            }
            else
            {
                toolStripStatusLabel1.Text = $"Błąd konwersji: {latText}, {lonText}";
            }
        }

        // --------------------------
        //  Events
        // --------------------------

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            PerformConversion();
        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4096.Checked) resolution = 4096;
            else if (radioButton2048.Checked) resolution = 2048;
            else if (radioButton1024.Checked) resolution = 1024;
            else if (radioButton512.Checked) resolution = 512;
        }

        private async void buttonConfAndDownload_Click(object sender, EventArgs e)
        {
            try
            {
                currentX = long.Parse(textBoxX.Text, CultureInfo.InvariantCulture);
                currentY = long.Parse(textBoxY.Text, CultureInfo.InvariantCulture);
                await DownloadMap();
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = $"Błąd danych startowych (błąd konfiguracji)";
                MessageBox.Show("Błąd danych startowych (błąd konfiguracji). Upewnij się, że używasz tylko cyfr i ewentualnie kropki.\n\n" + ex.Message, "Błąd formatu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // Navigation controls: Shift the map by exactly one tile size in the specified direction
        private async void buttonRight_Click(object sender, EventArgs e)
        {
            currentX += MapSourceBase.TileSize;
            DataRefresh();
            await DownloadMap();
        }

        private async void buttonLeft_Click(object sender, EventArgs e)
        {
            currentX -= MapSourceBase.TileSize;
            DataRefresh();
            await DownloadMap();
        }

        private async void buttonUp_Click(object sender, EventArgs e)
        {
            currentY += MapSourceBase.TileSize;
            DataRefresh();
            await DownloadMap();
        }

        private async void buttonDown_Click(object sender, EventArgs e)
        {
            currentY -= MapSourceBase.TileSize;
            DataRefresh();
            await DownloadMap();
        }

        private void kuidsInFolderListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kuidsInFolderListBox.SelectedItem is string selectedItem)
            {
                OpenImageFromFile(selectedItem);
            }
        }

        // Parses the folder name of a double-clicked item to extract its coordinates and load it
        private void kuidsInFolderListBox_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (kuidsInFolderListBox.SelectedItem is string selectedItem)
                {
                    if (TrainzFileManager.TryParseTileFolderName(selectedItem, out var tileInfo))
                    {
                        currentX = tileInfo.X;
                        currentY = tileInfo.Y;
                        DataRefresh();

                        if (GeoHelperEPSG2180.IsWithin2180Bounds(currentX, currentY))
                        {
                            var (lat, lon) = GeoHelperEPSG2180.Meters2180ToLatLon(currentX, currentY);
                            textBoxLat.Text = lat.ToString(CultureInfo.InvariantCulture);
                            textBoxLon.Text = lon.ToString(CultureInfo.InvariantCulture);
                            radioButtonEpsg2180.Enabled = true;
                            radioButtonEpsg2180.Checked = true;
                        }
                        else
                        {
                            var (lat, lon) = GeoHelperEPSG3857.Meters3857ToLatLon(currentX, currentY);
                            textBoxLat.Text = lat.ToString(CultureInfo.InvariantCulture);
                            textBoxLon.Text = lon.ToString(CultureInfo.InvariantCulture);
                            bool inPoland = GeoHelperEPSG2180.IsWithinPolandBounds(lat, lon);
                            radioButtonEpsg2180.Enabled = inPoland;
                            radioButtonEpsg3857.Checked = true;
                        }
                    }
                    else
                    {
                        toolStripStatusLabel1.Text = $"Błąd nazwy podkładu";
                        MessageBox.Show("Błąd nazwy podkładu.", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd nazwy podkładu:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void basemapFolderListBox_Click(object sender, EventArgs e)
        {
            if (basemapFolderListBox.SelectedItem is string selectedItem)
            {
                textBoxDestinationFolder.Text = selectedItem;
            }
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://github.com/Ignacy110/TrainzBasemapMaker") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                toolStripStatusLabel1.Text = $"Błąd otwarcia strony internetowej";
                MessageBox.Show("Błąd otwarcia strony:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutProgramForm info = new AboutProgramForm())
            {
                info.ShowDialog();
            }
        }

        private void checkBoxCreateFiles_CheckedChanged(object sender, EventArgs e)
        {
            BasemapFolderListBoxRefresh();
            panel1.Enabled = checkBoxCreateFiles.Checked;
        }

        private void textBoxDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            basemapGroup = textBoxDestinationFolder.Text;
            UpdateNextFreeCounter();
            KuidsInFolderListBoxRefresh();
        }

        private void textBoxDesignation_TextChanged(object sender, EventArgs e)
        {
            basemapGroupDesignation = textBoxDesignation.Text;
        }

        private void textBoxCounter_TextChanged(object sender, EventArgs e)
        {
            if (int.TryParse(textBoxCounter.Text, out int result))
            {
                counter = result;
            }
        }

        // Input validation: Ensures the user can only type digits and control characters (like Backspace)
        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;

                if (sender is TextBox textBox)
                {
                    // Show a balloon tooltip to inform the user about invalid input
                    warningToolTip.Hide(textBox);
                    warningToolTip.Show("Tutaj możesz wpisać tylko cyfry!", textBox, 50, -75, 2000);
                }
            }
        }

        private void findSmallestFreeBasemapNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateNextFreeCounter();
            toolStripStatusLabel1.Text = $"Automatycznie dobrano numer podkładu: {textBoxCounter.Text}";
        }

        private void findFreeKuidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateNextFreeKuidPart2();
            toolStripStatusLabel1.Text = $"Automatycznie dobrano numer kuidu (część 2): {textBoxKuidPart2.Text}";
        }

        private void refreshFolderAndBasemapListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            KuidsInFolderListBoxRefresh();
            BasemapFolderListBoxRefresh();
            toolStripStatusLabel1.Text = $"Odświeżono listę folderów";
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PreferencesForm info = new PreferencesForm())
            {
                info.ShowDialog();
            }
        }

        private void comboBoxMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMapType.SelectedItem is IMapSource selected)
            {
                // Enable or disable the year text box depending on the map provider's capabilities
                textBoxBasemapDate.Enabled = selected.SupportsTime;
                label14.Enabled = selected.SupportsTime;

                // 4096px resolution is enabled dynamically based on provider capabilities
                radioButton4096.Enabled = selected.AllowsHighResolution;

                // Fallback to 2048px if the unsupported 4096px was currently selected
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

        private void batchProcessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (BatchToolForm info = new BatchToolForm())
            {
                info.ShowDialog();
                BasemapFolderListBoxRefresh();
            }
        }

        private void buttonMarkPointMap_Click(object sender, EventArgs e)
        {
            using (var mapPicker = new MapPickerForm())
            {
                // The map picker dialog pauses execution until the user makes a selection
                if (mapPicker.ShowDialog() == DialogResult.OK)
                {
                    // Apply coordinates only if the user confirmed the selection
                    textBoxLat.Text = mapPicker.SelectedLat.ToString(CultureInfo.InvariantCulture);
                    textBoxLon.Text = mapPicker.SelectedLon.ToString(CultureInfo.InvariantCulture);
                    PerformConversion();
                }
            }
        }
    }
}