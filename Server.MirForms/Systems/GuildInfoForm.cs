using Server.MirEnvir;
using Server.MirObjects;

namespace Server
{
    public partial class GuildInfoForm : Form
    {
        public Envir Envir => SMain.EditEnvir;

        public bool GuildsChanged = false;
        GuildBuffInfo SelectedBuff;

        public GuildInfoForm()
        {
            InitializeComponent();

            for (int i = 0; i < Settings.Guild_ExperienceList.Count; i++)
                GuildLevelListcomboBox.Items.Add(i);
            for (int i = 0; i < Settings.Guild_CreationCostList.Count; i++)
                GuildCreateListcomboBox.Items.Add(i);
            GuildItemNamecomboBox.Items.Clear();
            GuildItemNamecomboBox.Items.Add("");
            for (int i = 0; i < Envir.ItemInfoList.Count; i++)
            {
                GuildItemNamecomboBox.Items.Add(Envir.ItemInfoList[i]);
            }
            for (int i = 0; i < Settings.Guild_BuffList.Count; i++)
                BuffList.Items.Add(Settings.Guild_BuffList[i]);
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
            if (BuffList.SelectedItem == null)
            {
                SelectedBuff = null;
                BuffPanel.Enabled = false;
                BufflblIndex.Text = "No Buff Selected";
                BufftxtName.Text = string.Empty;
                BuffTxtLevelReq.Text = string.Empty;
                BufftxtPointsReq.Text = string.Empty;
                BufftxtTimeLimit.Text = string.Empty;
                BufftxtActivationCost.Text = string.Empty;
                bufftxtIcon.Text = string.Empty;
                BufftxtAc.Text = string.Empty;
                BufftxtMac.Text = string.Empty;
                BufftxtDc.Text = string.Empty;
                BufftxtMc.Text = string.Empty;
                BufftxtSc.Text = string.Empty;
                BufftxtAttack.Text = string.Empty;
                BufftxtHpRegen.Text = string.Empty;
                BufftxtMpRegen.Text = string.Empty;
                BufftxtMaxHp.Text = string.Empty;
                BufftxtMaxMp.Text = string.Empty;
                BufftxtMineRate.Text = string.Empty;
                BufftxtGemRate.Text = string.Empty;
                BufftxtFishRate.Text = string.Empty;
                BufftxtExpRate.Text = string.Empty;
                BufftxtCraftRate.Text = string.Empty;
                BufftxtSkillRate.Text = string.Empty;
                BufftxtDropRate.Text = string.Empty;
                BufftxtGoldRate.Text = string.Empty;
            }
            else
            {
                SelectedBuff  = (GuildBuffInfo)BuffList.SelectedItem;
                BuffPanel.Enabled = true;
                BufflblIndex.Text = $"Index:  {SelectedBuff.Id}";
                BufftxtName.Text = SelectedBuff.Name;
                BuffTxtLevelReq.Text = SelectedBuff.LevelRequirement.ToString();
                BufftxtPointsReq.Text = SelectedBuff.PointsRequirement.ToString();
                BufftxtTimeLimit.Text = SelectedBuff.TimeLimit.ToString();
                BufftxtActivationCost.Text = SelectedBuff.ActivationCost.ToString();
                bufftxtIcon.Text = SelectedBuff.Icon.ToString();
                BufftxtAc.Text = SelectedBuff.Stats[Stat.MaxAC].ToString();
                BufftxtMac.Text = SelectedBuff.Stats[Stat.MaxMAC].ToString();
                BufftxtDc.Text = SelectedBuff.Stats[Stat.MaxDC].ToString();
                BufftxtMc.Text = SelectedBuff.Stats[Stat.MaxMC].ToString();
                BufftxtSc.Text = SelectedBuff.Stats[Stat.MaxSC].ToString();
                BufftxtAttack.Text = SelectedBuff.Stats[Stat.AttackBonus].ToString();
                BufftxtHpRegen.Text = SelectedBuff.Stats[Stat.HealthRecovery].ToString();
                BufftxtMpRegen.Text = SelectedBuff.Stats[Stat.SpellRecovery].ToString();
                BufftxtMaxHp.Text = SelectedBuff.Stats[Stat.HP].ToString();
                BufftxtMaxMp.Text = SelectedBuff.Stats[Stat.MP].ToString();
                BufftxtMineRate.Text = SelectedBuff.Stats[Stat.MineRatePercent].ToString();
                BufftxtGemRate.Text = SelectedBuff.Stats[Stat.GemRatePercent].ToString();
                BufftxtFishRate.Text = SelectedBuff.Stats[Stat.FishRatePercent].ToString();
                BufftxtExpRate.Text = SelectedBuff.Stats[Stat.ExpRatePercent].ToString();
                BufftxtCraftRate.Text = SelectedBuff.Stats[Stat.CraftRatePercent].ToString();
                BufftxtSkillRate.Text = SelectedBuff.Stats[Stat.SkillGainMultiplier].ToString();
                BufftxtDropRate.Text = SelectedBuff.Stats[Stat.ItemDropRatePercent].ToString();
                BufftxtGoldRate.Text = SelectedBuff.Stats[Stat.GoldDropRatePercent].ToString();
            }
        }

        private void GuildMinOwnerLeveltextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            if (!byte.TryParse(ActiveControl.Text, out byte temp))
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

            if (!byte.TryParse(ActiveControl.Text, out byte temp))
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

            if (!byte.TryParse(ActiveControl.Text, out byte temp))
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
            Settings.Guild_CreationCostList.Add(new GuildItemVolume());
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
            //if (GuildLevelListcomboBox.SelectedItem == null) return;

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

            if (!uint.TryParse(ActiveControl.Text, out uint temp))
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

            if (!long.TryParse(ActiveControl.Text, out long temp))
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

            if (!int.TryParse(ActiveControl.Text, out int temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_MembercapList[GuildLevelListcomboBox.SelectedIndex] = temp;
            UpdateGuildInterface();
            GuildsChanged = true;
        }


        private void WarLengthTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            if (!long.TryParse(ActiveControl.Text, out long temp))
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

            if (!uint.TryParse(ActiveControl.Text, out uint temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.Guild_WarCost = temp;
            GuildsChanged = true;
        }

        private bool IsValid(ref byte input, object sender)
        {
            if (ActiveControl != sender) return false;
            if (SelectedBuff == null) return false;
            if (!byte.TryParse(ActiveControl.Text, out input))
            {
                ActiveControl.BackColor = Color.Red;
                return false;
            }
            return true;
        }

        private bool IsValid(ref uint input, object sender)
        {
            if (ActiveControl != sender) return false;
            if (SelectedBuff == null) return false;
            if (!uint.TryParse(ActiveControl.Text, out input))
            {
                ActiveControl.BackColor = Color.Red;
                return false;
            }
            return true;
        }

        private bool IsValid(ref int input, object sender)
        {
            if (ActiveControl != sender) return false;
            if (SelectedBuff == null) return false;
            if (!int.TryParse(ActiveControl.Text, out input))
            {
                ActiveControl.BackColor = Color.Red;
                return false;
            }
            return true;
        }

        private void BuffAdd_Click(object sender, EventArgs e)
        {
            int Index = 0;
            for (int i = 0; i < Settings.Guild_BuffList.Count; i++)
                if (Index < Settings.Guild_BuffList[i].Id)
                    Index = Settings.Guild_BuffList[i].Id;

            GuildBuffInfo NewBuff = new GuildBuffInfo
            {
                Id = ++Index,
                Name = "Buff " + Index.ToString()
            };

            Settings.Guild_BuffList.Add(NewBuff);
            BuffList.Items.Add(NewBuff);
            GuildsChanged = true;   
        }

        private void BuffDelete_Click(object sender, EventArgs e)
        {
            if (BuffList.SelectedItem == null) return;

            if (MessageBox.Show("Are you sure you want to remove the selected guildbuff?", "Remove guildbuff?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            Settings.Guild_BuffList.Remove((GuildBuffInfo)BuffList.SelectedItem);
            BuffList.Items.RemoveAt(BuffList.SelectedIndex);
            GuildsChanged = true;
            UpdateGuildInterface();
        }

        private void BufftxtName_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (SelectedBuff == null) return;
            if ((ActiveControl.Text == "") || (ActiveControl.Text.Length > 20))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Name = ActiveControl.Text;
            GuildsChanged = true;
        }

        private void BuffList_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateGuildInterface();
        }

        private void BuffTxtLevelReq_TextChanged(object sender, EventArgs e)
        {  
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.LevelRequirement = temp;
            GuildsChanged = true;
        }

        private void BufftxtPointsReq_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.PointsRequirement = temp;
            GuildsChanged = true;
        }

        private void BufftxtTimeLimit_TextChanged(object sender, EventArgs e)
        {
            uint temp2 = 0;
            int temp = 0;
            if (!IsValid(ref temp2, sender) || !IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.TimeLimit = temp;
            GuildsChanged = true;
        }

        private void BufftxtActivationCost_TextChanged(object sender, EventArgs e)
        {
            uint temp2 = 0;
            int temp = 0;
            if (!IsValid(ref temp2, sender) || !IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.ActivationCost = temp;
            GuildsChanged = true;
        }

        private void BufftxtAc_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.MaxAC] = temp;
            GuildsChanged = true;
        }

        private void BufftxtMac_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.MaxMAC] = temp;
            GuildsChanged = true;
        }

        private void BufftxtDc_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.MaxDC] = temp;
            GuildsChanged = true;
        }

        private void BufftxtMc_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.MaxMC] = temp;
            GuildsChanged = true;
        }

        private void BufftxtSc_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.MaxSC] = temp;
            GuildsChanged = true;
        }

        private void BufftxtAttack_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.AttackBonus] = temp;
            GuildsChanged = true;
        }

        private void BufftxtMaxHp_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.HP] = temp;
            GuildsChanged = true;
        }

        private void BufftxtMaxMp_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.MP] = temp;
            GuildsChanged = true;
        }

        private void BufftxtHpRegen_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.HealthRecovery] = temp;
            GuildsChanged = true;
        }

        private void BufftxtMpRegen_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.SpellRecovery] = temp;
            GuildsChanged = true;
        }

        private void BufftxtMineRate_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.MineRatePercent] = temp;
            GuildsChanged = true;
        }

        private void BufftxtGemRate_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.GemRatePercent] = temp;
            GuildsChanged = true;
        }

        private void BufftxtFishRate_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.FishRatePercent] = temp;
            GuildsChanged = true;
        }

        private void BufftxtExpRate_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.ExpRatePercent] = temp;
            GuildsChanged = true;
        }

        private void BufftxtCraftRate_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.CraftRatePercent] = temp;
            GuildsChanged = true;
        }

        private void BufftxtSkillRate_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.SkillGainMultiplier] = temp;
            GuildsChanged = true;
        }

        private void BufftxtDropRate_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.ItemDropRatePercent] = temp;
            GuildsChanged = true;
        }

        private void BufftxtGoldRate_TextChanged(object sender, EventArgs e)
        {
            byte temp = 0;
            if (!IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Stats[Stat.GoldDropRatePercent] = temp;
            GuildsChanged = true;
        }

        private void bufftxtIcon_TextChanged(object sender, EventArgs e)
        {
            uint temp2 = 0;
            int temp = 0;
            if (!IsValid(ref temp2, sender) || !IsValid(ref temp, sender)) return;
            ActiveControl.BackColor = SystemColors.Window;
            SelectedBuff.Icon = temp;
            GuildsChanged = true;
        }
    }
}
