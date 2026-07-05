using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OriginLiveView
{
    public partial class GotoRaDec : Form
    {
        private readonly TelescopeControl _telescopeControl;
        public GotoRaDec(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl;
        }

        private void GotoRaDec_Load(object sender, EventArgs e)
        {
            //TextBoxRA.Text = GlobalData.CaptureRA.ToString();
            //TextBoxDec.Text = GlobalData.CaptureDec.ToString();
        }

        private async void ButtonGoto_Click(object sender, EventArgs e)
        {
            try
            {
                // Parse the decimal degrees from the text boxes
                double raDegrees = Convert.ToDouble(TextBoxRA.Text);
                double decDegrees = Convert.ToDouble(TextBoxDec.Text);

                // Convert degrees to radians (degrees * π/180)
                double raRadians = raDegrees * Math.PI / 180.0;
                double decRadians = decDegrees * Math.PI / 180.0;

                // Assign to GlobalData variables
                GlobalData.GotoRA = raRadians;
                GlobalData.GotoDec = decRadians;
                var logger = new CommandLogger();
                logger.LogCommand($"GOTO {GlobalData.GotoRA}  {GlobalData.GotoDec}");
                await _telescopeControl.SendSlewToCommandAsync();
                GotoRaDec.ActiveForm?.Close();
            }
            catch (FormatException)
            {
                // Handle invalid input (e.g., non-numeric text)
                TextBoxRA.Text = "Invalid RA";
                TextBoxDec.Text = "Invalid Dec";
            }
            catch (Exception ex)
            {
                // Handle other errors
                TextBoxRA.Text = $"Error: {ex.Message}";
                TextBoxDec.Text = $"Error: {ex.Message}";
                
                
            }
        }
    }
}
