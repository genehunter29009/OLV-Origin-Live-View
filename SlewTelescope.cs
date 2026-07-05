using System;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OriginLiveView
{
    public partial class SlewTelescope : Form
    {
        private readonly TelescopeControl _telescopeControl;

        public SlewTelescope(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
            // Enable form-level key events
            this.KeyPreview = true;
            this.KeyDown += SlewTelescope_KeyDown; // Start slewing on key press
            this.KeyUp += SlewTelescope_KeyUp; // Stop slewing on key release
            // Wire up MouseDown and MouseUp for buttons
            buttonSlewU.MouseDown += ButtonSlew_MouseDown;
            buttonSlewU.MouseUp += ButtonSlew_MouseUp;
            buttonSlewD.MouseDown += ButtonSlew_MouseDown;
            buttonSlewD.MouseUp += ButtonSlew_MouseUp;
            buttonSlewR.MouseDown += ButtonSlew_MouseDown;
            buttonSlewR.MouseUp += ButtonSlew_MouseUp;
            buttonSlewL.MouseDown += ButtonSlew_MouseDown;
            buttonSlewL.MouseUp += ButtonSlew_MouseUp;
            // Ensure form is focusable
            this.Activated += (s, e) => this.Focus();
        }

        private void Focus_Load(object sender, EventArgs e)
        {
        }

        private void buttonAutoFocus_Click(object sender, EventArgs e)
        {
        }

        private void trackBar1_Scroll(object sender, EventArgs e)
        {
            GlobalData.SlewSpeed = trackBarSlewSpeed.Value;
            label1.Text = $"Speed: {trackBarSlewSpeed.Value}"; // Update label1
        }

        // Placeholder Click handlers to prevent designer errors
        private void buttonSlewU_Click(object sender, EventArgs e) { }
        private void buttonSlewD_Click(object sender, EventArgs e) { }
        private void buttonSlewR_Click(object sender, EventArgs e) { }
        private void buttonSlewL_Click(object sender, EventArgs e) { }

        private async void ButtonSlew_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (trackBarSlewSpeed.Value <= 0)
                {
                    MessageBox.Show("Please select a positive slew speed.");
                    return;
                }
                //string direction = "";
                if (sender == buttonSlewU)
                {
                    GlobalData.SlewAltRate = trackBarSlewSpeed.Value; // Upward slewing
                    GlobalData.SlewAzRate = 0;
                    //direction = "Up";
                }
                else if (sender == buttonSlewD)
                {
                    GlobalData.SlewAltRate = -trackBarSlewSpeed.Value; // Downward slewing
                    GlobalData.SlewAzRate = 0;
                    //direction = "Down";
                }
                else if (sender == buttonSlewR)
                {
                    GlobalData.SlewAltRate = 0;
                    GlobalData.SlewAzRate = trackBarSlewSpeed.Value; // Rightward slewing
                    //direction = "Right";
                }
                else if (sender == buttonSlewL)
                {
                    GlobalData.SlewAltRate = 0;
                    GlobalData.SlewAzRate = -trackBarSlewSpeed.Value; // Leftward slewing
                    //direction = "Left";
                }
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                //Console.WriteLine($"MouseDown Slew{direction}: AltRate={GlobalData.SlewAltRate}, AzRate={GlobalData.SlewAzRate}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error slewing: {ex.Message}");
            }
        }

        private async void ButtonSlew_MouseUp(object sender, MouseEventArgs e)
        {
            await StopSlewing("MouseUp");
        }

        private async void SlewTelescope_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (trackBarSlewSpeed.Value <= 0)
                {
                    //Console.WriteLine("Invalid slew speed, skipping KeyDown");
                    return;
                }
                //string direction = "";
                switch (e.KeyCode)
                {
                    case Keys.Up:
                        GlobalData.SlewAltRate = trackBarSlewSpeed.Value; // Upward slewing
                        GlobalData.SlewAzRate = 0;
                        //direction = "Up";
                        break;
                    case Keys.Down:
                        GlobalData.SlewAltRate = -trackBarSlewSpeed.Value; // Downward slewing
                        GlobalData.SlewAzRate = 0;
                        //direction = "Down";
                        break;
                    case Keys.Right:
                        GlobalData.SlewAltRate = 0;
                        GlobalData.SlewAzRate = trackBarSlewSpeed.Value; // Rightward slewing
                        //direction = "Right";
                        break;
                    case Keys.Left:
                        GlobalData.SlewAltRate = 0;
                        GlobalData.SlewAzRate = -trackBarSlewSpeed.Value; // Leftward slewing
                        //direction = "Left";
                        break;
                    default:
                        return; // Ignore other keys
                }
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                //Console.WriteLine($"KeyDown Slew{direction}: AltRate={GlobalData.SlewAltRate}, AzRate={GlobalData.SlewAzRate}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error slewing: {ex.Message}");
            }
        }

        private async void SlewTelescope_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
               // Console.WriteLine($"KeyUp detected: {e.KeyCode}");
                if (e.KeyCode == Keys.Up || e.KeyCode == Keys.Down || e.KeyCode == Keys.Left || e.KeyCode == Keys.Right)
                {
                    await StopSlewing($"KeyUp {e.KeyCode}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping slew: {ex.Message}");
            }
        }

        private async Task StopSlewing(string source)
        {
            try
            {
                //Console.WriteLine($"{source}: Stopping slew");
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                //Console.WriteLine($"{source}: Sent first stop command");
                // Short delay to allow telescope to process
                await Task.Delay(200);
                // Retry stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                //Console.WriteLine($"{source}: Sent second stop command after 200ms delay");
                // TODO: If this doesn't stop slewing, try a dedicated stop command (e.g., StopSlewAsync)
                // await _telescopeControl.StopSlewAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping slew: {ex.Message}");
            }
        }
    }
}