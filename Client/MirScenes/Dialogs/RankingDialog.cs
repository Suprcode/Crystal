using Client.MirControls;
using Client.MirGraphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Client.MirSounds;
using System.Windows.Forms;

namespace Client.MirScenes.Dialogs
{
    //thx to Pete107/Petesn00beh for the base of this :p
    public class RankingDialog : MirImageControl
    {
        public MirButton AllButton, WarButton, WizButton, TaoButton, SinButton, ArchButton, Tab7;
        public MirButton CloseButton;

        public byte RankType = 0;
        public RankingRow[] Rows = new RankingRow[20];
        public List<Rank_Character_Info>[] RankList = new List<Rank_Character_Info>[6];

        public long[] LastRequest = new long[6];

        public RankingDialog()
        {
            Index = 1330;
            Library = Libraries.Prguse2;
            //Size = new Size(288, 324);
            Movable = true;
            Sort = true;
            Location = new Point((800 - Size.Width) / 2, (600 - Size.Height) / 2);

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
            AllButton.Click += (o, e) => RequestRanks(0);
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
            TaoButton.Click += (o, e) => RequestRanks(3);
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
            WarButton.Click += (o, e) => RequestRanks(1);
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
            WizButton.Click += (o, e) => RequestRanks(2);
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
            SinButton.Click += (o, e) => RequestRanks(4);
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
            ArchButton.Click += (o, e) => RequestRanks(5);

            for (int i = 0; i < Rows.Count(); i++)
            {
                Rows[i] = new RankingRow() { Parent = this, Location = new Point(32, 32 + i * 15) };
            }
            for (int i = 0; i < RankList.Length; i++)
            {
                RankList[i] = new List<Rank_Character_Info>();
            }
        }

        public void Show()
        {
            if (Visible) return;
            Visible = true;
            RequestRanks(RankType);
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

        public void RequestRanks(byte RankType)
        {
            if (RankType > 6) return;
            if ((LastRequest[RankType] != 0) && ((LastRequest[RankType] + 300 * 1000) > CMain.Time))
            {
                SelectRank(RankType);
                return;
            }
            LastRequest[RankType] = CMain.Time;
            MirNetwork.Network.Enqueue(new ClientPackets.GetRanking { RankIndex = RankType });
        }

        public void RecieveRanks(List<Rank_Character_Info> Ranking, byte rankType)
        {
            RankList[rankType].Clear();
            RankList[rankType] = Ranking;
            SelectRank(rankType);
        }

        public void SelectRank(byte rankType)
        {
            RankType = rankType;
            for (int i = 0; i < Rows.Count(); i++)
            {
                Rows[i].Clear();
            }
            for (int i = 0; i < RankList[RankType].Count; i++)
            {
                if (i > Rows.Count()) break;
                Rows[i].Update(RankList[RankType][i], i + 1);
            }
        }


        public sealed class RankingRow : MirControl
        {
            public Rank_Character_Info Listing;
            public MirLabel RankLabel, NameLabel, LevelLabel, ClassLabel;
            public long Index;

            public RankingRow()
            {
                Sound = SoundList.ButtonA;
                BorderColour = Color.Lime;
                Visible = false;

                RankLabel = new MirLabel
                {
                    Location = new Point(0, 64),
                    AutoSize = true,
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };
                NameLabel = new MirLabel
                {
                    Location = new Point(55, 64),
                    AutoSize = true,
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };

                ClassLabel = new MirLabel
                {
                    Location = new Point(150, 64),
                    AutoSize = true,
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };

                LevelLabel = new MirLabel
                {
                    Location = new Point(220, 64),
                    AutoSize = true,
                    DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                    Parent = this,
                    NotControl = true,
                };
            }

            public void Clear()
            {
                Visible = false;
                NameLabel.Text = string.Empty;
                RankLabel.Text = string.Empty;
                LevelLabel.Text = string.Empty;
                ClassLabel.Text = string.Empty;
            }
            public void Update(Rank_Character_Info listing, int RankIndex)
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
