using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Button;

namespace OriginLiveView
{
   
    

    public partial class OLVMosaic : Form
    {
        private readonly TelescopeControl _telescopeControl;
        private int CaptureTimeMinutes = 0;

        public OLVMosaic(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
            // Attach real-time validation
            textBoxStartTime.Validating += textBoxStartTime_Validating;
        }

        
        private void textBoxStartTime_Validating(object sender, CancelEventArgs e)
        {
            string input = textBoxStartTime.Text.Trim();
            if (!string.IsNullOrWhiteSpace(input) &&
                !DateTime.TryParseExact(input, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out _))
            {
                MessageBox.Show("Please enter the date and time in 'dd MM yyyy HH:mm:ss' format (e.g., '31 05 2025 14:00:00').", "Invalid Format", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                e.Cancel = true;
            }
        }
        private void LogMosaicFrame(int frameIndex, double frameRA, double frameDec, string frameStartTime, string mosaicName)
        {
            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
            string folderPath = Path.Combine(desktopPath, "OLVimages");
            string logFilePath = Path.Combine(folderPath, "OLVMosaicLog.txt");

            // Create directory if it doesn't exist
            Directory.CreateDirectory(folderPath);

            // Log entry format
            string logEntry = $"MosaicName={mosaicName}, Frame {frameIndex + 1}: RA={frameRA:F6}, DEC={frameDec:F6}, StartTime={frameStartTime}, Timestamp={DateTime.Now:yyyy-MM-dd HH:mm:ss}\n";

            // Append to file (creates file if it doesn't exist)
            File.AppendAllText(logFilePath, logEntry);
        }
        private void OLVMosaic_Load(object sender, EventArgs e)
        {
            try
            {
                // Initialize form fields with global data
                textBoxObjectRA.Text = GlobalData.CaptureRA.ToString("F6"); // Format to 6 decimal places
                textBoxObjectDEC.Text = GlobalData.CaptureDec.ToString("F6");

                // Set default start time (rounded to next hour if minutes >= 30)
                var now = DateTime.Now;
                var targetHour = now.Minute >= 30 ? now.Hour + 1 : now.Hour;
                if (targetHour >= 24) // Handle day rollover
                {
                    targetHour -= 24;
                    textBoxStartTime.Text = now.Date.AddDays(1).AddHours(targetHour).ToString("dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
                else
                {
                    textBoxStartTime.Text = now.Date.AddHours(targetHour).ToString("dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
        }

        private async void buttonScheduleImageCapture_Click_1(object sender, EventArgs e)
        {
            try
            {
                // Validate input fields
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

                // Validate numeric inputs
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

                const double MaxRA = 2 * Math.PI; // ≈6.283185 radians
                const double MinDec = -Math.PI / 2; // ≈-1.570796 radians
                const double MaxDec = Math.PI / 2; // ≈1.570796 radians
                if (!double.TryParse(textBoxObjectRA.Text, out double captureRA) || captureRA < 0 || captureRA > MaxRA)
                {
                    MessageBox.Show($"Right Ascension (RA) must be a number between 0 and {MaxRA} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxObjectDEC.Text, out double captureDec) || captureDec < MinDec || captureDec > MaxDec)
                {
                    MessageBox.Show($"Declination (DEC) must be a number between {MinDec} and {MaxDec} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate start time format
                string startTimeInput = textBoxStartTime.Text.Trim();
                if (!Regex.IsMatch(startTimeInput, @"^\d{2}\s\d{2}\s\d{4}\s\d{2}:\d{2}:\d{2}$"))
                {
                    MessageBox.Show("Start Time format is incorrect. Use 'dd MM yyyy HH:mm:ss' (e.g., '31 05 2025 14:00:00').", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParseExact(startTimeInput, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    MessageBox.Show("Start Time must be a valid date and time in the format 'dd MM yyyy HH:mm:ss' (e.g., '31 05 2025 14:00:00'). Ensure day is 01-31, month is 01-12, and time is 00:00:00-23:59:59.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (startTime < DateTime.Now)
                {
                    MessageBox.Show("Start Time cannot be in the past. Please choose a future date and time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate numDelay
                if (numDelay.Value < 0)
                {
                    MessageBox.Show("Delay must be a non-negative number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update global data (base settings)
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.MinStarttime = startTimeInput;
                GlobalData.Name = textBoxName.Text.Trim();
                GlobalData.ObjectMagnitude = objectMagnitude=10;
                GlobalData.ObjectSize = objectSize;
                GlobalData.SaveRaw = CheckBoxSaveRaw.Checked; // Respect user input, removed forced Checked = true
                GlobalData.TotalDuration = CaptureTimeMinutes * 60;
                GlobalData.CaptureRA = captureRA;
                GlobalData.CaptureDec = captureDec;
                GlobalData.ScheduleAF = CheckBoxAF.Checked;
                GlobalData.SchedulePowerDown = CheckBoxPowerDown.Checked;
                GlobalData.DisableAutoCancel = checkBoxAutoCancel.Checked;

                // Math for 2x2 mosaic (2 rows, 2 columns, centered on captureRA/captureDec, 10% overlap)
                // The field of view of the Celestron Origin is:
                // RA: 0.022168 radians (76.2 arcminutes)
                // DEC: 0.014836 radians (51 arcminutes)
                // All calculations in radians
                //178C Horizontal FOV = 7.3344 mm / 335 mm ≈ 0.02189 radians.
                //178C Vertical FOV = 4.9152 mm / 335 mm ≈ 0.01467 radians.
                //678C Horizontal FOV = 7.712 mm / 335 mm ≈ 0.02302 radians.
                //678C Vertical FOV = 4.36 mm / 335 mm ≈ 0.01301 radians.

                double overlapPercentage = 0.1;
                if (CheckBoxOverride.Checked) overlapPercentage = (double)NumericOverride.Value / 100; // Assume % input; divide by 100 if needed

                double raFov = 0.022168; //* (1 - overlapPercentage);  
                double decFov = 0.014836; //* (1 - overlapPercentage);
                switch (comboBoxCam.SelectedItem?.ToString())
                {
                    case "178C":
                        raFov = (0.02189) * (1 - overlapPercentage); // ≈ 0.02189 radians
                        decFov = (0.01467) * (1 - overlapPercentage); // ≈ 0.01467 radians
                        break;
                    case "678C":
                        raFov = (0.02302) * (1 - overlapPercentage); // ≈ 0.02302 radians
                        decFov = (0.01301) * (1 - overlapPercentage); // ≈ 0.01301 radians
                        break;
                }

                if (Math.Abs(captureDec) > 1.48352986) // ~85°
                    throw new Exception("Declination too close to pole; RA offset too large.");

                double decStepHalf = decFov / 2;
                double decUpper = captureDec + decStepHalf;
                double decLower = captureDec - decStepHalf;

                // Per-row raOffset
                double raOffsetUpper = raFov / Math.Cos(decUpper);
                double raOffsetLower = raFov / Math.Cos(decLower);

                double[,] raDecOffsets = new double[4, 2]
                {
                    { -raOffsetUpper / 2, decStepHalf }, // Left-upper
                    {  raOffsetUpper / 2, decStepHalf }, // Right-upper
                    { -raOffsetLower / 2, -decStepHalf }, // Left-lower
                    {  raOffsetLower / 2, -decStepHalf }  // Right-lower
                };

                // Validate bounds for all 4 frames (existing loop is fine, i < 4)

                // In scheduling loop (i < 4):
                // GlobalData.CaptureRA = captureRA + raDecOffsets[i, 0];
                // GlobalData.CaptureDec = captureDec + raDecOffsets[i, 1];

                // Validate RA/DEC bounds for all frames
                for (int i = 0; i < 4; i++)
                {
                    double frameRA = captureRA + raDecOffsets[i, 0];
                    double frameDec = captureDec + raDecOffsets[i, 1];
                    if (frameRA < 0 || frameRA > MaxRA)
                    {
                        MessageBox.Show($"Frame {i + 1} has invalid RA ({frameRA:F6}). Must be between 0 and {MaxRA} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (frameDec < MinDec || frameDec > MaxDec)
                    {
                        MessageBox.Show($"Frame {i + 1} has invalid DEC ({frameDec:F6}). Must be between {MinDec} and {MaxDec} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Math for time difference
                double timePerFrame = (double)CaptureTimeMinutes + (double)numDelay.Value;
                string[] frameStartTimes = new string[4];
                for (int i = 0; i < 4; i++)
                {
                    frameStartTimes[i] = startTime.AddMinutes(i * timePerFrame).ToString("dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }

                
                var logger = new CommandLogger();
                logger.LogCommand($"ScheduleEQMosaic2x2  {GlobalData.ExposureTime} {GlobalData.TotalDuration} {GlobalData.ISO} {GlobalData.MinStarttime} {GlobalData.Name}  {GlobalData.CaptureRA} {GlobalData.CaptureDec} {GlobalData.SaveRaw} {GlobalData.ObjectMagnitude} {GlobalData.ObjectSize} {GlobalData.ScheduleAF} {GlobalData.SchedulePowerDown} {GlobalData.DisableAutoCancel}");

                // Disable UI to prevent multiple submissions
                buttonScheduleImageCapture.Enabled = false;
                try
                {
                    // Schedule 4 frames
                    for (int i = 0; i < 4; i++)
                    {
                        GlobalData.CaptureRA = captureRA + raDecOffsets[i, 0];
                        GlobalData.CaptureDec = captureDec + raDecOffsets[i, 1];
                        GlobalData.MinStarttime = frameStartTimes[i];
                        LogMosaicFrame(i, GlobalData.CaptureRA, GlobalData.CaptureDec, frameStartTimes[i], GlobalData.Name); // Add logging
                        await _telescopeControl.SendScheduleImageCommandAsync();
                        
                        //ErrorLogger logger = new ErrorLogger();
                        //logger.LogError($"Command Schedule Image Sent to Origin: = {i}");
                    }

                    MessageBox.Show("2x2 scheduled successfully with 10% overlap, centered on input coordinates.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    buttonScheduleImageCapture.Enabled = true;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Invalid input format: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
        }

        private async void ButtonScheduleMosaic3_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input fields
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

                // Validate numeric inputs
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

                const double MaxRA = 2 * Math.PI; // ≈6.283185 radians
                const double MinDec = -Math.PI / 2; // ≈-1.570796 radians
                const double MaxDec = Math.PI / 2; // ≈1.570796 radians
                if (!double.TryParse(textBoxObjectRA.Text, out double captureRA) || captureRA < 0 || captureRA > MaxRA)
                {
                    MessageBox.Show($"Right Ascension (RA) must be a number between 0 and {MaxRA} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxObjectDEC.Text, out double captureDec) || captureDec < MinDec || captureDec > MaxDec)
                {
                    MessageBox.Show($"Declination (DEC) must be a number between {MinDec} and {MaxDec} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate start time format
                string startTimeInput = textBoxStartTime.Text.Trim();
                if (!Regex.IsMatch(startTimeInput, @"^\d{2}\s\d{2}\s\d{4}\s\d{2}:\d{2}:\d{2}$"))
                {
                    MessageBox.Show("Start Time format is incorrect. Use 'dd MM yyyy HH:mm:ss' (e.g., '31 05 2025 14:00:00').", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParseExact(startTimeInput, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    MessageBox.Show("Start Time must be a valid date and time in the format 'dd MM yyyy HH:mm:ss' (e.g., '31 05 2025 14:00:00'). Ensure day is 01-31, month is 01-12, and time is 00:00:00-23:59:59.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (startTime < DateTime.Now)
                {
                    MessageBox.Show("Start Time cannot be in the past. Please choose a future date and time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate numDelay
                if (numDelay.Value < 0)
                {
                    MessageBox.Show("Delay must be a non-negative number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update global data (base settings)
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.MinStarttime = startTimeInput;
                GlobalData.Name = textBoxName.Text.Trim();
                GlobalData.ObjectMagnitude = objectMagnitude;
                GlobalData.ObjectSize = objectSize;
                GlobalData.SaveRaw = CheckBoxSaveRaw.Checked; // Respect user input
                GlobalData.TotalDuration = CaptureTimeMinutes * 60;
                GlobalData.CaptureRA = captureRA;
                GlobalData.CaptureDec = captureDec;
                GlobalData.ScheduleAF = CheckBoxAF.Checked;
                GlobalData.SchedulePowerDown = CheckBoxPowerDown.Checked;
                GlobalData.DisableAutoCancel = checkBoxAutoCancel.Checked;

                // Math for 3x3 mosaic (3 rows, 3 columns, centered on captureRA/captureDec, 10% overlap)
                // The field of view of the Celestron Origin is:
                // RA: 0.022168 radians (76.2 arcminutes)
                // DEC: 0.014836 radians (51 arcminutes)
                // All calculations in radians

                double overlapPercentage = 0.1; // 10% overlap
                if (CheckBoxOverride.Checked)
                {
                    overlapPercentage = Convert.ToDouble(NumericOverride.Value) / 100; // Convert percentage (e.g., 10) to decimal (0.1)
                }
                if (overlapPercentage < 0 || overlapPercentage > 0.5)
                {
                    MessageBox.Show("Overlap must be between 0% and 50%.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //double raFov = 0.022168 * (1 - overlapPercentage); // Step size for 10% overlap (radians)
                //double decFov = 0.014836 * (1 - overlapPercentage); // Step size for 10% overlap (radians)
                double raFov = 0.022168; //* (1 - overlapPercentage);  
                double decFov = 0.014836; //* (1 - overlapPercentage);
                switch (comboBoxCam.SelectedItem?.ToString())
                {
                    case "178C":
                        raFov = (0.02189) * (1 - overlapPercentage); // ≈ 0.02189 radians
                        decFov = (0.01467) * (1 - overlapPercentage); // ≈ 0.01467 radians
                        break;
                    case "678C":
                        raFov = (0.02302) * (1 - overlapPercentage); // ≈ 0.02302 radians
                        decFov = (0.01301) * (1 - overlapPercentage); // ≈ 0.01301 radians
                        break;
                }


                if (Math.Abs(captureDec) > 1.48352986) // 85° in radians
                {
                    throw new Exception("Declination too close to pole; RA offset too large.");
                }

                double decOffset = decFov ; //  DEC step for 3 rows, centered on captureDec
                double decUpper = captureDec + decOffset;
                double decMiddle = captureDec;
                double decLower = captureDec - decOffset;

                // Calculate RA offset per row to account for DEC-dependent scaling
                double raOffsetUpper = raFov / Math.Cos(decUpper); // RA step for top row
                double raOffsetMiddle = raFov / Math.Cos(decMiddle); // RA step for middle row
                double raOffsetLower = raFov / Math.Cos(decLower); // RA step for bottom row

                double[,] raDecOffsets = new double[9, 2]
                {
    { -raOffsetUpper,  decOffset },  // Top-left
    {  0,             decOffset },  // Top-center
    {  raOffsetUpper,  decOffset },  // Top-right
    { -raOffsetMiddle, 0 },         // Middle-left
    {  0,             0 },         // Middle-center
    {  raOffsetMiddle, 0 },         // Middle-right
    { -raOffsetLower, -decOffset }, // Bottom-left
    {  0,            -decOffset }, // Bottom-center
    {  raOffsetLower, -decOffset }  // Bottom-right
                };

                // Validate RA/DEC bounds for all frames
                for (int i = 0; i < 9; i++)
                {
                    double frameRA = captureRA + raDecOffsets[i, 0];
                    double frameDec = captureDec + raDecOffsets[i, 1];
                    if (frameRA < 0 || frameRA > MaxRA)
                    {
                        MessageBox.Show($"Frame {i + 1} has invalid RA ({frameRA:F6}). Must be between 0 and {MaxRA} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (frameDec < MinDec || frameDec > MaxDec)
                    {
                        MessageBox.Show($"Frame {i + 1} has invalid DEC ({frameDec:F6}). Must be between {MinDec} and {MaxDec} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Math for time difference
                double timePerFrame = (double)CaptureTimeMinutes + (double)numDelay.Value;
                string[] frameStartTimes = new string[9];
                for (int i = 0; i < 9; i++)
                {
                    frameStartTimes[i] = startTime.AddMinutes(i * timePerFrame).ToString("dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }

                               
                var logger = new CommandLogger();
                logger.LogCommand($"ScheduleEQMosaic3x3  {GlobalData.ExposureTime} {GlobalData.TotalDuration} {GlobalData.ISO} {GlobalData.MinStarttime} {GlobalData.Name}  {GlobalData.CaptureRA} {GlobalData.CaptureDec} {GlobalData.SaveRaw} {GlobalData.ObjectMagnitude} {GlobalData.ObjectSize} {GlobalData.ScheduleAF} {GlobalData.SchedulePowerDown} {GlobalData.DisableAutoCancel}");

                // Disable UI to prevent multiple submissions
                ButtonScheduleMosaic3.Enabled = false;
                try
                {
                    // Schedule 9 frames
                    for (int i = 0; i < 9; i++)
                    {
                        GlobalData.CaptureRA = captureRA + raDecOffsets[i, 0];
                        GlobalData.CaptureDec = captureDec + raDecOffsets[i, 1];
                        GlobalData.MinStarttime = frameStartTimes[i];
                        LogMosaicFrame(i, GlobalData.CaptureRA, GlobalData.CaptureDec, frameStartTimes[i], GlobalData.Name); // Add logging
                        await _telescopeControl.SendScheduleImageCommandAsync();
                        
                    }

                    MessageBox.Show("3x3 scheduled successfully with 10% overlap, centered on input coordinates.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    ButtonScheduleMosaic3.Enabled = true;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Invalid input format: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
        }

        private async void Button12_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input fields
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

                // Validate numeric inputs
                if (!int.TryParse(textBoxTotalExposure.Text, out int captureTimeMinutes) || captureTimeMinutes <= 0)
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

                const double MaxRA = 2 * Math.PI; // ≈6.283185 radians
                const double MinDec = -Math.PI / 2; // ≈-1.570796 radians
                const double MaxDec = Math.PI / 2; // ≈1.570796 radians
                if (!double.TryParse(textBoxObjectRA.Text, out double captureRA) || captureRA < 0 || captureRA > MaxRA)
                {
                    MessageBox.Show($"Right Ascension (RA) must be a number between 0 and {MaxRA} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxObjectDEC.Text, out double captureDec) || captureDec < MinDec || captureDec > MaxDec)
                {
                    MessageBox.Show($"Declination (DEC) must be a number between {MinDec} and {MaxDec} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate start time format
                string startTimeInput = textBoxStartTime.Text.Trim();
                if (!Regex.IsMatch(startTimeInput, @"^\d{2}\s\d{2}\s\d{4}\s\d{2}:\d{2}:\d{2}$"))
                {
                    MessageBox.Show("Start Time format is incorrect. Use 'dd MM yyyy HH:mm:ss' (e.g., '31 05 2025 14:00:00').", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParseExact(startTimeInput, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    MessageBox.Show("Start Time must be a valid date and time in the format 'dd MM yyyy HH:mm:ss' (e.g., '31 05 2025 14:00:00'). Ensure day is 01-31, month is 01-12, and time is 00:00:00-23:59:59.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (startTime < DateTime.Now)
                {
                    MessageBox.Show("Start Time cannot be in the past. Please choose a future date and time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate numDelay
                if (numDelay.Value < 0)
                {
                    MessageBox.Show("Delay must be a non-negative number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update global data (base settings)
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.MinStarttime = startTimeInput;
                GlobalData.Name = textBoxName.Text.Trim();
                GlobalData.ObjectMagnitude = objectMagnitude;
                GlobalData.ObjectSize = objectSize;
                GlobalData.SaveRaw = CheckBoxSaveRaw.Checked;
                GlobalData.TotalDuration = captureTimeMinutes * 60;
                GlobalData.CaptureRA = captureRA;
                GlobalData.CaptureDec = captureDec;
                GlobalData.ScheduleAF = CheckBoxAF.Checked;
                GlobalData.SchedulePowerDown = CheckBoxPowerDown.Checked;
                GlobalData.DisableAutoCancel = checkBoxAutoCancel.Checked;

                // Math for 2x1 mosaic (vertical, centered on captureRA/captureDec, 10% overlap in DEC)
                double overlapPercentage = 0.1; // 10% overlap
                if (CheckBoxOverride.Checked)
                {
                    overlapPercentage = Convert.ToDouble(NumericOverride.Value);
                }

                //178C Horizontal FOV = 7.3344 mm / 335 mm ≈ 0.02189 radians.
                //178C Vertical FOV = 4.9152 mm / 335 mm ≈ 0.01467 radians.
                //678C Horizontal FOV = 7.712 mm / 335 mm ≈ 0.02302 radians.
                //678C Vertical FOV = 4.36 mm / 335 mm ≈ 0.01301 radians.
                double raFov = 0.022168; //* (1 - overlapPercentage);  
                double decFov = 0.014836; //* (1 - overlapPercentage);
                switch (comboBoxCam.SelectedItem?.ToString())
                {
                    case "178C":
                        raFov = (0.02189) * (1 - overlapPercentage); // ≈ 0.02189 radians
                        decFov = (0.01467) * (1 - overlapPercentage); // ≈ 0.01467 radians
                        break;
                    case "678C":
                        raFov = (0.02302) * (1 - overlapPercentage); // ≈ 0.02302 radians
                        decFov = (0.01301) * (1 - overlapPercentage); // ≈ 0.01301 radians
                        break;
                }
                //double decFov = 0.014836 * (1 - overlapPercentage); // 90% of 51' in radians (45.9')
                double decOffset = decFov / 2; // Half the separation for centering
                double[,] raDecOffsets = new double[2, 2]
                {
            { 0,  decOffset }, // Top (DEC + offset)
            { 0, -decOffset }  // Bottom (DEC - offset)
                };

                // Validate RA/DEC bounds for all frames
                for (int i = 0; i < 2; i++)
                {
                    double frameRA = captureRA + raDecOffsets[i, 0];
                    double frameDec = captureDec + raDecOffsets[i, 1];
                    if (frameRA < 0 || frameRA > MaxRA)
                    {
                        MessageBox.Show($"Frame {i + 1} has invalid RA ({frameRA:F6}). Must be between 0 and {MaxRA} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (frameDec < MinDec || frameDec > MaxDec)
                    {
                        MessageBox.Show($"Frame {i + 1} has invalid DEC ({frameDec:F6}). Must be between {MinDec} and {MaxDec} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Math for time difference
                double timePerFrame = (double)captureTimeMinutes + (double)numDelay.Value;
                string[] frameStartTimes = new string[2];
                for (int i = 0; i < 2; i++)
                {
                    frameStartTimes[i] = startTime.AddMinutes(i * timePerFrame).ToString("dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }

                // Store base name
                
                var logger = new CommandLogger();
                logger.LogCommand($"ScheduleEQMosaic2x1  {GlobalData.ExposureTime} {GlobalData.TotalDuration} {GlobalData.ISO} {GlobalData.MinStarttime} {GlobalData.Name}  {GlobalData.CaptureRA} {GlobalData.CaptureDec} {GlobalData.SaveRaw} {GlobalData.ObjectMagnitude} {GlobalData.ObjectSize} {GlobalData.ScheduleAF} {GlobalData.SchedulePowerDown} {GlobalData.DisableAutoCancel}");

                // Disable UI to prevent multiple submissions
                Button12.Enabled = false;
                try
                {
                    // Schedule 2 frames
                    for (int i = 0; i < 2; i++)
                    {
                        GlobalData.CaptureRA = captureRA + raDecOffsets[i, 0];
                        GlobalData.CaptureDec = captureDec + raDecOffsets[i, 1];
                        GlobalData.MinStarttime = frameStartTimes[i];
                        LogMosaicFrame(i, GlobalData.CaptureRA, GlobalData.CaptureDec, frameStartTimes[i], GlobalData.Name); // Add logging
                        await _telescopeControl.SendScheduleImageCommandAsync();
                        
                    }

                    MessageBox.Show("2x1 scheduled successfully , centered on input coordinates.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    Button12.Enabled = true;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Invalid input format: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
        }

        private async void Button23_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input fields
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

                // Validate numeric inputs
                if (!int.TryParse(textBoxTotalExposure.Text, out int captureTimeMinutes) || captureTimeMinutes <= 0)
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

                const double MaxRA = 2 * Math.PI; // ≈6.283185 radians
                const double MinDec = -Math.PI / 2; // ≈-1.570796 radians
                const double MaxDec = Math.PI / 2; // ≈1.570796 radians
                if (!double.TryParse(textBoxObjectRA.Text, out double captureRA) || captureRA < 0 || captureRA > MaxRA)
                {
                    MessageBox.Show($"Right Ascension (RA) must be a number between 0 and {MaxRA} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxObjectDEC.Text, out double captureDec) || captureDec < MinDec || captureDec > MaxDec)
                {
                    MessageBox.Show($"Declination (DEC) must be a number between {MinDec} and {MaxDec} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate start time format
                string startTimeInput = textBoxStartTime.Text.Trim();
                if (!Regex.IsMatch(startTimeInput, @"^\d{2}\s\d{2}\s\d{4}\s\d{2}:\d{2}:\d{2}$"))
                {
                    MessageBox.Show("Start Time format is incorrect. Use 'dd MM yyyy HH:mm:ss' (e.g., '31 05 2025 14:00:00').", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParseExact(startTimeInput, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    MessageBox.Show("Start Time must be a valid date and time in the format 'dd MM yyyy HH:mm:ss' (e.g., '31 05 2025 14:00:00'). Ensure day is 01-31, month is 01-12, and time is 00:00:00-23:59:59.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (startTime < DateTime.Now)
                {
                    MessageBox.Show("Start Time cannot be in the past. Please choose a future date and time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate numDelay
                if (numDelay.Value < 0)
                {
                    MessageBox.Show("Delay must be a non-negative number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update global data (base settings)
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.MinStarttime = startTimeInput;
                GlobalData.Name = textBoxName.Text.Trim();
                GlobalData.ObjectMagnitude = objectMagnitude;
                GlobalData.ObjectSize = objectSize;
                GlobalData.SaveRaw = CheckBoxSaveRaw.Checked;
                GlobalData.TotalDuration = captureTimeMinutes * 60;
                GlobalData.CaptureRA = captureRA;
                GlobalData.CaptureDec = captureDec;
                GlobalData.ScheduleAF = CheckBoxAF.Checked;
                GlobalData.SchedulePowerDown = CheckBoxPowerDown.Checked;
                GlobalData.DisableAutoCancel = checkBoxAutoCancel.Checked;
                // Math for 2x3 mosaic (2 rows, 3 columns, centered on captureRA/captureDec, 10% overlap)
                // The field of view of the Celestron Origin is:
                // RA: 0.022168 radians (76.2 arcminutes)
                // DEC: 0.014836 radians (51 arcminutes)
                // All calculations in radians

                double overlapPercentage = 0.1; // 10% overlap
                if (CheckBoxOverride.Checked)
                {
                    overlapPercentage = Convert.ToDouble(NumericOverride.Value) / 100; // Convert percentage (e.g., 10) to decimal (0.1)
                }

                //double raFov = 0.022168 * (1 - overlapPercentage); // Step size for 10% overlap (radians)
                //double decFov = 0.014836 * (1 - overlapPercentage); // Step size for 10% overlap (radians)
                double raFov = 0.022168; //* (1 - overlapPercentage);  
                double decFov = 0.014836; //* (1 - overlapPercentage);
                switch (comboBoxCam.SelectedItem?.ToString())
                {
                    case "178C":
                        raFov = (0.02189) * (1 - overlapPercentage); // ≈ 0.02189 radians
                        decFov = (0.01467) * (1 - overlapPercentage); // ≈ 0.01467 radians
                        break;
                    case "678C":
                        raFov = (0.02302) * (1 - overlapPercentage); // ≈ 0.02302 radians
                        decFov = (0.01301) * (1 - overlapPercentage); // ≈ 0.01301 radians
                        break;
                }

                if (Math.Abs(captureDec) > 1.48352986) // 85° in radians
                {
                    throw new Exception("Declination too close to pole; RA offset too large.");
                }

                double decOffset = decFov / 2; // Half DEC step for 2 rows
                double decUpper = captureDec + decOffset;
                double decLower = captureDec - decOffset;

                // Calculate RA offset per row to account for DEC-dependent scaling
                double raOffsetUpper = raFov / Math.Cos(decUpper); // RA step for top row
                double raOffsetLower = raFov / Math.Cos(decLower); // RA step for bottom row

                double[,] raDecOffsets = new double[6, 2]
                {
                     { -raOffsetUpper,  decOffset }, // Top-left
                     {  0,             decOffset }, // Top-center
                     {  raOffsetUpper,  decOffset }, // Top-right
                     { -raOffsetLower, -decOffset }, // Bottom-left
                     {  0,            -decOffset }, // Bottom-center
                     {  raOffsetLower, -decOffset }  // Bottom-right
                };

                // Validate RA/DEC bounds for all frames
                for (int i = 0; i < 6; i++)
                {
                    double frameRA = captureRA + raDecOffsets[i, 0];
                    double frameDec = captureDec + raDecOffsets[i, 1];
                    if (frameRA < 0 || frameRA > MaxRA)
                    {
                        MessageBox.Show($"Frame {i + 1} has invalid RA ({frameRA:F6}). Must be between 0 and {MaxRA} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (frameDec < MinDec || frameDec > MaxDec)
                    {
                        MessageBox.Show($"Frame {i + 1} has invalid DEC ({frameDec:F6}). Must be between {MinDec} and {MaxDec} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Math for time difference
                double timePerFrame = (double)captureTimeMinutes + (double)numDelay.Value;
                string[] frameStartTimes = new string[6];
                for (int i = 0; i < 6; i++)
                {
                    frameStartTimes[i] = startTime.AddMinutes(i * timePerFrame).ToString("dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }

                var logger = new CommandLogger();
                logger.LogCommand($"ScheduleEQMosaic3x2  {GlobalData.ExposureTime} {GlobalData.TotalDuration} {GlobalData.ISO} {GlobalData.MinStarttime} {GlobalData.Name}  {GlobalData.CaptureRA} {GlobalData.CaptureDec} {GlobalData.SaveRaw} {GlobalData.ObjectMagnitude} {GlobalData.ObjectSize} {GlobalData.ScheduleAF} {GlobalData.SchedulePowerDown} {GlobalData.DisableAutoCancel}");

                // Disable UI to prevent multiple submissions
                Button23.Enabled = false;
                try
                {
                    // Schedule 6 frames
                    for (int i = 0; i < 6; i++)
                    {
                        GlobalData.CaptureRA = captureRA + raDecOffsets[i, 0];
                        GlobalData.CaptureDec = captureDec + raDecOffsets[i, 1];
                        GlobalData.MinStarttime = frameStartTimes[i];
                        LogMosaicFrame(i, GlobalData.CaptureRA, GlobalData.CaptureDec, frameStartTimes[i], GlobalData.Name); // Add logging
                        await _telescopeControl.SendScheduleImageCommandAsync();
                        
                    }

                    MessageBox.Show("2x3 scheduled successfully with 10% overlap, centered on input coordinates.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    Button23.Enabled = true;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Invalid input format: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
        }

        private async void Button32_Click(object sender, EventArgs e)
        {
            try
            {
                // ==================== ALL VALIDATION (unchanged) ====================
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

                if (!int.TryParse(textBoxTotalExposure.Text, out int captureTimeMinutes) || captureTimeMinutes <= 0)
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

                const double MaxRA = 2 * Math.PI;
                const double MinDec = -Math.PI / 2;
                const double MaxDec = Math.PI / 2;

                if (!double.TryParse(textBoxObjectRA.Text, out double captureRA) || captureRA < 0 || captureRA > MaxRA)
                {
                    MessageBox.Show($"Right Ascension (RA) must be a number between 0 and {MaxRA:F6} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(textBoxObjectDEC.Text, out double captureDec) || captureDec < MinDec || captureDec > MaxDec)
                {
                    MessageBox.Show($"Declination (DEC) must be a number between {MinDec:F6} and {MaxDec:F6} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string startTimeInput = textBoxStartTime.Text.Trim();
                if (!Regex.IsMatch(startTimeInput, @"^\d{2}\s\d{2}\s\d{4}\s\d{2}:\d{2}:\d{2}$") ||
                    !DateTime.TryParseExact(startTimeInput, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime) ||
                    startTime < DateTime.Now)
                {
                    MessageBox.Show("Start Time is invalid or in the past.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (numDelay.Value < 0)
                {
                    MessageBox.Show("Delay must be non-negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ==================== GLOBAL DATA UPDATE ====================
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.MinStarttime = startTimeInput;
                GlobalData.Name = textBoxName.Text.Trim();
                GlobalData.ObjectMagnitude = objectMagnitude;
                GlobalData.ObjectSize = objectSize;
                GlobalData.SaveRaw = CheckBoxSaveRaw.Checked;
                GlobalData.TotalDuration = captureTimeMinutes * 60;
                GlobalData.CaptureRA = captureRA;
                GlobalData.CaptureDec = captureDec;
                GlobalData.ScheduleAF = CheckBoxAF.Checked;
                GlobalData.SchedulePowerDown = CheckBoxPowerDown.Checked;
                GlobalData.DisableAutoCancel = checkBoxAutoCancel.Checked;

                // ==================== 3×2 MOSAIC MATH (NOW CORRECT!) ====================
                double overlapPercentage = CheckBoxOverride.Checked
                    ? Convert.ToDouble(NumericOverride.Value) / 100.0
                    : 0.1;

                if (overlapPercentage < 0 || overlapPercentage > 0.5)
                {
                    MessageBox.Show("Overlap must be between 0% and 50%.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double raFov = 0.022168 * (1 - overlapPercentage);
                double decFov = 0.014836 * (1 - overlapPercentage);

                switch (comboBoxCam.SelectedItem?.ToString())
                {
                    case "178C":
                        raFov = 0.02189 * (1 - overlapPercentage);
                        decFov = 0.01467 * (1 - overlapPercentage);
                        break;
                    case "678C":
                        raFov = 0.02302 * (1 - overlapPercentage);
                        decFov = 0.01301 * (1 - overlapPercentage);
                        break;
                }

                if (Math.Abs(captureDec) > 1.48352986) // ~85°
                    throw new Exception("Declination too close to pole; RA offset too large.");

                // Row declinations
                double decTop = captureDec + decFov;
                double decMiddle = captureDec;
                double decBottom = captureDec - decFov;

                // RA scaling per row
                double raOffsetTop = raFov / Math.Cos(decTop);
                double raOffsetMiddle = raFov / Math.Cos(decMiddle);
                double raOffsetBottom = raFov / Math.Cos(decBottom);

                double[,] raDecOffsets = new double[6, 2]
                {
            { -raOffsetTop / 2,   decFov },  // Top-left
            {  raOffsetTop / 2,   decFov },  // Top-right
            { -raOffsetMiddle / 2, 0      },  // Middle-left
            {  raOffsetMiddle / 2, 0      },  // Middle-right
            { -raOffsetBottom / 2,-decFov },  // Bottom-left
            {  raOffsetBottom / 2,-decFov }   // Bottom-right
                };

                // ==================== VALIDATION & SCHEDULING ====================
                for (int i = 0; i < 6; i++)
                {
                    double frameRA = captureRA + raDecOffsets[i, 0];
                    double frameDec = captureDec + raDecOffsets[i, 1];

                    if (frameRA < 0 || frameRA > MaxRA || frameDec < MinDec || frameDec > MaxDec)
                    {
                        MessageBox.Show($"Frame {i + 1} out of bounds (RA: {frameRA:F6}, Dec: {frameDec:F6})", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                double timePerFrame = captureTimeMinutes + (double)numDelay.Value;
                string[] frameStartTimes = new string[6];
                for (int i = 0; i < 6; i++)
                    frameStartTimes[i] = startTime.AddMinutes(i * timePerFrame).ToString("dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                var logger = new CommandLogger();
                logger.LogCommand($"ScheduleEQMosaic3x2 {GlobalData.ExposureTime} {GlobalData.TotalDuration} {GlobalData.ISO} {GlobalData.MinStarttime} {GlobalData.Name} {GlobalData.CaptureRA} {GlobalData.CaptureDec} {GlobalData.SaveRaw} {GlobalData.ObjectMagnitude} {GlobalData.ObjectSize} {GlobalData.ScheduleAF} {GlobalData.SchedulePowerDown} {GlobalData.DisableAutoCancel}");

                Button32.Enabled = false;
                try
                {
                    for (int i = 0; i < 6; i++)
                    {
                        GlobalData.CaptureRA = captureRA + raDecOffsets[i, 0];
                        GlobalData.CaptureDec = captureDec + raDecOffsets[i, 1];
                        GlobalData.MinStarttime = frameStartTimes[i];
                        LogMosaicFrame(i, GlobalData.CaptureRA, GlobalData.CaptureDec, frameStartTimes[i], GlobalData.Name);
                        await _telescopeControl.SendScheduleImageCommandAsync();
                    }
                    MessageBox.Show("3×2 mosaic scheduled successfully!\nCorrect per-row RA scaling applied.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    Button32.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
        }

        private async void Button44_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input fields
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

                // Validate numeric inputs
                if (!int.TryParse(textBoxTotalExposure.Text, out int captureTimeMinutes) || captureTimeMinutes <= 0)
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

                const double MaxRA = 2 * Math.PI; // ≈6.283185 radians
                const double MinDec = -Math.PI / 2; // ≈-1.570796 radians
                const double MaxDec = Math.PI / 2; // ≈1.570796 radians
                if (!double.TryParse(textBoxObjectRA.Text, out double captureRA) || captureRA < 0 || captureRA > MaxRA)
                {
                    MessageBox.Show($"Right Ascension (RA) must be a number between 0 and {MaxRA:F6} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxObjectDEC.Text, out double captureDec) || captureDec < MinDec || captureDec > MaxDec)
                {
                    MessageBox.Show($"Declination (DEC) must be a number between {MinDec:F6} and {MaxDec:F6} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate start time format
                string startTimeInput = textBoxStartTime.Text.Trim();
                if (!Regex.IsMatch(startTimeInput, @"^\d{2}\s\d{2}\s\d{4}\s\d{2}:\d{2}:\d{2}$"))
                {
                    MessageBox.Show("Start Time format is incorrect. Use 'dd MM yyyy HH:mm:ss' (e.g., '31 05 2025 14:00:00').", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!DateTime.TryParseExact(startTimeInput, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime))
                {
                    MessageBox.Show("Start Time must be a valid date and time in the format 'dd MM yyyy HH:mm:ss' (e.g., '31 05 2025 14:00:00'). Ensure day is 01-31, month is 01-12, and time is 00:00:00-23:59:59.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (startTime < DateTime.Now)
                {
                    MessageBox.Show("Start Time cannot be in the past. Please choose a future date and time.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate numDelay
                if (numDelay?.Value < 0)
                {
                    MessageBox.Show("Delay must be a non-negative number.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Incorporate overlap percentage override
                double overlapPercentage = 0.1; // 10% overlap
                if (CheckBoxOverride.Checked)
                {
                    overlapPercentage = Convert.ToDouble(NumericOverride.Value);
                }

                // Update global data
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.MinStarttime = startTimeInput;
                GlobalData.Name = textBoxName.Text.Trim();
                GlobalData.ObjectMagnitude = objectMagnitude;
                GlobalData.ObjectSize = objectSize;
                GlobalData.SaveRaw = CheckBoxSaveRaw?.Checked ?? false;
                GlobalData.TotalDuration = captureTimeMinutes * 60;
                GlobalData.CaptureRA = captureRA;
                GlobalData.CaptureDec = captureDec;
                GlobalData.ScheduleAF = CheckBoxAF?.Checked ?? false;
                GlobalData.SchedulePowerDown = CheckBoxPowerDown?.Checked ?? false;
                GlobalData.DisableAutoCancel = checkBoxAutoCancel.Checked;

                // Math for 4x4 mosaic (4 rows, 4 columns, centered on captureRA/captureDec, 10% overlap)
                // The field of view of the Celestron Origin is:
                // RA: 0.022168 radians (76.2 arcminutes)
                // DEC: 0.014836 radians (51 arcminutes)
                // All calculations in radians


                if (CheckBoxOverride.Checked)
                {
                    overlapPercentage = Convert.ToDouble(NumericOverride.Value) / 100; // Convert percentage (e.g., 10) to decimal (0.1)
                }
                if (overlapPercentage < 0 || overlapPercentage > 0.5)
                {
                    MessageBox.Show("Overlap must be between 0% and 50%.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                //double raFov = 0.022168 * (1 - overlapPercentage); // Step size for 10% overlap (radians)
                //double decFov = 0.014836 * (1 - overlapPercentage); // Step size for 10% overlap (radians)

                double raFov = 0.022168; //* (1 - overlapPercentage);  
                double decFov = 0.014836; //* (1 - overlapPercentage);
                switch (comboBoxCam.SelectedItem?.ToString())
                {
                    case "178C":
                        raFov = (0.02189) * (1 - overlapPercentage); // ≈ 0.02189 radians
                        decFov = (0.01467) * (1 - overlapPercentage); // ≈ 0.01467 radians
                        break;
                    case "678C":
                        raFov = (0.02302) * (1 - overlapPercentage); // ≈ 0.02302 radians
                        decFov = (0.01301) * (1 - overlapPercentage); // ≈ 0.01301 radians
                        break;
                }

                if (Math.Abs(captureDec) > 1.48352986) // 85° in radians
                {
                    throw new Exception("Declination too close to pole; RA offset too large.");
                }

                double decOffsetStep = decFov ; // DEC step for 4 rows, centered on captureDec
                double decTop = captureDec + 1.5 * decOffsetStep;
                double decSecond = captureDec + 0.5 * decOffsetStep;
                double decThird = captureDec - 0.5 * decOffsetStep;
                double decBottom = captureDec - 1.5 * decOffsetStep;

                // Calculate RA offset per row to account for DEC-dependent scaling
                double raOffsetTop = raFov / Math.Cos(decTop); // RA step for top row
                double raOffsetSecond = raFov / Math.Cos(decSecond); // RA step for second row
                double raOffsetThird = raFov / Math.Cos(decThird); // RA step for third row
                double raOffsetBottom = raFov / Math.Cos(decBottom); // RA step for bottom row

                double[,] raDecOffsets = new double[16, 2]
                {
                        { -1.5 * raOffsetTop,    1.5 * decOffsetStep }, // Top row, left to right
                        { -0.5 * raOffsetTop,    1.5 * decOffsetStep },
                        {  0.5 * raOffsetTop,    1.5 * decOffsetStep },
                        {  1.5 * raOffsetTop,    1.5 * decOffsetStep },
                        { -1.5 * raOffsetSecond, 0.5 * decOffsetStep }, // Second row
                        { -0.5 * raOffsetSecond, 0.5 * decOffsetStep },
                        {  0.5 * raOffsetSecond, 0.5 * decOffsetStep },
                        {  1.5 * raOffsetSecond, 0.5 * decOffsetStep },
                        { -1.5 * raOffsetThird, -0.5 * decOffsetStep }, // Third row
                        { -0.5 * raOffsetThird, -0.5 * decOffsetStep },
                        {  0.5 * raOffsetThird, -0.5 * decOffsetStep },
                        {  1.5 * raOffsetThird, -0.5 * decOffsetStep },
                        { -1.5 * raOffsetBottom, -1.5 * decOffsetStep }, // Bottom row
                        { -0.5 * raOffsetBottom, -1.5 * decOffsetStep },
                        {  0.5 * raOffsetBottom, -1.5 * decOffsetStep },
                        {  1.5 * raOffsetBottom, -1.5 * decOffsetStep }
                };

                // Validate RA/DEC bounds for all frames
                for (int i = 0; i < 16; i++)
                {
                    double frameRA = captureRA + raDecOffsets[i, 0];
                    double frameDec = captureDec + raDecOffsets[i, 1];
                    if (frameRA < 0 || frameRA > MaxRA)
                    {
                        MessageBox.Show($"Frame {i + 1} has invalid RA ({frameRA:F6}). Must be between 0 and {MaxRA:F6} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                    if (frameDec < MinDec || frameDec > MaxDec)
                    {
                        MessageBox.Show($"Frame {i + 1} has invalid DEC ({frameDec:F6}). Must be between {MinDec:F6} and {MaxDec:F6} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Calculate frame start times
                double timePerFrame = captureTimeMinutes + Convert.ToDouble(numDelay?.Value ?? 0);
                string[] frameStartTimes = new string[16];
                for (int i = 0; i < 16; i++)
                {
                    frameStartTimes[i] = startTime.AddMinutes(i * timePerFrame).ToString("dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                }

                // Log command
                var logger = new CommandLogger();
                logger.LogCommand($"ScheduleEQMosaic4x4 {GlobalData.ExposureTime} {GlobalData.TotalDuration} {GlobalData.ISO} {GlobalData.MinStarttime} {GlobalData.Name} {GlobalData.CaptureRA} {GlobalData.CaptureDec} {GlobalData.SaveRaw} {GlobalData.ObjectMagnitude} {GlobalData.ObjectSize} {GlobalData.ScheduleAF} {GlobalData.SchedulePowerDown} {GlobalData.DisableAutoCancel}");

                // Disable UI to prevent multiple submissions
                Button44.Enabled = false;
                try
                {
                    // Schedule 16 frames
                    for (int i = 0; i < 16; i++)
                    {
                        GlobalData.CaptureRA = captureRA + raDecOffsets[i, 0];
                        GlobalData.CaptureDec = captureDec + raDecOffsets[i, 1];
                        GlobalData.MinStarttime = frameStartTimes[i];
                        LogMosaicFrame(i + 1, GlobalData.CaptureRA, GlobalData.CaptureDec, frameStartTimes[i], GlobalData.Name);
                        await _telescopeControl.SendScheduleImageCommandAsync();
                    }

                    MessageBox.Show("4x4 mosaic scheduled successfully with specified overlap, centered on input coordinates.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    Button44.Enabled = true;
                }
            }
            catch (FormatException ex)
            {
                MessageBox.Show($"Invalid input format: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An unexpected error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
        }

        private async void Button43_Click(object sender, EventArgs e)
        {
            try
            {
                // ==================== VALIDATION (same as all others) ====================
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

                if (!int.TryParse(textBoxTotalExposure.Text, out int captureTimeMinutes) || captureTimeMinutes <= 0)
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

                const double MaxRA = 2 * Math.PI;
                const double MinDec = -Math.PI / 2;
                const double MaxDec = Math.PI / 2;

                if (!double.TryParse(textBoxObjectRA.Text, out double captureRA) || captureRA < 0 || captureRA > MaxRA)
                {
                    MessageBox.Show($"Right Ascension (RA) must be between 0 and {MaxRA:F6} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (!double.TryParse(textBoxObjectDEC.Text, out double captureDec) || captureDec < MinDec || captureDec > MaxDec)
                {
                    MessageBox.Show($"Declination (DEC) must be between {MinDec:F6} and {MaxDec:F6} radians.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                string startTimeInput = textBoxStartTime.Text.Trim();
                if (!Regex.IsMatch(startTimeInput, @"^\d{2}\s\d{2}\s\d{4}\s\d{2}:\d{2}:\d{2}$") ||
                    !DateTime.TryParseExact(startTimeInput, "dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime startTime) ||
                    startTime < DateTime.Now)
                {
                    MessageBox.Show("Start Time is invalid or in the past.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                if (numDelay.Value < 0)
                {
                    MessageBox.Show("Delay must be non-negative.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // ==================== GLOBAL DATA UPDATE ====================
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.MinStarttime = startTimeInput;
                GlobalData.Name = textBoxName.Text.Trim();
                GlobalData.ObjectMagnitude = objectMagnitude;
                GlobalData.ObjectSize = objectSize;
                GlobalData.SaveRaw = CheckBoxSaveRaw.Checked;
                GlobalData.TotalDuration = captureTimeMinutes * 60;
                GlobalData.CaptureRA = captureRA;
                GlobalData.CaptureDec = captureDec;
                GlobalData.ScheduleAF = CheckBoxAF.Checked;
                GlobalData.SchedulePowerDown = CheckBoxPowerDown.Checked;
                GlobalData.DisableAutoCancel = checkBoxAutoCancel.Checked;

                // ==================== 4×3 MOSAIC — 4 ROWS TALL × 3 COLUMNS WIDE ====================
                double overlapPercentage = CheckBoxOverride.Checked
                    ? Convert.ToDouble(NumericOverride.Value) / 100.0
                    : 0.1;

                if (overlapPercentage < 0 || overlapPercentage > 0.5)
                {
                    MessageBox.Show("Overlap must be between 0% and 50%.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double raFov = 0.022168 * (1 - overlapPercentage);
                double decFov = 0.014836 * (1 - overlapPercentage);

                switch (comboBoxCam.SelectedItem?.ToString())
                {
                    case "178C":
                        raFov = 0.02189 * (1 - overlapPercentage);
                        decFov = 0.01467 * (1 - overlapPercentage);
                        break;
                    case "678C":
                        raFov = 0.02302 * (1 - overlapPercentage);
                        decFov = 0.01301 * (1 - overlapPercentage);
                        break;
                }

                if (Math.Abs(captureDec) > 1.48352986) // ~85°
                    throw new Exception("Declination too close to pole; RA offset too large.");

                // Declination positions for 4 rows (centered)
                double decRow1 = captureDec + 1.5 * decFov;   // Top row
                double decRow2 = captureDec + 0.5 * decFov;
                double decRow3 = captureDec - 0.5 * decFov;
                double decRow4 = captureDec - 1.5 * decFov;   // Bottom row

                // RA scaling for each row
                double raStepRow1 = raFov / Math.Cos(decRow1);
                double raStepRow2 = raFov / Math.Cos(decRow2);
                double raStepRow3 = raFov / Math.Cos(decRow3);
                double raStepRow4 = raFov / Math.Cos(decRow4);

                double[,] raDecOffsets = new double[12, 2]
                {
                    // Row 1 (top)
                    { -raStepRow1,  1.5 * decFov },
                    {  0,           1.5 * decFov },
                    {  raStepRow1,  1.5 * decFov },

                    // Row 2
                    { -raStepRow2,  0.5 * decFov },
                    {  0,           0.5 * decFov },
                    {  raStepRow2,  0.5 * decFov },

                    // Row 3
                    { -raStepRow3, -0.5 * decFov },
                    {  0,          -0.5 * decFov },
                    {  raStepRow3, -0.5 * decFov },

                    // Row 4 (bottom)
                    { -raStepRow4, -1.5 * decFov },
                    {  0,          -1.5 * decFov },
                    {  raStepRow4, -1.5 * decFov }
                };

                // Validate bounds
                for (int i = 0; i < 12; i++)
                {
                    double frameRA = captureRA + raDecOffsets[i, 0];
                    double frameDec = captureDec + raDecOffsets[i, 1];
                    if (frameRA < 0 || frameRA > MaxRA || frameDec < MinDec || frameDec > MaxDec)
                    {
                        MessageBox.Show($"Frame {i + 1} out of bounds (RA: {frameRA:F6}, Dec: {frameDec:F6})", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Timing
                double timePerFrame = captureTimeMinutes + (double)numDelay.Value;
                string[] frameStartTimes = new string[12];
                for (int i = 0; i < 12; i++)
                    frameStartTimes[i] = startTime.AddMinutes(i * timePerFrame).ToString("dd MM yyyy HH:mm:ss", CultureInfo.InvariantCulture);

                var logger = new CommandLogger();
                logger.LogCommand($"ScheduleEQMosaic4x3Tall {GlobalData.ExposureTime} {GlobalData.TotalDuration} {GlobalData.ISO} {GlobalData.MinStarttime} {GlobalData.Name} {GlobalData.CaptureRA} {GlobalData.CaptureDec} {GlobalData.SaveRaw} {GlobalData.ObjectMagnitude} {GlobalData.ObjectSize} {GlobalData.ScheduleAF} {GlobalData.SchedulePowerDown} {GlobalData.DisableAutoCancel}");

                Button43.Enabled = false;
                try
                {
                    for (int i = 0; i < 12; i++)
                    {
                        GlobalData.CaptureRA = captureRA + raDecOffsets[i, 0];
                        GlobalData.CaptureDec = captureDec + raDecOffsets[i, 1];
                        GlobalData.MinStarttime = frameStartTimes[i];
                        LogMosaicFrame(i, GlobalData.CaptureRA, GlobalData.CaptureDec, frameStartTimes[i], GlobalData.Name);
                        await _telescopeControl.SendScheduleImageCommandAsync();
                    }
                    MessageBox.Show("4×3 Tall mosaic (4 rows × 3 columns) scheduled successfully!\nPerfect for vertical deep-sky objects.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                finally
                {
                    Button43.Enabled = true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                System.Diagnostics.Debug.WriteLine($"Error: {ex}");
            }
        }
    }
}