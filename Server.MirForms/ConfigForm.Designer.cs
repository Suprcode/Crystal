namespace Server
{
    partial class ConfigForm
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
            this.SaveButton = new System.Windows.Forms.Button();
            this.configTabs = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.DBVersionLabel = new System.Windows.Forms.Label();
            this.ServerVersionLabel = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.RelogDelayTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.VersionCheckBox = new System.Windows.Forms.CheckBox();
            this.VPathBrowseButton = new System.Windows.Forms.Button();
            this.VPathTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.MaxUserTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.TimeOutTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PortTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.IPAddressTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.tabPage3 = new System.Windows.Forms.TabPage();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.Resolution_textbox = new System.Windows.Forms.TextBox();
            this.AllowArcherCheckBox = new System.Windows.Forms.CheckBox();
            this.AllowAssassinCheckBox = new System.Windows.Forms.CheckBox();
            this.StartGameCheckBox = new System.Windows.Forms.CheckBox();
            this.DCharacterCheckBox = new System.Windows.Forms.CheckBox();
            this.NCharacterCheckBox = new System.Windows.Forms.CheckBox();
            this.LoginCheckBox = new System.Windows.Forms.CheckBox();
            this.PasswordCheckBox = new System.Windows.Forms.CheckBox();
            this.AccountCheckBox = new System.Windows.Forms.CheckBox();
            this.tabPage4 = new System.Windows.Forms.TabPage();
            this.SaveDelayTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.tabPage5 = new System.Windows.Forms.TabPage();
            this.SafeZoneHealingCheckBox = new System.Windows.Forms.CheckBox();
            this.SafeZoneBorderCheckBox = new System.Windows.Forms.CheckBox();
            this.VPathDialog = new System.Windows.Forms.OpenFileDialog();
            this.UseSQLServerCheckbox = new System.Windows.Forms.CheckBox();
            this.configTabs.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.tabPage3.SuspendLayout();
            this.tabPage4.SuspendLayout();
            this.tabPage5.SuspendLayout();
            this.SuspendLayout();
            // 
            // SaveButton
            // 
            this.SaveButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.SaveButton.Location = new System.Drawing.Point(528, 529);
            this.SaveButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(112, 35);
            this.SaveButton.TabIndex = 6;
            this.SaveButton.Text = "Close";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // configTabs
            // 
            this.configTabs.Controls.Add(this.tabPage1);
            this.configTabs.Controls.Add(this.tabPage2);
            this.configTabs.Controls.Add(this.tabPage3);
            this.configTabs.Controls.Add(this.tabPage4);
            this.configTabs.Controls.Add(this.tabPage5);
            this.configTabs.Location = new System.Drawing.Point(18, 18);
            this.configTabs.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.configTabs.Name = "configTabs";
            this.configTabs.SelectedIndex = 0;
            this.configTabs.Size = new System.Drawing.Size(622, 502);
            this.configTabs.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.RelogDelayTextBox);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Controls.Add(this.VersionCheckBox);
            this.tabPage1.Controls.Add(this.VPathBrowseButton);
            this.tabPage1.Controls.Add(this.VPathTextBox);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage1.Size = new System.Drawing.Size(614, 469);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "General";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label11);
            this.groupBox1.Controls.Add(this.DBVersionLabel);
            this.groupBox1.Controls.Add(this.ServerVersionLabel);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Location = new System.Drawing.Point(9, 354);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.groupBox1.Size = new System.Drawing.Size(592, 98);
            this.groupBox1.TabIndex = 25;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Version Info";
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(9, 65);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 20);
            this.label11.TabIndex = 23;
            this.label11.Text = "Database";
            // 
            // DBVersionLabel
            // 
            this.DBVersionLabel.AutoSize = true;
            this.DBVersionLabel.Location = new System.Drawing.Point(114, 65);
            this.DBVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.DBVersionLabel.Name = "DBVersionLabel";
            this.DBVersionLabel.Size = new System.Drawing.Size(63, 20);
            this.DBVersionLabel.TabIndex = 24;
            this.DBVersionLabel.Text = "Version";
            // 
            // ServerVersionLabel
            // 
            this.ServerVersionLabel.AutoSize = true;
            this.ServerVersionLabel.Location = new System.Drawing.Point(114, 29);
            this.ServerVersionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ServerVersionLabel.Name = "ServerVersionLabel";
            this.ServerVersionLabel.Size = new System.Drawing.Size(63, 20);
            this.ServerVersionLabel.TabIndex = 7;
            this.ServerVersionLabel.Text = "Version";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(9, 29);
            this.label10.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(55, 20);
            this.label10.TabIndex = 22;
            this.label10.Text = "Server";
            // 
            // RelogDelayTextBox
            // 
            this.RelogDelayTextBox.Location = new System.Drawing.Point(134, 100);
            this.RelogDelayTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.RelogDelayTextBox.MaxLength = 5;
            this.RelogDelayTextBox.Name = "RelogDelayTextBox";
            this.RelogDelayTextBox.Size = new System.Drawing.Size(138, 26);
            this.RelogDelayTextBox.TabIndex = 21;
            this.RelogDelayTextBox.TextChanged += new System.EventHandler(this.CheckUShort);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(22, 105);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(99, 20);
            this.label7.TabIndex = 20;
            this.label7.Text = "Relog Delay:";
            // 
            // VersionCheckBox
            // 
            this.VersionCheckBox.AutoSize = true;
            this.VersionCheckBox.Location = new System.Drawing.Point(134, 65);
            this.VersionCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.VersionCheckBox.Name = "VersionCheckBox";
            this.VersionCheckBox.Size = new System.Drawing.Size(138, 24);
            this.VersionCheckBox.TabIndex = 3;
            this.VersionCheckBox.Text = "Check Version";
            this.VersionCheckBox.UseVisualStyleBackColor = true;
            // 
            // VPathBrowseButton
            // 
            this.VPathBrowseButton.Location = new System.Drawing.Point(560, 22);
            this.VPathBrowseButton.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.VPathBrowseButton.Name = "VPathBrowseButton";
            this.VPathBrowseButton.Size = new System.Drawing.Size(42, 35);
            this.VPathBrowseButton.TabIndex = 2;
            this.VPathBrowseButton.Text = "...";
            this.VPathBrowseButton.UseVisualStyleBackColor = true;
            this.VPathBrowseButton.Click += new System.EventHandler(this.VPathBrowseButton_Click);
            // 
            // VPathTextBox
            // 
            this.VPathTextBox.Location = new System.Drawing.Point(134, 25);
            this.VPathTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.VPathTextBox.Name = "VPathTextBox";
            this.VPathTextBox.ReadOnly = true;
            this.VPathTextBox.Size = new System.Drawing.Size(415, 26);
            this.VPathTextBox.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(20, 29);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(104, 20);
            this.label1.TabIndex = 0;
            this.label1.Text = "Version Path:";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.MaxUserTextBox);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.TimeOutTextBox);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.PortTextBox);
            this.tabPage2.Controls.Add(this.label3);
            this.tabPage2.Controls.Add(this.IPAddressTextBox);
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage2.Size = new System.Drawing.Size(614, 469);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Network";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // MaxUserTextBox
            // 
            this.MaxUserTextBox.Location = new System.Drawing.Point(134, 145);
            this.MaxUserTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.MaxUserTextBox.MaxLength = 5;
            this.MaxUserTextBox.Name = "MaxUserTextBox";
            this.MaxUserTextBox.Size = new System.Drawing.Size(61, 26);
            this.MaxUserTextBox.TabIndex = 17;
            this.MaxUserTextBox.TextChanged += new System.EventHandler(this.CheckUShort);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(42, 149);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(80, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "Max User:";
            // 
            // TimeOutTextBox
            // 
            this.TimeOutTextBox.Location = new System.Drawing.Point(134, 105);
            this.TimeOutTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.TimeOutTextBox.MaxLength = 5;
            this.TimeOutTextBox.Name = "TimeOutTextBox";
            this.TimeOutTextBox.Size = new System.Drawing.Size(138, 26);
            this.TimeOutTextBox.TabIndex = 15;
            this.TimeOutTextBox.TextChanged += new System.EventHandler(this.CheckUShort);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 109);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(73, 20);
            this.label4.TabIndex = 14;
            this.label4.Text = "TimeOut:";
            // 
            // PortTextBox
            // 
            this.PortTextBox.Location = new System.Drawing.Point(134, 65);
            this.PortTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PortTextBox.MaxLength = 5;
            this.PortTextBox.Name = "PortTextBox";
            this.PortTextBox.Size = new System.Drawing.Size(61, 26);
            this.PortTextBox.TabIndex = 13;
            this.PortTextBox.TextChanged += new System.EventHandler(this.CheckUShort);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(81, 69);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 20);
            this.label3.TabIndex = 12;
            this.label3.Text = "Port:";
            // 
            // IPAddressTextBox
            // 
            this.IPAddressTextBox.Location = new System.Drawing.Point(134, 25);
            this.IPAddressTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.IPAddressTextBox.MaxLength = 15;
            this.IPAddressTextBox.Name = "IPAddressTextBox";
            this.IPAddressTextBox.Size = new System.Drawing.Size(138, 26);
            this.IPAddressTextBox.TabIndex = 11;
            this.IPAddressTextBox.TextChanged += new System.EventHandler(this.IPAddressCheck);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(33, 29);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(91, 20);
            this.label2.TabIndex = 10;
            this.label2.Text = "IP Address:";
            // 
            // tabPage3
            // 
            this.tabPage3.Controls.Add(this.label9);
            this.tabPage3.Controls.Add(this.label8);
            this.tabPage3.Controls.Add(this.Resolution_textbox);
            this.tabPage3.Controls.Add(this.AllowArcherCheckBox);
            this.tabPage3.Controls.Add(this.AllowAssassinCheckBox);
            this.tabPage3.Controls.Add(this.StartGameCheckBox);
            this.tabPage3.Controls.Add(this.DCharacterCheckBox);
            this.tabPage3.Controls.Add(this.NCharacterCheckBox);
            this.tabPage3.Controls.Add(this.LoginCheckBox);
            this.tabPage3.Controls.Add(this.PasswordCheckBox);
            this.tabPage3.Controls.Add(this.AccountCheckBox);
            this.tabPage3.Location = new System.Drawing.Point(4, 29);
            this.tabPage3.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Name = "tabPage3";
            this.tabPage3.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage3.Size = new System.Drawing.Size(614, 469);
            this.tabPage3.TabIndex = 2;
            this.tabPage3.Text = "Permissions";
            this.tabPage3.UseVisualStyleBackColor = true;
            this.tabPage3.Click += new System.EventHandler(this.tabPage3_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(32, 358);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(177, 20);
            this.label9.TabIndex = 16;
            this.label9.Text = "Max Resolution Allowed";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(0, 0);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(51, 20);
            this.label8.TabIndex = 15;
            this.label8.Text = "label8";
            // 
            // Resolution_textbox
            // 
            this.Resolution_textbox.Location = new System.Drawing.Point(220, 354);
            this.Resolution_textbox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Resolution_textbox.Name = "Resolution_textbox";
            this.Resolution_textbox.Size = new System.Drawing.Size(118, 26);
            this.Resolution_textbox.TabIndex = 14;
            this.Resolution_textbox.TextChanged += new System.EventHandler(this.Resolution_textbox_TextChanged);
            // 
            // AllowArcherCheckBox
            // 
            this.AllowArcherCheckBox.AutoSize = true;
            this.AllowArcherCheckBox.Location = new System.Drawing.Point(36, 303);
            this.AllowArcherCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AllowArcherCheckBox.Name = "AllowArcherCheckBox";
            this.AllowArcherCheckBox.Size = new System.Drawing.Size(175, 24);
            this.AllowArcherCheckBox.TabIndex = 13;
            this.AllowArcherCheckBox.Text = "Allow Create Archer";
            this.AllowArcherCheckBox.UseVisualStyleBackColor = true;
            // 
            // AllowAssassinCheckBox
            // 
            this.AllowAssassinCheckBox.AutoSize = true;
            this.AllowAssassinCheckBox.Location = new System.Drawing.Point(36, 266);
            this.AllowAssassinCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AllowAssassinCheckBox.Name = "AllowAssassinCheckBox";
            this.AllowAssassinCheckBox.Size = new System.Drawing.Size(192, 24);
            this.AllowAssassinCheckBox.TabIndex = 12;
            this.AllowAssassinCheckBox.Text = "Allow Create Assassin";
            this.AllowAssassinCheckBox.UseVisualStyleBackColor = true;
            // 
            // StartGameCheckBox
            // 
            this.StartGameCheckBox.AutoSize = true;
            this.StartGameCheckBox.Location = new System.Drawing.Point(36, 208);
            this.StartGameCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.StartGameCheckBox.Name = "StartGameCheckBox";
            this.StartGameCheckBox.Size = new System.Drawing.Size(159, 24);
            this.StartGameCheckBox.TabIndex = 11;
            this.StartGameCheckBox.Text = "Allow Start Game";
            this.StartGameCheckBox.UseVisualStyleBackColor = true;
            // 
            // DCharacterCheckBox
            // 
            this.DCharacterCheckBox.AutoSize = true;
            this.DCharacterCheckBox.Location = new System.Drawing.Point(36, 172);
            this.DCharacterCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.DCharacterCheckBox.Name = "DCharacterCheckBox";
            this.DCharacterCheckBox.Size = new System.Drawing.Size(197, 24);
            this.DCharacterCheckBox.TabIndex = 10;
            this.DCharacterCheckBox.Text = "Allow Delete Character";
            this.DCharacterCheckBox.UseVisualStyleBackColor = true;
            // 
            // NCharacterCheckBox
            // 
            this.NCharacterCheckBox.AutoSize = true;
            this.NCharacterCheckBox.Location = new System.Drawing.Point(36, 137);
            this.NCharacterCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.NCharacterCheckBox.Name = "NCharacterCheckBox";
            this.NCharacterCheckBox.Size = new System.Drawing.Size(181, 24);
            this.NCharacterCheckBox.TabIndex = 9;
            this.NCharacterCheckBox.Text = "Allow New Character";
            this.NCharacterCheckBox.UseVisualStyleBackColor = true;
            // 
            // LoginCheckBox
            // 
            this.LoginCheckBox.AutoSize = true;
            this.LoginCheckBox.Location = new System.Drawing.Point(36, 102);
            this.LoginCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.LoginCheckBox.Name = "LoginCheckBox";
            this.LoginCheckBox.Size = new System.Drawing.Size(115, 24);
            this.LoginCheckBox.TabIndex = 8;
            this.LoginCheckBox.Text = "Allow Login";
            this.LoginCheckBox.UseVisualStyleBackColor = true;
            // 
            // PasswordCheckBox
            // 
            this.PasswordCheckBox.AutoSize = true;
            this.PasswordCheckBox.Location = new System.Drawing.Point(36, 66);
            this.PasswordCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.PasswordCheckBox.Name = "PasswordCheckBox";
            this.PasswordCheckBox.Size = new System.Drawing.Size(205, 24);
            this.PasswordCheckBox.TabIndex = 7;
            this.PasswordCheckBox.Text = "Allow Change Password";
            this.PasswordCheckBox.UseVisualStyleBackColor = true;
            // 
            // AccountCheckBox
            // 
            this.AccountCheckBox.AutoSize = true;
            this.AccountCheckBox.Location = new System.Drawing.Point(36, 31);
            this.AccountCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.AccountCheckBox.Name = "AccountCheckBox";
            this.AccountCheckBox.Size = new System.Drawing.Size(170, 24);
            this.AccountCheckBox.TabIndex = 6;
            this.AccountCheckBox.Text = "Allow New Account";
            this.AccountCheckBox.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            this.tabPage4.Controls.Add(this.UseSQLServerCheckbox);
            this.tabPage4.Controls.Add(this.SaveDelayTextBox);
            this.tabPage4.Controls.Add(this.label6);
            this.tabPage4.Location = new System.Drawing.Point(4, 29);
            this.tabPage4.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage4.Name = "tabPage4";
            this.tabPage4.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage4.Size = new System.Drawing.Size(614, 469);
            this.tabPage4.TabIndex = 3;
            this.tabPage4.Text = "Database";
            this.tabPage4.UseVisualStyleBackColor = true;
            // 
            // SaveDelayTextBox
            // 
            this.SaveDelayTextBox.Location = new System.Drawing.Point(134, 25);
            this.SaveDelayTextBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SaveDelayTextBox.MaxLength = 5;
            this.SaveDelayTextBox.Name = "SaveDelayTextBox";
            this.SaveDelayTextBox.Size = new System.Drawing.Size(138, 26);
            this.SaveDelayTextBox.TabIndex = 25;
            this.SaveDelayTextBox.TextChanged += new System.EventHandler(this.CheckUShort);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(27, 29);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(93, 20);
            this.label6.TabIndex = 24;
            this.label6.Text = "Save Delay:";
            // 
            // tabPage5
            // 
            this.tabPage5.Controls.Add(this.SafeZoneHealingCheckBox);
            this.tabPage5.Controls.Add(this.SafeZoneBorderCheckBox);
            this.tabPage5.Location = new System.Drawing.Point(4, 29);
            this.tabPage5.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage5.Name = "tabPage5";
            this.tabPage5.Padding = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.tabPage5.Size = new System.Drawing.Size(614, 469);
            this.tabPage5.TabIndex = 4;
            this.tabPage5.Text = "Optional";
            this.tabPage5.UseVisualStyleBackColor = true;
            // 
            // SafeZoneHealingCheckBox
            // 
            this.SafeZoneHealingCheckBox.AutoSize = true;
            this.SafeZoneHealingCheckBox.Location = new System.Drawing.Point(36, 66);
            this.SafeZoneHealingCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SafeZoneHealingCheckBox.Name = "SafeZoneHealingCheckBox";
            this.SafeZoneHealingCheckBox.Size = new System.Drawing.Size(164, 24);
            this.SafeZoneHealingCheckBox.TabIndex = 1;
            this.SafeZoneHealingCheckBox.Text = "SafeZone Healing";
            this.SafeZoneHealingCheckBox.UseVisualStyleBackColor = true;
            this.SafeZoneHealingCheckBox.CheckedChanged += new System.EventHandler(this.SafeZoneHealingCheckBox_CheckedChanged);
            // 
            // SafeZoneBorderCheckBox
            // 
            this.SafeZoneBorderCheckBox.AutoSize = true;
            this.SafeZoneBorderCheckBox.Location = new System.Drawing.Point(36, 31);
            this.SafeZoneBorderCheckBox.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.SafeZoneBorderCheckBox.Name = "SafeZoneBorderCheckBox";
            this.SafeZoneBorderCheckBox.Size = new System.Drawing.Size(158, 24);
            this.SafeZoneBorderCheckBox.TabIndex = 0;
            this.SafeZoneBorderCheckBox.Text = "SafeZone Border";
            this.SafeZoneBorderCheckBox.UseVisualStyleBackColor = true;
            this.SafeZoneBorderCheckBox.CheckedChanged += new System.EventHandler(this.SafeZoneBorderCheckBox_CheckedChanged);
            // 
            // VPathDialog
            // 
            this.VPathDialog.FileName = "Mir2.Exe";
            this.VPathDialog.Filter = "Executable Files (*.exe)|*.exe";
            // 
            // UseSQLServerCheckbox
            // 
            this.UseSQLServerCheckbox.AutoSize = true;
            this.UseSQLServerCheckbox.Location = new System.Drawing.Point(31, 59);
            this.UseSQLServerCheckbox.Name = "UseSQLServerCheckbox";
            this.UseSQLServerCheckbox.Size = new System.Drawing.Size(142, 24);
            this.UseSQLServerCheckbox.TabIndex = 26;
            this.UseSQLServerCheckbox.Text = "UseSQLServer";
            this.UseSQLServerCheckbox.UseVisualStyleBackColor = true;
            // 
            // ConfigForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(658, 574);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.configTabs);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "ConfigForm";
            this.Text = "ConfigForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ConfigForm_FormClosed);
            this.configTabs.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tabPage3.ResumeLayout(false);
            this.tabPage3.PerformLayout();
            this.tabPage4.ResumeLayout(false);
            this.tabPage4.PerformLayout();
            this.tabPage5.ResumeLayout(false);
            this.tabPage5.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button SaveButton;
        private System.Windows.Forms.TabControl configTabs;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TextBox RelogDelayTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.CheckBox VersionCheckBox;
        private System.Windows.Forms.Button VPathBrowseButton;
        private System.Windows.Forms.TextBox VPathTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.TextBox MaxUserTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TimeOutTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PortTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox IPAddressTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog VPathDialog;
        private System.Windows.Forms.TabPage tabPage3;
        private System.Windows.Forms.CheckBox StartGameCheckBox;
        private System.Windows.Forms.CheckBox DCharacterCheckBox;
        private System.Windows.Forms.CheckBox NCharacterCheckBox;
        private System.Windows.Forms.CheckBox LoginCheckBox;
        private System.Windows.Forms.CheckBox PasswordCheckBox;
        private System.Windows.Forms.CheckBox AccountCheckBox;
        private System.Windows.Forms.TabPage tabPage4;
        private System.Windows.Forms.TextBox SaveDelayTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TabPage tabPage5;
        private System.Windows.Forms.CheckBox SafeZoneBorderCheckBox;
        private System.Windows.Forms.CheckBox SafeZoneHealingCheckBox;
        private System.Windows.Forms.CheckBox AllowArcherCheckBox;
        private System.Windows.Forms.CheckBox AllowAssassinCheckBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox Resolution_textbox;
        private System.Windows.Forms.Label ServerVersionLabel;
        private System.Windows.Forms.Label DBVersionLabel;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.CheckBox UseSQLServerCheckbox;
    }
}