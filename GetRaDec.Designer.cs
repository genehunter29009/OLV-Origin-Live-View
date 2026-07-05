namespace OriginLiveView
{
    partial class GetRaDec
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GetRaDec));
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.LabelRA = new System.Windows.Forms.Label();
            this.LabelDec = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(39, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "RA: ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(39, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 20);
            this.label2.TabIndex = 1;
            this.label2.Text = "Dec:";
            // 
            // LabelRA
            // 
            this.LabelRA.AutoSize = true;
            this.LabelRA.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LabelRA.Location = new System.Drawing.Point(113, 22);
            this.LabelRA.Name = "LabelRA";
            this.LabelRA.Size = new System.Drawing.Size(0, 20);
            this.LabelRA.TabIndex = 2;
            // 
            // LabelDec
            // 
            this.LabelDec.AutoSize = true;
            this.LabelDec.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LabelDec.Location = new System.Drawing.Point(113, 51);
            this.LabelDec.Name = "LabelDec";
            this.LabelDec.Size = new System.Drawing.Size(0, 20);
            this.LabelDec.TabIndex = 3;
            // 
            // GetRaDec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(382, 93);
            this.Controls.Add(this.LabelDec);
            this.Controls.Add(this.LabelRA);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GetRaDec";
            this.Load += new System.EventHandler(this.GetRaDec_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label LabelRA;
        private System.Windows.Forms.Label LabelDec;
    }
}