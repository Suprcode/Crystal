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
using Client.MirObjects;

namespace Client.MirScenes.Dialogs
{
    public sealed class TrustMerchantDialog : MirImageControl
    {
        public static bool UserMode = false;

        public uint Amount = 0, MinAmount = Globals.MinConsignment, MaxAmount = Globals.MaxConsignment;

        public static long SearchTime, MarketTime;

        public MirTextBox SearchTextBox, PriceTextBox;
        public MirButton FindButton, RefreshButton, MailButton, BuyButton, CloseButton, NextButton, BackButton;
        public MirImageControl TitleLabel;
        public MirLabel ItemLabel, PriceLabel, SellerLabel, PageLabel;
        public MirLabel DateLabel, ExpireLabel;
        public MirLabel NameLabel, TotalPriceLabel, SplitPriceLabel;

        public MirItemCell ItemCell, tempCell;
        public static UserItem SellItemSlot;
        public MirButton SellItemButton;

        public List<ClientAuction> Listings = new List<ClientAuction>();

        public int Page, PageCount;
        public static AuctionRow Selected;
        public AuctionRow[] Rows = new AuctionRow[10];

        public MirButton UpButton, DownButton, PositionBar;
        public MirButton MarketButton, ConsignmentButton;
        public MirImageControl MarketPage, ConsignmentPage;

        public MirImageControl FilterBox, FilterBackground;
        public MirButton ShowAllButton, WeaponButton, DraperyItemsButton, AccessoriesItemsButton, ConsumableItemsButton;
        public MirButton EnhEquipButton, BooksButton, CraftingSystemButton, PetsItemButton;

        public MirButton Armours, Helmets, Belts, Boots, Stones;// Drapery Items
        public MirButton Necklace, Bracelets, Rings;// Accessories Items
        public MirButton RecoveryPotion, PowerUp, Scroll, Script; //Consumable Items
        public MirButton Gem, Orb, Awake; //Enhanced Equipment
        public MirButton Warrior, Wizard, Taoist, Assassin, Archer; //Books
        public MirButton Materials, Fish, Meat, Ore; //Crafting System
        public MirButton NoveltyPets, NoveltyEquipment, Mounts, Reins, Bells, Ribbon, Mask; //Pets

        public TrustMerchantDialog()
        {


            Index = 787;
            Library = Libraries.Title;
            Sort = true;

            #region TrustMerchant Buttons

            MarketButton = new MirButton
            {
                Index = 790,
                PressedIndex = 789,
                Library = Libraries.Title,
                Location = new Point(9, 32),
                Parent = this,
            };
            MarketButton.Click += (o, e) =>
            {

                TMerchantDialog(0);
                if (tempCell != null)
                {
                    tempCell.Locked = false;
                    SellItemSlot = null;
                    tempCell = null;
                }
            };
            ConsignmentButton = new MirButton
            {
                Index = 792,
                PressedIndex = 791,
                Library = Libraries.Title,
                Location = new Point(104, 32),
                Parent = this,
                Visible = true,
            };
            ConsignmentButton.Click += (o, e) =>
            {
                TMerchantDialog(1);

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

            BackButton = new MirButton
            {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Library = Libraries.Prguse2,
                Location = new Point(252, 419),
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
                Location = new Point(321, 419),
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

            #endregion

            #endregion

            MarketPage = new MirImageControl()
            {
                Index = 787,
                Library = Libraries.Title,
                Visible = false,
                Sort = true,
            };

            ConsignmentPage = new MirImageControl()
            {
                Index = 788,
                Library = Libraries.Title,
                Visible = false,
                Sort = true,
            };

            ShowAllButton = new MirButton
            {
                Index = 920,
                PressedIndex = 921,
                HoverIndex = 921,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(7, 60),
                Parent = this,
                Text = "Show All Items",
                CenterText = true,
            };


            ShowAllButton.Click += (o, e) => SwitchTab(0);

            WeaponButton = new MirButton
            {
                Index = 920,
                PressedIndex = 921,
                HoverIndex = 921,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(7, ShowAllButton.Location.Y + 23),
                Parent = this,
                Text = "Weapon Items",
                CenterText = true,
                Visible = true,
            };

            WeaponButton.Click += (o, e) => SwitchTab(1);

            DraperyItemsButton = new MirButton
            {
                Index = 920,
                PressedIndex = 921,
                HoverIndex = 921,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(7, WeaponButton.Location.Y + 23),
                Parent = this,
                Text = "Drapery Items",
                CenterText = true,
                Visible = true,
            };
            DraperyItemsButton.Click += (o, e) => SwitchTab(2);

            #region Drapery Filtering.
            Armours = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, DraperyItemsButton.Location.Y + 23),
                Parent = this,
                Text = "Armours",
                CenterText = true,
                Visible = false,
            };
            Armours.Click += (o, e) => SwitchTab(9);

            Helmets = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Armours.Location.Y + 23),
                Parent = this,
                Text = "Helmets",
                CenterText = true,
                Visible = false,
            };
            Helmets.Click += (o, e) => SwitchTab(10);

            Belts = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Helmets.Location.Y + 23),
                Parent = this,
                Text = "Belts",
                CenterText = true,
                Visible = false,
            };
            Belts.Click += (o, e) => SwitchTab(11);

            Boots = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Belts.Location.Y + 23),
                Parent = this,
                Text = "Boots",
                CenterText = true,
                Visible = false,
            };
            Boots.Click += (o, e) => SwitchTab(12);

            Stones = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Boots.Location.Y + 23),
                Parent = this,
                Text = "Stones",
                CenterText = true,
                Visible = false,
            };
            Stones.Click += (o, e) => SwitchTab(13);
            #endregion

            AccessoriesItemsButton = new MirButton
            {
                Index = 920,
                PressedIndex = 921,
                HoverIndex = 921,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(7, DraperyItemsButton.Location.Y + 23),
                Parent = this,
                Text = "Accessorie Items",
                CenterText = true,
                Visible = true,
            };
            AccessoriesItemsButton.Click += (o, e) => SwitchTab(3);

            #region Accessories Items
            Necklace = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, AccessoriesItemsButton.Location.Y + 23),
                Parent = this,
                Text = "Necklace",
                CenterText = true,
                Visible = false,
            };
            Necklace.Click += (o, e) => SwitchTab(14);

            Bracelets = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Necklace.Location.Y + 23),
                Parent = this,
                Text = "Bracelets",
                CenterText = true,
                Visible = false,
            };
            Bracelets.Click += (o, e) => SwitchTab(15);

            Rings = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Bracelets.Location.Y + 23),
                Parent = this,
                Text = "Rings",
                CenterText = true,
                Visible = false,
            };
            Rings.Click += (o, e) => SwitchTab(16);
            #endregion

            ConsumableItemsButton = new MirButton
            {
                Index = 920,
                PressedIndex = 921,
                HoverIndex = 921,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(7, AccessoriesItemsButton.Location.Y + 23),
                Parent = this,
                Text = "Consumable Items",
                CenterText = true,
                Visible = true,
            };

            ConsumableItemsButton.Click += (o, e) => SwitchTab(4);

            #region Consumable Items
            RecoveryPotion = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, ConsumableItemsButton.Location.Y + 23),
                Parent = this,
                Text = "  Recovery Pots",
                CenterText = true,
                Visible = false,
            };
            RecoveryPotion.Click += (o, e) => SwitchTab(17);

            PowerUp = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, RecoveryPotion.Location.Y + 23),
                Parent = this,
                Text = "Buff Potions",
                CenterText = true,
                Visible = false,
            };
            PowerUp.Click += (o, e) => SwitchTab(18);

            Scroll = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, PowerUp.Location.Y + 23),
                Parent = this,
                Text = "Scrolls + Oils",
                CenterText = true,
                Visible = false,
            };
            Scroll.Click += (o, e) => SwitchTab(19);

            Script = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Scroll.Location.Y + 23),
                Parent = this,
                Text = "Other Items",
                CenterText = true,
                Visible = false,
            };
            Script.Click += (o, e) => SwitchTab(20);
            #endregion

            EnhEquipButton = new MirButton
            {
                Index = 920,
                PressedIndex = 921,
                HoverIndex = 921,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(7, ConsumableItemsButton.Location.Y + 23),
                Parent = this,
                Text = "Enhancing Equip",
                CenterText = true,
                Visible = true,
            };
            EnhEquipButton.Click += (o, e) => SwitchTab(5);

            #region Enhancing Equipment
            Gem = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, EnhEquipButton.Location.Y + 23),
                Parent = this,
                Text = "Gems",
                CenterText = true,
                Visible = false,
            };
            Gem.Click += (o, e) => SwitchTab(21);

            Orb = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Gem.Location.Y + 23),
                Parent = this,
                Text = "Orbs",
                CenterText = true,
                Visible = false,
            };
            Orb.Click += (o, e) => SwitchTab(22);

            Awake = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Orb.Location.Y + 23),
                Parent = this,
                Text = "Awakening",
                CenterText = true,
                Visible = false,
            };
            Awake.Click += (o, e) => SwitchTab(23);
            #endregion

            BooksButton = new MirButton
            {
                Index = 920,
                PressedIndex = 921,
                HoverIndex = 921,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(7, EnhEquipButton.Location.Y + 23),
                Parent = this,
                Text = "Books",
                CenterText = true,
                Visible = true,
            };

            BooksButton.Click += (o, e) => SwitchTab(6);

            #region Class Books
            Warrior = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, BooksButton.Location.Y + 23),
                Parent = this,
                Text = "Warrior",
                CenterText = true,
                Visible = false,
            };
            Warrior.Click += (o, e) => SwitchTab(24);

            Wizard = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Warrior.Location.Y + 23),
                Parent = this,
                Text = "Wizard",
                CenterText = true,
                Visible = false,
            };
            Wizard.Click += (o, e) => SwitchTab(25);

            Taoist = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Wizard.Location.Y + 23),
                Parent = this,
                Text = "Taoist",
                CenterText = true,
                Visible = false,
            };
            Taoist.Click += (o, e) => SwitchTab(26);

            Assassin = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Taoist.Location.Y + 23),
                Parent = this,
                Text = "Assassin",
                CenterText = true,
                Visible = false,
            };
            Assassin.Click += (o, e) => SwitchTab(27);

            Archer = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Assassin.Location.Y + 23),
                Parent = this,
                Text = "Archer",
                CenterText = true,
                Visible = false,
            };
            Archer.Click += (o, e) => SwitchTab(28);
            #endregion

            CraftingSystemButton = new MirButton
            {
                Index = 920,
                PressedIndex = 921,
                HoverIndex = 921,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(7, BooksButton.Location.Y + 23),
                Parent = this,
                Text = "Crafting System",
                CenterText = true,
                Visible = true,
            };

            CraftingSystemButton.Click += (o, e) => SwitchTab(7);

            #region Crafting System (CraftingMaterials)
            Materials = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, CraftingSystemButton.Location.Y + 23),
                Parent = this,
                Text = "Materials",
                CenterText = true,
                Visible = false,
            };
            Materials.Click += (o, e) => SwitchTab(29);

            Fish = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Materials.Location.Y + 23),
                Parent = this,
                Text = "Fish",
                CenterText = true,
                Visible = false,
            };
            Fish.Click += (o, e) => SwitchTab(30);

            Meat = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Fish.Location.Y + 23),
                Parent = this,
                Text = "Meat",
                CenterText = true,
                Visible = false,
            };
            Meat.Click += (o, e) => SwitchTab(31);

            Ore = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Meat.Location.Y + 23),
                Parent = this,
                Text = "Ores",
                CenterText = true,
                Visible = false,
            };
            Ore.Click += (o, e) => SwitchTab(32);
            #endregion

            PetsItemButton = new MirButton
            {
                Index = 920,
                PressedIndex = 921,
                HoverIndex = 921,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(7, CraftingSystemButton.Location.Y + 23),
                Parent = this,
                Text = "Pet Equipment",
                CenterText = true,
                Visible = true,
            };
            PetsItemButton.Click += (o, e) => SwitchTab(8);

            #region Pets & Mounts
            NoveltyPets = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, PetsItemButton.Location.Y + 23),
                Parent = this,
                Text = "  Novelty Pets",
                CenterText = true,
                Visible = false,
            };
            NoveltyPets.Click += (o, e) => SwitchTab(33);

            NoveltyEquipment = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, NoveltyPets.Location.Y + 23),
                Parent = this,
                Text = "  Novelty Equip",
                CenterText = true,
                Visible = false,
            };
            NoveltyEquipment.Click += (o, e) => SwitchTab(34);

            Mounts = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, NoveltyEquipment.Location.Y + 23),
                Parent = this,
                Text = "Mounts",
                CenterText = true,
                Visible = false,
            };
            Mounts.Click += (o, e) => SwitchTab(35);

            Reins = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Mounts.Location.Y + 23),
                Parent = this,
                Text = "  Reins",
                CenterText = true,
                Visible = false,
            };
            Reins.Click += (o, e) => SwitchTab(36);

            Bells = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Reins.Location.Y + 23),
                Parent = this,
                Text = "  Bells",
                CenterText = true,
                Visible = false,
            };
            Bells.Click += (o, e) => SwitchTab(37);

            Ribbon = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Bells.Location.Y + 23),
                Parent = this,
                Text = "  Ribbon",
                CenterText = true,
                Visible = false,
            };
            Ribbon.Click += (o, e) => SwitchTab(38);

            Mask = new MirButton
            {
                Index = 922,
                PressedIndex = 923,
                HoverIndex = 923,
                Library = Libraries.Prguse2,
                Sound = SoundList.ButtonA,
                Location = new Point(13, Ribbon.Location.Y + 23),
                Parent = this,
                Text = "  Mask",
                CenterText = true,
                Visible = false,
            };
            Mask.Click += (o, e) => SwitchTab(39);
            #endregion

            #region Market Buttons

            MailButton = new MirButton
            {
                Index = 437,
                HoverIndex = 438,
                PressedIndex = 439,
                Library = Libraries.Prguse,
                Location = new Point(200, 448),
                Sound = SoundList.ButtonA,
                Parent = this,
            };
            MailButton.Click += (o, e) =>
            {
                if (Selected == null || CMain.Time < MarketTime) return;

                MirMessageBox box = new MirMessageBox(string.Format("Are you sure you want to buy {0} for {1}?", Selected.Listing.Item.FriendlyName, Selected.Listing.Price), MirMessageBoxButtons.YesNo);
                box.YesButton.Click += (o1, e2) =>
                {
                    MarketTime = CMain.Time + 3000;
                    Network.Enqueue(new C.MarketBuy { AuctionID = Selected.Listing.AuctionID, MailItems = true });
                };
                box.Show();
            };

            RefreshButton = new MirButton
            {
                Index = 663,
                HoverIndex = 664,
                PressedIndex = 665,
                Library = Libraries.Prguse,
                Location = new Point(172, 448),
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
                Index = 796,
                HoverIndex = 797,
                PressedIndex = 798,
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
                        Network.Enqueue(new C.MarketBuy { AuctionID = Selected.Listing.AuctionID, MailItems = false });
                    };
                    box.Show();
                }
            };
            #endregion

            #region Search

            SearchTextBox = new MirTextBox
            {
                Location = new Point(12, 452),
                Size = new Size(108, 1),
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
                Location = new Point(122, 448),
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
                });
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
                Size = new Size(100, 1),
                MaxLength = 20,
                Parent = this,
                CanLoseFocus = true,
                Visible = false,
            };
            PriceTextBox.TextBox.TextChanged += TextBox_TextChanged;
            PriceTextBox.TextBox.KeyPress += MirInputBox_KeyPress;

            SellItemButton = new MirButton
            {
                Index = 793,
                PressedIndex = 794,
                HoverIndex = 795,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(40, 195),
                Parent = this,
                Visible = false,
                Enabled = false
            };
            SellItemButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.ConsignItem { UniqueID = SellItemSlot.UniqueID, Price = Amount });
                SellItemSlot = null;
                PriceTextBox.Text = null;
                SellItemButton.Enabled = false;
                TMerchantDialog(1);
            };


            #endregion

            for (int i = 0; i < Rows.Length; i++)
            {
                Rows[i] = new AuctionRow
                {
                    Location = new Point(127, 82 + i * 33),
                    Parent = this,
                };
                Rows[i].Click += (o, e) =>
                {
                    Selected = (AuctionRow)o;
                    UpdateInterface();
                };


            }



        }

        public void UpdateInterface()
        {

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
        public void TMerchantDialog(byte TMDid)
        {
            MarketButton.Index = 790;
            ConsignmentButton.Index = 792;


            setdefault();

            switch (TMDid)
            {
                case 0:
                    Index = 787;
                    MarketButton.Index = 789;
                    BuyButton.Index = 796;
                    BuyButton.HoverIndex = 797;
                    BuyButton.PressedIndex = 798;
                    SwitchTab(0);
                    setvalues(0);
                    Network.Enqueue(new C.MarketSearch
                    {
                        Match = "",
                        Type = ItemType.Nothing,
                        Usermode = false
                    });
                    break;
                case 1:
                    Index = 788;
                    BuyButton.Index = 799;
                    BuyButton.HoverIndex = 800;
                    BuyButton.PressedIndex = 801;
                    ConsignmentButton.Index = 791;
                    setvalues(1);
                    Network.Enqueue(new C.MarketSearch
                    {
                        Match = "",
                        Type = ItemType.Nothing,
                        Usermode = true
                    });
                    break;

            }
        }

        private void setvalues(byte i)
        {
            switch (i)
            {
                case 0:
                    FindButton.Visible = true;
                    SellItemButton.Visible = false;
                    ShowAllButton.Visible = true;
                    WeaponButton.Visible = true;
                    DraperyItemsButton.Visible = true;
                    AccessoriesItemsButton.Visible = true;
                    ConsumableItemsButton.Visible = true;
                    EnhEquipButton.Visible = true;
                    BooksButton.Visible = true;
                    MailButton.Visible = true;
                    CraftingSystemButton.Visible = true;
                    PetsItemButton.Visible = true;
                    PriceTextBox.Visible = false;
                    ItemCell.Visible = false;
                    SellItemButton.Visible = false;
                    SearchTextBox.Visible = true;
                    RefreshButton.Visible = true;
                    break;
                case 1:
                    MailButton.Visible = false;
                    ShowAllButton.Visible = false;
                    WeaponButton.Visible = false;
                    DraperyItemsButton.Visible = false;
                    AccessoriesItemsButton.Visible = false;
                    ConsumableItemsButton.Visible = false;
                    EnhEquipButton.Visible = false;
                    BooksButton.Visible = false;
                    CraftingSystemButton.Visible = false;
                    PetsItemButton.Visible = false;
                    PriceTextBox.Visible = true;
                    PriceTextBox.Text = null;
                    ItemCell.Visible = true;
                    SellItemButton.Visible = true;
                    SellItemButton.Enabled = false;
                    FindButton.Visible = false;
                    SearchTextBox.Visible = false;
                    RefreshButton.Visible = false;
                    break;
            }
        }
        public void SwitchTab(byte STabid)
        {



            switch (STabid)
            {
                case 0:
                    setdefault();
                    ShowAllButton.Index = 921;
                    SetLocations(0);
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Nothing, Usermode = false });
                    break;
                case 1:
                    setdefault();
                    WeaponButton.Index = 921;
                    SetLocations(1);
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Weapon, Usermode = false });
                    break;
                case 2:
                    setdefault();
                    DraperyItemsButton.Index = 921;
                    Armours.Visible = true;
                    Helmets.Visible = true;
                    Belts.Visible = true;
                    Boots.Visible = true;
                    Stones.Visible = true;
                    SetLocations(2);
                    break;
                case 3:
                    setdefault();
                    AccessoriesItemsButton.Index = 921;
                    Necklace.Visible = true;
                    Bracelets.Visible = true;
                    Rings.Visible = true;
                    SetLocations(3);
                    break;
                case 4:
                    setdefault();
                    ConsumableItemsButton.Index = 921;
                    RecoveryPotion.Visible = true;
                    PowerUp.Visible = true;
                    Scroll.Visible = true;
                    Script.Visible = true;
                    SetLocations(4);
                    break;
                case 5:
                    setdefault();
                    EnhEquipButton.Index = 921;
                    Gem.Visible = true;
                    Orb.Visible = true;
                    Awake.Visible = true;
                    SetLocations(5);
                    break;
                case 6:
                    setdefault();
                    BooksButton.Index = 921;
                    Warrior.Visible = true;
                    Wizard.Visible = true;
                    Taoist.Visible = true;
                    Assassin.Visible = true;
                    Archer.Visible = true;
                    SetLocations(6);
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Book, Usermode = false });
                    break;
                case 7:
                    setdefault();
                    CraftingSystemButton.Index = 921;
                    Materials.Visible = true;
                    Fish.Visible = true;
                    Meat.Visible = true;
                    Ore.Visible = true;
                    SetLocations(7);
                    break;
                case 8:
                    setdefault();
                    PetsItemButton.Index = 921;
                    NoveltyPets.Visible = true;
                    NoveltyEquipment.Visible = true;
                    Mounts.Visible = true;
                    Reins.Visible = true;
                    Bells.Visible = true;
                    Ribbon.Visible = true;
                    Mask.Visible = true;
                    SetLocations(8);
                    break;
                case 9:
                    Armours.Index = 923;
                    Helmets.Index = 922;
                    Belts.Index = 922;
                    Boots.Index = 922;
                    Stones.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Armour, Usermode = false });
                    break;
                case 10:
                    Armours.Index = 922;
                    Helmets.Index = 923;
                    Belts.Index = 922;
                    Boots.Index = 922;
                    Stones.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Helmet, Usermode = false });
                    break;
                case 11:
                    Armours.Index = 922;
                    Helmets.Index = 922;
                    Belts.Index = 923;
                    Boots.Index = 922;
                    Stones.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Belt, Usermode = false });
                    break;
                case 12:
                    Armours.Index = 922;
                    Helmets.Index = 922;
                    Belts.Index = 922;
                    Boots.Index = 923;
                    Stones.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Boots, Usermode = false });
                    break;
                case 13:
                    Armours.Index = 922;
                    Helmets.Index = 922;
                    Belts.Index = 922;
                    Boots.Index = 922;
                    Stones.Index = 923;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Stone, Usermode = false });
                    break;
                case 14:
                    Necklace.Index = 923;
                    Bracelets.Index = 922;
                    Rings.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Necklace, Usermode = false });
                    break;
                case 15:
                    Necklace.Index = 922;
                    Bracelets.Index = 923;
                    Rings.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Bracelet, Usermode = false });
                    break;
                case 16:
                    Necklace.Index = 922;
                    Bracelets.Index = 922;
                    Rings.Index = 923;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Ring, Usermode = false });
                    break;
                case 17:
                    RecoveryPotion.Index = 923;
                    PowerUp.Index = 922;
                    Scroll.Index = 922;
                    Script.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Potion, Usermode = false, MaxShape = 2 });
                    break;
                case 18:
                    RecoveryPotion.Index = 922;
                    PowerUp.Index = 923;
                    Scroll.Index = 922;
                    Script.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Potion, Usermode = false, MinShape = 3, MaxShape = 4 });
                    break;
                case 19:
                    RecoveryPotion.Index = 922;
                    PowerUp.Index = 922;
                    Scroll.Index = 923;
                    Script.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Scroll, Usermode = false });
                    break;
                case 20:
                    RecoveryPotion.Index = 922;
                    PowerUp.Index = 922;
                    Scroll.Index = 922;
                    Script.Index = 923;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Script, Usermode = false });
                    break;
                case 21:
                    Gem.Index = 923;
                    Orb.Index = 922;
                    Awake.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Gem, Usermode = false, MinShape = 3, MaxShape = 3 });
                    break;
                case 22:
                    Gem.Index = 922;
                    Orb.Index = 923;
                    Awake.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Gem, Usermode = false, MinShape = 4, MaxShape = 4 });
                    break;
                case 23:
                    Gem.Index = 922;
                    Orb.Index = 922;
                    Awake.Index = 923;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Awakening, Usermode = false });
                    break;
                case 24:
                    Warrior.Index = 923;
                    Wizard.Index = 922;
                    Taoist.Index = 922;
                    Assassin.Index = 922;
                    Archer.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Book, Usermode = false, MaxShape = 30 });
                    break;
                case 25:
                    Warrior.Index = 922;
                    Wizard.Index = 923;
                    Taoist.Index = 922;
                    Assassin.Index = 922;
                    Archer.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Book, Usermode = false, MinShape = 31, MaxShape = 60 });
                    break;
                case 26:
                    Warrior.Index = 922;
                    Wizard.Index = 922;
                    Taoist.Index = 923;
                    Assassin.Index = 922;
                    Archer.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Book, Usermode = false, MinShape = 61, MaxShape = 90 });
                    break;
                case 27:
                    Warrior.Index = 922;
                    Wizard.Index = 922;
                    Taoist.Index = 922;
                    Assassin.Index = 923;
                    Archer.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Book, Usermode = false, MinShape = 91, MaxShape = 120 });
                    break;
                case 28:
                    Warrior.Index = 922;
                    Wizard.Index = 922;
                    Taoist.Index = 922;
                    Assassin.Index = 922;
                    Archer.Index = 923;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Book, Usermode = false, MinShape = 121, MaxShape = 150 });
                    break;
                case 29:
                    Materials.Index = 923;
                    Fish.Index = 922;
                    Meat.Index = 922;
                    Ore.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.CraftingMaterial, Usermode = false });
                    break;
                case 30:
                    Materials.Index = 922;
                    Fish.Index = 923;
                    Meat.Index = 922;
                    Ore.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Fish, Usermode = false });
                    break;
                case 31:
                    Materials.Index = 922;
                    Fish.Index = 922;
                    Meat.Index = 923;
                    Ore.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Meat, Usermode = false });
                    break;
                case 32:
                    Materials.Index = 922;
                    Fish.Index = 922;
                    Meat.Index = 922;
                    Ore.Index = 923;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Ore, Usermode = false });
                    break;
                case 33:
                    NoveltyPets.Index = 923;
                    NoveltyEquipment.Index = 922;
                    Mounts.Index = 922;
                    Reins.Index = 922;
                    Bells.Index = 922;
                    Ribbon.Index = 922;
                    Mask.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Pets, Usermode = false, MinShape = 0, MaxShape = 13 });
                    break;
                case 34:
                    NoveltyPets.Index = 922;
                    NoveltyEquipment.Index = 923;
                    Mounts.Index = 922;
                    Reins.Index = 922;
                    Bells.Index = 922;
                    Ribbon.Index = 922;
                    Mask.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Pets, Usermode = false, MinShape = 20, MaxShape = 28 });
                    break;
                case 35:
                    NoveltyPets.Index = 922;
                    NoveltyEquipment.Index = 922;
                    Mounts.Index = 923;
                    Reins.Index = 922;
                    Bells.Index = 922;
                    Ribbon.Index = 922;
                    Mask.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Mount, Usermode = false });
                    break;
                case 36:
                    NoveltyPets.Index = 922;
                    NoveltyEquipment.Index = 922;
                    Mounts.Index = 922;
                    Reins.Index = 923;
                    Bells.Index = 922;
                    Ribbon.Index = 922;
                    Mask.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Reins, Usermode = false });
                    break;
                case 37:
                    NoveltyPets.Index = 922;
                    NoveltyEquipment.Index = 922;
                    Mounts.Index = 922;
                    Reins.Index = 922;
                    Bells.Index = 923;
                    Ribbon.Index = 922;
                    Mask.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Bells, Usermode = false });
                    break;
                case 38:
                    NoveltyPets.Index = 922;
                    NoveltyEquipment.Index = 922;
                    Mounts.Index = 922;
                    Reins.Index = 922;
                    Bells.Index = 922;
                    Ribbon.Index = 923;
                    Mask.Index = 922;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Ribbon, Usermode = false });
                    break;
                case 39:
                    NoveltyPets.Index = 922;
                    NoveltyEquipment.Index = 922;
                    Mounts.Index = 922;
                    Reins.Index = 922;
                    Bells.Index = 922;
                    Ribbon.Index = 922;
                    Mask.Index = 923;
                    Network.Enqueue(new C.MarketSearch { Match = SearchTextBox.Text, Type = ItemType.Mask, Usermode = false });
                    break;
            }
        }

        private void TextBox_TextChanged(object sender, EventArgs e)
        {
            if (uint.TryParse(PriceTextBox.Text, out Amount) && Amount >= MinAmount)
            {
                PriceTextBox.BorderColour = Color.Lime;


                if (Amount > MaxAmount)
                {
                    Amount = MaxAmount;
                    PriceTextBox.Text = MaxAmount.ToString();
                    PriceTextBox.TextBox.SelectionStart = PriceTextBox.Text.Length;
                    SellItemButton.Enabled = true;
                }

                if (Amount == MaxAmount)
                    PriceTextBox.BorderColour = Color.Orange;
                SellItemButton.Enabled = true;
            }
            else
            {
                PriceTextBox.BorderColour = Color.Red;
                SellItemButton.Enabled = false;
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
        public void setdefault()
        {
            ShowAllButton.Index = 920;
            WeaponButton.Index = 920;
            
            DraperyItemsButton.Index = 920;
            Armours.Index = 922;
            Armours.Visible = false;
            Helmets.Index = 922;
            Helmets.Visible = false;
            Belts.Index = 922;
            Belts.Visible = false;
            Boots.Index = 922;
            Boots.Visible = false;
            Stones.Index = 922;
            Stones.Visible = false;
            
            AccessoriesItemsButton.Index = 920;
            Necklace.Index = 922;
            Necklace.Visible = false;
            Bracelets.Index = 922;
            Bracelets.Visible = false;
            Rings.Index = 922;
            Rings.Visible = false;
            
            ConsumableItemsButton.Index = 920;
            RecoveryPotion.Index = 922;
            RecoveryPotion.Visible = false;
            PowerUp.Index = 922;
            PowerUp.Visible = false;
            Scroll.Index = 922;
            Scroll.Visible = false;
            Script.Index = 922;
            Script.Visible = false;
            
            EnhEquipButton.Index = 920;
            Gem.Index = 922;
            Gem.Visible = false;
            Orb.Index = 922;
            Orb.Visible = false;
            Awake.Index = 922;
            Awake.Visible = false;
            
            BooksButton.Index = 920;
            Warrior.Index = 922;
            Warrior.Visible = false;
            Wizard.Index = 922;
            Wizard.Visible = false;
            Taoist.Index = 922;
            Taoist.Visible = false;
            Assassin.Index = 922;
            Assassin.Visible = false;
            Archer.Index = 922;
            Archer.Visible = false;

            CraftingSystemButton.Index = 920;
            Materials.Index = 922;
            Materials.Visible = false;
            Fish.Index = 922;
            Fish.Visible = false;
            Meat.Index = 922;
            Meat.Visible = false;
            Ore.Index = 922;
            Ore.Visible = false;

            PetsItemButton.Index = 920;
            NoveltyPets.Index = 922;
            NoveltyPets.Visible = false;
            NoveltyEquipment.Index = 922;
            NoveltyEquipment.Visible = false;
            Mounts.Index = 922;
            Mounts.Visible = false;
            Reins.Index = 922;
            Reins.Visible = false;
            Bells.Index = 922;
            Bells.Visible = false;
            Ribbon.Index = 922;
            Ribbon.Visible = false;
            Mask.Index = 922;
            Mask.Visible = false;
        }

        public void SetLocations(int i)
        {
            switch (i)
            {
                case 0:
                case 1:
                case 8://8
                    {
                        ShowAllButton.Location = new Point(7, 60);
                        WeaponButton.Location = new Point(7, ShowAllButton.Location.Y + 23);
                        DraperyItemsButton.Location = new Point(7, WeaponButton.Location.Y + 23);
                        AccessoriesItemsButton.Location = new Point(7, DraperyItemsButton.Location.Y + 23);
                        ConsumableItemsButton.Location = new Point(7, AccessoriesItemsButton.Location.Y + 23);
                        EnhEquipButton.Location = new Point(7, ConsumableItemsButton.Location.Y + 23);
                        BooksButton.Location = new Point(7, EnhEquipButton.Location.Y + 23);
                        CraftingSystemButton.Location = new Point(7, BooksButton.Location.Y + 23);
                        PetsItemButton.Location = new Point(7, CraftingSystemButton.Location.Y + 23);
                        break;
                    }
                case 2:
                    {
                        ShowAllButton.Location = new Point(7, 60);
                        WeaponButton.Location = new Point(7, ShowAllButton.Location.Y + 23);
                        DraperyItemsButton.Location = new Point(7, WeaponButton.Location.Y + 23);
                        AccessoriesItemsButton.Location = new Point(7, Stones.Location.Y + 23);
                        ConsumableItemsButton.Location = new Point(7, AccessoriesItemsButton.Location.Y + 23);
                        EnhEquipButton.Location = new Point(7, ConsumableItemsButton.Location.Y + 23);
                        BooksButton.Location = new Point(7, EnhEquipButton.Location.Y + 23);
                        CraftingSystemButton.Location = new Point(7, BooksButton.Location.Y + 23);
                        PetsItemButton.Location = new Point(7, CraftingSystemButton.Location.Y + 23);
                        break;
                    }
                case 3:
                    {
                        ShowAllButton.Location = new Point(7, 60);
                        WeaponButton.Location = new Point(7, ShowAllButton.Location.Y + 23);
                        DraperyItemsButton.Location = new Point(7, WeaponButton.Location.Y + 23);
                        AccessoriesItemsButton.Location = new Point(7, DraperyItemsButton.Location.Y + 23);
                        ConsumableItemsButton.Location = new Point(7, Rings.Location.Y + 23);
                        EnhEquipButton.Location = new Point(7, ConsumableItemsButton.Location.Y + 23);
                        BooksButton.Location = new Point(7, EnhEquipButton.Location.Y + 23);
                        CraftingSystemButton.Location = new Point(7, BooksButton.Location.Y + 23);
                        PetsItemButton.Location = new Point(7, CraftingSystemButton.Location.Y + 23);
                        break;
                    }
                case 4:
                    {
                        ShowAllButton.Location = new Point(7, 60);
                        WeaponButton.Location = new Point(7, ShowAllButton.Location.Y + 23);
                        DraperyItemsButton.Location = new Point(7, WeaponButton.Location.Y + 23);
                        AccessoriesItemsButton.Location = new Point(7, DraperyItemsButton.Location.Y + 23);
                        ConsumableItemsButton.Location = new Point(7, AccessoriesItemsButton.Location.Y + 23);
                        EnhEquipButton.Location = new Point(7, Script.Location.Y + 23);
                        BooksButton.Location = new Point(7, EnhEquipButton.Location.Y + 23);
                        CraftingSystemButton.Location = new Point(7, BooksButton.Location.Y + 23);
                        PetsItemButton.Location = new Point(7, CraftingSystemButton.Location.Y + 23);
                        break;
                    }
                case 5:
                    {
                        ShowAllButton.Location = new Point(7, 60);
                        WeaponButton.Location = new Point(7, ShowAllButton.Location.Y + 23);
                        DraperyItemsButton.Location = new Point(7, WeaponButton.Location.Y + 23);
                        AccessoriesItemsButton.Location = new Point(7, DraperyItemsButton.Location.Y + 23);
                        ConsumableItemsButton.Location = new Point(7, AccessoriesItemsButton.Location.Y + 23);
                        EnhEquipButton.Location = new Point(7, ConsumableItemsButton.Location.Y + 23);
                        BooksButton.Location = new Point(7, Awake.Location.Y + 23);
                        CraftingSystemButton.Location = new Point(7, BooksButton.Location.Y + 23);
                        PetsItemButton.Location = new Point(7, CraftingSystemButton.Location.Y + 23);
                        break;
                    }
                case 6:
                    {
                        ShowAllButton.Location = new Point(7, 60);
                        WeaponButton.Location = new Point(7, ShowAllButton.Location.Y + 23);
                        DraperyItemsButton.Location = new Point(7, WeaponButton.Location.Y + 23);
                        AccessoriesItemsButton.Location = new Point(7, DraperyItemsButton.Location.Y + 23);
                        ConsumableItemsButton.Location = new Point(7, AccessoriesItemsButton.Location.Y + 23);
                        EnhEquipButton.Location = new Point(7, ConsumableItemsButton.Location.Y + 23);
                        BooksButton.Location = new Point(7, EnhEquipButton.Location.Y + 23);
                        CraftingSystemButton.Location = new Point(7, Archer.Location.Y + 23);
                        PetsItemButton.Location = new Point(7, CraftingSystemButton.Location.Y + 23);
                        break;
                    }

                case 7:
                    {
                        ShowAllButton.Location = new Point(7, 60);
                        WeaponButton.Location = new Point(7, ShowAllButton.Location.Y + 23);
                        DraperyItemsButton.Location = new Point(7, WeaponButton.Location.Y + 23);
                        AccessoriesItemsButton.Location = new Point(7, DraperyItemsButton.Location.Y + 23);
                        ConsumableItemsButton.Location = new Point(7, AccessoriesItemsButton.Location.Y + 23);
                        EnhEquipButton.Location = new Point(7, ConsumableItemsButton.Location.Y + 23);
                        BooksButton.Location = new Point(7, EnhEquipButton.Location.Y + 23);
                        CraftingSystemButton.Location = new Point(7, BooksButton.Location.Y + 23);
                        PetsItemButton.Location = new Point(7, Ore.Location.Y + 23);
                        break;
                    }
            }
        }

        public void Hide()
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
        public void Show()
        {
            if (Visible) return;
            Visible = true;
            TMerchantDialog(0);
            SwitchTab(0);
            UpdateInterface();
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
                BorderColour = Color.Lime;

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

        #endregion

            void AuctionRow_BeforeDraw(object sender, EventArgs e)
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

                ExpireLabel.Visible = Listing != null;

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
                PriceLabel.Text = Listing.Price.ToString("###,###,##0");

                NameLabel.ForeColour = GameScene.Scene.GradeNameColor(Listing.Item.Info.Grade);
                if (NameLabel.ForeColour == Color.Yellow)
                    NameLabel.ForeColour = Color.White;

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
