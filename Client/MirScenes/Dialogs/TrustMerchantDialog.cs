using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using C = ClientPackets;


namespace Client.MirScenes.Dialogs
{
    public sealed class TrustMerchantDialog : MirImageControl
    {
        public static bool UserMode = false;
        public static MarketPanelType MarketType;

        public uint Amount = 0,
            MinConsignAmount = Globals.MinConsignment, MaxConsignAmount = Globals.MaxConsignment,
            MinBidAmount = Globals.MinStartingBid, MaxBidAmount = Globals.MaxStartingBid;

        public static long SearchTime, MarketTime;

        public MirTextBox SearchTextBox, PriceTextBox;
        public MirButton FindButton, RefreshButton, MailButton, BuyButton, SellNowButton, CloseButton, NextButton, BackButton;
        public MirImageControl TitleLabel;
        public MirLabel ItemLabel, PriceLabel, SellerLabel, PageLabel;
        public MirLabel DateLabel, ExpireLabel;
        public MirLabel NameLabel, TotalPriceLabel, SplitPriceLabel;
        public MirLabel HelpLabel;
        public MirLabel TitleSalePriceLabel, TitleSellLabel, TitleItemLabel, TitlePriceLabel, TitleExpiryLabel;

        public MirItemCell ItemCell, tempCell;
        public static UserItem SellItemSlot;
        public MirButton SellItemButton;

        public List<ClientAuction> Listings = new List<ClientAuction>();
        public List<ClientAuction> GameShopListings = new List<ClientAuction>();

        public int Page, PageCount;
        public static AuctionRow Selected;
        public AuctionRow[] Rows = new AuctionRow[10];

        public MirButton UpButton, DownButton, PositionBar;
        public MirButton MarketButton, ConsignmentButton, AuctionButton, GameShopButton;

        public MirImageControl FilterBox, FilterBackground;

        private readonly string consignmentText = $"1. Consignment is {Globals.ConsignmentCost} gold per item \r\n\r\n2. 1% of sale price is paid to Trust Merchant " +
            $"at sale end\r\n\r\n3. Maximum {Globals.ConsignmentLength} days of item sale registration until item is removed\r\n\r\n4. Maximum of unlimited " +
            $"items allowed for sale\r\n\r\n5. Sale price can be set between: {Globals.MinConsignment} - {Globals.MaxConsignment} gold";

        private readonly string auctionText = $"1. Auction cost is {Globals.AuctionCost} gold, max starting bid is {Globals.MaxStartingBid} gold per item \r\n\r\n2. 1% of final bid price is paid to Trust Merchant " +
            $"at auction end\r\n\r\n3. Maximum {Globals.ConsignmentLength} days of item sale registration, afterwards the item will be sent to highest bidder\r\n\r\n4. Maximum of unlimited " +
            $"items allowed for auction\r\n\r\n";

        private MirLabel TotalGold;

        public List<Filter> Filters = new List<Filter>();

        List<MirButton> FilterButtons = new List<MirButton>();
        List<MirLabel> FilterLabels = new List<MirLabel>();

        private int Skip = 0;
        private int MaxLines = 19;
        private int SelectedIndex = -1;
        private int SelectedSubIndex = -1;

        private int PossibleTotal = 0;
        private int PosX, PosMinY, PosMaxY;

        public TrustMerchantDialog()
        {
            Index = 786;
            Library = Libraries.Title;
            Sort = true;
            Movable = true;
            Size = new Size(492, 478);

            #region TrustMerchant Buttons

            MarketButton = new MirButton
            {
                Index = 789,
                PressedIndex = 788,
                Library = Libraries.Title,
                Location = new Point(9, 35),
                Parent = this,
            };
            MarketButton.Click += (o, e) =>
            {
                TMerchantDialog(MarketPanelType.Market);
                if (tempCell != null)
                {
                    tempCell.Locked = false;
                    SellItemSlot = null;
                    tempCell = null;
                }
            };

            ConsignmentButton = new MirButton
            {
                Index = 791,
                PressedIndex = 790,
                Library = Libraries.Title,
                Location = new Point(104, 35),
                Parent = this,
                Visible = true,
            };
            ConsignmentButton.Click += (o, e) =>
            {
                TMerchantDialog(MarketPanelType.Consign);
            };

            AuctionButton = new MirButton
            {
                Index = 817,
                PressedIndex = 816,
                Library = Libraries.Title,
                Location = new Point(199, 35),
                Parent = this,
                Visible = true,
            };
            AuctionButton.Click += (o, e) =>
            {
                TMerchantDialog(MarketPanelType.Auction);
            };

            GameShopButton = new MirButton
            {
                Index = 819,
                PressedIndex = 818,
                Library = Libraries.Title,
                Location = new Point(389, 35),
                Parent = this,
                Visible = true,
            };
            GameShopButton.Click += (o, e) =>
            {
                TMerchantDialog(MarketPanelType.GameShop);
            };

            CloseButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Location = new Point(465, 3),
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Parent = this,
            };
            CloseButton.Click += (o, e) => Hide();

            #region Page Buttons & Label

            HelpLabel = new MirLabel
            {
                Text = consignmentText,
                Parent = this,
                Size = new Size(115, 205),
                Location = new Point(8, 237),
                Font = new Font(Settings.FontName, Settings.FontSize - 1),
                ForeColour = Color.White,
                Visible = false
            };

            BackButton = new MirButton
            {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Library = Libraries.Prguse2,
                Location = new Point(251, 419),
                Sound = SoundList.ButtonA,
                Parent = this,
            };
            BackButton.Click += (o, e) =>
            {
                if (Page <= 0) return;

                Page--;
                UpdateInterface();
            };

            NextButton = new MirButton
            {
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Library = Libraries.Prguse2,
                Location = new Point(320, 419),
                Sound = SoundList.ButtonA,
                Parent = this,
            };
            NextButton.Click += (o, e) =>
            {
                if (Page >= PageCount - 1) return;
                if (Page < (Listings.Count - 1) / 10)
                {
                    Page++;
                    UpdateInterface();
                    return;
                }

                Network.Enqueue(new C.MarketPage { Page = Page + 1 });

            };

            PageLabel = new MirLabel
            {
                Size = new Size(70, 18),
                Location = new Point(260, 419),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Text = "0/0",
                NotControl = true,
                Parent = this,
            };

            UpButton = new MirButton
            {
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(108, 60),
                Sound = SoundList.ButtonA,
                Visible = true
            };
            UpButton.Click += (o, e) =>
            {
                if (Skip <= 0) return;

                Skip--;


                UpdatePositionBar();
                DrawFilters(SelectedIndex, SelectedSubIndex);
            };


            DownButton = new MirButton
            {
                Index = 207,
                HoverIndex = 208,
                Library = Libraries.Prguse2,
                PressedIndex = 209,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(108, 429),
                Sound = SoundList.ButtonA,
                Visible = true
            };

            DownButton.Click += (o, e) =>
            {
                if (Skip + MaxLines >= PossibleTotal) return;

                Skip++;

                UpdatePositionBar();
                DrawFilters(SelectedIndex, SelectedSubIndex);
            };

            PositionBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                PressedIndex = 206,
                Library = Libraries.Prguse2,
                Location = new Point(108, 73),
                Parent = this,
                Movable = true,
                Sound = SoundList.None,
                Visible = false
            };

            PosX = PositionBar.Location.X;
            PosMinY = UpButton.Location.Y + 13;
            PosMaxY = DownButton.Location.Y - 19;

            PositionBar.OnMoving += PositionBar_OnMoving;

            #endregion

            #endregion


            SetupFilters();

            #region Market Buttons

            MailButton = new MirButton
            {
                Index = 437,
                HoverIndex = 438,
                PressedIndex = 439,
                Library = Libraries.Prguse,
                Location = new Point(350, 448),
                Sound = SoundList.ButtonA,
                Parent = this,
            };
            MailButton.Click += (o, e) =>
            {
                if (Selected == null || CMain.Time < MarketTime) return;

                string message = $"I am interested in purchasing {Selected.Listing.Item.FriendlyName} for {Selected.Listing.Price}.";

                GameScene.Scene.MailComposeLetterDialog.ComposeMail(Selected.Listing.Seller, message);
            };

            RefreshButton = new MirButton
            {
                Index = 663,
                HoverIndex = 664,
                PressedIndex = 665,
                Library = Libraries.Prguse,
                Location = new Point(320, 448),
                Sound = SoundList.ButtonA,
                Parent = this,
            };
            RefreshButton.Click += (o, e) =>
            {
                if (CMain.Time < SearchTime)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat(string.Format("You can search again after {0} seconds.", Math.Ceiling((SearchTime - CMain.Time) / 1000D)), ChatType.System);
                    return;
                }
                SearchTime = CMain.Time + Globals.SearchDelay;
                SearchTextBox.Text = string.Empty;
                Network.Enqueue(new C.MarketRefresh());
            };

            BuyButton = new MirButton
            {
                Index = 703,
                HoverIndex = 704,
                PressedIndex = 705,
                Library = Libraries.Title,
                Location = new Point(380, 448),
                Sound = SoundList.ButtonA,
                Parent = this,
            };
            BuyButton.Click += (o, e) =>
            {
                if (Selected == null || CMain.Time < MarketTime) return;

                if (UserMode)
                {
                    if (Selected.Listing.ItemType == MarketItemType.Consign)
                    {
                        if (Selected.Listing.Seller == "For Sale")
                        {
                            MirMessageBox box = new MirMessageBox(string.Format("{0} has not sold, Are you sure you want to get it back?", Selected.Listing.Item.FriendlyName), MirMessageBoxButtons.YesNo);
                            box.YesButton.Click += (o1, e2) =>
                            {
                                MarketTime = CMain.Time + 3000;
                                Network.Enqueue(new C.MarketGetBack { AuctionID = Selected.Listing.AuctionID });
                            };
                            box.Show();
                        }
                        else
                        {
                            MarketTime = CMain.Time + 3000;
                            Network.Enqueue(new C.MarketGetBack { AuctionID = Selected.Listing.AuctionID });
                        }
                    }
                    else if (Selected.Listing.ItemType == MarketItemType.Auction)
                    {
                        if (Selected.Listing.Seller == "No Bid")
                        {
                            MirMessageBox box = new MirMessageBox(string.Format("{0} has not sold, Are you sure you want to get it back?", Selected.Listing.Item.FriendlyName), MirMessageBoxButtons.YesNo);
                            box.YesButton.Click += (o1, e2) =>
                            {
                                MarketTime = CMain.Time + 3000;
                                Network.Enqueue(new C.MarketGetBack { AuctionID = Selected.Listing.AuctionID });
                            };
                            box.Show();
                        }
                        else
                        {
                            MarketTime = CMain.Time + 3000;
                            Network.Enqueue(new C.MarketGetBack { AuctionID = Selected.Listing.AuctionID });
                        }
                    }
                }
                else
                {
                    switch (Selected.Listing.ItemType)
                    {
                        case MarketItemType.Consign:
                        case MarketItemType.GameShop:
                            {
                                MirMessageBox box = new MirMessageBox(string.Format("Are you sure you want to buy {0} for {1:#,##0} {2}?", Selected.Listing.Item.FriendlyName, Selected.Listing.Price, MarketType == MarketPanelType.GameShop ? "Credits" : "Gold"), MirMessageBoxButtons.YesNo);
                                box.YesButton.Click += (o1, e2) =>
                                {
                                    MarketTime = CMain.Time + 3000;
                                    Network.Enqueue(new C.MarketBuy { AuctionID = Selected.Listing.AuctionID });
                                };
                                box.Show();
                            }
                            break;
                        case MarketItemType.Auction:
                            {
                                MirAmountBox bidAmount = new MirAmountBox("Bid Amount:", Selected.Listing.Item.Info.Image, uint.MaxValue, Selected.Listing.Price + 1, Selected.Listing.Price + 1);

                                bidAmount.OKButton.Click += (o1, e1) =>
                                {
                                    MirMessageBox box = new MirMessageBox(string.Format("Are you sure you want to bid {0:#,##0} Gold for {1}?", bidAmount.Amount, Selected.Listing.Item.FriendlyName), MirMessageBoxButtons.YesNo);
                                    box.YesButton.Click += (o2, e2) =>
                                    {
                                        MarketTime = CMain.Time + 3000;
                                        Network.Enqueue(new C.MarketBuy { AuctionID = Selected.Listing.AuctionID, BidPrice = bidAmount.Amount });
                                    };

                                    box.Show();
                                };

                                bidAmount.Show();
                            }
                            break;
                    }
                }
            };

            SellNowButton = new MirButton
            {
                Index = 700,
                HoverIndex = 701,
                PressedIndex = 702,
                Library = Libraries.Title,
                Location = new Point(324, 448),
                Sound = SoundList.ButtonA,
                Parent = this,
            };
            SellNowButton.Click += (o, e) =>
            {
                if (Selected == null || CMain.Time < MarketTime) return;

                MarketTime = CMain.Time + 3000;
                Network.Enqueue(new C.MarketSellNow { AuctionID = Selected.Listing.AuctionID });
            };

            #endregion

            #region Search

            SearchTextBox = new MirTextBox
            {
                //Location = new Point(174, 452),
                //Location = new Point(240, 451),
                Location = new Point(11, 452),
                Size = new Size(110, 18),
                MaxLength = 20,
                Parent = this,
                CanLoseFocus = true,
            };
            SearchTextBox.TextBox.KeyPress += SearchTextBox_KeyPress;
            SearchTextBox.TextBox.KeyUp += SearchTextBox_KeyUp;
            SearchTextBox.TextBox.KeyDown += SearchTextBox_KeyDown;

            FindButton = new MirButton
            {
                Index = 480,
                HoverIndex = 481,
                PressedIndex = 482,
                Library = Libraries.Title,
                Location = new Point(124, 448),
                Sound = SoundList.ButtonA,
                Parent = this,
            };
            FindButton.Click += (o, e) =>
            {
                if (String.IsNullOrEmpty(SearchTextBox.Text)) return;
                if (CMain.Time < SearchTime)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat(string.Format("You can search again after {0} seconds.", Math.Ceiling((SearchTime - CMain.Time) / 1000D)), ChatType.System);
                    return;
                }

                SearchTime = CMain.Time + Globals.SearchDelay;
                Network.Enqueue(new C.MarketSearch
                {
                    Match = SearchTextBox.Text,
                    MarketType = MarketType
                });
            };

            #endregion

            #region Gold Label
            TotalGold = new MirLabel
            {
                Size = new Size(120, 16),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Location = new Point(6, 451),
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, Settings.FontSize),
                Visible = false
            };
            #endregion

            #region ItemCell

            ItemCell = new MirItemCell
            {
                BorderColour = Color.Lime,
                GridType = MirGridType.TrustMerchant,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(47, 104),
                ItemSlot = 0,
                Visible = false
            };
            ItemCell.Click += (o, e) => ItemCell_Click();

            PriceTextBox = new MirTextBox
            {
                Location = new Point(15, 165),
                Size = new Size(100, 18),
                MaxLength = 20,
                Parent = this,
                CanLoseFocus = true,
                Visible = false,
            };
            PriceTextBox.TextBox.TextChanged += TextBox_TextChanged;
            PriceTextBox.TextBox.KeyPress += MirInputBox_KeyPress;

            SellItemButton = new MirButton
            {
                Index = 700,
                PressedIndex = 702,
                HoverIndex = 701,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(39, 188),
                Parent = this,
                Visible = false,
                Enabled = false
            };
            SellItemButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.ConsignItem { UniqueID = SellItemSlot.UniqueID, Price = Amount, Type = MarketType });
                SellItemSlot = null;
                PriceTextBox.Text = null;
                SellItemButton.Enabled = false;
                TMerchantDialog(MarketType);
            };


            #endregion

            #region Auction Rows
            for (int i = 0; i < Rows.Length; i++)
            {
                Rows[i] = new AuctionRow
                {
                    Location = new Point(127, 82 + i * 33),
                    Parent = this
                };
                Rows[i].Click += (o, e) =>
                {
                    Selected = (AuctionRow)o;
                    UpdateInterface();
                };
            }
            #endregion

            #region Labels

            TitleSalePriceLabel = new MirLabel
            {
                Text = "SALE PRICE",
                Parent = this,
                Font = new Font(Settings.FontName, Settings.FontSize - 1, FontStyle.Italic),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Size = new Size(100, 21),
                Location = new Point(15, 142)
            };

            TitleSellLabel = new MirLabel
            {
                Text = "SELL ITEM",
                Parent = this,
                Font = new Font(Settings.FontName, Settings.FontSize - 1, FontStyle.Italic),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Size = new Size(110, 21),
                Location = new Point(10, 60)
            };

            TitleItemLabel = new MirLabel
            {
                Text = "ITEM",
                Parent = this,
                Font = new Font(Settings.FontName, Settings.FontSize - 1, FontStyle.Italic),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Size = new Size(166, 21),
                Location = new Point(127, 60)
            };

            TitlePriceLabel = new MirLabel
            {
                Text = "PRICE",
                Parent = this,
                Font = new Font(Settings.FontName, Settings.FontSize - 1, FontStyle.Italic),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Size = new Size(88, 21),
                Location = new Point(295, 60)
            };

            TitleExpiryLabel = new MirLabel
            {
                Text = "EXPIRY",
                Parent = this,
                Font = new Font(Settings.FontName, Settings.FontSize - 1, FontStyle.Italic),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Size = new Size(98, 21),
                Location = new Point(384, 60)
            };

            #endregion
        }

        private void SetupFilters()
        {
            var all = new Filter { Index = 0, Title = "Show All Items", Type = ItemType.Nothing };
            var weapon = new Filter { Index = 1, Title = "Weapon Items", Type = ItemType.Weapon };
            var drapery = new Filter { Index = 2, Title = "Drapery Items", Type = null };
            var accessory = new Filter { Index = 3, Title = "Accessory Items", Type = null };
            var consumable = new Filter { Index = 4, Title = "Consumable Items", Type = null };
            var enhancement = new Filter { Index = 5, Title = "Enhancement", Type = null };
            var book = new Filter { Index = 6, Title = "Books", Type = null };
            var crafting = new Filter { Index = 7, Title = "Craft Items", Type = null };

            Filters.Add(all);
            Filters.Add(weapon);
            Filters.Add(drapery);
            Filters.Add(accessory);
            Filters.Add(consumable);
            Filters.Add(enhancement);
            Filters.Add(book);
            Filters.Add(crafting);

            drapery.SubFilters.Add(new Filter { Index = 201, Title = "Armour", Type = ItemType.Armour });
            drapery.SubFilters.Add(new Filter { Index = 202, Title = "Helmet", Type = ItemType.Helmet });
            drapery.SubFilters.Add(new Filter { Index = 203, Title = "Belt", Type = ItemType.Belt });
            drapery.SubFilters.Add(new Filter { Index = 204, Title = "Boots", Type = ItemType.Boots });
            drapery.SubFilters.Add(new Filter { Index = 205, Title = "Stone", Type = ItemType.Stone });

            accessory.SubFilters.Add(new Filter { Index = 301, Title = "Necklaces", Type = ItemType.Necklace });
            accessory.SubFilters.Add(new Filter { Index = 302, Title = "Bracelets", Type = ItemType.Bracelet });
            accessory.SubFilters.Add(new Filter { Index = 303, Title = "Rings", Type = ItemType.Ring });

            consumable.SubFilters.Add(new Filter { Index = 401, Title = "Recovery Pots", Type = ItemType.Potion, MaxShape = 2 });
            consumable.SubFilters.Add(new Filter { Index = 402, Title = "Buff Pots", Type = ItemType.Potion, MinShape = 3, MaxShape = 4 });
            consumable.SubFilters.Add(new Filter { Index = 403, Title = "Scrolls / Oils", Type = ItemType.Scroll });
            consumable.SubFilters.Add(new Filter { Index = 404, Title = "Misc Items", Type = ItemType.Script });

            enhancement.SubFilters.Add(new Filter { Index = 501, Title = "Gems", Type = ItemType.Potion, MinShape = 3, MaxShape = 3 });
            enhancement.SubFilters.Add(new Filter { Index = 502, Title = "Orbs", Type = ItemType.Potion, MinShape = 4, MaxShape = 4 });

            book.SubFilters.Add(new Filter { Index = 601, Title = "Warrior", Type = ItemType.Book, MaxShape = 30 });
            book.SubFilters.Add(new Filter { Index = 602, Title = "Wizard", Type = ItemType.Book, MinShape = 31, MaxShape = 60 });
            book.SubFilters.Add(new Filter { Index = 603, Title = "Taoist", Type = ItemType.Book, MinShape = 61, MaxShape = 90 });
            book.SubFilters.Add(new Filter { Index = 604, Title = "Assassin", Type = ItemType.Book, MinShape = 91, MaxShape = 120 });
            book.SubFilters.Add(new Filter { Index = 605, Title = "Archer", Type = ItemType.Book, MinShape = 121, MaxShape = 150 });

            crafting.SubFilters.Add(new Filter { Index = 701, Title = "Materials", Type = ItemType.CraftingMaterial });
            crafting.SubFilters.Add(new Filter { Index = 703, Title = "Meat", Type = ItemType.Meat });
            crafting.SubFilters.Add(new Filter { Index = 704, Title = "Ore", Type = ItemType.Ore });
        }

        private void DrawFilters(int index, int subIndex)
        {
            SelectedIndex = index;
            SelectedSubIndex = subIndex;

            int btnx = 7;
            int btny = 60;

            int current = 0;
            int skipped = Skip;

            //Dispose all buttons and labels
            foreach (var item in FilterButtons)
                item.Dispose();

            foreach (var item in FilterLabels)
                item.Dispose();

            FilterButtons.Clear();
            FilterLabels.Clear();

            PossibleTotal = Filters.Count;

            foreach (var item in Filters)
            {
                if (skipped > 0)
                {
                    skipped--;
                    continue;
                }

                if (current >= MaxLines)
                {
                    break;
                }

                current++;

                var btn = new MirButton
                {
                    Index = 920,
                    PressedIndex = 921,
                    HoverIndex = 921,
                    Library = Libraries.Prguse2,
                    Sound = SoundList.ButtonA,
                    Location = new Point(btnx, btny),
                    Parent = this,
                };
                btn.Click += (o, e) =>
                {
                    if (item.SubFilters.Any())
                    {
                        DrawFilters(item.Index, -1);
                    }
                    else
                    {
                        DrawFilters(item.Index, -1);

                        if (item.Type.HasValue)
                        {
                            Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = item.Type.Value, Usermode = false, MinShape = item.MinShape, MaxShape = item.MaxShape, MarketType = MarketType });
                        }
                    }
                };

                var lbl = new MirLabel
                {
                    Size = new Size(99, 18),
                    Location = new Point(2, 1),
                    Parent = btn,
                    Text = item.Title,
                    NotControl = true,
                };

                btny += 20;

                FilterButtons.Add(btn);
                FilterLabels.Add(lbl);


                if (item.Index == index)
                {
                    PossibleTotal += item.SubFilters.Count;

                    btn.Index = 921;

                    //btny += 2;

                    if (item.SubFilters.Any())
                    {
                        btny += 2;
                    }

                    foreach (var subItem in item.SubFilters)
                    {
                        if (skipped > 0)
                        {
                            skipped--;
                            continue;
                        }

                        if (current >= MaxLines)
                        {
                            break;
                        }

                        current++;

                        var subBtn = new MirButton
                        {
                            Index = 922,
                            PressedIndex = 923,
                            HoverIndex = 923,
                            Library = Libraries.Prguse2,
                            Sound = SoundList.ButtonA,
                            Location = new Point(btnx, btny),
                            Parent = this,
                        };
                        subBtn.Click += (o, e) =>
                        {
                            DrawFilters(item.Index, subItem.Index);

                            if (subItem.Type.HasValue)
                            {
                                Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = subItem.Type.Value, Usermode = false, MinShape = subItem.MinShape, MaxShape = subItem.MaxShape, MarketType = MarketType });
                            }
                        };


                        var sublbl = new MirLabel
                        {
                            Size = new Size(99, 18),
                            Location = new Point(10, 1),
                            Parent = subBtn,
                            Text = subItem.Title,
                            NotControl = true,
                        };

                        if (subItem.Index == subIndex)
                        {
                            subBtn.Index = 923;
                        }

                        FilterButtons.Add(subBtn);
                        FilterLabels.Add(sublbl);

                        btny += 21;
                    }
                }

            }

            UpdatePositionBar();
        }

        private void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int x = PosX;
            int y = PositionBar.Location.Y;

            if (y >= PosMaxY) y = PosMaxY;
            if (y <= PosMinY) y = PosMinY;

            int location = y - PosMinY;
            int interval = (PosMaxY - PosMinY) / (PossibleTotal - MaxLines);

            double yPoint = (double)location / interval;

            Skip = Convert.ToInt16(Math.Floor(yPoint));

            PositionBar.Location = new Point(x, y);

            DrawFilters(SelectedIndex, SelectedSubIndex);
        }

        private void UpdatePositionBar()
        {
            if (PossibleTotal <= MaxLines)
            {
                PositionBar.Visible = false;
                return;
            }

            PositionBar.Visible = true;

            int interval = (PosMaxY - PosMinY) / (PossibleTotal - MaxLines);

            int x = PosX;
            int y = PosMinY + (Skip * interval);

            if (y >= PosMaxY) y = PosMaxY;
            if (y <= PosMinY) y = PosMinY;


            PositionBar.Location = new Point(x, y);
        }

        public void UpdateInterface()
        {
            PageLabel.Text = string.Format("{0}/{1}", Page + 1, PageCount);
            TotalGold.Text = MarketType == MarketPanelType.GameShop ? GameScene.Credit.ToString("###,###,##0") : GameScene.Gold.ToString("###,###,##0");

            for (int i = 0; i < 10; i++)
            {
                if (i + Page * 10 >= Listings.Count)
                {
                    Rows[i].Clear();
                    if (Rows[i] == Selected) Selected = null;
                }
                else
                {
                    if (Rows[i] == Selected && Selected.Listing != Listings[i + Page * 10])
                    {
                        Selected.Border = false;
                        Selected = null;
                    }

                    Rows[i].Update(Listings[i + Page * 10]);
                }
            }

            for (int i = 0; i < Rows.Length; i++)
            {
                Rows[i].Border = Rows[i] == Selected;
            }

            if (Selected != null)
            {
                BuyButton.Enabled = true;
                BuyButton.GrayScale = false;
                MailButton.Enabled = true;
                MailButton.GrayScale = false;
            }
            else
            {
                BuyButton.Enabled = false;
                BuyButton.GrayScale = true;
                MailButton.Enabled = false;
                MailButton.GrayScale = true;
            }

            if (Selected != null && Selected.Listing.Seller == "Bid Met")
            {
                SellNowButton.Enabled = true;
                SellNowButton.GrayScale = false;
            }
            else
            {
                SellNowButton.Enabled = false;
                SellNowButton.GrayScale = true;
            }
        }

        private void SearchTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (CMain.Time < SearchTime)
            {
                GameScene.Scene.ChatDialog.ReceiveChat(string.Format("You can search again after {0} seconds.", Math.Ceiling((SearchTime - CMain.Time) / 1000D)), ChatType.System);
                return;
            }

            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                    e.Handled = true;
                    if (string.IsNullOrEmpty(SearchTextBox.Text)) return;
                    SearchTime = CMain.Time + Globals.SearchDelay;
                    Network.Enqueue(new C.MarketSearch
                    {
                        Match = SearchTextBox.Text,
                        MarketType = MarketType
                    });
                    Program.Form.ActiveControl = null;
                    break;
                case (char)Keys.Escape:
                    e.Handled = true;
                    break;
            }
        }
        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            CMain.Shift = e.Shift;
            CMain.Alt = e.Alt;
            CMain.Ctrl = e.Control;

            switch (e.KeyCode)
            {
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.Tab:
                case Keys.Escape:
                    CMain.CMain_KeyUp(sender, e);
                    break;

            }
        }
        private void SearchTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            CMain.Shift = e.Shift;
            CMain.Alt = e.Alt;
            CMain.Ctrl = e.Control;

            switch (e.KeyCode)
            {
                case Keys.F1:
                case Keys.F2:
                case Keys.F3:
                case Keys.F4:
                case Keys.F5:
                case Keys.F6:
                case Keys.F7:
                case Keys.F8:
                case Keys.F9:
                case Keys.F10:
                case Keys.F11:
                case Keys.F12:
                case Keys.Tab:
                case Keys.Escape:
                    CMain.CMain_KeyDown(sender, e);
                    break;

            }
        }

        public void TMerchantDialog(MarketPanelType type)
        {
            MarketButton.Index = 789;
            ConsignmentButton.Index = 791;
            AuctionButton.Index = 817;
            GameShopButton.Index = 819;

            switch (type)
            {
                case MarketPanelType.Market:
                    Index = 786;
                    MarketButton.Index = 788;
                    BuyButton.Index = 703;
                    BuyButton.HoverIndex = 704;
                    BuyButton.PressedIndex = 705;
                    SellNowButton.Visible = false;
                    UpButton.Visible = true;
                    DownButton.Visible = true;
                    PositionBar.Visible = true;
                    DrawFilters(0, -1);

                    TitleSalePriceLabel.Visible = false;
                    TitleSellLabel.Visible = false;
                    TitleItemLabel.Visible = true;
                    TitlePriceLabel.Visible = true;
                    TitleExpiryLabel.Visible = true;
                    TitleSalePriceLabel.Text = "SALE PRICE";
                    TitleSellLabel.Text = "SELL ITEM";
                    TitleItemLabel.Text = "ITEM";
                    TitlePriceLabel.Text = "PRICE / BID";
                    TitleExpiryLabel.Text = "SELLER / EXPIRY";

                    //TotalGold.Visible = true;
                    PriceTextBox.Visible = false;
                    ItemCell.Visible = false;
                    MailButton.Visible = true;
                    FindButton.Visible = true;
                    SellItemButton.Visible = false;
                    SearchTextBox.Visible = true;
                    RefreshButton.Visible = true;
                    HelpLabel.Visible = false;
                    MarketType = MarketPanelType.Market;
                    Network.Enqueue(new C.MarketSearch
                    {
                        Match = "",
                        Type = ItemType.Nothing,
                        Usermode = false,
                        MarketType = MarketType
                    });
                    break;
                case MarketPanelType.Consign:
                    Index = 787;
                    ConsignmentButton.Index = 790;

                    MailButton.Visible = false;
                    BuyButton.Index = 706;
                    BuyButton.HoverIndex = 707;
                    BuyButton.PressedIndex = 708;
                    SellNowButton.Visible = false;
                    UpButton.Visible = false;
                    DownButton.Visible = false;
                    PositionBar.Visible = false;
                    //TotalGold.Visible = false;
                    PriceTextBox.Visible = true;
                    PriceTextBox.Text = null;
                    ItemCell.Visible = true;
                    SellItemButton.Visible = true;
                    SellItemButton.Enabled = false;
                    FindButton.Visible = false;
                    SearchTextBox.Visible = false;
                    RefreshButton.Visible = false;
                    HelpLabel.Visible = true;
                    HelpLabel.Text = consignmentText;

                    TitleSalePriceLabel.Visible = true;
                    TitleSellLabel.Visible = true;
                    TitleItemLabel.Visible = true;
                    TitlePriceLabel.Visible = true;
                    TitleExpiryLabel.Visible = true;
                    TitleSalePriceLabel.Text = "SALE PRICE";
                    TitleSellLabel.Text = "SELL ITEM";
                    TitleItemLabel.Text = "ITEM";
                    TitlePriceLabel.Text = "PRICE";
                    TitleExpiryLabel.Text = "EXPIRY";

                    foreach (var item in FilterButtons)
                    {
                        item.Visible = false;
                    }

                    MarketType = MarketPanelType.Consign;
                    Network.Enqueue(new C.MarketSearch
                    {
                        Match = "",
                        Type = ItemType.Nothing,
                        Usermode = true,
                        MarketType = MarketType
                    });
                    break;
                case MarketPanelType.Auction:
                    Index = 787;
                    AuctionButton.Index = 816;

                    MailButton.Visible = false;
                    BuyButton.Index = 706;
                    BuyButton.HoverIndex = 707;
                    BuyButton.PressedIndex = 708;
                    SellNowButton.Visible = true;
                    UpButton.Visible = false;
                    DownButton.Visible = false;
                    PositionBar.Visible = false;
                    //TotalGold.Visible = false;
                    PriceTextBox.Visible = true;
                    PriceTextBox.Text = null;
                    ItemCell.Visible = true;
                    SellItemButton.Visible = true;
                    SellItemButton.Enabled = false;
                    FindButton.Visible = false;
                    SearchTextBox.Visible = false;
                    RefreshButton.Visible = false;
                    HelpLabel.Visible = true;
                    HelpLabel.Text = auctionText;

                    TitleSalePriceLabel.Visible = true;
                    TitleSellLabel.Visible = true;
                    TitleItemLabel.Visible = true;
                    TitlePriceLabel.Visible = true;
                    TitleExpiryLabel.Visible = true;
                    TitleSalePriceLabel.Text = "STARTING BID";
                    TitleSellLabel.Text = "SELL ITEM";
                    TitleItemLabel.Text = "ITEM";
                    TitlePriceLabel.Text = "HIGHEST BID";
                    TitleExpiryLabel.Text = "END DATE";

                    foreach (var item in FilterButtons)
                    {
                        item.Visible = false;
                    }

                    MarketType = MarketPanelType.Auction;
                    Network.Enqueue(new C.MarketSearch
                    {
                        Match = "",
                        Type = ItemType.Nothing,
                        Usermode = true,
                        MarketType = MarketType
                    });
                    break;
                case MarketPanelType.GameShop:
                    Index = 786;
                    GameShopButton.Index = 818;

                    BuyButton.Index = 703;
                    BuyButton.HoverIndex = 704;
                    BuyButton.PressedIndex = 705;
                    SellNowButton.Visible = false;
                    UpButton.Visible = true;
                    DownButton.Visible = true;
                    PositionBar.Visible = true;
                    DrawFilters(0, -1);
                    //TotalGold.Visible = true;
                    PriceTextBox.Visible = false;
                    ItemCell.Visible = false;
                    MailButton.Visible = false;
                    FindButton.Visible = true;
                    SellItemButton.Visible = false;
                    SearchTextBox.Visible = true;
                    RefreshButton.Visible = false;
                    HelpLabel.Visible = false;

                    TitleSalePriceLabel.Visible = false;
                    TitleSellLabel.Visible = false;
                    TitleItemLabel.Visible = true;
                    TitlePriceLabel.Visible = true;
                    TitleExpiryLabel.Visible = true;
                    TitleSalePriceLabel.Text = "SALE PRICE";
                    TitleSellLabel.Text = "SELL ITEM";
                    TitleItemLabel.Text = "ITEM";
                    TitlePriceLabel.Text = "PRICE";
                    TitleExpiryLabel.Text = "";

                    MarketType = MarketPanelType.GameShop;
                    Network.Enqueue(new C.MarketSearch
                    {
                        Match = "",
                        Type = ItemType.Nothing,
                        Usermode = false,
                        MarketType = MarketType
                    });
                    break;
            }

            UpdateInterface();
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (MarketType == MarketPanelType.Consign)
            {
                if (uint.TryParse(PriceTextBox.Text, out Amount) && Amount >= MinConsignAmount)
                {
                    PriceTextBox.BorderColour = Color.Lime;

                    if (Amount > MaxConsignAmount)
                    {
                        Amount = MaxConsignAmount;
                        PriceTextBox.Text = MaxConsignAmount.ToString();
                        PriceTextBox.TextBox.SelectionStart = PriceTextBox.Text.Length;
                        SellItemButton.Enabled = true;
                    }

                    if (Amount == MaxConsignAmount)
                        PriceTextBox.BorderColour = Color.Orange;
                    SellItemButton.Enabled = true;
                }
                else
                {
                    PriceTextBox.BorderColour = Color.Red;
                    SellItemButton.Enabled = false;
                }
            }
            else if (MarketType == MarketPanelType.Auction)
            {
                if (uint.TryParse(PriceTextBox.Text, out Amount) && Amount >= MinBidAmount)
                {
                    PriceTextBox.BorderColour = Color.Lime;

                    if (Amount > MaxBidAmount)
                    {
                        Amount = MaxBidAmount;
                        PriceTextBox.Text = MaxBidAmount.ToString();
                        PriceTextBox.TextBox.SelectionStart = PriceTextBox.Text.Length;
                        SellItemButton.Enabled = true;
                    }

                    if (Amount == MaxBidAmount)
                        PriceTextBox.BorderColour = Color.Orange;
                    SellItemButton.Enabled = true;
                }
                else
                {
                    PriceTextBox.BorderColour = Color.Red;
                    SellItemButton.Enabled = false;
                }
            }
        }

        private void MirInputBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsControl(e.KeyChar)
                && !char.IsDigit(e.KeyChar))
            {
                e.Handled = true;
            }
        }

        private void ItemCell_Click()
        {
            if (tempCell != null)
            {
                tempCell.Locked = false;
                SellItemSlot = null;
                tempCell = null;
            }

            if (GameScene.SelectedCell == null || GameScene.SelectedCell.GridType != MirGridType.Inventory ||
                  GameScene.SelectedCell.Item != null && GameScene.SelectedCell.Item.Info.Durability < 0)
                return;

            SellItemSlot = GameScene.SelectedCell.Item;
            tempCell = GameScene.SelectedCell;
            tempCell.Locked = true;
            GameScene.SelectedCell = null;
            PriceTextBox.SetFocus();
        }

        public override void Hide()
        {
            if (!Visible) return;
            Visible = false;

            Listings.Clear();

            if (tempCell != null)
            {
                tempCell.Locked = false;
                SellItemSlot = null;
                tempCell = null;
            }
        }
        public void Show(bool gameshop = false)
        {
            if (Visible) return;
            Visible = true;

            if (gameshop)
            {
                MarketButton.Visible = false;
                ConsignmentButton.Visible = false;
                AuctionButton.Visible = false;
                GameShopButton.Visible = true;
                TMerchantDialog(MarketPanelType.GameShop);
            }
            else
            {
                MarketButton.Visible = true;
                ConsignmentButton.Visible = true;
                AuctionButton.Visible = true;
                GameShopButton.Visible = false;
                TMerchantDialog(MarketPanelType.Market);
            }
        }

        #region AuctionRow
        public sealed class AuctionRow : MirControl
        {
            public ClientAuction Listing = null;

            public MirLabel NameLabel, PriceLabel, SellerLabel, ExpireLabel;
            public MirImageControl IconImage, SelectedImage;
            public bool Selected = false;

            Size IconArea = new Size(34, 32);

            public AuctionRow()
            {
                Size = new Size(354, 32);
                Sound = SoundList.ButtonA;
                BorderColour = Color.FromArgb(255, 200, 100, 0);
                BeforeDraw += AuctionRow_BeforeDraw;

                NameLabel = new MirLabel
                {
                    AutoSize = true,
                    Size = new Size(140, 20),
                    Location = new Point(38, 8),
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    NotControl = true,
                    Parent = this,
                };

                PriceLabel = new MirLabel
                {
                    AutoSize = true,
                    Size = new Size(178, 20),
                    Location = new Point(170, 8),
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    NotControl = true,
                    Parent = this,
                };

                SellerLabel = new MirLabel
                {
                    AutoSize = true,
                    Size = new Size(148, 20),
                    Location = new Point(256, 0),
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    NotControl = true,
                    Parent = this,
                };

                IconImage = new MirImageControl
                {
                    Index = 0,
                    Library = Libraries.Prguse,
                    Location = new Point(0, 0),
                    Parent = this,
                };


                SelectedImage = new MirImageControl
                {
                    Index = 545,
                    Library = Libraries.Prguse,
                    Location = new Point(-5, -3),
                    Parent = this,
                    Visible = false,
                    NotControl = true
                };

                ExpireLabel = new MirLabel
                {
                    AutoSize = true,
                    Location = new Point(256, 14),
                    Size = new Size(110, 22),
                    DrawFormat = TextFormatFlags.Left | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };

                UpdateInterface();
            }

            private void AuctionRow_BeforeDraw(object sender, EventArgs e)
            {
                UpdateInterface();
            }
            public void UpdateInterface()
            {
                if (Listing == null) return;

                IconImage.Visible = true;

                if (Listing.Item.Count > 0)
                {
                    IconImage.Index = Listing.Item.Info.Image;
                    IconImage.Library = Libraries.Items;
                }
                else
                {
                    IconImage.Index = 540;
                    IconImage.Library = Libraries.Prguse;
                }

                IconImage.Location = new Point((IconArea.Width - IconImage.Size.Width) / 2, (IconArea.Height - IconImage.Size.Height) / 2);

                SellerLabel.Visible = Listing.ItemType == MarketItemType.Consign || Listing.ItemType == MarketItemType.Auction;
                ExpireLabel.Visible = Listing != null && (Listing.ItemType == MarketItemType.Consign || Listing.ItemType == MarketItemType.Auction);

                if (Listing == null) return;

                ExpireLabel.Text = string.Format("{0:dd/MM/yy HH:mm:ss}", Listing.ConsignmentDate.AddDays(Globals.ConsignmentLength));
            }
            protected override void Dispose(bool disposing)
            {
                base.Dispose(disposing);

                SelectedImage = null;
                IconImage = null;

                Selected = false;
            }
            public void Clear()
            {
                Visible = false;
                NameLabel.Text = string.Empty;
                PriceLabel.Text = string.Empty;
                SellerLabel.Text = string.Empty;
            }
            public void Update(ClientAuction listing)
            {
                Listing = listing;
                NameLabel.Text = Listing.Item.FriendlyName;
                PriceLabel.Text = String.Format("{0:###,###,##0} {1}", Listing.Price, listing.ItemType == MarketItemType.Auction ? "Bid" : "");

                NameLabel.ForeColour = GameScene.Scene.GradeNameColor(Listing.Item.Info.Grade);
                if (NameLabel.ForeColour == Color.Yellow)
                    NameLabel.ForeColour = Color.White;

                if (Listing.Price > 10000000) //10Mil
                    PriceLabel.ForeColour = Color.Red;
                else if (listing.Price > 1000000) //1Million
                    PriceLabel.ForeColour = Color.Orange;
                else if (listing.Price > 100000) //1Million
                    PriceLabel.ForeColour = Color.LawnGreen;
                else if (listing.Price > 10000) //1Million
                    PriceLabel.ForeColour = Color.DeepSkyBlue;
                else
                    PriceLabel.ForeColour = Color.White;


                SellerLabel.Text = Listing.Seller;
                SellerLabel.ForeColour = Color.White;

                if (UserMode)
                {
                    switch (Listing.Seller)
                    {
                        case "Sold":
                            SellerLabel.ForeColour = Color.Gold;
                            break;
                        case "Expired":
                            SellerLabel.ForeColour = Color.Red;
                            break;
                        case "Bid Met":
                            SellerLabel.ForeColour = Color.LawnGreen;
                            break;
                        default:
                            SellerLabel.ForeColour = Color.White;
                            break;
                    }
                }
                Visible = true;
            }
            protected override void OnMouseEnter()
            {
                if (Listing == null) return;

                base.OnMouseEnter();
                GameScene.Scene.CreateItemLabel(Listing.Item);
            }
            protected override void OnMouseLeave()
            {
                if (Listing == null) return;

                base.OnMouseLeave();
                GameScene.Scene.DisposeItemLabel();
                GameScene.HoverItem = null;
            }
        }
        #endregion

        public class Filter
        {
            public int Index { get; set; } = -1;
            public string Title { get; set; } = "";
            public ItemType? Type { get; set; } = ItemType.Nothing;
            public short MinShape { get; set; } = 0;
            public short MaxShape { get; set; } = short.MaxValue;

            public List<Filter> SubFilters = new List<Filter>();
        }
    }
}
