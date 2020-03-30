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
                 txtSkillIcon.Text = "0";
                 txtSkillLvl1Points.Text = "0";
                 txtSkillLvl1Req.Text = "0";
                 txtSkillLvl2Points.Text = "0";
                 txtSkillLvl2Req.Text = "0";
                 txtSkillLvl3Points.Text = "0";
                 txtSkillLvl3Req.Text = "0";
                 txtMPBase.Text = "0";
                 txtMPIncrease.Text = "0";
                 txtDelayBase.Text = "0";
                 txtDelayReduction.Text = "0";
                 txtDmgBaseMin.Text = "0";
                 txtDmgBaseMax.Text = "0";
                 txtDmgBonusMin.Text = "0";
                 txtDmgBonusMax.Text = "0";
                 textBox1.Text = "0";
                 textBox2.Text = "0";
            }
             else
             {
                 tabControl1.Enabled = true;
                 lblSelected.Text = "Selected Skill: " + _selectedMagicInfo.ToString();

                 lblDamageExample.Text = string.Format("Damage @ Skill level 0: {0:000}-{1:000}   |||   level 1: {2:000}-{3:000}   |||   level 2: {4:000}-{5:000}   |||   level 3: {6:000}-{7:000} |||   level 4: {8:000}-{9:000}", GetMinPower(0), GetMaxPower(0), GetMinPower(1), GetMaxPower(1), GetMinPower(2), GetMaxPower(2), GetMinPower(3), GetMaxPower(3), GetMinPower(4), GetMaxPower(4));
                 lblDamageExplained.Text = string.Format("Damage: {{Random(minstat-maxstat) + [<(random({0}-{1})/4) X (skill level +1)> + random<{2}-{3}>]}}  X  {{{4} + (skill level * {5})}}", _selectedMagicInfo.MPowerBase, _selectedMagicInfo.MPowerBase + _selectedMagicInfo.MPowerBonus, _selectedMagicInfo.PowerBase, _selectedMagicInfo.PowerBonus + _selectedMagicInfo.PowerBase, _selectedMagicInfo.MultiplierBase, _selectedMagicInfo.MultiplierBonus);

                 txtSkillIcon.Text = _selectedMagicInfo.Icon.ToString();
                 txtSkillLvl1Points.Text = _selectedMagicInfo.Need1.ToString();
                 txtSkillLvl1Req.Text = _selectedMagicInfo.Level1.ToString();
                 txtSkillLvl2Points.Text = _selectedMagicInfo.Need2.ToString();
                 txtSkillLvl2Req.Text = _selectedMagicInfo.Level2.ToString();
                 txtSkillLvl3Points.Text = _selectedMagicInfo.Need3.ToString();
                 txtSkillLvl3Req.Text = _selectedMagicInfo.Level3.ToString();
                 textBox1.Text = _selectedMagicInfo.Need4.ToString();
                 textBox2.Text = _selectedMagicInfo.Level4.ToString();
                 txtMPBase.Text = _selectedMagicInfo.BaseCost.ToString();
                 txtMPIncrease.Text = _selectedMagicInfo.LevelCost.ToString();
                 txtDelayBase.Text = _selectedMagicInfo.DelayBase.ToString();
                 txtDelayReduction.Text = _selectedMagicInfo.DelayReduction.ToString();
                 txtDmgBaseMin.Text = _selectedMagicInfo.PowerBase.ToString();
                 txtDmgBaseMax.Text = (_selectedMagicInfo.PowerBase + _selectedMagicInfo.PowerBonus).ToString();
                 txtDmgBonusMin.Text = _selectedMagicInfo.MPowerBase.ToString();
                 txtDmgBonusMax.Text = (_selectedMagicInfo.MPowerBase + _selectedMagicInfo.MPowerBonus).ToString();
                 if (field != 1)
                    txtDmgMultBase.Text = _selectedMagicInfo.MultiplierBase.ToString();
                 if (field != 2)
                 txtDmgMultBoost.Text = _selectedMagicInfo.MultiplierBonus.ToString();
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

                checkBox1.Checked = _selectedMagicInfo.HumUpTrain;
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

        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.MagiclistBox = new System.Windows.Forms.ListBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.label24 = new System.Windows.Forms.Label();
            this.label23 = new System.Windows.Forms.Label();
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
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.txtSkillLvl3Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl2Points = new System.Windows.Forms.TextBox();
            this.txtSkillLvl1Points = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.txtSkillLvl3Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl2Req = new System.Windows.Forms.TextBox();
            this.txtSkillLvl1Req = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.txtSkillIcon = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblBookValid = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.label25 = new System.Windows.Forms.Label();
            this.label26 = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.panel4.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // MagiclistBox
            // 
            this.MagiclistBox.Dock = System.Windows.Forms.DockStyle.Left;
            this.MagiclistBox.FormattingEnabled = true;
            this.MagiclistBox.ItemHeight = 12;
            this.MagiclistBox.Location = new System.Drawing.Point(0, 0);
            this.MagiclistBox.Name = "MagiclistBox";
            this.MagiclistBox.Size = new System.Drawing.Size(225, 678);
            this.MagiclistBox.TabIndex = 0;
            this.MagiclistBox.SelectedIndexChanged += new System.EventHandler(this.MagiclistBox_SelectedIndexChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(225, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(826, 678);
            this.tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.lblDamageExample);
            this.tabPage1.Controls.Add(this.lblDamageExplained);
            this.tabPage1.Controls.Add(this.lblSelected);
            this.tabPage1.Controls.Add(this.panel4);
            this.tabPage1.Controls.Add(this.label23);
            this.tabPage1.Controls.Add(this.panel3);
            this.tabPage1.Controls.Add(this.panel2);
            this.tabPage1.Controls.Add(this.label24);
            this.tabPage1.Controls.Add(this.panel1);
            this.tabPage1.Controls.Add(this.txtSkillIcon);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.lblBookValid);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(818, 652);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Basics";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            this.label24.AutoSize = true;
            this.label24.Location = new System.Drawing.Point(14, 38);
            this.label24.Name = "label24";
            this.label24.Size = new System.Drawing.Size(59, 12);
            this.label24.TabIndex = 12;
            this.label24.Text = "技能名字:";
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(168, 38);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(23, 12);
            this.label23.TabIndex = 11;
            this.label23.Text = "书:";
            // 
            // lblDamageExample
            // 
            this.lblDamageExample.AutoSize = true;
            this.lblDamageExample.Location = new System.Drawing.Point(11, 464);
            this.lblDamageExample.Name = "lblDamageExample";
            this.lblDamageExample.Size = new System.Drawing.Size(89, 12);
            this.lblDamageExample.TabIndex = 0;
            this.lblDamageExample.Text = "伤害 @技能等级";
            // 
            // lblDamageExplained
            // 
            this.lblDamageExplained.AutoSize = true;
            this.lblDamageExplained.Location = new System.Drawing.Point(11, 436);
            this.lblDamageExplained.Name = "lblDamageExplained";
            this.lblDamageExplained.Size = new System.Drawing.Size(35, 12);
            this.lblDamageExplained.TabIndex = 9;
            this.lblDamageExplained.Text = "伤害:";
            // 
            // lblSelected
            // 
            this.lblSelected.AutoSize = true;
            this.lblSelected.Location = new System.Drawing.Point(20, 3);
            this.lblSelected.Name = "lblSelected";
            this.lblSelected.Size = new System.Drawing.Size(65, 12);
            this.lblSelected.TabIndex = 8;
            this.lblSelected.Text = "选中技能: ";
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
            this.panel4.Location = new System.Drawing.Point(14, 236);
            this.panel4.Name = "panel4";
            this.panel4.Size = new System.Drawing.Size(233, 191);
            this.panel4.TabIndex = 6;
            // 
            // txtDmgMultBoost
            // 
            this.txtDmgMultBoost.Location = new System.Drawing.Point(168, 157);
            this.txtDmgMultBoost.Name = "txtDmgMultBoost";
            this.txtDmgMultBoost.Size = new System.Drawing.Size(46, 21);
            this.txtDmgMultBoost.TabIndex = 14;
            this.toolTip1.SetToolTip(this.txtDmgMultBoost, "extra multiplyer apply\'d for every skill level");
            this.txtDmgMultBoost.TextChanged += new System.EventHandler(this.txtDmgMultBoost_TextChanged);
            // 
            // txtDmgMultBase
            // 
            this.txtDmgMultBase.Location = new System.Drawing.Point(168, 131);
            this.txtDmgMultBase.Name = "txtDmgMultBase";
            this.txtDmgMultBase.Size = new System.Drawing.Size(46, 21);
            this.txtDmgMultBase.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txtDmgMultBase, "multiplier apply\'d to total skill dmg");
            this.txtDmgMultBase.TextChanged += new System.EventHandler(this.txtDmgMultBase_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(12, 160);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(137, 12);
            this.label21.TabIndex = 12;
            this.label21.Text = "每等级多人释放技能伤害";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(12, 134);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(101, 12);
            this.label22.TabIndex = 11;
            this.label22.Text = "多人释放基础伤害";
            // 
            // txtDmgBonusMax
            // 
            this.txtDmgBonusMax.Location = new System.Drawing.Point(168, 105);
            this.txtDmgBonusMax.Name = "txtDmgBonusMax";
            this.txtDmgBonusMax.Size = new System.Drawing.Size(46, 21);
            this.txtDmgBonusMax.TabIndex = 10;
            this.toolTip1.SetToolTip(this.txtDmgBonusMax, "Damage bonus at skill level \'4\' ");
            this.txtDmgBonusMax.TextChanged += new System.EventHandler(this.txtDmgBonusMax_TextChanged);
            // 
            // txtDmgBonusMin
            // 
            this.txtDmgBonusMin.Location = new System.Drawing.Point(168, 79);
            this.txtDmgBonusMin.Name = "txtDmgBonusMin";
            this.txtDmgBonusMin.Size = new System.Drawing.Size(46, 21);
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
            this.label18.Size = new System.Drawing.Size(95, 12);
            this.label18.TabIndex = 8;
            this.label18.Text = "最大技能3级伤害";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(12, 82);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(95, 12);
            this.label19.TabIndex = 7;
            this.label19.Text = "最低技能3级伤害";
            // 
            // txtDmgBaseMax
            // 
            this.txtDmgBaseMax.Location = new System.Drawing.Point(168, 53);
            this.txtDmgBaseMax.Name = "txtDmgBaseMax";
            this.txtDmgBaseMax.Size = new System.Drawing.Size(46, 21);
            this.txtDmgBaseMax.TabIndex = 6;
            this.toolTip1.SetToolTip(this.txtDmgBaseMax, "Damage at skill level 0");
            this.txtDmgBaseMax.TextChanged += new System.EventHandler(this.txtDmgBaseMax_TextChanged);
            // 
            // txtDmgBaseMin
            // 
            this.txtDmgBaseMin.Location = new System.Drawing.Point(168, 27);
            this.txtDmgBaseMin.Name = "txtDmgBaseMin";
            this.txtDmgBaseMin.Size = new System.Drawing.Size(46, 21);
            this.txtDmgBaseMin.TabIndex = 5;
            this.toolTip1.SetToolTip(this.txtDmgBaseMin, "Damage at skill level 0");
            this.txtDmgBaseMin.TextChanged += new System.EventHandler(this.txtDmgBaseMin_TextChanged);
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(12, 56);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(77, 12);
            this.label17.TabIndex = 2;
            this.label17.Text = "最大基础伤害";
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(12, 30);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(77, 12);
            this.label16.TabIndex = 1;
            this.label16.Text = "最小基础伤害";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(5, 8);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(53, 12);
            this.label15.TabIndex = 0;
            this.label15.Text = "伤害升值";
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
            this.panel3.Location = new System.Drawing.Point(253, 236);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(216, 191);
            this.panel3.TabIndex = 5;
            this.toolTip1.SetToolTip(this.panel3, "delay = <base delay> - (skill level * <decrease>)");
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(21, 76);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(77, 24);
            this.label20.TabIndex = 15;
            this.label20.Text = "技能伤害范围\r\n (0 无限制)";
            // 
            // txtRange
            // 
            this.txtRange.Location = new System.Drawing.Point(121, 74);
            this.txtRange.Name = "txtRange";
            this.txtRange.Size = new System.Drawing.Size(79, 21);
            this.txtRange.TabIndex = 14;
            this.txtRange.TextChanged += new System.EventHandler(this.txtRange_TextChanged);
            // 
            // txtDelayReduction
            // 
            this.txtDelayReduction.Location = new System.Drawing.Point(121, 47);
            this.txtDelayReduction.Name = "txtDelayReduction";
            this.txtDelayReduction.Size = new System.Drawing.Size(79, 21);
            this.txtDelayReduction.TabIndex = 13;
            this.toolTip1.SetToolTip(this.txtDelayReduction, "delay = <base delay> - (skill level * <decrease>)");
            this.txtDelayReduction.TextChanged += new System.EventHandler(this.txtDelayReduction_TextChanged);
            // 
            // txtDelayBase
            // 
            this.txtDelayBase.Location = new System.Drawing.Point(121, 22);
            this.txtDelayBase.Name = "txtDelayBase";
            this.txtDelayBase.Size = new System.Drawing.Size(79, 21);
            this.txtDelayBase.TabIndex = 12;
            this.toolTip1.SetToolTip(this.txtDelayBase, "delay = <base delay> - (skill level * <decrease>)");
            this.txtDelayBase.TextChanged += new System.EventHandler(this.txtDelayBase_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(12, 50);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(89, 12);
            this.label14.TabIndex = 2;
            this.label14.Text = "每一级减少延迟";
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(12, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(77, 12);
            this.label13.TabIndex = 1;
            this.label13.Text = "技能基础延迟";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(12, 8);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(101, 12);
            this.label12.TabIndex = 0;
            this.label12.Text = "技能延迟 (毫秒!)";
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.txtMPIncrease);
            this.panel2.Controls.Add(this.txtMPBase);
            this.panel2.Controls.Add(this.label11);
            this.panel2.Controls.Add(this.label10);
            this.panel2.Controls.Add(this.label9);
            this.panel2.Location = new System.Drawing.Point(253, 52);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(216, 178);
            this.panel2.TabIndex = 4;
            // 
            // txtMPIncrease
            // 
            this.txtMPIncrease.Location = new System.Drawing.Point(135, 47);
            this.txtMPIncrease.Name = "txtMPIncrease";
            this.txtMPIncrease.Size = new System.Drawing.Size(46, 21);
            this.txtMPIncrease.TabIndex = 12;
            this.toolTip1.SetToolTip(this.txtMPIncrease, "extra amount of mp used each level");
            this.txtMPIncrease.TextChanged += new System.EventHandler(this.txtMPIncrease_TextChanged);
            // 
            // txtMPBase
            // 
            this.txtMPBase.Location = new System.Drawing.Point(135, 22);
            this.txtMPBase.Name = "txtMPBase";
            this.txtMPBase.Size = new System.Drawing.Size(46, 21);
            this.txtMPBase.TabIndex = 11;
            this.toolTip1.SetToolTip(this.txtMPBase, "Mp usage when skill is level 0");
            this.txtMPBase.TextChanged += new System.EventHandler(this.txtMPBase_TextChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(12, 50);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(125, 12);
            this.label11.TabIndex = 2;
            this.label11.Text = "技能等级消耗MP基础值";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(12, 25);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(101, 12);
            this.label10.TabIndex = 1;
            this.label10.Text = "技能消耗MP基础值";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(12, 6);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(77, 12);
            this.label9.TabIndex = 0;
            this.label9.Text = "技能消耗MP值";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.label26);
            this.panel1.Controls.Add(this.label25);
            this.panel1.Controls.Add(this.checkBox1);
            this.panel1.Controls.Add(this.textBox1);
            this.panel1.Controls.Add(this.textBox2);
            this.panel1.Controls.Add(this.txtSkillLvl3Points);
            this.panel1.Controls.Add(this.txtSkillLvl2Points);
            this.panel1.Controls.Add(this.txtSkillLvl1Points);
            this.panel1.Controls.Add(this.label6);
            this.panel1.Controls.Add(this.label7);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Controls.Add(this.txtSkillLvl3Req);
            this.panel1.Controls.Add(this.txtSkillLvl2Req);
            this.panel1.Controls.Add(this.txtSkillLvl1Req);
            this.panel1.Controls.Add(this.label5);
            this.panel1.Controls.Add(this.label4);
            this.panel1.Controls.Add(this.label3);
            this.panel1.Controls.Add(this.label2);
            this.panel1.Location = new System.Drawing.Point(13, 53);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(234, 177);
            this.panel1.TabIndex = 3;
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.ForeColor = System.Drawing.Color.Red;
            this.checkBox1.Location = new System.Drawing.Point(48, 111);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(90, 16);
            this.checkBox1.TabIndex = 13;
            this.checkBox1.Text = "羽化4级技能";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(169, 142);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(46, 21);
            this.textBox1.TabIndex = 16;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(57, 142);
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(46, 21);
            this.textBox2.TabIndex = 14;
            this.textBox2.TextChanged += new System.EventHandler(this.textBox2_TextChanged);
            // 
            // txtSkillLvl3Points
            // 
            this.txtSkillLvl3Points.Location = new System.Drawing.Point(169, 72);
            this.txtSkillLvl3Points.Name = "txtSkillLvl3Points";
            this.txtSkillLvl3Points.Size = new System.Drawing.Size(46, 21);
            this.txtSkillLvl3Points.TabIndex = 12;
            this.txtSkillLvl3Points.TextChanged += new System.EventHandler(this.txtSkillLvl3Points_TextChanged);
            // 
            // txtSkillLvl2Points
            // 
            this.txtSkillLvl2Points.Location = new System.Drawing.Point(169, 47);
            this.txtSkillLvl2Points.Name = "txtSkillLvl2Points";
            this.txtSkillLvl2Points.Size = new System.Drawing.Size(46, 21);
            this.txtSkillLvl2Points.TabIndex = 11;
            this.txtSkillLvl2Points.TextChanged += new System.EventHandler(this.txtSkillLvl2Points_TextChanged);
            // 
            // txtSkillLvl1Points
            // 
            this.txtSkillLvl1Points.Location = new System.Drawing.Point(169, 22);
            this.txtSkillLvl1Points.Name = "txtSkillLvl1Points";
            this.txtSkillLvl1Points.Size = new System.Drawing.Size(46, 21);
            this.txtSkillLvl1Points.TabIndex = 10;
            this.txtSkillLvl1Points.TextChanged += new System.EventHandler(this.txtSkillLvl1Points_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(110, 75);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(41, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "技能点";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(110, 50);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 12);
            this.label7.TabIndex = 8;
            this.label7.Text = "技能点";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(110, 25);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(41, 12);
            this.label8.TabIndex = 7;
            this.label8.Text = "技能点";
            // 
            // txtSkillLvl3Req
            // 
            this.txtSkillLvl3Req.Location = new System.Drawing.Point(57, 72);
            this.txtSkillLvl3Req.Name = "txtSkillLvl3Req";
            this.txtSkillLvl3Req.Size = new System.Drawing.Size(46, 21);
            this.txtSkillLvl3Req.TabIndex = 6;
            this.txtSkillLvl3Req.TextChanged += new System.EventHandler(this.txtSkillLvl3Req_TextChanged);
            // 
            // txtSkillLvl2Req
            // 
            this.txtSkillLvl2Req.Location = new System.Drawing.Point(57, 47);
            this.txtSkillLvl2Req.Name = "txtSkillLvl2Req";
            this.txtSkillLvl2Req.Size = new System.Drawing.Size(46, 21);
            this.txtSkillLvl2Req.TabIndex = 5;
            this.txtSkillLvl2Req.TextChanged += new System.EventHandler(this.txtSkillLvl2Req_TextChanged);
            // 
            // txtSkillLvl1Req
            // 
            this.txtSkillLvl1Req.Location = new System.Drawing.Point(57, 22);
            this.txtSkillLvl1Req.Name = "txtSkillLvl1Req";
            this.txtSkillLvl1Req.Size = new System.Drawing.Size(46, 21);
            this.txtSkillLvl1Req.TabIndex = 4;
            this.txtSkillLvl1Req.TextChanged += new System.EventHandler(this.txtSkillLvl1Req_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(41, 12);
            this.label5.TabIndex = 3;
            this.label5.Text = "等级 3";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 50);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(41, 12);
            this.label4.TabIndex = 2;
            this.label4.Text = "等级 2";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 25);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 1;
            this.label3.Text = "等级 1";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 6);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(77, 12);
            this.label2.TabIndex = 0;
            this.label2.Text = "技能升级需求";
            // 
            // txtSkillIcon
            // 
            this.txtSkillIcon.Location = new System.Drawing.Point(311, 20);
            this.txtSkillIcon.Name = "txtSkillIcon";
            this.txtSkillIcon.Size = new System.Drawing.Size(41, 21);
            this.txtSkillIcon.TabIndex = 2;
            this.txtSkillIcon.TextChanged += new System.EventHandler(this.txtSkillIcon_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(250, 23);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "技能图标: ";
            // 
            // lblBookValid
            // 
            this.lblBookValid.AutoSize = true;
            this.lblBookValid.Location = new System.Drawing.Point(222, 3);
            this.lblBookValid.Name = "lblBookValid";
            this.lblBookValid.Size = new System.Drawing.Size(119, 12);
            this.lblBookValid.TabIndex = 0;
            this.lblBookValid.Text = "Searching for books";
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(10, 145);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(41, 12);
            this.label25.TabIndex = 17;
            this.label25.Text = "等级 4";
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(110, 145);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(41, 12);
            this.label26.TabIndex = 18;
            this.label26.Text = "技能点";
            // 
            // MagicInfoForm
            // 
            this.ClientSize = new System.Drawing.Size(1051, 678);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.MagiclistBox);
            this.Name = "MagicInfoForm";
            this.Text = "技能设置";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.MagicInfoForm_FormClosed);
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
            byte temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level1 = temp;
        }

        private void txtSkillLvl2Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level2 = temp;
        }

        private void txtSkillLvl3Req_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level3 = temp;
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


        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            byte temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Level4 = temp;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            ushort temp = 0;
            if (!IsValid(ref temp)) return;

            ActiveControl.BackColor = SystemColors.Window;
            _selectedMagicInfo.Need4 = temp;
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
                        
                _selectedMagicInfo.HumUpTrain = checkBox1.Checked;

           

        }
    }
}
