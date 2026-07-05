namespace OriginLiveView
{
    partial class Focuser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Focuser));
            this.TextBoxPosition = new System.Windows.Forms.TextBox();
            this.ButtonMin100 = new System.Windows.Forms.Button();
            this.ButtonMin10 = new System.Windows.Forms.Button();
            this.ButtonMin1 = new System.Windows.Forms.Button();
            this.ButtonPlus1 = new System.Windows.Forms.Button();
            this.ButtonPlus10 = new System.Windows.Forms.Button();
            this.ButtonPlus100 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ButtonAuto = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // TextBoxPosition
            // 
            this.TextBoxPosition.Location = new System.Drawing.Point(97, 18);
            this.TextBoxPosition.Name = "TextBoxPosition";
            this.TextBoxPosition.Size = new System.Drawing.Size(70, 26);
            this.TextBoxPosition.TabIndex = 0;
            // 
            // ButtonMin100
            // 
            this.ButtonMin100.Location = new System.Drawing.Point(21, 59);
            this.ButtonMin100.Name = "ButtonMin100";
            this.ButtonMin100.Size = new System.Drawing.Size(58, 31);
            this.ButtonMin100.TabIndex = 1;
            this.ButtonMin100.Text = "-100";
            this.ButtonMin100.UseVisualStyleBackColor = true;
            this.ButtonMin100.Click += new System.EventHandler(this.ButtonMin100_Click);
            // 
            // ButtonMin10
            // 
            this.ButtonMin10.Location = new System.Drawing.Point(76, 59);
            this.ButtonMin10.Name = "ButtonMin10";
            this.ButtonMin10.Size = new System.Drawing.Size(58, 31);
            this.ButtonMin10.TabIndex = 2;
            this.ButtonMin10.Text = "-10";
            this.ButtonMin10.UseVisualStyleBackColor = true;
            this.ButtonMin10.Click += new System.EventHandler(this.ButtonMin10_Click);
            // 
            // ButtonMin1
            // 
            this.ButtonMin1.Location = new System.Drawing.Point(132, 59);
            this.ButtonMin1.Name = "ButtonMin1";
            this.ButtonMin1.Size = new System.Drawing.Size(58, 31);
            this.ButtonMin1.TabIndex = 3;
            this.ButtonMin1.Text = "-1";
            this.ButtonMin1.UseVisualStyleBackColor = true;
            this.ButtonMin1.Click += new System.EventHandler(this.ButtonMin1_Click);
            // 
            // ButtonPlus1
            // 
            this.ButtonPlus1.Location = new System.Drawing.Point(184, 59);
            this.ButtonPlus1.Name = "ButtonPlus1";
            this.ButtonPlus1.Size = new System.Drawing.Size(58, 31);
            this.ButtonPlus1.TabIndex = 4;
            this.ButtonPlus1.Text = "+1";
            this.ButtonPlus1.UseVisualStyleBackColor = true;
            this.ButtonPlus1.Click += new System.EventHandler(this.ButtonPlus1_Click);
            // 
            // ButtonPlus10
            // 
            this.ButtonPlus10.Location = new System.Drawing.Point(238, 59);
            this.ButtonPlus10.Name = "ButtonPlus10";
            this.ButtonPlus10.Size = new System.Drawing.Size(58, 31);
            this.ButtonPlus10.TabIndex = 5;
            this.ButtonPlus10.Text = "+10";
            this.ButtonPlus10.UseVisualStyleBackColor = true;
            this.ButtonPlus10.Click += new System.EventHandler(this.ButtonPlus10_Click);
            // 
            // ButtonPlus100
            // 
            this.ButtonPlus100.Location = new System.Drawing.Point(292, 59);
            this.ButtonPlus100.Name = "ButtonPlus100";
            this.ButtonPlus100.Size = new System.Drawing.Size(58, 31);
            this.ButtonPlus100.TabIndex = 6;
            this.ButtonPlus100.Text = "+100";
            this.ButtonPlus100.UseVisualStyleBackColor = true;
            this.ButtonPlus100.Click += new System.EventHandler(this.ButtonPlus100_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(17, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Position";
            // 
            // ButtonAuto
            // 
            this.ButtonAuto.Location = new System.Drawing.Point(206, 14);
            this.ButtonAuto.Name = "ButtonAuto";
            this.ButtonAuto.Size = new System.Drawing.Size(144, 35);
            this.ButtonAuto.TabIndex = 9;
            this.ButtonAuto.TabStop = false;
            this.ButtonAuto.Text = "Auto Focus";
            this.ButtonAuto.UseVisualStyleBackColor = true;
            this.ButtonAuto.Click += new System.EventHandler(this.ButtonAuto_Click);
            // 
            // Focuser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(378, 110);
            this.Controls.Add(this.ButtonAuto);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonPlus100);
            this.Controls.Add(this.ButtonPlus10);
            this.Controls.Add(this.ButtonPlus1);
            this.Controls.Add(this.ButtonMin1);
            this.Controls.Add(this.ButtonMin10);
            this.Controls.Add(this.ButtonMin100);
            this.Controls.Add(this.TextBoxPosition);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(300, 100);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Focuser";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = " Focuser Tool";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.Focuser_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox TextBoxPosition;
        private System.Windows.Forms.Button ButtonMin100;
        private System.Windows.Forms.Button ButtonMin10;
        private System.Windows.Forms.Button ButtonMin1;
        private System.Windows.Forms.Button ButtonPlus1;
        private System.Windows.Forms.Button ButtonPlus10;
        private System.Windows.Forms.Button ButtonPlus100;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ButtonAuto;
    }
}