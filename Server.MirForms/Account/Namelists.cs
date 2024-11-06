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
            string relativePath = NamelistView.SelectedItems[0].Text;

            // Construct the full path to the .txt file
            string fullPath = Path.Combine("Envir", "Namelists", relativePath + ".txt");

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
    }
}