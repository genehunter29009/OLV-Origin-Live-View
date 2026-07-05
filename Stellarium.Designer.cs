namespace OriginLiveView
{
    partial class Stellarium
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Stellarium));
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.STIP_Box = new System.Windows.Forms.TextBox();
            this.STIP = new System.Windows.Forms.Label();
            this.STPort = new System.Windows.Forms.Label();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.ConnectedStatus = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(28, 190);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(170, 37);
            this.button1.TabIndex = 0;
            this.button1.Text = "Start Server";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(222, 190);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(157, 37);
            this.button2.TabIndex = 1;
            this.button2.Text = "Stop Server";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // STIP_Box
            // 
            this.STIP_Box.Location = new System.Drawing.Point(209, 63);
            this.STIP_Box.Name = "STIP_Box";
            this.STIP_Box.Size = new System.Drawing.Size(170, 26);
            this.STIP_Box.TabIndex = 2;
            this.STIP_Box.Text = "127.0.0.1";
            // 
            // STIP
            // 
            this.STIP.AutoSize = true;
            this.STIP.ForeColor = System.Drawing.SystemColors.Window;
            this.STIP.Location = new System.Drawing.Point(43, 66);
            this.STIP.Name = "STIP";
            this.STIP.Size = new System.Drawing.Size(141, 20);
            this.STIP.TabIndex = 4;
            this.STIP.Text = "Server  IP Address";
            // 
            // STPort
            // 
            this.STPort.AutoSize = true;
            this.STPort.ForeColor = System.Drawing.SystemColors.Window;
            this.STPort.Location = new System.Drawing.Point(39, 107);
            this.STPort.Name = "STPort";
            this.STPort.Size = new System.Drawing.Size(226, 20);
            this.STPort.TabIndex = 5;
            this.STPort.Text = "Telescope Server Port:   10001";
            // 
            // ConnectedStatus
            // 
            this.ConnectedStatus.AutoSize = true;
            this.ConnectedStatus.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ConnectedStatus.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.ConnectedStatus.Location = new System.Drawing.Point(23, 18);
            this.ConnectedStatus.Name = "ConnectedStatus";
            this.ConnectedStatus.Size = new System.Drawing.Size(208, 25);
            this.ConnectedStatus.TabIndex = 6;
            this.ConnectedStatus.Text = "Telescope Server  Idle";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Window;
            this.label1.Location = new System.Drawing.Point(39, 147);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(224, 20);
            this.label1.TabIndex = 8;
            this.label1.Text = "Schedule Server Port:    10002";
            // 
            // Stellarium
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(413, 255);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ConnectedStatus);
            this.Controls.Add(this.STPort);
            this.Controls.Add(this.STIP);
            this.Controls.Add(this.STIP_Box);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(0, 800);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Stellarium";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Telescope Server";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Stellarium_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.TextBox STIP_Box;
        private System.Windows.Forms.Label STIP;
        private System.Windows.Forms.Label STPort;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.Label ConnectedStatus;
        private System.Windows.Forms.Label label1;
    }
}