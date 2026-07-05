using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OriginLiveView
{
    public partial class DewHeater : Form
    {
        private readonly TelescopeControl _telescopeControl;

        public DewHeater(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));

            // Subscribe to ValueChanged events for both TrackBars
            TrackBarPower.ValueChanged += TrackBarPower_ValueChanged;
            TrackBarAgressiveness.ValueChanged += TrackBarAgressiveness_ValueChanged;
        }

        private void TrackBarPower_ValueChanged(object sender, EventArgs e)
        {
            // Update LabelPower with the scaled TrackBarPower value (divided by 100.0)
            LabelPower.Text = $"Power Level = {(TrackBarPower.Value):F2} ";
        }

        private void TrackBarAgressiveness_ValueChanged(object sender, EventArgs e)
        {
            // Update LabelAgressiveness with the raw TrackBarAgressiveness value
            LabelAgressivness.Text = $"Agressiveness = {(TrackBarAgressiveness.Value):F2} "; // e.g., "5"
        }

        private async void ButtonAuto_Click(object sender, EventArgs e)
        {
            GlobalData.DewAgressiveness = TrackBarAgressiveness.Value; // Set aggressiveness to 5
            await _telescopeControl.SendDewHeaterAutoCommandAsync();
            var logger = new CommandLogger();
            logger.LogCommand($"DewHeater Auto {GlobalData.DewAgressiveness}");
        }

        private async void ButtonPower_Click(object sender, EventArgs e)
        {
            GlobalData.DewPower = TrackBarPower.Value / 100.0; // Set power to 0.10
            await _telescopeControl.SendDewHeaterManualCommandAsync();
            var logger = new CommandLogger();
            logger.LogCommand($"DewHeater Manual {GlobalData.DewPower}");
        }
    }
}