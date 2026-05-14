
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
//
//
// This form utilizes the Microsoft.Web.WebView2 library
// for embedding web content and rendering the map interface.
// Microsoft.Web.WebView2 is authored by Microsoft Corporation
// and is licensed under the Microsoft Software License.
//
// This form also utilizes the Leaflet.js library (version 1.9.4)
// for interactive map functionality and coordinate retrieval.
// Leaflet.js is authored by Volodymyr Agafonkin and contributors
// and is licensed under the BSD 2-Clause License.
//
// Map data is provided by OpenStreetMap
// and is licensed under the Open Data Commons Open Database License (ODbL).

using Microsoft.Web.WebView2.Core;
using System.Diagnostics;
using System.Globalization;
using System.Text.Json;

namespace TrainzBasemapMaker
{
    public partial class MapPickerForm : Form
    {
        // Public properties to store and retrieve the selected coordinates from MainForm
        public double SelectedLat { get; private set; }
        public double SelectedLon { get; private set; }

        public MapPickerForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Initializes the WebView2 browser, configures the User-Agent, 
        /// and loads the local HTML map file.
        /// </summary>
        public async Task InitBrowser()
        {
            await webView21.EnsureCoreWebView2Async(null);

            // Set a custom User-Agent to comply with OpenStreetMap Tile Usage Policy
            webView21.CoreWebView2.Settings.UserAgent = "TrainzBasemapMaker/v0.5.0-alpha (https://github.com/Ignacy110/TrainzBasemapMaker)";

            // Build the absolute path to the HTML file within the project folder structure
            string indexPath = Path.Combine(Application.StartupPath, "Forms", "MapPickerForm", "Web", "map.html");

            // Navigate to the local file using the file:/// protocol
            webView21.CoreWebView2.Navigate("file:///" + indexPath);

            // Subscribe to the event that receives messages sent from JavaScript
            webView21.CoreWebView2.WebMessageReceived += WebView21_WebMessageReceived;
        }

        /// <summary>
        /// Handles coordinate data sent from the Leaflet map via window.chrome.webview.postMessage.
        /// </summary>
        private void WebView21_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string json = e.WebMessageAsJson;

            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    double lat = doc.RootElement.GetProperty("lat").GetDouble();
                    double lon = doc.RootElement.GetProperty("lon").GetDouble();

                    // Update UI text boxes with coordinates using InvariantCulture (dot separator)
                    textBoxLat.Text = lat.ToString(CultureInfo.InvariantCulture);
                    textBoxLon.Text = lon.ToString(CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Error receiving data from the map: " + ex.Message);
            }
        }

        private async void MapPickerForm_Load(object sender, EventArgs e)
        {
            await InitBrowser();
        }

        // Validates input and returns the result to the calling form
        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            // Ensure input is valid before closing the dialog
            if (double.TryParse(textBoxLat.Text, CultureInfo.InvariantCulture, out double lat) &&
                double.TryParse(textBoxLon.Text, CultureInfo.InvariantCulture, out double lon))
            {
                SelectedLat = lat;
                SelectedLon = lon;

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            else
            {
                MessageBox.Show("Współrzędne mają nieprawidłowy format!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}