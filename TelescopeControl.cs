using System;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
//using System.Text.Json;

namespace OriginLiveView
{
    public class TelescopeControl
    {
        private readonly ClientWebSocket _ws;
        private readonly CancellationTokenSource _cancellationTokenSource;
        private readonly SemaphoreSlim _sendSemaphore = new SemaphoreSlim(1, 1); // Ensure one SendAsync at a time
        private readonly ErrorLogger _logger = new ErrorLogger(); // Single instance

        public TelescopeControl(ClientWebSocket webSocket, CancellationTokenSource cts)
        {
            _ws = webSocket ?? throw new ArgumentNullException(nameof(webSocket));
            _cancellationTokenSource = cts ?? throw new ArgumentNullException(nameof(cts));
        }

        public bool IsConnected => _ws != null && _ws.State == WebSocketState.Open;

        public async Task SendCommandAsync(object command)
        {
            if (_ws == null || _ws.State != WebSocketState.Open)
            {
                throw new InvalidOperationException("WebSocket is not connected.");
            }

            await _sendSemaphore.WaitAsync();
            try
            {
                string json = JsonConvert.SerializeObject(command);
                byte[] buffer = Encoding.UTF8.GetBytes(json);
                await _ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                //ErrorLogger logger = new ErrorLogger();
                _logger.LogError($"Error sending command: {ex.Message}");
                throw;
            }
            finally
            {
                _sendSemaphore.Release();
            }
        }


        public async Task SendGetStatusCommandAsync()
        {
            if (GlobalData.SquenceID == 0) GlobalData.SquenceID = 1; // Initialize to non-zero
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "GetStatus",
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command",
                Destination = "TaskController"
            };

            await SendCommandAsync(command);
        }


        public async Task SendSlewToCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "GotoRaDec",
                Dec = GlobalData.GotoDec,
                Destination = "Mount",
                Ra = GlobalData.GotoRA,
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
            };

            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            //logger.LogError($"Goto Sent to Telescope Control Command {command} DEC: {GlobalData.GotoDec} Destination:Mount  RA:{GlobalData.GotoRA} SeqID: {GlobalData.SquenceID}");
            _logger.LogError($"Command Sent GOTO to Origin {command}");
        }

        public async Task SendStartCaptureCommandAsync()
        {
            GlobalData.SquenceID++;
            string OriginUuid = GlobalData.GenerateUuid();
            //Console.WriteLine(OriginUuid);
            var command = new
            {
                Command = "RunImaging",
                Destination = "TaskController",
                Ra = GlobalData.RA,
                Dec = GlobalData.Dec,
                ExposureTime = GlobalData.ExposureTime,
                ISO = GlobalData.ISO,
                Name = GlobalData.Name,
                ObjectMagnitude = 0.70456099510192871,
                SaveRawImage = GlobalData.SaveRaw,
                TotalDuration = GlobalData.TotalDuration,
                ContinueImagingIfStackingFails = GlobalData.DisableAutoCancel,
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command",
                Uuid = OriginUuid

            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Capture Sent to Origin ");
        }
        public async Task SendStopCaptureCommandAsync()
        {
            GlobalData.SquenceID++;
            string OriginUuid = GlobalData.GenerateUuid();
            //Console.WriteLine(OriginUuid);
            var command = new
            {
                Command = "HaltTasks",
                Destination = "TaskController",
                Type = "Command",
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView"

            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Halt Sent to Origin ");
        }
        public async Task SendSyncCommandAsync(double ra, double dec)
        {
            var command = new
            {

            };
            await SendCommandAsync(command);
        }
        public async Task SendInitializeCommandAsync()
        {
            GlobalData.SquenceID++;
            var currentDateTime = DateTime.Now; // Declare currentDateTime here
            var command = new
            {
                Command = "RunInitialize",
                Destination = "TaskController",
                Device = "TaskController",
                Date = currentDateTime.ToString("dd MM yyyy"),
                Latitude = GlobalData.Latitude,                                  //-0.00927098,
                Longitude = GlobalData.Longitude,                                 //-1.40415451,
                Time = currentDateTime.ToString("HH:mm:ss"),
                TimeZone = GlobalData.TimeZone,
                FakeInitialize = GlobalData.FakeInitialize,
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command",

            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Initialize Sent to Origin ");

        }

        public async Task SendRebootCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "Reboot",
                Destination = "System",
                Device = "TaskController",
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command",
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Reboot Sent to Origin ");

        }
        public async Task SendSwitchBootPartitionCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "SwapBootPartiton",
                Destination = "System",
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command",
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Switch Boot Partition Sent to Origin");
        }

        public async Task SendRestoreFactoryFlatsCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "TaskController",
                Command = "RestoreFlatsToFactory",
                Type = "Command",
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Restore Factory Darks Sent to Origin ");

        }
        public async Task SendRestoreFactoryDarksCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "TaskController",
                Command = "RestoreDarksToFactory",
                Type = "Command",
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Restore Factory Darks Sent to Origin ");

        }
        public async Task SendTakeDarksCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "TaskController",
                Command = "RunGenerateNewDarks",
                Type = "Command",
                BitDepth = GlobalData.DARKBitDepth,
                ExposureTime = GlobalData.DARKExposureTime,
                ISO = GlobalData.DARKISO,
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Take Darks Sent to Origin");

        }
        public async Task SendTakeFlatsCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "TaskController",
                Command = "RunGenerateNewFlat",
                Type = "Command",
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Take Flats Sent to Origin ");

        }
        public async Task SendFilterClearCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Camera",
                Command = "SetFilter",
                Type = "Command",
                SequenceID = GlobalData.SquenceID,
                Filter = "Clear"
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Filter Clear Sent to Origin ");

        }
        public async Task SendFilterNebulaCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Camera",
                Command = "SetFilter",
                Type = "Command",
                SequenceID = GlobalData.SquenceID,
                Filter = "Nebula"
            };
            await SendCommandAsync(command);
            // ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Filter Nebula Sent to Origin");

        }
        public async Task SendGetFilterCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Camera",
                Command = "GetFilter",
                Type = "Command",
                SequenceID = GlobalData.SquenceID

            };
            await SendCommandAsync(command);
            // ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Filter Nebula Sent to Origin ");

        }
        public async Task SendPowerDownCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "System",
                Command = "PowerDownOrigin",
                Type = "Command",
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Power Down Sent to Origin");

        }

        public async Task SendFansCommandAsync()
        {
            GlobalData.SquenceID++;
            GlobalData.CPUFanOn = true;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Environment",
                Command = "SetFans",
                Type = "Command",
                SequenceID = GlobalData.SquenceID,
                CpuFanOn = GlobalData.CPUFan,
                OtaFanOn = GlobalData.OTAFan
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command CPU Fan On Sent to Origin ");

        }

        public async Task SendDewHeaterAutoCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "DewHeater",
                Command = "EnableAuto",
                Type = "Command",
                SequenceID = GlobalData.SquenceID,
                Aggression = GlobalData.DewAgressiveness

            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Dew Heater Auto Agression set to 5 Sent to Origin ");

        }
        public async Task SendDewHeaterManualCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "DewHeater",
                Command = "EnableManual",
                Type = "Command",
                SequenceID = GlobalData.SquenceID,
                PowerLevel = GlobalData.DewPower

            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Dew Heater Auto Agression set to 5 Sent to Origin ");

        }
        public async Task SendScheduleImageCommandAsync()
        {
            try
            {
                GlobalData.SquenceID++; // Corrected from SquenceID
                string originUuid = GlobalData.GenerateUuid();
                string commandUuid = GlobalData.GenerateUuid();

                var command = new
                {
                    AutoFocusAfterSlew = GlobalData.ScheduleAF,
                    Command = "RunObservingList",
                    Destination = "TaskController",
                    Location = new[]
                    {
                    new
                    {
                        Dec = GlobalData.CaptureDec,
                        MinimumStartTime = GlobalData.MinStarttime,
                        Name = GlobalData.Name,
                        ObjectMagnitude = GlobalData.ObjectMagnitude,
                        ObjectSize = GlobalData.ObjectSize,
                        Ra = GlobalData.CaptureRA,
                        SaveRawImage = GlobalData.SaveRaw,
                        TotalDuration = GlobalData.TotalDuration,
                        ExposureTime = GlobalData.ExposureTime,
                        ISO = GlobalData.ISO,
                        ContinueImagingIfStackingFails = GlobalData.DisableAutoCancel,
                        Uuid = commandUuid
                    }
                },
                    PowerDownOnCompletion = GlobalData.SchedulePowerDown,
                    SequenceID = GlobalData.SquenceID,
                    Source = "OriginLiveView",
                    Type = "Command",
                    Uuid = originUuid
                };

                await SendCommandAsync(command);

                //ErrorLogger logger = new ErrorLogger();
               //_logger.LogError($"Command Schedule Image Sent to Origin: {JsonConvert.SerializeObject(command, Formatting.Indented)}");
                _logger.LogError("Command Schedule Image Sent to Origin: ");
            }
            catch (Exception ex)
            {
                //ErrorLogger logger = new ErrorLogger();
                _logger.LogError($"Failed to send Schedule Image command: {ex.Message}");
                throw; // Rethrow or handle as needed
            }
        }
        public async Task SendGenerateLogsCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "System",
                Command = "GetLogs",
                Type = "Command",
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Get Logs Sent to Origin ");

        }
        public async Task SendGetSensorsCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Environment",
                Command = "GetSensors",
                Type = "Command",
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Get Sensor Data Sent to Origin");

        }
        public async Task SendGetFocusPositionCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Focuser",
                Command = "GetPosition",
                Type = "Command",
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Get Focus Position Data Sent to Origin ");

        }
        public async Task SendSetFocusPositionCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Focuser",
                Command = "MoveTo",
                Type = "Command",
                SequenceID = GlobalData.SquenceID,
                Value = GlobalData.GotoFocuserPosition

            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Get Focus Position Data Sent to Origin");

        }
        public async Task SendSlewTelescopeAltCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Mount",
                Command = "Slew",
                Type = "Command",
                SequenceID = GlobalData.SquenceID,
                AltRate = GlobalData.SlewAltRate,
                AzmRate = GlobalData.SlewAzRate,

            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Slew Telescope Sent to Origin");

        }
        public async Task SendCameraSettingsCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Camera",
                Command = "SetCaptureParameters",
                Type = "Command",
                Binning = GlobalData.CameraBin,
                BitDepth = GlobalData.CameraBitDepth,
                ColorBBalance = GlobalData.CameraBlueGain,
                ColorGBalance = GlobalData.CameraGreenGain,
                ColorRBalance = GlobalData.CameraRedGain,
                Exposure = GlobalData.CameraExposure,
                ISO = GlobalData.ISO,
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command SetCaptureParameter Sent to Origin");

        }
        public async Task SendGetCameraSettingsCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Camera",
                Command = "GetCaptureParameters",
                Type = "Command",
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Get CaptureParameter Sent to Origin");

        }
        public async Task SendAutoFocusGotoCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Focuser",
                Command = "AutoFocusAfterGoto",
                Type = "Command",
                Value = GlobalData.FocusonGoto,
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Focus on Goto Sent to Origin ");

        }
        public async Task SendAutoFocusTempChangeCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "Focuser",
                Command = "AutoFocusOnTemperatureChange",
                Type = "Command",
                EnableTemperatureShiftAutoFocus = GlobalData.FocusonTempChange,
                TemperatureThresholdC = GlobalData.TemperatureChange,
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Focus on Temperature Change Sent to Origin");

        }
        public async Task SendScheduleMosaicImageCommandAsync()
        {
            GlobalData.SquenceID++;
            string OriginUuid = GlobalData.GenerateUuid();
            var command = new
            {
                Command = "CaptureMosaic",
                Destination = "TaskController",
                AutoFocusAfterSlew = GlobalData.MosaicAF,
                MosaicHeight = GlobalData.MosaicHeight,
                MosaicWidth = GlobalData.MosaicWidth,
                MosaicOrientation = GlobalData.MosaicOrientation,
                ObjectDec = GlobalData.CaptureDec,
                ExposureTime = GlobalData.ExposureTime,
                ISO = GlobalData.ISO,
                MinimumStartTime = GlobalData.MinStarttime,
                ObjectName = GlobalData.Name,
                ObjectMagnitude = GlobalData.ObjectMagnitude,
                ObjectSize = GlobalData.ObjectSize,
                ObjectRa = GlobalData.CaptureRA,
                SaveRawImage = GlobalData.SaveRaw,
                TotalDuration = GlobalData.TotalDuration,
                PowerDownOnCompletion = GlobalData.MosaicPowerDown,
                ContinueImagingIfStackingFails = GlobalData.DisableAutoCancel,
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command",
                Uuid = OriginUuid

            };
            await SendCommandAsync(command);
            //Console.WriteLine($"Mosaic Capture Sent: {command}");
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Capture Mosaic Sent to Origin ");
        }
        public async Task SendComposeMosaicImageCommandAsync()
        {
            GlobalData.SquenceID++;
            //string OriginUuid = GlobalData.GenerateUuid();
            var command = new
            {
                Command = "ComposeMosaic",
                Destination = "TaskController",
                MosaicHeight = GlobalData.MosaicHeight,
                MosaicWidth = GlobalData.MosaicWidth,
                MosaicOrientation = GlobalData.MosaicOrientation,
                ObjectRa = GlobalData.CaptureRA,
                ObjectDec = GlobalData.CaptureDec,
                //MinimumStartTime = GlobalData.MinStarttime,
                ObjectName = GlobalData.Name,
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
                //Uuid = OriginUuid

            };
            await SendCommandAsync(command);
            //Console.WriteLine($"Mosaic Capture Sent: {command}");
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Compose Mosaic Sent to Origin ");
        }
        public async Task SendRetakeDarksCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "TaskController",
                Command = "RunGenerateNewDarks",
                Type = "Command",
                BitDepth = GlobalData.CameraBitDepth,
                ExposureTime = GlobalData.ExposureTime,
                ISO = GlobalData.ISO,
                SequenceID = GlobalData.SquenceID
            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Retake Darks Sent to Origin");
        }
        public async Task SendRetakeFlatsCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Source = "OriginLiveView",
                Destination = "TaskController",
                Command = "RunGenerateNewFlat",
                Type = "Command",
                SequenceID = GlobalData.SquenceID

            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Retake Flats Sent to Origin");
        }
        public async Task SendPolarAlignCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "PolarAlign",
                Destination = "TaskController",
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
            };
            await SendCommandAsync(command);

            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("CommandPolar Align Sent to Origin:");
        }
        public async Task SendMountSettingsCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "SetMountConfig",
                Destination = "Mount",
                IsEquatorial = GlobalData.IsEQ,
                OnWedge = GlobalData.IsWesge,
                AltBacklash = GlobalData.AltBacklash,
                AzmBacklashh = GlobalData.AzBacklash,
                CustomRate9Speed = GlobalData.MountCustSpeed,
                EnableCustomRate9 = GlobalData.MountCustEnabled,
                EnablePec = GlobalData.MountPecEnabled,
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
            };
            await SendCommandAsync(command);

            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command Mount Settings Sent to Origin ");
        }
        public async Task SendELPanelConnectCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "Connect",
                Destination = "ElPanel",
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
            };
            await SendCommandAsync(command);

            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command EL Panel Connect Sent to Origin ");
        }
        public async Task SendELPanelDisconnectCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "Disconnect",
                Destination = "ElPanel",
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
            };
            await SendCommandAsync(command);

            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command EL Panel DisConnect Sent to Origin ");
        }
        public async Task SendELPanelLightOnCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "TurnOnLight",
                Destination = "ElPanel",
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
            };
            await SendCommandAsync(command);

            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command EL Panel Light On Sent to Origin ");
        }
        public async Task SendELPanelLightOffCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "TurnOffLight",
                Destination = "ElPanel",
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
            };
            await SendCommandAsync(command);

            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command EL Panel Light Off Sent to Origin ");
        }
        public async Task SendELPanelSetLightLevelCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "SetLightLevel",
                Destination = "ElPanel",
                LightLevel = GlobalData.ElPanelLightLevel,
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
            };
            await SendCommandAsync(command);

            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command EL Panel Set Light Level Sent to Origin ");
        }
        public async Task SendRunSampleCaptureCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "RunSampleCapture",
                Destination = "TaskController",
                ExposureTime = GlobalData.BiasExposuretime,
                ISO = GlobalData.ISO,
                Calibrate = GlobalData.Calibrate,
                Debayer = GlobalData.Debayer,
                Binning = GlobalData.Binning,
                BitDepth = GlobalData.BitDepth,
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
            };
            await SendCommandAsync(command);

            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command RunSampleCapture Sent to Origin ");
        }
        public async Task SendAutoFocusCommandAsync()
        {
            GlobalData.SquenceID++;
            var command = new
            {
                Command = "RunInfinityAutoFocus",
                Destination = "TaskController",
                Range = 1,
                UseFineFocus = true,
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView",
                Type = "Command"
            };
            await SendCommandAsync(command);

            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command EL Panel Set Light Level Sent to Origin ");
        }
        public async Task SendRUnAddReferenceCommandAsync()
        {
            GlobalData.SquenceID++;
            string OriginUuid = GlobalData.GenerateUuid();
            //Console.WriteLine(OriginUuid);
            var command = new
            {
                Command = "RunAddReference",
                Destination = "TaskController",
                Type = "Command",
                SequenceID = GlobalData.SquenceID,
                Source = "OriginLiveView"

            };
            await SendCommandAsync(command);
            //ErrorLogger logger = new ErrorLogger();
            _logger.LogError("Command RunAddReference Sent to Origin ");
        }

    }
    }