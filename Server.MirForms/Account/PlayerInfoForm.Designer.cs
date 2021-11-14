namespace Server
{
    partial class PlayerInfoForm
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
            this.QuestList = new System.Windows.Forms.ListView();
            this.Index = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.Status = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.label11 = new System.Windows.Forms.Label();
            this.DeleteQuestButton = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.NameTextBox = new System.Windows.Forms.TextBox();
            this.IndexTextBox = new System.Windows.Forms.TextBox();
            this.LevelTextBox = new System.Windows.Forms.TextBox();
            this.UpdateButton = new System.Windows.Forms.Button();
            this.KickButton = new System.Windows.Forms.Button();
            this.SendMessageTextBox = new System.Windows.Forms.TextBox();
            this.SendMessageButton = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.OpenAccountButton = new System.Windows.Forms.Button();
            this.SafeZoneButton = new System.Windows.Forms.Button();
            this.label9 = new System.Windows.Forms.Label();
            this.ChatBanExpiryTextBox = new System.Windows.Forms.TextBox();
            this.ChatBanButton = new System.Windows.Forms.Button();
            this.KillPetsButton = new System.Windows.Forms.Button();
            this.KillButton = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.CurrentMapLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.PKPointsLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.OnlineTimeLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.GoldLabel = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.CurrentIPLabel = new System.Windows.Forms.Label();
            this.groupBox4 = new System.Windows.Forms.GroupBox();
            this.GGConfirmGroup = new System.Windows.Forms.GroupBox();
            this.ConfirmOrder = new System.Windows.Forms.Button();
            this.SelectedGGLabel = new System.Windows.Forms.Label();
            this.SelectedPlayerLabel = new System.Windows.Forms.Label();
            this.GGAmountText = new System.Windows.Forms.TextBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.GGConfirmGroup.SuspendLayout();
            this.SuspendLayout();
            // 
            // QuestList
            // 
            this.QuestList.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Index,
            this.Status});
            this.QuestList.FullRowSelect = true;
            this.QuestList.GridLines = true;
            this.QuestList.HideSelection = false;
            this.QuestList.Location = new System.Drawing.Point(651, 39);
            this.QuestList.Margin = new System.Windows.Forms.Padding(3, 2, 3, 2);
            this.QuestList.Name = "QuestList";
            this.QuestList.Size = new System.Drawing.Size(311, 325);
            this.QuestList.TabIndex = 3;
            this.QuestList.UseCompatibleStateImageBehavior = false;
            this.QuestList.View = System.Windows.Forms.View.Details;
            this.QuestList.SelectedIndexChanged += new System.EventHandler(this.QuestList_SelectedIndexChanged);
            // 
            // Index
            // 
            this.Index.Text = "Index";
            this.Index.Width = 146;
            // 
            // Status
            // 
            this.Status.Text = "Status";
            this.Status.Width = 115;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(761, 15);
            this.label11.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(53, 17);
            this.label11.TabIndex = 25;
            this.label11.Text = "Quests";
            // 
            // DeleteQuestButton
            // 
            this.DeleteQuestButton.Location = new System.Drawing.Point(783, 372);
            this.DeleteQuestButton.Margin = new System.Windows.Forms.Padding(4);
            this.DeleteQuestButton.Name = "DeleteQuestButton";
            this.DeleteQuestButton.Size = new System.Drawing.Size(77, 28);
            this.DeleteQuestButton.TabIndex = 11;
            this.DeleteQuestButton.Text = "Delete";
            this.DeleteQuestButton.UseVisualStyleBackColor = true;
            this.DeleteQuestButton.Click += new System.EventHandler(this.DeleteQuestButton_Click);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(224, 96);
            this.button1.Margin = new System.Windows.Forms.Padding(4);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(132, 28);
            this.button1.TabIndex = 24;
            this.button1.Text = "Player Items";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 57);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(57, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(12, 25);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(53, 17);
            this.label2.TabIndex = 2;
            this.label2.Text = "Index : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 91);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(54, 17);
            this.label3.TabIndex = 3;
            this.label3.Text = "Level : ";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(80, 53);
            this.NameTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(132, 22);
            this.NameTextBox.TabIndex = 4;
            // 
            // IndexTextBox
            // 
            this.IndexTextBox.Enabled = false;
            this.IndexTextBox.Location = new System.Drawing.Point(80, 21);
            this.IndexTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.IndexTextBox.Name = "IndexTextBox";
            this.IndexTextBox.Size = new System.Drawing.Size(132, 22);
            this.IndexTextBox.TabIndex = 5;
            // 
            // LevelTextBox
            // 
            this.LevelTextBox.Location = new System.Drawing.Point(80, 87);
            this.LevelTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.LevelTextBox.Name = "LevelTextBox";
            this.LevelTextBox.Size = new System.Drawing.Size(132, 22);
            this.LevelTextBox.TabIndex = 6;
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(116, 119);
            this.UpdateButton.Margin = new System.Windows.Forms.Padding(4);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(100, 28);
            this.UpdateButton.TabIndex = 7;
            this.UpdateButton.Text = "Update Details";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // KickButton
            // 
            this.KickButton.Location = new System.Drawing.Point(8, 23);
            this.KickButton.Margin = new System.Windows.Forms.Padding(4);
            this.KickButton.Name = "KickButton";
            this.KickButton.Size = new System.Drawing.Size(100, 28);
            this.KickButton.TabIndex = 8;
            this.KickButton.Text = "Kick Player";
            this.KickButton.UseVisualStyleBackColor = true;
            this.KickButton.Click += new System.EventHandler(this.KickButton_Click);
            // 
            // SendMessageTextBox
            // 
            this.SendMessageTextBox.Location = new System.Drawing.Point(8, 23);
            this.SendMessageTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.SendMessageTextBox.Name = "SendMessageTextBox";
            this.SendMessageTextBox.Size = new System.Drawing.Size(396, 22);
            this.SendMessageTextBox.TabIndex = 9;
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Location = new System.Drawing.Point(413, 21);
            this.SendMessageButton.Margin = new System.Windows.Forms.Padding(4);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(77, 28);
            this.SendMessageButton.TabIndex = 10;
            this.SendMessageButton.Text = "Send";
            this.SendMessageButton.UseVisualStyleBackColor = true;
            this.SendMessageButton.Click += new System.EventHandler(this.SendMessageButton_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.label3);
            this.groupBox1.Controls.Add(this.NameTextBox);
            this.groupBox1.Controls.Add(this.IndexTextBox);
            this.groupBox1.Controls.Add(this.UpdateButton);
            this.groupBox1.Controls.Add(this.LevelTextBox);
            this.groupBox1.Location = new System.Drawing.Point(8, 15);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox1.Size = new System.Drawing.Size(224, 167);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Character Info";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.OpenAccountButton);
            this.groupBox2.Controls.Add(this.SafeZoneButton);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.ChatBanExpiryTextBox);
            this.groupBox2.Controls.Add(this.ChatBanButton);
            this.groupBox2.Controls.Add(this.KillPetsButton);
            this.groupBox2.Controls.Add(this.KillButton);
            this.groupBox2.Controls.Add(this.KickButton);
            this.groupBox2.Location = new System.Drawing.Point(240, 15);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox2.Size = new System.Drawing.Size(371, 167);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Actions";
            // 
            // OpenAccountButton
            // 
            this.OpenAccountButton.Location = new System.Drawing.Point(224, 23);
            this.OpenAccountButton.Margin = new System.Windows.Forms.Padding(4);
            this.OpenAccountButton.Name = "OpenAccountButton";
            this.OpenAccountButton.Size = new System.Drawing.Size(132, 28);
            this.OpenAccountButton.TabIndex = 23;
            this.OpenAccountButton.Text = "Open Account";
            this.OpenAccountButton.UseVisualStyleBackColor = true;
            this.OpenAccountButton.Click += new System.EventHandler(this.OpenAccountButton_Click);
            // 
            // SafeZoneButton
            // 
            this.SafeZoneButton.Location = new System.Drawing.Point(116, 23);
            this.SafeZoneButton.Margin = new System.Windows.Forms.Padding(4);
            this.SafeZoneButton.Name = "SafeZoneButton";
            this.SafeZoneButton.Size = new System.Drawing.Size(100, 28);
            this.SafeZoneButton.TabIndex = 22;
            this.SafeZoneButton.Text = "Safezone";
            this.SafeZoneButton.UseVisualStyleBackColor = true;
            this.SafeZoneButton.Click += new System.EventHandler(this.SafeZoneButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(124, 135);
            this.label9.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(66, 17);
            this.label9.TabIndex = 21;
            this.label9.Text = "Expires : ";
            // 
            // ChatBanExpiryTextBox
            // 
            this.ChatBanExpiryTextBox.Location = new System.Drawing.Point(199, 132);
            this.ChatBanExpiryTextBox.Margin = new System.Windows.Forms.Padding(4);
            this.ChatBanExpiryTextBox.Name = "ChatBanExpiryTextBox";
            this.ChatBanExpiryTextBox.Size = new System.Drawing.Size(156, 22);
            this.ChatBanExpiryTextBox.TabIndex = 20;
            this.ChatBanExpiryTextBox.TextChanged += new System.EventHandler(this.ChatBanExpiryTextBox_TextChanged);
            // 
            // ChatBanButton
            // 
            this.ChatBanButton.Location = new System.Drawing.Point(8, 129);
            this.ChatBanButton.Margin = new System.Windows.Forms.Padding(4);
            this.ChatBanButton.Name = "ChatBanButton";
            this.ChatBanButton.Size = new System.Drawing.Size(100, 28);
            this.ChatBanButton.TabIndex = 19;
            this.ChatBanButton.Text = "Chat Ban";
            this.ChatBanButton.UseVisualStyleBackColor = true;
            this.ChatBanButton.Click += new System.EventHandler(this.ChatBanButton_Click);
            // 
            // KillPetsButton
            // 
            this.KillPetsButton.Location = new System.Drawing.Point(8, 95);
            this.KillPetsButton.Margin = new System.Windows.Forms.Padding(4);
            this.KillPetsButton.Name = "KillPetsButton";
            this.KillPetsButton.Size = new System.Drawing.Size(100, 28);
            this.KillPetsButton.TabIndex = 18;
            this.KillPetsButton.Text = "Kill Pets";
            this.KillPetsButton.UseVisualStyleBackColor = true;
            this.KillPetsButton.Click += new System.EventHandler(this.KillPetsButton_Click);
            // 
            // KillButton
            // 
            this.KillButton.Location = new System.Drawing.Point(8, 59);
            this.KillButton.Margin = new System.Windows.Forms.Padding(4);
            this.KillButton.Name = "KillButton";
            this.KillButton.Size = new System.Drawing.Size(100, 28);
            this.KillButton.TabIndex = 17;
            this.KillButton.Text = "Kill Player";
            this.KillButton.UseVisualStyleBackColor = true;
            this.KillButton.Click += new System.EventHandler(this.KillButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SendMessageTextBox);
            this.groupBox3.Controls.Add(this.SendMessageButton);
            this.groupBox3.Location = new System.Drawing.Point(8, 336);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(499, 60);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Send Message";
            // 
            // CurrentMapLabel
            // 
            this.CurrentMapLabel.AutoSize = true;
            this.CurrentMapLabel.Location = new System.Drawing.Point(151, 38);
            this.CurrentMapLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CurrentMapLabel.Name = "CurrentMapLabel";
            this.CurrentMapLabel.Size = new System.Drawing.Size(43, 17);
            this.CurrentMapLabel.TabIndex = 15;
            this.CurrentMapLabel.Text = "$map";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(13, 38);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(125, 17);
            this.label5.TabIndex = 16;
            this.label5.Text = "Current Location : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 59);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(81, 17);
            this.label4.TabIndex = 17;
            this.label4.Text = "PK Points : ";
            // 
            // PKPointsLabel
            // 
            this.PKPointsLabel.AutoSize = true;
            this.PKPointsLabel.Location = new System.Drawing.Point(151, 59);
            this.PKPointsLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.PKPointsLabel.Name = "PKPointsLabel";
            this.PKPointsLabel.Size = new System.Drawing.Size(62, 17);
            this.PKPointsLabel.TabIndex = 18;
            this.PKPointsLabel.Text = "$pkpoint";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(13, 80);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(96, 17);
            this.label6.TabIndex = 19;
            this.label6.Text = "Online Time : ";
            // 
            // OnlineTimeLabel
            // 
            this.OnlineTimeLabel.AutoSize = true;
            this.OnlineTimeLabel.Location = new System.Drawing.Point(151, 80);
            this.OnlineTimeLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.OnlineTimeLabel.Name = "OnlineTimeLabel";
            this.OnlineTimeLabel.Size = new System.Drawing.Size(80, 17);
            this.OnlineTimeLabel.TabIndex = 20;
            this.OnlineTimeLabel.Text = "$onlinetime";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(13, 20);
            this.label7.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 17);
            this.label7.TabIndex = 21;
            this.label7.Text = "Total Gold : ";
            // 
            // GoldLabel
            // 
            this.GoldLabel.AutoSize = true;
            this.GoldLabel.Location = new System.Drawing.Point(151, 20);
            this.GoldLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.GoldLabel.Name = "GoldLabel";
            this.GoldLabel.Size = new System.Drawing.Size(43, 17);
            this.GoldLabel.TabIndex = 22;
            this.GoldLabel.Text = "$gold";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(13, 101);
            this.label8.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(83, 17);
            this.label8.TabIndex = 23;
            this.label8.Text = "Current IP : ";
            // 
            // CurrentIPLabel
            // 
            this.CurrentIPLabel.AutoSize = true;
            this.CurrentIPLabel.Location = new System.Drawing.Point(151, 101);
            this.CurrentIPLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CurrentIPLabel.Name = "CurrentIPLabel";
            this.CurrentIPLabel.Size = new System.Drawing.Size(28, 17);
            this.CurrentIPLabel.TabIndex = 24;
            this.CurrentIPLabel.Text = "$IP";
            // 
            // groupBox4
            // 
            this.groupBox4.Controls.Add(this.label7);
            this.groupBox4.Controls.Add(this.CurrentIPLabel);
            this.groupBox4.Controls.Add(this.CurrentMapLabel);
            this.groupBox4.Controls.Add(this.label8);
            this.groupBox4.Controls.Add(this.label5);
            this.groupBox4.Controls.Add(this.GoldLabel);
            this.groupBox4.Controls.Add(this.label4);
            this.groupBox4.Controls.Add(this.PKPointsLabel);
            this.groupBox4.Controls.Add(this.OnlineTimeLabel);
            this.groupBox4.Controls.Add(this.label6);
            this.groupBox4.Location = new System.Drawing.Point(8, 190);
            this.groupBox4.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox4.Size = new System.Drawing.Size(263, 139);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Details";
            // 
            // GGConfirmGroup
            // 
            this.GGConfirmGroup.Controls.Add(this.ConfirmOrder);
            this.GGConfirmGroup.Controls.Add(this.SelectedGGLabel);
            this.GGConfirmGroup.Controls.Add(this.SelectedPlayerLabel);
            this.GGConfirmGroup.Controls.Add(this.GGAmountText);
            this.GGConfirmGroup.Enabled = false;
            this.GGConfirmGroup.Location = new System.Drawing.Point(8, 403);
            this.GGConfirmGroup.Name = "GGConfirmGroup";
            this.GGConfirmGroup.Size = new System.Drawing.Size(249, 150);
            this.GGConfirmGroup.TabIndex = 27;
            this.GGConfirmGroup.TabStop = false;
            this.GGConfirmGroup.Text = "GameGold Orders";
            // 
            // ConfirmOrder
            // 
            this.ConfirmOrder.Location = new System.Drawing.Point(6, 83);
            this.ConfirmOrder.Name = "ConfirmOrder";
            this.ConfirmOrder.Size = new System.Drawing.Size(237, 55);
            this.ConfirmOrder.TabIndex = 3;
            this.ConfirmOrder.Text = "Confirm Order";
            this.ConfirmOrder.UseVisualStyleBackColor = true;
            this.ConfirmOrder.Click += new System.EventHandler(this.ConfirmOrder_Click);
            // 
            // SelectedGGLabel
            // 
            this.SelectedGGLabel.AutoSize = true;
            this.SelectedGGLabel.Location = new System.Drawing.Point(9, 46);
            this.SelectedGGLabel.Name = "SelectedGGLabel";
            this.SelectedGGLabel.Size = new System.Drawing.Size(72, 17);
            this.SelectedGGLabel.TabIndex = 2;
            this.SelectedGGLabel.Text = "Amount: 0";
            // 
            // SelectedPlayerLabel
            // 
            this.SelectedPlayerLabel.AutoSize = true;
            this.SelectedPlayerLabel.Location = new System.Drawing.Point(9, 64);
            this.SelectedPlayerLabel.Name = "SelectedPlayerLabel";
            this.SelectedPlayerLabel.Size = new System.Drawing.Size(101, 17);
            this.SelectedPlayerLabel.TabIndex = 1;
            this.SelectedPlayerLabel.Text = "Player: Not set";
            // 
            // GGAmountText
            // 
            this.GGAmountText.Location = new System.Drawing.Point(6, 19);
            this.GGAmountText.Name = "GGAmountText";
            this.GGAmountText.Size = new System.Drawing.Size(237, 22);
            this.GGAmountText.TabIndex = 0;
            this.GGAmountText.TextChanged += new System.EventHandler(this.GGAmountText_TextChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(274, 403);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 21);
            this.checkBox1.TabIndex = 28;
            this.checkBox1.Text = "Enable";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // PlayerInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(970, 562);
            this.Controls.Add(this.checkBox1);
            this.Controls.Add(this.GGConfirmGroup);
            this.Controls.Add(this.DeleteQuestButton);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.QuestList);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "PlayerInfoForm";
            this.Text = "PlayerInfoForm";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox4.ResumeLayout(false);
            this.groupBox4.PerformLayout();
            this.GGConfirmGroup.ResumeLayout(false);
            this.GGConfirmGroup.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox NameTextBox;
        private System.Windows.Forms.TextBox IndexTextBox;
        private System.Windows.Forms.TextBox LevelTextBox;
        private System.Windows.Forms.Button UpdateButton;
        private System.Windows.Forms.Button KickButton;
        private System.Windows.Forms.TextBox SendMessageTextBox;
        private System.Windows.Forms.Button SendMessageButton;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Button KillButton;
        private System.Windows.Forms.Label CurrentMapLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button KillPetsButton;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label PKPointsLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label OnlineTimeLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label GoldLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label CurrentIPLabel;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.Button ChatBanButton;
        private System.Windows.Forms.TextBox ChatBanExpiryTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button SafeZoneButton;
        private System.Windows.Forms.Button OpenAccountButton;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ListView QuestList;
        private System.Windows.Forms.ColumnHeader Index;
        private System.Windows.Forms.ColumnHeader Status;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button DeleteQuestButton;
        private System.Windows.Forms.GroupBox GGConfirmGroup;
        private System.Windows.Forms.Button ConfirmOrder;
        private System.Windows.Forms.Label SelectedGGLabel;
        private System.Windows.Forms.Label SelectedPlayerLabel;
        private System.Windows.Forms.TextBox GGAmountText;
        private System.Windows.Forms.CheckBox checkBox1;
    }
}