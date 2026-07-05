namespace OriginLiveView
{
    partial class OLVTakeFlat
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OLVTakeFlat));
            this.LabelCamTemp = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.buttonStartImage = new System.Windows.Forms.Button();
            this.labelExposureTime = new System.Windows.Forms.Label();
            this.textBoxISO = new System.Windows.Forms.TextBox();
            this.labelExposure = new System.Windows.Forms.Label();
            this.textBoxExposure = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // LabelCamTemp
            // 
            this.LabelCamTemp.AutoSize = true;
            this.LabelCamTemp.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.LabelCamTemp.Location = new System.Drawing.Point(200, 23);
            this.LabelCamTemp.Name = "LabelCamTemp";
            this.LabelCamTemp.Size = new System.Drawing.Size(81, 20);
            this.LabelCamTemp.TabIndex = 46;
            this.LabelCamTemp.Text = "Searching";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.label1.Location = new System.Drawing.Point(21, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(160, 20);
            this.label1.TabIndex = 45;
            this.label1.Text = "Camera Temperature";
            // 
            // buttonStartImage
            // 
            this.buttonStartImage.Location = new System.Drawing.Point(31, 158);
            this.buttonStartImage.Name = "buttonStartImage";
            this.buttonStartImage.Size = new System.Drawing.Size(250, 44);
            this.buttonStartImage.TabIndex = 44;
            this.buttonStartImage.Text = "Start Download Flats";
            this.buttonStartImage.UseVisualStyleBackColor = true;
            this.buttonStartImage.Click += new System.EventHandler(this.buttonStartImage_Click);
            // 
            // labelExposureTime
            // 
            this.labelExposureTime.AutoSize = true;
            this.labelExposureTime.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelExposureTime.Location = new System.Drawing.Point(21, 111);
            this.labelExposureTime.Name = "labelExposureTime";
            this.labelExposureTime.Size = new System.Drawing.Size(108, 20);
            this.labelExposureTime.TabIndex = 43;
            this.labelExposureTime.Text = "Exposure ISO";
            // 
            // textBoxISO
            // 
            this.textBoxISO.Location = new System.Drawing.Point(204, 108);
            this.textBoxISO.Name = "textBoxISO";
            this.textBoxISO.Size = new System.Drawing.Size(71, 26);
            this.textBoxISO.TabIndex = 42;
            this.textBoxISO.Text = "100";
            // 
            // labelExposure
            // 
            this.labelExposure.AutoSize = true;
            this.labelExposure.ForeColor = System.Drawing.SystemColors.ButtonFace;
            this.labelExposure.Location = new System.Drawing.Point(21, 66);
            this.labelExposure.Name = "labelExposure";
            this.labelExposure.Size = new System.Drawing.Size(142, 20);
            this.labelExposure.TabIndex = 41;
            this.labelExposure.Text = "Exposure time Sec";
            // 
            // textBoxExposure
            // 
            this.textBoxExposure.Location = new System.Drawing.Point(204, 63);
            this.textBoxExposure.Name = "textBoxExposure";
            this.textBoxExposure.Size = new System.Drawing.Size(71, 26);
            this.textBoxExposure.TabIndex = 40;
            this.textBoxExposure.Text = "0.5";
            // 
            // OLVTakeFlat
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(310, 228);
            this.Controls.Add(this.LabelCamTemp);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStartImage);
            this.Controls.Add(this.labelExposureTime);
            this.Controls.Add(this.textBoxISO);
            this.Controls.Add(this.labelExposure);
            this.Controls.Add(this.textBoxExposure);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OLVTakeFlat";
            this.Text = " OLV Tale Flats";
            this.Load += new System.EventHandler(this.OLVTakeFlat_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label LabelCamTemp;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button buttonStartImage;
        private System.Windows.Forms.Label labelExposureTime;
        private System.Windows.Forms.TextBox textBoxISO;
        private System.Windows.Forms.Label labelExposure;
        private System.Windows.Forms.TextBox textBoxExposure;
    }
}