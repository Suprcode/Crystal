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

        public bool FishingChanged = false, MailChanged = false, GoodsChanged = false, RefineChanged = false, MarriageChanged = false, MentorChanged = false, GemChanged = false, SpawnChanged = false;

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
            UpdateRefine();
            UpdateMarriage();
            UpdateMentor();
            UpdateGem();
            UpdateSpawnTick();
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
            ReplaceRingCost_textbox.Text = Settings.ReplaceWedRingCost.ToString();

        }

        private void UpdateMentor()
        {
            MenteeSkillBoost_checkbox.Checked = Settings.MentorSkillBoost;
            MentorLevelGap_textbox.Text = Settings.MentorLevelGap.ToString();
            MentorLength_textbox.Text = Settings.MentorLength.ToString();
            MentorDamageBoost_textbox.Text = Settings.MentorDamageBoost.ToString();
            MenteeExpBoost_textbox.Text = Settings.MentorExpBoost.ToString();
            MenteeExpBank_textbox.Text = Settings.MenteeExpBank.ToString();
        }

        private void UpdateRefine()
        {
            WeaponOnly_checkbox.Checked = Settings.OnlyRefineWeapon;
            BaseChance_textbox.Text = Settings.RefineBaseChance.ToString();
            RefineTime_textbox.Text = Settings.RefineTime.ToString();
            NormalStat_textbox.Text = Settings.RefineIncrease.ToString();
            CritChance_textbox.Text = Settings.RefineCritChance.ToString();
            CritMultiplier_textbox.Text = Settings.RefineCritIncrease.ToString();
            WepDimReturn_textbox.Text = Settings.RefineWepStatReduce.ToString();
            ItemDimReturn_textbox.Text = Settings.RefineItemStatReduce.ToString();
            RefineCost_textbox.Text = Settings.RefineCost.ToString();
            OreName_textbox.Text = Settings.RefineOreName.ToString();
        }
        private void UpdateGem()
        {
            GemStatCheckBox.Checked = Settings.GemStatIndependent;
        }

        private void UpdateSpawnTick()
        {
            txtSpawnTickDefault.Text = Envir.RespawnTick.BaseSpawnRate.ToString();
            if (lbSpawnTickList.Items.Count != Envir.RespawnTick.Respawn.Count)
            {
                lbSpawnTickList.ClearSelected();
                lbSpawnTickList.Items.Clear();
                foreach (RespawnTickOption Option in Envir.RespawnTick.Respawn)
                    lbSpawnTickList.Items.Add(Option);
                pnlSpawnTickConfig.Enabled = false;
                txtSpawnTickSpeed.Text = string.Empty;
                txtSpawnTickUsers.Text = string.Empty;
            }
            else
            {
                if (lbSpawnTickList.SelectedIndex == -1)
                {
                    pnlSpawnTickConfig.Enabled = false;
                    txtSpawnTickSpeed.Text = string.Empty;
                    txtSpawnTickUsers.Text = string.Empty;
                }
                else
                {
                    pnlSpawnTickConfig.Enabled = true;
                    RespawnTickOption Option = (RespawnTickOption)lbSpawnTickList.SelectedItem;
                    txtSpawnTickSpeed.Text = string.Format("{0:0.0}", Option.DelayLoss);
                    txtSpawnTickUsers.Text = Option.UserCount.ToString();
                }
            }
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

            if (MentorChanged)
                Settings.SaveMentor();

            if (GemChanged)
                Settings.SaveGem();
            if (SpawnChanged)
                Envir.SaveDB();
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

            if (mob == null) return;

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

        private void SystemInfoForm_Load(object sender, EventArgs e)
        {

        }

        private void WeaponOnly_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            Settings.OnlyRefineWeapon = WeaponOnly_checkbox.Checked;
            RefineChanged = true;
        }

        private void BaseChance_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 100 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.RefineBaseChance = temp;
            RefineChanged = true;
        }

        private void RefineTime_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp > 1000 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.RefineTime = temp;
            RefineChanged = true;
        }

        private void NormalStat_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 255 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.RefineIncrease = temp;
            RefineChanged = true;
        }

        private void CritChance_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 100 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.RefineCritChance = temp;
            RefineChanged = true;
        }

        private void CritMultiplier_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 255 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.RefineCritIncrease = temp;
            RefineChanged = true;
        }

        private void WepDimReturn_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 100 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.RefineWepStatReduce = temp;
            RefineChanged = true;
        }

        private void ItemDimReturn_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 100 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.RefineItemStatReduce = temp;
            RefineChanged = true;
        }

        private void RefineCost_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp > 2000 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.RefineCost = temp;
            RefineChanged = true;
        }

        private void OreName_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.RefineOreName = ActiveControl.Text;
            RefineChanged = true;
        }

        private void ReplaceRingCost_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || temp > 2000 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.ReplaceWedRingCost = temp;
            MarriageChanged = true;
        }

        private void MentorLevelGap_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 255 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.MentorLevelGap = temp;
            MentorChanged = true;
        }

        private void MentorLength_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 255 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.MentorLength = temp;
            MentorChanged = true;
        }

        private void MentorDamageBoost_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 255 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.MentorDamageBoost = temp;
            MentorChanged = true;
        }

        private void MenteeExpBoost_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 255 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.MentorExpBoost = temp;
            MentorChanged = true;
        }

        private void MenteeExpBank_textbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 255 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.MenteeExpBank = temp;
            MentorChanged = true;
        }

        private void MenteeSkillBoost_checkbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.MentorSkillBoost = MenteeSkillBoost_checkbox.Checked;
            MentorChanged = true;
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
        #region Gem
        private void GemStatCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            Settings.GemStatIndependent = GemStatCheckBox.Checked;
            GemChanged = true;
        }
        #endregion

        private void txtSpawnTickDefault_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp) || temp > 255 || temp < 0)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Envir.RespawnTick.BaseSpawnRate = temp;
            SpawnChanged = true;
        }

        private void btnSpawnTickAdd_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            RespawnTickOption Option = new RespawnTickOption();
            Envir.RespawnTick.Respawn.Add(Option);
            lbSpawnTickList.Items.Add(Option);
            lbSpawnTickList.SelectedIndex = Envir.RespawnTick.Respawn.Count - 1;
            UpdateSpawnTick();
            SpawnChanged = true;
        }

        private void btnSpawnTickRemove_Click(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (lbSpawnTickList.SelectedIndex == -1) return;
            if (MessageBox.Show("Are you sure you want to delete the index?", "Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            Envir.RespawnTick.Respawn.RemoveAt(lbSpawnTickList.SelectedIndex);
            //lbSpawnTickList.Items.RemoveAt(lbSpawnTickList.SelectedIndex);

            UpdateSpawnTick();
            SpawnChanged = true;
        }

        private void lbSpawnTickList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSpawnTick();
        }

        private void txtSpawnTickUsers_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (lbSpawnTickList.SelectedIndex == -1) return;
            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp) || (temp < 0))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Envir.RespawnTick.Respawn[lbSpawnTickList.SelectedIndex].UserCount = temp;
            lbSpawnTickList.Items[lbSpawnTickList.SelectedIndex] = lbSpawnTickList.SelectedItem;//need this to update the string displayed
            //lbSpawnTickList.Refresh();
            txtSpawnTickUsers.Focus();
            txtSpawnTickUsers.SelectionStart = txtSpawnTickUsers.Text.Length;
            SpawnChanged = true;
        }

        private void txtSpawnTickSpeed_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (lbSpawnTickList.SelectedIndex == -1) return;
            double temp;

            if (!double.TryParse(ActiveControl.Text, out temp) || (temp <= 0) || (temp > 1.0))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Envir.RespawnTick.Respawn[lbSpawnTickList.SelectedIndex].DelayLoss = temp;
            SpawnChanged = true;
        }
    }
}