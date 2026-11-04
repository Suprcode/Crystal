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
                new HelpPage(GameLanguage.Get("Shortcut Information"), -1, new ShortcutPage1 { Parent = this } ) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Shortcut Information"), -1, new ShortcutPage2 { Parent = this } ) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Chat Shortcuts"), -1, new ShortcutPage3 { Parent = this } ) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Movements"), 0, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Attacking"), 1, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Collecting Items"), 2, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Health"), 3, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Skills"), 4, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Skills"), 5, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Mana"), 6, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Chatting"), 7, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Groups"), 8, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Durability:"), 9, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Purchasing"), 10, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Selling"), 11, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Repairing"), 12, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Trading"), 13, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Inspecting"), 14, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Statistics"), 15, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Statistics"), 16, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Statistics"), 17, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Statistics"), 18, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Statistics"), 19, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Statistics"), 20, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Quests"), 21, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Quests"), 22, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Quests"), 23, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Quests"), 24, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Mounts"), 25, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Mounts"), 26, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Fishing"), 27, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Gems and Orbs"), 28, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Heroes"), 29, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Heroes"), 30, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Heroes"), 31, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Heroes"), 32, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Heroes"), 33, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Guild Buffs"), 34, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Guild Buffs"), 35, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Guild Buffs"), 36, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Awakening"), 37, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Awakening"), 38, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Awakening"), 39, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Awakening"), 40, null) { Parent = this, Location = location, Visible = false },
                new HelpPage(GameLanguage.Get("Awakening"), 41, null) { Parent = this, Location = location, Visible = false },
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
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Exit), GameLanguage.Get("Exit the game")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Logout), GameLanguage.Get("Log out")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill1) + "-" + CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill8), GameLanguage.Get("Skill buttons")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Inventory), GameLanguage.Get("Inventory window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Equipment), GameLanguage.Get("Status window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Skills), GameLanguage.Get("Skill window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Group), GameLanguage.Get("Group window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Trade), GameLanguage.Get("Trade window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Friends), GameLanguage.Get("Friend window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Minimap), GameLanguage.Get("Minimap window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Guilds), GameLanguage.Get("Guild window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.GameShop), GameLanguage.Get("Gameshop window (open / close)")),
                //Shortcuts.Add(new ShortcutInfo("K", "Rental window (open / close)"));
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Relationship), GameLanguage.Get("Engagement window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Belt), GameLanguage.Get("Belt window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Options), GameLanguage.Get("Option window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Help), GameLanguage.Get("Help window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Mount), GameLanguage.Get("Mount / Dismount ride")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.TargetSpellLockOn), GameLanguage.Get("Lock spell onto target not cursor location"))
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
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.ChangePetmode), GameLanguage.Get("Toggle pet attack pet")),
                //Shortcuts.Add(new ShortcutInfo("Ctrl + F", "Change the font in the chat box"));
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.ChangeAttackmode), GameLanguage.Get("Toggle player attack mode")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodePeace), GameLanguage.Get("Peace Mode - Attack monsters only")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeGroup), GameLanguage.Get("Group Mode - Attack all subjects except your group members")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeGuild), GameLanguage.Get("Guild Mode - Attack all subjects except your guild members")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeRedbrown), GameLanguage.Get("Good/Evil Mode - Attack PK players and monsters only")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeAll), GameLanguage.Get("All Attack Mode - Attack all subjects")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Bigmap), GameLanguage.Get("Show the field map")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Skillbar), GameLanguage.Get("Show the skill bar")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Autorun), GameLanguage.Get("Auto run on / off")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Cameramode), GameLanguage.Get("Show / Hide interface")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Pickup), GameLanguage.Get("Highlight / Pickup Items")),
                new ShortcutInfo(GameLanguage.Get("Ctrl + Right Click"), GameLanguage.Get("Show other players kits")),
                //Shortcuts.Add(new ShortcutInfo("F12", "Chat macros"));
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Screenshot), GameLanguage.Get("Screen Capture")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Fishing), GameLanguage.Get("Open / Close fishing window")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.Mentor), GameLanguage.Get("Mentor window (open / close)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.CreaturePickup), GameLanguage.Get("Creature Pickup (Multi Mouse Target)")),
                new ShortcutInfo(CMain.InputKeys.GetKey(KeybindOptions.CreatureAutoPickup), GameLanguage.Get("Creature Pickup (Single Mouse Target)"))
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
                new ShortcutInfo("/(username)", GameLanguage.Get("Command to whisper to others")),
                new ShortcutInfo("!(text)", GameLanguage.Get("Command to shout to others nearby")),
                new ShortcutInfo("!~(text)", GameLanguage.Get("Command to guild chat"))
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
                Text = GameLanguage.Get("Shortcuts"),
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
                Text = GameLanguage.Get("Information"),
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
