using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using SlimDX;
using SlimDX.Direct3D9;
using Font = System.Drawing.Font;
using S = ServerPackets;
using C = ClientPackets;
using Effect = Client.MirObjects.Effect;

using Client.MirScenes.Dialogs;
using System.Drawing.Imaging;

namespace Client.MirScenes.Dialogs
{
    public sealed class MainDialog : MirImageControl
    {
        public static UserObject User
        {
            get { return MapObject.User; }
            set { MapObject.User = value; }
        }

        public MirImageControl ExperienceBar, WeightBar, LeftCap, RightCap;
        public MirButton GameShopButton, MenuButton, InventoryButton, CharacterButton, SkillButton, QuestButton, OptionButton;
        public MirControl HealthOrb;
        public MirLabel HealthLabel, ManaLabel, TopLabel, BottomLabel, LevelLabel, CharacterName, ExperienceLabel, GoldLabel, WeightLabel, SpaceLabel, AModeLabel, PModeLabel, SModeLabel;

        public MirButton CustomButton1, CustomButton2;

        public bool HPOnly
        {
            get { return User != null && User.Class == MirClass.Warrior && User.Level < 26; }
        }

        public MainDialog()
        {
            Index = Settings.Resolution == 800 ? 0 : Settings.Resolution == 1024 ? 1 : 2;
            Library = Libraries.Prguse;
            Location = new Point(((Settings.ScreenWidth / 2) - (Size.Width / 2)), Settings.ScreenHeight - Size.Height);
            PixelDetect = true;

            LeftCap = new MirImageControl
            {
                Index = 12,
                Library = Libraries.Prguse,
                Location = new Point(-67, this.Size.Height - 96),
                Parent = this,
                Visible = false
            };
            RightCap = new MirImageControl
            {
                Index = 13,
                Library = Libraries.Prguse,
                Location = new Point(1024, this.Size.Height - 104),
                Parent = this,
                Visible = false
            };

            if (Settings.Resolution > 1024)
            {
                LeftCap.Visible = true;
                RightCap.Visible = true;
            }

            InventoryButton = new MirButton
            {
                HoverIndex = 1904,
                Index = 1903,
                Library = Libraries.Prguse,
                Location = new Point(this.Size.Width - 96, 76),
                Parent = this,
                PressedIndex = 1905,
                Sound = SoundList.ButtonA,
                Hint = string.Format(GameLanguage.Inventory, CMain.InputKeys.GetKey(KeybindOptions.Inventory))
            };
            InventoryButton.Click += (o, e) =>
            {
                if (GameScene.Scene.InventoryDialog.Visible)
                    GameScene.Scene.InventoryDialog.Hide();
                else
                    GameScene.Scene.InventoryDialog.Show();
            };

            CharacterButton = new MirButton
            {
                HoverIndex = 1901,
                Index = 1900,
                Library = Libraries.Prguse,
                Location = new Point(this.Size.Width - 119, 76),
                Parent = this,
                PressedIndex = 1902,
                Sound = SoundList.ButtonA,
                Hint = string.Format(GameLanguage.Character, CMain.InputKeys.GetKey(KeybindOptions.Equipment))
            };
            CharacterButton.Click += (o, e) =>
            {
                if (GameScene.Scene.CharacterDialog.Visible && GameScene.Scene.CharacterDialog.CharacterPage.Visible)
                    GameScene.Scene.CharacterDialog.Hide();
                else
                {
                    GameScene.Scene.CharacterDialog.Show();
                    GameScene.Scene.CharacterDialog.ShowCharacterPage();
                }
            };

            SkillButton = new MirButton
            {
                HoverIndex = 1907,
                Index = 1906,
                Library = Libraries.Prguse,
                Location = new Point(this.Size.Width - 73, 76),
                Parent = this,
                PressedIndex = 1908,
                Sound = SoundList.ButtonA,
                Hint = string.Format(GameLanguage.Skills, CMain.InputKeys.GetKey(KeybindOptions.Skills))
            };
            SkillButton.Click += (o, e) =>
            {
                if (GameScene.Scene.CharacterDialog.Visible && GameScene.Scene.CharacterDialog.SkillPage.Visible)
                    GameScene.Scene.CharacterDialog.Hide();
                else
                {
                    GameScene.Scene.CharacterDialog.Show();
                    GameScene.Scene.CharacterDialog.ShowSkillPage();
                }
            };

            QuestButton = new MirButton
            {
                HoverIndex = 1910,
                Index = 1909,
                Library = Libraries.Prguse,
                Location = new Point(this.Size.Width - 50, 76),
                Parent = this,
                PressedIndex = 1911,
                Sound = SoundList.ButtonA,
                Hint = string.Format(GameLanguage.Quests, CMain.InputKeys.GetKey(KeybindOptions.Quests))
            };
            QuestButton.Click += (o, e) =>
            {
                if (!GameScene.Scene.QuestLogDialog.Visible)
                    GameScene.Scene.QuestLogDialog.Show();
                else GameScene.Scene.QuestLogDialog.Hide();
            };

            OptionButton = new MirButton
            {
                HoverIndex = 1913,
                Index = 1912,
                Library = Libraries.Prguse,
                Location = new Point(this.Size.Width - 27, 76),
                Parent = this,
                PressedIndex = 1914,
                Sound = SoundList.ButtonA,
                Hint = string.Format(GameLanguage.Options, CMain.InputKeys.GetKey(KeybindOptions.Options))
            };
            OptionButton.Click += (o, e) =>
            {
                if (!GameScene.Scene.OptionDialog.Visible)
                    GameScene.Scene.OptionDialog.Show();
                else GameScene.Scene.OptionDialog.Hide();
            };

            MenuButton = new MirButton
            {
                HoverIndex = 1961,
                Index = 1960,
                Library = Libraries.Prguse,
                Location = new Point(this.Size.Width - 55, 35),
                Parent = this,
                PressedIndex = 1962,
                Sound = SoundList.ButtonC,
                Hint = GameLanguage.Menu
            };
            MenuButton.Click += (o, e) =>
            {
                if (!GameScene.Scene.MenuDialog.Visible) GameScene.Scene.MenuDialog.Show();
                else GameScene.Scene.MenuDialog.Hide();
            };

            GameShopButton = new MirButton
            {
                HoverIndex = 827,
                Index = 826,
                Library = Libraries.Prguse,
                Location = new Point(this.Size.Width - 105, 35),
                Parent = this,
                PressedIndex = 828,
                Sound = SoundList.ButtonC,
                Hint = string.Format(GameLanguage.GameShop, CMain.InputKeys.GetKey(KeybindOptions.GameShop))
            };
            GameShopButton.Click += (o, e) =>
            {
                if (!GameScene.Scene.GameShopDialog.Visible) GameScene.Scene.GameShopDialog.Show();
                else GameScene.Scene.GameShopDialog.Hide();
            };

            HealthOrb = new MirControl
            {
                Parent = this,
                Location = new Point(0, 30),
                NotControl = true,
            };

            HealthOrb.BeforeDraw += HealthOrb_BeforeDraw;

            HealthLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(0, 27),
                Parent = HealthOrb
            };
            HealthLabel.SizeChanged += Label_SizeChanged;

            ManaLabel = new MirLabel
            {
                AutoSize = true,
                Location = new Point(0, 42),
                Parent = HealthOrb
            };
            ManaLabel.SizeChanged += Label_SizeChanged;

            TopLabel = new MirLabel
            {
                Size = new Size(85, 30),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Location = new Point(9, 20),
                Parent = HealthOrb,
            };

            BottomLabel = new MirLabel
            {
                Size = new Size(85, 30),
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Location = new Point(9, 50),
                Parent = HealthOrb,
            };

            LevelLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(5, 108)
            };

            CharacterName = new MirLabel
            {
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                Location = new Point(6, 120),
                Size = new Size(90, 16)
            };


            ExperienceBar = new MirImageControl
            {
                Index = Settings.Resolution != 800 ? 8 : 7,
                Library = Libraries.Prguse,
                Location = new Point(9, 143),
                Parent = this,
                DrawImage = false,
                NotControl = true,
            };
            ExperienceBar.BeforeDraw += ExperienceBar_BeforeDraw;

            ExperienceLabel = new MirLabel
            {
                AutoSize = true,
                Parent = ExperienceBar,
                NotControl = true,
            };

            GoldLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.VerticalCenter,
                Font = new Font(Settings.FontName, 8F),
                Location = new Point(this.Size.Width - 105, 119),
                Parent = this,
                Size = new Size(99, 13),
                Sound = SoundList.Gold,
            };
            GoldLabel.Click += (o, e) =>
            {
                if (GameScene.SelectedCell == null)
                    GameScene.PickedUpGold = !GameScene.PickedUpGold && GameScene.Gold > 0;
            };



            WeightBar = new MirImageControl
            {
                Index = 76,
                Library = Libraries.Prguse,
                Location = new Point(this.Size.Width - 105, 103),
                Parent = this,
                DrawImage = false,
                NotControl = true,
            };
            WeightBar.BeforeDraw += WeightBar_BeforeDraw;

            WeightLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(this.Size.Width - 105, 101),
                Size = new Size(40, 14),
            };

            SpaceLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(this.Size.Width - 30, 101),
                Size = new Size(26, 14),
            };

            CustomButton1 = new MirButton
            {
                Index = 2164,
                HoverIndex = 2165,
                PressedIndex = 2166,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(this.Size.Width - 160, 65),
                Size = new Size(20, 20),
                Sound = SoundList.ButtonA,
                Visible = !Settings.ModeView
            };
            CustomButton1.Click += (o, e) =>
            {
                GameScene.Scene.CustomPanel1.Toggle();
            };

            CustomButton2 = new MirButton
            {
                Index = 2167,
                HoverIndex = 2168,
                PressedIndex = 2169,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(this.Size.Width - 160, 90),
                Size = new Size(20, 20),
                Sound = SoundList.ButtonA
            };
            CustomButton2.Click += (o, e) =>
            {
                Network.Enqueue(new C.CallNPC { ObjectID = uint.MaxValue });
            };

            AModeLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.Yellow,
                OutLineColour = Color.Black,
                Parent = this,
                Location = new Point(Settings.Resolution != 800 ? 899 : 675, Settings.Resolution != 800 ? -448 : -280),
                Visible = Settings.ModeView
            };

            PModeLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.Orange,
                OutLineColour = Color.Black,
                Parent = this,
                Location = new Point(230, 125),
                Visible = Settings.ModeView
            };

            SModeLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.LimeGreen,
                OutLineColour = Color.Black,
                Parent = this,
                Location = new Point(Settings.Resolution != 800 ? 899 : 675, Settings.Resolution != 800 ? -463 : -295),
                Visible = Settings.ModeView
            };
        }

        public void Process()
        {
            switch (GameScene.Scene.AMode)
            {
                case AttackMode.Peace:
                    AModeLabel.Text = GameLanguage.AttackMode_Peace;
                    break;
                case AttackMode.Group:
                    AModeLabel.Text = GameLanguage.AttackMode_Group;
                    break;
                case AttackMode.Guild:
                    AModeLabel.Text = GameLanguage.AttackMode_Guild;
                    break;
                case AttackMode.EnemyGuild:
                    AModeLabel.Text = GameLanguage.AttackMode_EnemyGuild;
                    break;
                case AttackMode.RedBrown:
                    AModeLabel.Text = GameLanguage.AttackMode_RedBrown;
                    break;
                case AttackMode.All:
                    AModeLabel.Text = GameLanguage.AttackMode_All;
                    break;
            }

            switch (GameScene.Scene.PMode)
            {
                case PetMode.Both:
                    PModeLabel.Text = GameLanguage.PetMode_Both;
                    break;
                case PetMode.MoveOnly:
                    PModeLabel.Text = GameLanguage.PetMode_MoveOnly;
                    break;
                case PetMode.AttackOnly:
                    PModeLabel.Text = GameLanguage.PetMode_AttackOnly;
                    break;
                case PetMode.None:
                    PModeLabel.Text = GameLanguage.PetMode_None;
                    break;
            }

            switch (Settings.SkillMode)
            {
                case true:
                    SModeLabel.Text = "[Skill Mode: ~]";
                    break;
                case false:
                    SModeLabel.Text = "[Skill Mode: Ctrl]";
                    break;
            }

            if (Settings.HPView)
            {
                HealthLabel.Text = string.Format("HP {0}/{1}", User.HP, User.Stats[Stat.HP]);
                ManaLabel.Text = HPOnly ? "" : string.Format("MP {0}/{1} ", User.MP, User.Stats[Stat.MP]);
                TopLabel.Text = string.Empty;
                BottomLabel.Text = string.Empty;
            }
            else
            {
                if (HPOnly)
                {
                    TopLabel.Text = string.Format("{0}\n" + "--", User.HP);
                    BottomLabel.Text = string.Format("{0}", User.Stats[Stat.HP]);
                }
                else
                {
                    TopLabel.Text = string.Format(" {0}    {1} \n" + "---------------", User.HP, User.MP);
                    BottomLabel.Text = string.Format(" {0}    {1} ", User.Stats[Stat.HP], User.Stats[Stat.MP]);
                }
                HealthLabel.Text = string.Empty;
                ManaLabel.Text = string.Empty;
            }

            LevelLabel.Text = User.Level.ToString();
            ExperienceLabel.Text = string.Format("{0:#0.##%}", User.Experience / (double)User.MaxExperience);
            ExperienceLabel.Location = new Point((ExperienceBar.Size.Width / 2) - 20, -10);
            GoldLabel.Text = GameScene.Gold.ToString("###,###,##0");
            CharacterName.Text = User.Name;
            SpaceLabel.Text = User.Inventory.Count(t => t == null).ToString();
            WeightLabel.Text = (MapObject.User.Stats[Stat.BagWeight] - MapObject.User.CurrentBagWeight).ToString();
        }

        private void Label_SizeChanged(object sender, EventArgs e)
        {
            if (!(sender is MirLabel l)) return;

            l.Location = new Point(50 - (l.Size.Width / 2), l.Location.Y);
        }

        private void HealthOrb_BeforeDraw(object sender, EventArgs e)
        {
            if (Libraries.Prguse == null) return;

            int height;
            if (User != null && User.HP != User.Stats[Stat.HP])
                height = (int)(80 * User.HP / (float)User.Stats[Stat.HP]);
            else
                height = 80;

            if (height < 0) height = 0;
            if (height > 80) height = 80;

            int orbImage = 4;

            bool hpOnly = false;

            if (HPOnly)
            {
                hpOnly = true;
                orbImage = 6;
            }

            Rectangle r = new Rectangle(0, 80 - height, hpOnly ? 100 : 50, height);
            Libraries.Prguse.Draw(orbImage, r, new Point(((Settings.ScreenWidth / 2) - (Size.Width / 2)), HealthOrb.DisplayLocation.Y + 80 - height), Color.White, false);

            if (hpOnly) return;

            if (User.MP != User.Stats[Stat.MP])
                height = (int)(80 * User.MP / (float)User.Stats[Stat.MP]);
            else
                height = 80;

            if (height < 0) height = 0;
            if (height > 80) height = 80;
            r = new Rectangle(51, 80 - height, 50, height);

            Libraries.Prguse.Draw(4, r, new Point(((Settings.ScreenWidth / 2) - (Size.Width / 2)) + 51, HealthOrb.DisplayLocation.Y + 80 - height), Color.White, false);
        }

        private void ExperienceBar_BeforeDraw(object sender, EventArgs e)
        {
            if (ExperienceBar.Library == null) return;

            double percent = MapObject.User.Experience / (double)MapObject.User.MaxExperience;
            if (percent > 1) percent = 1;
            if (percent <= 0) return;

            Rectangle section = new Rectangle
            {
                Size = new Size((int)((ExperienceBar.Size.Width - 3) * percent), ExperienceBar.Size.Height)
            };

            ExperienceBar.Library.Draw(ExperienceBar.Index, section, ExperienceBar.DisplayLocation, Color.White, false);
        }

        private void WeightBar_BeforeDraw(object sender, EventArgs e)
        {
            if (WeightBar.Library == null) return;
            double percent = MapObject.User.CurrentBagWeight / (double)MapObject.User.Stats[Stat.BagWeight];
            if (percent > 1) percent = 1;
            if (percent <= 0) return;

            Rectangle section = new Rectangle
            {
                Size = new Size((int)((WeightBar.Size.Width - 2) * percent), WeightBar.Size.Height)
            };

            WeightBar.Library.Draw(WeightBar.Index, section, WeightBar.DisplayLocation, Color.White, false);
        }
    }
    public sealed class ChatDialog : MirImageControl
    {
        public List<ChatHistory> FullHistory = new List<ChatHistory>();
        public List<ChatHistory> History = new List<ChatHistory>();
        public List<MirLabel> ChatLines = new List<MirLabel>();

        public List<ChatItem> LinkedItems = new List<ChatItem>();
        public List<MirLabel> LinkedItemButtons = new List<MirLabel>();

        public MirButton HomeButton, UpButton, EndButton, DownButton, PositionBar;
        public MirImageControl CountBar;
        public MirTextBox ChatTextBox;
        public Font ChatFont = new Font(Settings.FontName, 8F);
        public string LastPM = string.Empty;

        public int StartIndex, LineCount = 4, WindowSize;
        public string ChatPrefix = "";

        public bool Transparent;

        public ChatDialog()
        {
            Index = Settings.Resolution != 800 ? 2221 : 2201;
            Library = Libraries.Prguse;
            Location = new Point(GameScene.Scene.MainDialog.Location.X + 230, Settings.ScreenHeight - 97);
            PixelDetect = true;

            KeyPress += ChatPanel_KeyPress;
            KeyDown += ChatPanel_KeyDown;
            MouseWheel += ChatPanel_MouseWheel;

            ChatTextBox = new MirTextBox
            {
                BackColour = Color.DarkGray,
                ForeColour = Color.Black,
                Parent = this,
                Size = new Size(Settings.Resolution != 800 ? 627 : 403, 13),
                Location = new Point(1, 54),
                MaxLength = Globals.MaxChatLength,
                Visible = false,
                Font = ChatFont,
            };
            ChatTextBox.TextBox.KeyPress += ChatTextBox_KeyPress;
            ChatTextBox.TextBox.KeyDown += ChatTextBox_KeyDown;
            ChatTextBox.TextBox.KeyUp += ChatTextBox_KeyUp;

            HomeButton = new MirButton
            {
                Index = 2018,
                HoverIndex = 2019,
                Library = Libraries.Prguse,
                Location = new Point(Settings.Resolution != 800 ? 618 : 394, 1),
                Parent = this,
                PressedIndex = 2020,
                Sound = SoundList.ButtonA,
            };
            HomeButton.Click += (o, e) =>
            {
                if (StartIndex == 0) return;
                StartIndex = 0;
                Update();
            };


            UpButton = new MirButton
            {
                Index = 2021,
                HoverIndex = 2022,
                Library = Libraries.Prguse,
                Location = new Point(Settings.Resolution != 800 ? 618 : 394, 9),
                Parent = this,
                PressedIndex = 2023,
                Sound = SoundList.ButtonA,
            };
            UpButton.Click += (o, e) =>
            {
                if (StartIndex == 0) return;
                StartIndex--;
                Update();
            };


            EndButton = new MirButton
            {
                Index = 2027,
                HoverIndex = 2028,
                Library = Libraries.Prguse,
                Location = new Point(Settings.Resolution != 800 ? 618 : 394, 45),
                Parent = this,
                PressedIndex = 2029,
                Sound = SoundList.ButtonA,
            };
            EndButton.Click += (o, e) =>
            {
                if (StartIndex == History.Count - 1) return;
                StartIndex = History.Count - 1;
                Update();
            };

            DownButton = new MirButton
            {
                Index = 2024,
                HoverIndex = 2025,
                Library = Libraries.Prguse,
                Location = new Point(Settings.Resolution != 800 ? 618 : 394, 39),
                Parent = this,
                PressedIndex = 2026,
                Sound = SoundList.ButtonA,
            };
            DownButton.Click += (o, e) =>
            {
                if (StartIndex == History.Count - 1) return;
                StartIndex++;
                Update();
            };



            CountBar = new MirImageControl
            {
                Index = 2012,
                Library = Libraries.Prguse,
                Location = new Point(Settings.Resolution != 800 ? 622 : 398, 16),
                Parent = this,
            };

            PositionBar = new MirButton
            {
                Index = 2015,
                HoverIndex = 2016,
                Library = Libraries.Prguse,
                Location = new Point(Settings.Resolution != 800 ? 619 : 395, 16),
                Parent = this,
                PressedIndex = 2017,
                Movable = true,
                Sound = SoundList.None,
            };
            PositionBar.OnMoving += PositionBar_OnMoving;
        }

        public void SetChatText(string newText)
        {
            string newMsg = ChatTextBox.Text += newText;

            if (newMsg.Length > Globals.MaxChatLength) return;

            ChatTextBox.Text = newMsg;
            ChatTextBox.SetFocus();
            ChatTextBox.Visible = true;
            ChatTextBox.TextBox.SelectionLength = 0;
            ChatTextBox.TextBox.SelectionStart = ChatTextBox.Text.Length;
        }

        private void ChatTextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)Keys.Enter:
                    e.Handled = true;
                    if (!string.IsNullOrEmpty(ChatTextBox.Text))
                    {
                        string msg = ChatTextBox.Text;

                        if (msg.ToUpper() == "@LEVELEFFECT")
                        {
                            Settings.LevelEffect = !Settings.LevelEffect;
                        }

                        if (msg.ToUpper() == "@TARGETDEAD")
                        {
                            Settings.TargetDead = !Settings.TargetDead;
                        }

                        Network.Enqueue(new C.Chat
                        {
                            Message = msg,
                            LinkedItems = new List<ChatItem>(LinkedItems)
                        });

                        if (ChatTextBox.Text[0] == '/')
                        {
                            string[] parts = ChatTextBox.Text.Split(' ');
                            if (parts.Length > 0)
                                LastPM = parts[0];
                        }
                    }
                    ChatTextBox.Visible = false;
                    ChatTextBox.Text = string.Empty;
                    LinkedItems.Clear();
                    break;
                case (char)Keys.Escape:
                    e.Handled = true;
                    ChatTextBox.Visible = false;
                    ChatTextBox.Text = string.Empty;
                    LinkedItems.Clear();
                    break;
            }
        }

        void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int x = Settings.Resolution != 800 ? 619 : 395;
            int y = PositionBar.Location.Y;
            if (y >= 16 + CountBar.Size.Height - PositionBar.Size.Height) y = 16 + CountBar.Size.Height - PositionBar.Size.Height;
            if (y < 16) y = 16;

            int h = CountBar.Size.Height - PositionBar.Size.Height;
            h = (int)((y - 16) / (h / (float)(History.Count - 1)));

            if (h != StartIndex)
            {
                StartIndex = h;
                Update();
            }

            PositionBar.Location = new Point(x, y);
        }

        public void ReceiveChat(string text, ChatType type)
        {
            Color foreColour, backColour;

            switch (type)
            {
                case ChatType.Hint:
                    backColour = Color.White;
                    foreColour = Color.DarkGreen;
                    break;
                case ChatType.Announcement:
                    backColour = Color.Blue;
                    foreColour = Color.White;
                    GameScene.Scene.ChatNoticeDialog.ShowNotice(RegexFunctions.CleanChatString(text));
                    break;
                case ChatType.LineMessage:
                    backColour = Color.Blue;
                    foreColour = Color.White;
                    break;
                case ChatType.Shout:
                    backColour = Color.Yellow;
                    foreColour = Color.Black;
                    break;
                case ChatType.Shout2:
                    backColour = Color.Green;
                    foreColour = Color.White;
                    break;
                case ChatType.Shout3:
                    backColour = Color.Purple;
                    foreColour = Color.White;
                    break;
                case ChatType.System:
                    backColour = Color.Red;
                    foreColour = Color.White;
                    break;
                case ChatType.System2:
                    backColour = Color.DarkRed;
                    foreColour = Color.White;
                    break;
                case ChatType.Group:
                    backColour = Color.White;
                    foreColour = Color.Brown;
                    break;
                case ChatType.WhisperOut:
                    foreColour = Color.CornflowerBlue;
                    backColour = Color.White;
                    break;
                case ChatType.WhisperIn:
                    foreColour = Color.DarkBlue;
                    backColour = Color.White;
                    break;
                case ChatType.Guild:
                    backColour = Color.White;
                    foreColour = Color.Green;
                    break;
                case ChatType.LevelUp:
                    backColour = Color.FromArgb(255, 225, 185, 250);
                    foreColour = Color.Blue;
                    break;
                case ChatType.RebornUp:
                    backColour = Color.Teal;
                    foreColour = Color.Blue;
                    break;
                case ChatType.InstanceStageUp:
                    backColour = Color.Teal;
                    foreColour = Color.Blue;
                    break;
                case ChatType.ChallengeStageUp:
                    backColour = Color.Teal;
                    foreColour = Color.Blue;
                    break;
                case ChatType.Relationship:
                    backColour = Color.Transparent;
                    foreColour = Color.HotPink;
                    break;
                case ChatType.Mentor:
                    backColour = Color.White;
                    foreColour = Color.Purple;
                    break;
                case ChatType.Socket:
                    backColour = Color.Yellow;
                    foreColour = Color.CadetBlue;
                    break;
                case ChatType.Orb:
                    backColour = Color.Yellow;
                    foreColour = Color.MidnightBlue;
                    break;
                default:
                    backColour = Color.White;
                    foreColour = Color.Black;
                    break;
            }

            List<string> chat = new List<string>();

            int chatWidth = Settings.Resolution != 800 ? 614 : 390;
            int index = 0;
            int matchCount = 0;

            for (int i = 1; i < text.Length; i++)
            {
                if (i - index < 0) continue;

                if (TextRenderer.MeasureText(CMain.Graphics, text.Substring(index, i - index), ChatFont).Width > chatWidth)
                {
                    int offset = i - index;
                    int newIndex = i - 1;

                    var itemLinkMatches = RegexFunctions.ChatItemLinks.Matches(text.Substring(index)).Cast<Match>();

                    if (itemLinkMatches.Any())
                    {
                        var match = itemLinkMatches.SingleOrDefault(x => (x.Index < (i - index)) && (x.Index + x.Length > offset - 1));

                        if (match != null)
                        {
                            offset = match.Index;
                            newIndex = match.Index;
                        }
                    }

                    chat.Add(text.Substring(index, offset - 1));
                    index = newIndex;
                }
            }

            chat.Add(text.Substring(index, text.Length - index));
            
            if (StartIndex == History.Count - LineCount)
                StartIndex += chat.Count;

            for (int i = 0; i < chat.Count; i++)
                FullHistory.Add(new ChatHistory { Text = chat[i], BackColour = backColour, ForeColour = foreColour, Type = type });

            Update();
        }

        public void Update()
        {
            History = new List<ChatHistory>();

            for (int i = 0; i < FullHistory.Count; i++)
            {
                switch (FullHistory[i].Type)
                {
                    case ChatType.Normal:
                    case ChatType.LineMessage:
                        if (Settings.FilterNormalChat) continue;
                        break;
                    case ChatType.WhisperIn:
                    case ChatType.WhisperOut:
                        if (Settings.FilterWhisperChat) continue;
                        break;
                    case ChatType.Shout:
                    case ChatType.Shout2:
                    case ChatType.Shout3:
                        if (Settings.FilterShoutChat) continue;
                        break;
                    case ChatType.System:
                    case ChatType.System2:
                        if (Settings.FilterSystemChat) continue;
                        break;
                    case ChatType.Group:
                        if (Settings.FilterGroupChat) continue;
                        break;
                    case ChatType.Guild:
                        if (Settings.FilterGuildChat) continue;
                        break;
                }

                History.Add(FullHistory[i]);
            }

            for (int i = 0; i < ChatLines.Count; i++)
                ChatLines[i].Dispose();

            for (int i = 0; i < LinkedItemButtons.Count; i++)
                LinkedItemButtons[i].Dispose();

            ChatLines.Clear();
            LinkedItemButtons.Clear();

            if (StartIndex >= History.Count) StartIndex = History.Count - 1;
            if (StartIndex < 0) StartIndex = 0;

            if (History.Count > 1)
            {
                int h = CountBar.Size.Height - PositionBar.Size.Height;
                h = (int)((h / (float)(History.Count - 1)) * StartIndex);
                PositionBar.Location = new Point(Settings.Resolution != 800 ? 619 : 395, 16 + h);
            }

            int y = 1;

            for (int i = StartIndex; i < History.Count; i++)
            {
                MirLabel temp = new MirLabel
                {
                    AutoSize = true,
                    BackColour = History[i].BackColour,
                    ForeColour = History[i].ForeColour,
                    Location = new Point(1, y),
                    OutLine = false,
                    Parent = this,
                    Text = History[i].Text,
                    Font = ChatFont,
                };
                temp.MouseWheel += ChatPanel_MouseWheel;
                ChatLines.Add(temp);

                temp.Click += (o, e) =>
                {
                    if (!(o is MirLabel l)) return;

                    string[] parts = l.Text.Split(':', ' ');
                    if (parts.Length == 0) return;

                    string name = Regex.Replace(parts[0], "[^A-Za-z0-9]", "");

                    ChatTextBox.SetFocus();
                    ChatTextBox.Text = string.Format("/{0} ", name);
                    ChatTextBox.Visible = true;
                    ChatTextBox.TextBox.SelectionLength = 0;
                    ChatTextBox.TextBox.SelectionStart = ChatTextBox.Text.Length;
                };

                string currentLine = History[i].Text;

                int oldLength = currentLine.Length;

                Capture capture = null;

                foreach (Match match in RegexFunctions.ChatItemLinks.Matches(currentLine).Cast<Match>().OrderBy(o => o.Index).ToList())
                {
                    try
                    {
                        int offSet = oldLength - currentLine.Length;

                        capture = match.Groups[1].Captures[0];
                        string[] values = capture.Value.Split('/');
                        currentLine = currentLine.Remove(capture.Index - 1 - offSet, capture.Length + 2).Insert(capture.Index - 1 - offSet, values[0]);
                        string text = currentLine.Substring(0, capture.Index - 1 - offSet) + " ";
                        Size size = TextRenderer.MeasureText(CMain.Graphics, text, temp.Font, temp.Size, TextFormatFlags.TextBoxControl);

                        ChatLink(values[0], ulong.Parse(values[1]), temp.Location.Add(new Point(size.Width - 10, 0)));
                    }
                    catch(Exception ex)
                    {
						//Temporary debug to catch unknown error
                        CMain.SaveError(ex.ToString());
                        CMain.SaveError(currentLine);
                        CMain.SaveError(capture.Value);
                        throw;
                    }
                }

                temp.Text = currentLine;

                y += 13;
                if (i - StartIndex == LineCount - 1) break;
            }

        }

        private void ChatLink(string name, ulong uniqueID, Point p)
        {
            UserItem item = GameScene.ChatItemList.FirstOrDefault(x => x.UniqueID == uniqueID);

            if (item != null)
            {
                MirLabel temp = new MirLabel
                {
                    AutoSize = true,
                    Visible = true,
                    Parent = this,
                    Location = p,
                    Text = name,
                    ForeColour = Color.Blue,
                    Sound = SoundList.ButtonC,
                    Font = ChatFont,
                    OutLine = false,
                };

                temp.MouseEnter += (o, e) => temp.ForeColour = Color.Red;
                temp.MouseLeave += (o, e) =>
                {
                    GameScene.Scene.DisposeItemLabel();
                    temp.ForeColour = Color.Blue;
                };
                temp.MouseDown += (o, e) => temp.ForeColour = Color.Blue;
                temp.MouseUp += (o, e) => temp.ForeColour = Color.Red;

                temp.Click += (o, e) =>
                {
                    GameScene.Scene.CreateItemLabel(item);
                };

                LinkedItemButtons.Add(temp);
            }
        }


        private void ChatPanel_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (StartIndex == 0) return;
                    StartIndex--;
                    break;
                case Keys.Home:
                    if (StartIndex == 0) return;
                    StartIndex = 0;
                    break;
                case Keys.Down:
                    if (StartIndex == History.Count - 1) return;
                    StartIndex++;
                    break;
                case Keys.End:
                    if (StartIndex == History.Count - 1) return;
                    StartIndex = History.Count - 1;
                    break;
                case Keys.PageUp:
                    if (StartIndex == 0) return;
                    StartIndex -= LineCount;
                    break;
                case Keys.PageDown:
                    if (StartIndex == History.Count - 1) return;
                    StartIndex += LineCount;
                    break;
                default:
                    return;
            }
            Update();
            e.Handled = true;
        }
        private void ChatPanel_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case '@':
                case '!':
                case ' ':
                case (char)Keys.Enter:
                    ChatTextBox.SetFocus();
                    if (e.KeyChar == '!') ChatTextBox.Text = "!";
                    if (e.KeyChar == '@') ChatTextBox.Text = "@";
                    if (ChatPrefix != "") ChatTextBox.Text = ChatPrefix;

                    ChatTextBox.Visible = true;
                    ChatTextBox.TextBox.SelectionLength = 0;
                    ChatTextBox.TextBox.SelectionStart = ChatTextBox.Text.Length;
                    e.Handled = true;
                    break;
                case '/':
                    ChatTextBox.SetFocus();
                    ChatTextBox.Text = LastPM + " ";
                    ChatTextBox.Visible = true;
                    ChatTextBox.TextBox.SelectionLength = 0;
                    ChatTextBox.TextBox.SelectionStart = ChatTextBox.Text.Length;
                    e.Handled = true;
                    break;
            }
        }
        private void ChatPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (StartIndex == 0 && count >= 0) return;
            if (StartIndex == History.Count - 1 && count <= 0) return;

            StartIndex -= count;
            Update();
        }
        private void ChatTextBox_KeyUp(object sender, KeyEventArgs e)
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
                    CMain.CMain_KeyUp(sender, e);
                    break;

            }
        }
        private void ChatTextBox_KeyDown(object sender, KeyEventArgs e)
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
                    CMain.CMain_KeyDown(sender, e);
                    break;

            }
        }


        public void ChangeSize()
        {
            if (++WindowSize >= 3) WindowSize = 0;

            int y = DisplayRectangle.Bottom;
            switch (WindowSize)
            {
                case 0:
                    LineCount = 4;
                    Index = Settings.Resolution != 800 ? 2221 : 2201;
                    CountBar.Index = 2012;
                    DownButton.Location = new Point(Settings.Resolution != 800 ? 618 : 394, 39);
                    EndButton.Location = new Point(Settings.Resolution != 800 ? 618 : 394, 45);
                    ChatTextBox.Location = new Point(1, 54);
                    break;
                case 1:
                    LineCount = 7;
                    Index = Settings.Resolution != 800 ? 2224 : 2204;
                    CountBar.Index = 2013;
                    DownButton.Location = new Point(Settings.Resolution != 800 ? 618 : 394, 39 + 48);
                    EndButton.Location = new Point(Settings.Resolution != 800 ? 618 : 394, 45 + 48);
                    ChatTextBox.Location = new Point(1, 54 + 48);
                    break;
                case 2:
                    LineCount = 11;
                    Index = Settings.Resolution != 800 ? 2227 : 2207;
                    CountBar.Index = 2014;
                    DownButton.Location = new Point(Settings.Resolution != 800 ? 618 : 394, 39 + 96);
                    EndButton.Location = new Point(Settings.Resolution != 800 ? 618 : 394, 45 + 96);
                    ChatTextBox.Location = new Point(1, 54 + 96);
                    break;
            }

            Location = new Point(Location.X, y - Size.Height);

            UpdateBackground();

            Update();
        }

        public void UpdateBackground()
        {
            int offset = Transparent ? 1 : 0;

            switch (WindowSize)
            {
                case 0:
                    Index = Settings.Resolution != 800 ? 2221 : 2201;
                    break;
                case 1:
                    Index = Settings.Resolution != 800 ? 2224 : 2204;
                    break;
                case 2:
                    Index = Settings.Resolution != 800 ? 2227 : 2207;
                    break;
            }

            Index -= offset;
        }

        public class ChatHistory
        {
            public string Text;
            public Color ForeColour, BackColour;
            public ChatType Type;
        }
    }
    public sealed class ChatControlBar : MirImageControl
    {
        public MirButton SizeButton, SettingsButton, NormalButton, ShoutButton, WhisperButton, LoverButton, MentorButton, GroupButton, GuildButton, ReportButton, TradeButton;

        public MirButton WebButton;
        public ChatControlBar()
        {
            Index = Settings.Resolution != 800 ? 2034 : 2035;
            Library = Libraries.Prguse;
            Location = new Point(GameScene.Scene.MainDialog.Location.X + 230, Settings.ScreenHeight - 112);

            SizeButton = new MirButton
            {
                Index = 2057,
                HoverIndex = 2058,
                PressedIndex = 2059,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(Settings.Resolution != 800 ? 574 : 350, 1),
                Visible = true,
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.Size
            };
            SizeButton.Click += (o, e) =>
            {
                GameScene.Scene.ChatDialog.ChangeSize();
                Location = new Point(Location.X, GameScene.Scene.ChatDialog.DisplayRectangle.Top - Size.Height);
                if (GameScene.Scene.BeltDialog.Index == 1932)
                    GameScene.Scene.BeltDialog.Location = new Point(GameScene.Scene.MainDialog.Location.X + 230, Location.Y - GameScene.Scene.BeltDialog.Size.Height);
            };

            SettingsButton = new MirButton
            {
                Index = 2060,
                HoverIndex = 2061,
                PressedIndex = 2062,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(Settings.Resolution != 800 ? 596 : 372, 1),
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.ChatSettings
            };
            SettingsButton.Click += (o, e) =>
            {
                if (GameScene.Scene.ChatOptionDialog.Visible)
                    GameScene.Scene.ChatOptionDialog.Hide();
                else
                    GameScene.Scene.ChatOptionDialog.Show();

                //GameScene.Scene.ChatDialog.Transparent = !GameScene.Scene.ChatDialog.Transparent;
                //GameScene.Scene.ChatDialog.UpdateBackground();
            };

            NormalButton = new MirButton
            {
                Index = 2036,
                HoverIndex = 2037,
                PressedIndex = 2038,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(12, 1),
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.Chat_All
            };
            NormalButton.Click += (o, e) =>
            {
                ToggleChatFilter("All");
            };

            ShoutButton = new MirButton
            {
                Index = 2039,
                HoverIndex = 2040,
                PressedIndex = 2041,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(34, 1),
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.Chat_Short
            };
            ShoutButton.Click += (o, e) =>
            {
                ToggleChatFilter("Shout");
            };

            WhisperButton = new MirButton
            {
                Index = 2042,
                HoverIndex = 2043,
                PressedIndex = 2044,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(56, 1),
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.Chat_Whisper
            };
            WhisperButton.Click += (o, e) =>
            {
                ToggleChatFilter("Whisper");
            };

            LoverButton = new MirButton
            {
                Index = 2045,
                HoverIndex = 2046,
                PressedIndex = 2047,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(78, 1),
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.Chat_Lover
            };
            LoverButton.Click += (o, e) =>
            {
                ToggleChatFilter("Lover");
            };

            MentorButton = new MirButton
            {
                Index = 2048,
                HoverIndex = 2049,
                PressedIndex = 2050,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(100, 1),
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.Chat_Mentor
            };
            MentorButton.Click += (o, e) =>
            {
                ToggleChatFilter("Mentor");
            };

            GroupButton = new MirButton
            {
                Index = 2051,
                HoverIndex = 2052,
                PressedIndex = 2053,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(122, 1),
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.Chat_Group
            };
            GroupButton.Click += (o, e) =>
            {
                ToggleChatFilter("Group");
            };

            GuildButton = new MirButton
            {
                Index = 2054,
                HoverIndex = 2055,
                PressedIndex = 2056,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(144, 1),
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.Chat_Guild
            };
            GuildButton.Click += (o, e) =>
            {
                Settings.ShowGuildChat = !Settings.ShowGuildChat;
                ToggleChatFilter("Guild");
            };

            WebButton = new MirButton
            {
                Index = 2070,
                PressedIndex = 2071,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(166, 1),
                Sound = SoundList.ButtonA,
                Hint = "Web Link"
            };
            WebButton.Click += (o, e) =>
            {
                System.Diagnostics.Process.Start(Settings.C_WebLink);
            };

            TradeButton = new MirButton
            {
                Index = 2004,
                HoverIndex = 2005,
                PressedIndex = 2006,
                Library = Libraries.Prguse,
                Location = new Point(166, 1),
                Parent = this,
                Sound = SoundList.ButtonC,
                Hint = string.Format(GameLanguage.Trade, CMain.InputKeys.GetKey(KeybindOptions.Trade)),
            };
            TradeButton.Click += (o, e) => Network.Enqueue(new C.TradeRequest());

            ReportButton = new MirButton
            {
                Index = 2063,
                HoverIndex = 2064,
                PressedIndex = 2065,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(Settings.Resolution != 800 ? 552 : 328, 1),
                Sound = SoundList.ButtonA,
                Hint = "Report",
                Visible = false
            };
            ReportButton.Click += (o, e) =>
            {
                GameScene.Scene.ReportDialog.Visible = !GameScene.Scene.ReportDialog.Visible;
            };

            ToggleChatFilter("All");
        }

        public void ToggleChatFilter(string chatFilter)
        {
            NormalButton.Index = 2036;
            NormalButton.HoverIndex = 2037;
            ShoutButton.Index = 2039;
            ShoutButton.HoverIndex = 2040;
            WhisperButton.Index = 2042;
            WhisperButton.HoverIndex = 2043;
            LoverButton.Index = 2045;
            LoverButton.HoverIndex = 2046;
            MentorButton.Index = 2048;
            MentorButton.HoverIndex = 2049;
            GroupButton.Index = 2051;
            GroupButton.HoverIndex = 2052;
            GuildButton.Index = 2054;
            GuildButton.HoverIndex = 2055;

            GameScene.Scene.ChatDialog.ChatPrefix = "";

            switch (chatFilter)
            {
                case "All":
                    NormalButton.Index = 2038;
                    NormalButton.HoverIndex = 2038;
                    GameScene.Scene.ChatDialog.ChatPrefix = "";
                    break;
                case "Shout":
                    ShoutButton.Index = 2041;
                    ShoutButton.HoverIndex = 2041;
                    GameScene.Scene.ChatDialog.ChatPrefix = "!";
                    break;
                case "Whisper":
                    WhisperButton.Index = 2044;
                    WhisperButton.HoverIndex = 2044;
                    GameScene.Scene.ChatDialog.ChatPrefix = "/";
                    break;
                case "Group":
                    GroupButton.Index = 2053;
                    GroupButton.HoverIndex = 2053;
                    GameScene.Scene.ChatDialog.ChatPrefix = "!!";
                    break;
                case "Guild":
                    GuildButton.Index = 2056;
                    GuildButton.HoverIndex = 2056;
                    GameScene.Scene.ChatDialog.ChatPrefix = "!~";
                    break;
                case "Lover":
                    LoverButton.Index = 2047;
                    LoverButton.HoverIndex = 2047;
                    GameScene.Scene.ChatDialog.ChatPrefix = ":)";
                    break;
                case "Mentor":
                    MentorButton.Index = 2050;
                    MentorButton.HoverIndex = 2050;
                    GameScene.Scene.ChatDialog.ChatPrefix = "!#";
                    break;
            }
        }
    }
    public sealed class InventoryDialog : MirImageControl
    {
        public MirImageControl WeightBar;
        public MirImageControl[] LockBar = new MirImageControl[10];
        public MirItemCell[] Grid;
        public MirItemCell[] QuestGrid;

        public MirButton CloseButton, ItemButton, ItemButton2, QuestButton, AddButton;
        public MirLabel GoldLabel, WeightLabel;

        public InventoryDialog()
        {
            Index = 196;
            Library = Libraries.Title;
            Movable = true;
            Sort = true;
            Visible = false;

            WeightBar = new MirImageControl
            {
                Index = 24,
                Library = Libraries.Prguse,
                Location = new Point(182, 217),
                Parent = this,
                DrawImage = false,
                NotControl = true,
            };

            ItemButton = new MirButton
            {
                Index = 197,
                Library = Libraries.Title,
                Location = new Point(6, 7),
                Parent = this,
                Size = new Size(72, 23),
                Sound = SoundList.ButtonA,
            };
            ItemButton.Click += Button_Click;

            ItemButton2 = new MirButton
            {
                Index = 738,
                Library = Libraries.Title,
                Location = new Point(76, 7),
                Parent = this,
                Size = new Size(72, 23),
                Sound = SoundList.ButtonA,
            };
            ItemButton2.Click += Button_Click;

            QuestButton = new MirButton
            {
                Index = 739,
                Library = Libraries.Title,
                Location = new Point(146, 7),
                Parent = this,
                Size = new Size(72, 23),
                Sound = SoundList.ButtonA,
            };
            QuestButton.Click += Button_Click;

            AddButton = new MirButton
            {
                Index = 483,
                HoverIndex = 484,
                PressedIndex = 485,
                Library = Libraries.Title,
                Location = new Point(235, 5),
                Parent = this,
                Size = new Size(72, 23),
                Sound = SoundList.ButtonA,
                Visible = false,
            };
            AddButton.Click += (o1, e) =>
            {
                int openLevel = (GameScene.User.Inventory.Length - 46) / 4;
                int openGold = (1000000 + openLevel * 1000000);
                MirMessageBox messageBox = new MirMessageBox(string.Format(GameLanguage.ExtraSlots4, openGold), MirMessageBoxButtons.OKCancel);

                messageBox.OKButton.Click += (o, a) =>
                {
                    Network.Enqueue(new C.Chat { Message = "@ADDINVENTORY" });
                };
                messageBox.Show();
            };

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(289, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            GoldLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(40, 212),
                Size = new Size(111, 14),
                Sound = SoundList.Gold,
            };
            GoldLabel.Click += (o, e) =>
            {
                if (GameScene.SelectedCell == null)
                    GameScene.PickedUpGold = !GameScene.PickedUpGold && GameScene.Gold > 0;
            };


            Grid = new MirItemCell[8 * 10];

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    int idx = 8 * y + x;
                    Grid[idx] = new MirItemCell
                    {
                        ItemSlot = 6 + idx,
                        GridType = MirGridType.Inventory,
                        Library = Libraries.Items,
                        Parent = this,
                        Location = new Point(x * 36 + 9 + x, y % 5 * 32 + 37 + y % 5),
                    };

                    if (idx >= 40)
                        Grid[idx].Visible = false;
                }
            }

            QuestGrid = new MirItemCell[8 * 5];

            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 5; y++)
                {
                    QuestGrid[8 * y + x] = new MirItemCell
                    {
                        ItemSlot = 8 * y + x,
                        GridType = MirGridType.QuestInventory,
                        Library = Libraries.Items,
                        Parent = this,
                        Location = new Point(x * 36 + 9 + x, y * 32 + 37 + y),
                        Visible = false
                    };
                }
            }

            WeightLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(268, 212),
                Size = new Size(26, 14)
            };
            WeightBar.BeforeDraw += WeightBar_BeforeDraw;

            for (int i = 0; i < LockBar.Length; i++)
            {
                LockBar[i] = new MirImageControl
                {
                    Index = 307,
                    Library = Libraries.Prguse2,
                    Location = new Point(9 + i % 2 * 148, 37 + i / 2 * 33),
                    Parent = this,
                    DrawImage = true,
                    NotControl = true,
                    Visible = false,
                };
            }

        }

        void Button_Click(object sender, EventArgs e)
        {
            if (GameScene.User.Inventory.Length == 46 && sender == ItemButton2)
            {
                MirMessageBox messageBox = new MirMessageBox(GameLanguage.ExtraSlots8, MirMessageBoxButtons.OKCancel);

                messageBox.OKButton.Click += (o, a) =>
                {
                    Network.Enqueue(new C.Chat { Message = "@ADDINVENTORY" });
                };
                messageBox.Show();
            }
            else
            {
                if (sender == ItemButton)
                {
                    RefreshInventory();
                }
                else if (sender == ItemButton2)
                {
                    RefreshInventory2();
                }
                else if (sender == QuestButton)
                {
                    Reset();

                    ItemButton.Index = 737;
                    ItemButton2.Index = 738;
                    QuestButton.Index = 198;

                    if (GameScene.User.Inventory.Length == 46)
                    {
                        ItemButton2.Index = 169;
                    }

                    foreach (var grid in QuestGrid)
                    {
                        grid.Visible = true;
                    }
                }
            }
        }

        void Reset()
        {
            foreach (MirItemCell grid in QuestGrid)
            {
                grid.Visible = false;
            }

            foreach (MirItemCell grid in Grid)
            {
                grid.Visible = false;
            }

            for (int i = 0; i < LockBar.Length; i++)
            {
                LockBar[i].Visible = false;
            }

            AddButton.Visible = false;
        }



        public void RefreshInventory()
        {
            Reset();

            ItemButton.Index = 197;
            ItemButton2.Index = 738;
            QuestButton.Index = 739;

            if (GameScene.User.Inventory.Length == 46)
            {
                ItemButton2.Index = 169;
            }

            foreach (var grid in Grid)
            {
                if (grid.ItemSlot < 46)
                    grid.Visible = true;
                else
                    grid.Visible = false;
            }
        }

        public void RefreshInventory2()
        {
            Reset();

            ItemButton.Index = 737;
            ItemButton2.Index = 168;
            QuestButton.Index = 739;

            foreach (var grid in Grid)
            {
                if (grid.ItemSlot < 46 || grid.ItemSlot >= GameScene.User.Inventory.Length)
                    grid.Visible = false;
                else
                    grid.Visible = true;
            }

            int openLevel = (GameScene.User.Inventory.Length - 46) / 4;
            for (int i = 0; i < LockBar.Length; i++)
            {
                LockBar[i].Visible = (i < openLevel) ? false : true;
            }

            AddButton.Visible = openLevel >= 10 ? false : true;
        }

        public void Process()
        {
            WeightLabel.Text = GameScene.User.Inventory.Count(t => t == null).ToString();
            //WeightLabel.Text = (MapObject.User.MaxBagWeight - MapObject.User.CurrentBagWeight).ToString();
            GoldLabel.Text = GameScene.Gold.ToString("###,###,##0");
        }


        private void WeightBar_BeforeDraw(object sender, EventArgs e)
        {
            if (WeightBar.Library == null) return;

            double percent = MapObject.User.CurrentBagWeight / (double)MapObject.User.Stats[Stat.BagWeight];
            if (percent > 1) percent = 1;
            if (percent <= 0) return;

            Rectangle section = new Rectangle
            {
                Size = new Size((int)((WeightBar.Size.Width - 3) * percent), WeightBar.Size.Height)
            };

            WeightBar.Library.Draw(WeightBar.Index, section, WeightBar.DisplayLocation, Color.White, false);
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

        public MirItemCell GetQuestCell(ulong id)
        {
            return QuestGrid.FirstOrDefault(t => t.Item != null && t.Item.UniqueID == id);
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
    public sealed class BeltDialog : MirImageControl
    {
        public MirLabel[] Key = new MirLabel[6];
        public MirButton CloseButton, RotateButton;
        public MirItemCell[] Grid;

        public BeltDialog()
        {
            Index = 1932;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Visible = true;
            Location = new Point(GameScene.Scene.MainDialog.Location.X + 230, Settings.ScreenHeight - 150);

            BeforeDraw += BeltPanel_BeforeDraw;

            for (int i = 0; i < Key.Length; i++)
            {
                Key[i] = new MirLabel
                {
                    Parent = this,
                    Size = new Size(26, 14),
                    Location = new Point(8 + i * 35, 2),
                    Text = (i + 1).ToString()
                };
            }

            RotateButton = new MirButton
            {
                HoverIndex = 1927,
                Index = 1926,
                Location = new Point(222, 3),
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
                Location = new Point(222, 19),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 1925,
                Sound = SoundList.ButtonA,
                Hint = string.Format(GameLanguage.Close, CMain.InputKeys.GetKey(KeybindOptions.Belt))
            };
            CloseButton.Click += (o, e) => Hide();

            Grid = new MirItemCell[6];

            for (int x = 0; x < 6; x++)
            {
                Grid[x] = new MirItemCell
                {
                    ItemSlot = x,
                    Size = new Size(32, 32),
                    GridType = MirGridType.Inventory,
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
                Libraries.Prguse.Draw(Index + 1, DisplayLocation, Color.White, false, 0.5F);
        }

        public void Flip()
        {
            //0,70 LOCATION
            if (Index == 1932)
            {
                Index = 1944;
                Location = new Point(0, 200);

                for (int x = 0; x < 6; x++)
                    Grid[x].Location = new Point(3, x * 35 + 12);

                CloseButton.Index = 1935;
                CloseButton.HoverIndex = 1936;
                CloseButton.Location = new Point(3, 222);
                CloseButton.PressedIndex = 1937;

                RotateButton.Index = 1938;
                RotateButton.HoverIndex = 1939;
                RotateButton.Location = new Point(19, 222);
                RotateButton.PressedIndex = 1940;

            }
            else
            {
                Index = 1932;
                Location = new Point(GameScene.Scene.MainDialog.Location.X + 230, Settings.ScreenHeight - 150);

                for (int x = 0; x < 6; x++)
                    Grid[x].Location = new Point(x * 35 + 12, 3);

                CloseButton.Index = 1923;
                CloseButton.HoverIndex = 1924;
                CloseButton.Location = new Point(222, 19);
                CloseButton.PressedIndex = 1925;

                RotateButton.Index = 1926;
                RotateButton.HoverIndex = 1927;
                RotateButton.Location = new Point(222, 3);
                RotateButton.PressedIndex = 1928;
            }

            for (int i = 0; i < Key.Length; i++)
            {
                Key[i].Location = (Index != 1932) ? new Point(-1, 11 + i * 35) : new Point(8 + i * 35, 2);
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
    public sealed class SkillBarDialog : MirImageControl
    {
        private readonly MirButton _switchBindsButton;

        public bool AltBind;
        public bool HasSkill = false;
        public byte BarIndex;

        //public bool TopBind = !Settings.SkillMode;
        public MirImageControl[] Cells = new MirImageControl[8];
        public MirLabel[] KeyNameLabels = new MirLabel[8];
        public MirLabel BindNumberLabel = new MirLabel();

        public MirImageControl[] CoolDowns = new MirImageControl[8];

        public SkillBarDialog()
        {
            Index = 2190;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Location = new Point(0, BarIndex * 20);
            Visible = true;

            BeforeDraw += MagicKeyDialog_BeforeDraw;

            _switchBindsButton = new MirButton
            {
                Index = 2247,
                Library = Libraries.Prguse,
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(16, 28),
                Location = new Point(0, 0)
            };
            _switchBindsButton.Click += (o, e) =>
            {
                //Settings.SkillSet = !Settings.SkillSet;

                Update();
            };

            for (var i = 0; i < Cells.Length; i++)
            {
                Cells[i] = new MirImageControl
                {
                    Index = -1,
                    Library = Libraries.MagIcon,
                    Parent = this,
                    Location = new Point(i * 25 + 15, 3),
                };
                int j = i + 1;
                Cells[i].Click += (o, e) =>
                    {
                        GameScene.Scene.UseSpell(j + (8 * BarIndex));
                    };

                CoolDowns[i] = new MirImageControl
                {
                    Library = Libraries.Prguse2,
                    Parent = this,
                    Location = new Point(i * 25 + 15, 3),
                    NotControl = true,
                    UseOffSet = true,
                    Opacity = 0.6F
                };
            }

            BindNumberLabel = new MirLabel
            {
                Text = "1",
                Font = new Font(Settings.FontName, 8F),
                ForeColour = Color.White,
                Parent = this,
                Location = new Point(0, 1),
                Size = new Size(10, 25),
                NotControl = true
            };

            for (var i = 0; i < KeyNameLabels.Length; i++)
            {
                KeyNameLabels[i] = new MirLabel
                {
                    Text = "F" + (i + 1),
                    Font = new Font(Settings.FontName, 8F),
                    ForeColour = Color.White,
                    Parent = this,
                    Location = new Point(i * 25 + 13, 0),
                    Size = new Size(25, 25),
                    NotControl = true
                };
            }
            OnMoving += SkillBar_OnMoving;
        }

        private void SkillBar_OnMoving(object sender, MouseEventArgs e)
        {
            if (BarIndex * 2 >= Settings.SkillbarLocation.Length) return;
            Settings.SkillbarLocation[BarIndex, 0] = this.Location.X;
            Settings.SkillbarLocation[BarIndex, 1] = this.Location.Y;
        }

        private string GetKey(int barindex, int i)
        {
            //KeybindOptions Type = KeybindOptions.Bar1Skill1;
            if ((barindex == 0) && (i == 1))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill1);
            if ((barindex == 0) && (i == 2))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill2);
            if ((barindex == 0) && (i == 3))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill3);
            if ((barindex == 0) && (i == 4))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill4);
            if ((barindex == 0) && (i == 5))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill5);
            if ((barindex == 0) && (i == 6))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill6);
            if ((barindex == 0) && (i == 7))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill7);
            if ((barindex == 0) && (i == 8))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill8);
            if ((barindex == 1) && (i == 1))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar2Skill1);
            if ((barindex == 1) && (i == 2))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar2Skill2);
            if ((barindex == 1) && (i == 3))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar2Skill3);
            if ((barindex == 1) && (i == 4))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar2Skill4);
            if ((barindex == 1) && (i == 5))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar2Skill5);
            if ((barindex == 1) && (i == 6))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar2Skill6);
            if ((barindex == 1) && (i == 7))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar2Skill7);
            if ((barindex == 1) && (i == 8))
                return CMain.InputKeys.GetKey(KeybindOptions.Bar2Skill8);
            return "";
        }
                    

        void MagicKeyDialog_BeforeDraw(object sender, EventArgs e)
        {
            Libraries.Prguse.Draw(2193, new Point(DisplayLocation.X + 12, DisplayLocation.Y), Color.White, true, 0.5F);
        }

        public void Update()
        {
            HasSkill = false;
            foreach (var m in GameScene.User.Magics)
            {
                if ((m.Key < (BarIndex * 8)+1) || (m.Key > ((BarIndex + 1) * 8)+1)) continue;
                HasSkill = true;
            }

            if (!Visible) return;
            Index = 2190;
            _switchBindsButton.Index = 2247;
            BindNumberLabel.Text = (BarIndex + 1).ToString();
            BindNumberLabel.Location = new Point(0, 1);

            for (var i = 1; i <= 8; i++)
            {
                Cells[i - 1].Index = -1;

                int offset = BarIndex * 8;
                string key = GetKey(BarIndex, i);
                KeyNameLabels[i - 1].Text = key;

                foreach (var m in GameScene.User.Magics)
                {
                    if (m.Key != i + offset) continue;
                    HasSkill = true;
                    ClientMagic magic = MapObject.User.GetMagic(m.Spell);
                    if (magic == null) continue;

                    //string key = m.Key > 8 ? string.Format("CTRL F{0}", i) : string.Format("F{0}", m.Key);

                    Cells[i - 1].Index = magic.Icon*2;
                    Cells[i - 1].Hint = string.Format("{0}\nMP: {1}\nCooldown: {2}\nKey: {3}", magic.Name,
                        (magic.BaseCost + (magic.LevelCost * magic.Level)), Functions.PrintTimeSpanFromMilliSeconds(magic.Delay), key);

                    KeyNameLabels[i - 1].Text = "";
                }
            }
        }


        public void Process()
        {
            ProcessSkillDelay();
        }

        private void ProcessSkillDelay()
        {
            if (!Visible) return;

            int offset = BarIndex * 8;

            for (int i = 0; i < Cells.Length; i++)
            {
                foreach (var magic in GameScene.User.Magics)
                {
                    if (magic.Key != i + offset + 1) continue;

                    int totalFrames = 22;
                    long timeLeft = magic.CastTime + magic.Delay - CMain.Time;

                    if (timeLeft < 100)
                    {
                        if (timeLeft > 0) { 
                            CoolDowns[i].Visible = false;
                           // CoolDowns[i].Dispose();
                        }
                        else
                            continue;
                    }

                    int delayPerFrame = (int)(magic.Delay / totalFrames);
                    int startFrame = totalFrames - (int)(timeLeft / delayPerFrame);

                    if ((CMain.Time <= magic.CastTime + magic.Delay))
                    {
                        CoolDowns[i].Visible = true;
                        CoolDowns[i].Index = 1260 + startFrame;
                    }
                }
            }
        }

        public override void Show()
        {
            if (Visible) return;
            if (!HasSkill) return;
            Settings.SkillBar = true;
            Visible = true;
            Update();
        }

        public override void Hide()
        {
            if (!Visible) return;
            Settings.SkillBar = false;
            Visible = false;
        }
    }
    public sealed class CharacterDialog : MirImageControl
    {
        public MirButton CloseButton, CharacterButton, StatusButton, StateButton, SkillButton;
        public MirImageControl CharacterPage, StatusPage, StatePage, SkillPage, ClassImage;

        public MirLabel NameLabel, GuildLabel, LoverLabel;
        public MirLabel ACLabel, MACLabel, DCLabel, MCLabel, SCLabel, HealthLabel, ManaLabel;
        public MirLabel CritRLabel, CritDLabel, LuckLabel, AttkSpdLabel, AccLabel, AgilLabel;
        public MirLabel ExpPLabel, BagWLabel, WearWLabel, HandWLabel, MagicRLabel, PoisonRecLabel, HealthRLabel, ManaRLabel, PoisonResLabel, HolyTLabel, FreezeLabel, PoisonAtkLabel;
        public MirLabel HeadingLabel, StatLabel;
        public MirButton NextButton, BackButton, APButton;

        public MirItemCell[] Grid;
        public MagicButton[] Magics;

        private MirAnimatedControl ArmEffect;
        private MirAnimatedControl WepEffect;

        public int StartIndex;

        public CharacterDialog()
        {
            Index = 504;
            Library = Libraries.Title;
            Location = new Point(Settings.ScreenWidth - 264, 0);
            Movable = true;
            Sort = true;

            BeforeDraw += (o, e) => RefreshInterface();

            ArmEffect = new MirAnimatedControl
            {
                Animated = true,
                Parent = this,
                Library = Libraries.ArmEffect,
                Blending = true,
            };
            WepEffect = new MirAnimatedControl
            {
                Animated = true,
                Parent = this,
                Library = Libraries.ArmEffect,
                Blending = true,
            };

            CharacterPage = new MirImageControl
            {
                Index = 340,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(0, 90),
            };
            CharacterPage.AfterDraw += (o, e) =>
            {
                if (Libraries.StateItems == null) return;
                ItemInfo RealItem = null;
                if (Grid[(int)EquipmentSlot.Armour].Item != null)
                {
                    if (GameScene.User.WingEffect == 1 || GameScene.User.WingEffect == 2)
                    {
                        int wingOffset = GameScene.User.WingEffect == 1 ? 2 : 4;

                        int genderOffset = MapObject.User.Gender == MirGender.Male ? 0 : 1;

                        Libraries.Prguse2.DrawBlend(1200 + wingOffset + genderOffset, DisplayLocation, Color.White, true, 1F);
                    }

                    RealItem = Functions.GetRealItem(Grid[(int)EquipmentSlot.Armour].Item.Info, MapObject.User.Level, MapObject.User.Class, GameScene.ItemInfoList);
                    Libraries.StateItems.Draw(RealItem.Image, DisplayLocation, Color.White, true, 1F);

                    if (RealItem.Effect == 116)
                    {
                        ArmEffect.Index = 154;
                        ArmEffect.AnimationCount = 1;
                        ArmEffect.AnimationDelay = 150;
                        Libraries.ArmEffect.DrawBlend(ArmEffect.Index, new Point(DisplayLocation.X + 48, DisplayLocation.Y + 175), Color.Transparent, true, 1F);
                    }
                }
                if (Grid[(int)EquipmentSlot.Weapon].Item != null)
                {
                    RealItem = Functions.GetRealItem(Grid[(int)EquipmentSlot.Weapon].Item.Info, MapObject.User.Level, MapObject.User.Class, GameScene.ItemInfoList);
                    Libraries.StateItems.Draw(RealItem.Image, DisplayLocation, Color.White, true, 1F);

                }

                if (Grid[(int)EquipmentSlot.Helmet].Item != null)
                    Libraries.StateItems.Draw(Grid[(int)EquipmentSlot.Helmet].Item.Info.Image, DisplayLocation, Color.White, true, 1F);
                else
                {
                    int hair = 441 + MapObject.User.Hair + (MapObject.User.Class == MirClass.Assassin ? 20 : 0) + (MapObject.User.Gender == MirGender.Male ? 0 : 40);

                    int offSetX = MapObject.User.Class == MirClass.Assassin ? (MapObject.User.Gender == MirGender.Male ? 6 : 4) : 0;
                    int offSetY = MapObject.User.Class == MirClass.Assassin ? (MapObject.User.Gender == MirGender.Male ? 25 : 18) : 0;

                    Libraries.Prguse.Draw(hair, new Point(DisplayLocation.X + offSetX, DisplayLocation.Y + offSetY), Color.White, true, 1F);
                }
            };

            StatusPage = new MirImageControl
            {
                Index = 506,
                Parent = this,
                Library = Libraries.Title,
                Location = new Point(0, 90),
                Visible = false,
            };
            StatusPage.BeforeDraw += (o, e) =>
            {
                ACLabel.Text = string.Format("{0}-{1}", MapObject.User.Stats[Stat.MinAC], MapObject.User.Stats[Stat.MaxAC]);
                MACLabel.Text = string.Format("{0}-{1}", MapObject.User.Stats[Stat.MinMAC], MapObject.User.Stats[Stat.MaxMAC]);
                DCLabel.Text = string.Format("{0}-{1}", MapObject.User.Stats[Stat.MinDC], MapObject.User.Stats[Stat.MaxDC]);
                MCLabel.Text = string.Format("{0}-{1}", MapObject.User.Stats[Stat.MinMC], MapObject.User.Stats[Stat.MaxMC]);
                SCLabel.Text = string.Format("{0}-{1}", MapObject.User.Stats[Stat.MinSC], MapObject.User.Stats[Stat.MaxSC]);
                HealthLabel.Text = string.Format("{0}/{1}", MapObject.User.HP, MapObject.User.Stats[Stat.HP]);
                ManaLabel.Text = string.Format("{0}/{1}", MapObject.User.MP, MapObject.User.Stats[Stat.MP]);
                CritRLabel.Text = string.Format("{0}%", MapObject.User.Stats[Stat.CriticalRate]);
                CritDLabel.Text = string.Format("{0}", MapObject.User.Stats[Stat.CriticalDamage]);
                AttkSpdLabel.Text = string.Format("{0}", MapObject.User.Stats[Stat.AttackSpeed]);
                AccLabel.Text = string.Format("+{0}", MapObject.User.Stats[Stat.Accuracy]);
                AgilLabel.Text = string.Format("+{0}", MapObject.User.Stats[Stat.Agility]);
                LuckLabel.Text = string.Format("{0}", MapObject.User.Stats[Stat.Luck]);
            };

            StatePage = new MirImageControl
            {
                Index = 507,
                Parent = this,
                Library = Libraries.Title,
                Location = new Point(0, 90),
                Visible = false
            };
            StatePage.BeforeDraw += (o, e) =>
            {
                ExpPLabel.Text = string.Format("{0:0.##%}", MapObject.User.Experience / (double)MapObject.User.MaxExperience);
                BagWLabel.Text = string.Format("{0}/{1}", MapObject.User.CurrentBagWeight, MapObject.User.Stats[Stat.BagWeight]);
                WearWLabel.Text = string.Format("{0}/{1}", MapObject.User.CurrentWearWeight, MapObject.User.Stats[Stat.WearWeight]);
                HandWLabel.Text = string.Format("{0}/{1}", MapObject.User.CurrentHandWeight, MapObject.User.Stats[Stat.HandWeight]);
                MagicRLabel.Text = string.Format("+{0}", MapObject.User.Stats[Stat.MagicResist]);
                PoisonResLabel.Text = string.Format("+{0}", MapObject.User.Stats[Stat.PoisonResist]);
                HealthRLabel.Text = string.Format("+{0}", MapObject.User.Stats[Stat.HealthRecovery]);
                ManaRLabel.Text = string.Format("+{0}", MapObject.User.Stats[Stat.SpellRecovery]);
                PoisonRecLabel.Text = string.Format("+{0}", MapObject.User.Stats[Stat.PoisonRecovery]);
                HolyTLabel.Text = string.Format("+{0}", MapObject.User.Stats[Stat.Holy]);
                FreezeLabel.Text = string.Format("+{0}", MapObject.User.Stats[Stat.Freezing]);
                PoisonAtkLabel.Text = string.Format("+{0}", MapObject.User.Stats[Stat.PoisonAttack]);
            };

            SkillPage = new MirImageControl
            {
                Index = 508,
                Parent = this,
                Library = Libraries.Title,
                Location = new Point(0, 90),
                Visible = false
            };

            CharacterButton = new MirButton
            {
                Index = 500,
                Library = Libraries.Title,
                Location = new Point(8, 70),
                Parent = this,
                PressedIndex = 500,
                Size = new Size(64, 20),
                Sound = SoundList.ButtonA,
            };
            CharacterButton.Click += (o, e) => ShowCharacterPage();

            StatusButton = new MirButton
            {
                Library = Libraries.Title,
                Location = new Point(70, 70),
                Parent = this,
                PressedIndex = 501,
                Size = new Size(64, 20),
                Sound = SoundList.ButtonA
            };
            StatusButton.Click += (o, e) => ShowStatusPage();

            StateButton = new MirButton
            {
                Library = Libraries.Title,
                Location = new Point(132, 70),
                Parent = this,
                PressedIndex = 502,
                Size = new Size(64, 20),
                Sound = SoundList.ButtonA
            };
            StateButton.Click += (o, e) => ShowStatePage();

            SkillButton = new MirButton
            {
                Library = Libraries.Title,
                Location = new Point(194, 70),
                Parent = this,
                PressedIndex = 503,
                Size = new Size(64, 20),
                Sound = SoundList.ButtonA
            };
            SkillButton.Click += (o, e) => ShowSkillPage();

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(241, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            APButton = new MirButton
            {
                Index = 78,
                HoverIndex = 79,
                PressedIndex = 80,
                Library = Libraries.CustomApSystem,
                Location = new Point(10, 320),
                Parent = this.StatusPage,
                Sound = SoundList.ButtonA
            };
            APButton.Click += (o, e) =>
            {
                if (GameScene.Scene.BonusDialog.Visible)
                    GameScene.Scene.BonusDialog.Hide();
                else
                    GameScene.Scene.BonusDialog.Show();
            };

            NameLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Parent = this,
                Location = new Point(0, 12),
                Size = new Size(264, 20),
                NotControl = true,
            };
            GuildLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Parent = this,
                Location = new Point(0, 33),
                Size = new Size(264, 30),
                NotControl = true,
            };
            ClassImage = new MirImageControl
            {
                Index = 100,
                Library = Libraries.Prguse,
                Location = new Point(15, 33),
                Parent = this,
                NotControl = true,
            };

            Grid = new MirItemCell[Enum.GetNames(typeof(EquipmentSlot)).Length];

            Grid[(int)EquipmentSlot.Weapon] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Weapon,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(131, 7),
            };

            Grid[(int)EquipmentSlot.Armour] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Armour,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(171, 7),
            };

            Grid[(int)EquipmentSlot.Helmet] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Helmet,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(211, 7),
            };

            Grid[(int)EquipmentSlot.Torch] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Torch,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(211, 134),
            };

            Grid[(int)EquipmentSlot.Necklace] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Necklace,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(211, 98),
            };

            Grid[(int)EquipmentSlot.BraceletL] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.BraceletL,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(16, 170),
            };

            Grid[(int)EquipmentSlot.BraceletR] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.BraceletR,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(211, 170),
            };

            Grid[(int)EquipmentSlot.RingL] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.RingL,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(16, 206),
            };

            Grid[(int)EquipmentSlot.RingR] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.RingR,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(211, 206),
            };


            Grid[(int)EquipmentSlot.Amulet] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Amulet,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(34, 242),
            };

            Grid[(int)EquipmentSlot.TaoPoison] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.TaoPoison,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(74, 242),
            };


            Grid[(int)EquipmentSlot.Boots] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Boots,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(114, 242),
            };

            Grid[(int)EquipmentSlot.Belt] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Belt,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(154, 242),
            };


            Grid[(int)EquipmentSlot.Stone] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Stone,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(194, 242),
            };

            Grid[(int)EquipmentSlot.Mount] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Mount,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(213, 303),
            };


            Grid[(int)EquipmentSlot.Trophy] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Trophy,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(173, 303),
            };
            Grid[(int)EquipmentSlot.ShoulderPads] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.ShoulderPads,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(133, 303),
            };

            Grid[(int)EquipmentSlot.Trinket] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Trinket,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(93, 303),
            };

            Grid[(int)EquipmentSlot.Medal] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Medal,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(53, 303),
            };
            Grid[(int)EquipmentSlot.Crystal] = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Crystal,
                GridType = MirGridType.Equipment,
                Parent = CharacterPage,
                Location = new Point(13, 303),
            };
            

            // STATS I
            HealthLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 20),
                NotControl = true,
                Text = "0-0",
            };

            ManaLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 38),
                NotControl = true,
                Text = "0-0",
            };

            ACLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 56),
                NotControl = true,
                Text = "0-0",
            };

            MACLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 74),
                NotControl = true,
                Text = "0-0",
            };
            DCLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 92),
                NotControl = true,
                Text = "0-0"
            };
            MCLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 110),
                NotControl = true,
                Text = "0/0"
            };
            SCLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 128),
                NotControl = true,
                Text = "0/0"
            };
            //Breezer - New Labels
            CritRLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 146),
                NotControl = true
            };
            CritDLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 164),
                NotControl = true
            };
            AttkSpdLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 182),
                NotControl = true
            };
            AccLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 200),
                NotControl = true
            };
            AgilLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 218),
                NotControl = true
            };
            LuckLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatusPage,
                Location = new Point(134, 236),
                NotControl = true
            };
            // STATS II 
            ExpPLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 20),
                NotControl = true,
                Text = "0-0",
            };

            BagWLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 38),
                NotControl = true,
                Text = "0-0",
            };

            WearWLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 56),
                NotControl = true,
                Text = "0-0",
            };

            HandWLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 74),
                NotControl = true,
                Text = "0-0",
            };
            MagicRLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 92),
                NotControl = true,
                Text = "0-0"
            };
            PoisonResLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 110),
                NotControl = true,
                Text = "0/0"
            };
            HealthRLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 128),
                NotControl = true,
                Text = "0/0"
            };
            //Breezer
            ManaRLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 146),
                NotControl = true
            };
            PoisonRecLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 164),
                NotControl = true
            };
            HolyTLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 182),
                NotControl = true
            };
            FreezeLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 200),
                NotControl = true
            };
            PoisonAtkLabel = new MirLabel
            {
                AutoSize = true,
                Parent = StatePage,
                Location = new Point(134, 218),
                NotControl = true
            };

            Magics = new MagicButton[9];

            for (int i = 0; i < Magics.Length; i++)
                Magics[i] = new MagicButton { Parent = SkillPage, Visible = false, Location = new Point(16, 8 + i * 33) };

            NextButton = new MirButton
            {
                Index = 2640,
                HoverIndex = 2641,
                PressedIndex = 2642,
                Location = new Point(140, 315),
                Library = Libraries.Prguse,
                Parent = SkillPage,
                Sound = SoundList.ButtonA,
            };
            NextButton.Click += (o, e) =>
            {
                if (StartIndex + 9 >= MapObject.User.Magics.Count) return;

                StartIndex += 9;
                RefreshInterface();
            };

            BackButton = new MirButton
            {
                Index = 2636,
                HoverIndex = 2637,
                PressedIndex = 2638,
                Location = new Point(90, 315),
                Library = Libraries.Prguse,
                Parent = SkillPage,
                Sound = SoundList.ButtonA,
            };
            BackButton.Click += (o, e) =>
            {
                if (StartIndex - 9 < 0) return;

                StartIndex -= 9;
                RefreshInterface();
            };
        }

        public override void Show()
        {
            if (Visible) return;
            Visible = true;
        }

        public override void Hide()
        {
            GameScene.Scene.SocketDialog.Hide();
            base.Hide();
        }

        public void ShowCharacterPage()
        {
            CharacterPage.Visible = true;
            StatusPage.Visible = false;
            StatePage.Visible = false;
            SkillPage.Visible = false;
            CharacterButton.Index = 500;
            StatusButton.Index = -1;
            StateButton.Index = -1;
            SkillButton.Index = -1;
        }

        private void ShowStatusPage()
        {
            CharacterPage.Visible = false;
            StatusPage.Visible = true;
            StatePage.Visible = false;
            SkillPage.Visible = false;
            CharacterButton.Index = -1;
            StatusButton.Index = 501;
            StateButton.Index = -1;
            SkillButton.Index = -1;
        }

        private void ShowStatePage()
        {
            CharacterPage.Visible = false;
            StatusPage.Visible = false;
            StatePage.Visible = true;
            SkillPage.Visible = false;
            CharacterButton.Index = -1;
            StatusButton.Index = -1;
            StateButton.Index = 502;
            SkillButton.Index = -1;
        }

        public void ShowSkillPage()
        {
            CharacterPage.Visible = false;
            StatusPage.Visible = false;
            StatePage.Visible = false;
            SkillPage.Visible = true;
            CharacterButton.Index = -1;
            StatusButton.Index = -1;
            StateButton.Index = -1;
            SkillButton.Index = 503;
            StartIndex = 0;
        }

        private void RefreshInterface()
        {
            int offSet = MapObject.User.Gender == MirGender.Male ? 0 : 1;

            Index = 504;
            CharacterPage.Index = 340 + offSet;

            switch (MapObject.User.Class)
            {
                case MirClass.Warrior:
                    ClassImage.Index = 100;// + offSet * 5;
                    break;
                case MirClass.Wizard:
                    ClassImage.Index = 101;// + offSet * 5;
                    break;
                case MirClass.Taoist:
                    ClassImage.Index = 102;// + offSet * 5;
                    break;
                case MirClass.Assassin:
                    ClassImage.Index = 103;// + offSet * 5;
                    break;
                case MirClass.Archer:
                    ClassImage.Index = 104;// + offSet * 5;
                    break;
            }

            NameLabel.Text = MapObject.User.Name;
            GuildLabel.Text = MapObject.User.GuildName + " " + MapObject.User.GuildRankName;

            for (int i = 0; i < Magics.Length; i++)
            {
                if (i + StartIndex >= MapObject.User.Magics.Count)
                {
                    Magics[i].Visible = false;
                    continue;
                }

                Magics[i].Visible = true;
                Magics[i].Update(MapObject.User.Magics[i + StartIndex]);
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
    public sealed class MiniMapDialog : MirImageControl
    {
        public MirImageControl LightSetting, NewMail;
        public MirButton ToggleButton, BigMapButton, MailButton;
        public MirLabel LocationLabel, MapNameLabel;
        private float _fade = 1F;
        private bool _bigMode = true;

        public MirLabel AModeLabel, PModeLabel;

        public List<MirLabel> QuestIcons = new List<MirLabel>();

        public MiniMapDialog()
        {
            Index = 2090;
            Library = Libraries.Prguse;
            Location = new Point(Settings.ScreenWidth - 126, 0);
            PixelDetect = true;

            BeforeDraw += MiniMap_BeforeDraw;
            AfterDraw += MiniMapDialog_AfterDraw;

            MapNameLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                Size = new Size(120, 18),
                Location = new Point(2, 2),
                NotControl = true,
            };

            LocationLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                Size = new Size(56, 18),
                Location = new Point(46, 131),
                NotControl = true,
            };

            MailButton = new MirButton
            {
                Index = 2099,
                HoverIndex = 2100,
                PressedIndex = 2101,
                Parent = this,
                Location = new Point(4, 131),
                Library = Libraries.Prguse,
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.Mail
            };
            MailButton.Click += (o, e) => GameScene.Scene.MailListDialog.Toggle();

            NewMail = new MirImageControl
            {
                Index = 544,
                Location = new Point(5, 132),
                Parent = this,
                Library = Libraries.Prguse,
                Visible = false,
                NotControl = true
            };

            BigMapButton = new MirButton
            {
                Index = 2096,
                HoverIndex = 2097,
                PressedIndex = 2098,
                Parent = this,
                Location = new Point(25, 131),
                Library = Libraries.Prguse,
                Sound = SoundList.ButtonA,
                Hint = string.Format(GameLanguage.BigMap, CMain.InputKeys.GetKey(KeybindOptions.Bigmap))
            };
            BigMapButton.Click += (o, e) => GameScene.Scene.BigMapDialog.Toggle();

            ToggleButton = new MirButton
            {
                Index = 2102,
                HoverIndex = 2103,
                PressedIndex = 2104,
                Parent = this,
                Location = new Point(109, 3),
                Library = Libraries.Prguse,
                Sound = SoundList.ButtonA,
                Hint = "MiniMap (" + CMain.InputKeys.GetKey(KeybindOptions.Minimap) + ")"
            };
            ToggleButton.Click += (o, e) => Toggle();

            LightSetting = new MirImageControl
            {
                Index = 2093,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(102, 131),
            };


            AModeLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.Yellow,
                OutLineColour = Color.Black,
                Parent = this,
                Location = new Point(115, 125)
            };

            PModeLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.Yellow,
                OutLineColour = Color.Black,
                Parent = this,
                Location = new Point(230, 125),
                Visible = false
            };
        }

        private void MiniMapDialog_AfterDraw(object sender, EventArgs e)
        {

        }

        private void MiniMap_BeforeDraw(object sender, EventArgs e)
        {

            foreach (var icon in QuestIcons)
                icon.Dispose();

            QuestIcons.Clear();

            MapControl map = GameScene.Scene.MapControl;
            if (map == null) return;

            if (map.MiniMap == 0 && Index != 2091)
            {
                SetSmallMode();
            }
            else if (map.MiniMap > 0 && _bigMode && Index == 2091)
            {
                SetBigMode();
            }

            if (map.MiniMap <= 0 || Index != 2090 || Libraries.MiniMap == null)
            {
                return;
            }

            Rectangle viewRect = new Rectangle(0, 0, 120, 108);
            Point drawLocation = Location;
            drawLocation.Offset(3, 22);

            Size miniMapSize = Libraries.MiniMap.GetSize(map.MiniMap);
            float scaleX = miniMapSize.Width / (float)map.Width;
            float scaleY = miniMapSize.Height / (float)map.Height;

            viewRect.Location = new Point(
                (int)(scaleX * MapObject.User.CurrentLocation.X) - viewRect.Width / 2,
                (int)(scaleY * MapObject.User.CurrentLocation.Y) - viewRect.Height / 2);

            //   viewRect.Location = viewRect.Location.Subtract(1, 1);
            if (viewRect.Right >= miniMapSize.Width)
                viewRect.X = miniMapSize.Width - viewRect.Width;
            if (viewRect.Bottom >= miniMapSize.Height)
                viewRect.Y = miniMapSize.Height - viewRect.Height;

            if (viewRect.X < 0) viewRect.X = 0;
            if (viewRect.Y < 0) viewRect.Y = 0;

            Libraries.MiniMap.Draw(map.MiniMap, viewRect, drawLocation, Color.FromArgb(255, 255, 255), _fade);


            int startPointX = (int)(viewRect.X / scaleX);
            int startPointY = (int)(viewRect.Y / scaleY);

            for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
            {
                MapObject ob = MapControl.Objects[i];

                if (ob.Race == ObjectType.Item || ob.Dead || ob.Race == ObjectType.Spell || ob.Sneaking) continue;
                float x = ((ob.CurrentLocation.X - startPointX) * scaleX) + drawLocation.X;
                float y = ((ob.CurrentLocation.Y - startPointY) * scaleY) + drawLocation.Y;

                Color colour;

                if ((GroupDialog.GroupList.Contains(ob.Name) && MapObject.User != ob) || ob.Name.EndsWith(string.Format("({0})", MapObject.User.Name)))
                    colour = Color.FromArgb(0, 0, 255);
                else
                    if (ob is PlayerObject)
                {
                    colour = Color.FromArgb(255, 255, 255);
                }
                else if (ob is NPCObject || ob.AI == 6)
                {
                    colour = Color.FromArgb(0, 255, 50);
                }
                else
                    colour = Color.FromArgb(255, 0, 0);

                DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 2, 2), Vector3.Zero, new Vector3((float)(x - 0.5), (float)(y - 0.5), 0.0F), colour);

                #region NPC Quest Icons

                if (ob is NPCObject npc && npc.GetAvailableQuests(true).Any())
                {
                    string text = "";
                    Color color = Color.Empty;

                    switch (npc.QuestIcon)
                    {
                        case QuestIcon.ExclamationBlue:
                            color = Color.DodgerBlue;
                            text = "!";
                            break;
                        case QuestIcon.ExclamationYellow:
                            color = Color.Yellow;
                            text = "!";
                            break;
                        case QuestIcon.ExclamationGreen:
                            color = Color.Green;
                            text = "!";
                            break;
                        case QuestIcon.QuestionBlue:
                            color = Color.DodgerBlue;
                            text = "?";
                            break;
                        case QuestIcon.QuestionWhite:
                            color = Color.White;
                            text = "?";
                            break;
                        case QuestIcon.QuestionYellow:
                            color = Color.Yellow;
                            text = "?";
                            break;
                        case QuestIcon.QuestionGreen:
                            color = Color.Green;
                            text = "?";
                            break;
                    }

                    QuestIcons.Add(new MirLabel
                    {
                        AutoSize = true,
                        Parent = GameScene.Scene.MiniMapDialog,
                        Font = new Font(Settings.FontName, 9f, FontStyle.Bold),
                        DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                        Text = text,
                        ForeColour = color,
                        Location = new Point((int)(x - Settings.ScreenWidth + GameScene.Scene.MiniMapDialog.Size.Width) - 6, (int)(y) - 10),
                        NotControl = true,
                        Visible = true,
                        Modal = true
                    });
                }

                #endregion

            }
        }

        public void Toggle()
        {
            if (_fade == 0F)
            {
                _bigMode = true;
                SetBigMode();
                _fade = 1F;
            }
            //else if(_fade == 1F)
            //{
            //    _bigMode = true;
            //    SetBigMode();
            //    _fade = 0.8F;
            //}
            else
            {
                _bigMode = false;
                SetSmallMode();
                _fade = 0;
            }

            Redraw();
        }

        private void SetSmallMode()
        {
            Index = 2091;
            int y = Size.Height - 23;
            MailButton.Location = new Point(4, y);
            NewMail.Location = new Point(5, y + 1);
            BigMapButton.Location = new Point(25, y);
            LocationLabel.Location = new Point(46, y);
            LightSetting.Location = new Point(102, y);

            GameScene.Scene.DuraStatusPanel.Location = new Point(GameScene.Scene.MiniMapDialog.Location.X + 86,
            GameScene.Scene.MiniMapDialog.Size.Height);
        }

        private void SetBigMode()
        {
            Index = 2090;
            int y = Size.Height - 23;
            MailButton.Location = new Point(4, y);
            NewMail.Location = new Point(5, y + 1);
            BigMapButton.Location = new Point(25, y);
            LocationLabel.Location = new Point(46, y);
            LightSetting.Location = new Point(102, y);

            GameScene.Scene.DuraStatusPanel.Location = new Point(GameScene.Scene.MiniMapDialog.Location.X + 86,
            GameScene.Scene.MiniMapDialog.Size.Height);
        }

        public void Process()
        {
            MapControl map = GameScene.Scene.MapControl;
            if (map == null) return;

            MapNameLabel.Text = map.Title;
            LocationLabel.Text = Functions.PointToString(MapObject.User.CurrentLocation);

            GameScene.Scene.MainDialog.SModeLabel.Location = new Point((GameScene.Scene.MiniMapDialog.Location.X - 3) - GameScene.Scene.MainDialog.Location.X,
            (GameScene.Scene.MiniMapDialog.Size.Height + 150) - Settings.ScreenHeight);
            GameScene.Scene.MainDialog.AModeLabel.Location = new Point((GameScene.Scene.MiniMapDialog.Location.X - 3) - GameScene.Scene.MainDialog.Location.X,
            (GameScene.Scene.MiniMapDialog.Size.Height + 165) - Settings.ScreenHeight);
            GameScene.Scene.MainDialog.PModeLabel.Location = new Point((GameScene.Scene.MiniMapDialog.Location.X - 3) - GameScene.Scene.MainDialog.Location.X,
            (GameScene.Scene.MiniMapDialog.Size.Height + 180) - Settings.ScreenHeight);

            if (GameScene.Scene.NewMail)
            {
                double time = (CMain.Time) / 100D;

                if (Math.Round(time) % 10 < 5 || GameScene.Scene.NewMailCounter >= 10)
                {
                    NewMail.Visible = true;
                }
                else
                {
                    if (NewMail.Visible)
                    {
                        GameScene.Scene.NewMailCounter++;
                    }

                    NewMail.Visible = false;
                }
            }
            else
            {
                NewMail.Visible = false;
            }
        }
    }
    public sealed class InspectDialog : MirImageControl
    {
        public static UserItem[] Items = new UserItem[14];
        public static uint InspectID;

        public string Name;
        public string GuildName;
        public string GuildRank;
        public MirClass Class;
        public MirGender Gender;
        public byte Hair;
        public ushort Level;
        public string LoverName;

        public MirButton CloseButton, GroupButton, FriendButton, MailButton, TradeButton, LoverButton;
        public MirImageControl CharacterPage, ClassImage;
        public MirLabel NameLabel;
        public MirLabel GuildLabel, LoverLabel;

        private MirAnimatedControl ArmEffect;
        private MirAnimatedControl WepEffect;

        public MirItemCell
            WeaponCell,
            ArmorCell,
            HelmetCell,
            TorchCell,
            NecklaceCell,
            BraceletLCell,
            BraceletRCell,
            RingLCell,
            RingRCell,
            AmuletCell,
            BeltCell,
            BootsCell,
            StoneCell,
            MountCell,
            TaoPoisonCell,
            TrophyCell,
            TrinketCell,
            MedalCell,
            CrystalCell,
            ShoulderPadsCell;

        public InspectDialog()
        {
            Index = 430;
            Library = Libraries.Prguse;
            Location = new Point(536, 0);
            Movable = true;
            Sort = true;

            ArmEffect = new MirAnimatedControl
            {
                Animated = true,
                AnimationCount = 48,
                AnimationDelay = 150,
                Index = 3190,
                Parent = this,
                Library = Libraries.StateEffect,
                Loop = true,
                Blending = true,
            };
            WepEffect = new MirAnimatedControl
            {
                Animated = true,
                AnimationCount = 48,
                AnimationDelay = 150,
                Index = 3190,
                Parent = this,
                Library = Libraries.StateEffect,
                Loop = true,
                Blending = true,
            };

            CharacterPage = new MirImageControl
            {
                Index = 340,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(0, 70),
            };
            CharacterPage.AfterDraw += (o, e) =>
            {
                if (Libraries.StateItems == null) return;

                ItemInfo RealItem = null;

                if (ArmorCell.Item != null)
                {
                    RealItem = Functions.GetRealItem(ArmorCell.Item.Info, Level, Class, GameScene.ItemInfoList);
                    Libraries.StateItems.Draw(RealItem.Image, new Point(DisplayLocation.X + 0, DisplayLocation.Y + -20), Color.White, true, 1F);

                    if (RealItem.Effect > 0)
                    {
                        int wingOffset = RealItem.Effect == 1 ? 2 : 4;

                        int genderOffset = MapObject.User.Gender == MirGender.Male ? 0 : 1;

                        Libraries.Prguse2.DrawBlend(1200 + wingOffset + genderOffset, new Point(DisplayLocation.X, DisplayLocation.Y - 20), Color.White, true, 1F);
                    }
                }

                if (WeaponCell.Item != null)
                {
                    RealItem = Functions.GetRealItem(WeaponCell.Item.Info, Level, Class, GameScene.ItemInfoList);
                    Libraries.StateItems.Draw(RealItem.Image, new Point(DisplayLocation.X, DisplayLocation.Y - 20),
                    Color.White, true, 1F);

                }

                if (HelmetCell.Item != null)
                    Libraries.StateItems.Draw(HelmetCell.Item.Info.Image, new Point(DisplayLocation.X, DisplayLocation.Y - 20), Color.White, true, 1F);
                else
                {
                    int hair = 441 + Hair + (Class == MirClass.Assassin ? 20 : 0) + (Gender == MirGender.Male ? 0 : 40);

                    int offSetX = Class == MirClass.Assassin ? (Gender == MirGender.Male ? 6 : 4) : 0;
                    int offSetY = Class == MirClass.Assassin ? (Gender == MirGender.Male ? 25 : 18) : 0;

                    Libraries.Prguse.Draw(hair, new Point(DisplayLocation.X + offSetX, DisplayLocation.Y + offSetY - 20), Color.White, true, 1F);
                }
            };


            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(241, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();



            GroupButton = new MirButton
            {
                HoverIndex = 432,
                Index = 431,
                Location = new Point(75, 419),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 433,
                Sound = SoundList.ButtonA,
                Hint = "Invite to Group",
            };
            GroupButton.Click += (o, e) =>
            {

                if (GroupDialog.GroupList.Count >= Globals.MaxGroup)
                {
                    GameScene.Scene.ChatDialog.ReceiveChat("Your group already has the maximum number of members.", ChatType.System);
                    return;
                }
                if (GroupDialog.GroupList.Count > 0 && GroupDialog.GroupList[0] != MapObject.User.Name)
                {

                    GameScene.Scene.ChatDialog.ReceiveChat("You are not the leader of your group.", ChatType.System);
                }

                Network.Enqueue(new C.AddMember { Name = Name });
                return;
            };

            FriendButton = new MirButton
            {
                HoverIndex = 435,
                Index = 434,
                Location = new Point(105, 419),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 436,
                Sound = SoundList.ButtonA,
                Hint = "Add to Friends List",
            };
            FriendButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.AddFriend { Name = Name, Blocked = false });
            };

            MailButton = new MirButton
            {
                HoverIndex = 438,
                Index = 437,
                Location = new Point(135, 419),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 439,
                Sound = SoundList.ButtonA,
                Hint = "Send Mail",
            };
            MailButton.Click += (o, e) => GameScene.Scene.MailComposeLetterDialog.ComposeMail(Name);

            TradeButton = new MirButton
            {
                HoverIndex = 524,
                Index = 523,
                Location = new Point(165, 419),
                Library = Libraries.Prguse,
                Parent = this,
                PressedIndex = 525,
                Sound = SoundList.ButtonA,
                Hint = "Trade",
            };
            TradeButton.Click += (o, e) => Network.Enqueue(new C.TradeRequest());

            NameLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                Location = new Point(50, 12),
                Size = new Size(190, 20),
                NotControl = true
            };
            NameLabel.Click += (o, e) =>
            {
                GameScene.Scene.ChatDialog.ChatTextBox.SetFocus();
                GameScene.Scene.ChatDialog.ChatTextBox.Text = string.Format("/{0} ", Name);
                GameScene.Scene.ChatDialog.ChatTextBox.Visible = true;
                GameScene.Scene.ChatDialog.ChatTextBox.TextBox.SelectionLength = 0;
                GameScene.Scene.ChatDialog.ChatTextBox.TextBox.SelectionStart = Name.Length + 2;

            };
            LoverButton = new MirButton
            {
                Index = 604,
                Location = new Point(17, 17),
                Library = Libraries.Prguse,
                Parent = this,
                Sound = SoundList.None
            };

            GuildLabel = new MirLabel
            {
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                Location = new Point(50, 33),
                Size = new Size(190, 30),
                NotControl = true,
            };

            ClassImage = new MirImageControl
            {
                Index = 100,
                Library = Libraries.Prguse,
                Location = new Point(15, 33),
                Parent = this,
                NotControl = true,
            };


            WeaponCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Weapon,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(131, 7),
            };

            ArmorCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Armour,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(171, 7),
            };

            HelmetCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Helmet,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(211, 7),
            };


            TorchCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Torch,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(211, 134),
            };

            NecklaceCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Necklace,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(211, 98),
            };

            BraceletLCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.BraceletL,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(16, 170),
            };
            BraceletRCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.BraceletR,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(211, 170),
            };
            RingLCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.RingL,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(16, 206),
            };
            RingRCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.RingR,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(211, 206),
            };

            AmuletCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Amulet,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(34, 242),
            };
            TaoPoisonCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.TaoPoison,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(74, 242),
            };

            BootsCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Boots,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(114, 242),
            };
            BeltCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Belt,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(154, 242),
            };

            StoneCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Stone,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(194, 242),
            };

            MountCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Mount,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(213, 303),
            };

            TrophyCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Trophy,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(173, 303),
            };
            ShoulderPadsCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.ShoulderPads,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(133, 303),
            };
            TrinketCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Trinket,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(93, 303),
            };
            MedalCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Medal,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(53, 303),
            };
            CrystalCell = new MirItemCell
            {
                ItemSlot = (int)EquipmentSlot.Crystal,
                GridType = MirGridType.Inspect,
                Parent = CharacterPage,
                Location = new Point(13, 303),
            };
            
        }

        public void RefreshInferface()
        {
            int offSet = Gender == MirGender.Male ? 0 : 1;

            CharacterPage.Index = 340 + offSet;

            switch (Class)
            {
                case MirClass.Warrior:
                    ClassImage.Index = 100;// + offSet * 5;
                    break;
                case MirClass.Wizard:
                    ClassImage.Index = 101;// + offSet * 5;
                    break;
                case MirClass.Taoist:
                    ClassImage.Index = 102;// + offSet * 5;
                    break;
                case MirClass.Assassin:
                    ClassImage.Index = 103;// + offSet * 5;
                    break;
                case MirClass.Archer:
                    ClassImage.Index = 104;// + offSet * 5;
                    break;
            }

            NameLabel.Text = Name;
            GuildLabel.Text = GuildName + " " + GuildRank;
            if (LoverName != "")
            {
                LoverButton.Visible = true;
                LoverButton.Hint = LoverName;
            }
            else
                LoverButton.Visible = false;


            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i] == null) continue;
                GameScene.Bind(Items[i]);
            }
        }


    }
    public sealed class OptionDialog : MirImageControl
    {
        public MirLabel SkillModeLabel, SkillBarLabel, EffectLabel, DropViewLabel, HPViewLabel, SoundBarLabel, MusicSoundBarLabel;
        public MirLabel BossHPBarLabel;
        public MirLabel NameViewLabel, NpcNameViewLabel, MonsterNameViewLabel;

        public MirButton SkillModeOn, SkillModeOff;
        public MirButton SkillBarOn, SkillBarOff;
        public MirButton EffectOn, EffectOff;
        public MirButton DropViewOn, DropViewOff;
        public MirButton NameViewOn, NameViewOff;
        public MirButton HPViewOn, HPViewOff;
        public MirImageControl SoundBar, MusicSoundBar;
        public MirImageControl SoundVolumeBar, MusicVolumeBar;
        public MirButton BossHPBarOn, BossHPBarOff;
        public MirButton NpcNameViewOn, NpcNameViewOff;
        public MirButton MonsterNameViewOn, MonsterNameViewOff;
        public MirButton MonsterNameView;

        public MirButton CloseButton;


        public OptionDialog()
        {
            Index = 411;
            Library = Libraries.Title;
            Movable = true;
            Sort = true;

            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 2);

            BeforeDraw += OptionPanel_BeforeDraw;

            SkillModeLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 40),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ Skill Mode"
            };
            SkillModeOn = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(125, 42),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 451,
            };
            SkillModeOn.Click += (o, e) =>
            {
                GameScene.Scene.ChangeSkillMode(false);
            };
            SkillModeOff = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(165, 42),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 454
            };
            SkillModeOff.Click += (o, e) =>
            {
                GameScene.Scene.ChangeSkillMode(true);
            };

            SkillBarLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 60),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ Skill Bar"
            };
            SkillBarOn = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(125, 62),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 457,
            };
            SkillBarOn.Click += (o, e) => Settings.SkillBar = true;
            SkillBarOff = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(165, 62),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 460
            };
            SkillBarOff.Click += (o, e) => Settings.SkillBar = false;

            EffectLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 80),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ Effect"
            };
            EffectOn = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(125, 82),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 457,
            };
            EffectOn.Click += (o, e) => Settings.Effect = true;
            EffectOff = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(165, 82),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 460
            };
            EffectOff.Click += (o, e) => Settings.Effect = false;

            DropViewLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 100),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ DropView"
            };
            DropViewOn = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(125, 102),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 457,
            };
            DropViewOn.Click += (o, e) => Settings.DropView = true;
            DropViewOff = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(165, 102),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 460
            };
            DropViewOff.Click += (o, e) => Settings.DropView = false;

            HPViewLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 120),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ HP/MP View"
            };
            HPViewOn = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(125, 122),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 122),
                PressedIndex = 463,
            };
            HPViewOn.Click += (o, e) =>
            {
                Settings.HPView = true;
                GameScene.Scene.ChatDialog.ReceiveChat("[HP/MP Mode 1]", ChatType.Hint);
            };
            HPViewOff = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(165, 122),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 466
            };
            HPViewOff.Click += (o, e) =>
            {
                Settings.HPView = false;
                GameScene.Scene.ChatDialog.ReceiveChat("[HP/MP Mode 2]", ChatType.Hint);
            };

            SoundBarLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 140),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ Sound"
            };
            SoundBar = new MirImageControl
            {
                Index = 427,
                Library = Libraries.Title,
                Location = new Point(126, 145),
                Size = new Size(40, 17),
                Parent = this,
                DrawImage = false,
            };
            SoundBar.MouseDown += SoundBar_MouseMove;
            SoundBar.MouseMove += SoundBar_MouseMove;
            SoundBar.BeforeDraw += SoundBar_BeforeDraw;
            SoundVolumeBar = new MirImageControl
            {
                Index = 429,
                Library = Libraries.Title,
                Location = new Point(125, 145),
                Parent = this,
                NotControl = true,
                Visible = false
            };

            MusicSoundBarLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 160),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ Music"
            };
            MusicSoundBar = new MirImageControl
            {
                Index = 428,
                Library = Libraries.Title,
                Location = new Point(126, 165),
                Parent = this,
                DrawImage = false,
            };
            MusicSoundBar.MouseDown += MusicSoundBar_MouseMove;
            MusicSoundBar.MouseMove += MusicSoundBar_MouseMove;
            MusicSoundBar.MouseUp += MusicSoundBar_MouseUp;
            MusicSoundBar.BeforeDraw += MusicSoundBar_BeforeDraw;
            MusicVolumeBar = new MirImageControl
            {
                Index = 20,
                Library = Libraries.Prguse,
                Location = new Point(125, 145),
                Parent = this,
                NotControl = true,
                Visible = false
            };

            NameViewLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 180),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ Player Name"
            };
            NameViewOn = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(125, 182),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 463,
            };
            NameViewOn.Click += (o, e) => Settings.NameView = true;
            NameViewOff = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(165, 182),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 466
            };
            NameViewOff.Click += (o, e) => Settings.NameView = false;

            BossHPBarLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 200),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ Boss HP"
            };
            BossHPBarOn = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(125, 202),
                Parent = this,
                Sound = SoundList.ButtonA,
                PressedIndex = 463,
            };
            BossHPBarOn.Click += (o, e) => Settings.BossHPBar = true;
            BossHPBarOff = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(165, 202),
                Parent = this,
                Sound = SoundList.ButtonA,
                PressedIndex = 466
            };
            BossHPBarOff.Click += (o, e) => Settings.BossHPBar = false;

            NpcNameViewLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 220),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ Npc Name"
            };
            NpcNameViewOn = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(125, 220),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 463,
            };
            NpcNameViewOn.Click += (o, e) => Settings.NpcNameView = true;
            NpcNameViewOff = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(165, 220),
                Parent = this,
                Sound = SoundList.ButtonA,
                Size = new Size(36, 17),
                PressedIndex = 466
            };
            NpcNameViewOff.Click += (o, e) => Settings.NpcNameView = false;

            MonsterNameViewLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(8, 240),
                NotControl = true,
                ForeColour = Color.LightSteelBlue,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                Text = "✭ Monster Name"
            };
            MonsterNameViewOn = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(125, 242),
                Parent = this,
                Sound = SoundList.ButtonA,
                PressedIndex = 463,
            };
            MonsterNameViewOn.Click += (o, e) => Settings.MonsterNameView = true;
            MonsterNameViewOff = new MirButton
            {
                Library = Libraries.Prguse2,
                Location = new Point(165, 242),
                Parent = this,
                Sound = SoundList.ButtonA,
                PressedIndex = 466
            };
            MonsterNameViewOff.Click += (o, e) => Settings.MonsterNameView = false;

            CloseButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                Library = Libraries.Prguse2,
                Location = new Point(Size.Width - 26, 5),
                Parent = this,
                Sound = SoundList.ButtonA,
                PressedIndex = 362,
            };
            CloseButton.Click += (o, e) => Hide();
        }

        public void ToggleSkillButtons(bool Ctrl)
        {
            foreach (KeyBind KeyCheck in CMain.InputKeys.Keylist)
            {
                if (KeyCheck.Key == Keys.None)
                    continue;
                if ((KeyCheck.function < KeybindOptions.Bar1Skill1) || (KeyCheck.function > KeybindOptions.Bar2Skill8)) continue;
                //need to test this 
                if ((KeyCheck.RequireCtrl != 1) && (KeyCheck.RequireTilde != 1)) continue;
                KeyCheck.RequireCtrl = (byte)(Ctrl ? 1 : 0);
                KeyCheck.RequireTilde = (byte)(Ctrl ? 0 : 1);
            }
        }

        private void SoundBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || SoundBar != ActiveControl) return;

            Point p = e.Location.Subtract(SoundBar.DisplayLocation);

            byte volume = (byte)(p.X / (double)SoundBar.Size.Width * 100);
            Settings.Volume = volume;


            double percent = Settings.Volume / 100D;
            if (percent > 1) percent = 1;

            SoundVolumeBar.Location = percent > 0 ? new Point(159 + (int)((SoundBar.Size.Width - 2) * percent), 165) : new Point(159, 165);
        }

        private void SoundBar_BeforeDraw(object sender, EventArgs e)
        {
            if (SoundBar.Library == null) return;

            double percent = Settings.Volume / 100D;
            if (percent > 1) percent = 1;
            if (percent > 0)
            {
                Rectangle section = new Rectangle
                {
                    Size = new Size((int)((SoundBar.Size.Width - 2) * percent), SoundBar.Size.Height)
                };

                SoundBar.Library.Draw(SoundBar.Index, section, SoundBar.DisplayLocation, Color.White, false);
                SoundVolumeBar.Location = new Point(125 + section.Size.Width, 142);
            }
            else
                SoundVolumeBar.Location = new Point(125, 142);
        }

        private void MusicSoundBar_BeforeDraw(object sender, EventArgs e)
        {
            if (MusicSoundBar.Library == null) return;

            double percent = Settings.MusicVolume / 100D;
            if (percent > 1) percent = 1;
            if (percent > 0)
            {
                Rectangle section = new Rectangle
                {
                    Size = new Size((int)((MusicSoundBar.Size.Width - 2) * percent), MusicSoundBar.Size.Height)
                };

                MusicSoundBar.Library.Draw(MusicSoundBar.Index, section, MusicSoundBar.DisplayLocation, Color.White, false);
                MusicVolumeBar.Location = new Point(159 + section.Size.Width, 165);
            }
            else
                MusicVolumeBar.Location = new Point(159, 165);
        }

        public void MusicSoundBar_MouseUp(object sender, MouseEventArgs e)
        {
            if (SoundManager.MusicVol <= -2900)
                SoundManager.MusicVol = -3000;
            if (SoundManager.MusicVol >= -100)
                SoundManager.MusicVol = 0;


            //SoundManager.Device.Dispose();
            //SoundManager.Create();
            //SoundManager.PlayMusic(SoundList.Music, true);

            if (SoundManager.Music == null) return;

            SoundManager.Music.SetVolume(SoundManager.MusicVol);

        }

        private void MusicSoundBar_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button != MouseButtons.Left || MusicSoundBar != ActiveControl) return;

            Point p = e.Location.Subtract(MusicSoundBar.DisplayLocation);

            byte volume = (byte)(p.X / (double)MusicSoundBar.Size.Width * 100);
            Settings.MusicVolume = volume;


            double percent = Settings.MusicVolume / 100D;
            if (percent > 1) percent = 1;

            MusicVolumeBar.Location = percent > 0 ? new Point(159 + (int)((MusicSoundBar.Size.Width - 2) * percent), 244) : new Point(159, 244);
        }

        private void OptionPanel_BeforeDraw(object sender, EventArgs e)
        {
            if (Settings.SkillMode)
            {
                SkillModeOn.Index = 452;
                SkillModeOff.Index = 453;
            }
            else
            {
                SkillModeOn.Index = 450;
                SkillModeOff.Index = 455;
            }

            if (Settings.SkillBar)
            {
                SkillBarOn.Index = 458;
                SkillBarOff.Index = 459;
            }
            else
            {
                SkillBarOn.Index = 456;
                SkillBarOff.Index = 461;
            }

            if (Settings.Effect)
            {
                EffectOn.Index = 458;
                EffectOff.Index = 459;
            }
            else
            {
                EffectOn.Index = 456;
                EffectOff.Index = 461;
            }

            if (Settings.DropView)
            {
                DropViewOn.Index = 458;
                DropViewOff.Index = 459;
            }
            else
            {
                DropViewOn.Index = 456;
                DropViewOff.Index = 461;
            }

            if (Settings.NameView)
            {
                NameViewOn.Index = 458;
                NameViewOff.Index = 459;
            }
            else
            {
                NameViewOn.Index = 456;
                NameViewOff.Index = 461;
            }

            if (Settings.NpcNameView)
            {
                NpcNameViewOn.Index = 458;
                NpcNameViewOff.Index = 459;
            }
            else
            {
                NpcNameViewOn.Index = 456;
                NpcNameViewOff.Index = 461;
            }

            if (Settings.MonsterNameView)
            {
                MonsterNameViewOn.Index = 458;
                MonsterNameViewOff.Index = 459;
            }
            else
            {
                MonsterNameViewOn.Index = 456;
                MonsterNameViewOff.Index = 461;
            }

            if (Settings.HPView)
            {
                HPViewOn.Index = 464;
                HPViewOff.Index = 465;
            }
            else
            {
                HPViewOn.Index = 462;
                HPViewOff.Index = 467;
            }

            if (Settings.BossHPBar)
            {
                BossHPBarOn.Index = 458;
                BossHPBarOff.Index = 459;
            }
            else
            {
                BossHPBarOn.Index = 456;
                BossHPBarOff.Index = 461;
            }
        }

    }
    public sealed class MenuDialog : MirImageControl
    {
        public MirButton ExitButton,
                         LogOutButton,
                         HelpButton,
                         KeyboardLayoutButton,
                         RankingButton,
                         CraftingButton,
                         IntelligentCreatureButton,
                         RideButton,
                         FishingButton,
                         FriendButton,
                         MentorButton,
                         RelationshipButton,
                         GroupButton,
                         GuildButton;

        public MenuDialog()
        {
            Index = 567;
            Parent = GameScene.Scene;
            Library = Libraries.Title;
            Location = new Point(Settings.ScreenWidth - Size.Width, GameScene.Scene.MainDialog.Location.Y - this.Size.Height + 15);
            Sort = true;
            Visible = false;
            Movable = true;

            ExitButton = new MirButton
            {
                HoverIndex = 634,
                Index = 633,
                Parent = this,
                Library = Libraries.Title,
                Location = new Point(3, 12),
                PressedIndex = 635,
                Hint = string.Format(GameLanguage.Exit, CMain.InputKeys.GetKey(KeybindOptions.Exit))
            };
            ExitButton.Click += (o, e) => GameScene.Scene.QuitGame();

            LogOutButton = new MirButton
            {
                HoverIndex = 637,
                Index = 636,
                Parent = this,
                Library = Libraries.Title,
                Location = new Point(3, 31),
                PressedIndex = 638,
                Hint = string.Format(GameLanguage.LogOut, CMain.InputKeys.GetKey(KeybindOptions.Logout))
            };
            LogOutButton.Click += (o, e) => GameScene.Scene.LogOut();


            HelpButton = new MirButton
            {
                Index = 1970,
                HoverIndex = 1971,
                PressedIndex = 1972,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 50),
                Hint = string.Format(GameLanguage.Help, CMain.InputKeys.GetKey(KeybindOptions.Help))
            };
            HelpButton.Click += (o, e) =>
            {
                if (GameScene.Scene.HelpDialog.Visible)
                    GameScene.Scene.HelpDialog.Hide();
                else GameScene.Scene.HelpDialog.Show();
            };

            KeyboardLayoutButton = new MirButton
            {
                Index = 1973,
                HoverIndex = 1974,
                PressedIndex = 1975,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 69),
                Visible = true,
                Hint = "Keyboard (" + CMain.InputKeys.GetKey(KeybindOptions.Keybind) + ")"
            };
            KeyboardLayoutButton.Click += (o, e) =>
            {
                if (GameScene.Scene.KeyboardLayoutDialog.Visible)
                    GameScene.Scene.KeyboardLayoutDialog.Hide();
                else GameScene.Scene.KeyboardLayoutDialog.Show();
            };

            RankingButton = new MirButton
            {
                Index = 2000,
                HoverIndex = 2001,
                PressedIndex = 2002,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 88),
                Hint = string.Format(GameLanguage.Ranking, CMain.InputKeys.GetKey(KeybindOptions.Ranking))
                //Visible = false
            };
            RankingButton.Click += (o, e) =>
            {
                if (GameScene.Scene.RankingDialog.Visible)
                    GameScene.Scene.RankingDialog.Hide();
                else GameScene.Scene.RankingDialog.Show();
            };

            CraftingButton = new MirButton
            {
                Index = 2000,
                HoverIndex = 2001,
                PressedIndex = 2002,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 107),
                Visible = false
            };
            CraftingButton.Click += (o, e) =>
            {

            };

            IntelligentCreatureButton = new MirButton
            {
                Index = 431,
                HoverIndex = 432,
                PressedIndex = 433,
                Parent = this,
                Library = Libraries.Prguse2,
                Location = new Point(3, 126),
                Hint = string.Format(GameLanguage.Creatures, CMain.InputKeys.GetKey(KeybindOptions.Creature))
            };
            IntelligentCreatureButton.Click += (o, e) =>
            {
                if (GameScene.Scene.IntelligentCreatureDialog.Visible)
                    GameScene.Scene.IntelligentCreatureDialog.Hide();
                else GameScene.Scene.IntelligentCreatureDialog.Show();
            };
            RideButton = new MirButton
            {
                Index = 1976,
                HoverIndex = 1977,
                PressedIndex = 1978,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 145),
                Hint = string.Format(GameLanguage.Mount, CMain.InputKeys.GetKey(KeybindOptions.MountWindow))
            };
            RideButton.Click += (o, e) =>
            {
                if (GameScene.Scene.MountDialog.Visible)
                    GameScene.Scene.MountDialog.Hide();
                else GameScene.Scene.MountDialog.Show();
            };

            FishingButton = new MirButton
            {
                Index = 1979,
                HoverIndex = 1980,
                PressedIndex = 1981,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 164),
                Hint = string.Format(GameLanguage.Fishing, CMain.InputKeys.GetKey(KeybindOptions.Fishing))
            };
            FishingButton.Click += (o, e) =>
            {
                if (GameScene.Scene.FishingDialog.Visible)
                    GameScene.Scene.FishingDialog.Hide();
                else GameScene.Scene.FishingDialog.Show();
            };

            FriendButton = new MirButton
            {
                Index = 1982,
                HoverIndex = 1983,
                PressedIndex = 1984,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 183),
                Visible = true,
                Hint = string.Format(GameLanguage.Friends, CMain.InputKeys.GetKey(KeybindOptions.Friends))
            };
            FriendButton.Click += (o, e) =>
            {
                if (GameScene.Scene.FriendDialog.Visible)
                    GameScene.Scene.FriendDialog.Hide();
                else GameScene.Scene.FriendDialog.Show();
            };

            MentorButton = new MirButton
            {
                Index = 1985,
                HoverIndex = 1986,
                PressedIndex = 1987,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 202),
                Visible = true,
                Hint = string.Format(GameLanguage.Mentor, CMain.InputKeys.GetKey(KeybindOptions.Mentor))
            };
            MentorButton.Click += (o, e) =>
            {
                if (GameScene.Scene.MentorDialog.Visible)
                    GameScene.Scene.MentorDialog.Hide();
                else GameScene.Scene.MentorDialog.Show();
            };


            RelationshipButton = new MirButton  /* lover button */
            {
                Index = 1988,
                HoverIndex = 1989,
                PressedIndex = 1990,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 221),
                Visible = true,
                Hint = string.Format(GameLanguage.Relationship, CMain.InputKeys.GetKey(KeybindOptions.Relationship))
            };
            RelationshipButton.Click += (o, e) =>
            {
                if (GameScene.Scene.RelationshipDialog.Visible)
                    GameScene.Scene.RelationshipDialog.Hide();
                else GameScene.Scene.RelationshipDialog.Show();
            };

            GroupButton = new MirButton
            {
                Index = 1991,
                HoverIndex = 1992,
                PressedIndex = 1993,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 240),
                Hint = string.Format(GameLanguage.Groups, CMain.InputKeys.GetKey(KeybindOptions.Group))
            };
            GroupButton.Click += (o, e) =>
            {
                if (GameScene.Scene.GroupDialog.Visible)
                    GameScene.Scene.GroupDialog.Hide();
                else GameScene.Scene.GroupDialog.Show();
            };

            GuildButton = new MirButton
            {
                Index = 1994,
                HoverIndex = 1995,
                PressedIndex = 1996,
                Parent = this,
                Library = Libraries.Prguse,
                Location = new Point(3, 259),
                Hint = string.Format(GameLanguage.Guild, CMain.InputKeys.GetKey(KeybindOptions.Guilds))
            };
            GuildButton.Click += (o, e) =>
            {
                if (GameScene.Scene.GuildDialog.Visible)
                    GameScene.Scene.GuildDialog.Hide();
                else GameScene.Scene.GuildDialog.Show();
            };

        }


    }
    public sealed class MagicButton : MirControl
    {
        public MirImageControl LevelImage, ExpImage;
        public MirButton SkillButton;
        public MirLabel LevelLabel, NameLabel, ExpLabel, KeyLabel;
        public ClientMagic Magic;
        public MirImageControl CoolDown;

        public MagicButton()
        {
            Size = new Size(231, 33);

            SkillButton = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.MagIcon2,
                Parent = this,
                Location = new Point(36, 0),
                Sound = SoundList.ButtonA,
            };
            SkillButton.Click += (o, e) => new AssignKeyPanel(Magic);

            LevelImage = new MirImageControl
            {
                Index = 516,
                Library = Libraries.Title,
                Location = new Point(73, 7),
                Parent = this,
                NotControl = true,
            };

            ExpImage = new MirImageControl
            {
                Index = 517,
                Library = Libraries.Title,
                Location = new Point(73, 19),
                Parent = this,
                NotControl = true,
            };

            LevelLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(88, 2),
                NotControl = true,
            };

            NameLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(109, 2),
                NotControl = true,
            };

            ExpLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(109, 15),
                NotControl = true,
            };

            KeyLabel = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(2, 2),
                NotControl = true,
            };

            CoolDown = new MirImageControl
            {
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(36, 0),
                Opacity = 0.6F,
                NotControl = true,
                UseOffSet = true,
            };
        }

        public void Update(ClientMagic magic)
        {
            Magic = magic;

            NameLabel.Text = Magic.Name;

            LevelLabel.Text = Magic.Level.ToString();
            switch (Magic.Level)
            {
                case 0:
                    ExpLabel.Text = string.Format("{0}/{1}", Magic.Experience, Magic.Need1);
                    break;
                case 1:
                    ExpLabel.Text = string.Format("{0}/{1}", Magic.Experience, Magic.Need2);
                    break;
                case 2:
                    ExpLabel.Text = string.Format("{0}/{1}", Magic.Experience, Magic.Need3);
                    break;
                case 3:
                    ExpLabel.Text = string.Format("{0}/{1}", Magic.Experience, Magic.Need4);
                    break;
                case 4:
                    ExpLabel.Text = string.Format("{0}/{1}", Magic.Experience, Magic.Need5);
                    break;
                case 5:
                    ExpLabel.Text = string.Format("{0}/{1}", Magic.Experience, Magic.Need6);
                    break;
                case 6:
                    ExpLabel.Text = string.Format("{0}/{1}", Magic.Experience, Magic.Need7);
                    break;
                case 7:
                    ExpLabel.Text = string.Format("{0}/{1}", Magic.Experience, Magic.Need8);
                    break;
                case 8:
                    ExpLabel.Text = string.Format("{0}/{1}", Magic.Experience, Magic.Need9);
                    break;
                case 9:
                    ExpLabel.Text = string.Format("{0}/{1}", Magic.Experience, Magic.Need10);
                    break;
                case 10:
                    ExpLabel.Text = "-";
                    break;
            }

            if (Magic.Key > 8)
            {
                int key = Magic.Key % 8;

                KeyLabel.Text = string.Format("CTRL" + Environment.NewLine + "F{0}", key != 0 ? key : 8);
            }
            else if (Magic.Key > 0)
                KeyLabel.Text = string.Format("F{0}", Magic.Key);
            else
                KeyLabel.Text = string.Empty;

            switch (magic.Spell)
            {  //Warrior
                case Spell.Fencing:
                    SkillButton.Hint = string.Format("Fencing \n\nHitting accuracy will be increased in accordance\nwith practice level.\nPassive Skill\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Slaying:
                    SkillButton.Hint = string.Format("Slaying \n\nHitting accuracy and destructive power will\nbe increased in accordance with practive level.\nPassive Skill\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Thrusting:
                    SkillButton.Hint = string.Format("Thrusting \n\nIncreases the reach of your hits destructive power\nwill increase in accordance with practive level.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.HalfMoon:
                    SkillButton.Hint = string.Format("HalfMoon \n\nCause damage to mobs in a semi circle with\nthe shock waves from your fast moving weapon.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.FlamingSword:
                    SkillButton.Hint = string.Format("FlamingSword \n\nCause additional damage by summoning the spirit\nof fire into weapon\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.ShoulderDash:
                    SkillButton.Hint = string.Format("ShoulderDash \n\nA warrior can push away mobs by charging\nthem with his shoulder, inflicting damage\nif they hit any obstacle.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.CrossHalfMoon:
                    SkillButton.Hint = string.Format("CrossHalfMoon \n\nA warrior uses two powerfull waves of Half Moon\nto inflict damage on all mobs stood next to them.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.TwinDrakeBlade:
                    SkillButton.Hint = string.Format("TwinDrakeBlade \n\nThe art of making multiple power attacks. It has a\nlow chance of stunning the mob temporarly. Stunned\nmobs get 1.5 times more damage inflicted.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.LionRoar:
                    SkillButton.Hint = string.Format("LionRoar \n\nParalyses mobs , duration increases with skill level.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.BladeAvalanche:
                    SkillButton.Hint = string.Format("BladeAvalanche \n\n3-Way Thrusting.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Entrapment:
                    SkillButton.Hint = string.Format("Entrapment \n\nParalyses mobs and draws them to the caster.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Rage:
                    SkillButton.Hint = string.Format("Rage " +
                        "\n \nEnhances your inner force to increase its power" +
                        "\nfor a certain time. Attack power and duration time" +
                        "\nwill depend on the skill level. Once the skill has been used" +
                        "\n you will have to wait to use it again." +
                        "\nSkill Level 0 = Increase DC by 10%, Time = 25 Sec" +
                        "\nSkill Level 1 = Increase DC by 13%, Time = 40 Sec" +
                        "\nSkill Level 2 = Increase DC by 16%, Time = 55 Sec" +
                        "\nSkill Level 3 = Increase DC by 20%, Time = 70 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.ProtectionField:
                    SkillButton.Hint = string.Format("ProtectionField " +
                        "\n \nConcentrates inner force and spreads it to all" +
                        "\n the parts of your body. This will enhance the" +
                        "\nprotection from enemies. Defense power and duration" +
                        "\nwill be depend on the skill level. Once the skill" +
                        "\n has been used, you will have to wait to use it again." +
                        "\nSkill Level 0 = Increase AC by 10%, Time = 25 Sec" +
                        "\nSkill Level 1 = Increase AC by 12%, Time = 40 Sec" +
                        "\nSkill Level 2 = Increase AC by 14%, Time = 55 Sec" +
                        "\nSkill Level 3 = Increase AC by 16%, Time = 70 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.SlashingBurst:
                    SkillButton.Hint = string.Format("SlashingBurst " +
                        "\n \nAllows The Warrior to Jump Over a Obejct or Monster." +
                        "\nSkill Level 0 = Jump 2 Space, CoolDown = 20 Sec" +
                        "\nSkill Level 1 = Jump 3 Space, CoolDown = 18 Sec" +
                        "\nSkill Level 2 = Jump 4 Space, CoolDown = 16 Sec" +
                        "\nSkill Level 3 = Jump 5 Space, CoolDown = 14 Sec" +
                        "\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.ImmortalSkin:
                    SkillButton.Hint = string.Format("ImmortalSkin " +
                        "\n \nSkill Level 0 = Increase AC/AMC by 20%, Deacrease DC by 15%, Time = 10 Sec" +
                        "\nSkill Level 1 = Increase AC/AMC by 30%, Deacrease DC by 20%, Time = 15 Sec" +
                        "\nSkill Level 2 = Increase AC/AMC by 40%, Deacrease DC by 25%, Time = 20 Sec" +
                        "\nSkill Level 3 = Increase AC/AMC by 50%, Deacrease DC by 30%, Time = 25 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Fury:
                    SkillButton.Hint = string.Format("Fury " +
                        "\n \nIncreases the warriors SpeedAttack for a set period of time." +
                        "\nSkill Level 0 = Increase AttackSpeed by 2, Time = 25 Sec" +
                        "\nSkill Level 1 = Increase AttackSpeed by 4, Time = 40 Sec" +
                        "\nSkill Level 2 = Increase AttackSpeed by 6, Time = 55 Sec" +
                        "\nSkill Level 3 = Increase AttackSpeed by 8, Time = 70 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                /*
            case Spell.SliceNDice:
                SkillButton.Hint = string.Format("SliceNDice \n\n Slice though your attacker with three powerfull hits.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.BladeStorm:
                SkillButton.Hint = string.Format("BladeStorm \n\n Slice though your attacker with powerfull wind blades.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.BladeDancer:
                SkillButton.Hint = string.Format("BladeDancer " +
                "\n \nSlice though your attacker with powerfull Fire blades." +
                "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.BlazingSword:
                SkillButton.Hint = string.Format("BlazingSword \n\nCause additional damage by summoning the spirit\nof fire into weapon\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
                */
                //Wizard
                case Spell.FireBall:
                    SkillButton.Hint = string.Format("Fireball \n\nInstant Casting \n\nElements of fire are gathered to form\na fireball. Throw at monsters for damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                /*
            case Spell.DragonFlames:
                SkillButton.Hint = string.Format("DragonFlames \nChanneling Casting \n\nElements of fire are gathered to form\na DragonFlame. Throw at monsters for damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.BrokenSoulCut:
                SkillButton.Hint = string.Format("BrokenSoulCut \nChanneling Casting \n\nElements of fire are gathered to form\na DragonFlame. Throw at monsters for damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.SoulReaper:
                SkillButton.Hint = string.Format("SoulReaper \nChanneling Casting \n\nElements of dark are gathered to form\na Dark Void. Throw at monsters for damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.LastJudgement:
                SkillButton.Hint = string.Format("LastJudgement \nChanneling Casting \n\nElements of fire are gathered to form\na Judgement power. Throw at monsters for damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.ThunderClap:
                SkillButton.Hint = string.Format("ThunderClap \nChanneling Casting \n\nElements of electricity are gathered to form\na Thunder. Throw at monsters for damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.ChopChopStar:
                SkillButton.Hint = string.Format("ChopChopStar \nChanneling Casting \n\nElements of wind are gathered to form\na cutting blade. cast at monsters for damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.SoulEaterSwamp:
                SkillButton.Hint = string.Format("SoulEaterSwamp \nChanneling Casting \n\nElements of earth are gathered to form\na explodin blast. cast at monsters for damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.HandOfGod:
                SkillButton.Hint = string.Format("HandOfGod \nChanneling Casting \n\nElements of fire are gathered to form\na explodin blast. cast at monsters for damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
                */
                case Spell.ThunderBolt:
                    SkillButton.Hint = string.Format("Thundebolt \n\nInstant Casting \n\nStrikes the foe with a lightning bolt \ninflicting high damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.GreatFireBall:
                    SkillButton.Hint = string.Format("GreatFireBall \n\nInstant Casting\n\nStronger then fire ball, Great Fire Ball\nwill fire up the mobs.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Repulsion:
                    SkillButton.Hint = string.Format("Repulsion \n\nInstant Casting\n\nPush away mobs useing the power of fire.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.HellFire:
                    SkillButton.Hint = string.Format("Hellfire \n\nInstant Casting\n\nShoots out a streak of fire attack\nthe monster in front.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Lightning:
                    SkillButton.Hint = string.Format("Lightning \n\nInstant Casting\n\nShoots out a steak of lightning to attack\nthe monster in front.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.ElectricShock:
                    SkillButton.Hint = string.Format("ElectrickShock \n\nInstant Casting\n\nStrong shock wave hits the mob and the\nmob will not be able to move or the mob\nwill get confused and fight for you.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Teleport:
                    SkillButton.Hint = string.Format("Teleport \n\nInstant Casting\n\nTeleport to a random spot.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.FireWall:
                    SkillButton.Hint = string.Format("FireWall " +
                        "\n \nInstant Casting" +
                        "\nThis skill will build a fire wall at a designated" +
                        "\nspot to attack the monster passing the area." +
                        "\nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.FireBang:
                    SkillButton.Hint = string.Format("FireBang \n\nInstant Casting\n\nFirebang will burst out fire at a designated spot to\nburn all the monster within the area.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.ThunderStorm:
                    SkillButton.Hint = string.Format("Thunderstorm \n\nInstant Casting\n\nThis skill will make a thunder storm with in a designated area \nto attack the monster with in.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.MagicShield:
                    SkillButton.Hint = string.Format("MagicShield " +
                        "\n \nInstant Casting" +
                        "\nThis skill will use Mp to create protective" +
                        "\nlayer around you" +
                        "\nAttack will be absorbed by the protective layer" +
                        "\nSkill Level 0 = Increase Shield Absortion by 15%, Time = 30 Sec" +
                        "\nSkill Level 1 = Increase Shield Absortion by 20%, Time = 60 Sec" +
                        "\nSkill Level 2 = Increase Shield Absortion by 25%, Time = 90 Sec" +
                        "\nSkill Level 3 = Increase Shield Absortion by 30%, Time = 120 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.CounterAttack:
                    SkillButton.Hint = string.Format("CounterAttack" +
                        "\nAttack will be absorbed by the protective layer" +
                        "\nSkill Level 0 = Increase Shield Absortion by 10%, Time = 10 Sec" +
                        "\nSkill Level 1 = Increase Shield Absortion by 15%, Time = 12 Sec" +
                        "\nSkill Level 2 = Increase Shield Absortion by 20%, Time = 14 Sec" +
                        "\nSkill Level 3 = Increase Shield Absortion by 25%, Time = 16 Sec" +
                        "\nAnd Reflect to the attacker" +
                        "\nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.WhirlWind:
                    SkillButton.Hint = string.Format("WhirlWind " +
                        "\n \nCause additional damage by summoning the spirit" +
                        "\nof Wind into weapon" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.TurnUndead:
                    SkillButton.Hint = string.Format("TurnUndead \n\nInstant Casting\n\nThis magic will bring birght light into \npower and attack undead monsters\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.IceStorm:
                    SkillButton.Hint = string.Format("IceStorm \n\nInstant Casting\n\nThis skill will make an ice storm with in a designated \narea to attack the monsters with in\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.FlameDisruptor:
                    SkillButton.Hint = string.Format("FlameDisruptor \n\nInstant Casting\n\nFlame from the underground will be brought\ninto surface to attack the mobs.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.FrostCrunch:
                    SkillButton.Hint = string.Format("FrostCrunch \n\nInstant Casting\n\nFreeze the elements in the air around the \nmonster to slow them down\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Mirroring:
                    SkillButton.Hint = string.Format("Mirroring \n\nInstant Casting\n\nCreate a mirror image of yourself to attack\nthe monsters together\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.FlameField:
                    SkillButton.Hint = string.Format("FlameField \n\nInstant Casting\n\nA powerful spell of fire is used to \ndamage surrounding enemies.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Vampirism:
                    SkillButton.Hint = string.Format("Vampirism \n\nInstant Casting\n\nUsing Mp take away monsters Hp to\nincrease your Hp.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Blizzard:
                    SkillButton.Hint = string.Format("Blizzard \n\nConcentrate inner force and spreads it to all\nthe parts of your body.This will enhance the\nprotection from enemies. Defense power and duration\ntime will depend on the skill level. Once the skill\nhas been used, you will have to wait to use it again.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.MeteorStrike:
                    SkillButton.Hint = string.Format("MeteorStrike \n\nInstant Casting\n\nAttacks all monsters within 5x5 square area with lumps \nof fire falling from the sky.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.IceThrust:
                    SkillButton.Hint = string.Format("IceThrust \n\nInstant Casting\n\nAttack monsters by creating an ice pillar.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.MagicBooster:
                    SkillButton.Hint = string.Format("MagicBooster " +
                        "\n \nLasting Effect" +
                        "\nIncrease magical damage, but comsume additional MP." +
                        "\nSkill Level 0 = Increase MC by 8%, Time = 25 Sec" +
                        "\nSkill Level 1 = Increase MC by 12%, Time = 40 Sec" +
                        "\nSkill Level 2 = Increase MC by 16%, Time = 55 Sec" +
                        "\nSkill Level 3 = Increase MC by 20%, Time = 70 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.FastMove:
                    SkillButton.Hint = string.Format("FastMove \n\nLimited Effect\n\nIncrease movemoent with rooted skills.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.StormEscape:
                    SkillButton.Hint = string.Format("StormEscape \n\nLimited Effect\n\nParalyze nearby enemies and teleport to the designated location.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                /*
            case Spell.LavaKing:
                SkillButton.Hint = string.Format("LavaKing \n\nLimited Effect\n\nInstant Casting\n\nAttacks all monsters within 5x5 square area with lumps lava.\nMay cause a burning poison.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.FrozenRains:
                SkillButton.Hint = string.Format("FrozenRains \n\nLimited Effect\n\nInstant Casting\n\nAttacks all monsters within 5x5 square area with Rains.\nMay cause a Frozen poison.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.SummonHolyDragon:
                SkillButton.Hint = string.Format("SummonHolyDragon \n\nInstant Casting\nRequired Items: Amulet\n\nSummon a Deman spirit.This holy spirit will\nuse strong thunder to attack monsters.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.DragNet:
                SkillButton.Hint = string.Format("Dragnet \n\nInstant Casting\n\nCast a electic field on monsters.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
                */
                //Taoist
                case Spell.SpiritSword:
                    SkillButton.Hint = string.Format("SpiritSword \n\nIncreases the chance of hitting the target in\n melee combat.\nPassive Skill\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Healing:
                    SkillButton.Hint = string.Format("Healing \n\nInstant Casting\n\n Heals a single target \nrecovering HP over time.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Poisoning:
                    SkillButton.Hint = string.Format("Poisoning \n\nInstant Casting\nRequired Items: Poison Powder\n\nThrow poison at mobs to weaken them.\nUse green poison to weaken Hp.\nUse red poison to weaken defense.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.SoulFireBall:
                    SkillButton.Hint = string.Format("SoulFireBall \n\nInstant Casting\nRequired Items: Amulet\n\nPut power into a scroll and throw it at \na mob. The scroll will burst into fire.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.SoulShield:
                    SkillButton.Hint = string.Format("SoulShield " +
                        "\n \nInstant Casting" +
                        "\nRequired Items: Amulet" +
                        "\nBless the partymembers to strengthen there magic defence." +
                        "\nSkill Level 0 = Increase AMC by 6%, Time = 10 Min" +
                        "\nSkill Level 1 = Increase AMC by 9%, Time = 15 Min" +
                        "\nSkill Level 2 = Increase AMC by 12%, Time = 20 Min" +
                        "\nSkill Level 3 = Increase AMC by 15%, Time = 25 Min" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.BlessedArmour:
                    SkillButton.Hint = string.Format("BlessedArmour " +
                        "\n \nInstant Casting\nRequired Items: Amulet" +
                        "\nBless the partymemebers to strenghten there defence." +
                        "\nSkill Level 0 = Increase AC by 6%, Time = 10 Min" +
                        "\nSkill Level 1 = Increase AC by 9%, Time = 15 Min" +
                        "\nSkill Level 2 = Increase AC by 12%, Time = 20 Min" +
                        "\nSkill Level 3 = Increase AC by 15%, Time = 25 Min" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.TrapHexagon:
                    SkillButton.Hint = string.Format("TrapHexagon \n\nInstant Casting\nRequired Items: Amulet\n\nTrap the monster with this magical power\n to stop them from moving. Any damages\nfrom outside source will allow the monsters\nto move again.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.SummonSkeleton:
                    SkillButton.Hint = string.Format("SummonSkeleton \n\nInstant Casting\nSummons a Powerful AOE Skeleton, Which will Fight Side By Side With You\nRequired Items: Amulet.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Hiding:
                    SkillButton.Hint = string.Format("Hiding \n\nInstant Casting\nRequired Items: Amulet\n\nMobs will not be able to spot you for a short\nmoment.Mobs will notice you if you start\nto move around.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.MassHiding:
                    SkillButton.Hint = string.Format("MassHiding \n\nInstant Casting\nRequired Items: Amulet\n\nMobs will not be able to spot you or your \nparty members for a short moment. \nMobs will notice you and your party if \nyou start to move around.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Revelation:
                    SkillButton.Hint = string.Format("Revelation \n\nInstant Casting\n\nYou will be able to read Hp of others\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.MassHealing:
                    SkillButton.Hint = string.Format("MassHealing \n\nInstant Casting\n\nHeal all injured players in the specified\narea by surrounding them with mana.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.SummonShinsu:
                    SkillButton.Hint = string.Format("SummonShinsu \n\nInstant Casting\nSummons a Dog, That Will fight Side By Side with you.\nRequired Items: Amulet.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.UltimateEnhancer:
                    SkillButton.Hint = string.Format("UltimateEnhancer " +
                        "\n \nInstant Casting\nRequired Items: Amulet" +
                        "\nBless the partymembers to strengthen there damage." +
                        "\nSkill Level 0 = Increase DC/MC/SC by 8%, Time = 10 Min" +
                        "\nSkill Level 1 = Increase DC/MC/SC by 12%, Time = 15 Min" +
                        "\nSkill Level 2 = Increase DC/MC/SC by 16%, Time = 20 Min" +
                        "\nSkill Level 3 = Increase DC/MC/SC by 20%, Time = 25 Min" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.EnergyRepulsor:
                    SkillButton.Hint = string.Format("EnergyRepulsor \n\nInstant Casting\n\nConcentrate your energy for one big blast to push away the monsters around you.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Purification:
                    SkillButton.Hint = string.Format("Purification \n\nInstant Casting\n\nHelp others to recover from poisoning and\nparalysis useing this skill.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.SummonHolyDeva:
                    SkillButton.Hint = string.Format("SummonHolyDeva \n\nInstant Casting\nRequired Items: Amulet\n\nSummon a holy spirit.This holy spirit will\nuse strong thunder to attack monsters.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Curse:
                    SkillButton.Hint = string.Format("Curse " +
                    "\n \nInstant Casting" +
                    "\nRequired Items: Amulet" +
                    "\nSkill Level 0 = Decreases stats by 5%, MobTime = 15 Sec, PlayerTime = 3 Sec" +
                    "\nSkill Level 1 = Decreases stats by 10%, MobTime = 20 Sec, PlayerTime = 5 Sec" +
                    "\nSkill Level 2 = Decreases stats by 15%, MobTime = 25 Sec, PlayerTime = 7 Sec" +
                    "\nSkill Level 3 = Decreases stats by 20%, MobTime = 30 Sec, PlayerTime = 9 Sec" +
                    "\nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Hallucination:
                    SkillButton.Hint = string.Format("Hallucination \n\nInstant Casting\nRequired Items: Amulet\n\nThe monster will only see hallucination \nand attack anyone on the way\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Reincarnation:
                    SkillButton.Hint = string.Format("Reincarnation \n\nInstant Casting\nRequired Items: Amulet\n\nRevives a dead players\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.PoisonCloud:
                    SkillButton.Hint = string.Format("PoisonCloud \n\nInstant Casting\nRequired Items: GreenPoison\n\nThrow the amulet and a very strong\npoison cloud will appear in the area.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.EnergyShield:
                    SkillButton.Hint = string.Format("EnergyShield \n\nInstant Casting\nRequired Items: Amulet\n\nCreate an enegy shield to heal immediately when attacked by monsters.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Plague:
                    SkillButton.Hint = string.Format("Plague " +
                        "\n \nInstant Casting" +
                        "\nRequired Items: Amulet" +
                        "\nDecreases targets MP and inflict target with various debuffs" +
                        "\nExample: Stun , Curse , Poison and Slow." +
                        "\nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.HealingCircle:
                    SkillButton.Hint = string.Format("HealingCircle \n\nInstant Casting\nTreatment area friendly target, and the enemy caused spell damage.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.PetEnhancer:
                    SkillButton.Hint = string.Format("PetEnhancer " +
                        "\n \nInstant Casting" +
                        "\nStrengthening pets defense and power." +
                        "\nSkill Level 0 = Increase (PET) stats by 20%, Time = 30 Sec" +
                        "\nSkill Level 1 = Increase (PET) stats by 35%, Time = 60 Sec" +
                        "\nSkill Level 2 = Increase (PET) stats by 50%, Time = 90 Sec" +
                        "\nSkill Level 3 = Increase (PET) stats by 65%, Time = 120 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    //SkillButton.Hint = string.Format("PetEnhancer \nInstant Casting\nIncrease stats 70% 80% 90% 100% from ur stats.\nStrengthening pets defense and power.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                /*
            case Spell.HeadShot:
                SkillButton.Hint = string.Format("HeadShot \n\nInstant Casting\nRequired Items: Amulet\n\nPut power into a scroll and throw it at \na mob. The scroll will burst into ice.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.HolyShield:
                SkillButton.Hint = string.Format("HolyShield " +
                    "\n \nInstant Casting\nRequired Items: Amulet" +
                    "\nBless the partymembers to strengthen there magic defence." +
                    "\nSkill Level 0 = Increase AC/AMC by 6%, Time = 20 Min" +
                    "\nSkill Level 1 = Increase AC/AMC by 9%, Time = 30 Min" +
                    "\nSkill Level 2 = Increase AC/AMC by 12%, Time = 40 Min" +
                    "\nSkill Level 3 = Increase AC/AMC by 15%, Time = 50 Min" +
                    "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
                */
                //Assassin
                case Spell.FatalSword:
                    SkillButton.Hint = string.Format("FatalSword \n\nIncrease attack damage on the monsters.\nalso increases accuracy a little.\nPassive Skill\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.DoubleSlash:
                    SkillButton.Hint = string.Format("DoubleSlash \n\nSlash the monster twice in a quick motion\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Haste:
                    SkillButton.Hint = string.Format("Haste " +
                        "\n \nIncrease the A.Speed" +
                        "\nSkill Level 0 = Increase A.Speed by 2, Time = 25 Sec" +
                        "\nSkill Level 1 = Increase A.Speed by 4, Time = 40 Sec" +
                        "\nSkill Level 2 = Increase A.Speed by 6, Time = 55 Sec" +
                        "\nSkill Level 3 = Increase A.Speed by 8, Time = 70 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? 0 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.FlashDash:
                    SkillButton.Hint = string.Format("FlashDash \n\nAttack a monster with quick slash and\nparalize the monster\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.HeavenlySword:
                    SkillButton.Hint = string.Format("HeavenlySword " +
                        "\n \nAttack monsters with in 2 steps radius" +
                    "\nSkill Level 0 = Increase Range Attack by 2" +
                    "\nSkill Level 1 = Increase Range Attack by 3" +
                    "\nSkill Level 2 = Increase Range Attack by 4" +
                    "\nSkill Level 3 = Increase Range Attack by 5" +
                        "\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.FireBurst:
                    SkillButton.Hint = string.Format("FireBurst \n\nPush away mobs surrounding you\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Trap:
                    SkillButton.Hint = string.Format("Trap \nInstant casting CoolTime 60 secs\n\nTrap the monster for a short while.\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.MoonLight:
                    SkillButton.Hint = string.Format("Moonlight " +
                        "\n \nHide yourself from monster by turning invisible" +
                        "\nGreater damage is done when you attack monster using this skill." +
                        "\nSkill Level 0 = Increase Invisible, Time = 10 Sec" +
                        "\nSkill Level 1 = Increase Invisible, Time = 15 Sec" +
                        "\nSkill Level 2 = Increase Invisible, Time = 20 Sec" +
                        "\nSkill Level 3 = Increase Invisible, Time = 25 Sec" +
                        "\nSneak Radius" +
                        "\nSkill Level 0 = Radius 4 Step" +
                        "\nSkill Level 1 = Radius 3 Step" +
                        "\nSkill Level 2 = Radius 2 Step" +
                        "\nSkill Level 3 = Radius 1 Step" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.MPEater:
                    SkillButton.Hint = string.Format("MpEater \nPassive\nAbsord monsters MP to recharge your MP\nCurrent Skill Level {0}\nNext Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.SwiftFeet:
                    SkillButton.Hint = string.Format("SwiftFeet " +
                        "\n \nIncreased Runing Speed" +
                        "\nSkill Level 0 = Increase Runing Speed, Time = 10 Sec" +
                        "\nSkill Level 1 = Increase Runing Speed, Time = 15 Sec" +
                        "\nSkill Level 2 = Increase Runing Speed, Time = 20 Sec" +
                        "\nSkill Level 3 = Increase Runing Speed, Time = 25 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? 0 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.LightBody:
                    SkillButton.Hint = string.Format("LightBody " +
                        "\n \nIncrease the Agility" +
                        "\nSkill Level 0 = Increase Agility by 2, Time = 25 Sec" +
                        "\nSkill Level 1 = Increase Agility by 4, Time = 40 Sec" +
                        "\nSkill Level 2 = Increase Agility by 6, Time = 55 Sec" +
                        "\nSkill Level 3 = Increase Agility by 8, Time = 70 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? 0 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.PoisonSword:
                    SkillButton.Hint = string.Format("PoisonSword " +
                        "\n \nPoison the monsters with a slash of you" +
                        "\nsword.Poison effect will damage the monster over time." +
                        "\nSkill Level 0 = Apply Target Poison, Time = 10 Sec" +
                        "\nSkill Level 1 = Apply Target Poison, Time = 15 Sec" +
                        "\nSkill Level 2 = Apply Target Poison, Time = 20 Sec" +
                        "\nSkill Level 3 = Apply Target Poison, Time = 25 Sec" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? 0 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.DarkBody:
                    SkillButton.Hint = string.Format("DarkBody " +
                        "\n \nCreate an illusion of yourself to attack" +
                        "\nthe monster while you become invisible." +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? 0 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.CrescentSlash:
                    SkillButton.Hint = string.Format("CrescentSlash " +
                        "\n \nBurst out of the power of your sword and attack all monsters around you." +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? 0 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.Hemorrhage:
                    SkillButton.Hint = string.Format("Hemorrhage " +
                        "\n \nPassive\nChance to deal cristical damage and inflict bleeding damage." +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? 0 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                case Spell.MoonMist:
                    SkillButton.Hint = string.Format("Moon Mist" +
                        "\nActive" +
                        "\nAbility to hide your self from Monster" +
                        "\nYour first attack will be stronger than normal." +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? 0 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;
                /*
            case Spell.FuryWaves:
                SkillButton.Hint = string.Format("FuryWaves " +
                    "\n \nBurst out of the power of your sword and attack all monsters around you." +
                    "\nThis could push or poison. " +
                    "\nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
            case Spell.ShadowStep:
                SkillButton.Hint = string.Format("ShadowStep " +
                    "\n \nIncreased Shadow Jump" +
                    "\nSkill Level 0 = CoolDown - 60 Sec" +
                    "\nSkill Level 1 = CoolDown - 50 Sec" +
                    "\nSkill Level 2 = CoolDown - 40 Sec" +
                    "\nSkill Level 3 = CoolDown - 30 Sec" +
                    "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                break;
                */
                case Spell.MentalState:
                    SkillButton.Hint = string.Format("MentalState " +
                        "\n \n         Protective" +
                        "\nIncrease AC/AMC by 10%, Deacrease DC by 10%" +
                        "\n \n         Aggresive" +
                        "\nDecrease AC/AMC by 10%, Increase DC by 10%" +
                        "\n \n         Regenerative" +
                        "\nIncrease HP/MPRegen by 5% from MaxHP/MP" +
                        "\n \n         CoolDown" +
                        "\nSkill Level 0 = CoolDown - 60 Min" +
                        "\nSkill Level 1 = CoolDown - 45 Min" +
                        "\nSkill Level 2 = CoolDown - 30 Min" +
                        "\nSkill Level 3 = CoolDown - 15 Min" +
                        "\n \nCurrent Skill Level {0}, Next Level {1}", Magic.Level, Magic.Level == 0 ? Magic.Level1 : Magic.Level == 1 ? Magic.Level2 : Magic.Level == 2 ? Magic.Level3 : Magic.Level == 3 ? Magic.Level4 : Magic.Level == 4 ? Magic.Level5 : Magic.Level == 5 ? Magic.Level6 : Magic.Level == 6 ? Magic.Level7 : Magic.Level == 7 ? Magic.Level8 : Magic.Level == 8 ? Magic.Level9 : Magic.Level == 9 ? Magic.Level10 : 0);
                    break;

                default:

                    break;
            }

            SkillButton.Index = Magic.Icon * 2;
            SkillButton.PressedIndex = Magic.Icon * 2 + 1;

            SetDelay();
        }

        public void SetDelay()
        {
            if (Magic == null) return;

            int totalFrames = 34;

            long timeLeft = Magic.CastTime + Magic.Delay - CMain.Time;

            if (timeLeft < 100)
            {
                CoolDown.Visible = false;
                return;
            }

            int delayPerFrame = (int)(Magic.Delay / totalFrames);
            int startFrame = totalFrames - (int)(timeLeft / delayPerFrame);

            if ((CMain.Time <= Magic.CastTime + Magic.Delay))
            {
                CoolDown.Visible = true;
                CoolDown.Index = 1290 + startFrame;
            }
        }
    }
    public sealed class AssignKeyPanel : MirImageControl
    {
        public MirButton SaveButton, NoneButton;

        public MirLabel TitleLabel;
        public MirImageControl MagicImage;
        public MirButton[] FKeys;

        public ClientMagic Magic;
        public byte Key;

        public AssignKeyPanel(ClientMagic magic)
        {
            Magic = magic;
            Key = magic.Key;

            Modal = true;
            Index = 710;
            Library = Libraries.Prguse;
            Location = Center;
            Parent = GameScene.Scene;
            Visible = true;

            MagicImage = new MirImageControl
            {
                Location = new Point(16, 16),
                Index = magic.Icon * 2,
                Library = Libraries.MagIcon2,
                Parent = this,
            };

            TitleLabel = new MirLabel
            {
                Location = new Point(49, 17),
                Parent = this,
                Size = new Size(230, 32),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.WordBreak,
                Text = string.Format(GameLanguage.SelectKey, magic.Name)
            };

            NoneButton = new MirButton
            {
                Index = 287, //154
                HoverIndex = 288,
                PressedIndex = 289,
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(284, 64),
            };
            NoneButton.Click += (o, e) => Key = 0;

            SaveButton = new MirButton
            {
                Library = Libraries.Title,
                Parent = this,
                Location = new Point(284, 101),
                Index = 156,
                HoverIndex = 157,
                PressedIndex = 158,
            };
            SaveButton.Click += (o, e) =>
            {
                for (int i = 0; i < MapObject.User.Magics.Count; i++)
                {
                    if (MapObject.User.Magics[i].Key == Key)
                        MapObject.User.Magics[i].Key = 0;
                }

                Network.Enqueue(new C.MagicKey { Spell = Magic.Spell, Key = Key });
                Magic.Key = Key;
                foreach (SkillBarDialog Bar in GameScene.Scene.SkillBarDialogs)
                    Bar.Update();

                Dispose();
            };


            FKeys = new MirButton[16];

            FKeys[0] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(17, 58),
                Sound = SoundList.ButtonA,
                Text = "F1"
            };
            FKeys[0].Click += (o, e) => Key = 1;

            FKeys[1] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(49, 58),
                Sound = SoundList.ButtonA,
                Text = "F2"
            };
            FKeys[1].Click += (o, e) => Key = 2;

            FKeys[2] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(81, 58),
                Sound = SoundList.ButtonA,
                Text = "F3"
            };
            FKeys[2].Click += (o, e) => Key = 3;

            FKeys[3] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(113, 58),
                Sound = SoundList.ButtonA,
                Text = "F4"
            };
            FKeys[3].Click += (o, e) => Key = 4;

            FKeys[4] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(150, 58),
                Sound = SoundList.ButtonA,
                Text = "F5"
            };
            FKeys[4].Click += (o, e) => Key = 5;

            FKeys[5] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(182, 58),
                Sound = SoundList.ButtonA,
                Text = "F6",
            };
            FKeys[5].Click += (o, e) => Key = 6;

            FKeys[6] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(214, 58),
                Sound = SoundList.ButtonA,
                Text = "F7"
            };
            FKeys[6].Click += (o, e) => Key = 7;

            FKeys[7] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(246, 58),
                Sound = SoundList.ButtonA,
                Text = "F8"
            };
            FKeys[7].Click += (o, e) => Key = 8;


            FKeys[8] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(17, 95),
                Sound = SoundList.ButtonA,
                Text = "Ctrl" + Environment.NewLine + "F1"
            };
            FKeys[8].Click += (o, e) => Key = 9;

            FKeys[9] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(49, 95),
                Sound = SoundList.ButtonA,
                Text = "Ctrl" + Environment.NewLine + "F2"
            };
            FKeys[9].Click += (o, e) => Key = 10;

            FKeys[10] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(81, 95),
                Sound = SoundList.ButtonA,
                Text = "Ctrl" + Environment.NewLine + "F3"
            };
            FKeys[10].Click += (o, e) => Key = 11;

            FKeys[11] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(113, 95),
                Sound = SoundList.ButtonA,
                Text = "Ctrl" + Environment.NewLine + "F4"
            };
            FKeys[11].Click += (o, e) => Key = 12;

            FKeys[12] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(150, 95),
                Sound = SoundList.ButtonA,
                Text = "Ctrl" + Environment.NewLine + "F5"
            };
            FKeys[12].Click += (o, e) => Key = 13;

            FKeys[13] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(182, 95),
                Sound = SoundList.ButtonA,
                Text = "Ctrl" + Environment.NewLine + "F6"
            };
            FKeys[13].Click += (o, e) => Key = 14;

            FKeys[14] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(214, 95),
                Sound = SoundList.ButtonA,
                Text = "Ctrl" + Environment.NewLine + "F7"
            };
            FKeys[14].Click += (o, e) => Key = 15;

            FKeys[15] = new MirButton
            {
                Index = 0,
                PressedIndex = 1,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(246, 95),
                Sound = SoundList.ButtonA,
                Text = "Ctrl" + Environment.NewLine + "F8"
            };
            FKeys[15].Click += (o, e) => Key = 16;

            BeforeDraw += AssignKeyPanel_BeforeDraw;
        }

        private void AssignKeyPanel_BeforeDraw(object sender, EventArgs e)
        {
            for (int i = 0; i < FKeys.Length; i++)
            {
                FKeys[i].Index = 1656;
                FKeys[i].HoverIndex = 1657;
                FKeys[i].PressedIndex = 1658;
                FKeys[i].Visible = true;
            }

            if (Key == 0 || Key > FKeys.Length) return;

            FKeys[Key - 1].Index = 1658;
            FKeys[Key - 1].HoverIndex = 1658;
            FKeys[Key - 1].PressedIndex = 1658;
        }
    }
    public sealed class BigMapDialog : MirControl
    {
	float ScaleX;
        float ScaleY;
	
        int BigMap_MouseCoordsProcessing_OffsetX, BigMap_MouseCoordsProcessing_OffsetY;
            
        public BigMapDialog()
        {
            NotControl = false;
            Location = new Point(130, 100);
            //Border = true;
            //BorderColour = Color.Lime;
            BeforeDraw += (o, e) => OnBeforeDraw();
            Sort = true;
	    
            MouseMove += UpdateBigMapCoordinates;
        }

	private void UpdateBigMapCoordinates(object sender, MouseEventArgs e)
        {
            int MouseCoordsOnBigMap_MapValue_X = (int)((e.Location.X - BigMap_MouseCoordsProcessing_OffsetX) / ScaleX);
            int MouseCoordsOnBigMap_MapValue_Y = (int)((e.Location.Y - BigMap_MouseCoordsProcessing_OffsetY) / ScaleY);
	    
            this.Hint = string.Format("{0},{1}", MouseCoordsOnBigMap_MapValue_X, MouseCoordsOnBigMap_MapValue_Y);
        }

        private void OnBeforeDraw()
        {
            MapControl map = GameScene.Scene.MapControl;
            if (map == null || !Visible) return;

            //int index = map.BigMap <= 0 ? map.MiniMap : map.BigMap;
            int index = map.BigMap;

            if (index <= 0)
            {
                if (Visible)
                {
                    Visible = false;
                }
                return;
            }

            TrySort();

            Rectangle viewRect = new Rectangle(0, 0, 600, 400);

            Size = Libraries.MiniMap.GetSize(index);

            if (Size.Width < 600)
                viewRect.Width = Size.Width;

            if (Size.Height < 400)
                viewRect.Height = Size.Height;

            viewRect.X = (Settings.ScreenWidth - viewRect.Width) / 2;
            viewRect.Y = (Settings.ScreenHeight - 120 - viewRect.Height) / 2;

	    BigMap_MouseCoordsProcessing_OffsetX = viewRect.X;
            BigMap_MouseCoordsProcessing_OffsetY = viewRect.Y;

            Location = viewRect.Location;
            Size = viewRect.Size;

            ScaleX = Size.Width / (float)map.Width;
            ScaleY = Size.Height / (float)map.Height;

            viewRect.Location = new Point(
                (int)(ScaleX * MapObject.User.CurrentLocation.X) - viewRect.Width / 2,
                (int)(ScaleY * MapObject.User.CurrentLocation.Y) - viewRect.Height / 2);

            if (viewRect.Right >= Size.Width)
                viewRect.X = Size.Width - viewRect.Width;
            if (viewRect.Bottom >= Size.Height)
                viewRect.Y = Size.Height - viewRect.Height;

            if (viewRect.X < 0) viewRect.X = 0;
            if (viewRect.Y < 0) viewRect.Y = 0;

            Libraries.MiniMap.Draw(index, Location, Size, Color.FromArgb(255, 255, 255));

            int startPointX = (int)(viewRect.X / ScaleX);
            int startPointY = (int)(viewRect.Y / ScaleY);

            for (int i = MapControl.Objects.Count - 1; i >= 0; i--)
            {
                MapObject ob = MapControl.Objects[i];


                if (ob.Race == ObjectType.Item || ob.Dead || ob.Race == ObjectType.Spell) continue; // || (ob.ObjectID != MapObject.User.ObjectID)
                float x = ((ob.CurrentLocation.X - startPointX) * ScaleX) + Location.X;
                float y = ((ob.CurrentLocation.Y - startPointY) * ScaleY) + Location.Y;

                Color colour;

                if ((GroupDialog.GroupList.Contains(ob.Name) && MapObject.User != ob) || ob.Name.EndsWith(string.Format("({0})", MapObject.User.Name)))
                    colour = Color.FromArgb(0, 0, 255);
                else
                    if (ob is PlayerObject)
                    colour = Color.FromArgb(255, 255, 255);
                else if (ob is NPCObject || ob.AI == 6)
                    colour = Color.FromArgb(0, 255, 50);
                else
                    colour = Color.FromArgb(255, 0, 0);

                DXManager.Sprite.Draw(DXManager.RadarTexture, new Rectangle(0, 0, 2, 2), Vector3.Zero, new Vector3((float)(x - 0.5), (float)(y - 0.5), 0.0F), colour);
            }
        }


        public void Toggle()
        {
            Visible = !Visible;

            Redraw();
        }
    }
    public sealed class DuraStatusDialog : MirImageControl
    {
        public MirButton Character;

        public DuraStatusDialog()
        {
            Size = new Size(40, 19);
            Location = new Point((GameScene.Scene.MiniMapDialog.Location.X + 86), GameScene.Scene.MiniMapDialog.Size.Height);

            Character = new MirButton()
            {
                Index = 2113,
                Library = Libraries.Prguse,
                Parent = this,
                Size = new Size(20, 19),
                Location = new Point(20, 0),
                HoverIndex = 2111,
                PressedIndex = 2112,
                Sound = SoundList.ButtonA,
                Hint = GameLanguage.DuraPanel
            };
            Character.Click += (o, e) =>
            {
                if (GameScene.Scene.CharacterDuraPanel.Visible == true)
                {
                    GameScene.Scene.CharacterDuraPanel.Hide();
                    Settings.DuraView = false;
                }
                else
                {
                    GameScene.Scene.CharacterDuraPanel.Show();
                    Settings.DuraView = true;
                }
            };
        }

    }
    public sealed class CharacterDuraPanel : MirImageControl
    {
        public MirImageControl GrayBackground, Background, Helmet, Armour, Belt, Boots, Weapon, Necklace, RightBracelet, LeftBracelet, RightRing, LeftRing, Torch, Stone, Amulet, Mount, Item1, Item2;

        public CharacterDuraPanel()
        {
            Index = 2105;
            Library = Libraries.Prguse;
            Movable = false;
            Location = new Point(Settings.ScreenWidth - 61, 200);

            GrayBackground = new MirImageControl()
            {
                Index = 2161,
                Library = Libraries.Prguse,
                Parent = this,
                Size = new Size(56, 80),
                Location = new Point(3, 3),
                Opacity = 0.4F
            };
            Background = new MirImageControl()
            {
                Index = 2162,
                Library = Libraries.Prguse,
                Parent = this,
                Size = new Size(56, 80),
                Location = new Point(3, 3),
            };

            #region Pieces

            Helmet = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 12), Location = new Point(24, 3) };
            Belt = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 7), Location = new Point(23, 23) };
            Armour = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(28, 32), Location = new Point(16, 11) };
            Boots = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(24, 9), Location = new Point(17, 43) };
            Weapon = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 33), Location = new Point(4, 5) };
            Necklace = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 12), Location = new Point(3, 67) };
            LeftBracelet = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 8), Location = new Point(3, 43) };
            RightBracelet = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 8), Location = new Point(43, 43) };
            LeftRing = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 12), Location = new Point(3, 54) };
            RightRing = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 12), Location = new Point(43, 54) };
            Torch = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(8, 32), Location = new Point(44, 5) };
            Stone = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 12), Location = new Point(30, 54) };
            Amulet = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 12), Location = new Point(16, 54) };
            Mount = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(12, 12), Location = new Point(43, 68) };
            Item1 = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(8, 12), Location = new Point(19, 67) };
            Item2 = new MirImageControl() { Index = -1, Library = Libraries.Prguse, Parent = Background, Size = new Size(8, 12), Location = new Point(31, 67) };

            #endregion
        }

        public void GetCharacterDura()
        {
            if (GameScene.Scene.CharacterDialog.Grid[0].Item == null) { Weapon.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[1].Item == null) { Armour.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[2].Item == null) { Helmet.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[3].Item == null) { Torch.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[4].Item == null) { Necklace.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[5].Item == null) { LeftBracelet.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[6].Item == null) { RightBracelet.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[7].Item == null) { LeftRing.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[8].Item == null) { RightRing.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[9].Item == null) { Amulet.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[10].Item == null) { Belt.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[11].Item == null) { Boots.Index = -1; }
            if (GameScene.Scene.CharacterDialog.Grid[12].Item == null) { Stone.Index = -1; }

            for (int i = 0; i < MapObject.User.Equipment.Length; i++)
            {
                if (MapObject.User.Equipment[i] == null) continue;
                UpdateCharacterDura(MapObject.User.Equipment[i]);
            }
        }
        public void UpdateCharacterDura(UserItem item)
        {
            int Warning = item.MaxDura / 2;
            int Danger = item.MaxDura / 5;
            ushort AmuletWarning = (ushort)(item.Info.StackSize / 2);
            ushort AmuletDanger = (ushort)(item.Info.StackSize / 5);

            switch (item.Info.Type)
            {
                case ItemType.Amulet: //Based on stacks of 5000
                    if (item.Count > AmuletWarning)
                        Amulet.Index = 2134;
                    if (item.Count <= AmuletWarning)
                        Amulet.Index = 2135;
                    if (item.Count <= AmuletDanger)
                        Amulet.Index = 2136;
                    if (item.Count == 0)
                        Amulet.Index = -1;
                    break;
                case ItemType.Armour:
                    if (item.CurrentDura > Warning)
                        Armour.Index = 2149;
                    if (item.CurrentDura <= Warning)
                        Armour.Index = 2150;
                    if (item.CurrentDura <= Danger)
                        Armour.Index = 2151;
                    if (item.CurrentDura == 0)
                        Armour.Index = -1;
                    break;
                case ItemType.Belt:
                    if (item.CurrentDura > Warning)
                        Belt.Index = 2158;
                    if (item.CurrentDura <= Warning)
                        Belt.Index = 2159;
                    if (item.CurrentDura <= Danger)
                        Belt.Index = 2160;
                    if (item.CurrentDura == 0)
                        Belt.Index = -1;
                    break;
                case ItemType.Boots:
                    if (item.CurrentDura > Warning)
                        Boots.Index = 2152;
                    if (item.CurrentDura <= Warning)
                        Boots.Index = 2153;
                    if (item.CurrentDura <= Danger)
                        Boots.Index = 2154;
                    if (item.CurrentDura == 0)
                        Boots.Index = -1;
                    break;
                case ItemType.Bracelet:
                    if (GameScene.Scene.CharacterDialog.Grid[(byte)EquipmentSlot.BraceletR].Item != null && item.UniqueID == GameScene.Scene.CharacterDialog.Grid[(byte)EquipmentSlot.BraceletR].Item.UniqueID)
                    {
                        if (item.CurrentDura > Warning)
                            RightBracelet.Index = 2143;
                        if (item.CurrentDura <= Warning)
                            RightBracelet.Index = 2144;
                        if (item.CurrentDura <= Danger)
                            RightBracelet.Index = 2145;
                        if (item.CurrentDura == 0)
                            RightBracelet.Index = -1;
                    }
                    else if (GameScene.Scene.CharacterDialog.Grid[(byte)EquipmentSlot.BraceletL].Item != null && item.UniqueID == GameScene.Scene.CharacterDialog.Grid[(byte)EquipmentSlot.BraceletL].Item.UniqueID)
                    {
                        if (item.CurrentDura > Warning)
                            LeftBracelet.Index = 2143;
                        if (item.CurrentDura <= Warning)
                            LeftBracelet.Index = 2144;
                        if (item.CurrentDura <= Danger)
                            LeftBracelet.Index = 2145;
                        if (item.CurrentDura == 0)
                            LeftBracelet.Index = -1;
                    }
                    break;
                case ItemType.Helmet:
                    if (item.CurrentDura > Warning)
                        Helmet.Index = 2155;
                    if (item.CurrentDura <= Warning)
                        Helmet.Index = 2156;
                    if (item.CurrentDura <= Danger)
                        Helmet.Index = 2157;
                    if (item.CurrentDura == 0)
                        Helmet.Index = -1;
                    break;
                case ItemType.Necklace:
                    if (item.CurrentDura > Warning)
                        Necklace.Index = 2122;
                    if (item.CurrentDura <= Warning)
                        Necklace.Index = 2123;
                    if (item.CurrentDura <= Danger)
                        Necklace.Index = 2124;
                    if (item.CurrentDura == 0)
                        Necklace.Index = -1;
                    break;
                case ItemType.Ring:
                    if (GameScene.Scene.CharacterDialog.Grid[(byte)EquipmentSlot.RingR].Item != null && item.UniqueID == GameScene.Scene.CharacterDialog.Grid[(byte)EquipmentSlot.RingR].Item.UniqueID)
                    {
                        if (item.CurrentDura > Warning)
                            RightRing.Index = 2131;
                        if (item.CurrentDura <= Warning)
                            RightRing.Index = 2132;
                        if (item.CurrentDura <= Danger)
                            RightRing.Index = 2133;
                        if (item.CurrentDura == 0)
                            RightRing.Index = -1;
                    }
                    else if (GameScene.Scene.CharacterDialog.Grid[(byte)EquipmentSlot.RingL].Item != null && item.UniqueID == GameScene.Scene.CharacterDialog.Grid[(byte)EquipmentSlot.RingL].Item.UniqueID)
                    {
                        if (item.CurrentDura > Warning)
                            LeftRing.Index = 2131;
                        if (item.CurrentDura <= Warning)
                            LeftRing.Index = 2132;
                        if (item.CurrentDura <= Danger)
                            LeftRing.Index = 2133;
                        if (item.CurrentDura == 0)
                            LeftRing.Index = -1;
                    }
                    break;
                case ItemType.Stone:
                    if (item.CurrentDura == 0)
                        Stone.Index = 2137;
                    break;
                case ItemType.Mount:
                    if (item.CurrentDura > Warning)
                        Mount.Index = 2140;
                    if (item.CurrentDura <= Warning)
                        Mount.Index = 2141;
                    if (item.CurrentDura <= Danger)
                        Mount.Index = 2142;
                    if (item.CurrentDura == 0)
                        Mount.Index = -1;
                    break;
                case ItemType.Torch:
                    if (item.CurrentDura > Warning)
                        Torch.Index = 2146;
                    if (item.CurrentDura <= Warning)
                        Torch.Index = 2147;
                    if (item.CurrentDura <= Danger)
                        Torch.Index = 2148;
                    if (item.CurrentDura == 0)
                        Torch.Index = -1;
                    break;
                case ItemType.Weapon:
                    if (item.CurrentDura > Warning)
                        Weapon.Index = 2125;
                    if (item.CurrentDura <= Warning)
                        Weapon.Index = 2126;
                    if (item.CurrentDura <= Danger)
                        Weapon.Index = 2127;
                    if (item.CurrentDura == 0)
                        Weapon.Index = -1;
                    break;
            }
        }

        public override void Hide()
        {
            if (!Visible) return;
            Visible = false;
            GameScene.Scene.DuraStatusPanel.Character.Index = 2113;
        }
        public override void Show()
        {
            if (Visible) return;
            Visible = true;
            GameScene.Scene.DuraStatusPanel.Character.Index = 2110;

            GetCharacterDura();
        }
    }

    public sealed class CustomPanel1 : MirImageControl
    {
        public MirButton Button1, Button2, Button3;

        public string AMode, PMode, SMode;

        public CustomPanel1(MirControl parent)
        {
            Index = 2179;
            Library = Libraries.Prguse;
            Size = new Size(24, 61);
            Parent = parent;

            Location = new Point(((Settings.ScreenWidth / 2) - (Size.Width / 2)) + 362, Settings.ScreenHeight - Size.Height - 77);

            Button1 = new MirButton //Skill
            {
                Index = 2173,
                HoverIndex = 2174,
                PressedIndex = 2175,
                Library = Libraries.Prguse,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(3, 3),
                Hint = "SkillMode"
            };
            Button1.Click += (o, e) =>
            {
                GameScene.Scene.ChangeSkillMode(null);
            };

            Button2 = new MirButton //Pet
            {
                Index = 2170,
                HoverIndex = 2171,
                PressedIndex = 2172,
                Library = Libraries.Prguse,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(3, 20),
                Hint = "PetMode"
            };
            Button2.Click += (o, e) =>
            {
                GameScene.Scene.ChangePetMode();
            };

            Button3 = new MirButton //Player
            {
                Index = 2176,
                HoverIndex = 2177,
                PressedIndex = 2178,
                Library = Libraries.Prguse,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(3, 37),
                Hint = "AttackMode"
            };
            Button3.Click += (o, e) =>
            {
                GameScene.Scene.ChangeAttackMode();
            };
        }

        public void Process()
        {
            switch (GameScene.Scene.AMode)
            {
                case AttackMode.Peace:
                    AMode = "[Mode: Peaceful]";
                    break;
                case AttackMode.Group:
                    AMode = "[Mode: Group]";
                    break;
                case AttackMode.Guild:
                    AMode = "[Mode: Guild]";
                    break;
                case AttackMode.EnemyGuild:
                    AMode = "[Mode: Enemy Guild]";
                    break;
                case AttackMode.RedBrown:
                    AMode = "[Mode: Red/Brown]";
                    break;
                case AttackMode.All:
                    AMode = "[Mode: Attack All]";
                    break;
            }

            switch (GameScene.Scene.PMode)
            {
                case PetMode.Both:
                    PMode = "[Mode: Attack and Move]";
                    break;
                case PetMode.MoveOnly:
                    PMode = "[Mode: Do Not Attack]";
                    break;
                case PetMode.AttackOnly:
                    PMode = "[Mode: Do Not Move]";
                    break;
                case PetMode.None:
                    PMode = "[Mode: Do Not Attack or Move]";
                    break;
            }

            switch (Settings.SkillMode)
            {
                case true:
                    SMode = "[Mode: ~]";
                    break;
                case false:
                    SMode = "[Mode: Ctrl]";
                    break;
            }

            //GameScene.Scene.MiniMapDialog.AModeLabel.Text = AMode;
            //GameScene.Scene.MiniMapDialog.PModeLabel.Text = PMode;
            //GameScene.Scene.MiniMapDialog.SModeLabel.Text = SMode;

            Button1.Hint = string.Format("Skill Mode\r\n{0}", SMode);
            Button2.Hint = string.Format("Pet Mode ({1})\r\n{0}", PMode, CMain.InputKeys.GetKey(KeybindOptions.ChangePetmode));
            Button3.Hint = string.Format("Attack Mode ({1})\r\n{0}", AMode, CMain.InputKeys.GetKey(KeybindOptions.ChangeAttackmode));
        }

        public void Toggle()
        {
            Visible = !Visible;
        }
    }
}
