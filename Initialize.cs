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
    public partial class Initialize : Form
    {
        private readonly TelescopeControl _telescopeControl;

        public Initialize(TelescopeControl telescopeControl)
        {
            InitializeComponent();
            _telescopeControl = telescopeControl ?? throw new ArgumentNullException(nameof(telescopeControl));
        }

        public static double DegreesToRadians(double degrees)
        {
            return degrees * Math.PI / 180.0;
        }

        private void Initialize_Load(object sender, EventArgs e)
        {
            try
            {
                textBoxLatitude.Text = (Properties.Settings.Default.SavedLatitude * 180.0 / Math.PI).ToString();
                textBoxLongitude.Text = (Properties.Settings.Default.SavedLongitude * 180.0 / Math.PI).ToString();
                ComboBoxTimeZone.Items.AddRange(GetTimeZones());
                ComboBoxTimeZone.SelectedItem = Properties.Settings.Default.SavedTimeZone;
                if (!ComboBoxTimeZone.Items.Contains(Properties.Settings.Default.SavedTimeZone))
                    ComboBoxTimeZone.SelectedItem = "America/Guayaquil";
                ComboBoxTimeZone.DropDownStyle = ComboBoxStyle.DropDownList;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string[] GetTimeZones()
        {
            return new string[]
            {
                "Africa/Abidjan",
                "Africa/Algiers",
                // ... (full 297 time zones as provided previously)
                "Pacific/Tongatapu"
            };
        }

        private async void button1_Click(object sender, EventArgs e)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(textBoxLatitude.Text) ||
                    string.IsNullOrWhiteSpace(textBoxLongitude.Text) ||
                    ComboBoxTimeZone.SelectedItem == null)
                {
                    MessageBox.Show("All fields must be filled.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxLatitude.Text, out double newLatitude) || newLatitude < -90 || newLatitude > 90)
                {
                    MessageBox.Show("Latitude must be a number between -90 and 90 degrees.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (!double.TryParse(textBoxLongitude.Text, out double longitude) || longitude < -180 || longitude > 180)
                {
                    MessageBox.Show("Longitude must be a number between -180 and 180 degrees.", "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                double latRadians = DegreesToRadians(newLatitude);
                double lonRadians = DegreesToRadians(longitude);

                GlobalData.Latitude = latRadians;
                GlobalData.Longitude = lonRadians;
                GlobalData.TimeZone = ComboBoxTimeZone.SelectedItem.ToString();
                GlobalData.FakeInitialize = CheckBoxFakeInit.Checked;

                // Save settings if CheckBoxSave is checked
                if (CheckBoxSave.Checked)
                {
                    try
                    {
                        Properties.Settings.Default.SavedLatitude = GlobalData.Latitude;
                        Properties.Settings.Default.SavedLongitude = GlobalData.Longitude;
                        Properties.Settings.Default.SavedTimeZone = GlobalData.TimeZone;
                        Properties.Settings.Default.Save();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Error saving settings: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                await _telescopeControl.SendInitializeCommandAsync();
                var logger = new CommandLogger();
                logger.LogCommand($"Initialize {GlobalData.Latitude}  {GlobalData.Longitude} {GlobalData.TimeZone}");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"An error occurred: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            try
            {
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error closing form: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void textBoxLatitude_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxLatitude.Text) ||
                !double.TryParse(textBoxLatitude.Text, out double lat) ||
                lat < -90 || lat > 90)
            {
                textBoxLatitude.BackColor = Color.LightPink;
            }
            else
            {
                textBoxLatitude.BackColor = SystemColors.Window;
            }
        }

        private void textBoxLongitude_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrWhiteSpace(textBoxLongitude.Text) ||
                !double.TryParse(textBoxLongitude.Text, out double lon) ||
                lon < -180 || lon > 180)
            {
                textBoxLongitude.BackColor = Color.LightPink;
            }
            else
            {
                textBoxLongitude.BackColor = SystemColors.Window;
            }
        }
    }
}