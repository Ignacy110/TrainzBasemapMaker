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
            webView21?.Dispose();
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
            groupBox2Selection = new GroupBox();
            labelArea = new Label();
            labelTileCount = new Label();
            buttonResetAnchor = new Button();
            buttonClearSelection = new Button();
            radioButtonModeBox = new RadioButton();
            radioButtonModeClick = new RadioButton();
            groupBox1CoordSystem = new GroupBox();
            comboBoxEpsg = new ComboBox();
            groupBoxMap = new GroupBox();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            groupBox4Configurator = new GroupBox();
            panelTrainzFiles = new Panel();
            buttonLoadFolder = new Button();
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
            label10 = new Label();
            groupBox5Download = new GroupBox();
            buttonCancel = new Button();
            buttonStartDownload = new Button();
            labelProgress = new Label();
            progressBar1 = new ProgressBar();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            groupBox3BasemapParams = new GroupBox();
            comboBoxMapType = new ComboBox();
            label2 = new Label();
            textBoxBasemapDate = new TextBox();
            label14 = new Label();
            comboBoxResolution = new ComboBox();
            label15 = new Label();
            groupBox2Selection.SuspendLayout();
            groupBox1CoordSystem.SuspendLayout();
            groupBoxMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            groupBox4Configurator.SuspendLayout();
            panelTrainzFiles.SuspendLayout();
            groupBox5Download.SuspendLayout();
            statusStrip1.SuspendLayout();
            groupBox3BasemapParams.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox2Selection
            // 
            groupBox2Selection.Controls.Add(labelArea);
            groupBox2Selection.Controls.Add(labelTileCount);
            groupBox2Selection.Controls.Add(buttonResetAnchor);
            groupBox2Selection.Controls.Add(buttonClearSelection);
            groupBox2Selection.Controls.Add(radioButtonModeBox);
            groupBox2Selection.Controls.Add(radioButtonModeClick);
            groupBox2Selection.Location = new Point(12, 75);
            groupBox2Selection.Name = "groupBox2Selection";
            groupBox2Selection.Size = new Size(214, 215);
            groupBox2Selection.TabIndex = 0;
            groupBox2Selection.TabStop = false;
            groupBox2Selection.Text = "2. Zaznaczanie obszaru";
            // 
            // labelArea
            // 
            labelArea.AutoSize = true;
            labelArea.Location = new Point(6, 190);
            labelArea.Name = "labelArea";
            labelArea.Size = new Size(128, 15);
            labelArea.TabIndex = 5;
            labelArea.Text = "Powierzchnia: 0.00 km²";
            // 
            // labelTileCount
            // 
            labelTileCount.AutoSize = true;
            labelTileCount.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            labelTileCount.Location = new Point(6, 171);
            labelTileCount.Name = "labelTileCount";
            labelTileCount.Size = new Size(112, 15);
            labelTileCount.TabIndex = 4;
            labelTileCount.Text = "Zaznaczono kafli: 0";
            // 
            // buttonResetAnchor
            // 
            buttonResetAnchor.Location = new Point(6, 125);
            buttonResetAnchor.Name = "buttonResetAnchor";
            buttonResetAnchor.Size = new Size(202, 26);
            buttonResetAnchor.TabIndex = 3;
            buttonResetAnchor.Text = "Resetuj punkt bazowy";
            buttonResetAnchor.UseVisualStyleBackColor = true;
            buttonResetAnchor.Click += buttonResetAnchor_Click;
            // 
            // buttonClearSelection
            // 
            buttonClearSelection.Location = new Point(6, 93);
            buttonClearSelection.Name = "buttonClearSelection";
            buttonClearSelection.Size = new Size(202, 26);
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
            radioButtonModeBox.Size = new Size(143, 19);
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
            radioButtonModeClick.Size = new Size(110, 19);
            radioButtonModeClick.TabIndex = 0;
            radioButtonModeClick.TabStop = true;
            radioButtonModeClick.Text = "Pędzel / klikanie";
            radioButtonModeClick.UseVisualStyleBackColor = true;
            radioButtonModeClick.CheckedChanged += RadioButtonMode_CheckedChanged;
            // 
            // groupBox1CoordSystem
            // 
            groupBox1CoordSystem.Controls.Add(comboBoxEpsg);
            groupBox1CoordSystem.Location = new Point(12, 12);
            groupBox1CoordSystem.Name = "groupBox1CoordSystem";
            groupBox1CoordSystem.Size = new Size(214, 57);
            groupBox1CoordSystem.TabIndex = 1;
            groupBox1CoordSystem.TabStop = false;
            groupBox1CoordSystem.Text = "1. Układ współrzędnych";
            // 
            // comboBoxEpsg
            // 
            comboBoxEpsg.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxEpsg.FormattingEnabled = true;
            comboBoxEpsg.Items.AddRange(new object[] { "EPSG:2180 (Polska)", "EPSG:3857 (Świat)" });
            comboBoxEpsg.Location = new Point(6, 26);
            comboBoxEpsg.Name = "comboBoxEpsg";
            comboBoxEpsg.Size = new Size(202, 23);
            comboBoxEpsg.TabIndex = 0;
            comboBoxEpsg.SelectedIndexChanged += ComboBoxEpsg_SelectedIndexChanged;
            // 
            // groupBoxMap
            // 
            groupBoxMap.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxMap.Controls.Add(webView21);
            groupBoxMap.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            groupBoxMap.Location = new Point(232, 103);
            groupBoxMap.Name = "groupBoxMap";
            groupBoxMap.Size = new Size(580, 580);
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
            webView21.Size = new Size(568, 546);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // groupBox4Configurator
            // 
            groupBox4Configurator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox4Configurator.Controls.Add(panelTrainzFiles);
            groupBox4Configurator.Location = new Point(818, 12);
            groupBox4Configurator.Name = "groupBox4Configurator";
            groupBox4Configurator.Size = new Size(217, 414);
            groupBox4Configurator.TabIndex = 3;
            groupBox4Configurator.TabStop = false;
            groupBox4Configurator.Text = "4. Konfiguracja pobieranych i tworzonych podkładów";
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
            panelTrainzFiles.Location = new Point(6, 39);
            panelTrainzFiles.Name = "panelTrainzFiles";
            panelTrainzFiles.Size = new Size(205, 370);
            panelTrainzFiles.TabIndex = 9;
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
            // groupBox5Download
            // 
            groupBox5Download.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox5Download.Controls.Add(buttonCancel);
            groupBox5Download.Controls.Add(buttonStartDownload);
            groupBox5Download.Controls.Add(labelProgress);
            groupBox5Download.Controls.Add(progressBar1);
            groupBox5Download.Location = new Point(818, 432);
            groupBox5Download.Name = "groupBox5Download";
            groupBox5Download.Size = new Size(217, 179);
            groupBox5Download.TabIndex = 4;
            groupBox5Download.TabStop = false;
            groupBox5Download.Text = "5. Pobieranie";
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
            labelProgress.Size = new Size(127, 15);
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
            statusStrip1.Location = new Point(0, 708);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1047, 22);
            statusStrip1.TabIndex = 5;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Margin = new Padding(10, 3, 0, 2);
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 17);
            // 
            // groupBox3BasemapParams
            // 
            groupBox3BasemapParams.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3BasemapParams.Controls.Add(comboBoxMapType);
            groupBox3BasemapParams.Controls.Add(label2);
            groupBox3BasemapParams.Controls.Add(textBoxBasemapDate);
            groupBox3BasemapParams.Controls.Add(label14);
            groupBox3BasemapParams.Controls.Add(comboBoxResolution);
            groupBox3BasemapParams.Controls.Add(label15);
            groupBox3BasemapParams.Location = new Point(232, 12);
            groupBox3BasemapParams.Name = "groupBox3BasemapParams";
            groupBox3BasemapParams.Size = new Size(580, 85);
            groupBox3BasemapParams.TabIndex = 6;
            groupBox3BasemapParams.TabStop = false;
            groupBox3BasemapParams.Text = "3. Parametry podkładów satelitarnych (basemap-ów)";
            // 
            // comboBoxMapType
            // 
            comboBoxMapType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            comboBoxMapType.DropDownWidth = 310;
            comboBoxMapType.FormattingEnabled = true;
            comboBoxMapType.Location = new Point(125, 50);
            comboBoxMapType.Name = "comboBoxMapType";
            comboBoxMapType.Size = new Size(307, 23);
            comboBoxMapType.TabIndex = 38;
            comboBoxMapType.SelectedIndexChanged += comboBoxMapType_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 25);
            label2.Name = "label2";
            label2.Size = new Size(106, 15);
            label2.TabIndex = 33;
            label2.Text = "Rozdzielczość [px]:";
            // 
            // textBoxBasemapDate
            // 
            textBoxBasemapDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxBasemapDate.Location = new Point(448, 50);
            textBoxBasemapDate.MaxLength = 4;
            textBoxBasemapDate.Name = "textBoxBasemapDate";
            textBoxBasemapDate.Size = new Size(67, 23);
            textBoxBasemapDate.TabIndex = 36;
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
            label14.TabIndex = 35;
            label14.Text = "Rok podkładów:";
            // 
            // comboBoxResolution
            // 
            comboBoxResolution.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxResolution.FormattingEnabled = true;
            comboBoxResolution.Location = new Point(6, 50);
            comboBoxResolution.Name = "comboBoxResolution";
            comboBoxResolution.Size = new Size(103, 23);
            comboBoxResolution.TabIndex = 34;
            comboBoxResolution.SelectedIndexChanged += comboBoxResolution_SelectedIndexChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(125, 25);
            label15.Name = "label15";
            label15.Size = new Size(107, 15);
            label15.TabIndex = 37;
            label15.Text = "Rodzaj podkładów:";
            // 
            // GridToolForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1047, 730);
            Controls.Add(groupBox3BasemapParams);
            Controls.Add(statusStrip1);
            Controls.Add(groupBox5Download);
            Controls.Add(groupBox4Configurator);
            Controls.Add(groupBoxMap);
            Controls.Add(groupBox1CoordSystem);
            Controls.Add(groupBox2Selection);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1063, 769);
            Name = "GridToolForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pobieranie obszarowe (siatka)";
            FormClosing += GridToolForm_FormClosing;
            Load += GridToolForm_Load;
            groupBox2Selection.ResumeLayout(false);
            groupBox2Selection.PerformLayout();
            groupBox1CoordSystem.ResumeLayout(false);
            groupBoxMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            groupBox4Configurator.ResumeLayout(false);
            panelTrainzFiles.ResumeLayout(false);
            panelTrainzFiles.PerformLayout();
            groupBox5Download.ResumeLayout(false);
            groupBox5Download.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            groupBox3BasemapParams.ResumeLayout(false);
            groupBox3BasemapParams.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private GroupBox groupBox2Selection;
        private RadioButton radioButtonModeBox;
        private RadioButton radioButtonModeClick;
        private Button buttonClearSelection;
        private Button buttonResetAnchor;
        private Label labelTileCount;
        private Label labelArea;
        private GroupBox groupBox1CoordSystem;
        private ComboBox comboBoxEpsg;
        private GroupBox groupBoxMap;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private GroupBox groupBox4Configurator;
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
        private GroupBox groupBox5Download;
        private ProgressBar progressBar1;
        private Label labelProgress;
        private Button buttonStartDownload;
        private Button buttonCancel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
        private GroupBox groupBox3BasemapParams;
        private ComboBox comboBoxMapType;
        private Label label2;
        private TextBox textBoxBasemapDate;
        private Label label14;
        private ComboBox comboBoxResolution;
        private Label label15;
    }
}
