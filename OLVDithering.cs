using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Timers; // For System.Timers.Timer

namespace OriginLiveView
{
    public partial class OLVDithering : Form
    {
        private readonly TelescopeControl _telescopeControl;
        private readonly System.Timers.Timer _ditheringTimer; // Timer for 1-minute intervals

        public OLVDithering(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));

            // Initialize the timer
            _ditheringTimer = new System.Timers.Timer(60000); // 60,000 ms = 1 minute
            _ditheringTimer.Elapsed += DitheringTimer_Elapsed;
            _ditheringTimer.AutoReset = true; // Repeat every minute
            _ditheringTimer.SynchronizingObject = this; // Ensure events are raised on the UI thread
        }
        private void TrackBarTime_Scroll(object sender, EventArgs e)
        {
            // Update the label to show the selected dither time
             LabelDitherTime.Text = $"Dither Time: {TrackBarTime.Value * 100} ms"; // Convert to milliseconds
 
        }
        private void TrackBarSpeed_Scroll(object sender, EventArgs e)
        {
            // Update the label to show the selected dither speed
            LabelSpeed.Text = $"Dither Speed: {TrackBarSpeed.Value}";
        }
        private async void DitheringTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            // This runs every minute while dithering is active
            //Console.WriteLine($"Dithering active at {DateTime.Now:HH:mm:ss}");
            //LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";

            // Start the dithering process
            int Dithertime = TrackBarTime.Value * 100;
            LabelDitherTime.Text = $"Dither Time: {Dithertime} ms";
            int DitherSpeed = TrackBarSpeed.Value;
            if (GlobalData.DitherCount == 1)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 2)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 3)
            {
                GlobalData.SlewAltRate = DitherSpeed;
                GlobalData.SlewAzRate = 0;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 4)
            {
                GlobalData.SlewAltRate = DitherSpeed;
                GlobalData.SlewAzRate = 0;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 5)
            {
                GlobalData.SlewAltRate = DitherSpeed;
                GlobalData.SlewAzRate = 0;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 6)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = -DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 7)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = -DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 8)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = -DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 9)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = -DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 10)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = -DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 11)
            {
                GlobalData.SlewAltRate = -DitherSpeed;
                GlobalData.SlewAzRate = 0;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 12)
            {
                GlobalData.SlewAltRate = -DitherSpeed;
                GlobalData.SlewAzRate = 0;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 13)
            {
                GlobalData.SlewAltRate = -DitherSpeed;
                GlobalData.SlewAzRate = 0;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 14)
            {
                GlobalData.SlewAltRate = -DitherSpeed;
                GlobalData.SlewAzRate = 0;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 15)
            {
                GlobalData.SlewAltRate = -DitherSpeed;
                GlobalData.SlewAzRate = 0;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 16)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 17)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 18)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 19)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 20)
            {
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = DitherSpeed;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 21)
            {
                GlobalData.SlewAltRate = DitherSpeed;
                GlobalData.SlewAzRate = 0;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            if (GlobalData.DitherCount == 22)
            {
                GlobalData.SlewAltRate = DitherSpeed;
                GlobalData.SlewAzRate = 0;
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
                await Task.Delay(Dithertime);
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = $"Alt Move:  {GlobalData.SlewAltRate}";
                LabelAz.Text = $"Az Move:  {GlobalData.SlewAzRate}";
                GlobalData.SlewAltRate = 0;
                GlobalData.SlewAzRate = 0;
                // Initial stop command
                await _telescopeControl.SendSlewTelescopeAltCommandAsync();
            }
            

            // end of dithering process
            GlobalData.DitherCount++;
            if (GlobalData.DitherCount > 21)
            {
                GlobalData.DitherCount = 3;
                LabelStatus.Text = $"Dither # {GlobalData.DitherCount} : {DateTime.Now:HH:mm:ss}";
                //Console.WriteLine($"Dithering completed at {DateTime.Now:HH:mm:ss}");
            }
        }

        private void ButtonStart_Click(object sender, EventArgs e)
        {
            //TrackBarTime.Locked = true; // Lock the trackbar to prevent changes during dithering
            LabelStatus.Text = "Dithering Started";
            GlobalData.DitherButton++;
            if (GlobalData.DitherButton == 1)
            {
                ButtonStart.Text = "Stop Dithering";
                _ditheringTimer.Start(); // Start the timer
                //Console.WriteLine("Dithering timer started.");
            }
            else if (GlobalData.DitherButton == 2)
            {
                GlobalData.DitherButton = 0;
                ButtonStart.Text = "Start Dithering";
                _ditheringTimer.Stop(); // Stop the timer
                LabelStatus.Text = $"Stopped Dither at : {DateTime.Now:HH:mm:ss}";
                LabelAlt.Text = "";
                LabelAz.Text = "";
                //Console.WriteLine("Dithering timer stopped.");
               // TrackBarTime.Locked = false; // Unlock the trackbar
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            // Clean up the timer when the form closes
            _ditheringTimer.Dispose();
            base.OnFormClosing(e);
        }

        private void OLVDithering_Load(object sender, EventArgs e)
        {
            int Dithertime = TrackBarTime.Value * 100;
            LabelDitherTime.Text = $"Dither Time: {Dithertime} ms";
            LabelSpeed.Text = $"Dither Speed: {TrackBarSpeed.Value} ";
        }
    }
}