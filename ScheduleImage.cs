using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Globalization; // Added for CultureInfo
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace OriginLiveView
{
    // Assumed GlobalData class definition
    

    public partial class ScheduleImage : Form
    {
        private readonly TelescopeControl _telescopeControl;
        private int CaptureTimeMinutes = 0;

        public ScheduleImage(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
        }

        private async void buttonScheduleImageCapture_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate inputs and collect errors
                var errors = new List<string>();
                if (string.IsNullOrWhiteSpace(textBoxTotalExposure.Text)) errors.Add("Total Exposure is required.");
                if (string.IsNullOrWhiteSpace(textBoxExposureTime.Text)) errors.Add("Exposure Time is required.");
                if (string.IsNullOrWhiteSpace(comboBoxISO.Text)) errors.Add("ISO is required.");
                if (string.IsNullOrWhiteSpace(textBoxStartTime.Text)) errors.Add("Start Time is required.");
                if (string.IsNullOrWhiteSpace(textBoxName.Text)) errors.Add("Name is required.");
                if (string.IsNullOrWhiteSpace(textBoxObjectMagnitude.Text)) errors.Add("Object Magnitude is required.");
                if (string.IsNullOrWhiteSpace(textBoxObjectSize.Text)) errors.Add("Object Size is required.");
                if (string.IsNullOrWhiteSpace(textBoxObjectRA.Text)) errors.Add("Right Ascension (RA) is required.");
                if (string.IsNullOrWhiteSpace(textBoxObjectDEC.Text)) errors.Add("Declination (DEC) is required.");

                if (errors.Any())
                {
                    MessageBox.Show(string.Join("\n", errors), "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Parse and validate numeric inputs
                if (!int.TryParse(textBoxTotalExposure.Text, out CaptureTimeMinutes) || CaptureTimeMinutes <= 0)
                {
                    MessageBox.Show("Total Exposure must be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(textBoxExposureTime.Text, out int exposureTime) || exposureTime <= 0)
                {
                    MessageBox.Show("Exposure Time must be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!int.TryParse(comboBoxISO.Text, out int iso) || iso <= 0)
                {
                    MessageBox.Show("ISO must be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxObjectMagnitude.Text, out double objectMagnitude))
                {
                    MessageBox.Show("Object Magnitude must be a valid number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxObjectSize.Text, out double objectSize) || objectSize <= 0)
                {
                    MessageBox.Show("Object Size must be a positive number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                const double MaxRA = 360.0;
                const double MinDec = -90.0;
                const double MaxDec = 90.0;
                if (!double.TryParse(textBoxObjectRA.Text, out double captureRA) || captureRA < 0 || captureRA > MaxRA)
                {
                    MessageBox.Show($"Right Ascension (RA) must be a number between 0 and {MaxRA} degrees.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxObjectDEC.Text, out double captureDec) || captureDec < MinDec || captureDec > MaxDec)
                {
                    MessageBox.Show($"Declination (DEC) must be a number between {MinDec} and {MaxDec} degrees.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate start time with specific format
                if (!DateTime.TryParseExact(textBoxStartTime.Text, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    MessageBox.Show("Start Time must be in the format 'dd MM yyyy HH:mm:ss'.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (startTime < DateTime.Now)
                {
                    MessageBox.Show("Start Time cannot be in the past.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update global data
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.MinStarttime = textBoxStartTime.Text;
                GlobalData.Name = textBoxName.Text.Trim();
                GlobalData.ObjectMagnitude = objectMagnitude;
                GlobalData.ObjectSize = objectSize;
                GlobalData.SaveRaw = CheckBoxSaveRaw.Checked;
                GlobalData.TotalDuration = CaptureTimeMinutes * 60;
                GlobalData.CaptureRA = captureRA;
                GlobalData.CaptureDec = captureDec;
                GlobalData.ScheduleAF = CheckBoxAF.Checked;
                GlobalData.SchedulePowerDown = CheckBoxPowerDown.Checked;
                GlobalData.DisableAutoCancel = checkBoxAutoCancel.Checked;
                // Disable UI to prevent multiple submissions
                buttonScheduleImageCapture.Enabled = false;
                try
                {
                    // Send command to telescope
                    await _telescopeControl.SendScheduleImageCommandAsync();
                    var logger = new CommandLogger();
                    logger.LogCommand($"ScheduleImage  {GlobalData.ExposureTime} {GlobalData.TotalDuration} {GlobalData.ISO} {GlobalData.MinStarttime} {GlobalData.Name}  {GlobalData.CaptureRA} {GlobalData.CaptureDec} {GlobalData.SaveRaw} {GlobalData.ObjectMagnitude} {GlobalData.ObjectSize} {GlobalData.ScheduleAF} {GlobalData.SchedulePowerDown}");
                    MessageBox.Show("Image capture scheduled successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    buttonScheduleImageCapture.Enabled = true;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Invalid input format: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error: {ex}");
                MessageBox.Show("An unexpected error occurred. Please try again.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            // Real-time validation for name field
            if (string.IsNullOrWhiteSpace(textBoxName.Text))
            {
                textBoxName.BackColor = Color.LightPink; // Highlight empty field
            }
            else
            {
                textBoxName.BackColor = SystemColors.Window; // Reset background
            }
        }

        private void ScheduleImage_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize form fields with global data
                textBoxObjectRA.Text = GlobalData.CaptureRA.ToString("F6"); // Format to 6 decimal places
                textBoxObjectDEC.Text = GlobalData.CaptureDec.ToString("F6");

                // Set default start time (rounded to next hour if minutes >= 30)
                var now = DateTime.Now;
                var targetHour = now.Minute >= 30 ? now.Hour + 1 : now.Hour;
                textBoxStartTime.Text = now.Date.AddHours(targetHour).ToString("dd MM yyyy HH:mm:ss");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}