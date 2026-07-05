using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OriginLiveView
{
    public partial class OLVTakeDark : Form
    {

        private int CaptureTimeMinutes = 0;
        private int CaptureTimeSeconds = 0;
        private readonly TelescopeControl _telescopeControl;
        public OLVTakeDark(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
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
                

                // Perform calculations and assignments
                CaptureTimeSeconds = CaptureTimeMinutes * 60;

                // Update global data
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.SquenceID++; // Increment sequence ID
                GlobalData.Name = textBoxName.Text.Trim(); // Trim to avoid extra spaces
                GlobalData.TotalDuration = CaptureTimeSeconds;
                GlobalData.SaveRaw = true;

                // Send start capture command
                await _telescopeControl.SendStartCaptureCommandAsync();
                MessageBox.Show("Image capture started successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
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

                
                // Perform calculations and assignments
                CaptureTimeSeconds = CaptureTimeMinutes * 60;

                // Update global data
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.SquenceID++; // Increment sequence ID
                GlobalData.Name = textBoxName.Text.Trim(); // Trim to avoid extra spaces
                GlobalData.TotalDuration = CaptureTimeSeconds;
                GlobalData.SaveRaw = true;

                // Send start capture command
                await _telescopeControl.SendStartCaptureCommandAsync();
                MessageBox.Show("Image capture started successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void OLVTakeDark_Load(object sender, EventArgs e)
        {
            LabelCamTemp.Text = GlobalData.CameraTemperature.ToString("F1") + " C";
        }

        private void Button_name_Click(object sender, EventArgs e)
        {
            textBoxName.Text = "dark-" + textBoxExposure.Text + "Sec-" + textBoxISO.Text + "ISO-" + GlobalData.CameraTemperature +"C";
        }

        private async void ButtonDownload_Click(object sender, EventArgs e)
        {
            string darksName = textBoxName.Text.Trim();
            if (string.IsNullOrWhiteSpace(darksName))
            {
                MessageBox.Show("Please enter a name in the text box.", "Input Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            //buttonDownload.Enabled = false;
            var logger = new ErrorLogger();

            try
            {
                // Define paths
                string ftpHost = "ftp://origin.local";
                string ftpPath = "/Astrophotography";
                string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                string darksBasePath = Path.Combine(desktopPath, "OLVImages", "darks", darksName);

                // Create destination folder
                Directory.CreateDirectory(darksBasePath);

                // Get list of matching directories
                var directories = await Task.Run(() => ListFtpDirectories(ftpHost + ftpPath, darksName));
                if (!directories.Any())
                {
                    MessageBox.Show($"No folders found containing '{darksName}' at {ftpHost}{ftpPath}.", "No Matches", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    return;
                }

                int filesDownloaded = 0;
                foreach (string dir in directories)
                {
                    filesDownloaded += await DownloadLightFitsFiles(dir, darksBasePath);
                }

                MessageBox.Show($"Successfully downloaded and renamed {filesDownloaded} Light FITS file(s) to {darksBasePath}.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                logger.LogError($"Failed to download darks: {ex.Message}");
                MessageBox.Show($"Error downloading darks: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                //buttonDownload.Enabled = true;
            }
        }

        private List<string> ListFtpDirectories(string ftpUrl, string searchText)
        {
            var directories = new List<string>();
            var request = (FtpWebRequest)WebRequest.Create(ftpUrl);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential("anonymous", "");
            request.UseBinary = true;
            request.KeepAlive = false;

            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        if (line.StartsWith("d") && line.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0)
                        {
                            string dirName = line.Substring(line.LastIndexOf(' ') + 1);
                            directories.Add($"{ftpUrl}/{dirName}");
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                throw new Exception($"Failed to list directories at {ftpUrl}: {ex.Message}", ex);
            }
            return directories;
        }

        private async Task<int> DownloadLightFitsFiles(string ftpDirUrl, string localDirPath)
        {
            int filesDownloaded = 0;
            var request = (FtpWebRequest)WebRequest.Create(ftpDirUrl);
            request.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            request.Credentials = new NetworkCredential("anonymous", "");
            request.UseBinary = true;
            request.KeepAlive = false;

            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var reader = new StreamReader(stream))
                {
                    string line;
                    while ((line = reader.ReadLine()) != null)
                    {
                        string name = line.Substring(line.LastIndexOf(' ') + 1);
                        string fullFtpPath = $"{ftpDirUrl}/{name}";

                        if (line.StartsWith("d"))
                        {
                            // Recursively process subdirectories
                            filesDownloaded += await DownloadLightFitsFiles(fullFtpPath, localDirPath);
                        }
                        else if (name.StartsWith("Light", StringComparison.OrdinalIgnoreCase) &&
                                 name.EndsWith(".fits", StringComparison.OrdinalIgnoreCase))
                        {
                            string newFileName = name.Replace("Light", "dark").Replace("light", "dark");
                            string localFilePath = Path.Combine(localDirPath, newFileName);
                            await DownloadFtpFile(fullFtpPath, localFilePath);
                            filesDownloaded++;
                        }
                    }
                }
            }
            catch (WebException ex)
            {
                throw new Exception($"Failed to process directory {ftpDirUrl}: {ex.Message}", ex);
            }
            return filesDownloaded;
        }

        private async Task DownloadFtpFile(string ftpFileUrl, string localFilePath)
        {
            var request = (FtpWebRequest)WebRequest.Create(ftpFileUrl);
            request.Method = WebRequestMethods.Ftp.DownloadFile;
            request.Credentials = new NetworkCredential("anonymous", "");
            request.UseBinary = true;
            request.KeepAlive = false;

            try
            {
                using (var response = (FtpWebResponse)request.GetResponse())
                using (var stream = response.GetResponseStream())
                using (var fileStream = File.Create(localFilePath))
                {
                    await stream.CopyToAsync(fileStream);
                }
                var logger = new ErrorLogger();
                logger.LogError($"Successfully downloaded and renamed {Path.GetFileName(localFilePath)}");
            }
            catch (WebException ex)
            {
                throw new Exception($"Failed to download file {ftpFileUrl}: {ex.Message}", ex);
            }
        }

        private async void buttonStartImage_Click_1(object sender, EventArgs e)
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

                // Perform calculations and assignments
                CaptureTimeSeconds = CaptureTimeMinutes * 60;

                // Update global data
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                GlobalData.SquenceID++; // Increment sequence ID
                GlobalData.Name = textBoxName.Text.Trim(); // Trim to avoid extra spaces
                GlobalData.TotalDuration = CaptureTimeSeconds;
                GlobalData.SaveRaw = true;
                GlobalData.DisableAutoCancel = true;

                // Send start capture command
                await _telescopeControl.SendStartCaptureCommandAsync();
                MessageBox.Show("Dark capture started successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                var logger = new ErrorLogger();
                logger.LogError($"Successfully Started taking Dark frames {GlobalData.Name}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
