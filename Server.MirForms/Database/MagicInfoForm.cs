using System;
using System.Drawing;
using Server.MirDatabase;
using System.Windows.Forms;
using Server.MirEnvir;

namespace Server
{
    public partial class MagicInfoForm : Form
    {

        public Envir Envir => SMain.EditEnvir;

        private MagicInfo _selectedMagicInfo;

        public MagicInfoForm()
        {
            InitializeComponent();
            for (int i = 0; i < Envir.MagicInfoList.Count; i++)
                MagiclistBox.Items.Add(Envir.MagicInfoList[i]);
            UpdateMagicForm();
        }

        private void UpdateMagicList()
        {
            MagiclistBox.Items.Clear();

            for (int i = 0; i < Envir.MagicInfoList.Count; i++)
            {
                if ((string.IsNullOrWhiteSpace(txtSearch.Text) || Envir.MagicInfoList[i].Name.IndexOf(txtSearch.Text, StringComparison.OrdinalIgnoreCase) >= 0))
                {
                    MagiclistBox.Items.Add(Envir.MagicInfoList[i]);
                }
            }
        }

        private void UpdateMagicForm(byte field = 0)
        {
             _selectedMagicInfo = (MagicInfo)MagiclistBox.SelectedItem;

             lblBookValid.BackColor = SystemColors.Window;

             if (_selectedMagicInfo == null)
             {
                 tabControl1.Enabled = false;
                 lblBookValid.Text = "Searching";
                 lblSelected.Text = "Selected Skill: none";
                 lblDamageExample.Text = "";
                 lblDamageExplained.Text = "";
                 PvPlblDamageExample.Text = "";
                 PvPlblDamageExplained.Text = "";
                 txtSkillIcon.Text = "0";
                 txtSkillLvl1Points.Text = "0";
                 txtSkillLvl1Req.Text = "0";
                 txtSkillLvl2Points.Text = "0";
                 txtSkillLvl2Req.Text = "0";
                 txtSkillLvl3Points.Text = "0";
                 txtSkillLvl3Req.Text = "0";
                txtSkillLvl4Points.Text = "0";
                txtSkillLvl4Req.Text = "0";
                txtSkillLvl5Points.Text = "0";
                txtSkillLvl5Req.Text = "0";
                txtSkillLvl6Points.Text = "0";
                txtSkillLvl6Req.Text = "0";
                txtSkillLvl7Points.Text = "0";
                txtSkillLvl7Req.Text = "0";
                txtSkillLvl8Points.Text = "0";
                txtSkillLvl8Req.Text = "0";
                txtSkillLvl9Points.Text = "0";
                txtSkillLvl9Req.Text = "0";
                txtSkillLvl10Points.Text = "0";
                txtSkillLvl10Req.Text = "0";
                txtMPBase.Text = "0";
                txtMPIncrease.Text = "0";
                txtDelayBase.Text = "0";
                txtDelayReduction.Text = "0";
                txtDmgBaseMin.Text = "0";
                txtDmgBaseMax.Text = "0";
                txtDmgBonusMin.Text = "0";
                txtDmgBonusMax.Text = "0";
                txtPvPDmgBaseMin.Text = "0";
                txtPvPDmgBaseMax.Text = "0";
                txtPvPDmgBonusMin.Text = "0";
                txtPvPDmgBonusMax.Text = "0";
             }
             else
             {
                 tabControl1.Enabled = true;
                 lblSelected.Text = "Selected Skill: " + _selectedMagicInfo.ToString();

                lblDamageExample.Text =
                   $"Damage @ Skill " +
                   $"level 0: {GetMinPower(0):000}-{GetMaxPower(0):000}     |||     " +
                   $"level 4: {GetMinPower(4):000}-{GetMaxPower(4):000}     |||     " +
                   $"level 8: {GetMinPower(8):000}-{GetMaxPower(8):000}\n                          " +
                   $"level 1: {GetMinPower(1):000}-{GetMaxPower(1):000}     |||     " +
                   $"level 5: {GetMinPower(5):000}-{GetMaxPower(5):000}     |||     " +
                   $"level 9: {GetMinPower(9):000}-{GetMaxPower(9):000}\n                          " +
                   $"level 2: {GetMinPower(2):000}-{GetMaxPower(2):000}     |||     " +
                   $"level 6: {GetMinPower(6):000}-{GetMaxPower(6):000}     |||     " +
                   $"level 10: {GetMinPower(10):000}-{GetMaxPower(10):000}\n                          " +
                   $"level 3: {GetMinPower(3):000}-{GetMaxPower(3):000}     |||     " +
                   $"level 7: {GetMinPower(7):000}-{GetMaxPower(7):000}";
                lblDamageExplained.Text =
                    $"Damage: {{Random(minstat-maxstat) + [<(random({_selectedMagicInfo.MPowerBase}-{_selectedMagicInfo.MPowerBase + _selectedMagicInfo.MPowerBonus})/4) X (skill level +1)> + random<{_selectedMagicInfo.PowerBase}-{_selectedMagicInfo.PowerBonus + _selectedMagicInfo.PowerBase}>]}}  X  {{{_selectedMagicInfo.MultiplierBase} + (skill level * {_selectedMagicInfo.MultiplierBonus})}}";
 
                PvPlblDamageExample.Text =
                    $"PvPDamage @ Skill " +
                    $"level 0: {GetMinPvPPower(0):000}-{GetMaxPvPPower(0):000}     |||     " +
                    $"level 4: {GetMinPvPPower(4):000}-{GetMaxPvPPower(4):000}     |||     " +
                    $"level 8: {GetMinPvPPower(8):000}-{GetMaxPvPPower(8):000}\n                                 " +
                    $"level 1: {GetMinPvPPower(1):000}-{GetMaxPvPPower(1):000}     |||     " +
                    $"level 5: {GetMinPvPPower(5):000}-{GetMaxPvPPower(5):000}     |||     " +
                    $"level 9: {GetMinPvPPower(9):000}-{GetMaxPvPPower(9):000}\n                                 " +
                    $"level 2: {GetMinPvPPower(2):000}-{GetMaxPvPPower(2):000}     |||     " +
                    $"level 6: {GetMinPvPPower(6):000}-{GetMaxPvPPower(6):000}     |||     " +
                    $"level 10: {GetMinPvPPower(10):000}-{GetMaxPvPPower(10):000}\n                                 " +
                    $"level 3: {GetMinPvPPower(3):000}-{GetMaxPvPPower(3):000}     |||     " +
                    $"level 7: {GetMinPvPPower(7):000}-{GetMaxPvPPower(7):000}";
                PvPlblDamageExplained.Text =
                    $"PvPDamage: {{Random(minstat-maxstat) + [<(random({_selectedMagicInfo.PvPMPowerBase}-{_selectedMagicInfo.PvPMPowerBase + _selectedMagicInfo.PvPMPowerBonus})/4) X (skill level +1)> + random<{_selectedMagicInfo.PvPPowerBase}-{_selectedMagicInfo.PvPPowerBonus + _selectedMagicInfo.PvPPowerBase}>]}}  X  {{{_selectedMagicInfo.PvPMultiplierBase} + (skill level * {_selectedMagicInfo.PvPMultiplierBonus})}}";

                 txtSkillIcon.Text = _selectedMagicInfo.Icon.ToString();
                 txtSkillLvl1Points.Text = _selectedMagicInfo.Need1.ToString();
                 txtSkillLvl1Req.Text = _selectedMagicInfo.Level1.ToString();
                 txtSkillLvl2Points.Text = _selectedMagicInfo.Need2.ToString();
                 txtSkillLvl2Req.Text = _selectedMagicInfo.Level2.ToString();
                 txtSkillLvl3Points.Text = _selectedMagicInfo.Need3.ToString();
                 txtSkillLvl3Req.Text = _selectedMagicInfo.Level3.ToString();
                txtSkillLvl4Points.Text = _selectedMagicInfo.Need4.ToString();
                txtSkillLvl4Req.Text = _selectedMagicInfo.Level4.ToString();
                txtSkillLvl5Points.Text = _selectedMagicInfo.Need5.ToString();
                txtSkillLvl5Req.Text = _selectedMagicInfo.Level5.ToString();
                txtSkillLvl6Points.Text = _selectedMagicInfo.Need6.ToString();
                txtSkillLvl6Req.Text = _selectedMagicInfo.Level6.ToString();
                txtSkillLvl7Points.Text = _selectedMagicInfo.Need7.ToString();
                txtSkillLvl7Req.Text = _selectedMagicInfo.Level7.ToString();
                txtSkillLvl8Points.Text = _selectedMagicInfo.Need8.ToString();
                txtSkillLvl8Req.Text = _selectedMagicInfo.Level8.ToString();
                txtSkillLvl9Points.Text = _selectedMagicInfo.Need9.ToString();
                txtSkillLvl9Req.Text = _selectedMagicInfo.Level9.ToString();
                txtSkillLvl10Points.Text = _selectedMagicInfo.Need10.ToString();
                txtSkillLvl10Req.Text = _selectedMagicInfo.Level10.ToString();
                txtMPBase.Text = _selectedMagicInfo.BaseCost.ToString();
                txtMPIncrease.Text = _selectedMagicInfo.LevelCost.ToString();
                txtDelayBase.Text = _selectedMagicInfo.DelayBase.ToString();
                txtDelayReduction.Text = _selectedMagicInfo.DelayReduction.ToString();
                txtDmgBaseMin.Text = _selectedMagicInfo.PowerBase.ToString();
                txtDmgBaseMax.Text = (_selectedMagicInfo.PowerBase + _selectedMagicInfo.PowerBonus).ToString();
                txtDmgBonusMin.Text = _selectedMagicInfo.MPowerBase.ToString();
                txtDmgBonusMax.Text = (_selectedMagicInfo.MPowerBase + _selectedMagicInfo.MPowerBonus).ToString();
                txtPvPDmgBaseMin.Text = _selectedMagicInfo.PvPPowerBase.ToString();
                txtPvPDmgBaseMax.Text = (_selectedMagicInfo.PvPPowerBase + _selectedMagicInfo.PvPPowerBonus).ToString();
                txtPvPDmgBonusMin.Text = _selectedMagicInfo.PvPMPowerBase.ToString();
                txtPvPDmgBonusMax.Text = (_selectedMagicInfo.PvPMPowerBase + _selectedMagicInfo.PvPMPowerBonus).ToString();
                if (field != 1)
                   txtDmgMultBase.Text = _selectedMagicInfo.MultiplierBase.ToString();
                if (field != 2)
                   txtDmgMultBoost.Text = _selectedMagicInfo.MultiplierBonus.ToString();
                if (field != 3)
                    txtPvPDmgMultBase.Text = _selectedMagicInfo.PvPMultiplierBase.ToString();
                if (field != 4)
                    txtPvPDmgMultBoost.Text = _selectedMagicInfo.PvPMultiplierBonus.ToString();

                txtRange.Text = _selectedMagicInfo.Range.ToString();

                ItemInfo Book = Envir.GetBook((short)_selectedMagicInfo.Spell);
                 if (Book != null)
                 {
                     lblBookValid.Text = Book.Name;
                 }
                 else
                 {
                     lblBookValid.Text = "No book found";
                     lblBookValid.BackColor = Color.Red;
                 }
                this.textBoxName.Text = _selectedMagicInfo.Name;
             }
        }

        private int GetMaxPower(byte level)
        {
            if (_selectedMagicInfo == null) return 0;
            return (int)Math.Round((((_selectedMagicInfo.MPowerBase + _selectedMagicInfo.MPowerBonus) / 4F) * (level + 1) + (_selectedMagicInfo.PowerBase + _selectedMagicInfo.PowerBonus))* (_selectedMagicInfo.MultiplierBase + (level * _selectedMagicInfo.MultiplierBonus)));
        }
        private int GetMinPower(byte level)
        {
            if (_selectedMagicInfo == null) return 0;
            return (int)Math.Round(((_selectedMagicInfo.MPowerBase / 4F) * (level + 1) + _selectedMagicInfo.PowerBase) * (_selectedMagicInfo.MultiplierBase + (level * _selectedMagicInfo.MultiplierBonus)));
        }

        private int GetMaxPvPPower(byte pvplevel)
        {
            if (_selectedMagicInfo == null) return 0;
            return (int)Math.Round((((_selectedMagicInfo.PvPMPowerBase + _selectedMagicInfo.PvPMPowerBonus) / 4F) * (pvplevel + 1) + (_selectedMagicInfo.PvPPowerBase + _selectedMagicInfo.PvPPowerBonus))* (_selectedMagicInfo.PvPMultiplierBase + (pvplevel * _selectedMagicInfo.PvPMultiplierBonus)));
        }
        private int GetMinPvPPower(byte pvplevel)
        {
            if (_selectedMagicInfo == null) return 0;
            return (int)Math.Round(((_selectedMagicInfo.PvPMPowerBase / 4F) * (pvplevel + 1) + _selectedMagicInfo.PvPPowerBase) * (_selectedMagicInfo.PvPMultiplierBase + (pvplevel * _selectedMagicInfo.PvPMultiplierBonus)));
        }

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panel5 = new System.Windows.Forms.Panel();
            this.txtPvPDmgMultBoost = new System.Windows.Forms.TextBox();
            this.txtPvPDmgMultBase = new System.Windows.Forms.TextBox();
            this.label21PvP = new System.Windows.Forms.Label();
            this.label22PvP = new System.Windows.Forms.Label();
            this.txtPvPDmgBonusMax = new System.Windows.Forms.TextBox();
            this.txtPvPDmgBonusMin = new System.Windows.Forms.TextBox();
            this.label18PvP = new System.Windows.Forms.Label();
            this.label19PvP = new System.Windows.Forms.Label();
            this.txtPvPDmgBaseMax = new System.Windows.Forms.TextBox();
            this.txtPvPDmgBaseMin = new System.Windows.Forms.TextBox();
            this.label17PvP = new System.Windows.Forms.Label();
            this.label16PvP = new System.Windows.Forms.Label();
            this.label15PvP = new System.Windows.Forms.Label();
            this.PvPlblDamageExample = new System.Windows.Forms.Label();
            this.PvPlblDamageExplained = new System.Windows.Forms.Label();
            this.MagiclistBox = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
            this.textBoxName = new System.Windows.Forms.TextBox();
            this.lblDamageExample = new System.Windows.Forms.Label();
            this.lblDamageExplained = new System.Windows.Forms.Label();
            this.lblSelected = new System.Windows.Forms.Label();
            this.panel4 = new System.Windows.Forms.Panel();
            this.txtDmgMultBoost = new System.Windows.Forms.TextBox();
            this.txtDmgMultBase = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.txtDmgBonusMax = new System.Windows.Forms.TextBox();
            this.txtDmgBonusMin = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.label19 = new System.Windows.Forms.Label();
            this.txtDmgBaseMax = new System.Windows.Forms.TextBox();
            this.txtDmgBaseMin = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label16 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.label20 = new System.Windows.Forms.Label();
            this.txtRange = new System.Windows.Forms.TextBox();
            this.txtDelayReduction = new System.Windows.Forms.TextBox();
            this.txtDelayBase = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label13 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.txtMPIncrease = new System.Windows.Forms.TextBox();
            this.txtMPBase = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.panel1 = new System.Windows.Forms.Panel();
            this.txtSkillLvl10Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl9Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl8Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl7Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl6Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl5Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl4Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl3Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl2Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl1Points = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSkillLvl10Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl9Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl8Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl7Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl6Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl5Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl4Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl3Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl2Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl1Req = new System.Windows.Forms.TextBox();
            this.label31 = new System.Windows.Forms.Label();
            this.label30 = new System.Windows.Forms.Label();
            this.label29 = new System.Windows.Forms.Label();
            this.label28 = new System.Windows.Forms.Label();
            this.label27 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.label25 = new System.Windows.Forms.Label();
            this.label38 = new System.Windows.Forms.Label();
            this.label37 = new System.Windows.Forms.Label();
            this.label36 = new System.Windows.Forms.Label();
            this.label35 = new System.Windows.Forms.Label();
            this.label34 = new System.Windows.Forms.Label();
            this.label33 = new System.Windows.Forms.Label();
            this.label32 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSkillIcon = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBookValid = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.toolTip2 = new System.Windows.Forms.ToolTip(this.components);
            this.panel5.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(3, 8);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(225, 22);
            this.txtSearch.TabIndex = 2;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.txtPvPDmgMultBoost);
            this.panel5.Controls.Add(this.txtPvPDmgMultBase);
            this.panel5.Controls.Add(this.label21PvP);
            this.panel5.Controls.Add(this.label22PvP);
            this.panel5.Controls.Add(this.txtPvPDmgBonusMax);
            this.panel5.Controls.Add(this.txtPvPDmgBonusMin);
            this.panel5.Controls.Add(this.label18PvP);
            this.panel5.Controls.Add(this.label19PvP);
            this.panel5.Controls.Add(this.txtPvPDmgBaseMax);
            this.panel5.Controls.Add(this.txtPvPDmgBaseMin);
            this.panel5.Controls.Add(this.label17PvP);
            this.panel5.Controls.Add(this.label16PvP);
            this.panel5.Controls.Add(this.label15PvP);
            this.panel5.Location = new System.Drawing.Point(477, 203);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(233, 191);
            this.panel5.TabIndex = 6;
            // 
            // txtPvPDmgMultBoost
            // 
            this.txtPvPDmgMultBoost.Location = new System.Drawing.Point(168, 157);
            this.txtPvPDmgMultBoost.Name = "txtPvPDmgMultBoost";
            this.txtPvPDmgMultBoost.Size = new System.Drawing.Size(46, 22);
            this.txtPvPDmgMultBoost.TabIndex = 14;
            this.toolTip2.SetToolTip(this.txtPvPDmgMultBoost, "extra multiplyer apply\'d for every skill level");
            this.txtPvPDmgMultBoost.TextChanged += new System.EventHandler(this.txtPvPDmgMultBoost_TextChanged);
            // 
            // txtPvPDmgMultBase
            // 
            this.txtPvPDmgMultBase.Location = new System.Drawing.Point(168, 131);
            this.txtPvPDmgMultBase.Name = "txtPvPDmgMultBase";
            this.txtPvPDmgMultBase.Size = new System.Drawing.Size(46, 22);
            this.txtPvPDmgMultBase.TabIndex = 13;
            this.toolTip2.SetToolTip(this.txtPvPDmgMultBase, "multiplier apply\'d to total skill dmg");
            this.txtPvPDmgMultBase.TextChanged += new System.EventHandler(this.txtPvPDmgMultBase_TextChanged);
            // 
            // label21PvP
            // 
            this.label21PvP.AutoSize = true;
            this.label21PvP.Location = new System.Drawing.Point(12, 160);
            this.label21PvP.Name = "label21PvP";
            this.label21PvP.Size = new System.Drawing.Size(204, 17);
            this.label21PvP.TabIndex = 12;
            this.label21PvP.Text = "Damage multiplyer boost/skilllvl";
            // 
            // label22PvP
            // 
            this.label22PvP.AutoSize = true;
            this.label22PvP.Location = new System.Drawing.Point(12, 134);
            this.label22PvP.Name = "label22PvP";
            this.label22PvP.Size = new System.Drawing.Size(160, 17);
            this.label22PvP.TabIndex = 11;
            this.label22PvP.Text = "Damage multiplyer base";
            // 
            // txtPvPDmgBonusMax
            // 
            this.txtPvPDmgBonusMax.Location = new System.Drawing.Point(168, 105);
            this.txtPvPDmgBonusMax.Name = "txtPvPDmgBonusMax";
            this.txtPvPDmgBonusMax.Size = new System.Drawing.Size(46, 22);
            this.txtPvPDmgBonusMax.TabIndex = 10;
            this.toolTip2.SetToolTip(this.txtPvPDmgBonusMax, "Damage bonus at skill level \'4\' ");
            this.txtPvPDmgBonusMax.TextChanged += new System.EventHandler(this.txtPvPDmgBonusMax_TextChanged);
            // 
            // txtPvPDmgBonusMin
            // 
            this.txtPvPDmgBonusMin.Location = new System.Drawing.Point(168, 79);
            this.txtPvPDmgBonusMin.Name = "txtPvPDmgBonusMin";
            this.txtPvPDmgBonusMin.Size = new System.Drawing.Size(46, 22);
            this.txtPvPDmgBonusMin.TabIndex = 9;
            this.toolTip2.SetToolTip(this.txtPvPDmgBonusMin, "Damage bonus at skill level \'4\' \r\nyou will get 1/4th of this bonus for every skil" +
        "l level\r\nnote ingame level 0 = 1 bonus, so level 3 = max bonus (4)");
            this.txtPvPDmgBonusMin.TextChanged += new System.EventHandler(this.txtPvPDmgBonusMin_TextChanged);
            // 
            // label18PvP
            // 
            this.label18PvP.AutoSize = true;
            this.label18PvP.Location = new System.Drawing.Point(12, 108);
            this.label18PvP.Name = "label18PvP";
            this.label18PvP.Size = new System.Drawing.Size(177, 17);
            this.label18PvP.TabIndex = 8;
            this.label18PvP.Text = "Maximum skill lvl 3 damage";
            // 
            // label19PvP
            // 
            this.label19PvP.AutoSize = true;
            this.label19PvP.Location = new System.Drawing.Point(12, 82);
            this.label19PvP.Name = "label19PvP";
            this.label19PvP.Size = new System.Drawing.Size(178, 17);
            this.label19PvP.TabIndex = 7;
            this.label19PvP.Text = "Minimum skill lvl 3 damage:";
            // 
            // txtPvPDmgBaseMax
            // 
            this.txtPvPDmgBaseMax.Location = new System.Drawing.Point(168, 53);
            this.txtPvPDmgBaseMax.Name = "txtPvPDmgBaseMax";
            this.txtPvPDmgBaseMax.Size = new System.Drawing.Size(46, 22);
            this.txtPvPDmgBaseMax.TabIndex = 6;
            this.toolTip2.SetToolTip(this.txtPvPDmgBaseMax, "Damage at skill level 0");
            this.txtPvPDmgBaseMax.TextChanged += new System.EventHandler(this.txtPvPDmgBaseMax_TextChanged);
            // 
            // txtPvPDmgBaseMin
            // 
            this.txtPvPDmgBaseMin.Location = new System.Drawing.Point(168, 27);
            this.txtPvPDmgBaseMin.Name = "txtPvPDmgBaseMin";
            this.txtPvPDmgBaseMin.Size = new System.Drawing.Size(46, 22);
            this.txtPvPDmgBaseMin.TabIndex = 5;
            this.toolTip2.SetToolTip(this.txtPvPDmgBaseMin, "Damage at skill level 0");
            this.txtPvPDmgBaseMin.TextChanged += new System.EventHandler(this.txtPvPDmgBaseMin_TextChanged);
            // 
            // label17PvP
            // 
            this.label17PvP.AutoSize = true;
            this.label17PvP.Location = new System.Drawing.Point(12, 56);
            this.label17PvP.Name = "label17PvP";
            this.label17PvP.Size = new System.Drawing.Size(156, 17);
            this.label17PvP.TabIndex = 2;
            this.label17PvP.Text = "Maximum base damage";
            // 
            // label16PvP
            // 
            this.label16PvP.AutoSize = true;
            this.label16PvP.Location = new System.Drawing.Point(12, 30);
            this.label16PvP.Name = "label16PvP";
            this.label16PvP.Size = new System.Drawing.Size(157, 17);
            this.label16PvP.TabIndex = 1;
            this.label16PvP.Text = "Minimum base damage:";
            // 
            // label15PvP
            // 
            this.label15PvP.AutoSize = true;
            this.label15PvP.Location = new System.Drawing.Point(5, 8);
            this.label15PvP.Name = "label15PvP";
            this.label15PvP.Size = new System.Drawing.Size(143, 17);
            this.label15PvP.TabIndex = 0;
            this.label15PvP.Text = "PvP Damage settings";
            // 
            // PvPlblDamageExample
            // 
            this.PvPlblDamageExample.AutoSize = true;
            this.PvPlblDamageExample.Location = new System.Drawing.Point(11, 485);
            this.PvPlblDamageExample.Name = "PvPlblDamageExample";
            this.PvPlblDamageExample.Size = new System.Drawing.Size(117, 17);
            this.PvPlblDamageExample.TabIndex = 0;
            this.PvPlblDamageExample.Text = "Damage example";
            // 
            // PvPlblDamageExplained
            // 
            this.PvPlblDamageExplained.AutoSize = true;
            this.PvPlblDamageExplained.Location = new System.Drawing.Point(11, 468);
            this.PvPlblDamageExplained.Name = "PvPlblDamageExplained";
            this.PvPlblDamageExplained.Size = new System.Drawing.Size(65, 17);
            this.PvPlblDamageExplained.TabIndex = 9;
            this.PvPlblDamageExplained.Text = "Damage:";
            // 
            // MagiclistBox
            // 
            this.MagiclistBox.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.MagiclistBox.FormattingEnabled = true;
            this.MagiclistBox.ItemHeight = 16;
            this.MagiclistBox.Location = new System.Drawing.Point(3, 33);
            this.MagiclistBox.Name = "MagiclistBox";
            this.MagiclistBox.Size = new System.Drawing.Size(225, 564);
            this.MagiclistBox.TabIndex = 0;
            this.MagiclistBox.SelectedIndexChanged += new System.EventHandler(this.MagiclistBox_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(250, 8);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(721, 589);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.PvPlblDamageExample);
            this.tabPage1.Controls.Add(this.PvPlblDamageExplained);
            this.tabPage1.Controls.Add(this.panel5);
            this.tabPage1.Controls.Add(this.label24);
            this.tabPage1.Controls.Add(this.label23);
            this.tabPage1.Controls.Add(this.textBoxName);
            this.tabPage1.Controls.Add(this.lblDamageExample);
            this.tabPage1.Controls.Add(this.lblDamageExplained);
            this.tabPage1.Controls.Add(this.lblSelected);
            this.tabPage1.Controls.Add(this.panel4);
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.txtSkillIcon);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lblBookValid);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(713, 560);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basics";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(20, 23);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(74, 17);
            this.label24.TabIndex = 12;
            this.label24.Text = "SkillName:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(181, 3);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(43, 17);
            this.label23.TabIndex = 11;
            this.label23.Text = "book:";
            // 
            // textBoxName
            // 
            this.textBoxName.Location = new System.Drawing.Point(89, 18);
            this.textBoxName.Name = "textBoxName";
            this.textBoxName.Size = new System.Drawing.Size(112, 22);
            this.textBoxName.TabIndex = 10;
            this.textBoxName.TextChanged += new System.EventHandler(this.textBoxName_TextChanged);
            // 
            // lblDamageExample
            // 
            this.lblDamageExample.AutoSize = true;
            this.lblDamageExample.Location = new System.Drawing.Point(11, 411);
            this.lblDamageExample.Name = "lblDamageExample";
            this.lblDamageExample.Size = new System.Drawing.Size(117, 17);
            this.lblDamageExample.TabIndex = 0;
            this.lblDamageExample.Text = "Damage example";
            // 
            // lblDamageExplained
            // 
            this.lblDamageExplained.AutoSize = true;
            this.lblDamageExplained.Location = new System.Drawing.Point(11, 394);
            this.lblDamageExplained.Name = "lblDamageExplained";
            this.lblDamageExplained.Size = new System.Drawing.Size(65, 17);
            this.lblDamageExplained.TabIndex = 9;
            this.lblDamageExplained.Text = "Damage:";
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Location = new System.Drawing.Point(20, 3);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(98, 17);
            this.lblSelected.TabIndex = 8;
            this.lblSelected.Text = "Selected skill: ";
            // 
            // panel4
            // 
            this.panel4.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel4.Controls.Add(this.txtDmgMultBoost);
            this.panel4.Controls.Add(this.txtDmgMultBase);
            this.panel4.Controls.Add(this.label21);
            this.panel4.Controls.Add(this.label22);
            this.panel4.Controls.Add(this.txtDmgBonusMax);
            this.panel4.Controls.Add(this.txtDmgBonusMin);
            this.panel4.Controls.Add(this.label18);
            this.panel4.Controls.Add(this.label19);
            this.panel4.Controls.Add(this.txtDmgBaseMax);
            this.panel4.Controls.Add(this.txtDmgBaseMin);
            this.panel4.Controls.Add(this.label17);
            this.panel4.Controls.Add(this.label16);
            this.panel4.Controls.Add(this.label15);
            this.panel4.Location = new System.Drawing.Point(477, 6);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(233, 191);
            this.panel4.TabIndex = 6;
            // 
            // txtDmgMultBoost
            // 
            this.txtDmgMultBoost.Location = new System.Drawing.Point(168, 157);
            this.txtDmgMultBoost.Name = "txtDmgMultBoost";
            this.txtDmgMultBoost.Size = new System.Drawing.Size(46, 22);
            this.txtDmgMultBoost.TabIndex = 14;
            this.toolTip1.SetToolTip(this.txtDmgMultBoost, "extra multiplyer apply\'d for every skill level");
            this.txtDmgMultBoost.TextChanged += new System.EventHandler(this.txtDmgMultBoost_TextChanged);
            // 
            // txtDmgMultBase
            // 
            this.txtDmgMultBase.Location = new System.Drawing.Point(168, 131);
            this.txtDmgMultBase.Name = "txtDmgMultBase";
            this.txtDmgMultBase.Size = new System.Drawing.Size(46, 22);
            this.txtDmgMultBase.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txtDmgMultBase, "multiplier apply\'d to total skill dmg");
            this.txtDmgMultBase.TextChanged += new System.EventHandler(this.txtDmgMultBase_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(12, 160);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(204, 17);
            this.label21.TabIndex = 12;
            this.label21.Text = "Damage multiplyer boost/skilllvl";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 134);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(160, 17);
            this.label22.TabIndex = 11;
            this.label22.Text = "Damage multiplyer base";
            // 
            // txtDmgBonusMax
            // 
            this.txtDmgBonusMax.Location = new System.Drawing.Point(168, 105);
            this.txtDmgBonusMax.Name = "txtDmgBonusMax";
            this.txtDmgBonusMax.Size = new System.Drawing.Size(46, 22);
            this.txtDmgBonusMax.TabIndex = 10;
            this.toolTip1.SetToolTip(this.txtDmgBonusMax, "Damage bonus at skill level \'4\' ");
            this.txtDmgBonusMax.TextChanged += new System.EventHandler(this.txtDmgBonusMax_TextChanged);
            // 
            // txtDmgBonusMin
            // 
            this.txtDmgBonusMin.Location = new System.Drawing.Point(168, 79);
            this.txtDmgBonusMin.Name = "txtDmgBonusMin";
            this.txtDmgBonusMin.Size = new System.Drawing.Size(46, 22);
            this.txtDmgBonusMin.TabIndex = 9;
            this.toolTip1.SetToolTip(this.txtDmgBonusMin, "Damage bonus at skill level \'4\' \r\nyou will get 1/4th of this bonus for every skil" +
        "l level\r\nnote ingame level 0 = 1 bonus, so level 3 = max bonus (4)");
            this.txtDmgBonusMin.TextChanged += new System.EventHandler(this.txtDmgBonusMin_TextChanged);
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(12, 108);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(177, 17);
            this.label18.TabIndex = 8;
            this.label18.Text = "Maximum skill lvl 3 damage";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 82);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(178, 17);
            this.label19.TabIndex = 7;
            this.label19.Text = "Minimum skill lvl 3 damage:";
            // 
            // txtDmgBaseMax
            // 
            this.txtDmgBaseMax.Location = new System.Drawing.Point(168, 53);
            this.txtDmgBaseMax.Name = "txtDmgBaseMax";
            this.txtDmgBaseMax.Size = new System.Drawing.Size(46, 22);
            this.txtDmgBaseMax.TabIndex = 6;
            this.toolTip1.SetToolTip(this.txtDmgBaseMax, "Damage at skill level 0");
            this.txtDmgBaseMax.TextChanged += new System.EventHandler(this.txtDmgBaseMax_TextChanged);
            // 
            // txtDmgBaseMin
            // 
            this.txtDmgBaseMin.Location = new System.Drawing.Point(168, 27);
            this.txtDmgBaseMin.Name = "txtDmgBaseMin";
            this.txtDmgBaseMin.Size = new System.Drawing.Size(46, 22);
            this.txtDmgBaseMin.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtDmgBaseMin, "Damage at skill level 0");
            this.txtDmgBaseMin.TextChanged += new System.EventHandler(this.txtDmgBaseMin_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 56);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(156, 17);
            this.label17.TabIndex = 2;
            this.label17.Text = "Maximum base damage";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 30);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(157, 17);
            this.label16.TabIndex = 1;
            this.label16.Text = "Minimum base damage:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 8);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(114, 17);
            this.label15.TabIndex = 0;
            this.label15.Text = "Damage settings";
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.label20);
            this.panel3.Controls.Add(this.txtRange);
            this.panel3.Controls.Add(this.txtDelayReduction);
            this.panel3.Controls.Add(this.txtDelayBase);
            this.panel3.Controls.Add(this.label14);
            this.panel3.Controls.Add(this.label13);
            this.panel3.Controls.Add(this.label12);
            this.panel3.Location = new System.Drawing.Point(253, 166);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(216, 191);
            this.panel3.TabIndex = 5;
            this.toolTip1.SetToolTip(this.panel3, "delay = <base delay> - (skill level * <decrease>)");
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(12, 77);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(122, 17);
            this.label20.TabIndex = 15;
            this.label20.Text = "Range (0 No limit)";
            // 
            // txtRange
            // 
            this.txtRange.Location = new System.Drawing.Point(121, 74);
            this.txtRange.Name = "txtRange";
            this.txtRange.Size = new System.Drawing.Size(79, 22);
            this.txtRange.TabIndex = 14;
            this.txtRange.TextChanged += new System.EventHandler(this.txtRange_TextChanged);
            // 
            // txtDelayReduction
            // 
            this.txtDelayReduction.Location = new System.Drawing.Point(121, 47);
            this.txtDelayReduction.Name = "txtDelayReduction";
            this.txtDelayReduction.Size = new System.Drawing.Size(79, 22);
            this.txtDelayReduction.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txtDelayReduction, "delay = <base delay> - (skill level * <decrease>)");
            this.txtDelayReduction.TextChanged += new System.EventHandler(this.txtDelayReduction_TextChanged);
            // 
            // txtDelayBase
            // 
            this.txtDelayBase.Location = new System.Drawing.Point(121, 22);
            this.txtDelayBase.Name = "txtDelayBase";
            this.txtDelayBase.Size = new System.Drawing.Size(79, 22);
            this.txtDelayBase.TabIndex = 12;
            this.toolTip1.SetToolTip(this.txtDelayBase, "delay = <base delay> - (skill level * <decrease>)");
            this.txtDelayBase.TextChanged += new System.EventHandler(this.txtDelayBase_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 50);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(137, 17);
            this.label14.TabIndex = 2;
            this.label14.Text = "Decrease / skill level";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(78, 17);
            this.label13.TabIndex = 1;
            this.label13.Text = "Base delay";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(152, 17);
            this.label12.TabIndex = 0;
            this.label12.Text = "Delay (in milliseconds!)";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.txtMPIncrease);
            this.panel2.Controls.Add(this.txtMPBase);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Location = new System.Drawing.Point(253, 53);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(216, 107);
            this.panel2.TabIndex = 4;
            // 
            // txtMPIncrease
            // 
            this.txtMPIncrease.Location = new System.Drawing.Point(135, 47);
            this.txtMPIncrease.Name = "txtMPIncrease";
            this.txtMPIncrease.Size = new System.Drawing.Size(46, 22);
            this.txtMPIncrease.TabIndex = 12;
            this.toolTip1.SetToolTip(this.txtMPIncrease, "extra amount of mp used each level");
            this.txtMPIncrease.TextChanged += new System.EventHandler(this.txtMPIncrease_TextChanged);
            // 
            // txtMPBase
            // 
            this.txtMPBase.Location = new System.Drawing.Point(135, 22);
            this.txtMPBase.Name = "txtMPBase";
            this.txtMPBase.Size = new System.Drawing.Size(46, 22);
            this.txtMPBase.TabIndex = 11;
            this.toolTip1.SetToolTip(this.txtMPBase, "Mp usage when skill is level 0");
            this.txtMPBase.TextChanged += new System.EventHandler(this.txtMPBase_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(154, 17);
            this.label11.TabIndex = 2;
            this.label11.Text = "MP increase each level";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(106, 17);
            this.label10.TabIndex = 1;
            this.label10.Text = "Base mp usage";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 17);
            this.label9.TabIndex = 0;
            this.label9.Text = "MP usage";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.txtSkillLvl10Points);
            this.panel1.Controls.Add(this.txtSkillLvl9Points);
            this.panel1.Controls.Add(this.txtSkillLvl8Points);
            this.panel1.Controls.Add(this.txtSkillLvl7Points);
            this.panel1.Controls.Add(this.txtSkillLvl6Points);
            this.panel1.Controls.Add(this.txtSkillLvl5Points);
            this.panel1.Controls.Add(this.txtSkillLvl4Points);
            this.panel1.Controls.Add(this.txtSkillLvl3Points);
            this.panel1.Controls.Add(this.txtSkillLvl2Points);
            this.panel1.Controls.Add(this.txtSkillLvl1Points);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtSkillLvl10Req);
            this.panel1.Controls.Add(this.txtSkillLvl9Req);
            this.panel1.Controls.Add(this.txtSkillLvl8Req);
            this.panel1.Controls.Add(this.txtSkillLvl7Req);
            this.panel1.Controls.Add(this.txtSkillLvl6Req);
            this.panel1.Controls.Add(this.txtSkillLvl5Req);
            this.panel1.Controls.Add(this.txtSkillLvl4Req);
            this.panel1.Controls.Add(this.txtSkillLvl3Req);
            this.panel1.Controls.Add(this.txtSkillLvl2Req);
            this.panel1.Controls.Add(this.txtSkillLvl1Req);
            this.panel1.Controls.Add(this.label31);
            this.panel1.Controls.Add(this.label30);
            this.panel1.Controls.Add(this.label29);
            this.panel1.Controls.Add(this.label28);
            this.panel1.Controls.Add(this.label27);
            this.panel1.Controls.Add(this.label26);
            this.panel1.Controls.Add(this.label25);
            this.panel1.Controls.Add(this.label38);
            this.panel1.Controls.Add(this.label37);
            this.panel1.Controls.Add(this.label36);
            this.panel1.Controls.Add(this.label35);
            this.panel1.Controls.Add(this.label34);
            this.panel1.Controls.Add(this.label33);
            this.panel1.Controls.Add(this.label32);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(13, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 304);
            this.panel1.TabIndex = 3;
            // 
            // txtSkillLvl10Points
            // 
            this.txtSkillLvl10Points.Location = new System.Drawing.Point(169, 247);
            this.txtSkillLvl10Points.Name = "txtSkillLvl10Points";
            this.txtSkillLvl10Points.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl10Points.TabIndex = 10;
            this.txtSkillLvl10Points.TextChanged += new System.EventHandler(this.txtSkillLvl10Points_TextChanged);
            // 
            // txtSkillLvl9Points
            // 
            this.txtSkillLvl9Points.Location = new System.Drawing.Point(169, 222);
            this.txtSkillLvl9Points.Name = "txtSkillLvl9Points";
            this.txtSkillLvl9Points.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl9Points.TabIndex = 10;
            this.txtSkillLvl9Points.TextChanged += new System.EventHandler(this.txtSkillLvl9Points_TextChanged);
            // 
            // txtSkillLvl8Points
            // 
            this.txtSkillLvl8Points.Location = new System.Drawing.Point(169, 197);
            this.txtSkillLvl8Points.Name = "txtSkillLvl8Points";
            this.txtSkillLvl8Points.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl8Points.TabIndex = 10;
            this.txtSkillLvl8Points.TextChanged += new System.EventHandler(this.txtSkillLvl8Points_TextChanged);
            // 
            // txtSkillLvl7Points
            // 
            this.txtSkillLvl7Points.Location = new System.Drawing.Point(169, 172);
            this.txtSkillLvl7Points.Name = "txtSkillLvl7Points";
            this.txtSkillLvl7Points.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl7Points.TabIndex = 10;
            this.txtSkillLvl7Points.TextChanged += new System.EventHandler(this.txtSkillLvl7Points_TextChanged);
            // 
            // txtSkillLvl6Points
            // 
            this.txtSkillLvl6Points.Location = new System.Drawing.Point(169, 147);
            this.txtSkillLvl6Points.Name = "txtSkillLvl6Points";
            this.txtSkillLvl6Points.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl6Points.TabIndex = 10;
            this.txtSkillLvl6Points.TextChanged += new System.EventHandler(this.txtSkillLvl6Points_TextChanged);
            // 
            // txtSkillLvl5Points
            // 
            this.txtSkillLvl5Points.Location = new System.Drawing.Point(169, 122);
            this.txtSkillLvl5Points.Name = "txtSkillLvl5Points";
            this.txtSkillLvl5Points.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl5Points.TabIndex = 10;
            this.txtSkillLvl5Points.TextChanged += new System.EventHandler(this.txtSkillLvl5Points_TextChanged);
            // 
            // txtSkillLvl4Points
            // 
            this.txtSkillLvl4Points.Location = new System.Drawing.Point(169, 97);
            this.txtSkillLvl4Points.Name = "txtSkillLvl4Points";
            this.txtSkillLvl4Points.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl4Points.TabIndex = 10;
            this.txtSkillLvl4Points.TextChanged += new System.EventHandler(this.txtSkillLvl4Points_TextChanged);
            // 
            // txtSkillLvl3Points
            // 
            this.txtSkillLvl3Points.Location = new System.Drawing.Point(169, 72);
            this.txtSkillLvl3Points.Name = "txtSkillLvl3Points";
            this.txtSkillLvl3Points.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl3Points.TabIndex = 12;
            this.txtSkillLvl3Points.TextChanged += new System.EventHandler(this.txtSkillLvl3Points_TextChanged);
            // 
            // txtSkillLvl2Points
            // 
            this.txtSkillLvl2Points.Location = new System.Drawing.Point(169, 47);
            this.txtSkillLvl2Points.Name = "txtSkillLvl2Points";
            this.txtSkillLvl2Points.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl2Points.TabIndex = 11;
            this.txtSkillLvl2Points.TextChanged += new System.EventHandler(this.txtSkillLvl2Points_TextChanged);
            // 
            // txtSkillLvl1Points
            // 
            this.txtSkillLvl1Points.Location = new System.Drawing.Point(169, 22);
            this.txtSkillLvl1Points.Name = "txtSkillLvl1Points";
            this.txtSkillLvl1Points.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl1Points.TabIndex = 10;
            this.txtSkillLvl1Points.TextChanged += new System.EventHandler(this.txtSkillLvl1Points_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(110, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(75, 17);
            this.label6.TabIndex = 9;
            this.label6.Text = "Skill points";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(110, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 17);
            this.label7.TabIndex = 8;
            this.label7.Text = "Skill points";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(110, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(75, 17);
            this.label8.TabIndex = 7;
            this.label8.Text = "Skill points";
            // 
            // txtSkillLvl10Req
            // 
            this.txtSkillLvl10Req.Location = new System.Drawing.Point(57, 247);
            this.txtSkillLvl10Req.Name = "txtSkillLvl10Req";
            this.txtSkillLvl10Req.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl10Req.TabIndex = 6;
            this.txtSkillLvl10Req.TextChanged += new System.EventHandler(this.txtSkillLvl10Req_TextChanged);
            // 
            // txtSkillLvl9Req
            // 
            this.txtSkillLvl9Req.Location = new System.Drawing.Point(57, 222);
            this.txtSkillLvl9Req.Name = "txtSkillLvl9Req";
            this.txtSkillLvl9Req.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl9Req.TabIndex = 6;
            this.txtSkillLvl9Req.TextChanged += new System.EventHandler(this.txtSkillLvl9Req_TextChanged);
            // 
            // txtSkillLvl8Req
            // 
            this.txtSkillLvl8Req.Location = new System.Drawing.Point(57, 197);
            this.txtSkillLvl8Req.Name = "txtSkillLvl8Req";
            this.txtSkillLvl8Req.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl8Req.TabIndex = 6;
            this.txtSkillLvl8Req.TextChanged += new System.EventHandler(this.txtSkillLvl8Req_TextChanged);
            // 
            // txtSkillLvl7Req
            // 
            this.txtSkillLvl7Req.Location = new System.Drawing.Point(57, 172);
            this.txtSkillLvl7Req.Name = "txtSkillLvl7Req";
            this.txtSkillLvl7Req.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl7Req.TabIndex = 6;
            this.txtSkillLvl7Req.TextChanged += new System.EventHandler(this.txtSkillLvl7Req_TextChanged);
            // 
            // txtSkillLvl6Req
            // 
            this.txtSkillLvl6Req.Location = new System.Drawing.Point(57, 147);
            this.txtSkillLvl6Req.Name = "txtSkillLvl6Req";
            this.txtSkillLvl6Req.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl6Req.TabIndex = 6;
            this.txtSkillLvl6Req.TextChanged += new System.EventHandler(this.txtSkillLvl6Req_TextChanged);
            // 
            // txtSkillLvl5Req
            // 
            this.txtSkillLvl5Req.Location = new System.Drawing.Point(57, 122);
            this.txtSkillLvl5Req.Name = "txtSkillLvl5Req";
            this.txtSkillLvl5Req.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl5Req.TabIndex = 6;
            this.txtSkillLvl5Req.TextChanged += new System.EventHandler(this.txtSkillLvl5Req_TextChanged);
            // 
            // txtSkillLvl4Req
            // 
            this.txtSkillLvl4Req.Location = new System.Drawing.Point(57, 97);
            this.txtSkillLvl4Req.Name = "txtSkillLvl4Req";
            this.txtSkillLvl4Req.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl4Req.TabIndex = 6;
            this.txtSkillLvl4Req.TextChanged += new System.EventHandler(this.txtSkillLvl4Req_TextChanged);
            // 
            // txtSkillLvl3Req
            // 
            this.txtSkillLvl3Req.Location = new System.Drawing.Point(57, 72);
            this.txtSkillLvl3Req.Name = "txtSkillLvl3Req";
            this.txtSkillLvl3Req.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl3Req.TabIndex = 6;
            this.txtSkillLvl3Req.TextChanged += new System.EventHandler(this.txtSkillLvl3Req_TextChanged);
            // 
            // txtSkillLvl2Req
            // 
            this.txtSkillLvl2Req.Location = new System.Drawing.Point(57, 47);
            this.txtSkillLvl2Req.Name = "txtSkillLvl2Req";
            this.txtSkillLvl2Req.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl2Req.TabIndex = 5;
            this.txtSkillLvl2Req.TextChanged += new System.EventHandler(this.txtSkillLvl2Req_TextChanged);
            // 
            // txtSkillLvl1Req
            // 
            this.txtSkillLvl1Req.Location = new System.Drawing.Point(57, 22);
            this.txtSkillLvl1Req.Name = "txtSkillLvl1Req";
            this.txtSkillLvl1Req.Size = new System.Drawing.Size(46, 22);
            this.txtSkillLvl1Req.TabIndex = 4;
            this.txtSkillLvl1Req.TextChanged += new System.EventHandler(this.txtSkillLvl1Req_TextChanged);
            // 
            // label31
            // 
            this.label31.AutoSize = true;
            this.label31.Location = new System.Drawing.Point(13, 250);
            this.label31.Name = "label31";
            this.label31.Size = new System.Drawing.Size(57, 17);
            this.label31.TabIndex = 3;
            this.label31.Text = "level 10";
            // 
            // label30
            // 
            this.label30.AutoSize = true;
            this.label30.Location = new System.Drawing.Point(13, 225);
            this.label30.Name = "label30";
            this.label30.Size = new System.Drawing.Size(49, 17);
            this.label30.TabIndex = 3;
            this.label30.Text = "level 9";
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(13, 200);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(49, 17);
            this.label29.TabIndex = 3;
            this.label29.Text = "level 8";
            // 
            // label28
            // 
            this.label28.AutoSize = true;
            this.label28.Location = new System.Drawing.Point(13, 175);
            this.label28.Name = "label28";
            this.label28.Size = new System.Drawing.Size(49, 17);
            this.label28.TabIndex = 3;
            this.label28.Text = "level 7";
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(13, 150);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(49, 17);
            this.label27.TabIndex = 3;
            this.label27.Text = "level 6";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(13, 125);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(49, 17);
            this.label26.TabIndex = 3;
            this.label26.Text = "level 5";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(13, 100);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(49, 17);
            this.label25.TabIndex = 3;
            this.label25.Text = "level 4";
            // 
            // label38
            // 
            this.label38.AutoSize = true;
            this.label38.Location = new System.Drawing.Point(110, 250);
            this.label38.Name = "label38";
            this.label38.Size = new System.Drawing.Size(75, 17);
            this.label38.TabIndex = 9;
            this.label38.Text = "Skill points";
            // 
            // label37
            // 
            this.label37.AutoSize = true;
            this.label37.Location = new System.Drawing.Point(110, 225);
            this.label37.Name = "label37";
            this.label37.Size = new System.Drawing.Size(75, 17);
            this.label37.TabIndex = 9;
            this.label37.Text = "Skill points";
            // 
            // label36
            // 
            this.label36.AutoSize = true;
            this.label36.Location = new System.Drawing.Point(110, 200);
            this.label36.Name = "label36";
            this.label36.Size = new System.Drawing.Size(75, 17);
            this.label36.TabIndex = 9;
            this.label36.Text = "Skill points";
            // 
            // label35
            // 
            this.label35.AutoSize = true;
            this.label35.Location = new System.Drawing.Point(110, 175);
            this.label35.Name = "label35";
            this.label35.Size = new System.Drawing.Size(75, 17);
            this.label35.TabIndex = 9;
            this.label35.Text = "Skill points";
            // 
            // label34
            // 
            this.label34.AutoSize = true;
            this.label34.Location = new System.Drawing.Point(110, 150);
            this.label34.Name = "label34";
            this.label34.Size = new System.Drawing.Size(75, 17);
            this.label34.TabIndex = 9;
            this.label34.Text = "Skill points";
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(110, 125);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(75, 17);
            this.label33.TabIndex = 9;
            this.label33.Text = "Skill points";
            // 
            // label32
            // 
            this.label32.AutoSize = true;
            this.label32.Location = new System.Drawing.Point(110, 100);
            this.label32.Name = "label32";
            this.label32.Size = new System.Drawing.Size(75, 17);
            this.label32.TabIndex = 9;
            this.label32.Text = "Skill points";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(49, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "level 3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(49, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "level 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(49, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "level 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(211, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Skill level increase requirements";
            // 
            // txtSkillIcon
            // 
            this.txtSkillIcon.Location = new System.Drawing.Point(311, 20);
            this.txtSkillIcon.Name = "txtSkillIcon";
            this.txtSkillIcon.Size = new System.Drawing.Size(41, 22);
            this.txtSkillIcon.TabIndex = 2;
            this.txtSkillIcon.TextChanged += new System.EventHandler(this.txtSkillIcon_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(250, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(71, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Skill icon: ";
            // 
            // lblBookValid
            // 
            this.lblBookValid.AutoSize = true;
            this.lblBookValid.Location = new System.Drawing.Point(222, 3);
            this.lblBookValid.Name = "lblBookValid";
            this.lblBookValid.Size = new System.Drawing.Size(135, 17);
            this.lblBookValid.TabIndex = 0;
            this.lblBookValid.Text = "Searching for books";
            // 
            // MagicInfoForm
            // 
            this.ClientSize = new System.Drawing.Size(984, 604);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.MagiclistBox);
            this.Name = "MagicInfoForm";
            this.Text = "Magic Settings";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MagicInfoForm_FormClosed);
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.panel4.ResumeLayout(false);
            this.panel4.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        private void MagicInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            //do something to save it all
            Envir.SaveDB();
        }

        private void MagiclistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMagicForm();
        }

        private bool IsValid(ref byte input)
        {
            if (!byte.TryParse(ActiveControl.Text, out input))
            {
                ActiveControl.BackColor = Color.Red;
                return false;
            }
            return true;
        }
        private bool IsValid(ref ushort input)
        {
            if (!ushort.TryParse(ActiveControl.Text, out input))
            {
                ActiveControl.BackColor = Color.Red;
                return false;
            }
            return true;
        }

        private bool IsValid(ref uint input)
        {
            if (!uint.TryParse(ActiveControl.Text, out input))
            {
                ActiveControl.BackColor = Color.Red;
                return false;
            }
            return true;
        }

        private bool IsValid(ref float input)
        {
            if (!float.TryParse(ActiveControl.Text, out input))
            {
                ActiveControl.BackColor = Color.Red;
                return false;
            }
            return true;
        }

        private void txtSkillIcon_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Icon = temp;
        }

        private void txtSkillLvl1Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level1 = temp;
        }

        private void txtSkillLvl2Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level2 = temp;
        }

        private void txtSkillLvl3Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level3 = temp;
        }
        private void txtSkillLvl4Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level4 = temp;
        }
        private void txtSkillLvl5Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level5 = temp;
        }
        private void txtSkillLvl6Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level6 = temp;
        }
        private void txtSkillLvl7Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level7 = temp;
        }
        private void txtSkillLvl8Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level8 = temp;
        }
        private void txtSkillLvl9Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level9 = temp;
        }
        private void txtSkillLvl10Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level10 = temp;
        }

        private void txtSkillLvl1Points_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need1 = temp;
        }

        private void txtSkillLvl2Points_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need2 = temp;
        }

        private void txtSkillLvl3Points_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need3 = temp;
        }
        private void txtSkillLvl4Points_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need4 = temp;
        }
        private void txtSkillLvl5Points_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need5 = temp;
        }
        private void txtSkillLvl6Points_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need6 = temp;
        }
        private void txtSkillLvl7Points_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need7 = temp;
        }
        private void txtSkillLvl8Points_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need8 = temp;
        }
        private void txtSkillLvl9Points_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need9 = temp;
        }
        private void txtSkillLvl10Points_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need10 = temp;
        }

        private void txtMPBase_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.BaseCost = temp;
        }

        private void txtMPIncrease_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.LevelCost = temp;
        }

        private void txtDmgBaseMin_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.PowerBase = temp;
            UpdateMagicForm();
        }

        private void txtDmgBaseMax_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;
            if (temp < _selectedMagicInfo.PowerBase)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.PowerBonus =  (ushort)(temp - _selectedMagicInfo.PowerBase);
            UpdateMagicForm();
        }

        private void txtDmgBonusMin_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.MPowerBase = temp;
            UpdateMagicForm();
        }

        private void txtDmgBonusMax_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;
            if (temp < _selectedMagicInfo.MPowerBase)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.MPowerBonus = (ushort)(temp - _selectedMagicInfo.MPowerBase);
            UpdateMagicForm();
        }

        private void txtDmgMultBase_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            float temp = 0;
            if (!IsValid(ref temp)) return;


            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.MultiplierBase = temp;
            UpdateMagicForm(1);
        }

        private void txtDmgMultBoost_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            float temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.MultiplierBonus = temp;
            UpdateMagicForm(2);
        }

        private void txtDelayBase_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            uint temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.DelayBase = temp;
        }

        private void txtDelayReduction_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            uint temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.DelayReduction = temp;
        }

        private void txtRange_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp = 0;
            if (!IsValid(ref temp)) return;
            
            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Range = temp;
        }

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            _selectedMagicInfo.Name = ActiveControl.Text;
            UpdateMagicForm();
            if (ActiveControl.Text == "")
            {
                ActiveControl.BackColor = Color.Red;
            }
            else {
                ActiveControl.BackColor = SystemColors.Window;              
            }            
        }
        #region PvP Damage

        private void txtPvPDmgBaseMin_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.PvPPowerBase = temp;
            UpdateMagicForm();
        }

        private void txtPvPDmgBaseMax_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;
            if (temp < _selectedMagicInfo.PvPPowerBase)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.PvPPowerBonus = (ushort)(temp - _selectedMagicInfo.PvPPowerBase);
            UpdateMagicForm();
        }

        private void txtPvPDmgBonusMin_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.PvPMPowerBase = temp;
            UpdateMagicForm();
        }

        private void txtPvPDmgBonusMax_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;
            if (temp < _selectedMagicInfo.PvPMPowerBase)
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.PvPMPowerBonus = (ushort)(temp - _selectedMagicInfo.PvPMPowerBase);
            UpdateMagicForm();
        }

        private void txtPvPDmgMultBase_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            float temp = 0;
            if (!IsValid(ref temp)) return;


            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.PvPMultiplierBase = temp;
            UpdateMagicForm(3);
        }

        private void txtPvPDmgMultBoost_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            float temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.PvPMultiplierBonus = temp;
            UpdateMagicForm(4);
        }
        #endregion

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            UpdateMagicList();
        }
    }
}
