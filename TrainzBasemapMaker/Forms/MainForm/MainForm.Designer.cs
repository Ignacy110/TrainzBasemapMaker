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
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPageSingle = new System.Windows.Forms.TabPage();
            this.tabPageGrid = new System.Windows.Forms.TabPage();
            this.tabPageTerrain = new System.Windows.Forms.TabPage();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.toolsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findFreeKuidToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.findSmallestFreeBasemapNumberToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.refreshFolderAndBasemapListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.batchProcessingToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.preferencesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.helpToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.websiteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aboutProgramToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tabControl.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSingle);
            this.tabControl.Controls.Add(this.tabPageGrid);
            this.tabControl.Controls.Add(this.tabPageTerrain);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 24);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1210, 826);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageSingle
            // 
            this.tabPageSingle.Location = new System.Drawing.Point(4, 24);
            this.tabPageSingle.Name = "tabPageSingle";
            this.tabPageSingle.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSingle.Size = new System.Drawing.Size(1202, 798);
            this.tabPageSingle.TabIndex = 0;
            this.tabPageSingle.Text = "Pojedynczy podkład (2D)";
            this.tabPageSingle.UseVisualStyleBackColor = true;
            // 
            // tabPageGrid
            // 
            this.tabPageGrid.Location = new System.Drawing.Point(4, 24);
            this.tabPageGrid.Name = "tabPageGrid";
            this.tabPageGrid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGrid.Size = new System.Drawing.Size(1202, 798);
            this.tabPageGrid.TabIndex = 1;
            this.tabPageGrid.Text = "Siatka podkładów (2D)";
            this.tabPageGrid.UseVisualStyleBackColor = true;
            // 
            // tabPageTerrain
            // 
            this.tabPageTerrain.Location = new System.Drawing.Point(4, 24);
            this.tabPageTerrain.Name = "tabPageTerrain";
            this.tabPageTerrain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTerrain.Size = new System.Drawing.Size(1202, 798);
            this.tabPageTerrain.TabIndex = 2;
            this.tabPageTerrain.Text = "Generator terenu (3D)";
            this.tabPageTerrain.UseVisualStyleBackColor = true;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolsToolStripMenuItem,
            this.preferencesToolStripMenuItem,
            this.helpToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1210, 24);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // toolsToolStripMenuItem
            // 
            this.toolsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.findFreeKuidToolStripMenuItem,
            this.findSmallestFreeBasemapNumberToolStripMenuItem,
            this.refreshFolderAndBasemapListToolStripMenuItem,
            this.toolStripMenuItem1,
            this.batchProcessingToolStripMenuItem});
            this.toolsToolStripMenuItem.Name = "toolsToolStripMenuItem";
            this.toolsToolStripMenuItem.Size = new System.Drawing.Size(70, 20);
            this.toolsToolStripMenuItem.Text = "&Narzędzia";
            // 
            // findFreeKuidToolStripMenuItem
            // 
            this.findFreeKuidToolStripMenuItem.Name = "findFreeKuidToolStripMenuItem";
            this.findFreeKuidToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.findFreeKuidToolStripMenuItem.Text = "Znajdź wolny numer &KUID";
            this.findFreeKuidToolStripMenuItem.Click += new System.EventHandler(this.findFreeKuidToolStripMenuItem_Click);
            // 
            // findSmallestFreeBasemapNumberToolStripMenuItem
            // 
            this.findSmallestFreeBasemapNumberToolStripMenuItem.Name = "findSmallestFreeBasemapNumberToolStripMenuItem";
            this.findSmallestFreeBasemapNumberToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.findSmallestFreeBasemapNumberToolStripMenuItem.Text = "Znajdź najmniejszy wolny numer &podkładu";
            this.findSmallestFreeBasemapNumberToolStripMenuItem.Click += new System.EventHandler(this.findSmallestFreeBasemapNumberToolStripMenuItem_Click);
            // 
            // refreshFolderAndBasemapListToolStripMenuItem
            // 
            this.refreshFolderAndBasemapListToolStripMenuItem.Name = "refreshFolderAndBasemapListToolStripMenuItem";
            this.refreshFolderAndBasemapListToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.refreshFolderAndBasemapListToolStripMenuItem.Text = "&Odśwież listę folderów";
            this.refreshFolderAndBasemapListToolStripMenuItem.Click += new System.EventHandler(this.refreshFolderAndBasemapListToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(270, 6);
            // 
            // batchProcessingToolStripMenuItem
            // 
            this.batchProcessingToolStripMenuItem.Name = "batchProcessingToolStripMenuItem";
            this.batchProcessingToolStripMenuItem.Size = new System.Drawing.Size(273, 22);
            this.batchProcessingToolStripMenuItem.Text = "Przetwarzanie seryjne";
            this.batchProcessingToolStripMenuItem.Click += new System.EventHandler(this.batchProcessingToolStripMenuItem_Click);
            // 
            // preferencesToolStripMenuItem
            // 
            this.preferencesToolStripMenuItem.Name = "preferencesToolStripMenuItem";
            this.preferencesToolStripMenuItem.Size = new System.Drawing.Size(80, 20);
            this.preferencesToolStripMenuItem.Text = "P&referencje";
            this.preferencesToolStripMenuItem.Click += new System.EventHandler(this.preferencesToolStripMenuItem_Click);
            // 
            // helpToolStripMenuItem
            // 
            this.helpToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.websiteToolStripMenuItem,
            this.aboutProgramToolStripMenuItem});
            this.helpToolStripMenuItem.Name = "helpToolStripMenuItem";
            this.helpToolStripMenuItem.Size = new System.Drawing.Size(57, 20);
            this.helpToolStripMenuItem.Text = "&Pomoc";
            // 
            // websiteToolStripMenuItem
            // 
            this.websiteToolStripMenuItem.Name = "websiteToolStripMenuItem";
            this.websiteToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.websiteToolStripMenuItem.Text = "&Strona programu - GitHub";
            this.websiteToolStripMenuItem.Click += new System.EventHandler(this.websiteToolStripMenuItem_Click);
            // 
            // aboutProgramToolStripMenuItem
            // 
            this.aboutProgramToolStripMenuItem.Name = "aboutProgramToolStripMenuItem";
            this.aboutProgramToolStripMenuItem.Size = new System.Drawing.Size(213, 22);
            this.aboutProgramToolStripMenuItem.Text = "&O programie";
            this.aboutProgramToolStripMenuItem.Click += new System.EventHandler(this.aboutProgramToolStripMenuItem_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 850);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Trainz Basemap Maker";
            this.tabControl.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSingle;
        private System.Windows.Forms.TabPage tabPageGrid;
        private System.Windows.Forms.TabPage tabPageTerrain;
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
    }
}
