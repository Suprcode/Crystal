using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using C = ClientPackets;

namespace Client.MirScenes.Dialogs
{
    public sealed class GameShopDialog : MirImageControl
    {
        public GameShopViewer Viewer;
        public MirLabel PageNumberLabel, totalGold, totalCredits;
        public MirButton ALL, War, Sin, Tao, Wiz, Arch;
        public MirButton allItems, topItems, Deals, New;
        public MirButton CloseButton, PreviousButton, NextButton;
        public MirButton UpButton, DownButton, PositionBar;


        public GameShopCell[] Grid;
        public MirLabel[] Filters = new MirLabel[22];
        List<String> CategoryList = new List<String>();
        List<GameShopItem> filteredShop = new List<GameShopItem>();
        List<GameShopItem> SearchResult = new List<GameShopItem>();
        public MirTextBox Search;
        public MirImageControl TitleLabel, FilterBackground;

        public string ClassFilter = "Show All";
        public string TypeFilter = "Show All";
        public string SectionFilter = "Show All";

        public int StartIndex = 0;
        public int Page = 0;
        public int CStartIndex = 0;
        public decimal maxPage;

        public GameShopDialog()
        {
            GameScene.GameShopInfoList.Clear();
            Index = 749;
            Library = Libraries.Title;
            Movable = true;
            Location = Center;
            Sort = true;

            TitleLabel = new MirImageControl
            {
                Index = 26,
                Library = Libraries.Title,
                Location = new Point(18, 9),
                Parent = this
            };

            Grid = new GameShopCell[4 * 2];
            for (int x = 0; x < 4; x++)
            {
                for (int y = 0; y < 2; y++)
                {
                    int idx = 4 * y + x;
                    Grid[idx] = new GameShopCell
                    {
                        Size = new Size(125, 146),
                        Visible = true,
                        Parent = this,
                    };
                }
            }

            CloseButton = new MirButton
            {
                HoverIndex = 361,
                Index = 360,
                Location = new Point(671, 4),
                Library = Libraries.Prguse2,
                Parent = this,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
            };
            CloseButton.Click += (o, e) => Hide();

            totalGold = new MirLabel
            {
                Size = new Size(100, 20),
                DrawFormat = TextFormatFlags.RightToLeft | TextFormatFlags.Right,

                Location = new Point(123, 449),
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 8F),
            };
            totalCredits = new MirLabel
            {
                Size = new Size(100, 20),
                DrawFormat = TextFormatFlags.RightToLeft | TextFormatFlags.Right,
                Location = new Point(5, 449),
                Parent = this,
                NotControl = true,
                Font = new Font(Settings.FontName, 8F)
            };

            UpButton = new MirButton
            {
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Library = Libraries.Prguse2,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(120, 103),
                Sound = SoundList.ButtonA,
                Visible = true
            };
            UpButton.Click += (o, e) =>
            {
                if (CStartIndex <= 0) return;

                CStartIndex--;

                SetCategories();
                UpdatePositionBar();
            };

            DownButton = new MirButton
            {
                Index = 207,
                HoverIndex = 208,
                Library = Libraries.Prguse2,
                PressedIndex = 209,
                Parent = this,
                Size = new Size(16, 14),
                Location = new Point(120, 421),
                Sound = SoundList.ButtonA,
                Visible = true
            };
            DownButton.Click += (o, e) =>
            {
                if (CStartIndex + 22 >= CategoryList.Count) return;

                CStartIndex++;

                SetCategories();
                UpdatePositionBar();
            };

            PositionBar = new MirButton
            {
                Index = 205,
                HoverIndex = 206,
                PressedIndex = 206,
                Library = Libraries.Prguse2,
                Location = new Point(120, 117),
                Parent = this,
                Movable = true,
                Sound = SoundList.None,
                Visible = true
            };
            PositionBar.OnMoving += PositionBar_OnMoving;




            FilterBackground = new MirImageControl
            {
                Index = 769,
                Library = Libraries.Title,
                Location = new Point(11, 102),
                Parent = this,
            };
            FilterBackground.MouseWheel += FilterScrolling;

            Search = new MirTextBox
            {
                BackColour = Color.FromArgb(4, 4, 4),
                ForeColour = Color.White,
                Parent = this,
                Size = new Size(140, 16),
                Location = new Point(540, 69),
                Font = new Font(Settings.FontName, 9F),
                MaxLength = 23,
                CanLoseFocus = true,
            };
            Search.TextBox.KeyUp += (o, e) =>
            {
                GetCategories();
            };

            allItems = new MirButton
            {
                Index = 770,
                Library = Libraries.Title,
                Location = new Point(138, 68),
                Visible = true,
                Parent = this,
                Sound = SoundList.ButtonA,

            };
            allItems.Click += (o, e) =>
            {
                SectionFilter = "Show All";
                ResetTabs();
                GetCategories();
            };
            topItems = new MirButton
            {
                Index = 776,
                Library = Libraries.Title,
                Location = new Point(209, 68),
                Visible = true,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            topItems.Click += (o, e) =>
            {
                SectionFilter = "TopItems";
                ResetTabs();
                GetCategories();
            };
            Deals = new MirButton
            {
                Index = 772,
                Library = Libraries.Title,
                Location = new Point(280, 68),
                Visible = true,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            Deals.Click += (o, e) =>
            {
                SectionFilter = "DealItems";
                ResetTabs();
                GetCategories();
            };
            New = new MirButton
            {
                Index = 774,
                Library = Libraries.Title,
                Location = new Point(351, 68),
                Visible = false,
                Parent = this,
                Sound = SoundList.ButtonA,
            };
            New.Click += (o, e) =>
            {
                SectionFilter = "NewItems";
                ResetTabs();
                New.Index = 775;
                GetCategories();
            };


            ALL = new MirButton
            {
                Index = 751,
                HoverIndex = 752,
                PressedIndex = 753,
                Library = Libraries.Title,
                Location = new Point(539, 37),
                Visible = true,
                Parent = this,
            };
            ALL.Click += (o, e) =>
            {
                ClassFilter = "Show All";
                TypeFilter = "Show All";
                GetCategories();
                ResetClass();
            };
            War = new MirButton
            {
                Index = 754,
                HoverIndex = 755,
                PressedIndex = 756,
                Library = Libraries.Title,
                Location = new Point(568, 38),
                Visible = true,
                Parent = this,
            };
            War.Click += (o, e) =>
            {
                ClassFilter = "Warrior";
                TypeFilter = "Show All";
                GetCategories();
                ResetClass();
            };
            Sin = new MirButton
            {
                Index = 757,
                HoverIndex = 758,
                PressedIndex = 759,
                Library = Libraries.Title,
                Location = new Point(591, 38),
                Visible = true,
                Parent = this,
            };
            Sin.Click += (o, e) =>
            {
                ClassFilter = "Assassin";
                TypeFilter = "Show All";
                GetCategories();
                ResetClass();
            };
            Tao = new MirButton
            {
                Index = 760,
                HoverIndex = 761,
                PressedIndex = 762,
                Library = Libraries.Title,
                Location = new Point(614, 38),
                Visible = true,
                Parent = this,
            };
            Tao.Click += (o, e) =>
            {
                ClassFilter = "Taoist";
                TypeFilter = "Show All";
                GetCategories();
                ResetClass();
            };
            Wiz = new MirButton
            {
                Index = 763,
                HoverIndex = 764,
                PressedIndex = 765,
                Library = Libraries.Title,
                Location = new Point(637, 38),
                Visible = true,
                Parent = this,
            };
            Wiz.Click += (o, e) =>
            {
                ClassFilter = "Wizard";
                TypeFilter = "Show All";
                GetCategories();
                ResetClass();
            };
            Arch = new MirButton
            {
                Index = 766,
                HoverIndex = 767,
                PressedIndex = 768,
                Library = Libraries.Title,
                Location = new Point(660, 38),
                Visible = true,
                Parent = this,
            };
            Arch.Click += (o, e) =>
            {
                ClassFilter = "Archer";
                TypeFilter = "Show All";
                GetCategories();
                ResetClass();
            };

            PageNumberLabel = new MirLabel
            {
                Text = "",
                Parent = this,
                Size = new Size(83, 17),
                Location = new Point(597, 446),
                DrawFormat = TextFormatFlags.HorizontalCenter | TextFormatFlags.VerticalCenter,
                Font = new Font(Settings.FontName, 7F),
            };

            PreviousButton = new MirButton
            {
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(600, 448),
                Sound = SoundList.ButtonA,
            };
            PreviousButton.Click += (o, e) =>
            {
                Page--;
                if (Page < 0) Page = 0;
                StartIndex = Grid.Length * Page;

                UpdateShop();
            };

            NextButton = new MirButton
            {
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Library = Libraries.Prguse2,
                Parent = this,
                Location = new Point(660, 448),
                Sound = SoundList.ButtonA,
            };
            NextButton.Click += (o, e) =>
            {
                Page++;
                if ((Page + 1) > maxPage) Page--;
                StartIndex = Grid.Length * Page;
                UpdateShop();
            };

            for (int i = 0; i < Filters.Length; i++)
            {
                Filters[i] = new MirLabel
                {
                    Parent = this,
                    Size = new Size(90, 20),
                    Location = new Point(15, 103 + (15 * i)),
                    Text = "Testing - " + i.ToString(),
                    ForeColour = Color.Gray,
                    Font = new Font(Settings.FontName, 7F),
                };
                Filters[i].Click += (o, e) =>
                {
                    MirLabel lab = (MirLabel)o;
                    TypeFilter = lab.Text;
                    Page = 0;
                    StartIndex = 0;
                    UpdateShop();
                    for (int p = 0; p < Filters.Length; p++)
                    {
                        if (Filters[p].Text == lab.Text) Filters[p].ForeColour = Color.FromArgb(230, 200, 160);
                        else Filters[p].ForeColour = Color.Gray;
                    }

                };
                Filters[i].MouseEnter += (o, e) =>
                {
                    MirLabel lab = (MirLabel)o;
                    for (int p = 0; p < Filters.Length; p++)
                    {
                        if (Filters[p].Text == lab.Text && Filters[p].ForeColour != Color.FromArgb(230, 200, 160)) Filters[p].ForeColour = Color.FromArgb(160, 140, 110);
                    }
                };
                Filters[i].MouseLeave += (o, e) =>
                {
                    MirLabel lab = (MirLabel)o;
                    for (int p = 0; p < Filters.Length; p++)
                    {
                        if (Filters[p].Text == lab.Text && Filters[p].ForeColour != Color.FromArgb(230, 200, 160)) Filters[p].ForeColour = Color.Gray;
                    }
                };
                Filters[i].MouseWheel += FilterScrolling;
            }

            Viewer = new GameShopViewer();

        }

        public void Hide()
        {
            if (!Visible) return;
            Viewer.Visible = false;
            Visible = false;
        }
        public void Show()
        {
            if (Visible) return;
            Visible = true;
            ClassFilter = GameScene.User.Class.ToString();
            SectionFilter = "Show All";
            ResetTabs();
            ResetClass();
            GetCategories();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            Search.Dispose();
            Search = null;

            PageNumberLabel = null;
            totalGold = null;
            totalCredits = null;
            ALL = null;
            War = null;
            Sin = null;
            Tao = null;
            Wiz = null;
            Arch = null;
            allItems = null;
            topItems = null;
            Deals = null;
            New = null;

            CloseButton = null;
            PreviousButton = null;
            NextButton = null;

            UpButton = null;
            DownButton = null;
            PositionBar = null;

            Grid = null;
            Filters = null;
            FilterBackground = null;

            Viewer.Dispose();

            CategoryList.Clear();
            filteredShop.Clear();
            SearchResult.Clear();
        }

        public void Process()
        {
            totalCredits.Text = GameScene.Credit.ToString("###,###,##0");
            totalGold.Text = GameScene.Gold.ToString("###,###,##0");
        }


        public void FilterScrolling(object sender, MouseEventArgs e)
        {
            int count = e.Delta / SystemInformation.MouseWheelScrollDelta;

            if (CStartIndex == 0 && count >= 0) return;
            if (CStartIndex == CategoryList.Count - 1 && count <= 0) return;
            if (CategoryList.Count <= 22) return;

            CStartIndex -= count;

            if (CStartIndex < 0) CStartIndex = 0;
            if (CStartIndex + 22 > CategoryList.Count - 1) CStartIndex = CategoryList.Count - 22;

            SetCategories();

            UpdatePositionBar();

        }

        private void UpdatePositionBar()
        {
            if (CategoryList.Count <= 22) return;

            int interval = 290 / (CategoryList.Count - 22);

            int x = 120;
            int y = 117 + (CStartIndex * interval);

            if (y >= 401) y = 401;
            if (y <= 117) y = 117;

            PositionBar.Location = new Point(x, y);
        }

        void PositionBar_OnMoving(object sender, MouseEventArgs e)
        {
            int x = 120;
            int y = PositionBar.Location.Y;

            if (y >= 401) y = 401;
            if (y <= 117) y = 117;

            if (CategoryList.Count > 22)
            {
                int location = y - 117;
                int interval = 284 / (CategoryList.Count - 22);

                double yPoint = location / interval;

                CStartIndex = Convert.ToInt16(Math.Floor(yPoint));
                SetCategories();
            }

            PositionBar.Location = new Point(x, y);
        }

        public void ResetTabs()
        {
            allItems.Index = 770;
            topItems.Index = 776;
            Deals.Index = 772;
            New.Index = 774;

            if (SectionFilter == "Show All") allItems.Index = 771;
            if (SectionFilter == "TopItems") topItems.Index = 777;
            if (SectionFilter == "DealItems") Deals.Index = 773;
            if (SectionFilter == "NewItems") New.Index = 775;
        }

        public void ResetClass()
        {
            ALL.Index = 751;
            War.Index = 754;
            Sin.Index = 757;
            Tao.Index = 760;
            Wiz.Index = 763;
            Arch.Index = 766;

            if (ClassFilter == "Show All") ALL.Index = 752;
            if (ClassFilter == "Warrior") War.Index = 755;
            if (ClassFilter == "Assassin") Sin.Index = 758;
            if (ClassFilter == "Taoist") Tao.Index = 761;
            if (ClassFilter == "Wizard") Wiz.Index = 764;
            if (ClassFilter == "Archer") Arch.Index = 767;
        }

        public void GetCategories()
        {
            TypeFilter = "Show All";
            Page = 0;
            StartIndex = 0;
            List<GameShopItem> shopList;

            if (Search.TextBox.Text != "")
                shopList = GameScene.GameShopInfoList.Where(f => f.Info.FriendlyName.ToLower().Contains(Search.TextBox.Text.ToLower())).ToList();
            else
                shopList = GameScene.GameShopInfoList;

            CategoryList.Clear();
            PositionBar.Location = new Point(120, 117);
            CategoryList.Add("Show All");

            for (int i = 0; i < shopList.Count; i++)
            {
                if (!CategoryList.Contains(shopList[i].Category) && shopList[i].Category != "")
                {
                    if (shopList[i].Class == ClassFilter || shopList[i].Class == "All" || ClassFilter == "Show All")
                    {
                        if (SectionFilter == "Show All" || SectionFilter == "TopItems" && shopList[i].TopItem || SectionFilter == "DealItems" && shopList[i].Deal || SectionFilter == "NewItems" && shopList[i].Date > DateTime.Now.AddDays(-7))
                            CategoryList.Add(shopList[i].Category);
                    }

                }
            }
            Filters[0].ForeColour = Color.FromArgb(230, 200, 160);
            CStartIndex = 0;
            SetCategories();
            UpdateShop();
        }

        public void SetCategories()
        {
            for (int i = 0; i < Filters.Length; i++)
            {
                if (i < CategoryList.Count)
                {
                    Filters[i].Text = CategoryList[i + CStartIndex];
                    Filters[i].ForeColour = Filters[i].Text == TypeFilter ? Color.FromArgb(230, 200, 160) : Color.Gray;
                    Filters[i].NotControl = false;
                }
                else
                {
                    Filters[i].Text = "";
                    Filters[i].NotControl = true;
                }
            }

        }
        public void UpdateShop()
        {
            List<GameShopItem> ShopList;

            if (Search.TextBox.Text != "")
                ShopList = GameScene.GameShopInfoList.Where(f => f.Info.FriendlyName.ToLower().Contains(Search.TextBox.Text.ToLower())).ToList();
            else
                ShopList = GameScene.GameShopInfoList;

            for (int i = 0; i < Grid.Length; i++)
            {
                if (Grid[i] != null) Grid[i].Dispose();
                Grid[i].Item = null;
            };


            filteredShop.Clear();

            for (int i = 0; i < ShopList.Count; i++)
            {
                if (ShopList[i].Class == ClassFilter || ShopList[i].Class == "All" || ClassFilter == "Show All")
                    if (ShopList[i].Category == TypeFilter || TypeFilter == "Show All")
                    {
                        if (SectionFilter == "Show All" || SectionFilter == "TopItems" && ShopList[i].TopItem || SectionFilter == "DealItems" && ShopList[i].Deal || SectionFilter == "NewItems" && ShopList[i].Date > DateTime.Now.AddDays(-7))
                            filteredShop.Add(ShopList[i]);
                    }
            }


            maxPage = ((decimal)filteredShop.Count / 8);
            maxPage = Math.Ceiling(maxPage);
            if (maxPage < 1) maxPage = 1;

            PageNumberLabel.Text = (Page + 1) + " / " + maxPage;

            int maxIndex = filteredShop.Count - 1;

            if (StartIndex > maxIndex) StartIndex = maxIndex;
            if (StartIndex < 0) StartIndex = 0;

            filteredShop = filteredShop.OrderBy(e => e.Info.FriendlyName).ToList();

            for (int i = 0; i < Grid.Length; i++)
            {
                if (i + StartIndex >= filteredShop.Count) break;

                if (Grid[i] != null) Grid[i].Dispose();

                Grid[i] = new GameShopCell
                {
                    Visible = true,
                    Item = filteredShop[i + StartIndex],
                    Size = new Size(125, 146),
                    Location = i < 4 ? new Point(152 + (i * 132), 115) : new Point(152 + ((i - 4) * 132), 275),
                    Parent = this,
                };
            }

            GameScene.Scene.GameShopDialog.Viewer.Visible = false;

        }

    }
}
