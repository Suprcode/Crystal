using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;

namespace Client.MirScenes.Dialogs
{
    public sealed class HelpDialog : MirImageControl
    {
        public List<HelpPage> Pages = new List<HelpPage>();

        public MirButton CloseButton, NextButton, PreviousButton;
        public MirLabel PageLabel;
        public HelpPage CurrentPage;

        public int CurrentPageNumber = 0;

        public HelpDialog()
        {
            Index = 920;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;

            Location = Center;

            MirImageControl TitleLabel = new MirImageControl
            {
                Index = 57,
                Library = Libraries.Title,
                Location = new Point(18, 9),
                Parent = this
            };

            PreviousButton = new MirButton
            {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(210, 485),
                Sound = SoundList.ButtonA,
            };
            PreviousButton.Click += (o, e) =>
            {
                CurrentPageNumber--;

                if (CurrentPageNumber < 0) CurrentPageNumber = Pages.Count - 1;

                DisplayPage(CurrentPageNumber);
            };

            NextButton = new MirButton
            {
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(310, 485),
                Sound = SoundList.ButtonA,
            };
            NextButton.Click += (o, e) =>
            {
                CurrentPageNumber++;

                if (CurrentPageNumber > Pages.Count - 1) CurrentPageNumber = 0;

                DisplayPage(CurrentPageNumber);
            };

            PageLabel = new MirLabel
            {
                Text = "",
                Font = new Font(Settings.FontName, 9F),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Parent = this,
                NotControl = true,
                Location = new Point(230, 480),
                Size = new Size(80, 20)
            };

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(509, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            LoadImagePages();

            DisplayPage();
        }

        private void LoadImagePages()
        {
            Point location = new Point(12, 35);

            Dictionary<string, string> keybinds = new Dictionary<string, string>();

            List<HelpPage> imagePages = new List<HelpPage> { 
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ShortcutInformation), -1, new ShortcutPage1 { Parent = this } ) { Parent = this, Location = location, Visible = false }, 
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ShortcutInformation), -1, new ShortcutPage2 { Parent = this } ) { Parent = this, Location = location, Visible = false }, 
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ChatShortcuts), -1, new ShortcutPage3 { Parent = this } ) { Parent = this, Location = location, Visible = false }, 
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Movements), 0, null) { Parent = this, Location = location, Visible = false }, 
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Attacking), 1, null) { Parent = this, Location = location, Visible = false }, 
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CollectingItems), 2, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Health), 3, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Skills), 4, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Skills), 5, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Mana), 6, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Chatting), 7, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Groups), 8, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Durability), 9, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Purchasing), 10, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Selling), 11, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Repairing), 12, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Trading), 13, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Inspecting), 14, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Statistics), 15, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Statistics), 16, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Statistics), 17, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Statistics), 18, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Statistics), 19, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Statistics), 20, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Quests), 21, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Quests), 22, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Quests), 23, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Quests), 24, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Mounts), 25, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Mounts), 26, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Fishing), 27, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GemsAndOrbs), 28, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Heroes), 29, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Heroes), 30, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Heroes), 31, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Heroes), 32, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Heroes), 33, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GuildBuffs), 34, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GuildBuffs), 35, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GuildBuffs), 36, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Awakening), 37, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Awakening), 38, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Awakening), 39, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Awakening), 40, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Awakening), 41, null) { Parent = this, Location = location, Visible = false },
            };

            Pages.AddRange(imagePages);
        }


        public void DisplayPage(string pageName)
        {
            if (Pages.Count < 1) return;

            for (int i = 0; i < Pages.Count; i++)
            {
                if (Pages[i].Title.ToLower() != pageName.ToLower()) continue;

                DisplayPage(i);
                break;
            }
        }

        public void DisplayPage(int id = 0)
        {
            if (Pages.Count < 1) return;

            if (id > Pages.Count - 1) id = Pages.Count - 1;
            if (id < 0) id = 0;

            if (CurrentPage != null)
            {
                CurrentPage.Visible = false;
                if (CurrentPage.Page != null) CurrentPage.Page.Visible = false;
            }

            CurrentPage = Pages[id];

            if (CurrentPage == null) return;

            CurrentPage.Visible = true;
            if (CurrentPage.Page != null) CurrentPage.Page.Visible = true;
            CurrentPageNumber = id;

            CurrentPage.PageTitleLabel.Text = id + 1 + ". " + CurrentPage.Title;

            PageLabel.Text = string.Format("{0} / {1}", id + 1, Pages.Count);

            Show();
        }


        public void Toggle()
        {
            if (!Visible)
                Show();
            else
                Hide();
        }
    }

    public class ShortcutPage1 : ShortcutInfoPage
    {
        public ShortcutPage1()
        {
            Shortcuts = new List<ShortcutInfo>
            {
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Exit), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ExitGame)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Logout), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LogOut)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill1) + "-" + CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill8), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SkillButtons)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Inventory), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.InventoryWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Equipment), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.StatusWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Skills), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.SkillWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Group), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GroupWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Trade), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.TradeWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Friends), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.FriendWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Minimap), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MinimapWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Guilds), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GuildWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.GameShop), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GameshopWindowOpenClose)),
                //Shortcuts.Add(new ShortcutInfo("K", "Rental window (open / close)"));
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Relationship), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.EngagementWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Belt), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.BeltWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Options), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.OptionWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Help), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.HelpWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Mount), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MountDismountRide)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.TargetSpellLockOn), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.LockSpellOnTargetNotCursor))
            };

            LoadKeyBinds();
        }
    }
    public class ShortcutPage2 : ShortcutInfoPage
    {
        public ShortcutPage2()
        {
            Shortcuts = new List<ShortcutInfo>
            {
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.ChangePetmode), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.TogglePetAttackPet)),
                //Shortcuts.Add(new ShortcutInfo("Ctrl + F", "Change the font in the chat box"));
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.ChangeAttackmode), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.TogglePlayerAttackMode)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodePeace), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.PeaceModeAttackMonstersOnly)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeGroup), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GroupModeAttackExceptMembers)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeGuild), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GuildModeAttackExceptMembers)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeRedbrown), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.GoodEvilModeAttackPKAndMonsters)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeAll), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AllAttackModeAllSubjects)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Bigmap), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ShowFieldMap)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Skillbar), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ShowSkillBar)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Autorun), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.AutoRunOnOff)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Cameramode), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ShowHideInterface)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Pickup), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.HighlightPickupItems)),
                new ShortcutInfo(GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CtrlRightClick), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ShowOtherPlayersKits)),
                //Shortcuts.Add(new ShortcutInfo("F12", "Chat macros"));
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Screenshot), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.ScreenCapture)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Fishing), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.OpenCloseFishingWindow)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Mentor), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.MentorWindowOpenClose)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.CreaturePickup), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CreaturePickupMultiMouseTarget)),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.CreatureAutoPickup), GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CreaturePickupSingleMouseTarget))
            };

            LoadKeyBinds();
        }
    }
    public class ShortcutPage3 : ShortcutInfoPage
    {
        public ShortcutPage3()
        {
            Shortcuts = new List<ShortcutInfo>
            {
                //Shortcuts.Add(new ShortcutInfo("` / Ctrl", "Change the skill bar"));
                new ShortcutInfo("/(username)", GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CommandWhisperOthers)),
                new ShortcutInfo("!(text)", GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CommandShoutNearby)),
                new ShortcutInfo("!~(text)", GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.CommandGuildChat))
            };

            LoadKeyBinds();
        }
    }

    public class ShortcutInfo
    {
        public string Shortcut { get; set; }
        public string Information { get; set; }

        public ShortcutInfo(string shortcut, string info)
        {
            Shortcut = shortcut.Replace("\n", " + ");
            Information = info;
        }
    }

    public class ShortcutInfoPage : MirControl
    {
        protected List<ShortcutInfo> Shortcuts = new List<ShortcutInfo>();

        public ShortcutInfoPage()
        {
            Visible = false;

            MirLabel shortcutTitleLabel = new MirLabel
            {
                Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Shortcuts),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                ForeColour = Color.White,
                Font = new Font(Settings.FontName, 10F),
                Parent = this,
                AutoSize = true,
                Location = new Point(13, 75),
                Size = new Size(100, 30)
            };

            MirLabel infoTitleLabel = new MirLabel
            {
                Text = GameLanguage.ClientTextMap.GetLocalization(ClientTextKeys.Information),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                ForeColour = Color.White,
                Font = new Font(Settings.FontName, 10F),
                Parent = this,
                AutoSize = true,
                Location = new Point(114, 75),
                Size = new Size(405, 30)
            };
        }

        public void LoadKeyBinds()
        {
            if (Shortcuts == null) return;

            for (int i = 0; i < Shortcuts.Count; i++)
            {
                MirLabel shortcutLabel = new MirLabel
                {
                    Text = Shortcuts[i].Shortcut,
                    ForeColour = Color.Yellow,
                    DrawFormat = TextFormatFlags.VerticalCenter,
                    Font = new Font(Settings.FontName, 9F),
                    Parent = this,
                    AutoSize = true,
                    Location = new Point(18, 107 + (20 * i)),
                    Size = new Size(95, 23),
                };

                MirLabel informationLabel = new MirLabel
                {
                    Text = Shortcuts[i].Information,
                    DrawFormat = TextFormatFlags.VerticalCenter,
                    ForeColour = Color.White,
                    Font = new Font(Settings.FontName, 9F),
                    Parent = this,
                    AutoSize = true,
                    Location = new Point(119, 107 + (20 * i)),
                    Size = new Size(400, 23),
                };
            }  
        }
    }

    public class HelpPage : MirControl
    {
        public string Title;
        public int ImageID;
        public MirControl Page;

        public MirLabel PageTitleLabel;

        public HelpPage(string title, int imageID, MirControl page)
        {
            Title = title;
            ImageID = imageID;
            Page = page;

            NotControl = true;
            Size = new System.Drawing.Size(508, 396 + 40);

            BeforeDraw += HelpPage_BeforeDraw;

            PageTitleLabel = new MirLabel
            {
                Text = Title,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                Size = new System.Drawing.Size(242, 30),
                Location = new Point(135, 4)
            };
        }

        void HelpPage_BeforeDraw(object sender, EventArgs e)
        {
            if (ImageID < 0) return;

            Libraries.Help.Draw(ImageID, new Point(DisplayLocation.X, DisplayLocation.Y + 40), Color.White, false);
        }
    }
}
