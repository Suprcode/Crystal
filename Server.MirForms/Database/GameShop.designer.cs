namespace Server
{
    partial class GameShop
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
            this.checkBox3 = new System.Windows.Forms.CheckBox();
            this.HuntPointsPrice_textbox = new System.Windows.Forms.TextBox();
            this.label14Hunt = new System.Windows.Forms.Label();
            this.checkBox2 = new System.Windows.Forms.CheckBox();
            this.checkBox1 = new System.Windows.Forms.CheckBox();
            this.button1 = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.GameShopListBox = new System.Windows.Forms.ListBox();
            this.label14 = new System.Windows.Forms.Label();
            this.GoldPrice_textbox = new System.Windows.Forms.TextBox();
            this.label21 = new System.Windows.Forms.Label();
            this.GPPrice_textbox = new System.Windows.Forms.TextBox();
            this.label29 = new System.Windows.Forms.Label();
            this.ItemDetails_gb = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.Count_textbox = new System.Windows.Forms.TextBox();
            this.LeftinStock_label = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.TotalSold_label = new System.Windows.Forms.Label();
            this.TopItem_checkbox = new System.Windows.Forms.CheckBox();
            this.DealofDay_checkbox = new System.Windows.Forms.CheckBox();
            this.Individual_checkbox = new System.Windows.Forms.CheckBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Stock_textbox = new System.Windows.Forms.TextBox();
            this.Category_textbox = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Class_combo = new System.Windows.Forms.ComboBox();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label2 = new System.Windows.Forms.Label();
            this.CredxGold_textbox = new System.Windows.Forms.TextBox();
            this.ServerLog_button = new System.Windows.Forms.Button();
            this.Remove_button = new System.Windows.Forms.Button();
            this.ClassFilter_lb = new System.Windows.Forms.ComboBox();
            this.SectionFilter_lb = new System.Windows.Forms.ComboBox();
            this.CategoryFilter_lb = new System.Windows.Forms.ComboBox();
            this.ResetFilter_button = new System.Windows.Forms.Button();
            this.ItemDetails_gb.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // checkBox3
            // 
            this.checkBox3.AutoSize = true;
            this.checkBox3.Location = new System.Drawing.Point(270, 154);
            this.checkBox3.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox3.Name = "checkBox3";
            this.checkBox3.Size = new System.Drawing.Size(99, 21);
            this.checkBox3.TabIndex = 118;
            this.checkBox3.Text = "HuntPoints";
            this.checkBox3.UseVisualStyleBackColor = true;
            this.checkBox3.CheckedChanged += new System.EventHandler(this.checkBox3_CheckedChanged);
            // 
            // HuntPointsPrice_textbox
            // 
            this.HuntPointsPrice_textbox.Location = new System.Drawing.Point(115, 154);
            this.HuntPointsPrice_textbox.Margin = new System.Windows.Forms.Padding(4);
            this.HuntPointsPrice_textbox.MaxLength = 0;
            this.HuntPointsPrice_textbox.Name = "HuntPointsPrice_textbox";
            this.HuntPointsPrice_textbox.Size = new System.Drawing.Size(149, 22);
            this.HuntPointsPrice_textbox.TabIndex = 86;
            this.HuntPointsPrice_textbox.TextChanged += new System.EventHandler(this.HuntPointsPrice_textbox_TextChanged);
            // 
            // label14Hunt
            // 
            this.label14Hunt.AutoSize = true;
            this.label14Hunt.Location = new System.Drawing.Point(31, 155);
            this.label14Hunt.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14Hunt.Name = "label14Hunt";
            this.label14Hunt.Size = new System.Drawing.Size(78, 17);
            this.label14Hunt.TabIndex = 90;
            this.label14Hunt.Text = "Hunt Price:";
            // 
            // checkBox2
            // 
            this.checkBox2.AutoSize = true;
            this.checkBox2.Location = new System.Drawing.Point(270, 124);
            this.checkBox2.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox2.Name = "checkBox2";
            this.checkBox2.Size = new System.Drawing.Size(60, 21);
            this.checkBox2.TabIndex = 119;
            this.checkBox2.Text = "Gold";
            this.checkBox2.UseVisualStyleBackColor = true;
            this.checkBox2.CheckedChanged += new System.EventHandler(this.checkBox2_CheckedChanged);
            // 
            // checkBox1
            // 
            this.checkBox1.AutoSize = true;
            this.checkBox1.Location = new System.Drawing.Point(270, 93);
            this.checkBox1.Margin = new System.Windows.Forms.Padding(4);
            this.checkBox1.Name = "checkBox1";
            this.checkBox1.Size = new System.Drawing.Size(74, 21);
            this.checkBox1.TabIndex = 118;
            this.checkBox1.Text = "Credits";
            this.checkBox1.UseVisualStyleBackColor = true;
            this.checkBox1.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(16, 529);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(102, 23);
            this.button1.TabIndex = 113;
            this.button1.Text = "Export";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(16, 504);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(102, 23);
            this.button2.TabIndex = 114;
            this.button2.Text = "Import";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // GameShopListBox
            // 
            this.GameShopListBox.FormattingEnabled = true;
            this.GameShopListBox.ItemHeight = 16;
            this.GameShopListBox.Location = new System.Drawing.Point(16, 95);
            this.GameShopListBox.Margin = new System.Windows.Forms.Padding(4);
            this.GameShopListBox.Name = "GameShopListBox";
            this.GameShopListBox.ScrollAlwaysVisible = true;
            this.GameShopListBox.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.GameShopListBox.Size = new System.Drawing.Size(267, 404);
            this.GameShopListBox.TabIndex = 11;
            this.GameShopListBox.SelectedIndexChanged += new System.EventHandler(this.GameShopListBox_SelectedIndexChanged);
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(31, 129);
            this.label14.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(78, 17);
            this.label14.TabIndex = 90;
            this.label14.Text = "Gold Price:";
            // 
            // GoldPrice_textbox
            // 
            this.GoldPrice_textbox.Location = new System.Drawing.Point(115, 124);
            this.GoldPrice_textbox.Margin = new System.Windows.Forms.Padding(4);
            this.GoldPrice_textbox.MaxLength = 0;
            this.GoldPrice_textbox.Name = "GoldPrice_textbox";
            this.GoldPrice_textbox.Size = new System.Drawing.Size(149, 22);
            this.GoldPrice_textbox.TabIndex = 86;
            this.GoldPrice_textbox.TextChanged += new System.EventHandler(this.GoldPrice_textbox_TextChanged);
            // 
            // label21
            // 
            this.label21.AutoSize = true;
            this.label21.Location = new System.Drawing.Point(24, 95);
            this.label21.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label21.Name = "label21";
            this.label21.Size = new System.Drawing.Size(85, 17);
            this.label21.TabIndex = 91;
            this.label21.Text = "Credit Price:";
            // 
            // GPPrice_textbox
            // 
            this.GPPrice_textbox.Location = new System.Drawing.Point(115, 91);
            this.GPPrice_textbox.Margin = new System.Windows.Forms.Padding(4);
            this.GPPrice_textbox.MaxLength = 0;
            this.GPPrice_textbox.Name = "GPPrice_textbox";
            this.GPPrice_textbox.Size = new System.Drawing.Size(149, 22);
            this.GPPrice_textbox.TabIndex = 87;
            this.GPPrice_textbox.TextChanged += new System.EventHandler(this.GPPrice_textbox_TextChanged);
            // 
            // label29
            // 
            this.label29.AutoSize = true;
            this.label29.Location = new System.Drawing.Point(11, 222);
            this.label29.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label29.Name = "label29";
            this.label29.Size = new System.Drawing.Size(97, 17);
            this.label29.TabIndex = 93;
            this.label29.Text = "Class Section:";
            // 
            // ItemDetails_gb
            // 
            this.ItemDetails_gb.BackColor = System.Drawing.Color.White;
            this.ItemDetails_gb.Controls.Add(this.HuntPointsPrice_textbox);
            this.ItemDetails_gb.Controls.Add(this.checkBox3);
            this.ItemDetails_gb.Controls.Add(this.checkBox2);
            this.ItemDetails_gb.Controls.Add(this.checkBox1);
            this.ItemDetails_gb.Controls.Add(this.label14Hunt);
            this.ItemDetails_gb.Controls.Add(this.label6);
            this.ItemDetails_gb.Controls.Add(this.Count_textbox);
            this.ItemDetails_gb.Controls.Add(this.LeftinStock_label);
            this.ItemDetails_gb.Controls.Add(this.label3);
            this.ItemDetails_gb.Controls.Add(this.label5);
            this.ItemDetails_gb.Controls.Add(this.TotalSold_label);
            this.ItemDetails_gb.Controls.Add(this.TopItem_checkbox);
            this.ItemDetails_gb.Controls.Add(this.DealofDay_checkbox);
            this.ItemDetails_gb.Controls.Add(this.Individual_checkbox);
            this.ItemDetails_gb.Controls.Add(this.label1);
            this.ItemDetails_gb.Controls.Add(this.Stock_textbox);
            this.ItemDetails_gb.Controls.Add(this.GoldPrice_textbox);
            this.ItemDetails_gb.Controls.Add(this.label14);
            this.ItemDetails_gb.Controls.Add(this.label21);
            this.ItemDetails_gb.Controls.Add(this.Category_textbox);
            this.ItemDetails_gb.Controls.Add(this.GPPrice_textbox);
            this.ItemDetails_gb.Controls.Add(this.label4);
            this.ItemDetails_gb.Controls.Add(this.label29);
            this.ItemDetails_gb.Controls.Add(this.Class_combo);
            this.ItemDetails_gb.Location = new System.Drawing.Point(292, 97);
            this.ItemDetails_gb.Margin = new System.Windows.Forms.Padding(4);
            this.ItemDetails_gb.Name = "ItemDetails_gb";
            this.ItemDetails_gb.Padding = new System.Windows.Forms.Padding(4);
            this.ItemDetails_gb.Size = new System.Drawing.Size(375, 402);
            this.ItemDetails_gb.TabIndex = 98;
            this.ItemDetails_gb.TabStop = false;
            this.ItemDetails_gb.Text = "Item Details";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(59, 187);
            this.label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(49, 17);
            this.label6.TabIndex = 117;
            this.label6.Text = "Count:";
            // 
            // Count_textbox
            // 
            this.Count_textbox.Location = new System.Drawing.Point(115, 184);
            this.Count_textbox.Margin = new System.Windows.Forms.Padding(4);
            this.Count_textbox.MaxLength = 0;
            this.Count_textbox.Name = "Count_textbox";
            this.Count_textbox.Size = new System.Drawing.Size(149, 22);
            this.Count_textbox.TabIndex = 116;
            this.Count_textbox.TextChanged += new System.EventHandler(this.Count_textbox_TextChanged);
            // 
            // LeftinStock_label
            // 
            this.LeftinStock_label.AutoSize = true;
            this.LeftinStock_label.Location = new System.Drawing.Point(111, 58);
            this.LeftinStock_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.LeftinStock_label.Name = "LeftinStock_label";
            this.LeftinStock_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.LeftinStock_label.Size = new System.Drawing.Size(32, 17);
            this.LeftinStock_label.TabIndex = 115;
            this.LeftinStock_label.Text = "100";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 58);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 17);
            this.label3.TabIndex = 114;
            this.label3.Text = "Left in Stock:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(32, 26);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(76, 17);
            this.label5.TabIndex = 113;
            this.label5.Text = "Total Sold:";
            // 
            // TotalSold_label
            // 
            this.TotalSold_label.AutoSize = true;
            this.TotalSold_label.Location = new System.Drawing.Point(109, 26);
            this.TotalSold_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TotalSold_label.Name = "TotalSold_label";
            this.TotalSold_label.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.TotalSold_label.Size = new System.Drawing.Size(32, 17);
            this.TotalSold_label.TabIndex = 112;
            this.TotalSold_label.Text = "100";
            // 
            // TopItem_checkbox
            // 
            this.TopItem_checkbox.AutoSize = true;
            this.TopItem_checkbox.Location = new System.Drawing.Point(39, 341);
            this.TopItem_checkbox.Margin = new System.Windows.Forms.Padding(4);
            this.TopItem_checkbox.Name = "TopItem_checkbox";
            this.TopItem_checkbox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.TopItem_checkbox.Size = new System.Drawing.Size(89, 21);
            this.TopItem_checkbox.TabIndex = 106;
            this.TopItem_checkbox.Text = ":Top Item";
            this.TopItem_checkbox.UseVisualStyleBackColor = true;
            this.TopItem_checkbox.CheckedChanged += new System.EventHandler(this.TopItem_checkbox_CheckedChanged);
            // 
            // DealofDay_checkbox
            // 
            this.DealofDay_checkbox.AutoSize = true;
            this.DealofDay_checkbox.Location = new System.Drawing.Point(36, 316);
            this.DealofDay_checkbox.Margin = new System.Windows.Forms.Padding(4);
            this.DealofDay_checkbox.Name = "DealofDay_checkbox";
            this.DealofDay_checkbox.RightToLeft = System.Windows.Forms.RightToLeft.Yes;
            this.DealofDay_checkbox.Size = new System.Drawing.Size(92, 21);
            this.DealofDay_checkbox.TabIndex = 105;
            this.DealofDay_checkbox.Text = ":Sale Item";
            this.DealofDay_checkbox.UseVisualStyleBackColor = true;
            this.DealofDay_checkbox.CheckedChanged += new System.EventHandler(this.DealofDay_checkbox_CheckedChanged);
            // 
            // Individual_checkbox
            // 
            this.Individual_checkbox.AutoSize = true;
            this.Individual_checkbox.Location = new System.Drawing.Point(217, 287);
            this.Individual_checkbox.Margin = new System.Windows.Forms.Padding(4);
            this.Individual_checkbox.Name = "Individual_checkbox";
            this.Individual_checkbox.Size = new System.Drawing.Size(103, 21);
            this.Individual_checkbox.TabIndex = 110;
            this.Individual_checkbox.Text = "Player Limit";
            this.Individual_checkbox.UseVisualStyleBackColor = true;
            this.Individual_checkbox.CheckedChanged += new System.EventHandler(this.Individual_checkbox_CheckedChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(59, 288);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 111;
            this.label1.Text = "Stock;";
            // 
            // Stock_textbox
            // 
            this.Stock_textbox.Location = new System.Drawing.Point(115, 284);
            this.Stock_textbox.Margin = new System.Windows.Forms.Padding(4);
            this.Stock_textbox.MaxLength = 0;
            this.Stock_textbox.Name = "Stock_textbox";
            this.Stock_textbox.Size = new System.Drawing.Size(95, 22);
            this.Stock_textbox.TabIndex = 109;
            this.Stock_textbox.TextChanged += new System.EventHandler(this.Stock_textbox_TextChanged);
            // 
            // Category_textbox
            // 
            this.Category_textbox.Location = new System.Drawing.Point(115, 251);
            this.Category_textbox.Margin = new System.Windows.Forms.Padding(4);
            this.Category_textbox.MaxLength = 0;
            this.Category_textbox.Name = "Category_textbox";
            this.Category_textbox.Size = new System.Drawing.Size(229, 22);
            this.Category_textbox.TabIndex = 108;
            this.Category_textbox.TextChanged += new System.EventHandler(this.Category_textbox_TextChanged);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(40, 255);
            this.label4.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(69, 17);
            this.label4.TabIndex = 106;
            this.label4.Text = "Category:";
            // 
            // Class_combo
            // 
            this.Class_combo.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Class_combo.FormattingEnabled = true;
            this.Class_combo.Items.AddRange(new object[] {
            "All",
            "Warrior",
            "Assassin",
            "Taoist",
            "Wizard",
            "Archer"});
            this.Class_combo.Location = new System.Drawing.Point(115, 218);
            this.Class_combo.Margin = new System.Windows.Forms.Padding(4);
            this.Class_combo.Name = "Class_combo";
            this.Class_combo.Size = new System.Drawing.Size(229, 24);
            this.Class_combo.TabIndex = 105;
            this.Class_combo.SelectedIndexChanged += new System.EventHandler(this.Class_combo_SelectedIndexChanged);
            // 
            // groupBox3
            // 
            this.groupBox3.BackColor = System.Drawing.Color.White;
            this.groupBox3.Controls.Add(this.label2);
            this.groupBox3.Controls.Add(this.CredxGold_textbox);
            this.groupBox3.Location = new System.Drawing.Point(292, 6);
            this.groupBox3.Margin = new System.Windows.Forms.Padding(4);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Padding = new System.Windows.Forms.Padding(4);
            this.groupBox3.Size = new System.Drawing.Size(356, 84);
            this.groupBox3.TabIndex = 105;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Gameshop Settings";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 32);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(93, 17);
            this.label2.TabIndex = 92;
            this.label2.Text = "Credit x Gold:";
            // 
            // CredxGold_textbox
            // 
            this.CredxGold_textbox.Location = new System.Drawing.Point(115, 27);
            this.CredxGold_textbox.Margin = new System.Windows.Forms.Padding(4);
            this.CredxGold_textbox.MaxLength = 0;
            this.CredxGold_textbox.Name = "CredxGold_textbox";
            this.CredxGold_textbox.Size = new System.Drawing.Size(85, 22);
            this.CredxGold_textbox.TabIndex = 88;
            this.CredxGold_textbox.TextChanged += new System.EventHandler(this.CredxGold_textbox_TextChanged);
            // 
            // ServerLog_button
            // 
            this.ServerLog_button.Location = new System.Drawing.Point(293, 501);
            this.ServerLog_button.Margin = new System.Windows.Forms.Padding(4);
            this.ServerLog_button.Name = "ServerLog_button";
            this.ServerLog_button.Size = new System.Drawing.Size(355, 28);
            this.ServerLog_button.TabIndex = 112;
            this.ServerLog_button.Text = "Reset Purchase Logs (Stock Levels will reset)";
            this.ServerLog_button.UseVisualStyleBackColor = true;
            this.ServerLog_button.Click += new System.EventHandler(this.ServerLog_button_Click);
            // 
            // Remove_button
            // 
            this.Remove_button.Location = new System.Drawing.Point(148, 504);
            this.Remove_button.Margin = new System.Windows.Forms.Padding(4);
            this.Remove_button.Name = "Remove_button";
            this.Remove_button.Size = new System.Drawing.Size(136, 28);
            this.Remove_button.TabIndex = 106;
            this.Remove_button.Text = "Remove Selected";
            this.Remove_button.UseVisualStyleBackColor = true;
            this.Remove_button.Click += new System.EventHandler(this.Remove_button_Click);
            // 
            // ClassFilter_lb
            // 
            this.ClassFilter_lb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ClassFilter_lb.FormattingEnabled = true;
            this.ClassFilter_lb.Location = new System.Drawing.Point(16, 6);
            this.ClassFilter_lb.Margin = new System.Windows.Forms.Padding(4);
            this.ClassFilter_lb.Name = "ClassFilter_lb";
            this.ClassFilter_lb.Size = new System.Drawing.Size(193, 24);
            this.ClassFilter_lb.TabIndex = 107;
            this.ClassFilter_lb.SelectedIndexChanged += new System.EventHandler(this.ClassFilter_lb_SelectedIndexChanged);
            // 
            // SectionFilter_lb
            // 
            this.SectionFilter_lb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.SectionFilter_lb.FormattingEnabled = true;
            this.SectionFilter_lb.Items.AddRange(new object[] {
            "All Items",
            "Top Items",
            "Sale Items",
            "New Items"});
            this.SectionFilter_lb.Location = new System.Drawing.Point(16, 34);
            this.SectionFilter_lb.Margin = new System.Windows.Forms.Padding(4);
            this.SectionFilter_lb.Name = "SectionFilter_lb";
            this.SectionFilter_lb.Size = new System.Drawing.Size(193, 24);
            this.SectionFilter_lb.TabIndex = 108;
            this.SectionFilter_lb.SelectedIndexChanged += new System.EventHandler(this.SectionFilter_lb_SelectedIndexChanged);
            // 
            // CategoryFilter_lb
            // 
            this.CategoryFilter_lb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CategoryFilter_lb.FormattingEnabled = true;
            this.CategoryFilter_lb.Location = new System.Drawing.Point(16, 63);
            this.CategoryFilter_lb.Margin = new System.Windows.Forms.Padding(4);
            this.CategoryFilter_lb.Name = "CategoryFilter_lb";
            this.CategoryFilter_lb.Size = new System.Drawing.Size(193, 24);
            this.CategoryFilter_lb.TabIndex = 109;
            this.CategoryFilter_lb.SelectedIndexChanged += new System.EventHandler(this.CategoryFilter_lb_SelectedIndexChanged);
            // 
            // ResetFilter_button
            // 
            this.ResetFilter_button.Location = new System.Drawing.Point(219, 5);
            this.ResetFilter_button.Margin = new System.Windows.Forms.Padding(4);
            this.ResetFilter_button.Name = "ResetFilter_button";
            this.ResetFilter_button.Size = new System.Drawing.Size(65, 85);
            this.ResetFilter_button.TabIndex = 110;
            this.ResetFilter_button.Text = "Reset Filter";
            this.ResetFilter_button.UseVisualStyleBackColor = true;
            this.ResetFilter_button.Click += new System.EventHandler(this.ResetFilter_button_Click);
            // 
            // GameShop
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(671, 564);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.ServerLog_button);
            this.Controls.Add(this.ResetFilter_button);
            this.Controls.Add(this.CategoryFilter_lb);
            this.Controls.Add(this.SectionFilter_lb);
            this.Controls.Add(this.ClassFilter_lb);
            this.Controls.Add(this.Remove_button);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.ItemDetails_gb);
            this.Controls.Add(this.GameShopListBox);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "GameShop";
            this.Text = "GameShop";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.GameShop_FormClosed);
            this.Load += new System.EventHandler(this.GameShop_Load);
            this.ItemDetails_gb.ResumeLayout(false);
            this.ItemDetails_gb.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.CheckBox checkBox2;
        private System.Windows.Forms.CheckBox checkBox1;
        private System.Windows.Forms.CheckBox checkBox3;
        private System.Windows.Forms.Label label14Hunt;
        private System.Windows.Forms.TextBox HuntPointsPrice_textbox;

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.ListBox GameShopListBox;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.TextBox GoldPrice_textbox;
        private System.Windows.Forms.Label label21;
        private System.Windows.Forms.TextBox GPPrice_textbox;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.GroupBox ItemDetails_gb;
        private System.Windows.Forms.ComboBox Class_combo;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox DealofDay_checkbox;
        private System.Windows.Forms.CheckBox TopItem_checkbox;
        private System.Windows.Forms.Button Remove_button;
        private System.Windows.Forms.TextBox Category_textbox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox Individual_checkbox;
        private System.Windows.Forms.TextBox Stock_textbox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox CredxGold_textbox;
        private System.Windows.Forms.Label TotalSold_label;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label LeftinStock_label;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox Count_textbox;
        private System.Windows.Forms.ComboBox ClassFilter_lb;
        private System.Windows.Forms.ComboBox SectionFilter_lb;
        private System.Windows.Forms.ComboBox CategoryFilter_lb;
        private System.Windows.Forms.Button ResetFilter_button;
        private System.Windows.Forms.Button ServerLog_button;
    }
}