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

        public bool FishingChanged = false, MailChanged = false, GoodsChanged = false, RefineChanged = false, MarriageChanged = false;

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
            UpdateMail();
            UpdateGoods();
            UpdateMarriage();
        }

        #region Update

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

        private void UpdateMail()
        {
            MailAutoSendGoldCheckbox.Checked = Settings.MailAutoSendGold;
            MailAutoSendItemsCheckbox.Checked = Settings.MailAutoSendItems;
            MailFreeWithStampCheckbox.Checked = Settings.MailFreeWithStamp;
            MailCostPer1kTextBox.Text = Settings.MailCostPer1KGold.ToString();
            MailInsurancePercentageTextBox.Text = Settings.MailItemInsurancePercentage.ToString();
        }

        private void UpdateGoods()
        {
            GoodsOnCheckBox.Checked = Settings.GoodsOn;
            GoodsMaxStoredTextBox.Text = Settings.GoodsMaxStored.ToString();
            GoodsBuyBackTimeTextBox.Text = Settings.GoodsBuyBackTime.ToString();
            GoodsBuyBackMaxStoredTextBox.Text = Settings.GoodsBuyBackMaxStored.ToString();
        }

        private void UpdateMarriage()
        {
            LoverRecall_checkbox.Checked = Settings.WeddingRingRecall;
            LoverBonusEXP_textbox.Text = Settings.LoverEXPBonus.ToString();
            MarriageCooldown_textbox.Text = Settings.MarriageCooldown.ToString();
            RequiredLevel_textbox.Text = Settings.MarriageLevelRequired.ToString();

        }

        #endregion

        private void SystemInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (FishingChanged)
                Settings.SaveFishing();

            if (MailChanged)
                Settings.SaveMail();

            if (GoodsChanged)
                Settings.SaveGoods();

            if (RefineChanged)
                Settings.SaveRefine();

            if (MarriageChanged)
                Settings.SaveMarriage();
        }

        #region Fishing

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

        #endregion

        #region Mail

        private void MailAutoSendGoldCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            Settings.MailAutoSendGold = MailAutoSendGoldCheckbox.Checked;
            MailChanged = true;
        }

        private void MailAutoSendItemsCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            Settings.MailAutoSendItems = MailAutoSendItemsCheckbox.Checked;
            MailChanged = true;
        }

        private void MailFreeWithStampCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            Settings.MailFreeWithStamp = MailFreeWithStampCheckbox.Checked;
            MailChanged = true;
        }

        private void MailCostPer1kTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp) || temp > 1000)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.MailCostPer1KGold = temp;
            MailChanged = true;
        }

        private void MailInsurancePercentageTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp) || temp > 100 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.MailItemInsurancePercentage = temp;
            MailChanged = true;
        }

        private void LoverRecall_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.WeddingRingRecall = LoverRecall_checkbox.Checked;
            MarriageChanged = true;
        }

        private void LoverBonusEXP_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp > 500 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.LoverEXPBonus = temp;
            MarriageChanged = true;
        }

        private void MarriageCooldown_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp > 365 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.MarriageCooldown = temp;
            MarriageChanged = true;
        }

        private void RequiredLevel_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp > 255 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.MarriageLevelRequired = temp;
            MarriageChanged = true;
        }

        #endregion

        #region Goods

        private void GoodsOnCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            Settings.GoodsOn = GoodsOnCheckBox.Checked;
            GoodsChanged = true;
        }

        private void GoodsMaxStoredTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp) || temp > 500 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.GoodsMaxStored = temp;
            GoodsChanged = true;
        }

        private void GoodsBuyBackTimeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp) || temp > 1440 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.GoodsBuyBackTime = temp;
            GoodsChanged = true;
        }

        private void GoodsBuyBackMaxStoredTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            uint temp;

            if (!uint.TryParse(ActiveControl.Text, out temp) || temp > 500 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.GoodsBuyBackMaxStored = temp;
            GoodsChanged = true;
        }

        #endregion
    }
}
