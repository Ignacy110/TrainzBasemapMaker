
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
        // ToolTip used to provide visual feedback for input validation errors
        private ToolTip warningToolTip = new ToolTip { IsBalloon = true, ToolTipTitle = "Błąd wprowadzania" };

        public PreferencesForm()
        {
            InitializeComponent();

            // Load user preferences from application settings upon initialization
            checkBoxAutoCounter.Checked = Properties.Settings.Default.AutoCounterNumber;
            checkBoxAutoKuid.Checked = Properties.Settings.Default.AutoKuidNumber;
            textBoxDefaultKuidFirstPart.Text = Properties.Settings.Default.DefaultKuidFirstPart;
            checkBoxDarkMode.Checked = Properties.Settings.Default.DarkMode;
            
            TrainzBasemapMaker.Classes.ThemeManager.ApplyTheme(this);

            // Subscribe to CheckedChanged event for immediate theme application
            checkBoxDarkMode.CheckedChanged += CheckBoxDarkMode_CheckedChanged;
        }

        private void CheckBoxDarkMode_CheckedChanged(object? sender, EventArgs e)
        {
            Properties.Settings.Default.DarkMode = checkBoxDarkMode.Checked;
            Properties.Settings.Default.Save();
            
            foreach (Form f in Application.OpenForms)
            {
                TrainzBasemapMaker.Classes.ThemeManager.ApplyTheme(f);
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

        /// <summary>
        /// Saves current UI values back to the application settings when the form is closing.
        /// </summary>
        private void PreferencesFormClosing(object sender, FormClosingEventArgs e)
        {
            // Transfer UI state to the Settings object
            Properties.Settings.Default.AutoCounterNumber = checkBoxAutoCounter.Checked;
            Properties.Settings.Default.AutoKuidNumber = checkBoxAutoKuid.Checked;
            Properties.Settings.Default.DefaultKuidFirstPart = textBoxDefaultKuidFirstPart.Text;
            
            Properties.Settings.Default.Save();
        }
    }
}
