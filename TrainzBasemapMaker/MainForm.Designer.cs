namespace TrainzBasemapMaker
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            label1 = new Label();
            textBoxLat = new TextBox();
            buttonConfAndDownload = new Button();
            textBoxLon = new TextBox();
            buttonUp = new Button();
            buttonLeft = new Button();
            buttonRight = new Button();
            buttonDown = new Button();
            label2 = new Label();
            groupBox2Configurator = new GroupBox();
            textBoxBasemapDate = new TextBox();
            label14 = new Label();
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
            radioButton512 = new RadioButton();
            checkBoxCreateFiles = new CheckBox();
            radioButton1024 = new RadioButton();
            label8 = new Label();
            radioButton2048 = new RadioButton();
            label7 = new Label();
            radioButton4096 = new RadioButton();
            label9 = new Label();
            textBoxY = new TextBox();
            textBoxX = new TextBox();
            buttonConvert = new Button();
            groupBox3Navigator = new GroupBox();
            label3 = new Label();
            pictureBox1 = new PictureBox();
            label5 = new Label();
            label6 = new Label();
            groupBox1Converter = new GroupBox();
            kuidsInFolderListBox = new ListBox();
            groupBox5BasemapViewer = new GroupBox();
            groupBox4KuidList = new GroupBox();
            menuStrip1 = new MenuStrip();
            narzędziaToolStripMenuItem = new ToolStripMenuItem();
            odświerzListęFoldrówIPodkładówToolStripMenuItem = new ToolStripMenuItem();
            znajdźNajmniejszyWolnyNrPodkładuToolStripMenuItem = new ToolStripMenuItem();
            znajdźWolnyKuidToolStripMenuItem = new ToolStripMenuItem();
            preferencjeToolStripMenuItem = new ToolStripMenuItem();
            pomocToolStripMenuItem = new ToolStripMenuItem();
            stronaProgramuToolStripMenuItem = new ToolStripMenuItem();
            informacjeOProgramieToolStripMenuItem = new ToolStripMenuItem();
            statusStrip1 = new StatusStrip();
            toolStripStatusLabel1 = new ToolStripStatusLabel();
            groupBox2Configurator.SuspendLayout();
            panel1.SuspendLayout();
            groupBox3Navigator.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            groupBox1Converter.SuspendLayout();
            groupBox5BasemapViewer.SuspendLayout();
            groupBox4KuidList.SuspendLayout();
            menuStrip1.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 21);
            label1.Name = "label1";
            label1.Size = new Size(149, 15);
            label1.TabIndex = 0;
            label1.Text = "Współrzędne geograficzne:";
            // 
            // textBoxLat
            // 
            textBoxLat.Location = new Point(6, 39);
            textBoxLat.Name = "textBoxLat";
            textBoxLat.Size = new Size(100, 23);
            textBoxLat.TabIndex = 1;
            // 
            // buttonConfAndDownload
            // 
            buttonConfAndDownload.Location = new Point(6, 530);
            buttonConfAndDownload.Name = "buttonConfAndDownload";
            buttonConfAndDownload.Size = new Size(188, 23);
            buttonConfAndDownload.TabIndex = 2;
            buttonConfAndDownload.Text = "Skonfiguruj i pobierz";
            buttonConfAndDownload.UseVisualStyleBackColor = true;
            buttonConfAndDownload.Click += buttonConfAndDownload_Click;
            // 
            // textBoxLon
            // 
            textBoxLon.Location = new Point(6, 68);
            textBoxLon.Name = "textBoxLon";
            textBoxLon.Size = new Size(100, 23);
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
            label2.Location = new Point(6, 98);
            label2.Name = "label2";
            label2.Size = new Size(93, 30);
            label2.TabIndex = 9;
            label2.Text = "Rozdzielczość\r\npodkładów [px]:";
            // 
            // groupBox2Configurator
            // 
            groupBox2Configurator.Controls.Add(textBoxBasemapDate);
            groupBox2Configurator.Controls.Add(label14);
            groupBox2Configurator.Controls.Add(panel1);
            groupBox2Configurator.Controls.Add(radioButton512);
            groupBox2Configurator.Controls.Add(checkBoxCreateFiles);
            groupBox2Configurator.Controls.Add(radioButton1024);
            groupBox2Configurator.Controls.Add(label8);
            groupBox2Configurator.Controls.Add(radioButton2048);
            groupBox2Configurator.Controls.Add(label7);
            groupBox2Configurator.Controls.Add(radioButton4096);
            groupBox2Configurator.Controls.Add(label9);
            groupBox2Configurator.Controls.Add(textBoxY);
            groupBox2Configurator.Controls.Add(textBoxX);
            groupBox2Configurator.Controls.Add(buttonConfAndDownload);
            groupBox2Configurator.Controls.Add(label2);
            groupBox2Configurator.Location = new Point(12, 159);
            groupBox2Configurator.Name = "groupBox2Configurator";
            groupBox2Configurator.Size = new Size(200, 559);
            groupBox2Configurator.TabIndex = 11;
            groupBox2Configurator.TabStop = false;
            groupBox2Configurator.Text = "2. Konfiguracja";
            // 
            // textBoxBasemapDate
            // 
            textBoxBasemapDate.Location = new Point(126, 131);
            textBoxBasemapDate.Name = "textBoxBasemapDate";
            textBoxBasemapDate.Size = new Size(59, 23);
            textBoxBasemapDate.TabIndex = 30;
            textBoxBasemapDate.TextAlign = HorizontalAlignment.Center;
            textBoxBasemapDate.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(126, 98);
            label14.Name = "label14";
            label14.Size = new Size(69, 30);
            label14.TabIndex = 21;
            label14.Text = "Rok\r\npodkładów:";
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
            panel1.Location = new Point(13, 261);
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
            textBoxDestinationFolder.Size = new Size(170, 23);
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
            // radioButton512
            // 
            radioButton512.AutoSize = true;
            radioButton512.Location = new Point(13, 206);
            radioButton512.Name = "radioButton512";
            radioButton512.Size = new Size(73, 19);
            radioButton512.TabIndex = 19;
            radioButton512.TabStop = true;
            radioButton512.Text = "512 x 512";
            radioButton512.UseVisualStyleBackColor = true;
            radioButton512.CheckedChanged += radioButtons_CheckedChanged;
            // 
            // checkBoxCreateFiles
            // 
            checkBoxCreateFiles.AutoSize = true;
            checkBoxCreateFiles.Location = new Point(6, 236);
            checkBoxCreateFiles.Name = "checkBoxCreateFiles";
            checkBoxCreateFiles.Size = new Size(160, 19);
            checkBoxCreateFiles.TabIndex = 18;
            checkBoxCreateFiles.Text = "Twórz foldery i pliki Trainz";
            checkBoxCreateFiles.UseVisualStyleBackColor = true;
            checkBoxCreateFiles.CheckedChanged += checkBoxCreateFiles_CheckedChanged;
            // 
            // radioButton1024
            // 
            radioButton1024.AutoSize = true;
            radioButton1024.Location = new Point(13, 181);
            radioButton1024.Name = "radioButton1024";
            radioButton1024.Size = new Size(85, 19);
            radioButton1024.TabIndex = 18;
            radioButton1024.TabStop = true;
            radioButton1024.Text = "1024 x 1024";
            radioButton1024.UseVisualStyleBackColor = true;
            radioButton1024.CheckedChanged += radioButtons_CheckedChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(112, 71);
            label8.Name = "label8";
            label8.Size = new Size(77, 15);
            label8.TabIndex = 17;
            label8.Text = "długość (lon)";
            // 
            // radioButton2048
            // 
            radioButton2048.AutoSize = true;
            radioButton2048.Location = new Point(13, 156);
            radioButton2048.Name = "radioButton2048";
            radioButton2048.Size = new Size(85, 19);
            radioButton2048.TabIndex = 17;
            radioButton2048.TabStop = true;
            radioButton2048.Text = "2048 x 2048";
            radioButton2048.UseVisualStyleBackColor = true;
            radioButton2048.CheckedChanged += radioButtons_CheckedChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(6, 21);
            label7.Name = "label7";
            label7.Size = new Size(132, 15);
            label7.TabIndex = 16;
            label7.Text = "Współrzędne EPSG:2180";
            // 
            // radioButton4096
            // 
            radioButton4096.AutoSize = true;
            radioButton4096.Location = new Point(13, 131);
            radioButton4096.Name = "radioButton4096";
            radioButton4096.Size = new Size(85, 19);
            radioButton4096.TabIndex = 16;
            radioButton4096.TabStop = true;
            radioButton4096.Text = "4096 x 4096";
            radioButton4096.UseVisualStyleBackColor = true;
            radioButton4096.CheckedChanged += radioButtons_CheckedChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(112, 47);
            label9.Name = "label9";
            label9.Size = new Size(82, 15);
            label9.TabIndex = 16;
            label9.Text = "szerokość (lat)";
            // 
            // textBoxY
            // 
            textBoxY.Location = new Point(6, 67);
            textBoxY.Name = "textBoxY";
            textBoxY.Size = new Size(100, 23);
            textBoxY.TabIndex = 12;
            // 
            // textBoxX
            // 
            textBoxX.Location = new Point(6, 39);
            textBoxX.Name = "textBoxX";
            textBoxX.Size = new Size(100, 23);
            textBoxX.TabIndex = 11;
            // 
            // buttonConvert
            // 
            buttonConvert.Location = new Point(6, 97);
            buttonConvert.Name = "buttonConvert";
            buttonConvert.Size = new Size(188, 23);
            buttonConvert.TabIndex = 13;
            buttonConvert.Text = "Konwertuj na EPSG:2180";
            buttonConvert.UseVisualStyleBackColor = true;
            buttonConvert.Click += buttonConvert_Click;
            // 
            // groupBox3Navigator
            // 
            groupBox3Navigator.Controls.Add(label3);
            groupBox3Navigator.Controls.Add(buttonLeft);
            groupBox3Navigator.Controls.Add(buttonUp);
            groupBox3Navigator.Controls.Add(buttonDown);
            groupBox3Navigator.Controls.Add(buttonRight);
            groupBox3Navigator.Location = new Point(896, 27);
            groupBox3Navigator.Name = "groupBox3Navigator";
            groupBox3Navigator.Size = new Size(217, 159);
            groupBox3Navigator.TabIndex = 12;
            groupBox3Navigator.TabStop = false;
            groupBox3Navigator.Text = "3. Nawigacja";
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
            pictureBox1.Location = new Point(6, 28);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(660, 660);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 13;
            pictureBox1.TabStop = false;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(112, 47);
            label5.Name = "label5";
            label5.Size = new Size(82, 15);
            label5.TabIndex = 14;
            label5.Text = "szerokość (lat)";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(112, 71);
            label6.Name = "label6";
            label6.Size = new Size(77, 15);
            label6.TabIndex = 15;
            label6.Text = "długość (lon)";
            // 
            // groupBox1Converter
            // 
            groupBox1Converter.Controls.Add(label6);
            groupBox1Converter.Controls.Add(label5);
            groupBox1Converter.Controls.Add(label1);
            groupBox1Converter.Controls.Add(buttonConvert);
            groupBox1Converter.Controls.Add(textBoxLon);
            groupBox1Converter.Controls.Add(textBoxLat);
            groupBox1Converter.Location = new Point(12, 27);
            groupBox1Converter.Name = "groupBox1Converter";
            groupBox1Converter.Size = new Size(200, 126);
            groupBox1Converter.TabIndex = 15;
            groupBox1Converter.TabStop = false;
            groupBox1Converter.Text = "1. Konwerter";
            // 
            // kuidsInFolderListBox
            // 
            kuidsInFolderListBox.FormattingEnabled = true;
            kuidsInFolderListBox.ItemHeight = 15;
            kuidsInFolderListBox.Location = new Point(6, 21);
            kuidsInFolderListBox.Name = "kuidsInFolderListBox";
            kuidsInFolderListBox.Size = new Size(205, 499);
            kuidsInFolderListBox.TabIndex = 16;
            kuidsInFolderListBox.SelectedIndexChanged += kuidsInFolderListBox_SelectedIndexChanged;
            kuidsInFolderListBox.DoubleClick += kuidsInFolderListBox_DoubleClick;
            // 
            // groupBox5BasemapViewer
            // 
            groupBox5BasemapViewer.Controls.Add(pictureBox1);
            groupBox5BasemapViewer.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point, 238);
            groupBox5BasemapViewer.Location = new Point(218, 24);
            groupBox5BasemapViewer.Name = "groupBox5BasemapViewer";
            groupBox5BasemapViewer.Size = new Size(672, 694);
            groupBox5BasemapViewer.TabIndex = 17;
            groupBox5BasemapViewer.TabStop = false;
            groupBox5BasemapViewer.Text = "Podgląd pobranego podkładu:";
            // 
            // groupBox4KuidList
            // 
            groupBox4KuidList.Controls.Add(kuidsInFolderListBox);
            groupBox4KuidList.Location = new Point(896, 192);
            groupBox4KuidList.Name = "groupBox4KuidList";
            groupBox4KuidList.Size = new Size(217, 526);
            groupBox4KuidList.TabIndex = 18;
            groupBox4KuidList.TabStop = false;
            groupBox4KuidList.Text = "Lista podkładów do Trainz:";
            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { narzędziaToolStripMenuItem, preferencjeToolStripMenuItem, pomocToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(1125, 24);
            menuStrip1.TabIndex = 19;
            menuStrip1.Text = "menuStrip1";
            // 
            // narzędziaToolStripMenuItem
            // 
            narzędziaToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { odświerzListęFoldrówIPodkładówToolStripMenuItem, znajdźNajmniejszyWolnyNrPodkładuToolStripMenuItem, znajdźWolnyKuidToolStripMenuItem });
            narzędziaToolStripMenuItem.Name = "narzędziaToolStripMenuItem";
            narzędziaToolStripMenuItem.Size = new Size(70, 20);
            narzędziaToolStripMenuItem.Text = "&Narzędzia";
            // 
            // odświerzListęFoldrówIPodkładówToolStripMenuItem
            // 
            odświerzListęFoldrówIPodkładówToolStripMenuItem.Name = "odświerzListęFoldrówIPodkładówToolStripMenuItem";
            odświerzListęFoldrówIPodkładówToolStripMenuItem.Size = new Size(276, 22);
            odświerzListęFoldrówIPodkładówToolStripMenuItem.Text = "&Odświerz listę foldrów i podkładów";
            odświerzListęFoldrówIPodkładówToolStripMenuItem.Click += odświerzListęFoldrówIPodkładówToolStripMenuItem_Click;
            // 
            // znajdźNajmniejszyWolnyNrPodkładuToolStripMenuItem
            // 
            znajdźNajmniejszyWolnyNrPodkładuToolStripMenuItem.Name = "znajdźNajmniejszyWolnyNrPodkładuToolStripMenuItem";
            znajdźNajmniejszyWolnyNrPodkładuToolStripMenuItem.Size = new Size(276, 22);
            znajdźNajmniejszyWolnyNrPodkładuToolStripMenuItem.Text = "Znajdź najmniejszy wolny nr &podkładu";
            znajdźNajmniejszyWolnyNrPodkładuToolStripMenuItem.Click += znajdźNajmniejszyWolnyNrPodkładuToolStripMenuItem_Click;
            // 
            // znajdźWolnyKuidToolStripMenuItem
            // 
            znajdźWolnyKuidToolStripMenuItem.Name = "znajdźWolnyKuidToolStripMenuItem";
            znajdźWolnyKuidToolStripMenuItem.Size = new Size(276, 22);
            znajdźWolnyKuidToolStripMenuItem.Text = "Znajdź wolny &Kuid";
            znajdźWolnyKuidToolStripMenuItem.Click += znajdźWolnyKuidToolStripMenuItem_Click;
            // 
            // preferencjeToolStripMenuItem
            // 
            preferencjeToolStripMenuItem.Name = "preferencjeToolStripMenuItem";
            preferencjeToolStripMenuItem.Size = new Size(78, 20);
            preferencjeToolStripMenuItem.Text = "P&referencje";
            preferencjeToolStripMenuItem.Click += preferencjeToolStripMenuItem_Click;
            // 
            // pomocToolStripMenuItem
            // 
            pomocToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { stronaProgramuToolStripMenuItem, informacjeOProgramieToolStripMenuItem });
            pomocToolStripMenuItem.Name = "pomocToolStripMenuItem";
            pomocToolStripMenuItem.Size = new Size(57, 20);
            pomocToolStripMenuItem.Text = "&Pomoc";
            // 
            // stronaProgramuToolStripMenuItem
            // 
            stronaProgramuToolStripMenuItem.Name = "stronaProgramuToolStripMenuItem";
            stronaProgramuToolStripMenuItem.Size = new Size(213, 22);
            stronaProgramuToolStripMenuItem.Text = "&Strona programu - GitHub";
            stronaProgramuToolStripMenuItem.Click += stronaProgramuToolStripMenuItem_Click;
            // 
            // informacjeOProgramieToolStripMenuItem
            // 
            informacjeOProgramieToolStripMenuItem.Name = "informacjeOProgramieToolStripMenuItem";
            informacjeOProgramieToolStripMenuItem.Size = new Size(213, 22);
            informacjeOProgramieToolStripMenuItem.Text = "&O programie";
            informacjeOProgramieToolStripMenuItem.Click += informacjeOProgramieToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel1 });
            statusStrip1.Location = new Point(0, 727);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new Size(1125, 22);
            statusStrip1.SizingGrip = false;
            statusStrip1.TabIndex = 20;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            toolStripStatusLabel1.Margin = new Padding(10, 3, 0, 2);
            toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            toolStripStatusLabel1.Size = new Size(0, 17);
            // 
            // MainForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1125, 749);
            Controls.Add(statusStrip1);
            Controls.Add(groupBox4KuidList);
            Controls.Add(groupBox5BasemapViewer);
            Controls.Add(groupBox1Converter);
            Controls.Add(groupBox3Navigator);
            Controls.Add(groupBox2Configurator);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = menuStrip1;
            MaximizeBox = false;
            Name = "MainForm";
            Text = "Trainz Basemap Maker";
            groupBox2Configurator.ResumeLayout(false);
            groupBox2Configurator.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            groupBox3Navigator.ResumeLayout(false);
            groupBox3Navigator.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            groupBox1Converter.ResumeLayout(false);
            groupBox1Converter.PerformLayout();
            groupBox5BasemapViewer.ResumeLayout(false);
            groupBox4KuidList.ResumeLayout(false);
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private TextBox textBoxLat;
        private Button buttonConfAndDownload;
        private TextBox textBoxLon;
        private Button buttonUp;
        private Button buttonLeft;
        private Button buttonRight;
        private Button buttonDown;
        private Label label2;
        private GroupBox groupBox2Configurator;
        private GroupBox groupBox3Navigator;
        private Label label3;
        private PictureBox pictureBox1;
        private TextBox textBoxY;
        private TextBox textBoxX;
        private Button buttonConvert;
        private Label label6;
        private Label label5;
        private GroupBox groupBox1Converter;
        private Label label8;
        private Label label7;
        private Label label9;
        private CheckBox checkBoxCreateFiles;
        private RadioButton radioButton4096;
        private RadioButton radioButton2048;
        private RadioButton radioButton512;
        private RadioButton radioButton1024;
        private ListBox kuidsInFolderListBox;
        private GroupBox groupBox5BasemapViewer;
        private GroupBox groupBox4KuidList;
        private MenuStrip menuStrip1;
        private ToolStripMenuItem pomocToolStripMenuItem;
        private ToolStripMenuItem stronaProgramuToolStripMenuItem;
        private ToolStripMenuItem informacjeOProgramieToolStripMenuItem;
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
        private ToolStripMenuItem narzędziaToolStripMenuItem;
        private ToolStripMenuItem znajdźWolnyKuidToolStripMenuItem;
        private ToolStripMenuItem znajdźNajmniejszyWolnyNrPodkładuToolStripMenuItem;
        private ToolStripMenuItem odświerzListęFoldrówIPodkładówToolStripMenuItem;
        private TextBox textBoxBasemapDate;
        private Label label14;
        private ToolStripMenuItem preferencjeToolStripMenuItem;
    }
}
