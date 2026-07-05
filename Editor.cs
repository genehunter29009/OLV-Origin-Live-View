using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Speech.Synthesis; // Added for SPEAK command

namespace OriginLiveView
{
    public partial class Editor : Form
    {
        private readonly TelescopeControl _telescopeControl;
        private string currentFilePath = null;
        private CancellationTokenSource _scriptCancellationTokenSource;
        private readonly SpeechSynthesizer _speechSynthesizer; // Added for SPEAK command

        public Editor(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
            _speechSynthesizer = new SpeechSynthesizer(); // Added for SPEAK command
        }

        private void Editor_Load(object sender, EventArgs e)
        {
            txtEditor.Text = "";
            stopScriptToolStripMenuItem.Enabled = false; // Disable stop button initially
            System.Diagnostics.Debug.WriteLine($"txtEditor: Enabled={txtEditor.Enabled}, ReadOnly={txtEditor.ReadOnly}, Multiline={txtEditor.Multiline}");
        }

        private void openToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentFilePath = openFileDialog1.FileName;
                    string[] lines = File.ReadAllLines(currentFilePath, Encoding.Default);
                    txtEditor.Text = string.Join("\r\n", lines);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void saveToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                if (saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    currentFilePath = saveFileDialog1.FileName;
                }
                else
                {
                    return;
                }
            }
            try
            {
                File.WriteAllText(currentFilePath, txtEditor.Text, Encoding.Default);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void saveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    currentFilePath = saveFileDialog1.FileName;
                    File.WriteAllText(currentFilePath, txtEditor.Text, Encoding.UTF8);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error saving file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void newToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
            if (txtEditor.TextLength > 0)
            {
                var result = MessageBox.Show("Save changes before creating a new file?", "Confirm", MessageBoxButtons.YesNoCancel);
                if (result == DialogResult.Yes)
                {
                    saveToolStripMenuItem_Click_1(sender, e);
                }
                else if (result == DialogResult.Cancel)
                {
                    return;
                }
            }
            txtEditor.Clear();
            currentFilePath = null;
        }

        public class ScriptLine
        {
            public string Command { get; set; }
            public List<string> Parameters { get; set; } = new List<string>();
            public bool IsValid { get; set; }
            public string Error { get; set; }
        }

        private ScriptLine ParseScriptLine(string line)
        {
            var parsed = new ScriptLine
            {
                IsValid = false,
                Command = "",
                Parameters = new List<string>()
            };

            try
            {
                string trimmedLine = line.Trim();
                if (string.IsNullOrEmpty(trimmedLine))
                    return parsed;

                string[] parts = trimmedLine.Split(new char[] { ' ', '\t' },
                    StringSplitOptions.RemoveEmptyEntries);

                if (parts.Length == 0)
                    return parsed;

                parsed.Command = parts[0].ToUpper();
                for (int i = 1; i < parts.Length; i++)
                {
                    parsed.Parameters.Add(parts[i]);
                }

                parsed.IsValid = true;
                parsed.Error = "";
            }
            catch (Exception ex)
            {
                parsed.Error = $"Parse error: {ex.Message}";
            }

            return parsed;
        }

        private async void runScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtEditor.Text))
            {
                MessageBox.Show("Please enter a script to run.", "No Script",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var result = MessageBox.Show("Execute this script?", "Confirm Execution",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            _scriptCancellationTokenSource = new CancellationTokenSource();
            CancellationToken token = _scriptCancellationTokenSource.Token;

            runScriptToolStripMenuItem.Enabled = false;
            stopScriptToolStripMenuItem.Enabled = true;
            txtEditor.ReadOnly = true;

            try
            {
                string[] lines = txtEditor.Lines;
                int lineNumber = 0;
                int processedLines = 0;

                foreach (string line in lines)
                {
                    lineNumber++;

                    if (token.IsCancellationRequested)
                    {
                        ShowStatus("Script execution cancelled.");
                        MessageBox.Show("Script execution was cancelled.", "Cancelled",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }

                    if (string.IsNullOrWhiteSpace(line) ||
                        line.TrimStart().StartsWith("#") ||
                        line.TrimStart().StartsWith("//"))
                    {
                        ShowStatus($"Line {lineNumber}: comment");
                        System.Diagnostics.Debug.WriteLine($"Line {lineNumber}: Skipped (empty or comment)");
                        continue;
                    }

                    ScriptLine parsedLine = ParseScriptLine(line);

                    if (parsedLine.IsValid)
                    {
                        processedLines++;
                        bool success = await ExecuteCommand(parsedLine);

                        if (success)
                        {
                            ShowStatus($"Line {lineNumber}: {parsedLine.Command} executed");
                            System.Diagnostics.Debug.WriteLine($"SUCCESS: {parsedLine.Command} - Params: [{string.Join(", ", parsedLine.Parameters)}]");
                        }
                        else
                        {
                            ShowStatus($"Line {lineNumber}: {parsedLine.Command} failed");
                            System.Diagnostics.Debug.WriteLine($"FAILED: {parsedLine.Command} - Params: [{string.Join(", ", parsedLine.Parameters)}]");
                        }
                    }
                    else
                    {
                        ShowStatus($"Line {lineNumber}: Parse error - {parsedLine.Error}");
                        System.Diagnostics.Debug.WriteLine($"PARSE ERROR: Line {lineNumber} - {parsedLine.Error}");
                    }

                    await Task.Delay(1000, token);
                }

                if (!token.IsCancellationRequested)
                {
                    MessageBox.Show($"Script processing completed. Processed {processedLines} commands out of {lineNumber} lines.",
                        "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (OperationCanceledException)
            {
                ShowStatus("Script execution cancelled.");
                MessageBox.Show("Script execution was cancelled.", "Cancelled",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                ShowStatus($"Error during script processing: {ex.Message}");
                MessageBox.Show($"Error during script processing:\n{ex.Message}",
                    "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                runScriptToolStripMenuItem.Enabled = true;
                stopScriptToolStripMenuItem.Enabled = false;
                txtEditor.ReadOnly = false;
                txtEditor.Focus();
                ShowStatus("Ready");
                System.Diagnostics.Debug.WriteLine("Finally block executed, ReadOnly set to false");
                _scriptCancellationTokenSource?.Dispose();
                _scriptCancellationTokenSource = null;
            }
        }

        private async void stopScriptToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (_scriptCancellationTokenSource != null)
            {
                _scriptCancellationTokenSource.Cancel();
                ShowStatus("Cancellation requested...");
                System.Diagnostics.Debug.WriteLine("Stop script requested");

                // Call ExecuteHaltCommand with empty parameters
                bool haltSuccess = await ExecuteHaltCommand(new List<string>());
                if (haltSuccess)
                {
                    ShowStatus("HALT: Successfully executed during script cancellation");
                    System.Diagnostics.Debug.WriteLine("HALT: Successfully executed during script cancellation");
                }
                else
                {
                    ShowStatus("HALT: Failed during script cancellation");
                    System.Diagnostics.Debug.WriteLine("HALT: Failed during script cancellation");
                }
            }
        }

        private async Task<bool> ExecuteCommand(ScriptLine command)
        {
            try
            {
                switch (command.Command.ToUpper())
                {
                    case "FILTER":
                        return await ExecuteFilterCommand(command.Parameters);
                    case "HALT":
                        return await ExecuteHaltCommand(command.Parameters);
                    case "GOTO":
                        return await ExecuteGotoCommand(command.Parameters);
                    case "WAIT":
                        return await ExecuteWaitCommand(command.Parameters);
                    case "RUNADDREFERENCE":
                        return await ExecuteRunAddReferenceCommand(command.Parameters);
                    case "STARTCAPTURE":
                        return await ExecuteStartCaptureCommand(command.Parameters);
                    case "WAITUNTIL":
                        return await ExecuteWaitUntilCommand(command.Parameters);
                    case "WAITUNTILTEMP":
                        return await ExecuteWaitUntilTempCommand(command.Parameters);
                    case "SPEAK": 
                        return await ExecuteSpeakCommand(command.Parameters);
                    case "POWERDOWN": 
                        return await ExecutePowerDownCommand(command.Parameters);
                    case "MSGBOX":
                        return await ExecuteMsgBoxCommand(command.Parameters);
                    case "RETAKEDARK": 
                        return await ExecuteReTakeDarkCommand(command.Parameters);
                    case "AUTOFOCUS": 
                        return await ExecuteAutoFocusCommand(command.Parameters);
                    default:
                        ShowStatus($"Unknown command: {command.Command}");
                        System.Diagnostics.Debug.WriteLine($"UNKNOWN COMMAND: {command.Command}");
                        return false;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Command execution error: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ExecuteSpeakCommand(List<string> parameters) // Added method for SPEAK command
        {
            try
            {
                // Check if there are parameters
                if (parameters.Count == 0)
                {
                    ShowStatus("SPEAK: No text provided");
                    System.Diagnostics.Debug.WriteLine("SPEAK: No parameters provided");
                    return false;
                }

                // Join parameters into a single string
                string textToSpeak = string.Join(" ", parameters);
                if (string.IsNullOrWhiteSpace(textToSpeak))
                {
                    ShowStatus("SPEAK: Text to speak cannot be empty");
                    System.Diagnostics.Debug.WriteLine("SPEAK: Empty text provided");
                    return false;
                }

                // Log the command
                var logger = new CommandLogger();
                logger.LogCommand($"SPEAK {textToSpeak}");
                ShowStatus($"SPEAK: Announcing '{textToSpeak}'");
                System.Diagnostics.Debug.WriteLine($"SPEAK: Announcing '{textToSpeak}'");

                // Speak the text asynchronously
                await Task.Run(() => _speechSynthesizer.SpeakAsync(textToSpeak), _scriptCancellationTokenSource?.Token ?? CancellationToken.None);

                ShowStatus($"SPEAK: Finished '{textToSpeak}'");
                System.Diagnostics.Debug.WriteLine($"SPEAK: Finished '{textToSpeak}'");
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("SPEAK: Cancelled");
                System.Diagnostics.Debug.WriteLine("SPEAK command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"SPEAK: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"SPEAK: Error - {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ExecuteStartCaptureCommand(List<string> parameters)
        {
            try
            {
                if (parameters.Count != 6)
                {
                    ShowStatus($"STARTCAPTURE: Expected 5 parameters, got {parameters.Count}");
                    System.Diagnostics.Debug.WriteLine($"STARTCAPTURE: Invalid parameter count, got {parameters.Count}, expected 5");
                    return false;
                }

                string name = parameters[0];
                if (string.IsNullOrWhiteSpace(name))
                {
                    ShowStatus("STARTCAPTURE: Name parameter cannot be empty");
                    return false;
                }

                if (!int.TryParse(parameters[1], out int exposureTime) || exposureTime <= 0)
                {
                    ShowStatus($"STARTCAPTURE: Invalid ExposureTime '{parameters[1]}', must be a positive integer");
                    return false;
                }

                if (!int.TryParse(parameters[2], out int totalDuration) || totalDuration <= 0)
                {
                    ShowStatus($"STARTCAPTURE: Invalid TotalDuration '{parameters[2]}', must be a positive integer");
                    return false;
                }

                if (!int.TryParse(parameters[3], out int iso) || iso <= 0)
                {
                    ShowStatus($"STARTCAPTURE: Invalid ISO '{parameters[3]}', must be a positive integer");
                    return false;
                }

                if (!bool.TryParse(parameters[4], out bool saveRaw))
                {
                    ShowStatus($"STARTCAPTURE: Invalid SaveRaw '{parameters[4]}', must be 'true' or 'false'");
                    return false;
                }

                if (!bool.TryParse(parameters[5], out bool DisableAutoCancel))
                {
                    ShowStatus($"STARTCAPTURE: Invalid DisabelAutoCancel '{parameters[5]}', must be 'true' or 'false'");
                    return false;
                }

                GlobalData.Name = name;
                GlobalData.ExposureTime = exposureTime;
                GlobalData.TotalDuration = totalDuration;
                GlobalData.ISO = iso;
                GlobalData.SaveRaw = saveRaw;
                GlobalData.SquenceID++; // Note: Typo in "SquenceID" (should be "SequenceID")
                GlobalData.DisableAutoCancel = DisableAutoCancel;

                string logMessage = $"STARTCAPTURE: Name={name}, ExposureTime={exposureTime}s, TotalDuration={totalDuration}s, ISO={iso}, SaveRaw={saveRaw}, SequenceID={GlobalData.SquenceID}";
                ShowStatus(logMessage);
                System.Diagnostics.Debug.WriteLine(logMessage);

                var logger = new CommandLogger();
                logger.LogCommand(logMessage);
                // Start the capture
                await _telescopeControl.SendStartCaptureCommandAsync();
                // 
                await Task.Delay(15000, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);
                while (GlobalData.State != "IDLE")
                {
                    await Task.Delay(5000); // Wait 5Seconds before checking again
                }
                logMessage = $"Image Capture Completed: Name={name}";
                logger.LogCommand(logMessage);
                ShowStatus(logMessage);
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("STARTCAPTURE: Cancelled");
                System.Diagnostics.Debug.WriteLine("STARTCAPTURE command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"STARTCAPTURE: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"STARTCAPTURE: Error - {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ExecuteFilterCommand(List<string> parameters)
        {
            try
            {
                if (parameters.Count != 1)
                {
                    ShowStatus($"FILTER: Expected 1 parameter (filter name), got {parameters.Count}");
                    return false;
                }

                string filterName = parameters[0].ToLower();
                if (filterName == "clear")
                {
                    await _telescopeControl.SendFilterClearCommandAsync();
                    var logger = new CommandLogger();
                    logger.LogCommand("Filter clear");
                    await _telescopeControl.SendGetFilterCommandAsync();
                }
                else if (filterName == "nebula")
                {
                    await _telescopeControl.SendFilterNebulaCommandAsync();
                    var logger = new CommandLogger();
                    logger.LogCommand("Filter nebula");
                    await _telescopeControl.SendGetFilterCommandAsync();
                }
                else
                {
                    ShowStatus($"FILTER: Invalid filter name '{filterName}', expected 'clear' or 'nebula'");
                    return false;
                }

                await Task.Delay(500, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("FILTER: Cancelled");
                System.Diagnostics.Debug.WriteLine("FILTER command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"FILTER: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Filter command error: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ExecuteHaltCommand(List<string> parameters)
        {
            try
            {
                if (parameters.Count > 0)
                {
                    ShowStatus($"HALT: Unexpected parameters ignored");
                }

                await _telescopeControl.SendStopCaptureCommandAsync();
                var logger = new CommandLogger();
                logger.LogCommand("Halt");
                await Task.Delay(100, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("HALT: Cancelled");
                System.Diagnostics.Debug.WriteLine("HALT command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"HALT: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Halt command error: {ex.Message}");
                return false;
            }
        }
        private async Task<bool> ExecutePowerDownCommand(List<string> parameters)
        {
            try
            {
                if (parameters.Count > 0)
                {
                    ShowStatus($"PowerDown: Unexpected parameters ignored");
                }

                await _telescopeControl.SendPowerDownCommandAsync();
                var logger = new CommandLogger();
                logger.LogCommand("PowerDown");
                await Task.Delay(100, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("PowerDown: Cancelled");
                System.Diagnostics.Debug.WriteLine("PowerDown command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"PowerDown: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Halt command error: {ex.Message}");
                return false;
            }
        }
        private async Task<bool> ExecuteReTakeDarkCommand(List<string> parameters)
        {
            try
            {
                if (parameters.Count != 2)
                {
                    ShowStatus($"RETAKEDARKS: Expected 2 parameters, got {parameters.Count}");
                    System.Diagnostics.Debug.WriteLine($"RETAKEDARKS: Invalid parameter count, got {parameters.Count}, expected 2");
                    return false;
                }
                                
                if (!int.TryParse(parameters[0], out int exposureTime) || exposureTime <= 0)
                {
                    ShowStatus($"RETAKEDARKS: Invalid ExposureTime '{parameters[1]}', must be a positive integer");
                    return false;
                }
                
                if (!int.TryParse(parameters[1], out int iso) || iso <= 0)
                {
                    ShowStatus($"RETAKEDARKS: Invalid ISO '{parameters[3]}', must be a positive integer");
                    return false;
                }

                GlobalData.CameraBitDepth = 16; // Dark frames are always 16 bit
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;
                
                string logMessage = $"RETAKEDARKS:  ExposureTime={exposureTime}s,  ISO={iso}";
                ShowStatus(logMessage);
                System.Diagnostics.Debug.WriteLine(logMessage);

                var logger = new CommandLogger();
                logger.LogCommand(logMessage);
                // Start the capture
                await _telescopeControl.SendRetakeDarksCommandAsync();
                // 
                await Task.Delay(15000, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);
                while (GlobalData.State != "IDLE")
                {
                    await Task.Delay(5000); // Wait 5Seconds before checking again
                }
                logMessage = $"RetakeDarks Completed: ";
                logger.LogCommand(logMessage);
                ShowStatus(logMessage);
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("RETAKEDARKS: Cancelled");
                System.Diagnostics.Debug.WriteLine("STARTCAPTURE command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"RETAKEDARKS: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"STARTCAPTURE: Error - {ex.Message}");
                return false;
            }
        }
        private async Task<bool> ExecuteAutoFocusCommand(List<string> parameters)
        {
            try
            {
                if (parameters.Count > 0)
                {
                    ShowStatus($"AutoFocus: Unexpected parameters ignored");
                }

                await _telescopeControl.SendAutoFocusCommandAsync();
                var logger = new CommandLogger();
                logger.LogCommand("AutoFocus");
                await Task.Delay(100, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("AutoFocus: Cancelled");
                System.Diagnostics.Debug.WriteLine("AutoFocus command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"AutoFocus: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"Halt command error: {ex.Message}");
                return false;
            }
        }
        private async Task<bool> ExecuteGotoCommand(List<string> parameters)
        {
            try
            {
                if (parameters == null || parameters.Count != 2)
                {
                    ShowStatus($"GOTO: Expected exactly 2 parameters (RA and Dec in radians), got {parameters?.Count ?? 0}");
                    return false;
                }

                if (!double.TryParse(parameters[0], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double ra) ||
                    !double.TryParse(parameters[1], System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out double dec))
                {
                    ShowStatus($"GOTO: Invalid RA '{parameters[0]}' or Dec '{parameters[1]}', must be valid numbers");
                    return false;
                }

                GlobalData.GotoRA = ra;
                GlobalData.GotoDec = dec;

                await _telescopeControl.SendSlewToCommandAsync();
                var logger = new CommandLogger();
                logger.LogCommand($"GOTO Started {GlobalData.GotoRA} {GlobalData.GotoDec}");
                ShowStatus($"GOTO Started {GlobalData.GotoRA} {GlobalData.GotoDec}");
                await Task.Delay(1000, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);
                while (GlobalData.State != "IDLE")
                {
                    await Task.Delay(5000); // Wait 5Seconds before checking again
                }

                logger.LogCommand($"GOTO Completed {GlobalData.GotoRA} {GlobalData.GotoDec}");
                ShowStatus($"GOTO Completed {GlobalData.GotoRA} {GlobalData.GotoDec}");
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("GOTO: Cancelled");
                System.Diagnostics.Debug.WriteLine("GOTO command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"GOTO: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"GOTO error: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ExecuteRunAddReferenceCommand(List<string> parameters)
        {
            try
            {
                if (parameters.Count > 0)
                {
                    ShowStatus($"RUNADDREFERENCE: Unexpected parameters ignored");
                }

                await _telescopeControl.SendRUnAddReferenceCommandAsync(); // Fixed method name
                var logger = new CommandLogger();
                logger.LogCommand("RunAddReference");
                await Task.Delay(100, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("RUNADDREFERENCE: Cancelled");
                System.Diagnostics.Debug.WriteLine("RUNADDREFERENCE command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"RUNADDREFERENCE: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"RunAddReference command error: {ex.Message}");
                return false;
            }
        }

        private async Task<bool> ExecuteWaitCommand(List<string> parameters)
        {
            try
            {
                if (parameters.Count != 1)
                {
                    ShowStatus($"WAIT: Expected exactly 1 parameter (WaitTime), got {parameters.Count}");
                    return false;
                }

                if (!int.TryParse(parameters[0], System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int waitTime) || waitTime <= 0)
                {
                    ShowStatus($"WAIT: Invalid WaitTime '{parameters[0]}', must be a positive integer");
                    return false;
                }
                int WaitTime = (waitTime * 1000);
                var logger = new CommandLogger();
                logger.LogCommand($"WAIT {waitTime}s");

                ShowStatus($"WAIT: Pausing for {waitTime}s");
                await Task.Delay(WaitTime, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);

                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("WAIT: Cancelled");
                System.Diagnostics.Debug.WriteLine("WAIT command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"WAIT: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"WAIT command error: {ex.Message}");
                return false;
            }
        }
        private async Task<bool> ExecuteWaitUntilCommand(List<string> parameters)
        {
            try
            {
                ShowStatus("WAIT_UNTIL: Starting execution");
                System.Diagnostics.Debug.WriteLine("WAIT_UNTIL: Starting execution");

                if (parameters.Count != 1)
                {
                    ShowStatus($"WAIT_UNTIL: Expected exactly 1 parameter (TargetTime in HHmm format), got {parameters.Count}");
                    System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL: Parameter count error, got {parameters.Count}");
                    return false;
                }

                string targetTimeStr = parameters[0];
                //ShowStatus($"WAIT_UNTIL: Input time string is '{targetTimeStr}'");
                //System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL: Input time string is '{targetTimeStr}'");

                if (!System.Text.RegularExpressions.Regex.IsMatch(targetTimeStr, @"^([01]\d|2[0-3])[0-5]\d$"))
                {
                    ShowStatus($"WAIT_UNTIL: Invalid TargetTime '{targetTimeStr}', must be in HHmm format (e.g., 1430 for 2:30 PM)");
                    System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL: Invalid format for '{targetTimeStr}'");
                    return false;
                }

                if (!int.TryParse(targetTimeStr.Substring(0, 2), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int hours) ||
                    !int.TryParse(targetTimeStr.Substring(2, 2), System.Globalization.NumberStyles.Integer, System.Globalization.CultureInfo.InvariantCulture, out int minutes))
                {
                    ShowStatus($"WAIT_UNTIL: Failed to parse TargetTime '{targetTimeStr}'");
                    System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL: Parse failure for '{targetTimeStr}', hours: {hours}, minutes: {minutes}");
                    return false;
                }

                //ShowStatus($"WAIT_UNTIL: Parsed hours: {hours}, minutes: {minutes}");
                //System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL: Parsed hours: {hours}, minutes: {minutes}");

                DateTime now = DateTime.Now;
                DateTime targetTime = new DateTime(now.Year, now.Month, now.Day, hours, minutes, 0);

                if (targetTime <= now)
                {
                    targetTime = targetTime.AddDays(1);
                    ShowStatus($"WAIT_UNTIL: Target time was in the past, adjusted to next day: {targetTime:HH:mm}");
                    System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL: Adjusted target time to {targetTime:HH:mm}");
                }

                TimeSpan waitDuration = targetTime - now;
                if (waitDuration.TotalMilliseconds <= 0)
                {
                    ShowStatus($"WAIT_UNTIL: Target time '{targetTimeStr}' is in the past or invalid after adjustment");
                    System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL: Invalid wait duration: {waitDuration.TotalMilliseconds}ms");
                    return false;
                }

                var logger = new CommandLogger();
                logger.LogCommand($"WAITUNTIL {targetTimeStr} ({targetTime:HH:mm})");
                ShowStatus($"WAIT_UNTIL: Pausing until {targetTime:HH:mm}");
                System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL: Pausing for {waitDuration.TotalSeconds:F2}s until {targetTime:HH:mm}");

                await Task.Delay((int)waitDuration.TotalMilliseconds, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);

                ShowStatus("WAIT_UNTIL: Wait completed successfully");
                System.Diagnostics.Debug.WriteLine("WAIT_UNTIL: Wait completed successfully");
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("WAIT_UNTIL: Cancelled");
                System.Diagnostics.Debug.WriteLine("WAIT_UNTIL: Command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"WAIT_UNTIL: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL: Error - {ex.Message}");
                return false;
            }
        }
        private async Task<bool> ExecuteWaitUntilTempCommand(List<string> parameters)
        {
            try
            {
                ShowStatus("WAIT_UNTIL_TEMP: Starting");
                System.Diagnostics.Debug.WriteLine("WAIT_UNTIL_TEMP: Starting");

                if (parameters.Count != 1)
                {
                    ShowStatus($"WAIT_UNTIL_TEMP: Expected 1 parameter (target °C), got {parameters.Count}");
                    return false;
                }

                if (!double.TryParse(parameters[0], System.Globalization.NumberStyles.Float,
                                     System.Globalization.CultureInfo.InvariantCulture, out double targetTemp))
                {
                    ShowStatus($"WAIT_UNTIL_TEMP: Invalid temperature: '{parameters[0]}'. Use e.g., 25.0");
                    return false;
                }

                var logger = new CommandLogger();
                logger.LogCommand($"WAITUNTILTEMP {targetTemp}°C");

                var cts = _scriptCancellationTokenSource?.Token ?? CancellationToken.None;

                while (true)
                {
                    cts.ThrowIfCancellationRequested();

                    double current = GlobalData.CameraTemperature;

                    if (current <= targetTemp)
                    {
                        ShowStatus($"WAIT_UNTIL_TEMP: Reached {current:F1}°C ≤ {targetTemp}°C. Proceeding.");
                        System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL_TEMP: Condition met at {current:F1}°C");
                        return true;
                    }

                    ShowStatus($"WAIT_UNTIL_TEMP: {current:F1}°C > {targetTemp}°C, waiting...");
                    await Task.Delay(1000, cts); // Check every 1 second
                }
            }
            catch (OperationCanceledException)
            {
                ShowStatus("WAIT_UNTIL_TEMP: Cancelled");
                System.Diagnostics.Debug.WriteLine("WAIT_UNTIL_TEMP: Cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"WAIT_UNTIL_TEMP: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"WAIT_UNTIL_TEMP: Exception - {ex}");
                return false;
            }
        }
        private async Task<bool> ExecuteMsgBoxCommand(List<string> parameters)
        {
            try
            {
                // Check if there are parameters
                if (parameters.Count == 0)
                {
                    ShowStatus("MSGBOX: No message provided");
                    System.Diagnostics.Debug.WriteLine("MSGBOX: No parameters provided");
                    return false;
                }

                // Join parameters into a single string
                string messageText = string.Join(" ", parameters);
                if (string.IsNullOrWhiteSpace(messageText))
                {
                    ShowStatus("MSGBOX: Message cannot be empty");
                    System.Diagnostics.Debug.WriteLine("MSGBOX: Empty message provided");
                    return false;
                }

                // Log the command
                var logger = new CommandLogger();
                logger.LogCommand($"MSGBOX {messageText}");
                ShowStatus($"MSGBOX: Displaying '{messageText}'");
                System.Diagnostics.Debug.WriteLine($"MSGBOX: Displaying '{messageText}'");

                // Display the message box and wait for user input
                await Task.Run(() =>
                {
                    MessageBox.Show(messageText, "Script Message", MessageBoxButtons.OK, MessageBoxIcon.Information, MessageBoxDefaultButton.Button1, MessageBoxOptions.DefaultDesktopOnly);
                }, _scriptCancellationTokenSource?.Token ?? CancellationToken.None);

                ShowStatus($"MSGBOX: Message '{messageText}' acknowledged");
                System.Diagnostics.Debug.WriteLine($"MSGBOX: Message '{messageText}' acknowledged");
                return true;
            }
            catch (OperationCanceledException)
            {
                ShowStatus("MSGBOX: Cancelled");
                System.Diagnostics.Debug.WriteLine("MSGBOX command cancelled");
                return false;
            }
            catch (Exception ex)
            {
                ShowStatus($"MSGBOX: Error - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"MSGBOX: Error - {ex.Message}");
                return false;
            }
        }
        private void ShowStatus(string message)
        {
            if (textBoxLog != null)
            {
                textBoxLog.AppendText($"STATUS: {message}\r\n");
            }
            System.Diagnostics.Debug.WriteLine($"STATUS: {message}");
        }

        private void ButtonGoto_Click(object sender, EventArgs e)
        {
            try
            {
                // Get RA and Dec from GlobalData
                double ra = GlobalData.GotoRA;
                double dec = GlobalData.GotoDec;
         

                // Format the GOTO command
                string gotoLine = $"GOTO {GlobalData.CaptureRA.ToString("F6")} {GlobalData.CaptureDec.ToString("F6")}";

                // Append the GOTO command to txtEditor
                if (txtEditor.Text.Length > 0 && !txtEditor.Text.EndsWith(Environment.NewLine))
                {
                    txtEditor.AppendText(Environment.NewLine);
                }
                txtEditor.AppendText(gotoLine);

                // Log the action
                var logger = new CommandLogger();
                logger.LogCommand(gotoLine);
                ShowStatus($"GOTO: Appended '{gotoLine}' to script");
                System.Diagnostics.Debug.WriteLine($"GOTO: Appended '{gotoLine}' to script");
            }
            catch (Exception ex)
            {
                ShowStatus($"GOTO: Error appending to script - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"GOTO: Error appending to script - {ex.Message}");
            }
        }

        private void ButtonFilter_Click(object sender, EventArgs e)
        {
            try
            {
                // Format the FILTER command
                string filterLine = "FILTER nebula";

                // Append the FILTER command to txtEditor
                if (txtEditor.Text.Length > 0 && !txtEditor.Text.EndsWith(Environment.NewLine))
                {
                    txtEditor.AppendText(Environment.NewLine);
                }
                txtEditor.AppendText(filterLine);

                // Log the action
                var logger = new CommandLogger();
                logger.LogCommand(filterLine);
                ShowStatus($"FILTER: Appended '{filterLine}' to script");
                System.Diagnostics.Debug.WriteLine($"FILTER: Appended '{filterLine}' to script");
            }
            catch (Exception ex)
            {
                ShowStatus($"FILTER: Error appending to script - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"FILTER: Error appending to script - {ex.Message}");
            }
        }

        private void ButtonSpeak_Click(object sender, EventArgs e)
        {
            try
            {
                // Format the SPEAK command
                string speakLine = "SPEAK";

                // Append the SPEAK command to txtEditor
                if (txtEditor.Text.Length > 0 && !txtEditor.Text.EndsWith(Environment.NewLine))
                {
                    txtEditor.AppendText(Environment.NewLine);
                }
                txtEditor.AppendText(speakLine);

                // Log the action
                var logger = new CommandLogger();
                logger.LogCommand(speakLine);
                ShowStatus($"SPEAK: Appended '{speakLine}' to script");
                System.Diagnostics.Debug.WriteLine($"SPEAK: Appended '{speakLine}' to script");
            }
            catch (Exception ex)
            {
                ShowStatus($"SPEAK: Error appending to script - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"SPEAK: Error appending to script - {ex.Message}");
            }
        }

        private void ButtonMsgBox_Click(object sender, EventArgs e)
        {
            try
            {
                // Format the MSGBOX command
                string msgBoxLine = "MSGBOX";

                // Append the MSGBOX command to txtEditor
                if (txtEditor.Text.Length > 0 && !txtEditor.Text.EndsWith(Environment.NewLine))
                {
                    txtEditor.AppendText(Environment.NewLine);
                }
                txtEditor.AppendText(msgBoxLine);

                // Log the action
                var logger = new CommandLogger();
                logger.LogCommand(msgBoxLine);
                ShowStatus($"MSGBOX: Appended '{msgBoxLine}' to script");
                System.Diagnostics.Debug.WriteLine($"MSGBOX: Appended '{msgBoxLine}' to script");
            }
            catch (Exception ex)
            {
                ShowStatus($"MSGBOX: Error appending to script - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"MSGBOX: Error appending to script - {ex.Message}");
            }
        }

        private void ButtonWait_Click(object sender, EventArgs e)
        {
            try
            {
                // Format the WAIT command
                string waitLine = "WAIT";

                // Append the WAIT command to txtEditor
                if (txtEditor.Text.Length > 0 && !txtEditor.Text.EndsWith(Environment.NewLine))
                {
                    txtEditor.AppendText(Environment.NewLine);
                }
                txtEditor.AppendText(waitLine);

                // Log the action
                var logger = new CommandLogger();
                logger.LogCommand(waitLine);
                ShowStatus($"WAIT: Appended '{waitLine}' to script");
                System.Diagnostics.Debug.WriteLine($"WAIT: Appended '{waitLine}' to script");
            }
            catch (Exception ex)
            {
                ShowStatus($"WAIT: Error appending to script - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"WAIT: Error appending to script - {ex.Message}");
            }
        }

        private void ButtonWaitUntil_Click(object sender, EventArgs e)
        {
            try
            {
                // Format the WAITUNTIL command
                string waitUntilLine = "WAITUNTIL";

                // Append the WAITUNTIL command to txtEditor
                if (txtEditor.Text.Length > 0 && !txtEditor.Text.EndsWith(Environment.NewLine))
                {
                    txtEditor.AppendText(Environment.NewLine);
                }
                txtEditor.AppendText(waitUntilLine);

                // Log the action
                var logger = new CommandLogger();
                logger.LogCommand(waitUntilLine);
                ShowStatus($"WAITUNTIL: Appended '{waitUntilLine}' to script");
                System.Diagnostics.Debug.WriteLine($"WAITUNTIL: Appended '{waitUntilLine}' to script");
            }
            catch (Exception ex)
            {
                ShowStatus($"WAITUNTIL: Error appending to script - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"WAITUNTIL: Error appending to script - {ex.Message}");
            }
        }

        private void ButtonStartCapture_Click(object sender, EventArgs e)
        {
            try
            {
                // Format the STARTCAPTURE command
                string startCaptureLine = "STARTCAPTURE 20 180 2000 true true";

                // Append the STARTCAPTURE command to txtEditor
                if (txtEditor.Text.Length > 0 && !txtEditor.Text.EndsWith(Environment.NewLine))
                {
                    txtEditor.AppendText(Environment.NewLine);
                }
                txtEditor.AppendText(startCaptureLine);

                // Log the action
                var logger = new CommandLogger();
                logger.LogCommand(startCaptureLine);
                ShowStatus($"STARTCAPTURE: Appended '{startCaptureLine}' to script");
                System.Diagnostics.Debug.WriteLine($"STARTCAPTURE: Appended '{startCaptureLine}' to script");
            }
            catch (Exception ex)
            {
                ShowStatus($"STARTCAPTURE: Error appending to script - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"STARTCAPTURE: Error appending to script - {ex.Message}");
            }
        }

        private void ButtonHalt_Click(object sender, EventArgs e)
        {
            try
            {
                // Format the HALT command
                string haltLine = "HALT";

                // Append the HALT command to txtEditor
                if (txtEditor.Text.Length > 0 && !txtEditor.Text.EndsWith(Environment.NewLine))
                {
                    txtEditor.AppendText(Environment.NewLine);
                }
                txtEditor.AppendText(haltLine);

                // Log the action
                var logger = new CommandLogger();
                logger.LogCommand(haltLine);
                ShowStatus($"HALT: Appended '{haltLine}' to script");
                System.Diagnostics.Debug.WriteLine($"HALT: Appended '{haltLine}' to script");
            }
            catch (Exception ex)
            {
                ShowStatus($"HALT: Error appending to script - {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"HALT: Error appending to script - {ex.Message}");
            }
        }
    }
}