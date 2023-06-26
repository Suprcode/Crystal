using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class HeroInventoryDialog : MirImageControl
    {
        public MirImageControl[] LockBar = new MirImageControl[4];
        public MirImageControl HPLockBar, MPLockBar;
        public MirItemCell[] Grid;
        public MirButton HPButton, MPButton;
        public MirLabel AutoHPPercentLabel, AutoMPPercentLabel;
        public MirItemCell HPItem, MPItem;

        private bool AutoPot => GameScene.Hero.AutoPot;
        private uint AutoHPPercent => GameScene.Hero.AutoHPPercent;
        private uint AutoMPPercent => GameScene.Hero.AutoMPPercent;

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

            HPButton = new MirButton
            {
                Index = 560,
                HoverIndex = 561,
                PressedIndex = 562,
                Library = Libraries.Title,
                Location = new Point(58, Size.Height - 60),
                Parent = this,
                Size = new Size(60, 25),
                Sound = SoundList.ButtonA,
                Visible = false,
            };
            HPButton.Click += (o1, e) =>
            {
                MirAmountBox amountBox = new MirAmountBox("Enter a value", 116, 99);
                amountBox.OKButton.Click += (o, a) => Network.Enqueue(new C.SetAutoPotValue { Stat = Stat.HP, Value = amountBox.Amount });
                amountBox.Show();
            };

            MPButton = new MirButton
            {
                Index = 563,
                HoverIndex = 564,
                PressedIndex = 565,
                Library = Libraries.Title,
                Location = new Point(206, Size.Height - 60),
                Parent = this,
                Size = new Size(60, 25),
                Sound = SoundList.ButtonA,
                Visible = false,
            };
            MPButton.Click += (o1, e) =>
            {
                MirAmountBox amountBox = new MirAmountBox("Enter a value", 116, 99);
                amountBox.OKButton.Click += (o, a) => Network.Enqueue(new C.SetAutoPotValue { Stat = Stat.MP, Value = amountBox.Amount });
                amountBox.Show();
            };

            AutoHPPercentLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(HPButton.Location.X, HPButton.Location.Y + 27),
                AutoSize = false,
                Size = new Size(60, 25),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Visible = false
            };

            AutoMPPercentLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(MPButton.Location.X, MPButton.Location.Y + 27),
                AutoSize = false,
                Size = new Size(60, 25),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Visible = false
            };

            HPItem = new MirItemCell
            {
                ItemSlot = 0,
                GridType = MirGridType.HeroHPItem,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(HPButton.Location.X + 64, HPButton.Location.Y + 5),
                Visible = false
            };

            MPItem = new MirItemCell
            {
                ItemSlot = 0,
                GridType = MirGridType.HeroMPItem,
                Library = Libraries.Items,
                Parent = this,
                Location = new Point(MPButton.Location.X - 40, MPButton.Location.Y + 5),
                Visible = false
            };

            RefreshInterface();
        }

        public void RefreshInterface()
        {
            foreach (MirItemCell grid in Grid)
            {
                grid.Enabled = grid.ItemSlot < GameScene.Hero.Inventory.Length;
            }

            for (int i = 0; i < LockBar.Length; i++)
            {
                LockBar[i].Visible = GameScene.Hero.Inventory.Length < 11 + 8 * i;
            }

            HPLockBar.Visible = !AutoPot;
            MPLockBar.Visible = !AutoPot;
            HPButton.Visible = AutoPot;
            MPButton.Visible = AutoPot;
            AutoHPPercentLabel.Visible = AutoPot;
            AutoMPPercentLabel.Visible = AutoPot;
            HPItem.Visible = AutoPot;
            MPItem.Visible = AutoPot;
            AutoHPPercentLabel.Text = AutoHPPercent.ToString() + '%';
            AutoMPPercentLabel.Text = AutoMPPercent.ToString() + '%';
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
    public sealed class HeroMenuPanel : MirImageControl
    {
        public MirButton HeroMagicsButton, HeroInventoryButton, HeroEquipmentButton;

        public HeroMenuPanel(MirControl parent)
        {
            Index = 2179;
            Library = Libraries.Prguse;
            Size = new Size(24, 61);
            Parent = parent;

            Location = new Point(((Settings.ScreenWidth / 2) - (Size.Width / 2)) + 362, Settings.ScreenHeight - Size.Height - 77);

            HeroMagicsButton = new MirButton
            {
                Index = 2173,
                HoverIndex = 2174,
                PressedIndex = 2175,
                Library = Libraries.Prguse,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(3, 3),
                Hint = string.Format(GameLanguage.HeroSkills, CMain.InputKeys.GetKey(KeybindOptions.HeroSkills))
            };
            HeroMagicsButton.Click += (o, e) =>
            {
                if (GameScene.Scene.HeroDialog.Visible && GameScene.Scene.HeroDialog.SkillPage.Visible)
                    GameScene.Scene.HeroDialog.Hide();
                else
                {
                    GameScene.Scene.HeroDialog.Show();
                    GameScene.Scene.HeroDialog.ShowSkillPage();
                }
            };

            HeroInventoryButton = new MirButton
            {
                Index = 2170,
                HoverIndex = 2171,
                PressedIndex = 2172,
                Library = Libraries.Prguse,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(3, 20),
                Hint = string.Format(GameLanguage.HeroInventory, CMain.InputKeys.GetKey(KeybindOptions.HeroInventory))
            };
            HeroInventoryButton.Click += (o, e) =>
            {
                GameScene.Scene.HeroInventoryDialog.Visible = !GameScene.Scene.HeroInventoryDialog.Visible;
            };

            HeroEquipmentButton = new MirButton
            {
                Index = 2176,
                HoverIndex = 2177,
                PressedIndex = 2178,
                Library = Libraries.Prguse,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(3, 37),
                Hint = string.Format(GameLanguage.HeroCharacter, CMain.InputKeys.GetKey(KeybindOptions.HeroEquipment))
            };
            HeroEquipmentButton.Click += (o, e) =>
            {
                if (GameScene.Scene.HeroDialog.Visible && GameScene.Scene.HeroDialog.CharacterPage.Visible)
                    GameScene.Scene.HeroDialog.Hide();
                else
                {
                    GameScene.Scene.HeroDialog.Show();
                    GameScene.Scene.HeroDialog.ShowCharacterPage();
                }
            };
        }

        public void Toggle()
        {
            Visible = !Visible;
        }
    }
    public sealed class HeroInfoPanel : MirImageControl
    {
        private MirImageControl Avatar, NameContainer, HealthContainer, HealthBar, ManaBar, ExperienceBar, DangerAvatar, DeadAvatar;
        private MirLabel NameLabel, LevelLabel, Hplabel, Mplabel, ExLabel;
        private DateTime NextAvatarChange;
        private HeroAutoPotPreview HPItem, MPItem;

        private string Name => GameScene.Hero.Name;
        private int Level => GameScene.Hero.Level;
        private MirClass Class => GameScene.Hero.Class;
        private MirGender Gender => GameScene.Hero.Gender;
        private long Experience => GameScene.Hero.Experience;
        private long MaxExperience => GameScene.Hero.MaxExperience;
        private byte PercentHealth => GameScene.Hero.PercentHealth;
        private byte PercentMana => GameScene.Hero.PercentMana;
        private bool Dead => GameScene.Scene.HeroSpawnState == HeroSpawnState.Dead;

        public HeroInfoPanel()
        {
            Index = 14;
            Library = Libraries.Prguse;
            Location = new Point(95, 48);

            Avatar = new MirImageControl
            {
                Index = 1400,
                Library = Libraries.Prguse,
                Location = new Point(14, 19),
                Parent = this,
                Visible = true
            };
            Avatar.BeforeDraw += Avatar_BeforeDraw;

            DangerAvatar = new MirImageControl
            {
                Index = 1750,
                Library = Libraries.Prguse,
                Location = new Point(14, 19),
                Parent = this,
                Visible = false
            };

            DeadAvatar = new MirImageControl
            {
                Index = 1379,
                Library = Libraries.Prguse,
                Location = new Point(14, 19),
                Parent = this,
                Visible = false
            };

            NameContainer = new MirImageControl
            {
                Index = 10,
                Library = Libraries.Prguse,
                Location = new Point(26, 60),
                Parent = this,
                Visible = true
            };

            LevelLabel = new MirLabel
            {
                AutoSize = false,
                Size = new Size(17, 14),
                Location = new Point(3, -1),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Parent = NameContainer,
            };

            NameLabel = new MirLabel
            {
                AutoSize = false,
                Size = new Size(97, 14),
                Location = new Point(2, 14),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Parent = NameContainer,
            };

            HealthContainer = new MirImageControl
            {
                Index = 11,
                Library = Libraries.Prguse,
                Location = new Point(57, 26),
                Parent = this,
                Visible = true
            };

            HealthBar = new MirImageControl
            {
                Index = 1951,
                Library = Libraries.Prguse,
                Location = new Point(18, 6),
                Parent = HealthContainer,
                Visible = true,
                DrawImage = false,
                NotControl = true
            };
            HealthBar.BeforeDraw += HealthBar_BeforeDraw;
            Hplabel = new MirLabel
            {
                AutoSize = false,
                Size = new Size(55, 18),
                Location = new Point(71, 28),
                DrawFormat = TextFormatFlags.Default,
                Parent = this,

            };
            ManaBar = new MirImageControl
            {
                Index = 1952,
                Library = Libraries.Prguse,
                Location = new Point(18, 19),
                Parent = HealthContainer,
                Visible = true,
                DrawImage = false,
                NotControl = true
            };
            ManaBar.BeforeDraw += ManaBar_BeforeDraw;
            Mplabel = new MirLabel
            {
                AutoSize = false,
                Size = new Size(55, 18),
                Location = new Point(71, 41),
                DrawFormat = TextFormatFlags.Default,
                Parent = this,
            }; 
            ExperienceBar = new MirImageControl
            {
                Index = 1953,
                Library = Libraries.Prguse,
                Location = new Point(18, 32),
                Parent = HealthContainer,
                Visible = true,
                DrawImage = false,
                NotControl = true
            };
            ExperienceBar.BeforeDraw += ExperienceBar_BeforeDraw;
            ExLabel = new MirLabel
            {
                AutoSize = false,
                Size = new Size(65, 18),
                Location = new Point(71, 54),
                DrawFormat = TextFormatFlags.Default,
                Parent = this,
            };
            HPItem = new HeroAutoPotPreview()
            {
                Parent = this,
                Location = new Point(86, 5),
                GetInfo = () => { return GameScene.Hero?.HPItem[0]?.Info; }
            };

            MPItem = new HeroAutoPotPreview()
            {
                Parent = this,
                Location = new Point(106, 5),
                GetInfo = () => { return GameScene.Hero?.MPItem[0]?.Info; }
            };
        }

        public void Update()
        {
            Avatar.Index = GameScene.Scene.HeroAvatar(Class, Gender);
            DangerAvatar.Index = Avatar.Index + 350;
            NameLabel.Text = Name;
            LevelLabel.Text = Level.ToString();
        }

        private void Avatar_BeforeDraw(object sender, EventArgs e)
        {
            DeadAvatar.Visible = Dead;
            if (PercentHealth > 20)
            {
                DangerAvatar.Visible = false;
                return;
            }
            if (CMain.Now < NextAvatarChange) return;

            NextAvatarChange = CMain.Now.AddMilliseconds(400);
            DangerAvatar.Visible = !DangerAvatar.Visible;
        }

        private void ExperienceBar_BeforeDraw(object sender, EventArgs e)
        {
            if (ExperienceBar.Library == null) return;

            //cast MaxExperience to double to force division to 2 decimal place
            double percent = Experience / (double)MaxExperience * 100;

            var test = (int)ExperienceBar.Size.Width * percent;

            Rectangle section = new()
            {
                Size = new Size((int)(ExperienceBar.Size.Width * percent), ExperienceBar.Size.Height)
            };

            ExperienceBar.Library.Draw(ExperienceBar.Index, section, ExperienceBar.DisplayLocation, Color.White, false);
            ExLabel.Text = string.Format("{0:F2}%", percent);
        }

        private void HealthBar_BeforeDraw(object sender, EventArgs e)
        {
            if (HealthBar.Library == null) return;

            double percent = PercentHealth / 100F;
            if (percent > 1) percent = 1;
            if (percent <= 0) return;

            Rectangle section = new Rectangle
            {
                Size = new Size((int)(HealthBar.Size.Width * percent), HealthBar.Size.Height)
            };

            HealthBar.Library.Draw(HealthBar.Index, section, HealthBar.DisplayLocation, Color.White, false);
            Hplabel.Text = GameScene.Hero?.HP.ToString() + "/" + GameScene.Hero?.Stats[Stat.HP].ToString();
        }

        private void ManaBar_BeforeDraw(object sender, EventArgs e)
        {
            if (ManaBar.Library == null) return;

            double percent = PercentMana / 100F;
            if (percent > 1) percent = 1;
            if (percent <= 0) return;

            Rectangle section = new Rectangle
            {
                Size = new Size((int)(ManaBar.Size.Width * percent), ManaBar.Size.Height)
            };

            ManaBar.Library.Draw(ManaBar.Index, section, ManaBar.DisplayLocation, Color.White, false);
            Mplabel.Text = GameScene.Hero?.MP.ToString() + "/" + GameScene.Hero?.Stats[Stat.MP].ToString();
        }
    }
    public sealed class HeroAutoPotPreview : MirImageControl
    {
        public Func<ItemInfo> GetInfo;
        private ItemInfo Info => GetInfo();
        public MirLabel AmountLabel;

        public HeroAutoPotPreview()
        {
            Index = 1392;
            Library = Libraries.Prguse;
            NotControl = true;
            AutoSize = false;
            Size = new Size(19, 15);

            AmountLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(0, 6),
                AutoSize = false,
                Size = new Size(22, 17),
                DrawFormat = TextFormatFlags.Right,
                ForeColour = Color.Yellow,
                Visible = false
            };

            AfterDraw += AutoPotPreview_AfterDraw;
        }

        void AutoPotPreview_AfterDraw(object sender, EventArgs e)
        {
            AmountLabel.Visible = Info != null;
            if (Info == null) return;
            AmountLabel.Text = CountItem(Info).ToString();
            Libraries.Items.Draw(Info.Image, DisplayLocation.Add(2, 3), Size, Color.White);
            AmountLabel.Draw();
        }

        int CountItem(ItemInfo info)
        {
            var count = 0;
            if (GameScene.Hero == null) return count;

            for (int i = 0; i < GameScene.Hero.Inventory.Length; i++)
            {
                UserItem item = GameScene.Hero.Inventory[i];
                if (item == null) continue;
                if (item.Info.Index != info.Index) continue;

                count += item.Count;
            }
            return count;
        }
    }
    public sealed class HeroBehaviourPanel : MirImageControl
    {
        private MirButton[] BehaviourButtons;
        public HeroBehaviourPanel()
        {
            AutoSize = false;
            Size = new Size(64, 17);
            DrawImage = false;
            Location = new Point(GameScene.Scene.MainDialog.Location.X + 165, GameScene.Scene.MainDialog.Location.Y + 37);

            BehaviourButtons = new MirButton[4];

            for (int i = 0; i < BehaviourButtons.Length; i++)
            {
                HeroBehaviour hb = (HeroBehaviour)i;
                BehaviourButtons[i] = new MirButton
                {
                    Index = 1840 + i,
                    DisabledIndex = 1844 + i,
                    Location = new Point(16 * i, 0),
                    Library = Libraries.Prguse,
                    Parent = this,
                    Sound = SoundList.ButtonA,
                    Hint = $"Hero Behaviour: {Enum.GetName(typeof(HeroBehaviour), i)}",
                    AllowDisabledMouseOver = true
                };
                BehaviourButtons[i].Click += (o, e) =>
                {                    
                    SetBehaviour(hb);
                };
            }
        }

        private void SetBehaviour(HeroBehaviour behaviour)
        {
            Network.Enqueue(new C.SetHeroBehaviour { Behaviour = behaviour });
        }
        public void UpdateBehaviour(HeroBehaviour behaviour)
        {
            for (int i = 0; i < BehaviourButtons.Length; i++)
                BehaviourButtons[i].Enabled = (byte)behaviour != i;
        }
    }
    public sealed class HeroManageDialog : MirImageControl
    {
        public HeroManageAvatar CurrentAvatar;
        public HeroManageAvatar[] Avatars = new HeroManageAvatar[8];
        public MirButton CloseButton;

        public HeroManageDialog()
        {
            Index = 1688;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Location = new Point(350, 350);

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(Size.Width - 24, 4),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            CurrentAvatar = new HeroManageAvatar() { Parent = this };

            for (int i = 0; i < Avatars.Length; i++)
            {
                int index = i;
                Avatars[i] = new HeroManageAvatar() { Parent = this };
                Avatars[i].Click += (o, e) =>
                {
                    MirMessageBox messageBox = new MirMessageBox($"Would you like to make {Avatars[index].Info.Name} your active Hero?", MirMessageBoxButtons.YesNo);
                    messageBox.YesButton.Click += (o, e) => Network.Enqueue(new C.ChangeHero { ListIndex = index + 1 });
                    messageBox.Show();
                };
            }
        }

        public void RefreshInterface()
        {
            for (int i = 0; i < Avatars.Length; i++)
            {
                Avatars[i].Location = new Point(98 + 60 * (i % 4), 61 + 40 * (int)(i / 4));
                if (i > GameScene.MaximumHeroCount - 2)
                {
                    Avatars[i].Info = null;
                    Avatars[i].Visible = i > GameScene.MaximumHeroCount - 2;
                    Avatars[i].NotControl = true;
                    continue;
                }

                Avatars[i].Info = GameScene.HeroStorage[i];
                Avatars[i].Visible = GameScene.HeroStorage[i] != null;
                Avatars[i].NotControl = false;
            }
        }

        public void SetCurrentHero(ClientHeroInformation hero)
        {
            CurrentAvatar.Location = new Point(15, 61);
            CurrentAvatar.Info = hero;
        }

        public override void Show()
        {
            RefreshInterface();
            base.Show();
        }
    }
    public sealed class HeroManageAvatar : MirImageControl
    {
        const int DefaultIndex = 1689;
        private ClientHeroInformation info;
        public ClientHeroInformation Info
        {
            get { return info; }
            set
            {
                info = value;
                if (info == null)
                {
                    Visible = false;
                }
                else
                {
                    Location = new Point(Location.X + 5, Location.Y + 5);
                    Index = GameScene.Scene.HeroAvatar(info.Class, info.Gender) + 370;
                    Hint = info.ToString();
                    Visible = true;
                }
            }
        }
        public HeroManageAvatar()
        {
            Index = DefaultIndex;
            Library = Libraries.Prguse;            
        }
    }
}

