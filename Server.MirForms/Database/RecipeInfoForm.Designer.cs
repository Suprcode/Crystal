namespace Server.Database
{
    partial class RecipeInfoForm
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
            RecipeList = new ListBox();
            label1 = new Label();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            RecipeGroupBox = new GroupBox();
            GoldTextBox = new TextBox();
            ChanceTextBox = new TextBox();
            CraftAmountTextBox = new TextBox();
            ItemTextBox = new TextBox();
            label6 = new Label();
            ToolsGroupBox = new GroupBox();
            ToolTextBox = new TextBox();
            IngredientsGroupBox = new GroupBox();
            IngredientName4TextBox = new TextBox();
            IngredientName3TextBox = new TextBox();
            IngredientName2TextBox = new TextBox();
            IngredientName1TextBox = new TextBox();
            label7 = new Label();
            label5 = new Label();
            IngredientAmount4TextBox = new TextBox();
            IngredientAmount3TextBox = new TextBox();
            IngredientAmount2TextBox = new TextBox();
            IngredientAmount1TextBox = new TextBox();
            NewRecipeButton = new Button();
            RecipeGroupBox.SuspendLayout();
            ToolsGroupBox.SuspendLayout();
            IngredientsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // RecipeList
            // 
            RecipeList.FormattingEnabled = true;
            RecipeList.ItemHeight = 15;
            RecipeList.Location = new Point(12, 12);
            RecipeList.Name = "RecipeList";
            RecipeList.Size = new Size(135, 319);
            RecipeList.TabIndex = 0;
            RecipeList.SelectedIndexChanged += RecipeList_SelectedIndexChanged;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 42);
            label1.Name = "label1";
            label1.Size = new Size(89, 15);
            label1.TabIndex = 1;
            label1.Text = "Craft Amount : ";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(5, 71);
            label2.Name = "label2";
            label2.Size = new Size(69, 15);
            label2.TabIndex = 2;
            label2.Text = "Chance % : ";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(6, 99);
            label3.Name = "label3";
            label3.Size = new Size(68, 15);
            label3.TabIndex = 3;
            label3.Text = "Gold Cost : ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(6, 19);
            label4.Name = "label4";
            label4.Size = new Size(38, 15);
            label4.TabIndex = 4;
            label4.Text = "Tool : ";
            // 
            // RecipeGroupBox
            // 
            RecipeGroupBox.Controls.Add(GoldTextBox);
            RecipeGroupBox.Controls.Add(ChanceTextBox);
            RecipeGroupBox.Controls.Add(CraftAmountTextBox);
            RecipeGroupBox.Controls.Add(ItemTextBox);
            RecipeGroupBox.Controls.Add(label6);
            RecipeGroupBox.Controls.Add(label1);
            RecipeGroupBox.Controls.Add(label2);
            RecipeGroupBox.Controls.Add(label3);
            RecipeGroupBox.Location = new Point(153, 12);
            RecipeGroupBox.Name = "RecipeGroupBox";
            RecipeGroupBox.Size = new Size(200, 131);
            RecipeGroupBox.TabIndex = 6;
            RecipeGroupBox.TabStop = false;
            RecipeGroupBox.Text = "Recipe (Required)";
            // 
            // GoldTextBox
            // 
            GoldTextBox.Location = new Point(79, 99);
            GoldTextBox.Name = "GoldTextBox";
            GoldTextBox.Size = new Size(60, 23);
            GoldTextBox.TabIndex = 8;
            // 
            // ChanceTextBox
            // 
            ChanceTextBox.Location = new Point(79, 68);
            ChanceTextBox.Name = "ChanceTextBox";
            ChanceTextBox.Size = new Size(40, 23);
            ChanceTextBox.TabIndex = 7;
            // 
            // CraftAmountTextBox
            // 
            CraftAmountTextBox.Location = new Point(99, 42);
            CraftAmountTextBox.Name = "CraftAmountTextBox";
            CraftAmountTextBox.Size = new Size(40, 23);
            CraftAmountTextBox.TabIndex = 6;
            // 
            // ItemTextBox
            // 
            ItemTextBox.Location = new Point(50, 16);
            ItemTextBox.Name = "ItemTextBox";
            ItemTextBox.Size = new Size(121, 23);
            ItemTextBox.TabIndex = 5;
            ItemTextBox.TextChanged += ItemTextBox_TextChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(6, 19);
            label6.Name = "label6";
            label6.Size = new Size(40, 15);
            label6.TabIndex = 4;
            label6.Text = "Item : ";
            // 
            // ToolsGroupBox
            // 
            ToolsGroupBox.Controls.Add(ToolTextBox);
            ToolsGroupBox.Controls.Add(label4);
            ToolsGroupBox.Location = new Point(153, 149);
            ToolsGroupBox.Name = "ToolsGroupBox";
            ToolsGroupBox.Size = new Size(200, 47);
            ToolsGroupBox.TabIndex = 7;
            ToolsGroupBox.TabStop = false;
            ToolsGroupBox.Text = "Tools (Optional)";
            // 
            // ToolTextBox
            // 
            ToolTextBox.Location = new Point(50, 18);
            ToolTextBox.Name = "ToolTextBox";
            ToolTextBox.Size = new Size(113, 23);
            ToolTextBox.TabIndex = 20;
            // 
            // IngredientsGroupBox
            // 
            IngredientsGroupBox.Controls.Add(IngredientName4TextBox);
            IngredientsGroupBox.Controls.Add(IngredientName3TextBox);
            IngredientsGroupBox.Controls.Add(IngredientName2TextBox);
            IngredientsGroupBox.Controls.Add(IngredientName1TextBox);
            IngredientsGroupBox.Controls.Add(label7);
            IngredientsGroupBox.Controls.Add(label5);
            IngredientsGroupBox.Controls.Add(IngredientAmount4TextBox);
            IngredientsGroupBox.Controls.Add(IngredientAmount3TextBox);
            IngredientsGroupBox.Controls.Add(IngredientAmount2TextBox);
            IngredientsGroupBox.Controls.Add(IngredientAmount1TextBox);
            IngredientsGroupBox.Location = new Point(153, 202);
            IngredientsGroupBox.Name = "IngredientsGroupBox";
            IngredientsGroupBox.Size = new Size(200, 160);
            IngredientsGroupBox.TabIndex = 8;
            IngredientsGroupBox.TabStop = false;
            IngredientsGroupBox.Text = "Ingredients (Required)";
            // 
            // IngredientName4TextBox
            // 
            IngredientName4TextBox.Location = new Point(6, 132);
            IngredientName4TextBox.Name = "IngredientName4TextBox";
            IngredientName4TextBox.Size = new Size(113, 23);
            IngredientName4TextBox.TabIndex = 19;
            // 
            // IngredientName3TextBox
            // 
            IngredientName3TextBox.Location = new Point(6, 103);
            IngredientName3TextBox.Name = "IngredientName3TextBox";
            IngredientName3TextBox.Size = new Size(113, 23);
            IngredientName3TextBox.TabIndex = 18;
            // 
            // IngredientName2TextBox
            // 
            IngredientName2TextBox.Location = new Point(6, 74);
            IngredientName2TextBox.Name = "IngredientName2TextBox";
            IngredientName2TextBox.Size = new Size(113, 23);
            IngredientName2TextBox.TabIndex = 17;
            // 
            // IngredientName1TextBox
            // 
            IngredientName1TextBox.Location = new Point(6, 45);
            IngredientName1TextBox.Name = "IngredientName1TextBox";
            IngredientName1TextBox.Size = new Size(113, 23);
            IngredientName1TextBox.TabIndex = 16;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(15, 19);
            label7.Name = "label7";
            label7.Size = new Size(96, 15);
            label7.TabIndex = 15;
            label7.Text = "Ingredient Name";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(138, 19);
            label5.Name = "label5";
            label5.Size = new Size(51, 15);
            label5.TabIndex = 14;
            label5.Text = "Amount";
            // 
            // IngredientAmount4TextBox
            // 
            IngredientAmount4TextBox.Location = new Point(135, 132);
            IngredientAmount4TextBox.Name = "IngredientAmount4TextBox";
            IngredientAmount4TextBox.Size = new Size(59, 23);
            IngredientAmount4TextBox.TabIndex = 13;
            // 
            // IngredientAmount3TextBox
            // 
            IngredientAmount3TextBox.Location = new Point(135, 103);
            IngredientAmount3TextBox.Name = "IngredientAmount3TextBox";
            IngredientAmount3TextBox.Size = new Size(59, 23);
            IngredientAmount3TextBox.TabIndex = 12;
            // 
            // IngredientAmount2TextBox
            // 
            IngredientAmount2TextBox.Location = new Point(135, 74);
            IngredientAmount2TextBox.Name = "IngredientAmount2TextBox";
            IngredientAmount2TextBox.Size = new Size(59, 23);
            IngredientAmount2TextBox.TabIndex = 11;
            // 
            // IngredientAmount1TextBox
            // 
            IngredientAmount1TextBox.Location = new Point(135, 45);
            IngredientAmount1TextBox.Name = "IngredientAmount1TextBox";
            IngredientAmount1TextBox.Size = new Size(59, 23);
            IngredientAmount1TextBox.TabIndex = 10;
            // 
            // NewRecipeButton
            // 
            NewRecipeButton.Location = new Point(40, 337);
            NewRecipeButton.Name = "NewRecipeButton";
            NewRecipeButton.Size = new Size(75, 23);
            NewRecipeButton.TabIndex = 9;
            NewRecipeButton.Text = "New";
            NewRecipeButton.UseVisualStyleBackColor = true;
            NewRecipeButton.Click += NewRecipeButton_Click;
            // 
            // RecipeInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(360, 369);
            Controls.Add(NewRecipeButton);
            Controls.Add(IngredientsGroupBox);
            Controls.Add(ToolsGroupBox);
            Controls.Add(RecipeGroupBox);
            Controls.Add(RecipeList);
            Name = "RecipeInfoForm";
            Text = "RecipeInfoForm";
            FormClosing += RecipeInfoForm_FormClosing;
            RecipeGroupBox.ResumeLayout(false);
            RecipeGroupBox.PerformLayout();
            ToolsGroupBox.ResumeLayout(false);
            ToolsGroupBox.PerformLayout();
            IngredientsGroupBox.ResumeLayout(false);
            IngredientsGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private ListBox RecipeList;
        private Label label1;
        private Label label2;
        private Label label3;
        private Label label4;
        private GroupBox RecipeGroupBox;
        private GroupBox ToolsGroupBox;
        private GroupBox IngredientsGroupBox;
        private Label label5;
        private TextBox IngredientAmount4TextBox;
        private TextBox IngredientAmount3TextBox;
        private TextBox IngredientAmount2TextBox;
        private TextBox IngredientAmount1TextBox;
        private Label label6;
        private TextBox GoldTextBox;
        private TextBox ChanceTextBox;
        private TextBox CraftAmountTextBox;
        private TextBox ItemTextBox;
        private Label label7;
        private TextBox ToolTextBox;
        private TextBox IngredientName4TextBox;
        private TextBox IngredientName3TextBox;
        private TextBox IngredientName2TextBox;
        private TextBox IngredientName1TextBox;
        private Button NewRecipeButton;
    }
}