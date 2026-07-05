using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OriginLiveView
{
    public partial class SchedulePick : Form
    {
        private readonly TelescopeControl _telescopeControl;
        public SchedulePick(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ScheduleImage scheduleImage = new ScheduleImage(_telescopeControl);
            scheduleImage.Show();
            this.Close(); // Close the current form
        }

        private void button2_Click(object sender, EventArgs e)
        {
            OLVMosaic olvMosaicMode = new OLVMosaic(_telescopeControl);
            olvMosaicMode.Show();
            this.Close(); // Close the current form
        }

        private void SchedulePick_Load(object sender, EventArgs e)
        {

        }
    }
}