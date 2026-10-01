// Trainz Basemap Maker
// https://github.com/Ignacy110/TrainzBasemapMaker
//
// Copyright (C) 2026 Ignacy110 (http://github.com/Ignacy110)
//
// This library is free software; you can redistribute it and/or
// modify it under the terms of the GNU Lesser General Public
// License as published by the Free Software Foundation; either
// version 2.1 of the License, or (at your option) any later version.
//
// This library is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
// Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public
// License along with this library; if not, see (http://www.gnu.org/licenses/).

namespace TrainzBasemapMaker
{
    partial class TerrainGridToolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(TerrainGridToolForm));
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
            labelRoutes = new Label();
            listBoxRoutes = new ListBox();
            buttonLoadRoute = new Button();
            buttonDeleteRoute = new Button();
            panelTrainzFiles = new Panel();
            labelKuidSeparator = new Label();
            textBoxKuidPart2 = new TextBox();
            textBoxKuidPart1 = new TextBox();
            label12 = new Label();
            textBoxDestinationFolder = new TextBox();
            label4 = new Label();
            groupBoxElevation = new GroupBox();
            radioButtonElevationRelative = new RadioButton();
            radioButtonElevationAbsolute = new RadioButton();
            groupBox5Download = new GroupBox();
            buttonCancel = new Button();
            buttonStartDownload = new Button();
            labelProgress = new Label();
            progressBar1 = new ProgressBar();
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
            groupBoxElevation.SuspendLayout();
            groupBox5Download.SuspendLayout();
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
            groupBox4Configurator.Controls.Add(labelRoutes);
            groupBox4Configurator.Controls.Add(listBoxRoutes);
            groupBox4Configurator.Controls.Add(buttonLoadRoute);
            groupBox4Configurator.Controls.Add(buttonDeleteRoute);
            groupBox4Configurator.Controls.Add(panelTrainzFiles);
            groupBox4Configurator.Controls.Add(groupBoxElevation);
            groupBox4Configurator.Location = new Point(818, 12);
            groupBox4Configurator.Name = "groupBox4Configurator";
            groupBox4Configurator.Size = new Size(217, 410);
            groupBox4Configurator.TabIndex = 3;
            groupBox4Configurator.TabStop = false;
            groupBox4Configurator.Text = "4. Konfiguracja pobieranej trasy";
            // 
            // labelRoutes
            // 
            labelRoutes.AutoSize = true;
            labelRoutes.Location = new Point(6, 20);
            labelRoutes.Name = "labelRoutes";
            labelRoutes.Size = new Size(120, 15);
            labelRoutes.TabIndex = 0;
            labelRoutes.Text = "Wygenerowane trasy:";
            // 
            // listBoxRoutes
            // 
            listBoxRoutes.FormattingEnabled = true;
            listBoxRoutes.ItemHeight = 15;
            listBoxRoutes.Location = new Point(6, 38);
            listBoxRoutes.Name = "listBoxRoutes";
            listBoxRoutes.Size = new Size(205, 109);
            listBoxRoutes.TabIndex = 1;
            listBoxRoutes.SelectedIndexChanged += listBoxRoutes_SelectedIndexChanged;
            listBoxRoutes.DoubleClick += listBoxRoutes_DoubleClick;
            // 
            // buttonLoadRoute
            // 
            buttonLoadRoute.Location = new Point(6, 153);
            buttonLoadRoute.Name = "buttonLoadRoute";
            buttonLoadRoute.Size = new Size(100, 26);
            buttonLoadRoute.TabIndex = 2;
            buttonLoadRoute.Text = "Wczytaj";
            buttonLoadRoute.UseVisualStyleBackColor = true;
            buttonLoadRoute.Click += buttonLoadRoute_Click;
            // 
            // buttonDeleteRoute
            // 
            buttonDeleteRoute.Location = new Point(111, 153);
            buttonDeleteRoute.Name = "buttonDeleteRoute";
            buttonDeleteRoute.Size = new Size(100, 26);
            buttonDeleteRoute.TabIndex = 3;
            buttonDeleteRoute.Text = "Usuń";
            buttonDeleteRoute.UseVisualStyleBackColor = true;
            buttonDeleteRoute.Click += buttonDeleteRoute_Click;
            // 
            // panelTrainzFiles
            // 
            panelTrainzFiles.Controls.Add(labelKuidSeparator);
            panelTrainzFiles.Controls.Add(textBoxKuidPart2);
            panelTrainzFiles.Controls.Add(textBoxKuidPart1);
            panelTrainzFiles.Controls.Add(label12);
            panelTrainzFiles.Controls.Add(textBoxDestinationFolder);
            panelTrainzFiles.Controls.Add(label4);
            panelTrainzFiles.Location = new Point(6, 185);
            panelTrainzFiles.Name = "panelTrainzFiles";
            panelTrainzFiles.Size = new Size(205, 105);
            panelTrainzFiles.TabIndex = 4;
            // 
            // labelKuidSeparator
            // 
            labelKuidSeparator.AutoSize = true;
            labelKuidSeparator.Location = new Point(94, 76);
            labelKuidSeparator.Name = "labelKuidSeparator";
            labelKuidSeparator.Size = new Size(10, 15);
            labelKuidSeparator.TabIndex = 4;
            labelKuidSeparator.Text = ":";
            // 
            // textBoxKuidPart2
            // 
            textBoxKuidPart2.Location = new Point(108, 73);
            textBoxKuidPart2.Name = "textBoxKuidPart2";
            textBoxKuidPart2.Size = new Size(89, 23);
            textBoxKuidPart2.TabIndex = 5;
            textBoxKuidPart2.KeyPress += OnlyNumbers_KeyPress;
            // 
            // textBoxKuidPart1
            // 
            textBoxKuidPart1.Location = new Point(3, 73);
            textBoxKuidPart1.Name = "textBoxKuidPart1";
            textBoxKuidPart1.Size = new Size(89, 23);
            textBoxKuidPart1.TabIndex = 3;
            textBoxKuidPart1.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(3, 53);
            label12.Name = "label12";
            label12.Size = new Size(36, 15);
            label12.TabIndex = 2;
            label12.Text = "KUID:";
            // 
            // textBoxDestinationFolder
            // 
            textBoxDestinationFolder.Location = new Point(3, 23);
            textBoxDestinationFolder.Name = "textBoxDestinationFolder";
            textBoxDestinationFolder.Size = new Size(194, 23);
            textBoxDestinationFolder.TabIndex = 1;
            textBoxDestinationFolder.TextChanged += textBoxDestinationFolder_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 5);
            label4.Name = "label4";
            label4.Size = new Size(73, 15);
            label4.TabIndex = 0;
            label4.Text = "Nazwa trasy:";
            // 
            // groupBoxElevation
            // 
            groupBoxElevation.Controls.Add(radioButtonElevationRelative);
            groupBoxElevation.Controls.Add(radioButtonElevationAbsolute);
            groupBoxElevation.Location = new Point(6, 296);
            groupBoxElevation.Name = "groupBoxElevation";
            groupBoxElevation.Size = new Size(205, 75);
            groupBoxElevation.TabIndex = 5;
            groupBoxElevation.TabStop = false;
            groupBoxElevation.Text = "Tryb wysokości:";
            // 
            // radioButtonElevationRelative
            // 
            radioButtonElevationRelative.AutoSize = true;
            radioButtonElevationRelative.Location = new Point(6, 45);
            radioButtonElevationRelative.Name = "radioButtonElevationRelative";
            radioButtonElevationRelative.Size = new Size(157, 19);
            radioButtonElevationRelative.TabIndex = 1;
            radioButtonElevationRelative.Text = "Wyrównaj do zera (wzgl.)";
            radioButtonElevationRelative.UseVisualStyleBackColor = true;
            // 
            // radioButtonElevationAbsolute
            // 
            radioButtonElevationAbsolute.AutoSize = true;
            radioButtonElevationAbsolute.Checked = true;
            radioButtonElevationAbsolute.Location = new Point(6, 20);
            radioButtonElevationAbsolute.Name = "radioButtonElevationAbsolute";
            radioButtonElevationAbsolute.Size = new Size(180, 19);
            radioButtonElevationAbsolute.TabIndex = 0;
            radioButtonElevationAbsolute.TabStop = true;
            radioButtonElevationAbsolute.Text = "Rzeczywista n.p.m. (bezwzgl.)";
            radioButtonElevationAbsolute.UseVisualStyleBackColor = true;
            // 
            // groupBox5Download
            // 
            groupBox5Download.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox5Download.Controls.Add(buttonCancel);
            groupBox5Download.Controls.Add(buttonStartDownload);
            groupBox5Download.Controls.Add(labelProgress);
            groupBox5Download.Controls.Add(progressBar1);
            groupBox5Download.Location = new Point(818, 428);
            groupBox5Download.Name = "groupBox5Download";
            groupBox5Download.Size = new Size(217, 179);
            groupBox5Download.TabIndex = 4;
            groupBox5Download.TabStop = false;
            groupBox5Download.Text = "5. Pobieranie i generowanie trasy";
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
            buttonStartDownload.Text = "Generuj teren (map.gnd)";
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
            comboBoxMapType.TabIndex = 44;
            comboBoxMapType.SelectedIndexChanged += comboBoxMapType_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(3, 25);
            label2.Name = "label2";
            label2.Size = new Size(106, 15);
            label2.TabIndex = 39;
            label2.Text = "Rozdzielczość [px]:";
            // 
            // textBoxBasemapDate
            // 
            textBoxBasemapDate.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            textBoxBasemapDate.Location = new Point(448, 50);
            textBoxBasemapDate.MaxLength = 4;
            textBoxBasemapDate.Name = "textBoxBasemapDate";
            textBoxBasemapDate.Size = new Size(67, 23);
            textBoxBasemapDate.TabIndex = 42;
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
            label14.TabIndex = 41;
            label14.Text = "Rok podkładów:";
            // 
            // comboBoxResolution
            // 
            comboBoxResolution.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxResolution.FormattingEnabled = true;
            comboBoxResolution.Location = new Point(6, 50);
            comboBoxResolution.Name = "comboBoxResolution";
            comboBoxResolution.Size = new Size(103, 23);
            comboBoxResolution.TabIndex = 40;
            comboBoxResolution.SelectedIndexChanged += comboBoxResolution_SelectedIndexChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(125, 25);
            label15.Name = "label15";
            label15.Size = new Size(107, 15);
            label15.TabIndex = 43;
            label15.Text = "Rodzaj podkładów:";
            // 
            // TerrainGridToolForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1047, 688);
            Controls.Add(groupBox3BasemapParams);
            Controls.Add(groupBox5Download);
            Controls.Add(groupBox4Configurator);
            Controls.Add(groupBoxMap);
            Controls.Add(groupBox1CoordSystem);
            Controls.Add(groupBox2Selection);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1063, 727);
            Name = "TerrainGridToolForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Generator terenu (map.gnd)";
            FormClosing += TerrainGridToolForm_FormClosing;
            Load += TerrainGridToolForm_Load;
            groupBox2Selection.ResumeLayout(false);
            groupBox2Selection.PerformLayout();
            groupBox1CoordSystem.ResumeLayout(false);
            groupBoxMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            groupBox4Configurator.ResumeLayout(false);
            groupBox4Configurator.PerformLayout();
            panelTrainzFiles.ResumeLayout(false);
            panelTrainzFiles.PerformLayout();
            groupBoxElevation.ResumeLayout(false);
            groupBoxElevation.PerformLayout();
            groupBox5Download.ResumeLayout(false);
            groupBox5Download.PerformLayout();
            groupBox3BasemapParams.ResumeLayout(false);
            groupBox3BasemapParams.PerformLayout();
            ResumeLayout(false);
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
        private Label labelRoutes;
        private ListBox listBoxRoutes;
        private Button buttonLoadRoute;
        private Button buttonDeleteRoute;
        private Panel panelTrainzFiles;
        private TextBox textBoxDestinationFolder;
        private Label label4;
        private Label labelKuidSeparator;
        private TextBox textBoxKuidPart2;
        private TextBox textBoxKuidPart1;
        private Label label12;
        private GroupBox groupBoxElevation;
        private RadioButton radioButtonElevationRelative;
        private RadioButton radioButtonElevationAbsolute;
        private GroupBox groupBox5Download;
        private ProgressBar progressBar1;
        private Label labelProgress;
        private Button buttonStartDownload;
        private Button buttonCancel;
        
        
        private GroupBox groupBox3BasemapParams;
        private ComboBox comboBoxMapType;
        private Label label2;
        private TextBox textBoxBasemapDate;
        private Label label14;
        private ComboBox comboBoxResolution;
        private Label label15;
    }
}
