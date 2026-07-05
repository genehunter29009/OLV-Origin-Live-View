using System;
using System.Net.NetworkInformation;
using System.Windows.Forms;

namespace OriginLiveView
{
    public static class GlobalData
    {
        public static double RA { get; set; } = 0.0; // Actual RA in hours
        public static double Dec { get; set; } = 0.0; //Actual Dec in degrees
        // Optional: Add microdegree versions for Stellarium compatibility
        public static double OLDRA { get; set; } = 0.0; // Actual RA in hours
        public static double OLDDec { get; set; } = 0.0; //Actual Dec in degrees
        // Optional: Add microdegree versions for Stellarium compatibility
        public static double CaptureRA { get; set; } = 0.0; // Capture RA in hours
        public static double CaptureDec { get; set; } = 0.0; // Capture Dec in degrees
        public static int RAMicrodegrees { get; set; } = 0; // Actual RA in microdegrees
        public static int DecMicrodegrees { get; set; } = 0; // Actual Dec in microdegrees
        public static double GotoRA { get; set; } = 0.0; // Goto RA in hours
        public static double GotoDec { get; set; } = 0.0; //Goto Dec in degrees

        public static object LockObject = new object();

        // Optional: Add microdegree versions for Stellarium compatibility
        public static int OLDRAMicrodegrees { get; set; } = 0; // Goto RA in microdegrees
        public static int OLDDecMicrodegrees { get; set; } = 0; // Goto Dec in microdegrees
        public static int ExposureTime { get; set; } = 20; // Exposure time in seconds\\
        public static int ISO { get; set; } = 200; // ISO value
        public static int BitDepth { get; set; } = 16; // Bit depth for the image
        public static int Binning { get; set; } = 1; // Binning factor for the image
        public static Boolean Debayer { get; set; } = false; // Debayering status
        public static Boolean Calibrate { get; set; } = false; // Calibration status
        public static double BiasExposuretime { get; set; } = 0.0; // Bias exposure time
        public static string ImageLocation { get; set; } = ""; // Bias location
        public static string BiasName { get; set; } = ""; // Bias name
        public static int BiasNameInt { get; set; } = 0; // Bias name integer
        public static Boolean BiasCaptureFlag { get; set; } = false; // Bias capture flag
        public static Boolean FlatCaptureFlag  { get; set; } = false; // Bias flat capture flag
        public static string Name { get; set; } = "OriginLiveView"; // Name of the application  
        public static Boolean SaveRaw { get; set; } = true; // Save raw image
        public static int SquenceID { get; set; } = 1; // Sequence ID for the image capture
        public static double Latitude { get; set; } = 0.0; // Latitude in degrees
        public static double Longitude { get; set; } = 0.0; // Longitude in degrees
        public static string TimeZone { get; set; } = "America/Guayaquil"; // Time zone
        public static int DARKBitDepth { get; set; } = 16; // Bit depth for dark frame 
        public static int DARKISO { get; set; } = 200; // ISO for dark frame
        public static int DARKExposureTime { get; set; } = 20; // Exposure time for dark frame 
        public static Boolean CPUFanOn { get; set; } = true; // CPU fan status
        public static Boolean OTAFanOn { get; set; } = true; // OTA fan status
        public static double ObjectSize { get; set; } = 0; // Size of the object
        public static double ObjectMagnitude { get; set; } = 0; // Magnitude of the object
        public static string MinStarttime { get; set; } = "00:00"; // Minimum start time for the object
        public static int TotalDuration { get; set; } = 0; // Total duration for the object
        public static int focuserposition { get; set; } = 0; // Focuser position
        public static int GotoFocuserPosition { get; set; } = 0; // Goto focuser position
        public static int AmbientTemperature { get; set; } = 0; // Ambient temperature
        public static int CameraTemperature { get; set; } = 0; // CCD temperature
        public static int CPUTemperature { get; set; } = 0; // CPU temperature 
        public static int DewPointTemperature { get; set; } = 0;  //Dew Point temperature
        public static int Humidity {  get; set; } = 0;  // humidity
        public static int FrontCellTemperature { get; set; } = 0; // Front cell temperature
        public static int SlewAltRate { get; set; } = 0; // Altitude rate
        public static int SlewAzRate { get; set; } = 0; // Azimuth rate
        public static double BatteryVoltage { get; set; } = 0.0; // Battery voltage
        //public static string BatteryLevel { get; set; } = ""; // Battery level
        public static int SlewSpeed  { get; set; } = 0; // Slew speed
        public static string Filter { get; set; } = ""; // Filter name
        public static int DewAgressiveness { get; set; } = 5; // Dew heater aggressiveness
        public static double DewPower { get; set; } = 0.10; // Dew heater power
        public static int CameraBin { get; set; } = 1; // Camera binning
        public static int CameraBitDepth { get; set; } = 16; // Camera bit depth
        public static double CameraBlueGain { get; set; } = 0.0; // Camera blue gain
        public static double CameraGreenGain { get; set; } = 0.0; // Camera green gain   
        public static double CameraRedGain { get; set; } = 0.0; // Camera red gain
        public static double CameraExposure { get; set; } = 0.0; // Camera exposure time
        public static Boolean FocusonGoto { get; set; } = false; // Focus on Goto
        public static Boolean DisableAutoCancel { get; set; } = true; // Auto cancel flag
        public static Boolean FocusonTempChange { get; set; } = false; // Focus on temperature change
        public static double TemperatureChange { get; set; } = 0.0; // Temperature change
        public static double MosaicHeight { get; set; } = 0.0; // Mosaic height 
        public static double MosaicWidth { get; set; } = 0.0; // Mosaic width
        public static double MosaicOrientation { get; set; } = 0.0; // Mosaic orientation
        public static Boolean MosaicPowerDown { get; set; } = false; // Mosaic power down
        public static Boolean MosaicAF { get; set; } = true; // Mosaic autofocus
        public static Boolean ScheduleAF { get; set; } = true; // Schedule autofocus
        public static Boolean SchedulePowerDown { get; set; } = false; // Schedule power down
        public static double AltitudeError { get; set; } // In degrees
        public static double AzimuthError { get; set; } // In degrees
        public static Boolean FakeInitialize { get; set; } = false; // Flag for fake initialization

        public static int DitherButton { get; set; } = 0; // Dither button state
        public static int DitherCount { get; set; } = 1; // Dither count

        public static Boolean OTAFan { get; set; } = true; // OTA fan status   
        public static Boolean CPUFan { get; set; } = true; // CPU fan status
        public static Boolean IsEQ { get; set; } = false; // Is EQ mount
        public static Boolean IsWesge { get; set; } = false;    
        public static int AltBacklash { get; set; } = 0; // Altitude backlash
        public static int AzBacklash { get; set; } = 0; // Azimuth backlash
        public static double MountCustSpeed { get; set; } = 0.0; // Custom mount speed
        public static Boolean MountCustEnabled { get; set; } = false; // Custom mount enabled   
        public static Boolean MountPecEnabled { get; set; } = false; // PEC enabled

        public static long FreeBytes { get; set; } = 0; // Free bytes on the disk
        public static double FreeGigabytes { get; set; }
        public static string Stage { get; set; } = ""; // Current stage of the process
        public static string State { get; set; } = ""; // Current state of the process
        public static int ElPanelLightLevel { get; set; } = 0; // Light level for the ELPanel
       

        public static string GenerateUuid()
        {
            return Guid.NewGuid().ToString();
        }
    }
}  
