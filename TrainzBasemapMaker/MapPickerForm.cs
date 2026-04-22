
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
        // Tu zapiszemy wynik, który odczytasz w MainForm
        public double SelectedLat { get; private set; }
        public double SelectedLon { get; private set; }

        public MapPickerForm()
        {
            InitializeComponent();
        }

        public async Task InitBrowser()
        {
            await webView21.EnsureCoreWebView2Async(null);

            webView21.CoreWebView2.Settings.UserAgent = "TrainzBasemapMaker/v0.5.0-aplha (https://github.com/Ignacy110/TrainzBasemapMaker)";

            // Budujemy ścieżkę do pliku
            string indexPath = Path.Combine(Application.StartupPath, "Web", "map.html");

            // Ładujemy plik (może wymagać file:/// na początku)
            webView21.CoreWebView2.Navigate("file:///" + indexPath);

            // TO BYŁO PODKREŚLONE - teraz metoda poniżej musi istnieć
            webView21.CoreWebView2.WebMessageReceived += WebView21_WebMessageReceived;
        }

        // TA METODA NAPRAWIA BŁĄD:
        private void WebView21_WebMessageReceived(object sender, CoreWebView2WebMessageReceivedEventArgs e)
        {
            string json = e.WebMessageAsJson;

            try
            {
                using (JsonDocument doc = JsonDocument.Parse(json))
                {
                    double lat = doc.RootElement.GetProperty("lat").GetDouble();
                    double lon = doc.RootElement.GetProperty("lon").GetDouble();

                    // Wpisujemy współrzędne do TextBoxów wewnątrz MapPickerForm
                    // Używamy InvariantCulture, aby zawsze mieć kropkę jako separator
                    textBoxLat.Text = lat.ToString(CultureInfo.InvariantCulture);
                    textBoxLon.Text = lon.ToString(CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("Błąd odbioru danych z mapy: " + ex.Message);
            }
        }

        private async void MapPickerForm_Load(object sender, EventArgs e)
        {
            // Dodaj await, żeby przeglądarka zdążyła się zainicjować
            await InitBrowser();
        }

        // PRZYCISK ZATWIERDŹ
        private void buttonConfirm_Click(object sender, EventArgs e)
        {
            // Próbujemy pobrać wartości z pól tekstowych (na wypadek, gdyby użytkownik coś tam wpisał ręcznie)
            if (double.TryParse(textBoxLat.Text, CultureInfo.InvariantCulture, out double lat) &&
                double.TryParse(textBoxLon.Text, CultureInfo.InvariantCulture, out double lon))
            {
                SelectedLat = lat;
                SelectedLon = lon;

                this.DialogResult = DialogResult.OK; // To sygnalizuje sukces
                this.Close();
            }
            else
            {
                MessageBox.Show("Współrzędne mają nieprawidłowy format!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        // PRZYCISK ANULUJ
        private void buttonCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel; // To sygnalizuje przerwanie
            this.Close();
        }
    }
}