namespace OriginLiveView
{
    partial class DewHeater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DewHeater));
            this.TrackBarAgressiveness = new System.Windows.Forms.TrackBar();
            this.TrackBarPower = new System.Windows.Forms.TrackBar();
            this.LabelAgressivness = new System.Windows.Forms.Label();
            this.LabelPower = new System.Windows.Forms.Label();
            this.ButtonAuto = new System.Windows.Forms.Button();
            this.ButtonPower = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarAgressiveness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarPower)).BeginInit();
            this.SuspendLayout();
            // 
            // TrackBarAgressiveness
            // 
            this.TrackBarAgressiveness.Location = new System.Drawing.Point(35, 26);
            this.TrackBarAgressiveness.Minimum = 1;
            this.TrackBarAgressiveness.Name = "TrackBarAgressiveness";
            this.TrackBarAgressiveness.Size = new System.Drawing.Size(173, 69);
            this.TrackBarAgressiveness.TabIndex = 0;
            this.TrackBarAgressiveness.Value = 5;
            // 
            // TrackBarPower
            // 
            this.TrackBarPower.Location = new System.Drawing.Point(35, 111);
            this.TrackBarPower.Maximum = 100;
            this.TrackBarPower.Minimum = 1;
            this.TrackBarPower.Name = "TrackBarPower";
            this.TrackBarPower.Size = new System.Drawing.Size(162, 69);
            this.TrackBarPower.TabIndex = 1;
            this.TrackBarPower.Value = 10;
            // 
            // LabelAgressivness
            // 
            this.LabelAgressivness.AutoSize = true;
            this.LabelAgressivness.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LabelAgressivness.Location = new System.Drawing.Point(49, 75);
            this.LabelAgressivness.Name = "LabelAgressivness";
            this.LabelAgressivness.Size = new System.Drawing.Size(148, 20);
            this.LabelAgressivness.TabIndex = 2;
            this.LabelAgressivness.Text = "Agressiveness 1-10";
            // 
            // LabelPower
            // 
            this.LabelPower.AutoSize = true;
            this.LabelPower.ForeColor = System.Drawing.SystemColors.ButtonHighlight;
            this.LabelPower.Location = new System.Drawing.Point(49, 171);
            this.LabelPower.Name = "LabelPower";
            this.LabelPower.Size = new System.Drawing.Size(139, 20);
            this.LabelPower.TabIndex = 3;
            this.LabelPower.Text = "Power Level 1-100";
            // 
            // ButtonAuto
            // 
            this.ButtonAuto.Location = new System.Drawing.Point(243, 39);
            this.ButtonAuto.Name = "ButtonAuto";
            this.ButtonAuto.Size = new System.Drawing.Size(170, 41);
            this.ButtonAuto.TabIndex = 4;
            this.ButtonAuto.Text = "Set Auto";
            this.ButtonAuto.UseVisualStyleBackColor = true;
            this.ButtonAuto.Click += new System.EventHandler(this.ButtonAuto_Click);
            // 
            // ButtonPower
            // 
            this.ButtonPower.ForeColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ButtonPower.Location = new System.Drawing.Point(243, 123);
            this.ButtonPower.Name = "ButtonPower";
            this.ButtonPower.Size = new System.Drawing.Size(170, 40);
            this.ButtonPower.TabIndex = 5;
            this.ButtonPower.Text = "Set Manual";
            this.ButtonPower.UseVisualStyleBackColor = true;
            this.ButtonPower.Click += new System.EventHandler(this.ButtonPower_Click);
            // 
            // DewHeater
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(457, 223);
            this.Controls.Add(this.ButtonPower);
            this.Controls.Add(this.ButtonAuto);
            this.Controls.Add(this.LabelPower);
            this.Controls.Add(this.LabelAgressivness);
            this.Controls.Add(this.TrackBarPower);
            this.Controls.Add(this.TrackBarAgressiveness);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "DewHeater";
            this.Text = " Dew Heater Control";
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarAgressiveness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TrackBarPower)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TrackBar TrackBarAgressiveness;
        private System.Windows.Forms.TrackBar TrackBarPower;
        private System.Windows.Forms.Label LabelAgressivness;
        private System.Windows.Forms.Label LabelPower;
        private System.Windows.Forms.Button ButtonAuto;
        private System.Windows.Forms.Button ButtonPower;
    }
}