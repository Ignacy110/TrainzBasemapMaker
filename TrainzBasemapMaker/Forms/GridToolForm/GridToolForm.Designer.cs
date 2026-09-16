namespace TrainzBasemapMaker
{
    partial class GridToolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GridToolForm));
            groupBox1Selection = new GroupBox();
            labelArea = new Label();
            labelTileCount = new Label();
            buttonResetAnchor = new Button();
            buttonClearSelection = new Button();
            radioButtonModeBox = new RadioButton();
            radioButtonModeClick = new RadioButton();
            groupBox2CoordSystem = new GroupBox();
            radioButtonEpsg3857 = new RadioButton();
            radioButtonEpsg2180 = new RadioButton();
            groupBoxMap = new GroupBox();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            groupBox3Config = new GroupBox();
            panelTrainzFiles = new Panel();
            labelKuidSeparator = new Label();
            textBoxKuidPart2 = new TextBox();
            textBoxKuidPart1 = new TextBox();
            label12 = new Label();
            textBoxCounter = new TextBox();
            label11 = new Label();
            textBoxDesignation = new TextBox();
            label13 = new Label();
            textBoxDestinationFolder = new TextBox();
            label4 = new Label();
            basemapFolderListBox = new ListBox();
            buttonLoadFolder = new Button();
            label10 = new Label();
            radioButton512 = new RadioButton();
            radioButton1024 = new RadioButton();
            radioButton2048 = new RadioButton();
            radioButton4096 = new RadioButton();
            label2 = new Label();
            textBoxBasemapDate = new TextBox();
            label14 = new Label();
            comboBoxMapType = new ComboBox();
            label15 = new Label();
            groupBox4Download = new GroupBox();
            buttonCancel = new Button();
            buttonStartDownload = new Button();
            labelProgress = new Label();
            progressBar1 = new ProgressBar();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            groupBox1Selection.SuspendLayout();
            groupBox2CoordSystem.SuspendLayout();
            groupBoxMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            groupBox3Config.SuspendLayout();
            panelTrainzFiles.SuspendLayout();
            groupBox4Download.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1Selection
            // 
            groupBox1Selection.Controls.Add(labelArea);
            groupBox1Selection.Controls.Add(labelTileCount);
            groupBox1Selection.Controls.Add(buttonResetAnchor);
            groupBox1Selection.Controls.Add(buttonClearSelection);
            groupBox1Selection.Controls.Add(radioButtonModeBox);
            groupBox1Selection.Controls.Add(radioButtonModeClick);
            groupBox1Selection.Location = new Point(12, 12);
            groupBox1Selection.Name = "groupBox1Selection";
            groupBox1Selection.Size = new Size(200, 215);
            groupBox1Selection.TabIndex = 0;
            groupBox1Selection.TabStop = false;
            groupBox1Selection.Text = "1. Zaznaczanie na siatce";
            // 
            // labelArea
            // 
            labelArea.AutoSize = true;
            labelArea.Location = new Point(6, 190);
            labelArea.Name = "labelArea";
            labelArea.Size = new Size(130, 15);
            labelArea.TabIndex = 5;
            labelArea.Text = "Powierzchnia: 0.00 km²";
            // 
            // labelTileCount
            // 
            labelTileCount.AutoSize = true;
            labelTileCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelTileCount.Location = new Point(6, 171);
            labelTileCount.Name = "labelTileCount";
            labelTileCount.Size = new Size(111, 15);
            labelTileCount.TabIndex = 4;
            labelTileCount.Text = "Zaznaczono kafli: 0";
            // 
            // buttonResetAnchor
            // 
            buttonResetAnchor.Location = new Point(6, 125);
            buttonResetAnchor.Name = "buttonResetAnchor";
            buttonResetAnchor.Size = new Size(188, 26);
            buttonResetAnchor.TabIndex = 3;
            buttonResetAnchor.Text = "Resetuj punkt bazowy";
            buttonResetAnchor.UseVisualStyleBackColor = true;
            buttonResetAnchor.Click += buttonResetAnchor_Click;
            // 
            // buttonClearSelection
            // 
            buttonClearSelection.Location = new Point(6, 93);
            buttonClearSelection.Name = "buttonClearSelection";
            buttonClearSelection.Size = new Size(188, 26);
            buttonClearSelection.TabIndex = 2;
            buttonClearSelection.Text = "Wyczyść zaznaczenie";
            buttonClearSelection.UseVisualStyleBackColor = true;
            buttonClearSelection.Click += buttonClearSelection_Click;
            // 
            // radioButtonModeBox
            // 
            radioButtonModeBox.AutoSize = true;
            radioButtonModeBox.Location = new Point(6, 55);
            radioButtonModeBox.Name = "radioButtonModeBox";
            radioButtonModeBox.Size = new Size(147, 19);
            radioButtonModeBox.TabIndex = 1;
            radioButtonModeBox.Text = "Zaznaczanie obszarem";
            radioButtonModeBox.UseVisualStyleBackColor = true;
            radioButtonModeBox.CheckedChanged += RadioButtonMode_CheckedChanged;
            // 
            // radioButtonModeClick
            // 
            radioButtonModeClick.AutoSize = true;
            radioButtonModeClick.Checked = true;
            radioButtonModeClick.Location = new Point(6, 26);
            radioButtonModeClick.Name = "radioButtonModeClick";
            radioButtonModeClick.Size = new Size(115, 19);
            radioButtonModeClick.TabIndex = 0;
            radioButtonModeClick.TabStop = true;
            radioButtonModeClick.Text = "Pędzel / klikanie";
            radioButtonModeClick.UseVisualStyleBackColor = true;
            radioButtonModeClick.CheckedChanged += RadioButtonMode_CheckedChanged;
            // 
            // groupBox2CoordSystem
            // 
            groupBox2CoordSystem.Controls.Add(radioButtonEpsg3857);
            groupBox2CoordSystem.Controls.Add(radioButtonEpsg2180);
            groupBox2CoordSystem.Location = new Point(12, 233);
            groupBox2CoordSystem.Name = "groupBox2CoordSystem";
            groupBox2CoordSystem.Size = new Size(200, 85);
            groupBox2CoordSystem.TabIndex = 1;
            groupBox2CoordSystem.TabStop = false;
            groupBox2CoordSystem.Text = "2. Układ współrzędnych";
            // 
            // radioButtonEpsg3857
            // 
            radioButtonEpsg3857.AutoSize = true;
            radioButtonEpsg3857.Location = new Point(6, 51);
            radioButtonEpsg3857.Name = "radioButtonEpsg3857";
            radioButtonEpsg3857.Size = new Size(140, 19);
            radioButtonEpsg3857.TabIndex = 1;
            radioButtonEpsg3857.Text = "EPSG:3857 (Globalny)";
            radioButtonEpsg3857.UseVisualStyleBackColor = true;
            radioButtonEpsg3857.CheckedChanged += RadioButtonEpsg_CheckedChanged;
            // 
            // radioButtonEpsg2180
            // 
            radioButtonEpsg2180.AutoSize = true;
            radioButtonEpsg2180.Checked = true;
            radioButtonEpsg2180.Location = new Point(6, 26);
            radioButtonEpsg2180.Name = "radioButtonEpsg2180";
            radioButtonEpsg2180.Size = new Size(130, 19);
            radioButtonEpsg2180.TabIndex = 0;
            radioButtonEpsg2180.TabStop = true;
            radioButtonEpsg2180.Text = "EPSG:2180 (Polska)";
            radioButtonEpsg2180.UseVisualStyleBackColor = true;
            radioButtonEpsg2180.CheckedChanged += RadioButtonEpsg_CheckedChanged;
            // 
            // groupBoxMap
            // 
            groupBoxMap.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxMap.Controls.Add(webView21);
            groupBoxMap.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            groupBoxMap.Location = new Point(218, 12);
            groupBoxMap.Name = "groupBoxMap";
            groupBoxMap.Size = new Size(738, 760);
            groupBoxMap.TabIndex = 2;
            groupBoxMap.TabStop = false;
            groupBoxMap.Text = "Wybór kafli na mapie (siatka lokalna):";
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(6, 28);
            webView21.Name = "webView21";
            webView21.Size = new Size(726, 726);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // groupBox3Config
            // 
            groupBox3Config.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox3Config.Controls.Add(panelTrainzFiles);
            groupBox3Config.Controls.Add(radioButton512);
            groupBox3Config.Controls.Add(radioButton1024);
            groupBox3Config.Controls.Add(radioButton2048);
            groupBox3Config.Controls.Add(radioButton4096);
            groupBox3Config.Controls.Add(label2);
            groupBox3Config.Controls.Add(textBoxBasemapDate);
            groupBox3Config.Controls.Add(label14);
            groupBox3Config.Controls.Add(comboBoxMapType);
            groupBox3Config.Controls.Add(label15);
            groupBox3Config.Location = new Point(962, 12);
            groupBox3Config.Name = "groupBox3Config";
            groupBox3Config.Size = new Size(217, 575);
            groupBox3Config.TabIndex = 3;
            groupBox3Config.TabStop = false;
            groupBox3Config.Text = "3. Konfiguracja";
            // 
            // panelTrainzFiles
            // 
            panelTrainzFiles.Controls.Add(buttonLoadFolder);
            panelTrainzFiles.Controls.Add(labelKuidSeparator);
            panelTrainzFiles.Controls.Add(textBoxKuidPart2);
            panelTrainzFiles.Controls.Add(textBoxKuidPart1);
            panelTrainzFiles.Controls.Add(label12);
            panelTrainzFiles.Controls.Add(textBoxCounter);
            panelTrainzFiles.Controls.Add(label11);
            panelTrainzFiles.Controls.Add(textBoxDesignation);
            panelTrainzFiles.Controls.Add(label13);
            panelTrainzFiles.Controls.Add(textBoxDestinationFolder);
            panelTrainzFiles.Controls.Add(label4);
            panelTrainzFiles.Controls.Add(basemapFolderListBox);
            panelTrainzFiles.Controls.Add(label10);
            panelTrainzFiles.Location = new Point(6, 175);
            panelTrainzFiles.Name = "panelTrainzFiles";
            panelTrainzFiles.Size = new Size(205, 394);
            panelTrainzFiles.TabIndex = 9;
            // 
            // labelKuidSeparator
            // 
            labelKuidSeparator.AutoSize = true;
            labelKuidSeparator.Location = new Point(94, 342);
            labelKuidSeparator.Name = "labelKuidSeparator";
            labelKuidSeparator.Size = new Size(10, 15);
            labelKuidSeparator.TabIndex = 11;
            labelKuidSeparator.Text = ":";
            // 
            // textBoxKuidPart2
            // 
            textBoxKuidPart2.Location = new Point(108, 339);
            textBoxKuidPart2.Name = "textBoxKuidPart2";
            textBoxKuidPart2.Size = new Size(89, 23);
            textBoxKuidPart2.TabIndex = 10;
            textBoxKuidPart2.KeyPress += OnlyNumbers_KeyPress;
            // 
            // textBoxKuidPart1
            // 
            textBoxKuidPart1.Location = new Point(3, 339);
            textBoxKuidPart1.Name = "textBoxKuidPart1";
            textBoxKuidPart1.Size = new Size(87, 23);
            textBoxKuidPart1.TabIndex = 9;
            textBoxKuidPart1.TextAlign = HorizontalAlignment.Right;
            textBoxKuidPart1.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(3, 321);
            label12.Name = "label12";
            label12.Size = new Size(111, 15);
            label12.TabIndex = 8;
            label12.Text = "Oznaczenie KUID-u:";
            // 
            // textBoxCounter
            // 
            textBoxCounter.Location = new Point(108, 291);
            textBoxCounter.Name = "textBoxCounter";
            textBoxCounter.Size = new Size(89, 23);
            textBoxCounter.TabIndex = 7;
            textBoxCounter.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(108, 273);
            label11.Name = "label11";
            label11.Size = new Size(76, 15);
            label11.TabIndex = 6;
            label11.Text = "Nr podkładu:";
            // 
            // textBoxDesignation
            // 
            textBoxDesignation.Location = new Point(3, 291);
            textBoxDesignation.Name = "textBoxDesignation";
            textBoxDesignation.Size = new Size(87, 23);
            textBoxDesignation.TabIndex = 5;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(3, 273);
            label13.Name = "label13";
            label13.Size = new Size(87, 15);
            label13.TabIndex = 4;
            label13.Text = "Ozn. podkładu:";
            // 
            // textBoxDestinationFolder
            // 
            textBoxDestinationFolder.Location = new Point(3, 243);
            textBoxDestinationFolder.Name = "textBoxDestinationFolder";
            textBoxDestinationFolder.Size = new Size(194, 23);
            textBoxDestinationFolder.TabIndex = 3;
            textBoxDestinationFolder.TextChanged += textBoxDestinationFolder_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 225);
            label4.Name = "label4";
            label4.Size = new Size(154, 15);
            label4.TabIndex = 2;
            label4.Text = "Nazwa docelowego folderu:";
            // 
            // buttonLoadFolder
            // 
            buttonLoadFolder.Location = new Point(3, 194);
            buttonLoadFolder.Name = "buttonLoadFolder";
            buttonLoadFolder.Size = new Size(194, 26);
            buttonLoadFolder.TabIndex = 12;
            buttonLoadFolder.Text = "Wczytaj z folderu";
            buttonLoadFolder.UseVisualStyleBackColor = true;
            buttonLoadFolder.Click += buttonLoadFolder_Click;
            // 
            // basemapFolderListBox
            // 
            basemapFolderListBox.FormattingEnabled = true;
            basemapFolderListBox.ItemHeight = 15;
            basemapFolderListBox.Location = new Point(3, 21);
            basemapFolderListBox.Name = "basemapFolderListBox";
            basemapFolderListBox.Size = new Size(194, 169);
            basemapFolderListBox.TabIndex = 1;
            basemapFolderListBox.Click += basemapFolderListBox_Click;
            basemapFolderListBox.DoubleClick += basemapFolderListBox_DoubleClick;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(3, 3);
            label10.Name = "label10";
            label10.Size = new Size(80, 15);
            label10.TabIndex = 0;
            label10.Text = "Twoje foldery:";
            // 
            // radioButton512
            // 
            radioButton512.AutoSize = true;
            radioButton512.Location = new Point(109, 147);
            radioButton512.Name = "radioButton512";
            radioButton512.Size = new Size(73, 19);
            radioButton512.TabIndex = 8;
            radioButton512.Text = "512 x 512";
            radioButton512.UseVisualStyleBackColor = true;
            // 
            // radioButton1024
            // 
            radioButton1024.AutoSize = true;
            radioButton1024.Location = new Point(9, 147);
            radioButton1024.Name = "radioButton1024";
            radioButton1024.Size = new Size(85, 19);
            radioButton1024.TabIndex = 7;
            radioButton1024.Text = "1024 x 1024";
            radioButton1024.UseVisualStyleBackColor = true;
            // 
            // radioButton2048
            // 
            radioButton2048.AutoSize = true;
            radioButton2048.Checked = true;
            radioButton2048.Location = new Point(109, 122);
            radioButton2048.Name = "radioButton2048";
            radioButton2048.Size = new Size(85, 19);
            radioButton2048.TabIndex = 6;
            radioButton2048.TabStop = true;
            radioButton2048.Text = "2048 x 2048";
            radioButton2048.UseVisualStyleBackColor = true;
            // 
            // radioButton4096
            // 
            radioButton4096.AutoSize = true;
            radioButton4096.Location = new Point(9, 122);
            radioButton4096.Name = "radioButton4096";
            radioButton4096.Size = new Size(85, 19);
            radioButton4096.TabIndex = 5;
            radioButton4096.Text = "4096 x 4096";
            radioButton4096.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 102);
            label2.Name = "label2";
            label2.Size = new Size(134, 15);
            label2.TabIndex = 4;
            label2.Text = "Rozdzielczość podkładu:";
            // 
            // textBoxBasemapDate
            // 
            textBoxBasemapDate.Location = new Point(136, 68);
            textBoxBasemapDate.MaxLength = 4;
            textBoxBasemapDate.Name = "textBoxBasemapDate";
            textBoxBasemapDate.Size = new Size(72, 23);
            textBoxBasemapDate.TabIndex = 3;
            textBoxBasemapDate.TextAlign = HorizontalAlignment.Center;
            textBoxBasemapDate.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(136, 50);
            label14.Name = "label14";
            label14.Size = new Size(30, 15);
            label14.TabIndex = 2;
            label14.Text = "Rok:";
            // 
            // comboBoxMapType
            // 
            comboBoxMapType.DropDownWidth = 280;
            comboBoxMapType.FormattingEnabled = true;
            comboBoxMapType.Location = new Point(6, 68);
            comboBoxMapType.Name = "comboBoxMapType";
            comboBoxMapType.Size = new Size(124, 23);
            comboBoxMapType.TabIndex = 1;
            comboBoxMapType.SelectedIndexChanged += comboBoxMapType_SelectedIndexChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(6, 50);
            label15.Name = "label15";
            label15.Size = new Size(107, 15);
            label15.TabIndex = 0;
            label15.Text = "Rodzaj podkładów:";
            // 
            // groupBox4Download
            // 
            groupBox4Download.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox4Download.Controls.Add(buttonCancel);
            groupBox4Download.Controls.Add(buttonStartDownload);
            groupBox4Download.Controls.Add(labelProgress);
            groupBox4Download.Controls.Add(progressBar1);
            groupBox4Download.Location = new Point(962, 593);
            groupBox4Download.Name = "groupBox4Download";
            groupBox4Download.Size = new Size(217, 179);
            groupBox4Download.TabIndex = 4;
            groupBox4Download.TabStop = false;
            groupBox4Download.Text = "4. Pobieranie";
            // 
            // buttonCancel
            // 
            buttonCancel.Enabled = false;
            buttonCancel.Location = new Point(6, 140);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(205, 28);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Anuluj";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonStartDownload
            // 
            buttonStartDownload.Location = new Point(6, 102);
            buttonStartDownload.Name = "buttonStartDownload";
            buttonStartDownload.Size = new Size(205, 32);
            buttonStartDownload.TabIndex = 2;
            buttonStartDownload.Text = "Pobierz podkłady";
            buttonStartDownload.UseVisualStyleBackColor = true;
            buttonStartDownload.Click += buttonStartDownload_Click;
            // 
            // labelProgress
            // 
            labelProgress.AutoSize = true;
            labelProgress.Location = new Point(6, 58);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(125, 15);
            labelProgress.TabIndex = 1;
            labelProgress.Text = "Gotowy do pobierania.";
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(6, 26);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(205, 23);
            progressBar1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            statusStrip1.ImageScalingSize = new Size(20, 20);
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 787);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1193, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Margin = new Padding(10, 3, 0, 2);
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 17);
            // 
            // GridToolForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1193, 809);
            Controls.Add(statusStrip1);
            Controls.Add(groupBox4Download);
            Controls.Add(groupBox3Config);
            Controls.Add(groupBoxMap);
            Controls.Add(groupBox2CoordSystem);
            Controls.Add(groupBox1Selection);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1209, 848);
            Name = "GridToolForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pobieranie obszarowe (siatka)";
            FormClosing += GridToolForm_FormClosing;
            Load += GridToolForm_Load;
            groupBox1Selection.ResumeLayout(false);
            groupBox1Selection.PerformLayout();
            groupBox2CoordSystem.ResumeLayout(false);
            groupBox2CoordSystem.PerformLayout();
            groupBoxMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            groupBox3Config.ResumeLayout(false);
            groupBox3Config.PerformLayout();
            panelTrainzFiles.ResumeLayout(false);
            panelTrainzFiles.PerformLayout();
            groupBox4Download.ResumeLayout(false);
            groupBox4Download.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox1Selection;
        private RadioButton radioButtonModeBox;
        private RadioButton radioButtonModeClick;
        private Button buttonClearSelection;
        private Button buttonResetAnchor;
        private Label labelTileCount;
        private Label labelArea;
        private GroupBox groupBox2CoordSystem;
        private RadioButton radioButtonEpsg3857;
        private RadioButton radioButtonEpsg2180;
        private GroupBox groupBoxMap;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private GroupBox groupBox3Config;
        private ComboBox comboBoxMapType;
        private Label label15;
        private TextBox textBoxBasemapDate;
        private Label label14;
        private RadioButton radioButton512;
        private RadioButton radioButton1024;
        private RadioButton radioButton2048;
        private RadioButton radioButton4096;
        private Label label2;
        private Panel panelTrainzFiles;
        private ListBox basemapFolderListBox;
        private Button buttonLoadFolder;
        private Label label10;
        private TextBox textBoxDestinationFolder;
        private Label label4;
        private TextBox textBoxDesignation;
        private Label label13;
        private TextBox textBoxCounter;
        private Label label11;
        private Label labelKuidSeparator;
        private TextBox textBoxKuidPart2;
        private TextBox textBoxKuidPart1;
        private Label label12;
        private GroupBox groupBox4Download;
        private ProgressBar progressBar1;
        private Label labelProgress;
        private Button buttonStartDownload;
        private Button buttonCancel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}
