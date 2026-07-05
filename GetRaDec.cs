using System;
using System.Windows.Forms;

namespace OriginLiveView
{
    public partial class GetRaDec : Form
    {
        private Timer _timer;

        public GetRaDec()
        {
            InitializeComponent();
            SetupTimer();
        }

        private void SetupTimer()
        {
            _timer = new Timer();
            _timer.Interval = 1000; // 1 second
            _timer.Tick += Timer_Tick;
            _timer.Start();
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            LabelRA.Text = double.IsNaN(GlobalData.RA) || GlobalData.RA == 0.0 ? "N/A" : $"RA: {GlobalData.RA.ToString()}";
            LabelDec.Text = double.IsNaN(GlobalData.Dec) || GlobalData.Dec == 0.0 ? "N/A" : $"Dec: {GlobalData.Dec.ToString()}";
        }

        private void GetRaDec_Load(object sender, EventArgs e)
        {
            LabelRA.Text = double.IsNaN(GlobalData.RA) || GlobalData.RA == 0.0 ? "N/A" : $"RA: {GlobalData.RA.ToString()}";
            LabelDec.Text = double.IsNaN(GlobalData.Dec) || GlobalData.Dec == 0.0 ? "N/A" : $"Dec: {GlobalData.Dec.ToString()}";
        }

        private void GetRaDec_FormClosing(object sender, FormClosingEventArgs e)
        {
            _timer?.Stop();
            _timer?.Dispose();
        }
    }

    // Assumed GlobalData class for reference
    
}