namespace OriginLiveView
{
    partial class CaptureImage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(CaptureImage));
            this.textBoxExposure = new System.Windows.Forms.TextBox();
            this.labelExposure = new System.Windows.Forms.Label();
            this.textBoxISO = new System.Windows.Forms.TextBox();
            this.labelExposureTime = new System.Windows.Forms.Label();
            this.labelExposureName = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.labelSaveRaw = new System.Windows.Forms.Label();
            this.buttonStartImage = new System.Windows.Forms.Button();
            this.buttonClose = new System.Windows.Forms.Button();
            this.textBoxTotalExposure = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.CheckBoxSaveRaw = new System.Windows.Forms.CheckBox();
            this.label17 = new System.Windows.Forms.Label();
            this.checkBoxAutoCancel = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // textBoxExposure
            // 
            this.textBoxExposure.Location = new System.Drawing.Point(208, 32);
            this.textBoxExposure.Name = "textBoxExposure";
            this.textBoxExposure.Size = new System.Drawing.Size(71, 26);
            this.textBoxExposure.TabIndex = 0;
            this.textBoxExposure.Text = "20";
            // 
            // labelExposure
            // 
            this.labelExposure.AutoSize = true;
            this.labelExposure.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelExposure.Location = new System.Drawing.Point(25, 35);
            this.labelExposure.Name = "labelExposure";
            this.labelExposure.Size = new System.Drawing.Size(142, 20);
            this.labelExposure.TabIndex = 1;
            this.labelExposure.Text = "Exposure time Sec";
            // 
            // textBoxISO
            // 
            this.textBoxISO.Location = new System.Drawing.Point(208, 77);
            this.textBoxISO.Name = "textBoxISO";
            this.textBoxISO.Size = new System.Drawing.Size(71, 26);
            this.textBoxISO.TabIndex = 2;
            this.textBoxISO.Text = "200";
            // 
            // labelExposureTime
            // 
            this.labelExposureTime.AutoSize = true;
            this.labelExposureTime.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelExposureTime.Location = new System.Drawing.Point(25, 80);
            this.labelExposureTime.Name = "labelExposureTime";
            this.labelExposureTime.Size = new System.Drawing.Size(108, 20);
            this.labelExposureTime.TabIndex = 3;
            this.labelExposureTime.Text = "Exposure ISO";
            // 
            // labelExposureName
            // 
            this.labelExposureName.AutoSize = true;
            this.labelExposureName.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelExposureName.Location = new System.Drawing.Point(25, 173);
            this.labelExposureName.Name = "labelExposureName";
            this.labelExposureName.Size = new System.Drawing.Size(79, 20);
            this.labelExposureName.TabIndex = 4;
            this.labelExposureName.Text = "Obj Name";
            this.labelExposureName.Click += new System.EventHandler(this.labelExposureName_Click);
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(124, 170);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(155, 26);
            this.textBoxName.TabIndex = 5;
            this.textBoxName.Text = "M51";
            // 
            // labelSaveRaw
            // 
            this.labelSaveRaw.AutoSize = true;
            this.labelSaveRaw.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelSaveRaw.Location = new System.Drawing.Point(19, 228);
            this.labelSaveRaw.Name = "labelSaveRaw";
            this.labelSaveRaw.Size = new System.Drawing.Size(85, 20);
            this.labelSaveRaw.TabIndex = 6;
            this.labelSaveRaw.Text = "Save Raw ";
            // 
            // buttonStartImage
            // 
            this.buttonStartImage.Location = new System.Drawing.Point(23, 401);
            this.buttonStartImage.Name = "buttonStartImage";
            this.buttonStartImage.Size = new System.Drawing.Size(119, 44);
            this.buttonStartImage.TabIndex = 8;
            this.buttonStartImage.Text = "Start Image";
            this.buttonStartImage.UseVisualStyleBackColor = true;
            this.buttonStartImage.Click += new System.EventHandler(this.buttonStartImage_Click);
            // 
            // buttonClose
            // 
            this.buttonClose.AutoSize = true;
            this.buttonClose.Location = new System.Drawing.Point(172, 398);
            this.buttonClose.Name = "buttonClose";
            this.buttonClose.Size = new System.Drawing.Size(107, 47);
            this.buttonClose.TabIndex = 10;
            this.buttonClose.Text = "Stop Image";
            this.buttonClose.UseVisualStyleBackColor = true;
            this.buttonClose.Click += new System.EventHandler(this.buttonClose_Click);
            // 
            // textBoxTotalExposure
            // 
            this.textBoxTotalExposure.Location = new System.Drawing.Point(208, 121);
            this.textBoxTotalExposure.Name = "textBoxTotalExposure";
            this.textBoxTotalExposure.Size = new System.Drawing.Size(71, 26);
            this.textBoxTotalExposure.TabIndex = 11;
            this.textBoxTotalExposure.Text = "60";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label6.Location = new System.Drawing.Point(25, 124);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(148, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Total Exposure  Min";
            // 
            // CheckBoxSaveRaw
            // 
            this.CheckBoxSaveRaw.AutoSize = true;
            this.CheckBoxSaveRaw.Checked = true;
            this.CheckBoxSaveRaw.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxSaveRaw.Location = new System.Drawing.Point(191, 228);
            this.CheckBoxSaveRaw.Name = "CheckBoxSaveRaw";
            this.CheckBoxSaveRaw.Size = new System.Drawing.Size(22, 21);
            this.CheckBoxSaveRaw.TabIndex = 13;
            this.CheckBoxSaveRaw.UseVisualStyleBackColor = true;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label17.Location = new System.Drawing.Point(19, 267);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(149, 20);
            this.label17.TabIndex = 74;
            this.label17.Text = "Disable AutoCancel";
            // 
            // checkBoxAutoCancel
            // 
            this.checkBoxAutoCancel.AutoSize = true;
            this.checkBoxAutoCancel.Checked = true;
            this.checkBoxAutoCancel.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBoxAutoCancel.Location = new System.Drawing.Point(191, 267);
            this.checkBoxAutoCancel.Name = "checkBoxAutoCancel";
            this.checkBoxAutoCancel.Size = new System.Drawing.Size(22, 21);
            this.checkBoxAutoCancel.TabIndex = 73;
            this.checkBoxAutoCancel.UseMnemonic = false;
            this.checkBoxAutoCancel.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(19, 309);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(119, 20);
            this.label1.TabIndex = 75;
            this.label1.Text = "Dither Distance";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label2.Location = new System.Drawing.Point(25, 350);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(131, 20);
            this.label2.TabIndex = 76;
            this.label2.Text = "Dither Frequency";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(191, 306);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(71, 26);
            this.textBox1.TabIndex = 77;
            this.textBox1.Text = "10";
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(191, 350);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(71, 26);
            this.textBox2.TabIndex = 78;
            this.textBox2.Text = "5";
            // 
            // CaptureImage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(309, 470);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.checkBoxAutoCancel);
            this.Controls.Add(this.CheckBoxSaveRaw);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.textBoxTotalExposure);
            this.Controls.Add(this.buttonClose);
            this.Controls.Add(this.buttonStartImage);
            this.Controls.Add(this.labelSaveRaw);
            this.Controls.Add(this.textBoxName);
            this.Controls.Add(this.labelExposureName);
            this.Controls.Add(this.labelExposureTime);
            this.Controls.Add(this.textBoxISO);
            this.Controls.Add(this.labelExposure);
            this.Controls.Add(this.textBoxExposure);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "CaptureImage";
            this.Text = "  Capture";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox textBoxExposure;
        private System.Windows.Forms.Label labelExposure;
        private System.Windows.Forms.TextBox textBoxISO;
        private System.Windows.Forms.Label labelExposureTime;
        private System.Windows.Forms.Label labelExposureName;
        private System.Windows.Forms.TextBox textBoxName;
        private System.Windows.Forms.Label labelSaveRaw;
        private System.Windows.Forms.Button buttonStartImage;
        private System.Windows.Forms.Button buttonClose;
        private System.Windows.Forms.TextBox textBoxTotalExposure;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox CheckBoxSaveRaw;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.CheckBox checkBoxAutoCancel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
    }
}