using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class TrustMerchantDialog : MirImageControl
    {
        public static bool UserMode = false;

        public static long SearchTime, MarketTime;

        public MirTextBox SearchTextBox;
        public MirButton FindButton, RefreshButton, MailButton, BuyButton, CloseButton, NextButton, BackButton;
        public MirImageControl TitleLabel;
        public MirLabel ItemLabel, PriceLabel, SellerLabel, PageLabel;
        public MirLabel DateLabel, ExpireLabel;
        public MirLabel NameLabel, TotalPriceLabel, SplitPriceLabel;

        public MirItemCell ItemCell;

        public List<ClientAuction> Listings = new List<ClientAuction>();


        public int Page, PageCount;
        public static AuctionRow Selected;
        public AuctionRow[] Rows = new AuctionRow[10];

        public TrustMerchantDialog()
        {
            Index = 670;
            Library = Libraries.Prguse;
            Sort = true;


            TitleLabel = new MirImageControl
            {
                Index = 24,
                Library = Libraries.Title,
                Location = new Point(18, 9),
                Parent = this
            };

            SearchTextBox = new MirTextBox
            {
                Location = new Point(19, 329),
                Parent = this,
                Size = new Size(104, 15),
                MaxLength = 20,
                CanLoseFocus = true
            };
            SearchTextBox.TextBox.KeyPress += SearchTextBox_KeyPress;
            SearchTextBox.TextBox.KeyUp += SearchTextBox_KeyUp;
            SearchTextBox.TextBox.KeyDown += SearchTextBox_KeyDown;

            FindButton = new MirButton
            {
                HoverIndex = 481,
                Index = 480,
                Location = new Point(130, 325),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 482,
                Sound = SoundList.ButtonA,
            };
            FindButton.Click += (o, e) =>
            {
                if (string.IsNullOrEmpty(SearchTextBox.Text)) return;
                if (CMain.Time < SearchTime)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat(string.Format("You can search again after {0} seconds.", Math.Ceiling((SearchTime - CMain.Time) / 1000D)), ChatType.System);
                    return;
                }

                SearchTime = CMain.Time + Globals.SearchDelay;
                Network.Enqueue(new C.MarketSearch
                {
                    Match = SearchTextBox.Text,
                });
            };

            RefreshButton = new MirButton
            {
                HoverIndex = 664,
                Index = 663,
                Location = new Point(190, 325),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 665,
                Sound = SoundList.ButtonA,
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


            MailButton = new MirButton
            {
                HoverIndex = 667,
                Index = 666,
                Location = new Point(225, 325),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 668,
                Sound = SoundList.ButtonA,
                Visible = false
            };

            BuyButton = new MirButton
            {
                HoverIndex = 484,
                Index = 483,
                Location = new Point(400, 325),
                Library = Libraries.Title,
                Parent = this,
                PressedIndex = 485,
                Sound = SoundList.ButtonA,
            };
            BuyButton.Click += (o, e) =>
            {
                if (Selected == null || CMain.Time < MarketTime) return;

                if (UserMode)
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
                else
                {
                    MirMessageBox box = new MirMessageBox(string.Format("Are you sure you want to buy {0} for {1}?", Selected.Listing.Item.FriendlyName, Selected.Listing.Price), MirMessageBoxButtons.YesNo);
                    box.YesButton.Click += (o1, e2) =>
                    {
                        MarketTime = CMain.Time + 3000;
                        Network.Enqueue(new C.MarketBuy { AuctionID = Selected.Listing.AuctionID });
                    };
                    box.Show();
                }
            };


            BackButton = new MirButton
            {
                Index = 398,
                Location = new Point(189, 298),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 399,
                Sound = SoundList.ButtonA,
            };
            BackButton.Click += (o, e) =>
            {
                if (Page <= 0) return;

                Page--;
                UpdateInterface();
            };

            NextButton = new MirButton
            {
                Index = 396,
                Location = new Point(283, 298),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 397,
                Sound = SoundList.ButtonA,
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

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(462, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            PageLabel = new MirLabel
            {
                Location = new Point(207, 298),
                Size = new Size(70, 18),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Parent = this,
                NotControl = true,
                Text = "0/0",
            };


            NameLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.Yellow,
                Location = new Point(20, 240),
                Parent = this,
                NotControl = true,
            };
            TotalPriceLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(20, 256),
                Parent = this,
                NotControl = true,
            };
            SplitPriceLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(20, 272),
                Parent = this,
                NotControl = true,
            };

            DateLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(250, 245),
                Parent = this,
                NotControl = true,
                Text = "Start Date:"
            };

            ExpireLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(250, 265),
                Parent = this,
                NotControl = true,
                Text = "Expire Date:"
            };

            ItemLabel = new MirLabel
            {
                Location = new Point(7, 32),
                Size = new Size(142, 22),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Text = "Item",
            };

            PriceLabel = new MirLabel
            {
                Location = new Point(148, 32),
                Size = new Size(180, 22),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Text = "Price",
            };

            SellerLabel = new MirLabel
            {
                Location = new Point(327, 32),
                Size = new Size(150, 22),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                NotControl = true,
                Text = "Seller",
            };

            for (int i = 0; i < Rows.Length; i++)
            {
                Rows[i] = new AuctionRow
                {
                    Location = new Point(8, 54 + i * 18),
                    Parent = this
                };
                Rows[i].Click += (o, e) =>
                {
                    Selected = (AuctionRow)o;
                    UpdateInterface();
                };
            }


            ItemCell = new MirItemCell
            {
                ItemSlot = 0,
                GridType = MirGridType.TrustMerchant,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(195, 248),
            };
        }

        public void UpdateInterface()
        {
            SellerLabel.Text = UserMode ? "Status" : "Seller";
            BuyButton.Index = UserMode ? 400 : 483;
            BuyButton.HoverIndex = UserMode ? 401 : 484;
            BuyButton.PressedIndex = UserMode ? 402 : 485;

            PageLabel.Text = string.Format("{0}/{1}", Page + 1, PageCount);

            for (int i = 0; i < 10; i++)
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

            for (int i = 0; i < Rows.Length; i++)
                Rows[i].Border = Rows[i] == Selected;



            NameLabel.Visible = Selected != null;
            TotalPriceLabel.Visible = Selected != null;
            SplitPriceLabel.Visible = Selected != null && Selected.Listing.Item.Count > 1;

            DateLabel.Visible = Selected != null;
            ExpireLabel.Visible = Selected != null;

            if (Selected == null) return;

            NameLabel.Text = Selected.Listing.Item.FriendlyName;

            TotalPriceLabel.Text = string.Format("Price: {0:#,##0}", Selected.Listing.Price);
            SplitPriceLabel.Text = string.Format("Each: {0:#,##0.#}", Selected.Listing.Price / (float)Selected.Listing.Item.Count);

            DateLabel.Text = string.Format("Start Date: {0}", Selected.Listing.ConsignmentDate);
            ExpireLabel.Text = string.Format("Finish Date: {0}", Selected.Listing.ConsignmentDate.AddDays(Globals.ConsignmentLength));

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


        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }
        public void Show()
        {
            if (Visible) return;
            Visible = true;
        }

        public sealed class AuctionRow : MirControl
        {
            public ClientAuction Listing;
            public MirLabel NameLabel, PriceLabel, SellerLabel;

            public AuctionRow()
            {
                Sound = SoundList.ButtonA;

                Size = new Size(468, 17);
                BorderColour = Color.Lime;

                NameLabel = new MirLabel
                {
                    Size = new Size(140, 17),
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };
                PriceLabel = new MirLabel
                {
                    Location = new Point(141, 0),
                    Size = new Size(178, 17),
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };

                SellerLabel = new MirLabel
                {
                    Location = new Point(320),
                    Size = new Size(148, 17),
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };

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
                PriceLabel.Text = Listing.Price.ToString("###,###,##0");

                NameLabel.ForeColour = Listing.Item.IsAdded ? Color.Cyan : Color.White;

                if (Listing.Price > 10000000) //10Mil
                    PriceLabel.ForeColour = Color.Red;
                else if (listing.Price > 1000000) //1Million
                    PriceLabel.ForeColour = Color.Orange;
                else if (listing.Price > 100000) //1Million
                    PriceLabel.ForeColour = Color.Green;
                else if (listing.Price > 10000) //1Million
                    PriceLabel.ForeColour = Color.DeepSkyBlue;
                else
                    PriceLabel.ForeColour = Color.White;


                SellerLabel.Text = Listing.Seller;

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
    }
}
