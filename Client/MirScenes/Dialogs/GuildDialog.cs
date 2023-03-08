using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirObjects;
using Client.MirSounds;
using Font = System.Drawing.Font;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{

    public sealed class GuildDialog : MirImageControl
    {
        #region NoticeBase
        public MirLabel GuildName;
        public MirButton CloseButton;
        #endregion

        #region GuildLeft
        public MirButton NoticeButton, MembersButton, StorageButton, RankButton;
        public MirImageControl NoticePage, MembersPage, StoragePage, RankPage;
        public MirImageControl StoragePageBase, MembersPageBase;
        public MirImageControl TitleLabel;
        #endregion

        #region GuildRight
        public MirButton BuffButton, StatusButton;
        public MirImageControl BuffPage, StatusPage, StatusPageBase;

        public GuildBuffButton[] Buffs;
        private bool RequestedList = false;
        public List<GuildBuffInfo> GuildBuffInfos = new List<GuildBuffInfo>();
        public List<GuildBuff> EnabledBuffs = new List<GuildBuff>();
        public int StartIndex = 0;
        private long LastRequest = 0;

        public string ActiveStats = "";
        #endregion

        #region DataValues
        public byte Level;
        public long Experience;
        public long MaxExperience;
        public uint Gold;
        public byte SparePoints;
        public int MemberCount;
        public int MaxMembers;
        public bool Voting;
        public byte ItemCount;
        public byte BuffCount;
        public static int MyRankId;
        public static GuildRankOptions MyOptions;
        public List<GuildRank> Ranks = new List<GuildRank>();
        public bool MembersChanged = true;
        public long LastMemberRequest = 0;
        public long LastGuildMsg = 0;
        public long LastRankNameChange = 0;
        public GuildRankOptions GetMyOptions()
        {
            return MyOptions;
        }
        #endregion

        #region NoticePagePub
        public bool NoticeChanged = true;
        public long LastNoticeRequest = 0;
        public int NoticeScrollIndex = 0;
        public MirButton NoticeUpButton, NoticeDownButton, NoticePositionBar, NoticeEditButton, NoticeSaveButton;
        public MirTextBox Notice;
        #endregion

        #region MembersPagePub
        public int MemberScrollIndex = 0, MembersShowCount = 1;
        public MirButton MembersUpButton, MembersDownButton, MembersPositionBar;
        public MirLabel MembersHeaderRank, MembersHeaderName, MembersHeaderStatus, MembersShowOffline;
        public MirButton MembersShowOfflineButton;
        public MirImageControl MembersShowOfflineStatus;
        public MirDropDownBox[] MembersRanks;
        public MirLabel[] MembersName, MembersStatus;
        public MirButton[] MembersDelete;
        public int MemberPageRows = 18;
        public bool MembersShowOfflinesetting = true;
        #endregion

        #region StatusPagePub
        public MirLabel StatusLevelLabel;
        public MirLabel StatusHeaders;
        public MirLabel StatusGuildName, StatusLevel, StatusMembers;
        public MirImageControl StatusExpBar;
        public MirLabel StatusExpLabel, RecruitMemberLabel;
        public MirTextBox MembersRecruitName;
        public MirButton RecruitMemberButton;
        #endregion

        #region StoragePagePub
        public MirLabel StorageGoldText;
        public MirButton StorageGoldAdd, StorageGoldRemove, StorageGoldIcon;
        public MirItemCell[] StorageGrid;
        public MirButton StorageUpButton, StorageDownButton, StoragePositionBar;
        public int StorageIndex = 1;
        #endregion

        #region RankPagePub

        public MirLabel RanksSelectTextR, RanksSelectTextL, PointsLeft;
        public MirTextBox RanksName;
        public MirImageControl[] RanksOptionsStatus;
        public MirButton[] RanksOptionsButtons;
        public MirLabel[] RanksOptionsTexts;
        public MirDropDownBox RanksSelectBox;
        public MirButton RanksSaveName, UpButton, DownButton, PositionBar;
        #endregion

        #region BuffPagePub
        #endregion

        #region GuildUI
        public GuildDialog()
        {
            Index = 180;
            Library = Libraries.Prguse;
            Movable = true;
            Sort = true;
            Location = Center;

            BeforeDraw += (o, e) => RefreshInterface();

            #region TabUI

            TitleLabel = new MirImageControl
            {
                Index = 25,
                Library = Libraries.Title,
                Location = new Point(18, 9),
                Parent = this
            };

            NoticeButton = new MirButton
            {
                Library = Libraries.Title,
                Index = 93,
                PressedIndex = 94,
                Sound = SoundList.ButtonA,
                Parent = this,
                Location = new Point(20, 38)
            };
            NoticeButton.Click += (o, e) => LeftDialog(0);
            MembersButton = new MirButton
            {
                Library = Libraries.Title,
                Index = 99,
                PressedIndex = 100,
                Sound = SoundList.ButtonA,
                Parent = this,
                Location = new Point(91, 38),
            };
            MembersButton.Click += (o, e) => LeftDialog(1);
            StorageButton = new MirButton
            {
                Library = Libraries.Title,
                Index = 105,
                PressedIndex = 106,
                Sound = SoundList.ButtonA,
                Parent = this,
                Location = new Point(162, 38),
                Visible = false
            };
            StorageButton.Click += (o, e) => LeftDialog(2);
            RankButton = new MirButton // Ranks
            {
                Library = Libraries.Title,
                Index = 101,
                Sound = SoundList.ButtonA,
                Parent = this,
                Location = new Point(233, 38),
                Visible = false,
            };
            RankButton.Click += (o, e) => LeftDialog(3);

            StatusButton = new MirButton
            {
                Library = Libraries.Title,
                Parent = this,
                Index = 103,
                Location = new Point(501, 38),
                Sound = SoundList.ButtonA,
            };
            StatusButton.Click += (o, e) => RightDialog(0);

            BuffButton = new MirButton
            {
                Library = Libraries.Title,
                Parent = this,
                Index = 95,
                Location = new Point(430, 38),
                Sound = SoundList.ButtonA,
                Visible = false,
            };
            BuffButton.Click += (o, e) => RightDialog(1);

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(565, 4),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA
            };
            CloseButton.Click += (o, e) => Hide();
            #endregion

            #region NoticePageUI
            NoticePage = new MirImageControl()
            {
                Parent = this,
                Size = new Size(352, 372),
                Location = new Point(0, 60),
                Visible = true
            };
            Notice = new MirTextBox()
            {
                ForeColour = Color.White,
                Font = new Font(Settings.FontName, 8F),
                Enabled = false,
                Visible = true,
                Parent = NoticePage,
                Size = new Size(322, 330),
                Location = new Point(13, 1)
            };
            Notice.MultiLine();

            NoticeEditButton = new MirButton
            {
                Visible = false,
                Index = 560,
                HoverIndex = 561,
                PressedIndex = 562,
                Library = Libraries.Prguse,
                Sound = SoundList.ButtonA,
                Parent = NoticePage,
                Location = new Point(20, 342)
            };
            NoticeEditButton.Click += (o, e) => EditNotice();

            NoticeSaveButton = new MirButton
            {
                Visible = false,
                Index = 554,
                HoverIndex = 555,
                PressedIndex = 556,
                Library = Libraries.Prguse,
                Sound = SoundList.ButtonA,
                Parent = NoticePage,
                Location = new Point(20, 342)
            };
            NoticeSaveButton.Click += (o, e) => EditNotice();

            NoticeUpButton = new MirButton
            {
                HoverIndex = 198,
                Index = 197,
                Visible = true,
                Library = Libraries.Prguse2,
                Location = new Point(337, 1),
                Size = new Size(16, 14),
                Parent = NoticePage,
                PressedIndex = 199,
                Sound = SoundList.ButtonA
            };
            NoticeUpButton.Click += (o, e) =>
            {
                if (NoticeScrollIndex == 0) return;
                NoticeScrollIndex--;
                UpdateNotice();
                UpdateNoticeScrollPosition();
            };

            NoticeDownButton = new MirButton
            {
                HoverIndex = 208,
                Index = 207,
                Visible = true,
                Library = Libraries.Prguse2,
                Location = new Point(337, 318),
                Size = new Size(16, 14),
                Parent = NoticePage,
                PressedIndex = 209,
                Sound = SoundList.ButtonA
            };
            NoticeDownButton.Click += (o, e) =>
            {
                if (NoticeScrollIndex == Notice.MultiText.Length - 1) return;
                if (NoticeScrollIndex >= Notice.MultiText.Length - 25) NoticeScrollIndex--;
                NoticeScrollIndex++;
                UpdateNotice(true);
                UpdateNoticeScrollPosition();
            };

            NoticePositionBar = new MirButton
            {
                Index = 206,
                Library = Libraries.Prguse2,
                Location = new Point(337, 16),
                Parent = NoticePage,
                Movable = true,
                Visible = true,
                Sound = SoundList.None
            };
            NoticePositionBar.OnMoving += NoticePositionBar_OnMoving;

            NoticePage.KeyDown += NoticePanel_KeyDown;
            NoticePage.MouseWheel += NoticePanel_MouseWheel;
            #endregion

            #region MembersPageUI
            MembersPage = new MirImageControl()
            {
                Parent = this,
                Size = new Size(352, 372),
                Location = new Point(0, 60),
                Visible = false
            };
            MembersPageBase = new MirImageControl()
            {
                Library = Libraries.Prguse,
                Index = 1852,
                Parent = MembersPage,
                Location = new Point(13, 1),
                Visible = true
            };
            MembersPageBase.MouseWheel += MembersPanel_MouseWheel;
            MembersPage.BeforeDraw += (o, e) => RequestUpdateMembers();
            MembersRanks = new MirDropDownBox[MemberPageRows];
            MembersName = new MirLabel[MemberPageRows];
            MembersStatus = new MirLabel[MemberPageRows];
            MembersDelete = new MirButton[MemberPageRows];

            for (int i = MembersRanks.Length - 1; i >= 0; i--)
            {
                int index = i;
                MembersRanks[i] = new MirDropDownBox()
                {
                    BackColour = i % 2 == 0 ? Color.FromArgb(255, 10, 10, 10) : Color.FromArgb(255, 15, 15, 15),
                    ForeColour = Color.White,
                    Parent = MembersPage,
                    Size = new Size(100, 14),
                    Location = new Point(24, 30 + (i * 15)),
                    Visible = false,
                    Enabled = false
                };
                MembersRanks[index].ValueChanged += (o, e) => OnNewRank(index, MembersRanks[index]._WantedIndex);
                MembersRanks[index].MouseWheel += MembersPanel_MouseWheel;
            }
            for (int i = 0; i < MembersName.Length; i++)
            {
                MembersName[i] = new MirLabel()
                {
                    BackColour = i % 2 == 0 ? Color.FromArgb(255, 05, 05, 05) : Color.FromArgb(255, 07, 07, 07),
                    ForeColour = Color.White,
                    Parent = MembersPage,
                    Size = new Size(100, 14),
                    Location = new Point(125, 30 + (i * 15)),
                    Visible = false,
                    Enabled = false,
                    Font = new Font(Settings.FontName, 7F)

                };
                MembersName[i].MouseWheel += MembersPanel_MouseWheel;
            }
            for (int i = 0; i < MembersStatus.Length; i++)
            {
                MembersStatus[i] = new MirLabel()
                {
                    BackColour = i % 2 == 0 ? Color.FromArgb(255, 10, 10, 10) : Color.FromArgb(255, 15, 15, 15),
                    ForeColour = Color.White,
                    Parent = MembersPage,
                    Size = new Size(100, 14),
                    Location = new Point(225, 30 + (i * 15)),
                    Visible = false,
                    Enabled = false,
                    Font = new Font(Settings.FontName, 7F)
                };
                MembersStatus[i].MouseWheel += MembersPanel_MouseWheel;
            }
            for (int i = 0; i < MembersDelete.Length; i++)
            {
                int index = i;
                MembersDelete[i] = new MirButton()
                {
                    Enabled = true,
                    Visible = true,
                    Location = new Point(210, 30 + (i * 15)),
                    Library = Libraries.Prguse,
                    Index = 917,
                    Parent = MembersPage
                };
                MembersDelete[index].MouseWheel += MembersPanel_MouseWheel;
                MembersDelete[index].Click += (o, e) => DeleteMember(index);
            }
            MembersUpButton = new MirButton
            {
                HoverIndex = 198,
                Index = 197,
                Visible = true,
                Library = Libraries.Prguse2,
                Location = new Point(337, 1),
                Size = new Size(16, 14),
                Parent = MembersPage,
                PressedIndex = 199,
                Sound = SoundList.ButtonA
            };
            MembersUpButton.Click += (o, e) =>
            {
                if (MemberScrollIndex == 0) return;
                MemberScrollIndex--;
                UpdateMembers();
                UpdateMembersScrollPosition();
            };
            MembersDownButton = new MirButton
            {
                HoverIndex = 208,
                Index = 207,
                Visible = true,
                Library = Libraries.Prguse2,
                Location = new Point(337, 318),
                Size = new Size(16, 14),
                Parent = MembersPage,
                PressedIndex = 209,
                Sound = SoundList.ButtonA
            };
            MembersDownButton.Click += (o, e) =>
            {
                if (MemberScrollIndex == MembersShowCount - MemberPageRows) return;
                MemberScrollIndex++;
                UpdateMembers();
                UpdateMembersScrollPosition();
            };

            MembersPositionBar = new MirButton
            {
                Index = 206,
                Library = Libraries.Prguse2,
                Location = new Point(337, 16),
                Parent = MembersPage,
                Movable = true,
                Sound = SoundList.None
            };
            MembersPositionBar.OnMoving += MembersPositionBar_OnMoving;

            MembersShowOfflineButton = new MirButton
            {
                Visible = true,
                Index = 1346,
                Library = Libraries.Prguse,
                Sound = SoundList.ButtonA,
                Parent = MembersPage,
                Location = new Point(230, 310)
            };
            MembersShowOfflineButton.Click += (o, e) => MembersShowOfflineSwitch();

            MembersShowOfflineStatus = new MirImageControl
            {
                Visible = true,
                Index = 1347,
                Library = Libraries.Prguse,
                Parent = MembersPage,
                Location = new Point(230, 310)
            };
            MembersShowOfflineStatus.Click += (o, e) => MembersShowOfflineSwitch();

            MembersShowOffline = new MirLabel
            {
                Visible = true,
                Text = "Show Offline",
                Location = new Point(245, 309),
                Parent = MembersPage,
                Size = new Size(150, 12),
                Font = new Font(Settings.FontName, 7F),
                ForeColour = Color.White
            };
            MembersPage.KeyDown += MembersPanel_KeyDown;
            MembersPage.MouseWheel += MembersPanel_MouseWheel;

            #endregion

            #region StatusDialogUI 
            StatusPage = new MirImageControl()
            {
                Parent = this,
                Size = new Size(230, 372),
                Location = new Point(355, 60),
                Visible = true
            };
            StatusPageBase = new MirImageControl()
            {
                Parent = StatusPage,
                Library = Libraries.Prguse,
                Index = 1850,
                Visible = true,
                Location = new Point(10, 2)
            };
            StatusPage.BeforeDraw += (o, e) =>
            {
                if (MapControl.User.GuildName == "")
                {
                    StatusGuildName.Text = "";
                    StatusLevel.Text = "";
                    StatusMembers.Text = "";
                }
                else
                {
                    StatusGuildName.Text = string.Format("{0}", MapObject.User.GuildName);
                    StatusLevel.Text = string.Format("{0}", Level);
                    StatusMembers.Text = string.Format("{0}{1}", MemberCount, MaxMembers == 0 ? "" : ("/" + MaxMembers.ToString()));
                }
            };
            StatusHeaders = new MirLabel()
            {
                Location = new Point(7, 47),
                DrawFormat = TextFormatFlags.Right,
                Size = new Size(75, 300),
                NotControl = true,
                Text = "Guild Name\n\nLevel\n\nMembers",
                Visible = true,
                Parent = StatusPage,
                ForeColour = Color.Gray,
            };
            StatusGuildName = new MirLabel()
            {
                Location = new Point(82, 47),
                Size = new Size(120, 200),
                NotControl = true,
                Text = "",
                Visible = true,
                Parent = StatusPage
            };
            StatusLevel = new MirLabel()
            {
                Location = new Point(82, 73),
                Size = new Size(120, 200),
                NotControl = true,
                Text = "",
                Visible = true,
                Parent = StatusPage
            };
            StatusMembers = new MirLabel()
            {
                Location = new Point(82, 99),
                Size = new Size(120, 200),
                NotControl = true,
                Text = "",
                Visible = true,
                Parent = StatusPage
            };
            StatusExpBar = new MirImageControl()
            {
                Visible = true,
                Index = 423,
                Library = Libraries.Prguse2,
                Location = new Point(322, 403),
                DrawImage = false,
                NotControl = true,
                Parent = this,
                Size = new Size(260, 15)
            };
            StatusExpBar.BeforeDraw += StatusExpBar_BeforeDraw;
            StatusExpLabel = new MirLabel()
            {
                Visible = true,
                DrawFormat = TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter,
                Location = new Point(322, 405),
                NotControl = true,
                Parent = this,
                Size = new Size(260, 15)
            };
            MembersRecruitName = new MirTextBox()
            {
                Location = new Point(40, 300),
                Size = new Size(130, 21),
                MaxLength = 20,
                Parent = StatusPage,
                Visible = true,
                Text = "",
                BackColour = Color.FromArgb(255, 25, 25, 25),
                Border = true,
                BorderColour = Color.FromArgb(255, 35, 35, 35),
                CanLoseFocus = true
            };
            RecruitMemberButton = new MirButton()
            {
                Parent = StatusPage,
                Enabled = true,
                Visible = true,
                Location = new Point(170, 298),
                Library = Libraries.Title,
                Index = 356,
                HoverIndex = 357,
                PressedIndex = 358
            };
            RecruitMemberButton.Click += (o, e) => AddMember();

            RecruitMemberLabel = new MirLabel()
            {
                Visible = true,
                Location = new Point(36, 283),
                NotControl = true,
                Parent = StatusPage,
                Text = "Recruit Member",
                Size = new Size(150, 15)
            };

            #endregion

            #region StorageDialogUI 
            StoragePage = new MirImageControl()
            {
                Parent = this,
                Size = new Size(352, 372),
                Location = new Point(0, 60),
                Visible = false
            };
            StoragePageBase = new MirImageControl()
            {
                Visible = true,
                Parent = StoragePage,
                Library = Libraries.Prguse,
                Index = 1851,
                Location = new Point(30, 19)
            };
            StoragePage.BeforeDraw += (o, e) =>
            {
                StorageGoldText.Text = Gold > 0 ? string.Format("{0:###,###,###}", Gold) : "0";
                if (MyRankId == 0)
                    StorageGoldRemove.Visible = true;
                else
                    StorageGoldRemove.Visible = false;

            };
            StorageGoldText = new MirLabel()
            {
                Parent = StoragePage,
                Size = new Size(125, 12),
                Location = new Point(194, 312),
                Visible = true,
                Text = "0",
                NotControl = true,
            };
            StorageGoldAdd = new MirButton()
            {
                Parent = StoragePage,
                Library = Libraries.Prguse,
                Index = 918,
                Visible = true,
                Enabled = true,
                Location = new Point(158, 313)
            };
            StorageGoldAdd.Click += (o, e) => StorageAddGold();

            StorageGoldRemove = new MirButton()
            {
                Parent = StoragePage,
                Library = Libraries.Prguse,
                Index = 917,
                Visible = true,
                Enabled = true,
                Location = new Point(142, 313)
            };
            StorageGoldRemove.Click += (o, e) => StorageRemoveGold();

            StorageGrid = new MirItemCell[8 * 14];
            {
                for (int x = 0; x < 8; x++)
                {
                    for (int y = 0; y < 14; y++)
                    {
                        int idx = 8 * y + x;
                        StorageGrid[idx] = new MirItemCell
                        {
                            ItemSlot = idx,
                            GridType = MirGridType.GuildStorage,
                            Library = Libraries.Items,
                            Parent = StoragePage,
                            Size = new Size(35, 35),
                            Location = new Point(x * 35 + 31 + x, y * 35 + 20 + y),
                        };
                        if (y > 7) StorageGrid[idx].Visible = false;
                        StorageGrid[idx].MouseWheel += StoragePanel_MouseWheel;
                    }
                }
            }

            StorageUpButton = new MirButton
            {
                HoverIndex = 198,
                Index = 197,
                Visible = true,
                Library = Libraries.Prguse2,
                Location = new Point(337, 1),
                Size = new Size(16, 14),
                Parent = StoragePage,
                PressedIndex = 199,
                Sound = SoundList.ButtonA
            };
            StorageUpButton.Click += (o, e) =>
            {
                if (StorageIndex == 0) return;
                StorageIndex--;
                UpdateStorage();
                StorageUpdatePositionBar();
            };

            StorageDownButton = new MirButton
            {
                HoverIndex = 208,
                Index = 207,
                Visible = true,
                Library = Libraries.Prguse2,
                Location = new Point(337, 318),
                Size = new Size(16, 14),
                Parent = StoragePage,
                PressedIndex = 209,
                Sound = SoundList.ButtonA
            };
            StorageDownButton.Click += (o, e) =>
            {
                if (StorageIndex >= 6) StorageIndex = 5;
                StorageIndex++;
                UpdateStorage();
                StorageUpdatePositionBar();
            };

            StoragePositionBar = new MirButton
            {
                Index = 206,
                Library = Libraries.Prguse2,
                Location = new Point(337, 16),
                Parent = StoragePage,
                Movable = true,
                Visible = true,
                Sound = SoundList.None
            };
            StoragePositionBar.OnMoving += StoragePositionBar_OnMoving;

            StoragePage.MouseWheel += StoragePanel_MouseWheel;
            StoragePageBase.MouseWheel += StoragePanel_MouseWheel;

            #endregion

            #region RankDialogUI
            RankPage = new MirImageControl()
            {
                Parent = this,
                Size = new Size(352, 372),
                Location = new Point(0, 60),
                Visible = false
            };
            RankPage.BeforeDraw += (o, e) => RequestUpdateMembers();
            RanksSelectTextL = new MirLabel()
            {
                Text = "Edit Rank",
                Location = new Point(42, 18),
                Size = new Size(150, 20),
                ForeColour = Color.White,
                Parent = RankPage,
                NotControl = true,

            };
            RanksSelectTextR = new MirLabel()
            {
                Text = "Select Rank",
                Location = new Point(198, 18),
                Size = new Size(150, 20),
                ForeColour = Color.White,
                Parent = RankPage,
                NotControl = true,

            };
            RanksSelectBox = new MirDropDownBox()
            {
                Parent = RankPage,
                Location = new Point(198, 36),
                Size = new Size(130, 16),
                ForeColour = Color.White,
                Visible = true,
                Enabled = true,
                BackColour = Color.FromArgb(255, 25, 25, 25),
                BorderColour = Color.FromArgb(255, 35, 35, 35),
            };
            RanksSelectBox.ValueChanged += (o, e) => OnRankSelect(RanksSelectBox._WantedIndex);

            RanksName = new MirTextBox()
            {
                Location = new Point(42, 36),
                Size = new Size(130, 16),
                MaxLength = 20,
                Parent = RankPage,
                Visible = true,
                Enabled = false,
                Text = "",
                Border = true,
                BackColour = Color.FromArgb(255, 25, 25, 25),
                BorderColour = Color.FromArgb(255, 35, 35, 35),
            };
            RanksName.BeforeDraw += (o, e) => RanksName_BeforeDraw();
            RanksName.TextBox.KeyPress += RanksName_KeyPress;
            RanksSaveName = new MirButton()
            {
                Location = new Point(155, 290),
                Enabled = false,
                Visible = true,
                Parent = RankPage,
                Index = 90,
                HoverIndex = 91,
                PressedIndex = 92,
                Library = Libraries.Title,
                Sound = SoundList.ButtonA
            };
            RanksSaveName.Click += (o, e) =>
            {
                RanksChangeName();
            };
            String[] Options = { "Edit ranks", "Recruit member", "Kick member", "Store item", "Retrieve item", "Alter alliance", "Change notice", "Activate Buff" };
            RanksOptionsButtons = new MirButton[8];
            RanksOptionsStatus = new MirImageControl[8];
            RanksOptionsTexts = new MirLabel[8];
            for (int i = 0; i < RanksOptionsButtons.Length; i++)
            {
                RanksOptionsButtons[i] = new MirButton()
                {
                    Visible = true,
                    Enabled = false,
                    Index = 1346,
                    Library = Libraries.Prguse,
                    Sound = SoundList.ButtonA,
                    Parent = RankPage,
                    Location = new Point(i % 2 == 0 ? 42 : 202, i % 2 == 0 ? 120 + (i * 20) : 120 + ((i - 1) * 20))
                };
                int index = i;
                RanksOptionsButtons[i].Click += (o, e) => SwitchRankOption(index);
            }
            for (int i = 0; i < RanksOptionsStatus.Length; i++)
            {
                RanksOptionsStatus[i] = new MirImageControl()
                {
                    Visible = false,
                    Index = 1347,
                    Library = Libraries.Prguse,
                    Parent = RankPage,
                    NotControl = true,
                    Location = new Point(i % 2 == 0 ? 42 : 202, i % 2 == 0 ? 120 + (i * 20) : 120 + ((i - 1) * 20))
                };
                int index = i;
                RanksOptionsStatus[i].Click += (o, e) => SwitchRankOption(index);
            }
            for (int i = 0; i < RanksOptionsTexts.Length; i++)
            {
                RanksOptionsTexts[i] = new MirLabel()
                {
                    Visible = true,
                    NotControl = true,
                    Parent = RankPage,
                    Location = new Point(17 + (i % 2 == 0 ? 42 : 202), i % 2 == 0 ? 118 + (i * 20) : 118 + ((i - 1) * 20)),
                    AutoSize = true,
                    Text = Options[i]
                };
            }

            #endregion

            #region BuffDialogUI

            BuffPage = new MirImageControl()
            {
                Parent = this,
                Size = new Size(352, 372),
                Location = new Point(360, 61),
                Index = 1853,
                Library = Libraries.Prguse,
                Visible = false
            };
            BuffPage.MouseWheel += BuffsPanel_MouseWheel;


            Buffs = new GuildBuffButton[8];
            for (byte i = 0; i < Buffs.Length; i++)
            {
                byte Id = i;
                Buffs[i] = new GuildBuffButton { Parent = BuffPage, Visible = true, Location = new Point(4, 27 + (i * 38)), Id = Id };
                Buffs[i].Click += (o, e) => RequestBuff(Id);
                Buffs[i].MouseWheel += BuffsPanel_MouseWheel;
            }

            PointsLeft = new MirLabel
            {
                DrawFormat = TextFormatFlags.HorizontalCenter,
                Parent = BuffPage,
                Location = new Point(118, 3),
                Size = new Size(100, 20),
                NotControl = true
            };

            UpButton = new MirButton
            {
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Library = Libraries.Prguse2,
                Location = new Point(203, 24),
                Parent = BuffPage,
                Sound = SoundList.ButtonA
            };
            UpButton.Click += (o, e) =>
            {
                if (StartIndex == 0) return;
                StartIndex--;
                UpdatePositionBar();
                RefreshInterface();
            };
            DownButton = new MirButton
            {
                Index = 207,
                HoverIndex = 208,
                PressedIndex = 209,
                Library = Libraries.Prguse2,
                Location = new Point(203, 317),
                Parent = BuffPage,
                Sound = SoundList.ButtonA
            };
            DownButton.Click += (o, e) =>
            {
                if (GuildBuffInfos.Count < 8) return;
                if (StartIndex == GuildBuffInfos.Count - 8) return;
                StartIndex++;
                UpdatePositionBar();
                RefreshInterface();
            };

            PositionBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                PressedIndex = 206,
                Library = Libraries.Prguse2,
                Parent = BuffPage,
                Movable = true,
                Sound = SoundList.None,
                Location = new Point(203, 39)
            };
            PositionBar.OnMoving += PositionBar_OnMoving;
            PositionBar.MouseUp += (o, e) => RefreshInterface();

            #endregion
        }
        #endregion

        public void RequestBuff(byte Id)
        {
            if ((Id + StartIndex) > GuildBuffInfos.Count) return;
            GuildBuffInfo BuffInfo = GuildBuffInfos[Id + StartIndex];
            if (BuffInfo == null) return;
            GuildBuff Buff = FindGuildBuff(BuffInfo.Id);
            if (Buff == null)
            {
                string Error = "";
                if (GameScene.Scene.GuildDialog.SparePoints < BuffInfo.PointsRequirement)
                    Error = "Insufficient points available.";
                if (GameScene.Scene.GuildDialog.Level < BuffInfo.LevelRequirement)
                    Error = "Guild level too low.";
                if (!GameScene.Scene.GuildDialog.GetMyOptions().HasFlag(GuildRankOptions.CanActivateBuff))
                    Error = "Guild rank does not allow buff activation.";
                if (Error != "")
                {
                    MirMessageBox messageBox = new MirMessageBox(Error);
                    messageBox.Show();
                    return;
                }
                if (CMain.Time < LastRequest + 100) return;
                LastRequest = CMain.Time;
                Network.Enqueue(new C.GuildBuffUpdate { Action = 1, Id = BuffInfo.Id });
            }
            else
            {
                string Error = "";
                if (Buff.Active)
                    Error = "Buff is still active.";
                if (GameScene.Scene.GuildDialog.Gold < BuffInfo.ActivationCost)
                    Error = "Insufficient guild funds.";
                if (!GameScene.Scene.GuildDialog.GetMyOptions().HasFlag(GuildRankOptions.CanActivateBuff))
                    Error = "Guild rank does not allow buff activation.";
                if (Error != "")
                {
                    MirMessageBox messageBox = new MirMessageBox(Error);
                    messageBox.Show();
                    return;
                }
                if (CMain.Time < LastRequest + 100) return;
                LastRequest = CMain.Time;
                Network.Enqueue(new C.GuildBuffUpdate { Action = 2, Id = BuffInfo.Id });
            }
        }

        public GuildBuff FindGuildBuff(int Index)
        {
            for (int i = 0; i < EnabledBuffs.Count; i++)
            {
                if ((EnabledBuffs[i] != null) && (EnabledBuffs[i].Id == Index))
                    return EnabledBuffs[i];
            }
            return null;
        }

        private void UpdatePositionBar()
        {
            int h = 277 - PositionBar.Size.Height;
            h = (int)((h / (float)(GuildBuffInfos.Count - 8)) * StartIndex);
            PositionBar.Location = new Point(203, 39 + h);
        }

        private void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            const int x = 203;
            int y = PositionBar.Location.Y;
            if (y >= 296) y = 296;
            if (y < 39) y = 39;

            int h = 277 - PositionBar.Size.Height;
            h = (int)Math.Round(((y - 39) / (h / (float)(GuildBuffInfos.Count - 8))));
            PositionBar.Location = new Point(x, y);
            if (h == StartIndex) return;
            StartIndex = h;
            RefreshInterface();
        }

        private void BuffsPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (StartIndex == 0 && count >= 0) return;
            if (StartIndex == (GuildBuffInfos.Count - 8) && count <= 0) return;
            StartIndex -= count > 0 ? 1 : -1;
            if (GuildBuffInfos.Count <= 8) StartIndex = 0;
            UpdatePositionBar();
            RefreshInterface();
        }

        public void RefreshInterface()
        {
            if (StartIndex < 0) StartIndex = 0;

            if (GuildBuffInfos.Count == 0) BuffButton.Visible = false;
            else BuffButton.Visible = true;

            if (MapObject.User.GuildName == "")
            {
                Hide();
                return;
            }
            else
            {
                PointsLeft.Text = GameScene.Scene.GuildDialog.SparePoints.ToString();
                for (int i = 0; i < Buffs.Length; i++)
                {
                    if ((StartIndex + i) > GuildBuffInfos.Count - 1)
                    {
                        Buffs[i].Visible = false;
                        continue;
                    }
                    GuildBuffInfo BuffInfo = GuildBuffInfos[i + StartIndex];
                    if (BuffInfo == null)
                    {
                        Buffs[i].Visible = false;
                        continue;
                    }
                    Buffs[i].Visible = true;
                    GuildBuff Buff = FindGuildBuff(BuffInfo.Id);
                    Buffs[i].Name.Text = BuffInfo.Name;
                    Buffs[i].Icon.Index = BuffInfo.Icon;

                    if (Buff == null)
                    {
                        if (BuffInfo.LevelRequirement > GameScene.Scene.GuildDialog.Level)
                        {
                            Buffs[i].Info.Text = "Insufficient Level";
                            Buffs[i].Info.ForeColour = Color.Red;
                            Buffs[i].Icon.Index += 2;
                        }
                        else
                        {
                            Buffs[i].Info.Text = "Available";
                            Buffs[i].Info.ForeColour = Buffs[i].Name.ForeColour;
                            Buffs[i].Icon.Index += 2;
                        }
                        Buffs[i].Obtained.Text = "";
                    }
                    else
                    {
                        if (BuffInfo.TimeLimit > 0)
                        {
                            if (Buff.Active)
                                Buffs[i].Info.Text = "Counting down.";
                            else
                                Buffs[i].Info.Text = "Expired.";
                        }
                        else
                            Buffs[i].Info.Text = "Obtained.";
                        Buffs[i].Info.ForeColour = Buffs[i].Name.ForeColour;
                        if (Buff.Active)
                        {
                            Buffs[i].Obtained.Text = "Active";
                            Buffs[i].Icon.Index += 1;
                        }
                        else
                            Buffs[i].Obtained.Text = "Inactive";
                    }
                }
            }
        }

        public GuildBuffInfo FindGuildBuffInfo(int Index)
        {
            if (!RequestedList)
            {
                RequestGuildBuffList();
            }
            for (int i = 0; i < GuildBuffInfos.Count; i++)
            {
                if (GuildBuffInfos[i].Id == Index)
                    return GuildBuffInfos[i];
            }
            return null;
        }

        public void RequestGuildBuffList()
        {
            if (!RequestedList)
            {
                RequestedList = true;
                Network.Enqueue(new C.GuildBuffUpdate { Action = 0 });
            }
        }

        public void UpdateActiveStats()
        {
            string text = "";

            var stats = new Stats();

            foreach (GuildBuff buff in EnabledBuffs)
            {
                if ((buff.Info == null) || (!buff.Active)) continue;

                stats.Add(buff.Info.Stats);
            }

            foreach (var val in stats.Values)
            {
                var c = val.Value < 0 ? "Decreases" : "Increases";

                var txt = $"{c} {val.Key} by: {val.Value}{(val.Key.ToString().Contains("Percent") ? "%" : "")}.\n";

                text += txt;
            }

            ActiveStats = text;
        }

        public void CreateHintLabel(byte Id)
        {
            GameScene.Scene.GuildBuffLabel = new MirControl
            {
                BackColour = Color.FromArgb(255, 50, 50, 50),
                Border = true,
                BorderColour = Color.Gray,
                NotControl = true,
                Parent = GameScene.Scene,
                Opacity = 0.7F,
                DrawControlTexture = true
            };
            if (Id + StartIndex > GuildBuffInfos.Count) return;

            GuildBuffInfo Buff = GuildBuffInfos[Id + StartIndex];
            if (Buff == null) return;
            MirLabel HintName = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Location = new Point(4, 4),
                OutLine = true,
                Parent = GameScene.Scene.GuildBuffLabel,
                Text = Buff.Name
            };

            GameScene.Scene.GuildBuffLabel.Size = new Size(Math.Max(GameScene.Scene.GuildBuffLabel.Size.Width, HintName.DisplayRectangle.Right + 4),
                Math.Max(GameScene.Scene.GuildBuffLabel.Size.Height, HintName.DisplayRectangle.Bottom));

            string ReqText = "";
            if (Buff.LevelRequirement > 0)
            {
                ReqText += "Minimum Guild Level: " + Buff.LevelRequirement.ToString();
            }
            if (Buff.PointsRequirement > 0)
            {
                if (ReqText != "") ReqText += "\n";
                ReqText += "Points Required: " + Buff.PointsRequirement.ToString();
            }
            if (Buff.ActivationCost > 0)
            {
                if (ReqText != "") ReqText += "\n";
                ReqText += "Activation Cost: " + Buff.ActivationCost.ToString() + " gold.";
                //if (ReqText != "") ReqText += "\n";
            }


            MirLabel RequiredLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Location = new Point(4, GameScene.Scene.GuildBuffLabel.DisplayRectangle.Bottom),
                OutLine = true,
                Parent = GameScene.Scene.GuildBuffLabel,
                Text = ReqText
            };
            GameScene.Scene.GuildBuffLabel.Size = new Size(Math.Max(GameScene.Scene.GuildBuffLabel.Size.Width, RequiredLabel.DisplayRectangle.Right + 4),
                Math.Max(GameScene.Scene.GuildBuffLabel.Size.Height, RequiredLabel.DisplayRectangle.Bottom));

            //code to dispay the buffs duration
            if (Buff.TimeLimit > 0)
            {
                GuildBuff activeBuff = FindGuildBuff(Buff.Id);

                string text = "";

                if (activeBuff != null && activeBuff.Active)
                {
                    text = string.Format("Time Remaining: {0} minutes", activeBuff.ActiveTimeRemaining);
                }
                else
                {
                    text = string.Format("Buff Length: {0} minutes.", Buff.TimeLimit.ToString());
                }
                MirLabel TimeLabel = new MirLabel
                {
                    AutoSize = true,
                    ForeColour = Color.White,
                    Location = new Point(4, GameScene.Scene.GuildBuffLabel.DisplayRectangle.Bottom),
                    OutLine = true,
                    Parent = GameScene.Scene.GuildBuffLabel,
                    Text = text
                };
                GameScene.Scene.GuildBuffLabel.Size = new Size(Math.Max(GameScene.Scene.GuildBuffLabel.Size.Width, TimeLabel.DisplayRectangle.Right + 4),
                    Math.Max(GameScene.Scene.GuildBuffLabel.Size.Height, TimeLabel.DisplayRectangle.Bottom));
            }


            //code to display the buff's effect
            MirLabel InfoLabel = new MirLabel
            {
                AutoSize = true,
                ForeColour = Color.White,
                Location = new Point(4, GameScene.Scene.GuildBuffLabel.DisplayRectangle.Bottom),
                OutLine = true,
                Parent = GameScene.Scene.GuildBuffLabel,
                Text = Buff.ShowStats()
            };
            GameScene.Scene.GuildBuffLabel.Size = new Size(Math.Max(GameScene.Scene.GuildBuffLabel.Size.Width, InfoLabel.DisplayRectangle.Right + 4),
                Math.Max(GameScene.Scene.GuildBuffLabel.Size.Height, InfoLabel.DisplayRectangle.Bottom));

        }

        #region ButtonResets
        public void ResetButtonStats()
        {
            if (MyOptions.HasFlag(GuildRankOptions.CanRetrieveItem) || MyOptions.HasFlag(GuildRankOptions.CanStoreItem))
                StorageButton.Visible = true;
            else
                StorageButton.Visible = false;

            if (MyOptions.HasFlag(GuildRankOptions.CanChangeRank))
                RankButton.Visible = true;
            else
                RankButton.Visible = false;

            if (MyOptions.HasFlag(GuildRankOptions.CanChangeNotice))
                NoticeEditButton.Visible = true;
            else
                NoticeEditButton.Visible = false;

            BuffButton.Visible = true;
        }
        #endregion

        #region NoticeDialogCode
        #region NoticeButtonStates
        public void EditNotice()
        {
            if (Notice.Enabled == false)
            {
                Notice.Enabled = true;
                Notice.SetFocus();
                NoticeEditButton.Visible = false;
                NoticeSaveButton.Visible = true;
            }
            else
            {
                Notice.Enabled = false;
                NoticeEditButton.Index = 560;
                NoticeEditButton.Visible = true;
                NoticeSaveButton.Visible = false;
                Network.Enqueue(new C.EditGuildNotice() { notice = Notice.MultiText.ToList() });
            }
        }
        public void NoticeChange(List<string> newnotice)
        {
            NoticeEditButton.Index = 560;
            Notice.Enabled = false;
            NoticeScrollIndex = 0;
            Notice.Text = "";
            Notice.MultiText = newnotice.ToArray();
            NoticeChanged = false;
            UpdateNotice();
        }
        #endregion

        #region AssholeNoticeScroller
        public void UpdateNotice(bool forward = false)
        {
            int NoticeScrollerIndex = NoticeScrollIndex;

            if (forward) NoticeScrollerIndex += 24;

            if (NoticeScrollerIndex >= Notice.MultiText.Length) NoticeScrollerIndex = Notice.MultiText.Length - 24;
            if (NoticeScrollerIndex < 0) NoticeScrollerIndex = 0;
            if (Notice.MultiText.Length != 0)
            {
                Notice.TextBox.SelectionLength = 1;
                Notice.TextBox.SelectionStart = Notice.TextBox.GetFirstCharIndexFromLine(NoticeScrollerIndex);
                Notice.TextBox.ScrollToCaret();
            }
        }

        private void UpdateNoticeScrollPosition()
        {
            int interval = 289 / (Notice.MultiText.Length - 25);

            int x = 337;
            int y = 16 + (NoticeScrollIndex * interval);

            if (y >= NoticeDownButton.Location.Y - 20) y = NoticeDownButton.Location.Y - 20;
            if (y <= 16) y = 16;

            NoticePositionBar.Location = new Point(x, y);
        }

        void NoticePositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            //int x = 337;
            int y = NoticePositionBar.Location.Y;
            int TempIndex = 0;
            bool forward;

            if (y < 16) y = 16;

            int location = y - 16;
            int interval = 289 / (Notice.MultiText.Length - 25);

            double yPoint = location / interval;

            TempIndex = Convert.ToInt16(Math.Floor(yPoint));

            if (NoticeScrollIndex < TempIndex)
            {
                forward = true;
                NoticeScrollIndex = Convert.ToInt16(Math.Floor(yPoint));
            }
            else
            {
                forward = false;
                NoticeScrollIndex = Convert.ToInt16(Math.Floor(yPoint));
            }

            if (NoticeScrollIndex >= Notice.MultiText.Length - 25) NoticeScrollIndex = Notice.MultiText.Length - 25;
            if (NoticeScrollIndex <= 0) NoticeScrollIndex = 0;

            UpdateNotice(forward);

            UpdateNoticeScrollPosition();
        }
        private void NoticePanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (NoticeScrollIndex == 0 && count >= 0) return;
            if (NoticeScrollIndex == Notice.MultiText.Length - 25 && count <= 0) return;

            if (count > 0)
            {
                NoticeScrollIndex--;
                UpdateNotice();
            }
            else
            {
                NoticeScrollIndex++;
                UpdateNotice(true);
            }


            UpdateNoticeScrollPosition();
        }

        private void NoticePanel_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (NoticeScrollIndex == 0) break;
                    if (NoticeScrollIndex >= 25) NoticeScrollIndex -= 24;
                    NoticeScrollIndex--;
                    break;
                case Keys.Home:
                    if (NoticeScrollIndex == 0) break;
                    NoticeScrollIndex = 0;
                    break;
                case Keys.Down:
                    if (NoticeScrollIndex == Notice.MultiText.Length - 1) break;
                    if (NoticeScrollIndex < 25) NoticeScrollIndex = 24;
                    NoticeScrollIndex++;
                    break;
                case Keys.End:
                    if (NoticeScrollIndex == Notice.MultiText.Length - 1) break;
                    NoticeScrollIndex = Notice.MultiText.Length - 1;
                    break;
                case Keys.PageUp:
                    if (NoticeScrollIndex == 0) break;
                    NoticeScrollIndex -= 25;
                    break;
                case Keys.PageDown:
                    if (NoticeScrollIndex == Notice.MultiText.Length - 25) break;
                    NoticeScrollIndex += 25;
                    break;
                default:
                    return;
            }
            UpdateNotice();
            e.Handled = true;
        }
        #endregion
        #endregion

        #region MembersDialogCode
        public void NewMembersList(List<GuildRank> NewRanks)
        {
            Ranks = NewRanks;
            MembersChanged = false;
            RefreshMemberList();
        }
        public void RefreshMemberList()
        {
            MemberScrollIndex = 0;
            List<string> RankNames = new List<string>();
            for (int i = 0; i < Ranks.Count; i++)
            {
                if (Ranks[i] != null)
                {
                    int index = i;
                    Ranks[i].Index = index;
                    RankNames.Add(Ranks[i].Name);
                }
                else
                    RankNames.Add("Missing Rank");
            }
            for (int i = 0; i < MembersRanks.Length; i++)
            {
                MembersRanks[i].Items = RankNames;
                MembersRanks[i].MinimumOption = MyRankId;
            }
            RanksSelectBox.Items = RankNames.ToList();
            RanksSelectBox.MinimumOption = 0;
            if (RankNames.Count < 255)
                RanksSelectBox.Items.Add("Add New");
            UpdateMembers();
            UpdateRanks();
        }

        public void MemberStatusChange(string name, bool online)
        {
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                    if (Ranks[i].Members[j].Name == name)
                        Ranks[i].Members[j].Online = online;
            UpdateMembers();
        }

        public void OnNewRank(int Index, int SelectedIndex)
        {
            if (SelectedIndex >= Ranks.Count) return;
            if (LastGuildMsg > CMain.Time) return;
            MirMessageBox messageBox = new MirMessageBox(string.Format("Are you sure you want to change the rank of {0} to {1}?", MembersName[Index].Text, Ranks[SelectedIndex].Name), MirMessageBoxButtons.YesNo);

            messageBox.YesButton.Click += (o, a) =>
            {
                Network.Enqueue(new C.EditGuildMember { ChangeType = 2, Name = MembersName[Index].Text, RankIndex = (byte)Ranks[SelectedIndex].Index, RankName = Ranks[SelectedIndex].Name });
                LastGuildMsg = CMain.Time + 5000;
            };
            messageBox.Show();
        }

        public void AddMember()
        {
            if (!MyOptions.HasFlag(GuildRankOptions.CanRecruit)) return;
            if (LastGuildMsg > CMain.Time) return;
            Network.Enqueue(new C.EditGuildMember { ChangeType = 0, Name = MembersRecruitName.Text });
            LastGuildMsg = CMain.Time + 5000;
            MembersRecruitName.Text = "";
        }

        public void DeleteMember(int Index)
        {
            if (MembersName[Index].Text == MapControl.User.Name) return;
            if (LastGuildMsg > CMain.Time) return;
            MirMessageBox messageBox = new MirMessageBox(string.Format("Are you sure you want to kick {0}?", MembersName[Index].Text), MirMessageBoxButtons.YesNo);

            messageBox.YesButton.Click += (o, a) =>
            {
                Network.Enqueue(new C.EditGuildMember { ChangeType = 1, Name = MembersName[Index].Text });
                LastGuildMsg = CMain.Time + 5000;
            };
            messageBox.Show();
        }

        public void UpdateMembers()
        {
            MembersShowCount = 0;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if (MembersShowOfflinesetting || Ranks[i].Members[j].Online) MembersShowCount++;
                }
            if (MembersShowCount < MemberPageRows)
                MemberScrollIndex = 0;


            for (int i = 0; i < MembersRanks.Length; i++)
            {
                if (MembersShowCount > i)
                {
                    MembersRanks[i].Visible = true;
                }
                else
                    MembersRanks[i].Visible = false;
            }
            for (int i = 0; i < MembersName.Length; i++)
            {
                if (MembersShowCount > i)
                    MembersName[i].Visible = true;
                else
                    MembersName[i].Visible = false;
            }
            for (int i = 0; i < MembersStatus.Length; i++)
            {
                if (MembersShowCount > i)
                    MembersStatus[i].Visible = true;
                else
                    MembersStatus[i].Visible = false;
            }
            for (int i = 0; i < MembersDelete.Length; i++)
            {
                MembersDelete[i].Visible = false;
            }
            if (MyOptions.HasFlag(GuildRankOptions.CanRecruit))
            {
                RecruitMemberButton.Visible = true;
                MembersRecruitName.Visible = true;
                RecruitMemberLabel.Visible = true;
            }
            else
            {
                RecruitMemberButton.Visible = false;
                MembersRecruitName.Visible = false;
                RecruitMemberLabel.Visible = false;
            }

            int Offset = 0;
            int RowCount = 0;
            DateTime now = CMain.Now;
            for (int i = 0; i < Ranks.Count; i++)
                for (int j = 0; j < Ranks[i].Members.Count; j++)
                {
                    if (Offset < MemberScrollIndex)
                    {
                        if ((MembersShowOfflinesetting) || (Ranks[i].Members[j].Online))
                            Offset++;
                    }
                    else
                    {

                        if ((!MembersShowOfflinesetting) && (Ranks[i].Members[j].Online == false)) continue;
                        if ((MyOptions.HasFlag(GuildRankOptions.CanChangeRank)) && (Ranks[i].Index >= MyRankId))
                            MembersRanks[RowCount].Enabled = true;
                        else
                            MembersRanks[RowCount].Enabled = false;
                        if ((MyOptions.HasFlag(GuildRankOptions.CanKick)) && (Ranks[i].Index >= MyRankId) && (Ranks[i].Members[j].Name != MapControl.User.Name)/* && (Ranks[i].Index != 0)*/)
                            MembersDelete[RowCount].Visible = true;
                        else
                            MembersDelete[RowCount].Visible = false;
                        MembersRanks[RowCount].SelectedIndex = Ranks[i].Index;
                        MembersName[RowCount].Text = Ranks[i].Members[j].Name;
                        if (Ranks[i].Members[j].Online)
                            MembersStatus[RowCount].ForeColour = Color.LimeGreen;
                        else
                            MembersStatus[RowCount].ForeColour = Color.White;
                        TimeSpan Diff = now - Ranks[i].Members[j].LastLogin.ToLocalTime();
                        string text;
                        if (Ranks[i].Members[j].Online)
                            text = "Online";
                        else
                        {
                            switch (Diff.Days)
                            {
                                case 0:
                                    text = "Today";
                                    break;
                                case 1:
                                    text = "Yesterday";
                                    break;
                                default:
                                    text = Diff.Days + "Days ago";
                                    break;
                            }
                        }
                        MembersStatus[RowCount].Text = text;
                        RowCount++;
                        if (RowCount > MemberPageRows - 1) return;
                    }
                }
        }
        public void MembersShowOfflineSwitch()
        {
            if (MembersShowOfflinesetting)
            {
                MembersShowOfflinesetting = false;
                MembersShowOfflineStatus.Visible = false;
            }
            else
            {
                MembersShowOfflinesetting = true;
                MembersShowOfflineStatus.Visible = true;
            }
            UpdateMembers();
        }
        public void MembersPositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int x = 337;
            int y = MembersPositionBar.Location.Y;

            if (y >= MembersDownButton.Location.Y - 20) y = MembersDownButton.Location.Y - 20;
            if (y < 16) y = 16;

            int location = y - 16;
            int interval = 289 / (MembersShowCount - MemberPageRows);

            double yPoint = location / interval;

            MemberScrollIndex = Convert.ToInt16(Math.Floor(yPoint));
            if (MemberScrollIndex > MembersShowCount - MemberPageRows) MemberScrollIndex = MembersShowCount - MemberPageRows;
            if (MemberScrollIndex <= 0) MemberScrollIndex = 0;

            UpdateMembers();

            MembersPositionBar.Location = new Point(x, y);
        }
        private void UpdateMembersScrollPosition()
        {
            int interval = 289 / (MembersShowCount - MemberPageRows);

            int x = 337;
            int y = 16 + (MemberScrollIndex * interval);

            if (y >= MembersDownButton.Location.Y - 20) y = MembersDownButton.Location.Y - 20;
            if (y <= 16) y = 16;

            MembersPositionBar.Location = new Point(x, y);
        }
        private void MembersPanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (MemberScrollIndex == 0 && count >= 0) return;
            if (MemberScrollIndex == MembersShowCount - MemberPageRows && count <= 0) return;
            MemberScrollIndex -= count > 0 ? 1 : -1;
            UpdateMembers();
            UpdateMembersScrollPosition();
        }

        private void MembersPanel_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Up:
                    if (MemberScrollIndex == 0) break;
                    MemberScrollIndex--;
                    break;
                case Keys.Home:
                    if (MemberScrollIndex == 0) break;
                    MemberScrollIndex = 0;
                    break;
                case Keys.Down:
                    if (MembersShowCount < MemberPageRows) break;
                    if (MemberScrollIndex == MembersShowCount - MemberPageRows) break;
                    MemberScrollIndex++;
                    break;
                case Keys.End:
                    if (MembersShowCount < MemberPageRows) break;
                    if (MemberScrollIndex == MembersShowCount - MemberPageRows) break;
                    MemberScrollIndex = MembersShowCount - MemberPageRows;
                    break;
                case Keys.PageUp:
                    if (MemberScrollIndex == 0) break;
                    MemberScrollIndex -= 25;
                    if (MemberScrollIndex < 0) MemberScrollIndex = 0;
                    break;
                case Keys.PageDown:
                    if (MembersShowCount < MemberPageRows) break;
                    if (MemberScrollIndex == MembersShowCount - 25) break;
                    MemberScrollIndex += 25;
                    if (MemberScrollIndex > (MembersShowCount - MemberPageRows)) MemberScrollIndex = MembersShowCount - MemberPageRows;
                    break;
                default:
                    return;
            }
            UpdateMembers();
            UpdateMembersScrollPosition();
            e.Handled = true;
        }
        #endregion

        #region StatusDialogCode
        private void StatusExpBar_BeforeDraw(object sender, EventArgs e)
        {
            if (GameScene.Scene.GuildDialog.MaxExperience == 0)
            {
                StatusExpLabel.Text = "";
                return;
            }
            if (StatusExpBar.Library == null) return;

            StatusExpBar.Library.Draw(424, StatusExpBar.DisplayLocation, new Size(260, 22), Color.White);

            double percent = GameScene.Scene.GuildDialog.Experience / (double)GameScene.Scene.GuildDialog.MaxExperience;
            StatusExpLabel.Text = string.Format("{0:#0.##%}", percent);
            if (percent > 1) percent = 1;
            if (percent <= 0) return;
            Rectangle section = new Rectangle
            {
                Size = new Size((int)((260 - 3) * percent), 22)
            };

            StatusExpBar.Library.Draw(StatusExpBar.Index, section, StatusExpBar.DisplayLocation, Color.White, false);

        }
        #endregion

        #region RankDialogCode
        public void NewRankRecieved(GuildRank New)
        {
            int NewIndex = Ranks.Count > 1 ? Ranks.Count - 1 : 1;
            Ranks.Insert(NewIndex, New);
            Ranks[Ranks.Count - 1].Index = Ranks.Count - 1;
            RefreshMemberList();
            UpdateRanks();
        }
        public void MyRankChanged(GuildRank New)
        {
            MyOptions = New.Options;

            MapObject.User.GuildRankName = New.Name;
            GuildMember Member = null;
            int OldRank = MyRankId;
            MyRankId = New.Index;
            if (OldRank >= Ranks.Count) return;
            for (int i = 0; i < Ranks[OldRank].Members.Count; i++)
                if (Ranks[OldRank].Members[i].Name == MapObject.User.Name)
                {
                    Member = Ranks[OldRank].Members[i];
                    Ranks[OldRank].Members.Remove(Member);
                    break;
                }

            if (Member == null) return;
            if (Ranks.Count <= New.Index)
            {
                Ranks[MyRankId].Members.Add(Member);
                MembersChanged = true;
                return;
            }
            Ranks[New.Index].Members.Add(Member);

            ResetButtonStats();
            UpdateMembers();

        }
        public void RankChangeRecieved(GuildRank New)
        {
            for (int i = 0; i < Ranks.Count; i++)
                if (Ranks[i].Index == New.Index)
                {
                    if (Ranks[i].Name == MapObject.User.GuildRankName)
                        for (int j = 0; j < Ranks[i].Members.Count; j++)
                            if (Ranks[i].Members[j].Name == MapObject.User.Name)
                            {
                                MapObject.User.GuildRankName = New.Name;
                                MyOptions = New.Options;
                                ResetButtonStats();
                                UpdateMembers();
                            }
                    if (Ranks[i].Name == New.Name)
                    {
                        Ranks[i] = New;
                    }
                    else
                    {
                        Ranks[i] = New;
                        RefreshMemberList();
                    }
                }
            UpdateRanks();
        }
        public void UpdateRanks()
        {
            if ((RanksSelectBox.SelectedIndex == -1) || (Ranks[RanksSelectBox.SelectedIndex].Index < MyRankId))
            {
                for (int i = 0; i < RanksOptionsButtons.Length; i++)
                    RanksOptionsButtons[i].Enabled = false;
                for (int i = 0; i < RanksOptionsStatus.Length; i++)
                    RanksOptionsStatus[i].Enabled = false;
                if (RanksSelectBox.SelectedIndex == -1)
                    for (int i = 0; i < RanksOptionsStatus.Length; i++)
                        RanksOptionsStatus[i].Visible = false;
                LastRankNameChange = long.MaxValue;
                RanksName.Text = "";
                return;
            }
            RanksName.Text = Ranks[RanksSelectBox.SelectedIndex].Name;
            if (Ranks[RanksSelectBox.SelectedIndex].Index >= MyRankId)
                LastRankNameChange = 0;
            else
                LastRankNameChange = long.MaxValue;
            for (int i = 0; i < RanksOptionsStatus.Length; i++)
            {
                if (Ranks[RanksSelectBox.SelectedIndex].Options.HasFlag((GuildRankOptions)(1 << i)))
                    RanksOptionsStatus[i].Visible = true;
                else
                    RanksOptionsStatus[i].Visible = false;
                if (Ranks[RanksSelectBox.SelectedIndex].Index >= MyRankId)
                    RanksOptionsButtons[i].Enabled = true;
                else
                    RanksOptionsButtons[i].Enabled = false;
            }
        }
        public void OnRankSelect(int Index)
        {
            if (Index < Ranks.Count)
                RanksSelectBox.SelectedIndex = Index;
            else
            {
                if (Ranks.Count == 255) return;
                if (LastGuildMsg > CMain.Time) return;
                MirMessageBox messageBox = new MirMessageBox("Are you sure you want to create a new rank?", MirMessageBoxButtons.YesNo);
                messageBox.YesButton.Click += (o, a) =>
                {
                    Network.Enqueue(new C.EditGuildMember { ChangeType = 4, RankName = String.Format("Rank-{0}", Ranks.Count - 1) });
                    LastGuildMsg = CMain.Time + 5000;
                };
                messageBox.Show();
            }
            UpdateRanks();
        }
        public void SwitchRankOption(int OptionIndex)
        {
            if ((RanksSelectBox.SelectedIndex == -1) || (RanksSelectBox.SelectedIndex >= Ranks.Count)) return;
            if (LastGuildMsg > CMain.Time) return;
            Network.Enqueue(new C.EditGuildMember { ChangeType = 5, RankIndex = (byte)Ranks[RanksSelectBox.SelectedIndex].Index, RankName = OptionIndex.ToString(), Name = RanksOptionsStatus[OptionIndex].Visible ? "false" : "true" });
            LastGuildMsg = CMain.Time + 300;
        }
        public void RanksChangeName()
        {
            if (!string.IsNullOrEmpty(RanksName.Text))
            {
                Network.Enqueue(new C.EditGuildMember { ChangeType = 3, RankIndex = (byte)Ranks[RanksSelectBox.SelectedIndex].Index, RankName = RanksName.Text });
                LastRankNameChange = CMain.Time + 5000;
                RanksName.Enabled = false;
            }
        }
        public void RanksName_KeyPress(object sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case (char)'\\':
                    e.Handled = true;
                    break;
                case (char)Keys.Enter:
                    e.Handled = true;
                    RanksChangeName();
                    break;
                case (char)Keys.Escape:
                    e.Handled = true;
                    UpdateRanks();
                    break;
            }
        }

        public void RanksName_BeforeDraw()
        {
            if (LastRankNameChange < CMain.Time)
            {
                RanksName.Enabled = true;
                RanksSaveName.Enabled = true;
            }
            else
            {
                RanksName.Enabled = false;
                RanksSaveName.Enabled = false;
            }
        }

        #endregion

        #region StorageCode

        public void StoragePositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int x = 337;
            int y = StoragePositionBar.Location.Y;

            if (y >= StorageDownButton.Location.Y - 20) y = StorageDownButton.Location.Y - 20;
            if (y < 16) y = 16;

            int location = y - 16;
            int interval = 289 / 8;

            double yPoint = location / interval;

            StorageIndex = Convert.ToInt16(Math.Floor(yPoint));
            if (StorageIndex > 6) StorageIndex = 6;
            if (StorageIndex <= 0) StorageIndex = 0;
            UpdateStorage();

            StoragePositionBar.Location = new Point(x, y);
        }

        private void StoragePanel_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (StorageIndex == 0 && count >= 0) return;
            if (StorageIndex == 6 && count <= 0) return;

            StorageIndex -= count;

            if (StorageIndex < 0) StorageIndex = 0;
            if (StorageIndex > 6) StorageIndex = 6;

            UpdateStorage();

            StorageUpdatePositionBar();
        }

        private void StorageUpdatePositionBar()
        {

            int interval = 289 / 6;

            int x = 337;
            int y = 16 + (StorageIndex * interval);

            if (y >= StorageDownButton.Location.Y - 20) y = StorageDownButton.Location.Y - 20;
            if (y <= 16) y = 16;

            StoragePositionBar.Location = new Point(x, y);
        }

        public void UpdateStorage()
        {
            for (int x = 0; x < 8; x++)
            {
                for (int y = 0; y < 14; y++)
                {
                    int idx = 8 * y + x;
                    if (StorageIndex <= y && (y < StorageIndex + 8))
                    {
                        StorageGrid[idx].Location = new Point(x * 35 + 31 + x, (y - StorageIndex) * 35 + 20 + (y - StorageIndex));
                        StorageGrid[idx].Visible = true;
                    }
                    else
                        StorageGrid[idx].Visible = false;
                }
            }
        }

        public void StorageAddGold()
        {
            if (LastGuildMsg > CMain.Time) return;
            MirAmountBox amountBox = new MirAmountBox("Deposit", 116, GameScene.Gold);

            amountBox.OKButton.Click += (o, a) =>
            {
                if (amountBox.Amount <= 0) return;
                LastGuildMsg = CMain.Time + 100;
                Network.Enqueue(new C.GuildStorageGoldChange
                {
                    Type = 0,
                    Amount = amountBox.Amount,
                });
            };

            amountBox.Show();
        }
        public void StorageRemoveGold()
        {
            if (LastGuildMsg > CMain.Time) return;
            MirAmountBox amountBox = new MirAmountBox("Gold to retrieve:", 116, Gold);

            amountBox.OKButton.Click += (o, a) =>
            {
                if (amountBox.Amount <= 0) return;
                LastGuildMsg = CMain.Time + 100;
                Network.Enqueue(new C.GuildStorageGoldChange
                {
                    Type = 1,
                    Amount = amountBox.Amount,
                });
            };

            amountBox.Show();
        }
        #endregion

        #region UpdateNotice
        public void RequestUpdateNotice()
        {
            if ((NoticeChanged) && (LastNoticeRequest < CMain.Time))
            {
                LastNoticeRequest = CMain.Time + 5000;
                Network.Enqueue(new C.RequestGuildInfo() { Type = 0 });
            }
        }
        public void RequestUpdateMembers()
        {
            if ((MembersChanged) && (LastMemberRequest < CMain.Time))
            {
                LastMemberRequest = CMain.Time + 5000;
                Network.Enqueue(new C.RequestGuildInfo() { Type = 1 });
            }
        }
        #endregion

        #region NoticeDialogPages
        public void RightDialog(byte Rpageid)
        {
            StatusPage.Visible = false;
            BuffPage.Visible = false;

            StatusButton.Index = 103;
            BuffButton.Index = 95;

            switch (Rpageid)
            {
                case 0:
                    StatusPage.Visible = true;
                    StatusButton.Index = 104;
                    break;
                case 1:
                    BuffPage.Visible = true;
                    BuffButton.Index = 96;
                    break;
            }
        }
        public void LeftDialog(byte Lpageid)
        {
            NoticePage.Visible = false;
            MembersPage.Visible = false;
            StoragePage.Visible = false;
            RankPage.Visible = false;

            NoticeButton.Index = 93;
            MembersButton.Index = 99;
            StorageButton.Index = 105;
            RankButton.Index = 101;

            switch (Lpageid)
            {
                case 0:
                    NoticePage.Visible = true;
                    NoticeButton.Index = 94;
                    RequestUpdateNotice();
                    break;
                case 1:
                    MembersPage.Visible = true;
                    MembersButton.Index = 100;
                    RequestUpdateMembers();
                    break;
                case 2:
                    StoragePage.Visible = true;
                    StorageButton.Index = 106;
                    Network.Enqueue(new C.GuildStorageItemChange() { Type = 3 });
                    break;
                case 3:
                    RankPage.Visible = true;
                    RankButton.Index = 102;
                    RequestUpdateMembers();
                    break;
            }
        }
        public void StatusChanged(GuildRankOptions status)
        {
            Notice.Enabled = false;
            NoticeEditButton.Index = 85;
            MyOptions = status;

            if (MyOptions.HasFlag(GuildRankOptions.CanChangeNotice))
                NoticeEditButton.Visible = true;
            else
                NoticeEditButton.Visible = false;

            if (MyOptions.HasFlag(GuildRankOptions.CanChangeRank))
                RankButton.Visible = true;
            else
                RankButton.Visible = false;

            if ((MyOptions.HasFlag(GuildRankOptions.CanStoreItem)) || (MyOptions.HasFlag(GuildRankOptions.CanRetrieveItem)))
                StorageButton.Visible = true;
            else
                StorageButton.Visible = false;

            if (GuildBuffInfos.Count == 0) BuffButton.Visible = false;
            else BuffButton.Visible = true;

        }
        #endregion

        #region GuildDialogChecks

        public override void Show()
        {
            if (Visible) return;

            if (MapControl.User.GuildName == "")
            {
                MirMessageBox messageBox = new MirMessageBox(GameLanguage.NotInGuild, MirMessageBoxButtons.OK);
                messageBox.Show();
                return;
            }
            Visible = true;

            if (NoticePage.Visible)
            {
                if ((NoticeChanged) && (LastNoticeRequest < CMain.Time))
                {
                    LastNoticeRequest = CMain.Time + 5000;
                    Network.Enqueue(new C.RequestGuildInfo() { Type = 0 });
                }
                NoticeButton.Index = 94;
            }
            if (StatusPage.Visible)
                StatusButton.Index = 104;


        }
        #endregion
    }
    public sealed class GuildBuffButton : MirControl
    {
        public byte Id;
        //public MirButton Icon;
        public MirImageControl Icon;
        public MirLabel Name, Info, Obtained;
        //public MirControl HintLabel;

        public GuildBuffButton()
        {
            BorderColour = Color.Orange;

            Size = new Size(188, 33);
            Icon = new MirImageControl
            {
                Index = 0,
                Library = Libraries.GuildSkill,
                Parent = this,
                //Location = new Point(1, 0),
                Location = new Point(1, 0),
                NotControl = true
            };
            Name = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(35, 1),
                NotControl = true
            };
            Info = new MirLabel
            {
                AutoSize = true,
                Parent = this,
                Location = new Point(35, 17),
                NotControl = true
            };
            Obtained = new MirLabel
            {
                DrawFormat = TextFormatFlags.Right,
                AutoSize = true,
                Parent = this,
                Location = new Point(140, 17),
                NotControl = true,
                Text = ""
            };
        }

        protected override void OnMouseEnter()
        {
            base.OnMouseEnter();
            GameScene.Scene.GuildDialog.CreateHintLabel(Id);
            Border = true;
        }
        protected override void OnMouseLeave()
        {
            base.OnMouseLeave();
            GameScene.Scene.DisposeGuildBuffLabel();
            Border = false;
        }
    }
}
