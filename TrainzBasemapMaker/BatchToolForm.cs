
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

namespace TrainzBasemapMaker
{
    public partial class BatchToolForm : Form
    {
        private TrainzFileManager _fileManager = new TrainzFileManager();

        private ToolTip warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "Błąd wprowadzania" };
        public BatchToolForm()
        {
            InitializeComponent();

            radioButton2048.Checked = true;
            textBoxBasemapDate.Text = "2026";

            comboBoxMapType.DataSource = WmsSource.availableMaps;
            comboBoxMapType.DisplayMember = "Name";

            BasemapFolderListBoxRefresh();
        }

        private void BasemapFolderListBoxRefresh()
        {
            basemapFolderListBox.Items.Clear();
            var groups = _fileManager.GetBasemapGroups();
            basemapFolderListBox.Items.AddRange(groups.ToArray());
        }

        private async void buttonConfAndDownload_Click(object sender, EventArgs e)
        {
            // 1. Walidacja
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

            // 2. Pobranie ustawień z UI
            string sourceGroup = basemapFolderListBox.SelectedItem.ToString();
            string targetGroup = textBoxDestinationFolder.Text;
            string targetDesignation = textBoxDesignation.Text;
            string year = textBoxBasemapDate.Text;
            WmsSource selectedMap = (WmsSource)comboBoxMapType.SelectedItem;
            int res = GetSelectedResolution();

            if(sourceGroup == targetGroup)
            {
                MessageBox.Show("Nazwa docelowego folderu musi być inna nić nazwa folderu źródłowego!", "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            UiEnabled(false);

            try
            {
                _fileManager.DeleteGroupFolder(targetGroup);

                var folders = _fileManager.GetKuidsInGroup(sourceGroup);
                int total = folders.Count;
                int current = 0;

                progressBar1.Minimum = 0;
                progressBar1.Maximum = total;
                progressBar1.Value = 0;

                labelProgress.Text = $"Przetworzono: {current} z {total}";
                labelProgress.Visible = true;

                foreach (var folder in folders)
                {
                    current++;

                    string[] parts = folder.Split('_');

                    if (parts.Length >= 7)
                    {
                        try
                        {
                            // Pobieramy dane ze starej nazwy folderu
                            int counter = int.Parse(parts[2]);
                            long x = long.Parse(parts[3]);
                            long y = long.Parse(parts[4]);
                            string kuid1 = parts[5]; // Zachowujemy oryginalny KUID cz. 1
                            string kuid2 = parts[6]; // Zachowujemy oryginalny KUID cz. 2

                            // Pobieranie obrazu
                            byte[] imageBytes = await selectedMap.GetMapImageAsync(year, x, y, res);

                            // Tworzenie plików w NOWEJ grupie
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
                            Debug.WriteLine($"Błąd kafelka {folder}: {ex.Message}");
                        }
                    }
                    progressBar1.Value = current;
                    labelProgress.Text = $"Przetworzono: {current} z {total}";
                }

                MessageBox.Show("Przetwarzanie seryjne zakończone pomyślnie!", "Sukces", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd krytyczny: " + ex.Message);
            }
            finally
            {
                UiEnabled(true);
                BasemapFolderListBoxRefresh(); // Odśwież listę, żeby zobaczyć nowy folder
            }
        }

        // Pomocnicza metoda do rozdzielczości
        private int GetSelectedResolution()
        {
            if (radioButton4096.Checked) return 4096;
            if (radioButton1024.Checked) return 1024;
            if (radioButton512.Checked) return 512;
            return 2048; // domyślny
        }

        // Prosta metoda do blokowania UI
        private void UiEnabled(bool enabled)
        {
            Cursor = enabled ? Cursors.Default : Cursors.WaitCursor;
            buttonConfAndDownload.Enabled = enabled;
            basemapFolderListBox.Enabled = enabled;
            groupBox1.Enabled = enabled;
            groupBox2.Enabled = enabled;
        }

        private void comboBoxMapType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBoxMapType.SelectedItem is WmsSource selected)
            {
                // Włączamy lub wyłączamy pole tekstowe roku w zależności od mapy
                textBoxBasemapDate.Enabled = selected.SupportsTime;
                label14.Enabled = selected.SupportsTime;

                bool isOrto = selected.Name.Contains("Ortofotomapa");

                radioButton4096.Enabled = isOrto;

                // Jeśli 4096 zostało wyłączone, a było zaznaczone - przełączamy na 2048
                if (!isOrto && radioButton4096.Checked)
                {
                    radioButton2048.Checked = true;
                }
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
                    warningToolTip.Show("Tutaj możesz wpisać tylko cyfry!", textBox, 50, -75, 2000);
                }
            }
        }
    }
}
