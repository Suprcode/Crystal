namespace Server
{
    partial class NPCInfoForm
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            TeleportToCheckBox = new CheckBox();
            label15 = new Label();
            BigMapIconTextBox = new TextBox();
            ShowBigMapCheckBox = new CheckBox();
            label14 = new Label();
            ConquestHidden_combo = new ComboBox();
            label2 = new Label();
            MapComboBox = new ComboBox();
            label11 = new Label();
            OpenNButton = new Button();
            NFileNameTextBox = new TextBox();
            label29 = new Label();
            NRateTextBox = new TextBox();
            ClearHButton = new Button();
            NNameTextBox = new TextBox();
            label13 = new Label();
            NPCIndexTextBox = new TextBox();
            label24 = new Label();
            label1 = new Label();
            NImageTextBox = new TextBox();
            NXTextBox = new TextBox();
            label28 = new Label();
            label30 = new Label();
            NYTextBox = new TextBox();
            tabPage2 = new TabPage();
            ConquestVisible_checkbox = new CheckBox();
            Flag_textbox = new TextBox();
            label12 = new Label();
            label10 = new Label();
            Day_combo = new ComboBox();
            Class_combo = new ComboBox();
            EndMin_num = new NumericUpDown();
            EndHour_combo = new ComboBox();
            label8 = new Label();
            label9 = new Label();
            StartMin_num = new NumericUpDown();
            StartHour_combo = new ComboBox();
            TimeVisible_checkbox = new CheckBox();
            label7 = new Label();
            MaxLev_textbox = new TextBox();
            label6 = new Label();
            label5 = new Label();
            label4 = new Label();
            label3 = new Label();
            MinLev_textbox = new TextBox();
            RemoveButton = new Button();
            AddButton = new Button();
            NPCInfoListBox = new ListBox();
            PasteMButton = new Button();
            CopyMButton = new Button();
            ExportButton = new Button();
            ImportButton = new Button();
            ExportSelectedButton = new Button();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)EndMin_num).BeginInit();
            ((System.ComponentModel.ISupportInitialize)StartMin_num).BeginInit();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(298, 47);
            tabControl1.Margin = new Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(606, 339);
            tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(TeleportToCheckBox);
            tabPage1.Controls.Add(label15);
            tabPage1.Controls.Add(BigMapIconTextBox);
            tabPage1.Controls.Add(ShowBigMapCheckBox);
            tabPage1.Controls.Add(label14);
            tabPage1.Controls.Add(ConquestHidden_combo);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(MapComboBox);
            tabPage1.Controls.Add(label11);
            tabPage1.Controls.Add(OpenNButton);
            tabPage1.Controls.Add(NFileNameTextBox);
            tabPage1.Controls.Add(label29);
            tabPage1.Controls.Add(NRateTextBox);
            tabPage1.Controls.Add(ClearHButton);
            tabPage1.Controls.Add(NNameTextBox);
            tabPage1.Controls.Add(label13);
            tabPage1.Controls.Add(NPCIndexTextBox);
            tabPage1.Controls.Add(label24);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(NImageTextBox);
            tabPage1.Controls.Add(NXTextBox);
            tabPage1.Controls.Add(label28);
            tabPage1.Controls.Add(label30);
            tabPage1.Controls.Add(NYTextBox);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new Size(598, 311);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Info";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // TeleportToCheckBox
            // 
            TeleportToCheckBox.AutoSize = true;
            TeleportToCheckBox.Location = new Point(293, 268);
            TeleportToCheckBox.Margin = new Padding(4, 3, 4, 3);
            TeleportToCheckBox.Name = "TeleportToCheckBox";
            TeleportToCheckBox.Size = new Size(107, 19);
            TeleportToCheckBox.TabIndex = 63;
            TeleportToCheckBox.Text = "Can Teleport To";
            TeleportToCheckBox.UseVisualStyleBackColor = true;
            TeleportToCheckBox.CheckedChanged += TeleportToCheckBox_CheckedChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(177, 270);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(33, 15);
            label15.TabIndex = 62;
            label15.Text = "Icon:";
            // 
            // BigMapIconTextBox
            // 
            BigMapIconTextBox.Location = new Point(219, 265);
            BigMapIconTextBox.Margin = new Padding(4, 3, 4, 3);
            BigMapIconTextBox.MaxLength = 5;
            BigMapIconTextBox.Name = "BigMapIconTextBox";
            BigMapIconTextBox.Size = new Size(42, 23);
            BigMapIconTextBox.TabIndex = 61;
            BigMapIconTextBox.TextChanged += BigMapIconTextBox_TextChanged;
            // 
            // ShowBigMapCheckBox
            // 
            ShowBigMapCheckBox.AutoSize = true;
            ShowBigMapCheckBox.Location = new Point(31, 269);
            ShowBigMapCheckBox.Margin = new Padding(4, 3, 4, 3);
            ShowBigMapCheckBox.Name = "ShowBigMapCheckBox";
            ShowBigMapCheckBox.Size = new Size(116, 19);
            ShowBigMapCheckBox.TabIndex = 60;
            ShowBigMapCheckBox.Text = "Show on BigMap";
            ShowBigMapCheckBox.UseVisualStyleBackColor = true;
            ShowBigMapCheckBox.CheckedChanged += ShowBigMapCheckBox_CheckedChanged;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(30, 225);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(61, 15);
            label14.TabIndex = 59;
            label14.Text = "Conquest:";
            // 
            // ConquestHidden_combo
            // 
            ConquestHidden_combo.DropDownStyle = ComboBoxStyle.DropDownList;
            ConquestHidden_combo.FormattingEnabled = true;
            ConquestHidden_combo.Items.AddRange(new object[] { "", "Warrior", "Wizard", "Taoist", "Assassin", "Archer" });
            ConquestHidden_combo.Location = new Point(102, 220);
            ConquestHidden_combo.Margin = new Padding(4, 3, 4, 3);
            ConquestHidden_combo.Name = "ConquestHidden_combo";
            ConquestHidden_combo.Size = new Size(153, 23);
            ConquestHidden_combo.TabIndex = 58;
            ConquestHidden_combo.SelectedIndexChanged += ConquestHidden_combo_SelectedIndexChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(58, 100);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(34, 15);
            label2.TabIndex = 32;
            label2.Text = "Map:";
            // 
            // MapComboBox
            // 
            MapComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MapComboBox.FormattingEnabled = true;
            MapComboBox.Location = new Point(102, 97);
            MapComboBox.Margin = new Padding(4, 3, 4, 3);
            MapComboBox.Name = "MapComboBox";
            MapComboBox.Size = new Size(153, 23);
            MapComboBox.TabIndex = 31;
            MapComboBox.SelectedIndexChanged += MapComboBox_SelectedIndexChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(28, 40);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(63, 15);
            label11.TabIndex = 23;
            label11.Text = "File Name:";
            // 
            // OpenNButton
            // 
            OpenNButton.Location = new Point(163, 2);
            OpenNButton.Margin = new Padding(4, 3, 4, 3);
            OpenNButton.Name = "OpenNButton";
            OpenNButton.Size = new Size(88, 27);
            OpenNButton.TabIndex = 30;
            OpenNButton.Text = "Open Script";
            OpenNButton.UseVisualStyleBackColor = true;
            OpenNButton.Click += OpenNButton_Click;
            // 
            // NFileNameTextBox
            // 
            NFileNameTextBox.Location = new Point(102, 37);
            NFileNameTextBox.Margin = new Padding(4, 3, 4, 3);
            NFileNameTextBox.MaxLength = 50;
            NFileNameTextBox.Name = "NFileNameTextBox";
            NFileNameTextBox.Size = new Size(209, 23);
            NFileNameTextBox.TabIndex = 22;
            NFileNameTextBox.TextChanged += NFileNameTextBox_TextChanged;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(167, 163);
            label29.Margin = new Padding(4, 0, 4, 0);
            label29.Name = "label29";
            label29.Size = new Size(33, 15);
            label29.TabIndex = 21;
            label29.Text = "Rate:";
            // 
            // NRateTextBox
            // 
            NRateTextBox.Location = new Point(212, 157);
            NRateTextBox.Margin = new Padding(4, 3, 4, 3);
            NRateTextBox.MaxLength = 3;
            NRateTextBox.Name = "NRateTextBox";
            NRateTextBox.Size = new Size(42, 23);
            NRateTextBox.TabIndex = 20;
            NRateTextBox.TextChanged += NRateTextBox_TextChanged;
            // 
            // ClearHButton
            // 
            ClearHButton.Location = new Point(181, 187);
            ClearHButton.Margin = new Padding(4, 3, 4, 3);
            ClearHButton.Name = "ClearHButton";
            ClearHButton.Size = new Size(88, 27);
            ClearHButton.TabIndex = 19;
            ClearHButton.Text = "Clear History";
            ClearHButton.UseVisualStyleBackColor = true;
            // 
            // NNameTextBox
            // 
            NNameTextBox.Location = new Point(102, 67);
            NNameTextBox.Margin = new Padding(4, 3, 4, 3);
            NNameTextBox.Name = "NNameTextBox";
            NNameTextBox.Size = new Size(209, 23);
            NNameTextBox.TabIndex = 14;
            NNameTextBox.TextChanged += NNameTextBox_TextChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(50, 70);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(42, 15);
            label13.TabIndex = 15;
            label13.Text = "Name:";
            // 
            // NPCIndexTextBox
            // 
            NPCIndexTextBox.Location = new Point(102, 5);
            NPCIndexTextBox.Margin = new Padding(4, 3, 4, 3);
            NPCIndexTextBox.Name = "NPCIndexTextBox";
            NPCIndexTextBox.ReadOnly = true;
            NPCIndexTextBox.Size = new Size(54, 23);
            NPCIndexTextBox.TabIndex = 0;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(49, 163);
            label24.Margin = new Padding(4, 0, 4, 0);
            label24.Name = "label24";
            label24.Size = new Size(43, 15);
            label24.TabIndex = 13;
            label24.Text = "Image:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(23, 8);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 4;
            label1.Text = "NPC Index:";
            // 
            // NImageTextBox
            // 
            NImageTextBox.Location = new Point(102, 158);
            NImageTextBox.Margin = new Padding(4, 3, 4, 3);
            NImageTextBox.MaxLength = 5;
            NImageTextBox.Name = "NImageTextBox";
            NImageTextBox.Size = new Size(42, 23);
            NImageTextBox.TabIndex = 11;
            NImageTextBox.TextChanged += NImageTextBox_TextChanged;
            // 
            // NXTextBox
            // 
            NXTextBox.Location = new Point(102, 128);
            NXTextBox.Margin = new Padding(4, 3, 4, 3);
            NXTextBox.MaxLength = 5;
            NXTextBox.Name = "NXTextBox";
            NXTextBox.Size = new Size(42, 23);
            NXTextBox.TabIndex = 2;
            NXTextBox.TextChanged += NXTextBox_TextChanged;
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(155, 132);
            label28.Margin = new Padding(4, 0, 4, 0);
            label28.Name = "label28";
            label28.Size = new Size(48, 15);
            label28.TabIndex = 10;
            label28.Text = "From Y:";
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(44, 132);
            label30.Margin = new Padding(4, 0, 4, 0);
            label30.Name = "label30";
            label30.Size = new Size(48, 15);
            label30.TabIndex = 3;
            label30.Text = "From X:";
            // 
            // NYTextBox
            // 
            NYTextBox.Location = new Point(212, 128);
            NYTextBox.Margin = new Padding(4, 3, 4, 3);
            NYTextBox.MaxLength = 5;
            NYTextBox.Name = "NYTextBox";
            NYTextBox.Size = new Size(42, 23);
            NYTextBox.TabIndex = 3;
            NYTextBox.TextChanged += NYTextBox_TextChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(ConquestVisible_checkbox);
            tabPage2.Controls.Add(Flag_textbox);
            tabPage2.Controls.Add(label12);
            tabPage2.Controls.Add(label10);
            tabPage2.Controls.Add(Day_combo);
            tabPage2.Controls.Add(Class_combo);
            tabPage2.Controls.Add(EndMin_num);
            tabPage2.Controls.Add(EndHour_combo);
            tabPage2.Controls.Add(label8);
            tabPage2.Controls.Add(label9);
            tabPage2.Controls.Add(StartMin_num);
            tabPage2.Controls.Add(StartHour_combo);
            tabPage2.Controls.Add(TimeVisible_checkbox);
            tabPage2.Controls.Add(label7);
            tabPage2.Controls.Add(MaxLev_textbox);
            tabPage2.Controls.Add(label6);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(MinLev_textbox);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Size = new Size(598, 311);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Visibility";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // ConquestVisible_checkbox
            // 
            ConquestVisible_checkbox.AutoSize = true;
            ConquestVisible_checkbox.CheckAlign = ContentAlignment.MiddleRight;
            ConquestVisible_checkbox.Location = new Point(200, 135);
            ConquestVisible_checkbox.Margin = new Padding(4, 3, 4, 3);
            ConquestVisible_checkbox.Name = "ConquestVisible_checkbox";
            ConquestVisible_checkbox.Size = new Size(152, 19);
            ConquestVisible_checkbox.TabIndex = 56;
            ConquestVisible_checkbox.Text = "Visible during Conquest";
            ConquestVisible_checkbox.UseVisualStyleBackColor = true;
            ConquestVisible_checkbox.CheckedChanged += ConquestVisible_checkbox_CheckedChanged;
            // 
            // Flag_textbox
            // 
            Flag_textbox.Location = new Point(131, 100);
            Flag_textbox.Margin = new Padding(4, 3, 4, 3);
            Flag_textbox.MaxLength = 3;
            Flag_textbox.Name = "Flag_textbox";
            Flag_textbox.Size = new Size(56, 23);
            Flag_textbox.TabIndex = 55;
            Flag_textbox.TextChanged += Flag_textbox_TextChanged;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(48, 104);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(76, 15);
            label12.TabIndex = 54;
            label12.Text = "Needed Flag:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(48, 73);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(76, 15);
            label10.TabIndex = 53;
            label10.Text = "Day to Show:";
            // 
            // Day_combo
            // 
            Day_combo.DropDownStyle = ComboBoxStyle.DropDownList;
            Day_combo.FormattingEnabled = true;
            Day_combo.Items.AddRange(new object[] { "", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" });
            Day_combo.Location = new Point(131, 68);
            Day_combo.Margin = new Padding(4, 3, 4, 3);
            Day_combo.Name = "Day_combo";
            Day_combo.Size = new Size(190, 23);
            Day_combo.TabIndex = 52;
            Day_combo.SelectedIndexChanged += Day_combo_SelectedIndexChanged;
            // 
            // Class_combo
            // 
            Class_combo.DropDownStyle = ComboBoxStyle.DropDownList;
            Class_combo.FormattingEnabled = true;
            Class_combo.Items.AddRange(new object[] { "", "Warrior", "Wizard", "Taoist", "Assassin", "Archer" });
            Class_combo.Location = new Point(131, 33);
            Class_combo.Margin = new Padding(4, 3, 4, 3);
            Class_combo.Name = "Class_combo";
            Class_combo.Size = new Size(101, 23);
            Class_combo.TabIndex = 51;
            Class_combo.SelectedIndexChanged += Class_combo_SelectedIndexChanged;
            // 
            // EndMin_num
            // 
            EndMin_num.Location = new Point(278, 196);
            EndMin_num.Margin = new Padding(4, 3, 4, 3);
            EndMin_num.Maximum = new decimal(new int[] { 59, 0, 0, 0 });
            EndMin_num.Minimum = new decimal(new int[] { 1, 0, 0, 0 });
            EndMin_num.Name = "EndMin_num";
            EndMin_num.Size = new Size(55, 23);
            EndMin_num.TabIndex = 50;
            EndMin_num.Value = new decimal(new int[] { 1, 0, 0, 0 });
            EndMin_num.ValueChanged += EndMin_num_ValueChanged;
            // 
            // EndHour_combo
            // 
            EndHour_combo.DropDownStyle = ComboBoxStyle.DropDownList;
            EndHour_combo.FormattingEnabled = true;
            EndHour_combo.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" });
            EndHour_combo.Location = new Point(131, 195);
            EndHour_combo.Margin = new Padding(4, 3, 4, 3);
            EndHour_combo.Name = "EndHour_combo";
            EndHour_combo.Size = new Size(56, 23);
            EndHour_combo.TabIndex = 49;
            EndHour_combo.SelectedIndexChanged += EndHour_combo_SelectedIndexChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(200, 198);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(71, 15);
            label8.TabIndex = 48;
            label8.Text = "End Minute:";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(63, 198);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(60, 15);
            label9.TabIndex = 47;
            label9.Text = "End Hour:";
            // 
            // StartMin_num
            // 
            StartMin_num.Location = new Point(278, 166);
            StartMin_num.Margin = new Padding(4, 3, 4, 3);
            StartMin_num.Maximum = new decimal(new int[] { 58, 0, 0, 0 });
            StartMin_num.Name = "StartMin_num";
            StartMin_num.Size = new Size(55, 23);
            StartMin_num.TabIndex = 46;
            StartMin_num.ValueChanged += StartMin_num_ValueChanged;
            // 
            // StartHour_combo
            // 
            StartHour_combo.DropDownStyle = ComboBoxStyle.DropDownList;
            StartHour_combo.FormattingEnabled = true;
            StartHour_combo.Items.AddRange(new object[] { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14", "15", "16", "17", "18", "19", "20", "21", "22", "23" });
            StartHour_combo.Location = new Point(131, 165);
            StartHour_combo.Margin = new Padding(4, 3, 4, 3);
            StartHour_combo.Name = "StartHour_combo";
            StartHour_combo.Size = new Size(56, 23);
            StartHour_combo.TabIndex = 45;
            StartHour_combo.SelectedIndexChanged += StartHour_combo_SelectedIndexChanged;
            // 
            // TimeVisible_checkbox
            // 
            TimeVisible_checkbox.AutoSize = true;
            TimeVisible_checkbox.CheckAlign = ContentAlignment.MiddleRight;
            TimeVisible_checkbox.Location = new Point(35, 135);
            TimeVisible_checkbox.Margin = new Padding(4, 3, 4, 3);
            TimeVisible_checkbox.Name = "TimeVisible_checkbox";
            TimeVisible_checkbox.Size = new Size(153, 19);
            TimeVisible_checkbox.TabIndex = 44;
            TimeVisible_checkbox.Text = "Only Visible at set Times";
            TimeVisible_checkbox.UseVisualStyleBackColor = true;
            TimeVisible_checkbox.CheckedChanged += TimeVisible_checkbox_CheckedChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(195, 7);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(63, 15);
            label7.TabIndex = 43;
            label7.Text = "Max Level:";
            // 
            // MaxLev_textbox
            // 
            MaxLev_textbox.Location = new Point(264, 3);
            MaxLev_textbox.Margin = new Padding(4, 3, 4, 3);
            MaxLev_textbox.MaxLength = 3;
            MaxLev_textbox.Name = "MaxLev_textbox";
            MaxLev_textbox.Size = new Size(56, 23);
            MaxLev_textbox.TabIndex = 42;
            MaxLev_textbox.TextChanged += MaxLev_textbox_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(36, 37);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(87, 15);
            label6.TabIndex = 40;
            label6.Text = "Class Required:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(200, 168);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(75, 15);
            label5.TabIndex = 37;
            label5.Text = "Start Minute:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(63, 168);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(64, 15);
            label4.TabIndex = 36;
            label4.Text = "Start Hour:";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(65, 7);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(61, 15);
            label3.TabIndex = 34;
            label3.Text = "Min Level:";
            // 
            // MinLev_textbox
            // 
            MinLev_textbox.Location = new Point(131, 3);
            MinLev_textbox.Margin = new Padding(4, 3, 4, 3);
            MinLev_textbox.MaxLength = 3;
            MinLev_textbox.Name = "MinLev_textbox";
            MinLev_textbox.Size = new Size(56, 23);
            MinLev_textbox.TabIndex = 33;
            MinLev_textbox.TextChanged += MinLev_textbox_TextChanged;
            // 
            // RemoveButton
            // 
            RemoveButton.Location = new Point(108, 14);
            RemoveButton.Margin = new Padding(4, 3, 4, 3);
            RemoveButton.Name = "RemoveButton";
            RemoveButton.Size = new Size(88, 27);
            RemoveButton.TabIndex = 14;
            RemoveButton.Text = "Remove";
            RemoveButton.UseVisualStyleBackColor = true;
            RemoveButton.Click += RemoveButton_Click;
            // 
            // AddButton
            // 
            AddButton.Location = new Point(14, 14);
            AddButton.Margin = new Padding(4, 3, 4, 3);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(88, 27);
            AddButton.TabIndex = 13;
            AddButton.Text = "Add";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // NPCInfoListBox
            // 
            NPCInfoListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            NPCInfoListBox.FormattingEnabled = true;
            NPCInfoListBox.ItemHeight = 15;
            NPCInfoListBox.Location = new Point(14, 47);
            NPCInfoListBox.Margin = new Padding(4, 3, 4, 3);
            NPCInfoListBox.Name = "NPCInfoListBox";
            NPCInfoListBox.SelectionMode = SelectionMode.MultiExtended;
            NPCInfoListBox.Size = new Size(276, 334);
            NPCInfoListBox.TabIndex = 15;
            NPCInfoListBox.SelectedIndexChanged += NPCInfoListBox_SelectedIndexChanged;
            // 
            // PasteMButton
            // 
            PasteMButton.Location = new Point(298, 14);
            PasteMButton.Margin = new Padding(4, 3, 4, 3);
            PasteMButton.Name = "PasteMButton";
            PasteMButton.Size = new Size(88, 27);
            PasteMButton.TabIndex = 22;
            PasteMButton.Text = "Paste";
            PasteMButton.UseVisualStyleBackColor = true;
            PasteMButton.Click += PasteMButton_Click;
            // 
            // CopyMButton
            // 
            CopyMButton.Location = new Point(203, 14);
            CopyMButton.Margin = new Padding(4, 3, 4, 3);
            CopyMButton.Name = "CopyMButton";
            CopyMButton.Size = new Size(88, 27);
            CopyMButton.TabIndex = 21;
            CopyMButton.Text = "Copy";
            CopyMButton.UseVisualStyleBackColor = true;
            CopyMButton.Click += CopyMButton_Click;
            // 
            // ExportButton
            // 
            ExportButton.Location = new Point(818, 14);
            ExportButton.Margin = new Padding(4, 3, 4, 3);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(88, 27);
            ExportButton.TabIndex = 23;
            ExportButton.Text = "Export All";
            ExportButton.UseVisualStyleBackColor = true;
            ExportButton.Click += ExportAllButton_Click;
            // 
            // ImportButton
            // 
            ImportButton.Location = new Point(581, 14);
            ImportButton.Margin = new Padding(4, 3, 4, 3);
            ImportButton.Name = "ImportButton";
            ImportButton.Size = new Size(88, 27);
            ImportButton.TabIndex = 24;
            ImportButton.Text = "Import";
            ImportButton.UseVisualStyleBackColor = true;
            ImportButton.Click += ImportButton_Click;
            // 
            // ExportSelectedButton
            // 
            ExportSelectedButton.Location = new Point(674, 14);
            ExportSelectedButton.Margin = new Padding(4, 3, 4, 3);
            ExportSelectedButton.Name = "ExportSelectedButton";
            ExportSelectedButton.Size = new Size(136, 27);
            ExportSelectedButton.TabIndex = 25;
            ExportSelectedButton.Text = "Export Selected";
            ExportSelectedButton.UseVisualStyleBackColor = true;
            ExportSelectedButton.Click += ExportSelected_Click;
            // 
            // NPCInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(917, 400);
            Controls.Add(ExportSelectedButton);
            Controls.Add(ImportButton);
            Controls.Add(ExportButton);
            Controls.Add(PasteMButton);
            Controls.Add(CopyMButton);
            Controls.Add(tabControl1);
            Controls.Add(RemoveButton);
            Controls.Add(AddButton);
            Controls.Add(NPCInfoListBox);
            Margin = new Padding(4, 3, 4, 3);
            Name = "NPCInfoForm";
            Text = "NPCInfoForm";
            FormClosed += NPCInfoForm_FormClosed;
            Load += NPCInfoForm_Load;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)EndMin_num).EndInit();
            ((System.ComponentModel.ISupportInitialize)StartMin_num).EndInit();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox NPCIndexTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button PasteMButton;
        private System.Windows.Forms.Button CopyMButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button ExportSelectedButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button OpenNButton;
        private System.Windows.Forms.TextBox NFileNameTextBox;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox NRateTextBox;
        private System.Windows.Forms.Button ClearHButton;
        private System.Windows.Forms.TextBox NNameTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label label24;
        private System.Windows.Forms.TextBox NImageTextBox;
        private System.Windows.Forms.TextBox NXTextBox;
        private System.Windows.Forms.Label label28;
        private System.Windows.Forms.Label label30;
        private System.Windows.Forms.TextBox NYTextBox;
        private System.Windows.Forms.ListBox NPCInfoListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox MapComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox MinLev_textbox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox Day_combo;
        private System.Windows.Forms.ComboBox Class_combo;
        private System.Windows.Forms.NumericUpDown EndMin_num;
        private System.Windows.Forms.ComboBox EndHour_combo;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown StartMin_num;
        private System.Windows.Forms.ComboBox StartHour_combo;
        private System.Windows.Forms.CheckBox TimeVisible_checkbox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox MaxLev_textbox;
        private System.Windows.Forms.TextBox Flag_textbox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.ComboBox ConquestHidden_combo;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox BigMapIconTextBox;
        private System.Windows.Forms.CheckBox ShowBigMapCheckBox;
        private System.Windows.Forms.CheckBox TeleportToCheckBox;
        private CheckBox ConquestVisible_checkbox;
    }
}