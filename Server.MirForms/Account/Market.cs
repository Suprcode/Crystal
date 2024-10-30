using Server.MirDatabase;
using Server.MirEnvir;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Server.Database
{
    public partial class Market : Form
    {
        public Market()
        {
            InitializeComponent();
            LoadMarket();
        }

        #region Load Market
        private void LoadMarket()
        {
            // Clear existing items in the MarketListing
            MarketListing.Items.Clear();

            // Retrieve all auctions from the user database and filter out expired or sold items
            List<AuctionInfo> allAuctions = Envir.Main.Auctions.ToList();
            List<AuctionInfo> activeAuctions = allAuctions.Where(a => !a.Expired && !a.Sold).ToList();

            // Update the TotalItemsLabel with the count of active items
            TotalItemsLabel.Text = $"Total Items: {activeAuctions.Count}";

            // Retrieve search keyword from SearchBox and convert to lowercase for case-insensitive search
            string searchKeyword = SearchBox.Text.Trim().ToLower();

            // Apply filters based on checkboxes and search keyword
            var filteredAuctions = activeAuctions.Where(a =>
            {
                bool matchesSearch = string.IsNullOrEmpty(searchKeyword) ||
                                     (FilterByItem.Checked && a.Item?.Info.FriendlyName.ToLower().Contains(searchKeyword) == true) ||
                                     (FilterByPlayer.Checked && a.SellerInfo?.Name.ToLower().Contains(searchKeyword) == true);

                return matchesSearch;
            }).ToList();

            // Iterate over each filtered auction listing and add it to the MarketListing
            foreach (var listing in filteredAuctions)
            {
                // Create a new ListViewItem with the item's name or "Unknown Item" as a fallback
                ListViewItem item = new ListViewItem(listing.Item?.Info.FriendlyName ?? "Unknown Item");

                // Add sub-items for UID, Price, Seller, and Expiry
                item.SubItems.Add(listing.AuctionID.ToString()); // Auction ID
                item.SubItems.Add(listing.Price.ToString("N0")); // Gold Price
                item.SubItems.Add(listing.SellerInfo?.Name ?? "Unknown Seller"); // Seller Name
                item.SubItems.Add(listing.ConsignmentDate.AddDays(7).ToString("g")); // Expiry date assuming 7 days from consignment

                // Add the item to the MarketListing
                MarketListing.Items.Add(item);
            }
        }
        #endregion

        #region Refresh Listings Button
        private void RefreshListings_Click(object sender, EventArgs e)
        {
            LoadMarket();
        }
        #endregion

        #region Delete Listing Button
        private void DeleteListingButton_Click(object sender, EventArgs e)
        {
            // Ensure an item is selected in the MarketListing
            if (MarketListing.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a listing to delete.");
                return;
            }

            // Get the selected auction ID from the ListView
            var selectedItem = MarketListing.SelectedItems[0];
            if (!ulong.TryParse(selectedItem.SubItems[1].Text, out ulong auctionId))
            {
                MessageBox.Show("Invalid Auction ID selected.");
                return;
            }

            // Find the auction in Envir.Main.Auctions by AuctionID
            var auction = Envir.Main.Auctions.FirstOrDefault(a => a.AuctionID == auctionId);
            if (auction == null)
            {
                MessageBox.Show("Auction listing not found.");
                return;
            }

            // Display a confirmation message
            var confirmResult = MessageBox.Show(
                "Are you sure you want to delete this listing?\n\n" +
                "Warning: This action is irreversible, and neither the item nor the asking price will be returned to the player.",
                "Confirm Deletion",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            // If the user clicks 'No', cancel the deletion
            if (confirmResult != DialogResult.Yes) return;

            // Send an announcement to the owner of the item (if online) with the reason from ReasonTextBox
            string reason = ReasonTextBox.Text.Trim();
            if (auction.SellerInfo?.Player != null && !string.IsNullOrEmpty(reason))
            {
                auction.SellerInfo.Player.ReceiveChat(reason, ChatType.Announcement);
            }

            // Remove the auction listing
            Envir.Main.Auctions.Remove(auction);

            // Refresh the MarketListing to reflect the removal
            LoadMarket();

            MessageBox.Show("Listing deleted successfully, and the owner has been notified.");
        }
        #endregion

        #region Expire Listings Button
        private void ExpireListingButton_Click(object sender, EventArgs e)
        {
            // Ensure an item is selected in the MarketListing
            if (MarketListing.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a listing to expire.");
                return;
            }

            // Get the selected auction ID from the ListView
            var selectedItem = MarketListing.SelectedItems[0];
            if (!ulong.TryParse(selectedItem.SubItems[1].Text, out ulong auctionId))
            {
                MessageBox.Show("Invalid Auction ID selected.");
                return;
            }

            // Find the auction in Envir.Main.Auctions by AuctionID
            var auction = Envir.Main.Auctions.FirstOrDefault(a => a.AuctionID == auctionId);
            if (auction == null)
            {
                MessageBox.Show("Auction listing not found.");
                return;
            }

            // Mark the listing as expired
            auction.Expired = true;

            // Refresh the MarketListing to reflect the update
            LoadMarket();

            MessageBox.Show("Listing marked as expired successfully.");
        }
        #endregion
    }
}
