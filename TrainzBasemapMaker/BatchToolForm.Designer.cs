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
            SuspendLayout();
            // 
            // basemapFolderListBox
            // 
            basemapFolderListBox.FormattingEnabled = true;
            basemapFolderListBox.ItemHeight = 15;
            basemapFolderListBox.Location = new Point(12, 67);
            basemapFolderListBox.Name = "basemapFolderListBox";
            basemapFolderListBox.Size = new Size(169, 94);
            basemapFolderListBox.TabIndex = 2;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(12, 49);
            label10.Name = "label10";
            label10.Size = new Size(80, 15);
            label10.TabIndex = 3;
            label10.Text = "Twoje foldery:";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(12, 194);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(169, 23);
            progressBar1.TabIndex = 4;
            // 
            // comboBoxMapType
            // 
            comboBoxMapType.DropDownWidth = 100;
            comboBoxMapType.FormattingEnabled = true;
            comboBoxMapType.Location = new Point(405, 67);
            comboBoxMapType.Name = "comboBoxMapType";
            comboBoxMapType.Size = new Size(67, 23);
            comboBoxMapType.TabIndex = 41;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(405, 34);
            label15.Name = "label15";
            label15.Size = new Size(69, 30);
            label15.TabIndex = 40;
            label15.Text = "Rodzaj\r\npodkładów:";
            // 
            // textBoxBasemapDate
            // 
            textBoxBasemapDate.Location = new Point(405, 141);
            textBoxBasemapDate.Name = "textBoxBasemapDate";
            textBoxBasemapDate.Size = new Size(67, 23);
            textBoxBasemapDate.TabIndex = 39;
            textBoxBasemapDate.TextAlign = HorizontalAlignment.Center;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(405, 106);
            label14.Name = "label14";
            label14.Size = new Size(69, 30);
            label14.TabIndex = 38;
            label14.Text = "Rok\r\npodkładów:";
            // 
            // radioButton512
            // 
            radioButton512.AutoSize = true;
            radioButton512.Location = new Point(300, 142);
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
            radioButton1024.Location = new Point(300, 117);
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
            radioButton2048.Location = new Point(300, 92);
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
            radioButton4096.Location = new Point(300, 67);
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
            label2.Location = new Point(293, 34);
            label2.Name = "label2";
            label2.Size = new Size(93, 30);
            label2.TabIndex = 33;
            label2.Text = "Rozdzielczość\r\npodkładów [px]:";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(293, 242);
            label13.Name = "label13";
            label13.Size = new Size(87, 15);
            label13.TabIndex = 45;
            label13.Text = "Ozn. podkładu:";
            // 
            // textBoxDesignation
            // 
            textBoxDesignation.Location = new Point(293, 260);
            textBoxDesignation.Name = "textBoxDesignation";
            textBoxDesignation.Size = new Size(75, 23);
            textBoxDesignation.TabIndex = 44;
            // 
            // textBoxDestinationFolder
            // 
            textBoxDestinationFolder.Location = new Point(293, 212);
            textBoxDestinationFolder.Name = "textBoxDestinationFolder";
            textBoxDestinationFolder.Size = new Size(169, 23);
            textBoxDestinationFolder.TabIndex = 42;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(293, 194);
            label4.Name = "label4";
            label4.Size = new Size(154, 15);
            label4.TabIndex = 43;
            label4.Text = "Nazwa docelowego folderu:";
            // 
            // buttonConfAndDownload
            // 
            buttonConfAndDownload.Location = new Point(286, 349);
            buttonConfAndDownload.Name = "buttonConfAndDownload";
            buttonConfAndDownload.Size = new Size(188, 23);
            buttonConfAndDownload.TabIndex = 46;
            buttonConfAndDownload.Text = "Przetwórz seryjnie";
            buttonConfAndDownload.UseVisualStyleBackColor = true;
            buttonConfAndDownload.Click += buttonConfAndDownload_Click;
            // 
            // BatchToolForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(buttonConfAndDownload);
            Controls.Add(label13);
            Controls.Add(textBoxDesignation);
            Controls.Add(textBoxDestinationFolder);
            Controls.Add(label4);
            Controls.Add(comboBoxMapType);
            Controls.Add(label15);
            Controls.Add(textBoxBasemapDate);
            Controls.Add(label14);
            Controls.Add(radioButton512);
            Controls.Add(radioButton1024);
            Controls.Add(radioButton2048);
            Controls.Add(radioButton4096);
            Controls.Add(label2);
            Controls.Add(progressBar1);
            Controls.Add(label10);
            Controls.Add(basemapFolderListBox);
            Icon = (Icon)resources.GetObject("$this.Icon");
            Name = "BatchToolForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Przetwarzanie seryjne";
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
    }
}