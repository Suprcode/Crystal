using ClientPackets;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Systems
{
    public partial class PositionMoveInfoForm : Form
    {
        public PositionMoveInfoForm()
        {
            InitializeComponent();
        }

        private void SaveButton_Click(object sender, EventArgs e)
        {
            if (uint.TryParse(PositionMoveCost.Text, out uint newCost)) // Ensure it's a valid uint
            {
                Settings.PositionMoveCost = newCost; // Update the setting
                SaveSettings(); // Call method to save the updated value
                MessageBox.Show("Position Move Cost updated!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Invalid input! Please enter a valid number.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void PositionMoveInfoForm_Load(object sender, EventArgs e)
        {
            PositionMoveCost.Text = Settings.PositionMoveCost.ToString();
        }
        public static void SaveSettings()
        {
            // Example: Save to a configuration file (adjust as needed)
            File.WriteAllText("Settings.ini", $"PositionMoveCost={Settings.PositionMoveCost}");
        }
    }
}
