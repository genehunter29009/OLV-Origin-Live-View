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
    public partial class MountSettings : Form
    {
        private readonly TelescopeControl _telescopeControl;
        public MountSettings(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl;
        }

        private async void button1_Click(object sender, EventArgs e)
        {
         GlobalData.IsEQ = CheckBoxEQ.Checked;
         GlobalData.IsWesge = CheckBoxWedge.Checked;
         GlobalData.AltBacklash = (int)numericAlt.Value;
         GlobalData.AzBacklash = (int)numericAz.Value;
            if (double.TryParse(TextBoxCustSpeed.Text, out double result))
            {
                GlobalData.MountCustSpeed = result;
            }
         GlobalData.MountCustEnabled = CheckBoxCustSpeed.Checked;
         GlobalData.MountPecEnabled = CheckBoxPEC.Checked;
         await _telescopeControl.SendMountSettingsCommandAsync();
        }

        private void MountSettings_Load(object sender, EventArgs e)
        {

        }
    }
}
