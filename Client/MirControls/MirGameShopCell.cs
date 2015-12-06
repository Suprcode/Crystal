using System;
using System.Drawing;
using System.Windows.Forms;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirScenes;
using Client.MirSounds;
using Client.MirScenes.Dialogs;
using C = ClientPackets;

namespace Client.MirControls
{
    public sealed class GameShopCell : MirImageControl
    {
        public MirLabel nameLabel, typeLabel, goldLabel, gpLabel, stockLabel, StockLabel, countLabel;
        public GameShopItem Item;
        public UserItem ShowItem;
        Rectangle ItemDisplayArea;
        public MirButton BuyItem, PreviewItem;
        public MirImageControl ViewerBackground;
        public byte Quantity = 1;
        public MirButton quantityUp, quantityDown;
        public MirLabel quantity;
        public GameShopViewer Viewer;

        public GameShopCell()
        {
            Size = new Size(125, 146);
            Index = 750;
            Library = Libraries.Title;
            MouseLeave += (o, e) =>
            {
                GameScene.Scene.DisposeItemLabel();
                GameScene.HoverItem = null;
                ShowItem = null;
            };

            nameLabel = new MirLabel
            {
                Size = new Size(125, 15),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Location = new Point(0, 13),
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 8F),
            };

            goldLabel = new MirLabel
            {
                Size = new Size(95, 20),
                DrawFormat = TextFormatFlags.RightToLeft | TextFormatFlags.Right,
                Location = new Point(2, 102),
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 8F)
            };

            gpLabel = new MirLabel
            {
                Size = new Size(95, 20),
                DrawFormat = TextFormatFlags.RightToLeft | TextFormatFlags.Right,
                Location = new Point(2, 81),
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 8F)
            };

            StockLabel = new MirLabel
            {
                Size = new Size(40, 20),
                Location = new Point(53, 37),
                Parent = this,
                NotControl = true,
                ForeColour = Color.Gray,
                Font = new Font(Settings.FontName, 7F),
                Text = "STOCK:"
            };

            stockLabel = new MirLabel
            {
                Size = new Size(20, 20),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Location = new Point(93, 37),
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 7F),
            };

            countLabel = new MirLabel
            {
                Size = new Size(30, 20),
                DrawFormat = TextFormatFlags.Right,
                Location = new Point(16, 60),
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 7F),
            };


            BuyItem = new MirButton
            {
                Index = 778,
                HoverIndex = 779,
                PressedIndex = 780,
                Location = new Point(42, 122),
                Library = Libraries.Title,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            BuyItem.Click += (o, e) =>
            {
                BuyProduct();
            };

            PreviewItem = new MirButton
            {
                Index = 781,
                HoverIndex = 782,
                PressedIndex = 783,
                Location = new Point(8, 122),
                Library = Libraries.Title,
                Parent = this,
                Sound = SoundList.ButtonA,
                Visible = false,
            };
            PreviewItem.Click += (o, e) =>
                {
                    GameScene.Scene.GameShopDialog.Viewer.Dispose();
                    GameScene.Scene.GameShopDialog.Viewer = new GameShopViewer
                    {
                        Parent = GameScene.Scene.GameShopDialog,
                        Visible = true,
                        Location = this.Location.X < 350 ? new Point(416, 115) : new Point(151, 115),
                    };
                    GameScene.Scene.GameShopDialog.Viewer.ViewerItem = Item;
                    GameScene.Scene.GameShopDialog.Viewer.UpdateViewer();
                };


            quantityUp = new MirButton
            {
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(97, 56),
                Sound = SoundList.ButtonA,
            };
            quantityUp.Click += (o, e) =>
            {
                if (CMain.Shift) Quantity += 10;
                else Quantity++;

                if (((decimal)(Quantity * Item.Count) / Item.Info.StackSize) > 5) Quantity = ((5 * Item.Info.StackSize) / Item.Count) > 99 ? Quantity = 99 : Quantity = (byte)((5 * Item.Info.StackSize) / Item.Count);
                if (Quantity >= 99) Quantity = 99;
                if (Item.Stock != 0 && Quantity > Item.Stock) Quantity = (byte)Item.Stock;
            };

            quantityDown = new MirButton
            {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(55, 56),
                Sound = SoundList.ButtonA,
            };
            quantityDown.Click += (o, e) =>
            {

                if (CMain.Shift) Quantity -= 10;
                else Quantity--;

                if (Quantity <= 1 || Quantity > 99) Quantity = 1;
            };

            quantity = new MirLabel
            {
                Size = new Size(20, 13),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Location = new Point(74, 56),
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 8F),
            };



        }

        public void BuyProduct()
        {
            uint CreditCost;
            uint GoldCost;
            MirMessageBox messageBox;

            if (Item.CreditPrice * Quantity <= GameScene.Credit)
            {
                CreditCost = Item.CreditPrice * Quantity;
                messageBox = new MirMessageBox(string.Format("Are you sure would you like to buy {1} x {0}({3}) for {2} Credits?", Item.Info.FriendlyName, Quantity, CreditCost, Item.Count), MirMessageBoxButtons.YesNo);
            }
            else
            { //Needs to attempt to pay with gold and credits
                if (GameScene.Gold >= (((Item.GoldPrice * Quantity) / (Item.CreditPrice * Quantity)) * ((Item.CreditPrice * Quantity) - GameScene.Credit)))
                {
                    GoldCost = ((Item.GoldPrice * Quantity) / (Item.CreditPrice * Quantity)) * ((Item.CreditPrice * Quantity) - GameScene.Credit);
                    CreditCost = GameScene.Credit;
                    if (CreditCost == 0)
                    {
                        messageBox = new MirMessageBox(string.Format("Are you sure would you like to buy {1} x {0}({3}) for {2} Gold?", Item.Info.FriendlyName, Quantity, GoldCost, Item.Count), MirMessageBoxButtons.YesNo);
                    }
                    else
                    {
                        messageBox = new MirMessageBox(string.Format("Are you sure would you like to buy {1} x {0}({4}) for {2} Credit and {3} Gold?", Item.Info.FriendlyName, Quantity, CreditCost, GoldCost, Item.Count), MirMessageBoxButtons.YesNo);
                    }
                }
                else
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("You can't afford the selected item.", ChatType.System);
                    return;
                }

            }

            messageBox.YesButton.Click += (o, e) => Network.Enqueue(new C.GameshopBuy { GIndex = Item.GIndex, Quantity = Quantity });
            messageBox.NoButton.Click += (o, e) => { };
            messageBox.Show();
        }

        public override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (GameScene.HoverItem != null && (Item.Info.Index != GameScene.HoverItem.Info.Index))
            {
                GameScene.Scene.DisposeItemLabel();
                GameScene.HoverItem = null;
                ShowItem = null;
            }

            if (ShowItem == null && ItemDisplayArea != null && ItemDisplayArea.Contains(CMain.MPoint))
            {
                ShowItem = new UserItem(Item.Info) { MaxDura = Item.Info.Durability, CurrentDura = Item.Info.Durability, Count = Item.Count };
                GameScene.Scene.CreateItemLabel(ShowItem);
            }
            else if (ShowItem != null && ItemDisplayArea != null && !ItemDisplayArea.Contains(CMain.MPoint))
            {
                GameScene.Scene.DisposeItemLabel();
                GameScene.HoverItem = null;
                ShowItem = null;
            }
        }

        public void UpdateText()
        {
            nameLabel.Text = (Item.Info.Type == ItemType.Pets && Item.Info.Shape == 26 && Item.Info.Effect != 7) ? "WonderDrug" : Item.Info.FriendlyName;
            nameLabel.Text = nameLabel.Text.Length > 17 ? nameLabel.Text.Substring(0, 17) : nameLabel.Text;
            nameLabel.ForeColour = GameScene.Scene.GradeNameColor(Item.Info.Grade);
            quantity.Text = Quantity.ToString();
            goldLabel.Text = (Item.GoldPrice * Quantity).ToString("###,###,##0");
            gpLabel.Text = (Item.CreditPrice * Quantity).ToString("###,###,##0");
            if (Item.Stock >= 99) stockLabel.Text = "99+";
            if (Item.Stock == 0) stockLabel.Text = "∞";
            else stockLabel.Text = Item.Stock.ToString();
            countLabel.Text = Item.Count.ToString();

            if (Item.Info.Type == ItemType.Mount || Item.Info.Type == ItemType.Weapon || Item.Info.Type == ItemType.Armour || Item.Info.Type == ItemType.Transform)
            {
                PreviewItem.Visible = true;
                BuyItem.Location = new Point(75, 122);
            }
        }

        protected internal override void DrawControl()
        {
            
            base.DrawControl();

            if (Item == null) return;

            UpdateText();

            Size size = Libraries.Items.GetTrueSize(Item.Info.Image);
            Point offSet = new Point((32 - size.Width) / 2, (32 - size.Height) / 2);

            Libraries.Items.Draw(Item.Info.Image, offSet.X + DisplayLocation.X + 12, offSet.Y + DisplayLocation.Y + 40);
            ItemDisplayArea = new Rectangle(new Point(offSet.X + DisplayLocation.X + 12, offSet.Y + DisplayLocation.Y + 40), size);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Item = null;
            GameScene.HoverItem = null;
            ShowItem = null;
        }

    }

    public sealed class GameShopViewer : MirImageControl
    {

        public MirAnimatedControl PreviewImage, WeaponImage, WeaponImage2, MountImage;

        public int StartIndex = 0;
        public int Direction = 6;
        public GameShopItem ViewerItem;

        public MirButton RightDirection, LeftDirection, CloseButton;


        public GameShopViewer()
        {
            Index = 785;// 314;
            Library = Libraries.Title;// Libraries.Prguse2;
            Location = new Point(405, 108);
            BeforeDraw += GameShopViewer_BeforeDraw;
            //Click += (o, e) =>
            //{
            //Visible = false;
            //};

            CloseButton = new MirButton
            {
                HoverIndex = 362,
                Index = 361,
                Location = new Point(230, 8),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 363,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) =>
            {
                Visible = false;
            };

            WeaponImage = new MirAnimatedControl
            {
                Animated = false,
                Location = new Point(105, 160),
                AnimationCount = 6,
                AnimationDelay = 150,
                Index = 0,
                Library = Libraries.Prguse,
                Loop = true,
                Parent = this,
                UseOffSet = true,
                NotControl = true,
            };
            WeaponImage2 = new MirAnimatedControl
            {
                Animated = false,
                Location = new Point(105, 160),
                AnimationCount = 6,
                AnimationDelay = 150,
                Index = 0,
                Library = Libraries.Prguse,
                Loop = true,
                Parent = this,
                UseOffSet = true,
                NotControl = true,
            };
            MountImage = new MirAnimatedControl
            {
                Animated = false,
                Location = new Point(105, 160),
                AnimationCount = 8,
                AnimationDelay = 150,
                Index = 0,
                Library = Libraries.Prguse,
                Loop = true,
                Parent = this,
                UseOffSet = true,
                NotControl = true,
            };

            PreviewImage = new MirAnimatedControl
            {
                Animated = false,
                Location = new Point(105, 160),
                AnimationCount = 6,
                AnimationDelay = 150,
                Index = 0,
                Library = Libraries.Prguse,
                Loop = true,
                Parent = this,
                UseOffSet = true,
                NotControl = true,
            };

            RightDirection = new MirButton
            {
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(160, 282),
                Sound = SoundList.ButtonA,
            };
            RightDirection.Click += (o, e) =>
            {
                Direction++;
                if (Direction > 8) Direction = 1;

                UpdateViewer();
            };

            LeftDirection = new MirButton
            {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(81, 282),
                Sound = SoundList.ButtonA,
            };
            LeftDirection.Click += (o, e) =>
            {
                Direction--;
                if (Direction == 0) Direction = 8;

                UpdateViewer();
            };

        }

        public void UpdateViewer()
        {
            this.Visible = true;
            if (ViewerItem.Info.Type == ItemType.Weapon) DrawWeapon();
            if (ViewerItem.Info.Type == ItemType.Armour) DrawArmour();
            if (ViewerItem.Info.Type == ItemType.Mount) DrawMount();
            if (ViewerItem.Info.Type == ItemType.Transform) DrawTransform();
        }

        private void GameShopViewer_BeforeDraw(object sender, EventArgs e)
        {
            BringToFront();
        }

        private void DrawMount()
        {
            WeaponImage.Visible = false;
            WeaponImage2.Visible = false;
            if (GameScene.User.Equipment[(int)EquipmentSlot.Armour] != null)
                PreviewImage.Library = Libraries.CArmours[GameScene.User.Equipment[(int)EquipmentSlot.Armour].Info.Shape];
            else
                PreviewImage.Library = Libraries.CArmours[0];

            if (GameScene.User.Gender == MirGender.Male)
                PreviewImage.Index = 448 + (8 * (Direction - 1));
            else
                PreviewImage.Index = 1256 + (8 * (Direction - 1));

            PreviewImage.AnimationCount = 8;
            

            MountImage.Library = Libraries.Mounts[ViewerItem.Info.Shape];
            MountImage.Index = 32 + (8 * (Direction - 1));

            PreviewImage.Animated = true;
            MountImage.Animated = true;

            MountImage.Visible = true;
        }

        private void DrawWeapon()
        {
            MountImage.Visible = false;

            if (GameScene.User.Equipment[(int)EquipmentSlot.Armour] != null)
                PreviewImage.Library = Libraries.CArmours[GameScene.User.Equipment[(int)EquipmentSlot.Armour].Info.Shape];
            else
                PreviewImage.Library = Libraries.CArmours[0];

            if (GameScene.User.Gender == MirGender.Male)
                PreviewImage.Index = 32 + (6 * (Direction - 1));
            else
                PreviewImage.Index = 840 + (6 * (Direction - 1));




            if (Direction > 1 && Direction < 5)
                WeaponImage.BringToFront();
            else
                PreviewImage.BringToFront();


            if (ViewerItem.Info.Shape >= 100 && ViewerItem.Info.Shape <= 199)
            {
                WeaponImage.Library = Libraries.AWeaponsR[ViewerItem.Info.Shape - 100];
                WeaponImage2.Library = Libraries.AWeaponsL[ViewerItem.Info.Shape - 100];
                WeaponImage2.Visible = true;
                WeaponImage2.Index = 32 + (6 * (Direction - 1));

                if (Direction >= 2 && Direction <= 3)
                {
                    WeaponImage2.BringToFront();
                    PreviewImage.BringToFront();
                    WeaponImage.BringToFront();
                }
                else if (Direction == 8 || Direction == 7)
                {
                    WeaponImage.BringToFront();
                    PreviewImage.BringToFront();
                    WeaponImage2.BringToFront();
                }
                else
                {
                    WeaponImage.BringToFront();
                    PreviewImage.BringToFront();
                }

            }
            else
            {
                WeaponImage2.Visible = false;

            }
            if (ViewerItem.Info.Shape >= 200)
            {
                WeaponImage.Library = Libraries.ARWeapons[ViewerItem.Info.Shape - 200];
                if (Direction >= 6 && Direction <= 8)
                {
                    PreviewImage.BringToFront();
                    WeaponImage.BringToFront();
                }
                    
            }
            
            if (ViewerItem.Info.Shape < 100) WeaponImage.Library = Libraries.CWeapons[ViewerItem.Info.Shape];


            WeaponImage.Index = 32 + (6 * (Direction - 1));

            PreviewImage.AnimationCount = 6;
            PreviewImage.Animated = true;
            WeaponImage2.Animated = true;
            WeaponImage.Animated = true;

            WeaponImage.Visible = true;
        }

        private void DrawArmour()
        {
            WeaponImage.Visible = false;
            WeaponImage2.Visible = false;
            MountImage.Visible = false;

            if (ViewerItem.Info.RequiredGender == RequiredGender.Male)
                PreviewImage.Index = 32 + (6 * (Direction - 1));
            else
                PreviewImage.Index = 840 + (6 * (Direction - 1));

            PreviewImage.Library = Libraries.CArmours[ViewerItem.Info.Shape];
            PreviewImage.AnimationCount = 6;
            PreviewImage.Animated = true;
            
        }

        private void DrawTransform()
        {
            WeaponImage.Visible = false;
            WeaponImage2.Visible = false;
            MountImage.Visible = false;

            PreviewImage.Index = 32 + (6 * (Direction - 1));

            PreviewImage.Library = Libraries.Transform[ViewerItem.Info.Shape];
            PreviewImage.AnimationCount = 6;
            PreviewImage.Animated = true;

        }

        private void DrawMask()
        {


        }

    }
}
