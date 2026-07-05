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
    public partial class AutoFocus : Form
    {
        private readonly TelescopeControl _telescopeControl;
        public AutoFocus(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl;
        }

        private void AutoFocus_Load(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }
       private async void buttonAutoFocus_Click(object sender, EventArgs e)
        {

            if (CheckBoxTempChange.Checked)
            {
                GlobalData.FocusonTempChange = true;
            }
            else
            {
                GlobalData.FocusonTempChange = false;
            }   
                if (CheckBoxGoto.Checked)
            {
                GlobalData.FocusonGoto = true;
            }
            else
            {
                GlobalData.FocusonGoto = false;
            }
            
            GlobalData.TemperatureChange = double.TryParse(TextBoxCelcius.Text, out double result) ? result : 0;


            
                  await _telescopeControl.SendAutoFocusTempChangeCommandAsync();
            
                  await _telescopeControl.SendAutoFocusGotoCommandAsync();
            var logger = new CommandLogger();
            logger.LogCommand($"AutoFocus  {GlobalData.FocusonGoto} {GlobalData.FocusonTempChange} {GlobalData.TemperatureChange} ");
            AutoFocus.ActiveForm?.Close();
            Close();

        }
    }
}
