namespace OriginLiveView
{
    partial class AutoFocus
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AutoFocus));
            this.buttonAutoFocus = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.TextBoxCelcius = new System.Windows.Forms.TextBox();
            this.CheckBoxTempChange = new System.Windows.Forms.CheckBox();
            this.CheckBoxGoto = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // buttonAutoFocus
            // 
            this.buttonAutoFocus.Location = new System.Drawing.Point(348, 23);
            this.buttonAutoFocus.Name = "buttonAutoFocus";
            this.buttonAutoFocus.Size = new System.Drawing.Size(181, 44);
            this.buttonAutoFocus.TabIndex = 0;
            this.buttonAutoFocus.TabStop = false;
            this.buttonAutoFocus.Text = "Save";
            this.buttonAutoFocus.UseVisualStyleBackColor = true;
            this.buttonAutoFocus.Click += new System.EventHandler(this.buttonAutoFocus_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.label1.Location = new System.Drawing.Point(128, 101);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(185, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Degrees Celcius Change";
            // 
            // TextBoxCelcius
            // 
            this.TextBoxCelcius.Location = new System.Drawing.Point(52, 98);
            this.TextBoxCelcius.Name = "TextBoxCelcius";
            this.TextBoxCelcius.Size = new System.Drawing.Size(56, 26);
            this.TextBoxCelcius.TabIndex = 5;
            this.TextBoxCelcius.Text = "2.0";
            // 
            // CheckBoxTempChange
            // 
            this.CheckBoxTempChange.AutoSize = true;
            this.CheckBoxTempChange.Checked = true;
            this.CheckBoxTempChange.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxTempChange.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CheckBoxTempChange.Location = new System.Drawing.Point(52, 59);
            this.CheckBoxTempChange.Name = "CheckBoxTempChange";
            this.CheckBoxTempChange.Size = new System.Drawing.Size(285, 24);
            this.CheckBoxTempChange.TabIndex = 6;
            this.CheckBoxTempChange.Text = "Autofocus on Temperature Change";
            this.CheckBoxTempChange.UseVisualStyleBackColor = true;
            // 
            // CheckBoxGoto
            // 
            this.CheckBoxGoto.AutoSize = true;
            this.CheckBoxGoto.Checked = true;
            this.CheckBoxGoto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.CheckBoxGoto.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.CheckBoxGoto.Location = new System.Drawing.Point(52, 23);
            this.CheckBoxGoto.Name = "CheckBoxGoto";
            this.CheckBoxGoto.Size = new System.Drawing.Size(175, 24);
            this.CheckBoxGoto.TabIndex = 7;
            this.CheckBoxGoto.Text = "AutoFocus on Goto";
            this.CheckBoxGoto.UseVisualStyleBackColor = true;
            // 
            // AutoFocus
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(541, 152);
            this.Controls.Add(this.CheckBoxGoto);
            this.Controls.Add(this.CheckBoxTempChange);
            this.Controls.Add(this.TextBoxCelcius);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonAutoFocus);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AutoFocus";
            this.Text = " AutoFocus";
            this.Load += new System.EventHandler(this.AutoFocus_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonAutoFocus;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox TextBoxCelcius;
        private System.Windows.Forms.CheckBox CheckBoxTempChange;
        private System.Windows.Forms.CheckBox CheckBoxGoto;
    }
}