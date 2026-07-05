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
    public partial class ELPanel : Form
    {
        private readonly TelescopeControl _telescopeControl;
        public ELPanel(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl;
        }

        private void ELPanel_Load(object sender, EventArgs e)
        {

        }

        private async void ButtonConnect_Click(object sender, EventArgs e)
        {
            await _telescopeControl.SendELPanelConnectCommandAsync();

            await _telescopeControl.SendELPanelLightOnCommandAsync();

        }

        private async void ButtonDisconnect_Click(object sender, EventArgs e)
        {
            await _telescopeControl.SendELPanelLightOffCommandAsync();
            
            await _telescopeControl.SendELPanelDisconnectCommandAsync();
            
        }

        private async void ButtonSetLevel_Click(object sender, EventArgs e)
        {
            try
            {
                if (TextBoxLightLevel != null && !string.IsNullOrWhiteSpace(TextBoxLightLevel.Text))
                {
                    if (int.TryParse(TextBoxLightLevel.Text, out int lightLevel))
                    {
                        // Assuming light level should be non-negative and within a valid range (e.g., 0-100)
                        if (lightLevel >= 0 && lightLevel <= 100) // Adjust range as needed
                        {
                            GlobalData.ElPanelLightLevel = lightLevel;
                        }
                        else
                        {
                            // Handle out-of-range input (e.g., show error to user)
                            MessageBox.Show("Light level must be between 0 and 100.");
                        }
                    }
                    else
                    {
                        // Handle non-numeric input
                        MessageBox.Show("Please enter a valid number for the light level.");
                    }
                }
                else
                {
                    // Handle empty or null textbox
                    MessageBox.Show("Light level cannot be empty.");
                }
            }
            catch (Exception ex)
            {
                // Catch unexpected errors
                MessageBox.Show($"An error occurred: {ex.Message}");
            }
            await _telescopeControl.SendELPanelSetLightLevelCommandAsync();
        }
    }
}
