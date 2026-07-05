namespace OriginLiveView
{
    partial class Editor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Editor));
            this.txtEditor = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.runScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.stopScriptToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.textBoxLog = new System.Windows.Forms.TextBox();
            this.ButtonGoto = new System.Windows.Forms.Button();
            this.ButtonFilter = new System.Windows.Forms.Button();
            this.ButtonSpeak = new System.Windows.Forms.Button();
            this.ButtonMsgBox = new System.Windows.Forms.Button();
            this.ButtonWait = new System.Windows.Forms.Button();
            this.ButtonWaitUntil = new System.Windows.Forms.Button();
            this.ButtonStartCapture = new System.Windows.Forms.Button();
            this.ButtonHalt = new System.Windows.Forms.Button();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtEditor
            // 
            this.txtEditor.AcceptsReturn = true;
            this.txtEditor.AcceptsTab = true;
            this.txtEditor.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.txtEditor.ForeColor = System.Drawing.SystemColors.InfoText;
            this.txtEditor.Location = new System.Drawing.Point(0, 72);
            this.txtEditor.MaxLength = 3276712;
            this.txtEditor.Multiline = true;
            this.txtEditor.Name = "txtEditor";
            this.txtEditor.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtEditor.Size = new System.Drawing.Size(1006, 624);
            this.txtEditor.TabIndex = 0;
            this.txtEditor.TabStop = false;
            // 
            // menuStrip1
            // 
            this.menuStrip1.GripMargin = new System.Windows.Forms.Padding(2, 2, 0, 2);
            this.menuStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1006, 33);
            this.menuStrip1.TabIndex = 1;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openToolStripMenuItem,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.newToolStripMenuItem,
            this.runScriptToolStripMenuItem,
            this.stopScriptToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(54, 29);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(201, 34);
            this.openToolStripMenuItem.Text = "Open";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click_1);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(201, 34);
            this.saveToolStripMenuItem.Text = "Save";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click_1);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(201, 34);
            this.saveAsToolStripMenuItem.Text = "Save as";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(201, 34);
            this.newToolStripMenuItem.Text = "New";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click_1);
            // 
            // runScriptToolStripMenuItem
            // 
            this.runScriptToolStripMenuItem.Name = "runScriptToolStripMenuItem";
            this.runScriptToolStripMenuItem.Size = new System.Drawing.Size(201, 34);
            this.runScriptToolStripMenuItem.Text = "Run Script";
            this.runScriptToolStripMenuItem.Click += new System.EventHandler(this.runScriptToolStripMenuItem_Click);
            // 
            // stopScriptToolStripMenuItem
            // 
            this.stopScriptToolStripMenuItem.Name = "stopScriptToolStripMenuItem";
            this.stopScriptToolStripMenuItem.Size = new System.Drawing.Size(201, 34);
            this.stopScriptToolStripMenuItem.Text = "Stop Script";
            this.stopScriptToolStripMenuItem.Click += new System.EventHandler(this.stopScriptToolStripMenuItem_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.ShowReadOnly = true;
            // 
            // textBoxLog
            // 
            this.textBoxLog.AcceptsReturn = true;
            this.textBoxLog.AcceptsTab = true;
            this.textBoxLog.BackColor = System.Drawing.SystemColors.InactiveBorder;
            this.textBoxLog.ForeColor = System.Drawing.SystemColors.InfoText;
            this.textBoxLog.Location = new System.Drawing.Point(0, 702);
            this.textBoxLog.MaxLength = 3276712;
            this.textBoxLog.Multiline = true;
            this.textBoxLog.Name = "textBoxLog";
            this.textBoxLog.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxLog.Size = new System.Drawing.Size(1006, 257);
            this.textBoxLog.TabIndex = 2;
            this.textBoxLog.TabStop = false;
            // 
            // ButtonGoto
            // 
            this.ButtonGoto.Location = new System.Drawing.Point(8, 36);
            this.ButtonGoto.Name = "ButtonGoto";
            this.ButtonGoto.Size = new System.Drawing.Size(118, 37);
            this.ButtonGoto.TabIndex = 3;
            this.ButtonGoto.Text = "Goto";
            this.ButtonGoto.UseVisualStyleBackColor = true;
            this.ButtonGoto.Click += new System.EventHandler(this.ButtonGoto_Click);
            // 
            // ButtonFilter
            // 
            this.ButtonFilter.Location = new System.Drawing.Point(132, 36);
            this.ButtonFilter.Name = "ButtonFilter";
            this.ButtonFilter.Size = new System.Drawing.Size(118, 37);
            this.ButtonFilter.TabIndex = 4;
            this.ButtonFilter.Text = "Filter";
            this.ButtonFilter.UseVisualStyleBackColor = true;
            this.ButtonFilter.Click += new System.EventHandler(this.ButtonFilter_Click);
            // 
            // ButtonSpeak
            // 
            this.ButtonSpeak.Location = new System.Drawing.Point(256, 36);
            this.ButtonSpeak.Name = "ButtonSpeak";
            this.ButtonSpeak.Size = new System.Drawing.Size(118, 37);
            this.ButtonSpeak.TabIndex = 5;
            this.ButtonSpeak.Text = "Speak";
            this.ButtonSpeak.UseVisualStyleBackColor = true;
            this.ButtonSpeak.Click += new System.EventHandler(this.ButtonSpeak_Click);
            // 
            // ButtonMsgBox
            // 
            this.ButtonMsgBox.Location = new System.Drawing.Point(380, 36);
            this.ButtonMsgBox.Name = "ButtonMsgBox";
            this.ButtonMsgBox.Size = new System.Drawing.Size(118, 37);
            this.ButtonMsgBox.TabIndex = 6;
            this.ButtonMsgBox.Text = "MsgBox";
            this.ButtonMsgBox.UseVisualStyleBackColor = true;
            this.ButtonMsgBox.Click += new System.EventHandler(this.ButtonMsgBox_Click);
            // 
            // ButtonWait
            // 
            this.ButtonWait.Location = new System.Drawing.Point(504, 36);
            this.ButtonWait.Name = "ButtonWait";
            this.ButtonWait.Size = new System.Drawing.Size(118, 37);
            this.ButtonWait.TabIndex = 7;
            this.ButtonWait.Text = "Wait";
            this.ButtonWait.UseVisualStyleBackColor = true;
            this.ButtonWait.Click += new System.EventHandler(this.ButtonWait_Click);
            // 
            // ButtonWaitUntil
            // 
            this.ButtonWaitUntil.Location = new System.Drawing.Point(628, 36);
            this.ButtonWaitUntil.Name = "ButtonWaitUntil";
            this.ButtonWaitUntil.Size = new System.Drawing.Size(118, 37);
            this.ButtonWaitUntil.TabIndex = 8;
            this.ButtonWaitUntil.Text = "WaitUntil";
            this.ButtonWaitUntil.UseVisualStyleBackColor = true;
            this.ButtonWaitUntil.Click += new System.EventHandler(this.ButtonWaitUntil_Click);
            // 
            // ButtonStartCapture
            // 
            this.ButtonStartCapture.Location = new System.Drawing.Point(752, 36);
            this.ButtonStartCapture.Name = "ButtonStartCapture";
            this.ButtonStartCapture.Size = new System.Drawing.Size(118, 37);
            this.ButtonStartCapture.TabIndex = 9;
            this.ButtonStartCapture.Text = "StartCapture";
            this.ButtonStartCapture.UseVisualStyleBackColor = true;
            this.ButtonStartCapture.Click += new System.EventHandler(this.ButtonStartCapture_Click);
            // 
            // ButtonHalt
            // 
            this.ButtonHalt.Location = new System.Drawing.Point(876, 36);
            this.ButtonHalt.Name = "ButtonHalt";
            this.ButtonHalt.Size = new System.Drawing.Size(118, 37);
            this.ButtonHalt.TabIndex = 10;
            this.ButtonHalt.Text = "Halt";
            this.ButtonHalt.UseVisualStyleBackColor = true;
            this.ButtonHalt.Click += new System.EventHandler(this.ButtonHalt_Click);
            // 
            // Editor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(1006, 959);
            this.Controls.Add(this.ButtonHalt);
            this.Controls.Add(this.ButtonStartCapture);
            this.Controls.Add(this.ButtonWaitUntil);
            this.Controls.Add(this.ButtonWait);
            this.Controls.Add(this.ButtonMsgBox);
            this.Controls.Add(this.ButtonSpeak);
            this.Controls.Add(this.ButtonFilter);
            this.Controls.Add(this.ButtonGoto);
            this.Controls.Add(this.textBoxLog);
            this.Controls.Add(this.txtEditor);
            this.Controls.Add(this.menuStrip1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Editor";
            this.Text = " ";
            this.Load += new System.EventHandler(this.Editor_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtEditor;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripMenuItem runScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem stopScriptToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.TextBox textBoxLog;
        private System.Windows.Forms.Button ButtonGoto;
        private System.Windows.Forms.Button ButtonFilter;
        private System.Windows.Forms.Button ButtonSpeak;
        private System.Windows.Forms.Button ButtonMsgBox;
        private System.Windows.Forms.Button ButtonWait;
        private System.Windows.Forms.Button ButtonWaitUntil;
        private System.Windows.Forms.Button ButtonStartCapture;
        private System.Windows.Forms.Button ButtonHalt;
    }
}