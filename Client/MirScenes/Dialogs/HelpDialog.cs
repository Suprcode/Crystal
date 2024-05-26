using Client.Forms;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;

namespace Client.MirScenes.Dialogs {
    public sealed class HelpDialog : MirImageControl {
        public List<HelpPage> Pages = new();

        public MirButton CloseButton, NextButton, PreviousButton;
        public MirLabel PageLabel;
        public HelpPage CurrentPage;

        public int CurrentPageNumber = 0;

        public HelpDialog() {
            Index = 920;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;

            Location = Center;

            MirImageControl TitleLabel = new() {
                Index = 57,
                Library = Libraries.Title,
                Location = new Point(18, 9),
                Parent = this
            };

            PreviousButton = new MirButton {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(210, 485),
                Sound = SoundList.ButtonA
            };
            PreviousButton.Click += (o, e) => {
                CurrentPageNumber--;

                if(CurrentPageNumber < 0) {
                    CurrentPageNumber = Pages.Count - 1;
                }

                DisplayPage(CurrentPageNumber);
            };

            NextButton = new MirButton {
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 16),
                Location = new Point(310, 485),
                Sound = SoundList.ButtonA
            };
            NextButton.Click += (o, e) => {
                CurrentPageNumber++;

                if(CurrentPageNumber > Pages.Count - 1) {
                    CurrentPageNumber = 0;
                }

                DisplayPage(CurrentPageNumber);
            };

            PageLabel = new MirLabel {
                Text = "",
                Font = new Font(Settings.FontName, 9F),
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Parent = this,
                NotControl = true,
                Location = new Point(230, 480),
                Size = new Size(80, 20)
            };

            CloseButton = new MirButton {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(509, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA
            };
            CloseButton.Click += (o, e) => Hide();

            LoadImagePages();

            DisplayPage();
        }

        private void LoadImagePages() {
            Point location = new(12, 35);

            Dictionary<string, string> keybinds = new();

            List<HelpPage> imagePages = new() {
                new("Shortcut Information", -1, new ShortcutPage1 { Parent = this })
                    { Parent = this, Location = location, Visible = false },
                new("Shortcut Information", -1, new ShortcutPage2 { Parent = this })
                    { Parent = this, Location = location, Visible = false },
                new("Chat Shortcuts", -1, new ShortcutPage3 { Parent = this })
                    { Parent = this, Location = location, Visible = false },
                new("Movements", 0, null) { Parent = this, Location = location, Visible = false },
                new("Attacking", 1, null) { Parent = this, Location = location, Visible = false },
                new("Collecting Items", 2, null) { Parent = this, Location = location, Visible = false },
                new("Health", 3, null) { Parent = this, Location = location, Visible = false },
                new("Skills", 4, null) { Parent = this, Location = location, Visible = false },
                new("Skills", 5, null) { Parent = this, Location = location, Visible = false },
                new("Mana", 6, null) { Parent = this, Location = location, Visible = false },
                new("Chatting", 7, null) { Parent = this, Location = location, Visible = false },
                new("Groups", 8, null) { Parent = this, Location = location, Visible = false },
                new("Durability", 9, null) { Parent = this, Location = location, Visible = false },
                new("Purchasing", 10, null) { Parent = this, Location = location, Visible = false },
                new("Selling", 11, null) { Parent = this, Location = location, Visible = false },
                new("Repairing", 12, null) { Parent = this, Location = location, Visible = false },
                new("Trading", 13, null) { Parent = this, Location = location, Visible = false },
                new("Inspecting", 14, null) { Parent = this, Location = location, Visible = false },
                new("Statistics", 15, null) { Parent = this, Location = location, Visible = false },
                new("Statistics", 16, null) { Parent = this, Location = location, Visible = false },
                new("Statistics", 17, null) { Parent = this, Location = location, Visible = false },
                new("Statistics", 18, null) { Parent = this, Location = location, Visible = false },
                new("Statistics", 19, null) { Parent = this, Location = location, Visible = false },
                new("Statistics", 20, null) { Parent = this, Location = location, Visible = false },
                new("Quests", 21, null) { Parent = this, Location = location, Visible = false },
                new("Quests", 22, null) { Parent = this, Location = location, Visible = false },
                new("Quests", 23, null) { Parent = this, Location = location, Visible = false },
                new("Quests", 24, null) { Parent = this, Location = location, Visible = false },
                new("Mounts", 25, null) { Parent = this, Location = location, Visible = false },
                new("Mounts", 26, null) { Parent = this, Location = location, Visible = false },
                new("Fishing", 27, null) { Parent = this, Location = location, Visible = false },
                new("Gems and Orbs", 28, null) { Parent = this, Location = location, Visible = false }
            };

            Pages.AddRange(imagePages);
        }


        public void DisplayPage(string pageName) {
            if(Pages.Count < 1) {
                return;
            }

            for (int i = 0; i < Pages.Count; i++) {
                if(Pages[i].Title.ToLower() != pageName.ToLower()) {
                    continue;
                }

                DisplayPage(i);
                break;
            }
        }

        public void DisplayPage(int id = 0) {
            if(Pages.Count < 1) {
                return;
            }

            if(id > Pages.Count - 1) {
                id = Pages.Count - 1;
            }

            if(id < 0) {
                id = 0;
            }

            if(CurrentPage != null) {
                CurrentPage.Visible = false;
                if(CurrentPage.Page != null) {
                    CurrentPage.Page.Visible = false;
                }
            }

            CurrentPage = Pages[id];

            if(CurrentPage == null) {
                return;
            }

            CurrentPage.Visible = true;
            if(CurrentPage.Page != null) {
                CurrentPage.Page.Visible = true;
            }

            CurrentPageNumber = id;

            CurrentPage.PageTitleLabel.Text = id + 1 + ". " + CurrentPage.Title;

            PageLabel.Text = string.Format("{0} / {1}", id + 1, Pages.Count);

            Show();
        }


        public void Toggle() {
            if(!Visible) {
                Show();
            } else {
                Hide();
            }
        }
    }

    public class ShortcutPage1 : ShortcutInfoPage {
        public ShortcutPage1() {
            Shortcuts = new List<ShortcutInfo> {
                new(CMain.InputKeys.GetKey(KeybindOptions.Exit), "Exit the game"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Logout), "Log out"),
                new(
                    CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill1) + "-" +
                    CMain.InputKeys.GetKey(KeybindOptions.Bar1Skill8), "Skill buttons"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Inventory), "Inventory window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Equipment), "Status window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Skills), "Skill window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Group), "Group window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Trade), "Trade window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Friends), "Friend window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Minimap), "Minimap window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Guilds), "Guild window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.GameShop), "Gameshop window (open / close)"),
                //Shortcuts.Add(new ShortcutInfo("K", "Rental window (open / close)"));
                new(CMain.InputKeys.GetKey(KeybindOptions.Relationship), "Engagement window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Belt), "Belt window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Options), "Option window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Help), "Help window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Mount), "Mount / Dismount ride"),
                new(CMain.InputKeys.GetKey(KeybindOptions.TargetSpellLockOn),
                    "Lock spell onto target not cursor location")
            };

            LoadKeyBinds();
        }
    }

    public class ShortcutPage2 : ShortcutInfoPage {
        public ShortcutPage2() {
            Shortcuts = new List<ShortcutInfo> {
                new(CMain.InputKeys.GetKey(KeybindOptions.ChangePetmode), "Toggle pet attack pet"),
                //Shortcuts.Add(new ShortcutInfo("Ctrl + F", "Change the font in the chat box"));
                new(CMain.InputKeys.GetKey(KeybindOptions.ChangeAttackmode), "Toggle player attack mode"),
                new(CMain.InputKeys.GetKey(KeybindOptions.AttackmodePeace), "Peace Mode - Attack monsters only"),
                new(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeGroup),
                    "Group Mode - Attack all subjects except your group members"),
                new(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeGuild),
                    "Guild Mode - Attack all subjects except your guild members"),
                new(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeRedbrown),
                    "Good/Evil Mode - Attack PK players and monsters only"),
                new(CMain.InputKeys.GetKey(KeybindOptions.AttackmodeAll), "All Attack Mode - Attack all subjects"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Bigmap), "Show the field map"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Skillbar), "Show the skill bar"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Autorun), "Auto run on / off"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Cameramode), "Show / Hide interface"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Pickup), "Highlight / Pickup Items"),
                new("Ctrl + Right Click", "Show other players kits"),
                //Shortcuts.Add(new ShortcutInfo("F12", "Chat macros"));
                new(CMain.InputKeys.GetKey(KeybindOptions.Screenshot), "Screen Capture"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Fishing), "Open / Close fishing window"),
                new(CMain.InputKeys.GetKey(KeybindOptions.Mentor), "Mentor window (open / close)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.CreaturePickup), "Creature Pickup (Multi Mouse Target)"),
                new(CMain.InputKeys.GetKey(KeybindOptions.CreatureAutoPickup), "Creature Pickup (Single Mouse Target)")
            };

            LoadKeyBinds();
        }
    }

    public class ShortcutPage3 : ShortcutInfoPage {
        public ShortcutPage3() {
            Shortcuts = new List<ShortcutInfo> {
                //Shortcuts.Add(new ShortcutInfo("` / Ctrl", "Change the skill bar"));
                new("/(username)", "Command to whisper to others"),
                new("!(text)", "Command to shout to others nearby"),
                new("!~(text)", "Command to guild chat")
            };

            LoadKeyBinds();
        }
    }

    public class ShortcutInfo {
        public string Shortcut { get; set; }
        public string Information { get; set; }

        public ShortcutInfo(string shortcut, string info) {
            Shortcut = shortcut.Replace("\n", " + ");
            Information = info;
        }
    }

    public class ShortcutInfoPage : MirControl {
        protected List<ShortcutInfo> Shortcuts = new();

        public ShortcutInfoPage() {
            Visible = false;

            MirLabel shortcutTitleLabel = new() {
                Text = "Shortcuts",
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                ForeColour = Color.White,
                Font = new Font(Settings.FontName, 10F),
                Parent = this,
                AutoSize = true,
                Location = new Point(13, 75),
                Size = new Size(100, 30)
            };

            MirLabel infoTitleLabel = new() {
                Text = "Information",
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                ForeColour = Color.White,
                Font = new Font(Settings.FontName, 10F),
                Parent = this,
                AutoSize = true,
                Location = new Point(114, 75),
                Size = new Size(405, 30)
            };
        }

        public void LoadKeyBinds() {
            if(Shortcuts == null) {
                return;
            }

            for (int i = 0; i < Shortcuts.Count; i++) {
                MirLabel shortcutLabel = new() {
                    Text = Shortcuts[i].Shortcut,
                    ForeColour = Color.Yellow,
                    DrawFormat = TextFormatFlags.VerticalCenter,
                    Font = new Font(Settings.FontName, 9F),
                    Parent = this,
                    AutoSize = true,
                    Location = new Point(18, 107 + (20 * i)),
                    Size = new Size(95, 23)
                };

                MirLabel informationLabel = new() {
                    Text = Shortcuts[i].Information,
                    DrawFormat = TextFormatFlags.VerticalCenter,
                    ForeColour = Color.White,
                    Font = new Font(Settings.FontName, 9F),
                    Parent = this,
                    AutoSize = true,
                    Location = new Point(119, 107 + (20 * i)),
                    Size = new Size(400, 23)
                };
            }
        }
    }

    public class HelpPage : MirControl {
        public string Title;
        public int ImageID;
        public MirControl Page;

        public MirLabel PageTitleLabel;

        public HelpPage(string title, int imageID, MirControl page) {
            Title = title;
            ImageID = imageID;
            Page = page;

            NotControl = true;
            Size = new Size(508, 396 + 40);

            BeforeDraw += HelpPage_BeforeDraw;

            PageTitleLabel = new MirLabel {
                Text = Title,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Parent = this,
                Size = new Size(242, 30),
                Location = new Point(135, 4)
            };
        }

        private void HelpPage_BeforeDraw(object sender, EventArgs e) {
            if(ImageID < 0) {
                return;
            }

            Libraries.Help.Draw(ImageID, new Point(DisplayLocation.X, DisplayLocation.Y + 40), Color.White, false);
        }
    }
}
