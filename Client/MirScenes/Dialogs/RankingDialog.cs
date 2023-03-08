using Client.MirControls;
using Client.MirGraphics;
using Client.MirSounds;

namespace Client.MirScenes.Dialogs
{
    public class RankingDialog : MirImageControl
    {
        public MirButton AllButton, WarButton, WizButton, TaoButton, SinButton, ArchButton, Tab7, NextButton, PrevButton, ScrollBar;
        public MirButton CloseButton;
        public MirLabel MyRank;
        public MirCheckBox OnlineOnlyButton;

        public byte RankType = 0;
        private int rowOffset;
        private int RowOffset
        {
            get { return rowOffset; }
            set
            {
                rowOffset = value;
                ScrollBar.Location = new Point(PrevButton.Location.X, (int)(PrevButton.Location.Y + 13 + rowOffset * GapPerRow));
            }
        }
        public int RankCount;
        public RankingRow[] Rows = new RankingRow[20];
        public List<RankCharacterInfo>[] RankList = new List<RankCharacterInfo>[6];
        public int[] Rank = new int[6];
        private bool OnlineOnly;

        public long[] LastRequest = new long[6];
        private float GapPerRow;
        private int ScrollHeight;
        private DateTime NextRequestTime = DateTime.MinValue;

        public RankingDialog()
        {
            Index = 728;
            Library = Libraries.Title;
            //Size = new Size(288, 324);
            Movable = true;
            Sort = true;
            Location = new Point((Settings.ScreenWidth - Size.Width) / 2, (Settings.ScreenHeight - Size.Height) / 2);

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(300, 3),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            AllButton = new MirButton
            {
                Index = 751,
                PressedIndex = 752,
                HoverIndex = 753,
                Library = Libraries.Title,
                Hint = "Overall TOP 20",
                Location = new Point(10, 38),
                Parent = this,
                Sound = SoundList.ButtonA,

            };
            AllButton.Click += (o, e) => SelectRank(0);
            TaoButton = new MirButton
            {
                Index = 760,
                PressedIndex = 761,
                HoverIndex = 762,
                Library = Libraries.Title,
                Hint = "TOP 20 Taoists",
                Location = new Point(40, 38),
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            TaoButton.Click += (o, e) => SelectRank(3);
            WarButton = new MirButton
            {
                Index = 754,
                PressedIndex = 755,
                HoverIndex = 756,
                Library = Libraries.Title,
                Hint = "TOP 20 Warriors",
                Location = new Point(60, 38),
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            WarButton.Click += (o, e) => SelectRank(1);
            WizButton = new MirButton
            {
                Index = 763,
                PressedIndex = 764,
                HoverIndex = 765,
                Library = Libraries.Title,
                Hint = "TOP 20 Wizards",
                Location = new Point(80, 38),
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            WizButton.Click += (o, e) => SelectRank(2);
            SinButton = new MirButton
            {
                Index = 757,
                PressedIndex = 758,
                HoverIndex = 759,
                Library = Libraries.Title,
                Hint = "TOP 20 Assasins",
                Location = new Point(100, 38),
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            SinButton.Click += (o, e) => SelectRank(4);
            ArchButton = new MirButton
            {
                Index = 766,
                PressedIndex = 767,
                HoverIndex = 768,
                Library = Libraries.Title,
                Hint = "TOP 20 Archers",
                Location = new Point(120, 38),
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            ArchButton.Click += (o, e) => SelectRank(5);

            NextButton = new MirButton
            {
                Index = 207,
                HoverIndex = 208,
                PressedIndex = 209,
                Library = Libraries.Prguse2,
                Location = new Point(299, 386),
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            NextButton.Click += (o, e) => Move(1);
            
            PrevButton = new MirButton
            {
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Library = Libraries.Prguse2,
                Location = new Point(299, 100),
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            PrevButton.Click += (o, e) => Move(-1);

            ScrollHeight = NextButton.Location.Y - PrevButton.Location.Y - 32;

            ScrollBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                PressedIndex = 206,
                Location = new Point(299, PrevButton.Location.Y + 13),
                Library = Libraries.Prguse2,
                Parent = this,
                Movable = true,
                Sound = SoundList.ButtonA,
            };
            ScrollBar.OnMoving += (o, e) =>
            {
                var y = ScrollBar.Location.Y;
                if (y < 110)
                    y = 110;
                if (y > 368)
                    y = 368;

                ScrollBar.Location = new Point(ScrollBar.Location.X, y);

                var row = Math.Max(0, Math.Min(RankCount - 20, (y - PrevButton.Location.Y - 13) / GapPerRow));
                RowOffset = (int)row;
                NextRequestTime = CMain.Now + TimeSpan.FromSeconds(0.5);
            };

            OnlineOnlyButton = new MirCheckBox { Index = 2086, UnTickedIndex = 2086, TickedIndex = 2087, Parent = this, Location = new Point(190, Size.Height - 20), Library = Libraries.Prguse };
            OnlineOnlyButton.LabelText = "Online Only";
            OnlineOnlyButton.Click += (o, e) =>
            {
                OnlineOnly = OnlineOnlyButton.Checked;
                RowOffset = 0;
                NextRequestTime = CMain.Now;
            };

            MyRank = new MirLabel
            {
                Text = "",
                Parent = this,
                Font = new Font(Settings.FontName, 10F, FontStyle.Bold),
                ForeColour = Color.BurlyWood,
                Location = new Point(229, 36),
                Size = new Size(82,22),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                //AutoSize = true
            };
            MyRank.Click += (o, e) => GoToMyRank();


            for (int i = 0; i < Rows.Count(); i++)
            {
                Rows[i] = new RankingRow() 
                { 
                    Parent = this, 
                    Location = new Point(32, 98 + i * 15),
                    Size = new Size(270,15),
                };
                Rows[i].MouseWheel += (o, e) => Ranking_MouseWheel(o, e);
            }
            for (int i = 0; i < RankList.Length; i++)
            {
                RankList[i] = new List<RankCharacterInfo>();
            }
        }

        public void Process()
        {
            if (NextRequestTime != DateTime.MinValue && CMain.Now > NextRequestTime)
            {
                NextRequestTime = DateTime.MinValue;
                RequestRanks(RankType);
            }
        }

        public override void Show()
        {
            if (Visible) return;
            Visible = true;
            RequestRanks(RankType);
        }

        public override void Hide()
        {
            if (!Visible) return;
            Visible = false;
        }

        public void Ranking_MouseWheel(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;
            Move(-count);
        }

        public void Toggle()
        {
            if (!Visible)
                Show();
            else
                Hide();
        }

        public void GoToMyRank()
        {

        }

        public void Move(int distance)
        {
            if (distance > 0)
            {//go down
                RowOffset = RowOffset < RankCount - 20 ? ++RowOffset : RowOffset;
            }
            else
            {//go up
                RowOffset = RowOffset > 0 ? --RowOffset : RowOffset;
            }
            NextRequestTime = CMain.Now + TimeSpan.FromSeconds(0.5);
        }

        public void RequestRanks(byte RankType)
        {
            if (RankType > 6) return;
            MirNetwork.Network.Enqueue(new ClientPackets.GetRanking { RankType = RankType, RankIndex = RowOffset, OnlineOnly = OnlineOnly});
        }

        public void RecieveRanks(List<RankCharacterInfo> Ranking, byte rankType, int MyRank, int Count)
        {
            RankList[rankType].Clear();
            RankList[rankType] = Ranking;
            Rank[rankType] = MyRank;
            RankCount = Count;
            UpdateRanks();
            var extraRows = Count - 20;
            GapPerRow = ScrollHeight / (float)extraRows;
        }

        public void SelectRank(byte rankType)
        {
            RankType = rankType;
            for (int i = 0; i < Rows.Count(); i++)
            {
                Rows[i].Clear();
            }
            RowOffset = 0;
            RequestRanks(RankType);            
        }

        public void UpdateRanks()
        {
            for (int i = 0; i < Rows.Count(); i++)
            {
                if (i >= RankCount)
                    Rows[i].Clear();
                else
                    Rows[i].Update(RankList[RankType][i], RowOffset + i + 1);
            }
            if (Rank[RankType] == 0)
                MyRank.Text = "Not Listed";
            else
                MyRank.Text = string.Format("Ranked: {0}", Rank[RankType]);
        }

        public sealed class RankingRow : MirControl
        {
            public RankCharacterInfo Listing;
            public MirLabel RankLabel, NameLabel, LevelLabel, ClassLabel;
            public long Index;

            public RankingRow()
            {
                Sound = SoundList.ButtonA;
                BorderColour = Color.Lime;
                Visible = false;
                Click += (o, e) => Inspect();

                RankLabel = new MirLabel
                {
                    Location = new Point(0, 0),
                    AutoSize = true,
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };
                NameLabel = new MirLabel
                {
                    Location = new Point(55, 0),
                    AutoSize = true,
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };

                ClassLabel = new MirLabel
                {
                    Location = new Point(150, 0),
                    AutoSize = true,
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };

                LevelLabel = new MirLabel
                {
                    Location = new Point(220, 0),
                    AutoSize = true,
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };
            }

            public void Inspect()
            {
                if (CMain.Time <= GameScene.InspectTime/* && Index == InspectDialog.InspectID*/) return;

                GameScene.InspectTime = CMain.Time + 500;
                InspectDialog.InspectID = (uint)Index;
                MirNetwork.Network.Enqueue(new ClientPackets.Inspect { ObjectID = (uint)Index, Ranking = true });
            }

            public void Clear()
            {
                Visible = false;
                NameLabel.Text = string.Empty;
                RankLabel.Text = string.Empty;
                LevelLabel.Text = string.Empty;
                ClassLabel.Text = string.Empty;
            }
            public void Update(RankCharacterInfo listing, int RankIndex)
            {
                Listing = listing;
                RankLabel.Text = RankIndex.ToString();
                LevelLabel.Text = Listing.level.ToString();
                ClassLabel.Text = Listing.Class.ToString();
                NameLabel.Text = listing.Name;
                Index = listing.PlayerId;
                if (RankLabel.Text == "1")
                {
                    RankLabel.ForeColour = Color.Gold;
                    NameLabel.ForeColour = Color.Gold;
                    LevelLabel.ForeColour = Color.Gold;
                    ClassLabel.ForeColour = Color.Gold;
                }
                if (RankLabel.Text == "2")
                {
                    RankLabel.ForeColour = Color.Silver;
                    NameLabel.ForeColour = Color.Silver;
                    LevelLabel.ForeColour = Color.Silver;
                    ClassLabel.ForeColour = Color.Silver;
                }
                if (RankLabel.Text == "3")
                {
                    RankLabel.ForeColour = Color.RosyBrown;
                    NameLabel.ForeColour = Color.RosyBrown;
                    LevelLabel.ForeColour = Color.RosyBrown;
                    ClassLabel.ForeColour = Color.RosyBrown;
                }
                else if (NameLabel.Text == GameScene.User.Name)
                {
                    RankLabel.ForeColour = Color.Green;
                    NameLabel.ForeColour = Color.Green;
                    LevelLabel.ForeColour = Color.Green;
                    ClassLabel.ForeColour = Color.Green;
                }
                else if (Convert.ToInt32(RankLabel.Text) > 3)
                {
                    RankLabel.ForeColour = Color.White;
                    NameLabel.ForeColour = Color.White;
                    LevelLabel.ForeColour = Color.White;
                    ClassLabel.ForeColour = Color.White;
                }

                Visible = true;
            }
        }
    }
}
