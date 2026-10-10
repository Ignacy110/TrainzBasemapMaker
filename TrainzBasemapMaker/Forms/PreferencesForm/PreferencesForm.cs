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

using System;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using TrainzBasemapMaker.Classes;
using TrainzBasemapMaker.Localization;

namespace TrainzBasemapMaker
{
    public partial class PreferencesForm : Form, ILocalizableForm
    {
        // ToolTip used to provide visual feedback for input validation errors
        private ToolTip warningToolTip = new ToolTip { IsBalloon = true };

        public PreferencesForm()
        {
            InitializeComponent();

            // Load user preferences from application settings upon initialization
            checkBoxAutoCounter.Checked = Properties.Settings.Default.AutoCounterNumber;
            checkBoxAutoKuid.Checked = Properties.Settings.Default.AutoKuidNumber;
            checkBoxKuidAutoCountPerFirstPart.Checked = Properties.Settings.Default.KuidAutoCountPerFirstPart;
            textBoxDefaultKuidFirstPart.Text = Properties.Settings.Default.DefaultKuidFirstPart;
            textBoxMinKuidPart2.Text = Math.Max(1, Properties.Settings.Default.MinKuidPart2).ToString();
            checkBoxDarkMode.Checked = Properties.Settings.Default.DarkMode;
            comboBoxBasemapSize.SelectedItem = Properties.Settings.Default.BasemapSize.ToString();
            textBoxTMI.Text = Properties.Settings.Default.TrainzMeshImporterPath;
            comboBoxLanguage.SelectedIndex = LocalizationManager.CurrentLanguage == LocalizationManager.LanguagePolish ? 1 : 0;

            ApplyLocalization();
            ThemeManager.ApplyTheme(this);

            // Subscribe to CheckedChanged event for immediate theme application
            checkBoxDarkMode.CheckedChanged += CheckBoxDarkMode_CheckedChanged;
        }

        public void ApplyLocalization()
        {
            this.Text = Strings.Pref_Title;
            label2.Text = Strings.Pref_Header;
            checkBoxAutoCounter.Text = Strings.Pref_AutoCounter;
            checkBoxAutoKuid.Text = Strings.Pref_AutoKuid;
            checkBoxKuidAutoCountPerFirstPart.Text = Strings.Pref_KuidPerPart1;
            label1.Text = Strings.Pref_DefaultKuidPart1;
            labelMinKuidPart2.Text = Strings.Pref_MinKuidPart2;
            checkBoxDarkMode.Text = Strings.Pref_DarkMode;
            label3.Text = Strings.Pref_BasemapSize;
            labelTMI.Text = Strings.Pref_TmiPath;
            buttonBrowseTMI.Text = Strings.Common_Browse;
            labelLanguage.Text = Strings.Pref_Language;
            warningToolTip.ToolTipTitle = Strings.Common_InputError;

            // Update language combo items without triggering index change
            int currentIdx = comboBoxLanguage.SelectedIndex;
            comboBoxLanguage.SelectedIndexChanged -= ComboBoxLanguage_SelectedIndexChanged;
            comboBoxLanguage.Items.Clear();
            comboBoxLanguage.Items.AddRange(new object[] { Strings.Pref_Language_English, Strings.Pref_Language_Polish });
            comboBoxLanguage.SelectedIndex = currentIdx >= 0 ? currentIdx : (LocalizationManager.CurrentLanguage == LocalizationManager.LanguagePolish ? 1 : 0);
            comboBoxLanguage.SelectedIndexChanged += ComboBoxLanguage_SelectedIndexChanged;
        }

        private void textBoxTMI_TextChanged(object? sender, EventArgs e)
        {
            Properties.Settings.Default.TrainzMeshImporterPath = textBoxTMI.Text;
        }

        private void buttonBrowseTMI_Click(object? sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = Strings.Pref_TmiFilter;
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    textBoxTMI.Text = ofd.FileName;
                }
            }
        }

        private void ComboBoxLanguage_SelectedIndexChanged(object? sender, EventArgs e)
        {
            string selectedLang = comboBoxLanguage.SelectedIndex == 1 ? LocalizationManager.LanguagePolish : LocalizationManager.LanguageEnglish;
            if (selectedLang != LocalizationManager.CurrentLanguage)
            {
                Properties.Settings.Default.Language = selectedLang;
                Properties.Settings.Default.Save();
                LocalizationManager.SetLanguage(selectedLang);
                LocalizationManager.ApplyLanguageToOpenForms();
            }
        }

        private void CheckBoxDarkMode_CheckedChanged(object? sender, EventArgs e)
        {
            Properties.Settings.Default.DarkMode = checkBoxDarkMode.Checked;
            Properties.Settings.Default.Save();
            
            foreach (Form f in Application.OpenForms.Cast<Form>().ToList())
            {
                ThemeManager.ApplyTheme(f);
                f.Invalidate(true);
                f.Refresh();
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
                    warningToolTip.Show(Strings.Common_OnlyDigitsTooltip, textBox, 50, -75, 2000);
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
            Properties.Settings.Default.KuidAutoCountPerFirstPart = checkBoxKuidAutoCountPerFirstPart.Checked;
            Properties.Settings.Default.DefaultKuidFirstPart = textBoxDefaultKuidFirstPart.Text;
            if (int.TryParse(textBoxMinKuidPart2.Text, out int minKuid) && minKuid >= 1)
            {
                Properties.Settings.Default.MinKuidPart2 = minKuid;
            }
            else
            {
                Properties.Settings.Default.MinKuidPart2 = 1;
            }
            if (int.TryParse(comboBoxBasemapSize.SelectedItem?.ToString(), out int size))
            {
                Properties.Settings.Default.BasemapSize = size;
            }
            Properties.Settings.Default.TrainzMeshImporterPath = textBoxTMI.Text;
            
            Properties.Settings.Default.Save();
        }
    }
}
