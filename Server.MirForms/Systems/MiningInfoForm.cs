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
    public partial class MiningInfoForm : Form
    {
        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        public bool MinesChanged = false;

        public MiningInfoForm()
        {
            InitializeComponent();

            for (int i = 0; i < Settings.MineSetList.Count; i++)
                MineIndexcomboBox.Items.Add(new ListItem(Settings.MineSetList[i].Name, (i + 1).ToString()));
            //MineIndexcomboBox.Items.Add(i+1);

            UpdateMines();

        }

        private void MiningInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MinesChanged)
                Settings.SaveMines();
        }

        private void UpdateMines()
        {
            if (MineIndexcomboBox.SelectedItem == null)
            {
                MineDropsIndexcomboBox.Items.Clear();
                MineNametextBox.Text = string.Empty;
                MineRegenDelaytextBox.Text = string.Empty;
                MineAttemptstextBox.Text = string.Empty;
                MineHitRatetextBox.Text = string.Empty;
                MineDropRatetextBox.Text = string.Empty;
                MineSlotstextBox.Text = string.Empty;
                MineItemNametextBox.Text = string.Empty;
                MineMinSlottextBox.Text = string.Empty;
                MineMaxSlottextBox.Text = string.Empty;
                MineMinQualitytextBox.Text = string.Empty;
                MineMaxQualitytextBox.Text = string.Empty;
                MineBonusChancetextBox.Text = string.Empty;
                MineMaxBonustextBox.Text = string.Empty;
            }
            else
            {
                if (MineIndexcomboBox.SelectedIndex >= Settings.MineSetList.Count)
                {
                    MineIndexcomboBox.SelectedItem = null;
                    UpdateMines();
                    return;
                }
                MineNametextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Name.ToString();
                MineRegenDelaytextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].SpotRegenRate.ToString();
                MineAttemptstextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].MaxStones.ToString();
                MineHitRatetextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].HitRate.ToString();
                MineDropRatetextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].DropRate.ToString();
                MineSlotstextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].TotalSlots.ToString();
                if (MineDropsIndexcomboBox.SelectedIndex >= Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops.Count) MineDropsIndexcomboBox.SelectedItem = null;
                if (MineDropsIndexcomboBox.SelectedItem == null)
                {
                    MineItemNametextBox.Text = string.Empty;
                    MineMinSlottextBox.Text = string.Empty;
                    MineMaxSlottextBox.Text = string.Empty;
                    MineMinQualitytextBox.Text = string.Empty;
                    MineMaxQualitytextBox.Text = string.Empty;
                    MineBonusChancetextBox.Text = string.Empty;
                    MineMaxBonustextBox.Text = string.Empty;
                }
                else
                {
                    MineItemNametextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].ItemName;
                    MineMinSlottextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MinSlot.ToString();
                    MineMaxSlottextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxSlot.ToString();
                    MineMinQualitytextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MinDura.ToString();
                    MineMaxQualitytextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxDura.ToString();
                    MineBonusChancetextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].BonusChance.ToString();
                    MineMaxBonustextBox.Text = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxBonusDura.ToString();
                }

            }
        }


        #region Events

        private void MineIndexcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            MineDropsIndexcomboBox.Items.Clear();
            if (MineIndexcomboBox.SelectedIndex < Settings.MineSetList.Count)
            {
                for (int i = 0; i < Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops.Count; i++)
                    MineDropsIndexcomboBox.Items.Add(i);
            }
            UpdateMines();
        }

        private void MineAddIndexbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            MinesChanged = true;
            Settings.MineSetList.Add(new MineSet());
            //MineIndexcomboBox.Items.Add(Settings.MineSetList.Count);
            MineIndexcomboBox.Items.Add(new ListItem(String.Empty, Settings.MineSetList.Count.ToString()));
            MineIndexcomboBox.SelectedIndex = Settings.MineSetList.Count - 1;
            MineDropsIndexcomboBox.Items.Clear();
            UpdateMines();
        }

        private void MineRemoveIndexbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MessageBox.Show("Are you sure you want to delete the last index?", "Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            MinesChanged = true;
            MineIndexcomboBox.Items.RemoveAt(Settings.MineSetList.Count - 1);
            Settings.MineSetList.RemoveAt(Settings.MineSetList.Count - 1);
            UpdateMines();
        }

        private void MineRegenDelaytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].SpotRegenRate = temp;
        }

        private void MineAttemptstextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].MaxStones = temp;
        }

        private void MineSlotstextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].TotalSlots = temp;
        }

        private void MineHitRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].HitRate = temp;
        }

        private void MineDropRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].DropRate = temp;
        }

        private void MineDropsIndexcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            UpdateMines();
        }

        private void MineAddDropbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops.Add(new MineDrop());
            MineDropsIndexcomboBox.Items.Add(Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops.Count - 1);
            MineDropsIndexcomboBox.SelectedIndex = Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops.Count - 1;
        }

        private void MineRemoveDropbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MessageBox.Show("Are you sure you want to delete the last index?", "Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            MinesChanged = true;
            MineDropsIndexcomboBox.Items.Remove(Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops.Count - 1);
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops.RemoveAt(Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops.Count - 1);
            UpdateMines();
        }

        private void MineItemNametextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            string temp = ActiveControl.Text;

            ActiveControl.BackColor = Color.Red;
            for (int i = 0; i < SMain.EditEnvir.ItemInfoList.Count; i++)
            {
                if (SMain.EditEnvir.ItemInfoList[i].Name == temp)
                {
                    ActiveControl.BackColor = SystemColors.Window;
                    break;
                }
            }
            if (ActiveControl.BackColor == Color.Red)
                return;

            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].ItemName = temp;
        }

        private void MineMinSlottextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MinSlot = temp;
        }

        private void MineMaxSlottextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxSlot = temp;
        }

        private void MineMinQualitytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MinDura = temp;
        }

        private void MineMaxQualitytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxDura = temp;
        }

        private void MineBonusChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].BonusChance = temp;
        }

        private void MineMaxBonustextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxBonusDura = temp;
        }

        private void MineNametextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MineIndexcomboBox.SelectedItem == null) return;
            string temp = ActiveControl.Text;

            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[MineIndexcomboBox.SelectedIndex].Name = temp;

            MineIndexcomboBox.Refresh();
        }

        #endregion

    }
}
