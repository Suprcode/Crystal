using System;
using System.Drawing;
using System.Linq;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class HeroInventoryDialog : MirImageControl
    {
        public MirImageControl[] LockBar = new MirImageControl[4];
        public MirImageControl HPLockBar, MPLockBar;
        public MirItemCell[] Grid;

        public MirButton CloseButton;

        public HeroInventoryDialog()
        {
            Index = 1422;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Visible = false;         

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(299, 2),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            Grid = new MirItemCell[8 * 5];

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    int idx = 8 * y + x;
                    Grid[idx] = new MirItemCell
                    {
                        ItemSlot = 2 + idx,
                        GridType = MirGridType.HeroInventory,
                        Library = Libraries.Items,
                        Parent = this,
                        Location = new Point(x * 36 + 14 + x, y % 5 * 32 + 23 + y % 5),                        
                    };

                    if (idx >= 40)
                        Grid[idx].Visible = false;
                }
            }

            for (int i = 0; i < LockBar.Length; i++)
            {
                LockBar[i] = new MirImageControl
                {
                    Index = 1423,
                    Library = Libraries.Prguse,
                    Location = new Point(14, 56 + i * 33),
                    Parent = this,
                    DrawImage = true,
                    NotControl = true,
                    Visible = false,
                };
            }

            HPLockBar = new MirImageControl
            {
                Index = 1428,
                Library = Libraries.Prguse,
                Location = new Point(57, 196),
                Parent = this,
                DrawImage = true,
                NotControl = true,
                Visible = false,
            };

            MPLockBar = new MirImageControl
            {
                Index = 1429,
                Library = Libraries.Prguse,
                Location = new Point(162, 196),
                Parent = this,
                DrawImage = true,
                NotControl = true,
                Visible = false,
            };

            RefreshInterface();
        }

        void RefreshInterface()
        {
            foreach (MirItemCell grid in Grid)
            {
                grid.Enabled = grid.ItemSlot < GameScene.Hero.Inventory.Length;
            }

            for (int i = 0; i < LockBar.Length; i++)
            {
                LockBar[i].Visible = GameScene.Hero.Inventory.Length < 11 + 8 * i;
            }

            HPLockBar.Visible = true;
            MPLockBar.Visible = true;
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

        public void DisplayItemGridEffect(ulong id, int type = 0)
        {
            MirItemCell cell = GetCell(id);

            if (cell.Item == null) return;

            MirAnimatedControl animEffect = null;

            switch (type)
            {
                case 0:
                    animEffect = new MirAnimatedControl
                    {
                        Animated = true,
                        AnimationCount = 9,
                        AnimationDelay = 150,
                        Index = 410,
                        Library = Libraries.Prguse,
                        Location = cell.Location,
                        Parent = this,
                        Loop = false,
                        NotControl = true,
                        UseOffSet = true,
                        Blending = true,
                        BlendingRate = 1F
                    };
                    animEffect.AfterAnimation += (o, e) => animEffect.Dispose();
                    SoundManager.PlaySound(20000 + (ushort)Spell.MagicShield * 10);
                    break;
            }
        }
    }

    public sealed class HeroBeltDialog : MirImageControl
    {
        public MirLabel[] Key = new MirLabel[2];
        public MirButton CloseButton, RotateButton;
        public MirItemCell[] Grid;

        public HeroBeltDialog()
        {
            Index = 1921;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Visible = true;
            Location = new Point(GameScene.Scene.MainDialog.Location.X + 475, Settings.ScreenHeight - 150);

            BeforeDraw += BeltPanel_BeforeDraw;

            for (int i = 0; i < Key.Length; i++)
            {
                Key[i] = new MirLabel
                {
                    Parent = this,
                    Size = new Size(26, 14),
                    Location = new Point(8 + i * 35, 2),
                    Text = (i + 7).ToString()
                };
            }

            RotateButton = new MirButton
            {
                HoverIndex = 1927,
                Index = 1926,
                Location = new Point(82, 3),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 1928,
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.Rotate
            };
            RotateButton.Click += (o, e) => Flip();

            CloseButton = new MirButton
            {
                HoverIndex = 1924,
                Index = 1923,
                Location = new Point(82, 19),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 1925,
                Sound = SoundList.ButtonA,
                Hint = string.Format(GameLanguage.Close, CMain.InputKeys.GetKey(KeybindOptions.Belt))
            };
            CloseButton.Click += (o, e) => Hide();

            Grid = new MirItemCell[2];

            for (int x = 0; x < 2; x++)
            {
                Grid[x] = new MirItemCell
                {
                    ItemSlot = x,
                    Size = new Size(32, 32),
                    GridType = MirGridType.HeroInventory,
                    Library = Libraries.Items,
                    Parent = this,
                    Location = new Point(x * 35 + 12, 3),
                };
            }

        }

        private void BeltPanel_BeforeDraw(object sender, EventArgs e)
        {
            //if Transparent return

            if (Libraries.Prguse != null)
                Libraries.Prguse.Draw(Index == 1921 ? 1934 : 1946, DisplayLocation, Color.White, false, 0.5F);
        }

        public void Flip()
        {
            //0,70 LOCATION
            if (Index == 1921)
            {
                Index = 1943;
                Location = new Point(0, 446);

                for (int x = 0; x < 2; x++)
                    Grid[x].Location = new Point(3, x * 35 + 12);

                CloseButton.Index = 1935;
                CloseButton.HoverIndex = 1936;
                CloseButton.Location = new Point(3, 82);
                CloseButton.PressedIndex = 1937;

                RotateButton.Index = 1938;
                RotateButton.HoverIndex = 1939;
                RotateButton.Location = new Point(19, 82);
                RotateButton.PressedIndex = 1940;

            }
            else
            {
                Index = 1921;
                Location = new Point(GameScene.Scene.MainDialog.Location.X + 475, Settings.ScreenHeight - 150);

                for (int x = 0; x < 2; x++)
                    Grid[x].Location = new Point(x * 35 + 12, 3);

                CloseButton.Index = 1923;
                CloseButton.HoverIndex = 1924;
                CloseButton.Location = new Point(82, 19);
                CloseButton.PressedIndex = 1925;

                RotateButton.Index = 1926;
                RotateButton.HoverIndex = 1927;
                RotateButton.Location = new Point(82, 3);
                RotateButton.PressedIndex = 1928;
            }

            for (int i = 0; i < Key.Length; i++)
            {
                Key[i].Location = (Index != 1921) ? new Point(-1, 11 + i * 35) : new Point(8 + i * 35, 2);
            }
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
}
