namespace TrainzBasemapMaker
{
    partial class BatchToolForm
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
            this.comboBoxResolution = new System.Windows.Forms.ComboBox();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchToolForm));
            basemapFolderListBox = new ListBox();
            label10 = new Label();
            progressBar1 = new ProgressBar();
            comboBoxMapType = new ComboBox();
            label15 = new Label();
            textBoxBasemapDate = new TextBox();
            label14 = new Label();
            label2 = new Label();
            label13 = new Label();
            textBoxDesignation = new TextBox();
            textBoxDestinationFolder = new TextBox();
            label4 = new Label();
            buttonConfAndDownload = new Button();
            label1 = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            comboBoxEpsg = new ComboBox();
            labelProgress = new Label();
            groupBox3 = new GroupBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox2.Controls.Add(this.comboBoxResolution);
            groupBox3.SuspendLayout();
            SuspendLayout();
            // 
            // basemapFolderListBox
            // 
            basemapFolderListBox.FormattingEnabled = true;
            basemapFolderListBox.ItemHeight = 15;
            basemapFolderListBox.Location = new Point(6, 41);
            basemapFolderListBox.Name = "basemapFolderListBox";
            basemapFolderListBox.Size = new Size(169, 154);
            basemapFolderListBox.TabIndex = 2;
            basemapFolderListBox.SelectedIndexChanged += basemapFolderListBox_SelectedIndexChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(6, 23);
            label10.Name = "label10";
            label10.Size = new Size(80, 15);
            label10.TabIndex = 3;
            label10.Text = "Twoje foldery:";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 281);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(608, 23);
            progressBar1.TabIndex = 4;
            // 
            // comboBoxMapType
            // 
            comboBoxMapType.DropDownWidth = 200;
            comboBoxMapType.FormattingEnabled = true;
            comboBoxMapType.Location = new Point(130, 58);
            comboBoxMapType.Name = "comboBoxMapType";
            comboBoxMapType.Size = new Size(142, 23);
            comboBoxMapType.TabIndex = 41;
            comboBoxMapType.SelectedIndexChanged += comboBoxMapType_SelectedIndexChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(130, 38);
            label15.Name = "label15";
            label15.Size = new Size(107, 15);
            label15.TabIndex = 40;
            label15.Text = "Rodzaj podkładów:";
            // 
            // textBoxBasemapDate
            // 
            textBoxBasemapDate.Location = new Point(303, 58);
            textBoxBasemapDate.MaxLength = 4;
            textBoxBasemapDate.Name = "textBoxBasemapDate";
            textBoxBasemapDate.Size = new Size(75, 23);
            textBoxBasemapDate.TabIndex = 39;
            textBoxBasemapDate.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(303, 23);
            label14.Name = "label14";
            label14.Size = new Size(69, 30);
            label14.TabIndex = 38;
            label14.Text = "Rok\r\npodkładów:";
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 23);
            label2.Name = "label2";
            label2.Size = new Size(93, 30);
            label2.TabIndex = 33;
            label2.Text = "Rozdzielczość\r\npodkładów [px]:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(303, 106);
            label13.Name = "label13";
            label13.Size = new Size(87, 15);
            label13.TabIndex = 45;
            label13.Text = "Ozn. podkładu:";
            // 
            // textBoxDesignation
            // 
            textBoxDesignation.Location = new Point(303, 124);
            textBoxDesignation.Name = "textBoxDesignation";
            textBoxDesignation.Size = new Size(75, 23);
            textBoxDesignation.TabIndex = 44;
            // 
            // textBoxDestinationFolder
            // 
            textBoxDestinationFolder.Location = new Point(130, 124);
            textBoxDestinationFolder.Name = "textBoxDestinationFolder";
            textBoxDestinationFolder.Size = new Size(142, 23);
            textBoxDestinationFolder.TabIndex = 42;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(128, 106);
            label4.Name = "label4";
            label4.Size = new Size(154, 15);
            label4.TabIndex = 43;
            label4.Text = "Nazwa docelowego folderu:";
            // 
            // buttonConfAndDownload
            // 
            buttonConfAndDownload.Location = new Point(222, 312);
            buttonConfAndDownload.Name = "buttonConfAndDownload";
            buttonConfAndDownload.Size = new Size(188, 23);
            buttonConfAndDownload.TabIndex = 46;
            buttonConfAndDownload.Text = "Przetwórz seryjnie";
            buttonConfAndDownload.UseVisualStyleBackColor = true;
            buttonConfAndDownload.Click += buttonConfAndDownload_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label1.Location = new Point(12, 9);
            label1.Name = "label1";
            label1.Size = new Size(225, 30);
            label1.TabIndex = 47;
            label1.Text = "Przetwarzanie seryjne:";
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(basemapFolderListBox);
            groupBox1.Controls.Add(label10);
            groupBox1.Location = new Point(12, 56);
            groupBox1.Name = "groupBox1";
            groupBox1.Size = new Size(207, 215);
            groupBox1.TabIndex = 48;
            groupBox1.TabStop = false;
            groupBox1.Text = "1. Wybierz folder do przetworzenia";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(textBoxDesignation);
            groupBox2.Controls.Add(label14);
            groupBox2.Controls.Add(textBoxDestinationFolder);
            groupBox2.Controls.Add(textBoxBasemapDate);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label15);
            groupBox2.Controls.Add(comboBoxMapType);
            groupBox2.Location = new Point(225, 111);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(395, 160);
            groupBox2.TabIndex = 49;
            groupBox2.TabStop = false;
            groupBox2.Text = "3. Ustaw parametry docelowe";
            // 
            // comboBoxEpsg
            // 
            comboBoxEpsg.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEpsg.FormattingEnabled = true;
            comboBoxEpsg.Items.AddRange(new object[] { "EPSG:2180 (Polska)", "EPSG:3857 (Świat)" });
            comboBoxEpsg.Location = new Point(13, 21);
            comboBoxEpsg.Name = "comboBoxEpsg";
            comboBoxEpsg.Size = new Size(188, 23);
            comboBoxEpsg.TabIndex = 47;
            // 
            // labelProgress
            // 
            labelProgress.AutoSize = true;
            labelProgress.Location = new Point(18, 329);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(82, 15);
            labelProgress.TabIndex = 50;
            labelProgress.Text = "Przetworzono:";
            labelProgress.Visible = false;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(comboBoxEpsg);
            groupBox3.Location = new Point(225, 56);
            groupBox3.Name = "groupBox3";
            groupBox3.Size = new Size(395, 49);
            groupBox3.TabIndex = 51;
            groupBox3.TabStop = false;
            groupBox3.Text = "2. Wybierz układ współrzędnych";
            // 
            // BatchToolForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(635, 372);
            Controls.Add(groupBox3);
            Controls.Add(labelProgress);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Controls.Add(label1);
            Controls.Add(buttonConfAndDownload);
            Controls.Add(progressBar1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "BatchToolForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Przetwarzanie seryjne";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
                        // 
            // comboBoxResolution
            // 
            this.comboBoxResolution.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxResolution.FormattingEnabled = true;
            this.comboBoxResolution.Location = new System.Drawing.Point(13, 56);
            this.comboBoxResolution.Name = "comboBoxResolution";
            this.comboBoxResolution.Size = new System.Drawing.Size(120, 23);
            this.comboBoxResolution.TabIndex = 20;
            this.comboBoxResolution.SelectedIndexChanged += new System.EventHandler(this.comboBoxResolution_SelectedIndexChanged);
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox basemapFolderListBox;
        private Label label10;
        private ProgressBar progressBar1;
        private ComboBox comboBoxMapType;
        private System.Windows.Forms.ComboBox comboBoxResolution;
        private Label label15;
        private TextBox textBoxBasemapDate;
        private Label label14;
        private Label label2;
        private Label label13;
        private TextBox textBoxDesignation;
        private TextBox textBoxDestinationFolder;
        private Label label4;
        private Button buttonConfAndDownload;
        private Label label1;
        private GroupBox groupBox1;
        private GroupBox groupBox2;
        private Label labelProgress;
        private ComboBox comboBoxEpsg;
        private GroupBox groupBox3;
    }
}