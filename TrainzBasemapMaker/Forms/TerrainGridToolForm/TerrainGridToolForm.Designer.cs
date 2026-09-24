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
            textBoxDestinationFolder = new TextBox();
            label4 = new Label();
            groupBoxElevation = new GroupBox();
            radioButtonElevationRelative = new RadioButton();
            radioButtonElevationAbsolute = new RadioButton();
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
            groupBoxElevation.SuspendLayout();
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
            groupBox3Config.Controls.Add(groupBoxElevation);
            groupBox3Config.Location = new Point(962, 12);
            groupBox3Config.Name = "groupBox3Config";
            groupBox3Config.Size = new Size(217, 215);
            groupBox3Config.TabIndex = 3;
            groupBox3Config.TabStop = false;
            groupBox3Config.Text = "3. Konfiguracja trasy";
            // 
            // groupBoxElevation
            // 
            groupBoxElevation.Controls.Add(radioButtonElevationRelative);
            groupBoxElevation.Controls.Add(radioButtonElevationAbsolute);
            groupBoxElevation.Location = new Point(6, 130);
            groupBoxElevation.Name = "groupBoxElevation";
            groupBoxElevation.Size = new Size(205, 75);
            groupBoxElevation.TabIndex = 1;
            groupBoxElevation.TabStop = false;
            groupBoxElevation.Text = "Tryb wysokości:";
            // 
            // radioButtonElevationRelative
            // 
            radioButtonElevationRelative.AutoSize = true;
            radioButtonElevationRelative.Location = new Point(6, 45);
            radioButtonElevationRelative.Name = "radioButtonElevationRelative";
            radioButtonElevationRelative.Size = new Size(160, 19);
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
            radioButtonElevationAbsolute.Size = new Size(166, 19);
            radioButtonElevationAbsolute.TabIndex = 0;
            radioButtonElevationAbsolute.TabStop = true;
            radioButtonElevationAbsolute.Text = "Rzeczywista n.p.m. (bezwzgl.)";
            radioButtonElevationAbsolute.UseVisualStyleBackColor = true;
            // 
            // panelTrainzFiles
            // 
            panelTrainzFiles.Controls.Add(labelKuidSeparator);
            panelTrainzFiles.Controls.Add(textBoxKuidPart2);
            panelTrainzFiles.Controls.Add(textBoxKuidPart1);
            panelTrainzFiles.Controls.Add(label12);
            panelTrainzFiles.Controls.Add(textBoxDestinationFolder);
            panelTrainzFiles.Controls.Add(label4);
            panelTrainzFiles.Location = new Point(6, 20);
            panelTrainzFiles.Name = "panelTrainzFiles";
            panelTrainzFiles.Size = new Size(205, 105);
            panelTrainzFiles.TabIndex = 0;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(3, 5);
            label4.Name = "label4";
            label4.Size = new Size(75, 15);
            label4.TabIndex = 0;
            label4.Text = "Nazwa trasy:";
            // 
            // textBoxDestinationFolder
            // 
            textBoxDestinationFolder.Location = new Point(3, 23);
            textBoxDestinationFolder.Name = "textBoxDestinationFolder";
            textBoxDestinationFolder.Size = new Size(194, 23);
            textBoxDestinationFolder.TabIndex = 1;
            textBoxDestinationFolder.TextChanged += textBoxDestinationFolder_TextChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(3, 53);
            label12.Name = "label12";
            label12.Size = new Size(37, 15);
            label12.TabIndex = 2;
            label12.Text = "KUID:";
            // 
            // textBoxKuidPart1
            // 
            textBoxKuidPart1.Location = new Point(3, 73);
            textBoxKuidPart1.Name = "textBoxKuidPart1";
            textBoxKuidPart1.Size = new Size(89, 23);
            textBoxKuidPart1.TabIndex = 3;
            textBoxKuidPart1.KeyPress += OnlyNumbers_KeyPress;
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
            // groupBox4Download
            // 
            groupBox4Download.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox4Download.Controls.Add(buttonCancel);
            groupBox4Download.Controls.Add(buttonStartDownload);
            groupBox4Download.Controls.Add(labelProgress);
            groupBox4Download.Controls.Add(progressBar1);
            groupBox4Download.Location = new Point(962, 237);
            groupBox4Download.Name = "groupBox4Download";
            groupBox4Download.Size = new Size(217, 179);
            groupBox4Download.TabIndex = 4;
            groupBox4Download.TabStop = false;
            groupBox4Download.Text = "4. Generowanie trasy";
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
            labelProgress.Size = new Size(130, 15);
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
            // TerrainGridToolForm
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
            Name = "TerrainGridToolForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Generator terenu (map.gnd)";
            FormClosing += TerrainGridToolForm_FormClosing;
            Load += TerrainGridToolForm_Load;
            groupBox1Selection.ResumeLayout(false);
            groupBox1Selection.PerformLayout();
            groupBox2CoordSystem.ResumeLayout(false);
            groupBox2CoordSystem.PerformLayout();
            groupBoxMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            groupBox3Config.ResumeLayout(false);
            groupBox3Config.PerformLayout();
            groupBoxElevation.ResumeLayout(false);
            groupBoxElevation.PerformLayout();
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
        private GroupBox groupBox4Download;
        private ProgressBar progressBar1;
        private Label labelProgress;
        private Button buttonStartDownload;
        private Button buttonCancel;
        private StatusStrip statusStrip1;
        private ToolStripStatusLabel toolStripStatusLabel1;
    }
}
