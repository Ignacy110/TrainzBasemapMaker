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
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;
using TrainzBasemapMaker.Classes;



namespace TrainzBasemapMaker
{
    public partial class MainForm : Form, ILocalizableForm
    {
        private SingleBasemapForm singleBasemapForm = null!;
        private UnifiedGridToolForm unifiedGridToolForm = null!;

        public MainForm()
        {
            InitializeComponent();
            ApplyLocalization();
            LoadTools();
            ThemeManager.ApplyTheme(this);
            
            this.Load += MainForm_Load;
            this.FormClosing += MainForm_FormClosing;
        }

        public void ApplyLocalization()
        {
            this.Text = Localization.Strings.App_Title;
            toolsToolStripMenuItem.Text = Localization.Strings.Main_Menu_Tools;
            findFreeKuidToolStripMenuItem.Text = Localization.Strings.Main_Menu_FindFreeKuid;
            findSmallestFreeBasemapNumberToolStripMenuItem.Text = Localization.Strings.Main_Menu_FindSmallestFreeBasemap;
            refreshFolderAndBasemapListToolStripMenuItem.Text = Localization.Strings.Main_Menu_RefreshFolderList;
            batchProcessingToolStripMenuItem.Text = Localization.Strings.Main_Menu_BatchProcessing;
            preferencesToolStripMenuItem.Text = Localization.Strings.Main_Menu_Preferences;
            helpToolStripMenuItem.Text = Localization.Strings.Main_Menu_Help;
            websiteToolStripMenuItem.Text = Localization.Strings.Main_Menu_GitHubWebsite;
            aboutProgramToolStripMenuItem.Text = Localization.Strings.Main_Menu_About;
            tabPageSingle.Text = Localization.Strings.Main_Tab_Single;
            tabPageUnified.Text = Localization.Strings.Main_Tab_Unified;
            toolStripStatusLabel1.Text = Localization.Strings.Common_Ready;
        }

        private void LoadTools()
        {
            // Load SingleBasemapForm
            singleBasemapForm = new SingleBasemapForm();
            EmbedFormInTab(singleBasemapForm, tabPageSingle);

            // Load UnifiedGridToolForm (replaces GridToolForm + TerrainGridToolForm)
            unifiedGridToolForm = new UnifiedGridToolForm();
            EmbedFormInTab(unifiedGridToolForm, tabPageUnified);
        }

        private void EmbedFormInTab(Form form, TabPage tabPage)
        {
            if (form is IMainMenuOperations op)
            {
                op.StatusUpdate += (msg) => 
                {
                    if (this.InvokeRequired)
                        this.Invoke(new Action(() => toolStripStatusLabel1.Text = msg));
                    else
                        toolStripStatusLabel1.Text = msg;
                };
            }
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

        private void findFreeKuidToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var activeTab = tabControl.SelectedTab;
            if (activeTab != null && activeTab.Controls.Count > 0 && activeTab.Controls[0] is IMainMenuOperations op)
            {
                op.FindFreeKuid();
            }
        }

        private void findSmallestFreeBasemapNumberToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var activeTab = tabControl.SelectedTab;
            if (activeTab != null && activeTab.Controls.Count > 0 && activeTab.Controls[0] is IMainMenuOperations op)
            {
                op.FindSmallestFreeBasemapNumber();
            }
        }

        private void refreshFolderAndBasemapListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var activeTab = tabControl.SelectedTab;
            if (activeTab != null && activeTab.Controls.Count > 0 && activeTab.Controls[0] is IMainMenuOperations op)
            {
                op.RefreshLists();
            }
        }

        private void batchProcessingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (BatchToolForm info = new BatchToolForm())
            {
                info.ShowDialog();
                // Refresh lists on all tabs if needed, or just the active one
                var activeTab = tabControl.SelectedTab;
                if (activeTab != null && activeTab.Controls.Count > 0 && activeTab.Controls[0] is IMainMenuOperations op)
                {
                    op.RefreshLists();
                }
            }
        }

        private void preferencesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (PreferencesForm info = new PreferencesForm())
            {
                info.ShowDialog();
                if (Properties.Settings.Default.AutoKuidNumber)
                {
                    var activeTab = tabControl.SelectedTab;
                    if (activeTab != null && activeTab.Controls.Count > 0 && activeTab.Controls[0] is IMainMenuOperations op)
                    {
                        op.FindFreeKuid();
                    }
                }
            }
        }

        private void websiteToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                Process.Start(new ProcessStartInfo("https://github.com/Ignacy110/TrainzBasemapMaker") { UseShellExecute = true });
            }
            catch { }
        }

        private void aboutProgramToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (AboutProgramForm info = new AboutProgramForm())
            {
                info.ShowDialog();
            }
        }
    }
}
