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
            SearchGroupBox = new GroupBox();
            ActionsGroupBox = new GroupBox();
            TotalItemsLabel = new Label();
            TotalItemsOwnedLabel = new Label();
            DeleteListingButton = new Button();
            ExpireListingButton = new Button();
            ReasonLabel = new Label();
            ReasonTextBox = new TextBox();
            SearchGroupBox.SuspendLayout();
            ActionsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // MarketListing
            // 
            MarketListing.BorderStyle = BorderStyle.None;
            MarketListing.Columns.AddRange(new ColumnHeader[] { ItemName, AID, Price, Seller, Expiry });
            MarketListing.Dock = DockStyle.Bottom;
            MarketListing.FullRowSelect = true;
            MarketListing.GridLines = true;
            MarketListing.Location = new Point(0, 175);
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
            SearchBox.Location = new Point(72, 22);
            SearchBox.Name = "SearchBox";
            SearchBox.Size = new Size(100, 23);
            SearchBox.TabIndex = 1;
            // 
            // FilterByPlayer
            // 
            FilterByPlayer.AutoSize = true;
            FilterByPlayer.Location = new Point(189, 24);
            FilterByPlayer.Name = "FilterByPlayer";
            FilterByPlayer.Size = new Size(103, 19);
            FilterByPlayer.TabIndex = 2;
            FilterByPlayer.Text = "Filter by Player";
            FilterByPlayer.UseVisualStyleBackColor = true;
            // 
            // FilterByItem
            // 
            FilterByItem.AutoSize = true;
            FilterByItem.Location = new Point(298, 24);
            FilterByItem.Name = "FilterByItem";
            FilterByItem.Size = new Size(95, 19);
            FilterByItem.TabIndex = 3;
            FilterByItem.Text = "Filter by Item";
            FilterByItem.UseVisualStyleBackColor = true;
            // 
            // SearchLabel
            // 
            SearchLabel.AutoSize = true;
            SearchLabel.Location = new Point(24, 26);
            SearchLabel.Name = "SearchLabel";
            SearchLabel.Size = new Size(42, 15);
            SearchLabel.TabIndex = 4;
            SearchLabel.Text = "Search";
            // 
            // RefreshListings
            // 
            RefreshListings.Location = new Point(399, 22);
            RefreshListings.Name = "RefreshListings";
            RefreshListings.Size = new Size(75, 23);
            RefreshListings.TabIndex = 5;
            RefreshListings.Text = "Refresh";
            RefreshListings.UseVisualStyleBackColor = true;
            RefreshListings.Click += RefreshListings_Click;
            // 
            // SearchGroupBox
            // 
            SearchGroupBox.Controls.Add(TotalItemsOwnedLabel);
            SearchGroupBox.Controls.Add(TotalItemsLabel);
            SearchGroupBox.Controls.Add(FilterByPlayer);
            SearchGroupBox.Controls.Add(RefreshListings);
            SearchGroupBox.Controls.Add(SearchBox);
            SearchGroupBox.Controls.Add(SearchLabel);
            SearchGroupBox.Controls.Add(FilterByItem);
            SearchGroupBox.Location = new Point(0, 0);
            SearchGroupBox.Name = "SearchGroupBox";
            SearchGroupBox.Size = new Size(562, 86);
            SearchGroupBox.TabIndex = 6;
            SearchGroupBox.TabStop = false;
            SearchGroupBox.Text = "Search/Stats";
            // 
            // ActionsGroupBox
            // 
            ActionsGroupBox.Controls.Add(ReasonTextBox);
            ActionsGroupBox.Controls.Add(ReasonLabel);
            ActionsGroupBox.Controls.Add(ExpireListingButton);
            ActionsGroupBox.Controls.Add(DeleteListingButton);
            ActionsGroupBox.Location = new Point(0, 92);
            ActionsGroupBox.Name = "ActionsGroupBox";
            ActionsGroupBox.Size = new Size(562, 83);
            ActionsGroupBox.TabIndex = 7;
            ActionsGroupBox.TabStop = false;
            ActionsGroupBox.Text = "Actions";
            // 
            // TotalItemsLabel
            // 
            TotalItemsLabel.AutoSize = true;
            TotalItemsLabel.Location = new Point(24, 58);
            TotalItemsLabel.Name = "TotalItemsLabel";
            TotalItemsLabel.Size = new Size(70, 15);
            TotalItemsLabel.TabIndex = 6;
            TotalItemsLabel.Text = "Total Items: ";
            // 
            // TotalItemsOwnedLabel
            // 
            TotalItemsOwnedLabel.AutoSize = true;
            TotalItemsOwnedLabel.Location = new Point(133, 58);
            TotalItemsOwnedLabel.Name = "TotalItemsOwnedLabel";
            TotalItemsOwnedLabel.Size = new Size(122, 15);
            TotalItemsOwnedLabel.TabIndex = 7;
            TotalItemsOwnedLabel.Text = "Total Items owned by:";
            // 
            // DeleteListingButton
            // 
            DeleteListingButton.Location = new Point(12, 22);
            DeleteListingButton.Name = "DeleteListingButton";
            DeleteListingButton.Size = new Size(96, 23);
            DeleteListingButton.TabIndex = 0;
            DeleteListingButton.Text = "Delete Listing";
            DeleteListingButton.UseVisualStyleBackColor = true;
            DeleteListingButton.Click += DeleteListingButton_Click;
            // 
            // ExpireListingButton
            // 
            ExpireListingButton.Location = new Point(12, 51);
            ExpireListingButton.Name = "ExpireListingButton";
            ExpireListingButton.Size = new Size(96, 23);
            ExpireListingButton.TabIndex = 1;
            ExpireListingButton.Text = "Expire Listing";
            ExpireListingButton.UseVisualStyleBackColor = true;
            ExpireListingButton.Click += ExpireListingButton_Click;
            // 
            // ReasonLabel
            // 
            ReasonLabel.AutoSize = true;
            ReasonLabel.Location = new Point(114, 43);
            ReasonLabel.Name = "ReasonLabel";
            ReasonLabel.Size = new Size(48, 15);
            ReasonLabel.TabIndex = 2;
            ReasonLabel.Text = "Reason:";
            // 
            // ReasonTextBox
            // 
            ReasonTextBox.Location = new Point(165, 40);
            ReasonTextBox.Name = "ReasonTextBox";
            ReasonTextBox.Size = new Size(385, 23);
            ReasonTextBox.TabIndex = 3;
            // 
            // Market
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(562, 580);
            Controls.Add(ActionsGroupBox);
            Controls.Add(SearchGroupBox);
            Controls.Add(MarketListing);
            Name = "Market";
            Text = "Market";
            SearchGroupBox.ResumeLayout(false);
            SearchGroupBox.PerformLayout();
            ActionsGroupBox.ResumeLayout(false);
            ActionsGroupBox.PerformLayout();
            ResumeLayout(false);
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
        private GroupBox SearchGroupBox;
        private Label TotalItemsOwnedLabel;
        private Label TotalItemsLabel;
        private GroupBox ActionsGroupBox;
        private TextBox ReasonTextBox;
        private Label ReasonLabel;
        private Button ExpireListingButton;
        private Button DeleteListingButton;
    }
}