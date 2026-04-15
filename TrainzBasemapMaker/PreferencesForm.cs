
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

namespace TrainzBasemapMaker
{
    public partial class PreferencesForm : Form
    {
        private ToolTip warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "Błąd wprowadzania" };

        public PreferencesForm()
        {
            InitializeComponent();

            checkBoxAutoCounter.Checked = Properties.Settings.Default.AutoCounterNumber;
            checkBoxAutoKuid.Checked = Properties.Settings.Default.AutoKuidNumber;
            textBoxDefaultKuidFirstPart.Text = Properties.Settings.Default.DefaultKuidFirstPart;
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

        private void PreferencesFormClosing(object sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.AutoCounterNumber = checkBoxAutoCounter.Checked;
            Properties.Settings.Default.AutoKuidNumber = checkBoxAutoKuid.Checked;
            Properties.Settings.Default.DefaultKuidFirstPart = textBoxDefaultKuidFirstPart.Text;

            Properties.Settings.Default.Save();
        }
    }
}
