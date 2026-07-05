using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OriginLiveView
{
    public partial class CaptureMosaic : Form
    {
       private readonly TelescopeControl _telescopeControl;       // Add this field
        
        public CaptureMosaic(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));

        }

        private async void ButtonCaptureMosaic_Click(object sender, EventArgs e)
        {
            // Named constants for conversion factors
            const double HeightConversionFactor = 0.01454; // e.g., degrees to radians
            const double WidthConversionFactor = 0.02182;  // e.g., degrees to radians
            const double DegreesToRadians = Math.PI / 180.0;

            // List to collect validation errors
            var errors = new List<string>();

            // Parse and validate inputs
            if (!double.TryParse(ComboBoxHeight.Text, out double height) || height <= 0)
                errors.Add("Height must be a positive number.");
            if (!double.TryParse(ComboBoxWidth.Text, out double width) || width <= 0)
                errors.Add("Width must be a positive number.");
            if (!double.TryParse(TextBoxOrientation.Text, out double orientation) || orientation < 0 || orientation > 360)
                errors.Add("Orientation must be between 0 and 360 degrees.");
            if (!int.TryParse(TextBoxExposure.Text, out int exposureTime) || exposureTime <= 0)
                errors.Add("Exposure time must be a positive integer.");
            if (!int.TryParse(ComboBoxISO.Text, out int iso) || iso < 100 || iso > 12800) // Example ISO range
                errors.Add("ISO must be a valid integer between 100 and 12800.");
            if (!double.TryParse(TextBoxObjMag.Text, out double objectMagnitude))
                errors.Add("Object magnitude must be a valid number.");
            if (!double.TryParse(TextBoxObjSize.Text, out double objectSize) || objectSize <= 0)
                errors.Add("Object size must be a positive number.");
            if (!int.TryParse(TextBoxExpTotal.Text, out int totalDuration) || totalDuration <= 0)
                errors.Add("Total duration must be a positive integer.");
            if (string.IsNullOrWhiteSpace(TextBoxStartTime.Text))
                errors.Add("Start time cannot be empty.");
            if (string.IsNullOrWhiteSpace(TextBoxObjName.Text))
                errors.Add("Object name cannot be empty.");

            // Display errors if any
            if (errors.Any())
            {
                MessageBox.Show(string.Join("\n", errors), "Input Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Convert and store data
            try
            {
                GlobalData.MosaicHeight = height * HeightConversionFactor;
                GlobalData.MosaicWidth = width * WidthConversionFactor;
                GlobalData.MosaicOrientation = orientation * DegreesToRadians;
                GlobalData.MosaicAF = CheckBoxAF.Checked;
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.MinStarttime = TextBoxStartTime.Text;
                GlobalData.Name = TextBoxObjName.Text;
                GlobalData.ObjectMagnitude = objectMagnitude;
                GlobalData.ObjectSize = objectSize;
                GlobalData.SaveRaw = CheckBoxSaveRaw.Checked;
                GlobalData.TotalDuration = totalDuration * 60; // Convert minutes to seconds
                GlobalData.MosaicPowerDown = CheckBoxPowerDown.Checked;

                // Send command to telescope
                await _telescopeControl.SendScheduleMosaicImageCommandAsync();
                MessageBox.Show("Mosaic capture command sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to send mosaic command: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CaptureMosaic_Load(object sender, EventArgs e)
        {
            try
            {
                // Validate and format RA and Dec
                if (double.IsNaN(GlobalData.CaptureRA) || double.IsInfinity(GlobalData.CaptureRA))
                {
                    TextBoxRA.Text = "Invalid RA";
                }
                else
                {
                    // Format RA using invariant culture for consistent decimal representation
                    TextBoxRA.Text = GlobalData.CaptureRA.ToString("F6", CultureInfo.InvariantCulture);
                }

                if (double.IsNaN(GlobalData.CaptureDec) || double.IsInfinity(GlobalData.CaptureDec))
                {
                    TextBoxDec.Text = "Invalid Dec";
                }
                else
                {
                    TextBoxDec.Text = GlobalData.CaptureDec.ToString("F6", CultureInfo.InvariantCulture);
                }

                // Calculate start time (rounded to current or next hour)
                DateTime now = DateTime.Now;
                DateTime startTime = now.Minute >= 30
                    ? now.Date.AddHours(now.Hour + 1)
                    : now.Date.AddHours(now.Hour);

                // Use culture-aware or user-preferred format
                TextBoxStartTime.Text = startTime.ToString("MM dd yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error initializing form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void ButtonComposeMosaic_Click(object sender, EventArgs e)
        {
            try
            {
                await _telescopeControl.SendComposeMosaicImageCommandAsync();
                MessageBox.Show("Mosaic composition command sent successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to compose mosaic: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
