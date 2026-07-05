namespace OriginLiveView
{
    partial class RetakeDarks
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RetakeDarks));
            this.ComboBitDepth = new System.Windows.Forms.ComboBox();
            this.ComboExposureTime = new System.Windows.Forms.ComboBox();
            this.ComboISO = new System.Windows.Forms.ComboBox();
            this.ButtonRetakeDarks = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ComboBitDepth
            // 
            this.ComboBitDepth.FormattingEnabled = true;
            this.ComboBitDepth.Items.AddRange(new object[] {
            "8",
            "16",
            "24"});
            this.ComboBitDepth.Location = new System.Drawing.Point(191, 24);
            this.ComboBitDepth.Name = "ComboBitDepth";
            this.ComboBitDepth.Size = new System.Drawing.Size(71, 28);
            this.ComboBitDepth.TabIndex = 0;
            this.ComboBitDepth.Text = "16";
            // 
            // ComboExposureTime
            // 
            this.ComboExposureTime.FormattingEnabled = true;
            this.ComboExposureTime.Items.AddRange(new object[] {
            "1",
            "5",
            "10",
            "15",
            "20",
            "30"});
            this.ComboExposureTime.Location = new System.Drawing.Point(191, 70);
            this.ComboExposureTime.Name = "ComboExposureTime";
            this.ComboExposureTime.Size = new System.Drawing.Size(71, 28);
            this.ComboExposureTime.TabIndex = 1;
            this.ComboExposureTime.Text = "15";
            // 
            // ComboISO
            // 
            this.ComboISO.FormattingEnabled = true;
            this.ComboISO.Items.AddRange(new object[] {
            "100",
            "200",
            "500",
            "1000",
            "2000"});
            this.ComboISO.Location = new System.Drawing.Point(191, 113);
            this.ComboISO.Name = "ComboISO";
            this.ComboISO.Size = new System.Drawing.Size(71, 28);
            this.ComboISO.TabIndex = 2;
            this.ComboISO.Text = "2000";
            // 
            // ButtonRetakeDarks
            // 
            this.ButtonRetakeDarks.Location = new System.Drawing.Point(37, 164);
            this.ButtonRetakeDarks.Name = "ButtonRetakeDarks";
            this.ButtonRetakeDarks.Size = new System.Drawing.Size(225, 34);
            this.ButtonRetakeDarks.TabIndex = 3;
            this.ButtonRetakeDarks.Text = "Retake Darks";
            this.ButtonRetakeDarks.UseVisualStyleBackColor = true;
            this.ButtonRetakeDarks.Click += new System.EventHandler(this.ButtonRetakeDarks_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(33, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(76, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Bit Depth";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(33, 70);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(126, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Exposure In Sec";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(33, 116);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(75, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "ISO Gain";
            // 
            // RetakeDarks
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(304, 226);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonRetakeDarks);
            this.Controls.Add(this.ComboISO);
            this.Controls.Add(this.ComboExposureTime);
            this.Controls.Add(this.ComboBitDepth);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(100, 600);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "RetakeDarks";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " Retake Darks";
            this.Load += new System.EventHandler(this.RetakeDarks_Load_1);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox ComboBitDepth;
        private System.Windows.Forms.ComboBox ComboExposureTime;
        private System.Windows.Forms.ComboBox ComboISO;
        private System.Windows.Forms.Button ButtonRetakeDarks;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
    }
}