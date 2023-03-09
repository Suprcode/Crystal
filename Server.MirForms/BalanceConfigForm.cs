using Server.MirEnvir;
using Shared.Extensions;

namespace Server
{
    public partial class BalanceConfigForm : Form
    {
        public Envir Envir => SMain.EditEnvir;
        public byte SelectedClassID = 0;

        public bool BaseStatsChanged = false;
        public bool RandomItemStatsChanged = false;
        public bool GuildsChanged = false;

        public bool Populating = false;

        public BalanceConfigForm()
        {
            InitializeComponent();
            ClassComboBox.Items.AddRange(Enum.GetValues(typeof(MirClass)).Cast<object>().ToArray());

            for (int i = 0; i < Settings.RandomItemStatsList.Count; i++)
                RISIndexcomboBox.Items.Add(i);

            UpdateStatInterface();
            UpdateRandomItemStats();


            this.BaseStatType.ValueType = typeof(Stat);
            this.BaseStatType.DataSource = Enum.GetValues(typeof(Stat));

            this.BaseStatFormula.ValueType = typeof(StatFormula);
            this.BaseStatFormula.DataSource = Enum.GetValues(typeof(StatFormula));

            this.CapType.ValueType = typeof(Stat);
            this.CapType.DataSource = Enum.GetValues(typeof(Stat));
        }

        private void BalanceConfigForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //save configs
            if (BaseStatsChanged)
            {
                Settings.SaveBaseStats();
                SMain.Envir.RequiresBaseStatUpdate();
            }

            if (RandomItemStatsChanged) Settings.SaveRandomItemStats();
            if (GuildsChanged) Settings.SaveGuildSettings();
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            lblClassStatExample.Visible = false;

            if (tabControl1.SelectedTab.Name == "tabPage3")
            {
                lblClassStatExample.Visible = true;
            }
        }

        private void ClassComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            SelectedClassID = (byte)ClassComboBox.SelectedItem;

            Populating = true;
            UpdateClassStatGridView();
            UpdateClassCapGridView();
            Populating = false;
        }

        private void UpdateStatInterface()
        {
            MagicResistWeigttextbox.Text = Settings.MagicResistWeight.ToString();
            PoisonResistWeighttextbox.Text = Settings.PoisonResistWeight.ToString();
            CritialRateWeighttextbox.Text = Settings.CriticalRateWeight.ToString();
            CriticalDamagetextbox.Text = Settings.CriticalDamageWeight.ToString();
            PoisonAttackWeighttextbox.Text = Settings.PoisonAttackWeight.ToString();
            FreezingWeighttextbox.Text = Settings.FreezingAttackWeight.ToString();
            HealthRegenWeighttextBox.Text = Settings.HealthRegenWeight.ToString();
            ManaRegenWeighttextBox.Text = Settings.ManaRegenWeight.ToString();

            CanFreezecheckBox.Checked = Settings.PvpCanFreeze;
            CanResistPoisoncheckBox.Checked = Settings.PvpCanResistPoison;
            CanResistMagiccheckBox.Checked = Settings.PvpCanResistMagic;
        }

        private void UpdateRandomItemStats()
        {
            if (RISIndexcomboBox.SelectedItem == null)
            {
                RISMaxDuraChancetextBox.Text = string.Empty;
                RISMaxDuraStatChancetextBox.Text = string.Empty;
                RISMaxDuraMaxStattextBox.Text = string.Empty;
                RISMaxAcChancetextBox.Text = string.Empty;
                RISMaxAcStatChancetextBox.Text = string.Empty;
                RISMaxAcMaxStattextBox.Text = string.Empty;
                RISMaxMacChancetextBox.Text = string.Empty;
                RISMaxMacStatChancetextBox.Text = string.Empty;
                RISMaxMacMaxStattextBox.Text = string.Empty;
                RISMaxDcChancetextBox.Text = string.Empty;
                RISMaxDcStatChancetextBox.Text = string.Empty;
                RISMaxDcMaxStattextBox.Text = string.Empty;
                RISMaxMcChancetextBox.Text = string.Empty;
                RISMaxMcStatChancetextBox.Text = string.Empty;
                RISMaxMcMaxStattextBox.Text = string.Empty;
                RISMaxScChancetextBox.Text = string.Empty;
                RISMaxScStatChancetextBox.Text = string.Empty;
                RISMaxScMaxStattextBox.Text = string.Empty;
                RISMaxAccChancetextBox.Text = string.Empty;
                RISMaxAccStatChancetextBox.Text = string.Empty;
                RISMaxAccMaxStattextBox.Text = string.Empty;
                RISMaxAgilChancetextBox.Text = string.Empty;
                RISMaxAgilStatChancetextBox.Text = string.Empty;
                RISMaxAgilMaxStattextBox.Text = string.Empty;
                RISMaxHpChancetextBox.Text = string.Empty;
                RISMaxHpStatChancetextBox.Text = string.Empty;
                RISMaxHpMaxStattextBox.Text = string.Empty;
                RISMaxMpChancetextBox.Text = string.Empty;
                RISMaxMpStatChancetextBox.Text = string.Empty;
                RISMaxMpMaxStattextBox.Text = string.Empty;
                RISStrongChancetextBox.Text = string.Empty;
                RISStrongStatChancetextBox.Text = string.Empty;
                RISStrongMaxStattextBox.Text = string.Empty;
                RISMagicResistChancetextBox.Text = string.Empty;
                RISMagicResistStatChancetextBox.Text = string.Empty;
                RISMagicResistMaxStattextBox.Text = string.Empty;
                RISPoisonResistChancetextBox.Text = string.Empty;
                RISPoisonResistStatChancetextBox.Text = string.Empty;
                RISPoisonResistMaxStattextBox.Text = string.Empty;
                RISHpRecovChancetextBox.Text = string.Empty;
                RISHpRecovStatChancetextBox.Text = string.Empty;
                RISHpRecovMaxStattextBox.Text = string.Empty;
                RISMpRecovChancetextBox.Text = string.Empty;
                RISMpRecovStatChancetextBox.Text = string.Empty;
                RISMpRecovMaxStattextBox.Text = string.Empty;
                RISPoisonRecovChancetextBox.Text = string.Empty;
                RISPoisonRecovStatChancetextBox.Text = string.Empty;
                RISPoisonRecovMaxStattextBox.Text = string.Empty;
                RISCriticalRateChancetextBox.Text = string.Empty;
                RISCriticalRateStatChancetextBox.Text = string.Empty;
                RISCriticalRateMaxStattextBox.Text = string.Empty;
                RISCriticalDamageChancetextBox.Text = string.Empty;
                RISCriticalDamageStatChancetextBox.Text = string.Empty;
                RISCriticalDamageMaxStattextBox.Text = string.Empty;
                RISFreezingChancetextBox.Text = string.Empty;
                RISFreezingStatChancetextBox.Text = string.Empty;
                RISFreezingMaxStattextBox.Text = string.Empty;
                RISPoisonAttackChancetextBox.Text = string.Empty;
                RISPoisonAttackStatChancetextBox.Text = string.Empty;
                RISPoisonAttackMaxStattextBox.Text = string.Empty;
                RISAttackSpeedChancetextBox.Text = string.Empty;
                RISAttackSpeedStatChancetextBox.Text = string.Empty;
                RISAttackSpeedMaxStattextBox.Text = string.Empty;
                RISLuckChancetextBox.Text = string.Empty;
                RISLuckStatChancetextBox.Text = string.Empty;
                RISLuckMaxStattextBox.Text = string.Empty;
                RISCurseChancetextBox.Text = string.Empty;
            }
            else
            {
                if (Settings.RandomItemStatsList.Count <= RISIndexcomboBox.SelectedIndex)
                {
                    RISIndexcomboBox.SelectedItem = null;
                    UpdateRandomItemStats();
                    return;
                }
                RandomItemStat stat = Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex];
                RISMaxDuraChancetextBox.Text = stat.MaxDuraChance.ToString();
                RISMaxDuraStatChancetextBox.Text = stat.MaxDuraStatChance.ToString();
                RISMaxDuraMaxStattextBox.Text = stat.MaxDuraMaxStat.ToString();
                RISMaxAcChancetextBox.Text = stat.MaxAcChance.ToString();
                RISMaxAcStatChancetextBox.Text = stat.MaxAcStatChance.ToString();
                RISMaxAcMaxStattextBox.Text = stat.MaxAcMaxStat.ToString();
                RISMaxMacChancetextBox.Text = stat.MaxMacChance.ToString();
                RISMaxMacStatChancetextBox.Text = stat.MaxMacStatChance.ToString();
                RISMaxMacMaxStattextBox.Text = stat.MaxMacMaxStat.ToString();
                RISMaxDcChancetextBox.Text = stat.MaxDcChance.ToString();
                RISMaxDcStatChancetextBox.Text = stat.MaxDcStatChance.ToString();
                RISMaxDcMaxStattextBox.Text = stat.MaxDcMaxStat.ToString();
                RISMaxMcChancetextBox.Text = stat.MaxMcChance.ToString();
                RISMaxMcStatChancetextBox.Text = stat.MaxMcStatChance.ToString();
                RISMaxMcMaxStattextBox.Text = stat.MaxMcMaxStat.ToString();
                RISMaxScChancetextBox.Text = stat.MaxScChance.ToString();
                RISMaxScStatChancetextBox.Text = stat.MaxScStatChance.ToString();
                RISMaxScMaxStattextBox.Text = stat.MaxScMaxStat.ToString();
                RISMaxAccChancetextBox.Text = stat.AccuracyChance.ToString();
                RISMaxAccStatChancetextBox.Text = stat.AccuracyStatChance.ToString();
                RISMaxAccMaxStattextBox.Text = stat.AccuracyMaxStat.ToString();
                RISMaxAgilChancetextBox.Text = stat.AgilityChance.ToString();
                RISMaxAgilStatChancetextBox.Text = stat.AgilityStatChance.ToString();
                RISMaxAgilMaxStattextBox.Text = stat.AgilityMaxStat.ToString();
                RISMaxHpChancetextBox.Text = stat.HpChance.ToString();
                RISMaxHpStatChancetextBox.Text = stat.HpStatChance.ToString();
                RISMaxHpMaxStattextBox.Text = stat.HpMaxStat.ToString();
                RISMaxMpChancetextBox.Text = stat.MpChance.ToString();
                RISMaxMpStatChancetextBox.Text = stat.MpStatChance.ToString();
                RISMaxMpMaxStattextBox.Text = stat.MpMaxStat.ToString();
                RISStrongChancetextBox.Text = stat.StrongChance.ToString();
                RISStrongStatChancetextBox.Text = stat.StrongStatChance.ToString();
                RISStrongMaxStattextBox.Text = stat.StrongMaxStat.ToString();
                RISMagicResistChancetextBox.Text = stat.MagicResistChance.ToString();
                RISMagicResistStatChancetextBox.Text = stat.MagicResistStatChance.ToString();
                RISMagicResistMaxStattextBox.Text = stat.MagicResistMaxStat.ToString();
                RISPoisonResistChancetextBox.Text = stat.PoisonResistChance.ToString();
                RISPoisonResistStatChancetextBox.Text = stat.PoisonResistStatChance.ToString();
                RISPoisonResistMaxStattextBox.Text = stat.PoisonResistMaxStat.ToString();
                RISHpRecovChancetextBox.Text = stat.HpRecovChance.ToString();
                RISHpRecovStatChancetextBox.Text = stat.HpRecovStatChance.ToString();
                RISHpRecovMaxStattextBox.Text = stat.HpRecovMaxStat.ToString();
                RISMpRecovChancetextBox.Text = stat.MpRecovChance.ToString();
                RISMpRecovStatChancetextBox.Text = stat.MpRecovStatChance.ToString();
                RISMpRecovMaxStattextBox.Text = stat.MpRecovMaxStat.ToString();
                RISPoisonRecovChancetextBox.Text = stat.PoisonRecovChance.ToString();
                RISPoisonRecovStatChancetextBox.Text = stat.PoisonRecovStatChance.ToString();
                RISPoisonRecovMaxStattextBox.Text = stat.PoisonRecovMaxStat.ToString();
                RISCriticalRateChancetextBox.Text = stat.CriticalRateChance.ToString();
                RISCriticalRateStatChancetextBox.Text = stat.CriticalRateStatChance.ToString();
                RISCriticalRateMaxStattextBox.Text = stat.CriticalRateMaxStat.ToString();
                RISCriticalDamageChancetextBox.Text = stat.CriticalDamageChance.ToString();
                RISCriticalDamageStatChancetextBox.Text = stat.CriticalDamageStatChance.ToString();
                RISCriticalDamageMaxStattextBox.Text = stat.CriticalDamageMaxStat.ToString();
                RISFreezingChancetextBox.Text = stat.FreezeChance.ToString();
                RISFreezingStatChancetextBox.Text = stat.FreezeStatChance.ToString();
                RISFreezingMaxStattextBox.Text = stat.FreezeMaxStat.ToString();
                RISPoisonAttackChancetextBox.Text = stat.PoisonAttackChance.ToString();
                RISPoisonAttackStatChancetextBox.Text = stat.PoisonAttackStatChance.ToString();
                RISPoisonAttackMaxStattextBox.Text = stat.PoisonAttackMaxStat.ToString();
                RISAttackSpeedChancetextBox.Text = stat.AttackSpeedChance.ToString();
                RISAttackSpeedStatChancetextBox.Text = stat.AttackSpeedStatChance.ToString();
                RISAttackSpeedMaxStattextBox.Text = stat.AttackSpeedMaxStat.ToString();
                RISLuckChancetextBox.Text = stat.LuckChance.ToString();
                RISLuckStatChancetextBox.Text = stat.LuckStatChance.ToString();
                RISLuckMaxStattextBox.Text = stat.LuckMaxStat.ToString();
                RISCurseChancetextBox.Text = stat.CurseChance.ToString();
                RISSlotChancetextBox.Text = stat.SlotChance.ToString();
                RISSlotStatChancetextBox.Text = stat.SlotStatChance.ToString();
                RISSlotMaxStattextBox.Text = stat.SlotMaxStat.ToString();
            }
        }

        #region ItemStats

        private void MagicResistWeigttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.MagicResistWeight = temp;
        }

        private void PoisonResistWeighttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.PoisonResistWeight = temp;
        }

        private void CritialRateWeighttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.CriticalRateWeight = temp;
        }

        private void CriticalDamagetextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.CriticalDamageWeight = temp;
        }

        private void FreezingWeighttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.FreezingAttackWeight = temp;
        }

        private void PoisonAttackWeighttextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            Settings.PoisonAttackWeight = temp;
        }

        private void CanResistMagiccheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.PvpCanResistMagic = CanResistMagiccheckBox.Checked;
        }

        private void CanResistPoisoncheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.PvpCanResistPoison = CanResistPoisoncheckBox.Checked;
        }

        private void CanFreezecheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            Settings.PvpCanFreeze = CanFreezecheckBox.Checked;
        }

        private void HealthRegenWeighttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            BaseStatsChanged = true;
            Settings.HealthRegenWeight = temp;
        }

        private void ManaRegenWeighttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            BaseStatsChanged = true;
            Settings.ManaRegenWeight = temp;
        }

        #endregion

        #region RandomItemStat
        private void RISIndexcomboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            UpdateRandomItemStats();
        }

        private void RISAddIndexbutton_Click(object sender, EventArgs e)
        {
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList.Add(new RandomItemStat());
            RISIndexcomboBox.Items.Add(Settings.RandomItemStatsList.Count - 1);
            RISIndexcomboBox.SelectedIndex = Settings.RandomItemStatsList.Count - 1;
            UpdateRandomItemStats();
        }

        private void RISDeleteIndexbutton_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete the last index?", "Delete?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;
            RandomItemStatsChanged = true;
            RISIndexcomboBox.Items.Remove(Settings.RandomItemStatsList.Count - 1);
            Settings.RandomItemStatsList.RemoveAt(Settings.RandomItemStatsList.Count - 1);
            UpdateRandomItemStats();
        }

        private void RISMaxDuraChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxDuraChance = temp;
        }

        private void RISMaxDuraStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxDuraStatChance = temp;
        }

        private void RISMaxDuraMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxDuraMaxStat = temp;
        }

        private void RISMaxAcChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxAcChance = temp;
        }

        private void RISMaxAcStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxAcStatChance = temp;
        }

        private void RISMaxAcMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxAcMaxStat = temp;
        }

        private void RISMaxMacChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxMacChance = temp;
        }

        private void RISMaxMacStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxMacStatChance = temp;
        }

        private void RISMaxMacMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxMacMaxStat = temp;
        }

        private void RISMaxDcChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxDcChance = temp;
        }

        private void RISMaxDcStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxDcStatChance = temp;
        }

        private void RISMaxDcMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxDcMaxStat = temp;
        }

        private void RISMaxMcChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxMcChance = temp;
        }

        private void RISMaxMcStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxMcStatChance = temp;
        }

        private void RISMaxMcMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxMcMaxStat = temp;
        }

        private void RISMaxScChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxScChance = temp;
        }

        private void RISMaxScStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxScStatChance = temp;
        }

        private void RISMaxScMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MaxScMaxStat = temp;
        }

        private void RISMaxAccChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].AccuracyChance = temp;
        }

        private void RISMaxAccStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].AccuracyStatChance = temp;
        }

        private void RISMaxAccMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].AccuracyMaxStat = temp;
        }

        private void RISMaxAgilChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].AgilityChance = temp;
        }

        private void RISMaxAgilStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].AgilityStatChance = temp;
        }

        private void RISMaxAgilMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].AgilityMaxStat = temp;
        }

        private void RISMaxHpChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].HpChance = temp;
        }

        private void RISMaxHpStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].HpStatChance = temp;
        }

        private void RISMaxHpMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].HpMaxStat = temp;
        }

        private void RISMaxMpChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MpChance = temp;
        }

        private void RISMaxMpStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MpStatChance = temp;
        }

        private void RISMaxMpMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MpMaxStat = temp;
        }

        private void RISStrongChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].StrongChance = temp;
        }

        private void RISStrongStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].StrongStatChance = temp;
        }

        private void RISStrongMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].StrongMaxStat = temp;
        }

        private void RISMagicResistChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MagicResistChance = temp;
        }

        private void RISMagicResistStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MagicResistStatChance = temp;
        }

        private void RISMagicResistMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MagicResistMaxStat = temp;
        }

        private void RISPoisonResistChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].PoisonResistChance = temp;
        }

        private void RISPoisonResistStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].PoisonResistStatChance = temp;
        }

        private void RISPoisonResistMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].PoisonResistMaxStat = temp;
        }

        private void RISHpRecovChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].HpRecovChance = temp;
        }

        private void RISHpRecovStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].HpRecovStatChance = temp;
        }

        private void RISHpRecovMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].HpRecovMaxStat = temp;
        }

        private void RISMpRecovChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MpRecovChance = temp;
        }

        private void RISMpRecovStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MpRecovStatChance = temp;
        }

        private void RISMpRecovMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].MpRecovMaxStat = temp;
        }

        private void RISPoisonRecovChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].PoisonRecovChance = temp;
        }

        private void RISPoisonRecovStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].PoisonRecovStatChance = temp;
        }

        private void RISPoisonRecovMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].PoisonRecovMaxStat = temp;
        }

        private void RISCriticalRateChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].CriticalRateChance = temp;
        }

        private void RISCriticalRateStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].CriticalRateStatChance = temp;
        }

        private void RISCriticalRateMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].CriticalRateMaxStat = temp;
        }

        private void RISCriticalDamageChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].CriticalDamageChance = temp;
        }

        private void RISCriticalDamageStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].CriticalDamageStatChance = temp;
        }

        private void RISCriticalDamageMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].CriticalDamageMaxStat = temp;
        }

        private void RISFreezingChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].FreezeChance = temp;
        }

        private void RISFreezingStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].FreezeStatChance = temp;
        }

        private void RISFreezingMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].FreezeMaxStat = temp;
        }

        private void RISPoisonAttackChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].PoisonAttackChance = temp;
        }

        private void RISPoisonAttackStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].PoisonAttackStatChance = temp;
        }

        private void RISPoisonAttackMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].PoisonAttackMaxStat = temp;
        }

        private void RISAttackSpeedChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].AttackSpeedChance = temp;
        }

        private void RISAttackSpeedStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].AttackSpeedStatChance = temp;
        }

        private void RISAttackSpeedMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].AttackSpeedMaxStat = temp;
        }

        private void RISLuckChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].LuckChance = temp;
        }

        private void RISLuckStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].LuckStatChance = temp;
        }

        private void RISLuckMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].LuckMaxStat = temp;
        }

        private void RISCurseChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].CurseChance = temp;
        }

        private void RISSlotChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].SlotChance = temp;
        }

        private void RISSlotStatChancetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].SlotStatChance = temp;
        }

        private void RISSlotMaxStattextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (RISIndexcomboBox.SelectedItem == null) return;
            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (temp < 1) || temp > 12)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            RandomItemStatsChanged = true;
            Settings.RandomItemStatsList[RISIndexcomboBox.SelectedIndex].SlotMaxStat = temp;
        }
        #endregion

        #region Class
        private void classGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            classGridView.Rows[e.RowIndex].ErrorText = "";

            if (classGridView.Rows[e.RowIndex].IsNewRow) { return; }

            if (e.ColumnIndex == 2 || e.ColumnIndex == 5)
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true;
                    classGridView.Rows[e.RowIndex].ErrorText = "the value must be an integer";
                }
            }
            else if (e.ColumnIndex == 3 || e.ColumnIndex == 4)
            {
                if (!float.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true;
                    classGridView.Rows[e.RowIndex].ErrorText = "the value must be a decimal";
                }
            }
        }

        private void classGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["BaseStatBase"].Value = 0;
            e.Row.Cells["BaseStatGain"].Value = 0.0F;
            e.Row.Cells["BaseStatGainRate"].Value = 0.0F;
            e.Row.Cells["BaseStatMax"].Value = 0;
        }

        private void classGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (classGridView.Rows[e.RowIndex].IsNewRow) return;

            if (classGridView.Columns[e.ColumnIndex].Name == "BaseStatFormula")
            {
                var formula = (StatFormula)classGridView.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

                var gainRateCell = classGridView.Rows[e.RowIndex].Cells["BaseStatGainRate"];

                if (formula == StatFormula.Health || formula == StatFormula.Mana)
                {
                    gainRateCell.ReadOnly = false;
                    gainRateCell.Style.BackColor = gainRateCell.OwningColumn.DefaultCellStyle.BackColor;
                    gainRateCell.Style.ForeColor = gainRateCell.OwningColumn.DefaultCellStyle.ForeColor;
                }
                else
                {
                    gainRateCell.ReadOnly = true;
                    gainRateCell.Style.BackColor = Color.LightGray;
                    gainRateCell.Style.ForeColor = Color.DarkGray;
                }
            }

            var statType = (Stat)classGridView.Rows[e.RowIndex].Cells["BaseStatType"].Value;

            UpdateBaseStatData();
            UpdateClassStatExample(statType);
        }

        private void classGridView_SelectionChanged(object sender, EventArgs e)
        {
            if (classGridView.SelectedRows.Count == 1)
            {
                var row = classGridView.SelectedRows[0];

                var statType = (Stat)row.Cells["BaseStatType"].Value;

                UpdateClassStatExample(statType);
            }
        }

        private void classGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            UpdateBaseStatData();
        }

        private void classCapGridView_CellValidating(object sender, DataGridViewCellValidatingEventArgs e)
        {
            classCapGridView.Rows[e.RowIndex].ErrorText = "";

            if (classCapGridView.Rows[e.RowIndex].IsNewRow) { return; }

            if (e.ColumnIndex == 1)
            {
                if (!int.TryParse(e.FormattedValue.ToString(), out _))
                {
                    e.Cancel = true;
                    classCapGridView.Rows[e.RowIndex].ErrorText = "the value must be an integer";
                }
            }
        }

        private void classCapGridView_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            if (classCapGridView.Rows[e.RowIndex].IsNewRow) return;

            var capType = (Stat)classCapGridView.Rows[e.RowIndex].Cells["CapType"].Value;

            UpdateCapStatData();
            UpdateClassStatExample(capType);
        }

        private void classCapGridView_DefaultValuesNeeded(object sender, DataGridViewRowEventArgs e)
        {
            e.Row.Cells["Value"].Value = 0;
        }

        private void classCapGridView_UserDeletedRow(object sender, DataGridViewRowEventArgs e)
        {
            UpdateCapStatData();
        }

        private void UpdateClassStatExample(Stat type)
        {
            if (Populating) return;

            var str = string.Format("{0} - {1}\r\r", ((MirClass)SelectedClassID), type);

            BaseStats classStats = Settings.ClassBaseStats[SelectedClassID];

            var stat = classStats.Stats.FirstOrDefault(x => x.Type == type);

            if (stat != null)
            {
                var cap = classStats.Caps[type];

                if (cap == 0) cap = int.MaxValue;

                for (int level = 1; level <= 50; level++)
                {
                    str += string.Format("Level {0}\t: {1}\n", level, Math.Min(cap, stat.Calculate((MirClass)SelectedClassID, level)));
                }

                lblClassStatExample.Text = str;
            }
        }

        private void UpdateClassStatGridView()
        {
            classGridView.Rows.Clear();

            BaseStats classStats = Settings.ClassBaseStats[SelectedClassID];

            foreach (var stat in classStats.Stats)
            {
                int rowIndex = classGridView.Rows.Add();

                var row = classGridView.Rows[rowIndex];

                row.Cells["BaseStatType"].Value = stat.Type;
                row.Cells["BaseStatFormula"].Value = stat.FormulaType;
                row.Cells["BaseStatBase"].Value = stat.Base;
                row.Cells["BaseStatGain"].Value = stat.Gain;
                row.Cells["BaseStatGainRate"].Value = stat.GainRate;
                row.Cells["BaseStatMax"].Value = stat.Max;
            }
        }

        private void UpdateClassCapGridView()
        {
            classCapGridView.Rows.Clear();

            BaseStats classStats = Settings.ClassBaseStats[SelectedClassID];

            foreach (var cap in classStats.Caps.Values)
            {
                int rowIndex = classCapGridView.Rows.Add();

                var row = classCapGridView.Rows[rowIndex];

                row.Cells["CapType"].Value = cap.Key;
                row.Cells["Value"].Value = cap.Value;
            }
        }

        private void UpdateBaseStatData()
        {
            if (Populating) return;

            BaseStatsChanged = true;

            BaseStats classStats = Settings.ClassBaseStats[SelectedClassID];

            classStats.Stats.Clear();

            foreach (DataGridViewRow row in classGridView.Rows)
            {
                var cells = row.Cells;

                if (cells[0].Value == null || cells[1].Value == null) continue;

                var type = (Stat)row.Cells["BaseStatType"].Value;
                var formula = (StatFormula)row.Cells["BaseStatFormula"].Value;

                if (classStats.Stats.Any(x => x.Type == type))
                {
                    MessageBox.Show(string.Format($"The stat '{type}' exists more than once so will not be saved."));
                    continue;
                }

                var baseStat = new BaseStat(type)
                {
                    FormulaType = (StatFormula)cells["BaseStatFormula"].Value.ValueOrDefault<byte>(),
                    Base = cells["BaseStatBase"].Value.ValueOrDefault<int>(),
                    Gain = cells["BaseStatGain"].Value.ValueOrDefault<float>(),
                    GainRate = cells["BaseStatGainRate"].Value.ValueOrDefault<float>(),
                    Max = cells["BaseStatMax"].Value.ValueOrDefault<int>()
                };

                classStats.Stats.Add(baseStat);
            }
        }

        private void UpdateCapStatData()
        {
            if (Populating) return;

            BaseStatsChanged = true;

            BaseStats classStats = Settings.ClassBaseStats[SelectedClassID];

            classStats.Caps.Clear();

            foreach (DataGridViewRow row in classCapGridView.Rows)
            {
                var cells = row.Cells;

                if (cells[0].Value == null || cells[1].Value == null) continue;

                var type = (Stat)row.Cells["CapType"].Value;
                var value = (int)row.Cells["Value"].Value.ValueOrDefault<int>();

                classStats.Caps[type] = value;
            }
        }

        #endregion
    }
}
