
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
    public partial class AboutProgramForm : Form, Classes.ILocalizableForm
    {
        public AboutProgramForm()
        {
            InitializeComponent();
            ApplyLocalization();

            // Set build metadata info
            labelVersion.Text = Classes.Constants.CurrentVersion;
            labelReleaseDate.Text = "13.09.2026";

            // Load the application icon from embedded resources using a memory stream
            using (var ms = new System.IO.MemoryStream(Properties.Resources.Icon))
            using (var temp = Image.FromStream(ms))
            {
                pictureBox1.Image = new Bitmap(temp);
            }
            
            TrainzBasemapMaker.Classes.ThemeManager.ApplyTheme(this);
        }

        public void ApplyLocalization()
        {
            this.Text = Localization.Strings.About_Title;
            label2.Text = Localization.Strings.About_Version;
            label3.Text = Localization.Strings.About_ReleaseDate;
            label4.Text = Localization.Strings.About_Author;
            label8.Text = Localization.Strings.About_License;
            label11.Text = Localization.Strings.About_GitHub;
            label5.Text = Localization.Strings.About_Libraries;
        }

        /// <summary>
        /// Custom paint handler to force crisp pixel-art scaling (Nearest Neighbor).
        /// This prevents the application icon from becoming blurry when rendered.
        /// </summary>
        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            if (pictureBox1.Image != null)
            {
                e.Graphics.DrawImage(pictureBox1.Image, 0, 0, pictureBox1.Width, pictureBox1.Height);
            }
        }

        private void linkLabelGitHubAuthor_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Open the author's GitHub profile in the default system browser
                Process.Start(new ProcessStartInfo("https://github.com/Ignacy110") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Localization.Strings.About_OpenUrlError, ex.Message), Localization.Strings.Common_Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void linkLabelGitHubSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                // Open the project repository in the default system browser
                Process.Start(new ProcessStartInfo("https://github.com/Ignacy110/TrainzBasemapMaker") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format(Localization.Strings.About_OpenUrlError, ex.Message), Localization.Strings.Common_Error, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}