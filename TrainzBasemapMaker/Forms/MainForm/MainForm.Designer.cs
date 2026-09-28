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
            this.tabControl.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPageSingle);
            this.tabControl.Controls.Add(this.tabPageGrid);
            this.tabControl.Controls.Add(this.tabPageTerrain);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(1210, 850);
            this.tabControl.TabIndex = 0;
            // 
            // tabPageSingle
            // 
            this.tabPageSingle.Location = new System.Drawing.Point(4, 24);
            this.tabPageSingle.Name = "tabPageSingle";
            this.tabPageSingle.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSingle.Size = new System.Drawing.Size(1202, 822);
            this.tabPageSingle.TabIndex = 0;
            this.tabPageSingle.Text = "Pojedynczy podkład (2D)";
            this.tabPageSingle.UseVisualStyleBackColor = true;
            // 
            // tabPageGrid
            // 
            this.tabPageGrid.Location = new System.Drawing.Point(4, 24);
            this.tabPageGrid.Name = "tabPageGrid";
            this.tabPageGrid.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageGrid.Size = new System.Drawing.Size(1202, 822);
            this.tabPageGrid.TabIndex = 1;
            this.tabPageGrid.Text = "Siatka podkładów (2D)";
            this.tabPageGrid.UseVisualStyleBackColor = true;
            // 
            // tabPageTerrain
            // 
            this.tabPageTerrain.Location = new System.Drawing.Point(4, 24);
            this.tabPageTerrain.Name = "tabPageTerrain";
            this.tabPageTerrain.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageTerrain.Size = new System.Drawing.Size(1202, 822);
            this.tabPageTerrain.TabIndex = 2;
            this.tabPageTerrain.Text = "Generator terenu (3D)";
            this.tabPageTerrain.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1210, 850);
            this.Controls.Add(this.tabControl);

            this.Name = "MainForm";
            this.Text = "Trainz Basemap Maker";
            this.tabControl.ResumeLayout(false);
            this.ResumeLayout(false);
        }

        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabPageSingle;
        private System.Windows.Forms.TabPage tabPageGrid;
        private System.Windows.Forms.TabPage tabPageTerrain;
    }
}
