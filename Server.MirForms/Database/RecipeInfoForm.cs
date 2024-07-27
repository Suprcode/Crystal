using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Database
{
    public partial class RecipeInfoForm : Form
    {
        private string currentFilePath;
        private bool isModified = false;
        private string originalCraftAmount;
        private string originalChance;
        private string originalGold;
        private string originalTool;
        private string originalIngredientName1;
        private string originalIngredientAmount1;
        private string originalIngredientName2;
        private string originalIngredientAmount2;
        private string originalIngredientName3;
        private string originalIngredientAmount3;
        private string originalIngredientName4;
        private string originalIngredientAmount4;
        public RecipeInfoForm()
        {
            InitializeComponent();
            this.Load += RecipeInfoForm_Load;
            RecipeList.SelectedIndexChanged += RecipeList_SelectedIndexChanged;

            #region Text Box Changed
            CraftAmountTextBox.TextChanged += TextBox_TextChanged;
            ChanceTextBox.TextChanged += TextBox_TextChanged;
            GoldTextBox.TextChanged += TextBox_TextChanged;
            ToolTextBox.TextChanged += TextBox_TextChanged;
            IngredientName1TextBox.TextChanged += TextBox_TextChanged;
            IngredientAmount1TextBox.TextChanged += TextBox_TextChanged;
            IngredientName2TextBox.TextChanged += TextBox_TextChanged;
            IngredientAmount2TextBox.TextChanged += TextBox_TextChanged;
            IngredientName3TextBox.TextChanged += TextBox_TextChanged;
            IngredientAmount3TextBox.TextChanged += TextBox_TextChanged;
            IngredientName4TextBox.TextChanged += TextBox_TextChanged;
            IngredientAmount4TextBox.TextChanged += TextBox_TextChanged;
            #endregion
        }
        #region Form Load
        private void RecipeInfoForm_Load(object sender, EventArgs e)
        {
            // Define the path to the directory containing recipes
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(currentDirectory, "Envir", "Recipe");

            // Ensure the directory exists
            if (Directory.Exists(directoryPath))
            {
                // Get all recipe files from the directory
                string[] recipeFiles = Directory.GetFiles(directoryPath);

                // Clear existing items from ListBox
                RecipeList.Items.Clear();

                // Populate the ListBox with recipe file names with index
                for (int i = 0; i < recipeFiles.Length; i++)
                {
                    // Get the file name without the path and without the extension
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(recipeFiles[i]);

                    // Format the item with an index number
                    string listItem = $"{i + 1}. {fileNameWithoutExtension}";

                    // Add the formatted item to the ListBox
                    RecipeList.Items.Add(listItem);
                }
            }
            else
            {
                MessageBox.Show("The recipe directory does not exist.", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Recipe Selected Index Changed
        private void RecipeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RecipeList.SelectedIndex == -1)
            {
                ClearTextBoxes();
                return;
            }

            if (isModified)
            {
                SaveRecipe();
            }

            string selectedItem = RecipeList.SelectedItem.ToString();
            string fileNameWithoutExtension = selectedItem.Substring(selectedItem.IndexOf(' ') + 1);

            ItemTextBox.Text = fileNameWithoutExtension;
            currentFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Envir", "Recipe", fileNameWithoutExtension + ".txt");

            if (File.Exists(currentFilePath))
            {
                string[] fileLines = File.ReadAllLines(currentFilePath);
                ParseAndDisplayRecipe(fileLines);
            }
            else
            {
                MessageBox.Show("The selected recipe file does not exist.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Parse and Display Recipe file
        private void ParseAndDisplayRecipe(string[] fileLines)
        {
            string currentSection = "";
            int ingredientIndex = 1;

            // Clear previous data
            ClearTextBoxes();

            foreach (string line in fileLines)
            {
                if (string.IsNullOrWhiteSpace(line))
                {
                    continue;
                }

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    currentSection = line;
                    continue;
                }

                switch (currentSection)
                {
                    case "[Recipe]":
                        var recipeParts = line.Split(new[] { ' ' }, 2);
                        if (recipeParts.Length == 2)
                        {
                            string key = recipeParts[0].Trim();
                            string value = recipeParts[1].Trim();

                            if (key == "Amount")
                            {
                                CraftAmountTextBox.Text = value;
                            }
                            else if (key == "Chance")
                            {
                                ChanceTextBox.Text = value;
                            }
                            else if (key == "Gold")
                            {
                                GoldTextBox.Text = value;
                            }
                        }
                        break;

                    case "[Tools]":
                        ToolTextBox.Text = line.Trim();
                        break;

                    case "[Ingredients]":
                        string[] parts = line.Split(new[] { ' ' }, 2);
                        string ingredientName = parts[0].Trim();
                        string ingredientAmount = parts.Length > 1 ? parts[1].Trim() : "";

                        switch (ingredientIndex)
                        {
                            case 1:
                                IngredientName1TextBox.Text = ingredientName;
                                IngredientAmount1TextBox.Text = ingredientAmount;
                                break;
                            case 2:
                                IngredientName2TextBox.Text = ingredientName;
                                IngredientAmount2TextBox.Text = ingredientAmount;
                                break;
                            case 3:
                                IngredientName3TextBox.Text = ingredientName;
                                IngredientAmount3TextBox.Text = ingredientAmount;
                                break;
                            case 4:
                                IngredientName4TextBox.Text = ingredientName;
                                IngredientAmount4TextBox.Text = ingredientAmount;
                                break;
                        }

                        ingredientIndex++;
                        break;
                }
            }
        }
        #endregion

        #region Clear Text Boxes
        private void ClearTextBoxes()
        {
            CraftAmountTextBox.Clear();
            ChanceTextBox.Clear();
            GoldTextBox.Clear();
            ToolTextBox.Clear();
            IngredientName1TextBox.Clear();
            IngredientAmount1TextBox.Clear();
            IngredientName2TextBox.Clear();
            IngredientAmount2TextBox.Clear();
            IngredientName3TextBox.Clear();
            IngredientAmount3TextBox.Clear();
            IngredientName4TextBox.Clear();
            IngredientAmount4TextBox.Clear();
        }
        #endregion

        #region Save Recipe
        private void SaveRecipe()
        {
            if (string.IsNullOrEmpty(currentFilePath))
            {
                return;
            }

            using (StreamWriter writer = new StreamWriter(currentFilePath))
            {
                writer.WriteLine("[Recipe]");
                writer.WriteLine($"Amount {CraftAmountTextBox.Text}");
                writer.WriteLine($"Chance {ChanceTextBox.Text}");
                writer.WriteLine($"Gold {GoldTextBox.Text}");
                writer.WriteLine();

                writer.WriteLine("[Tools]");
                writer.WriteLine(ToolTextBox.Text);
                writer.WriteLine();

                writer.WriteLine("[Ingredients]");
                if (!string.IsNullOrEmpty(IngredientName1TextBox.Text))
                {
                    writer.WriteLine($"{IngredientName1TextBox.Text} {IngredientAmount1TextBox.Text}");
                }
                if (!string.IsNullOrEmpty(IngredientName2TextBox.Text))
                {
                    writer.WriteLine($"{IngredientName2TextBox.Text} {IngredientAmount2TextBox.Text}");
                }
                if (!string.IsNullOrEmpty(IngredientName3TextBox.Text))
                {
                    writer.WriteLine($"{IngredientName3TextBox.Text} {IngredientAmount3TextBox.Text}");
                }
                if (!string.IsNullOrEmpty(IngredientName4TextBox.Text))
                {
                    writer.WriteLine($"{IngredientName4TextBox.Text} {IngredientAmount4TextBox.Text}");
                }
            }

            // Mark as not modified
            isModified = false;
        }

        private void RecipeInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (isModified)
            {
                SaveRecipe();
            }
        }
        #endregion

        #region Text Box Change Events
        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            // Check if any text box value has changed from its original value
            if (CraftAmountTextBox.Text != originalCraftAmount ||
                ChanceTextBox.Text != originalChance ||
                GoldTextBox.Text != originalGold ||
                ToolTextBox.Text != originalTool ||
                IngredientName1TextBox.Text != originalIngredientName1 ||
                IngredientAmount1TextBox.Text != originalIngredientAmount1 ||
                IngredientName2TextBox.Text != originalIngredientName2 ||
                IngredientAmount2TextBox.Text != originalIngredientAmount2 ||
                IngredientName3TextBox.Text != originalIngredientName3 ||
                IngredientAmount3TextBox.Text != originalIngredientAmount3 ||
                IngredientName4TextBox.Text != originalIngredientName4 ||
                IngredientAmount4TextBox.Text != originalIngredientAmount4)
            {
                isModified = true;
            }
        }
        #endregion

        #region Create new Recipe
        private const string TempFileName = "UntitledRecipe.txt";
        private void NewRecipeButton_Click(object sender, EventArgs e)
        {
            // Define the path to the directory containing recipes
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(currentDirectory, "Envir", "Recipe");

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("The recipe directory does not exist.", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Get the filename from ItemTextBox and prepare full filename
            string displayFileName = ItemTextBox.Text.Trim();
            string fileName = string.IsNullOrEmpty(displayFileName) ? TempFileName : $"{displayFileName}.txt";

            // Path to the file
            string filePath = Path.Combine(directoryPath, fileName);

            // Check if file already exists
            if (File.Exists(filePath) && fileName != TempFileName)
            {
                MessageBox.Show("A file with this name already exists.", "File Exists", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Create an empty text file at the generated path
            File.Create(filePath).Close(); // .Create() creates the file, .Close() closes the file handle

            // Update the ListBox to include the new recipe file
            string newItem = $"{RecipeList.Items.Count + 1}. {Path.GetFileNameWithoutExtension(fileName)}";
            RecipeList.Items.Add(newItem);

            // Select the new item in the ListBox
            RecipeList.SelectedIndex = RecipeList.Items.Count - 1;

            // Set the ItemTextBox to the new filename without extension if it was initially temporary
            if (fileName == TempFileName)
            {
                ItemTextBox.Text = Path.GetFileNameWithoutExtension(fileName);
            }
        }
        #endregion

        #region Name Text Box
        private bool isUpdatingTextBox = false;
        private void ItemTextBox_TextChanged(object sender, EventArgs e)
        {
            // Ensure there's a selected item in the ListBox
            if (RecipeList.SelectedIndex == -1)
                return;

            // Get the new display filename from ItemTextBox
            string newDisplayName = ItemTextBox.Text.Trim();
            if (string.IsNullOrEmpty(newDisplayName))
            {
                MessageBox.Show("Filename cannot be empty.", "Invalid Filename", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Prepare full filename with extension
            string newFileName = $"{newDisplayName}.txt";
            string oldDisplayName = RecipeList.SelectedItem.ToString().Substring(3).Trim(); // Extract old display name
            string oldFileName = $"{oldDisplayName}.txt";

            // Paths for old and new files
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Envir", "Recipe");
            string oldFilePath = Path.Combine(directoryPath, oldFileName);
            string newFilePath = Path.Combine(directoryPath, newFileName);

            // Check if the new filename already exists
            if (File.Exists(newFilePath))
            {
                // Revert the ItemTextBox to the old filename
                ItemTextBox.Text = oldDisplayName;
                return;
            }

            // Rename the file if needed
            if (oldFileName != newFileName && File.Exists(oldFilePath))
            {
                try
                {
                    File.Move(oldFilePath, newFilePath);

                    // Update ListBox item
                    RecipeList.Items[RecipeList.SelectedIndex] = $"{RecipeList.SelectedIndex + 1}. {newDisplayName}";
                }
                catch (IOException ex)
                {
                    MessageBox.Show($"An error occurred while renaming the file: {ex.Message}", "Rename Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            #endregion
        }
    }
}