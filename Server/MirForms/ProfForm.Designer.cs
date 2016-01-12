namespace Server
{
    partial class CraftingForm
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
            this.Profs_lb = new System.Windows.Forms.ListBox();
            this.Profs_tab = new System.Windows.Forms.TabControl();
            this.Prof_tab = new System.Windows.Forms.TabPage();
            this.label12 = new System.Windows.Forms.Label();
            this.ProfType_cb = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.ProfName_tb = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.ProfIndex_tb = new System.Windows.Forms.TextBox();
            this.Recipes_tab = new System.Windows.Forms.TabPage();
            this.RemoveRecipe_button = new System.Windows.Forms.Button();
            this.AddRecipe_button = new System.Windows.Forms.Button();
            this.Recipe_gb = new System.Windows.Forms.GroupBox();
            this.label11 = new System.Windows.Forms.Label();
            this.IngredientCount_tb = new System.Windows.Forms.TextBox();
            this.label10 = new System.Windows.Forms.Label();
            this.Index_tb = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.CraftTime_tb = new System.Windows.Forms.TextBox();
            this.StartRecipe_cb = new System.Windows.Forms.CheckBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.CraftItem_cb = new System.Windows.Forms.ComboBox();
            this.RemoveIng_button = new System.Windows.Forms.Button();
            this.AddIng_button = new System.Windows.Forms.Button();
            this.Ingredient_cb = new System.Windows.Forms.ComboBox();
            this.Ingredients_lb = new System.Windows.Forms.ListBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Exp_tb = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.Level_tb = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Name_tb = new System.Windows.Forms.TextBox();
            this.Recipes_lb = new System.Windows.Forms.ListBox();
            this.AddProf_button = new System.Windows.Forms.Button();
            this.RemoveProf_button = new System.Windows.Forms.Button();
            this.Profs_tab.SuspendLayout();
            this.Prof_tab.SuspendLayout();
            this.Recipes_tab.SuspendLayout();
            this.Recipe_gb.SuspendLayout();
            this.SuspendLayout();
            // 
            // Profs_lb
            // 
            this.Profs_lb.FormattingEnabled = true;
            this.Profs_lb.Location = new System.Drawing.Point(12, 12);
            this.Profs_lb.Name = "Profs_lb";
            this.Profs_lb.Size = new System.Drawing.Size(174, 316);
            this.Profs_lb.TabIndex = 0;
            this.Profs_lb.SelectedIndexChanged += new System.EventHandler(this.Profs_lb_SelectedIndexChanged);
            // 
            // Profs_tab
            // 
            this.Profs_tab.Controls.Add(this.Prof_tab);
            this.Profs_tab.Controls.Add(this.Recipes_tab);
            this.Profs_tab.Enabled = false;
            this.Profs_tab.Location = new System.Drawing.Point(192, 12);
            this.Profs_tab.Name = "Profs_tab";
            this.Profs_tab.SelectedIndex = 0;
            this.Profs_tab.Size = new System.Drawing.Size(640, 344);
            this.Profs_tab.TabIndex = 1;
            // 
            // Prof_tab
            // 
            this.Prof_tab.Controls.Add(this.label12);
            this.Prof_tab.Controls.Add(this.ProfType_cb);
            this.Prof_tab.Controls.Add(this.label9);
            this.Prof_tab.Controls.Add(this.ProfName_tb);
            this.Prof_tab.Controls.Add(this.label8);
            this.Prof_tab.Controls.Add(this.ProfIndex_tb);
            this.Prof_tab.Location = new System.Drawing.Point(4, 22);
            this.Prof_tab.Name = "Prof_tab";
            this.Prof_tab.Size = new System.Drawing.Size(632, 318);
            this.Prof_tab.TabIndex = 1;
            this.Prof_tab.Text = "Info";
            this.Prof_tab.UseVisualStyleBackColor = true;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(23, 83);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(88, 13);
            this.label12.TabIndex = 5;
            this.label12.Text = "Profession Type:";
            // 
            // ProfType_cb
            // 
            this.ProfType_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.ProfType_cb.FormattingEnabled = true;
            this.ProfType_cb.Location = new System.Drawing.Point(26, 98);
            this.ProfType_cb.Name = "ProfType_cb";
            this.ProfType_cb.Size = new System.Drawing.Size(185, 21);
            this.ProfType_cb.TabIndex = 4;
            this.ProfType_cb.SelectedIndexChanged += new System.EventHandler(this.ProfType_cb_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(23, 33);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(38, 13);
            this.label9.TabIndex = 3;
            this.label9.Text = "Name:";
            // 
            // ProfName_tb
            // 
            this.ProfName_tb.Location = new System.Drawing.Point(26, 49);
            this.ProfName_tb.Name = "ProfName_tb";
            this.ProfName_tb.Size = new System.Drawing.Size(185, 21);
            this.ProfName_tb.TabIndex = 2;
            this.ProfName_tb.TextChanged += new System.EventHandler(this.ProfName_tb_TextChanged);
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(516, 16);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(39, 13);
            this.label8.TabIndex = 1;
            this.label8.Text = "Index:";
            // 
            // ProfIndex_tb
            // 
            this.ProfIndex_tb.Enabled = false;
            this.ProfIndex_tb.Location = new System.Drawing.Point(556, 13);
            this.ProfIndex_tb.Name = "ProfIndex_tb";
            this.ProfIndex_tb.Size = new System.Drawing.Size(63, 21);
            this.ProfIndex_tb.TabIndex = 0;
            // 
            // Recipes_tab
            // 
            this.Recipes_tab.Controls.Add(this.RemoveRecipe_button);
            this.Recipes_tab.Controls.Add(this.AddRecipe_button);
            this.Recipes_tab.Controls.Add(this.Recipe_gb);
            this.Recipes_tab.Controls.Add(this.Recipes_lb);
            this.Recipes_tab.Location = new System.Drawing.Point(4, 22);
            this.Recipes_tab.Name = "Recipes_tab";
            this.Recipes_tab.Padding = new System.Windows.Forms.Padding(3);
            this.Recipes_tab.Size = new System.Drawing.Size(632, 318);
            this.Recipes_tab.TabIndex = 0;
            this.Recipes_tab.Text = "Recipes";
            this.Recipes_tab.UseVisualStyleBackColor = true;
            // 
            // RemoveRecipe_button
            // 
            this.RemoveRecipe_button.Location = new System.Drawing.Point(85, 288);
            this.RemoveRecipe_button.Name = "RemoveRecipe_button";
            this.RemoveRecipe_button.Size = new System.Drawing.Size(55, 23);
            this.RemoveRecipe_button.TabIndex = 4;
            this.RemoveRecipe_button.Text = "Remove";
            this.RemoveRecipe_button.UseVisualStyleBackColor = true;
            this.RemoveRecipe_button.Click += new System.EventHandler(this.RemoveRecipe_button_Click);
            // 
            // AddRecipe_button
            // 
            this.AddRecipe_button.Location = new System.Drawing.Point(5, 288);
            this.AddRecipe_button.Name = "AddRecipe_button";
            this.AddRecipe_button.Size = new System.Drawing.Size(35, 23);
            this.AddRecipe_button.TabIndex = 4;
            this.AddRecipe_button.Text = "Add";
            this.AddRecipe_button.UseVisualStyleBackColor = true;
            this.AddRecipe_button.Click += new System.EventHandler(this.AddRecipe_button_Click);
            // 
            // Recipe_gb
            // 
            this.Recipe_gb.Controls.Add(this.label11);
            this.Recipe_gb.Controls.Add(this.IngredientCount_tb);
            this.Recipe_gb.Controls.Add(this.label10);
            this.Recipe_gb.Controls.Add(this.Index_tb);
            this.Recipe_gb.Controls.Add(this.label7);
            this.Recipe_gb.Controls.Add(this.label6);
            this.Recipe_gb.Controls.Add(this.CraftTime_tb);
            this.Recipe_gb.Controls.Add(this.StartRecipe_cb);
            this.Recipe_gb.Controls.Add(this.label5);
            this.Recipe_gb.Controls.Add(this.label4);
            this.Recipe_gb.Controls.Add(this.CraftItem_cb);
            this.Recipe_gb.Controls.Add(this.RemoveIng_button);
            this.Recipe_gb.Controls.Add(this.AddIng_button);
            this.Recipe_gb.Controls.Add(this.Ingredient_cb);
            this.Recipe_gb.Controls.Add(this.Ingredients_lb);
            this.Recipe_gb.Controls.Add(this.label3);
            this.Recipe_gb.Controls.Add(this.Exp_tb);
            this.Recipe_gb.Controls.Add(this.label2);
            this.Recipe_gb.Controls.Add(this.Level_tb);
            this.Recipe_gb.Controls.Add(this.label1);
            this.Recipe_gb.Controls.Add(this.Name_tb);
            this.Recipe_gb.Location = new System.Drawing.Point(145, 2);
            this.Recipe_gb.Name = "Recipe_gb";
            this.Recipe_gb.Size = new System.Drawing.Size(481, 309);
            this.Recipe_gb.TabIndex = 1;
            this.Recipe_gb.TabStop = false;
            this.Recipe_gb.Text = "Recipe";
            this.Recipe_gb.Enter += new System.EventHandler(this.Recipe_gb_Enter);
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(387, 114);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 13);
            this.label11.TabIndex = 20;
            this.label11.Text = "Count:";
            // 
            // IngredientCount_tb
            // 
            this.IngredientCount_tb.Location = new System.Drawing.Point(389, 129);
            this.IngredientCount_tb.Name = "IngredientCount_tb";
            this.IngredientCount_tb.Size = new System.Drawing.Size(57, 21);
            this.IngredientCount_tb.TabIndex = 19;
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(372, 17);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(39, 13);
            this.label10.TabIndex = 18;
            this.label10.Text = "Index:";
            // 
            // Index_tb
            // 
            this.Index_tb.Enabled = false;
            this.Index_tb.Location = new System.Drawing.Point(412, 14);
            this.Index_tb.Name = "Index_tb";
            this.Index_tb.Size = new System.Drawing.Size(63, 21);
            this.Index_tb.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Tahoma", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(128, 185);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(43, 11);
            this.label7.TabIndex = 16;
            this.label7.Text = "(seconds)";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(17, 165);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(61, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Craft Time:";
            // 
            // CraftTime_tb
            // 
            this.CraftTime_tb.Location = new System.Drawing.Point(19, 180);
            this.CraftTime_tb.Name = "CraftTime_tb";
            this.CraftTime_tb.Size = new System.Drawing.Size(109, 21);
            this.CraftTime_tb.TabIndex = 14;
            this.CraftTime_tb.TextChanged += new System.EventHandler(this.CraftTime_tb_TextChanged);
            // 
            // StartRecipe_cb
            // 
            this.StartRecipe_cb.AutoSize = true;
            this.StartRecipe_cb.Location = new System.Drawing.Point(19, 222);
            this.StartRecipe_cb.Name = "StartRecipe_cb";
            this.StartRecipe_cb.Size = new System.Drawing.Size(99, 17);
            this.StartRecipe_cb.TabIndex = 13;
            this.StartRecipe_cb.Text = "Starting Recipe";
            this.StartRecipe_cb.UseVisualStyleBackColor = true;
            this.StartRecipe_cb.CheckedChanged += new System.EventHandler(this.StartRecipe_cb_CheckedChanged);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(251, 114);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(66, 13);
            this.label5.TabIndex = 12;
            this.label5.Text = "Ingredients:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(251, 55);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(61, 13);
            this.label4.TabIndex = 11;
            this.label4.Text = "Craft Item:";
            // 
            // CraftItem_cb
            // 
            this.CraftItem_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.CraftItem_cb.FormattingEnabled = true;
            this.CraftItem_cb.Location = new System.Drawing.Point(254, 70);
            this.CraftItem_cb.Name = "CraftItem_cb";
            this.CraftItem_cb.Size = new System.Drawing.Size(213, 21);
            this.CraftItem_cb.TabIndex = 10;
            this.CraftItem_cb.SelectedIndexChanged += new System.EventHandler(this.CraftItem_cb_SelectedIndexChanged);
            // 
            // RemoveIng_button
            // 
            this.RemoveIng_button.Location = new System.Drawing.Point(447, 249);
            this.RemoveIng_button.Name = "RemoveIng_button";
            this.RemoveIng_button.Size = new System.Drawing.Size(21, 23);
            this.RemoveIng_button.TabIndex = 9;
            this.RemoveIng_button.Text = "-";
            this.RemoveIng_button.UseVisualStyleBackColor = true;
            this.RemoveIng_button.Click += new System.EventHandler(this.RemoveIng_button_Click);
            // 
            // AddIng_button
            // 
            this.AddIng_button.Location = new System.Drawing.Point(447, 128);
            this.AddIng_button.Name = "AddIng_button";
            this.AddIng_button.Size = new System.Drawing.Size(21, 23);
            this.AddIng_button.TabIndex = 8;
            this.AddIng_button.Text = "+";
            this.AddIng_button.UseVisualStyleBackColor = true;
            this.AddIng_button.Click += new System.EventHandler(this.AddIng_button_Click);
            // 
            // Ingredient_cb
            // 
            this.Ingredient_cb.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.Ingredient_cb.FormattingEnabled = true;
            this.Ingredient_cb.Location = new System.Drawing.Point(254, 129);
            this.Ingredient_cb.Name = "Ingredient_cb";
            this.Ingredient_cb.Size = new System.Drawing.Size(133, 21);
            this.Ingredient_cb.TabIndex = 7;
            // 
            // Ingredients_lb
            // 
            this.Ingredients_lb.FormattingEnabled = true;
            this.Ingredients_lb.Location = new System.Drawing.Point(254, 153);
            this.Ingredients_lb.Name = "Ingredients_lb";
            this.Ingredients_lb.Size = new System.Drawing.Size(213, 95);
            this.Ingredients_lb.TabIndex = 6;
            this.Ingredients_lb.SelectedIndexChanged += new System.EventHandler(this.Ingredients_lb_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(126, 108);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Exp:";
            // 
            // Exp_tb
            // 
            this.Exp_tb.Location = new System.Drawing.Point(129, 123);
            this.Exp_tb.Name = "Exp_tb";
            this.Exp_tb.Size = new System.Drawing.Size(89, 21);
            this.Exp_tb.TabIndex = 4;
            this.Exp_tb.TextChanged += new System.EventHandler(this.Exp_tb_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(17, 108);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(36, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Level:";
            // 
            // Level_tb
            // 
            this.Level_tb.Location = new System.Drawing.Point(19, 123);
            this.Level_tb.Name = "Level_tb";
            this.Level_tb.Size = new System.Drawing.Size(89, 21);
            this.Level_tb.TabIndex = 2;
            this.Level_tb.TextChanged += new System.EventHandler(this.Level_tb_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(17, 55);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(34, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Name";
            // 
            // Name_tb
            // 
            this.Name_tb.Location = new System.Drawing.Point(19, 70);
            this.Name_tb.Name = "Name_tb";
            this.Name_tb.Size = new System.Drawing.Size(199, 21);
            this.Name_tb.TabIndex = 0;
            this.Name_tb.TextChanged += new System.EventHandler(this.Name_tb_TextChanged);
            // 
            // Recipes_lb
            // 
            this.Recipes_lb.FormattingEnabled = true;
            this.Recipes_lb.Location = new System.Drawing.Point(6, 8);
            this.Recipes_lb.Name = "Recipes_lb";
            this.Recipes_lb.Size = new System.Drawing.Size(133, 277);
            this.Recipes_lb.TabIndex = 0;
            this.Recipes_lb.SelectedIndexChanged += new System.EventHandler(this.Recipes_lb_SelectedIndexChanged);
            // 
            // AddProf_button
            // 
            this.AddProf_button.Location = new System.Drawing.Point(11, 334);
            this.AddProf_button.Name = "AddProf_button";
            this.AddProf_button.Size = new System.Drawing.Size(35, 23);
            this.AddProf_button.TabIndex = 2;
            this.AddProf_button.Text = "Add";
            this.AddProf_button.UseVisualStyleBackColor = true;
            this.AddProf_button.Click += new System.EventHandler(this.AddProf_button_Click);
            // 
            // RemoveProf_button
            // 
            this.RemoveProf_button.Location = new System.Drawing.Point(132, 334);
            this.RemoveProf_button.Name = "RemoveProf_button";
            this.RemoveProf_button.Size = new System.Drawing.Size(55, 23);
            this.RemoveProf_button.TabIndex = 3;
            this.RemoveProf_button.Text = "Remove";
            this.RemoveProf_button.UseVisualStyleBackColor = true;
            this.RemoveProf_button.Click += new System.EventHandler(this.RemoveProf_button_Click);
            // 
            // CraftingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(838, 368);
            this.Controls.Add(this.RemoveProf_button);
            this.Controls.Add(this.AddProf_button);
            this.Controls.Add(this.Profs_tab);
            this.Controls.Add(this.Profs_lb);
            this.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "CraftingForm";
            this.ShowIcon = false;
            this.Text = "Crafting";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CraftingForm_FormClosed);
            this.Load += new System.EventHandler(this.CraftingForm_Load);
            this.Profs_tab.ResumeLayout(false);
            this.Prof_tab.ResumeLayout(false);
            this.Prof_tab.PerformLayout();
            this.Recipes_tab.ResumeLayout(false);
            this.Recipe_gb.ResumeLayout(false);
            this.Recipe_gb.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ListBox Profs_lb;
        private System.Windows.Forms.TabControl Profs_tab;
        private System.Windows.Forms.TabPage Recipes_tab;
        private System.Windows.Forms.GroupBox Recipe_gb;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox CraftTime_tb;
        private System.Windows.Forms.CheckBox StartRecipe_cb;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox CraftItem_cb;
        private System.Windows.Forms.Button RemoveIng_button;
        private System.Windows.Forms.Button AddIng_button;
        private System.Windows.Forms.ComboBox Ingredient_cb;
        private System.Windows.Forms.ListBox Ingredients_lb;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox Exp_tb;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox Level_tb;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox Name_tb;
        private System.Windows.Forms.ListBox Recipes_lb;
        private System.Windows.Forms.Button AddProf_button;
        private System.Windows.Forms.Button RemoveProf_button;
        private System.Windows.Forms.TabPage Prof_tab;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox ProfName_tb;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox ProfIndex_tb;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox Index_tb;
        private System.Windows.Forms.Button RemoveRecipe_button;
        private System.Windows.Forms.Button AddRecipe_button;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox IngredientCount_tb;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.ComboBox ProfType_cb;
    }
}