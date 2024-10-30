namespace Server.Database
{
    partial class Market
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
            MarketListing = new ListView();
            ItemName = new ColumnHeader();
            AID = new ColumnHeader();
            Price = new ColumnHeader();
            Seller = new ColumnHeader();
            Expiry = new ColumnHeader();
            SearchBox = new TextBox();
            FilterByPlayer = new CheckBox();
            FilterByItem = new CheckBox();
            SearchLabel = new Label();
            RefreshListings = new Button();
            SuspendLayout();
            // 
            // MarketListing
            // 
            MarketListing.BorderStyle = BorderStyle.None;
            MarketListing.Columns.AddRange(new ColumnHeader[] { ItemName, AID, Price, Seller, Expiry });
            MarketListing.Dock = DockStyle.Bottom;
            MarketListing.FullRowSelect = true;
            MarketListing.GridLines = true;
            MarketListing.Location = new Point(0, 45);
            MarketListing.Name = "MarketListing";
            MarketListing.Size = new Size(562, 405);
            MarketListing.TabIndex = 0;
            MarketListing.UseCompatibleStateImageBehavior = false;
            MarketListing.View = View.Details;
            // 
            // ItemName
            // 
            ItemName.Text = "Item";
            ItemName.Width = 120;
            // 
            // AID
            // 
            AID.Text = "Auction ID";
            AID.Width = 80;
            // 
            // Price
            // 
            Price.Text = "Price";
            Price.Width = 100;
            // 
            // Seller
            // 
            Seller.Text = "Seller";
            Seller.Width = 120;
            // 
            // Expiry
            // 
            Expiry.Text = "Expiry";
            Expiry.Width = 140;
            // 
            // SearchBox
            // 
            SearchBox.Location = new Point(69, 12);
            SearchBox.Name = "SearchBox";
            SearchBox.Size = new Size(100, 23);
            SearchBox.TabIndex = 1;
            // 
            // FilterByPlayer
            // 
            FilterByPlayer.AutoSize = true;
            FilterByPlayer.Location = new Point(186, 14);
            FilterByPlayer.Name = "FilterByPlayer";
            FilterByPlayer.Size = new Size(103, 19);
            FilterByPlayer.TabIndex = 2;
            FilterByPlayer.Text = "Filter by Player";
            FilterByPlayer.UseVisualStyleBackColor = true;
            // 
            // FilterByItem
            // 
            FilterByItem.AutoSize = true;
            FilterByItem.Location = new Point(295, 14);
            FilterByItem.Name = "FilterByItem";
            FilterByItem.Size = new Size(95, 19);
            FilterByItem.TabIndex = 3;
            FilterByItem.Text = "Filter by Item";
            FilterByItem.UseVisualStyleBackColor = true;
            // 
            // SearchLabel
            // 
            SearchLabel.AutoSize = true;
            SearchLabel.Location = new Point(21, 16);
            SearchLabel.Name = "SearchLabel";
            SearchLabel.Size = new Size(42, 15);
            SearchLabel.TabIndex = 4;
            SearchLabel.Text = "Search";
            // 
            // RefreshListings
            // 
            RefreshListings.Location = new Point(396, 12);
            RefreshListings.Name = "RefreshListings";
            RefreshListings.Size = new Size(75, 23);
            RefreshListings.TabIndex = 5;
            RefreshListings.Text = "Refresh";
            RefreshListings.UseVisualStyleBackColor = true;
            RefreshListings.Click += RefreshListings_Click;
            // 
            // Market
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(562, 450);
            Controls.Add(RefreshListings);
            Controls.Add(SearchLabel);
            Controls.Add(FilterByItem);
            Controls.Add(FilterByPlayer);
            Controls.Add(SearchBox);
            Controls.Add(MarketListing);
            Name = "Market";
            Text = "Market";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private ListView MarketListing;
        private ColumnHeader ItemName;
        private ColumnHeader AID;
        private ColumnHeader Price;
        private ColumnHeader Seller;
        private ColumnHeader Expiry;
        private TextBox SearchBox;
        private CheckBox FilterByPlayer;
        private CheckBox FilterByItem;
        private Label SearchLabel;
        private Button RefreshListings;
    }
}