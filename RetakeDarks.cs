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
    public partial class RetakeDarks : Form
    {
        private readonly TelescopeControl _telescopeControl;

        public RetakeDarks(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
        }

        private void RetakeDarks_Load(object sender, EventArgs e)
        {
            
        }

        private async void ButtonRetakeDarks_Click(object sender, EventArgs e)
        {
            try
            {
                // Validate input fields
                if (string.IsNullOrWhiteSpace(ComboBitDepth.Text) ||
                    string.IsNullOrWhiteSpace(ComboExposureTime.Text) ||
                    string.IsNullOrWhiteSpace(ComboISO.Text))
                {
                    MessageBox.Show("All fields must be filled.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate bit depth
                if (!int.TryParse(ComboBitDepth.Text, out int bitDepth) || bitDepth <= 0)
                {
                    MessageBox.Show("Bit Depth must be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Optional: Restrict bit depth to common values (e.g., 8, 12, 14, 16)
                if (bitDepth != 8 && bitDepth != 12 && bitDepth != 14 && bitDepth != 16)
                {
                    MessageBox.Show("Bit Depth must be one of: 8, 12, 14, or 16.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate exposure time
                if (!int.TryParse(ComboExposureTime.Text, out int exposureTime) || exposureTime <= 0)
                {
                    MessageBox.Show("Exposure Time must be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Validate ISO
                if (!int.TryParse(ComboISO.Text, out int iso) || iso <= 0)
                {
                    MessageBox.Show("ISO must be a positive integer.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                // Update global data
                GlobalData.CameraBitDepth = bitDepth;
                GlobalData.ExposureTime = exposureTime;
                GlobalData.ISO = iso;

                // Send retake darks command
                await _telescopeControl.SendRetakeDarksCommandAsync();
                MessageBox.Show("Dark frames retaken successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                // Close the form after successful operation
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ComboBitDepth_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add real-time validation for bit depth
            if (string.IsNullOrWhiteSpace(ComboBitDepth.Text) ||
                !int.TryParse(ComboBitDepth.Text, out int bitDepth) ||
                (bitDepth != 8 && bitDepth != 12 && bitDepth != 14 && bitDepth != 16))
            {
                ComboBitDepth.BackColor = Color.LightPink; // Highlight invalid field
            }
            else
            {
                ComboBitDepth.BackColor = SystemColors.Window; // Reset background
            }
        }

        private void ComboExposureTime_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add real-time validation for exposure time
            if (string.IsNullOrWhiteSpace(ComboExposureTime.Text) ||
                !int.TryParse(ComboExposureTime.Text, out int exposureTime) ||
                exposureTime <= 0)
            {
                ComboExposureTime.BackColor = Color.LightPink; // Highlight invalid field
            }
            else
            {
                ComboExposureTime.BackColor = SystemColors.Window; // Reset background
            }
        }

        private void ComboISO_TextChanged(object sender, EventArgs e)
        {
            // Optional: Add real-time validation for ISO
            if (string.IsNullOrWhiteSpace(ComboISO.Text) ||
                !int.TryParse(ComboISO.Text, out int iso) ||
                iso <= 0)
            {
                ComboISO.BackColor = Color.LightPink; // Highlight invalid field
            }
            else
            {
                ComboISO.BackColor = SystemColors.Window; // Reset background
            }
        }

        private void RetakeDarks_Load_1(object sender, EventArgs e)
        {

        }
    }
}
