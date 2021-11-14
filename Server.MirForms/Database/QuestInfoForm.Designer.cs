namespace Server
{
    partial class QuestInfoForm
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
            this.percentageBox = new System.Windows.Forms.CheckBox();
            this.autoCompleteBox = new System.Windows.Forms.CheckBox();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.QuestInfoPanel = new System.Windows.Forms.Panel();
            this.label5 = new System.Windows.Forms.Label();
            this.TimeLimitTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.RequiredMaxLevelTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.QFlagTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.QItemTextBox = new System.Windows.Forms.TextBox();
            this.QKillTextBox = new System.Windows.Forms.TextBox();
            this.QGotoTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.RequiredClassComboBox = new System.Windows.Forms.ComboBox();
            this.RequiredQuestComboBox = new System.Windows.Forms.ComboBox();
            this.RequiredMinLevelTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.QTypeComboBox = new System.Windows.Forms.ComboBox();
            this.label11 = new System.Windows.Forms.Label();
            this.OpenQButton = new System.Windows.Forms.Button();
            this.QFileNameTextBox = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.QGroupTextBox = new System.Windows.Forms.TextBox();
            this.QNameTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.QuestIndexTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.RemoveButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.QuestInfoListBox = new System.Windows.Forms.ListBox();
            this.PasteMButton = new System.Windows.Forms.Button();
            this.CopyMButton = new System.Windows.Forms.Button();
            this.ExportButton = new System.Windows.Forms.Button();
            this.ImportButton = new System.Windows.Forms.Button();
            this.ExportSelectedButton = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.QuestInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // percentageBox
            // 
            this.percentageBox.AutoSize = true;
            this.percentageBox.Location = new System.Drawing.Point(404, 270);
            this.percentageBox.Margin = new System.Windows.Forms.Padding(4);
            this.percentageBox.Name = "percentageBox";
            this.percentageBox.Size = new System.Drawing.Size(177, 21);
            this.percentageBox.TabIndex = 58;
            this.percentageBox.Text = "Percentage Experience";
            this.percentageBox.UseVisualStyleBackColor = true;
            this.percentageBox.CheckedChanged += new System.EventHandler(this.percentageBox_CheckedChanged);
            // 
            // autoCompleteBox
            // 
            this.autoCompleteBox.AutoSize = true;
            this.autoCompleteBox.Location = new System.Drawing.Point(591, 271);
            this.autoCompleteBox.Margin = new System.Windows.Forms.Padding(4);
            this.autoCompleteBox.Name = "autoCompleteBox";
            this.autoCompleteBox.Size = new System.Drawing.Size(116, 21);
            this.autoCompleteBox.TabIndex = 61;
            this.autoCompleteBox.Text = "Autocomplete";
            this.autoCompleteBox.UseVisualStyleBackColor = true;
            this.autoCompleteBox.CheckedChanged += new System.EventHandler(this.autoCompleteBox_CheckedChanged);
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(17, 50);
            this.txtSearch.Margin = new System.Windows.Forms.Padding(4);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(205, 22);
            this.txtSearch.TabIndex = 26;
            this.txtSearch.TextChanged += new System.EventHandler(this.txtSearch_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Location = new System.Drawing.Point(232, 50);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(4);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(803, 353);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.QuestInfoPanel);
            this.tabPage1.Location = new System.Drawing.Point(4, 25);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4);
            this.tabPage1.Size = new System.Drawing.Size(795, 324);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Info";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // QuestInfoPanel
            // 
            this.QuestInfoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.QuestInfoPanel.Controls.Add(this.label5);
            this.QuestInfoPanel.Controls.Add(this.TimeLimitTextBox);
            this.QuestInfoPanel.Controls.Add(this.percentageBox);
            this.QuestInfoPanel.Controls.Add(this.autoCompleteBox);
            this.QuestInfoPanel.Controls.Add(this.label4);
            this.QuestInfoPanel.Controls.Add(this.RequiredMaxLevelTextBox);
            this.QuestInfoPanel.Controls.Add(this.label3);
            this.QuestInfoPanel.Controls.Add(this.QFlagTextBox);
            this.QuestInfoPanel.Controls.Add(this.label14);
            this.QuestInfoPanel.Controls.Add(this.label12);
            this.QuestInfoPanel.Controls.Add(this.label10);
            this.QuestInfoPanel.Controls.Add(this.QItemTextBox);
            this.QuestInfoPanel.Controls.Add(this.QKillTextBox);
            this.QuestInfoPanel.Controls.Add(this.QGotoTextBox);
            this.QuestInfoPanel.Controls.Add(this.label9);
            this.QuestInfoPanel.Controls.Add(this.label8);
            this.QuestInfoPanel.Controls.Add(this.label7);
            this.QuestInfoPanel.Controls.Add(this.RequiredClassComboBox);
            this.QuestInfoPanel.Controls.Add(this.RequiredQuestComboBox);
            this.QuestInfoPanel.Controls.Add(this.RequiredMinLevelTextBox);
            this.QuestInfoPanel.Controls.Add(this.label2);
            this.QuestInfoPanel.Controls.Add(this.QTypeComboBox);
            this.QuestInfoPanel.Controls.Add(this.label11);
            this.QuestInfoPanel.Controls.Add(this.OpenQButton);
            this.QuestInfoPanel.Controls.Add(this.QFileNameTextBox);
            this.QuestInfoPanel.Controls.Add(this.label29);
            this.QuestInfoPanel.Controls.Add(this.QGroupTextBox);
            this.QuestInfoPanel.Controls.Add(this.QNameTextBox);
            this.QuestInfoPanel.Controls.Add(this.label13);
            this.QuestInfoPanel.Controls.Add(this.QuestIndexTextBox);
            this.QuestInfoPanel.Controls.Add(this.label1);
            this.QuestInfoPanel.Enabled = false;
            this.QuestInfoPanel.Location = new System.Drawing.Point(4, 7);
            this.QuestInfoPanel.Margin = new System.Windows.Forms.Padding(4);
            this.QuestInfoPanel.Name = "QuestInfoPanel";
            this.QuestInfoPanel.Size = new System.Drawing.Size(780, 306);
            this.QuestInfoPanel.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(457, 237);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(145, 17);
            this.label5.TabIndex = 59;
            this.label5.Text = "Time Limit (Seconds):";
            // 
            // TimeLimitTextBox
            // 
            this.TimeLimitTextBox.Location = new System.Drawing.Point(608, 233);
            this.TimeLimitTextBox.Name = "TimeLimitTextBox";
            this.TimeLimitTextBox.Size = new System.Drawing.Size(121, 22);
            this.TimeLimitTextBox.TabIndex = 58;
            this.TimeLimitTextBox.TextChanged += new System.EventHandler(this.TimeLimitTextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(465, 41);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(137, 17);
            this.label4.TabIndex = 57;
            this.label4.Text = "Required Max Level:";
            // 
            // RequiredMaxLevelTextBox
            // 
            this.RequiredMaxLevelTextBox.Location = new System.Drawing.Point(608, 34);
            this.RequiredMaxLevelTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.RequiredMaxLevelTextBox.MaxLength = 3;
            this.RequiredMaxLevelTextBox.Name = "RequiredMaxLevelTextBox";
            this.RequiredMaxLevelTextBox.Size = new System.Drawing.Size(160, 22);
            this.RequiredMaxLevelTextBox.TabIndex = 56;
            this.RequiredMaxLevelTextBox.TextChanged += new System.EventHandler(this.RequiredMaxLevelTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(36, 271);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 17);
            this.label3.TabIndex = 55;
            this.label3.Text = "Flag Text:";
            // 
            // QFlagTextBox
            // 
            this.QFlagTextBox.Location = new System.Drawing.Point(116, 267);
            this.QFlagTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.QFlagTextBox.Name = "QFlagTextBox";
            this.QFlagTextBox.Size = new System.Drawing.Size(239, 22);
            this.QFlagTextBox.TabIndex = 54;
            this.QFlagTextBox.TextChanged += new System.EventHandler(this.QFlagTextBox_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(36, 238);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(69, 17);
            this.label14.TabIndex = 53;
            this.label14.Text = "Item Text:";
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(45, 204);
            this.label12.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(61, 17);
            this.label12.TabIndex = 52;
            this.label12.Text = "Kill Text:";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(32, 171);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(74, 17);
            this.label10.TabIndex = 51;
            this.label10.Text = "Goto Text:";
            // 
            // QItemTextBox
            // 
            this.QItemTextBox.Location = new System.Drawing.Point(116, 234);
            this.QItemTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.QItemTextBox.Name = "QItemTextBox";
            this.QItemTextBox.Size = new System.Drawing.Size(239, 22);
            this.QItemTextBox.TabIndex = 49;
            this.QItemTextBox.TextChanged += new System.EventHandler(this.QItemTextBox_TextChanged);
            // 
            // QKillTextBox
            // 
            this.QKillTextBox.Location = new System.Drawing.Point(116, 201);
            this.QKillTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.QKillTextBox.Name = "QKillTextBox";
            this.QKillTextBox.Size = new System.Drawing.Size(239, 22);
            this.QKillTextBox.TabIndex = 48;
            this.QKillTextBox.TextChanged += new System.EventHandler(this.QKillTextBox_TextChanged);
            // 
            // QGotoTextBox
            // 
            this.QGotoTextBox.Location = new System.Drawing.Point(116, 167);
            this.QGotoTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.QGotoTextBox.Name = "QGotoTextBox";
            this.QGotoTextBox.Size = new System.Drawing.Size(239, 22);
            this.QGotoTextBox.TabIndex = 47;
            this.QGotoTextBox.TextChanged += new System.EventHandler(this.QGotoTextBox_TextChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(497, 105);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(108, 17);
            this.label9.TabIndex = 46;
            this.label9.Text = "Required Class:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(493, 73);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(112, 17);
            this.label8.TabIndex = 45;
            this.label8.Text = "Required Quest:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(469, 9);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(134, 17);
            this.label7.TabIndex = 44;
            this.label7.Text = "Required Min Level:";
            // 
            // RequiredClassComboBox
            // 
            this.RequiredClassComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RequiredClassComboBox.FormattingEnabled = true;
            this.RequiredClassComboBox.Location = new System.Drawing.Point(608, 101);
            this.RequiredClassComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.RequiredClassComboBox.Name = "RequiredClassComboBox";
            this.RequiredClassComboBox.Size = new System.Drawing.Size(160, 24);
            this.RequiredClassComboBox.TabIndex = 43;
            this.RequiredClassComboBox.SelectedIndexChanged += new System.EventHandler(this.RequiredClassComboBox_SelectedIndexChanged);
            // 
            // RequiredQuestComboBox
            // 
            this.RequiredQuestComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.RequiredQuestComboBox.FormattingEnabled = true;
            this.RequiredQuestComboBox.Location = new System.Drawing.Point(608, 66);
            this.RequiredQuestComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.RequiredQuestComboBox.Name = "RequiredQuestComboBox";
            this.RequiredQuestComboBox.Size = new System.Drawing.Size(160, 24);
            this.RequiredQuestComboBox.TabIndex = 42;
            this.RequiredQuestComboBox.SelectedIndexChanged += new System.EventHandler(this.RequiredQuestComboBox_SelectedIndexChanged);
            // 
            // RequiredMinLevelTextBox
            // 
            this.RequiredMinLevelTextBox.Location = new System.Drawing.Point(608, 5);
            this.RequiredMinLevelTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.RequiredMinLevelTextBox.MaxLength = 3;
            this.RequiredMinLevelTextBox.Name = "RequiredMinLevelTextBox";
            this.RequiredMinLevelTextBox.Size = new System.Drawing.Size(160, 22);
            this.RequiredMinLevelTextBox.TabIndex = 41;
            this.RequiredMinLevelTextBox.TextChanged += new System.EventHandler(this.RequiredMinLevelTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(63, 105);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(44, 17);
            this.label2.TabIndex = 32;
            this.label2.Text = "Type:";
            // 
            // QTypeComboBox
            // 
            this.QTypeComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.QTypeComboBox.FormattingEnabled = true;
            this.QTypeComboBox.Location = new System.Drawing.Point(116, 101);
            this.QTypeComboBox.Margin = new System.Windows.Forms.Padding(4);
            this.QTypeComboBox.Name = "QTypeComboBox";
            this.QTypeComboBox.Size = new System.Drawing.Size(239, 24);
            this.QTypeComboBox.TabIndex = 31;
            this.QTypeComboBox.SelectedIndexChanged += new System.EventHandler(this.QTypeComboBox_SelectedIndexChanged);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(32, 138);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(75, 17);
            this.label11.TabIndex = 23;
            this.label11.Text = "File Name:";
            // 
            // OpenQButton
            // 
            this.OpenQButton.Location = new System.Drawing.Point(369, 132);
            this.OpenQButton.Margin = new System.Windows.Forms.Padding(4);
            this.OpenQButton.Name = "OpenQButton";
            this.OpenQButton.Size = new System.Drawing.Size(100, 28);
            this.OpenQButton.TabIndex = 30;
            this.OpenQButton.Text = "Open Script";
            this.OpenQButton.UseVisualStyleBackColor = true;
            this.OpenQButton.Click += new System.EventHandler(this.OpenQButton_Click);
            // 
            // QFileNameTextBox
            // 
            this.QFileNameTextBox.Location = new System.Drawing.Point(116, 134);
            this.QFileNameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.QFileNameTextBox.Name = "QFileNameTextBox";
            this.QFileNameTextBox.Size = new System.Drawing.Size(239, 22);
            this.QFileNameTextBox.TabIndex = 22;
            this.QFileNameTextBox.TextChanged += new System.EventHandler(this.QFileNameTextBox_TextChanged);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Cursor = System.Windows.Forms.Cursors.Default;
            this.label29.Location = new System.Drawing.Point(56, 73);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(52, 17);
            this.label29.TabIndex = 21;
            this.label29.Text = "Group:";
            // 
            // QGroupTextBox
            // 
            this.QGroupTextBox.Location = new System.Drawing.Point(116, 69);
            this.QGroupTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.QGroupTextBox.MaxLength = 20;
            this.QGroupTextBox.Name = "QGroupTextBox";
            this.QGroupTextBox.Size = new System.Drawing.Size(239, 22);
            this.QGroupTextBox.TabIndex = 20;
            this.QGroupTextBox.TextChanged += new System.EventHandler(this.QGroupTextBox_TextChanged);
            // 
            // QNameTextBox
            // 
            this.QNameTextBox.Location = new System.Drawing.Point(116, 37);
            this.QNameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.QNameTextBox.MaxLength = 30;
            this.QNameTextBox.Name = "QNameTextBox";
            this.QNameTextBox.Size = new System.Drawing.Size(239, 22);
            this.QNameTextBox.TabIndex = 14;
            this.QNameTextBox.TextChanged += new System.EventHandler(this.QNameTextBox_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(57, 41);
            this.label13.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(49, 17);
            this.label13.TabIndex = 15;
            this.label13.Text = "Name:";
            // 
            // QuestIndexTextBox
            // 
            this.QuestIndexTextBox.Location = new System.Drawing.Point(116, 5);
            this.QuestIndexTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.QuestIndexTextBox.Name = "QuestIndexTextBox";
            this.QuestIndexTextBox.ReadOnly = true;
            this.QuestIndexTextBox.Size = new System.Drawing.Size(61, 22);
            this.QuestIndexTextBox.TabIndex = 0;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(19, 9);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(87, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "Quest Index:";
            // 
            // RemoveButton
            // 
            this.RemoveButton.Location = new System.Drawing.Point(124, 15);
            this.RemoveButton.Margin = new System.Windows.Forms.Padding(4);
            this.RemoveButton.Name = "RemoveButton";
            this.RemoveButton.Size = new System.Drawing.Size(100, 28);
            this.RemoveButton.TabIndex = 14;
            this.RemoveButton.Text = "Remove";
            this.RemoveButton.UseVisualStyleBackColor = true;
            this.RemoveButton.Click += new System.EventHandler(this.RemoveButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.Location = new System.Drawing.Point(16, 15);
            this.AddButton.Margin = new System.Windows.Forms.Padding(4);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(100, 28);
            this.AddButton.TabIndex = 13;
            this.AddButton.Text = "Add";
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // QuestInfoListBox
            // 
            this.QuestInfoListBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.QuestInfoListBox.FormattingEnabled = true;
            this.QuestInfoListBox.ItemHeight = 16;
            this.QuestInfoListBox.Location = new System.Drawing.Point(16, 82);
            this.QuestInfoListBox.Margin = new System.Windows.Forms.Padding(4);
            this.QuestInfoListBox.Name = "QuestInfoListBox";
            this.QuestInfoListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.QuestInfoListBox.Size = new System.Drawing.Size(207, 324);
            this.QuestInfoListBox.TabIndex = 15;
            this.QuestInfoListBox.SelectedIndexChanged += new System.EventHandler(this.QuestInfoListBox_SelectedIndexChanged);
            // 
            // PasteMButton
            // 
            this.PasteMButton.Location = new System.Drawing.Point(340, 15);
            this.PasteMButton.Margin = new System.Windows.Forms.Padding(4);
            this.PasteMButton.Name = "PasteMButton";
            this.PasteMButton.Size = new System.Drawing.Size(100, 28);
            this.PasteMButton.TabIndex = 22;
            this.PasteMButton.Text = "Paste";
            this.PasteMButton.UseVisualStyleBackColor = true;
            this.PasteMButton.Click += new System.EventHandler(this.PasteMButton_Click);
            // 
            // CopyMButton
            // 
            this.CopyMButton.Location = new System.Drawing.Point(232, 15);
            this.CopyMButton.Margin = new System.Windows.Forms.Padding(4);
            this.CopyMButton.Name = "CopyMButton";
            this.CopyMButton.Size = new System.Drawing.Size(100, 28);
            this.CopyMButton.TabIndex = 21;
            this.CopyMButton.Text = "Copy";
            this.CopyMButton.UseVisualStyleBackColor = true;
            // 
            // ExportButton
            // 
            this.ExportButton.Location = new System.Drawing.Point(935, 15);
            this.ExportButton.Margin = new System.Windows.Forms.Padding(4);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(100, 28);
            this.ExportButton.TabIndex = 23;
            this.ExportButton.Text = "Export All";
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportAllButton_Click);
            // 
            // ImportButton
            // 
            this.ImportButton.Location = new System.Drawing.Point(664, 15);
            this.ImportButton.Margin = new System.Windows.Forms.Padding(4);
            this.ImportButton.Name = "ImportButton";
            this.ImportButton.Size = new System.Drawing.Size(100, 28);
            this.ImportButton.TabIndex = 24;
            this.ImportButton.Text = "Import";
            this.ImportButton.UseVisualStyleBackColor = true;
            this.ImportButton.Click += new System.EventHandler(this.ImportButton_Click);
            // 
            // ExportSelectedButton
            // 
            this.ExportSelectedButton.Location = new System.Drawing.Point(771, 15);
            this.ExportSelectedButton.Margin = new System.Windows.Forms.Padding(4);
            this.ExportSelectedButton.Name = "ExportSelectedButton";
            this.ExportSelectedButton.Size = new System.Drawing.Size(156, 28);
            this.ExportSelectedButton.TabIndex = 25;
            this.ExportSelectedButton.Text = "Export Selected";
            this.ExportSelectedButton.UseVisualStyleBackColor = true;
            this.ExportSelectedButton.Click += new System.EventHandler(this.ExportSelected_Click);
            // 
            // QuestInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1051, 410);
            this.Controls.Add(this.txtSearch);
            this.Controls.Add(this.ExportSelectedButton);
            this.Controls.Add(this.ImportButton);
            this.Controls.Add(this.ExportButton);
            this.Controls.Add(this.PasteMButton);
            this.Controls.Add(this.CopyMButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.RemoveButton);
            this.Controls.Add(this.AddButton);
            this.Controls.Add(this.QuestInfoListBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "QuestInfoForm";
            this.Text = "QuestInfoForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.QuestInfoForm_FormClosed);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.QuestInfoPanel.ResumeLayout(false);
            this.QuestInfoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.CheckBox percentageBox;
        private System.Windows.Forms.CheckBox autoCompleteBox;

        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel QuestInfoPanel;
        private System.Windows.Forms.TextBox QuestIndexTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button PasteMButton;
        private System.Windows.Forms.Button CopyMButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button ExportSelectedButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button OpenQButton;
        private System.Windows.Forms.TextBox QFileNameTextBox;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox QGroupTextBox;
        private System.Windows.Forms.TextBox QNameTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListBox QuestInfoListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox QTypeComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox RequiredClassComboBox;
        private System.Windows.Forms.ComboBox RequiredQuestComboBox;
        private System.Windows.Forms.TextBox RequiredMinLevelTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox QItemTextBox;
        private System.Windows.Forms.TextBox QKillTextBox;
        private System.Windows.Forms.TextBox QGotoTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox QFlagTextBox;
        private System.Windows.Forms.TextBox RequiredMaxLevelTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TimeLimitTextBox;
    }
}