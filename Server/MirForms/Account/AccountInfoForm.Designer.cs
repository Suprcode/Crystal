namespace Server
{
    partial class AccountInfoForm
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
            this.CreateButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.FilterTextBox = new System.Windows.Forms.TextBox();
            this.RefreshButton = new System.Windows.Forms.Button();
            this.AccountInfoPanel = new System.Windows.Forms.Panel();
            this.AdminCheckBox = new System.Windows.Forms.CheckBox();
            this.PermBanButton = new System.Windows.Forms.Button();
            this.WeekBanButton = new System.Windows.Forms.Button();
            this.DayBanButton = new System.Windows.Forms.Button();
            this.BannedCheckBox = new System.Windows.Forms.CheckBox();
            this.ExpiryDateTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.BanReasonTextBox = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.LastDateTextBox = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.LastIPTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.CreationDateTextBox = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.CreationIPTextBox = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.EMailTextBox = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.AnswerTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.QuestionTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.BirthDateTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.UserNameTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.PasswordTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.AccountIDTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.FilterPlayerTextBox = new System.Windows.Forms.TextBox();
            this.AccountInfoListView = new Server.ListViewNF();
            this.indexHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.accountIDHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.passwordHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.userNameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.adminHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.bannedHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.banReasonHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.expiryDateHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.MatchFilterCheckBox = new System.Windows.Forms.CheckBox();
            this.WipeCharButton = new System.Windows.Forms.Button();
            this.AccountInfoPanel.SuspendLayout();
            this.SuspendLayout();
            // 
            // CreateButton
            // 
            this.CreateButton.Location = new System.Drawing.Point(12, 12);
            this.CreateButton.Name = "CreateButton";
            this.CreateButton.Size = new System.Drawing.Size(75, 23);
            this.CreateButton.TabIndex = 9;
            this.CreateButton.Text = "Create";
            this.CreateButton.UseVisualStyleBackColor = true;
            this.CreateButton.Click += new System.EventHandler(this.CreateButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 11;
            this.label1.Text = "Filter Account ID:";
            // 
            // FilterTextBox
            // 
            this.FilterTextBox.Location = new System.Drawing.Point(107, 39);
            this.FilterTextBox.Name = "FilterTextBox";
            this.FilterTextBox.Size = new System.Drawing.Size(100, 20);
            this.FilterTextBox.TabIndex = 12;
            // 
            // RefreshButton
            // 
            this.RefreshButton.Location = new System.Drawing.Point(390, 37);
            this.RefreshButton.Name = "RefreshButton";
            this.RefreshButton.Size = new System.Drawing.Size(75, 23);
            this.RefreshButton.TabIndex = 13;
            this.RefreshButton.Text = "Refresh";
            this.RefreshButton.UseVisualStyleBackColor = true;
            this.RefreshButton.Click += new System.EventHandler(this.RefreshButton_Click);
            // 
            // AccountInfoPanel
            // 
            this.AccountInfoPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountInfoPanel.Controls.Add(this.AdminCheckBox);
            this.AccountInfoPanel.Controls.Add(this.PermBanButton);
            this.AccountInfoPanel.Controls.Add(this.WeekBanButton);
            this.AccountInfoPanel.Controls.Add(this.DayBanButton);
            this.AccountInfoPanel.Controls.Add(this.BannedCheckBox);
            this.AccountInfoPanel.Controls.Add(this.ExpiryDateTextBox);
            this.AccountInfoPanel.Controls.Add(this.label14);
            this.AccountInfoPanel.Controls.Add(this.BanReasonTextBox);
            this.AccountInfoPanel.Controls.Add(this.label13);
            this.AccountInfoPanel.Controls.Add(this.LastDateTextBox);
            this.AccountInfoPanel.Controls.Add(this.label11);
            this.AccountInfoPanel.Controls.Add(this.LastIPTextBox);
            this.AccountInfoPanel.Controls.Add(this.label12);
            this.AccountInfoPanel.Controls.Add(this.CreationDateTextBox);
            this.AccountInfoPanel.Controls.Add(this.label9);
            this.AccountInfoPanel.Controls.Add(this.CreationIPTextBox);
            this.AccountInfoPanel.Controls.Add(this.label10);
            this.AccountInfoPanel.Controls.Add(this.EMailTextBox);
            this.AccountInfoPanel.Controls.Add(this.label8);
            this.AccountInfoPanel.Controls.Add(this.AnswerTextBox);
            this.AccountInfoPanel.Controls.Add(this.label7);
            this.AccountInfoPanel.Controls.Add(this.QuestionTextBox);
            this.AccountInfoPanel.Controls.Add(this.label6);
            this.AccountInfoPanel.Controls.Add(this.BirthDateTextBox);
            this.AccountInfoPanel.Controls.Add(this.label5);
            this.AccountInfoPanel.Controls.Add(this.UserNameTextBox);
            this.AccountInfoPanel.Controls.Add(this.label4);
            this.AccountInfoPanel.Controls.Add(this.PasswordTextBox);
            this.AccountInfoPanel.Controls.Add(this.label3);
            this.AccountInfoPanel.Controls.Add(this.AccountIDTextBox);
            this.AccountInfoPanel.Controls.Add(this.label2);
            this.AccountInfoPanel.Location = new System.Drawing.Point(12, 240);
            this.AccountInfoPanel.Name = "AccountInfoPanel";
            this.AccountInfoPanel.Size = new System.Drawing.Size(616, 209);
            this.AccountInfoPanel.TabIndex = 14;
            // 
            // AdminCheckBox
            // 
            this.AdminCheckBox.AutoSize = true;
            this.AdminCheckBox.Location = new System.Drawing.Point(207, 17);
            this.AdminCheckBox.Name = "AdminCheckBox";
            this.AdminCheckBox.Size = new System.Drawing.Size(86, 17);
            this.AdminCheckBox.TabIndex = 32;
            this.AdminCheckBox.Text = "Administrator";
            this.AdminCheckBox.UseVisualStyleBackColor = true;
            this.AdminCheckBox.CheckedChanged += new System.EventHandler(this.AdminCheckBox_CheckedChanged);
            // 
            // PermBanButton
            // 
            this.PermBanButton.Location = new System.Drawing.Point(511, 178);
            this.PermBanButton.Name = "PermBanButton";
            this.PermBanButton.Size = new System.Drawing.Size(75, 23);
            this.PermBanButton.TabIndex = 31;
            this.PermBanButton.Text = "Perm Ban";
            this.PermBanButton.UseVisualStyleBackColor = true;
            this.PermBanButton.Click += new System.EventHandler(this.PermBanButton_Click);
            // 
            // WeekBanButton
            // 
            this.WeekBanButton.Location = new System.Drawing.Point(430, 180);
            this.WeekBanButton.Name = "WeekBanButton";
            this.WeekBanButton.Size = new System.Drawing.Size(75, 23);
            this.WeekBanButton.TabIndex = 30;
            this.WeekBanButton.Text = "Week Ban";
            this.WeekBanButton.UseVisualStyleBackColor = true;
            this.WeekBanButton.Click += new System.EventHandler(this.WeekBanButton_Click);
            // 
            // DayBanButton
            // 
            this.DayBanButton.Location = new System.Drawing.Point(349, 180);
            this.DayBanButton.Name = "DayBanButton";
            this.DayBanButton.Size = new System.Drawing.Size(75, 23);
            this.DayBanButton.TabIndex = 29;
            this.DayBanButton.Text = "Day Ban";
            this.DayBanButton.UseVisualStyleBackColor = true;
            this.DayBanButton.Click += new System.EventHandler(this.DayBanButton_Click);
            // 
            // BannedCheckBox
            // 
            this.BannedCheckBox.AutoSize = true;
            this.BannedCheckBox.Location = new System.Drawing.Point(523, 156);
            this.BannedCheckBox.Name = "BannedCheckBox";
            this.BannedCheckBox.Size = new System.Drawing.Size(63, 17);
            this.BannedCheckBox.TabIndex = 28;
            this.BannedCheckBox.Text = "Banned";
            this.BannedCheckBox.UseVisualStyleBackColor = true;
            this.BannedCheckBox.CheckedChanged += new System.EventHandler(this.BannedCheckBox_CheckedChanged);
            // 
            // ExpiryDateTextBox
            // 
            this.ExpiryDateTextBox.Location = new System.Drawing.Point(394, 154);
            this.ExpiryDateTextBox.Name = "ExpiryDateTextBox";
            this.ExpiryDateTextBox.Size = new System.Drawing.Size(120, 20);
            this.ExpiryDateTextBox.TabIndex = 27;
            this.ExpiryDateTextBox.TextChanged += new System.EventHandler(this.ExpiryDateTextBox_TextChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(324, 157);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(64, 13);
            this.label14.TabIndex = 26;
            this.label14.Text = "Expiry Date:";
            // 
            // BanReasonTextBox
            // 
            this.BanReasonTextBox.Location = new System.Drawing.Point(394, 128);
            this.BanReasonTextBox.Name = "BanReasonTextBox";
            this.BanReasonTextBox.Size = new System.Drawing.Size(192, 20);
            this.BanReasonTextBox.TabIndex = 25;
            this.BanReasonTextBox.TextChanged += new System.EventHandler(this.BanReasonTextBox_TextChanged);
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(319, 131);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 13);
            this.label13.TabIndex = 24;
            this.label13.Text = "Ban Reason:";
            // 
            // LastDateTextBox
            // 
            this.LastDateTextBox.Location = new System.Drawing.Point(394, 95);
            this.LastDateTextBox.Name = "LastDateTextBox";
            this.LastDateTextBox.ReadOnly = true;
            this.LastDateTextBox.Size = new System.Drawing.Size(120, 20);
            this.LastDateTextBox.TabIndex = 23;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(332, 98);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(56, 13);
            this.label11.TabIndex = 22;
            this.label11.Text = "Last Date:";
            // 
            // LastIPTextBox
            // 
            this.LastIPTextBox.Location = new System.Drawing.Point(394, 69);
            this.LastIPTextBox.Name = "LastIPTextBox";
            this.LastIPTextBox.ReadOnly = true;
            this.LastIPTextBox.Size = new System.Drawing.Size(100, 20);
            this.LastIPTextBox.TabIndex = 21;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(345, 72);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(43, 13);
            this.label12.TabIndex = 20;
            this.label12.Text = "Last IP:";
            // 
            // CreationDateTextBox
            // 
            this.CreationDateTextBox.Location = new System.Drawing.Point(394, 43);
            this.CreationDateTextBox.Name = "CreationDateTextBox";
            this.CreationDateTextBox.ReadOnly = true;
            this.CreationDateTextBox.Size = new System.Drawing.Size(120, 20);
            this.CreationDateTextBox.TabIndex = 19;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(313, 46);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(75, 13);
            this.label9.TabIndex = 18;
            this.label9.Text = "Creation Date:";
            // 
            // CreationIPTextBox
            // 
            this.CreationIPTextBox.Location = new System.Drawing.Point(394, 17);
            this.CreationIPTextBox.Name = "CreationIPTextBox";
            this.CreationIPTextBox.ReadOnly = true;
            this.CreationIPTextBox.Size = new System.Drawing.Size(100, 20);
            this.CreationIPTextBox.TabIndex = 17;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(326, 20);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(62, 13);
            this.label10.TabIndex = 16;
            this.label10.Text = "Creation IP:";
            // 
            // EMailTextBox
            // 
            this.EMailTextBox.Location = new System.Drawing.Point(95, 180);
            this.EMailTextBox.Name = "EMailTextBox";
            this.EMailTextBox.Size = new System.Drawing.Size(192, 20);
            this.EMailTextBox.TabIndex = 15;
            this.EMailTextBox.TextChanged += new System.EventHandler(this.EMailTextBox_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(12, 183);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(77, 13);
            this.label8.TabIndex = 14;
            this.label8.Text = "EMail Address:";
            // 
            // AnswerTextBox
            // 
            this.AnswerTextBox.Location = new System.Drawing.Point(95, 154);
            this.AnswerTextBox.Name = "AnswerTextBox";
            this.AnswerTextBox.Size = new System.Drawing.Size(132, 20);
            this.AnswerTextBox.TabIndex = 13;
            this.AnswerTextBox.TextChanged += new System.EventHandler(this.AnswerTextBox_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(44, 157);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(45, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Answer:";
            // 
            // QuestionTextBox
            // 
            this.QuestionTextBox.Location = new System.Drawing.Point(95, 128);
            this.QuestionTextBox.Name = "QuestionTextBox";
            this.QuestionTextBox.Size = new System.Drawing.Size(132, 20);
            this.QuestionTextBox.TabIndex = 11;
            this.QuestionTextBox.TextChanged += new System.EventHandler(this.QuestionTextBox_TextChanged);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(37, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(52, 13);
            this.label6.TabIndex = 10;
            this.label6.Text = "Question:";
            // 
            // BirthDateTextBox
            // 
            this.BirthDateTextBox.Location = new System.Drawing.Point(95, 102);
            this.BirthDateTextBox.Name = "BirthDateTextBox";
            this.BirthDateTextBox.Size = new System.Drawing.Size(73, 20);
            this.BirthDateTextBox.TabIndex = 9;
            this.BirthDateTextBox.TextChanged += new System.EventHandler(this.BirthDateTextBox_TextChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 105);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(57, 13);
            this.label5.TabIndex = 8;
            this.label5.Text = "Birth Date:";
            // 
            // UserNameTextBox
            // 
            this.UserNameTextBox.Location = new System.Drawing.Point(95, 76);
            this.UserNameTextBox.Name = "UserNameTextBox";
            this.UserNameTextBox.Size = new System.Drawing.Size(100, 20);
            this.UserNameTextBox.TabIndex = 7;
            this.UserNameTextBox.TextChanged += new System.EventHandler(this.UserNameTextBox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(26, 79);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(63, 13);
            this.label4.TabIndex = 6;
            this.label4.Text = "User Name:";
            // 
            // PasswordTextBox
            // 
            this.PasswordTextBox.Location = new System.Drawing.Point(95, 40);
            this.PasswordTextBox.Name = "PasswordTextBox";
            this.PasswordTextBox.Size = new System.Drawing.Size(100, 20);
            this.PasswordTextBox.TabIndex = 5;
            this.PasswordTextBox.TextChanged += new System.EventHandler(this.PasswordTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(33, 43);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(56, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "Password:";
            // 
            // AccountIDTextBox
            // 
            this.AccountIDTextBox.Location = new System.Drawing.Point(95, 14);
            this.AccountIDTextBox.Name = "AccountIDTextBox";
            this.AccountIDTextBox.Size = new System.Drawing.Size(100, 20);
            this.AccountIDTextBox.TabIndex = 3;
            this.AccountIDTextBox.TextChanged += new System.EventHandler(this.AccountIDTextBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(25, 17);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Account ID:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(214, 42);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(64, 13);
            this.label15.TabIndex = 15;
            this.label15.Text = "Filter Player:";
            // 
            // FilterPlayerTextBox
            // 
            this.FilterPlayerTextBox.Location = new System.Drawing.Point(284, 39);
            this.FilterPlayerTextBox.Name = "FilterPlayerTextBox";
            this.FilterPlayerTextBox.Size = new System.Drawing.Size(100, 20);
            this.FilterPlayerTextBox.TabIndex = 16;
            // 
            // AccountInfoListView
            // 
            this.AccountInfoListView.AllowColumnReorder = true;
            this.AccountInfoListView.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.AccountInfoListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.indexHeader,
            this.accountIDHeader,
            this.passwordHeader,
            this.userNameHeader,
            this.adminHeader,
            this.bannedHeader,
            this.banReasonHeader,
            this.expiryDateHeader});
            this.AccountInfoListView.FullRowSelect = true;
            this.AccountInfoListView.GridLines = true;
            this.AccountInfoListView.HideSelection = false;
            this.AccountInfoListView.Location = new System.Drawing.Point(10, 65);
            this.AccountInfoListView.Name = "AccountInfoListView";
            this.AccountInfoListView.Size = new System.Drawing.Size(618, 169);
            this.AccountInfoListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.AccountInfoListView.TabIndex = 8;
            this.AccountInfoListView.UseCompatibleStateImageBehavior = false;
            this.AccountInfoListView.View = System.Windows.Forms.View.Details;
            this.AccountInfoListView.SelectedIndexChanged += new System.EventHandler(this.AccountInfoListView_SelectedIndexChanged);
            // 
            // indexHeader
            // 
            this.indexHeader.Text = "Index";
            // 
            // accountIDHeader
            // 
            this.accountIDHeader.Text = "Account ID";
            this.accountIDHeader.Width = 92;
            // 
            // passwordHeader
            // 
            this.passwordHeader.Text = "Password";
            // 
            // userNameHeader
            // 
            this.userNameHeader.Text = "User Name";
            this.userNameHeader.Width = 75;
            // 
            // adminHeader
            // 
            this.adminHeader.Text = "Administrator";
            this.adminHeader.Width = 73;
            // 
            // bannedHeader
            // 
            this.bannedHeader.Text = "Banned";
            this.bannedHeader.Width = 54;
            // 
            // banReasonHeader
            // 
            this.banReasonHeader.Text = "Ban Reason";
            this.banReasonHeader.Width = 74;
            // 
            // expiryDateHeader
            // 
            this.expiryDateHeader.Text = "Expiry Date";
            this.expiryDateHeader.Width = 81;
            // 
            // MatchFilterCheckBox
            // 
            this.MatchFilterCheckBox.AutoSize = true;
            this.MatchFilterCheckBox.Location = new System.Drawing.Point(472, 42);
            this.MatchFilterCheckBox.Name = "MatchFilterCheckBox";
            this.MatchFilterCheckBox.Size = new System.Drawing.Size(81, 17);
            this.MatchFilterCheckBox.TabIndex = 17;
            this.MatchFilterCheckBox.Text = "Match Filter";
            this.MatchFilterCheckBox.UseVisualStyleBackColor = true;
            // 
            // WipeCharButton
            // 
            this.WipeCharButton.Location = new System.Drawing.Point(93, 12);
            this.WipeCharButton.Name = "WipeCharButton";
            this.WipeCharButton.Size = new System.Drawing.Size(115, 23);
            this.WipeCharButton.TabIndex = 18;
            this.WipeCharButton.Text = "Wipe All Characters";
            this.WipeCharButton.UseVisualStyleBackColor = true;
            this.WipeCharButton.Click += new System.EventHandler(this.WipeCharButton_Click);
            // 
            // AccountInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 461);
            this.Controls.Add(this.WipeCharButton);
            this.Controls.Add(this.MatchFilterCheckBox);
            this.Controls.Add(this.FilterPlayerTextBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.AccountInfoPanel);
            this.Controls.Add(this.RefreshButton);
            this.Controls.Add(this.FilterTextBox);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.CreateButton);
            this.Controls.Add(this.AccountInfoListView);
            this.Name = "AccountInfoForm";
            this.Text = "AccountInfoForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AccountInfoForm_FormClosed);
            this.AccountInfoPanel.ResumeLayout(false);
            this.AccountInfoPanel.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button CreateButton;
        private ListViewNF AccountInfoListView;
        private System.Windows.Forms.ColumnHeader indexHeader;
        private System.Windows.Forms.ColumnHeader accountIDHeader;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox FilterTextBox;
        private System.Windows.Forms.ColumnHeader passwordHeader;
        private System.Windows.Forms.ColumnHeader userNameHeader;
        private System.Windows.Forms.ColumnHeader bannedHeader;
        private System.Windows.Forms.ColumnHeader banReasonHeader;
        private System.Windows.Forms.ColumnHeader expiryDateHeader;
        private System.Windows.Forms.Button RefreshButton;
        private System.Windows.Forms.Panel AccountInfoPanel;
        private System.Windows.Forms.TextBox AnswerTextBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox QuestionTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox BirthDateTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox UserNameTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox AccountIDTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox EMailTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox LastDateTextBox;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox LastIPTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox CreationDateTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox CreationIPTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox ExpiryDateTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox BanReasonTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox BannedCheckBox;
        private System.Windows.Forms.Button PermBanButton;
        private System.Windows.Forms.Button WeekBanButton;
        private System.Windows.Forms.Button DayBanButton;
        private System.Windows.Forms.CheckBox AdminCheckBox;
        private System.Windows.Forms.ColumnHeader adminHeader;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox FilterPlayerTextBox;
        private System.Windows.Forms.CheckBox MatchFilterCheckBox;
        private System.Windows.Forms.Button WipeCharButton;
    }
}