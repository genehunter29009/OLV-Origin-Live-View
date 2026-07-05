using System;
using System.Drawing;
using System.Net.WebSockets;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Windows.Forms;
using static OriginLiveView.MainForm;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace OriginLiveView
{
    public partial class CaptureImage : Form
    {
        private int CaptureTimeMinutes = 0;
        private int CaptureTimeSeconds = 0;
        private readonly TelescopeControl _telescopeControl;

        public CaptureImage(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize form fields with default values if needed
                textBoxName.Text = $"Capture_{GlobalData.SquenceID + 1}"; // Set default name
                textBoxTotalExposure.Text = "5"; // Default to 1 minute
                textBoxExposure.Text = "20"; // Default to 1 second
                textBoxISO.Text = "200"; // Default ISO
                CheckBoxSaveRaw.Checked = true; // Default to save raw
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonClose_Click(object sender, EventArgs e)
        {
            try
            {
                // Send stop capture command
                await _telescopeControl.SendStopCaptureCommandAsync();
                MessageBox.Show("Capture stopped successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error stopping capture: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void buttonStartImage_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input fields
                if (string.IsNullOrWhiteSpace(textBoxTotalExposure.Text) ||
                    string.IsNullOrWhiteSpace(textBoxExposure.Text) ||
                    string.IsNullOrWhiteSpace(textBoxISO.Text) ||
                    string.IsNullOrWhiteSpace(textBoxName.Text))
                {
                    MessageBox.Show("All fields must be filled.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate numeric inputs
                if (!int.TryParse(textBoxTotalExposure.Text, out CaptureTimeMinutes) || CaptureTimeMinutes <= 0)
                {
                    MessageBox.Show("Total Exposure must be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(textBoxExposure.Text, out int exposureTime) || exposureTime <= 0)
                {
                    MessageBox.Show("Exposure Time must be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(textBoxISO.Text, out int iso) || iso <= 0)
                {
                    MessageBox.Show("ISO must be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Ensure exposure time is reasonable relative to total duration
                if (exposureTime > CaptureTimeMinutes * 60)
                {
                    MessageBox.Show("Exposure Time cannot exceed Total Exposure duration.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Set default radio button state
                CheckBoxSaveRaw.Checked = true;

                // Perform calculations and assignments
                CaptureTimeSeconds = CaptureTimeMinutes * 60;

                // Update global data
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.SquenceID++; // Increment sequence ID
                GlobalData.Name = textBoxName.Text.Trim(); // Trim to avoid extra spaces
                GlobalData.TotalDuration = CaptureTimeSeconds;
                GlobalData.SaveRaw = CheckBoxSaveRaw.Checked;
                GlobalData.DisableAutoCancel = checkBoxAutoCancel.Checked;

                // Send start capture command
                await _telescopeControl.SendStartCaptureCommandAsync();
                var logger = new CommandLogger();
                logger.LogCommand($"CaptureImage  {GlobalData.ExposureTime} {GlobalData.ISO} {GlobalData.TotalDuration} {GlobalData.Name} {GlobalData.SaveRaw}");
                MessageBox.Show("Capture started successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void labelExposureName_Click(object sender, EventArgs e)
        {
            // Optional: Add functionality if needed
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add real-time validation for name field
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                textBoxName.BackColor = Color.LightPink; // Highlight empty field
            }
            else
            {
                textBoxName.BackColor = SystemColors.Window; // Reset background
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}