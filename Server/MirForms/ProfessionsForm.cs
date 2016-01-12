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
    public partial class ProfessionsForm : Form
    {
        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        public bool MinesChanged = false;
        public bool FishingChanged = false;



        public ProfessionsForm()
        {
            InitializeComponent();

            for (int i = 0; i < Settings.MineSetList.Count; i++)
                Mines_lb.Items.Add(new ListItem(Settings.MineSetList[i].Name, (i + 1).ToString()));
            //MineIndexcomboBox.Items.Add(i+1);

            UpdateMines();
            UpdateFishing();
        }

        private void UpdateFishing()
        {
            FishingAttemptsTextBox.Text = Settings.FishingAttempts.ToString();
            FishingSuccessRateStartTextBox.Text = Settings.FishingSuccessStart.ToString();
            FishingSuccessRateMultiplierTextBox.Text = Settings.FishingSuccessMultiplier.ToString();
            FishingDelayTextBox.Text = Settings.FishingDelay.ToString();
            MonsterSpawnChanceTextBox.Text = Settings.FishingMobSpawnChance.ToString();

            FishingMobIndexComboBox.Items.Clear();
            for (int i = 0; i < Envir.MonsterInfoList.Count; i++)
            {
                FishingMobIndexComboBox.Items.Add(Envir.MonsterInfoList[i]);
            }

            MirDatabase.MonsterInfo fishingMob = Envir.GetMonsterInfo(Settings.FishingMonster);

            if (fishingMob != null)
                FishingMobIndexComboBox.SelectedIndex = Envir.GetMonsterInfo(Settings.FishingMonster).Index - 1;
        }

        private void UpdateMines()
        {
            if (Mines_lb.SelectedItem == null)
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
                if (Mines_lb.SelectedIndex >= Settings.MineSetList.Count)
                {
                    Mines_lb.SelectedItem = null;
                    UpdateMines();
                    return;
                }
                MineNametextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].Name.ToString();
                MineRegenDelaytextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].SpotRegenRate.ToString();
                MineAttemptstextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].MaxStones.ToString();
                MineHitRatetextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].HitRate.ToString();
                MineDropRatetextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].DropRate.ToString();
                MineSlotstextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].TotalSlots.ToString();
                if (MineDropsIndexcomboBox.SelectedIndex >= Settings.MineSetList[Mines_lb.SelectedIndex].Drops.Count) MineDropsIndexcomboBox.SelectedItem = null;
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
                    MineItemNametextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].ItemName;
                    MineMinSlottextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MinSlot.ToString();
                    MineMaxSlottextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxSlot.ToString();
                    MineMinQualitytextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MinDura.ToString();
                    MineMaxQualitytextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxDura.ToString();
                    MineBonusChancetextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].BonusChance.ToString();
                    MineMaxBonustextBox.Text = Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxBonusDura.ToString();
                }

            }
        }

        private void ProfessionsForm_Load(object sender, EventArgs e)
        {

        }

        private void MineRemoveIndexbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MessageBox.Show("Are you sure you want to delete the last index?", "Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            MinesChanged = true;
            Mines_lb.Items.RemoveAt(Settings.MineSetList.Count - 1);
            Settings.MineSetList.RemoveAt(Settings.MineSetList.Count - 1);
            UpdateMines();
        }

        private void MineAddIndexbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            MinesChanged = true;
            Settings.MineSetList.Add(new MineSet());
            //MineIndexcomboBox.Items.Add(Settings.MineSetList.Count);
            Mines_lb.Items.Add(new ListItem(String.Empty, Settings.MineSetList.Count.ToString()));
            Mines_lb.SelectedIndex = Settings.MineSetList.Count - 1;
            MineDropsIndexcomboBox.Items.Clear();
            UpdateMines();
        }

        private void Mines_lb_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            MineDropsIndexcomboBox.Items.Clear();
            if (Mines_lb.SelectedIndex < Settings.MineSetList.Count)
            {
                for (int i = 0; i < Settings.MineSetList[Mines_lb.SelectedIndex].Drops.Count; i++)
                    MineDropsIndexcomboBox.Items.Add(i);
            }
            UpdateMines();
        }

        private void MineNametextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            string temp = ActiveControl.Text;

            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].Name = temp;

            Mines_lb.Refresh();
        }

        private void MineRegenDelaytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].SpotRegenRate = temp;
        }

        private void MineAttemptstextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].MaxStones = temp;
        }

        private void MineSlotstextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].TotalSlots = temp;
        }

        private void MineHitRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].HitRate = temp;
        }

        private void groupBox7_Enter(object sender, EventArgs e)
        {

        }

        private void MineDropRatetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].DropRate = temp;
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
            Settings.MineSetList[Mines_lb.SelectedIndex].Drops.Add(new MineDrop());
            MineDropsIndexcomboBox.Items.Add(Settings.MineSetList[Mines_lb.SelectedIndex].Drops.Count - 1);
            MineDropsIndexcomboBox.SelectedIndex = Settings.MineSetList[Mines_lb.SelectedIndex].Drops.Count - 1;
        }

        private void MineRemoveDropbutton_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (MessageBox.Show("Are you sure you want to delete the last index?", "Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            MinesChanged = true;
            MineDropsIndexcomboBox.Items.Remove(Settings.MineSetList[Mines_lb.SelectedIndex].Drops.Count - 1);
            Settings.MineSetList[Mines_lb.SelectedIndex].Drops.RemoveAt(Settings.MineSetList[Mines_lb.SelectedIndex].Drops.Count - 1);
            UpdateMines();
        }

        private void MineItemNametextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
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
            Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].ItemName = temp;
        }

        private void MineMinSlottextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MinSlot = temp;
        }

        private void MineMaxSlottextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxSlot = temp;
        }

        private void MineMinQualitytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MinDura = temp;
        }

        private void MineMaxQualitytextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxDura = temp;
        }

        private void MineBonusChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].BonusChance = temp;
        }

        private void MineMaxBonustextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (Mines_lb.SelectedItem == null) return;
            if (MineDropsIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            MinesChanged = true;
            Settings.MineSetList[Mines_lb.SelectedIndex].Drops[MineDropsIndexcomboBox.SelectedIndex].MaxBonusDura = temp;
        }

        private void ProfessionsForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (MinesChanged)
                Settings.SaveMines();

            if (FishingChanged)
                Settings.SaveFishing();
        }

        private void FishingAttemptsTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp > 100 || temp < 10)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.FishingAttempts = temp;
            FishingChanged = true;
        }

        private void FishingSuccessRateStartTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp > 100 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.FishingSuccessStart = temp;
            FishingChanged = true;
        }

        private void FishingSuccessRateMultiplierTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp > 100 || temp < 1)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.FishingSuccessMultiplier = temp;
            FishingChanged = true;
        }

        private void FishingDelayTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            long temp;

            if (!long.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.FishingDelay = temp;
            FishingChanged = true;
        }

        private void FishingMobIndexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            MirDatabase.MonsterInfo mob = Envir.MonsterInfoList[FishingMobIndexComboBox.SelectedIndex];

            if (mob == null) return;

            Settings.FishingMonster = mob.Name;

            FishingChanged = true;
        }

        private void MonsterSpawnChanceTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.FishingMobSpawnChance = temp;
            FishingChanged = true;
        }
    }
}
