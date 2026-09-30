namespace TrainzBasemapMaker
{
    partial class SingleBasemapForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SingleBasemapForm));
            comboBoxResolution = new ComboBox();
            textBoxLat = new TextBox();
            buttonConfAndDownload = new Button();
            textBoxLon = new TextBox();
            buttonUp = new Button();
            buttonLeft = new Button();
            buttonRight = new Button();
            buttonDown = new Button();
            label2 = new Label();
            groupBox3Configurator = new GroupBox();
            textBoxX = new TextBox();
            label9 = new Label();
            panel1 = new Panel();
            textBoxKuidPart2 = new TextBox();
            label13 = new Label();
            textBoxDesignation = new TextBox();
            labelKuidSeparator = new Label();
            textBoxKuidPart1 = new TextBox();
            label12 = new Label();
            textBoxCounter = new TextBox();
            label11 = new Label();
            label10 = new Label();
            basemapFolderListBox = new ListBox();
            textBoxDestinationFolder = new TextBox();
            label4 = new Label();
            textBoxY = new TextBox();
            checkBoxCreateFiles = new CheckBox();
            label8 = new Label();
            label7 = new Label();
            comboBoxMapType = new ComboBox();
            label15 = new Label();
            textBoxBasemapDate = new TextBox();
            label14 = new Label();
            buttonConvert = new Button();
            groupBox5Navigator = new GroupBox();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            label5 = new Label();
            label6 = new Label();
            groupBox1GeoCoords = new GroupBox();
            buttonMarkPointMap = new Button();
            comboBoxEpsg = new ComboBox();
            kuidsInFolderListBox = new ListBox();
            groupBox7BasemapViewer = new GroupBox();
            groupBox6KuidList = new GroupBox();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            groupBox2TargetCoords = new GroupBox();
            groupBox4BasemapParams = new GroupBox();
            groupBox3Configurator.SuspendLayout();
            panel1.SuspendLayout();
            groupBox5Navigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1GeoCoords.SuspendLayout();
            groupBox7BasemapViewer.SuspendLayout();
            groupBox6KuidList.SuspendLayout();
            statusStrip1.SuspendLayout();
            groupBox2TargetCoords.SuspendLayout();
            groupBox4BasemapParams.SuspendLayout();
            SuspendLayout();
            // 
            // comboBoxResolution
            // 
            comboBoxResolution.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxResolution.FormattingEnabled = true;
            comboBoxResolution.Location = new Point(6, 50);
            comboBoxResolution.Name = "comboBoxResolution";
            comboBoxResolution.Size = new Size(103, 23);
            comboBoxResolution.TabIndex = 20;
            comboBoxResolution.SelectedIndexChanged += comboBoxResolution_SelectedIndexChanged;
            // 
            // textBoxLat
            // 
            textBoxLat.Location = new Point(6, 50);
            textBoxLat.Name = "textBoxLat";
            textBoxLat.Size = new Size(114, 23);
            textBoxLat.TabIndex = 1;
            // 
            // buttonConfAndDownload
            // 
            buttonConfAndDownload.Location = new Point(6, 415);
            buttonConfAndDownload.Name = "buttonConfAndDownload";
            buttonConfAndDownload.Size = new Size(188, 23);
            buttonConfAndDownload.TabIndex = 2;
            buttonConfAndDownload.Text = "Skonfiguruj i pobierz";
            buttonConfAndDownload.UseVisualStyleBackColor = true;
            buttonConfAndDownload.Click += buttonConfAndDownload_Click;
            // 
            // textBoxLon
            // 
            textBoxLon.Location = new Point(6, 79);
            textBoxLon.Name = "textBoxLon";
            textBoxLon.Size = new Size(114, 23);
            textBoxLon.TabIndex = 3;
            // 
            // buttonUp
            // 
            buttonUp.Location = new Point(70, 53);
            buttonUp.Name = "buttonUp";
            buttonUp.Size = new Size(80, 23);
            buttonUp.TabIndex = 4;
            buttonUp.Text = "W górę";
            buttonUp.UseVisualStyleBackColor = true;
            buttonUp.Click += buttonUp_Click;
            // 
            // buttonLeft
            // 
            buttonLeft.Location = new Point(27, 82);
            buttonLeft.Name = "buttonLeft";
            buttonLeft.Size = new Size(80, 23);
            buttonLeft.TabIndex = 5;
            buttonLeft.Text = "W lewo";
            buttonLeft.UseVisualStyleBackColor = true;
            buttonLeft.Click += buttonLeft_Click;
            // 
            // buttonRight
            // 
            buttonRight.Location = new Point(113, 82);
            buttonRight.Name = "buttonRight";
            buttonRight.Size = new Size(80, 23);
            buttonRight.TabIndex = 6;
            buttonRight.Text = "W prawo";
            buttonRight.UseVisualStyleBackColor = true;
            buttonRight.Click += buttonRight_Click;
            // 
            // buttonDown
            // 
            buttonDown.Location = new Point(70, 111);
            buttonDown.Name = "buttonDown";
            buttonDown.Size = new Size(80, 23);
            buttonDown.TabIndex = 7;
            buttonDown.Text = "W dół";
            buttonDown.UseVisualStyleBackColor = true;
            buttonDown.Click += buttonDown_Click;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 25);
            label2.Name = "label2";
            label2.Size = new Size(106, 15);
            label2.TabIndex = 9;
            label2.Text = "Rozdzielczość [px]:";
            // 
            // groupBox3Configurator
            // 
            groupBox3Configurator.Controls.Add(textBoxX);
            groupBox3Configurator.Controls.Add(label9);
            groupBox3Configurator.Controls.Add(panel1);
            groupBox3Configurator.Controls.Add(textBoxY);
            groupBox3Configurator.Controls.Add(checkBoxCreateFiles);
            groupBox3Configurator.Controls.Add(label8);
            groupBox3Configurator.Controls.Add(label7);
            groupBox3Configurator.Controls.Add(buttonConfAndDownload);
            groupBox3Configurator.Location = new Point(12, 228);
            groupBox3Configurator.Name = "groupBox3Configurator";
            groupBox3Configurator.Size = new Size(214, 455);
            groupBox3Configurator.TabIndex = 11;
            groupBox3Configurator.TabStop = false;
            groupBox3Configurator.Text = "2. Konfiguracja pobieranych i tworzonych podkładów";
            // 
            // textBoxX
            // 
            textBoxX.Location = new Point(6, 64);
            textBoxX.Name = "textBoxX";
            textBoxX.Size = new Size(100, 23);
            textBoxX.TabIndex = 11;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(112, 66);
            label9.Name = "label9";
            label9.Size = new Size(82, 15);
            label9.TabIndex = 16;
            label9.Text = "szerokość (lat)";
            // 
            // panel1
            // 
            panel1.Controls.Add(textBoxKuidPart2);
            panel1.Controls.Add(label13);
            panel1.Controls.Add(textBoxDesignation);
            panel1.Controls.Add(labelKuidSeparator);
            panel1.Controls.Add(textBoxKuidPart1);
            panel1.Controls.Add(label12);
            panel1.Controls.Add(textBoxCounter);
            panel1.Controls.Add(label11);
            panel1.Controls.Add(label10);
            panel1.Controls.Add(basemapFolderListBox);
            panel1.Controls.Add(textBoxDestinationFolder);
            panel1.Controls.Add(label4);
            panel1.Location = new Point(11, 146);
            panel1.Name = "panel1";
            panel1.Size = new Size(178, 263);
            panel1.TabIndex = 20;
            // 
            // textBoxKuidPart2
            // 
            textBoxKuidPart2.Location = new Point(97, 236);
            textBoxKuidPart2.Name = "textBoxKuidPart2";
            textBoxKuidPart2.Size = new Size(75, 23);
            textBoxKuidPart2.TabIndex = 29;
            textBoxKuidPart2.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(3, 170);
            label13.Name = "label13";
            label13.Size = new Size(87, 15);
            label13.TabIndex = 28;
            label13.Text = "Ozn. podkładu:";
            // 
            // textBoxDesignation
            // 
            textBoxDesignation.Location = new Point(3, 188);
            textBoxDesignation.Name = "textBoxDesignation";
            textBoxDesignation.Size = new Size(75, 23);
            textBoxDesignation.TabIndex = 27;
            textBoxDesignation.TextChanged += textBoxDesignation_TextChanged;
            // 
            // labelKuidSeparator
            // 
            labelKuidSeparator.AutoSize = true;
            labelKuidSeparator.Location = new Point(84, 239);
            labelKuidSeparator.Name = "labelKuidSeparator";
            labelKuidSeparator.Size = new Size(10, 15);
            labelKuidSeparator.TabIndex = 26;
            labelKuidSeparator.Text = ":";
            // 
            // textBoxKuidPart1
            // 
            textBoxKuidPart1.Location = new Point(3, 236);
            textBoxKuidPart1.Name = "textBoxKuidPart1";
            textBoxKuidPart1.Size = new Size(75, 23);
            textBoxKuidPart1.TabIndex = 25;
            textBoxKuidPart1.TextAlign = HorizontalAlignment.Right;
            textBoxKuidPart1.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(3, 218);
            label12.Name = "label12";
            label12.Size = new Size(111, 15);
            label12.TabIndex = 24;
            label12.Text = "Oznaczenie KUID-u:";
            // 
            // textBoxCounter
            // 
            textBoxCounter.Location = new Point(97, 188);
            textBoxCounter.Name = "textBoxCounter";
            textBoxCounter.Size = new Size(75, 23);
            textBoxCounter.TabIndex = 23;
            textBoxCounter.TextChanged += textBoxCounter_TextChanged;
            textBoxCounter.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(97, 170);
            label11.Name = "label11";
            label11.Size = new Size(76, 15);
            label11.TabIndex = 22;
            label11.Text = "Nr podkładu:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(3, 0);
            label10.Name = "label10";
            label10.Size = new Size(80, 15);
            label10.TabIndex = 0;
            label10.Text = "Twoje foldery:";
            // 
            // basemapFolderListBox
            // 
            basemapFolderListBox.FormattingEnabled = true;
            basemapFolderListBox.ItemHeight = 15;
            basemapFolderListBox.Location = new Point(3, 18);
            basemapFolderListBox.Name = "basemapFolderListBox";
            basemapFolderListBox.Size = new Size(169, 94);
            basemapFolderListBox.TabIndex = 1;
            basemapFolderListBox.Click += basemapFolderListBox_Click;
            // 
            // textBoxDestinationFolder
            // 
            textBoxDestinationFolder.Location = new Point(3, 140);
            textBoxDestinationFolder.Name = "textBoxDestinationFolder";
            textBoxDestinationFolder.Size = new Size(169, 23);
            textBoxDestinationFolder.TabIndex = 20;
            textBoxDestinationFolder.TextChanged += textBoxDestinationFolder_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 122);
            label4.Name = "label4";
            label4.Size = new Size(154, 15);
            label4.TabIndex = 21;
            label4.Text = "Nazwa docelowego folderu:";
            // 
            // textBoxY
            // 
            textBoxY.Location = new Point(6, 92);
            textBoxY.Name = "textBoxY";
            textBoxY.Size = new Size(100, 23);
            textBoxY.TabIndex = 12;
            // 
            // checkBoxCreateFiles
            // 
            checkBoxCreateFiles.AutoSize = true;
            checkBoxCreateFiles.Location = new Point(6, 121);
            checkBoxCreateFiles.Name = "checkBoxCreateFiles";
            checkBoxCreateFiles.Size = new Size(160, 19);
            checkBoxCreateFiles.TabIndex = 18;
            checkBoxCreateFiles.Text = "Twórz foldery i pliki Trainz";
            checkBoxCreateFiles.UseVisualStyleBackColor = true;
            checkBoxCreateFiles.CheckedChanged += checkBoxCreateFiles_CheckedChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(112, 95);
            label8.Name = "label8";
            label8.Size = new Size(77, 15);
            label8.TabIndex = 17;
            label8.Text = "długość (lon)";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 46);
            label7.Name = "label7";
            label7.Size = new Size(132, 15);
            label7.TabIndex = 16;
            label7.Text = "Współrzędne EPSG:2180";
            // 
            // comboBoxMapType
            // 
            comboBoxMapType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBoxMapType.DropDownWidth = 310;
            comboBoxMapType.FormattingEnabled = true;
            comboBoxMapType.Location = new Point(125, 50);
            comboBoxMapType.Name = "comboBoxMapType";
            comboBoxMapType.Size = new Size(307, 23);
            comboBoxMapType.TabIndex = 32;
            comboBoxMapType.SelectedIndexChanged += comboBoxMapType_SelectedIndexChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(125, 25);
            label15.Name = "label15";
            label15.Size = new Size(107, 15);
            label15.TabIndex = 31;
            label15.Text = "Rodzaj podkładów:";
            // 
            // textBoxBasemapDate
            // 
            textBoxBasemapDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxBasemapDate.Location = new Point(448, 50);
            textBoxBasemapDate.MaxLength = 4;
            textBoxBasemapDate.Name = "textBoxBasemapDate";
            textBoxBasemapDate.Size = new Size(67, 23);
            textBoxBasemapDate.TabIndex = 30;
            textBoxBasemapDate.TextAlign = HorizontalAlignment.Center;
            textBoxBasemapDate.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label14
            // 
            label14.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            label14.AutoSize = true;
            label14.Location = new Point(448, 29);
            label14.Name = "label14";
            label14.Size = new Size(92, 15);
            label14.TabIndex = 21;
            label14.Text = "Rok podkładów:";
            // 
            // buttonConvert
            // 
            buttonConvert.Location = new Point(6, 51);
            buttonConvert.Name = "buttonConvert";
            buttonConvert.Size = new Size(188, 23);
            buttonConvert.TabIndex = 13;
            buttonConvert.Text = "Konwertuj na EPSG:2180";
            buttonConvert.UseVisualStyleBackColor = true;
            buttonConvert.Click += buttonConvert_Click;
            // 
            // groupBox5Navigator
            // 
            groupBox5Navigator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox5Navigator.Controls.Add(label3);
            groupBox5Navigator.Controls.Add(buttonLeft);
            groupBox5Navigator.Controls.Add(buttonUp);
            groupBox5Navigator.Controls.Add(buttonDown);
            groupBox5Navigator.Controls.Add(buttonRight);
            groupBox5Navigator.Location = new Point(818, 12);
            groupBox5Navigator.Name = "groupBox5Navigator";
            groupBox5Navigator.Size = new Size(217, 159);
            groupBox5Navigator.TabIndex = 12;
            groupBox5Navigator.TabStop = false;
            groupBox5Navigator.Text = "5. Nawigacja";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(7, 21);
            label3.Name = "label3";
            label3.Size = new Size(102, 15);
            label3.TabIndex = 11;
            label3.Text = "Nawiguj i pobierz:";
            // 
            // pictureBox1
            // 
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.Location = new Point(6, 28);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(568, 535);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(126, 53);
            label5.Name = "label5";
            label5.Size = new Size(82, 15);
            label5.TabIndex = 14;
            label5.Text = "szerokość (lat)";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(126, 82);
            label6.Name = "label6";
            label6.Size = new Size(77, 15);
            label6.TabIndex = 15;
            label6.Text = "długość (lon)";
            // 
            // groupBox1GeoCoords
            // 
            groupBox1GeoCoords.Controls.Add(buttonMarkPointMap);
            groupBox1GeoCoords.Controls.Add(label6);
            groupBox1GeoCoords.Controls.Add(label5);
            groupBox1GeoCoords.Controls.Add(textBoxLon);
            groupBox1GeoCoords.Controls.Add(textBoxLat);
            groupBox1GeoCoords.Location = new Point(12, 12);
            groupBox1GeoCoords.Name = "groupBox1GeoCoords";
            groupBox1GeoCoords.Size = new Size(214, 115);
            groupBox1GeoCoords.TabIndex = 15;
            groupBox1GeoCoords.TabStop = false;
            groupBox1GeoCoords.Text = "1. Współrzędne geograficzne (DD)";
            // 
            // buttonMarkPointMap
            // 
            buttonMarkPointMap.Location = new Point(6, 21);
            buttonMarkPointMap.Name = "buttonMarkPointMap";
            buttonMarkPointMap.Size = new Size(202, 23);
            buttonMarkPointMap.TabIndex = 16;
            buttonMarkPointMap.Text = "Wybierz na mapie";
            buttonMarkPointMap.UseVisualStyleBackColor = true;
            buttonMarkPointMap.Click += buttonMarkPointMap_Click;
            // 
            // comboBoxEpsg
            // 
            comboBoxEpsg.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEpsg.FormattingEnabled = true;
            comboBoxEpsg.Items.AddRange(new object[] { "EPSG:2180 (Polska)", "EPSG:3857 (Świat)" });
            comboBoxEpsg.Location = new Point(6, 22);
            comboBoxEpsg.Name = "comboBoxEpsg";
            comboBoxEpsg.Size = new Size(188, 23);
            comboBoxEpsg.TabIndex = 33;
            comboBoxEpsg.SelectedIndexChanged += ComboBoxEpsg_SelectedIndexChanged;
            // 
            // kuidsInFolderListBox
            // 
            kuidsInFolderListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            kuidsInFolderListBox.FormattingEnabled = true;
            kuidsInFolderListBox.ItemHeight = 15;
            kuidsInFolderListBox.Location = new Point(6, 21);
            kuidsInFolderListBox.Name = "kuidsInFolderListBox";
            kuidsInFolderListBox.Size = new Size(205, 469);
            kuidsInFolderListBox.TabIndex = 16;
            kuidsInFolderListBox.SelectedIndexChanged += kuidsInFolderListBox_SelectedIndexChanged;
            kuidsInFolderListBox.DoubleClick += kuidsInFolderListBox_DoubleClick;
            // 
            // groupBox7BasemapViewer
            // 
            groupBox7BasemapViewer.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBox7BasemapViewer.Controls.Add(pictureBox1);
            groupBox7BasemapViewer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            groupBox7BasemapViewer.Location = new Point(232, 103);
            groupBox7BasemapViewer.Name = "groupBox7BasemapViewer";
            groupBox7BasemapViewer.Size = new Size(580, 580);
            groupBox7BasemapViewer.TabIndex = 17;
            groupBox7BasemapViewer.TabStop = false;
            groupBox7BasemapViewer.Text = "Podgląd pobranego podkładu:";
            // 
            // groupBox6KuidList
            // 
            groupBox6KuidList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Right;
            groupBox6KuidList.Controls.Add(kuidsInFolderListBox);
            groupBox6KuidList.Location = new Point(818, 177);
            groupBox6KuidList.Name = "groupBox6KuidList";
            groupBox6KuidList.Size = new Size(217, 506);
            groupBox6KuidList.TabIndex = 18;
            groupBox6KuidList.TabStop = false;
            groupBox6KuidList.Text = "Lista podkładów do Trainz:";
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 708);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1047, 22);
            statusStrip1.TabIndex = 20;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Margin = new Padding(10, 3, 0, 2);
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 17);
            // 
            // groupBox2TargetCoords
            // 
            groupBox2TargetCoords.Controls.Add(comboBoxEpsg);
            groupBox2TargetCoords.Controls.Add(buttonConvert);
            groupBox2TargetCoords.Location = new Point(12, 133);
            groupBox2TargetCoords.Name = "groupBox2TargetCoords";
            groupBox2TargetCoords.Size = new Size(214, 89);
            groupBox2TargetCoords.TabIndex = 21;
            groupBox2TargetCoords.TabStop = false;
            groupBox2TargetCoords.Text = "2. Docelowy format współrzędnych";
            // 
            // groupBox4BasemapParams
            // 
            groupBox4BasemapParams.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox4BasemapParams.Controls.Add(comboBoxMapType);
            groupBox4BasemapParams.Controls.Add(textBoxBasemapDate);
            groupBox4BasemapParams.Controls.Add(comboBoxResolution);
            groupBox4BasemapParams.Controls.Add(label15);
            groupBox4BasemapParams.Controls.Add(label14);
            groupBox4BasemapParams.Controls.Add(label2);
            groupBox4BasemapParams.Location = new Point(232, 12);
            groupBox4BasemapParams.Name = "groupBox4BasemapParams";
            groupBox4BasemapParams.Size = new Size(580, 85);
            groupBox4BasemapParams.TabIndex = 22;
            groupBox4BasemapParams.TabStop = false;
            groupBox4BasemapParams.Text = "4. Parametry podkładów satelitarnych (basemap-ów)";
            // 
            // SingleBasemapForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1047, 730);
            Controls.Add(groupBox4BasemapParams);
            Controls.Add(groupBox2TargetCoords);
            Controls.Add(statusStrip1);
            Controls.Add(groupBox6KuidList);
            Controls.Add(groupBox7BasemapViewer);
            Controls.Add(groupBox1GeoCoords);
            Controls.Add(groupBox5Navigator);
            Controls.Add(groupBox3Configurator);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1063, 769);
            Name = "SingleBasemapForm";
            Text = "Trainz Basemap Maker";
            groupBox3Configurator.ResumeLayout(false);
            groupBox3Configurator.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox5Navigator.ResumeLayout(false);
            groupBox5Navigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1GeoCoords.ResumeLayout(false);
            groupBox1GeoCoords.PerformLayout();
            groupBox7BasemapViewer.ResumeLayout(false);
            groupBox6KuidList.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            groupBox2TargetCoords.ResumeLayout(false);
            groupBox4BasemapParams.ResumeLayout(false);
            groupBox4BasemapParams.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private TextBox textBoxLat;
        private Button buttonConfAndDownload;
        private TextBox textBoxLon;
        private Button buttonUp;
        private Button buttonLeft;
        private Button buttonRight;
        private Button buttonDown;
        private Label label2;
        private GroupBox groupBox3Configurator;
        private GroupBox groupBox5Navigator;
        private Label label3;
        private PictureBox pictureBox1;
        private TextBox textBoxY;
        private TextBox textBoxX;
        private Button buttonConvert;
        private Label label6;
        private Label label5;
        private GroupBox groupBox1GeoCoords;
        private Label label8;
        private Label label7;
        private Label label9;
        private CheckBox checkBoxCreateFiles;
        private ListBox kuidsInFolderListBox;
        private GroupBox groupBox7BasemapViewer;
        private GroupBox groupBox6KuidList;
        private Label label4;
        private TextBox textBoxDestinationFolder;
        private Label label10;
        private ListBox basemapFolderListBox;
        private Panel panel1;
        private TextBox textBoxCounter;
        private Label label11;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private TextBox textBoxKuidPart1;
        private Label label12;
        private Label labelKuidSeparator;
        private TextBox textBoxDesignation;
        private Label label13;
        private TextBox textBoxKuidPart2;
        private TextBox textBoxBasemapDate;
        private Label label14;
        private Label label15;
        private ComboBox comboBoxMapType;
        private System.Windows.Forms.ComboBox comboBoxResolution;
        private Button buttonMarkPointMap;
        private ComboBox comboBoxEpsg;
        private GroupBox groupBox2TargetCoords;
        private GroupBox groupBox4BasemapParams;
    }
}
