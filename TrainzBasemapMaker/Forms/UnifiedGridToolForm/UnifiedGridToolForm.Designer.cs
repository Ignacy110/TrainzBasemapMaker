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
    partial class UnifiedGridToolForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(UnifiedGridToolForm));
            groupBox1CoordSystem = new GroupBox();
            comboBoxEpsg = new ComboBox();
            groupBox2Selection = new GroupBox();
            labelArea = new Label();
            labelTileCount = new Label();
            buttonResetAnchor = new Button();
            buttonClearSelection = new Button();
            radioButtonModeBox = new RadioButton();
            radioButtonModeClick = new RadioButton();
            groupBoxMap = new GroupBox();
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            groupBox3BasemapParams = new GroupBox();
            comboBoxResolution = new ComboBox();
            label2 = new Label();
            checkedListBoxMapType = new CheckedListBox();
            buttonSelectAllMaps = new Button();
            buttonDeselectAllMaps = new Button();
            labelMapSelectionCount = new Label();
            label15 = new Label();
            textBoxBasemapDate = new TextBox();
            label14 = new Label();
            groupBox4Configurator = new GroupBox();
            labelRoutes = new Label();
            listBoxRoutes = new ListBox();
            buttonLoadRoute = new Button();
            buttonDeleteRoute = new Button();
            panelTrainzFiles = new Panel();
            label4 = new Label();
            textBoxDestinationFolder = new TextBox();
            labelDesignation = new Label();
            textBoxDesignation = new TextBox();
            labelCounter = new Label();
            textBoxCounter = new TextBox();
            label12 = new Label();
            textBoxKuidPart1 = new TextBox();
            labelKuidSeparator = new Label();
            textBoxKuidPart2 = new TextBox();
            groupBox5Download = new GroupBox();
            checkBoxGenerate2DBasemaps = new CheckBox();
            checkBoxPlace2DOnMap = new CheckBox();
            checkBoxGenerate3DBasemaps = new CheckBox();
            checkBoxPlace3DOnMap = new CheckBox();
            checkBoxGenerate3DTerrain = new CheckBox();
            groupBoxElevation = new GroupBox();
            radioButtonElevationAbsolute = new RadioButton();
            radioButtonElevationRelative = new RadioButton();
            progressBar1 = new ProgressBar();
            labelProgress = new Label();
            buttonStartDownload = new Button();
            buttonCancel = new Button();
            groupBox1CoordSystem.SuspendLayout();
            groupBox2Selection.SuspendLayout();
            groupBoxMap.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            groupBox3BasemapParams.SuspendLayout();
            groupBox4Configurator.SuspendLayout();
            panelTrainzFiles.SuspendLayout();
            groupBox5Download.SuspendLayout();
            groupBoxElevation.SuspendLayout();
            SuspendLayout();
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
            labelTileCount.Size = new Size(162, 15);
            labelTileCount.TabIndex = 4;
            labelTileCount.Text = "Zaznaczono baseboardów: 0";
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
            // groupBoxMap
            // 
            groupBoxMap.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            groupBoxMap.Controls.Add(webView21);
            groupBoxMap.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            groupBoxMap.Location = new Point(232, 163);
            groupBoxMap.Name = "groupBoxMap";
            groupBoxMap.Size = new Size(580, 520);
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
            webView21.Size = new Size(568, 486);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // groupBox3BasemapParams
            // 
            groupBox3BasemapParams.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            groupBox3BasemapParams.Controls.Add(buttonDeselectAllMaps);
            groupBox3BasemapParams.Controls.Add(buttonSelectAllMaps);
            groupBox3BasemapParams.Controls.Add(labelMapSelectionCount);
            groupBox3BasemapParams.Controls.Add(checkedListBoxMapType);
            groupBox3BasemapParams.Controls.Add(comboBoxResolution);
            groupBox3BasemapParams.Controls.Add(label2);
            groupBox3BasemapParams.Controls.Add(label15);
            groupBox3BasemapParams.Controls.Add(textBoxBasemapDate);
            groupBox3BasemapParams.Controls.Add(label14);
            groupBox3BasemapParams.Location = new Point(232, 12);
            groupBox3BasemapParams.Name = "groupBox3BasemapParams";
            groupBox3BasemapParams.Size = new Size(580, 145);
            groupBox3BasemapParams.TabIndex = 6;
            groupBox3BasemapParams.TabStop = false;
            groupBox3BasemapParams.Text = "3. Parametry podkładów";
            // 
            // comboBoxResolution
            // 
            comboBoxResolution.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxResolution.FormattingEnabled = true;
            comboBoxResolution.Location = new Point(6, 42);
            comboBoxResolution.Name = "comboBoxResolution";
            comboBoxResolution.Size = new Size(125, 23);
            comboBoxResolution.TabIndex = 34;
            comboBoxResolution.SelectedIndexChanged += comboBoxResolution_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 22);
            label2.Name = "label2";
            label2.Size = new Size(116, 15);
            label2.TabIndex = 33;
            label2.Text = "Maks. rozdzielczość:";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(6, 75);
            label14.Name = "label14";
            label14.Size = new Size(117, 15);
            label14.TabIndex = 35;
            label14.Text = "Rok (jeśli dostępny):";
            // 
            // textBoxBasemapDate
            // 
            textBoxBasemapDate.Location = new Point(6, 95);
            textBoxBasemapDate.MaxLength = 4;
            textBoxBasemapDate.Name = "textBoxBasemapDate";
            textBoxBasemapDate.Size = new Size(125, 23);
            textBoxBasemapDate.TabIndex = 36;
            textBoxBasemapDate.TextAlign = HorizontalAlignment.Center;
            textBoxBasemapDate.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(142, 22);
            label15.Name = "label15";
            label15.Size = new Size(335, 15);
            label15.TabIndex = 37;
            label15.Text = "Rodzaje podkładów (każdy utworzy osobną warstwę w Trainz):";
            // 
            // checkedListBoxMapType
            // 
            checkedListBoxMapType.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right;
            checkedListBoxMapType.CheckOnClick = true;
            checkedListBoxMapType.FormattingEnabled = true;
            checkedListBoxMapType.Location = new Point(142, 42);
            checkedListBoxMapType.Name = "checkedListBoxMapType";
            checkedListBoxMapType.Size = new Size(428, 70);
            checkedListBoxMapType.TabIndex = 38;
            checkedListBoxMapType.ItemCheck += checkedListBoxMapType_ItemCheck;
            // 
            // buttonSelectAllMaps
            // 
            buttonSelectAllMaps.Location = new Point(142, 116);
            buttonSelectAllMaps.Name = "buttonSelectAllMaps";
            buttonSelectAllMaps.Size = new Size(110, 23);
            buttonSelectAllMaps.TabIndex = 39;
            buttonSelectAllMaps.Text = "Zaznacz wszystkie";
            buttonSelectAllMaps.UseVisualStyleBackColor = true;
            buttonSelectAllMaps.Click += buttonSelectAllMaps_Click;
            // 
            // buttonDeselectAllMaps
            // 
            buttonDeselectAllMaps.Location = new Point(258, 116);
            buttonDeselectAllMaps.Name = "buttonDeselectAllMaps";
            buttonDeselectAllMaps.Size = new Size(110, 23);
            buttonDeselectAllMaps.TabIndex = 40;
            buttonDeselectAllMaps.Text = "Odznacz wszystkie";
            buttonDeselectAllMaps.UseVisualStyleBackColor = true;
            buttonDeselectAllMaps.Click += buttonDeselectAllMaps_Click;
            // 
            // labelMapSelectionCount
            // 
            labelMapSelectionCount.AutoSize = true;
            labelMapSelectionCount.Location = new Point(378, 120);
            labelMapSelectionCount.Name = "labelMapSelectionCount";
            labelMapSelectionCount.Size = new Size(67, 15);
            labelMapSelectionCount.TabIndex = 41;
            labelMapSelectionCount.Text = "Wybrano: 1";
            // 
            // groupBox4Configurator
            // 
            groupBox4Configurator.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox4Configurator.Controls.Add(labelRoutes);
            groupBox4Configurator.Controls.Add(listBoxRoutes);
            groupBox4Configurator.Controls.Add(buttonLoadRoute);
            groupBox4Configurator.Controls.Add(buttonDeleteRoute);
            groupBox4Configurator.Controls.Add(panelTrainzFiles);
            groupBox4Configurator.Location = new Point(818, 12);
            groupBox4Configurator.Name = "groupBox4Configurator";
            groupBox4Configurator.Size = new Size(217, 340);
            groupBox4Configurator.TabIndex = 3;
            groupBox4Configurator.TabStop = false;
            groupBox4Configurator.Text = "4. Konfiguracja";
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
            panelTrainzFiles.Controls.Add(label4);
            panelTrainzFiles.Controls.Add(textBoxDestinationFolder);
            panelTrainzFiles.Controls.Add(labelDesignation);
            panelTrainzFiles.Controls.Add(textBoxDesignation);
            panelTrainzFiles.Controls.Add(labelCounter);
            panelTrainzFiles.Controls.Add(textBoxCounter);
            panelTrainzFiles.Controls.Add(label12);
            panelTrainzFiles.Controls.Add(textBoxKuidPart1);
            panelTrainzFiles.Controls.Add(labelKuidSeparator);
            panelTrainzFiles.Controls.Add(textBoxKuidPart2);
            panelTrainzFiles.Location = new Point(6, 185);
            panelTrainzFiles.Name = "panelTrainzFiles";
            panelTrainzFiles.Size = new Size(205, 151);
            panelTrainzFiles.TabIndex = 4;
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
            // textBoxDestinationFolder
            // 
            textBoxDestinationFolder.Location = new Point(3, 23);
            textBoxDestinationFolder.Name = "textBoxDestinationFolder";
            textBoxDestinationFolder.Size = new Size(194, 23);
            textBoxDestinationFolder.TabIndex = 1;
            textBoxDestinationFolder.TextChanged += textBoxDestinationFolder_TextChanged;
            // 
            // labelDesignation
            // 
            labelDesignation.AutoSize = true;
            labelDesignation.Location = new Point(3, 55);
            labelDesignation.Name = "labelDesignation";
            labelDesignation.Size = new Size(87, 15);
            labelDesignation.TabIndex = 2;
            labelDesignation.Text = "Ozn. podkładu:";
            // 
            // textBoxDesignation
            // 
            textBoxDesignation.Location = new Point(3, 73);
            textBoxDesignation.Name = "textBoxDesignation";
            textBoxDesignation.Size = new Size(87, 23);
            textBoxDesignation.TabIndex = 3;
            // 
            // labelCounter
            // 
            labelCounter.AutoSize = true;
            labelCounter.Location = new Point(108, 55);
            labelCounter.Name = "labelCounter";
            labelCounter.Size = new Size(76, 15);
            labelCounter.TabIndex = 4;
            labelCounter.Text = "Nr podkładu:";
            // 
            // textBoxCounter
            // 
            textBoxCounter.Location = new Point(108, 73);
            textBoxCounter.Name = "textBoxCounter";
            textBoxCounter.Size = new Size(89, 23);
            textBoxCounter.TabIndex = 5;
            textBoxCounter.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(3, 105);
            label12.Name = "label12";
            label12.Size = new Size(111, 15);
            label12.TabIndex = 6;
            label12.Text = "Oznaczenie KUID-u:";
            // 
            // textBoxKuidPart1
            // 
            textBoxKuidPart1.Location = new Point(3, 123);
            textBoxKuidPart1.Name = "textBoxKuidPart1";
            textBoxKuidPart1.Size = new Size(87, 23);
            textBoxKuidPart1.TabIndex = 7;
            textBoxKuidPart1.TextAlign = HorizontalAlignment.Right;
            textBoxKuidPart1.KeyPress += OnlyNumbers_KeyPress;
            // 
            // labelKuidSeparator
            // 
            labelKuidSeparator.AutoSize = true;
            labelKuidSeparator.Location = new Point(94, 126);
            labelKuidSeparator.Name = "labelKuidSeparator";
            labelKuidSeparator.Size = new Size(10, 15);
            labelKuidSeparator.TabIndex = 8;
            labelKuidSeparator.Text = ":";
            // 
            // textBoxKuidPart2
            // 
            textBoxKuidPart2.Location = new Point(108, 123);
            textBoxKuidPart2.Name = "textBoxKuidPart2";
            textBoxKuidPart2.Size = new Size(89, 23);
            textBoxKuidPart2.TabIndex = 9;
            textBoxKuidPart2.KeyPress += OnlyNumbers_KeyPress;
            // 
            // groupBox5Download
            // 
            groupBox5Download.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            groupBox5Download.Controls.Add(checkBoxGenerate2DBasemaps);
            groupBox5Download.Controls.Add(checkBoxPlace2DOnMap);
            groupBox5Download.Controls.Add(checkBoxGenerate3DBasemaps);
            groupBox5Download.Controls.Add(checkBoxPlace3DOnMap);
            groupBox5Download.Controls.Add(checkBoxGenerate3DTerrain);
            groupBox5Download.Controls.Add(groupBoxElevation);
            groupBox5Download.Controls.Add(progressBar1);
            groupBox5Download.Controls.Add(labelProgress);
            groupBox5Download.Controls.Add(buttonStartDownload);
            groupBox5Download.Controls.Add(buttonCancel);
            groupBox5Download.Location = new Point(818, 358);
            groupBox5Download.Name = "groupBox5Download";
            groupBox5Download.Size = new Size(217, 325);
            groupBox5Download.TabIndex = 4;
            groupBox5Download.TabStop = false;
            groupBox5Download.Text = "5. Pobieranie i generowanie mapy";
            // 
            // checkBoxGenerate2DBasemaps
            // 
            checkBoxGenerate2DBasemaps.AutoSize = true;
            checkBoxGenerate2DBasemaps.Location = new Point(6, 22);
            checkBoxGenerate2DBasemaps.Name = "checkBoxGenerate2DBasemaps";
            checkBoxGenerate2DBasemaps.Size = new Size(136, 19);
            checkBoxGenerate2DBasemaps.TabIndex = 0;
            checkBoxGenerate2DBasemaps.Text = "Generuj podkłady 2D";
            checkBoxGenerate2DBasemaps.UseVisualStyleBackColor = true;
            checkBoxGenerate2DBasemaps.CheckedChanged += checkBoxGenerate2DBasemaps_CheckedChanged;
            // 
            // checkBoxPlace2DOnMap
            // 
            checkBoxPlace2DOnMap.AutoSize = true;
            checkBoxPlace2DOnMap.Enabled = false;
            checkBoxPlace2DOnMap.Location = new Point(20, 44);
            checkBoxPlace2DOnMap.Name = "checkBoxPlace2DOnMap";
            checkBoxPlace2DOnMap.Size = new Size(170, 19);
            checkBoxPlace2DOnMap.TabIndex = 1;
            checkBoxPlace2DOnMap.Text = "Ułóż podkłady 2D na mapie";
            checkBoxPlace2DOnMap.UseVisualStyleBackColor = true;
            // 
            // checkBoxGenerate3DBasemaps
            // 
            checkBoxGenerate3DBasemaps.AutoSize = true;
            checkBoxGenerate3DBasemaps.Location = new Point(6, 68);
            checkBoxGenerate3DBasemaps.Name = "checkBoxGenerate3DBasemaps";
            checkBoxGenerate3DBasemaps.Size = new Size(136, 19);
            checkBoxGenerate3DBasemaps.TabIndex = 2;
            checkBoxGenerate3DBasemaps.Text = "Generuj podkłady 3D";
            checkBoxGenerate3DBasemaps.UseVisualStyleBackColor = true;
            checkBoxGenerate3DBasemaps.CheckedChanged += checkBoxGenerate3DBasemaps_CheckedChanged;
            // 
            // checkBoxPlace3DOnMap
            // 
            checkBoxPlace3DOnMap.AutoSize = true;
            checkBoxPlace3DOnMap.Enabled = false;
            checkBoxPlace3DOnMap.Location = new Point(20, 90);
            checkBoxPlace3DOnMap.Name = "checkBoxPlace3DOnMap";
            checkBoxPlace3DOnMap.Size = new Size(170, 19);
            checkBoxPlace3DOnMap.TabIndex = 3;
            checkBoxPlace3DOnMap.Text = "Ułóż podkłady 3D na mapie";
            checkBoxPlace3DOnMap.UseVisualStyleBackColor = true;
            // 
            // checkBoxGenerate3DTerrain
            // 
            checkBoxGenerate3DTerrain.AutoSize = true;
            checkBoxGenerate3DTerrain.Location = new Point(6, 114);
            checkBoxGenerate3DTerrain.Name = "checkBoxGenerate3DTerrain";
            checkBoxGenerate3DTerrain.Size = new Size(114, 19);
            checkBoxGenerate3DTerrain.TabIndex = 4;
            checkBoxGenerate3DTerrain.Text = "Generuj teren 3D";
            checkBoxGenerate3DTerrain.UseVisualStyleBackColor = true;
            checkBoxGenerate3DTerrain.CheckedChanged += checkBoxGenerate3DTerrain_CheckedChanged;
            // 
            // groupBoxElevation
            // 
            groupBoxElevation.Controls.Add(radioButtonElevationAbsolute);
            groupBoxElevation.Controls.Add(radioButtonElevationRelative);
            groupBoxElevation.Enabled = false;
            groupBoxElevation.Location = new Point(6, 136);
            groupBoxElevation.Name = "groupBoxElevation";
            groupBoxElevation.Size = new Size(205, 68);
            groupBoxElevation.TabIndex = 5;
            groupBoxElevation.TabStop = false;
            groupBoxElevation.Text = "Tryb wysokości:";
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
            // radioButtonElevationRelative
            // 
            radioButtonElevationRelative.AutoSize = true;
            radioButtonElevationRelative.Location = new Point(6, 43);
            radioButtonElevationRelative.Name = "radioButtonElevationRelative";
            radioButtonElevationRelative.Size = new Size(157, 19);
            radioButtonElevationRelative.TabIndex = 1;
            radioButtonElevationRelative.Text = "Wyrównaj do zera (wzgl.)";
            radioButtonElevationRelative.UseVisualStyleBackColor = true;
            // 
            // progressBar1
            // 
            progressBar1.Location = new Point(6, 212);
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new Size(205, 23);
            progressBar1.TabIndex = 6;
            // 
            // labelProgress
            // 
            labelProgress.AutoSize = true;
            labelProgress.Location = new Point(6, 238);
            labelProgress.Name = "labelProgress";
            labelProgress.Size = new Size(127, 15);
            labelProgress.TabIndex = 7;
            labelProgress.Text = "Gotowy do pobierania.";
            // 
            // buttonStartDownload
            // 
            buttonStartDownload.Location = new Point(6, 254);
            buttonStartDownload.Name = "buttonStartDownload";
            buttonStartDownload.Size = new Size(205, 25);
            buttonStartDownload.TabIndex = 8;
            buttonStartDownload.Text = "Generuj / Pobierz";
            buttonStartDownload.UseVisualStyleBackColor = true;
            buttonStartDownload.Click += buttonStartDownload_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Enabled = false;
            buttonCancel.Location = new Point(6, 254);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(205, 14);
            buttonCancel.TabIndex = 9;
            buttonCancel.Text = "Anuluj";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // UnifiedGridToolForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1047, 700);
            Controls.Add(groupBox3BasemapParams);
            Controls.Add(groupBox5Download);
            Controls.Add(groupBox4Configurator);
            Controls.Add(groupBoxMap);
            Controls.Add(groupBox1CoordSystem);
            Controls.Add(groupBox2Selection);
            Icon = (Icon)resources.GetObject("$this.Icon");
            MinimumSize = new Size(1063, 739);
            Name = "UnifiedGridToolForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Pobieranie obszarowe i generowanie terenu";
            FormClosing += UnifiedGridToolForm_FormClosing;
            Load += UnifiedGridToolForm_Load;
            groupBox1CoordSystem.ResumeLayout(false);
            groupBox2Selection.ResumeLayout(false);
            groupBox2Selection.PerformLayout();
            groupBoxMap.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            groupBox3BasemapParams.ResumeLayout(false);
            groupBox3BasemapParams.PerformLayout();
            groupBox4Configurator.ResumeLayout(false);
            groupBox4Configurator.PerformLayout();
            panelTrainzFiles.ResumeLayout(false);
            panelTrainzFiles.PerformLayout();
            groupBox5Download.ResumeLayout(false);
            groupBox5Download.PerformLayout();
            groupBoxElevation.ResumeLayout(false);
            groupBoxElevation.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        // Left panel
        private GroupBox groupBox1CoordSystem;
        private ComboBox comboBoxEpsg;
        private GroupBox groupBox2Selection;
        private RadioButton radioButtonModeClick;
        private RadioButton radioButtonModeBox;
        private Button buttonClearSelection;
        private Button buttonResetAnchor;
        private Label labelTileCount;
        private Label labelArea;
        // Center map
        private GroupBox groupBoxMap;
        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        // Top basemap params
        private GroupBox groupBox3BasemapParams;
        private Label label2;
        private ComboBox comboBoxResolution;
        private Label label15;
        private CheckedListBox checkedListBoxMapType;
        private Button buttonSelectAllMaps;
        private Button buttonDeselectAllMaps;
        private Label labelMapSelectionCount;
        private Label label14;
        private TextBox textBoxBasemapDate;
        // Right configurator
        private GroupBox groupBox4Configurator;
        private Label labelRoutes;
        private ListBox listBoxRoutes;
        private Button buttonLoadRoute;
        private Button buttonDeleteRoute;
        private Panel panelTrainzFiles;
        private Label label4;
        private TextBox textBoxDestinationFolder;
        private Label labelDesignation;
        private TextBox textBoxDesignation;
        private Label labelCounter;
        private TextBox textBoxCounter;
        private Label label12;
        private TextBox textBoxKuidPart1;
        private Label labelKuidSeparator;
        private TextBox textBoxKuidPart2;
        // Right download/generate
        private GroupBox groupBox5Download;
        private CheckBox checkBoxGenerate2DBasemaps;
        private CheckBox checkBoxPlace2DOnMap;
        private CheckBox checkBoxGenerate3DBasemaps;
        private CheckBox checkBoxPlace3DOnMap;
        private CheckBox checkBoxGenerate3DTerrain;
        private GroupBox groupBoxElevation;
        private RadioButton radioButtonElevationAbsolute;
        private RadioButton radioButtonElevationRelative;
        private ProgressBar progressBar1;
        private Label labelProgress;
        private Button buttonStartDownload;
        private Button buttonCancel;
    }
}
