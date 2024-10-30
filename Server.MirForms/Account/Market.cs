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

        #region Trust Merchant
        private void LoadMarket()
        {
            // Clear existing items in the MarketListing
            MarketListing.Items.Clear();

            // Retrieve all auctions from the user database and filter out expired or sold items
            List<AuctionInfo> allAuctions = Envir.Main.Auctions.ToList();
            List<AuctionInfo> activeAuctions = allAuctions.Where(a => !a.Expired && !a.Sold).ToList();

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

        private void RefreshListings_Click(object sender, EventArgs e)
        {
            LoadMarket();
        }
    }
        #endregion
}
