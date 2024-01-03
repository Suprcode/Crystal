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
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            NameTextBox = new TextBox();
            IndexTextBox = new TextBox();
            LevelTextBox = new TextBox();
            UpdateButton = new Button();
            KickButton = new Button();
            SendMessageTextBox = new TextBox();
            SendMessageButton = new Button();
            groupBox1 = new GroupBox();
            groupBox2 = new GroupBox();
            OpenAccountButton = new Button();
            SafeZoneButton = new Button();
            label9 = new Label();
            ChatBanExpiryTextBox = new TextBox();
            ChatBanButton = new Button();
            KillPetsButton = new Button();
            KillButton = new Button();
            groupBox3 = new GroupBox();
            CurrentMapLabel = new Label();
            label5 = new Label();
            label4 = new Label();
            PKPointsLabel = new Label();
            label6 = new Label();
            OnlineTimeLabel = new Label();
            label7 = new Label();
            GoldLabel = new Label();
            label8 = new Label();
            CurrentIPLabel = new Label();
            groupBox4 = new GroupBox();
            ResultLabel = new Label();
            FlagSearchBox = new TextBox();
            FlagSearch = new Label();
            FlagUp = new Button();
            FlagDown = new Button();
            QuestDown = new Button();
            QuestUp = new Button();
            QuestResultLabel = new Label();
            QuestSearchBox = new TextBox();
            label10 = new Label();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBox4.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(8, 53);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(48, 15);
            label1.TabIndex = 1;
            label1.Text = "Name : ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(10, 23);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(45, 15);
            label2.TabIndex = 2;
            label2.Text = "Index : ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(10, 85);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(43, 15);
            label3.TabIndex = 3;
            label3.Text = "Level : ";
            // 
            // NameTextBox
            // 
            NameTextBox.Location = new Point(70, 50);
            NameTextBox.Margin = new Padding(4, 3, 4, 3);
            NameTextBox.Name = "NameTextBox";
            NameTextBox.Size = new Size(116, 23);
            NameTextBox.TabIndex = 4;
            // 
            // IndexTextBox
            // 
            IndexTextBox.Enabled = false;
            IndexTextBox.Location = new Point(70, 20);
            IndexTextBox.Margin = new Padding(4, 3, 4, 3);
            IndexTextBox.Name = "IndexTextBox";
            IndexTextBox.Size = new Size(116, 23);
            IndexTextBox.TabIndex = 5;
            // 
            // LevelTextBox
            // 
            LevelTextBox.Location = new Point(70, 82);
            LevelTextBox.Margin = new Padding(4, 3, 4, 3);
            LevelTextBox.Name = "LevelTextBox";
            LevelTextBox.Size = new Size(116, 23);
            LevelTextBox.TabIndex = 6;
            // 
            // UpdateButton
            // 
            UpdateButton.Location = new Point(102, 112);
            UpdateButton.Margin = new Padding(4, 3, 4, 3);
            UpdateButton.Name = "UpdateButton";
            UpdateButton.Size = new Size(88, 27);
            UpdateButton.TabIndex = 7;
            UpdateButton.Text = "Update Details";
            UpdateButton.UseVisualStyleBackColor = true;
            UpdateButton.Click += UpdateButton_Click;
            // 
            // KickButton
            // 
            KickButton.Location = new Point(7, 22);
            KickButton.Margin = new Padding(4, 3, 4, 3);
            KickButton.Name = "KickButton";
            KickButton.Size = new Size(88, 27);
            KickButton.TabIndex = 8;
            KickButton.Text = "Kick Player";
            KickButton.UseVisualStyleBackColor = true;
            KickButton.Click += KickButton_Click;
            // 
            // SendMessageTextBox
            // 
            SendMessageTextBox.Location = new Point(7, 22);
            SendMessageTextBox.Margin = new Padding(4, 3, 4, 3);
            SendMessageTextBox.Name = "SendMessageTextBox";
            SendMessageTextBox.Size = new Size(347, 23);
            SendMessageTextBox.TabIndex = 9;
            // 
            // SendMessageButton
            // 
            SendMessageButton.Location = new Point(362, 20);
            SendMessageButton.Margin = new Padding(4, 3, 4, 3);
            SendMessageButton.Name = "SendMessageButton";
            SendMessageButton.Size = new Size(68, 27);
            SendMessageButton.TabIndex = 10;
            SendMessageButton.Text = "Send";
            SendMessageButton.UseVisualStyleBackColor = true;
            SendMessageButton.Click += SendMessageButton_Click;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(label2);
            groupBox1.Controls.Add(label1);
            groupBox1.Controls.Add(label3);
            groupBox1.Controls.Add(NameTextBox);
            groupBox1.Controls.Add(IndexTextBox);
            groupBox1.Controls.Add(UpdateButton);
            groupBox1.Controls.Add(LevelTextBox);
            groupBox1.Location = new Point(7, 14);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(196, 157);
            groupBox1.TabIndex = 11;
            groupBox1.TabStop = false;
            groupBox1.Text = "Character Info";
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(OpenAccountButton);
            groupBox2.Controls.Add(SafeZoneButton);
            groupBox2.Controls.Add(label9);
            groupBox2.Controls.Add(ChatBanExpiryTextBox);
            groupBox2.Controls.Add(ChatBanButton);
            groupBox2.Controls.Add(KillPetsButton);
            groupBox2.Controls.Add(KillButton);
            groupBox2.Controls.Add(KickButton);
            groupBox2.Location = new Point(210, 14);
            groupBox2.Margin = new Padding(4, 3, 4, 3);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new Padding(4, 3, 4, 3);
            groupBox2.Size = new Size(324, 157);
            groupBox2.TabIndex = 12;
            groupBox2.TabStop = false;
            groupBox2.Text = "Actions";
            // 
            // OpenAccountButton
            // 
            OpenAccountButton.Location = new Point(196, 22);
            OpenAccountButton.Margin = new Padding(4, 3, 4, 3);
            OpenAccountButton.Name = "OpenAccountButton";
            OpenAccountButton.Size = new Size(115, 27);
            OpenAccountButton.TabIndex = 23;
            OpenAccountButton.Text = "Open Account";
            OpenAccountButton.UseVisualStyleBackColor = true;
            OpenAccountButton.Click += OpenAccountButton_Click;
            // 
            // SafeZoneButton
            // 
            SafeZoneButton.Location = new Point(102, 22);
            SafeZoneButton.Margin = new Padding(4, 3, 4, 3);
            SafeZoneButton.Name = "SafeZoneButton";
            SafeZoneButton.Size = new Size(88, 27);
            SafeZoneButton.TabIndex = 22;
            SafeZoneButton.Text = "Safezone";
            SafeZoneButton.UseVisualStyleBackColor = true;
            SafeZoneButton.Click += SafeZoneButton_Click;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(108, 127);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(53, 15);
            label9.TabIndex = 21;
            label9.Text = "Expires : ";
            // 
            // ChatBanExpiryTextBox
            // 
            ChatBanExpiryTextBox.Location = new Point(174, 123);
            ChatBanExpiryTextBox.Margin = new Padding(4, 3, 4, 3);
            ChatBanExpiryTextBox.Name = "ChatBanExpiryTextBox";
            ChatBanExpiryTextBox.Size = new Size(137, 23);
            ChatBanExpiryTextBox.TabIndex = 20;
            ChatBanExpiryTextBox.TextChanged += ChatBanExpiryTextBox_TextChanged;
            // 
            // ChatBanButton
            // 
            ChatBanButton.Location = new Point(7, 121);
            ChatBanButton.Margin = new Padding(4, 3, 4, 3);
            ChatBanButton.Name = "ChatBanButton";
            ChatBanButton.Size = new Size(88, 27);
            ChatBanButton.TabIndex = 19;
            ChatBanButton.Text = "Chat Ban";
            ChatBanButton.UseVisualStyleBackColor = true;
            ChatBanButton.Click += ChatBanButton_Click;
            // 
            // KillPetsButton
            // 
            KillPetsButton.Location = new Point(7, 89);
            KillPetsButton.Margin = new Padding(4, 3, 4, 3);
            KillPetsButton.Name = "KillPetsButton";
            KillPetsButton.Size = new Size(88, 27);
            KillPetsButton.TabIndex = 18;
            KillPetsButton.Text = "Kill Pets";
            KillPetsButton.UseVisualStyleBackColor = true;
            KillPetsButton.Click += KillPetsButton_Click;
            // 
            // KillButton
            // 
            KillButton.Location = new Point(7, 55);
            KillButton.Margin = new Padding(4, 3, 4, 3);
            KillButton.Name = "KillButton";
            KillButton.Size = new Size(88, 27);
            KillButton.TabIndex = 17;
            KillButton.Text = "Kill Player";
            KillButton.UseVisualStyleBackColor = true;
            KillButton.Click += KillButton_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(SendMessageTextBox);
            groupBox3.Controls.Add(SendMessageButton);
            groupBox3.Location = new Point(7, 315);
            groupBox3.Margin = new Padding(4, 3, 4, 3);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new Padding(4, 3, 4, 3);
            groupBox3.Size = new Size(436, 57);
            groupBox3.TabIndex = 13;
            groupBox3.TabStop = false;
            groupBox3.Text = "Send Message";
            // 
            // CurrentMapLabel
            // 
            CurrentMapLabel.AutoSize = true;
            CurrentMapLabel.Location = new Point(132, 36);
            CurrentMapLabel.Margin = new Padding(4, 0, 4, 0);
            CurrentMapLabel.Name = "CurrentMapLabel";
            CurrentMapLabel.Size = new Size(37, 15);
            CurrentMapLabel.TabIndex = 15;
            CurrentMapLabel.Text = "$map";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 36);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(105, 15);
            label5.TabIndex = 16;
            label5.Text = "Current Location : ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(12, 55);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(66, 15);
            label4.TabIndex = 17;
            label4.Text = "PK Points : ";
            // 
            // PKPointsLabel
            // 
            PKPointsLabel.AutoSize = true;
            PKPointsLabel.Location = new Point(132, 55);
            PKPointsLabel.Margin = new Padding(4, 0, 4, 0);
            PKPointsLabel.Name = "PKPointsLabel";
            PKPointsLabel.Size = new Size(54, 15);
            PKPointsLabel.TabIndex = 18;
            PKPointsLabel.Text = "$pkpoint";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(12, 75);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(80, 15);
            label6.TabIndex = 19;
            label6.Text = "Online Time : ";
            // 
            // OnlineTimeLabel
            // 
            OnlineTimeLabel.AutoSize = true;
            OnlineTimeLabel.Location = new Point(132, 75);
            OnlineTimeLabel.Margin = new Padding(4, 0, 4, 0);
            OnlineTimeLabel.Name = "OnlineTimeLabel";
            OnlineTimeLabel.Size = new Size(70, 15);
            OnlineTimeLabel.TabIndex = 20;
            OnlineTimeLabel.Text = "$onlinetime";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 18);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(69, 15);
            label7.TabIndex = 21;
            label7.Text = "Total Gold : ";
            // 
            // GoldLabel
            // 
            GoldLabel.AutoSize = true;
            GoldLabel.Location = new Point(132, 18);
            GoldLabel.Margin = new Padding(4, 0, 4, 0);
            GoldLabel.Name = "GoldLabel";
            GoldLabel.Size = new Size(37, 15);
            GoldLabel.TabIndex = 22;
            GoldLabel.Text = "$gold";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(12, 95);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(69, 15);
            label8.TabIndex = 23;
            label8.Text = "Current IP : ";
            // 
            // CurrentIPLabel
            // 
            CurrentIPLabel.AutoSize = true;
            CurrentIPLabel.Location = new Point(132, 95);
            CurrentIPLabel.Margin = new Padding(4, 0, 4, 0);
            CurrentIPLabel.Name = "CurrentIPLabel";
            CurrentIPLabel.Size = new Size(23, 15);
            CurrentIPLabel.TabIndex = 24;
            CurrentIPLabel.Text = "$IP";
            CurrentIPLabel.Click += CurrentIPLabel_Click;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label7);
            groupBox4.Controls.Add(CurrentIPLabel);
            groupBox4.Controls.Add(CurrentMapLabel);
            groupBox4.Controls.Add(label8);
            groupBox4.Controls.Add(label5);
            groupBox4.Controls.Add(GoldLabel);
            groupBox4.Controls.Add(label4);
            groupBox4.Controls.Add(PKPointsLabel);
            groupBox4.Controls.Add(OnlineTimeLabel);
            groupBox4.Controls.Add(label6);
            groupBox4.Location = new Point(7, 178);
            groupBox4.Margin = new Padding(4, 3, 4, 3);
            groupBox4.Name = "groupBox4";
            groupBox4.Padding = new Padding(4, 3, 4, 3);
            groupBox4.Size = new Size(364, 130);
            groupBox4.TabIndex = 25;
            groupBox4.TabStop = false;
            groupBox4.Text = "Details";
            // 
            // ResultLabel
            // 
            ResultLabel.AutoSize = true;
            ResultLabel.Location = new Point(403, 218);
            ResultLabel.Name = "ResultLabel";
            ResultLabel.Size = new Size(0, 15);
            ResultLabel.TabIndex = 30;
            // 
            // FlagSearchBox
            // 
            FlagSearchBox.Location = new Point(403, 192);
            FlagSearchBox.Name = "FlagSearchBox";
            FlagSearchBox.Size = new Size(100, 23);
            FlagSearchBox.TabIndex = 29;
            FlagSearchBox.TextChanged += FlagSearchBox_TextChanged;
            // 
            // FlagSearch
            // 
            FlagSearch.AutoSize = true;
            FlagSearch.Location = new Point(421, 174);
            FlagSearch.Name = "FlagSearch";
            FlagSearch.Size = new Size(67, 15);
            FlagSearch.TabIndex = 28;
            FlagSearch.Text = "Flag Search";
            // 
            // FlagUp
            // 
            FlagUp.Location = new Point(510, 192);
            FlagUp.Name = "FlagUp";
            FlagUp.Size = new Size(24, 23);
            FlagUp.TabIndex = 31;
            FlagUp.Text = "+";
            FlagUp.UseVisualStyleBackColor = true;
            FlagUp.Click += FlagUp_Click;
            // 
            // FlagDown
            // 
            FlagDown.Location = new Point(373, 192);
            FlagDown.Name = "FlagDown";
            FlagDown.Size = new Size(24, 23);
            FlagDown.TabIndex = 32;
            FlagDown.Text = "-";
            FlagDown.UseVisualStyleBackColor = true;
            FlagDown.Click += FlagDown_Click;
            // 
            // QuestDown
            // 
            QuestDown.Location = new Point(372, 269);
            QuestDown.Name = "QuestDown";
            QuestDown.Size = new Size(24, 23);
            QuestDown.TabIndex = 44;
            QuestDown.Text = "-";
            QuestDown.UseVisualStyleBackColor = true;
            QuestDown.Click += QuestDown_Click;
            // 
            // QuestUp
            // 
            QuestUp.Location = new Point(509, 269);
            QuestUp.Name = "QuestUp";
            QuestUp.Size = new Size(24, 23);
            QuestUp.TabIndex = 43;
            QuestUp.Text = "+";
            QuestUp.UseVisualStyleBackColor = true;
            QuestUp.Click += QuestUp_Click;
            // 
            // QuestResultLabel
            // 
            QuestResultLabel.AutoSize = true;
            QuestResultLabel.Location = new Point(403, 296);
            QuestResultLabel.Name = "QuestResultLabel";
            QuestResultLabel.Size = new Size(0, 15);
            QuestResultLabel.TabIndex = 42;
            // 
            // QuestSearchBox
            // 
            QuestSearchBox.Location = new Point(403, 270);
            QuestSearchBox.Name = "QuestSearchBox";
            QuestSearchBox.Size = new Size(100, 23);
            QuestSearchBox.TabIndex = 41;
            QuestSearchBox.TextChanged += QuestSearchBox_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(421, 252);
            label10.Name = "label10";
            label10.Size = new Size(76, 15);
            label10.TabIndex = 40;
            label10.Text = "Quest Search";
            // 
            // PlayerInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(544, 378);
            Controls.Add(QuestDown);
            Controls.Add(QuestUp);
            Controls.Add(QuestResultLabel);
            Controls.Add(QuestSearchBox);
            Controls.Add(label10);
            Controls.Add(FlagDown);
            Controls.Add(FlagUp);
            Controls.Add(ResultLabel);
            Controls.Add(FlagSearchBox);
            Controls.Add(FlagSearch);
            Controls.Add(groupBox4);
            Controls.Add(groupBox3);
            Controls.Add(groupBox2);
            Controls.Add(groupBox1);
            Margin = new Padding(4, 3, 4, 3);
            Name = "PlayerInfoForm";
            Text = "PlayerInfoForm";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private Label ResultLabel;
        private TextBox FlagSearchBox;
        private Label FlagSearch;
        private Button FlagUp;
        private Button FlagDown;
        private Button QuestDown;
        private Button QuestUp;
        private Label QuestResultLabel;
        private TextBox QuestSearchBox;
        private Label label10;
    }
}