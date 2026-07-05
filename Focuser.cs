using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OriginLiveView
{
    public partial class Focuser : Form
    {
        private readonly TelescopeControl _telescopeControl;
        private readonly Timer _updateTimer; // Timer for updating text box
        //private string FocuserString = null;

        public Focuser(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl;

            // Initialize the timer
            _updateTimer = new Timer
            {
                Interval = 1000 // 1000 ms = 1 second
            };
            _updateTimer.Tick += UpdateTimer_Tick; // Attach event handler
        }

        private void Focuser_Load(object sender, EventArgs e)
        {
            // Set initial value
            TextBoxPosition.Text = GlobalData.focuserposition.ToString();
            // Start the timer
            _updateTimer.Start();
        }

        // Timer event handler to update TextBoxPosition.Text
        private void UpdateTimer_Tick(object sender, EventArgs e)
        {
            TextBoxPosition.Text = GlobalData.focuserposition.ToString();
        }

        // Stop the timer when the form closes to prevent memory leaks
        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            _updateTimer.Stop();
            _updateTimer.Dispose();
            base.OnFormClosing(e);
        }

        private async void ButtonPlus1_Click(object sender, EventArgs e)
        {
            GlobalData.GotoFocuserPosition = GlobalData.focuserposition + 1;
            await _telescopeControl.SendSetFocusPositionCommandAsync();
        }

        private async void ButtonPlus10_Click(object sender, EventArgs e)
        {
            GlobalData.GotoFocuserPosition = GlobalData.focuserposition + 10;
            await _telescopeControl.SendSetFocusPositionCommandAsync();
        }

        private async void ButtonPlus100_Click(object sender, EventArgs e)
        {
            GlobalData.GotoFocuserPosition = GlobalData.focuserposition + 100;
            await _telescopeControl.SendSetFocusPositionCommandAsync();
        }

        private async void ButtonMin1_Click(object sender, EventArgs e)
        {
            GlobalData.GotoFocuserPosition = GlobalData.focuserposition - 1;
            await _telescopeControl.SendSetFocusPositionCommandAsync();
        }

        private async void ButtonMin10_Click(object sender, EventArgs e)
        {
            GlobalData.GotoFocuserPosition = GlobalData.focuserposition - 10;
            await _telescopeControl.SendSetFocusPositionCommandAsync();
        }

        private async void ButtonMin100_Click(object sender, EventArgs e)
        {
            GlobalData.GotoFocuserPosition = GlobalData.focuserposition - 100;
            await _telescopeControl.SendSetFocusPositionCommandAsync();
        }

        private async void ButtonAuto_Click(object sender, EventArgs e)
        {
         await _telescopeControl.SendAutoFocusCommandAsync();
        }
    }
}