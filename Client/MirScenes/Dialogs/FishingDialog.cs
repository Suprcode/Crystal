using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C = ClientPackets;
using S = ServerPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class FishingDialog : MirImageControl
    {
        public MirLabel TitleLabel;
        public MirButton CloseButton;
        public MirItemCell[] Grid;

        public MirControl FishingRod;

        public FishingDialog()
        {
            Index = 1340;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Location = Center;
            BeforeDraw += FishingDialog_BeforeDraw;

            TitleLabel = new MirLabel
            {
                Location = new Point(10, 4),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Parent = this,
                NotControl = true,
                Size = new Size(180, 20),
            };

            FishingRod = new MirControl
            {
                Parent = this,
                Location = new Point(0, 30),
                NotControl = true,
            };
            FishingRod.BeforeDraw += FishingRod_BeforeDraw;

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(175, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };

            CloseButton.Click += (o, e) => Hide();

            Grid = new MirItemCell[Enum.GetNames(typeof(FishingSlot)).Length];

            Grid[(int)FishingSlot.Hook] = new MirItemCell
            {
                ItemSlot = (int)FishingSlot.Hook,
                GridType = MirGridType.Fishing,
                Parent = this,
                Size = new Size(34, 30),
                Location = new Point(17, 203),
            };
            Grid[(int)FishingSlot.Float] = new MirItemCell
            {
                ItemSlot = (int)FishingSlot.Float,
                GridType = MirGridType.Fishing,
                Parent = this,
                Size = new Size(34, 30),
                Location = new Point(17, 241),
            };

            Grid[(int)FishingSlot.Bait] = new MirItemCell
            {
                ItemSlot = (int)FishingSlot.Bait,
                GridType = MirGridType.Fishing,
                Parent = this,
                Size = new Size(34, 30),
                Location = new Point(57, 241),
            };

            Grid[(int)FishingSlot.Finder] = new MirItemCell
            {
                ItemSlot = (int)FishingSlot.Finder,
                GridType = MirGridType.Fishing,
                Parent = this,
                Size = new Size(34, 30),
                Location = new Point(97, 241),
            };

            Grid[(int)FishingSlot.Reel] = new MirItemCell
            {
                ItemSlot = (int)FishingSlot.Reel,
                GridType = MirGridType.Fishing,
                Parent = this,
                Size = new Size(34, 30),
                Location = new Point(137, 241),
            };
        }

        void FishingDialog_BeforeDraw(object sender, EventArgs e)
        {
            UserItem item = MapObject.User.Equipment[(int)EquipmentSlot.Weapon];

            if (MapObject.User.HasFishingRod && item != null)
            {
                TitleLabel.Text = item.Name;
            }
        }

        void FishingRod_BeforeDraw(object sender, EventArgs e)
        {
            int FishingImage = 0;
            if (MapObject.User.HasFishingRod)
            {
                UserItem rod = MapObject.User.Equipment[(int)EquipmentSlot.Weapon];

                if (GameScene.User.Weapon == 49)
                    FishingImage = 1333;
                else if (GameScene.User.Weapon == 50)
                    FishingImage = 1335;

                if (rod != null && rod.Slots.Length >= 5 && rod.Slots[(int)FishingSlot.Hook] != null)
                {
                    FishingImage++;
                }
            }

            Libraries.StateItems.Draw(FishingImage, new Point(Location.X + 10, Location.Y + 40), Color.White, false);
        }


        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }
        public void Show()
        {
            if (Visible) return;

            if (!GameScene.User.HasFishingRod)
            {
                MirMessageBox messageBox = new MirMessageBox("You are not holding a fishing rod.", MirMessageBoxButtons.OK);
                messageBox.Show();
                return;
            }

            Visible = true;
        }

        public MirItemCell GetCell(ulong id)
        {
            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i].Item == null || Grid[i].Item.UniqueID != id) continue;
                return Grid[i];
            }
            return null;
        }
    }
    public sealed class FishingStatusDialog : MirImageControl
    {
        public MirImageControl TitleLabel, AutoCastBox, ESCTick, ESCExit, FishDisableButton;
        public MirControl ChanceBar, ProgressBar;
        public MirLabel ChanceLabel;
        public MirButton CloseButton, AutoCastButton, FishButton, ESCExitButton;

        public int ChancePercent = 0, ProgressPercent = 0;

        private bool _canAutoCast = false;
        private bool _autoCast = false;
        public bool bEscExit = false;

        public FishingStatusDialog()
        {
            Index = 1341;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Size = new Size(244, 128);
            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, 300);
            BeforeDraw += FishingStatusDialog_BeforeDraw;

            ChanceBar = new MirControl
            {
                Parent = this,
                Location = new Point(14, 64),
                NotControl = true,
            };
            ChanceBar.BeforeDraw += ChanceBar_BeforeDraw;

            ChanceLabel = new MirLabel
            {
                Location = new Point(14, 62),
                Size = new Size(216, 12),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Parent = this,
                NotControl = true,
            };

            ProgressBar = new MirControl
            {
                Parent = this,
                Location = new Point(14, 79),
                NotControl = true,
            };
            ProgressBar.BeforeDraw += ProgressBar_BeforeDraw;

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(216, 4),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) =>
            {
                Cancel();
            };

            FishDisableButton = new MirImageControl
            {
                Index = 149,
                Location = new Point(47, 95),
                Library = Libraries.Title,
                Parent = this,
                NotControl = true
            };

            FishButton = new MirAnimatedButton()
            {
                Animated = true,
                AnimationCount = 10,
                Loop = true,
                AnimationDelay = 130,
                Index = 170,
                PressedIndex = 142,
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(47, 95),
                Sound = SoundList.ButtonA,
                Visible = false
            };
            FishButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.FishingCast { CastOut = false });
            };

            AutoCastButton = new MirButton
            {
                Index = 180,
                HoverIndex = 181,
                PressedIndex = 182,
                Location = new Point(110, 95),
                Library = Libraries.Title,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            AutoCastButton.Click += (o, e) =>
            {
                if (_canAutoCast)
                {
                    _autoCast = !_autoCast;

                    //AutoCastTick.Visible = _autoCast;
                    AutoCastBox.Index = _autoCast ? 1344 : 1343;

                    Network.Enqueue(new C.FishingChangeAutocast { AutoCast = _autoCast });
                }
            };

            AutoCastBox = new MirImageControl
            {
                Index = 1343,
                Location = new Point(172, 95),
                Library = Libraries.Prguse,
                Parent = this
            };

            ESCExitButton = new MirButton
            {
                Index = 1346,
                HoverIndex = 1346,
                PressedIndex = 1346,
                Location = new Point(135, 41),
                Library = Libraries.Prguse,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            ESCExitButton.Click += (o, e) =>
            {
                bEscExit = !bEscExit;
                ESCTick.Visible = bEscExit;
            };

            ESCTick = new MirImageControl
            {
                Index = 1347,
                Location = new Point(135, 41),
                Library = Libraries.Prguse,
                Parent = this,
                Visible = false,
                NotControl = true,
            };

            ESCExit = new MirImageControl
            {
                Index = 45,
                Location = new Point(150, 40),
                Library = Libraries.Title,
                Parent = this,
                NotControl = true,
            };
        }

        void FishingStatusDialog_BeforeDraw(object sender, EventArgs e)
        {
            bool oldCanAutoCast = _canAutoCast;

            if (MapObject.User.HasFishingRod)
            {
                UserItem rod = MapObject.User.Equipment[(int)EquipmentSlot.Weapon];

                if (rod == null || rod.Slots.Length < 5 || rod.Slots[(int)FishingSlot.Reel] == null)
                {
                    _canAutoCast = false;
                    AutoCastBox.Visible = false;
                    AutoCastButton.Visible = false;
                }
                else
                {
                    _canAutoCast = true;
                    AutoCastBox.Visible = true;
                    AutoCastButton.Visible = true;
                }
            }

            if (_autoCast && !_canAutoCast)
            {
                _autoCast = false;

                Network.Enqueue(new C.FishingChangeAutocast { AutoCast = _autoCast });
            }
        }

        void ChanceBar_BeforeDraw(object sender, EventArgs e)
        {
            if (Libraries.Prguse == null) return;

            int width;

            width = (int)(2.16 * ChancePercent);

            if (width < 0) width = 0;
            if (width > 216) width = 216;
            Rectangle r = new Rectangle(0, 0, width, 12);
            Libraries.Prguse.Draw(1342, r, new Point(ChanceBar.DisplayLocation.X, ChanceBar.DisplayLocation.Y), Color.White, false);
        }

        void ProgressBar_BeforeDraw(object sender, EventArgs e)
        {
            if (Libraries.Prguse == null) return;

            int width;

            width = (int)(2.16 * ProgressPercent);

            if (width < 0) width = 0;
            if (width > 216) width = 216;

            Rectangle r = new Rectangle(0, 0, width, 8);
            Libraries.Prguse.Draw(1349, r, new Point(ProgressBar.DisplayLocation.X, ProgressBar.DisplayLocation.Y), Color.White, false);
        }

        public void Cancel()
        {
            if (Visible)
                Network.Enqueue(new C.FishingCast { CastOut = false });
            Hide();

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

    }
}
