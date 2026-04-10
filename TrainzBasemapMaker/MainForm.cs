using System.Diagnostics;
using System.Globalization;

namespace TrainzBasemapMaker
{
    public partial class MainForm : Form
    {
        // --------------------------
        //  Declaration of variables
        // --------------------------

        // variables for storing the current top-left position in the EPSG:2180 standard
        private long currentX; // variables to store the current position of X (lat)
        private long currentY; // variables to store the current position of Y (lon)

        private const long TileSize = 500; // a constant defining the size of the downloaded area

        private int resolution = 2048; // a variable that stores the selected resolution (default 2048px at the start)

        private int counter = 1; // a variable storing the current basemap number (default 1 to start)

        string basemapGroup = "Podk³ady"; // a variable storing the name of the group (folder) to which we are currently saving the backgrounds (by default, "Podk³ady")

        string basemapGroupDesignation = "P";

        private TrainzFileManager _fileManager = new TrainzFileManager();

        private static readonly HttpClient _httpClient = new HttpClient();

        // Initializes a private ToolTip instance with balloon styling and a custom title for warnings.
        private ToolTip warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "B³¹d wprowadzania" };


        // --------------------------
        //  Form initialization
        // --------------------------

        public MainForm()
        {
            InitializeComponent();

            // setting sample default values
            textBoxLat.Text = "50.6633297"; // lat
            textBoxLon.Text = "17.923025";  // lon

            // default settings
            radioButton2048.Checked = true;
            checkBoxCreateFiles.Checked = true;
            textBoxDestinationFolder.Text = basemapGroup;
            textBoxDesignation.Text = basemapGroupDesignation;
            textBoxCounter.Text = counter.ToString();
            textBoxKuidPart1.Text = "0";
            labelKuidPart2.Text = $": {counter}";

            // refreshing lists
            KuidsInFolderListBoxRefresh();
            BasemapFolderListBoxRefresh();
        }


        // --------------------------
        //  Auxiliary methods
        // --------------------------

        private void DataRefresh() // refreshing the displayed data
        {
            textBoxX.Text = currentX.ToString();
            textBoxY.Text = currentY.ToString();

            textBoxCounter.Text = counter.ToString();
            labelKuidPart2.Text = $": {counter}";

            //var (lat, lon) = GeoHelperEPSG2180.Meters2180ToLatLon(currentX, currentY);
            //label1.Text = $"Lat: {lat:F6}, Lon: {lon:F6}";
        }

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
                MessageBox.Show("B³¹d odœwie¿ania listy podk³adów:\n\n" + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BasemapFolderListBoxRefresh()
        {
            basemapFolderListBox.Items.Clear();
            var groups = _fileManager.GetBasemapGroups();
            basemapFolderListBox.Items.AddRange(groups.ToArray());
        }

        private async Task DownloadMap() // downloading satellite basemaps
        {
            UiEnabled(false);

            double xLeft = currentX;
            double yTop = currentY;
            double xRight = xLeft + TileSize;
            double yBottom = yTop - TileSize;

            string url = "https://mapy.geoportal.gov.pl/wss/service/PZGIK/ORTO/WMS/StandardResolution?" +
                         "SERVICE=WMS&VERSION=1.1.1&REQUEST=GetMap" +
                         "&LAYERS=Raster" +
                         "&SRS=EPSG:2180" +
                         $"&BBOX={xLeft.ToString(CultureInfo.InvariantCulture)}," +
                         $"{yBottom.ToString(CultureInfo.InvariantCulture)}," +
                         $"{xRight.ToString(CultureInfo.InvariantCulture)}," +
                         $"{yTop.ToString(CultureInfo.InvariantCulture)}" +
                         $"&WIDTH={resolution.ToString()}&HEIGHT={resolution.ToString()}" +
                         "&FORMAT=image/jpeg" +
                         "&STYLES=";

            try
            {
                toolStripStatusLabel1.Text = $"Pobieranie podk³adu...";

                HttpResponseMessage response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();

                // downloading the image to memory
                byte[] imageBytes = await response.Content.ReadAsByteArrayAsync();

                // preview image
                using (var ms = new MemoryStream(imageBytes))
                {
                    var oldImage = pictureBox1.Image;
                    pictureBox1.Image = Image.FromStream(ms);
                    oldImage?.Dispose(); // deleting the previous image from memory
                }

                toolStripStatusLabel1.Text = $"Pobrano podk³ad w pamiêci: {currentX}_{currentY}";

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
                            textBoxKuidPart1.Text
                        );

                        if (success)
                        {
                            counter++;
                            toolStripStatusLabel1.Text = "Zapisano pomyœlnie!";
                        }
                        else
                        {
                            toolStripStatusLabel1.Text = "Podk³ad ju¿ istnieje – pominiêto.";
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("B³¹d zapisu plików:\n\n" + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                        toolStripStatusLabel1.Text = "B³¹d zapisu plików!";
                    }

                    KuidsInFolderListBoxRefresh();
                    BasemapFolderListBoxRefresh();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("B³¹d pobierania mapy:\n\n" + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                toolStripStatusLabel1.Text = $"Nieudana próba pobrania podk³adu: {currentX}_{currentY}";
            }
            finally
            {
                UiEnabled(true);
            }
        }

        private void UiEnabled(bool enabled)
        {
            if (enabled)
            {
                Cursor = Cursors.Default;
                groupBox1Converter.Enabled = true;
                groupBox2Configurator.Enabled = true;
                groupBox3Navigator.Enabled = true;
                groupBox4KuidList.Enabled = true;
            }
            else
            {
                Cursor = Cursors.WaitCursor;
                groupBox1Converter.Enabled = false;
                groupBox2Configurator.Enabled = false;
                groupBox3Navigator.Enabled = false;
                groupBox4KuidList.Enabled = false;
            }
        }

        // --------------------------
        //  Events
        // --------------------------

        private void buttonConvert_Click(object sender, EventArgs e)
        {
            string latText = textBoxLat.Text.Replace(',', '.');
            string lonText = textBoxLon.Text.Replace(',', '.');

            if (double.TryParse(latText, NumberStyles.Any, CultureInfo.InvariantCulture, out double lat) &&
                double.TryParse(lonText, NumberStyles.Any, CultureInfo.InvariantCulture, out double lon))
            {
                var (x, y) = GeoHelperEPSG2180.LatLonToMeters2180(lat, lon);
                currentX = (long)Math.Round(x);
                currentY = (long)Math.Round(y);
                DataRefresh();
            }
            else
            {
                MessageBox.Show("Wprowadzono nieprawid³owe wspó³rzêdne geograficzne. Upewnij siê, ¿e u¿ywasz tylko cyfr i kropek.", "B³¹d formatu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void radioButtons_CheckedChanged(object sender, EventArgs e)
        {
            if (radioButton4096.Checked)
            {
                resolution = 4096;
            }
            else if (radioButton2048.Checked)
            {
                resolution = 2048;
            }
            else if (radioButton1024.Checked)
            {
                resolution = 1024;
            }
            else if (radioButton512.Checked)
            {
                resolution = 512;
            }
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
                MessageBox.Show("Wprowadzono nieprawid³owe wspó³rzêdne geograficzne (EPSG2180). Upewnij siê, ¿e u¿ywasz tylko cyfr i kropek.\n\n" + ex.Message, "B³¹d formatu", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                toolStripStatusLabel1.Text = $"B³¹d danych startowych (b³¹d konfiguracji)";
            }
        }

        private async void buttonRight_Click(object sender, EventArgs e)
        {
            currentX += TileSize;
            DataRefresh();
            await DownloadMap();
        }

        private async void buttonLeft_Click(object sender, EventArgs e)
        {
            currentX -= TileSize;
            DataRefresh();
            await DownloadMap();
        }

        private async void buttonUp_Click(object sender, EventArgs e)
        {
            currentY += TileSize;
            DataRefresh();
            await DownloadMap();
        }

        private async void buttonDown_Click(object sender, EventArgs e)
        {
            currentY -= TileSize;
            DataRefresh();
            await DownloadMap();
        }

        private void kuidsInFolderListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (kuidsInFolderListBox.SelectedItem != null)
            {
                string zaznaczonyElement = kuidsInFolderListBox.SelectedItem.ToString();
                OpenImageFromFile(zaznaczonyElement);
            }
        }

        private void kuidsInFolderListBox_DoubleClick(object sender, EventArgs e)
        {
            try
            {
                if (kuidsInFolderListBox.SelectedItem != null)
                {
                    string zaznaczonyElement = kuidsInFolderListBox.SelectedItem.ToString();

                    string[] parts = zaznaczonyElement.Split('_');

                    if (parts.Length >= 4)
                    {
                        int clickedCounter = int.Parse(parts[2]);
                        counter = clickedCounter + 1;

                        currentX = long.Parse(parts[3]);
                        currentY = long.Parse(parts[4]);

                        DataRefresh();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("B³¹d nazwy podk³adu:\n\n" + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void basemapFolderListBox_Click(object sender, EventArgs e)
        {
            if (basemapFolderListBox.SelectedItem != null)
            {
                string zaznaczonyElement = basemapFolderListBox.SelectedItem.ToString();

                textBoxDestinationFolder.Text = zaznaczonyElement;
            }
        }

        private void stronaProgramuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("*") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("B³¹d otwarcia strony:\n\n" + ex.Message, "B³¹d", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void informacjeOProgramieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutProgramForm info = new AboutProgramForm())
            {
                info.ShowDialog();
            }
        }

        private void checkBoxCreateFiles_CheckedChanged(object sender, EventArgs e)
        {
            BasemapFolderListBoxRefresh();
            if (checkBoxCreateFiles.Checked)
            {
                panel1.Enabled = true;
            }
            else
            {
                panel1.Enabled = false;
            }
        }

        private void textBoxDestinationFolder_TextChanged(object sender, EventArgs e)
        {
            basemapGroup = textBoxDestinationFolder.Text;
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

        private void OnlyNumbers_KeyPress(object sender, KeyPressEventArgs e)
        {
            // if the character is not a number or control key (e.g. Backspace)
            if (!char.IsControl(e.KeyChar) && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;

                if (sender is TextBox textBox)
                {
                    // showing information in balloon (cloud) to the user
                    warningToolTip.Hide(textBox);
                    warningToolTip.Show("Tutaj mo¿esz wpisaæ tylko cyfry!", textBox, 50, -75, 2000);
                }
            }
        }
    }
}