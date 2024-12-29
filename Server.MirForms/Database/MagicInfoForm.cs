using Server.MirDatabase;
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
            MagicSearchBox_TextChanged(this, EventArgs.Empty);
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
            }
            else
            {
                tabControl1.Enabled = true;
                lblSelected.Text = "Selected Skill: " + _selectedMagicInfo.ToString();
                lblDamageExample.Text =
                    $"Damage @ Skill level 0: {GetMinPower(0):000}-{GetMaxPower(0):000}   |||   level 1: {GetMinPower(1):000}-{GetMaxPower(1):000}   |||   level 2: {GetMinPower(2):000}-{GetMaxPower(2):000}   |||   level 3: {GetMinPower(3):000}-{GetMaxPower(3):000}";
                lblDamageExplained.Text =
                    $"Damage: {{Random(minstat-maxstat) + [<(random({_selectedMagicInfo.MPowerBase}-{_selectedMagicInfo.MPowerBase + _selectedMagicInfo.MPowerBonus})/4) X (skill level +1)> + random<{_selectedMagicInfo.PowerBase}-{_selectedMagicInfo.PowerBonus + _selectedMagicInfo.PowerBase}>]}}  X  {{{_selectedMagicInfo.MultiplierBase} + (skill level * {_selectedMagicInfo.MultiplierBonus})}}";
                txtSkillIcon.Text = _selectedMagicInfo.Icon.ToString();
                txtSkillLvl1Points.Text = _selectedMagicInfo.Need1.ToString();
                txtSkillLvl1Req.Text = _selectedMagicInfo.Level1.ToString();
                txtSkillLvl2Points.Text = _selectedMagicInfo.Need2.ToString();
                txtSkillLvl2Req.Text = _selectedMagicInfo.Level2.ToString();
                txtSkillLvl3Points.Text = _selectedMagicInfo.Need3.ToString();
                txtSkillLvl3Req.Text = _selectedMagicInfo.Level3.ToString();
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
                textBoxName.Text = _selectedMagicInfo.Name;
            }
        }

        private int GetMaxPower(byte level)
        {
            if (_selectedMagicInfo == null) return 0;
            return (int)Math.Round((((_selectedMagicInfo.MPowerBase + _selectedMagicInfo.MPowerBonus) / 4F) * (level + 1) + (_selectedMagicInfo.PowerBase + _selectedMagicInfo.PowerBonus)) * (_selectedMagicInfo.MultiplierBase + (level * _selectedMagicInfo.MultiplierBonus)));
        }
        private int GetMinPower(byte level)
        {
            if (_selectedMagicInfo == null) return 0;
            return (int)Math.Round(((_selectedMagicInfo.MPowerBase / 4F) * (level + 1) + _selectedMagicInfo.PowerBase) * (_selectedMagicInfo.MultiplierBase + (level * _selectedMagicInfo.MultiplierBonus)));
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            MagiclistBox = new ListBox();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            label24 = new Label();
            label23 = new Label();
            textBoxName = new TextBox();
            lblDamageExample = new Label();
            lblDamageExplained = new Label();
            lblSelected = new Label();
            panel4 = new Panel();
            txtDmgMultBoost = new TextBox();
            txtDmgMultBase = new TextBox();
            label21 = new Label();
            label22 = new Label();
            txtDmgBonusMax = new TextBox();
            txtDmgBonusMin = new TextBox();
            label18 = new Label();
            label19 = new Label();
            txtDmgBaseMax = new TextBox();
            txtDmgBaseMin = new TextBox();
            label17 = new Label();
            label16 = new Label();
            label15 = new Label();
            panel3 = new Panel();
            label20 = new Label();
            txtRange = new TextBox();
            txtDelayReduction = new TextBox();
            txtDelayBase = new TextBox();
            label14 = new Label();
            label13 = new Label();
            label12 = new Label();
            panel2 = new Panel();
            txtMPIncrease = new TextBox();
            txtMPBase = new TextBox();
            label11 = new Label();
            label10 = new Label();
            label9 = new Label();
            panel1 = new Panel();
            txtSkillLvl3Points = new TextBox();
            txtSkillLvl2Points = new TextBox();
            txtSkillLvl1Points = new TextBox();
            label6 = new Label();
            label7 = new Label();
            label8 = new Label();
            txtSkillLvl3Req = new TextBox();
            txtSkillLvl2Req = new TextBox();
            txtSkillLvl1Req = new TextBox();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            label2 = new Label();
            txtSkillIcon = new TextBox();
            label1 = new Label();
            lblBookValid = new Label();
            toolTip1 = new ToolTip(components);
            MagicSearchBox = new TextBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            panel4.SuspendLayout();
            panel3.SuspendLayout();
            panel2.SuspendLayout();
            panel1.SuspendLayout();
            SuspendLayout();
            // 
            // MagiclistBox
            // 
            MagiclistBox.Dock = DockStyle.Left;
            MagiclistBox.FormattingEnabled = true;
            MagiclistBox.ItemHeight = 15;
            MagiclistBox.Location = new Point(0, 0);
            MagiclistBox.Name = "MagiclistBox";
            MagiclistBox.Size = new Size(225, 542);
            MagiclistBox.TabIndex = 0;
            MagiclistBox.SelectedIndexChanged += MagiclistBox_SelectedIndexChanged;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(225, 0);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(702, 542);
            tabControl1.TabIndex = 1;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label24);
            tabPage1.Controls.Add(label23);
            tabPage1.Controls.Add(textBoxName);
            tabPage1.Controls.Add(lblDamageExample);
            tabPage1.Controls.Add(lblDamageExplained);
            tabPage1.Controls.Add(lblSelected);
            tabPage1.Controls.Add(panel4);
            tabPage1.Controls.Add(panel3);
            tabPage1.Controls.Add(panel2);
            tabPage1.Controls.Add(panel1);
            tabPage1.Controls.Add(txtSkillIcon);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(lblBookValid);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(694, 514);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Basics";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(20, 23);
            label24.Name = "label24";
            label24.Size = new Size(63, 15);
            label24.TabIndex = 12;
            label24.Text = "SkillName:";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(181, 3);
            label23.Name = "label23";
            label23.Size = new Size(37, 15);
            label23.TabIndex = 11;
            label23.Text = "book:";
            // 
            // textBoxName
            // 
            textBoxName.Location = new Point(89, 18);
            textBoxName.Name = "textBoxName";
            textBoxName.Size = new Size(112, 23);
            textBoxName.TabIndex = 10;
            textBoxName.TextChanged += textBoxName_TextChanged;
            // 
            // lblDamageExample
            // 
            lblDamageExample.AutoSize = true;
            lblDamageExample.Location = new Point(11, 394);
            lblDamageExample.Name = "lblDamageExample";
            lblDamageExample.Size = new Size(98, 15);
            lblDamageExample.TabIndex = 0;
            lblDamageExample.Text = "Damage example";
            // 
            // lblDamageExplained
            // 
            lblDamageExplained.AutoSize = true;
            lblDamageExplained.Location = new Point(11, 366);
            lblDamageExplained.Name = "lblDamageExplained";
            lblDamageExplained.Size = new Size(54, 15);
            lblDamageExplained.TabIndex = 9;
            lblDamageExplained.Text = "Damage:";
            // 
            // lblSelected
            // 
            lblSelected.AutoSize = true;
            lblSelected.Location = new Point(20, 3);
            lblSelected.Name = "lblSelected";
            lblSelected.Size = new Size(80, 15);
            lblSelected.TabIndex = 8;
            lblSelected.Text = "Selected skill: ";
            // 
            // panel4
            // 
            panel4.BorderStyle = BorderStyle.FixedSingle;
            panel4.Controls.Add(txtDmgMultBoost);
            panel4.Controls.Add(txtDmgMultBase);
            panel4.Controls.Add(label21);
            panel4.Controls.Add(label22);
            panel4.Controls.Add(txtDmgBonusMax);
            panel4.Controls.Add(txtDmgBonusMin);
            panel4.Controls.Add(label18);
            panel4.Controls.Add(label19);
            panel4.Controls.Add(txtDmgBaseMax);
            panel4.Controls.Add(txtDmgBaseMin);
            panel4.Controls.Add(label17);
            panel4.Controls.Add(label16);
            panel4.Controls.Add(label15);
            panel4.Location = new Point(14, 166);
            panel4.Name = "panel4";
            panel4.Size = new Size(233, 191);
            panel4.TabIndex = 6;
            // 
            // txtDmgMultBoost
            // 
            txtDmgMultBoost.Location = new Point(168, 157);
            txtDmgMultBoost.Name = "txtDmgMultBoost";
            txtDmgMultBoost.Size = new Size(46, 23);
            txtDmgMultBoost.TabIndex = 14;
            toolTip1.SetToolTip(txtDmgMultBoost, "extra multiplyer apply'd for every skill level");
            txtDmgMultBoost.TextChanged += txtDmgMultBoost_TextChanged;
            // 
            // txtDmgMultBase
            // 
            txtDmgMultBase.Location = new Point(168, 131);
            txtDmgMultBase.Name = "txtDmgMultBase";
            txtDmgMultBase.Size = new Size(46, 23);
            txtDmgMultBase.TabIndex = 13;
            toolTip1.SetToolTip(txtDmgMultBase, "multiplier apply'd to total skill dmg");
            txtDmgMultBase.TextChanged += txtDmgMultBase_TextChanged;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(12, 160);
            label21.Name = "label21";
            label21.Size = new Size(178, 15);
            label21.TabIndex = 12;
            label21.Text = "Damage multiplyer boost/skilllvl";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(12, 134);
            label22.Name = "label22";
            label22.Size = new Size(135, 15);
            label22.TabIndex = 11;
            label22.Text = "Damage multiplyer base";
            // 
            // txtDmgBonusMax
            // 
            txtDmgBonusMax.Location = new Point(168, 105);
            txtDmgBonusMax.Name = "txtDmgBonusMax";
            txtDmgBonusMax.Size = new Size(46, 23);
            txtDmgBonusMax.TabIndex = 10;
            toolTip1.SetToolTip(txtDmgBonusMax, "Damage bonus at skill level '4' ");
            txtDmgBonusMax.TextChanged += txtDmgBonusMax_TextChanged;
            // 
            // txtDmgBonusMin
            // 
            txtDmgBonusMin.Location = new Point(168, 79);
            txtDmgBonusMin.Name = "txtDmgBonusMin";
            txtDmgBonusMin.Size = new Size(46, 23);
            txtDmgBonusMin.TabIndex = 9;
            toolTip1.SetToolTip(txtDmgBonusMin, "Damage bonus at skill level '4' \r\nyou will get 1/4th of this bonus for every skill level\r\nnote ingame level 0 = 1 bonus, so level 3 = max bonus (4)");
            txtDmgBonusMin.TextChanged += txtDmgBonusMin_TextChanged;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(12, 108);
            label18.Name = "label18";
            label18.Size = new Size(154, 15);
            label18.TabIndex = 8;
            label18.Text = "Maximum skill lvl 3 damage";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(12, 82);
            label19.Name = "label19";
            label19.Size = new Size(156, 15);
            label19.TabIndex = 7;
            label19.Text = "Minimum skill lvl 3 damage:";
            // 
            // txtDmgBaseMax
            // 
            txtDmgBaseMax.Location = new Point(168, 53);
            txtDmgBaseMax.Name = "txtDmgBaseMax";
            txtDmgBaseMax.Size = new Size(46, 23);
            txtDmgBaseMax.TabIndex = 6;
            toolTip1.SetToolTip(txtDmgBaseMax, "Damage at skill level 0");
            txtDmgBaseMax.TextChanged += txtDmgBaseMax_TextChanged;
            // 
            // txtDmgBaseMin
            // 
            txtDmgBaseMin.Location = new Point(168, 27);
            txtDmgBaseMin.Name = "txtDmgBaseMin";
            txtDmgBaseMin.Size = new Size(46, 23);
            txtDmgBaseMin.TabIndex = 5;
            toolTip1.SetToolTip(txtDmgBaseMin, "Damage at skill level 0");
            txtDmgBaseMin.TextChanged += txtDmgBaseMin_TextChanged;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(12, 56);
            label17.Name = "label17";
            label17.Size = new Size(134, 15);
            label17.TabIndex = 2;
            label17.Text = "Maximum base damage";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(12, 30);
            label16.Name = "label16";
            label16.Size = new Size(136, 15);
            label16.TabIndex = 1;
            label16.Text = "Minimum base damage:";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(5, 8);
            label15.Name = "label15";
            label15.Size = new Size(95, 15);
            label15.TabIndex = 0;
            label15.Text = "Damage settings";
            // 
            // panel3
            // 
            panel3.BorderStyle = BorderStyle.FixedSingle;
            panel3.Controls.Add(label20);
            panel3.Controls.Add(txtRange);
            panel3.Controls.Add(txtDelayReduction);
            panel3.Controls.Add(txtDelayBase);
            panel3.Controls.Add(label14);
            panel3.Controls.Add(label13);
            panel3.Controls.Add(label12);
            panel3.Location = new Point(253, 166);
            panel3.Name = "panel3";
            panel3.Size = new Size(216, 191);
            panel3.TabIndex = 5;
            toolTip1.SetToolTip(panel3, "delay = <base delay> - (skill level * <decrease>)");
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(12, 77);
            label20.Name = "label20";
            label20.Size = new Size(103, 15);
            label20.TabIndex = 15;
            label20.Text = "Range (0 No limit)";
            // 
            // txtRange
            // 
            txtRange.Location = new Point(121, 74);
            txtRange.Name = "txtRange";
            txtRange.Size = new Size(79, 23);
            txtRange.TabIndex = 14;
            txtRange.TextChanged += txtRange_TextChanged;
            // 
            // txtDelayReduction
            // 
            txtDelayReduction.Location = new Point(121, 47);
            txtDelayReduction.Name = "txtDelayReduction";
            txtDelayReduction.Size = new Size(79, 23);
            txtDelayReduction.TabIndex = 13;
            toolTip1.SetToolTip(txtDelayReduction, "delay = <base delay> - (skill level * <decrease>)");
            txtDelayReduction.TextChanged += txtDelayReduction_TextChanged;
            // 
            // txtDelayBase
            // 
            txtDelayBase.Location = new Point(121, 22);
            txtDelayBase.Name = "txtDelayBase";
            txtDelayBase.Size = new Size(79, 23);
            txtDelayBase.TabIndex = 12;
            toolTip1.SetToolTip(txtDelayBase, "delay = <base delay> - (skill level * <decrease>)");
            txtDelayBase.TextChanged += txtDelayBase_TextChanged;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(12, 50);
            label14.Name = "label14";
            label14.Size = new Size(112, 15);
            label14.TabIndex = 2;
            label14.Text = "Decrease / skill level";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(12, 25);
            label13.Name = "label13";
            label13.Size = new Size(62, 15);
            label13.TabIndex = 1;
            label13.Text = "Base delay";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(12, 8);
            label12.Name = "label12";
            label12.Size = new Size(129, 15);
            label12.TabIndex = 0;
            label12.Text = "Delay (in milliseconds!)";
            // 
            // panel2
            // 
            panel2.BorderStyle = BorderStyle.FixedSingle;
            panel2.Controls.Add(txtMPIncrease);
            panel2.Controls.Add(txtMPBase);
            panel2.Controls.Add(label11);
            panel2.Controls.Add(label10);
            panel2.Controls.Add(label9);
            panel2.Location = new Point(253, 53);
            panel2.Name = "panel2";
            panel2.Size = new Size(216, 107);
            panel2.TabIndex = 4;
            // 
            // txtMPIncrease
            // 
            txtMPIncrease.Location = new Point(135, 47);
            txtMPIncrease.Name = "txtMPIncrease";
            txtMPIncrease.Size = new Size(46, 23);
            txtMPIncrease.TabIndex = 12;
            toolTip1.SetToolTip(txtMPIncrease, "extra amount of mp used each level");
            txtMPIncrease.TextChanged += txtMPIncrease_TextChanged;
            // 
            // txtMPBase
            // 
            txtMPBase.Location = new Point(135, 22);
            txtMPBase.Name = "txtMPBase";
            txtMPBase.Size = new Size(46, 23);
            txtMPBase.TabIndex = 11;
            toolTip1.SetToolTip(txtMPBase, "Mp usage when skill is level 0");
            txtMPBase.TextChanged += txtMPBase_TextChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(12, 50);
            label11.Name = "label11";
            label11.Size = new Size(126, 15);
            label11.TabIndex = 2;
            label11.Text = "MP increase each level";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(12, 25);
            label10.Name = "label10";
            label10.Size = new Size(86, 15);
            label10.TabIndex = 1;
            label10.Text = "Base mp usage";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(12, 6);
            label9.Name = "label9";
            label9.Size = new Size(59, 15);
            label9.TabIndex = 0;
            label9.Text = "MP usage";
            // 
            // panel1
            // 
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(txtSkillLvl3Points);
            panel1.Controls.Add(txtSkillLvl2Points);
            panel1.Controls.Add(txtSkillLvl1Points);
            panel1.Controls.Add(label6);
            panel1.Controls.Add(label7);
            panel1.Controls.Add(label8);
            panel1.Controls.Add(txtSkillLvl3Req);
            panel1.Controls.Add(txtSkillLvl2Req);
            panel1.Controls.Add(txtSkillLvl1Req);
            panel1.Controls.Add(label5);
            panel1.Controls.Add(label4);
            panel1.Controls.Add(label3);
            panel1.Controls.Add(label2);
            panel1.Location = new Point(13, 53);
            panel1.Name = "panel1";
            panel1.Size = new Size(234, 107);
            panel1.TabIndex = 3;
            // 
            // txtSkillLvl3Points
            // 
            txtSkillLvl3Points.Location = new Point(169, 72);
            txtSkillLvl3Points.Name = "txtSkillLvl3Points";
            txtSkillLvl3Points.Size = new Size(46, 23);
            txtSkillLvl3Points.TabIndex = 12;
            txtSkillLvl3Points.TextChanged += txtSkillLvl3Points_TextChanged;
            // 
            // txtSkillLvl2Points
            // 
            txtSkillLvl2Points.Location = new Point(169, 47);
            txtSkillLvl2Points.Name = "txtSkillLvl2Points";
            txtSkillLvl2Points.Size = new Size(46, 23);
            txtSkillLvl2Points.TabIndex = 11;
            txtSkillLvl2Points.TextChanged += txtSkillLvl2Points_TextChanged;
            // 
            // txtSkillLvl1Points
            // 
            txtSkillLvl1Points.Location = new Point(169, 22);
            txtSkillLvl1Points.Name = "txtSkillLvl1Points";
            txtSkillLvl1Points.Size = new Size(46, 23);
            txtSkillLvl1Points.TabIndex = 10;
            txtSkillLvl1Points.TextChanged += txtSkillLvl1Points_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(110, 75);
            label6.Name = "label6";
            label6.Size = new Size(64, 15);
            label6.TabIndex = 9;
            label6.Text = "Skill points";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(110, 50);
            label7.Name = "label7";
            label7.Size = new Size(64, 15);
            label7.TabIndex = 8;
            label7.Text = "Skill points";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(110, 25);
            label8.Name = "label8";
            label8.Size = new Size(64, 15);
            label8.TabIndex = 7;
            label8.Text = "Skill points";
            // 
            // txtSkillLvl3Req
            // 
            txtSkillLvl3Req.Location = new Point(57, 72);
            txtSkillLvl3Req.Name = "txtSkillLvl3Req";
            txtSkillLvl3Req.Size = new Size(46, 23);
            txtSkillLvl3Req.TabIndex = 6;
            txtSkillLvl3Req.TextChanged += txtSkillLvl3Req_TextChanged;
            // 
            // txtSkillLvl2Req
            // 
            txtSkillLvl2Req.Location = new Point(57, 47);
            txtSkillLvl2Req.Name = "txtSkillLvl2Req";
            txtSkillLvl2Req.Size = new Size(46, 23);
            txtSkillLvl2Req.TabIndex = 5;
            txtSkillLvl2Req.TextChanged += txtSkillLvl2Req_TextChanged;
            // 
            // txtSkillLvl1Req
            // 
            txtSkillLvl1Req.Location = new Point(57, 22);
            txtSkillLvl1Req.Name = "txtSkillLvl1Req";
            txtSkillLvl1Req.Size = new Size(46, 23);
            txtSkillLvl1Req.TabIndex = 4;
            txtSkillLvl1Req.TextChanged += txtSkillLvl1Req_TextChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(13, 75);
            label5.Name = "label5";
            label5.Size = new Size(40, 15);
            label5.TabIndex = 3;
            label5.Text = "level 3";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(13, 50);
            label4.Name = "label4";
            label4.Size = new Size(40, 15);
            label4.TabIndex = 2;
            label4.Text = "level 2";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(13, 25);
            label3.Name = "label3";
            label3.Size = new Size(40, 15);
            label3.TabIndex = 1;
            label3.Text = "level 1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(6, 6);
            label2.Name = "label2";
            label2.Size = new Size(174, 15);
            label2.TabIndex = 0;
            label2.Text = "Skill level increase requirements";
            // 
            // txtSkillIcon
            // 
            txtSkillIcon.Location = new Point(311, 20);
            txtSkillIcon.Name = "txtSkillIcon";
            txtSkillIcon.Size = new Size(41, 23);
            txtSkillIcon.TabIndex = 2;
            txtSkillIcon.TextChanged += txtSkillIcon_TextChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(250, 23);
            label1.Name = "label1";
            label1.Size = new Size(60, 15);
            label1.TabIndex = 1;
            label1.Text = "Skill icon: ";
            // 
            // lblBookValid
            // 
            lblBookValid.AutoSize = true;
            lblBookValid.Location = new Point(222, 3);
            lblBookValid.Name = "lblBookValid";
            lblBookValid.Size = new Size(112, 15);
            lblBookValid.TabIndex = 0;
            lblBookValid.Text = "Searching for books";
            // 
            // MagicSearchBox
            // 
            MagicSearchBox.Location = new Point(276, 0);
            MagicSearchBox.Name = "MagicSearchBox";
            MagicSearchBox.PlaceholderText = "Search...";
            MagicSearchBox.Size = new Size(201, 23);
            MagicSearchBox.TabIndex = 2;
            MagicSearchBox.TextChanged += MagicSearchBox_TextChanged;
            // 
            // MagicInfoForm
            // 
            ClientSize = new Size(927, 542);
            Controls.Add(MagicSearchBox);
            Controls.Add(tabControl1);
            Controls.Add(MagiclistBox);
            Name = "MagicInfoForm";
            Text = "Magic Settings";
            FormClosed += MagicInfoForm_FormClosed;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            panel4.ResumeLayout(false);
            panel4.PerformLayout();
            panel3.ResumeLayout(false);
            panel3.PerformLayout();
            panel2.ResumeLayout(false);
            panel2.PerformLayout();
            panel1.ResumeLayout(false);
            panel1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
            _selectedMagicInfo.PowerBonus = (ushort)(temp - _selectedMagicInfo.PowerBase);
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

        private void textBoxName_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            _selectedMagicInfo.Name = ActiveControl.Text;
            UpdateMagicForm();
            if (ActiveControl.Text == "")
            {
                ActiveControl.BackColor = Color.Red;
            }
            else
            {
                ActiveControl.BackColor = SystemColors.Window;
            }
        }

        #region Search Box
        private void MagicSearchBox_TextChanged(object sender, EventArgs e)
        {
            // Show all items when the search box is cleared or placeholder is active
            if (string.IsNullOrWhiteSpace(MagicSearchBox.Text))
            {
                MagiclistBox.Items.Clear();
                foreach (var magic in Envir.MagicInfoList)
                {
                    MagiclistBox.Items.Add(magic);
                }
                return;
            }

            string searchText = MagicSearchBox.Text.ToLower();
            MagiclistBox.Items.Clear();

            // Add filtered items to the list
            foreach (var magic in Envir.MagicInfoList)
            {
                if (!string.IsNullOrEmpty(magic.Name) && magic.Name.ToLower().Contains(searchText))
                {
                    MagiclistBox.Items.Add(magic);
                }
            }
        }
        #endregion
    }
}