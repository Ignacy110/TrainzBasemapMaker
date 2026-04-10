using System.Diagnostics;

namespace TrainzBasemapMaker
{
    public partial class AboutProgramForm : Form
    {
        public AboutProgramForm()
        {
            InitializeComponent();

            var ms = new System.IO.MemoryStream(Properties.Resources.Icon);
            pictureBox1.Image = Image.FromStream(ms);

        }

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
                Process.Start(new ProcessStartInfo("https://github.com/Ignacy110") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd otwarcia strony:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void linkLabelGitHubSite_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("*") { UseShellExecute = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show("Błąd otwarcia strony:\n\n" + ex.Message, "Błąd", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
    }
}