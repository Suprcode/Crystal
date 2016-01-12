namespace Server
{
    partial class ProfessionsForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.ProfSettings = new System.Windows.Forms.TabControl();
            this.Mining_tab = new System.Windows.Forms.TabPage();
            this.Mines_lb = new System.Windows.Forms.ListBox();
            this.MineRemoveIndexbutton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox7 = new System.Windows.Forms.GroupBox();
            this.label79 = new System.Windows.Forms.Label();
            this.MineNametextBox = new System.Windows.Forms.TextBox();
            this.MineSlotstextBox = new System.Windows.Forms.TextBox();
            this.label70 = new System.Windows.Forms.Label();
            this.label69 = new System.Windows.Forms.Label();
            this.label68 = new System.Windows.Forms.Label();
            this.MineDropRatetextBox = new System.Windows.Forms.TextBox();
            this.MineHitRatetextBox = new System.Windows.Forms.TextBox();
            this.MineAttemptstextBox = new System.Windows.Forms.TextBox();
            this.MineRegenDelaytextBox = new System.Windows.Forms.TextBox();
            this.label67 = new System.Windows.Forms.Label();
            this.label66 = new System.Windows.Forms.Label();
            this.label65 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label78 = new System.Windows.Forms.Label();
            this.MineDropsIndexcomboBox = new System.Windows.Forms.ComboBox();
            this.groupBox8 = new System.Windows.Forms.GroupBox();
            this.MineMaxBonustextBox = new System.Windows.Forms.TextBox();
            this.label77 = new System.Windows.Forms.Label();
            this.MineBonusChancetextBox = new System.Windows.Forms.TextBox();
            this.label76 = new System.Windows.Forms.Label();
            this.MineMaxQualitytextBox = new System.Windows.Forms.TextBox();
            this.label75 = new System.Windows.Forms.Label();
            this.MineMinQualitytextBox = new System.Windows.Forms.TextBox();
            this.label74 = new System.Windows.Forms.Label();
            this.MineMaxSlottextBox = new System.Windows.Forms.TextBox();
            this.label73 = new System.Windows.Forms.Label();
            this.MineMinSlottextBox = new System.Windows.Forms.TextBox();
            this.label72 = new System.Windows.Forms.Label();
            this.MineItemNametextBox = new System.Windows.Forms.TextBox();
            this.label71 = new System.Windows.Forms.Label();
            this.MineRemoveDropbutton = new System.Windows.Forms.Button();
            this.MineAddDropbutton = new System.Windows.Forms.Button();
            this.MineAddIndexbutton = new System.Windows.Forms.Button();
            this.Gathering_tab = new System.Windows.Forms.TabPage();
            this.Fishing_tab = new System.Windows.Forms.TabPage();
            this.Crafting_tab = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.MonsterSpawnChanceTextBox = new System.Windows.Forms.TextBox();
            this.FishingMobIndexComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FishingSuccessRateMultiplierTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FishingDelayTextBox = new System.Windows.Forms.TextBox();
            this.FishingSuccessRateStartTextBox = new System.Windows.Forms.TextBox();
            this.FishingAttemptsTextBox = new System.Windows.Forms.TextBox();
            this.ProfSettings.SuspendLayout();
            this.Mining_tab.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox7.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.groupBox8.SuspendLayout();
            this.Fishing_tab.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // ProfSettings
            // 
            this.ProfSettings.Controls.Add(this.Mining_tab);
            this.ProfSettings.Controls.Add(this.Gathering_tab);
            this.ProfSettings.Controls.Add(this.Fishing_tab);
            this.ProfSettings.Controls.Add(this.Crafting_tab);
            this.ProfSettings.Location = new System.Drawing.Point(12, 12);
            this.ProfSettings.Name = "ProfSettings";
            this.ProfSettings.SelectedIndex = 0;
            this.ProfSettings.Size = new System.Drawing.Size(823, 411);
            this.ProfSettings.TabIndex = 0;
            // 
            // Mining_tab
            // 
            this.Mining_tab.Controls.Add(this.Mines_lb);
            this.Mining_tab.Controls.Add(this.MineRemoveIndexbutton);
            this.Mining_tab.Controls.Add(this.tabControl1);
            this.Mining_tab.Controls.Add(this.MineAddIndexbutton);
            this.Mining_tab.Location = new System.Drawing.Point(4, 22);
            this.Mining_tab.Name = "Mining_tab";
            this.Mining_tab.Padding = new System.Windows.Forms.Padding(3);
            this.Mining_tab.Size = new System.Drawing.Size(815, 385);
            this.Mining_tab.TabIndex = 0;
            this.Mining_tab.Text = "Mining";
            this.Mining_tab.UseVisualStyleBackColor = true;
            // 
            // Mines_lb
            // 
            this.Mines_lb.FormattingEnabled = true;
            this.Mines_lb.Location = new System.Drawing.Point(6, 6);
            this.Mines_lb.Name = "Mines_lb";
            this.Mines_lb.Size = new System.Drawing.Size(144, 342);
            this.Mines_lb.TabIndex = 32;
            this.Mines_lb.SelectedIndexChanged += new System.EventHandler(this.Mines_lb_SelectedIndexChanged);
            // 
            // MineRemoveIndexbutton
            // 
            this.MineRemoveIndexbutton.Location = new System.Drawing.Point(5, 350);
            this.MineRemoveIndexbutton.Name = "MineRemoveIndexbutton";
            this.MineRemoveIndexbutton.Size = new System.Drawing.Size(21, 21);
            this.MineRemoveIndexbutton.TabIndex = 31;
            this.MineRemoveIndexbutton.Text = "-";
            this.MineRemoveIndexbutton.UseVisualStyleBackColor = true;
            this.MineRemoveIndexbutton.Click += new System.EventHandler(this.MineRemoveIndexbutton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage3);
            this.tabControl1.Location = new System.Drawing.Point(156, 6);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(653, 369);
            this.tabControl1.TabIndex = 27;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox7);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(645, 343);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Stats";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox7
            // 
            this.groupBox7.Controls.Add(this.label79);
            this.groupBox7.Controls.Add(this.MineNametextBox);
            this.groupBox7.Controls.Add(this.MineSlotstextBox);
            this.groupBox7.Controls.Add(this.label70);
            this.groupBox7.Controls.Add(this.label69);
            this.groupBox7.Controls.Add(this.label68);
            this.groupBox7.Controls.Add(this.MineDropRatetextBox);
            this.groupBox7.Controls.Add(this.MineHitRatetextBox);
            this.groupBox7.Controls.Add(this.MineAttemptstextBox);
            this.groupBox7.Controls.Add(this.MineRegenDelaytextBox);
            this.groupBox7.Controls.Add(this.label67);
            this.groupBox7.Controls.Add(this.label66);
            this.groupBox7.Controls.Add(this.label65);
            this.groupBox7.Location = new System.Drawing.Point(10, 6);
            this.groupBox7.Name = "groupBox7";
            this.groupBox7.Size = new System.Drawing.Size(403, 121);
            this.groupBox7.TabIndex = 27;
            this.groupBox7.TabStop = false;
            this.groupBox7.Text = "Mine Base Stat";
            this.groupBox7.Enter += new System.EventHandler(this.groupBox7_Enter);
            // 
            // label79
            // 
            this.label79.AutoSize = true;
            this.label79.Location = new System.Drawing.Point(8, 19);
            this.label79.Name = "label79";
            this.label79.Size = new System.Drawing.Size(38, 13);
            this.label79.TabIndex = 23;
            this.label79.Text = "Name:";
            // 
            // MineNametextBox
            // 
            this.MineNametextBox.Location = new System.Drawing.Point(97, 16);
            this.MineNametextBox.Name = "MineNametextBox";
            this.MineNametextBox.Size = new System.Drawing.Size(100, 20);
            this.MineNametextBox.TabIndex = 22;
            this.MineNametextBox.TextChanged += new System.EventHandler(this.MineNametextBox_TextChanged);
            // 
            // MineSlotstextBox
            // 
            this.MineSlotstextBox.Location = new System.Drawing.Point(97, 92);
            this.MineSlotstextBox.Name = "MineSlotstextBox";
            this.MineSlotstextBox.Size = new System.Drawing.Size(34, 20);
            this.MineSlotstextBox.TabIndex = 10;
            this.MineSlotstextBox.TextChanged += new System.EventHandler(this.MineSlotstextBox_TextChanged);
            // 
            // label70
            // 
            this.label70.AutoSize = true;
            this.label70.Location = new System.Drawing.Point(6, 95);
            this.label70.Name = "label70";
            this.label70.Size = new System.Drawing.Size(33, 13);
            this.label70.TabIndex = 9;
            this.label70.Text = "Slots:";
            // 
            // label69
            // 
            this.label69.AutoSize = true;
            this.label69.Location = new System.Drawing.Point(286, 45);
            this.label69.Name = "label69";
            this.label69.Size = new System.Drawing.Size(56, 13);
            this.label69.TabIndex = 8;
            this.label69.Text = "DropRate:";
            // 
            // label68
            // 
            this.label68.AutoSize = true;
            this.label68.Location = new System.Drawing.Point(286, 19);
            this.label68.Name = "label68";
            this.label68.Size = new System.Drawing.Size(46, 13);
            this.label68.TabIndex = 7;
            this.label68.Text = "HitRate:";
            // 
            // MineDropRatetextBox
            // 
            this.MineDropRatetextBox.Location = new System.Drawing.Point(362, 42);
            this.MineDropRatetextBox.Name = "MineDropRatetextBox";
            this.MineDropRatetextBox.Size = new System.Drawing.Size(34, 20);
            this.MineDropRatetextBox.TabIndex = 6;
            this.MineDropRatetextBox.TextChanged += new System.EventHandler(this.MineDropRatetextBox_TextChanged);
            // 
            // MineHitRatetextBox
            // 
            this.MineHitRatetextBox.Location = new System.Drawing.Point(362, 16);
            this.MineHitRatetextBox.Name = "MineHitRatetextBox";
            this.MineHitRatetextBox.Size = new System.Drawing.Size(34, 20);
            this.MineHitRatetextBox.TabIndex = 5;
            this.MineHitRatetextBox.TextChanged += new System.EventHandler(this.MineHitRatetextBox_TextChanged);
            // 
            // MineAttemptstextBox
            // 
            this.MineAttemptstextBox.Location = new System.Drawing.Point(97, 68);
            this.MineAttemptstextBox.Name = "MineAttemptstextBox";
            this.MineAttemptstextBox.Size = new System.Drawing.Size(34, 20);
            this.MineAttemptstextBox.TabIndex = 4;
            this.MineAttemptstextBox.TextChanged += new System.EventHandler(this.MineAttemptstextBox_TextChanged);
            // 
            // MineRegenDelaytextBox
            // 
            this.MineRegenDelaytextBox.Location = new System.Drawing.Point(97, 42);
            this.MineRegenDelaytextBox.Name = "MineRegenDelaytextBox";
            this.MineRegenDelaytextBox.Size = new System.Drawing.Size(34, 20);
            this.MineRegenDelaytextBox.TabIndex = 3;
            this.MineRegenDelaytextBox.TextChanged += new System.EventHandler(this.MineRegenDelaytextBox_TextChanged);
            // 
            // label67
            // 
            this.label67.AutoSize = true;
            this.label67.Location = new System.Drawing.Point(8, 71);
            this.label67.Name = "label67";
            this.label67.Size = new System.Drawing.Size(83, 13);
            this.label67.TabIndex = 2;
            this.label67.Text = "Attempts/regen:";
            // 
            // label66
            // 
            this.label66.AutoSize = true;
            this.label66.Location = new System.Drawing.Point(148, 45);
            this.label66.Name = "label66";
            this.label66.Size = new System.Drawing.Size(49, 13);
            this.label66.TabIndex = 1;
            this.label66.Text = "(minutes)";
            // 
            // label65
            // 
            this.label65.AutoSize = true;
            this.label65.Location = new System.Drawing.Point(8, 45);
            this.label65.Name = "label65";
            this.label65.Size = new System.Drawing.Size(70, 13);
            this.label65.TabIndex = 0;
            this.label65.Text = "Regen delay:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label78);
            this.tabPage3.Controls.Add(this.MineDropsIndexcomboBox);
            this.tabPage3.Controls.Add(this.groupBox8);
            this.tabPage3.Controls.Add(this.MineRemoveDropbutton);
            this.tabPage3.Controls.Add(this.MineAddDropbutton);
            this.tabPage3.Location = new System.Drawing.Point(4, 22);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Size = new System.Drawing.Size(645, 343);
            this.tabPage3.TabIndex = 1;
            this.tabPage3.Text = "Drops";
            this.tabPage3.UseVisualStyleBackColor = true;
            // 
            // label78
            // 
            this.label78.AutoSize = true;
            this.label78.Location = new System.Drawing.Point(7, 40);
            this.label78.Name = "label78";
            this.label78.Size = new System.Drawing.Size(38, 13);
            this.label78.TabIndex = 26;
            this.label78.Text = "Drops:";
            // 
            // MineDropsIndexcomboBox
            // 
            this.MineDropsIndexcomboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MineDropsIndexcomboBox.FormattingEnabled = true;
            this.MineDropsIndexcomboBox.Location = new System.Drawing.Point(12, 12);
            this.MineDropsIndexcomboBox.Name = "MineDropsIndexcomboBox";
            this.MineDropsIndexcomboBox.Size = new System.Drawing.Size(129, 21);
            this.MineDropsIndexcomboBox.TabIndex = 22;
            this.MineDropsIndexcomboBox.SelectedIndexChanged += new System.EventHandler(this.MineDropsIndexcomboBox_SelectedIndexChanged);
            // 
            // groupBox8
            // 
            this.groupBox8.Controls.Add(this.MineMaxBonustextBox);
            this.groupBox8.Controls.Add(this.label77);
            this.groupBox8.Controls.Add(this.MineBonusChancetextBox);
            this.groupBox8.Controls.Add(this.label76);
            this.groupBox8.Controls.Add(this.MineMaxQualitytextBox);
            this.groupBox8.Controls.Add(this.label75);
            this.groupBox8.Controls.Add(this.MineMinQualitytextBox);
            this.groupBox8.Controls.Add(this.label74);
            this.groupBox8.Controls.Add(this.MineMaxSlottextBox);
            this.groupBox8.Controls.Add(this.label73);
            this.groupBox8.Controls.Add(this.MineMinSlottextBox);
            this.groupBox8.Controls.Add(this.label72);
            this.groupBox8.Controls.Add(this.MineItemNametextBox);
            this.groupBox8.Controls.Add(this.label71);
            this.groupBox8.Location = new System.Drawing.Point(10, 39);
            this.groupBox8.Name = "groupBox8";
            this.groupBox8.Size = new System.Drawing.Size(188, 219);
            this.groupBox8.TabIndex = 25;
            this.groupBox8.TabStop = false;
            // 
            // MineMaxBonustextBox
            // 
            this.MineMaxBonustextBox.Location = new System.Drawing.Point(99, 151);
            this.MineMaxBonustextBox.Name = "MineMaxBonustextBox";
            this.MineMaxBonustextBox.Size = new System.Drawing.Size(34, 20);
            this.MineMaxBonustextBox.TabIndex = 34;
            this.MineMaxBonustextBox.TextChanged += new System.EventHandler(this.MineMaxBonustextBox_TextChanged);
            // 
            // label77
            // 
            this.label77.AutoSize = true;
            this.label77.Location = new System.Drawing.Point(10, 154);
            this.label77.Name = "label77";
            this.label77.Size = new System.Drawing.Size(84, 13);
            this.label77.TabIndex = 33;
            this.label77.Text = "Maximum Bonus";
            // 
            // MineBonusChancetextBox
            // 
            this.MineBonusChancetextBox.Location = new System.Drawing.Point(99, 127);
            this.MineBonusChancetextBox.Name = "MineBonusChancetextBox";
            this.MineBonusChancetextBox.Size = new System.Drawing.Size(34, 20);
            this.MineBonusChancetextBox.TabIndex = 32;
            this.MineBonusChancetextBox.TextChanged += new System.EventHandler(this.MineBonusChancetextBox_TextChanged);
            // 
            // label76
            // 
            this.label76.AutoSize = true;
            this.label76.Location = new System.Drawing.Point(10, 130);
            this.label76.Name = "label76";
            this.label76.Size = new System.Drawing.Size(77, 13);
            this.label76.TabIndex = 31;
            this.label76.Text = "Bonus Chance";
            // 
            // MineMaxQualitytextBox
            // 
            this.MineMaxQualitytextBox.Location = new System.Drawing.Point(99, 104);
            this.MineMaxQualitytextBox.Name = "MineMaxQualitytextBox";
            this.MineMaxQualitytextBox.Size = new System.Drawing.Size(34, 20);
            this.MineMaxQualitytextBox.TabIndex = 30;
            this.MineMaxQualitytextBox.TextChanged += new System.EventHandler(this.MineMaxQualitytextBox_TextChanged);
            // 
            // label75
            // 
            this.label75.AutoSize = true;
            this.label75.Location = new System.Drawing.Point(10, 107);
            this.label75.Name = "label75";
            this.label75.Size = new System.Drawing.Size(86, 13);
            this.label75.TabIndex = 29;
            this.label75.Text = "Maximum Quality";
            // 
            // MineMinQualitytextBox
            // 
            this.MineMinQualitytextBox.Location = new System.Drawing.Point(99, 81);
            this.MineMinQualitytextBox.Name = "MineMinQualitytextBox";
            this.MineMinQualitytextBox.Size = new System.Drawing.Size(34, 20);
            this.MineMinQualitytextBox.TabIndex = 28;
            this.MineMinQualitytextBox.TextChanged += new System.EventHandler(this.MineMinQualitytextBox_TextChanged);
            // 
            // label74
            // 
            this.label74.AutoSize = true;
            this.label74.Location = new System.Drawing.Point(10, 84);
            this.label74.Name = "label74";
            this.label74.Size = new System.Drawing.Size(83, 13);
            this.label74.TabIndex = 27;
            this.label74.Text = "Minimum Quality";
            // 
            // MineMaxSlottextBox
            // 
            this.MineMaxSlottextBox.Location = new System.Drawing.Point(99, 59);
            this.MineMaxSlottextBox.Name = "MineMaxSlottextBox";
            this.MineMaxSlottextBox.Size = new System.Drawing.Size(34, 20);
            this.MineMaxSlottextBox.TabIndex = 26;
            this.MineMaxSlottextBox.TextChanged += new System.EventHandler(this.MineMaxSlottextBox_TextChanged);
            // 
            // label73
            // 
            this.label73.AutoSize = true;
            this.label73.Location = new System.Drawing.Point(10, 62);
            this.label73.Name = "label73";
            this.label73.Size = new System.Drawing.Size(51, 13);
            this.label73.TabIndex = 25;
            this.label73.Text = "Max Slot:";
            // 
            // MineMinSlottextBox
            // 
            this.MineMinSlottextBox.Location = new System.Drawing.Point(99, 36);
            this.MineMinSlottextBox.Name = "MineMinSlottextBox";
            this.MineMinSlottextBox.Size = new System.Drawing.Size(34, 20);
            this.MineMinSlottextBox.TabIndex = 24;
            this.MineMinSlottextBox.TextChanged += new System.EventHandler(this.MineMinSlottextBox_TextChanged);
            // 
            // label72
            // 
            this.label72.AutoSize = true;
            this.label72.Location = new System.Drawing.Point(10, 39);
            this.label72.Name = "label72";
            this.label72.Size = new System.Drawing.Size(48, 13);
            this.label72.TabIndex = 23;
            this.label72.Text = "Min Slot:";
            // 
            // MineItemNametextBox
            // 
            this.MineItemNametextBox.Location = new System.Drawing.Point(99, 13);
            this.MineItemNametextBox.Name = "MineItemNametextBox";
            this.MineItemNametextBox.Size = new System.Drawing.Size(83, 20);
            this.MineItemNametextBox.TabIndex = 22;
            this.MineItemNametextBox.TextChanged += new System.EventHandler(this.MineItemNametextBox_TextChanged);
            // 
            // label71
            // 
            this.label71.AutoSize = true;
            this.label71.Location = new System.Drawing.Point(10, 16);
            this.label71.Name = "label71";
            this.label71.Size = new System.Drawing.Size(58, 13);
            this.label71.TabIndex = 21;
            this.label71.Text = "ItemName:";
            // 
            // MineRemoveDropbutton
            // 
            this.MineRemoveDropbutton.Location = new System.Drawing.Point(177, 12);
            this.MineRemoveDropbutton.Name = "MineRemoveDropbutton";
            this.MineRemoveDropbutton.Size = new System.Drawing.Size(21, 21);
            this.MineRemoveDropbutton.TabIndex = 24;
            this.MineRemoveDropbutton.Text = "-";
            this.MineRemoveDropbutton.UseVisualStyleBackColor = true;
            this.MineRemoveDropbutton.Click += new System.EventHandler(this.MineRemoveDropbutton_Click);
            // 
            // MineAddDropbutton
            // 
            this.MineAddDropbutton.Location = new System.Drawing.Point(148, 12);
            this.MineAddDropbutton.Name = "MineAddDropbutton";
            this.MineAddDropbutton.Size = new System.Drawing.Size(21, 21);
            this.MineAddDropbutton.TabIndex = 23;
            this.MineAddDropbutton.Text = "+";
            this.MineAddDropbutton.UseVisualStyleBackColor = true;
            this.MineAddDropbutton.Click += new System.EventHandler(this.MineAddDropbutton_Click);
            // 
            // MineAddIndexbutton
            // 
            this.MineAddIndexbutton.Location = new System.Drawing.Point(130, 350);
            this.MineAddIndexbutton.Name = "MineAddIndexbutton";
            this.MineAddIndexbutton.Size = new System.Drawing.Size(21, 21);
            this.MineAddIndexbutton.TabIndex = 30;
            this.MineAddIndexbutton.Text = "+";
            this.MineAddIndexbutton.UseVisualStyleBackColor = true;
            this.MineAddIndexbutton.Click += new System.EventHandler(this.MineAddIndexbutton_Click);
            // 
            // Gathering_tab
            // 
            this.Gathering_tab.Location = new System.Drawing.Point(4, 22);
            this.Gathering_tab.Name = "Gathering_tab";
            this.Gathering_tab.Padding = new System.Windows.Forms.Padding(3);
            this.Gathering_tab.Size = new System.Drawing.Size(815, 385);
            this.Gathering_tab.TabIndex = 1;
            this.Gathering_tab.Text = "Gathering";
            this.Gathering_tab.UseVisualStyleBackColor = true;
            // 
            // Fishing_tab
            // 
            this.Fishing_tab.Controls.Add(this.groupBox1);
            this.Fishing_tab.Controls.Add(this.label5);
            this.Fishing_tab.Controls.Add(this.FishingSuccessRateMultiplierTextBox);
            this.Fishing_tab.Controls.Add(this.label3);
            this.Fishing_tab.Controls.Add(this.label2);
            this.Fishing_tab.Controls.Add(this.label1);
            this.Fishing_tab.Controls.Add(this.FishingDelayTextBox);
            this.Fishing_tab.Controls.Add(this.FishingSuccessRateStartTextBox);
            this.Fishing_tab.Controls.Add(this.FishingAttemptsTextBox);
            this.Fishing_tab.Location = new System.Drawing.Point(4, 22);
            this.Fishing_tab.Name = "Fishing_tab";
            this.Fishing_tab.Size = new System.Drawing.Size(815, 385);
            this.Fishing_tab.TabIndex = 2;
            this.Fishing_tab.Text = "Fishing";
            this.Fishing_tab.UseVisualStyleBackColor = true;
            // 
            // Crafting_tab
            // 
            this.Crafting_tab.Location = new System.Drawing.Point(4, 22);
            this.Crafting_tab.Name = "Crafting_tab";
            this.Crafting_tab.Size = new System.Drawing.Size(815, 385);
            this.Crafting_tab.TabIndex = 3;
            this.Crafting_tab.Text = "Crafting";
            this.Crafting_tab.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.MonsterSpawnChanceTextBox);
            this.groupBox1.Controls.Add(this.FishingMobIndexComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(20, 166);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(353, 72);
            this.groupBox1.TabIndex = 21;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Monster";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Mob Spawn : ";
            // 
            // MonsterSpawnChanceTextBox
            // 
            this.MonsterSpawnChanceTextBox.Location = new System.Drawing.Point(137, 41);
            this.MonsterSpawnChanceTextBox.Name = "MonsterSpawnChanceTextBox";
            this.MonsterSpawnChanceTextBox.Size = new System.Drawing.Size(100, 20);
            this.MonsterSpawnChanceTextBox.TabIndex = 3;
            this.MonsterSpawnChanceTextBox.TextChanged += new System.EventHandler(this.MonsterSpawnChanceTextBox_TextChanged);
            // 
            // FishingMobIndexComboBox
            // 
            this.FishingMobIndexComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FishingMobIndexComboBox.FormattingEnabled = true;
            this.FishingMobIndexComboBox.Location = new System.Drawing.Point(137, 16);
            this.FishingMobIndexComboBox.Name = "FishingMobIndexComboBox";
            this.FishingMobIndexComboBox.Size = new System.Drawing.Size(100, 21);
            this.FishingMobIndexComboBox.TabIndex = 10;
            this.FishingMobIndexComboBox.SelectedIndexChanged += new System.EventHandler(this.FishingMobIndexComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Mob Spawn Chance % : ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(23, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 13);
            this.label5.TabIndex = 20;
            this.label5.Text = "Success Rate Multiplier : ";
            // 
            // FishingSuccessRateMultiplierTextBox
            // 
            this.FishingSuccessRateMultiplierTextBox.Location = new System.Drawing.Point(157, 76);
            this.FishingSuccessRateMultiplierTextBox.Name = "FishingSuccessRateMultiplierTextBox";
            this.FishingSuccessRateMultiplierTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingSuccessRateMultiplierTextBox.TabIndex = 19;
            this.FishingSuccessRateMultiplierTextBox.TextChanged += new System.EventHandler(this.FishingSuccessRateMultiplierTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(23, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 18;
            this.label3.Text = "Delay / ms : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(23, 53);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Success Rate Start % : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 27);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 16;
            this.label1.Text = "Attempts / round : ";
            // 
            // FishingDelayTextBox
            // 
            this.FishingDelayTextBox.Location = new System.Drawing.Point(157, 102);
            this.FishingDelayTextBox.Name = "FishingDelayTextBox";
            this.FishingDelayTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingDelayTextBox.TabIndex = 15;
            this.FishingDelayTextBox.TextChanged += new System.EventHandler(this.FishingDelayTextBox_TextChanged);
            // 
            // FishingSuccessRateStartTextBox
            // 
            this.FishingSuccessRateStartTextBox.Location = new System.Drawing.Point(157, 50);
            this.FishingSuccessRateStartTextBox.Name = "FishingSuccessRateStartTextBox";
            this.FishingSuccessRateStartTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingSuccessRateStartTextBox.TabIndex = 14;
            this.FishingSuccessRateStartTextBox.TextChanged += new System.EventHandler(this.FishingSuccessRateStartTextBox_TextChanged);
            // 
            // FishingAttemptsTextBox
            // 
            this.FishingAttemptsTextBox.Location = new System.Drawing.Point(157, 24);
            this.FishingAttemptsTextBox.Name = "FishingAttemptsTextBox";
            this.FishingAttemptsTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingAttemptsTextBox.TabIndex = 13;
            this.FishingAttemptsTextBox.TextChanged += new System.EventHandler(this.FishingAttemptsTextBox_TextChanged);
            // 
            // ProfessionsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(847, 435);
            this.Controls.Add(this.ProfSettings);
            this.Name = "ProfessionsForm";
            this.Text = "ProfessionsForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ProfessionsForm_FormClosed);
            this.Load += new System.EventHandler(this.ProfessionsForm_Load);
            this.ProfSettings.ResumeLayout(false);
            this.Mining_tab.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox7.ResumeLayout(false);
            this.groupBox7.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.groupBox8.ResumeLayout(false);
            this.groupBox8.PerformLayout();
            this.Fishing_tab.ResumeLayout(false);
            this.Fishing_tab.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl ProfSettings;
        private System.Windows.Forms.TabPage Mining_tab;
        private System.Windows.Forms.TabPage Gathering_tab;
        private System.Windows.Forms.ListBox Mines_lb;
        private System.Windows.Forms.Button MineRemoveIndexbutton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox7;
        private System.Windows.Forms.Label label79;
        private System.Windows.Forms.TextBox MineNametextBox;
        private System.Windows.Forms.TextBox MineSlotstextBox;
        private System.Windows.Forms.Label label70;
        private System.Windows.Forms.Label label69;
        private System.Windows.Forms.Label label68;
        private System.Windows.Forms.TextBox MineDropRatetextBox;
        private System.Windows.Forms.TextBox MineHitRatetextBox;
        private System.Windows.Forms.TextBox MineAttemptstextBox;
        private System.Windows.Forms.TextBox MineRegenDelaytextBox;
        private System.Windows.Forms.Label label67;
        private System.Windows.Forms.Label label66;
        private System.Windows.Forms.Label label65;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.Label label78;
        private System.Windows.Forms.ComboBox MineDropsIndexcomboBox;
        private System.Windows.Forms.GroupBox groupBox8;
        private System.Windows.Forms.TextBox MineMaxBonustextBox;
        private System.Windows.Forms.Label label77;
        private System.Windows.Forms.TextBox MineBonusChancetextBox;
        private System.Windows.Forms.Label label76;
        private System.Windows.Forms.TextBox MineMaxQualitytextBox;
        private System.Windows.Forms.Label label75;
        private System.Windows.Forms.TextBox MineMinQualitytextBox;
        private System.Windows.Forms.Label label74;
        private System.Windows.Forms.TextBox MineMaxSlottextBox;
        private System.Windows.Forms.Label label73;
        private System.Windows.Forms.TextBox MineMinSlottextBox;
        private System.Windows.Forms.Label label72;
        private System.Windows.Forms.TextBox MineItemNametextBox;
        private System.Windows.Forms.Label label71;
        private System.Windows.Forms.Button MineRemoveDropbutton;
        private System.Windows.Forms.Button MineAddDropbutton;
        private System.Windows.Forms.Button MineAddIndexbutton;
        private System.Windows.Forms.TabPage Fishing_tab;
        private System.Windows.Forms.TabPage Crafting_tab;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox MonsterSpawnChanceTextBox;
        private System.Windows.Forms.ComboBox FishingMobIndexComboBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox FishingSuccessRateMultiplierTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FishingDelayTextBox;
        private System.Windows.Forms.TextBox FishingSuccessRateStartTextBox;
        private System.Windows.Forms.TextBox FishingAttemptsTextBox;
    }
}