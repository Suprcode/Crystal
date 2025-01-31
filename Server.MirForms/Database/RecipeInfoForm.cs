using System;
using System.IO;
using System.Windows.Forms;

namespace Server.Database
{
    public partial class RecipeInfoForm : Form
    {
        private string currentFilePath;
        private bool isModified = false;
        private bool _ignoreItemSelectionEvent = false;

        public RecipeInfoForm()
        {
            InitializeComponent();
            this.Load += RecipeInfoForm_Load;
            SaveButton.Click += SaveButton_Click;
        }

        #region Form Load
        private void RecipeInfoForm_Load(object sender, EventArgs e)
        {
            // Define the directory path where the recipes are stored
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(currentDirectory, "Envir", "Recipe");

            // Clear the selected item in the ItemComboBox on load
            ItemComboBox.SelectedIndex = -1;  // No item selected initially

            // Check if the directory exists
            if (Directory.Exists(directoryPath))
            {
                // Get all recipe files (with .txt extension) from the directory
                string[] recipeFiles = Directory.GetFiles(directoryPath, "*.txt");

                // Clear existing items in the RecipeList (if any)
                RecipeList.Items.Clear();

                // Add recipe filenames to the RecipeList
                foreach (string recipeFile in recipeFiles)
                {
                    // Get the filename without the extension
                    string fileNameWithoutExtension = Path.GetFileNameWithoutExtension(recipeFile);
                    RecipeList.Items.Add(fileNameWithoutExtension);
                }
            }
            else
            {
                MessageBox.Show("The recipe directory does not exist.", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            LoadItemsIntoComboBox();
            UpdateRecipeCount();
        }
        #endregion

        #region Recipe Count
        private void UpdateRecipeCount()
        {
            // Define the directory path where the recipes are stored
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(currentDirectory, "Envir", "Recipe");

            int recipeCount = 0;

            // Check if the directory exists
            if (Directory.Exists(directoryPath))
            {
                // Count all recipe files with a .txt extension
                string[] recipeFiles = Directory.GetFiles(directoryPath, "*.txt");
                recipeCount = recipeFiles.Length;
            }

            // Update the RecipeCountLabel with the total count
            RecipeCountLabel.Text = $"Recipe Count: {recipeCount}";
        }
        #endregion

        #region Recipe List Index Changed
        private void RecipeList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RecipeList.SelectedIndex == -1)
                return;

            // Get the selected recipe file name
            string selectedRecipeName = RecipeList.SelectedItem.ToString();
            LoadRecipe(selectedRecipeName);
        }
        #endregion

        #region Clear Recipe 
        private void ClearRecipeForm()
        {
            // Clear Craft Amount, Chance, and Gold fields
            CraftAmountTextBox.Text = string.Empty;
            ChanceTextBox.Text = string.Empty;
            GoldTextBox.Text = string.Empty;

            // Clear Ingredient Name ComboBoxes
            IngredientName1ComboBox.SelectedIndex = -1;
            IngredientName2ComboBox.SelectedIndex = -1;
            IngredientName3ComboBox.SelectedIndex = -1;
            IngredientName4ComboBox.SelectedIndex = -1;
            IngredientName5ComboBox.SelectedIndex = -1;
            IngredientName6ComboBox.SelectedIndex = -1;

            // Clear Ingredient Amount and Durability TextBoxes
            IngredientAmount1TextBox.Text = string.Empty;
            IngredientAmount2TextBox.Text = string.Empty;
            IngredientAmount3TextBox.Text = string.Empty;
            IngredientAmount4TextBox.Text = string.Empty;
            IngredientAmount5TextBox.Text = string.Empty;
            IngredientAmount6TextBox.Text = string.Empty;

            IngredientDura1TextBox.Text = string.Empty;
            IngredientDura2TextBox.Text = string.Empty;
            IngredientDura3TextBox.Text = string.Empty;
            IngredientDura4TextBox.Text = string.Empty;
            IngredientDura5TextBox.Text = string.Empty;
            IngredientDura6TextBox.Text = string.Empty;
        }
        #endregion

        #region Load Recipes
        private void LoadRecipe(string recipeName)
        {
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(currentDirectory, "Envir", "Recipe");
            string filePath = Path.Combine(directoryPath, recipeName + ".txt");

            // Check if the file exists
            if (File.Exists(filePath))
            {
                // Read the lines from the recipe file and parse as needed
                string[] fileLines = File.ReadAllLines(filePath);
                ParseAndDisplayRecipe(fileLines);

                // Temporarily ignore the combo box event while we select the item
                _ignoreItemSelectionEvent = true;
                try
                {
                    SetItemComboBoxSelection(recipeName);
                }
                finally
                {
                    _ignoreItemSelectionEvent = false;
                }

                // Set isModified to true since the recipe was loaded and might be changed
                isModified = true;
                currentFilePath = filePath; // Store the current file path for later rename operations
            }
            else
            {
                MessageBox.Show("The selected recipe file does not exist.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Item Combo Box
        private void SetItemComboBoxSelection(string recipeName)
        {
            // Look for an item name matching the recipe
            foreach (var item in SMain.EditEnvir.ItemInfoList)
            {
                if (item.Name.Equals(recipeName, StringComparison.OrdinalIgnoreCase))
                {
                    // Programmatically set the ComboBox
                    ItemComboBox.SelectedItem = item.Name;
                    break;
                }
            }
        }
        #endregion

        #region Parse Recipe
        private void ParseAndDisplayRecipe(string[] fileLines)
        {
            // Initialise default values
            string amount = "";
            string chance = "";
            string gold = "";

            // Tool data
            string[] tools = new string[3];

            // Ingredient data
            string[] ingredientNames = new string[6];
            string[] ingredientAmounts = new string[6];
            string[] ingredientDurabilities = new string[6];

            // Flags to track current section
            bool inToolsSection = false;
            bool inIngredientsSection = false;

            int toolIndex = 0;
            int ingredientIndex = 0;

            foreach (string line in fileLines)
            {
                // Skip empty lines
                if (string.IsNullOrWhiteSpace(line)) continue;

                // Detect [Recipe] section (skip header)
                if (line.StartsWith("[Recipe]"))
                {
                    inToolsSection = false;
                    inIngredientsSection = false;
                    continue;
                }

                // Parse key-value pairs for [Recipe] section
                if (line.StartsWith("Amount ") || line.StartsWith("Chance ") || line.StartsWith("Gold "))
                {
                    string[] parts = line.Split(new[] { ' ' }, 2);
                    if (parts.Length == 2)
                    {
                        string key = parts[0].Trim();
                        string value = parts[1].Trim();

                        if (key == "Amount") amount = value;
                        else if (key == "Chance") chance = value;
                        else if (key == "Gold") gold = value;
                    }
                    continue;
                }

                // Detect [Tools] section
                if (line.StartsWith("[Tools]"))
                {
                    inToolsSection = true;
                    inIngredientsSection = false;
                    continue;
                }

                // Detect [Ingredients] section
                if (line.StartsWith("[Ingredients]"))
                {
                    inToolsSection = false;
                    inIngredientsSection = true;
                    continue;
                }

                // Parse tools (only in [Tools] section)
                if (inToolsSection && toolIndex < 3)
                {
                    tools[toolIndex++] = line.Trim();
                    continue;
                }

                // Parse ingredients (only in [Ingredients] section)
                if (inIngredientsSection && ingredientIndex < 6)
                {
                    string[] ingredientParts = line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

                    if (ingredientParts.Length >= 1)
                    {
                        ingredientNames[ingredientIndex] = ingredientParts[0].Trim();

                        if (ingredientParts.Length >= 2)
                            ingredientAmounts[ingredientIndex] = ingredientParts[1].Trim();

                        if (ingredientParts.Length == 3)
                            ingredientDurabilities[ingredientIndex] = ingredientParts[2].Trim();

                        ingredientIndex++;
                    }
                }
            }

            // Update text boxes with parsed values
            CraftAmountTextBox.Text = amount;
            ChanceTextBox.Text = chance;
            GoldTextBox.Text = gold;

            // Populate tool ComboBoxes
            PopulateToolComboBox(Tool1ComboBox, tools[0]);
            PopulateToolComboBox(Tool2ComboBox, tools[1]);
            PopulateToolComboBox(Tool3ComboBox, tools[2]);

            // Populate ingredient ComboBoxes
            PopulateIngredientComboBox(IngredientName1ComboBox, ingredientNames[0]);
            PopulateIngredientComboBox(IngredientName2ComboBox, ingredientNames[1]);
            PopulateIngredientComboBox(IngredientName3ComboBox, ingredientNames[2]);
            PopulateIngredientComboBox(IngredientName4ComboBox, ingredientNames[3]);
            PopulateIngredientComboBox(IngredientName5ComboBox, ingredientNames[4]);
            PopulateIngredientComboBox(IngredientName6ComboBox, ingredientNames[5]);

            // Populate ingredient amount and durability text boxes
            IngredientAmount1TextBox.Text = ingredientAmounts[0];
            IngredientAmount2TextBox.Text = ingredientAmounts[1];
            IngredientAmount3TextBox.Text = ingredientAmounts[2];
            IngredientAmount4TextBox.Text = ingredientAmounts[3];
            IngredientAmount5TextBox.Text = ingredientAmounts[4];
            IngredientAmount6TextBox.Text = ingredientAmounts[5];

            IngredientDura1TextBox.Text = ingredientDurabilities[0];
            IngredientDura2TextBox.Text = ingredientDurabilities[1];
            IngredientDura3TextBox.Text = ingredientDurabilities[2];
            IngredientDura4TextBox.Text = ingredientDurabilities[3];
            IngredientDura5TextBox.Text = ingredientDurabilities[4];
            IngredientDura6TextBox.Text = ingredientDurabilities[5];
        }
        #endregion

        #region Populate Tools
        private void PopulateToolComboBox(ComboBox toolComboBox, string toolName)
        {
            // Clear the ComboBox before adding new items
            toolComboBox.Items.Clear();

            // Add "None" as the first item
            toolComboBox.Items.Add("None");

            // Add items from the item database
            foreach (var item in SMain.EditEnvir.ItemInfoList)
            {
                if (!string.IsNullOrEmpty(item.Name))
                {
                    toolComboBox.Items.Add(item.Name);
                }
            }

            // Set the selected item if the tool name is valid
            if (!string.IsNullOrEmpty(toolName))
            {
                toolComboBox.SelectedItem = toolName;
            }
            else
            {
                toolComboBox.SelectedIndex = 0; // Default to "None"
            }
        }
        #endregion

        #region Populate Ingredients
        private void PopulateIngredientComboBox(ComboBox comboBox, string ingredientName)
        {
            comboBox.Items.Clear(); // Clear the ComboBox first

            // Add "None" as the first item
            comboBox.Items.Add("None");

            // Ensure ItemInfoList is populated
            if (SMain.EditEnvir.ItemInfoList != null && SMain.EditEnvir.ItemInfoList.Count > 0)
            {
                // Add item names to the ComboBox
                foreach (var item in SMain.EditEnvir.ItemInfoList)
                {
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        comboBox.Items.Add(item.Name);
                    }
                }

                // Set the selected item to the ingredient name (if it exists in the list)
                if (!string.IsNullOrEmpty(ingredientName) && comboBox.Items.Contains(ingredientName))
                {
                    comboBox.SelectedItem = ingredientName;
                }
                else
                {
                    comboBox.SelectedIndex = 0; // Default to "None"
                }
            }
            else
            {
                MessageBox.Show("No items found in the ItemInfoList.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Load Items
        private void LoadItemsIntoComboBox()
        {
            // Ensure ItemInfoList is populated
            if (SMain.EditEnvir.ItemInfoList != null && SMain.EditEnvir.ItemInfoList.Count > 0)
            {
                ItemComboBox.Items.Clear();

                // Loop through ItemInfoList and add item names to the combo box
                foreach (var item in SMain.EditEnvir.ItemInfoList)
                {
                    if (!string.IsNullOrEmpty(item.Name))
                    {
                        ItemComboBox.Items.Add(item.Name);
                    }
                }

                // Optionally set the first item as selected if the ComboBox is not empty
                if (ItemComboBox.Items.Count > 0)
                {
                    ItemComboBox.SelectedIndex = -1;  // No item selected initially
                }
            }
            else
            {
                MessageBox.Show("No items found in the ItemInfoList.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }
        #endregion

        #region Save Recipe
        private void SaveRecipe()
        {
            if (string.IsNullOrEmpty(currentFilePath)) return;

            try
            {
                using (StreamWriter writer = new StreamWriter(currentFilePath))
                {
                    // Write [Recipe] section
                    writer.WriteLine("[Recipe]");
                    writer.WriteLine($"Amount {CraftAmountTextBox.Text}");
                    writer.WriteLine($"Chance {ChanceTextBox.Text}");
                    writer.WriteLine($"Gold {GoldTextBox.Text}");

                    writer.WriteLine();
                    writer.WriteLine("[Tools]");

                    // Write tools, skipping None
                    if (Tool1ComboBox.SelectedItem.ToString() != "None")
                        writer.WriteLine(Tool1ComboBox.SelectedItem.ToString());
                    if (Tool2ComboBox.SelectedItem.ToString() != "None")
                        writer.WriteLine(Tool2ComboBox.SelectedItem.ToString());
                    if (Tool3ComboBox.SelectedItem.ToString() != "None")
                        writer.WriteLine(Tool3ComboBox.SelectedItem.ToString());

                    writer.WriteLine();
                    writer.WriteLine("[Ingredients]");

                    // Write ingredients, skipping None
                    WriteIngredient(writer, IngredientName1ComboBox, IngredientAmount1TextBox, IngredientDura1TextBox);
                    WriteIngredient(writer, IngredientName2ComboBox, IngredientAmount2TextBox, IngredientDura2TextBox);
                    WriteIngredient(writer, IngredientName3ComboBox, IngredientAmount3TextBox, IngredientDura3TextBox);
                    WriteIngredient(writer, IngredientName4ComboBox, IngredientAmount4TextBox, IngredientDura4TextBox);
                    WriteIngredient(writer, IngredientName5ComboBox, IngredientAmount5TextBox, IngredientDura5TextBox);
                    WriteIngredient(writer, IngredientName6ComboBox, IngredientAmount6TextBox, IngredientDura6TextBox);
                }

                MessageBox.Show("Recipe saved successfully.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                isModified = false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving recipe: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void WriteIngredient(StreamWriter writer, ComboBox nameComboBox, TextBox amountTextBox, TextBox duraTextBox)
        {
            if (nameComboBox.SelectedItem.ToString() == "None") return; // Skip if None is selected

            string name = nameComboBox.SelectedItem.ToString();
            string amount = string.IsNullOrWhiteSpace(amountTextBox.Text) ? "" : amountTextBox.Text;
            string dura = string.IsNullOrWhiteSpace(duraTextBox.Text) ? "" : duraTextBox.Text;

            // Write ingredient line with optional amount and durability
            writer.WriteLine($"{name} {amount} {dura}".Trim());
        }
        #endregion

        #region Form Close
        private void RecipeInfoForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Prompt to save only if isModified is true
            if (isModified)
            {
                // You could prompt before auto-saving, but here we'll just save
                SaveRecipe();
            }
        }
        #endregion

        #region New Recipe Button
        private void NewRecipeButton_Click(object sender, EventArgs e)
        {
            // Define the directory path where the recipes are stored
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(currentDirectory, "Envir", "Recipe");

            // Ensure the directory exists
            if (!Directory.Exists(directoryPath))
            {
                MessageBox.Show("The recipe directory does not exist.", "Directory Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Generate a unique file name for the new recipe
            string newRecipeName = "NewRecipe";
            string newRecipePath = Path.Combine(directoryPath, $"{newRecipeName}.txt");
            int counter = 1;

            // Avoid overwriting existing files by appending a number to the name if needed
            while (File.Exists(newRecipePath))
            {
                newRecipeName = $"NewRecipe{counter}";
                newRecipePath = Path.Combine(directoryPath, $"{newRecipeName}.txt");
                counter++;
            }

            // Create a blank recipe file with the specified layout
            try
            {
                using (StreamWriter writer = new StreamWriter(newRecipePath))
                {
                    writer.WriteLine("[Recipe]");
                    writer.WriteLine("Amount ");
                    writer.WriteLine("Chance ");
                    writer.WriteLine("Gold ");
                    writer.WriteLine();
                    writer.WriteLine("[Tools]");
                    writer.WriteLine();
                    writer.WriteLine("[Ingredients]");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create the new recipe file. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            // Update the form to display the new recipe
            RecipeList.Items.Add(newRecipeName);
            RecipeList.SelectedItem = newRecipeName; // Automatically select the new recipe

            // Reset all combo boxes and text boxes to their default values
            ItemComboBox.SelectedIndex = 0; // "None"
            CraftAmountTextBox.Text = string.Empty;
            ChanceTextBox.Text = string.Empty;
            GoldTextBox.Text = string.Empty;

            Tool1ComboBox.SelectedIndex = 0; // "None"
            Tool2ComboBox.SelectedIndex = 0; // "None"
            Tool3ComboBox.SelectedIndex = 0; // "None"

            IngredientName1ComboBox.SelectedIndex = 0; // "None"
            IngredientName2ComboBox.SelectedIndex = 0; // "None"
            IngredientName3ComboBox.SelectedIndex = 0; // "None"
            IngredientName4ComboBox.SelectedIndex = 0; // "None"
            IngredientName5ComboBox.SelectedIndex = 0; // "None"
            IngredientName6ComboBox.SelectedIndex = 0; // "None"

            IngredientAmount1TextBox.Text = string.Empty;
            IngredientAmount2TextBox.Text = string.Empty;
            IngredientAmount3TextBox.Text = string.Empty;
            IngredientAmount4TextBox.Text = string.Empty;
            IngredientAmount5TextBox.Text = string.Empty;
            IngredientAmount6TextBox.Text = string.Empty;

            IngredientDura1TextBox.Text = string.Empty;
            IngredientDura2TextBox.Text = string.Empty;
            IngredientDura3TextBox.Text = string.Empty;
            IngredientDura4TextBox.Text = string.Empty;
            IngredientDura5TextBox.Text = string.Empty;
            IngredientDura6TextBox.Text = string.Empty;

            // Update the current file path and set the modified flag
            currentFilePath = newRecipePath;
            isModified = true;

            MessageBox.Show($"New recipe created: {newRecipeName}.txt", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            UpdateRecipeCount();
        }
        #endregion

        #region Item Combo Box Index Change Event
        private void ItemComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            // If we're currently ignoring this event (because we're loading), do nothing
            if (_ignoreItemSelectionEvent) return;

            // If there's no file loaded, do nothing
            if (string.IsNullOrEmpty(currentFilePath)) return;

            string selectedItemName = ItemComboBox.SelectedItem?.ToString();
            if (string.IsNullOrEmpty(selectedItemName)) return;

            // Compare old vs new
            string oldRecipeName = Path.GetFileNameWithoutExtension(currentFilePath);
            string oldTrimmed = oldRecipeName.Trim();
            string newTrimmed = selectedItemName.Trim();

            // If the name is effectively the same, skip
            if (oldTrimmed.Equals(newTrimmed, StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            // Otherwise rename
            UpdateRecipeFileName(selectedItemName);

            // Update the list item if something was actually renamed
            if (RecipeList.SelectedIndex >= 0)
            {
                RecipeList.Items[RecipeList.SelectedIndex] = selectedItemName;
            }

            isModified = true;
        }
        #endregion

        #region Update Recipe File Name
        private void UpdateRecipeFileName(string newItemName)
        {
            string directoryPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Envir", "Recipe");
            string newFilePath = Path.Combine(directoryPath, newItemName + ".txt");

            // If both paths match (ignoring case), skip the rename
            if (currentFilePath.Equals(newFilePath, StringComparison.OrdinalIgnoreCase))
                return;

            // If a different file with that new name already exists, warn and skip
            if (File.Exists(newFilePath))
            {
                MessageBox.Show(
                    $"A recipe named '{newItemName}' already exists. Cannot rename.",
                    "Rename Error",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning
                );
                return;
            }

            // Rename the existing file
            if (File.Exists(currentFilePath))
            {
                try
                {
                    File.Move(currentFilePath, newFilePath);
                    currentFilePath = newFilePath;
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error renaming recipe file: {ex.Message}",
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        #endregion

        #region Open Recipe Button
        private void OpenRecipeButton_Click(object sender, EventArgs e)
        {
            // Check if a recipe has been selected from the RecipeList
            if (RecipeList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a recipe from the list.", "No Recipe Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected recipe file name
            string selectedRecipeName = RecipeList.SelectedItem.ToString();
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(currentDirectory, "Envir", "Recipe");
            string filePath = Path.Combine(directoryPath, selectedRecipeName + ".txt");

            // Check if the recipe file exists
            if (File.Exists(filePath))
            {
                try
                {
                    // Use Process.Start with the file path
                    System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                    {
                        FileName = filePath,
                        UseShellExecute = true // Use the default system shell to open
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error opening file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else
            {
                MessageBox.Show("The selected recipe file does not exist.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion

        #region Save Button
        private void SaveButton_Click(object sender, EventArgs e)
        {
            SaveRecipe();
        }
        #endregion

        #region Delete Button
        private void DeleteButton_Click(object sender, EventArgs e)
        {
            // Check if a recipe is selected
            if (RecipeList.SelectedIndex == -1)
            {
                MessageBox.Show("Please select a recipe to delete.", "No Recipe Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Get the selected recipe name
            string selectedRecipeName = RecipeList.SelectedItem.ToString();
            string currentDirectory = AppDomain.CurrentDomain.BaseDirectory;
            string directoryPath = Path.Combine(currentDirectory, "Envir", "Recipe");
            string filePath = Path.Combine(directoryPath, $"{selectedRecipeName}.txt");

            // Confirm deletion
            var result = MessageBox.Show(
                $"Are you sure you want to delete the recipe: {selectedRecipeName}?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Question
            );
            if (result != DialogResult.Yes) return;

            // Delete the file
            try
            {
                if (File.Exists(filePath))
                {
                    File.Delete(filePath);
                    RecipeList.Items.Remove(selectedRecipeName); // Remove from the list
                    MessageBox.Show($"Recipe deleted: {selectedRecipeName}.txt", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                    // Clear the form and reset controls
                    ClearForm();
                }
                else
                {
                    MessageBox.Show("The selected recipe file does not exist.", "File Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to delete the recipe file. Error: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            // Update the recipe count
            UpdateRecipeCount();
        }

        // Clears the form fields
        private void ClearForm()
        {
            RecipeList.SelectedIndex = -1; // No recipe selected
            ItemComboBox.SelectedIndex = 0; // Reset to "None"
            CraftAmountTextBox.Text = string.Empty;
            ChanceTextBox.Text = string.Empty;
            GoldTextBox.Text = string.Empty;

            Tool1ComboBox.SelectedIndex = 0; // Reset to "None"
            Tool2ComboBox.SelectedIndex = 0;
            Tool3ComboBox.SelectedIndex = 0;

            IngredientName1ComboBox.SelectedIndex = 0; // Reset to "None"
            IngredientName2ComboBox.SelectedIndex = 0;
            IngredientName3ComboBox.SelectedIndex = 0;
            IngredientName4ComboBox.SelectedIndex = 0;
            IngredientName5ComboBox.SelectedIndex = 0;
            IngredientName6ComboBox.SelectedIndex = 0;

            IngredientAmount1TextBox.Text = string.Empty;
            IngredientAmount2TextBox.Text = string.Empty;
            IngredientAmount3TextBox.Text = string.Empty;
            IngredientAmount4TextBox.Text = string.Empty;
            IngredientAmount5TextBox.Text = string.Empty;
            IngredientAmount6TextBox.Text = string.Empty;

            IngredientDura1TextBox.Text = string.Empty;
            IngredientDura2TextBox.Text = string.Empty;
            IngredientDura3TextBox.Text = string.Empty;
            IngredientDura4TextBox.Text = string.Empty;
            IngredientDura5TextBox.Text = string.Empty;
            IngredientDura6TextBox.Text = string.Empty;
        }

#endregion
    }
}