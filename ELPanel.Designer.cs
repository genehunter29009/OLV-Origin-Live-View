namespace OriginLiveView
{
    partial class ELPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ELPanel));
            this.ButtonConnect = new System.Windows.Forms.Button();
            this.ButtonDisconnect = new System.Windows.Forms.Button();
            this.ButtonSetLevel = new System.Windows.Forms.Button();
            this.TextBoxLightLevel = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // ButtonConnect
            // 
            this.ButtonConnect.Location = new System.Drawing.Point(12, 30);
            this.ButtonConnect.Name = "ButtonConnect";
            this.ButtonConnect.Size = new System.Drawing.Size(109, 32);
            this.ButtonConnect.TabIndex = 0;
            this.ButtonConnect.Text = "Connect";
            this.ButtonConnect.UseVisualStyleBackColor = true;
            this.ButtonConnect.Click += new System.EventHandler(this.ButtonConnect_Click);
            // 
            // ButtonDisconnect
            // 
            this.ButtonDisconnect.Location = new System.Drawing.Point(137, 30);
            this.ButtonDisconnect.Name = "ButtonDisconnect";
            this.ButtonDisconnect.Size = new System.Drawing.Size(109, 32);
            this.ButtonDisconnect.TabIndex = 3;
            this.ButtonDisconnect.Text = "Disconnect";
            this.ButtonDisconnect.UseVisualStyleBackColor = true;
            this.ButtonDisconnect.Click += new System.EventHandler(this.ButtonDisconnect_Click);
            // 
            // ButtonSetLevel
            // 
            this.ButtonSetLevel.Location = new System.Drawing.Point(12, 80);
            this.ButtonSetLevel.Name = "ButtonSetLevel";
            this.ButtonSetLevel.Size = new System.Drawing.Size(109, 30);
            this.ButtonSetLevel.TabIndex = 4;
            this.ButtonSetLevel.Text = "Set Level";
            this.ButtonSetLevel.UseVisualStyleBackColor = true;
            this.ButtonSetLevel.Click += new System.EventHandler(this.ButtonSetLevel_Click);
            // 
            // TextBoxLightLevel
            // 
            this.TextBoxLightLevel.Location = new System.Drawing.Point(137, 82);
            this.TextBoxLightLevel.Name = "TextBoxLightLevel";
            this.TextBoxLightLevel.Size = new System.Drawing.Size(74, 26);
            this.TextBoxLightLevel.TabIndex = 5;
            this.TextBoxLightLevel.Text = "60";
            // 
            // ELPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(276, 134);
            this.Controls.Add(this.TextBoxLightLevel);
            this.Controls.Add(this.ButtonSetLevel);
            this.Controls.Add(this.ButtonDisconnect);
            this.Controls.Add(this.ButtonConnect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "ELPanel";
            this.Text = " EL Panel";
            this.Load += new System.EventHandler(this.ELPanel_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button ButtonConnect;
        private System.Windows.Forms.Button ButtonDisconnect;
        private System.Windows.Forms.Button ButtonSetLevel;
        private System.Windows.Forms.TextBox TextBoxLightLevel;
    }
}