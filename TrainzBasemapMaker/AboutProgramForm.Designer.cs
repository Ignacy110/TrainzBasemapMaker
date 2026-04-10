namespace TrainzBasemapMaker
{
    partial class AboutProgramForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutProgramForm));
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            pictureBox1 = new PictureBox();
            linkLabelGitHubAuthor = new LinkLabel();
            label5 = new Label();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            linkLabelGitHubSite = new LinkLabel();
            label11 = new Label();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label1.Location = new Point(100, 22);
            label1.Name = "label1";
            label1.Size = new Size(232, 30);
            label1.TabIndex = 0;
            label1.Text = "Trainz Basemap Maker";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(102, 66);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 1;
            label2.Text = "Wersja:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(102, 91);
            label3.Name = "label3";
            label3.Size = new Size(81, 15);
            label3.TabIndex = 2;
            label3.Text = "Data wydania:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(102, 141);
            label4.Name = "label4";
            label4.Size = new Size(40, 15);
            label4.TabIndex = 3;
            label4.Text = "Autor:";
            // 
            // pictureBox1
            // 
            pictureBox1.Location = new Point(22, 66);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(64, 64);
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.TabIndex = 4;
            pictureBox1.TabStop = false;
            pictureBox1.Paint += pictureBox1_Paint;
            // 
            // linkLabelGitHubAuthor
            // 
            linkLabelGitHubAuthor.AutoSize = true;
            linkLabelGitHubAuthor.LinkArea = new LinkArea(1, 28);
            linkLabelGitHubAuthor.Location = new Point(250, 140);
            linkLabelGitHubAuthor.Name = "linkLabelGitHubAuthor";
            linkLabelGitHubAuthor.Size = new Size(174, 21);
            linkLabelGitHubAuthor.TabIndex = 5;
            linkLabelGitHubAuthor.TabStop = true;
            linkLabelGitHubAuthor.Text = "(https://github.com/Ignacy110)";
            linkLabelGitHubAuthor.UseCompatibleTextRendering = true;
            linkLabelGitHubAuthor.LinkClicked += linkLabelGitHubAuthor_LinkClicked;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(189, 66);
            label5.Name = "label5";
            label5.Size = new Size(37, 15);
            label5.TabIndex = 6;
            label5.Text = "v0.0.1";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(189, 91);
            label6.Name = "label6";
            label6.Size = new Size(61, 15);
            label6.TabIndex = 7;
            label6.Text = "08.04.2026";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(189, 141);
            label7.Name = "label7";
            label7.Size = new Size(60, 15);
            label7.TabIndex = 8;
            label7.Text = "Ignacy110";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(102, 116);
            label8.Name = "label8";
            label8.Size = new Size(53, 15);
            label8.TabIndex = 9;
            label8.Text = "Licencja:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(189, 116);
            label9.Name = "label9";
            label9.Size = new Size(93, 15);
            label9.TabIndex = 10;
            label9.Text = "LGPL-2.1 license";
            // 
            // linkLabelGitHubSite
            // 
            linkLabelGitHubSite.AutoSize = true;
            linkLabelGitHubSite.LinkArea = new LinkArea(1, 43);
            linkLabelGitHubSite.Location = new Point(102, 192);
            linkLabelGitHubSite.Name = "linkLabelGitHubSite";
            linkLabelGitHubSite.Size = new Size(17, 21);
            linkLabelGitHubSite.TabIndex = 12;
            linkLabelGitHubSite.TabStop = true;
            linkLabelGitHubSite.Text = "(*)";
            linkLabelGitHubSite.UseCompatibleTextRendering = true;
            linkLabelGitHubSite.LinkClicked += linkLabelGitHubSite_LinkClicked;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(102, 177);
            label11.Name = "label11";
            label11.Size = new Size(149, 15);
            label11.TabIndex = 11;
            label11.Text = "Strona programu - GitHub:";
            // 
            // AboutProgramForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(440, 248);
            Controls.Add(linkLabelGitHubSite);
            Controls.Add(label11);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(linkLabelGitHubAuthor);
            Controls.Add(pictureBox1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "AboutProgramForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "O programie";
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private PictureBox pictureBox1;
        private LinkLabel linkLabelGitHubAuthor;
        private Label label5;
        private Label label6;
        private Label label7;
        private Label label8;
        private Label label9;
        private LinkLabel linkLabelGitHubSite;
        private Label label11;
    }
}