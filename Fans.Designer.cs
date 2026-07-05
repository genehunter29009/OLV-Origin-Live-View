namespace OriginLiveView
{
    partial class Fans
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Fans));
            this.CheckBoxOTA = new System.Windows.Forms.CheckBox();
            this.CheckBoxCPU = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // CheckBoxOTA
            // 
            this.CheckBoxOTA.AutoSize = true;
            this.CheckBoxOTA.Location = new System.Drawing.Point(35, 25);
            this.CheckBoxOTA.Name = "CheckBoxOTA";
            this.CheckBoxOTA.Size = new System.Drawing.Size(113, 24);
            this.CheckBoxOTA.TabIndex = 0;
            this.CheckBoxOTA.Text = "checkBox1";
            this.CheckBoxOTA.UseVisualStyleBackColor = true;
            // 
            // CheckBoxCPU
            // 
            this.CheckBoxCPU.AutoSize = true;
            this.CheckBoxCPU.Location = new System.Drawing.Point(35, 55);
            this.CheckBoxCPU.Name = "CheckBoxCPU";
            this.CheckBoxCPU.Size = new System.Drawing.Size(113, 24);
            this.CheckBoxCPU.TabIndex = 1;
            this.CheckBoxCPU.Text = "checkBox2";
            this.CheckBoxCPU.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(64, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(73, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "OTA Fan";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(64, 56);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(78, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "CPU  Fan";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(26, 95);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 35);
            this.button1.TabIndex = 4;
            this.button1.Text = "Set Fans";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // Fans
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(163, 152);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CheckBoxCPU);
            this.Controls.Add(this.CheckBoxOTA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(100, 400);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Fans";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " Fans";
            this.Load += new System.EventHandler(this.Fans_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CheckBoxOTA;
        private System.Windows.Forms.CheckBox CheckBoxCPU;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
    }
}