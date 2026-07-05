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
    public partial class Fans : Form
    {
        private readonly TelescopeControl _telescopeControl;
        public Fans(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
         GlobalData.CPUFan = CheckBoxCPU.Checked;
         GlobalData.OTAFan = CheckBoxOTA.Checked;
         await _telescopeControl.SendFansCommandAsync();
         var logger = new CommandLogger();
         logger.LogCommand($"Fans {GlobalData.OTAFan} {GlobalData.CPUFan}");
            Fans.ActiveForm?.Close();
        }

        private void Fans_Load(object sender, EventArgs e)
        {
            CheckBoxCPU.Checked = GlobalData.CPUFan;
            CheckBoxOTA.Checked = GlobalData.OTAFan;
        }
    }
}
