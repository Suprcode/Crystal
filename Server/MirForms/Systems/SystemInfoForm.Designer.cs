namespace Server
{
    partial class SystemInfoForm
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.MonsterSpawnChanceTextBox = new System.Windows.Forms.TextBox();
            this.FishingMobIndexComboBox = new System.Windows.Forms.ComboBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.FishingSuccessRateMultiplierTextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.FishingDelayTextBox = new System.Windows.Forms.TextBox();
            this.FishingSuccessRateStartTextBox = new System.Windows.Forms.TextBox();
            this.FishingAttemptsTextBox = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.MailAutoSendGoldCheckbox = new System.Windows.Forms.CheckBox();
            this.MailAutoSendItemsCheckbox = new System.Windows.Forms.CheckBox();
            this.MailFreeWithStampCheckbox = new System.Windows.Forms.CheckBox();
            this.MailCostPer1kTextBox = new System.Windows.Forms.TextBox();
            this.MailInsurancePercentageTextBox = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.label5);
            this.tabPage1.Controls.Add(this.FishingSuccessRateMultiplierTextBox);
            this.tabPage1.Controls.Add(this.label3);
            this.tabPage1.Controls.Add(this.label2);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.FishingDelayTextBox);
            this.tabPage1.Controls.Add(this.FishingSuccessRateStartTextBox);
            this.tabPage1.Controls.Add(this.FishingAttemptsTextBox);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(365, 229);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Fishing";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.MonsterSpawnChanceTextBox);
            this.groupBox1.Controls.Add(this.FishingMobIndexComboBox);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Location = new System.Drawing.Point(6, 151);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(353, 72);
            this.groupBox1.TabIndex = 12;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Monster";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 19);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(73, 13);
            this.label6.TabIndex = 11;
            this.label6.Text = "Mob Spawn : ";
            // 
            // MonsterSpawnChanceTextBox
            // 
            this.MonsterSpawnChanceTextBox.Location = new System.Drawing.Point(137, 41);
            this.MonsterSpawnChanceTextBox.Name = "MonsterSpawnChanceTextBox";
            this.MonsterSpawnChanceTextBox.Size = new System.Drawing.Size(100, 20);
            this.MonsterSpawnChanceTextBox.TabIndex = 3;
            this.MonsterSpawnChanceTextBox.TextChanged += new System.EventHandler(this.MonsterSpawnChanceTextBox_TextChanged);
            // 
            // FishingMobIndexComboBox
            // 
            this.FishingMobIndexComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.FishingMobIndexComboBox.FormattingEnabled = true;
            this.FishingMobIndexComboBox.Location = new System.Drawing.Point(137, 16);
            this.FishingMobIndexComboBox.Name = "FishingMobIndexComboBox";
            this.FishingMobIndexComboBox.Size = new System.Drawing.Size(100, 21);
            this.FishingMobIndexComboBox.TabIndex = 10;
            this.FishingMobIndexComboBox.SelectedIndexChanged += new System.EventHandler(this.FishingMobIndexComboBox_SelectedIndexChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 44);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(124, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Mob Spawn Chance % : ";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(9, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(127, 13);
            this.label5.TabIndex = 9;
            this.label5.Text = "Success Rate Multiplier : ";
            // 
            // FishingSuccessRateMultiplierTextBox
            // 
            this.FishingSuccessRateMultiplierTextBox.Location = new System.Drawing.Point(143, 61);
            this.FishingSuccessRateMultiplierTextBox.Name = "FishingSuccessRateMultiplierTextBox";
            this.FishingSuccessRateMultiplierTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingSuccessRateMultiplierTextBox.TabIndex = 8;
            this.FishingSuccessRateMultiplierTextBox.TextChanged += new System.EventHandler(this.FishingSuccessRateMultiplierTextBox_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(9, 90);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(67, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Delay / ms : ";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(9, 38);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(119, 13);
            this.label2.TabIndex = 5;
            this.label2.Text = "Success Rate Start % : ";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(95, 13);
            this.label1.TabIndex = 4;
            this.label1.Text = "Attempts / round : ";
            // 
            // FishingDelayTextBox
            // 
            this.FishingDelayTextBox.Location = new System.Drawing.Point(143, 87);
            this.FishingDelayTextBox.Name = "FishingDelayTextBox";
            this.FishingDelayTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingDelayTextBox.TabIndex = 2;
            this.FishingDelayTextBox.TextChanged += new System.EventHandler(this.FishingDelayTextBox_TextChanged);
            // 
            // FishingSuccessRateStartTextBox
            // 
            this.FishingSuccessRateStartTextBox.Location = new System.Drawing.Point(143, 35);
            this.FishingSuccessRateStartTextBox.Name = "FishingSuccessRateStartTextBox";
            this.FishingSuccessRateStartTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingSuccessRateStartTextBox.TabIndex = 1;
            this.FishingSuccessRateStartTextBox.TextChanged += new System.EventHandler(this.FishingSuccessRateStartTextBox_TextChanged);
            // 
            // FishingAttemptsTextBox
            // 
            this.FishingAttemptsTextBox.Location = new System.Drawing.Point(143, 9);
            this.FishingAttemptsTextBox.Name = "FishingAttemptsTextBox";
            this.FishingAttemptsTextBox.Size = new System.Drawing.Size(100, 20);
            this.FishingAttemptsTextBox.TabIndex = 0;
            this.FishingAttemptsTextBox.TextChanged += new System.EventHandler(this.FishingAttemptsTextBox_TextChanged);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(12, 12);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(373, 255);
            this.tabControl1.TabIndex = 0;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.label8);
            this.tabPage2.Controls.Add(this.label7);
            this.tabPage2.Controls.Add(this.MailInsurancePercentageTextBox);
            this.tabPage2.Controls.Add(this.MailCostPer1kTextBox);
            this.tabPage2.Controls.Add(this.MailFreeWithStampCheckbox);
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(365, 229);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Mail";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.MailAutoSendItemsCheckbox);
            this.groupBox2.Controls.Add(this.MailAutoSendGoldCheckbox);
            this.groupBox2.Location = new System.Drawing.Point(7, 7);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(90, 69);
            this.groupBox2.TabIndex = 0;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Auto Send";
            // 
            // MailAutoSendGoldCheckbox
            // 
            this.MailAutoSendGoldCheckbox.AutoSize = true;
            this.MailAutoSendGoldCheckbox.Location = new System.Drawing.Point(7, 20);
            this.MailAutoSendGoldCheckbox.Name = "MailAutoSendGoldCheckbox";
            this.MailAutoSendGoldCheckbox.Size = new System.Drawing.Size(48, 17);
            this.MailAutoSendGoldCheckbox.TabIndex = 0;
            this.MailAutoSendGoldCheckbox.Text = "Gold";
            this.MailAutoSendGoldCheckbox.UseVisualStyleBackColor = true;
            this.MailAutoSendGoldCheckbox.CheckedChanged += new System.EventHandler(this.MailAutoSendGoldCheckbox_CheckedChanged);
            // 
            // MailAutoSendItemsCheckbox
            // 
            this.MailAutoSendItemsCheckbox.AutoSize = true;
            this.MailAutoSendItemsCheckbox.Location = new System.Drawing.Point(7, 44);
            this.MailAutoSendItemsCheckbox.Name = "MailAutoSendItemsCheckbox";
            this.MailAutoSendItemsCheckbox.Size = new System.Drawing.Size(51, 17);
            this.MailAutoSendItemsCheckbox.TabIndex = 1;
            this.MailAutoSendItemsCheckbox.Text = "Items";
            this.MailAutoSendItemsCheckbox.UseVisualStyleBackColor = true;
            this.MailAutoSendItemsCheckbox.CheckedChanged += new System.EventHandler(this.MailAutoSendItemsCheckbox_CheckedChanged);
            // 
            // MailFreeWithStampCheckbox
            // 
            this.MailFreeWithStampCheckbox.AutoSize = true;
            this.MailFreeWithStampCheckbox.Location = new System.Drawing.Point(120, 7);
            this.MailFreeWithStampCheckbox.Name = "MailFreeWithStampCheckbox";
            this.MailFreeWithStampCheckbox.Size = new System.Drawing.Size(150, 17);
            this.MailFreeWithStampCheckbox.TabIndex = 1;
            this.MailFreeWithStampCheckbox.Text = "Send Mail Free with stamp";
            this.MailFreeWithStampCheckbox.UseVisualStyleBackColor = true;
            this.MailFreeWithStampCheckbox.CheckedChanged += new System.EventHandler(this.MailFreeWithStampCheckbox_CheckedChanged);
            // 
            // MailCostPer1kTextBox
            // 
            this.MailCostPer1kTextBox.Location = new System.Drawing.Point(235, 30);
            this.MailCostPer1kTextBox.Name = "MailCostPer1kTextBox";
            this.MailCostPer1kTextBox.Size = new System.Drawing.Size(100, 20);
            this.MailCostPer1kTextBox.TabIndex = 2;
            this.MailCostPer1kTextBox.TextChanged += new System.EventHandler(this.MailCostPer1kTextBox_TextChanged);
            // 
            // MailInsurancePercentageTextBox
            // 
            this.MailInsurancePercentageTextBox.Location = new System.Drawing.Point(235, 56);
            this.MailInsurancePercentageTextBox.Name = "MailInsurancePercentageTextBox";
            this.MailInsurancePercentageTextBox.Size = new System.Drawing.Size(100, 20);
            this.MailInsurancePercentageTextBox.TabIndex = 3;
            this.MailInsurancePercentageTextBox.TextChanged += new System.EventHandler(this.MailInsurancePercentageTextBox_TextChanged);
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(117, 33);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(61, 13);
            this.label7.TabIndex = 4;
            this.label7.Text = "Cost per 1k";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(117, 59);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 5;
            this.label8.Text = "Insurance % Per Item";
            // 
            // SystemInfoForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(394, 279);
            this.Controls.Add(this.tabControl1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "SystemInfoForm";
            this.Text = "SystemInfoForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.SystemInfoForm_FormClosed);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox MonsterSpawnChanceTextBox;
        private System.Windows.Forms.TextBox FishingDelayTextBox;
        private System.Windows.Forms.TextBox FishingSuccessRateStartTextBox;
        private System.Windows.Forms.TextBox FishingAttemptsTextBox;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox FishingSuccessRateMultiplierTextBox;
        private System.Windows.Forms.ComboBox FishingMobIndexComboBox;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.CheckBox MailAutoSendItemsCheckbox;
        private System.Windows.Forms.CheckBox MailAutoSendGoldCheckbox;
        private System.Windows.Forms.CheckBox MailFreeWithStampCheckbox;
        private System.Windows.Forms.TextBox MailCostPer1kTextBox;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox MailInsurancePercentageTextBox;

    }
}