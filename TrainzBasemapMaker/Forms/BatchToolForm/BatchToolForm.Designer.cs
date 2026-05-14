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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BatchToolForm));
            basemapFolderListBox = new ListBox();
            label10 = new Label();
            progressBar1 = new ProgressBar();
            comboBoxMapType = new ComboBox();
            label15 = new Label();
            textBoxBasemapDate = new TextBox();
            label14 = new Label();
            radioButton512 = new RadioButton();
            radioButton1024 = new RadioButton();
            radioButton2048 = new RadioButton();
            radioButton4096 = new RadioButton();
            label2 = new Label();
            label13 = new Label();
            textBoxDesignation = new TextBox();
            textBoxDestinationFolder = new TextBox();
            label4 = new Label();
            buttonConfAndDownload = new Button();
            label1 = new Label();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            labelProgress = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // basemapFolderListBox
            // 
            basemapFolderListBox.FormattingEnabled = true;
            basemapFolderListBox.ItemHeight = 15;
            basemapFolderListBox.Location = new Point(6, 41);
            basemapFolderListBox.Name = "basemapFolderListBox";
            basemapFolderListBox.Size = new Size(169, 109);
            basemapFolderListBox.TabIndex = 2;
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
            progressBar1.Location = new Point(18, 238);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(602, 23);
            progressBar1.TabIndex = 4;
            // 
            // comboBoxMapType
            // 
            comboBoxMapType.DropDownWidth = 100;
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
            // radioButton512
            // 
            radioButton512.AutoSize = true;
            radioButton512.Location = new Point(13, 131);
            radioButton512.Name = "radioButton512";
            radioButton512.Size = new Size(73, 19);
            radioButton512.TabIndex = 37;
            radioButton512.TabStop = true;
            radioButton512.Text = "512 x 512";
            radioButton512.UseVisualStyleBackColor = true;
            // 
            // radioButton1024
            // 
            radioButton1024.AutoSize = true;
            radioButton1024.Location = new Point(13, 106);
            radioButton1024.Name = "radioButton1024";
            radioButton1024.Size = new Size(85, 19);
            radioButton1024.TabIndex = 36;
            radioButton1024.TabStop = true;
            radioButton1024.Text = "1024 x 1024";
            radioButton1024.UseVisualStyleBackColor = true;
            // 
            // radioButton2048
            // 
            radioButton2048.AutoSize = true;
            radioButton2048.Location = new Point(13, 81);
            radioButton2048.Name = "radioButton2048";
            radioButton2048.Size = new Size(85, 19);
            radioButton2048.TabIndex = 35;
            radioButton2048.TabStop = true;
            radioButton2048.Text = "2048 x 2048";
            radioButton2048.UseVisualStyleBackColor = true;
            // 
            // radioButton4096
            // 
            radioButton4096.AutoSize = true;
            radioButton4096.Location = new Point(13, 56);
            radioButton4096.Name = "radioButton4096";
            radioButton4096.Size = new Size(85, 19);
            radioButton4096.TabIndex = 34;
            radioButton4096.TabStop = true;
            radioButton4096.Text = "4096 x 4096";
            radioButton4096.UseVisualStyleBackColor = true;
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
            buttonConfAndDownload.Location = new Point(213, 279);
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
            groupBox1.Size = new Size(207, 163);
            groupBox1.TabIndex = 48;
            groupBox1.TabStop = false;
            groupBox1.Text = "1. Wybierz folder do przetworzenia";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(label2);
            groupBox2.Controls.Add(radioButton4096);
            groupBox2.Controls.Add(radioButton2048);
            groupBox2.Controls.Add(radioButton1024);
            groupBox2.Controls.Add(label13);
            groupBox2.Controls.Add(radioButton512);
            groupBox2.Controls.Add(textBoxDesignation);
            groupBox2.Controls.Add(label14);
            groupBox2.Controls.Add(textBoxDestinationFolder);
            groupBox2.Controls.Add(textBoxBasemapDate);
            groupBox2.Controls.Add(label4);
            groupBox2.Controls.Add(label15);
            groupBox2.Controls.Add(comboBoxMapType);
            groupBox2.Location = new Point(225, 56);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(395, 163);
            groupBox2.TabIndex = 49;
            groupBox2.TabStop = false;
            groupBox2.Text = "2. Ustaw parametry docelowe";
            // 
            // labelProgress
            // 
            labelProgress.AutoSize = true;
            labelProgress.Location = new Point(18, 279);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(82, 15);
            labelProgress.TabIndex = 50;
            labelProgress.Text = "Przetworzono:";
            labelProgress.Visible = false;
            // 
            // BatchToolForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(635, 322);
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
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListBox basemapFolderListBox;
        private Label label10;
        private ProgressBar progressBar1;
        private ComboBox comboBoxMapType;
        private Label label15;
        private TextBox textBoxBasemapDate;
        private Label label14;
        private RadioButton radioButton512;
        private RadioButton radioButton1024;
        private RadioButton radioButton2048;
        private RadioButton radioButton4096;
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
    }
}