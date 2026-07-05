namespace OriginLiveView
{
    partial class SlewTelescope
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SlewTelescope));
            this.buttonSlewU = new System.Windows.Forms.Button();
            this.buttonSlewR = new System.Windows.Forms.Button();
            this.buttonSlewL = new System.Windows.Forms.Button();
            this.buttonSlewD = new System.Windows.Forms.Button();
            this.trackBarSlewSpeed = new System.Windows.Forms.TrackBar();
            this.label1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSlewSpeed)).BeginInit();
            this.SuspendLayout();
            // 
            // buttonSlewU
            // 
            this.buttonSlewU.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSlewU.Location = new System.Drawing.Point(278, 19);
            this.buttonSlewU.Name = "buttonSlewU";
            this.buttonSlewU.Size = new System.Drawing.Size(51, 34);
            this.buttonSlewU.TabIndex = 0;
            this.buttonSlewU.Text = "U";
            this.buttonSlewU.UseVisualStyleBackColor = true;
            this.buttonSlewU.Click += new System.EventHandler(this.buttonSlewU_Click);
            // 
            // buttonSlewR
            // 
            this.buttonSlewR.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSlewR.Location = new System.Drawing.Point(335, 55);
            this.buttonSlewR.Name = "buttonSlewR";
            this.buttonSlewR.Size = new System.Drawing.Size(51, 34);
            this.buttonSlewR.TabIndex = 1;
            this.buttonSlewR.Text = "R";
            this.buttonSlewR.UseVisualStyleBackColor = true;
            this.buttonSlewR.Click += new System.EventHandler(this.buttonSlewR_Click);
            // 
            // buttonSlewL
            // 
            this.buttonSlewL.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSlewL.Location = new System.Drawing.Point(216, 55);
            this.buttonSlewL.Name = "buttonSlewL";
            this.buttonSlewL.Size = new System.Drawing.Size(51, 34);
            this.buttonSlewL.TabIndex = 2;
            this.buttonSlewL.Text = "L";
            this.buttonSlewL.UseVisualStyleBackColor = true;
            this.buttonSlewL.Click += new System.EventHandler(this.buttonSlewL_Click);
            // 
            // buttonSlewD
            // 
            this.buttonSlewD.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.buttonSlewD.Location = new System.Drawing.Point(278, 91);
            this.buttonSlewD.Name = "buttonSlewD";
            this.buttonSlewD.Size = new System.Drawing.Size(51, 34);
            this.buttonSlewD.TabIndex = 3;
            this.buttonSlewD.Text = "D";
            this.buttonSlewD.UseVisualStyleBackColor = true;
            this.buttonSlewD.Click += new System.EventHandler(this.buttonSlewD_Click);
            // 
            // trackBarSlewSpeed
            // 
            this.trackBarSlewSpeed.Location = new System.Drawing.Point(30, 20);
            this.trackBarSlewSpeed.Name = "trackBarSlewSpeed";
            this.trackBarSlewSpeed.Size = new System.Drawing.Size(145, 69);
            this.trackBarSlewSpeed.TabIndex = 4;
            this.trackBarSlewSpeed.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.trackBarSlewSpeed.Value = 4;
            this.trackBarSlewSpeed.Scroll += new System.EventHandler(this.trackBar1_Scroll);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(48, 62);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 6;
            this.label1.Text = "Speed  1 -1 0";
            // 
            // SlewTelescope
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(411, 150);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.trackBarSlewSpeed);
            this.Controls.Add(this.buttonSlewD);
            this.Controls.Add(this.buttonSlewL);
            this.Controls.Add(this.buttonSlewR);
            this.Controls.Add(this.buttonSlewU);
            this.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(200, 100);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SlewTelescope";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Slew Telescope Control";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Focus_Load);
            ((System.ComponentModel.ISupportInitialize)(this.trackBarSlewSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSlewU;
        private System.Windows.Forms.Button buttonSlewR;
        private System.Windows.Forms.Button buttonSlewL;
        private System.Windows.Forms.Button buttonSlewD;
        private System.Windows.Forms.TrackBar trackBarSlewSpeed;
        private System.Windows.Forms.Label label1;
    }
}