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
            SaveButton = new Button();
            configTabs = new TabControl();
            tabPage1 = new TabPage();
            groupBox1 = new GroupBox();
            label11 = new Label();
            DBVersionLabel = new Label();
            ServerVersionLabel = new Label();
            label10 = new Label();
            RelogDelayTextBox = new TextBox();
            label7 = new Label();
            VersionCheckBox = new CheckBox();
            VPathBrowseButton = new Button();
            VPathTextBox = new TextBox();
            label1 = new Label();
            tabPage2 = new TabPage();
            maxConnectionsPerIP = new TextBox();
            lblMaxConnectionsPerIP = new Label();
            StartHTTPCheckBox = new CheckBox();
            label15 = new Label();
            HTTPTrustedIPAddressTextBox = new TextBox();
            label14 = new Label();
            HTTPIPAddressTextBox = new TextBox();
            label13 = new Label();
            MaxUserTextBox = new TextBox();
            label5 = new Label();
            TimeOutTextBox = new TextBox();
            label4 = new Label();
            PortTextBox = new TextBox();
            label3 = new Label();
            IPAddressTextBox = new TextBox();
            label2 = new Label();
            tabPage3 = new TabPage();
            label9 = new Label();
            label8 = new Label();
            Resolution_textbox = new TextBox();
            AllowArcherCheckBox = new CheckBox();
            AllowAssassinCheckBox = new CheckBox();
            StartGameCheckBox = new CheckBox();
            DCharacterCheckBox = new CheckBox();
            NCharacterCheckBox = new CheckBox();
            LoginCheckBox = new CheckBox();
            PasswordCheckBox = new CheckBox();
            AccountCheckBox = new CheckBox();
            tabPage4 = new TabPage();
            label12 = new Label();
            SaveDelayTextBox = new TextBox();
            label6 = new Label();
            tabPage6 = new TabPage();
            gbRestedExpRates = new GroupBox();
            label22 = new Label();
            label23 = new Label();
            label21 = new Label();
            label20 = new Label();
            tbRestedPeriod = new TextBox();
            tbRestedBuffLength = new TextBox();
            tbMaxRestedBonus = new TextBox();
            tbRestedExpBonus = new TextBox();
            lblMaxRestedBonus = new Label();
            lblRestedExpBonus = new Label();
            lblRestedBuffLength = new Label();
            lblPeriod = new Label();
            gbGlobals = new GroupBox();
            label19 = new Label();
            label18 = new Label();
            dropRateInput = new NumericUpDown();
            lblDropRate = new Label();
            expRateInput = new NumericUpDown();
            lblExpRate = new Label();
            tabPage5 = new TabPage();
            label16 = new Label();
            lineMessageTimeTextBox = new TextBox();
            label17 = new Label();
            gameMasterEffect_CheckBox = new CheckBox();
            SafeZoneHealingCheckBox = new CheckBox();
            SafeZoneBorderCheckBox = new CheckBox();
            VPathDialog = new OpenFileDialog();
            groupBox2 = new GroupBox();
            RemoveSinDrops = new Button();
            RemoveArcDrops = new Button();
            ReaddArcDrops = new Button();
            ReaddSinDrops = new Button();
            configTabs.SuspendLayout();
            tabPage1.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage6.SuspendLayout();
            gbRestedExpRates.SuspendLayout();
            gbGlobals.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)dropRateInput).BeginInit();
            ((System.ComponentModel.ISupportInitialize)expRateInput).BeginInit();
            tabPage5.SuspendLayout();
            groupBox2.SuspendLayout();
            SuspendLayout();
            // 
            // SaveButton
            // 
            SaveButton.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            SaveButton.Location = new Point(411, 398);
            SaveButton.Margin = new Padding(3, 4, 3, 4);
            SaveButton.Name = "SaveButton";
            SaveButton.Size = new Size(87, 26);
            SaveButton.TabIndex = 6;
            SaveButton.Text = "Close";
            SaveButton.UseVisualStyleBackColor = true;
            SaveButton.Click += SaveButton_Click;
            // 
            // configTabs
            // 
            configTabs.Controls.Add(tabPage1);
            configTabs.Controls.Add(tabPage2);
            configTabs.Controls.Add(tabPage3);
            configTabs.Controls.Add(tabPage4);
            configTabs.Controls.Add(tabPage6);
            configTabs.Controls.Add(tabPage5);
            configTabs.Location = new Point(14, 14);
            configTabs.Margin = new Padding(3, 4, 3, 4);
            configTabs.Name = "configTabs";
            configTabs.SelectedIndex = 0;
            configTabs.Size = new Size(484, 376);
            configTabs.TabIndex = 5;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(groupBox2);
            tabPage1.Controls.Add(groupBox1);
            tabPage1.Controls.Add(RelogDelayTextBox);
            tabPage1.Controls.Add(label7);
            tabPage1.Controls.Add(VersionCheckBox);
            tabPage1.Controls.Add(VPathBrowseButton);
            tabPage1.Controls.Add(VPathTextBox);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(3, 4, 3, 4);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3, 4, 3, 4);
            tabPage1.Size = new Size(476, 348);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Version";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label11);
            groupBox1.Controls.Add(DBVersionLabel);
            groupBox1.Controls.Add(ServerVersionLabel);
            groupBox1.Controls.Add(label10);
            groupBox1.Location = new Point(7, 266);
            groupBox1.Margin = new Padding(3, 4, 3, 4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(3, 4, 3, 4);
            groupBox1.Size = new Size(460, 74);
            groupBox1.TabIndex = 25;
            groupBox1.TabStop = false;
            groupBox1.Text = "Version Info";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(7, 49);
            label11.Name = "label11";
            label11.Size = new Size(55, 15);
            label11.TabIndex = 23;
            label11.Text = "Database";
            // 
            // DBVersionLabel
            // 
            DBVersionLabel.AutoSize = true;
            DBVersionLabel.Location = new Point(89, 49);
            DBVersionLabel.Name = "DBVersionLabel";
            DBVersionLabel.Size = new Size(45, 15);
            DBVersionLabel.TabIndex = 24;
            DBVersionLabel.Text = "Version";
            // 
            // ServerVersionLabel
            // 
            ServerVersionLabel.AutoSize = true;
            ServerVersionLabel.Location = new Point(89, 23);
            ServerVersionLabel.Name = "ServerVersionLabel";
            ServerVersionLabel.Size = new Size(45, 15);
            ServerVersionLabel.TabIndex = 7;
            ServerVersionLabel.Text = "Version";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(7, 23);
            label10.Name = "label10";
            label10.Size = new Size(39, 15);
            label10.TabIndex = 22;
            label10.Text = "Server";
            // 
            // RelogDelayTextBox
            // 
            RelogDelayTextBox.Location = new Point(104, 75);
            RelogDelayTextBox.Margin = new Padding(3, 4, 3, 4);
            RelogDelayTextBox.MaxLength = 5;
            RelogDelayTextBox.Name = "RelogDelayTextBox";
            RelogDelayTextBox.Size = new Size(108, 23);
            RelogDelayTextBox.TabIndex = 21;
            RelogDelayTextBox.TextChanged += CheckUShort;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(17, 79);
            label7.Name = "label7";
            label7.Size = new Size(72, 15);
            label7.TabIndex = 20;
            label7.Text = "Relog Delay:";
            // 
            // VersionCheckBox
            // 
            VersionCheckBox.AutoSize = true;
            VersionCheckBox.Location = new Point(104, 49);
            VersionCheckBox.Margin = new Padding(3, 4, 3, 4);
            VersionCheckBox.Name = "VersionCheckBox";
            VersionCheckBox.Size = new Size(150, 19);
            VersionCheckBox.TabIndex = 3;
            VersionCheckBox.Text = "Check for client version";
            VersionCheckBox.UseVisualStyleBackColor = true;
            // 
            // VPathBrowseButton
            // 
            VPathBrowseButton.Location = new Point(436, 16);
            VPathBrowseButton.Margin = new Padding(3, 4, 3, 4);
            VPathBrowseButton.Name = "VPathBrowseButton";
            VPathBrowseButton.Size = new Size(33, 26);
            VPathBrowseButton.TabIndex = 2;
            VPathBrowseButton.Text = "...";
            VPathBrowseButton.UseVisualStyleBackColor = true;
            VPathBrowseButton.Click += VPathBrowseButton_Click;
            // 
            // VPathTextBox
            // 
            VPathTextBox.Location = new Point(104, 19);
            VPathTextBox.Margin = new Padding(3, 4, 3, 4);
            VPathTextBox.Name = "VPathTextBox";
            VPathTextBox.ReadOnly = true;
            VPathTextBox.Size = new Size(324, 23);
            VPathTextBox.TabIndex = 1;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 23);
            label1.Name = "label1";
            label1.Size = new Size(75, 15);
            label1.TabIndex = 0;
            label1.Text = "Version Path:";
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(maxConnectionsPerIP);
            tabPage2.Controls.Add(lblMaxConnectionsPerIP);
            tabPage2.Controls.Add(StartHTTPCheckBox);
            tabPage2.Controls.Add(label15);
            tabPage2.Controls.Add(HTTPTrustedIPAddressTextBox);
            tabPage2.Controls.Add(label14);
            tabPage2.Controls.Add(HTTPIPAddressTextBox);
            tabPage2.Controls.Add(label13);
            tabPage2.Controls.Add(MaxUserTextBox);
            tabPage2.Controls.Add(label5);
            tabPage2.Controls.Add(TimeOutTextBox);
            tabPage2.Controls.Add(label4);
            tabPage2.Controls.Add(PortTextBox);
            tabPage2.Controls.Add(label3);
            tabPage2.Controls.Add(IPAddressTextBox);
            tabPage2.Controls.Add(label2);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(3, 4, 3, 4);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3, 4, 3, 4);
            tabPage2.Size = new Size(476, 348);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Network";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // maxConnectionsPerIP
            // 
            maxConnectionsPerIP.Location = new Point(142, 144);
            maxConnectionsPerIP.Margin = new Padding(3, 4, 3, 4);
            maxConnectionsPerIP.MaxLength = 5;
            maxConnectionsPerIP.Name = "maxConnectionsPerIP";
            maxConnectionsPerIP.Size = new Size(48, 23);
            maxConnectionsPerIP.TabIndex = 25;
            // 
            // lblMaxConnectionsPerIP
            // 
            lblMaxConnectionsPerIP.AutoSize = true;
            lblMaxConnectionsPerIP.Location = new Point(26, 147);
            lblMaxConnectionsPerIP.Name = "lblMaxConnectionsPerIP";
            lblMaxConnectionsPerIP.Size = new Size(115, 15);
            lblMaxConnectionsPerIP.TabIndex = 24;
            lblMaxConnectionsPerIP.Text = "Max connections/IP:";
            // 
            // StartHTTPCheckBox
            // 
            StartHTTPCheckBox.AutoSize = true;
            StartHTTPCheckBox.Location = new Point(28, 180);
            StartHTTPCheckBox.Margin = new Padding(3, 4, 3, 4);
            StartHTTPCheckBox.Name = "StartHTTPCheckBox";
            StartHTTPCheckBox.Size = new Size(123, 19);
            StartHTTPCheckBox.TabIndex = 23;
            StartHTTPCheckBox.Text = "Start HTTP Service";
            StartHTTPCheckBox.UseVisualStyleBackColor = true;
            StartHTTPCheckBox.CheckedChanged += StartHTTPCheckBox_CheckedChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(26, 285);
            label15.Name = "label15";
            label15.Size = new Size(224, 15);
            label15.TabIndex = 22;
            label15.Text = "(http service only allow trusted IP to visit)";
            // 
            // HTTPTrustedIPAddressTextBox
            // 
            HTTPTrustedIPAddressTextBox.Location = new Point(175, 248);
            HTTPTrustedIPAddressTextBox.Margin = new Padding(3, 4, 3, 4);
            HTTPTrustedIPAddressTextBox.MaxLength = 30;
            HTTPTrustedIPAddressTextBox.Name = "HTTPTrustedIPAddressTextBox";
            HTTPTrustedIPAddressTextBox.Size = new Size(198, 23);
            HTTPTrustedIPAddressTextBox.TabIndex = 21;
            HTTPTrustedIPAddressTextBox.TextChanged += HTTPTrustedIPAddressTextBox_TextChanged;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(26, 251);
            label14.Name = "label14";
            label14.Size = new Size(140, 15);
            label14.TabIndex = 20;
            label14.Text = "HTTP Trusted IP Address:";
            // 
            // HTTPIPAddressTextBox
            // 
            HTTPIPAddressTextBox.Location = new Point(175, 211);
            HTTPIPAddressTextBox.Margin = new Padding(3, 4, 3, 4);
            HTTPIPAddressTextBox.MaxLength = 30;
            HTTPIPAddressTextBox.Name = "HTTPIPAddressTextBox";
            HTTPIPAddressTextBox.Size = new Size(198, 23);
            HTTPIPAddressTextBox.TabIndex = 19;
            HTTPIPAddressTextBox.TextChanged += HTTPIPAddressTextBox_TextChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(26, 214);
            label13.Name = "label13";
            label13.Size = new Size(98, 15);
            label13.TabIndex = 18;
            label13.Text = "HTTP IP Address:";
            // 
            // MaxUserTextBox
            // 
            MaxUserTextBox.Location = new Point(142, 109);
            MaxUserTextBox.Margin = new Padding(3, 4, 3, 4);
            MaxUserTextBox.MaxLength = 5;
            MaxUserTextBox.Name = "MaxUserTextBox";
            MaxUserTextBox.Size = new Size(48, 23);
            MaxUserTextBox.TabIndex = 17;
            MaxUserTextBox.TextChanged += CheckUShort;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(71, 113);
            label5.Name = "label5";
            label5.Size = new Size(58, 15);
            label5.TabIndex = 16;
            label5.Text = "Max User:";
            // 
            // TimeOutTextBox
            // 
            TimeOutTextBox.Location = new Point(142, 79);
            TimeOutTextBox.Margin = new Padding(3, 4, 3, 4);
            TimeOutTextBox.MaxLength = 5;
            TimeOutTextBox.Name = "TimeOutTextBox";
            TimeOutTextBox.Size = new Size(108, 23);
            TimeOutTextBox.TabIndex = 15;
            TimeOutTextBox.TextChanged += CheckUShort;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(77, 83);
            label4.Name = "label4";
            label4.Size = new Size(57, 15);
            label4.TabIndex = 14;
            label4.Text = "TimeOut:";
            // 
            // PortTextBox
            // 
            PortTextBox.Location = new Point(142, 49);
            PortTextBox.Margin = new Padding(3, 4, 3, 4);
            PortTextBox.MaxLength = 5;
            PortTextBox.Name = "PortTextBox";
            PortTextBox.Size = new Size(48, 23);
            PortTextBox.TabIndex = 13;
            PortTextBox.TextChanged += CheckUShort;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(101, 53);
            label3.Name = "label3";
            label3.Size = new Size(32, 15);
            label3.TabIndex = 12;
            label3.Text = "Port:";
            // 
            // IPAddressTextBox
            // 
            IPAddressTextBox.Location = new Point(142, 19);
            IPAddressTextBox.Margin = new Padding(3, 4, 3, 4);
            IPAddressTextBox.MaxLength = 15;
            IPAddressTextBox.Name = "IPAddressTextBox";
            IPAddressTextBox.Size = new Size(108, 23);
            IPAddressTextBox.TabIndex = 11;
            IPAddressTextBox.TextChanged += IPAddressCheck;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(64, 23);
            label2.Name = "label2";
            label2.Size = new Size(65, 15);
            label2.TabIndex = 10;
            label2.Text = "IP Address:";
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(label9);
            tabPage3.Controls.Add(label8);
            tabPage3.Controls.Add(Resolution_textbox);
            tabPage3.Controls.Add(AllowArcherCheckBox);
            tabPage3.Controls.Add(AllowAssassinCheckBox);
            tabPage3.Controls.Add(StartGameCheckBox);
            tabPage3.Controls.Add(DCharacterCheckBox);
            tabPage3.Controls.Add(NCharacterCheckBox);
            tabPage3.Controls.Add(LoginCheckBox);
            tabPage3.Controls.Add(PasswordCheckBox);
            tabPage3.Controls.Add(AccountCheckBox);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Margin = new Padding(3, 4, 3, 4);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3, 4, 3, 4);
            tabPage3.Size = new Size(476, 348);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Permissions";
            tabPage3.UseVisualStyleBackColor = true;
            tabPage3.Click += tabPage3_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(25, 268);
            label9.Name = "label9";
            label9.Size = new Size(134, 15);
            label9.TabIndex = 16;
            label9.Text = "Max Resolution Allowed";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(0, 0);
            label8.Name = "label8";
            label8.Size = new Size(38, 15);
            label8.TabIndex = 15;
            label8.Text = "label8";
            // 
            // Resolution_textbox
            // 
            Resolution_textbox.Location = new Point(171, 266);
            Resolution_textbox.Margin = new Padding(3, 4, 3, 4);
            Resolution_textbox.Name = "Resolution_textbox";
            Resolution_textbox.Size = new Size(93, 23);
            Resolution_textbox.TabIndex = 14;
            Resolution_textbox.TextChanged += Resolution_textbox_TextChanged;
            // 
            // AllowArcherCheckBox
            // 
            AllowArcherCheckBox.AutoSize = true;
            AllowArcherCheckBox.Location = new Point(28, 227);
            AllowArcherCheckBox.Margin = new Padding(3, 4, 3, 4);
            AllowArcherCheckBox.Name = "AllowArcherCheckBox";
            AllowArcherCheckBox.Size = new Size(206, 19);
            AllowArcherCheckBox.TabIndex = 13;
            AllowArcherCheckBox.Text = "Allow Creation of the Archer Class";
            AllowArcherCheckBox.UseVisualStyleBackColor = true;
            // 
            // AllowAssassinCheckBox
            // 
            AllowAssassinCheckBox.AutoSize = true;
            AllowAssassinCheckBox.Location = new Point(28, 200);
            AllowAssassinCheckBox.Margin = new Padding(3, 4, 3, 4);
            AllowAssassinCheckBox.Name = "AllowAssassinCheckBox";
            AllowAssassinCheckBox.Size = new Size(215, 19);
            AllowAssassinCheckBox.TabIndex = 12;
            AllowAssassinCheckBox.Text = "Allow Creation of the Assassin Class";
            AllowAssassinCheckBox.UseVisualStyleBackColor = true;
            // 
            // StartGameCheckBox
            // 
            StartGameCheckBox.AutoSize = true;
            StartGameCheckBox.Location = new Point(28, 156);
            StartGameCheckBox.Margin = new Padding(3, 4, 3, 4);
            StartGameCheckBox.Name = "StartGameCheckBox";
            StartGameCheckBox.Size = new Size(245, 19);
            StartGameCheckBox.TabIndex = 11;
            StartGameCheckBox.Text = "Allow Characters to Login to Game World";
            StartGameCheckBox.UseVisualStyleBackColor = true;
            // 
            // DCharacterCheckBox
            // 
            DCharacterCheckBox.AutoSize = true;
            DCharacterCheckBox.Location = new Point(28, 129);
            DCharacterCheckBox.Margin = new Padding(3, 4, 3, 4);
            DCharacterCheckBox.Name = "DCharacterCheckBox";
            DCharacterCheckBox.Size = new Size(157, 19);
            DCharacterCheckBox.TabIndex = 10;
            DCharacterCheckBox.Text = "Allow Character Deletion";
            DCharacterCheckBox.UseVisualStyleBackColor = true;
            // 
            // NCharacterCheckBox
            // 
            NCharacterCheckBox.AutoSize = true;
            NCharacterCheckBox.Location = new Point(28, 103);
            NCharacterCheckBox.Margin = new Padding(3, 4, 3, 4);
            NCharacterCheckBox.Name = "NCharacterCheckBox";
            NCharacterCheckBox.Size = new Size(185, 19);
            NCharacterCheckBox.TabIndex = 9;
            NCharacterCheckBox.Text = "Allow New Character Creation";
            NCharacterCheckBox.UseVisualStyleBackColor = true;
            // 
            // LoginCheckBox
            // 
            LoginCheckBox.AutoSize = true;
            LoginCheckBox.Location = new Point(28, 76);
            LoginCheckBox.Margin = new Padding(3, 4, 3, 4);
            LoginCheckBox.Name = "LoginCheckBox";
            LoginCheckBox.Size = new Size(158, 19);
            LoginCheckBox.TabIndex = 8;
            LoginCheckBox.Text = "Allow Accounts To Login";
            LoginCheckBox.UseVisualStyleBackColor = true;
            // 
            // PasswordCheckBox
            // 
            PasswordCheckBox.AutoSize = true;
            PasswordCheckBox.Location = new Point(28, 50);
            PasswordCheckBox.Margin = new Padding(3, 4, 3, 4);
            PasswordCheckBox.Name = "PasswordCheckBox";
            PasswordCheckBox.Size = new Size(230, 19);
            PasswordCheckBox.TabIndex = 7;
            PasswordCheckBox.Text = "Allow Users To Change Their Password";
            PasswordCheckBox.UseVisualStyleBackColor = true;
            // 
            // AccountCheckBox
            // 
            AccountCheckBox.AutoSize = true;
            AccountCheckBox.Location = new Point(28, 23);
            AccountCheckBox.Margin = new Padding(3, 4, 3, 4);
            AccountCheckBox.Name = "AccountCheckBox";
            AccountCheckBox.Size = new Size(179, 19);
            AccountCheckBox.TabIndex = 6;
            AccountCheckBox.Text = "Allow New Account Creation";
            AccountCheckBox.UseVisualStyleBackColor = true;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(label12);
            tabPage4.Controls.Add(SaveDelayTextBox);
            tabPage4.Controls.Add(label6);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Margin = new Padding(3, 4, 3, 4);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3, 4, 3, 4);
            tabPage4.Size = new Size(476, 348);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Database";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(219, 23);
            label12.Name = "label12";
            label12.Size = new Size(50, 15);
            label12.TabIndex = 26;
            label12.Text = "minutes";
            // 
            // SaveDelayTextBox
            // 
            SaveDelayTextBox.Location = new Point(104, 19);
            SaveDelayTextBox.Margin = new Padding(3, 4, 3, 4);
            SaveDelayTextBox.MaxLength = 5;
            SaveDelayTextBox.Name = "SaveDelayTextBox";
            SaveDelayTextBox.Size = new Size(108, 23);
            SaveDelayTextBox.TabIndex = 25;
            SaveDelayTextBox.TextChanged += CheckUShort;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(21, 23);
            label6.Name = "label6";
            label6.Size = new Size(66, 15);
            label6.TabIndex = 24;
            label6.Text = "Save Delay:";
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(gbRestedExpRates);
            tabPage6.Controls.Add(gbGlobals);
            tabPage6.Location = new Point(4, 24);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(476, 348);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "Rates";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // gbRestedExpRates
            // 
            gbRestedExpRates.Controls.Add(label22);
            gbRestedExpRates.Controls.Add(label23);
            gbRestedExpRates.Controls.Add(label21);
            gbRestedExpRates.Controls.Add(label20);
            gbRestedExpRates.Controls.Add(tbRestedPeriod);
            gbRestedExpRates.Controls.Add(tbRestedBuffLength);
            gbRestedExpRates.Controls.Add(tbMaxRestedBonus);
            gbRestedExpRates.Controls.Add(tbRestedExpBonus);
            gbRestedExpRates.Controls.Add(lblMaxRestedBonus);
            gbRestedExpRates.Controls.Add(lblRestedExpBonus);
            gbRestedExpRates.Controls.Add(lblRestedBuffLength);
            gbRestedExpRates.Controls.Add(lblPeriod);
            gbRestedExpRates.Location = new Point(17, 133);
            gbRestedExpRates.Name = "gbRestedExpRates";
            gbRestedExpRates.Size = new Size(228, 150);
            gbRestedExpRates.TabIndex = 8;
            gbRestedExpRates.TabStop = false;
            gbRestedExpRates.Text = "Rested exp";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(158, 88);
            label22.Name = "label22";
            label22.Size = new Size(17, 15);
            label22.TabIndex = 12;
            label22.Text = "%";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(158, 121);
            label23.Name = "label23";
            label23.Size = new Size(12, 15);
            label23.TabIndex = 12;
            label23.Text = "x";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(158, 55);
            label21.Name = "label21";
            label21.Size = new Size(33, 15);
            label21.TabIndex = 12;
            label21.Text = "mins";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(158, 25);
            label20.Name = "label20";
            label20.Size = new Size(33, 15);
            label20.TabIndex = 12;
            label20.Text = "mins";
            // 
            // tbRestedPeriod
            // 
            tbRestedPeriod.Location = new Point(96, 21);
            tbRestedPeriod.Name = "tbRestedPeriod";
            tbRestedPeriod.Size = new Size(56, 23);
            tbRestedPeriod.TabIndex = 11;
            tbRestedPeriod.KeyPress += tbRestedPeriod_KeyPress;
            // 
            // tbRestedBuffLength
            // 
            tbRestedBuffLength.Location = new Point(96, 51);
            tbRestedBuffLength.Name = "tbRestedBuffLength";
            tbRestedBuffLength.Size = new Size(56, 23);
            tbRestedBuffLength.TabIndex = 11;
            tbRestedBuffLength.KeyPress += tbRestedBuffLength_KeyPress;
            // 
            // tbMaxRestedBonus
            // 
            tbMaxRestedBonus.Location = new Point(96, 118);
            tbMaxRestedBonus.Name = "tbMaxRestedBonus";
            tbMaxRestedBonus.Size = new Size(56, 23);
            tbMaxRestedBonus.TabIndex = 11;
            tbMaxRestedBonus.KeyPress += tbMaxRestedBonus_KeyPress;
            // 
            // tbRestedExpBonus
            // 
            tbRestedExpBonus.Location = new Point(96, 85);
            tbRestedExpBonus.Name = "tbRestedExpBonus";
            tbRestedExpBonus.Size = new Size(56, 23);
            tbRestedExpBonus.TabIndex = 11;
            tbRestedExpBonus.KeyPress += tbRestedExpBonus_KeyPress;
            // 
            // lblMaxRestedBonus
            // 
            lblMaxRestedBonus.AutoSize = true;
            lblMaxRestedBonus.Location = new Point(13, 121);
            lblMaxRestedBonus.Name = "lblMaxRestedBonus";
            lblMaxRestedBonus.Size = new Size(68, 15);
            lblMaxRestedBonus.TabIndex = 0;
            lblMaxRestedBonus.Text = "Max bonus:";
            // 
            // lblRestedExpBonus
            // 
            lblRestedExpBonus.AutoSize = true;
            lblRestedExpBonus.Location = new Point(13, 88);
            lblRestedExpBonus.Name = "lblRestedExpBonus";
            lblRestedExpBonus.Size = new Size(64, 15);
            lblRestedExpBonus.TabIndex = 0;
            lblRestedExpBonus.Text = "Exp bonus:";
            // 
            // lblRestedBuffLength
            // 
            lblRestedBuffLength.AutoSize = true;
            lblRestedBuffLength.Location = new Point(13, 55);
            lblRestedBuffLength.Name = "lblRestedBuffLength";
            lblRestedBuffLength.Size = new Size(69, 15);
            lblRestedBuffLength.TabIndex = 0;
            lblRestedBuffLength.Text = "Buff length:";
            // 
            // lblPeriod
            // 
            lblPeriod.AutoSize = true;
            lblPeriod.Location = new Point(13, 24);
            lblPeriod.Name = "lblPeriod";
            lblPeriod.Size = new Size(44, 15);
            lblPeriod.TabIndex = 0;
            lblPeriod.Text = "Period:";
            // 
            // gbGlobals
            // 
            gbGlobals.Controls.Add(label19);
            gbGlobals.Controls.Add(label18);
            gbGlobals.Controls.Add(dropRateInput);
            gbGlobals.Controls.Add(lblDropRate);
            gbGlobals.Controls.Add(expRateInput);
            gbGlobals.Controls.Add(lblExpRate);
            gbGlobals.Location = new Point(17, 18);
            gbGlobals.Name = "gbGlobals";
            gbGlobals.Size = new Size(228, 100);
            gbGlobals.TabIndex = 7;
            gbGlobals.TabStop = false;
            gbGlobals.Text = "Globals";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(139, 63);
            label19.Name = "label19";
            label19.Size = new Size(12, 15);
            label19.TabIndex = 11;
            label19.Text = "x";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(139, 24);
            label18.Name = "label18";
            label18.Size = new Size(12, 15);
            label18.TabIndex = 12;
            label18.Text = "x";
            // 
            // dropRateInput
            // 
            dropRateInput.DecimalPlaces = 2;
            dropRateInput.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            dropRateInput.Location = new Point(82, 61);
            dropRateInput.Name = "dropRateInput";
            dropRateInput.Size = new Size(51, 23);
            dropRateInput.TabIndex = 9;
            // 
            // lblDropRate
            // 
            lblDropRate.AutoSize = true;
            lblDropRate.Location = new Point(13, 63);
            lblDropRate.Name = "lblDropRate";
            lblDropRate.Size = new Size(59, 15);
            lblDropRate.TabIndex = 7;
            lblDropRate.Text = "Drop rate:";
            // 
            // expRateInput
            // 
            expRateInput.DecimalPlaces = 2;
            expRateInput.Increment = new decimal(new int[] { 1, 0, 0, 131072 });
            expRateInput.Location = new Point(82, 22);
            expRateInput.Name = "expRateInput";
            expRateInput.Size = new Size(51, 23);
            expRateInput.TabIndex = 10;
            // 
            // lblExpRate
            // 
            lblExpRate.AutoSize = true;
            lblExpRate.Location = new Point(13, 24);
            lblExpRate.Name = "lblExpRate";
            lblExpRate.Size = new Size(51, 15);
            lblExpRate.TabIndex = 8;
            lblExpRate.Text = "Exp rate:";
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(label16);
            tabPage5.Controls.Add(lineMessageTimeTextBox);
            tabPage5.Controls.Add(label17);
            tabPage5.Controls.Add(gameMasterEffect_CheckBox);
            tabPage5.Controls.Add(SafeZoneHealingCheckBox);
            tabPage5.Controls.Add(SafeZoneBorderCheckBox);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Margin = new Padding(3, 4, 3, 4);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3, 4, 3, 4);
            tabPage5.Size = new Size(476, 348);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Optional";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(229, 105);
            label16.Name = "label16";
            label16.Size = new Size(50, 15);
            label16.TabIndex = 29;
            label16.Text = "minutes";
            // 
            // lineMessageTimeTextBox
            // 
            lineMessageTimeTextBox.Location = new Point(183, 103);
            lineMessageTimeTextBox.Margin = new Padding(3, 4, 3, 4);
            lineMessageTimeTextBox.MaxLength = 5;
            lineMessageTimeTextBox.Name = "lineMessageTimeTextBox";
            lineMessageTimeTextBox.Size = new Size(41, 23);
            lineMessageTimeTextBox.TabIndex = 28;
            lineMessageTimeTextBox.Text = "10";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(25, 105);
            label17.Name = "label17";
            label17.Size = new Size(142, 15);
            label17.TabIndex = 27;
            label17.Text = "Line Message Frequency :";
            // 
            // gameMasterEffect_CheckBox
            // 
            gameMasterEffect_CheckBox.AutoSize = true;
            gameMasterEffect_CheckBox.Location = new Point(28, 76);
            gameMasterEffect_CheckBox.Margin = new Padding(3, 4, 3, 4);
            gameMasterEffect_CheckBox.Name = "gameMasterEffect_CheckBox";
            gameMasterEffect_CheckBox.Size = new Size(129, 19);
            gameMasterEffect_CheckBox.TabIndex = 2;
            gameMasterEffect_CheckBox.Text = "Game Master Effect";
            gameMasterEffect_CheckBox.UseVisualStyleBackColor = true;
            // 
            // SafeZoneHealingCheckBox
            // 
            SafeZoneHealingCheckBox.AutoSize = true;
            SafeZoneHealingCheckBox.Location = new Point(28, 50);
            SafeZoneHealingCheckBox.Margin = new Padding(3, 4, 3, 4);
            SafeZoneHealingCheckBox.Name = "SafeZoneHealingCheckBox";
            SafeZoneHealingCheckBox.Size = new Size(197, 19);
            SafeZoneHealingCheckBox.TabIndex = 1;
            SafeZoneHealingCheckBox.Text = "Enable auto-healing in SafeZone";
            SafeZoneHealingCheckBox.UseVisualStyleBackColor = true;
            SafeZoneHealingCheckBox.CheckedChanged += SafeZoneHealingCheckBox_CheckedChanged;
            // 
            // SafeZoneBorderCheckBox
            // 
            SafeZoneBorderCheckBox.AutoSize = true;
            SafeZoneBorderCheckBox.Location = new Point(28, 23);
            SafeZoneBorderCheckBox.Margin = new Padding(3, 4, 3, 4);
            SafeZoneBorderCheckBox.Name = "SafeZoneBorderCheckBox";
            SafeZoneBorderCheckBox.Size = new Size(150, 19);
            SafeZoneBorderCheckBox.TabIndex = 0;
            SafeZoneBorderCheckBox.Text = "Show SafeZone Borders";
            SafeZoneBorderCheckBox.UseVisualStyleBackColor = true;
            SafeZoneBorderCheckBox.CheckedChanged += SafeZoneBorderCheckBox_CheckedChanged;
            // 
            // VPathDialog
            // 
            VPathDialog.FileName = "Mir2.Exe";
            VPathDialog.Filter = "Executable Files (*.exe)|*.exe";
            VPathDialog.Multiselect = true;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(ReaddArcDrops);
            groupBox2.Controls.Add(ReaddSinDrops);
            groupBox2.Controls.Add(RemoveArcDrops);
            groupBox2.Controls.Add(RemoveSinDrops);
            groupBox2.Location = new Point(17, 115);
            groupBox2.Name = "groupBox2";
            groupBox2.Size = new Size(310, 84);
            groupBox2.TabIndex = 26;
            groupBox2.TabStop = false;
            groupBox2.Text = "Drops";
            // 
            // RemoveSinDrops
            // 
            RemoveSinDrops.Location = new Point(6, 22);
            RemoveSinDrops.Name = "RemoveSinDrops";
            RemoveSinDrops.Size = new Size(144, 23);
            RemoveSinDrops.TabIndex = 0;
            RemoveSinDrops.Text = "Remove Assassin Drops";
            RemoveSinDrops.UseVisualStyleBackColor = true;
            RemoveSinDrops.Click += RemoveSinDrops_Click;
            // 
            // RemoveArcDrops
            // 
            RemoveArcDrops.Location = new Point(6, 51);
            RemoveArcDrops.Name = "RemoveArcDrops";
            RemoveArcDrops.Size = new Size(144, 23);
            RemoveArcDrops.TabIndex = 1;
            RemoveArcDrops.Text = "Remove Archer Drops";
            RemoveArcDrops.UseVisualStyleBackColor = true;
            RemoveArcDrops.Click += RemoveArcDrops_Click;
            // 
            // ReaddArcDrops
            // 
            ReaddArcDrops.Location = new Point(158, 51);
            ReaddArcDrops.Name = "ReaddArcDrops";
            ReaddArcDrops.Size = new Size(144, 23);
            ReaddArcDrops.TabIndex = 3;
            ReaddArcDrops.Text = "ReAdd Archer Drops";
            ReaddArcDrops.UseVisualStyleBackColor = true;
            ReaddArcDrops.Click += ReaddArcDrops_Click;
            // 
            // ReaddSinDrops
            // 
            ReaddSinDrops.Location = new Point(158, 22);
            ReaddSinDrops.Name = "ReaddSinDrops";
            ReaddSinDrops.Size = new Size(144, 23);
            ReaddSinDrops.TabIndex = 2;
            ReaddSinDrops.Text = "ReAdd Assassin Drops";
            ReaddSinDrops.UseVisualStyleBackColor = true;
            ReaddSinDrops.Click += ReaddSinDrops_Click;
            // 
            // ConfigForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(512, 430);
            Controls.Add(SaveButton);
            Controls.Add(configTabs);
            Margin = new Padding(3, 4, 3, 4);
            Name = "ConfigForm";
            Text = "Server Config Form";
            FormClosed += ConfigForm_FormClosed;
            configTabs.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            tabPage6.ResumeLayout(false);
            gbRestedExpRates.ResumeLayout(false);
            gbRestedExpRates.PerformLayout();
            gbGlobals.ResumeLayout(false);
            gbGlobals.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)dropRateInput).EndInit();
            ((System.ComponentModel.ISupportInitialize)expRateInput).EndInit();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            groupBox2.ResumeLayout(false);
            ResumeLayout(false);
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
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.CheckBox gameMasterEffect_CheckBox;
        private System.Windows.Forms.TextBox HTTPIPAddressTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox HTTPTrustedIPAddressTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.CheckBox StartHTTPCheckBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox lineMessageTimeTextBox;
        private System.Windows.Forms.Label label17;
        private TextBox maxConnectionsPerIP;
        private Label lblMaxConnectionsPerIP;
        private TabPage tabPage6;
        private GroupBox gbRestedExpRates;
        private GroupBox gbGlobals;
        private Label label19;
        private Label label18;
        private NumericUpDown dropRateInput;
        private Label lblDropRate;
        private NumericUpDown expRateInput;
        private Label lblExpRate;
        private Label lblMaxRestedBonus;
        private Label lblRestedExpBonus;
        private Label lblRestedBuffLength;
        private Label lblPeriod;
        private TextBox tbRestedBuffLength;
        private TextBox tbMaxRestedBonus;
        private TextBox tbRestedExpBonus;
        private TextBox tbRestedPeriod;
        private Label label22;
        private Label label23;
        private Label label21;
        private Label label20;
        private GroupBox groupBox2;
        private Button ReaddArcDrops;
        private Button ReaddSinDrops;
        private Button RemoveArcDrops;
        private Button RemoveSinDrops;
    }
}