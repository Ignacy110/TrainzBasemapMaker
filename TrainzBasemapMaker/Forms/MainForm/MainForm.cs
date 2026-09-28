using System;
using System.Drawing;
using System.Windows.Forms;
using TrainzBasemapMaker.Classes;

namespace TrainzBasemapMaker
{
    public partial class MainForm : Form
    {
        private SingleBasemapForm singleBasemapForm;
        private GridToolForm gridToolForm;
        private TerrainGridToolForm terrainToolForm;

        public MainForm()
        {
            InitializeComponent();
            ThemeManager.ApplyTheme(this);
            LoadTools();
            
            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;
        }

        private void LoadTools()
        {
            // Load SingleBasemapForm (Pojedynczy podkład)
            singleBasemapForm = new SingleBasemapForm();
            EmbedFormInTab(singleBasemapForm, tabPageSingle);

            // Przenieś MenuStrip z SingleBasemapForm do MainForm, aby był na samej górze i zawsze widoczny
            if (singleBasemapForm.MainMenuStrip != null)
            {
                var menu = singleBasemapForm.MainMenuStrip;
                singleBasemapForm.Controls.Remove(menu);
                this.Controls.Add(menu);
                this.MainMenuStrip = menu;
                
                // Zapewnienie, że MenuStrip jest przypięty do góry
                menu.Dock = DockStyle.Top;
                
                // TabControl ma Dock = Fill, więc musi "wypełnić resztę" 
                // BringToFront daje mu niższy priorytet w kolejności dokowania (wypełnia to, co zostało po przypięciu MenuStrip)
                tabControl.BringToFront();
            }

            // Load GridToolForm (Siatka podkładów)
            gridToolForm = new GridToolForm();
            EmbedFormInTab(gridToolForm, tabPageGrid);

            // Load TerrainGridToolForm (Generator terenu)
            terrainToolForm = new TerrainGridToolForm();
            EmbedFormInTab(terrainToolForm, tabPageTerrain);
        }

        private void EmbedFormInTab(Form form, TabPage tabPage)
        {
            form.TopLevel = false;
            form.FormBorderStyle = FormBorderStyle.None;
            form.Dock = DockStyle.Fill;
            tabPage.Controls.Add(form);
            form.Show();
        }

        private void MainForm_Load(object? sender, EventArgs e)
        {
            this.Size = Properties.Settings.Default.MainFormSize;
            this.WindowState = Properties.Settings.Default.MainFormState;
        }

        private void MainForm_FormClosing(object? sender, FormClosingEventArgs e)
        {
            Properties.Settings.Default.MainFormState = this.WindowState;
            if (this.WindowState == FormWindowState.Normal)
            {
                Properties.Settings.Default.MainFormSize = this.Size;
            }
            else
            {
                Properties.Settings.Default.MainFormSize = this.RestoreBounds.Size;
            }
            Properties.Settings.Default.Save();
        }
    }
}
