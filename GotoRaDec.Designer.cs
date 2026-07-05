namespace OriginLiveView
{
    partial class GotoRaDec
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GotoRaDec));
            this.TextBoxRA = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.ButtonGoto = new System.Windows.Forms.Button();
            this.TextBoxDec = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // TextBoxRA
            // 
            this.TextBoxRA.Location = new System.Drawing.Point(22, 24);
            this.TextBoxRA.Name = "TextBoxRA";
            this.TextBoxRA.Size = new System.Drawing.Size(99, 26);
            this.TextBoxRA.TabIndex = 0;
            this.TextBoxRA.Text = "10.6847";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.Control;
            this.label1.Location = new System.Drawing.Point(152, 30);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "RA";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.Control;
            this.label2.Location = new System.Drawing.Point(152, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(43, 20);
            this.label2.TabIndex = 3;
            this.label2.Text = "DEC";
            // 
            // ButtonGoto
            // 
            this.ButtonGoto.Location = new System.Drawing.Point(22, 118);
            this.ButtonGoto.Name = "ButtonGoto";
            this.ButtonGoto.Size = new System.Drawing.Size(173, 43);
            this.ButtonGoto.TabIndex = 4;
            this.ButtonGoto.Text = "GoTo Ra Dec";
            this.ButtonGoto.UseVisualStyleBackColor = true;
            this.ButtonGoto.Click += new System.EventHandler(this.ButtonGoto_Click);
            // 
            // TextBoxDec
            // 
            this.TextBoxDec.Location = new System.Drawing.Point(22, 73);
            this.TextBoxDec.Name = "TextBoxDec";
            this.TextBoxDec.Size = new System.Drawing.Size(99, 26);
            this.TextBoxDec.TabIndex = 5;
            this.TextBoxDec.Text = "41.2688";
            // 
            // GotoRaDec
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(232, 179);
            this.Controls.Add(this.TextBoxDec);
            this.Controls.Add(this.ButtonGoto);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.TextBoxRA);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "GotoRaDec";
            this.Text = " Goto Ra Dec";
            this.Load += new System.EventHandler(this.GotoRaDec_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxRA;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button ButtonGoto;
        private System.Windows.Forms.TextBox TextBoxDec;
    }
}