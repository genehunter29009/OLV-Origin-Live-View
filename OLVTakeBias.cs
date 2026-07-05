using System;
using System.IO;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OriginLiveView
{
    public partial class OLVTakeBias : Form
    {
        private readonly TelescopeControl _telescopeControl;
        private const int exposureTime = 0; // Bias frames typically have no exposure time
        private int iso = 100; // ISO for bias frames is usually set to 100
        public OLVTakeBias(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
        }

        private void OLVTakeBias_Load(object sender, EventArgs e)
        {
            LabelCamTemp.Text = GlobalData.CameraTemperature.ToString("F1") + " C";
        }
        
        private async void buttonStartImage_Click(object sender, EventArgs e)
        {
            GlobalData.BiasName = "Bias-" + GlobalData.CameraTemperature + "C" + "-ISO" + textBoxISO.Text; // Set the name for the bias frames
            GlobalData.BiasCaptureFlag = true; // Set the flag to indicate bias capture is in progress
            GlobalData.FlatCaptureFlag = false; // Ensure flat capture flag is reset
            try
            {
                // Update global data
                GlobalData.BiasExposuretime = double.Parse(textBoxExposure.Text);// Set bias exposure time
                int isoValue;
                if (int.TryParse(textBoxISO.Text, out isoValue))
                {
                    GlobalData.ISO = isoValue; // Assign the parsed integer
                }
                GlobalData.BitDepth = 16; // Set bit depth for bias frames
                GlobalData.Binning = 1; // Set binning factor for bias frames
                GlobalData.Debayer = false; // Bias frames are not debayered
                GlobalData.Calibrate = false; // Bias frames are not calibrated
                GlobalData.SquenceID++; // Increment sequence ID
               
                // Send start capture command
                await _telescopeControl.SendRunSampleCaptureCommandAsync();
                //MessageBox.Show("Bias capture started successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var logger = new ErrorLogger();
                logger.LogError($"Successfully Started taking Bias frames {GlobalData.Name}");
                GlobalData.BiasNameInt++; // Increment the bias name integer for the next capture
                LabelCamTemp.Text = GlobalData.CameraTemperature.ToString("F1") + " C";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LabelCamTemp_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void labelExposureTime_Click(object sender, EventArgs e)
        {

        }

        private void textBoxISO_TextChanged(object sender, EventArgs e)
        {

        }

        private void labelExposure_Click(object sender, EventArgs e)
        {

        }

        private void textBoxExposure_TextChanged(object sender, EventArgs e)
        {

        }
    }
}