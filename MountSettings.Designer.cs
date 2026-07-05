namespace OriginLiveView
{
    partial class MountSettings
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MountSettings));
            this.CheckBoxEQ = new System.Windows.Forms.CheckBox();
            this.CheckBoxWedge = new System.Windows.Forms.CheckBox();
            this.CheckBoxPEC = new System.Windows.Forms.CheckBox();
            this.ButtonSetMount = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CheckBoxCustSpeed = new System.Windows.Forms.CheckBox();
            this.TextBoxCustSpeed = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.numericAlt = new System.Windows.Forms.NumericUpDown();
            this.numericAz = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.numericAlt)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericAz)).BeginInit();
            this.SuspendLayout();
            // 
            // CheckBoxEQ
            // 
            this.CheckBoxEQ.AutoSize = true;
            this.CheckBoxEQ.Location = new System.Drawing.Point(37, 21);
            this.CheckBoxEQ.Name = "CheckBoxEQ";
            this.CheckBoxEQ.Size = new System.Drawing.Size(113, 24);
            this.CheckBoxEQ.TabIndex = 0;
            this.CheckBoxEQ.Text = "checkBox1";
            this.CheckBoxEQ.UseVisualStyleBackColor = true;
            // 
            // CheckBoxWedge
            // 
            this.CheckBoxWedge.AutoSize = true;
            this.CheckBoxWedge.Location = new System.Drawing.Point(38, 63);
            this.CheckBoxWedge.Name = "CheckBoxWedge";
            this.CheckBoxWedge.Size = new System.Drawing.Size(113, 24);
            this.CheckBoxWedge.TabIndex = 1;
            this.CheckBoxWedge.Text = "checkBox2";
            this.CheckBoxWedge.UseVisualStyleBackColor = true;
            // 
            // CheckBoxPEC
            // 
            this.CheckBoxPEC.AutoSize = true;
            this.CheckBoxPEC.Location = new System.Drawing.Point(38, 289);
            this.CheckBoxPEC.Name = "CheckBoxPEC";
            this.CheckBoxPEC.Size = new System.Drawing.Size(113, 24);
            this.CheckBoxPEC.TabIndex = 2;
            this.CheckBoxPEC.Text = "checkBox3";
            this.CheckBoxPEC.UseVisualStyleBackColor = true;
            // 
            // ButtonSetMount
            // 
            this.ButtonSetMount.Location = new System.Drawing.Point(38, 334);
            this.ButtonSetMount.Name = "ButtonSetMount";
            this.ButtonSetMount.Size = new System.Drawing.Size(288, 35);
            this.ButtonSetMount.TabIndex = 6;
            this.ButtonSetMount.Text = "Save Mount Settings";
            this.ButtonSetMount.UseVisualStyleBackColor = true;
            this.ButtonSetMount.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(151, 21);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(102, 20);
            this.label1.TabIndex = 7;
            this.label1.Text = "Is  Equatorial";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label2.Location = new System.Drawing.Point(151, 63);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(112, 20);
            this.label2.TabIndex = 8;
            this.label2.Text = "On EQ Wedge";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label3.Location = new System.Drawing.Point(151, 289);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(95, 20);
            this.label3.TabIndex = 9;
            this.label3.Text = "Enable PEC";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label4.Location = new System.Drawing.Point(151, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(101, 20);
            this.label4.TabIndex = 10;
            this.label4.Text = "Alt  Backlash";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label5.Location = new System.Drawing.Point(151, 153);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(101, 20);
            this.label5.TabIndex = 11;
            this.label5.Text = "Az  Backlash";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label6.Location = new System.Drawing.Point(157, 247);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(169, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Enable Custom Speed";
            // 
            // CheckBoxCustSpeed
            // 
            this.CheckBoxCustSpeed.AutoSize = true;
            this.CheckBoxCustSpeed.Location = new System.Drawing.Point(38, 243);
            this.CheckBoxCustSpeed.Name = "CheckBoxCustSpeed";
            this.CheckBoxCustSpeed.Size = new System.Drawing.Size(113, 24);
            this.CheckBoxCustSpeed.TabIndex = 13;
            this.CheckBoxCustSpeed.Text = "checkBox4";
            this.CheckBoxCustSpeed.UseVisualStyleBackColor = true;
            // 
            // TextBoxCustSpeed
            // 
            this.TextBoxCustSpeed.Location = new System.Drawing.Point(37, 198);
            this.TextBoxCustSpeed.Name = "TextBoxCustSpeed";
            this.TextBoxCustSpeed.Size = new System.Drawing.Size(75, 26);
            this.TextBoxCustSpeed.TabIndex = 14;
            this.TextBoxCustSpeed.Text = "0";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label7.Location = new System.Drawing.Point(152, 201);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(123, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Cusstom Speed";
            // 
            // numericAlt
            // 
            this.numericAlt.Location = new System.Drawing.Point(37, 105);
            this.numericAlt.Name = "numericAlt";
            this.numericAlt.Size = new System.Drawing.Size(39, 26);
            this.numericAlt.TabIndex = 16;
            // 
            // numericAz
            // 
            this.numericAz.Location = new System.Drawing.Point(38, 151);
            this.numericAz.Name = "numericAz";
            this.numericAz.Size = new System.Drawing.Size(39, 26);
            this.numericAz.TabIndex = 17;
            // 
            // MountSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(362, 392);
            this.Controls.Add(this.numericAz);
            this.Controls.Add(this.numericAlt);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.TextBoxCustSpeed);
            this.Controls.Add(this.CheckBoxCustSpeed);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ButtonSetMount);
            this.Controls.Add(this.CheckBoxPEC);
            this.Controls.Add(this.CheckBoxWedge);
            this.Controls.Add(this.CheckBoxEQ);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Location = new System.Drawing.Point(300, 500);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "MountSettings";
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "Mount Settings";
            this.Load += new System.EventHandler(this.MountSettings_Load);
            ((System.ComponentModel.ISupportInitialize)(this.numericAlt)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numericAz)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox CheckBoxEQ;
        private System.Windows.Forms.CheckBox CheckBoxWedge;
        private System.Windows.Forms.CheckBox CheckBoxPEC;
        private System.Windows.Forms.Button ButtonSetMount;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.CheckBox CheckBoxCustSpeed;
        private System.Windows.Forms.TextBox TextBoxCustSpeed;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown numericAlt;
        private System.Windows.Forms.NumericUpDown numericAz;
    }
}