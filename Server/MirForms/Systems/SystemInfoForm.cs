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
    public partial class SystemInfoForm : Form
    {
        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        public bool FishingChanged = false;

        public SystemInfoForm()
        {
            InitializeComponent();
        }

        public SystemInfoForm(int selectedTab = 0)
        {
            InitializeComponent();

            if (selectedTab > this.tabControl1.TabCount - 1) selectedTab = 0;

            this.tabControl1.SelectedIndex = selectedTab;

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

        private void SystemInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
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

        private void FishingMobIndexComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            MirDatabase.MonsterInfo mob = Envir.MonsterInfoList[FishingMobIndexComboBox.SelectedIndex];

            if(mob == null) return;

            Settings.FishingMonster = mob.Name;

            FishingChanged = true;
        }
    }
}
