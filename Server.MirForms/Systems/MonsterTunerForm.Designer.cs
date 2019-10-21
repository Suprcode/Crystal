namespace Server.MirForms.Systems
{
    partial class MonsterTunerForm
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
            this.SelectMonsterComboBox = new System.Windows.Forms.ComboBox();
            this.updateButton = new System.Windows.Forms.Button();
            this.CoolEyeTextBox = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.ViewRangeTextBox = new System.Windows.Forms.TextBox();
            this.label33 = new System.Windows.Forms.Label();
            this.MSpeedTextBox = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.ASpeedTextBox = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.LevelTextBox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.EffectTextBox = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.AgilityTextBox = new System.Windows.Forms.TextBox();
            this.label26 = new System.Windows.Forms.Label();
            this.AccuracyTextBox = new System.Windows.Forms.TextBox();
            this.label27 = new System.Windows.Forms.Label();
            this.HPTextBox = new System.Windows.Forms.TextBox();
            this.label25 = new System.Windows.Forms.Label();
            this.MaxSCTextBox = new System.Windows.Forms.TextBox();
            this.label22 = new System.Windows.Forms.Label();
            this.MinSCTextBox = new System.Windows.Forms.TextBox();
            this.label23 = new System.Windows.Forms.Label();
            this.MaxMCTextBox = new System.Windows.Forms.TextBox();
            this.label18 = new System.Windows.Forms.Label();
            this.MinMCTextBox = new System.Windows.Forms.TextBox();
            this.label19 = new System.Windows.Forms.Label();
            this.MaxDCTextBox = new System.Windows.Forms.TextBox();
            this.label20 = new System.Windows.Forms.Label();
            this.MinDCTextBox = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.MaxMACTextBox = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.MinMACTextBox = new System.Windows.Forms.TextBox();
            this.label17 = new System.Windows.Forms.Label();
            this.MaxACTextBox = new System.Windows.Forms.TextBox();
            this.label15 = new System.Windows.Forms.Label();
            this.MinACTextBox = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.MonsterNameTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SaveButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // SelectMonsterComboBox
            // 
            this.SelectMonsterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SelectMonsterComboBox.FormattingEnabled = true;
            this.SelectMonsterComboBox.Location = new System.Drawing.Point(3, 3);
            this.SelectMonsterComboBox.Name = "SelectMonsterComboBox";
            this.SelectMonsterComboBox.Size = new System.Drawing.Size(196, 21);
            this.SelectMonsterComboBox.TabIndex = 0;
            this.SelectMonsterComboBox.SelectedIndexChanged += new System.EventHandler(this.SelectMonsterComboBox_SelectedIndexChanged);
            // 
            // updateButton
            // 
            this.updateButton.Location = new System.Drawing.Point(3, 216);
            this.updateButton.Name = "updateButton";
            this.updateButton.Size = new System.Drawing.Size(75, 23);
            this.updateButton.TabIndex = 59;
            this.updateButton.Text = "Update";
            this.updateButton.UseVisualStyleBackColor = true;
            this.updateButton.Click += new System.EventHandler(this.updateButton_Click);
            // 
            // CoolEyeTextBox
            // 
            this.CoolEyeTextBox.Location = new System.Drawing.Point(508, 83);
            this.CoolEyeTextBox.MaxLength = 3;
            this.CoolEyeTextBox.Name = "CoolEyeTextBox";
            this.CoolEyeTextBox.Size = new System.Drawing.Size(30, 20);
            this.CoolEyeTextBox.TabIndex = 123;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(450, 86);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(52, 13);
            this.label12.TabIndex = 124;
            this.label12.Text = "Cool Eye:";
            // 
            // ViewRangeTextBox
            // 
            this.ViewRangeTextBox.Location = new System.Drawing.Point(436, 57);
            this.ViewRangeTextBox.MaxLength = 3;
            this.ViewRangeTextBox.Name = "ViewRangeTextBox";
            this.ViewRangeTextBox.Size = new System.Drawing.Size(30, 20);
            this.ViewRangeTextBox.TabIndex = 121;
            // 
            // label33
            // 
            this.label33.AutoSize = true;
            this.label33.Location = new System.Drawing.Point(362, 60);
            this.label33.Name = "label33";
            this.label33.Size = new System.Drawing.Size(68, 13);
            this.label33.TabIndex = 122;
            this.label33.Text = "View Range:";
            // 
            // MSpeedTextBox
            // 
            this.MSpeedTextBox.Location = new System.Drawing.Point(216, 186);
            this.MSpeedTextBox.MaxLength = 5;
            this.MSpeedTextBox.Name = "MSpeedTextBox";
            this.MSpeedTextBox.Size = new System.Drawing.Size(40, 20);
            this.MSpeedTextBox.TabIndex = 104;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(139, 189);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(71, 13);
            this.label6.TabIndex = 120;
            this.label6.Text = "Move Speed:";
            // 
            // ASpeedTextBox
            // 
            this.ASpeedTextBox.Location = new System.Drawing.Point(93, 186);
            this.ASpeedTextBox.MaxLength = 5;
            this.ASpeedTextBox.Name = "ASpeedTextBox";
            this.ASpeedTextBox.Size = new System.Drawing.Size(40, 20);
            this.ASpeedTextBox.TabIndex = 103;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 189);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(75, 13);
            this.label5.TabIndex = 119;
            this.label5.Text = "Attack Speed:";
            // 
            // LevelTextBox
            // 
            this.LevelTextBox.Location = new System.Drawing.Point(251, 56);
            this.LevelTextBox.MaxLength = 3;
            this.LevelTextBox.Name = "LevelTextBox";
            this.LevelTextBox.Size = new System.Drawing.Size(30, 20);
            this.LevelTextBox.TabIndex = 87;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(209, 60);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(36, 13);
            this.label4.TabIndex = 118;
            this.label4.Text = "Level:";
            // 
            // EffectTextBox
            // 
            this.EffectTextBox.Location = new System.Drawing.Point(173, 56);
            this.EffectTextBox.MaxLength = 3;
            this.EffectTextBox.Name = "EffectTextBox";
            this.EffectTextBox.Size = new System.Drawing.Size(30, 20);
            this.EffectTextBox.TabIndex = 86;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(129, 60);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(38, 13);
            this.label2.TabIndex = 117;
            this.label2.Text = "Effect:";
            // 
            // AgilityTextBox
            // 
            this.AgilityTextBox.Location = new System.Drawing.Point(182, 160);
            this.AgilityTextBox.MaxLength = 3;
            this.AgilityTextBox.Name = "AgilityTextBox";
            this.AgilityTextBox.Size = new System.Drawing.Size(30, 20);
            this.AgilityTextBox.TabIndex = 102;
            // 
            // label26
            // 
            this.label26.AutoSize = true;
            this.label26.Location = new System.Drawing.Point(139, 163);
            this.label26.Name = "label26";
            this.label26.Size = new System.Drawing.Size(37, 13);
            this.label26.TabIndex = 116;
            this.label26.Text = "Agility:";
            // 
            // AccuracyTextBox
            // 
            this.AccuracyTextBox.Location = new System.Drawing.Point(93, 160);
            this.AccuracyTextBox.MaxLength = 3;
            this.AccuracyTextBox.Name = "AccuracyTextBox";
            this.AccuracyTextBox.Size = new System.Drawing.Size(30, 20);
            this.AccuracyTextBox.TabIndex = 101;
            // 
            // label27
            // 
            this.label27.AutoSize = true;
            this.label27.Location = new System.Drawing.Point(32, 163);
            this.label27.Name = "label27";
            this.label27.Size = new System.Drawing.Size(55, 13);
            this.label27.TabIndex = 115;
            this.label27.Text = "Accuracy:";
            // 
            // HPTextBox
            // 
            this.HPTextBox.Location = new System.Drawing.Point(93, 82);
            this.HPTextBox.MaxLength = 10;
            this.HPTextBox.Name = "HPTextBox";
            this.HPTextBox.Size = new System.Drawing.Size(72, 20);
            this.HPTextBox.TabIndex = 88;
            // 
            // label25
            // 
            this.label25.AutoSize = true;
            this.label25.Location = new System.Drawing.Point(62, 85);
            this.label25.Name = "label25";
            this.label25.Size = new System.Drawing.Size(25, 13);
            this.label25.TabIndex = 114;
            this.label25.Text = "HP:";
            // 
            // MaxSCTextBox
            // 
            this.MaxSCTextBox.Location = new System.Drawing.Point(554, 134);
            this.MaxSCTextBox.MaxLength = 3;
            this.MaxSCTextBox.Name = "MaxSCTextBox";
            this.MaxSCTextBox.Size = new System.Drawing.Size(30, 20);
            this.MaxSCTextBox.TabIndex = 100;
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(499, 137);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(47, 13);
            this.label22.TabIndex = 113;
            this.label22.Text = "Max SC:";
            // 
            // MinSCTextBox
            // 
            this.MinSCTextBox.Location = new System.Drawing.Point(463, 134);
            this.MinSCTextBox.MaxLength = 3;
            this.MinSCTextBox.Name = "MinSCTextBox";
            this.MinSCTextBox.Size = new System.Drawing.Size(30, 20);
            this.MinSCTextBox.TabIndex = 99;
            // 
            // label23
            // 
            this.label23.AutoSize = true;
            this.label23.Location = new System.Drawing.Point(411, 137);
            this.label23.Name = "label23";
            this.label23.Size = new System.Drawing.Size(44, 13);
            this.label23.TabIndex = 112;
            this.label23.Text = "Min SC:";
            // 
            // MaxMCTextBox
            // 
            this.MaxMCTextBox.Location = new System.Drawing.Point(375, 134);
            this.MaxMCTextBox.MaxLength = 3;
            this.MaxMCTextBox.Name = "MaxMCTextBox";
            this.MaxMCTextBox.Size = new System.Drawing.Size(30, 20);
            this.MaxMCTextBox.TabIndex = 98;
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(320, 137);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(49, 13);
            this.label18.TabIndex = 111;
            this.label18.Text = "Max MC:";
            // 
            // MinMCTextBox
            // 
            this.MinMCTextBox.Location = new System.Drawing.Point(277, 134);
            this.MinMCTextBox.MaxLength = 3;
            this.MinMCTextBox.Name = "MinMCTextBox";
            this.MinMCTextBox.Size = new System.Drawing.Size(30, 20);
            this.MinMCTextBox.TabIndex = 97;
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(225, 137);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(46, 13);
            this.label19.TabIndex = 110;
            this.label19.Text = "Min MC:";
            // 
            // MaxDCTextBox
            // 
            this.MaxDCTextBox.Location = new System.Drawing.Point(182, 134);
            this.MaxDCTextBox.MaxLength = 3;
            this.MaxDCTextBox.Name = "MaxDCTextBox";
            this.MaxDCTextBox.Size = new System.Drawing.Size(30, 20);
            this.MaxDCTextBox.TabIndex = 95;
            // 
            // label20
            // 
            this.label20.AutoSize = true;
            this.label20.Location = new System.Drawing.Point(129, 137);
            this.label20.Name = "label20";
            this.label20.Size = new System.Drawing.Size(48, 13);
            this.label20.TabIndex = 109;
            this.label20.Text = "Max DC:";
            // 
            // MinDCTextBox
            // 
            this.MinDCTextBox.Location = new System.Drawing.Point(93, 134);
            this.MinDCTextBox.MaxLength = 3;
            this.MinDCTextBox.Name = "MinDCTextBox";
            this.MinDCTextBox.Size = new System.Drawing.Size(30, 20);
            this.MinDCTextBox.TabIndex = 94;
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(42, 141);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(45, 13);
            this.label21.TabIndex = 108;
            this.label21.Text = "Min DC:";
            // 
            // MaxMACTextBox
            // 
            this.MaxMACTextBox.Location = new System.Drawing.Point(375, 108);
            this.MaxMACTextBox.MaxLength = 3;
            this.MaxMACTextBox.Name = "MaxMACTextBox";
            this.MaxMACTextBox.Size = new System.Drawing.Size(30, 20);
            this.MaxMACTextBox.TabIndex = 93;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(313, 111);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(56, 13);
            this.label16.TabIndex = 107;
            this.label16.Text = "Max MAC:";
            // 
            // MinMACTextBox
            // 
            this.MinMACTextBox.Location = new System.Drawing.Point(277, 108);
            this.MinMACTextBox.MaxLength = 3;
            this.MinMACTextBox.Name = "MinMACTextBox";
            this.MinMACTextBox.Size = new System.Drawing.Size(30, 20);
            this.MinMACTextBox.TabIndex = 92;
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(218, 111);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(53, 13);
            this.label17.TabIndex = 96;
            this.label17.Text = "Min MAC:";
            // 
            // MaxACTextBox
            // 
            this.MaxACTextBox.Location = new System.Drawing.Point(182, 108);
            this.MaxACTextBox.MaxLength = 3;
            this.MaxACTextBox.Name = "MaxACTextBox";
            this.MaxACTextBox.Size = new System.Drawing.Size(30, 20);
            this.MaxACTextBox.TabIndex = 91;
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(129, 111);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(47, 13);
            this.label15.TabIndex = 106;
            this.label15.Text = "Max AC:";
            // 
            // MinACTextBox
            // 
            this.MinACTextBox.Location = new System.Drawing.Point(93, 108);
            this.MinACTextBox.MaxLength = 3;
            this.MinACTextBox.Name = "MinACTextBox";
            this.MinACTextBox.Size = new System.Drawing.Size(30, 20);
            this.MinACTextBox.TabIndex = 90;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(43, 111);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(44, 13);
            this.label14.TabIndex = 105;
            this.label14.Text = "Min AC:";
            // 
            // MonsterNameTextBox
            // 
            this.MonsterNameTextBox.Location = new System.Drawing.Point(93, 30);
            this.MonsterNameTextBox.Name = "MonsterNameTextBox";
            this.MonsterNameTextBox.Size = new System.Drawing.Size(115, 20);
            this.MonsterNameTextBox.TabIndex = 85;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(8, 34);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(79, 13);
            this.label3.TabIndex = 89;
            this.label3.Text = "Monster Name:";
            // 
            // SaveButton
            // 
            this.SaveButton.Location = new System.Drawing.Point(84, 216);
            this.SaveButton.Name = "SaveButton";
            this.SaveButton.Size = new System.Drawing.Size(75, 23);
            this.SaveButton.TabIndex = 125;
            this.SaveButton.Text = "Save To DB";
            this.SaveButton.UseVisualStyleBackColor = true;
            this.SaveButton.Click += new System.EventHandler(this.SaveButton_Click);
            // 
            // MonsterTunerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(591, 246);
            this.Controls.Add(this.SaveButton);
            this.Controls.Add(this.CoolEyeTextBox);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.ViewRangeTextBox);
            this.Controls.Add(this.label33);
            this.Controls.Add(this.MSpeedTextBox);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.ASpeedTextBox);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.LevelTextBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EffectTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.AgilityTextBox);
            this.Controls.Add(this.label26);
            this.Controls.Add(this.AccuracyTextBox);
            this.Controls.Add(this.label27);
            this.Controls.Add(this.HPTextBox);
            this.Controls.Add(this.label25);
            this.Controls.Add(this.MaxSCTextBox);
            this.Controls.Add(this.label22);
            this.Controls.Add(this.MinSCTextBox);
            this.Controls.Add(this.label23);
            this.Controls.Add(this.MaxMCTextBox);
            this.Controls.Add(this.label18);
            this.Controls.Add(this.MinMCTextBox);
            this.Controls.Add(this.label19);
            this.Controls.Add(this.MaxDCTextBox);
            this.Controls.Add(this.label20);
            this.Controls.Add(this.MinDCTextBox);
            this.Controls.Add(this.label21);
            this.Controls.Add(this.MaxMACTextBox);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.MinMACTextBox);
            this.Controls.Add(this.label17);
            this.Controls.Add(this.MaxACTextBox);
            this.Controls.Add(this.label15);
            this.Controls.Add(this.MinACTextBox);
            this.Controls.Add(this.label14);
            this.Controls.Add(this.MonsterNameTextBox);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.updateButton);
            this.Controls.Add(this.SelectMonsterComboBox);
            this.Name = "MonsterTunerForm";
            this.Text = "Monster Tuner";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox SelectMonsterComboBox;
        private System.Windows.Forms.Button updateButton;
        private System.Windows.Forms.TextBox CoolEyeTextBox;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox ViewRangeTextBox;
        private System.Windows.Forms.Label label33;
        private System.Windows.Forms.TextBox MSpeedTextBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox ASpeedTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox LevelTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox EffectTextBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox AgilityTextBox;
        private System.Windows.Forms.Label label26;
        private System.Windows.Forms.TextBox AccuracyTextBox;
        private System.Windows.Forms.Label label27;
        private System.Windows.Forms.TextBox HPTextBox;
        private System.Windows.Forms.Label label25;
        private System.Windows.Forms.TextBox MaxSCTextBox;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox MinSCTextBox;
        private System.Windows.Forms.Label label23;
        private System.Windows.Forms.TextBox MaxMCTextBox;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox MinMCTextBox;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TextBox MaxDCTextBox;
        private System.Windows.Forms.Label label20;
        private System.Windows.Forms.TextBox MinDCTextBox;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox MaxMACTextBox;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.TextBox MinMACTextBox;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.TextBox MaxACTextBox;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox MinACTextBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox MonsterNameTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button SaveButton;
    }
}