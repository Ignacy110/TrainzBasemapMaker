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
            textBoxDefaultKuidFirstPart = new TextBox();
            label1 = new Label();
            label2 = new Label();
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
            // textBoxDefaultKuidFirstPart
            // 
            textBoxDefaultKuidFirstPart.Location = new Point(38, 114);
            textBoxDefaultKuidFirstPart.Name = "textBoxDefaultKuidFirstPart";
            textBoxDefaultKuidFirstPart.Size = new Size(100, 23);
            textBoxDefaultKuidFirstPart.TabIndex = 2;
            textBoxDefaultKuidFirstPart.TextAlign = HorizontalAlignment.Right;
            textBoxDefaultKuidFirstPart.KeyPress += OnlyNumbers_KeyPress;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(144, 117);
            label1.Name = "label1";
            label1.Size = new Size(240, 15);
            label1.TabIndex = 3;
            label1.Text = "Domyślne oznaczenie pierwszej części Kuidu";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI Semibold", 15.75F, FontStyle.Bold, GraphicsUnit.Point, 238);
            label2.Location = new Point(28, 21);
            label2.Name = "label2";
            label2.Size = new Size(124, 30);
            label2.TabIndex = 4;
            label2.Text = "Preferencje:";
            // 
            // PreferencesForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(414, 181);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(textBoxDefaultKuidFirstPart);
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
        private TextBox textBoxDefaultKuidFirstPart;
        private Label label1;
        private Label label2;
    }
}