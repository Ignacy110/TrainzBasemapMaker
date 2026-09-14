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
            splitContainer1 = new SplitContainer();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            panelSidebar = new Panel();
            groupBoxDownload = new GroupBox();
            labelProgress = new Label();
            buttonCancel = new Button();
            buttonStartDownload = new Button();
            progressBar1 = new ProgressBar();
            groupBoxTrainz = new GroupBox();
            labelKuidSep = new Label();
            textBoxKuidPart2 = new TextBox();
            textBoxKuidPart1 = new TextBox();
            labelKuid = new Label();
            textBoxCounter = new TextBox();
            labelCounter = new Label();
            textBoxDesignation = new TextBox();
            labelDesignation = new Label();
            textBoxDestinationFolder = new TextBox();
            labelFolder = new Label();
            groupBoxParams = new GroupBox();
            textBoxBasemapDate = new TextBox();
            labelDate = new Label();
            radioButton512 = new RadioButton();
            radioButton1024 = new RadioButton();
            radioButton2048 = new RadioButton();
            radioButton4096 = new RadioButton();
            labelRes = new Label();
            comboBoxMapType = new ComboBox();
            labelMapType = new Label();
            groupBoxSelection = new GroupBox();
            buttonResetAnchor = new Button();
            buttonClearSelection = new Button();
            buttonSelectViewport = new Button();
            labelArea = new Label();
            labelTileCount = new Label();
            groupBoxCoordSystem = new GroupBox();
            radioButtonEpsg3857 = new RadioButton();
            radioButtonEpsg2180 = new RadioButton();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            panelSidebar.SuspendLayout();
            groupBoxDownload.SuspendLayout();
            groupBoxTrainz.SuspendLayout();
            groupBoxParams.SuspendLayout();
            groupBoxSelection.SuspendLayout();
            groupBoxCoordSystem.SuspendLayout();
            SuspendLayout();
            // 
            // splitContainer1
            // 
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.FixedPanel = FixedPanel.Panel2;
            splitContainer1.Location = new Point(0, 0);
            splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(webView21);
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(panelSidebar);
            splitContainer1.Panel2MinSize = 340;
            splitContainer1.Size = new Size(1264, 821);
            splitContainer1.SplitterDistance = 910;
            splitContainer1.TabIndex = 0;
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Dock = DockStyle.Fill;
            webView21.Location = new Point(0, 0);
            webView21.Name = "webView21";
            webView21.Size = new Size(910, 821);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // panelSidebar
            // 
            panelSidebar.AutoScroll = true;
            panelSidebar.Controls.Add(groupBoxDownload);
            panelSidebar.Controls.Add(groupBoxTrainz);
            panelSidebar.Controls.Add(groupBoxParams);
            panelSidebar.Controls.Add(groupBoxSelection);
            panelSidebar.Controls.Add(groupBoxCoordSystem);
            panelSidebar.Dock = DockStyle.Fill;
            panelSidebar.Location = new Point(0, 0);
            panelSidebar.Name = "panelSidebar";
            panelSidebar.Padding = new Padding(8);
            panelSidebar.Size = new Size(350, 821);
            panelSidebar.TabIndex = 0;
            // 
            // groupBoxDownload
            // 
            groupBoxDownload.Controls.Add(labelProgress);
            groupBoxDownload.Controls.Add(buttonCancel);
            groupBoxDownload.Controls.Add(buttonStartDownload);
            groupBoxDownload.Controls.Add(progressBar1);
            groupBoxDownload.Dock = DockStyle.Top;
            groupBoxDownload.Location = new Point(8, 650);
            groupBoxDownload.Name = "groupBoxDownload";
            groupBoxDownload.Size = new Size(334, 150);
            groupBoxDownload.TabIndex = 4;
            groupBoxDownload.TabStop = false;
            groupBoxDownload.Text = "5. Pobieranie";
            // 
            // labelProgress
            // 
            labelProgress.AutoSize = true;
            labelProgress.Location = new Point(10, 55);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(125, 15);
            labelProgress.TabIndex = 3;
            labelProgress.Text = "Gotowy do pobierania.";
            // 
            // buttonCancel
            // 
            buttonCancel.Enabled = false;
            buttonCancel.Location = new Point(175, 80);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(145, 36);
            buttonCancel.TabIndex = 2;
            buttonCancel.Text = "Anuluj";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonStartDownload
            // 
            buttonStartDownload.BackColor = Color.FromArgb(0, 120, 212);
            buttonStartDownload.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            buttonStartDownload.ForeColor = Color.White;
            buttonStartDownload.Location = new Point(10, 80);
            buttonStartDownload.Name = "buttonStartDownload";
            buttonStartDownload.Size = new Size(155, 36);
            buttonStartDownload.TabIndex = 1;
            buttonStartDownload.Text = "Pobierz podkłady";
            buttonStartDownload.UseVisualStyleBackColor = false;
            buttonStartDownload.Click += buttonStartDownload_Click;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(10, 25);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(310, 23);
            progressBar1.TabIndex = 0;
            // 
            // groupBoxTrainz
            // 
            groupBoxTrainz.Controls.Add(labelKuidSep);
            groupBoxTrainz.Controls.Add(textBoxKuidPart2);
            groupBoxTrainz.Controls.Add(textBoxKuidPart1);
            groupBoxTrainz.Controls.Add(labelKuid);
            groupBoxTrainz.Controls.Add(textBoxCounter);
            groupBoxTrainz.Controls.Add(labelCounter);
            groupBoxTrainz.Controls.Add(textBoxDesignation);
            groupBoxTrainz.Controls.Add(labelDesignation);
            groupBoxTrainz.Controls.Add(textBoxDestinationFolder);
            groupBoxTrainz.Controls.Add(labelFolder);
            groupBoxTrainz.Dock = DockStyle.Top;
            groupBoxTrainz.Location = new Point(8, 440);
            groupBoxTrainz.Name = "groupBoxTrainz";
            groupBoxTrainz.Size = new Size(334, 210);
            groupBoxTrainz.TabIndex = 3;
            groupBoxTrainz.TabStop = false;
            groupBoxTrainz.Text = "4. Konfiguracja Trainz";
            // 
            // labelKuidSep
            // 
            labelKuidSep.AutoSize = true;
            labelKuidSep.Location = new Point(155, 175);
            labelKuidSep.Name = "labelKuidSep";
            labelKuidSep.Size = new Size(10, 15);
            labelKuidSep.TabIndex = 9;
            labelKuidSep.Text = ":";
            // 
            // textBoxKuidPart2
            // 
            textBoxKuidPart2.Location = new Point(170, 172);
            textBoxKuidPart2.Name = "textBoxKuidPart2";
            textBoxKuidPart2.Size = new Size(150, 23);
            textBoxKuidPart2.TabIndex = 8;
            textBoxKuidPart2.KeyPress += OnlyNumbers_KeyPress;
            // 
            // textBoxKuidPart1
            // 
            textBoxKuidPart1.Location = new Point(10, 172);
            textBoxKuidPart1.Name = "textBoxKuidPart1";
            textBoxKuidPart1.Size = new Size(140, 23);
            textBoxKuidPart1.TabIndex = 7;
            textBoxKuidPart1.TextAlign = HorizontalAlignment.Right;
            textBoxKuidPart1.KeyPress += OnlyNumbers_KeyPress;
            // 
            // labelKuid
            // 
            labelKuid.AutoSize = true;
            labelKuid.Location = new Point(10, 154);
            labelKuid.Name = "labelKuid";
            labelKuid.Size = new Size(158, 15);
            labelKuid.TabIndex = 6;
            labelKuid.Text = "Początkowy KUID (cz. 1 : 2):";
            // 
            // textBoxCounter
            // 
            textBoxCounter.Location = new Point(170, 120);
            textBoxCounter.Name = "textBoxCounter";
            textBoxCounter.Size = new Size(150, 23);
            textBoxCounter.TabIndex = 5;
            textBoxCounter.KeyPress += OnlyNumbers_KeyPress;
            // 
            // labelCounter
            // 
            labelCounter.AutoSize = true;
            labelCounter.Location = new Point(170, 102);
            labelCounter.Name = "labelCounter";
            labelCounter.Size = new Size(120, 15);
            labelCounter.TabIndex = 4;
            labelCounter.Text = "Początkowy nr podkł.:";
            // 
            // textBoxDesignation
            // 
            textBoxDesignation.Location = new Point(10, 120);
            textBoxDesignation.Name = "textBoxDesignation";
            textBoxDesignation.Size = new Size(140, 23);
            textBoxDesignation.TabIndex = 3;
            // 
            // labelDesignation
            // 
            labelDesignation.AutoSize = true;
            labelDesignation.Location = new Point(10, 102);
            labelDesignation.Name = "labelDesignation";
            labelDesignation.Size = new Size(127, 15);
            labelDesignation.TabIndex = 2;
            labelDesignation.Text = "Oznaczenie podkładów:";
            // 
            // textBoxDestinationFolder
            // 
            textBoxDestinationFolder.Location = new Point(10, 68);
            textBoxDestinationFolder.Name = "textBoxDestinationFolder";
            textBoxDestinationFolder.Size = new Size(310, 23);
            textBoxDestinationFolder.TabIndex = 1;
            textBoxDestinationFolder.TextChanged += textBoxDestinationFolder_TextChanged;
            // 
            // labelFolder
            // 
            labelFolder.AutoSize = true;
            labelFolder.Location = new Point(10, 25);
            labelFolder.Name = "labelFolder";
            labelFolder.Size = new Size(154, 30);
            labelFolder.TabIndex = 0;
            labelFolder.Text = "Nazwa folderu docelowego\r\n(grupy podkładów):";
            // 
            // groupBoxParams
            // 
            groupBoxParams.Controls.Add(textBoxBasemapDate);
            groupBoxParams.Controls.Add(labelDate);
            groupBoxParams.Controls.Add(radioButton512);
            groupBoxParams.Controls.Add(radioButton1024);
            groupBoxParams.Controls.Add(radioButton2048);
            groupBoxParams.Controls.Add(radioButton4096);
            groupBoxParams.Controls.Add(labelRes);
            groupBoxParams.Controls.Add(comboBoxMapType);
            groupBoxParams.Controls.Add(labelMapType);
            groupBoxParams.Dock = DockStyle.Top;
            groupBoxParams.Location = new Point(8, 230);
            groupBoxParams.Name = "groupBoxParams";
            groupBoxParams.Size = new Size(334, 210);
            groupBoxParams.TabIndex = 2;
            groupBoxParams.TabStop = false;
            groupBoxParams.Text = "3. Parametry obrazu";
            // 
            // textBoxBasemapDate
            // 
            textBoxBasemapDate.Location = new Point(10, 92);
            textBoxBasemapDate.MaxLength = 4;
            textBoxBasemapDate.Name = "textBoxBasemapDate";
            textBoxBasemapDate.Size = new Size(100, 23);
            textBoxBasemapDate.TabIndex = 8;
            textBoxBasemapDate.KeyPress += OnlyNumbers_KeyPress;
            // 
            // labelDate
            // 
            labelDate.AutoSize = true;
            labelDate.Location = new Point(10, 74);
            labelDate.Name = "labelDate";
            labelDate.Size = new Size(95, 15);
            labelDate.TabIndex = 7;
            labelDate.Text = "Rok podkładów:";
            // 
            // radioButton512
            // 
            radioButton512.AutoSize = true;
            radioButton512.Location = new Point(170, 175);
            radioButton512.Name = "radioButton512";
            radioButton512.Size = new Size(73, 19);
            radioButton512.TabIndex = 6;
            radioButton512.Text = "512 x 512";
            radioButton512.UseVisualStyleBackColor = true;
            // 
            // radioButton1024
            // 
            radioButton1024.AutoSize = true;
            radioButton1024.Location = new Point(10, 175);
            radioButton1024.Name = "radioButton1024";
            radioButton1024.Size = new Size(85, 19);
            radioButton1024.TabIndex = 5;
            radioButton1024.Text = "1024 x 1024";
            radioButton1024.UseVisualStyleBackColor = true;
            // 
            // radioButton2048
            // 
            radioButton2048.AutoSize = true;
            radioButton2048.Checked = true;
            radioButton2048.Location = new Point(170, 148);
            radioButton2048.Name = "radioButton2048";
            radioButton2048.Size = new Size(85, 19);
            radioButton2048.TabIndex = 4;
            radioButton2048.TabStop = true;
            radioButton2048.Text = "2048 x 2048";
            radioButton2048.UseVisualStyleBackColor = true;
            // 
            // radioButton4096
            // 
            radioButton4096.AutoSize = true;
            radioButton4096.Location = new Point(10, 148);
            radioButton4096.Name = "radioButton4096";
            radioButton4096.Size = new Size(85, 19);
            radioButton4096.TabIndex = 3;
            radioButton4096.Text = "4096 x 4096";
            radioButton4096.UseVisualStyleBackColor = true;
            // 
            // labelRes
            // 
            labelRes.AutoSize = true;
            labelRes.Location = new Point(10, 127);
            labelRes.Name = "labelRes";
            labelRes.Size = new Size(134, 15);
            labelRes.TabIndex = 2;
            labelRes.Text = "Rozdzielczość podkładu:";
            // 
            // comboBoxMapType
            // 
            comboBoxMapType.DropDownWidth = 280;
            comboBoxMapType.FormattingEnabled = true;
            comboBoxMapType.Location = new Point(10, 43);
            comboBoxMapType.Name = "comboBoxMapType";
            comboBoxMapType.Size = new Size(310, 23);
            comboBoxMapType.TabIndex = 1;
            comboBoxMapType.SelectedIndexChanged += comboBoxMapType_SelectedIndexChanged;
            // 
            // labelMapType
            // 
            labelMapType.AutoSize = true;
            labelMapType.Location = new Point(10, 25);
            labelMapType.Name = "labelMapType";
            labelMapType.Size = new Size(107, 15);
            labelMapType.TabIndex = 0;
            labelMapType.Text = "Rodzaj podkładów:";
            // 
            // groupBoxSelection
            // 
            groupBoxSelection.Controls.Add(buttonResetAnchor);
            groupBoxSelection.Controls.Add(buttonClearSelection);
            groupBoxSelection.Controls.Add(buttonSelectViewport);
            groupBoxSelection.Controls.Add(labelArea);
            groupBoxSelection.Controls.Add(labelTileCount);
            groupBoxSelection.Dock = DockStyle.Top;
            groupBoxSelection.Location = new Point(8, 70);
            groupBoxSelection.Name = "groupBoxSelection";
            groupBoxSelection.Size = new Size(334, 160);
            groupBoxSelection.TabIndex = 1;
            groupBoxSelection.TabStop = false;
            groupBoxSelection.Text = "2. Informacje o zaznaczeniu";
            // 
            // buttonResetAnchor
            // 
            buttonResetAnchor.Location = new Point(10, 122);
            buttonResetAnchor.Name = "buttonResetAnchor";
            buttonResetAnchor.Size = new Size(310, 28);
            buttonResetAnchor.TabIndex = 4;
            buttonResetAnchor.Text = "🔄 Resetuj punkt bazowy siatki";
            buttonResetAnchor.UseVisualStyleBackColor = true;
            buttonResetAnchor.Click += buttonResetAnchor_Click;
            // 
            // buttonClearSelection
            // 
            buttonClearSelection.Location = new Point(170, 88);
            buttonClearSelection.Name = "buttonClearSelection";
            buttonClearSelection.Size = new Size(150, 28);
            buttonClearSelection.TabIndex = 3;
            buttonClearSelection.Text = "❌ Wyczyść";
            buttonClearSelection.UseVisualStyleBackColor = true;
            buttonClearSelection.Click += buttonClearSelection_Click;
            // 
            // buttonSelectViewport
            // 
            buttonSelectViewport.Location = new Point(10, 88);
            buttonSelectViewport.Name = "buttonSelectViewport";
            buttonSelectViewport.Size = new Size(150, 28);
            buttonSelectViewport.TabIndex = 2;
            buttonSelectViewport.Text = "🔲 Zaznacz widok";
            buttonSelectViewport.UseVisualStyleBackColor = true;
            buttonSelectViewport.Click += buttonSelectViewport_Click;
            // 
            // labelArea
            // 
            labelArea.AutoSize = true;
            labelArea.Font = new Font("Segoe UI", 9.5F);
            labelArea.Location = new Point(10, 55);
            labelArea.Name = "labelArea";
            labelArea.Size = new Size(160, 17);
            labelArea.TabIndex = 1;
            labelArea.Text = "Powierzchnia: 0.00 km²";
            // 
            // labelTileCount
            // 
            labelTileCount.AutoSize = true;
            labelTileCount.Font = new Font("Segoe UI", 10.5F, FontStyle.Bold);
            labelTileCount.ForeColor = Color.FromArgb(0, 102, 204);
            labelTileCount.Location = new Point(10, 25);
            labelTileCount.Name = "labelTileCount";
            labelTileCount.Size = new Size(181, 19);
            labelTileCount.TabIndex = 0;
            labelTileCount.Text = "Zaznaczono kafli: 0";
            // 
            // groupBoxCoordSystem
            // 
            groupBoxCoordSystem.Controls.Add(radioButtonEpsg3857);
            groupBoxCoordSystem.Controls.Add(radioButtonEpsg2180);
            groupBoxCoordSystem.Dock = DockStyle.Top;
            groupBoxCoordSystem.Location = new Point(8, 8);
            groupBoxCoordSystem.Name = "groupBoxCoordSystem";
            groupBoxCoordSystem.Size = new Size(334, 62);
            groupBoxCoordSystem.TabIndex = 0;
            groupBoxCoordSystem.TabStop = false;
            groupBoxCoordSystem.Text = "1. Układ współrzędnych";
            // 
            // radioButtonEpsg3857
            // 
            radioButtonEpsg3857.AutoSize = true;
            radioButtonEpsg3857.Location = new Point(170, 26);
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
            radioButtonEpsg2180.Location = new Point(10, 26);
            radioButtonEpsg2180.Name = "radioButtonEpsg2180";
            radioButtonEpsg2180.Size = new Size(130, 19);
            radioButtonEpsg2180.TabIndex = 0;
            radioButtonEpsg2180.TabStop = true;
            radioButtonEpsg2180.Text = "EPSG:2180 (Polska)";
            radioButtonEpsg2180.UseVisualStyleBackColor = true;
            radioButtonEpsg2180.CheckedChanged += RadioButtonEpsg_CheckedChanged;
            // 
            // GridToolForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1264, 821);
            Controls.Add(splitContainer1);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1000, 700);
            Name = "GridToolForm";
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pobieranie obszarowe (siatka)";
            FormClosing += GridToolForm_FormClosing;
            Load += GridToolForm_Load;
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            panelSidebar.ResumeLayout(false);
            groupBoxDownload.ResumeLayout(false);
            groupBoxDownload.PerformLayout();
            groupBoxTrainz.ResumeLayout(false);
            groupBoxTrainz.PerformLayout();
            groupBoxParams.ResumeLayout(false);
            groupBoxParams.PerformLayout();
            groupBoxSelection.ResumeLayout(false);
            groupBoxSelection.PerformLayout();
            groupBoxCoordSystem.ResumeLayout(false);
            groupBoxCoordSystem.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private SplitContainer splitContainer1;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private Panel panelSidebar;
        private GroupBox groupBoxCoordSystem;
        private RadioButton radioButtonEpsg3857;
        private RadioButton radioButtonEpsg2180;
        private GroupBox groupBoxSelection;
        private Label labelArea;
        private Label labelTileCount;
        private Button buttonResetAnchor;
        private Button buttonClearSelection;
        private Button buttonSelectViewport;
        private GroupBox groupBoxParams;
        private RadioButton radioButton512;
        private RadioButton radioButton1024;
        private RadioButton radioButton2048;
        private RadioButton radioButton4096;
        private Label labelRes;
        private ComboBox comboBoxMapType;
        private Label labelMapType;
        private TextBox textBoxBasemapDate;
        private Label labelDate;
        private GroupBox groupBoxTrainz;
        private TextBox textBoxDestinationFolder;
        private Label labelFolder;
        private TextBox textBoxCounter;
        private Label labelCounter;
        private TextBox textBoxDesignation;
        private Label labelDesignation;
        private Label labelKuidSep;
        private TextBox textBoxKuidPart2;
        private TextBox textBoxKuidPart1;
        private Label labelKuid;
        private GroupBox groupBoxDownload;
        private Label labelProgress;
        private Button buttonCancel;
        private Button buttonStartDownload;
        private ProgressBar progressBar1;
    }
}
