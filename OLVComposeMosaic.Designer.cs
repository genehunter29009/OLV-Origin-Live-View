namespace OriginLiveView
{
    partial class OLVComposeMosaic
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(OLVComposeMosaic));
            this.TextBoxName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonDownload = new System.Windows.Forms.Button();
            this.ButtonCopyRen = new System.Windows.Forms.Button();
            this.ButtonCompose = new System.Windows.Forms.Button();
            this.ButtonCleanup = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxName
            // 
            this.TextBoxName.Location = new System.Drawing.Point(31, 41);
            this.TextBoxName.Name = "TextBoxName";
            this.TextBoxName.Size = new System.Drawing.Size(184, 26);
            this.TextBoxName.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(236, 47);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(105, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "Mosaic Name";
            // 
            // ButtonDownload
            // 
            this.ButtonDownload.Location = new System.Drawing.Point(31, 87);
            this.ButtonDownload.Name = "ButtonDownload";
            this.ButtonDownload.Size = new System.Drawing.Size(310, 44);
            this.ButtonDownload.TabIndex = 2;
            this.ButtonDownload.Text = "Download Files";
            this.ButtonDownload.UseVisualStyleBackColor = true;
            this.ButtonDownload.Click += new System.EventHandler(this.ButtonDownload_Click);
            // 
            // ButtonCopyRen
            // 
            this.ButtonCopyRen.Location = new System.Drawing.Point(31, 152);
            this.ButtonCopyRen.Name = "ButtonCopyRen";
            this.ButtonCopyRen.Size = new System.Drawing.Size(310, 44);
            this.ButtonCopyRen.TabIndex = 3;
            this.ButtonCopyRen.Text = "Copy and Rename Files";
            this.ButtonCopyRen.UseVisualStyleBackColor = true;
            this.ButtonCopyRen.Click += new System.EventHandler(this.ButtonCopyRen_Click);
            // 
            // ButtonCompose
            // 
            this.ButtonCompose.Location = new System.Drawing.Point(31, 216);
            this.ButtonCompose.Name = "ButtonCompose";
            this.ButtonCompose.Size = new System.Drawing.Size(310, 44);
            this.ButtonCompose.TabIndex = 4;
            this.ButtonCompose.Text = "Compose Mosaic with Siril";
            this.ButtonCompose.UseVisualStyleBackColor = true;
            this.ButtonCompose.Click += new System.EventHandler(this.ButtonCompose_Click);
            // 
            // ButtonCleanup
            // 
            this.ButtonCleanup.Location = new System.Drawing.Point(31, 276);
            this.ButtonCleanup.Name = "ButtonCleanup";
            this.ButtonCleanup.Size = new System.Drawing.Size(310, 44);
            this.ButtonCleanup.TabIndex = 5;
            this.ButtonCleanup.Text = "Cleanup Delete Files";
            this.ButtonCleanup.UseVisualStyleBackColor = true;
            this.ButtonCleanup.Click += new System.EventHandler(this.ButtonCleanup_Click);
            // 
            // OLVComposeMosaic
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(375, 341);
            this.Controls.Add(this.ButtonCleanup);
            this.Controls.Add(this.ButtonCompose);
            this.Controls.Add(this.ButtonCopyRen);
            this.Controls.Add(this.ButtonDownload);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxName);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "OLVComposeMosaic";
            this.Text = " OLV Compose Mosaic";
            this.Load += new System.EventHandler(this.OLVComposeMosaic_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxName;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonDownload;
        //private System.Windows.Forms.Button ButtonCopyRename;
        private System.Windows.Forms.Button ButtonCopyRen;
        private System.Windows.Forms.Button ButtonCompose;
        private System.Windows.Forms.Button ButtonCleanup;
    }
}