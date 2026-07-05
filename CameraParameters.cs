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
    
    public partial class CameraParameters : Form
    {
        private readonly TelescopeControl _telescopeControl;
        public CameraParameters(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
        }

        private void CameraParameters_Load(object sender, EventArgs e)
        {
           TextBoxBin.Text = GlobalData.CameraBin.ToString();
            TextBoxBitDepth.Text = GlobalData.CameraBitDepth.ToString();
            TextBoxBlue.Text = GlobalData.CameraBlueGain.ToString();
            TextBoxGreen.Text = GlobalData.CameraGreenGain.ToString();
            TextBoxRed.Text = GlobalData.CameraRedGain.ToString();
            TextBoxISO.Text = GlobalData.ISO.ToString();
            TextBoxExposure.Text = GlobalData.CameraExposure.ToString();
        }

        private async void ButtonSet_Click(object sender, EventArgs e)
        {
            GlobalData.CameraBin = int.Parse(TextBoxBin.Text);
            GlobalData.CameraBitDepth = int.Parse(TextBoxBitDepth.Text);
            GlobalData.CameraBlueGain = double.Parse(TextBoxBlue.Text);
            GlobalData.CameraGreenGain = double.Parse(TextBoxGreen.Text);
            GlobalData.CameraRedGain = double.Parse(TextBoxRed.Text);
            GlobalData.ISO = int.Parse(TextBoxISO.Text);
            GlobalData.CameraExposure = double.Parse(TextBoxExposure.Text);  
            
            await _telescopeControl.SendCameraSettingsCommandAsync();
        }
       
    }
}
