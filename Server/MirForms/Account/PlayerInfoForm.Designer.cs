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
            this.OpenAccountButton = new System.Windows.Forms.Button();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox4.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(7, 46);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(44, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(42, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Index : ";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 74);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Level : ";
            // 
            // NameTextBox
            // 
            this.NameTextBox.Location = new System.Drawing.Point(60, 43);
            this.NameTextBox.Name = "NameTextBox";
            this.NameTextBox.Size = new System.Drawing.Size(100, 20);
            this.NameTextBox.TabIndex = 4;
            // 
            // IndexTextBox
            // 
            this.IndexTextBox.Enabled = false;
            this.IndexTextBox.Location = new System.Drawing.Point(60, 17);
            this.IndexTextBox.Name = "IndexTextBox";
            this.IndexTextBox.Size = new System.Drawing.Size(100, 20);
            this.IndexTextBox.TabIndex = 5;
            // 
            // LevelTextBox
            // 
            this.LevelTextBox.Location = new System.Drawing.Point(60, 71);
            this.LevelTextBox.Name = "LevelTextBox";
            this.LevelTextBox.Size = new System.Drawing.Size(100, 20);
            this.LevelTextBox.TabIndex = 6;
            // 
            // UpdateButton
            // 
            this.UpdateButton.Location = new System.Drawing.Point(87, 97);
            this.UpdateButton.Name = "UpdateButton";
            this.UpdateButton.Size = new System.Drawing.Size(75, 23);
            this.UpdateButton.TabIndex = 7;
            this.UpdateButton.Text = "Update Details";
            this.UpdateButton.UseVisualStyleBackColor = true;
            this.UpdateButton.Click += new System.EventHandler(this.UpdateButton_Click);
            // 
            // KickButton
            // 
            this.KickButton.Location = new System.Drawing.Point(6, 19);
            this.KickButton.Name = "KickButton";
            this.KickButton.Size = new System.Drawing.Size(75, 23);
            this.KickButton.TabIndex = 8;
            this.KickButton.Text = "Kick Player";
            this.KickButton.UseVisualStyleBackColor = true;
            this.KickButton.Click += new System.EventHandler(this.KickButton_Click);
            // 
            // SendMessageTextBox
            // 
            this.SendMessageTextBox.Location = new System.Drawing.Point(6, 19);
            this.SendMessageTextBox.Name = "SendMessageTextBox";
            this.SendMessageTextBox.Size = new System.Drawing.Size(298, 20);
            this.SendMessageTextBox.TabIndex = 9;
            // 
            // SendMessageButton
            // 
            this.SendMessageButton.Location = new System.Drawing.Point(310, 17);
            this.SendMessageButton.Name = "SendMessageButton";
            this.SendMessageButton.Size = new System.Drawing.Size(58, 23);
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
            this.groupBox1.Location = new System.Drawing.Point(6, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(168, 136);
            this.groupBox1.TabIndex = 11;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Character Info";
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
            this.groupBox2.Location = new System.Drawing.Point(180, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(278, 136);
            this.groupBox2.TabIndex = 12;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Actions";
            // 
            // SafeZoneButton
            // 
            this.SafeZoneButton.Location = new System.Drawing.Point(87, 19);
            this.SafeZoneButton.Name = "SafeZoneButton";
            this.SafeZoneButton.Size = new System.Drawing.Size(75, 23);
            this.SafeZoneButton.TabIndex = 22;
            this.SafeZoneButton.Text = "Safezone";
            this.SafeZoneButton.UseVisualStyleBackColor = true;
            this.SafeZoneButton.Click += new System.EventHandler(this.SafeZoneButton_Click);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(93, 110);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(50, 13);
            this.label9.TabIndex = 21;
            this.label9.Text = "Expires : ";
            // 
            // ChatBanExpiryTextBox
            // 
            this.ChatBanExpiryTextBox.Location = new System.Drawing.Point(149, 107);
            this.ChatBanExpiryTextBox.Name = "ChatBanExpiryTextBox";
            this.ChatBanExpiryTextBox.Size = new System.Drawing.Size(118, 20);
            this.ChatBanExpiryTextBox.TabIndex = 20;
            this.ChatBanExpiryTextBox.TextChanged += new System.EventHandler(this.ChatBanExpiryTextBox_TextChanged);
            // 
            // ChatBanButton
            // 
            this.ChatBanButton.Location = new System.Drawing.Point(6, 105);
            this.ChatBanButton.Name = "ChatBanButton";
            this.ChatBanButton.Size = new System.Drawing.Size(75, 23);
            this.ChatBanButton.TabIndex = 19;
            this.ChatBanButton.Text = "Chat Ban";
            this.ChatBanButton.UseVisualStyleBackColor = true;
            this.ChatBanButton.Click += new System.EventHandler(this.ChatBanButton_Click);
            // 
            // KillPetsButton
            // 
            this.KillPetsButton.Location = new System.Drawing.Point(6, 77);
            this.KillPetsButton.Name = "KillPetsButton";
            this.KillPetsButton.Size = new System.Drawing.Size(75, 23);
            this.KillPetsButton.TabIndex = 18;
            this.KillPetsButton.Text = "Kill Pets";
            this.KillPetsButton.UseVisualStyleBackColor = true;
            this.KillPetsButton.Click += new System.EventHandler(this.KillPetsButton_Click);
            // 
            // KillButton
            // 
            this.KillButton.Location = new System.Drawing.Point(6, 48);
            this.KillButton.Name = "KillButton";
            this.KillButton.Size = new System.Drawing.Size(75, 23);
            this.KillButton.TabIndex = 17;
            this.KillButton.Text = "Kill Player";
            this.KillButton.UseVisualStyleBackColor = true;
            this.KillButton.Click += new System.EventHandler(this.KillButton_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.SendMessageTextBox);
            this.groupBox3.Controls.Add(this.SendMessageButton);
            this.groupBox3.Location = new System.Drawing.Point(6, 273);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(374, 49);
            this.groupBox3.TabIndex = 13;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Send Message";
            // 
            // CurrentMapLabel
            // 
            this.CurrentMapLabel.AutoSize = true;
            this.CurrentMapLabel.Location = new System.Drawing.Point(113, 31);
            this.CurrentMapLabel.Name = "CurrentMapLabel";
            this.CurrentMapLabel.Size = new System.Drawing.Size(33, 13);
            this.CurrentMapLabel.TabIndex = 15;
            this.CurrentMapLabel.Text = "$map";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 31);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(94, 13);
            this.label5.TabIndex = 16;
            this.label5.Text = "Current Location : ";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(10, 48);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(62, 13);
            this.label4.TabIndex = 17;
            this.label4.Text = "PK Points : ";
            // 
            // PKPointsLabel
            // 
            this.PKPointsLabel.AutoSize = true;
            this.PKPointsLabel.Location = new System.Drawing.Point(113, 48);
            this.PKPointsLabel.Name = "PKPointsLabel";
            this.PKPointsLabel.Size = new System.Drawing.Size(48, 13);
            this.PKPointsLabel.TabIndex = 18;
            this.PKPointsLabel.Text = "$pkpoint";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 65);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(72, 13);
            this.label6.TabIndex = 19;
            this.label6.Text = "Online Time : ";
            // 
            // OnlineTimeLabel
            // 
            this.OnlineTimeLabel.AutoSize = true;
            this.OnlineTimeLabel.Location = new System.Drawing.Point(113, 65);
            this.OnlineTimeLabel.Name = "OnlineTimeLabel";
            this.OnlineTimeLabel.Size = new System.Drawing.Size(60, 13);
            this.OnlineTimeLabel.TabIndex = 20;
            this.OnlineTimeLabel.Text = "$onlinetime";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(10, 16);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(65, 13);
            this.label7.TabIndex = 21;
            this.label7.Text = "Total Gold : ";
            // 
            // GoldLabel
            // 
            this.GoldLabel.AutoSize = true;
            this.GoldLabel.Location = new System.Drawing.Point(113, 16);
            this.GoldLabel.Name = "GoldLabel";
            this.GoldLabel.Size = new System.Drawing.Size(33, 13);
            this.GoldLabel.TabIndex = 22;
            this.GoldLabel.Text = "$gold";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(10, 82);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(63, 13);
            this.label8.TabIndex = 23;
            this.label8.Text = "Current IP : ";
            // 
            // CurrentIPLabel
            // 
            this.CurrentIPLabel.AutoSize = true;
            this.CurrentIPLabel.Location = new System.Drawing.Point(113, 82);
            this.CurrentIPLabel.Name = "CurrentIPLabel";
            this.CurrentIPLabel.Size = new System.Drawing.Size(23, 13);
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
            this.groupBox4.Location = new System.Drawing.Point(6, 154);
            this.groupBox4.Name = "groupBox4";
            this.groupBox4.Size = new System.Drawing.Size(197, 113);
            this.groupBox4.TabIndex = 25;
            this.groupBox4.TabStop = false;
            this.groupBox4.Text = "Details";
            // 
            // OpenAccountButton
            // 
            this.OpenAccountButton.Location = new System.Drawing.Point(168, 19);
            this.OpenAccountButton.Name = "OpenAccountButton";
            this.OpenAccountButton.Size = new System.Drawing.Size(99, 23);
            this.OpenAccountButton.TabIndex = 23;
            this.OpenAccountButton.Text = "Open Account";
            this.OpenAccountButton.UseVisualStyleBackColor = true;
            this.OpenAccountButton.Click += new System.EventHandler(this.OpenAccountButton_Click);
            // 
            // PlayerInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(466, 328);
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