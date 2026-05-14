namespace TrainzBasemapMaker
{
    partial class MapPickerForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MapPickerForm));
            webView21 = new Microsoft.Web.WebView2.WinForms.WebView2();
            label5 = new Label();
            label1 = new Label();
            textBoxLat = new TextBox();
            label6 = new Label();
            textBoxLon = new TextBox();
            buttonCancel = new Button();
            buttonConfirm = new Button();
            ((System.ComponentModel.ISupportInitialize)webView21).BeginInit();
            SuspendLayout();
            // 
            // webView21
            // 
            webView21.AllowExternalDrop = true;
            webView21.CreationProperties = null;
            webView21.DefaultBackgroundColor = Color.White;
            webView21.Location = new Point(10, 9);
            webView21.Margin = new Padding(3, 2, 3, 2);
            webView21.Name = "webView21";
            webView21.Size = new Size(879, 538);
            webView21.TabIndex = 0;
            webView21.ZoomFactor = 1D;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(166, 579);
            label5.Name = "label5";
            label5.Size = new Size(82, 15);
            label5.TabIndex = 19;
            label5.Text = "szerokość (lat)";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(45, 558);
            label1.Name = "label1";
            label1.Size = new Size(149, 15);
            label1.TabIndex = 16;
            label1.Text = "Współrzędne geograficzne:";
            // 
            // textBoxLat
            // 
            textBoxLat.Location = new Point(45, 576);
            textBoxLat.Name = "textBoxLat";
            textBoxLat.Size = new Size(115, 23);
            textBoxLat.TabIndex = 17;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(393, 579);
            label6.Name = "label6";
            label6.Size = new Size(77, 15);
            label6.TabIndex = 22;
            label6.Text = "długość (lon)";
            // 
            // textBoxLon
            // 
            textBoxLon.Location = new Point(272, 576);
            textBoxLon.Name = "textBoxLon";
            textBoxLon.Size = new Size(115, 23);
            textBoxLon.TabIndex = 21;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new Point(640, 575);
            buttonCancel.Margin = new Padding(3, 2, 3, 2);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new Size(100, 22);
            buttonCancel.TabIndex = 23;
            buttonCancel.Text = "Anuluj";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // buttonConfirm
            // 
            buttonConfirm.Location = new Point(756, 575);
            buttonConfirm.Margin = new Padding(3, 2, 3, 2);
            buttonConfirm.Name = "buttonConfirm";
            buttonConfirm.Size = new Size(100, 22);
            buttonConfirm.TabIndex = 24;
            buttonConfirm.Text = "Zatwierdź";
            buttonConfirm.UseVisualStyleBackColor = true;
            buttonConfirm.Click += buttonConfirm_Click;
            // 
            // MapPickerForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(901, 613);
            Controls.Add(buttonConfirm);
            Controls.Add(buttonCancel);
            Controls.Add(label6);
            Controls.Add(textBoxLon);
            Controls.Add(label5);
            Controls.Add(label1);
            Controls.Add(textBoxLat);
            Controls.Add(webView21);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(3, 2, 3, 2);
            MaximizeBox = false;
            Name = "MapPickerForm";
            ShowInTaskbar = false;
            StartPosition = FormStartPosition.CenterParent;
            Text = "MapPickerForm";
            Load += MapPickerForm_Load;
            ((System.ComponentModel.ISupportInitialize)webView21).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Microsoft.Web.WebView2.WinForms.WebView2 webView21;
        private Label label5;
        private Label label1;
        private TextBox textBoxLat;
        private Label label6;
        private TextBox textBoxLon;
        private Button buttonCancel;
        private Button buttonConfirm;
    }
}