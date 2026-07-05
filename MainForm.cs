using System;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json.Linq;
using System.Security.AccessControl;
using System.Reflection.Emit;
//using TelescopeControl;

namespace OriginLiveView
{
    public partial class MainForm : Form
    {
        private ClientWebSocket ws;
        private CancellationTokenSource cancellationTokenSource;
        private TelescopeControl telescopeControl;
        private const string wsUrl = "ws://origin.local:80/SmartScope-1.0/mountControlEndpoint";
        private readonly string ImageBase = "http://origin.local/SmartScope-1.0/dev2/";
        private string ImageLocation = "";
        private string ImageOldLocation = "";
        private string OriginCommand = "";
        private string ErrorMessage = "";
        private double BatteryPercent = 100.0;
        private double AmbientTermperature_dbl = 0.0;
        private double Humidity_dbl = 0.0;
        private double DewPoint_dbl = 0.0;
        private double CameraTemperature_dbl = 0.0;
        private double CPUTemperature_dbl = 0.0;
        private double FrontCellTemperature_dbl = 0.0;

        public MainForm()
        {
            InitializeComponent();
            pictureBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            this.KeyPreview = true;
        }

        private async void MainForm_Load(object sender, EventArgs e)
        {
            var logger = new CommandLogger();
            string timestamp = DateTime.Now.ToString("MM-dd HH:mm");
            logger.LogCommand($"\n[{timestamp}]: OLV Startup ");
            
            try
            {
                string desktopPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "OLVimages");
                Directory.CreateDirectory(desktopPath); // Ensure the OLVimages folder exists
                string filePath = Path.Combine(desktopPath, "OLVErrorLog.txt");

                if (File.Exists(filePath))
                {
                    File.WriteAllText(filePath, string.Empty); // Clear the file content
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred: {ex.Message}");
            }

            await ConnectToTelescopeAsync();
            
        }

        private async Task ConnectToTelescopeAsync()
        {
            ErrorLogger logger = new ErrorLogger();
            int retryCount = 0;
            const int maxRetries = 3;
            const int timeoutMs = 30000; // 30-second timeout
            const int retryDelayMs = 5000; // 5-second delay between retries

            while (retryCount < maxRetries)
            {
                ws = new ClientWebSocket();
                cancellationTokenSource = new CancellationTokenSource();
                try
                {
                    logger.LogError($"Attempting to connect to {wsUrl} (Attempt {retryCount + 1}/{maxRetries})...");
                    LabelStatus.Text = $"Connecting... (Attempt {retryCount + 1})";
                    LabelStatus.ForeColor = Color.Orange;

                    cancellationTokenSource.CancelAfter(timeoutMs);
                    await ws.ConnectAsync(new Uri(wsUrl), cancellationTokenSource.Token);

                    logger.LogError("Connected successfully!");
                    LabelStatus.Text = "Live View";
                    LabelStatus.ForeColor = Color.Green;

                    telescopeControl = new TelescopeControl(ws, cancellationTokenSource);
                    await Task.WhenAll(ReceiveMessagesAsync(), SendHeartbeatAsync());
                    await telescopeControl.SendGetStatusCommandAsync();
                    return;
                }
                catch (OperationCanceledException)
                {
                    logger.LogError($"Connection timed out after {timeoutMs / 1000} seconds.");
                    retryCount++;
                    LabelStatus.Text = $"Timeout (Attempt {retryCount}/{maxRetries})";
                    await Task.Delay(retryDelayMs);
                }
                catch (WebSocketException ex)
                {
                    logger.LogError($"WebSocket connection failed: {ex.Message}, ErrorCode: {ex.WebSocketErrorCode}");
                    retryCount++;
                    LabelStatus.Text = $"Connection Failed (Attempt {retryCount}/{maxRetries})";
                    await Task.Delay(retryDelayMs);
                }
                catch (Exception ex)
                {
                    logger.LogError($"Unexpected connection error: {ex.Message}, StackTrace: {ex.StackTrace}");
                    retryCount++;
                    LabelStatus.Text = $"Error (Attempt {retryCount}/{maxRetries})";
                    await Task.Delay(retryDelayMs);
                }
                finally
                {
                    if (ws != null && ws.State != WebSocketState.Open)
                    {
                        ws.Dispose();
                        ws = null;
                        cancellationTokenSource.Dispose();
                        cancellationTokenSource = null;
                    }
                }
            }

            logger.LogError("Failed to connect to telescope after maximum retries.");
            LabelStatus.Text = "Connection Failed";
            LabelStatus.ForeColor = Color.Red;
            MessageBox.Show("Unable to connect to telescope after multiple attempts. Please check network and telescope status.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async Task SendHeartbeatAsync()
        {
            ErrorLogger logger = new ErrorLogger();
            while (ws != null && ws.State == WebSocketState.Open)
            {
                try
                {
                    await telescopeControl.SendGetStatusCommandAsync();
                    await Task.Delay(30000); // 30 seconds
                }
                catch (Exception ex)
                {
                    logger.LogError($"Heartbeat failed: {ex.Message}");
                    break;
                }
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[4096];
            var receivedData = new StringBuilder();
            ErrorLogger logger = new ErrorLogger();

            while (ws != null && ws.State == WebSocketState.Open)
            {
                try
                {
                    var result = await ws.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        receivedData.Append(message);
                        if (result.EndOfMessage)
                        {
                            string jsonString = receivedData.ToString();
                            //logger.LogError($"Received JSON: {jsonString}");
                            if (!string.IsNullOrWhiteSpace(jsonString))
                            {
                                JObject jsonObject = JObject.Parse(jsonString);
                                ProcessJsonMessage(jsonObject);
                            }
                            receivedData.Clear();
                        }
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        logger.LogError("WebSocket closed by server.");
                        await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Server closed connection", CancellationToken.None);
                        break;
                    }
                }
                catch (Exception ex)
                {
                    logger.LogError($"Error receiving WebSocket message: {ex.Message}");
                    break;
                }
            }

            if (ws == null || ws.State != WebSocketState.Open)
            {
                logger.LogError("WebSocket disconnected, attempting to reconnect...");
                LabelStatus.Text = "Disconnected";
                LabelStatus.ForeColor = Color.Red;
                await Task.Delay(2000);
                await ReconnectAsync();
            }
        }

        private async Task ReconnectAsync()
        {
            ErrorLogger logger = new ErrorLogger();
            int retryCount = 0;
            const int maxRetries = 5;
            const int delayMs = 5000;

            while (retryCount < maxRetries && (ws == null || ws.State != WebSocketState.Open))
            {
                retryCount++;
                logger.LogError($"Reconnection attempt {retryCount}/{maxRetries}...");
                LabelStatus.Text = $"Reconnecting... (Attempt {retryCount})";
                LabelStatus.ForeColor = Color.Orange;

                try
                {
                    ws = new ClientWebSocket();
                    cancellationTokenSource = new CancellationTokenSource();
                    cancellationTokenSource.CancelAfter(10000);
                    await ws.ConnectAsync(new Uri(wsUrl), cancellationTokenSource.Token);
                    logger.LogError("Reconnected successfully!");
                    LabelStatus.Text = "Live View";
                    LabelStatus.ForeColor = Color.Green;
                    telescopeControl = new TelescopeControl(ws, cancellationTokenSource);
                    await Task.WhenAll(ReceiveMessagesAsync(), SendHeartbeatAsync());
                    await telescopeControl.SendGetStatusCommandAsync();
                    return;
                }
                catch (Exception ex)
                {
                    logger.LogError($"Reconnection attempt {retryCount} failed: {ex.Message}");
                    await Task.Delay(delayMs);
                }
                finally
                {
                    if (ws != null && ws.State != WebSocketState.Open)
                    {
                        ws.Dispose();
                        ws = null;
                        cancellationTokenSource.Dispose();
                        cancellationTokenSource = null;
                    }
                }
            }

            logger.LogError("Failed to reconnect after maximum retries.");
            LabelStatus.Text = "Connection Failed";
            LabelStatus.ForeColor = Color.Red;
            MessageBox.Show("Unable to reconnect to telescope. Please check network and telescope status.", "Connection Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ws != null && ws.State == WebSocketState.Open)
            {
                await ws.CloseAsync(WebSocketCloseStatus.NormalClosure, "Application closing", CancellationToken.None);
                ws.Dispose();
            }
            cancellationTokenSource?.Dispose();
        }

        private void ProcessJsonMessage(JObject jsonObject)
        {
            foreach (JProperty property in jsonObject.Properties())
            {
                if (property.Name == "Command")
                {
                    OriginCommand = property.Value.ToString();
                }

                if (OriginCommand == "Error")
                {
                    if (property.Name == "ErrorMessage")
                    {
                        ErrorMessage = property.Value.ToString();
                        MessageBox.Show($"Telescope Error: {ErrorMessage}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                    continue;
                }

                if (OriginCommand == "NewImageReady")
                {
                    if (property.Name == "FileLocation")
                    {
                        ImageLocation = ImageBase + property.Value.ToString();
                        GlobalData.ImageLocation = ImageLocation;
                        if (ImageOldLocation != ImageLocation)
                        {
                            pictureBox1.ImageLocation = ImageLocation;
                            ImageOldLocation = ImageLocation;
                        }
                        if (GlobalData.BiasCaptureFlag &&  !string.IsNullOrEmpty(GlobalData.ImageLocation) &&  Path.GetExtension(GlobalData.ImageLocation).Equals(".fits", StringComparison.OrdinalIgnoreCase))
                        {
                            // Define folder path: Desktop\OLVimages\biases\{GlobalData.BiasName}
                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string biasFolder = Path.Combine(desktopPath, "OLVimages", "biases", GlobalData.BiasName);

                            // Create folder if it doesn't exist
                            Directory.CreateDirectory(biasFolder);

                            // Create new file name using GlobalData.BiasNameInt (e.g., bias001.fit)
                            string newFileName = Path.Combine(biasFolder, $"bias{GlobalData.BiasNameInt:D3}.fit");

                            // Start download in a background thread
                            Task.Run(async () =>
                            {
                                try
                                {
                                    using (var client = new HttpClient())
                                    {
                                        var response = await client.GetAsync(GlobalData.ImageLocation);
                                        response.EnsureSuccessStatusCode();
                                        var fileBytes = await response.Content.ReadAsByteArrayAsync();
                                        File.WriteAllBytes(newFileName, fileBytes);
                                        //MessageBox.Show("Bias capture started successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                catch (Exception)
                                {
                                    // Silently handle errors (no UI feedback)
                                }
                            });

                            // Existing logic, unchanged (removed duplicate)
                            GlobalData.BiasCaptureFlag = false;
                        }
                        if (GlobalData.FlatCaptureFlag && !string.IsNullOrEmpty(GlobalData.ImageLocation) && Path.GetExtension(GlobalData.ImageLocation).Equals(".fits", StringComparison.OrdinalIgnoreCase))
                        {
                            // Define folder path: Desktop\OLVimages\bias\{GlobalData.BiasName}
                            string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                            string biasFolder = Path.Combine(desktopPath, "OLVimages", "flats", GlobalData.BiasName);

                            // Create folder if it doesn't exist
                            Directory.CreateDirectory(biasFolder);

                            // Create new file name using GlobalData.BiasNameInt (e.g., bias001.fit)
                            string newFileName = Path.Combine(biasFolder, $"flat{GlobalData.BiasNameInt:D3}.fit");

                            // Start download in a background thread
                            Task.Run(async () =>
                            {
                                try
                                {
                                    using (var client = new HttpClient())
                                    {
                                        var response = await client.GetAsync(GlobalData.ImageLocation);
                                        response.EnsureSuccessStatusCode();
                                        var fileBytes = await response.Content.ReadAsByteArrayAsync();
                                        File.WriteAllBytes(newFileName, fileBytes);
                                        //MessageBox.Show("Bias capture started successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                                    }
                                }
                                catch (Exception)
                                {
                                    // Silently handle errors (no UI feedback)
                                }
                            });

                            // Existing logic, unchanged (removed duplicate)
                            GlobalData.BiasCaptureFlag = false;
                            GlobalData.FlatCaptureFlag = false;
                        }
                    }
                }

                if (OriginCommand == "GetStatus")
                {
                    if (property.Name == "Ra")
                    {
                        GlobalData.RA = property.Value.ToObject<double>();
                        GlobalData.RAMicrodegrees = (int)(GlobalData.RA * 683565276.595);
                    }
                    if (property.Name == "Dec")
                    {
                        GlobalData.Dec = property.Value.ToObject<double>();
                        GlobalData.DecMicrodegrees = (int)(GlobalData.Dec * 1367130553.191);
                    }
                    if (property.Name == "ImagingInfo")
                    {
                        JObject imagingInfo = property.Value as JObject;
                        if (imagingInfo != null)
                        {
                            string listStatus = imagingInfo["ListStatus"]?.ToString();
                            int numObjectsRemaining = imagingInfo["NumObjectsRemaining"]?.ToObject<int>() ?? 0;

                            var objectInfoList = imagingInfo["ObjectInfoList"] as JArray;
                            if (objectInfoList != null)
                            {
                                StringBuilder objectSummary = new StringBuilder();
                                foreach (JObject obj in objectInfoList)
                                {
                                    string objectName = obj["ObjectName"]?.ToString();
                                    double remainingTime = obj["RemainingTime"]?.ToObject<double>() ?? 0.0;
                                    string stackingStatus = obj["StackingStatus"]?.ToString();
                                    string MinStartTime = obj["MinimumStartTime"]?.ToString();
                                    string StackDepth = obj["StackDepth"]?.ToString();
                                    string TotalTime = obj["TotalTime"]?.ToString();
                                    objectSummary.AppendLine($"Object: {objectName}, Time: {remainingTime}s, Total Time {TotalTime}  Status: {stackingStatus} Stacking Depth: {StackDepth}  Start time: {MinStartTime}");
                                    
                                    ToolStatusLabel.Text = $"Imaging  {objectSummary}  ";
                                }
                            }
                        }
                    }
                    if (property.Name == "PolarAlignInfo")
                    {
                        JObject polarAlignInfo = property.Value as JObject;
                        if (polarAlignInfo != null)
                        {
                            try
                            {
                                double altitudeError = polarAlignInfo["AltitudeError"]?.ToObject<double>() ?? 0.0;
                                double azimuthError = polarAlignInfo["AzimuthError"]?.ToObject<double>() ?? 0.0;

                                if (Math.Abs(altitudeError) > 5 || Math.Abs(azimuthError) > 5)
                                {
                                    MessageBox.Show("Invalid polar alignment data received.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    return;
                                }

                                GlobalData.AltitudeError = altitudeError;
                                GlobalData.AzimuthError = azimuthError;

                                LabelAltAzError.Text = $"Polar Alignment: Alt Error = {GlobalData.AltitudeError:F3}°, Az Error = {GlobalData.AzimuthError:F3}°";
                            }
                            catch (Exception ex)
                            {
                                ErrorLogger logger = new ErrorLogger();
                                logger.LogError($"Error processing PolarAlignInfo: {ex.Message}");
                                MessageBox.Show($"Error processing polar alignment data: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            }
                        }
                    }
                    if (property.Name == "ErrorMessage")
                    {
                        MessageBox.Show($"Error Message: {property.Value}", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    if (property.Name == "AmbientTemperature")
                    {
                        AmbientTermperature_dbl = property.Value.ToObject<double>();
                        GlobalData.AmbientTemperature = (int)Math.Round(AmbientTermperature_dbl);
                        labelAmbientTemperature.Text = GlobalData.AmbientTemperature + "°C";
                    }
                    if (property.Name == "Humidity")
                    {
                        Humidity_dbl = property.Value.ToObject<double>();
                        GlobalData.Humidity = (int)Math.Round(Humidity_dbl);
                        labelHumidity.Text = GlobalData.Humidity + "%";
                    }
                    if (property.Name == "DewPoint")
                    {
                        DewPoint_dbl = property.Value.ToObject<double>();
                        GlobalData.DewPointTemperature = (int)Math.Round(DewPoint_dbl);
                        labelDewPoint.Text = GlobalData.DewPointTemperature + "°C";
                    }
                    if (property.Name == "Position")
                    {
                        GlobalData.focuserposition = property.Value.ToObject<int>();
                        labelFocuserPosition.Text = GlobalData.focuserposition.ToString();
                    }
                    if (property.Name == "CameraTemperature")
                    {
                        CameraTemperature_dbl = property.Value.ToObject<double>();
                        GlobalData.CameraTemperature = (int)Math.Round(CameraTemperature_dbl);
                        labelCameraTemp.Text = GlobalData.CameraTemperature.ToString() + "°C";
                    }
                    if (property.Name == "CpuTemperature")
                    {
                        CPUTemperature_dbl = property.Value.ToObject<double>();
                        GlobalData.CPUTemperature = (int)Math.Round(CPUTemperature_dbl);
                        labelCPUTemp.Text = GlobalData.CPUTemperature.ToString() + "°C";
                    }
                    if (property.Name == "Binning")
                    {
                        GlobalData.CameraBin = property.Value.ToObject<int>();
                    }
                    if (property.Name == "BitDepth")
                    {
                        GlobalData.CameraBitDepth = property.Value.ToObject<int>();
                    }
                    if (property.Name == "ColorBBalance")
                    {
                        GlobalData.CameraBlueGain = property.Value.ToObject<int>();
                    }
                    if (property.Name == "ColorGBalance")
                    {
                        GlobalData.CameraGreenGain = property.Value.ToObject<int>();
                    }
                    if (property.Name == "ColorRBalance")
                    {
                        GlobalData.CameraRedGain = property.Value.ToObject<int>();
                    }
                    if (property.Name == "FrontCellTemperature")
                    {
                        FrontCellTemperature_dbl = property.Value.ToObject<double>();
                        GlobalData.FrontCellTemperature = (int)Math.Round(FrontCellTemperature_dbl);
                        labelFrontCell.Text = GlobalData.FrontCellTemperature.ToString() + "°C";
                    }
                    if (property.Name == "CpuFanOn")
                    {
                        label1CPUFan.Text = property.Value.ToString();
                    }
                    if (property.Name == "OtaFanOn")
                    {
                        label1OTAFan.Text = property.Value.ToString();
                    }
                    if (property.Name == "Stage")
                    {
                        GlobalData.Stage = property.Value.ToString();
                        LabelStage.Text = GlobalData.Stage;
                    }
                    if (property.Name == "State")
                    {
                        GlobalData.State = property.Value.ToString();
                        if (GlobalData.State == "IDLE")
                        {
                            LabelState.Text = "";
                            LabelStage.Text = "";
                            ToolStatusLabel.Text = "";
                        }
                        else
                        {
                            LabelState.Text = GlobalData.State;
                        }
                    }
                    if (property.Name == "FreeBytes")
                    {
                        GlobalData.FreeBytes = property.Value.ToObject<long>();
                        double freeGigabytes = GlobalData.FreeBytes / 1_000_000_000.0; // Convert to decimal GB
                        LabelDisk.Text = freeGigabytes.ToString("F2") + " GB"; // Set text with 2 decimal places

                        // Set color based on freeGigabytes
                        if (freeGigabytes > 30)
                        {
                            LabelDisk.ForeColor = Color.Green; // Over 30 GB
                        }
                        else if (freeGigabytes >= 10 && freeGigabytes <= 30)
                        {
                            LabelDisk.ForeColor = Color.Yellow; // 10-30 GB (inclusive)
                        }
                        else // freeGigabytes < 10
                        {
                            LabelDisk.ForeColor = Color.Red; // Under 10 GB
                        }                                                           
                    }
                    if (property.Name == "Mode")
                    {
                        label1Heater.Text = property.Value.ToString();
                    }
                    if (property.Name == "BatteryVoltage")
                    {
                        GlobalData.BatteryVoltage = property.Value.ToObject<double>();
                        if (GlobalData.BatteryVoltage > 10.2)
                        {
                            BatteryPercent = 100;
                            labelBattery.Text = BatteryPercent + "%";
                        }
                        else if (GlobalData.BatteryVoltage >= 9.20 && GlobalData.BatteryVoltage <= 10.20)
                        {
                            BatteryPercent = (int)Math.Round(((GlobalData.BatteryVoltage - 9.20) / (10.20 - 9.20)) * 100);
                            labelBattery.Text = BatteryPercent + "%";
                        }
                        else
                        {
                            BatteryPercent = 0;
                            labelBattery.Text = BatteryPercent + "%";
                        }
                    }
                }
                if (OriginCommand == "GetFilter")
                {
                    if (property.Name == "Filter")
                    {
                        labelFilter.Text = property.Value.ToString();
                    }
                }

                if (OriginCommand == "GetCaptureParameters")
                {
                    if (property.Name == "ISO")
                    {
                        GlobalData.ISO = property.Value.ToObject<int>();
                    }
                    if (property.Name == "Exposure")
                    {
                        GlobalData.CameraExposure = property.Value.ToObject<double>();
                    }
                }
            }
        }

        private void stellariumToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Stellarium stellariumForm = new Stellarium(telescopeControl);
            stellariumForm.Show();
        }

        private void saveImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(ImageLocation))
                {
                    MessageBox.Show("No image URL provided.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                using (SaveFileDialog saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Filter = "JPEG Image|*.jpg|All Files|*.*";
                    saveFileDialog.Title = "Save Live View Image";
                    saveFileDialog.DefaultExt = "jpg";
                    saveFileDialog.AddExtension = true;
                    string liveViewPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop), "OLVimages", "LiveView");
                    Directory.CreateDirectory(liveViewPath); // Ensure the OLVImages\LiveView folder exists
                    saveFileDialog.InitialDirectory = liveViewPath;
                    saveFileDialog.FileName = $"OriginLiveView_{DateTime.Now:yyyyMMdd_HHmmss}.jpg";

                    if (saveFileDialog.ShowDialog() != DialogResult.OK)
                    {
                        return;
                    }

                    string savePath = saveFileDialog.FileName;

                    using (WebClient client = new WebClient())
                    {
                        client.DownloadFile(ImageLocation, savePath);
                        MessageBox.Show($"Image saved successfully to: {savePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (WebException webEx)
            {
                ErrorLogger logger = new ErrorLogger();
                logger.LogError($"Network error downloading image: {webEx.Message}");
                MessageBox.Show("Failed to download image due to a network issue.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                ErrorLogger logger = new ErrorLogger();
                logger.LogError($"Error downloading image: {ex.Message}");
                MessageBox.Show("An error occurred while saving the image.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
        }

        private void takeImageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CaptureImage captureImage = new CaptureImage(telescopeControl);
            captureImage.Show();
        }

        private void initializeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Initialize Initialize = new Initialize(telescopeControl);
            Initialize.Show();
            

        }

        private void aboutToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            AboutBox1 aboutBox = new AboutBox1();
            aboutBox.ShowDialog();
        }

        private void nightModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //this.BackColor = Color.FromArgb(139, 0, 0); // Dark red for entire form
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private async void rebootToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await telescopeControl.SendRebootCommandAsync();
            MessageBox.Show("Rebooting Origin!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        public static void OpenFtpInExplorer(string ftpAddress)
        {
            try
            {
                Process.Start("explorer.exe", ftpAddress);
            }
            catch (System.ComponentModel.Win32Exception e)
            {
                Console.WriteLine($"Error starting explorer: {e.Message}");
            }
        }

        private void switchBootPartitionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //await telescopeControl.SendSwitchBootPartitionCommandAsync();
            MessageBox.Show("currently Disabled", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void downloadImagesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFtpInExplorer("ftp://origin.local/Astrophotography");
        }


        private async void powerDownOriginToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await telescopeControl.SendPowerDownCommandAsync();
            MessageBox.Show("Powering Down Origin!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void scheduleImaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ScheduleImage scheduleImage = new ScheduleImage(telescopeControl);
            scheduleImage.Show();
            //MessageBox.Show("Under Development!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void originToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }


        private async void generateLogszipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await telescopeControl.SendGenerateLogsCommandAsync();
            MessageBox.Show("Origin Logs have been requested", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void downlodLogszipToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    string url = "http://origin.local/dev/Logs/logs.zip";
                    string desktopPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
                    string olvImagesPath = Path.Combine(desktopPath, "OLVimages");

                    // Create OLVimages folder if it doesn't exist
                    if (!Directory.Exists(olvImagesPath))
                    {
                        Directory.CreateDirectory(olvImagesPath);
                    }

                    string savePath = Path.Combine(olvImagesPath, "logs.zip");

                    byte[] fileBytes = await client.GetByteArrayAsync(url);
                    File.WriteAllBytes(savePath, fileBytes);
                    MessageBox.Show($"Logs downloaded successfully to {savePath}", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Failed to download logs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (IOException ex)
            {
                MessageBox.Show($"Failed to save logs: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveFileDialog3_FileOk(object sender, System.ComponentModel.CancelEventArgs e)
        {
        }
        
        private void caputreParametersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CameraParameters cameraParameters = new CameraParameters(telescopeControl);
            cameraParameters.Show();
        }

        private void instructionsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("notepad", $"\"{Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "help.txt")}\"");
        }

        private async void haltTasksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await telescopeControl.SendStopCaptureCommandAsync();
            ToolStatusLabel.Text = "";
            LabelAltAzError.Text = "";
            var logger = new CommandLogger();
            logger.LogCommand("Halt");
        }

        private async void restoreFactoryDarksToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            await telescopeControl.SendRestoreFactoryDarksCommandAsync();
            MessageBox.Show("Restoring Factory Darks!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void restoreFactoryFlatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await telescopeControl.SendRestoreFactoryFlatsCommandAsync();
            MessageBox.Show("Restoring Factory Flats!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private async void retakeFllatsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await telescopeControl.SendRetakeDarksCommandAsync();
        }

        private void retakeDarksToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RetakeDarks retakeDarks = new RetakeDarks(telescopeControl);
            retakeDarks.Show();
        }

        private void oLVDitheringToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OLVDithering olvDithering = new OLVDithering(telescopeControl);
            olvDithering.Show();
        }

        private void oLVMosaicModeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //OLVMosaic olvMosaicMode = new OLVMosaic(telescopeControl);
            //olvMosaicMode.Show();

        }

        private async void polarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Polar Alignment waiting on offical release from Celestron.!", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            await telescopeControl.SendPolarAlignCommandAsync();
        }

        private void dewHeaterToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DewHeater dewHeater = new DewHeater(telescopeControl);
            dewHeater.Show();
        }

        private void autoFocuToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AutoFocus autoFocus = new AutoFocus(telescopeControl);
            autoFocus.Show();
        }

        private void focusingToolToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Focuser focuser = new Focuser(telescopeControl);
            focuser.Show();
        }

        private void fansToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Fans fans = new Fans(telescopeControl);
            fans.Show();
        }

        private async void clearFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await telescopeControl.SendFilterClearCommandAsync();
            await Task.Delay(100);
            await telescopeControl.SendGetFilterCommandAsync();
            var logger = new CommandLogger();
            logger.LogCommand($"Filter Clear");
        }

        private async void nebulaFilterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await telescopeControl.SendFilterNebulaCommandAsync();
            await Task.Delay(100);
            await telescopeControl.SendGetFilterCommandAsync();
            var logger = new CommandLogger();
            logger.LogCommand($"Filter Nebula");
        }

        private void settingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MountSettings mountSettings = new MountSettings(telescopeControl);
            mountSettings.Show();
        }

        private void gotoRaDecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GotoRaDec gotoRaDec = new GotoRaDec(telescopeControl);
            gotoRaDec.Show();
        }

        private void slewTelescopeToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            SlewTelescope focus = new SlewTelescope(telescopeControl);
            focus.Show();
        }

        private void captureMosaicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OLVMosaic olvMosaicMode = new OLVMosaic(telescopeControl);
            olvMosaicMode.Show();
        }

        private void composeMosaicToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OLVComposeMosaic olvcomposemosaic = new OLVComposeMosaic();
            olvcomposemosaic.Show();
        }

        private void oLVTakeDownloadDarksToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OLVTakeDark oLVTakeDark = new OLVTakeDark(telescopeControl);
            oLVTakeDark.Show();
        }

        private void oLVTakeBiasFramesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //MessageBox.Show("Under Construction! ", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Information);
            OLVTakeBias oLVTakeBias = new OLVTakeBias(telescopeControl);
            oLVTakeBias.Show();
        }


        private void eLPanelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ELPanel elPanel = new ELPanel(telescopeControl);
            elPanel.Show();
        }

        private void oLVTakeFlatFramesToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            OLVTakeFlat oLVTakeFlat = new OLVTakeFlat(telescopeControl);
            oLVTakeFlat.Show();
        }

        private void getRaDecToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GetRaDec getRaDec = new GetRaDec(); 
            getRaDec.Show();
        }

        private async void addReferenceToModelToolStripMenuItem_Click(object sender, EventArgs e)
        {
            await telescopeControl.SendRUnAddReferenceCommandAsync();
            var logger = new CommandLogger();
            logger.LogCommand("RunAddRefernce");
        }

        private void oLVScriptingToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Editor editor = new Editor(telescopeControl);
            editor.Show();
        }
    }
}