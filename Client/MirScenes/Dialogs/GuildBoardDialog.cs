using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using ClientPackets;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace Client.MirScenes.Dialogs
{
    public sealed class GuildBoardDialog : MirImageControl
    {

        public MirButton ReadButton, WriteButton, NoticeButton, CloseButton, RefreshButton, LeftButton, RightButton;

        public MirLabel WriterLabel, MessageTextLabel, GuildNameLabel, PageLabel;

        public MirImageControl TitleLabel;

        private int FindIndex = 0;

        private int RequestIndex = 0;

        private int ViewCount = 10;

        //private int Type = -1;

        private long LastMsgTime;

        private List<GuildBoardCell> GuildBoards = new List<GuildBoardCell>();

        public static GuildBoardCell SelectBoard = null;

        public GuildBoardDialog()
        {
            Index = 688;
            Library = Libraries.Prguse;
            Movable = true;
            Location = new Point(0, 0);
            Sort = true;
            //BeforeDraw += new EventHandlerDraw_BeforeDraw);

            TitleLabel = new MirImageControl
            {
                Index = 46,
                Library = Libraries.Title,
                Location = new Point(20, 4),
                Parent = this
            };

            GuildNameLabel = new MirLabel
            {
                Text = "Guild Name",
                Location = new Point(200, 41),
                Parent = this,
                AutoSize = true,
                DrawFormat = TextFormatFlags.Default,
                Font = new Font(Settings.FontName, 14f),
                ForeColour = Color.White,
                NotControl = true
            };
            WriterLabel = new MirLabel
            {
                Text = "Writer",
                Location = new Point(64, 72),
                Parent = this,
                AutoSize = true,
                DrawFormat = TextFormatFlags.Default,
                Font = new Font(Settings.FontName, 10f),
                ForeColour = Color.Yellow,
                NotControl = true
            };
            MessageTextLabel = new MirLabel
            {
                Text = "Message Text",
                Location = new Point(264, 72),
                Parent = this,
                AutoSize = true,
                DrawFormat = TextFormatFlags.Default,
                Font = new Font(Settings.FontName, 10f),
                ForeColour = Color.Yellow,
                NotControl = true
            };
            PageLabel = new MirLabel
            {
                Text = "1/1",
                Location = new Point(234, 273),
                Parent = this,
                AutoSize = true,
                DrawFormat = TextFormatFlags.Default,
                Font = new Font(Settings.FontName, 9f),
                ForeColour = Color.White,
                NotControl = true
            };
            CloseButton = new MirButton
            {
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Location = new Point(464, 5),
                Library = Libraries.Prguse2,
                Parent = this,
                Sound = SoundList.ButtonA
            };
            CloseButton.Click += (o, e) =>
            {
                Hide();
            };
            RefreshButton = new MirButton
            {
                Index = 663,
                HoverIndex = 664,
                PressedIndex = 665,
                Location = new Point(335, 293),
                Library = Libraries.Prguse,
                Parent = this,
                Sound = SoundList.ButtonA,
                Hint = "Refresh"
            };
            RefreshButton.Click += (o, e) =>
            {
                bool flag = LastMsgTime > CMain.Time;
                if (!flag)
                {
                    LastMsgTime = CMain.Time + 500L;
                    Network.Enqueue(new RequestGuildHouseBoards());
                }
            };

            ReadButton = new MirButton
            {
                Index = 560,
                HoverIndex = 561,
                PressedIndex = 562,
                Location = new Point(370, 293),
                Library = Libraries.Prguse,
                Parent = this,
                Sound = SoundList.ButtonA,
                Hint = "Read"
            };
            ReadButton.Click += (o, e) =>
            {
                bool flag = GuildBoardDialog.SelectBoard == null;
                if (!flag)
                {
                    bool flag2 = LastMsgTime > CMain.Time;
                    if (!flag2)
                    {
                        LastMsgTime = CMain.Time + 500L;
                        GameScene.Scene.GuildReadLetterDialog.ReadBoard(GuildBoardDialog.SelectBoard.Info);
                        GuildBoardDialog.SelectBoard = null;
                    }
                }
            };

            WriteButton = new MirButton
            {
                Index = 554,
                HoverIndex = 555,
                PressedIndex = 556,
                Location = new Point(405, 293),
                Library = Libraries.Prguse,
                Parent = this,
                Sound = SoundList.ButtonA,
                Hint = "Write"
            };
            MirControl arg_375_0 = WriteButton;
            //EventHandler arg_375_1;
            //if ((arg_375_1 = GuildBoardDialog.<> c.<> 9__16_3) == null)
            //{
            //    arg_375_1 = (GuildBoardDialog.<> c.<> 9__16_3 = new EventHandler(GuildBoardDialog.<> c.<> 9.<.ctor > b__16_3));
            //}
            //arg_375_0.Click += arg_375_1;

            NoticeButton = new MirButton
            {
                Index = 575,
                HoverIndex = 576,
                PressedIndex = 577,
                Location = new Point(440, 293),
                Library = Libraries.Prguse,
                Parent = this,
                Sound = SoundList.ButtonA,
                Hint = "Notice"
            };
            NoticeButton.Click += (o, e) =>
            {
                bool flag = GuildBoardDialog.SelectBoard == null;
                if (!flag)
                {
                    bool flag2 = LastMsgTime > CMain.Time;
                    if (!flag2)
                    {
                        LastMsgTime = CMain.Time + 500L;
                        GuildBoardDialog.SelectBoard.Info.Notice = !GuildBoardDialog.SelectBoard.Info.Notice;
                        Network.Enqueue(new SendGuildHouseBoard
                        {
                            Info = GuildBoardDialog.SelectBoard.Info,
                            Mode = 1
                        });
                        GuildBoardDialog.SelectBoard = null;
                    }
                }
            };

            LeftButton = new MirButton
            {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Location = new Point(185, 272),
                Library = Libraries.Prguse2,
                Parent = this,
                Sound = SoundList.ButtonA
            };

            LeftButton.Click += (o, e) =>
            {
                bool flag = FindIndex <= 0;
                if (flag)
                {
                    FindIndex = GuildBoards.Count / ViewCount;
                }
                else
                {
                    FindIndex--;
                }
                RefreshBoardList();
            };

            RightButton = new MirButton
            {
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Location = new Point(285, 272),
                Library = Libraries.Prguse2,
                Parent = this,
                Sound = SoundList.ButtonA
            };
            RightButton.Click += (o, e) =>
            {
                bool flag = FindIndex >= GuildBoards.Count / ViewCount;
                if (flag)
                {
                    FindIndex = 0;
                }
                else
                {
                    FindIndex++;
                }
                RefreshBoardList();
            };
        }

        private void Draw_BeforeDraw(object sender, EventArgs e)
        {
            PageLabel.Text = FindIndex + 1 + "/" + (GuildBoards.Count / ViewCount + 1);
            int size = TitleLabel.Size.Width / 2;
            TitleLabel.Location = new Point(244 - size, TitleLabel.Location.Y);
            size = PageLabel.Size.Width / 2;
            PageLabel.Location = new Point(244 - size, PageLabel.Location.Y);
        }

        public void SetBoardList(List<BoardInfo> list)
        {
            ClearList();
            bool flag = MapObject.User != null;
            if (flag)
            {
                GuildNameLabel.Text = MapObject.User.GuildName;
            }
            //if ((arg_4A_1 = GuildBoardDialog.<> c.<> 9__19_0) == null)
            //{
            //  arg_4A_1 = (GuildBoardDialog.<> c.<> 9__19_0 = new Comparison<BoardInfo>(GuildBoardDialog.<> c.<> 9.< SetBoardList > b__19_0));
            //}
            //list.Sort(arg_4A_1);
            for (int i = 0; i < list.Count; i++)
            {
                GuildBoards.Add(new GuildBoardCell
                {
                    Info = list[i],
                    Location = new Point(10, 94 + 18 * (i % ViewCount)),
                    Size = new Size(465, 15),
                    Parent = this,
                    Visible = true
                });
            }
            RefreshBoardList();
        }

        public void ClearList()
        {
            for (int i = 0; i < GuildBoards.Count; i++)
            {
                GuildBoards[i].Hide();
                GuildBoards[i].Destroy();
            }
            GuildBoards.Clear();
            FindIndex = 0;
        }

        private void RefreshBoardList()
        {
            for (int i = 0; i < GuildBoards.Count; i++)
            {
                RequestIndex = FindIndex * ViewCount;
                bool flag = RequestIndex <= i && RequestIndex + ViewCount > i;
                if (flag)
                {
                    GuildBoards[i].Show();
                }
                else
                {
                    GuildBoards[i].Hide();
                }
            }
        }

        public void Show()
        {
            bool visible = Visible;
            if (!visible)
            {
                Visible = true;
            }
        }

        public void Hide()
        {
            bool flag = !Visible;
            if (!flag)
            {
                Visible = false;
            }
        }
    }
}