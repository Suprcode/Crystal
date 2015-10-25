using Client.MirControls;
using Client.MirGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Client.MirSounds;

namespace Client.MirScenes.Dialogs
{
    public class ChatOptionDialog : MirImageControl
    {
        public MirButton FilterTabButton, ChatTabButton;
        public MirButton CloseButton;

        public MirButton AllButton, GeneralButton, WhisperButton, ShoutButton, SystemButton, LoverButton, MentorButton, GroupButton, GuildButton;
        public MirButton TransparencyOnButton, TransparencyOffButton;

        public bool AllFiltersOff = true;

        public ChatOptionDialog()
        {
            Index = 466;
            Library = Libraries.Title;
            Size = new Size(224, 180);
            Movable = true;
            Sort = true;
            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 2);

            BeforeDraw += ChatFilterPanel_BeforeDraw;

            FilterTabButton = new MirButton
            {
                Index = 463,
                PressedIndex = 462,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(8, 8)
            };
            FilterTabButton.Click += (o, e) => SwitchTab(0);

            ChatTabButton = new MirButton
            {
                Index = 464,
                PressedIndex = 465,
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Location = new Point(78, 8)
            };
            ChatTabButton.Click += (o, e) => SwitchTab(1);

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(198, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            #region Filters

            AllButton = new MirButton
            {
                Index = 2087,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(74, 47),
                Sound = SoundList.ButtonA,
                Size = new Size(16, 12)
            };
            AllButton.Click += (o, e) => ToggleAllFilters();

            GeneralButton = new MirButton
            {
                Index = 2071,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(40, 69),
                Sound = SoundList.ButtonA
            };
            GeneralButton.Click += (o, e) =>
            {
                Settings.FilterNormalChat = !Settings.FilterNormalChat;
                CheckAllFilters();
            };

            WhisperButton = new MirButton
            {
                Index = 2075,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(40, 92),
                Sound = SoundList.ButtonA
            };
            WhisperButton.Click += (o, e) =>
            {
                Settings.FilterWhisperChat = !Settings.FilterWhisperChat;
                CheckAllFilters();
            };

            ShoutButton = new MirButton
            {
                Index = 2073,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(40, 115),
                Sound = SoundList.ButtonA
            };
            ShoutButton.Click += (o, e) =>
            {
                Settings.FilterShoutChat = !Settings.FilterShoutChat;
                CheckAllFilters();
            };

            SystemButton = new MirButton
            {
                Index = 2085,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(40, 138),
                Sound = SoundList.ButtonA
            };
            SystemButton.Click += (o, e) =>
            {
                Settings.FilterSystemChat = !Settings.FilterSystemChat;
                CheckAllFilters();
            };

            LoverButton = new MirButton
            {
                Index = 2077,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(135, 69),
                Sound = SoundList.ButtonA
            };
            LoverButton.Click += (o, e) =>
            {
                Settings.FilterLoverChat = !Settings.FilterLoverChat;
                CheckAllFilters();
            };

            MentorButton = new MirButton
            {
                Index = 2079,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(135, 92),
                Sound = SoundList.ButtonA
            };
            MentorButton.Click += (o, e) =>
            {
                Settings.FilterMentorChat = !Settings.FilterMentorChat;
                CheckAllFilters();
            };

            GroupButton = new MirButton
            {
                Index = 2081,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(135, 115),
                Sound = SoundList.ButtonA
            };
            GroupButton.Click += (o, e) =>
            {
                Settings.FilterGroupChat = !Settings.FilterGroupChat;
                CheckAllFilters();
            };

            GuildButton = new MirButton
            {
                Index = 2083,
                Library = Libraries.Prguse,
                Parent = this,
                Location = new Point(135, 138),
                Sound = SoundList.ButtonA
            };
            GuildButton.Click += (o, e) =>
            {
                Settings.FilterGuildChat = !Settings.FilterGuildChat;
                CheckAllFilters();
            };

            #endregion

            #region Transparency

            TransparencyOffButton = new MirButton
            {
                Index = 471,
                HoverIndex = 472,
                PressedIndex = 470,
                Location = new Point(45, 90),
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Visible = false
            };
            TransparencyOffButton.Click += (o, e) =>
            {
                Settings.TransparentChat = false;
                UpdateTransparency();
            };

            TransparencyOnButton = new MirButton
            {
                Index = 474,
                HoverIndex = 475,
                PressedIndex = 473,
                Location = new Point(115, 90),
                Parent = this,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA,
                Visible = false
            };
            TransparencyOnButton.Click += (o, e) =>
            {
                Settings.TransparentChat = true;
                UpdateTransparency();
            };

            #endregion

            CheckAllFilters();
            UpdateTransparency();
        }

        private void ChatFilterPanel_BeforeDraw(object sender, EventArgs e)
        {
            AllButton.Index = AllFiltersOff ? 2087 : 2086;

            GeneralButton.Index = Settings.FilterNormalChat ? 2070 : 2071;
            WhisperButton.Index = Settings.FilterWhisperChat ? 2074 : 2075;
            ShoutButton.Index = Settings.FilterShoutChat ? 2072 : 2073;
            SystemButton.Index = Settings.FilterSystemChat ? 2084 : 2085;
            LoverButton.Index = Settings.FilterLoverChat ? 2076 : 2077;
            MentorButton.Index = Settings.FilterMentorChat ? 2078 : 2079;
            GroupButton.Index = Settings.FilterGroupChat ? 2080 : 2081; 
            GuildButton.Index = Settings.FilterGuildChat ? 2082 : 2083;
        }

        private void SwitchTab(int tab = 0)
        {
            if(tab == 0)
            {
                FilterTabButton.Index = 463;
                FilterTabButton.PressedIndex = 462;
                ChatTabButton.Index = 464;
                ChatTabButton.PressedIndex = 465;
                Index = 466;

                //Show all buttons on filter tab
                AllButton.Visible = true;
                GeneralButton.Visible = true;
                WhisperButton.Visible = true;
                ShoutButton.Visible = true;
                SystemButton.Visible = true;
                LoverButton.Visible = true;
                MentorButton.Visible = true;
                GroupButton.Visible = true;
                GuildButton.Visible = true;

                //hide all transparency buttons
                TransparencyOffButton.Visible = false;
                TransparencyOnButton.Visible = false;
            }
            else if(tab == 1)
            {
                FilterTabButton.Index = 462;
                FilterTabButton.PressedIndex = 463;
                ChatTabButton.Index = 465;
                ChatTabButton.PressedIndex = 464;
                Index = 467;

                //Hide all buttons on filter tab
                AllButton.Visible = false;
                GeneralButton.Visible = false;
                WhisperButton.Visible = false;
                ShoutButton.Visible = false;
                SystemButton.Visible = false;
                LoverButton.Visible = false;
                MentorButton.Visible = false;
                GroupButton.Visible = false;
                GuildButton.Visible = false;

                //show all transparency buttons
                TransparencyOffButton.Visible = true;
                TransparencyOnButton.Visible = true;
            }
        }

        private void CheckAllFilters()
        {
            if (!Settings.FilterNormalChat && !Settings.FilterWhisperChat 
                && !Settings.FilterShoutChat && !Settings.FilterSystemChat
                && !Settings.FilterLoverChat && !Settings.FilterMentorChat 
                && !Settings.FilterGroupChat && !Settings.FilterGuildChat)
            {
                AllFiltersOff = true;
            }
            else
            {
                AllFiltersOff = false;
            }

            GameScene.Scene.ChatDialog.Update();
        }
        private void ToggleAllFilters()
        {
            if(AllFiltersOff)
            {
                //turn all filters on
                Settings.FilterNormalChat = true;
                Settings.FilterWhisperChat = true;
                Settings.FilterShoutChat = true;
                Settings.FilterSystemChat = true;
                Settings.FilterLoverChat = true;
                Settings.FilterMentorChat = true;
                Settings.FilterGroupChat = true;
                Settings.FilterGuildChat = true;

                AllButton.Index = 2086;
            }
            else
            {
                //turn all filters off
                Settings.FilterNormalChat = false;
                Settings.FilterWhisperChat = false;
                Settings.FilterShoutChat = false;
                Settings.FilterSystemChat = false;
                Settings.FilterLoverChat = false;
                Settings.FilterMentorChat = false;
                Settings.FilterGroupChat = false;
                Settings.FilterGuildChat = false;

                AllButton.Index = 2087;
            }

            AllFiltersOff = !AllFiltersOff;

            GameScene.Scene.ChatDialog.Update();
        }

        private void UpdateTransparency()
        {
            if (Settings.TransparentChat)
            {
                GameScene.Scene.ChatDialog.ForeColour = Color.FromArgb(190, 15, 0, 0);
                GameScene.Scene.ChatDialog.BackColour = Color.FromArgb(190, 15, 0, 0);

                TransparencyOnButton.Index = 474;
                TransparencyOnButton.HoverIndex = 475;

                TransparencyOffButton.Index = 470;
                TransparencyOffButton.HoverIndex = 470;
            }
            else
            {
                GameScene.Scene.ChatDialog.ForeColour = Color.White;
                GameScene.Scene.ChatDialog.BackColour = Color.White;

                TransparencyOnButton.Index = 473;
                TransparencyOnButton.HoverIndex = 473;

                TransparencyOffButton.Index = 471;
                TransparencyOffButton.HoverIndex = 472;
            }
        }

        public void Show()
        {
            if (Visible) return;
            Visible = true;
        }

        public void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }

        public void Toggle()
        {
            if (!Visible)
                Show();
            else
                Hide();
        }
    }
}
