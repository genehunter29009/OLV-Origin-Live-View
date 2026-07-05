namespace OriginLiveView
{
    partial class OLVDithering
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OLVDithering));
            this.ButtonStart = new System.Windows.Forms.Button();
            this.LabelStatus = new System.Windows.Forms.Label();
            this.LabelAlt = new System.Windows.Forms.Label();
            this.LabelAz = new System.Windows.Forms.Label();
            this.TrackBarTime = new System.Windows.Forms.TrackBar();
            this.LabelDitherTime = new System.Windows.Forms.Label();
            this.TrackBarSpeed = new System.Windows.Forms.TrackBar();
            this.LabelSpeed = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarTime)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarSpeed)).BeginInit();
            this.TrackBarSpeed.Scroll += new System.EventHandler(this.TrackBarSpeed_Scroll);
            this.SuspendLayout();
            // 
            // ButtonStart
            // 
            this.ButtonStart.Location = new System.Drawing.Point(30, 127);
            this.ButtonStart.Name = "ButtonStart";
            this.ButtonStart.Size = new System.Drawing.Size(184, 39);
            this.ButtonStart.TabIndex = 0;
            this.ButtonStart.Text = "Start Dithering";
            this.ButtonStart.UseVisualStyleBackColor = true;
            this.ButtonStart.Click += new System.EventHandler(this.ButtonStart_Click);
            // 
            // LabelStatus
            // 
            this.LabelStatus.AutoSize = true;
            this.LabelStatus.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LabelStatus.Location = new System.Drawing.Point(51, 23);
            this.LabelStatus.Name = "LabelStatus";
            this.LabelStatus.Size = new System.Drawing.Size(124, 20);
            this.LabelStatus.TabIndex = 1;
            this.LabelStatus.Text = "Dithering Status";
            // 
            // LabelAlt
            // 
            this.LabelAlt.AutoSize = true;
            this.LabelAlt.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LabelAlt.Location = new System.Drawing.Point(54, 43);
            this.LabelAlt.Name = "LabelAlt";
            this.LabelAlt.Size = new System.Drawing.Size(0, 20);
            this.LabelAlt.TabIndex = 2;
            // 
            // LabelAz
            // 
            this.LabelAz.AutoSize = true;
            this.LabelAz.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LabelAz.Location = new System.Drawing.Point(54, 63);
            this.LabelAz.Name = "LabelAz";
            this.LabelAz.Size = new System.Drawing.Size(0, 20);
            this.LabelAz.TabIndex = 3;
            // 
            // TrackBarTime
            // 
            this.TrackBarTime.Location = new System.Drawing.Point(253, 20);
            this.TrackBarTime.Maximum = 5;
            this.TrackBarTime.Minimum = 1;
            this.TrackBarTime.Name = "TrackBarTime";
            this.TrackBarTime.Size = new System.Drawing.Size(174, 69);
            this.TrackBarTime.TabIndex = 4;
            this.TrackBarTime.Value = 2;
            this.TrackBarTime.Scroll += new System.EventHandler(this.TrackBarTime_Scroll);
            // 
            // LabelDitherTime
            // 
            this.LabelDitherTime.AutoSize = true;
            this.LabelDitherTime.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LabelDitherTime.Location = new System.Drawing.Point(261, 63);
            this.LabelDitherTime.Name = "LabelDitherTime";
            this.LabelDitherTime.Size = new System.Drawing.Size(103, 20);
            this.LabelDitherTime.TabIndex = 5;
            this.LabelDitherTime.Text = "Dither Time =";
            // 
            // TrackBarSpeed
            // 
            this.TrackBarSpeed.Location = new System.Drawing.Point(253, 104);
            this.TrackBarSpeed.Maximum = 5;
            this.TrackBarSpeed.Minimum = 1;
            this.TrackBarSpeed.Name = "TrackBarSpeed";
            this.TrackBarSpeed.Size = new System.Drawing.Size(174, 69);
            this.TrackBarSpeed.TabIndex = 6;
            this.TrackBarSpeed.Value = 2;
            // 
            // LabelSpeed
            // 
            this.LabelSpeed.AutoSize = true;
            this.LabelSpeed.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LabelSpeed.Location = new System.Drawing.Point(261, 153);
            this.LabelSpeed.Name = "LabelSpeed";
            this.LabelSpeed.Size = new System.Drawing.Size(116, 20);
            this.LabelSpeed.TabIndex = 7;
            this.LabelSpeed.Text = "Dither Speed =";
            // 
            // OLVDithering
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(446, 203);
            this.Controls.Add(this.LabelSpeed);
            this.Controls.Add(this.TrackBarSpeed);
            this.Controls.Add(this.LabelDitherTime);
            this.Controls.Add(this.TrackBarTime);
            this.Controls.Add(this.LabelAz);
            this.Controls.Add(this.LabelAlt);
            this.Controls.Add(this.LabelStatus);
            this.Controls.Add(this.ButtonStart);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(200, 600);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OLVDithering";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " OLV Dithering";
            this.Load += new System.EventHandler(this.OLVDithering_Load);
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarTime)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarSpeed)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonStart;
        private System.Windows.Forms.Label LabelStatus;
        private System.Windows.Forms.Label LabelAlt;
        private System.Windows.Forms.Label LabelAz;
        private System.Windows.Forms.TrackBar TrackBarTime;
        private System.Windows.Forms.Label LabelDitherTime;
        private System.Windows.Forms.TrackBar TrackBarSpeed;
        private System.Windows.Forms.Label LabelSpeed;
    }
}