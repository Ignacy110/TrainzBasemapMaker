namespace TrainzBasemapMaker
{
    partial class PreferencesForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PreferencesForm));
            checkBoxAutoCounter = new CheckBox();
            checkBoxAutoKuid = new CheckBox();
            checkBoxKuidAutoCountPerFirstPart = new CheckBox();
            textBoxDefaultKuidFirstPart = new TextBox();
            label1 = new Label();
            textBoxMinKuidPart2 = new TextBox();
            labelMinKuidPart2 = new Label();
            label2 = new Label();
            checkBoxDarkMode = new CheckBox();
            comboBoxBasemapSize = new ComboBox();
            label3 = new Label();
            SuspendLayout();
            // 
            // checkBoxAutoCounter
            // 
            checkBoxAutoCounter.AutoSize = true;
            checkBoxAutoCounter.Location = new Point(38, 64);
            checkBoxAutoCounter.Name = "checkBoxAutoCounter";
            checkBoxAutoCounter.Size = new Size(265, 19);
            checkBoxAutoCounter.TabIndex = 0;
            checkBoxAutoCounter.Text = "Automatyczne zwiększanie numeru podkładu";
            checkBoxAutoCounter.UseVisualStyleBackColor = true;
            // 
            // checkBoxAutoKuid
            // 
            checkBoxAutoKuid.AutoSize = true;
            checkBoxAutoKuid.Location = new Point(38, 89);
            checkBoxAutoKuid.Name = "checkBoxAutoKuid";
            checkBoxAutoKuid.Size = new Size(331, 19);
            checkBoxAutoKuid.TabIndex = 1;
            checkBoxAutoKuid.Text = "Automatyczne ustawianie drugiej części oznaczenia Kuidu";
            checkBoxAutoKuid.UseVisualStyleBackColor = true;
            // 
            // checkBoxKuidAutoCountPerFirstPart
            // 
            checkBoxKuidAutoCountPerFirstPart.AutoSize = true;
            checkBoxKuidAutoCountPerFirstPart.Location = new Point(38, 114);
            checkBoxKuidAutoCountPerFirstPart.Name = "checkBoxKuidAutoCountPerFirstPart";
            checkBoxKuidAutoCountPerFirstPart.Size = new Size(335, 19);
            checkBoxKuidAutoCountPerFirstPart.TabIndex = 2;
            checkBoxKuidAutoCountPerFirstPart.Text = "Szukaj wolnego KUID part 2 osobno dla danego part 1";
            checkBoxKuidAutoCountPerFirstPart.UseVisualStyleBackColor = true;
            // 
            // textBoxDefaultKuidFirstPart
            // 
            textBoxDefaultKuidFirstPart.Location = new Point(38, 140);
            textBoxDefaultKuidFirstPart.Name = "textBoxDefaultKuidFirstPart";
            textBoxDefaultKuidFirstPart.Size = new Size(100, 23);
            textBoxDefaultKuidFirstPart.TabIndex = 3;
            textBoxDefaultKuidFirstPart.TextAlign = HorizontalAlignment.Right;
            textBoxDefaultKuidFirstPart.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(144, 143);
            label1.Name = "label1";
            label1.Size = new Size(240, 15);
            label1.TabIndex = 4;
            label1.Text = "Domyślne oznaczenie pierwszej części Kuidu";
            // 
            // textBoxMinKuidPart2
            // 
            textBoxMinKuidPart2.Location = new Point(38, 169);
            textBoxMinKuidPart2.Name = "textBoxMinKuidPart2";
            textBoxMinKuidPart2.Size = new Size(100, 23);
            textBoxMinKuidPart2.TabIndex = 5;
            textBoxMinKuidPart2.TextAlign = HorizontalAlignment.Right;
            textBoxMinKuidPart2.KeyPress += OnlyNumbers_KeyPress;
            // 
            // labelMinKuidPart2
            // 
            labelMinKuidPart2.AutoSize = true;
            labelMinKuidPart2.Location = new Point(144, 172);
            labelMinKuidPart2.Name = "labelMinKuidPart2";
            labelMinKuidPart2.Size = new Size(224, 15);
            labelMinKuidPart2.TabIndex = 6;
            labelMinKuidPart2.Text = "Minimalna wartość drugiej części Kuidu";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label2.Location = new Point(28, 21);
            label2.Name = "label2";
            label2.Size = new Size(124, 30);
            label2.TabIndex = 7;
            label2.Text = "Preferencje:";
            // 
            // checkBoxDarkMode
            // 
            checkBoxDarkMode.AutoSize = true;
            checkBoxDarkMode.Location = new Point(38, 201);
            checkBoxDarkMode.Name = "checkBoxDarkMode";
            checkBoxDarkMode.Size = new Size(100, 19);
            checkBoxDarkMode.TabIndex = 8;
            checkBoxDarkMode.Text = "Ciemny motyw";
            checkBoxDarkMode.UseVisualStyleBackColor = true;
            // 
            // 
            // comboBoxBasemapSize
            // 
            comboBoxBasemapSize.DropDownStyle = ComboBoxStyle.DropDownList;
            comboBoxBasemapSize.FormattingEnabled = true;
            comboBoxBasemapSize.Items.AddRange(new object[] {
            "500",
            "720"});
            comboBoxBasemapSize.Location = new Point(38, 230);
            comboBoxBasemapSize.Name = "comboBoxBasemapSize";
            comboBoxBasemapSize.Size = new Size(121, 23);
            comboBoxBasemapSize.TabIndex = 9;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(165, 233);
            label3.Name = "label3";
            label3.Size = new Size(124, 15);
            label3.TabIndex = 10;
            label3.Text = "Rozmiar podkładu (m)";
            // 
            // PreferencesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(414, 274);
            Controls.Add(label3);
            Controls.Add(comboBoxBasemapSize);
            Controls.Add(checkBoxDarkMode);
            Controls.Add(label2);
            Controls.Add(labelMinKuidPart2);
            Controls.Add(textBoxMinKuidPart2);
            Controls.Add(label1);
            Controls.Add(textBoxDefaultKuidFirstPart);
            Controls.Add(checkBoxKuidAutoCountPerFirstPart);
            Controls.Add(checkBoxAutoKuid);
            Controls.Add(checkBoxAutoCounter);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "PreferencesForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "Preferencje";
            FormClosing += PreferencesFormClosing;
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private CheckBox checkBoxAutoCounter;
        private CheckBox checkBoxAutoKuid;
        private CheckBox checkBoxKuidAutoCountPerFirstPart;
        private TextBox textBoxDefaultKuidFirstPart;
        private Label label1;
        private TextBox textBoxMinKuidPart2;
        private Label labelMinKuidPart2;
        private Label label2;
        private CheckBox checkBoxDarkMode;
        private ComboBox comboBoxBasemapSize;
        private Label label3;
    }
}