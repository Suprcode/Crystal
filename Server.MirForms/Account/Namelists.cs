using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Server.Account
{
    public partial class Namelists : Form
    {
        public Namelists()
        {
            InitializeComponent();
            UpdateNamelists();
            EnsureDailyResetsFileExists();
        }

        #region Update Name List
        private void UpdateNamelists()
        {
            // Define the directory path for the Namelists folder
            string namelistsPath = Path.Combine("Envir", "Namelists");

            // Ensure the directory exists
            if (!Directory.Exists(namelistsPath))
            {
                NamelistView.Items.Clear();
                NamelistView.Items.Add("Namelists directory not found.");
                NamelistCount.Text = "Namelist Count: 0";
                TotalPlayerLabel.Text = "Total Players: 0 (In all Namelists)";
                TotalUniquePlayerLabel.Text = "Total Unique Players: 0 (In all Namelists)";
                return;
            }

            // Clear the NamelistView before updating
            NamelistView.Items.Clear();

            // Track the number of namelists, total players, and unique players
            int namelistCount = 0;
            int totalPlayerCount = 0;
            HashSet<string> uniquePlayers = new HashSet<string>();

            // Iterate over each text file in the directory and subdirectories
            foreach (string filePath in Directory.GetFiles(namelistsPath, "*.txt", SearchOption.AllDirectories))
            {
                // Get the relative path from the Namelists directory
                string relativePath = Path.GetRelativePath(namelistsPath, filePath);

                // Remove the .txt extension
                relativePath = Path.ChangeExtension(relativePath, null);

                // Add the relative path to the NamelistView
                NamelistView.Items.Add(relativePath);

                // Increment the namelist count
                namelistCount++;

                // Count players in the current file and add to totalPlayerCount
                string[] players = File.ReadAllLines(filePath);
                totalPlayerCount += players.Length;

                // Add each player to the HashSet to track unique players
                foreach (string player in players)
                {
                    uniquePlayers.Add(player);
                }
            }

            // Update the labels with the total and unique counts
            NamelistCount.Text = $"Namelist Count: {namelistCount}";
            TotalPlayerLabel.Text = $"Total Players: {totalPlayerCount} (In all Namelists)";
            TotalUniquePlayerLabel.Text = $"Total Unique Players: {uniquePlayers.Count} (In all Namelists)";
        }

        private void NamelistView_SelectedIndexChanged(object sender, EventArgs e)
        {
            // Clear the contents of the NamelistViewBox before updating
            NamelistViewBox.Items.Clear();

            // Check if any item is selected
            if (NamelistView.SelectedItems.Count == 0) return;

            // Get the selected item's text, which is the relative path without the .txt extension
            string namelistFilename = NamelistView.SelectedItems[0].Text;

            // Construct the full path to the .txt file
            string fullPath = Path.Combine("Envir", "Namelists", namelistFilename + ".txt");

            // Check if the file exists
            if (File.Exists(fullPath))
            {
                // Read all lines from the file
                string[] lines = File.ReadAllLines(fullPath);

                // Check if the file is empty and display "Empty" if so, otherwise display each line as a new item
                if (lines.Length == 0)
                {
                    NamelistViewBox.Items.Add("Empty");
                }
                else
                {
                    foreach (string line in lines)
                    {
                        NamelistViewBox.Items.Add(line);
                    }
                }
            }
            else
            {
                // Display a message if the file is not found
                NamelistViewBox.Items.Add("File not found.");
            }

            // Refresh the NamelistResetCheckBox based on NewbieResets.txt
            RefreshNamelistResetCheckBox(namelistFilename);
        }
        #endregion

        #region Refresh Button
        private void RefreshButton_Click(object sender, EventArgs e)
        {
            // Define the directory path for the Namelists folder
            string namelistsPath = Path.Combine("Envir", "Namelists");

            // Ensure the directory exists
            if (!Directory.Exists(namelistsPath))
            {
                NamelistView.Items.Clear();
                NamelistView.Items.Add("Namelists directory not found.");
                NamelistCountLabel.Text = "Found in: 0 Namelists";
                return;
            }

            // Get the player's name from FindPlayerBox
            string playerName = FindPlayerBox.Text.Trim();

            // Clear the NamelistView before updating
            NamelistView.Items.Clear();

            // If the player name is empty, reload all namelists and reset the count label
            if (string.IsNullOrEmpty(playerName))
            {
                UpdateNamelists();
                NamelistCountLabel.Text = "Found in: 0 Namelists";
                return;
            }

            // Track the count of namelists containing the player's name
            int count = 0;

            // Iterate over each text file in the directory and subdirectories
            foreach (string filePath in Directory.GetFiles(namelistsPath, "*.txt", SearchOption.AllDirectories))
            {
                // Read all lines from the current file
                string[] lines = File.ReadAllLines(filePath);

                // Check if any line contains the player's name
                if (lines.Any(line => line.Contains(playerName)))
                {
                    // Get the relative path from the Namelists directory
                    string relativePath = Path.GetRelativePath(namelistsPath, filePath);

                    // Remove the .txt extension
                    relativePath = Path.ChangeExtension(relativePath, null);

                    // Add the relative path to the NamelistView
                    NamelistView.Items.Add(relativePath);

                    // Increment the count of namelists containing the player's name
                    count++;
                }
            }

            // Update the NamelistCountLabel with the count of namelists containing the player
            NamelistCountLabel.Text = $"Found in: {count} Namelists";

            // If no files contain the player's name, add a message to the NamelistView
            if (count == 0)
            {
                NamelistView.Items.Add("Player not found on any Namelists.");
            }
        }
        #endregion

        #region Delete Player Button
        private void DeletePlayerButton_Click(object sender, EventArgs e)
        {
            // Ensure an item is selected in NamelistViewBox
            if (NamelistViewBox.SelectedItems.Count == 0) return;

            // Get the selected player's name
            string playerToDelete = NamelistViewBox.SelectedItems[0].Text;

            // Ensure an item is selected in NamelistView to get the relevant file
            if (NamelistView.SelectedItems.Count == 0) return;

            // Get the file associated with the selected namelist
            string relativePath = NamelistView.SelectedItems[0].Text;
            string fullPath = Path.Combine("Envir", "Namelists", relativePath + ".txt");

            // Read all lines from the file and remove the selected player's name
            var lines = File.ReadAllLines(fullPath).ToList();
            bool removed = lines.Remove(playerToDelete);

            if (removed)
            {
                // Write updated lines back to the file
                File.WriteAllLines(fullPath, lines);

                // Remove the player from the ListView display
                NamelistViewBox.Items.Remove(NamelistViewBox.SelectedItems[0]);

                // If the file becomes empty, display "Empty"
                if (lines.Count == 0)
                {
                    NamelistViewBox.Items.Add("Empty");
                }
            }
        }
        #endregion

        #region Add Player Button
        private void AddPlayerButton_Click(object sender, EventArgs e)
        {
            // Ensure a namelist is selected in NamelistView
            if (NamelistView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a namelist to add the player to.");
                return;
            }

            // Prompt for the player's name
            string playerName = Microsoft.VisualBasic.Interaction.InputBox("Enter the player's name:", "Add Player", "");

            // Check if the input was empty
            if (string.IsNullOrWhiteSpace(playerName))
            {
                MessageBox.Show("Player name cannot be empty.");
                return;
            }

            // Get the file associated with the selected namelist
            string relativePath = NamelistView.SelectedItems[0].Text;
            string fullPath = Path.Combine("Envir", "Namelists", relativePath + ".txt");

            // Read all lines from the file to check if the player is already listed
            var lines = File.ReadAllLines(fullPath).ToList();
            if (lines.Contains(playerName))
            {
                MessageBox.Show("Player is already in the selected namelist.");
                return;
            }

            // Add the player to the file and update the ListView display
            lines.Add(playerName);
            File.WriteAllLines(fullPath, lines);

            // Refresh NamelistViewBox to display the new player if the current namelist is selected
            if (NamelistViewBox.Items.Contains(new ListViewItem("Empty")))
            {
                NamelistViewBox.Items.Clear();
            }
            NamelistViewBox.Items.Add(playerName);
        }
        #endregion

        #region Create Namelist Button
        private void CreateNamelistButton_Click(object sender, EventArgs e)
        {
            // Prompt for the namelist name
            string namelistName = Microsoft.VisualBasic.Interaction.InputBox("Enter the name for the new namelist:", "Create Namelist", "");

            // Check if the input was empty
            if (string.IsNullOrWhiteSpace(namelistName))
            {
                MessageBox.Show("Namelist name cannot be empty.");
                return;
            }

            // Define the full path for the new namelist file
            string namelistsPath = Path.Combine("Envir", "Namelists");
            string fullPath = Path.Combine(namelistsPath, namelistName + ".txt");

            // Check if the file already exists
            if (File.Exists(fullPath))
            {
                MessageBox.Show("A namelist with this name already exists.");
                return;
            }

            // Create the new namelist file
            File.Create(fullPath).Dispose();

            // Update the NamelistView to include the new namelist
            UpdateNamelists();
        }
        #endregion

        #region Delete Namelist Button
        private void DeleteNamelistButton_Click(object sender, EventArgs e)
        {
            // Ensure a namelist is selected in NamelistView
            if (NamelistView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a namelist to delete.");
                return;
            }

            // Get the selected namelist's relative path
            string relativePath = NamelistView.SelectedItems[0].Text;
            string fullPath = Path.Combine("Envir", "Namelists", relativePath + ".txt");

            // Confirm deletion
            var confirmResult = MessageBox.Show($"Are you sure you want to delete the namelist '{relativePath}'?",
                                                 "Confirm Delete",
                                                 MessageBoxButtons.YesNo,
                                                 MessageBoxIcon.Warning);

            if (confirmResult == DialogResult.Yes)
            {
                // Check if the file exists before attempting to delete
                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    UpdateNamelists(); // Refresh the NamelistView to reflect deletion
                }
                else
                {
                    MessageBox.Show("Namelist file not found.");
                }
            }
        }
        #endregion

        #region Namelist Resets

        private void EnsureDailyResetsFileExists()
        {
            // Define paths for the files
            string dailyResetsPath = Path.Combine("Envir", "SystemScripts", "00Default", "Login", "DailyResets.txt");
            string loginScriptPath = Path.Combine("Envir", "SystemScripts", "00Default", "Login.txt");

            // Default content for DailyResets.txt
            string defaultDailyResetsContent = @"[@MAIN]
{
#IF
DAYOFWEEK SUNDAY
#ACT
#ELSEACT
GOTO @Monday

[@Monday]
#IF
DAYOFWEEK MONDAY
#ACT
#ELSEACT
GOTO @Tuesday

[@Tuesday]
#IF
DAYOFWEEK TUESDAY
#ACT
#ELSEACT
GOTO @Wednesday

[@Wednesday]
#IF
DAYOFWEEK WEDNESDAY
#ACT
#ELSEACT
GOTO @Thursday

[@Thursday]
#IF
DAYOFWEEK THURSDAY
#ACT
#ELSEACT
GOTO @Friday

[@Friday]
#IF
DAYOFWEEK FRIDAY
#ACT
#ELSEACT
GOTO @Saturday

[@Saturday]
#IF
DAYOFWEEK SATURDAY
#ACT
}";

            // Create DailyResets.txt if it doesn't exist
            if (!File.Exists(dailyResetsPath))
            {
                File.WriteAllText(dailyResetsPath, defaultDailyResetsContent);
                MessageBox.Show("DailyResets.txt file was missing and has been created with default content.");
            }

            // Line to include in Login.txt
            string includeLine = "#INCLUDE [SystemScripts\\00Default\\Login\\DailyResets.txt] @Main";

            // Ensure the include line exists in Login.txt
            if (File.Exists(loginScriptPath))
            {
                var loginLines = File.ReadAllLines(loginScriptPath).ToList();

                // Add the include line if it doesn't already exist
                if (!loginLines.Any(line => line.Trim().Equals(includeLine, StringComparison.OrdinalIgnoreCase)))
                {
                    loginLines.Add(includeLine);
                    File.WriteAllLines(loginScriptPath, loginLines);
                    MessageBox.Show("The include line for DailyResets.txt was added to Login.txt.");
                }
            }
            else
            {
                MessageBox.Show("Login.txt file not found. Could not add the include line.");
            }
        }
        private void UpdateNamelistReset(string day, string namelistFilename, bool isChecked)
        {
            // Define the path for the DailyResets.txt file
            string scriptPath = Path.Combine("Envir", "SystemScripts", "00Default", "Login", "DailyResets.txt");

            // Ensure the file exists; if not, create an empty file
            if (!File.Exists(scriptPath))
            {
                File.WriteAllText(scriptPath, ""); // Create an empty file
            }

            // Read the contents of the existing script
            var lines = File.ReadAllLines(scriptPath).ToList();

            // Define the command to add or remove
            string clearCommand = $"CLEARNAMELIST {namelistFilename}.txt";

            if (day.Equals("SUNDAY", StringComparison.OrdinalIgnoreCase))
            {
                // Special handling for Sunday (after [@MAIN] section)
                int mainIndex = lines.FindIndex(line => line.Trim().Equals("[@MAIN]", StringComparison.OrdinalIgnoreCase));
                if (mainIndex != -1)
                {
                    int sundayActIndex = lines.FindIndex(mainIndex + 1, line => line.Trim().StartsWith("#ACT"));
                    int gotoMondayIndex = lines.FindIndex(mainIndex + 1, line => line.Trim().StartsWith("GOTO @Monday"));

                    if (sundayActIndex != -1 && gotoMondayIndex != -1 && sundayActIndex < gotoMondayIndex)
                    {
                        if (isChecked)
                        {
                            bool commandExists = lines.Skip(sundayActIndex + 1).Take(gotoMondayIndex - (sundayActIndex + 1))
                                                      .Any(line => line.Trim().Equals(clearCommand, StringComparison.OrdinalIgnoreCase));
                            if (!commandExists)
                            {
                                lines.Insert(sundayActIndex + 1, clearCommand);
                            }
                        }
                        else
                        {
                            int commandIndex = lines.FindIndex(sundayActIndex + 1, gotoMondayIndex - (sundayActIndex + 1),
                                                               line => line.Trim().Equals(clearCommand, StringComparison.OrdinalIgnoreCase));
                            if (commandIndex != -1)
                            {
                                lines.RemoveAt(commandIndex);
                            }
                        }
                    }
                }
            }
            else
            {
                // Handle other days (Monday to Saturday)
                string daySection = $"[@{day}]";
                int dayIndex = lines.FindIndex(line => line.Trim().Equals(daySection, StringComparison.OrdinalIgnoreCase));

                if (dayIndex != -1)
                {
                    int nextSectionIndex = lines.FindIndex(dayIndex + 1, line => line.Trim().StartsWith("[@"));
                    int endOfSection = nextSectionIndex != -1 ? nextSectionIndex : lines.Count;

                    int actIndex = lines.FindIndex(dayIndex + 1, endOfSection - (dayIndex + 1), line => line.Trim().StartsWith("#ACT"));

                    if (actIndex != -1)
                    {
                        if (isChecked)
                        {
                            bool commandExists = lines.Skip(actIndex + 1).Take(endOfSection - (actIndex + 1))
                                                      .Any(line => line.Trim().Equals(clearCommand, StringComparison.OrdinalIgnoreCase));
                            if (!commandExists)
                            {
                                lines.Insert(actIndex + 1, clearCommand);
                            }
                        }
                        else
                        {
                            int commandIndex = lines.FindIndex(actIndex + 1, endOfSection - (actIndex + 1),
                                                               line => line.Trim().Equals(clearCommand, StringComparison.OrdinalIgnoreCase));
                            if (commandIndex != -1)
                            {
                                lines.RemoveAt(commandIndex);
                            }
                        }
                    }
                }
            }

            // Write the updated content back to the file
            File.WriteAllLines(scriptPath, lines);
        }

        private void NamelistResetCheckBox_ItemCheck(object sender, ItemCheckEventArgs e)
        {
            // Ensure a namelist is selected in NamelistView
            if (NamelistView.SelectedItems.Count == 0) return;

            // Get the selected namelist filename (without .txt extension)
            string namelistFilename = NamelistView.SelectedItems[0].Text;

            // Get the day from the CheckedListBox based on the index being checked or unchecked
            string selectedDay = NamelistResetCheckBox.Items[e.Index].ToString().ToUpper();

            // Determine if the item is being checked or unchecked
            bool isChecked = e.NewValue == CheckState.Checked;

            // Update the NamelistReset.txt based on the checked state
            UpdateNamelistReset(selectedDay, namelistFilename, isChecked);
        }

        private void RefreshNamelistResetCheckBox(string namelistFilename)
        {
            // Define the path for NewbieResets.txt
            string scriptPath = Path.Combine("Envir", "SystemScripts", "00Default", "Login", "DailyResets.txt");

            // Create the file if it doesn't exist
            if (!File.Exists(scriptPath))
            {
                File.Create(scriptPath).Dispose();
            }

            // Read the contents of NewbieResets.txt
            var lines = File.ReadAllLines(scriptPath).ToList();

            // Iterate over each item in NamelistResetCheckBox (Sunday to Saturday)
            for (int i = 0; i < NamelistResetCheckBox.Items.Count; i++)
            {
                string day = NamelistResetCheckBox.Items[i].ToString().ToUpper();
                string clearCommand = $"CLEARNAMELIST {namelistFilename}.txt";

                // Find the section corresponding to the selected day
                string daySection = $"[@{day}]";
                int dayIndex = lines.FindIndex(line => line.Trim().Equals(daySection, StringComparison.OrdinalIgnoreCase));

                // Check if the clear command is present under the selected day
                bool isChecked = false;
                if (dayIndex != -1)
                {
                    // Check if the clear command exists in the day section
                    int commandIndex = lines.FindIndex(dayIndex, line => line.Trim().Equals(clearCommand, StringComparison.OrdinalIgnoreCase));
                    isChecked = commandIndex != -1;
                }

                // Update the checked state of the checkbox
                NamelistResetCheckBox.SetItemChecked(i, isChecked);
            }
        }
        private void ViewResetScript_Click(object sender, EventArgs e)
        {
            // Define the path for the DailyResets.txt file
            string scriptPath = Path.Combine("Envir", "SystemScripts", "00Default", "Login", "DailyResets.txt");

            // Check if the file exists
            if (File.Exists(scriptPath))
            {
                // Open the file in the default text editor
                System.Diagnostics.Process.Start("notepad.exe", scriptPath);
            }
            else
            {
                MessageBox.Show("DailyResets.txt file not found.");
            }
        }
        #endregion

        #region Open Namelist Button
        private void OpenNameListButton_Click(object sender, EventArgs e)
        {
            // Ensure a namelist is selected in NamelistView
            if (NamelistView.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a namelist to open.");
                return;
            }

            // Get the selected namelist filename (without .txt extension)
            string namelistFilename = NamelistView.SelectedItems[0].Text;

            // Construct the full path to the namelist file
            string fullPath = Path.Combine("Envir", "Namelists", namelistFilename + ".txt");

            // Check if the file exists
            if (File.Exists(fullPath))
            {
                // Open the file in Notepad
                System.Diagnostics.Process.Start("notepad.exe", fullPath);
            }
            else
            {
                MessageBox.Show("The selected namelist file was not found.");
            }
        }

        #endregion

        private void NukePlayerButton_Click(object sender, EventArgs e)
        {
            // Ensure a player is selected in NamelistViewBox
            if (NamelistViewBox.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a player to delete from all namelists.");
                return;
            }

            // Get the selected player's name
            string playerToDelete = NamelistViewBox.SelectedItems[0].Text;

            // Show a confirmation dialog
            var confirmResult = MessageBox.Show(
                $"Are you sure you want to delete '{playerToDelete}' from all namelists?",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            // If the user clicked No, cancel the operation
            if (confirmResult != DialogResult.Yes) return;

            // Define the directory path for the Namelists folder
            string namelistsPath = Path.Combine("Envir", "Namelists");

            // Ensure the Namelists folder exists
            if (!Directory.Exists(namelistsPath))
            {
                MessageBox.Show("Namelists directory not found.");
                return;
            }

            // Iterate over each text file in the Namelists folder
            foreach (string filePath in Directory.GetFiles(namelistsPath, "*.txt", SearchOption.AllDirectories))
            {
                // Read all lines from the current file
                var lines = File.ReadAllLines(filePath).ToList();

                // Check if the player exists in the file and remove them
                bool removed = lines.Remove(playerToDelete);

                // If the player was removed, write the updated lines back to the file
                if (removed)
                {
                    File.WriteAllLines(filePath, lines);
                }
            }

            // Inform the user that the player was deleted from all namelists
            MessageBox.Show($"Player '{playerToDelete}' has been deleted from all namelists.");

            // Optionally, remove the player from the NamelistViewBox display
            NamelistViewBox.Items.Remove(NamelistViewBox.SelectedItems[0]);
        }
    }
}