using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Web.UI.WebControls;
using System.Windows.Forms;
using Server.MirEnvir;

namespace Server
{
    public partial class GuildInfoForm : Form
    {
        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        public bool GuildsChanged = false;

        public GuildInfoForm()
        {
            InitializeComponent();

            for (int i = 0; i < Settings.Guild_ExperienceList.Count; i++)
                GuildLevelListcomboBox.Items.Add(i);
            for (int i = 0; i < Settings.Guild_CreationCostList.Count; i++)
                GuildCreateListcomboBox.Items.Add(i);
            for (int i = 0; i < Settings.Guild_BuffList.Count; i++)
                GuildBuffListcomboBox.Items.Add(i);
            GuildItemNamecomboBox.Items.Clear();
            GuildItemNamecomboBox.Items.Add("");
            for (int i = 0; i < Envir.ItemInfoList.Count; i++)
            {
                GuildItemNamecomboBox.Items.Add(Envir.ItemInfoList[i]);
            }

            UpdateGuildInterface();
        }

        private void GuildInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (GuildsChanged)
                Settings.SaveGuildSettings();
        }

        private void UpdateGuildInterface()
        {
            GuildMinOwnerLeveltextBox.Text = Settings.Guild_RequiredLevel.ToString();
            GuildPPLtextBox.Text = Settings.Guild_PointPerLevel.ToString();
            GuildExpratetextBox.Text = Settings.Guild_ExpRate.ToString();
            WarLengthTextBox.Text = Settings.Guild_WarTime.ToString();
            WarCostTextBox.Text = Settings.Guild_WarCost.ToString();

            if ((GuildLevelListcomboBox.SelectedItem == null) || (GuildLevelListcomboBox.SelectedIndex >= Settings.Guild_ExperienceList.Count) || (GuildLevelListcomboBox.SelectedIndex >= Settings.Guild_MembercapList.Count))
            {
                GuildExpNeededtextBox.Text = string.Empty;
                GuildMemberCaptextBox.Text = string.Empty;
            }
            else
            {
                GuildExpNeededtextBox.Text = Settings.Guild_ExperienceList[GuildLevelListcomboBox.SelectedIndex].ToString();
                GuildMemberCaptextBox.Text = Settings.Guild_MembercapList[GuildLevelListcomboBox.SelectedIndex].ToString();
            }
            if ((GuildCreateListcomboBox.SelectedItem == null) || (GuildCreateListcomboBox.SelectedIndex >= Settings.Guild_CreationCostList.Count))
            {
                GuildItemNamecomboBox.SelectedIndex = 0;
                GuildAmounttextBox.Text = string.Empty;
            }
            else
            {
                if (Settings.Guild_CreationCostList[GuildCreateListcomboBox.SelectedIndex].Item == null)
                    GuildItemNamecomboBox.SelectedIndex = 0;
                else
                    GuildItemNamecomboBox.SelectedIndex = Settings.Guild_CreationCostList[GuildCreateListcomboBox.SelectedIndex].Item.Index;
                GuildAmounttextBox.Text = Settings.Guild_CreationCostList[GuildCreateListcomboBox.SelectedIndex].Amount.ToString();
            }
            if ((GuildBuffListcomboBox.SelectedItem == null) || (GuildBuffListcomboBox.SelectedIndex >= Settings.Guild_BuffList.Count))
            {
                GuildRequiredPointstextBox.Text = string.Empty;
                GuildMinGuildLeveltextBox.Text = string.Empty;
                GuildRunTimetextBox.Text = string.Empty;
                GuildCosttextBox.Text = string.Empty;
            }
            else
            {
                GuildRequiredPointstextBox.Text = Settings.Guild_BuffList[GuildBuffListcomboBox.SelectedIndex].PointsNeeded.ToString();
                GuildMinGuildLeveltextBox.Text = Settings.Guild_BuffList[GuildBuffListcomboBox.SelectedIndex].MinimumLevel.ToString();
                GuildRunTimetextBox.Text = Settings.Guild_BuffList[GuildBuffListcomboBox.SelectedIndex].RunTime.ToString();
                GuildCosttextBox.Text = Settings.Guild_BuffList[GuildBuffListcomboBox.SelectedIndex].Cost.ToString();

            }
        }

        private void GuildMinOwnerLeveltextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_RequiredLevel = temp;
            GuildsChanged = true;
        }

        private void GuildPPLtextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_PointPerLevel = temp;
            GuildsChanged = true;
        }

        private void GuildExpratetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_ExpRate = (float)temp / 100;
            GuildsChanged = true;
        }

        private void GuildCreateListcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            UpdateGuildInterface();
        }

        private void GuildAddCreatItembutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.Guild_CreationCostList.Add(new ItemVolume());
            GuildCreateListcomboBox.Items.Add(Settings.Guild_CreationCostList.Count - 1);
            GuildCreateListcomboBox.SelectedIndex = Settings.Guild_CreationCostList.Count - 1;
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildDeleteCreateItembutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MessageBox.Show("Are you sure you want to delete the last index?", "Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            GuildCreateListcomboBox.Items.RemoveAt(Settings.Guild_CreationCostList.Count - 1);
            Settings.Guild_CreationCostList.RemoveAt(Settings.Guild_CreationCostList.Count - 1);
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildItemNamecomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (GuildItemNamecomboBox.SelectedIndex == 0)
            {
                Settings.Guild_CreationCostList[GuildCreateListcomboBox.SelectedIndex].Item = null;
                Settings.Guild_CreationCostList[GuildCreateListcomboBox.SelectedIndex].ItemName = "";
            }
            else
            {
                Settings.Guild_CreationCostList[GuildCreateListcomboBox.SelectedIndex].Item = (ItemInfo)GuildItemNamecomboBox.SelectedItem;
                Settings.Guild_CreationCostList[GuildCreateListcomboBox.SelectedIndex].ItemName = Settings.Guild_CreationCostList[GuildCreateListcomboBox.SelectedIndex].Item.Name;
            }
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildAmounttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (GuildLevelListcomboBox.SelectedItem == null) return;
            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_CreationCostList[GuildCreateListcomboBox.SelectedIndex].Amount = temp;
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildLevelListcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            UpdateGuildInterface();
        }

        private void GuildAddLevelbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.Guild_ExperienceList.Add(0);
            Settings.Guild_MembercapList.Add(0);
            GuildLevelListcomboBox.Items.Add(Settings.Guild_ExperienceList.Count - 1);
            GuildLevelListcomboBox.SelectedIndex = Settings.Guild_ExperienceList.Count - 1;
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildDeleteLevelbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MessageBox.Show("Are you sure you want to delete the last index?", "Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            GuildLevelListcomboBox.Items.RemoveAt(Settings.Guild_ExperienceList.Count - 1);
            Settings.Guild_ExperienceList.RemoveAt(Settings.Guild_ExperienceList.Count - 1);
            Settings.Guild_MembercapList.RemoveAt(Settings.Guild_MembercapList.Count - 1);
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildExpNeededtextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (GuildLevelListcomboBox.SelectedItem == null) return;
            long temp;

            if (!long.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_ExperienceList[GuildLevelListcomboBox.SelectedIndex] = temp;
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildMemberCaptextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (GuildLevelListcomboBox.SelectedItem == null) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_MembercapList[GuildLevelListcomboBox.SelectedIndex] = temp;
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildBuffListcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            UpdateGuildInterface();
        }

        private void GuildAddBuffbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.Guild_BuffList.Add(new GuildBuff());
            GuildBuffListcomboBox.Items.Add(Settings.Guild_BuffList.Count - 1);
            GuildBuffListcomboBox.SelectedIndex = Settings.Guild_BuffList.Count - 1;
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildDeleteBuffbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MessageBox.Show("Are you sure you want to delete the last index?", "Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            GuildBuffListcomboBox.Items.RemoveAt(Settings.Guild_BuffList.Count - 1);
            Settings.Guild_BuffList.RemoveAt(Settings.Guild_BuffList.Count - 1);
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildRequiredPointstextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (GuildBuffListcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_BuffList[GuildBuffListcomboBox.SelectedIndex].PointsNeeded = temp;
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildMinGuildLeveltextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (GuildBuffListcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_BuffList[GuildBuffListcomboBox.SelectedIndex].MinimumLevel = temp;
            UpdateGuildInterface();
            GuildsChanged = true;

        }

        private void GuildRunTimetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (GuildBuffListcomboBox.SelectedItem == null) return;
            long temp;

            if (!long.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_BuffList[GuildBuffListcomboBox.SelectedIndex].RunTime = temp;
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void GuildCosttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (GuildBuffListcomboBox.SelectedItem == null) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_BuffList[GuildBuffListcomboBox.SelectedIndex].Cost = temp;
            UpdateGuildInterface();
            GuildsChanged = true;
        }

        private void WarLengthTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            long temp;

            if (!long.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_WarTime = temp;
            GuildsChanged = true;
        }

        private void WarCostTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_WarCost = temp;
            GuildsChanged = true;
        }
    }
}
