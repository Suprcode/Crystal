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
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 42);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 12);
            this.label1.TabIndex = 1;
            this.label1.Text = "名字 : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(47, 12);
            this.label2.TabIndex = 2;
            this.label2.Text = "索引 : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 68);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(47, 12);
            this.label3.TabIndex = 3;
            this.label3.Text = "等级 : ";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(60, 40);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(100, 21);
            this.NameTextBox.TabIndex = 4;
            // 
            // IndexTextBox
            // 
            this.IndexTextBox.Enabled = false;
            this.IndexTextBox.Location = new System.Drawing.Point(60, 16);
            this.IndexTextBox.Name = "IndexTextBox";
            this.IndexTextBox.Size = new System.Drawing.Size(100, 21);
            this.IndexTextBox.TabIndex = 5;
            // 
            // LevelTextBox
            // 
            this.LevelTextBox.Location = new System.Drawing.Point(60, 66);
            this.LevelTextBox.Name = "LevelTextBox";
            this.LevelTextBox.Size = new System.Drawing.Size(100, 21);
            this.LevelTextBox.TabIndex = 6;
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(87, 90);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(75, 21);
            this.UpdateButton.TabIndex = 7;
            this.UpdateButton.Text = "更新";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // KickButton
            // 
            this.KickButton.Location = new System.Drawing.Point(6, 18);
            this.KickButton.Name = "KickButton";
            this.KickButton.Size = new System.Drawing.Size(75, 21);
            this.KickButton.TabIndex = 8;
            this.KickButton.Text = "踢下线";
            this.KickButton.UseVisualStyleBackColor = true;
            this.KickButton.Click += new System.EventHandler(this.KickButton_Click);
            // 
            // SendMessageTextBox
            // 
            this.SendMessageTextBox.Location = new System.Drawing.Point(6, 18);
            this.SendMessageTextBox.Name = "SendMessageTextBox";
            this.SendMessageTextBox.Size = new System.Drawing.Size(298, 21);
            this.SendMessageTextBox.TabIndex = 9;
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Location = new System.Drawing.Point(310, 18);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(58, 21);
            this.SendMessageButton.TabIndex = 10;
            this.SendMessageButton.Text = "发送";
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
            this.groupBox1.Location = new System.Drawing.Point(6, 11);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 126);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "人物信息";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.OpenAccountButton);
            this.groupBox2.Controls.Add(this.SafeZoneButton);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.ChatBanExpiryTextBox);
            this.groupBox2.Controls.Add(this.ChatBanButton);
            this.groupBox2.Controls.Add(this.KillPetsButton);
            this.groupBox2.Controls.Add(this.KillButton);
            this.groupBox2.Controls.Add(this.KickButton);
            this.groupBox2.Location = new System.Drawing.Point(180, 11);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 126);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Actions";
            // 
            // OpenAccountButton
            // 
            this.OpenAccountButton.Location = new System.Drawing.Point(168, 18);
            this.OpenAccountButton.Name = "OpenAccountButton";
            this.OpenAccountButton.Size = new System.Drawing.Size(99, 21);
            this.OpenAccountButton.TabIndex = 23;
            this.OpenAccountButton.Text = "打开账户";
            this.OpenAccountButton.UseVisualStyleBackColor = true;
            this.OpenAccountButton.Click += new System.EventHandler(this.OpenAccountButton_Click);
            // 
            // SafeZoneButton
            // 
            this.SafeZoneButton.Location = new System.Drawing.Point(87, 18);
            this.SafeZoneButton.Name = "SafeZoneButton";
            this.SafeZoneButton.Size = new System.Drawing.Size(75, 21);
            this.SafeZoneButton.TabIndex = 22;
            this.SafeZoneButton.Text = "安全区";
            this.SafeZoneButton.UseVisualStyleBackColor = true;
            this.SafeZoneButton.Click += new System.EventHandler(this.SafeZoneButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(93, 102);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(47, 12);
            this.label9.TabIndex = 21;
            this.label9.Text = "过期 : ";
            // 
            // ChatBanExpiryTextBox
            // 
            this.ChatBanExpiryTextBox.Location = new System.Drawing.Point(149, 99);
            this.ChatBanExpiryTextBox.Name = "ChatBanExpiryTextBox";
            this.ChatBanExpiryTextBox.Size = new System.Drawing.Size(118, 21);
            this.ChatBanExpiryTextBox.TabIndex = 20;
            this.ChatBanExpiryTextBox.TextChanged += new System.EventHandler(this.ChatBanExpiryTextBox_TextChanged);
            // 
            // ChatBanButton
            // 
            this.ChatBanButton.Location = new System.Drawing.Point(6, 97);
            this.ChatBanButton.Name = "ChatBanButton";
            this.ChatBanButton.Size = new System.Drawing.Size(75, 21);
            this.ChatBanButton.TabIndex = 19;
            this.ChatBanButton.Text = "禁止聊天";
            this.ChatBanButton.UseVisualStyleBackColor = true;
            this.ChatBanButton.Click += new System.EventHandler(this.ChatBanButton_Click);
            // 
            // KillPetsButton
            // 
            this.KillPetsButton.Location = new System.Drawing.Point(6, 71);
            this.KillPetsButton.Name = "KillPetsButton";
            this.KillPetsButton.Size = new System.Drawing.Size(75, 21);
            this.KillPetsButton.TabIndex = 18;
            this.KillPetsButton.Text = "杀死宠物";
            this.KillPetsButton.UseVisualStyleBackColor = true;
            this.KillPetsButton.Click += new System.EventHandler(this.KillPetsButton_Click);
            // 
            // KillButton
            // 
            this.KillButton.Location = new System.Drawing.Point(6, 44);
            this.KillButton.Name = "KillButton";
            this.KillButton.Size = new System.Drawing.Size(75, 21);
            this.KillButton.TabIndex = 17;
            this.KillButton.Text = "杀死";
            this.KillButton.UseVisualStyleBackColor = true;
            this.KillButton.Click += new System.EventHandler(this.KillButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SendMessageTextBox);
            this.groupBox3.Controls.Add(this.SendMessageButton);
            this.groupBox3.Location = new System.Drawing.Point(6, 252);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(374, 45);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "发送消息";
            // 
            // CurrentMapLabel
            // 
            this.CurrentMapLabel.AutoSize = true;
            this.CurrentMapLabel.Location = new System.Drawing.Point(113, 29);
            this.CurrentMapLabel.Name = "CurrentMapLabel";
            this.CurrentMapLabel.Size = new System.Drawing.Size(29, 12);
            this.CurrentMapLabel.TabIndex = 15;
            this.CurrentMapLabel.Text = "$map";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 29);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 12);
            this.label5.TabIndex = 16;
            this.label5.Text = "当前位置 : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(47, 12);
            this.label4.TabIndex = 17;
            this.label4.Text = "PK点 : ";
            // 
            // PKPointsLabel
            // 
            this.PKPointsLabel.AutoSize = true;
            this.PKPointsLabel.Location = new System.Drawing.Point(113, 44);
            this.PKPointsLabel.Name = "PKPointsLabel";
            this.PKPointsLabel.Size = new System.Drawing.Size(53, 12);
            this.PKPointsLabel.TabIndex = 18;
            this.PKPointsLabel.Text = "$pkpoint";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 60);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "在线时间 : ";
            // 
            // OnlineTimeLabel
            // 
            this.OnlineTimeLabel.AutoSize = true;
            this.OnlineTimeLabel.Location = new System.Drawing.Point(113, 60);
            this.OnlineTimeLabel.Name = "OnlineTimeLabel";
            this.OnlineTimeLabel.Size = new System.Drawing.Size(71, 12);
            this.OnlineTimeLabel.TabIndex = 20;
            this.OnlineTimeLabel.Text = "$onlinetime";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 15);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(59, 12);
            this.label7.TabIndex = 21;
            this.label7.Text = "总金币 : ";
            // 
            // GoldLabel
            // 
            this.GoldLabel.AutoSize = true;
            this.GoldLabel.Location = new System.Drawing.Point(113, 15);
            this.GoldLabel.Name = "GoldLabel";
            this.GoldLabel.Size = new System.Drawing.Size(35, 12);
            this.GoldLabel.TabIndex = 22;
            this.GoldLabel.Text = "$gold";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 76);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(65, 12);
            this.label8.TabIndex = 23;
            this.label8.Text = "当前 IP : ";
            // 
            // CurrentIPLabel
            // 
            this.CurrentIPLabel.AutoSize = true;
            this.CurrentIPLabel.Location = new System.Drawing.Point(113, 76);
            this.CurrentIPLabel.Name = "CurrentIPLabel";
            this.CurrentIPLabel.Size = new System.Drawing.Size(23, 12);
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
            this.groupBox4.Location = new System.Drawing.Point(6, 142);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(197, 104);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "详细";
            // 
            // PlayerInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 303);
            this.Controls.Add(this.groupBox4);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
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
            this.ResumeLayout(false);

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
    }
}