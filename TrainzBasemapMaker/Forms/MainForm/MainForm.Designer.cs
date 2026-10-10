namespace TrainzBasemapMaker
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tabControl = new TrainzBasemapMaker.Classes.DarkTabControl();
            tabPageSingle = new TabPage();
            tabPageUnified = new TabPage();
            menuStrip1 = new MenuStrip();
            toolsToolStripMenuItem = new ToolStripMenuItem();
            findFreeKuidToolStripMenuItem = new ToolStripMenuItem();
            findSmallestFreeBasemapNumberToolStripMenuItem = new ToolStripMenuItem();
            refreshFolderAndBasemapListToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            batchProcessingToolStripMenuItem = new ToolStripMenuItem();
            preferencesToolStripMenuItem = new ToolStripMenuItem();
            helpToolStripMenuItem = new ToolStripMenuItem();
            websiteToolStripMenuItem = new ToolStripMenuItem();
            aboutProgramToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            toolStripStatusSpacer = new ToolStripStatusLabel();
            toolStripStatusUpdate = new ToolStripStatusLabel();
            tabControl.SuspendLayout();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabPageSingle);
            tabControl.Controls.Add(tabPageUnified);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 24);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1084, 765);
            tabControl.TabIndex = 0;
            // 
            // tabPageSingle
            // 
            tabPageSingle.BackColor = Color.FromArgb(45, 45, 48);
            tabPageSingle.Location = new Point(4, 24);
            tabPageSingle.Name = "tabPageSingle";
            tabPageSingle.Padding = new Padding(0);
            tabPageSingle.Size = new Size(1076, 737);
            tabPageSingle.TabIndex = 0;
            tabPageSingle.Text = "Pojedynczy podkład (2D)";
            tabPageSingle.UseVisualStyleBackColor = false;
            // 
            // tabPageUnified
            // 
            tabPageUnified.BackColor = Color.FromArgb(45, 45, 48);
            tabPageUnified.Location = new Point(4, 24);
            tabPageUnified.Name = "tabPageUnified";
            tabPageUnified.Padding = new Padding(0);
            tabPageUnified.Size = new Size(1176, 737);
            tabPageUnified.TabIndex = 1;
            tabPageUnified.Text = "Pobieranie obszarowe i teren (3D)";
            tabPageUnified.UseVisualStyleBackColor = false;
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { toolsToolStripMenuItem, preferencesToolStripMenuItem, helpToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1084, 24);
            menuStrip1.TabIndex = 1;
            menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            toolsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { findFreeKuidToolStripMenuItem, findSmallestFreeBasemapNumberToolStripMenuItem, refreshFolderAndBasemapListToolStripMenuItem, toolStripMenuItem1, batchProcessingToolStripMenuItem });
            toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            toolsToolStripMenuItem.Size = new Size(70, 20);
            toolsToolStripMenuItem.Text = "&Narzędzia";
            // 
            // findFreeKuidToolStripMenuItem
            // 
            findFreeKuidToolStripMenuItem.Name = "findFreeKuidToolStripMenuItem";
            findFreeKuidToolStripMenuItem.Size = new Size(300, 22);
            findFreeKuidToolStripMenuItem.Text = "Znajdź wolny numer &KUID";
            findFreeKuidToolStripMenuItem.Click += findFreeKuidToolStripMenuItem_Click;
            // 
            // findSmallestFreeBasemapNumberToolStripMenuItem
            // 
            findSmallestFreeBasemapNumberToolStripMenuItem.Name = "findSmallestFreeBasemapNumberToolStripMenuItem";
            findSmallestFreeBasemapNumberToolStripMenuItem.Size = new Size(300, 22);
            findSmallestFreeBasemapNumberToolStripMenuItem.Text = "Znajdź najmniejszy wolny numer &podkładu";
            findSmallestFreeBasemapNumberToolStripMenuItem.Click += findSmallestFreeBasemapNumberToolStripMenuItem_Click;
            // 
            // refreshFolderAndBasemapListToolStripMenuItem
            // 
            refreshFolderAndBasemapListToolStripMenuItem.Name = "refreshFolderAndBasemapListToolStripMenuItem";
            refreshFolderAndBasemapListToolStripMenuItem.Size = new Size(300, 22);
            refreshFolderAndBasemapListToolStripMenuItem.Text = "&Odśwież listę folderów";
            refreshFolderAndBasemapListToolStripMenuItem.Click += refreshFolderAndBasemapListToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(297, 6);
            // 
            // batchProcessingToolStripMenuItem
            // 
            batchProcessingToolStripMenuItem.Name = "batchProcessingToolStripMenuItem";
            batchProcessingToolStripMenuItem.Size = new Size(300, 22);
            batchProcessingToolStripMenuItem.Text = "Przetwarzanie seryjne";
            batchProcessingToolStripMenuItem.Click += batchProcessingToolStripMenuItem_Click;
            // 
            // preferencesToolStripMenuItem
            // 
            preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            preferencesToolStripMenuItem.Size = new Size(78, 20);
            preferencesToolStripMenuItem.Text = "P&referencje";
            preferencesToolStripMenuItem.Click += preferencesToolStripMenuItem_Click;
            // 
            // helpToolStripMenuItem
            // 
            helpToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { websiteToolStripMenuItem, aboutProgramToolStripMenuItem });
            helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            helpToolStripMenuItem.Size = new Size(57, 20);
            helpToolStripMenuItem.Text = "&Pomoc";
            // 
            // websiteToolStripMenuItem
            // 
            websiteToolStripMenuItem.Name = "websiteToolStripMenuItem";
            websiteToolStripMenuItem.Size = new Size(213, 22);
            websiteToolStripMenuItem.Text = "&Strona programu - GitHub";
            websiteToolStripMenuItem.Click += websiteToolStripMenuItem_Click;
            // 
            // aboutProgramToolStripMenuItem
            // 
            aboutProgramToolStripMenuItem.Name = "aboutProgramToolStripMenuItem";
            aboutProgramToolStripMenuItem.Size = new Size(213, 22);
            aboutProgramToolStripMenuItem.Text = "&O programie";
            aboutProgramToolStripMenuItem.Click += aboutProgramToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1, toolStripStatusSpacer, toolStripStatusUpdate });
            statusStrip1.Location = new Point(0, 789);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1084, 22);
            statusStrip1.TabIndex = 2;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(51, 17);
            toolStripStatusLabel1.Text = "Gotowy.";
            // 
            // toolStripStatusSpacer
            // 
            toolStripStatusSpacer.Name = "toolStripStatusSpacer";
            toolStripStatusSpacer.Size = new Size(1018, 17);
            toolStripStatusSpacer.Spring = true;
            // 
            // toolStripStatusUpdate
            // 
            toolStripStatusUpdate.IsLink = true;
            toolStripStatusUpdate.LinkBehavior = LinkBehavior.HoverUnderline;
            toolStripStatusUpdate.Name = "toolStripStatusUpdate";
            toolStripStatusUpdate.Size = new Size(0, 17);
            toolStripStatusUpdate.Visible = false;
            toolStripStatusUpdate.Click += toolStripStatusUpdate_Click;
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1084, 811);
            Controls.Add(tabControl);
            Controls.Add(statusStrip1);
            Controls.Add(menuStrip1);
            MainMenuStrip = menuStrip1;
            MinimumSize = new Size(1100, 850);
            Name = "MainForm";
            Text = "Trainz Basemap Maker";
            tabControl.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        private TrainzBasemapMaker.Classes.DarkTabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSingle;
        private System.Windows.Forms.TabPage tabPageUnified;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem toolsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findFreeKuidToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem findSmallestFreeBasemapNumberToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem refreshFolderAndBasemapListToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem batchProcessingToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem preferencesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem helpToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem websiteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aboutProgramToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        public System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusSpacer;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusUpdate;
    }
}
