using Client.MirControls;
using Client.MirGraphics;
using Client.MirNetwork;
using Client.MirSounds;
using System.Globalization;
using System.Text.RegularExpressions;
using Shared;
using C = ClientPackets;
using S = ServerPackets;

namespace Client.MirScenes.Dialogs
{
    public class CodexDialog : MirImageControl
    {
        public static CodexDialog Instance;

        private static readonly Regex RewardTextTokenRx = new Regex(
        @"^(?<name>[\p{L}\p{N}\s]+?)\s*(?:[:=])?\s*(?<sign>[+\-]?)\s*(?<left>\d+(?:\.\d+)?)(?<leftsuffix>%?)\s*(?:~\s*(?<right>\d+(?:\.\d+)?)(?<rightsuffix>%?))?\s*$",
        RegexOptions.Compiled | RegexOptions.CultureInvariant);

        public static Func<int, int> IconResolver;

        private const int BG_INDEX = 310;
        private const int ROW_BG_INDEX = 330;
        private const int CELL_FRAME_IDX = 331;
        private const int COLL_EMBLEM_IDX = 326;

        private const int RIGHT_ICON_BOOK = 310;
        private const int RIGHT_ICON_CHARACTER = 311;
        private const int RIGHT_ICON_LIMITED = 313;
        private const int RIGHT_ICON_EVENT = 314;

        private const int BAR_BASE = 315;
        private const int BAR_GREEN = 316;
        private const int BAR_BLUE = 317;

        private const int XP_BAR_BACK = 318;
        private const int XP_BAR_FILL = 319;

        private const int DD_PANEL_IDX = 323;
        private const int DD_BAR_NORMAL_IDX = 324;
        private const int DD_BAR_HOVER_IDX = 325;
        private const int DD_VISIBLE_ROWS = 9;

        private static readonly Point DD_HDR_POS = new Point(102, 112);
        private static readonly Point DD_PANEL_POS = new Point(100, 126);
        private static readonly Size DD_PANEL_SIZE = new Size(180, 250);
        private const int DD_ITEM_H = 16;
        private const int DD_ITEM_X_PAD = 8;
        private const int DD_ITEM_Y_PAD = 8;

        private static string L(string key, params object[] args)
        {
            if (!GameLanguage.ClientTextMap.Text.TryGetValue(key, out var value)) value = key;
            return (args != null && args.Length > 0) ? string.Format(value, args) : value;
        }

        private string _filterStatKey = null;
        private readonly List<string> _ddKeys = new List<string>();

        private const int DIALOG_W = 820;
        private const int DIALOG_H = 520;

        private const int TOP_BAR_H = 75;
        private const int LEFT_NAV_W = 120;
        private const int LEFT_NAV_TOP_PAD = 75;

        private const int ROW_H = 64;
        private const int ROW_GAP = 1;
        private static int RowStep => ROW_H + ROW_GAP;

        private const int VISIBLE_ROWS = 5;
        private const int LIST_X = 100;
        private const int LIST_Y = 134;
        private const int VIEWPORT_WIDTH = 420;
        private const int VIEWPORT_HEIGHT = (ROW_H * VISIBLE_ROWS) + (ROW_GAP * (VISIBLE_ROWS - 1)) + 12;

        private const int EMBLEM_READY_IDX = 320;
        private const int EMBLEM_DONE_IDX = 322;

        private static readonly Point SCROLL_UP_POS = new Point(551, 138);
        private static readonly Size SCROLL_BTN_SIZE = new Size(18, 18);
        private static readonly Point SCROLL_DOWN_POS = new Point(551, 532);
        private static readonly Point SCROLL_THUMB_POS = new Point(551, 154);
        private static readonly Size SCROLL_THUMB_SIZE = new Size(16, 30);

        private static class RightUI
        {
            public static Rectangle Panel = new Rectangle(567, TOP_BAR_H + 60, 155, DIALOG_H - (TOP_BAR_H + 30) - 4);

            public static Point Header = new Point(29, 30);
            public static Point Prev = new Point(3, 30);
            public static Point Next = new Point(131, 30);

            public static int RowStartY = 80;
            public static int RowStepY = 62;

            public static Point Icon0 = new Point(3, 3 + RowStartY);
            public static Point Bar0 = new Point(5, 41 + RowStartY);
            public static Point Text0 = new Point(43, 25 + RowStartY);

            public static Rectangle RewardsRect = new Rectangle(0, 79, 135, 332);
            public static Point RewardsUp = new Point(138, 81);
            public static Point RewardsDown = new Point(138, 388);
            public static Point RewardsThumb = new Point(138, 75);
        }

        private const int ROW_TITLE_Y = 3;
        private const int ROW_REWARD_Y = 18;
        private const int ROW_LINE_STEP = 14;

        private const int XP_BAR_X = 104;
        private const int XP_BAR_Y = 51;
        private const int XP_BAR_H = 14;

        private MirControl _leftNav;
        private MirControl _listViewport;
        private MirControl _maskTop, _maskBottom;

        private MirButton _scrollUp, _scrollDown;
        private MirImageControl _scrollThumb;

        private MirImageControl _claimedSetsEmblem;

        private MirTextBox _searchBox;
        private MirButton _searchButton, _refreshSearchButton;
        private MirControl _searchGate;
        private bool _searchActivated;

        private bool _gatePressed;
        private Point _gateDownPt;
        private const int GateDragSqr = 9;

        private MirButton _ddHeader;
        private MirImageControl _ddPanel;
        private MirControl _ddViewport;
        private MirButton _ddUp, _ddDown;
        private MirImageControl _ddThumb;
        //private readonly List<MirButton> _ddItems = new List<MirButton>();
        private readonly List<string> _ddOptions = new List<string>{
            "All","HP","Attack","Defense","Crit Rate","Crit Damage","Max HP","Max MP","HP Regen","MP Regen"
        };
        private int _ddSelectedIndex = 0;
        private int _ddScrollPx = 0;
        private bool _ddOpen = false;
        private MirControl _ddOverflowMaskBottom;
        private MirControl _ddOverflowMaskTop;

        private MirButton _tabChar, _tabLim, _tabEvt;
        private byte _sectionBucket = 0;

        private MirButton _btnAll;
        private readonly Dictionary<ItemGrade, MirButton> _btnByRarity = new Dictionary<ItemGrade, MirButton>();
        private ItemGrade? _filterRarity = null;

        private const int NAV_BTN_W = 82;
        private const int NAV_BTN_H = 24;
        private const int NAV_BTN_X = 1;
        private const int NAV_BTN_Y0 = 8;
        private const int NAV_BTN_STEP = 24;

        private const int TAB_X0 = 7;
        private const int TAB_Y = 79;
        private const int TAB_STEP = 72;

        private MirControl _rightPanel;

        private readonly List<MirImageControl> _rightIcons = new List<MirImageControl>();
        private readonly List<MirImageControl> _rightBars = new List<MirImageControl>();
        private readonly List<MirLabel> _rightLabels = new List<MirLabel>();

        private MirLabel _rightHeader;
        private MirButton _rightPrev, _rightNext;

        private MirLabel _levelLabel;
        private MirImageControl _xpBarFill;
        private MirLabel _xpText;

        private readonly int[] _barFound = new int[4];
        private readonly int[] _barNeed = new int[4];

        private enum RightPage { Progress = 0, Rewards = 1 }
        private RightPage _rightPage = RightPage.Progress;

        private MirControl _statsViewport;
        private MirButton _statsScrollUp, _statsScrollDown;
        private MirImageControl _statsScrollThumb;
        private int _statsScrollOffsetPx;
        private const int StatsLineH = 18;
        private readonly List<MirLabel> _statsLines = new List<MirLabel>();

        private MirControl _rewardsViewport;
        private MirButton _rewardsScrollUp, _rewardsScrollDown;
        private MirImageControl _rewardsScrollThumb;
        private int _rewardsScrollOffsetPx;
        private readonly List<MirLabel> _rewardsLines = new List<MirLabel>();

        private const int VisibleLineCount = 18;
        private readonly List<string> _rewardsData = new List<string>();
        private int _firstVisibleLine = 0;

        private MirControl _rewardsMaskTop, _rewardsMaskBottom;

        private readonly List<RowVM> _rows = new List<RowVM>();
        private List<RowVM> _viewRows = new List<RowVM>();
        private string _searchQuery = string.Empty;

        private int _selectedIndex = -1;
        private int _scrollOffsetPx = 0;

        public static readonly Dictionary<int, Stats> RewardBySet = new Dictionary<int, Stats>();
        public static readonly HashSet<int> ClaimedSetIds = new HashSet<int>();
        public static event Action CodexChanged;

        private static int _suppressTooltipUntilMs;
        private static bool TooltipSuppressed => Environment.TickCount < _suppressTooltipUntilMs;
        private static void SuppressTooltips(int ms = 200) => _suppressTooltipUntilMs = Environment.TickCount + ms;

        private static readonly Rectangle LEVEL_HINT_RECT = new Rectangle(35, 30, 55, 45);
        private MirControl _levelHintHotspot;

        private int _lastSetCompleteFxAt = 0;
        private MirControl _levelFxHost;

        private bool _setFxPlaying = false;
        private bool _pendingLevelUpFx = false;
        private int _lastLevelSeen = 0;

        private readonly List<MirControl> _ddItemRows = new List<MirControl>();

        private MirImageControl _stoneIcon, _jadeIcon;
        private MirLabel _stoneCountLbl, _jadeCountLbl;
        private int _stoneCount, _jadeCount;

        private static readonly Point CURRENCY_STONE_POS = new Point(375, 102);
        private static readonly Point CURRENCY_JADE_POS = new Point(450, 102);

        private const int ICON_STONE_IDX = 332;
        private const int ICON_JADE_IDX = 333;

        private const ItemGrade STONE_SUBS_FOR = ItemGrade.Rare;
        private const ItemGrade JADE_SUBS_FOR = ItemGrade.Legendary;

        private static bool _allowCrossGradeCurrency = true;
        private static int _itemInfoStone = 0;
        private static int _itemInfoJade = 0;

        private const int STONE_INFO_ID = 0;
        private const int JADE_INFO_ID = 0;

        private long _lastInvSig = -1;

        public sealed class RowVM
        {
            public int SetId;
            public string Title;
            public short Found, Required;
            public bool Claimed;
            public bool Active;
            public bool KeepStats;
            public DateTime? StartTime;
            public DateTime? EndTime;

            public readonly List<int> ReqItemIndices = new List<int>();
            public readonly List<sbyte> ReqStages = new List<sbyte>();
            public readonly List<int> ReqItemIcons = new List<int>();
            public readonly List<bool> ReqRegistered = new List<bool>();

            public Stats Reward = new Stats();
            public string RewardText;

            public byte Bucket = 0;

            public int RewardXP;
            public ItemGrade Rarity;
        }

        private static Color RarityColor(ItemGrade r)
        {
            switch (r)
            {
                case ItemGrade.Common: return Color.WhiteSmoke;
                case ItemGrade.Rare: return Color.FromArgb(90, 170, 255);
                case ItemGrade.Legendary: return Color.FromArgb(255, 156, 0);
                case ItemGrade.Mythical: return Color.FromArgb(200, 120, 255);
                case ItemGrade.Heroic: return Color.FromArgb(255, 85, 120);
                case ItemGrade.None:
                default: return Color.White;
            }
        }

        private static string PrettyStatName(Stat s)
        {
            switch (s)
            {
                case Stat.MaxDC: return "Max DC";
                case Stat.MinDC: return "Min DC";
                case Stat.Accuracy: return "Accuracy";
                case Stat.HP: return "HP";
                case Stat.Luck: return "Luck";
                case Stat.Strength: return "Strength";
                case Stat.Intelligence: return "Intelligence";
                case Stat.AttackBonus: return "Power";
                case Stat.MinDamage: return "Damage Min";
                case Stat.MaxDamage: return "Damage Max";
                default: return s.ToString();
            }
        }

        private IEnumerable<string> BuildStatLines(RowVM data)
        {
            var vals = data?.Reward?.Values;
            if (vals != null && vals.Count > 0)
            {
                var d = new Dictionary<Stat, int>(vals);

                NormalizeFlatGroupStatsToMax(d);

                var lines = new List<string>();

                void EmitPair(Stat minStat, Stat maxStat, string label)
                {
                    bool hasMin = d.TryGetValue(minStat, out int minVal);
                    bool hasMax = d.TryGetValue(maxStat, out int maxVal);

                    if (hasMin && hasMax)
                    {
                        lines.Add($"{label} {minVal} ~ {maxVal}");
                        d.Remove(minStat);
                        d.Remove(maxStat);
                    }
                }

                EmitPair(Stat.MinAC, Stat.MaxAC, "AC");
                EmitPair(Stat.MinMAC, Stat.MaxMAC, "MAC");
                EmitPair(Stat.MinDC, Stat.MaxDC, "DC");
                EmitPair(Stat.MinMC, Stat.MaxMC, "MC");
                EmitPair(Stat.MinSC, Stat.MaxSC, "SC");
                EmitPair(Stat.MinDamage, Stat.MaxDamage, "Damage");

                foreach (var kv in d)
                {
                    string key = kv.Key.ToString();
                    if (key.StartsWith("Min", StringComparison.OrdinalIgnoreCase) ||
                        key.StartsWith("Max", StringComparison.OrdinalIgnoreCase))
                    {
                        lines.Add($"{key} +{kv.Value}");
                    }
                    else
                    {
                        lines.Add($"{PrettyStatName(kv.Key)} +{kv.Value}");
                    }
                }

                return lines;
            }

            var list = new List<string>();
            var t = data?.RewardText;
            if (!string.IsNullOrWhiteSpace(t))
            {
                foreach (var token in t.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                {
                    var s = token.Trim();
                    if (s.Length == 0) continue;

                    if (Regex.IsMatch(s, @"^(Min|Max)\s*(AC|MAC|DC|MC|SC)\b", RegexOptions.IgnoreCase))
                    {
                        s = Regex.Replace(s, @"^(?<k>(?:Min|Max)\s*(?:AC|MAC|DC|MC|SC))\s*(?<sign>[+\-]?)\s*(?<num>\d+)$",
                                               m => $"{m.Groups["k"].Value} {(m.Groups["sign"].Value == "-" ? "-" : "+")}{m.Groups["num"].Value}",
                                               RegexOptions.IgnoreCase);
                        list.Add(s);
                        continue;
                    }

                    var m2 = RewardTextTokenRx.Match(s);
                    if (!m2.Success)
                    {
                        list.Add(s);
                        continue;
                    }

                    var rawName = m2.Groups["name"].Value.Trim();
                    var sign = m2.Groups["sign"].Value == "-" ? "-" : "+";
                    string leftRaw = m2.Groups["left"].Value;
                    string leftSuffix = m2.Groups["leftsuffix"].Value;
                    decimal left = decimal.Parse(leftRaw, CultureInfo.InvariantCulture);
                    bool hasRight = m2.Groups["right"].Success;
                    string rightRaw = hasRight ? m2.Groups["right"].Value : string.Empty;
                    string rightSuffix = hasRight ? m2.Groups["rightsuffix"].Value : string.Empty;
                    decimal right = hasRight ? decimal.Parse(rightRaw, CultureInfo.InvariantCulture) : 0M;

                    string leftDisplay = leftRaw + leftSuffix;
                    string rightDisplay = hasRight ? rightRaw + rightSuffix : string.Empty;

                    var keyNoSpace = Regex.Replace(rawName, @"\s+", "");
                    bool isRangeGroup =
                        keyNoSpace.Equals("AC", StringComparison.OrdinalIgnoreCase) ||
                        keyNoSpace.Equals("MAC", StringComparison.OrdinalIgnoreCase) ||
                        keyNoSpace.Equals("DC", StringComparison.OrdinalIgnoreCase) ||
                        keyNoSpace.Equals("MC", StringComparison.OrdinalIgnoreCase) ||
                        keyNoSpace.Equals("SC", StringComparison.OrdinalIgnoreCase);

                    if (isRangeGroup)
                    {
                        var grp = keyNoSpace.ToUpperInvariant();

                        if (hasRight)
                        {
                            list.Add($"Min{grp} +{Math.Max(0M, left).ToString(CultureInfo.InvariantCulture)}");
                            list.Add($"Max{grp} +{Math.Max(0M, right).ToString(CultureInfo.InvariantCulture)}");
                        }
                        else
                        {
                            list.Add($"Max{grp} {sign}{left.ToString(CultureInfo.InvariantCulture)}");
                        }
                    }
                    else
                    {
                        if (hasRight)
                        {
                            list.Add($"{rawName} {sign}{leftDisplay} ~ {rightDisplay}");
                        }
                        else
                        {
                            list.Add($"{rawName} {sign}{leftDisplay}");
                        }
                    }
                }
            }
            return list;
        }

        private static IEnumerable<string> BuildStatLines(Stats bag)
        {
            var lines = new List<string>();
            if (bag == null || bag.Values == null || bag.Values.Count == 0) return lines;

            var d = new Dictionary<Stat, int>(bag.Values);

            NormalizeFlatGroupStatsToMax(d);

            void EmitPair(Stat minStat, Stat maxStat, string label)
            {
                if (d.TryGetValue(minStat, out int minVal) && d.TryGetValue(maxStat, out int maxVal))
                {
                    lines.Add($"{label} {minVal} ~ {maxVal}");
                    d.Remove(minStat);
                    d.Remove(maxStat);
                }
            }

            EmitPair(Stat.MinAC, Stat.MaxAC, "AC");
            EmitPair(Stat.MinMAC, Stat.MaxMAC, "MAC");
            EmitPair(Stat.MinDC, Stat.MaxDC, "DC");
            EmitPair(Stat.MinMC, Stat.MaxMC, "MC");
            EmitPair(Stat.MinSC, Stat.MaxSC, "SC");
            EmitPair(Stat.MinDamage, Stat.MaxDamage, "Damage");

            foreach (var kv in d)
                if (kv.Value != 0)
                {
                    string key = kv.Key.ToString();
                    if (key.StartsWith("Min", StringComparison.OrdinalIgnoreCase) ||
                        key.StartsWith("Max", StringComparison.OrdinalIgnoreCase))
                        lines.Add($"{key} +{kv.Value}");
                    else
                        lines.Add($"{PrettyStatName(kv.Key)} + {kv.Value}");
                }

            return lines;
        }


        private static void NormalizeFlatGroupStatsToMax(Dictionary<Stat, int> bag)
        {
            if (bag == null || bag.Count == 0) return;

            foreach (var key in bag.Keys.ToList())
            {
                var name = key.ToString();
                if (!name.Equals("AC", StringComparison.OrdinalIgnoreCase) &&
                    !name.Equals("MAC", StringComparison.OrdinalIgnoreCase) &&
                    !name.Equals("DC", StringComparison.OrdinalIgnoreCase) &&
                    !name.Equals("MC", StringComparison.OrdinalIgnoreCase) &&
                    !name.Equals("SC", StringComparison.OrdinalIgnoreCase))
                    continue;

                int val = bag[key];
                bag.Remove(key);

                if (Enum.TryParse<Stat>("Max" + name, true, out var maxKey))
                    bag[maxKey] = (bag.TryGetValue(maxKey, out var cur) ? cur : 0) + val;
            }
        }

        public static CodexDialog GetOrCreate(GameScene scene)
        {
            if (Instance != null && !Instance.IsDisposed) return Instance;

            Instance = new CodexDialog
            {
                Parent = scene,
                Visible = false
            };
            return Instance;
        }

        private void CloseDropdown()
        {
            _ddOpen = false;

            if (_ddPanel != null)
                _ddPanel.Visible = false;

            if (_ddOverflowMaskTop != null)
                _ddOverflowMaskTop.Visible = false;

            if (_ddOverflowMaskBottom != null)
                _ddOverflowMaskBottom.Visible = false;
        }

        public static void Toggle(GameScene scene)
        {
            var dlg = GetOrCreate(scene);
            bool show = !dlg.Visible;
            dlg.Visible = show;
            if (show) dlg.BringToFront();
            else
            {
                dlg.DeactivateSearch();
                dlg.CloseDropdown();
            }
        }

        public void ShowDialog()
        {
            Visible = true;
            BringToFront();
            if (Location.X < 0 || Location.Y < 0 || Location.X > 2000 || Location.Y > 2000)
                Location = new Point(160, 120);
        }

        public CodexDialog()
        {
            Instance = this;
            Sort = true;
            Movable = true;
            Visible = false;

            Library = Libraries.Title_32bit;
            Index = BG_INDEX;
            Size = new Size(DIALOG_W, DIALOG_H);
            Location = new Point(160, 120);

            BuildUI();
            DoLayout();
            UpdateRightPage();
        }

        public override void Show()
        {
            if (!GameScene.AllowCodex) return;
            base.Show();
        }

        public void ApplyPermissions()
        {
            if (!GameScene.AllowCodex && Visible)
                Hide();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && Instance == this) Instance = null;
            DeactivateSearch(rebuildGate: false);
            base.Dispose(disposing);
        }

        private int GetXpBarWidth()
        {
            try
            {
                var s = Libraries.UI_32bit.GetSize(XP_BAR_BACK);
                if (s.Width > 0) return s.Width;
            }
            catch { }
            return 160;
        }
        private int GetXpBarInnerWidth() => Math.Max(0, GetXpBarWidth() - 8);
        private int GetXpBarLeftInnerX() => XP_BAR_X + 4;

        private void BuildUI()
        {
            _claimedSetsEmblem = new MirImageControl
            {
                Parent = this,
                Library = Libraries.UI_32bit,
                Index = COLL_EMBLEM_IDX,
                Location = new Point(656, 25),
                Hint = "Claimed Set Bonuses",
                NotControl = false
            };

            _levelLabel = new MirLabel
            {
                Parent = this,
                Location = new Point(102, 31),
                AutoSize = true,
                ForeColour = Color.WhiteSmoke,
                Font = new Font(Settings.FontName, 12f, FontStyle.Bold),
            };

            if (_levelHintHotspot == null)
            {
                _levelHintHotspot = new MirControl
                {
                    Parent = this,
                    Location = new Point(LEVEL_HINT_RECT.X, LEVEL_HINT_RECT.Y),
                    Size = new Size(LEVEL_HINT_RECT.Width, LEVEL_HINT_RECT.Height),
                    Opacity = 0f,
                    NotControl = false
                };
            }

            new MirImageControl
            {
                Parent = this,
                Library = Libraries.UI_32bit,
                Index = XP_BAR_BACK,
                Location = new Point(XP_BAR_X, XP_BAR_Y),
            };

            _xpBarFill = new MirImageControl
            {
                Parent = this,
                Library = Libraries.UI_32bit,
                Index = XP_BAR_FILL,
                Location = new Point(GetXpBarLeftInnerX(), XP_BAR_Y + 2),
                DrawImage = false,
                NotControl = true
            };
            _xpBarFill.BeforeDraw += (s, e) => DrawXpFill(_xpBarFill);

            _xpText = new MirLabel
            {
                Parent = this,
                AutoSize = true,
                ForeColour = Color.White,
                Font = new Font(Settings.FontName, 9.0f),
                Text = "0 / 8"
            };

            BuildTopTabs();

            _leftNav = new MirControl
            {
                Parent = this,
                BackColour = Color.FromArgb(20, 20, 20),
                Border = true
            };

            _listViewport = new MirControl
            {
                Parent = this,
                BackColour = Color.FromArgb(12, 12, 12),
                Border = true
            };
            AttachWheel(_listViewport);

            _maskTop = new MirControl
            {
                Parent = this,
                BackColour = BackColour == Color.Empty ? Color.Black : BackColour,
                NotControl = true
            };
            _maskBottom = new MirControl
            {
                Parent = this,
                BackColour = BackColour == Color.Empty ? Color.Black : BackColour,
                NotControl = true
            };

            _scrollUp = new MirButton
            {
                Parent = this,
                Library = Libraries.Prguse2,
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Sound = SoundList.ButtonA,
                Size = SCROLL_BTN_SIZE
            };
            _scrollUp.Click += (s, e) => ScrollBy(-RowStep);

            _scrollDown = new MirButton
            {
                Parent = this,
                Library = Libraries.Prguse2,
                Index = 207,
                HoverIndex = 208,
                PressedIndex = 209,
                Sound = SoundList.ButtonA,
                Size = SCROLL_BTN_SIZE
            };
            _scrollDown.Click += (s, e) => ScrollBy(+RowStep);

            _scrollThumb = new MirImageControl
            {
                Parent = this,
                Library = Libraries.Prguse2,
                Index = 205,
                Size = SCROLL_THUMB_SIZE
            };

            _rightPanel = new MirControl
            {
                Parent = this,
                BackColour = Color.FromArgb(20, 20, 20, 20),
            };

            _rightPrev = new MirButton
            {
                Parent = _rightPanel,
                Library = Libraries.Prguse2,
                Index = 240,
                HoverIndex = 241,
                PressedIndex = 242,
                Sound = SoundList.ButtonA,
                Size = new Size(18, 18)
            };
            _rightPrev.Click += (o, e) => SwitchRightPage(-1);

            _rightHeader = new MirLabel
            {
                Parent = _rightPanel,
                AutoSize = true,
                ForeColour = Color.WhiteSmoke,
                Font = new Font(Settings.FontName, 9.5f, FontStyle.Bold),
                Text = "Progress"
            };

            _rightNext = new MirButton
            {
                Parent = _rightPanel,
                Library = Libraries.Prguse2,
                Index = 243,
                HoverIndex = 244,
                PressedIndex = 245,
                Sound = SoundList.ButtonA,
                Size = new Size(18, 18)
            };
            _rightNext.Click += (o, e) => SwitchRightPage(+1);

            BuildProgressRow(0, "All");
            BuildProgressRow(1, "Character");
            BuildProgressRow(2, "Limited");
            BuildProgressRow(3, "Event");

            _statsViewport = new MirControl
            {
                Parent = _rightPanel,
                BackColour = Color.FromArgb(16, 16, 16),
                Border = true,
                Visible = false
            };
            _statsViewport.MouseWheel += (o, e) =>
            {
                int ticks = Math.Max(1, Math.Abs(e.Delta) / 120);
                int dir = e.Delta > 0 ? -1 : +1;
                ScrollStatsBy(dir * StatsLineH * ticks);
            };

            _statsScrollUp = new MirButton
            {
                Parent = _rightPanel,
                Library = Libraries.Prguse2,
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Sound = SoundList.ButtonA,
                Size = SCROLL_BTN_SIZE,
                Visible = false
            };
            _statsScrollUp.Click += (s, e) => ScrollStatsBy(-StatsLineH * 3);

            _statsScrollDown = new MirButton
            {
                Parent = _rightPanel,
                Library = Libraries.Prguse2,
                Index = 207,
                HoverIndex = 208,
                PressedIndex = 209,
                Sound = SoundList.ButtonA,
                Size = SCROLL_BTN_SIZE,
                Visible = false
            };
            _statsScrollDown.Click += (s, e) => ScrollStatsBy(StatsLineH * 3);

            _statsScrollThumb = new MirImageControl
            {
                Parent = _rightPanel,
                Library = Libraries.Prguse2,
                Index = 205,
                Size = SCROLL_THUMB_SIZE,
                Visible = false
            };

            _rewardsViewport = new MirControl
            {
                Parent = _rightPanel,
                BackColour = Color.FromArgb(16, 16, 16),
                Visible = false
            };
            _rewardsViewport.MouseWheel += (o, e) =>
            {
                int ticks = Math.Max(1, Math.Abs(e.Delta) / 120);
                int dir = e.Delta > 0 ? -1 : +1;
                ScrollRewardsBy(dir * StatsLineH * ticks);
            };

            _rewardsMaskTop = new MirControl
            {
                Parent = _rightPanel,
                BackColour = _rightPanel.BackColour == Color.Empty ? Color.Black : _rightPanel.BackColour,
                NotControl = true,
                Visible = false
            };
            _rewardsMaskBottom = new MirControl
            {
                Parent = _rightPanel,
                BackColour = _rightPanel.BackColour == Color.Empty ? Color.Black : _rightPanel.BackColour,
                NotControl = true,
                Visible = false
            };
            _rewardsScrollUp = new MirButton
            {
                Parent = _rightPanel,
                Library = Libraries.Prguse2,
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Sound = SoundList.ButtonA,
                Size = SCROLL_BTN_SIZE,
                Visible = false
            };
            _rewardsScrollUp.Click += (s, e) => ScrollRewardsBy(-StatsLineH * 3);

            _rewardsScrollDown = new MirButton
            {
                Parent = _rightPanel,
                Library = Libraries.Prguse2,
                Index = 207,
                HoverIndex = 208,
                PressedIndex = 209,
                Sound = SoundList.ButtonA,
                Size = SCROLL_BTN_SIZE,
                Visible = false
            };
            _rewardsScrollDown.Click += (s, e) => ScrollRewardsBy(StatsLineH * 3);

            _rewardsScrollThumb = new MirImageControl
            {
                Parent = _rightPanel,
                Library = Libraries.Prguse2,
                Index = 205,
                Size = SCROLL_THUMB_SIZE,
                Visible = false
            };

            _searchBox = new MirTextBox
            {
                Parent = this,
                Location = new Point(535, 111),
                Size = new Size(110, 17),
                Font = new Font(Settings.FontName, 8F),
                BackColour = Color.Black,
                ForeColour = Color.White,
                BorderColour = Color.Gray,
                Border = true,
                Text = string.Empty
            };
            _searchBox.Enabled = false;

            _searchBox.KeyPress += (s, e) =>
            {
                if (!_searchActivated) return;
                if (e.KeyChar == (char)Keys.Return || e.KeyChar == (char)Keys.LineFeed)
                {
                    if (_searchBox.ForeColour != Color.Gray)
                        ApplySearch(_searchBox.Text);
                    e.Handled = true;
                }
            };

            _searchBox.Click += (s, e) =>
            {
                if (!_searchActivated) ActivateSearch();
            };

            _searchActivated = false;
            _searchGate = new MirControl
            {
                Parent = this,
                Location = _searchBox.Location,
                Size = _searchBox.Size,
                Opacity = 0f,
                Border = false,
            };
            _searchGate.MouseDown += (s, e) =>
            {
                if (e.Button != MouseButtons.Left) return;
                _gatePressed = true;
                _gateDownPt = e.Location;
            };
            _searchGate.MouseUp += (s, e) =>
            {
                if (!_gatePressed || e.Button != MouseButtons.Left) return;
                _gatePressed = false;
                int dx = e.Location.X - _gateDownPt.X;
                int dy = e.Location.Y - _gateDownPt.Y;
                if (dx * dx + dy * dy <= GateDragSqr) ActivateSearch();
            };

            _searchButton = new MirButton
            {
                Parent = this,
                Library = Libraries.Title,
                Index = 480,
                HoverIndex = 481,
                PressedIndex = 482,
                Location = new Point(648, 107),
                Size = new Size(60, 22),
                Sound = SoundList.ButtonA,
            };
            _searchButton.Click += (o, e) =>
            {
                if (!_searchActivated)
                {
                    ActivateSearch();
                    return;
                }
                ApplySearch(_searchBox.Text);
            };

            _refreshSearchButton = new MirButton
            {
                Parent = this,
                Library = Libraries.Prguse2,
                Index = 267,
                HoverIndex = 268,
                PressedIndex = 269,
                Location = new Point(697, 107),
                Size = new Size(60, 22),
                Sound = SoundList.ButtonA,
            };
            _refreshSearchButton.Click += (o, e) => RefreshAllFilters();

            BuildCurrencyUI();

            var close = new MirButton
            {
                Parent = this,
                Library = Libraries.Prguse2,
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Sound = SoundList.ButtonA,
                Size = new Size(22, 18),
                Location = new Point(Size.Width - 24, 6)
            };
            close.Click += (s, e) =>
            {
                DeactivateSearch();
                CloseDropdown();
                Hide();
            };

            BuildFilterDropdownUI();
            _ddPanel?.BringToFront();
            _ddHeader?.BringToFront();

            _levelHintHotspot = new MirControl
            {
                Parent = this,
                Location = new Point(LEVEL_HINT_RECT.X, LEVEL_HINT_RECT.Y),
                Size = new Size(LEVEL_HINT_RECT.Width, LEVEL_HINT_RECT.Height),
                Opacity = 0f,
                Border = true,
                Visible = true
            };
        }

        private MirButton MakeTopTab(Point p, int idxNormal, int idxHover, int idxPressed, string text, string hint, Action onClick)
        {
            var b = new MirButton
            {
                Parent = this,
                Library = Libraries.Title_32bit,
                Index = idxNormal,
                HoverIndex = idxHover,
                PressedIndex = idxPressed,
                Location = p,
                Size = new Size(80, 26),
                Sound = SoundList.ButtonA,
            };
            b.Click += (o, e) => onClick();
            return b;
        }

        private void BuildTopTabs()
        {
            _tabChar = MakeTopTab(new Point(TAB_X0 + 0 * TAB_STEP, TAB_Y), 320, 321, 322, "캐릭터", "Tab: Character", () =>
            {
                _sectionBucket = 0;
                ApplySearch(_searchQuery, keepCursor: false);
                UpdateTopTabVisuals();
            });

            _tabLim = MakeTopTab(new Point(TAB_X0 + 1 * TAB_STEP, TAB_Y), 324, 325, 326, "한정판", "Tab: Limited", () =>
            {
                _sectionBucket = 1;
                ApplySearch(_searchQuery, keepCursor: false);
                UpdateTopTabVisuals();
            });

            _tabEvt = MakeTopTab(new Point(TAB_X0 + 2 * TAB_STEP, TAB_Y), 328, 329, 330, "이벤트", "Tab: Event", () =>
            {
                _sectionBucket = 2;
                ApplySearch(_searchQuery, keepCursor: false);
                UpdateTopTabVisuals();
            });

            UpdateTopTabVisuals();
        }

        private void UpdateTopTabVisuals()
        {
            if (_tabChar != null) _tabChar.Index = (_sectionBucket == 0) ? _tabChar.PressedIndex : 320;
            if (_tabLim != null) _tabLim.Index = (_sectionBucket == 1) ? _tabLim.PressedIndex : 324;
            if (_tabEvt != null) _tabEvt.Index = (_sectionBucket == 2) ? _tabEvt.PressedIndex : 328;
        }

        private void BuildFilterDropdownUI()
        {
            BuildDropdownOptionsFromExistingFilters();

            _ddHeader = new MirButton
            {
                Parent = this,
                Library = Libraries.UI_32bit,
                Index = DD_BAR_NORMAL_IDX,
                HoverIndex = DD_BAR_HOVER_IDX,
                PressedIndex = DD_BAR_HOVER_IDX,
                Location = DD_HDR_POS,
                Size = new Size(156, 22),
                CenterText = true,
                Text = (_ddOptions.Count > 0) ? _ddOptions[_ddSelectedIndex] : "Filter",
                Sound = SoundList.ButtonA
            };
            _ddHeader.Click += (s, e) => ToggleDropdown();

            _ddPanel = new MirImageControl
            {
                Parent = this,
                Library = Libraries.UI_32bit,
                Index = DD_PANEL_IDX,
                Location = DD_PANEL_POS,
                Size = DD_PANEL_SIZE,
                Visible = false
            };

            _ddViewport = new MirControl
            {
                Parent = _ddPanel,
                Location = new Point(DD_ITEM_X_PAD, DD_ITEM_Y_PAD),
                Size = new Size(DD_PANEL_SIZE.Width - DD_ITEM_X_PAD - 24, DD_VISIBLE_ROWS * DD_ITEM_H),
            };
            AttachWheel(_ddViewport);

            UpdateDropdownOverflowMasks();

            int upX = _ddPanel.Size.Width - SCROLL_BTN_SIZE.Width;
            _ddUp = new MirButton
            {
                Parent = _ddPanel,
                Library = Libraries.Prguse2,
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Location = new Point(upX, _ddViewport.Location.Y - SCROLL_BTN_SIZE.Height - -19),
                Size = SCROLL_BTN_SIZE,
                Sound = SoundList.ButtonA
            };
            _ddUp.Click += (s, e) => ScrollDropdownBy(-DD_ITEM_H);

            _ddDown = new MirButton
            {
                Parent = _ddPanel,
                Library = Libraries.Prguse2,
                Index = 207,
                HoverIndex = 208,
                PressedIndex = 209,
                Location = new Point(upX, _ddViewport.Location.Y + _ddViewport.Size.Height + -13),
                Size = SCROLL_BTN_SIZE,
                Sound = SoundList.ButtonA
            };
            _ddDown.Click += (s, e) => ScrollDropdownBy(+DD_ITEM_H);

            bool ddDragging = false;
            int ddGrabOffsetY = 0;

            _ddPanel.MouseMove += (s, e) =>
            {
                if (!ddDragging) return;

                int contentH = _ddOptions.Count * DD_ITEM_H;
                int viewportH = _ddViewport.Size.Height;
                int inner = Math.Max(0, contentH - viewportH);

                int minY = _ddViewport.Location.Y;
                int maxY = _ddViewport.Location.Y + _ddViewport.Size.Height - _ddThumb.Size.Height;

                int newTop = e.Y - ddGrabOffsetY;
                newTop = Math.Max(minY, Math.Min(maxY, newTop));

                float pct = (inner == 0) ? 0f : (float)(newTop - minY) / (float)(maxY - minY);

                int raw = (inner > 0) ? (int)Math.Round(inner * pct) : 0;
                int desired = (raw / DD_ITEM_H) * DD_ITEM_H;

                int delta = desired - _ddScrollPx;
                if (delta != 0) ScrollDropdownBy(delta);
            };
            _ddPanel.MouseUp += (s, e) =>
            {
                if (e.Button != MouseButtons.Left) return;
                ddDragging = false;
            };
            _ddPanel.MouseLeave += (s, e) => { ddDragging = false; };

            RebuildDropdownItems();
            UpdateDropdownScrollUI();
        }

        private void ToggleDropdown()
        {
            _ddOpen = !_ddOpen;

            if (_ddPanel != null)
                _ddPanel.Visible = _ddOpen;

            if (_ddOpen)
            {
                UpdateDropdownOverflowMasks();

                if (_ddOverflowMaskTop != null) _ddOverflowMaskTop.Visible = true;
                if (_ddOverflowMaskBottom != null) _ddOverflowMaskBottom.Visible = true;

                _ddPanel?.BringToFront();
                _ddOverflowMaskTop?.BringToFront();
                _ddOverflowMaskBottom?.BringToFront();
                _ddHeader?.BringToFront();

                RebuildDropdownItems();
                UpdateDropdownScrollUI();
            }
            else
            {
                if (_ddOverflowMaskTop != null) _ddOverflowMaskTop.Visible = false;
                if (_ddOverflowMaskBottom != null) _ddOverflowMaskBottom.Visible = false;
            }
        }

        private void RebuildDropdownItems()
        {
            if (_ddViewport == null) return;

            foreach (var r in _ddItemRows) r.Dispose();
            _ddItemRows.Clear();

            if (_ddOptions == null || _ddOptions.Count == 0) return;

            var barSize = Libraries.UI_32bit.GetSize(DD_BAR_NORMAL_IDX);
            int barYOffset = Math.Max(0, (DD_ITEM_H - barSize.Height) / 2);

            for (int i = 0; i < _ddOptions.Count; i++)
            {
                int y = i * DD_ITEM_H;

                var row = new MirControl
                {
                    Parent = _ddViewport,
                    Location = new Point(0, y),
                    Size = new Size(_ddViewport.Size.Width, DD_ITEM_H),
                    Border = false
                };

                var bar = new MirImageControl
                {
                    Parent = row,
                    Library = Libraries.UI_32bit,
                    Index = DD_BAR_NORMAL_IDX,
                    Location = new Point(0, barYOffset),
                    NotControl = true
                };

                var label = new MirLabel
                {
                    Parent = row,
                    Location = new Point(0, -1),
                    AutoSize = false,
                    Size = row.Size,
                    Text = _ddOptions[i],
                    ForeColour = Color.White,
                    DrawFormat = TextFormatFlags.HorizontalCenter |
                                 TextFormatFlags.VerticalCenter |
                                 TextFormatFlags.SingleLine
                };

                row.MouseEnter += (s, e) => { bar.Index = DD_BAR_HOVER_IDX; };
                row.MouseLeave += (s, e) => { bar.Index = DD_BAR_NORMAL_IDX; };
                label.MouseEnter += (s, e) => { bar.Index = DD_BAR_HOVER_IDX; };
                label.MouseLeave += (s, e) => { bar.Index = DD_BAR_NORMAL_IDX; };

                int captured = i;

                row.Click += (s, e) => SelectDropdownIndex(captured);
                label.Click += (s, e) => SelectDropdownIndex(captured);

                _ddItemRows.Add(row);
            }

            RepositionDropdownItems();
        }


        private void SelectDropdownIndex(int idx)
        {
            if (idx < 0 || idx >= _ddOptions.Count) return;

            _ddSelectedIndex = idx;
            if (_ddHeader != null) _ddHeader.Text = _ddOptions[idx];

            _filterStatKey = (idx == 0) ? null : _ddKeys[idx];

            ApplySearch(_searchQuery, keepCursor: false);

            CloseDropdown();
        }

        private void ScrollDropdownBy(int deltaPx)
        {
            _ddScrollPx += deltaPx;
            ClampDropdownScroll();
            RepositionDropdownItems();
            UpdateDropdownScrollUI();
        }

        private void ClampDropdownScroll()
        {
            int contentH = (_ddOptions != null ? _ddOptions.Count : 0) * DD_ITEM_H;
            int viewportH = (_ddViewport != null) ? _ddViewport.Size.Height : 0;
            int maxOff = Math.Max(0, contentH - viewportH);

            if (_ddScrollPx < 0) _ddScrollPx = 0;
            if (_ddScrollPx > maxOff) _ddScrollPx = maxOff;

            if (DD_ITEM_H > 1) _ddScrollPx = (_ddScrollPx / DD_ITEM_H) * DD_ITEM_H;
        }

        private void RepositionDropdownItems()
        {
            if (_ddViewport == null || _ddItemRows.Count == 0) return;

            int yOff = -_ddScrollPx;

            for (int i = 0; i < _ddItemRows.Count; i++)
            {
                var row = _ddItemRows[i];
                row.Location = new Point(0, i * DD_ITEM_H + yOff);

                bool vis = row.Location.Y + DD_ITEM_H > 0 && row.Location.Y < _ddViewport.Size.Height;
                if (row.Visible != vis) row.Visible = vis;
            }
        }

        private void UpdateDropdownScrollUI()
        {
            if (_ddPanel == null || _ddViewport == null || _ddUp == null || _ddDown == null) return;

            int contentH = (_ddOptions != null ? _ddOptions.Count : 0) * DD_ITEM_H;
            int viewportH = _ddViewport.Size.Height;
            int inner = Math.Max(0, contentH - viewportH);

            bool canScroll = inner > 0;

            _ddUp.Enabled = canScroll;
            _ddDown.Enabled = canScroll;

            if (_ddThumb == null) return;

            _ddThumb.Visible = _ddPanel.Visible && canScroll;
            if (!canScroll) return;

            if (_ddScrollPx < 0) _ddScrollPx = 0;
            if (_ddScrollPx > inner) _ddScrollPx = inner;
            if (DD_ITEM_H > 1) _ddScrollPx = (_ddScrollPx / DD_ITEM_H) * DD_ITEM_H;

            const int DD_THUMB_Y_OFFSET = 0;
            int minY = _ddViewport.Location.Y + DD_THUMB_Y_OFFSET;
            int maxY = _ddViewport.Location.Y + _ddViewport.Size.Height - _ddThumb.Size.Height - DD_THUMB_Y_OFFSET;

            if (maxY <= minY) return;

            float pct = (inner == 0) ? 0f : (float)_ddScrollPx / inner;
            int y = minY + (int)Math.Round(pct * (maxY - minY));
            int x = _ddUp.Location.X + 1;

            _ddThumb.Location = new Point(x, y);
        }

        private void BuildDropdownOptionsFromExistingFilters()
        {
            if (_ddOptions == null || _rows == null) return;

            _ddOptions.Clear();
            _ddKeys.Clear();

            _ddOptions.Add("All");
            _ddKeys.Add(null);

            var present = new HashSet<string>();
            foreach (var r in _rows) foreach (var k in ExtractFilterKeysFromRow(r)) present.Add(k);

            foreach (var pair in _filterKeyOrder)
            {
                if (present.Contains(pair.key))
                {
                    _ddKeys.Add(pair.key);
                    _ddOptions.Add(pair.label);
                }
            }

            if (_ddSelectedIndex < 0) _ddSelectedIndex = 0;
            if (_ddSelectedIndex >= _ddOptions.Count) _ddSelectedIndex = _ddOptions.Count - 1;
        }

        private static string CanonicalStatKey(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            string s = raw.Trim().ToLowerInvariant();

            // English → keys
            if (s.StartsWith("strength") || s == "str") return "strength";
            if (s.StartsWith("intelligence") || s == "int") return "intelligence";
            if (s.StartsWith("endurance") || s.StartsWith("stamina") || s == "sta") return "endurance";
            if (s.StartsWith("willpower") || s.StartsWith("will")) return "willpower";
            if (s.StartsWith("power") || s.StartsWith("might") || s.Contains("attack bonus")) return "power";
            if (s.StartsWith("damage")) return "damage";
            if (s.Contains("crit") && (s.Contains("rate") || s.Contains("chance"))) return "crit_rate";
            if (s.Contains("crit") && s.Contains("damage")) return "crit_damage";
            if (s == "hp" || s.StartsWith("max hp")) return "max_hp";
            if (s == "mp" || s.StartsWith("max mp")) return "max_mp";
            if (s.Contains("hp") && s.Contains("regen")) return "hp_regen";
            if (s.Contains("mp") && s.Contains("regen")) return "mp_regen";

            // group + common stats
            if (s == "ac") return "ac";
            if (s == "mac") return "mac";
            if (s == "dc") return "dc";
            if (s == "mc") return "mc";
            if (s == "sc") return "sc";
            if (s == "accuracy") return "accuracy";
            if (s == "luck") return "luck";

            return null;
        }

        private static readonly (string key, string label)[] _filterKeyOrder = new[]
        {
        ("strength",    "Strength"),
        ("intelligence","Intelligence"),
        ("power",       "Power"),
        ("damage",      "Damage"),
        ("endurance",   "Endurance"),
        ("willpower",   "Willpower"),
        ("crit_rate",   "Critical Hit Chance"),
        ("crit_damage", "Critical Damage"),
        ("max_hp",      "Max HP"),
        ("max_mp",      "Max MP"),
        ("hp_regen",    "HP Regeneration"),
        ("mp_regen",    "MP Regeneration"),

        ("ac", "AC"), ("mac","MAC"), ("dc","DC"), ("mc","MC"), ("sc","SC"),
        ("accuracy","Accuracy"), ("luck","Luck"),
        };

        private HashSet<string> ExtractFilterKeysFromRow(RowVM vm)
        {
            var set = new HashSet<string>();
            if (vm == null) return set;

            var values = vm.Reward?.Values;
            if (values != null && values.Count > 0)
            {
                foreach (var kv in values)
                {
                    string name = kv.Key.ToString();
                    if (name.StartsWith("Min")) name = name.Substring(3);
                    if (name.StartsWith("Max")) name = name.Substring(3);
                    var k = CanonicalStatKey(name) ?? CanonicalStatKey(PrettyStatName(kv.Key));
                    if (k != null) set.Add(k);
                }
            }

            foreach (var line in BuildStatLines(vm))
            {
                string token = line;
                int cut = token.IndexOfAny(" +-0123456789~:".ToCharArray());
                if (cut > 0) token = token.Substring(0, cut).Trim();
                var k = CanonicalStatKey(token);
                if (k != null) set.Add(k);
            }

            return set;
        }

        private void UpdateDropdownOverflowMasks()
        {
            if (_ddPanel == null || _ddViewport == null) return;

            var native = Libraries.UI_32bit.GetSize(DD_PANEL_IDX);

            int topPad = _ddViewport.Location.Y;
            int innerH = _ddViewport.Size.Height;
            int bottomPad = Math.Max(0, native.Height - (topPad + innerH));

            var bg = (BackColour == Color.Empty ? Color.Black : BackColour);

            if (_ddOverflowMaskTop == null)
                _ddOverflowMaskTop = new MirControl { Parent = this, BackColour = bg, NotControl = true, Visible = false };

            if (_ddOverflowMaskBottom == null)
                _ddOverflowMaskBottom = new MirControl { Parent = this, BackColour = bg, NotControl = true, Visible = false };

            Point shell = _ddPanel.Location;

            _ddOverflowMaskTop.Location = new Point(shell.X, shell.Y);
            _ddOverflowMaskTop.Size = new Size(native.Width, Math.Max(0, topPad));

            _ddOverflowMaskBottom.Location = new Point(shell.X, shell.Y + topPad + innerH);
            _ddOverflowMaskBottom.Size = new Size(native.Width, bottomPad);
        }

        private void BuildProgressRow(int idx, string label)
        {
            int iconIdx = idx switch
            {
                1 => RIGHT_ICON_CHARACTER,
                2 => RIGHT_ICON_LIMITED,
                3 => RIGHT_ICON_EVENT,
                _ => RIGHT_ICON_BOOK,
            };

            var icon = new MirImageControl
            {
                Parent = _rightPanel,
                Library = Libraries.UI_32bit,
                Index = iconIdx,
                NotControl = true
            };
            _rightIcons.Add(icon);

            var bar = new MirImageControl
            {
                Parent = _rightPanel,
                Library = Libraries.UI_32bit,
                Index = BAR_BLUE,
                DrawImage = false,
                NotControl = true,
                Size = new Size(160, 14),
                Visible = true
            };
            bar.BeforeDraw += (s, e) => DrawRightBar(idx, bar);
            _rightBars.Add(bar);

            var lbl = new MirLabel
            {
                Parent = _rightPanel,
                AutoSize = true,
                ForeColour = Color.Silver,
                Text = $"{label} (0/0)"
            };
            _rightLabels.Add(lbl);
        }

        private void DrawRightBar(int idx, MirImageControl bar)
        {
            if (bar == null || bar.Library == null) return;

            var loc = bar.DisplayLocation;
            var size = bar.Size;

            bar.Library.Draw(BAR_BASE, loc, size, Color.White);

            int need = _barNeed[idx];
            int found = _barFound[idx];
            if (need <= 0) return;

            double percent = found / (double)need;
            percent = Math.Max(0, Math.Min(1, percent));

            if (percent <= 0) return;

            if (percent >= 1.0)
            {
                bar.Library.Draw(BAR_GREEN, loc, size, Color.White);
                return;
            }

            int inner = Math.Max(0, size.Width - 3);
            int fillW = (int)(inner * percent);
            if (fillW <= 0) return;

            Rectangle section = new Rectangle(Point.Empty, new Size(fillW, size.Height));
            bar.Library.Draw(BAR_BLUE, section, loc, Color.White, false);
        }

        private void DrawXpFill(MirImageControl fill)
        {
            if (fill == null || fill.Library == null) return;

            ComputeLevelProgress(out int level, out int xpWithin, out int needWithin);

            double pct = needWithin > 0 ? xpWithin / (double)needWithin : 0.0;
            pct = Math.Max(0, Math.Min(1, pct));

            int innerWidth = GetXpBarInnerWidth();
            int fillW = (int)Math.Round(innerWidth * pct);
            if (fillW <= 0) return;

            var loc = fill.DisplayLocation;
            Rectangle section = new Rectangle(Point.Empty, new Size(fillW, XP_BAR_H));
            fill.Library.Draw(XP_BAR_FILL, section, loc, Color.White, false);
        }

        private void DoLayout()
        {
            _leftNav.Location = new Point(8, TOP_BAR_H + LEFT_NAV_TOP_PAD);
            _leftNav.Size = new Size(LEFT_NAV_W - 10, Size.Height - TOP_BAR_H - LEFT_NAV_TOP_PAD - 8);

            _listViewport.Location = new Point(LIST_X, LIST_Y);
            _listViewport.Size = new Size(VIEWPORT_WIDTH, VIEWPORT_HEIGHT);

            _maskTop.Location = new Point(_listViewport.Location.X, _listViewport.Location.Y - 2000);
            _maskTop.Size = new Size(_listViewport.Size.Width, 2000);
            _maskBottom.Location = new Point(_listViewport.Location.X, _listViewport.Location.Y + _listViewport.Size.Height);
            _maskBottom.Size = new Size(_listViewport.Size.Width, 2000);

            _scrollUp.Location = SCROLL_UP_POS;
            _scrollDown.Location = SCROLL_DOWN_POS;
            _scrollThumb.Location = SCROLL_THUMB_POS;

            _rightPanel.Location = RightUI.Panel.Location;
            _rightPanel.Size = RightUI.Panel.Size;

            if (_rightHeader != null) _rightHeader.Location = RightUI.Header;
            if (_rightPrev != null) _rightPrev.Location = RightUI.Prev;
            if (_rightNext != null) _rightNext.Location = RightUI.Next;

            for (int i = 0; i < _rightLabels.Count; i++)
            {
                int yAdd = i * RightUI.RowStepY;

                if (i < _rightIcons.Count && _rightIcons[i] != null)
                    _rightIcons[i].Location = new Point(RightUI.Icon0.X, RightUI.Icon0.Y + yAdd);

                if (i < _rightBars.Count && _rightBars[i] != null)
                {
                    _rightBars[i].Location = new Point(RightUI.Bar0.X, RightUI.Bar0.Y + yAdd);
                    _rightBars[i].Size = new Size(160, 14);
                    _rightBars[i].Visible = true;
                }

                if (_rightLabels[i] != null)
                    _rightLabels[i].Location = new Point(RightUI.Text0.X, RightUI.Text0.Y + yAdd);
            }

            _rewardsViewport.Location = RightUI.RewardsRect.Location;
            _rewardsViewport.Size = RightUI.RewardsRect.Size;
            _rewardsScrollUp.Location = RightUI.RewardsUp;
            _rewardsScrollDown.Location = RightUI.RewardsDown;
            _rewardsScrollThumb.Location = RightUI.RewardsThumb;

            if (_rewardsViewport != null)
            {
                if (_rewardsMaskTop != null)
                {
                    _rewardsMaskTop.Location = new Point(_rewardsViewport.Location.X, _rewardsViewport.Location.Y - 2000);
                    _rewardsMaskTop.Size = new Size(_rewardsViewport.Size.Width, 2000);
                }
                if (_rewardsMaskBottom != null)
                {
                    _rewardsMaskBottom.Location = new Point(_rewardsViewport.Location.X, _rewardsViewport.Location.Y + _rewardsViewport.Size.Height);
                    _rewardsMaskBottom.Size = new Size(_rewardsViewport.Size.Width, 2000);
                }
            }
            RebuildList();
            UpdateScrollButtons();
            UpdateScrollThumb();
            RefreshHeaderLevelXp();
            RefreshLevelHint();
            UpdateRightPage();
            RefreshClaimedSetsEmblem();
            ComputeLevelProgress(out int lvl0, out _, out _);
            _lastLevelSeen = Math.Max(1, lvl0);
        }

        public void ApplySync(S.ItemCodexSync p)
        {
            _rows.Clear();
            RewardBySet.Clear();
            ClaimedSetIds.Clear();

            if (p?.Rows != null)
            {
                foreach (var r in p.Rows)
                {
                    var vm = new RowVM
                    {
                        SetId = r.Id,
                        Title = r.Name,
                        Found = r.Found,
                        Required = r.Required,
                        Claimed = r.Claimed,
                        Reward = r.Reward ?? new Stats(),
                        RewardText = r.RewardPreview,
                        Bucket = r.Bucket,
                        RewardXP = r.RewardXP,
                    Rarity = (ItemGrade)r.Rarity,
                    Active = r.Active,
                    KeepStats = r.KeepStats,
                    StartTime = (r.StartTicks >= 0) ? new DateTime(r.StartTicks) : (DateTime?)null,
                    EndTime = (r.EndTicks >= 0) ? new DateTime(r.EndTicks) : (DateTime?)null
                    };

                    if (r.ReqItemIndices != null) vm.ReqItemIndices.AddRange(r.ReqItemIndices);
                    if (r.ReqStages != null) vm.ReqStages.AddRange(r.ReqStages);
                    if (r.ReqItemIcons != null) vm.ReqItemIcons.AddRange(r.ReqItemIcons);
                    if (r.ReqRegistered != null) vm.ReqRegistered.AddRange(r.ReqRegistered);

                    _rows.Add(vm);

                    RewardBySet[r.Id] = vm.Reward;
                    if (vm.Claimed) ClaimedSetIds.Add(r.Id);
                }
            }

            _viewRows = _rows.ToList();
            _selectedIndex = _viewRows.Count > 0 ? 0 : -1;
            _scrollOffsetPx = 0;

            BuildLeftRarityButtons();
            BuildDropdownOptionsFromExistingFilters();

            if (_filterStatKey != null && (_ddKeys == null || !_ddKeys.Contains(_filterStatKey)))
            {
                _filterStatKey = null;
                _ddSelectedIndex = 0;
            }

            if (_ddHeader != null && _ddOptions.Count > 0)
                _ddHeader.Text = _ddOptions[_ddSelectedIndex];

            RebuildDropdownItems();
            UpdateDropdownScrollUI();

            ComputeLevelProgress(out int lvlNow, out _, out _);
            _lastLevelSeen = Math.Max(1, lvlNow);

            bool filtersActive = _filterStatKey != null
                                 || _filterRarity.HasValue
                                 || !string.IsNullOrWhiteSpace(_searchQuery);

            if (filtersActive)
            {
                ApplySearch(_searchQuery, keepCursor: false);
            }
            else
            {
                RebuildList();
                UpdateScrollThumb();
            }

            RecalcRightCounters();
            RefreshHeaderLevelXp();
            RefreshLevelHint();

            RebuildStatsList();
            RebuildRewardsList();
            UpdateRightPage();
            RefreshClaimedSetsEmblem();

            CodexChanged?.Invoke();
        }

        public void ApplyUpdate(S.ItemCodexUpdate p)
        {
            if (p == null) return;

            var row = _rows.FirstOrDefault(x => x.SetId == p.Id);
            if (row == null) return;

            row.Found = p.Found;
            row.Required = p.Required;
            row.Claimed = p.Claimed;

            if (row.Claimed) ClaimedSetIds.Add(p.Id);
            else ClaimedSetIds.Remove(p.Id);

            ApplySearch(_searchQuery, keepCursor: true);
            RecalcRightCounters();
            RefreshHeaderLevelXp();
            RefreshLevelHint();

            RefreshRarityButtonStates();

            RebuildStatsList();
            RebuildRewardsList();
            UpdateRightPage();
            RefreshClaimedSetsEmblem();

            CodexChanged?.Invoke();
        }

        public void MarkRequirement(int setId, int itemInfoId, sbyte stage, bool registered)
        {
            var vm = _rows.FirstOrDefault(x => x.SetId == setId);
            if (vm == null) return;

            int ix = FindRequirementSlot(vm, itemInfoId, stage);
            if (ix >= 0)
            {
                while (vm.ReqRegistered.Count < vm.ReqItemIndices.Count) vm.ReqRegistered.Add(false);
                vm.ReqRegistered[ix] = registered;

                int got = vm.ReqRegistered.Count(b => b);
                int need = vm.Required > 0 ? vm.Required : vm.ReqItemIndices.Count;
                vm.Found = (short)Math.Min(need, got);
            }

            ApplySearch(_searchQuery, keepCursor: true);
            RecalcRightCounters();
            RefreshHeaderLevelXp();
            RefreshLevelHint();

            RebuildStatsList();
            RebuildRewardsList();
            UpdateRightPage();
        }

        private static int FindRequirementSlot(RowVM vm, int itemInfoId, sbyte stage)
        {
            if (vm == null) return -1;
            for (int i = 0; i < vm.ReqItemIndices.Count; i++)
            {
                if (vm.ReqItemIndices[i] != itemInfoId) continue;
                sbyte reqStage = (i < vm.ReqStages.Count) ? vm.ReqStages[i] : (sbyte)-1;
                if (reqStage == -1 || stage == -1 || reqStage == stage)
                    return i;
            }
            return -1;
        }

        private MirButton MakeNavButton(Point p, string text, string hint)
        {
            return new MirButton
            {
                Parent = _leftNav,
                Library = Libraries.Prguse2,
                Index = 918,
                HoverIndex = 919,
                PressedIndex = 919,
                Location = p,
                Size = new Size(NAV_BTN_W, NAV_BTN_H),
                Sound = SoundList.ButtonA,
                Text = text,
                Hint = hint
            };
        }

        private void BuildLeftRarityButtons()
        {
            if (_leftNav == null) return;

            foreach (var c in _leftNav.Controls.ToArray())
                if (c is MirButton b && (ReferenceEquals(b, _btnAll) || _btnByRarity.Values.Contains(b)))
                    b.Dispose();

            _btnAll = null;
            _btnByRarity.Clear();

            int y = NAV_BTN_Y0;

            _btnAll = MakeNavButton(new Point(NAV_BTN_X, y), L("Codex_ShowAllButton"), L("Codex_ShowAllHint"));
            _btnAll.Click += (s, e) => SetRarityFilter(null);
            y += NAV_BTN_STEP;

            var present = _rows
                .Where(r => r != null && r.Rarity != ItemGrade.None)
                .Select(r => r.Rarity)
                .Distinct()
                .OrderBy(r => r);

            foreach (var r in present)
            {
                string label = r.ToString();
                string buttonText = $"{label}";
                var btn = MakeNavButton(new Point(NAV_BTN_X, y), buttonText, L("Codex_ShowRarityHint", label));
                var rr = r;
                btn.Click += (s, e) => SetRarityFilter(rr);
                _btnByRarity[rr] = btn;
                y += NAV_BTN_STEP;
            }

            RefreshRarityButtonStates();
        }

        private void SetRarityFilter(ItemGrade? rarity, bool rebuild = true)
        {
            _filterRarity = rarity;
            RefreshRarityButtonStates();

            if (rebuild)
            {
                ApplySearch(_searchQuery, keepCursor: false);
            }
        }

        private void RefreshRarityButtonStates()
        {
            if (_btnAll != null)
                _btnAll.Index = (_filterRarity == null) ? 919 : 918;

            foreach (var kv in _btnByRarity)
            {
                bool active = _filterRarity.HasValue && kv.Key == _filterRarity.Value;
                kv.Value.Index = active ? 919 : 918;
                kv.Value.ForeColour = active ? Color.White : Color.Gainsboro;
            }
        }

        private void ActivateSearch()
        {
            if (_searchActivated) return;
            _searchActivated = true;

            _searchGate?.Dispose();
            _searchGate = null;

            if (_searchBox != null)
                _searchBox.Enabled = true;
        }

        private void ApplySearch(string raw, bool keepCursor = false)
        {
            _searchQuery = raw ?? string.Empty;
            string q = _searchQuery.Trim().ToLowerInvariant();

            IEnumerable<RowVM> src = _rows;

            src = src.Where(r => r != null && r.Bucket == _sectionBucket);

            if (_filterRarity.HasValue)
                src = src.Where(r => r.Rarity == _filterRarity.Value);

            if (!string.IsNullOrEmpty(_filterStatKey))
                src = src.Where(r => ExtractFilterKeysFromRow(r).Contains(_filterStatKey));

            if (!string.IsNullOrEmpty(q))
            {
                src = src.Where(r =>
                {
                    if ((r.Title ?? "").ToLowerInvariant().Contains(q)) return true;

                    foreach (var ix in r.ReqItemIndices)
                    {
                        if (ix.ToString().Contains(q)) return true;
                        var info = GameScene.Scene?.GetItemInfo(ix);
                        if (info != null && (info.Name ?? "").ToLowerInvariant().Contains(q)) return true;
                    }
                    return false;
                });
            }

            _viewRows = src.ToList();

            if (!keepCursor)
            {
                _selectedIndex = _viewRows.Count > 0 ? 0 : -1;
                _scrollOffsetPx = 0;
            }

            RebuildList();
            UpdateScrollButtons();
            UpdateScrollThumb();
        }

        private void DeactivateSearch(bool rebuildGate = true)
        {
            _searchActivated = false;

            if (_searchBox != null)
                _searchBox.Enabled = false;

            if (!rebuildGate || IsDisposed || _searchBox == null) return;
            if (_searchGate == null)
            {
                _searchGate = new MirControl
                {
                    Parent = this,
                    Location = _searchBox.Location,
                    Size = _searchBox.Size,
                    Opacity = 0f,
                    Border = false,
                };
                _searchGate.MouseDown += (s, e) =>
                {
                    if (e.Button != MouseButtons.Left) return;
                    _gatePressed = true;
                    _gateDownPt = e.Location;
                };
                _searchGate.MouseUp += (s, e) =>
                {
                    if (!_gatePressed || e.Button != MouseButtons.Left) return;
                    _gatePressed = false;
                    int dx = e.Location.X - _gateDownPt.X;
                    int dy = e.Location.Y - _gateDownPt.Y;
                    if (dx * dx + dy * dy <= GateDragSqr) ActivateSearch();
                };
            }
        }

        private void ClearSearch()
        {
            _searchQuery = string.Empty;

            IEnumerable<RowVM> src = _rows.Where(r => r != null && r.Bucket == _sectionBucket);
            if (_filterRarity.HasValue) src = src.Where(r => r.Rarity == _filterRarity.Value);

            _viewRows = src.ToList();
            _selectedIndex = _viewRows.Count > 0 ? 0 : -1;
            _scrollOffsetPx = 0;

            if (_searchBox != null)
                _searchBox.Text = string.Empty;

            RebuildList();
            UpdateScrollButtons();
            UpdateScrollThumb();
        }

        private void RefreshAllFilters()
        {
            _searchQuery = string.Empty;
            if (_searchBox != null) _searchBox.Text = string.Empty;

            SetRarityFilter(null, rebuild: false);

            _sectionBucket = 0;

            BuildLeftRarityButtons();
            UpdateTopTabVisuals();

            ApplySearch(_searchQuery, keepCursor: false);
            UpdateScrollButtons();
            UpdateScrollThumb();
            RecalcRightCounters();
            RefreshHeaderLevelXp();
            RefreshLevelHint();
            UpdateRightPage();
        }

        private void RebuildList()
        {
            if (_listViewport == null) return;

            foreach (var c in _listViewport.Controls.ToArray())
                c.Dispose();

            int viewH = _listViewport.Size.Height;
            int firstIndex = Math.Max(0, _scrollOffsetPx / RowStep);
            int intraOffset = _scrollOffsetPx % RowStep;

            int y = -intraOffset;
            for (int i = firstIndex; i < _viewRows.Count; i++)
            {
                if (y >= viewH) break;

                var rc = MakeRow(_viewRows[i], i);
                rc.Parent = _listViewport;
                rc.Location = new Point(6, y + 6);
                y += RowStep;
            }

            _scrollUp?.BringToFront();
            _scrollDown?.BringToFront();
            _scrollThumb?.BringToFront();
        }

        private int MaxSubLinesForRow()
        {
            int usable = ROW_H - ROW_REWARD_Y - 4;
            return Math.Max(1, usable / ROW_LINE_STEP);
        }

        private MirControl MakeRow(RowVM data, int index)
        {
            var row = new MirControl
            {
                Size = new Size(_listViewport.Size.Width - 12, ROW_H),
                Border = false
            };

            var hintLines = new List<string>();
            if (data.StartTime.HasValue) hintLines.Add(L("Codex_StartsOn", data.StartTime.Value.ToString("yyyy-MM-dd HH:mm")));
            if (data.EndTime.HasValue) hintLines.Add(L("Codex_ActiveUntil", data.EndTime.Value.ToString("yyyy-MM-dd HH:mm")));
            if (!data.Active) hintLines.Add(data.KeepStats ? L("Codex_ExpiredKeep") : L("Codex_Expired"));
            if (hintLines.Count > 0) row.Hint = string.Join("\n", hintLines);

            AttachWheel(row);
            row.MouseDown += (s, e) =>
            {
                if (e.Button == MouseButtons.Left) SelectIndex(index);
            };

            _ = new MirImageControl
            {
                Parent = row,
                Library = Libraries.UI_32bit,
                Index = ROW_BG_INDEX,
                Location = new Point(0, 0),
                NotControl = true
            };

            const int cell = 32;
            const int pad = 4;
            const int ACTION_RESERVE_W = 84;
            const int GRID_OFFSET_X = 40;
            const int GRID_OFFSET_Y = -9;
            const int L5_SHIFT_X = 0;
            const int L5_SHIFT_Y = 13;

            int totalReq = Math.Min(10, data.ReqItemIndices?.Count ?? 0);

            int labelRightLimit;
            if (totalReq > 0)
            {
                Point[] layout5 = new[]
                {
            new Point(L5_SHIFT_X + 0 * (cell + pad), L5_SHIFT_Y + 0),
            new Point(L5_SHIFT_X + 1 * (cell + pad), L5_SHIFT_Y + 0),
            new Point(L5_SHIFT_X + 2 * (cell + pad), L5_SHIFT_Y + 0),
            new Point(L5_SHIFT_X + 3 * (cell + pad), L5_SHIFT_Y + 0),
            new Point(L5_SHIFT_X + 4 * (cell + pad), L5_SHIFT_Y + 0),
        };
                Point[] layout10 = new[]
                {
            new Point(0 * (cell + pad), 0),
            new Point(1 * (cell + pad), 0),
            new Point(2 * (cell + pad), 0),
            new Point(3 * (cell + pad), 0),
            new Point(4 * (cell + pad), 0),

            new Point(0 * (cell + pad), cell + pad),
            new Point(1 * (cell + pad), cell + pad),
            new Point(2 * (cell + pad), cell + pad),
            new Point(3 * (cell + pad), cell + pad),
            new Point(4 * (cell + pad), cell + pad),
        };
                var tplProbe = (totalReq <= 5) ? layout5 : layout10;

                int rightEdge = row.Size.Width - 10 - ACTION_RESERVE_W + GRID_OFFSET_X;
                int minCx = int.MaxValue;
                for (int i = 0; i < totalReq; i++)
                {
                    Point off = tplProbe[i];
                    int cx = rightEdge - cell - off.X;
                    if (cx < minCx) minCx = cx;
                }

                labelRightLimit = (minCx == int.MaxValue) ? row.Size.Width - ACTION_RESERVE_W - 10 : minCx - 6;
            }
            else
            {
                labelRightLimit = row.Size.Width - ACTION_RESERVE_W - 10;
            }

            int labelLeft = 3;
            int maxLabelW = Math.Max(120, labelRightLimit - labelLeft);

            Color titleColor = data.Active ? RarityColor(data.Rarity) : Color.DimGray;

            _ = new MirLabel
            {
                Parent = row,
                Location = new Point(labelLeft, ROW_TITLE_Y),
                AutoSize = false,
                Size = new Size(maxLabelW, 20),
                ForeColour = titleColor,
                Font = new Font(Settings.FontName, 10f, FontStyle.Bold),
                Text = data.Title ?? L("Codex_UntitledCollection")
            };

            int lineY = ROW_REWARD_Y;
            int shown = 0;
            int maxLines = MaxSubLinesForRow();

            Color codexExpColor = Color.FromArgb(118, 206, 255);
            Color statRewardColor = Color.FromArgb(210, 234, 255);

            void AddSubLine(string text, Color color)
            {
                if (string.IsNullOrWhiteSpace(text) || shown >= maxLines) return;
                _ = new MirLabel
                {
                    Parent = row,
                    Location = new Point(labelLeft, lineY),
                    AutoSize = false,
                    Size = new Size(maxLabelW, 18),
                    ForeColour = color,
                    Font = new Font(Settings.FontName, 9f, FontStyle.Regular),
                    Text = text
                };
                shown++;
                lineY += ROW_LINE_STEP;
            }

            // Time/status lines (Event/Limited)
            string timeLine = null;
            if (data.EndTime.HasValue && data.Active)
                timeLine = L("Codex_ActiveUntil", data.EndTime.Value.ToString("yyyy-MM-dd HH:mm"));
            else if (data.StartTime.HasValue && !data.Active)
                timeLine = L("Codex_StartsOn", data.StartTime.Value.ToString("yyyy-MM-dd HH:mm"));

            if (!string.IsNullOrEmpty(timeLine))
                AddSubLine(timeLine, Color.Gainsboro);

            if (!data.Active)
            {
                string status = data.KeepStats ? L("Codex_ExpiredKeep") : L("Codex_Expired");
                AddSubLine(status, Color.LightGray);
            }

            if (data.RewardXP > 0)
                AddSubLine(L("Codex_ExpLabel", data.RewardXP), codexExpColor);

            var rawLines = BuildStatLines(data);
            var preview = BuildRowPreviewLines(rawLines, 3, out var fullHint);

            foreach (var line in preview)
            {
                AddSubLine(line, statRewardColor);
                if (shown >= maxLines) break;
            }

            if (!string.IsNullOrEmpty(fullHint))
            {
                int hintWidth = Math.Max(32, labelRightLimit);
                var hintArea = new MirControl
                {
                    Parent = row,
                    Location = new Point(0, 0),
                    Size = new Size(hintWidth, row.Size.Height),
                    Opacity = 0f,
                    Hint = fullHint,
                    Border = false
                };
                AttachWheel(hintArea);
                hintArea.MouseDown += (s, e) => { if (e.Button == MouseButtons.Left) SelectIndex(index); };
            }

            if (totalReq > 0)
            {
                int rightEdge = row.Size.Width - 10 - ACTION_RESERVE_W + GRID_OFFSET_X;
                int baseTopY = 12 + GRID_OFFSET_Y;

                Point[] layout5 = new[]
                {
            new Point(L5_SHIFT_X + 0 * (cell + pad), L5_SHIFT_Y + 0),
            new Point(L5_SHIFT_X + 1 * (cell + pad), L5_SHIFT_Y + 0),
            new Point(L5_SHIFT_X + 2 * (cell + pad), L5_SHIFT_Y + 0),
            new Point(L5_SHIFT_X + 3 * (cell + pad), L5_SHIFT_Y + 0),
            new Point(L5_SHIFT_X + 4 * (cell + pad), L5_SHIFT_Y + 0),
        };
                Point[] layout10 = new[]
                {
            new Point(0 * (cell + pad), 0),
            new Point(1 * (cell + pad), 0),
            new Point(2 * (cell + pad), 0),
            new Point(3 * (cell + pad), 0),
            new Point(4 * (cell + pad), 0),

            new Point(0 * (cell + pad), cell + pad),
            new Point(1 * (cell + pad), cell + pad),
            new Point(2 * (cell + pad), cell + pad),
            new Point(3 * (cell + pad), cell + pad),
            new Point(4 * (cell + pad), cell + pad),
        };

                var tpl = (totalReq <= 5) ? layout5 : layout10;

                while (data.ReqRegistered.Count < data.ReqItemIndices.Count) data.ReqRegistered.Add(false);

                for (int i = 0; i < totalReq; i++)
                {
                    int itemInfoId = data.ReqItemIndices[i];
                    sbyte reqStage = (i < data.ReqStages.Count) ? data.ReqStages[i] : (sbyte)-1;
                    Point off = tpl[i];

                    int cx = rightEdge - cell - off.X;
                    int cy = baseTopY + off.Y;

                    int iconIndex = (i < data.ReqItemIcons.Count) ? data.ReqItemIcons[i] : 0;
                    if (iconIndex <= 0)
                    {
                        var info = GameScene.Scene?.GetItemInfo(itemInfoId);
                        if (info != null) iconIndex = info.Image;
                        if (IconResolver != null) iconIndex = IconResolver(itemInfoId);
                    }

                    bool registered = data.ReqRegistered[i];
                    bool hasInInv = HasItemInAnyInventory(itemInfoId, reqStage);
                    bool needsGrey = !(registered || hasInInv);

                    _ = new MirImageControl
                    {
                        Parent = row,
                        Library = Libraries.UI_32bit,
                        Index = CELL_FRAME_IDX,
                        Location = new Point(cx, cy),
                        NotControl = true
                    };

                    if (iconIndex > 0)
                    {
                        // Center the item icon within the cell frame
                        Size iconSize = Libraries.Items.GetSize(iconIndex);
                        if (iconSize.IsEmpty) iconSize = new Size(cell - 4, cell - 4);
                        int ix = cx + (cell - iconSize.Width) / 2;
                        int iy = cy + (cell - iconSize.Height) / 2;

                        var iconCtrl = new MirImageControl
                        {
                            Parent = row,
                            Library = Libraries.Items,
                            Index = iconIndex,
                            Location = new Point(ix, iy),
                            Size = iconSize,
                            DrawImage = true,
                            NotControl = true
                        };

                        if (needsGrey)
                        {
                            int drawIndex = iconIndex;
                            iconCtrl.DrawImage = false;
                            iconCtrl.BeforeDraw += (s, e) =>
                            {
                                var lib = Libraries.Items;
                                var pos = iconCtrl.DisplayLocation;
                                lib.Draw(drawIndex, pos, Color.FromArgb(140, 140, 140));
                            };
                        }
                    }

                    if (registered)
                    {
                        _ = new MirImageControl
                        {
                            Parent = row,
                            Library = Libraries.UI_32bit,
                            Index = EMBLEM_DONE_IDX,
                            Location = new Point(cx + cell - 15, cy + cell - 19),
                            NotControl = true
                        };
                    }
                    else if (hasInInv)
                    {
                        _ = new MirImageControl
                        {
                            Parent = row,
                            Library = Libraries.UI_32bit,
                            Index = EMBLEM_READY_IDX,
                            Location = new Point(cx + 2, cy + 2),
                            NotControl = true
                        };
                    }

                    var hit = new MirControl
                    {
                        Parent = row,
                        Location = new Point(cx, cy),
                        Size = new Size(cell, cell),
                        Opacity = 0f
                    };

                    hit.MouseEnter += (s, e) =>
                    {
                        if (TooltipSuppressed) return;
                        var h = (MirControl)s;
                        Point anchor = new Point(h.DisplayLocation.X, h.DisplayLocation.Y + h.Size.Height + 2);
                        ShowItemTooltip(itemInfoId, reqStage, anchor);
                    };

                    hit.MouseLeave += (s, e) => HideItemTooltip();

                    hit.MouseWheel += (o, e) =>
                    {
                        HideItemTooltip();
                        SuppressTooltips(200);
                        int ticks = Math.Max(1, Math.Abs(e.Delta) / 120);
                        int dir = e.Delta > 0 ? -1 : +1;
                        ScrollBy(dir * RowStep * ticks);
                    };
                }
            }

            const int STATUS_RIGHT_MARGIN = -36;
            const int STATUS_TOP_Y_CLAIMED = 25;

            const int ACTION_BTN_RIGHT_MARGIN = -28;
            const int ACTION_BTN_TOP_Y = 22;

            bool SetIsComplete(RowVM r) => Math.Max(0, (int)r.Required) > 0 && Math.Max(0, (int)r.Found) >= Math.Max(0, (int)r.Required);
            bool finished = SetIsComplete(data);

            if (finished && !data.Claimed)
            {
                var actionBtn = new MirButton
                {
                    Parent = row,
                    Library = Libraries.Title_32bit,
                    Index = 307,
                    HoverIndex = 308,
                    PressedIndex = 309,
                    Sound = SoundList.ButtonA,
                };
                actionBtn.Location = new Point(
                    row.Size.Width - ACTION_BTN_RIGHT_MARGIN - actionBtn.Size.Width,
                    ACTION_BTN_TOP_Y
                );
                actionBtn.Click += (o, e) => HandleActionClick(actionBtn, data);
            }
            else if (!finished)
            {
                bool hasEligibleItem = TryFindFirstEligibleRequirement(data, out _, out _);

                bool canUseStone = (data.Rarity == ItemGrade.Rare) && (GameScene.Stone > 0);
                bool canUseJade = (data.Rarity == ItemGrade.Legendary) && (GameScene.Jade > 0);
                bool canUseCurrency = canUseStone || canUseJade;

                if (hasEligibleItem || canUseCurrency)
                {
                    var submitBtn = new MirButton
                    {
                        Parent = row,
                        Library = Libraries.Title_32bit,
                        Index = 304,
                        HoverIndex = 305,
                        PressedIndex = 306,
                        Sound = SoundList.ButtonA,
                    };
                    submitBtn.Location = new Point(
                        row.Size.Width - ACTION_BTN_RIGHT_MARGIN - submitBtn.Size.Width,
                        ACTION_BTN_TOP_Y
                    );
                    submitBtn.Click += (o, e) => PromptSubmitChoice(data);
                }
                else
                {
                    var incompleteLbl = new MirLabel
                    {
                        Parent = row,
                        AutoSize = true,
                        ForeColour = Color.Silver,
                        Font = new Font(Settings.FontName, 9f, FontStyle.Regular),
                        Text = "[ Incomplete ]",
                        NotControl = true
                    };
                    incompleteLbl.Location = new Point(
                        row.Size.Width - STATUS_RIGHT_MARGIN - incompleteLbl.Size.Width,
                        STATUS_TOP_Y_CLAIMED
                    );
                }
            }
            else
            {
                var completedLine = new MirLabel
                {
                    Parent = row,
                    AutoSize = true,
                    ForeColour = Color.LimeGreen,
                    Font = new Font(Settings.FontName, 9f, FontStyle.Regular),
                    Text = "[ Completed ]",
                    NotControl = true
                };
                completedLine.Location = new Point(
                    row.Size.Width - STATUS_RIGHT_MARGIN - completedLine.Size.Width,
                    STATUS_TOP_Y_CLAIMED
                );
            }

            return row;
        }

        private void HandleActionClick(MirButton btn, RowVM data)
        {
            if (btn == null || data == null) return;
            btn.Enabled = false;

            bool finished = data.Required > 0 && data.Found >= data.Required;
            if (finished && !data.Claimed)
            {
                TryClaimSet(data.SetId);
                return;
            }

            if (TryFindFirstEligibleRequirement(data, out int eligibleInfoId, out sbyte eligibleStage))
            {
                TrySubmitItem(data.SetId, eligibleInfoId, eligibleStage);
                return;
            }

            GameScene.Scene?.ChatDialog.ReceiveChat(L("Codex_NothingToSubmit"), ChatType.Hint);
        }

        private static bool HasItemInAnyInventory(int itemInfoId, sbyte stage = -1)
        {
            bool Matches(UserItem it)
            {
                if (it?.Info == null || it.Info.Index != itemInfoId) return false;
                return true;
            }

            var user = GameScene.User;
            if (user?.Inventory != null)
                foreach (var it in user.Inventory)
                    if (Matches(it)) return true;

            var hero = GameScene.Hero;
            if (hero?.Inventory != null)
                foreach (var it in hero.Inventory)
                    if (Matches(it)) return true;

            return false;
        }

        private bool TryFindFirstEligibleRequirement(RowVM vm, out int itemInfoId, out sbyte stage)
        {
            itemInfoId = -1;
            stage = -1;
            if (vm == null) return false;

            while (vm.ReqRegistered.Count < vm.ReqItemIndices.Count)
                vm.ReqRegistered.Add(false);

            for (int i = 0; i < vm.ReqItemIndices.Count; i++)
            {
                if (vm.ReqRegistered[i]) continue;
                int infoId = vm.ReqItemIndices[i];
                sbyte reqStage = (i < vm.ReqStages.Count) ? vm.ReqStages[i] : (sbyte)-1;
                if (HasItemInAnyInventory(infoId, reqStage))
                {
                    itemInfoId = infoId;
                    stage = reqStage;
                    return true;
                }
            }
            return false;
        }

        private void TryClaimSet(int setId)
        {
            var vm = _rows.FirstOrDefault(x => x.SetId == setId);
            if (vm == null) return;

            Network.Enqueue(new C.ClaimItemCodex { Id = setId });
            ApplyLocalClaim(vm);

            GameScene.Scene?.ChatDialog.ReceiveChat(L("Codex_RewardClaimed"), ChatType.Hint);
        }

        private void ApplyLocalClaim(RowVM vm)
        {
            if (vm == null) return;

            if (!ClaimedSetIds.Contains(vm.SetId))
                ClaimedSetIds.Add(vm.SetId);

            vm.Claimed = true;

            ApplySearch(_searchQuery, keepCursor: true);
            RecalcRightCounters();
            RefreshHeaderLevelXp();
            RefreshLevelHint();
            GameScene.User?.RefreshStats();

            PlaySetCompleteEffect($"Collection set completed: {vm.Title}", 2.0f, -60, 120, 2000);

            RebuildStatsList();
            RebuildRewardsList();
            UpdateRightPage();
            RefreshClaimedSetsEmblem();
        }

        private void ApplyLocalRewardIfCompleted(RowVM vm)
        {
            if (vm == null) return;

            bool finished = vm.Required > 0 && vm.Found >= vm.Required;
            if (!finished) return;

            if (!ClaimedSetIds.Contains(vm.SetId))
            {
                ClaimedSetIds.Add(vm.SetId);
                vm.Claimed = true;

                ApplySearch(_searchQuery, keepCursor: true);
                RecalcRightCounters();
                RefreshHeaderLevelXp();
                RefreshLevelHint();
                GameScene.User?.RefreshStats();

                PlaySetCompleteEffect(L("Codex_SetCompletedMessage", vm.Title), 2.0f, -60, 120, 2000);
                PlaySetCompleteEffect(L("Codex_SetCompletedMessage", vm.Title), 2.0f, -60, 120, 2000);

                GameScene.Scene?.ChatDialog.ReceiveChat(L("Codex_CollectionCompleted"), ChatType.Hint);

                Network.Enqueue(new C.ClaimItemCodex { Id = vm.SetId });

                RebuildStatsList();
                RebuildRewardsList();
                UpdateRightPage();
                RefreshClaimedSetsEmblem();
            }
        }

        private void PromptSubmitChoice(RowVM data)
        {
            if (data == null) return;
            var scene = GameScene.Scene;

            bool hasEligibleItem = TryFindFirstEligibleRequirement(data, out int eligibleInfoId, out sbyte eligibleStage);

            bool haveStone = GameScene.Stone > 0;
            bool haveJade = GameScene.Jade > 0;

            bool stoneAllowed = haveStone && IsCurrencyAllowedForRarity(Currency.Stone, data.Rarity);
            bool jadeAllowed = haveJade && IsCurrencyAllowedForRarity(Currency.Jade, data.Rarity);
            bool canUseCurrency = stoneAllowed || jadeAllowed;

            Currency defaultCur = Currency.None;
            if (data.Rarity == ItemGrade.Rare && stoneAllowed) defaultCur = Currency.Stone;
            if (data.Rarity == ItemGrade.Legendary && jadeAllowed) defaultCur = Currency.Jade;
            if (defaultCur == Currency.None) defaultCur = stoneAllowed ? Currency.Stone :
                                                               jadeAllowed ? Currency.Jade : Currency.None;

            string curLabel = defaultCur == Currency.Jade ? L("Codex_JadeLabel") : L("Codex_StoneLabel");
            int curCount = defaultCur == Currency.Jade ? (int)GameScene.Jade : (int)GameScene.Stone;

            string itemName = scene?.GetItemInfo(eligibleInfoId)?.Name ?? "item";
            string stageLabel = eligibleStage >= 0 ? $" (Stage {eligibleStage})" : string.Empty;
            int itemCount = hasEligibleItem ? CountEligibleInBag(eligibleInfoId, eligibleStage) : 0;

            if (canUseCurrency && hasEligibleItem)
            {
                string msg =
                    $"Submit this entry?\n\n" +
                    $"• Use Currency ({curLabel}) — consumes 1 {curLabel}. You have {curCount}.\n" +
                    $"• Use Inventory Item — consumes 1 \"{itemName}{stageLabel}\". You own {itemCount}.\n\n" +
                    $"Yes = Currency ({curLabel})    •    No = Inventory Item";

                var box = new MirMessageBox(msg, MirMessageBoxButtons.YesNo);

                box.YesButton.Click += (o, e) =>
                {
                    var cBox = new MirMessageBox($"Use 1× {curLabel} to submit this entry?\nYou have {curCount}.",
                                                 MirMessageBoxButtons.OKCancel);
                    cBox.OKButton.Click += (o2, e2) => TryUseCurrency(data.SetId, defaultCur);
                    cBox.Show(); cBox.BringToFront();
                };

                box.NoButton.Click += (o, e) => TrySubmitItem(data.SetId, eligibleInfoId, eligibleStage);

                box.Show(); box.BringToFront();
                return;
            }

            if (canUseCurrency && !hasEligibleItem)
            {
                var cBox = new MirMessageBox($"Use 1× {curLabel} to submit this entry?\nYou have {curCount}.",
                                             MirMessageBoxButtons.OKCancel);
                cBox.OKButton.Click += (o, e) => TryUseCurrency(data.SetId, defaultCur);
                cBox.Show(); cBox.BringToFront();
                return;
            }

            if (hasEligibleItem)
            {
                TrySubmitItem(data.SetId, eligibleInfoId, eligibleStage);
                return;
            }

            scene?.ChatDialog.ReceiveChat(L("Codex_NoEligibleItemOrCurrency"), ChatType.Hint);
        }

        private int CountEligibleInBag(int infoId, sbyte stage)
        {
            if (infoId <= 0 || GameScene.User == null) return 0;
            return GameScene.User.Inventory.Count(u =>
                u?.Info != null &&
                u.Info.Index == infoId &&
                stage < 0);
        }

        private void TrySubmitItem(int setId, int itemInfoIndex, sbyte stage)
        {
            var scene = GameScene.Scene;
            var user = GameScene.User;

            if (user == null || user.Inventory == null)
            {
                scene?.ChatDialog.ReceiveChat(L("Codex_InventoryNotAvailable"), ChatType.Hint);
                return;
            }

            bool TryPickFrom(IEnumerable<UserItem> bag, out ulong selected)
            {
                selected = 0;
                if (bag == null) return false;
                foreach (var it in bag)
                {
                    if (it?.Info == null || it.Info.Index != itemInfoIndex) continue;
                    selected = it.UniqueID;
                    return true;
                }
                return false;
            }

            ulong uid = 0;
            if (!TryPickFrom(user.Inventory, out uid))
            {
                if (GameScene.Hero?.Inventory != null)
                    TryPickFrom(GameScene.Hero.Inventory, out uid);
            }

            var info = scene?.GetItemInfo(itemInfoIndex);
            string itemName = info?.Name ?? "item";
            string stageLabel = stage >= 0 ? $" (Stage {stage})" : string.Empty;

            if (uid == 0)
            {
                scene?.ChatDialog.ReceiveChat(L("Codex_MissingItemStage", $"{itemName}{stageLabel}"), ChatType.Hint);
                return;
            }

            var box = new MirMessageBox(
                L("Codex_SubmitConfirm", $"{itemName}{stageLabel}"),
                MirMessageBoxButtons.OKCancel);

            box.OKButton.Click += (o, e) =>
            {
                Network.Enqueue(new C.SubmitItemToCodex
                {
                    SetId = setId,
                    ItemInfoId = itemInfoIndex,
                    Stage = stage,
                    UniqueID = uid
                });

                scene?.ChatDialog.ReceiveChat(L("Codex_SubmittingItem", $"{itemName}{stageLabel}"), ChatType.Hint);

                var vm = _rows.FirstOrDefault(x => x.SetId == setId);
                if (vm != null)
                {
                    int reqIx = FindRequirementSlot(vm, itemInfoIndex, stage);
                    if (reqIx >= 0)
                    {
                        while (vm.ReqRegistered.Count < vm.ReqItemIndices.Count) vm.ReqRegistered.Add(false);
                        if (!vm.ReqRegistered[reqIx]) vm.ReqRegistered[reqIx] = true;
                    }

                    int got = vm.ReqRegistered.Count(b => b);
                    int need = vm.Required > 0 ? vm.Required : vm.ReqItemIndices.Count;
                    vm.Found = (short)Math.Min(need, got);

                    ApplySearch(_searchQuery, keepCursor: true);
                    RecalcRightCounters();
                    RefreshHeaderLevelXp();
                    RefreshLevelHint();

                    ApplyLocalRewardIfCompleted(vm);
                }
            };

            box.Show();
            box.BringToFront();
        }

        private void RecalcRightCounters()
        {
            if (_rightLabels.Count == 0) return;

            (int done, int total) setAll = (0, 0), setCh = (0, 0), setLim = (0, 0), setEvt = (0, 0);

            foreach (var r in _rows)
            {
                if (r == null) continue;

                bool complete = r.Required > 0 && r.Found >= r.Required;

                switch (r.Bucket)
                {
                    case 1: setLim.total++; if (complete) setLim.done++; break;
                    case 2: setEvt.total++; if (complete) setEvt.done++; break;
                    default: setCh.total++; if (complete) setCh.done++; break;
                }
                setAll.total++; if (complete) setAll.done++;
            }

            SetRightProgress(0, "All", setAll.done, setAll.total);
            SetRightProgress(1, "Character", setCh.done, setCh.total);
            SetRightProgress(2, "Limited", setLim.done, setLim.total);
            SetRightProgress(3, "Event", setEvt.done, setEvt.total);

            SetBarFill(0, setAll.done, setAll.total);
            SetBarFill(1, setCh.done, setCh.total);
            SetBarFill(2, setLim.done, setLim.total);
            SetBarFill(3, setEvt.done, setEvt.total);
        }

        private void SetRightProgress(int idx, string name, int done, int total)
        {
            if ((uint)idx >= (uint)_rightLabels.Count) return;
            _rightLabels[idx].Text = $"{name} ({done}/{total})";
        }

        private void SetBarFill(int idx, int found, int need)
        {
            if (idx < 0 || idx >= _rightBars.Count) return;

            _barFound[idx] = Math.Max(0, found);
            _barNeed[idx] = Math.Max(0, need);
        }

        private void ScrollBy(int deltaPx)
        {
            HideItemTooltip();
            SuppressTooltips(200);

            _scrollOffsetPx += deltaPx;
            ClampScroll();
            RebuildList();
            UpdateScrollButtons();
            UpdateScrollThumb();
        }

        private void ClampScroll()
        {
            int maxOff = GetMaxOffsetPx();
            if (_scrollOffsetPx < 0) _scrollOffsetPx = 0;
            if (_scrollOffsetPx > maxOff) _scrollOffsetPx = maxOff;
        }

        private int GetMaxOffsetPx()
        {
            if (_viewRows.Count <= 0) return 0;
            int contentH = (_viewRows.Count * ROW_H) + ((_viewRows.Count - 1) * ROW_GAP) + 12;
            int viewH = _listViewport.Size.Height;
            return Math.Max(0, contentH - viewH);
        }

        private void UpdateScrollButtons()
        {
            int maxOff = GetMaxOffsetPx();
            if (_scrollUp != null) _scrollUp.Enabled = _scrollOffsetPx > 0;
            if (_scrollDown != null) _scrollDown.Enabled = _scrollOffsetPx < maxOff;
        }

        private void UpdateScrollThumb()
        {
            if (_scrollThumb == null || _scrollUp == null || _scrollDown == null) return;

            int minY = _scrollUp.Location.Y + _scrollUp.Size.Height + 2;
            int maxY = _scrollDown.Location.Y - _scrollThumb.Size.Height - 2;

            int maxOffPx = GetMaxOffsetPx();
            if (maxOffPx <= 0)
            {
                _scrollThumb.Location = new Point(_scrollThumb.Location.X, minY);
                return;
            }

            float percent = (float)_scrollOffsetPx / maxOffPx;
            int y = minY + (int)((maxY - minY) * percent);
            _scrollThumb.Location = new Point(_scrollThumb.Location.X, y);
        }

        private void AttachWheel(MirControl c)
        {
            if (c == null) return;
            c.MouseWheel += (o, e) =>
            {
                HideItemTooltip();
                SuppressTooltips(200);

                int ticks = Math.Max(1, Math.Abs(e.Delta) / 120);
                int dir = e.Delta > 0 ? -1 : +1;
                ScrollBy(dir * RowStep * ticks);
            };
        }

        private void ShowItemTooltip(int itemInfoIndex, sbyte stage, Point anchor)
        {
            if (TooltipSuppressed) return;

            var scene = GameScene.Scene;
            var info = scene?.GetItemInfo(itemInfoIndex);
            if (scene == null || info == null) return;

            var ui = new UserItem(info)
            {
                Count = 1,
                CurrentDura = info.Durability,
                MaxDura = info.Durability
            };

            scene.DisposeItemLabel();
            scene.CreateItemLabel(ui);
            if (scene.ItemLabel != null)
            {
                scene.ItemLabel.Visible = false;

                int maxX = Math.Max(0, scene.Size.Width - scene.ItemLabel.Size.Width - 2);
                int maxY = Math.Max(0, scene.Size.Height - scene.ItemLabel.Size.Height - 2);
                int px = Math.Min(maxX, Math.Max(0, anchor.X));
                int py = Math.Min(maxY, Math.Max(0, anchor.Y));

                scene.ItemLabel.Location = new Point(px, py);
                scene.ItemLabel.BringToFront();
                scene.ItemLabel.Visible = true;
            }
        }

        private void HideItemTooltip()
        {
            GameScene.Scene?.DisposeItemLabel();
        }

        private void SelectIndex(int idx)
        {
            if (_viewRows.Count == 0) return;

            _selectedIndex = Math.Max(0, Math.Min(idx, _viewRows.Count - 1));

            int selTop = _selectedIndex * RowStep;
            int selBottom = selTop + ROW_H;

            int viewTop = _scrollOffsetPx;
            int viewBot = _scrollOffsetPx + _listViewport.Size.Height;

            if (selTop < viewTop) _scrollOffsetPx = selTop;
            else if (selBottom > viewBot) _scrollOffsetPx = selBottom - _listViewport.Size.Height;

            ClampScroll();
            RebuildList();
        }

        private void ComputeLevelProgress(out int level, out int xpWithin, out int needWithin)
        {
            int totalXp = 0;
            bool anyXp = false;

            foreach (var r in _rows)
            {
                if (r == null || !r.Claimed) continue;
                if (r.RewardXP > 0) { totalXp += r.RewardXP; anyXp = true; }
                else totalXp += 1;
            }

            if (!anyXp)
                totalXp = _rows.Count(rr => rr != null && rr.Required > 0 && rr.Found >= rr.Required && rr.Claimed);

            var tiers = Enum.GetValues(typeof(CodexLevel))
                            .Cast<CodexLevel>()
                            .Select(v => (int)v)
                            .Where(v => v > 0)
                            .OrderBy(v => v)
                            .ToArray();

            if (tiers.Length == 0)
            {
                level = 1;
                xpWithin = Math.Max(0, totalXp);
                needWithin = 1;
                UpdateLevelHint(level);
                return;
            }

            level = 1;
            int remaining = Math.Max(0, totalXp);
            int idx = 0;

            while (idx < tiers.Length && remaining >= tiers[idx])
            {
                remaining -= tiers[idx];
                level++;
                idx++;
            }

            int need = tiers[Math.Min(idx, tiers.Length - 1)];
            xpWithin = remaining;
            needWithin = need;

            UpdateLevelHint(level);
        }

        private void RefreshHeaderLevelXp()
        {
            ComputeLevelProgress(out int level, out int xpWithin, out int needWithin);

            int previous = _lastLevelSeen;
            _lastLevelSeen = Math.Max(1, level);

            if (previous > 0 && _lastLevelSeen > previous)
            {
                _pendingLevelUpFx = true;

                if (!_setFxPlaying)
                {
                    var delay = new System.Windows.Forms.Timer { Interval = 350 };
                    delay.Tick += (s, e) =>
                    {
                        delay.Stop(); delay.Dispose();
                        if (_pendingLevelUpFx && !_setFxPlaying)
                        {
                            _pendingLevelUpFx = false;
                            PlayLevelUpEffect(L("Codex_LevelUp", _lastLevelSeen));
                        }
                    };
                    delay.Start();
                }
            }

            if (_levelLabel != null)
                _levelLabel.Text = L("Codex_LevelLabel", Math.Max(1, level));

            if (_xpText != null)
            {
                _xpText.Text = $"{Math.Max(0, xpWithin)} / {Math.Max(1, needWithin)}";
                int barW = GetXpBarWidth();
                Size sz = TextRenderer.MeasureText(_xpText.Text, _xpText.Font);
                int cx = XP_BAR_X + (barW - sz.Width) / 2;
                _xpText.Location = new Point(cx, XP_BAR_Y + 1);
            }
        }

        private void UpdateLevelHint(int level)
        {
            if (_levelHintHotspot == null) return;

            var lines = BuildCollectionLevelHint(level);

            _levelHintHotspot.Hint =
                (lines == null || lines.Length == 0)
                ? L("Codex_LevelHint", Math.Max(1, level))
                : L("Codex_LevelHint", Math.Max(1, level)) + "\n" + string.Join("\n", lines);

            _levelHintHotspot.Visible = true;
            _levelHintHotspot.Location = new Point(LEVEL_HINT_RECT.X, LEVEL_HINT_RECT.Y);
            _levelHintHotspot.Size = new Size(LEVEL_HINT_RECT.Width, LEVEL_HINT_RECT.Height);
        }

        private void SwitchRightPage(int delta)
        {
            int x = ((int)_rightPage + delta) % 2;
            if (x < 0) x += 2;
            _rightPage = (RightPage)x;
            UpdateRightPage();
        }

        private void UpdateRightPage()
        {
            if (_rightHeader != null)
            {
                _rightHeader.Text = _rightPage switch
                {
                    RightPage.Progress => "Progress",
                    RightPage.Rewards => "Rewards",
                    _ => "Progress"
                };
                _rightHeader.Location = RightUI.Header;
            }

            bool showProgress = _rightPage == RightPage.Progress;
            for (int i = 0; i < _rightIcons.Count; i++) if (_rightIcons[i] != null) _rightIcons[i].Visible = showProgress;
            for (int i = 0; i < _rightBars.Count; i++) if (_rightBars[i] != null) _rightBars[i].Visible = showProgress && (_barNeed[i] > 0);
            for (int i = 0; i < _rightLabels.Count; i++) if (_rightLabels[i] != null) _rightLabels[i].Visible = showProgress;

            bool showRewards = _rightPage == RightPage.Rewards;
            UpdateRewardsVisible(showRewards);
            if (showRewards) RebuildRewardsList();

        }

        private Stats GetTotalClaimedStats(out int totalXp)
        {
            totalXp = 0;
            var total = new Stats();

            foreach (var vm in _rows)
            {
                if (vm == null || !vm.Claimed) continue;

                totalXp += (vm.RewardXP > 0) ? vm.RewardXP : 1;

                var s = vm.Reward;
                if (s?.Values == null) continue;

                foreach (var kv in s.Values)
                    total[kv.Key] = total[kv.Key] + kv.Value;
            }
            return total;
        }

        private void RebuildStatsList()
        {
            if (_statsViewport == null) return;

            foreach (var c in _statsViewport.Controls.ToArray())
                c.Dispose();
            _statsLines.Clear();
            _statsScrollOffsetPx = 0;

            int totalXp;
            Stats total = GetTotalClaimedStats(out totalXp);

            int y = 4;
            var prettyStats = FriendlyStatLines(BuildStatLines(total));
            foreach (var line in prettyStats)
            {
                var lbl = new MirLabel
                {
                    Parent = _statsViewport,
                    AutoSize = true,
                    ForeColour = Color.White,
                    Font = new Font(Settings.FontName, 9f),
                    Location = new Point(6, y),
                    Text = line
                };
                _statsLines.Add(lbl);
                y += StatsLineH;
            }

            UpdateStatsScrollUI();
        }

        private void ScrollStatsBy(int deltaPx)
        {
            int inner = Math.Max(0, _statsLines.Count * StatsLineH + 8 - _statsViewport.Size.Height);
            _statsScrollOffsetPx = Math.Max(0, Math.Min(inner, _statsScrollOffsetPx + deltaPx));

            int yBase = 4 - _statsScrollOffsetPx;
            for (int i = 0; i < _statsLines.Count; i++)
                _statsLines[i].Location = new Point(6, yBase + i * StatsLineH);

            UpdateStatsScrollUI();
        }

        private void UpdateStatsScrollUI()
        {
            if (_statsViewport == null) return;

            int inner = Math.Max(0, (_statsLines.Count * StatsLineH) + 8 - _statsViewport.Size.Height);

            bool canScroll = inner > 0;
            if (_statsScrollUp != null) _statsScrollUp.Enabled = canScroll;
            if (_statsScrollDown != null) _statsScrollDown.Enabled = canScroll;

            if (_statsScrollThumb != null) _statsScrollThumb.Visible = false;
        }

        private static int RewardLineOrderKey(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return int.MaxValue;

            if (s.StartsWith("AC ", StringComparison.OrdinalIgnoreCase)) return 0;
            if (s.StartsWith("MAC ", StringComparison.OrdinalIgnoreCase)) return 1;
            if (s.StartsWith("DC ", StringComparison.OrdinalIgnoreCase)) return 2;
            if (s.StartsWith("MC ", StringComparison.OrdinalIgnoreCase)) return 3;
            if (s.StartsWith("SC ", StringComparison.OrdinalIgnoreCase)) return 4;

            if (s.StartsWith("Accuracy", StringComparison.OrdinalIgnoreCase)) return 5;
            if (s.StartsWith("Agility", StringComparison.OrdinalIgnoreCase)) return 6;

            return 1000;
        }

        private void RebuildRewardsList()
        {
            if (_rewardsViewport == null) return;

            foreach (var c in _rewardsViewport.Controls.ToArray()) c.Dispose();
            _rewardsLines.Clear();
            _rewardsData.Clear();
            _rewardsScrollOffsetPx = 0;
            _firstVisibleLine = 0;

            int _;
            var setTotals = GetTotalClaimedStats(out _);

            var levelTotals = GetCollectionLevelBonusStats();

            var merged = new Stats();
            void AddInto(Stats s)
            {
                if (s?.Values == null) return;
                foreach (var kv in s.Values)
                    merged[kv.Key] = merged[kv.Key] + kv.Value;
            }
            AddInto(setTotals);
            AddInto(levelTotals);

            var pretty = FriendlyStatLines(BuildStatLines(merged))
                .OrderBy(RewardLineOrderKey)
                .ThenBy(s => s, StringComparer.OrdinalIgnoreCase);

            foreach (var line in pretty)
                _rewardsData.Add(line);

            EnsureRewardsPool();
            RepopulateRewardsSlice();
            ScrollRewardsBy(0);
        }

        private void RepopulateRewardsSlice()
        {
            if (_rewardsViewport == null || _rewardsLines.Count == 0) return;

            for (int j = 0; j < _rewardsLines.Count; j++)
            {
                int i = _firstVisibleLine + j;
                var lbl = _rewardsLines[j];

                if (i < _rewardsData.Count)
                {
                    lbl.Text = _rewardsData[i];
                    lbl.Visible = true;
                }
                else
                {
                    lbl.Text = string.Empty;
                    lbl.Visible = false;
                }
            }
        }

        private void EnsureRewardsPool()
        {
            if (_rewardsViewport == null) return;
            if (_rewardsLines.Count > 0) return;

            for (int i = 0; i < VisibleLineCount; i++)
            {
                var lbl = new MirLabel
                {
                    Parent = _rewardsViewport,
                    AutoSize = true,
                    ForeColour = Color.White,
                    Font = new Font(Settings.FontName, 9f),
                    Location = new Point(6, 4 + i * StatsLineH),
                    Visible = false
                };
                _rewardsLines.Add(lbl);
            }
        }

        private void ScrollRewardsBy(int deltaPx)
        {
            if (_rewardsViewport == null) return;

            int contentH = (_rewardsData.Count * StatsLineH) + 8;
            int viewH = _rewardsViewport.Size.Height;
            int maxOffset = Math.Max(0, contentH - viewH);

            _rewardsScrollOffsetPx = Math.Max(0, Math.Min(maxOffset, _rewardsScrollOffsetPx + deltaPx));

            int newFirst = _rewardsScrollOffsetPx / StatsLineH;
            if (newFirst != _firstVisibleLine)
            {
                _firstVisibleLine = newFirst;
                RepopulateRewardsSlice();
            }

            int remainder = _rewardsScrollOffsetPx % StatsLineH;
            for (int j = 0; j < _rewardsLines.Count; j++)
            {
                var lbl = _rewardsLines[j];
                int y = 4 + (j * StatsLineH) - remainder;
                lbl.Location = new Point(6, y);

                int h = Math.Max(StatsLineH, lbl.Size.Height);
                bool inside = lbl.Visible && (y + h) > 0 && y < viewH;
                lbl.Visible = inside;
            }

            UpdateRewardsScrollUI();
        }

        private void UpdateRewardsScrollUI()
        {
            if (_rewardsViewport == null) return;

            int inner = Math.Max(0, (_rewardsData.Count * StatsLineH) + 8 - _rewardsViewport.Size.Height);

            bool canScroll = inner > 0;
            if (_rewardsScrollUp != null) _rewardsScrollUp.Enabled = canScroll;
            if (_rewardsScrollDown != null) _rewardsScrollDown.Enabled = canScroll;

            if (_rewardsScrollThumb != null) _rewardsScrollThumb.Visible = false;
        }

        private void UpdateRewardsVisible(bool showRewards)
        {
            if (_rewardsViewport != null) _rewardsViewport.Visible = showRewards;
            if (_rewardsScrollUp != null) _rewardsScrollUp.Visible = showRewards;
            if (_rewardsScrollDown != null) _rewardsScrollDown.Visible = showRewards;

            if (_rewardsScrollThumb != null) _rewardsScrollThumb.Visible = false;

            if (_rewardsMaskTop != null) _rewardsMaskTop.Visible = showRewards;
            if (_rewardsMaskBottom != null) _rewardsMaskBottom.Visible = showRewards;

            if (!showRewards) return;

            _rewardsViewport.BringToFront();
            _rewardsMaskTop.BringToFront();
            _rewardsMaskBottom.BringToFront();
            _rewardsScrollUp.BringToFront();
            _rewardsScrollDown.BringToFront();

            ScrollRewardsBy(0);
        }

        private void AdjustRewardsMasksToViewport()
        {
            if (_rewardsViewport == null) return;

            if (_rewardsMaskTop != null)
            {
                _rewardsMaskTop.Location = new Point(_rewardsViewport.Location.X, _rewardsViewport.Location.Y - 2000);
                _rewardsMaskTop.Size = new Size(_rewardsViewport.Size.Width, 2000);
            }
            if (_rewardsMaskBottom != null)
            {
                _rewardsMaskBottom.Location = new Point(_rewardsViewport.Location.X, _rewardsViewport.Location.Y + _rewardsViewport.Size.Height);
                _rewardsMaskBottom.Size = new Size(_rewardsViewport.Size.Width, 2000);
            }
        }

        private static readonly Dictionary<string, string> StatShortNames =
            new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            {
                ["Accuracy"] = "Accuracy",
                ["Agility"] = "Agility",

                // Attack speed
                ["AttackSpeed"] = "A.Speed",
                ["AttackSpeedRatePercent"] = "A.Speed %",

                // Weights
                ["BagWeight"] = "B - Weight",
                ["HandWeight"] = "H - Weight",
                ["WearWeight"] = "W - Weight",

                // Poison / resists / recovery
                ["PoisonAttack"] = "PSN Attack",
                ["PoisonResist"] = "PSN Res",
                ["PoisonRecovery"] = "PSN Regen",

                // Recovery
                ["HealthRecovery"] = "HP Regen",
                ["SpellRecovery"] = "MP Regen",

                // Crits
                ["CriticalRate"] = "Crit %",
                ["CriticalDamage"] = "Crit DMG",
                ["MinDamage"] = "Damage Min",
                ["MaxDamage"] = "Damage Max",

                // Other common resists / misc (optional shorthands)
                ["MagicResist"] = "Magic Res",
                ["Freezing"] = "Freeze",
                ["Reflect"] = "Reflect",
                ["Strong"] = "Strong",
                ["Holy"] = "Holy",

                // Resource %
                ["HPRatePercent"] = "HP %",
                ["MPRatePercent"] = "MP %",

                // Leech / reductions / shields
                ["HPDrainRatePercent"] = "Life Steal %",
                ["DamageReductionPercent"] = "Damage Red. %",
                ["EnergyShieldPercent"] = "Energy Shield %",
                ["EnergyShieldHPGain"] = "Shield HP +",

                // Economy / meta progression
                ["ExpRatePercent"] = "EXP %",
                ["ItemDropRatePercent"] = "Drop %",
                ["GoldDropRatePercent"] = "Gold %",
                ["MineRatePercent"] = "Mine %",
                ["GemRatePercent"] = "Gem %",
                ["FishRatePercent"] = "Fish %",
                ["CraftRatePercent"] = "Craft %",
                ["SkillGainMultiplier"] = "Skill Gain ×",

                // Attack flat bonus
                ["Strength"] = "Strength",
                ["Intelligence"] = "Intelligence",
                ["AttackBonus"] = "Power",

                // Social
                ["LoverExpRatePercent"] = "Couple EXP %",
                ["MentorDamageRatePercent"] = "Mentor Dmg %",
                ["MentorExpRatePercent"] = "Mentor EXP %",

                // Penalties
                ["ManaPenaltyPercent"] = "Mana Pen %",
                ["TeleportManaPenaltyPercent"] = "TP Mana Pen %",
            };

        private static string ShortenStatLine(string line)
        {
            if (string.IsNullOrWhiteSpace(line)) return line;

            int colon = line.IndexOf(':');
            string name, rest;

            if (colon >= 0)
            {
                name = line.Substring(0, colon).Trim();
                rest = line.Substring(colon);
            }
            else
            {
                int cut = -1;
                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];
                    if (c == '+' || c == '-' || char.IsDigit(c))
                    {
                        cut = (i > 0 && line[i - 1] == ' ') ? i - 1 : i;
                        break;
                    }
                }
                if (cut > 0) { name = line.Substring(0, cut).Trim(); rest = line.Substring(cut); }
                else { name = line.Trim(); rest = string.Empty; }
            }

            if (StatShortNames.TryGetValue(name, out var shortName))
                name = shortName;

            return string.IsNullOrEmpty(rest) ? name : name + rest;
        }

        private static IEnumerable<string> CoalesceMinMax(IEnumerable<string> raw)
        {
            var rx = new Regex(
                @"^(?<kind>Min|Max)(?<grp>AC|MAC|DC|MC|SC)\s*:?\s*(?<sign>[+\-]?)\s*(?<num>\d+)",
                RegexOptions.Compiled);

            var order = new List<string>();
            var pairs = new Dictionary<string, (int? min, int? max)>(StringComparer.OrdinalIgnoreCase);
            var passthrough = new List<string>();

            foreach (var line in raw)
            {
                var m = rx.Match(line);
                if (!m.Success) { passthrough.Add(line); continue; }

                string grp = m.Groups["grp"].Value;
                bool isMin = m.Groups["kind"].Value.Equals("Min", StringComparison.OrdinalIgnoreCase);
                int val = int.Parse((m.Groups["sign"].Value == "-" ? "-" : "") + m.Groups["num"].Value);

                if (!pairs.ContainsKey(grp)) { pairs[grp] = (null, null); order.Add(grp); }
                var cur = pairs[grp];
                if (isMin) cur.min = val; else cur.max = val;
                pairs[grp] = cur;
            }

            foreach (var s in passthrough)
                yield return s;

            foreach (var grp in order)
            {
                var (min, max) = pairs[grp];
                int left = min ?? 0;
                int right = max ?? 0;
                yield return $"{grp} {left}~{right}";
            }
        }

        private static IEnumerable<string> FriendlyStatLines(IEnumerable<string> raw)
            => CoalesceMinMax(raw).Select(ShortenStatLine);

        private static List<string> LimitLinesWithHint(IEnumerable<string> lines, int max, out string hint)
        {
            var all = lines.ToList();
            if (all.Count <= max) { hint = null; return all; }

            hint = string.Join("\n", all);
            var preview = all.Take(max).ToList();
            preview.Add($"… +{all.Count - max} more");
            return preview;
        }

        private static List<string> BuildRowPreviewLines(IEnumerable<string> raw, int max, out string hint)
        {
            var pretty = FriendlyStatLines(raw);
            return LimitLinesWithHint(pretty, max, out hint);
        }

        private const int MaxLevel = 20;

        private static readonly int[] XPPerLevel =
        {
            0, 0, 8, 11, 14, 17, 20, 23, 26, 29, 32, 35, 38, 41, 44, 47, 51, 55, 59, 63, 67
        };

        // Crit %, Crit DMG %, HP Regen %, MP Regen %, DC max, MC max
        private static readonly (float crit, float critDmg, int hpRec, int mpRec, int dcMax, int mcMax)[] LV1 =
        {
            // 0-index dummy:
            (0,0,0,0,0,0),
            (0,0,0,0,0,0),   // 1
            (0,0,10,10,1,1), // 2
            (0,0,15,15,1,1), // 3
            (0,0,15,15,1,1), // 4
            (0,0,20,20,1,1), // 5
            (0,0,25,25,1,1), // 6
            (0,0,25,25,1,1), // 7
            (0,0,30,30,1,1), // 8
            (0,0,30,30,1,1), // 9
            (0,0,30,30,2,2), // 10
            (1,5,30,30,2,2), // 11
            (1,5.5f,30,30,2,2), // 12
            (1,6,30,30,2,2), // 13
            (1,6.5f,30,30,2,2), // 14
            (1,6.5f,30,30,3,3), // 15
            (1.5f,7,30,30,3,3), // 16
            (2,7.5f,30,30,3,3), // 17
            (2,8,30,30,3,3), // 18
            (2,9,30,30,4,4), // 19
            (2,10,30,30,5,5), // 20
        };

        // SC max, AC max, MAC max, A.Speed(?) max, Accuracy, Agility
        private static readonly (int scMax, int acMax, int macMax, int speedMax, int precision, int quickness)[] LV2 =
        {
            (0,0,0,0,0,0), // 0
            (0,0,0,0,0,0), // 1
            (1,0,0,0,1,0), // 2
            (1,0,0,0,1,1), // 3
            (1,1,1,0,1,1), // 4
            (1,2,2,0,1,1), // 5
            (1,2,2,0,1,1), // 6
            (1,2,2,1,1,1), // 7
            (1,2,2,1,1,1), // 8
            (1,2,2,2,2,1), // 9
            (2,2,2,2,2,1), // 10
            (2,2,2,2,2,2), // 11
            (2,2,2,2,2,2), // 12
            (2,2,2,2,2,2), // 13
            (2,2,2,3,2,2), // 14
            (3,2,2,3,3,2), // 15
            (3,2,2,4,3,2), // 16
            (3,3,3,4,3,3), // 17
            (3,4,4,4,3,3), // 18
            (4,4,4,4,3,3), // 19
            (5,5,5,4,3,3), // 20
        };

        // Addiction Resist, Magic Resist, HandWeight, WearWeight, BagWeight
        private static readonly (int addRes, int magRes, int handW, int wearW, int bagW)[] LV3 =
        {
            (0,0,0,0,0),  // 0
            (0,0,0,0,0),  // 1
            (0,0,0,0,50), // 2
            (0,0,0,0,60), // 3
            (0,0,20,0,70), // 4
            (0,0,20,20,80), // 5
            (0,0,20,20,90),
            (0,0,20,20,100),
            (0,0,20,20,110),
            (0,0,20,20,120),
            (0,0,20,20,130),
            (0,0,20,20,140),
            (0,0,20,20,150),
            (0,0,20,20,160),
            (0,0,20,20,170),
            (0,0,20,20,180),
            (0,0,20,20,200),
            (0,0,20,20,200),
            (0,0,20,20,200),
            (1,0,20,20,200),
            (1,1,20,20,200),
        };

        private static string[] BuildCollectionLevelHint(int level)
        {
            level = Math.Max(1, Math.Min(MaxLevel, level));

            var stats = BuildLevelStats(level);

            var pretty = FriendlyStatLines(BuildStatLines(stats))
                .OrderBy(LevelHintOrderKey)
                .ThenBy(s => s, StringComparer.OrdinalIgnoreCase)
                .ToArray();

            return pretty;
        }

        private static Stats BuildLevelStats(int level)
        {
            if (level < 1) level = 1;
            if (level > 20) level = 20;

            // Arrays 1..20 (index 0 unused)
            int[] critPct = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2 };
            int[] critDmg = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 5, 6, 6, 7, 7, 7, 8, 8, 9, 10 };
            int[] hpRec = { 0, 0, 10, 15, 15, 20, 25, 25, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 };
            int[] mpRec = { 0, 0, 10, 15, 15, 20, 25, 25, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30, 30 };
            int[] dcMax = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] mcMax = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] scMax = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] acMax = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] macMax = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] precision = { 0, 0, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 3 };
            int[] quickness = { 0, 0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 2, 3, 3, 3, 3 };

            int[] minAC = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] maxAC = { 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 4, 4, 5 };
            int[] minMAC = { 0, 0, 0, 0, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 4, 4, 5 };
            int[] maxMAC = { 0, 0, 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 3, 4, 4, 5 };
            int[] minDC = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] maxDC = { 0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 5 };
            int[] minMC = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] maxMC = { 0, 0, 1, 1, 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 4, 5 };
            int[] minSC = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] maxSC = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1, 1, 1, 1, 3, 3, 3, 3, 4, 5 };

            int[] addRes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 1 };
            int[] magRes = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 1 };
            int[] handW = { 0, 0, 0, 0, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20, 20 };
            int[] wearW = { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
            int[] bagW = { 0, 0, 50, 60, 70, 80, 90, 100, 110, 120, 130, 140, 150, 160, 170, 180, 200, 200, 200, 200 };

            var s = new Stats();
            if (critPct[level] != 0) s[Stat.CriticalRate] = s[Stat.CriticalRate] + critPct[level];
            if (critDmg[level] != 0) s[Stat.CriticalDamage] = s[Stat.CriticalDamage] + critDmg[level];
            if (hpRec[level] != 0) s[Stat.HealthRecovery] = s[Stat.HealthRecovery] + hpRec[level];
            if (mpRec[level] != 0) s[Stat.SpellRecovery] = s[Stat.SpellRecovery] + mpRec[level];

            if (minAC[level] != 0) s[Stat.MinAC] = s[Stat.MinAC] + minAC[level];
            if (maxAC[level] != 0) s[Stat.MaxAC] = s[Stat.MaxAC] + maxAC[level];
            if (minMAC[level] != 0) s[Stat.MinMAC] = s[Stat.MinMAC] + minMAC[level];
            if (maxMAC[level] != 0) s[Stat.MaxMAC] = s[Stat.MaxMAC] + maxMAC[level];
            if (minDC[level] != 0) s[Stat.MinDC] = s[Stat.MinDC] + minDC[level];
            if (maxDC[level] != 0) s[Stat.MaxDC] = s[Stat.MaxDC] + maxDC[level];
            if (minMC[level] != 0) s[Stat.MinMC] = s[Stat.MinMC] + minMC[level];
            if (maxMC[level] != 0) s[Stat.MaxMC] = s[Stat.MaxMC] + maxMC[level];
            if (minSC[level] != 0) s[Stat.MinSC] = s[Stat.MinSC] + minSC[level];
            if (maxSC[level] != 0) s[Stat.MaxSC] = s[Stat.MaxSC] + maxSC[level];

            if (precision[level] != 0) s[Stat.Accuracy] = s[Stat.Accuracy] + precision[level];
            if (quickness[level] != 0) s[Stat.Agility] = s[Stat.Agility] + quickness[level];

            if (addRes[level] != 0) s[Stat.PoisonResist] = s[Stat.PoisonResist] + addRes[level];
            if (magRes[level] != 0) s[Stat.MagicResist] = s[Stat.MagicResist] + magRes[level];

            if (handW[level] != 0) s[Stat.HandWeight] = s[Stat.HandWeight] + handW[level];
            if (wearW[level] != 0) s[Stat.WearWeight] = s[Stat.WearWeight] + wearW[level];
            if (bagW[level] != 0) s[Stat.BagWeight] = s[Stat.BagWeight] + bagW[level];

            return s;
        }

        private static int LevelHintOrderKey(string s)
        {
            if (string.IsNullOrWhiteSpace(s)) return int.MaxValue;
            s = s.Trim();

            // Range groups first (same as Rewards order)
            if (s.StartsWith("AC ", StringComparison.OrdinalIgnoreCase)) return 0;
            if (s.StartsWith("MAC ", StringComparison.OrdinalIgnoreCase)) return 1;
            if (s.StartsWith("DC ", StringComparison.OrdinalIgnoreCase)) return 2;
            if (s.StartsWith("MC ", StringComparison.OrdinalIgnoreCase)) return 3;
            if (s.StartsWith("SC ", StringComparison.OrdinalIgnoreCase)) return 4;

            // Primaries
            if (s.StartsWith("Accuracy", StringComparison.OrdinalIgnoreCase)) return 5;
            if (s.StartsWith("Agility", StringComparison.OrdinalIgnoreCase)) return 6;

            // Tempo / % bonuses
            if (s.StartsWith("A.Speed", StringComparison.OrdinalIgnoreCase) ||
                s.StartsWith("Attack Speed", StringComparison.OrdinalIgnoreCase)) return 7;
            if (s.StartsWith("Crit %", StringComparison.OrdinalIgnoreCase)) return 8;
            if (s.StartsWith("Crit DMG", StringComparison.OrdinalIgnoreCase)) return 9;
            if (s.StartsWith("HP Regen", StringComparison.OrdinalIgnoreCase)) return 10;
            if (s.StartsWith("MP Regen", StringComparison.OrdinalIgnoreCase)) return 11;

            // Resists
            if (s.StartsWith("Magic Res", StringComparison.OrdinalIgnoreCase)) return 12;
            if (s.StartsWith("Addiction Res", StringComparison.OrdinalIgnoreCase) ||
                s.StartsWith("PSN Res", StringComparison.OrdinalIgnoreCase)) return 13;

            // Weights
            if (s.StartsWith("H - Weight", StringComparison.OrdinalIgnoreCase)) return 20;
            if (s.StartsWith("W - Weight", StringComparison.OrdinalIgnoreCase)) return 21;
            if (s.StartsWith("B - Weight", StringComparison.OrdinalIgnoreCase)) return 22;

            return 1000;
        }

        private static Stats SumStats(Stats a, Stats b)
        {
            var r = new Stats();
            if (a != null)
                foreach (var kv in a.Values?.ToArray() ?? Array.Empty<KeyValuePair<Stat, int>>())
                    if (kv.Value != 0) { try { r[kv.Key] = r[kv.Key] + kv.Value; } catch { try { r[kv.Key] = kv.Value; } catch { } } }
            if (b != null)
                foreach (var kv in b.Values?.ToArray() ?? Array.Empty<KeyValuePair<Stat, int>>())
                    if (kv.Value != 0) { try { r[kv.Key] = r[kv.Key] + kv.Value; } catch { try { r[kv.Key] = kv.Value; } catch { } } }
            return r;
        }

        public static Stats GetCollectionLevelBonusStats()
        {
            var dlg = Instance;
            if (dlg == null) return new Stats();
            dlg.ComputeLevelProgress(out int level, out _, out _);
            return BuildLevelStats(level);
        }

        public static int GetCollectionLevel()
        {
            var dlg = Instance;
            if (dlg == null) return 1;
            dlg.ComputeLevelProgress(out int level, out _, out _);
            return Math.Max(1, level);
        }

        private void RefreshLevelHint()
        {
            if (_levelHintHotspot == null) return;
            ComputeLevelProgress(out int level, out _, out _);
            UpdateLevelHint(level);
        }

        private void RefreshClaimedSetsEmblem()
        {
            if (_claimedSetsEmblem == null) return;

            int _xp;
            var totals = GetTotalClaimedStats(out _xp);

            var lines = FriendlyStatLines(BuildStatLines(totals))
                        .OrderBy(RewardLineOrderKey)
                        .ThenBy(s => s, StringComparer.OrdinalIgnoreCase)
                .Where(l => !string.IsNullOrWhiteSpace(l))
                .ToList();

            _claimedSetsEmblem.Hint = (lines.Count == 0)
                ? L("Codex_ClaimedSetBonuses")
                : L("Codex_ClaimedSetBonuses") + "\n" + string.Join("\n", lines);
        }

        private void PlaySetCompleteEffect(string message = null,
                                   float scale = 2.0f,
                                   int yOffset = -60,
                                   int frameMs = 120,
                                   int lingerMs = 2000)
        {
            const int START = 890, END = 904;

            if (Environment.TickCount - _lastSetCompleteFxAt < 600) return;
            _lastSetCompleteFxAt = Environment.TickCount;

            var scene = GameScene.Scene;
            var lib = Libraries.Effect_32bit;
            if (scene == null || lib == null) return;
            message ??= L("Codex_SetCompleteEffect");

            _setFxPlaying = true;

            var host = new MirControl
            {
                Parent = scene,
                NotControl = true,
                Visible = true,
                Size = Size.Empty
            };
            host.BringToFront();

            MirLabel msgShadow = null, msg = null;
            if (!string.IsNullOrWhiteSpace(message))
            {
                var fnt = new Font(Settings.FontName, 13.0f, FontStyle.Bold);
                msgShadow = new MirLabel { Parent = host, AutoSize = true, ForeColour = Color.FromArgb(32, 32, 32), Font = fnt, Text = message };
                msg = new MirLabel { Parent = host, AutoSize = true, ForeColour = Color.WhiteSmoke, Font = fnt, Text = message };
            }

            int frame = START;
            int linger = 0;

            host.BeforeDraw += (s, e) =>
            {
                var center = new Point(scene.Size.Width / 2, scene.Size.Height / 2 + yOffset);
                DrawEffectFrameScaled(lib, frame, center, scale);

                if (msg != null)
                {
                    int mx = center.X - (msg.Size.Width / 2);
                    int my = center.Y + 36;
                    if (msgShadow != null) msgShadow.Location = new Point(mx + 1, my + 1);
                    msg.Location = new Point(mx, my);
                }
            };

            var timer = new System.Windows.Forms.Timer { Interval = frameMs };
            timer.Tick += (s, e) =>
            {
                if (frame < END) { frame++; scene.Redraw(); return; }
                if (linger < lingerMs) { linger += timer.Interval; return; }

                timer.Stop(); timer.Dispose();
                try { host.Dispose(); } catch { }

                _setFxPlaying = false;

                if (_pendingLevelUpFx)
                {
                    _pendingLevelUpFx = false;
                    var after = new System.Windows.Forms.Timer { Interval = 350 };
                    after.Tick += (s2, e2) =>
                    {
                        after.Stop(); after.Dispose();
                        if (!_setFxPlaying)
                            PlayLevelUpEffect($"Collection Level Up! Lv.{_lastLevelSeen}");
                    };
                    after.Start();
                }

                scene.Redraw();
            };

            timer.Start();

            if (!string.IsNullOrWhiteSpace(message))
                GameScene.Scene?.ChatDialog.ReceiveChat(message, ChatType.Hint);
        }

        private static void DrawEffectFrameScaled(MLibrary lib, int index, Point center, float scale)
        {
            try
            {
                var mi = lib.GetType().GetMethod("Draw",
                    new[] { typeof(int), typeof(Point), typeof(Color), typeof(bool), typeof(float) });
                if (mi != null)
                {
                    mi.Invoke(lib, new object[] { index, center, Color.White, true, scale });
                    return;
                }
            }
            catch { /* fall through to non-scaled draw */ }

            lib.Draw(index, center, Color.White, true);
        }

        private void PlayLevelUpEffect(string message = null,
                               float scale = 2.0f,
                               int yOffset = -60,
                               int frameMs = 120,
                               int lingerMs = 2000)
        {
            message ??= L("Codex_LevelUpShort");
            var scene = GameScene.Scene;
            var lib = Libraries.Effect_32bit;
            if (scene == null || lib == null) return;

            _levelFxHost?.Dispose();
            _levelFxHost = new MirControl
            {
                Parent = scene,
                NotControl = true,
                Visible = true,
                Size = Size.Empty
            };
            _levelFxHost.BringToFront();

            const int START = 920, END = 934;
            int frame = START;
            int linger = 0;

            MirLabel msgShadow = null, msg = null;
            if (!string.IsNullOrWhiteSpace(message))
            {
                var fnt = new Font(Settings.FontName, 13.0f, FontStyle.Bold);
                msgShadow = new MirLabel { Parent = _levelFxHost, AutoSize = true, ForeColour = Color.FromArgb(32, 32, 32), Font = fnt, Text = message };
                msg = new MirLabel { Parent = _levelFxHost, AutoSize = true, ForeColour = Color.WhiteSmoke, Font = fnt, Text = message };
            }

            _levelFxHost.BeforeDraw += (s, e) =>
            {
                var center = new Point(scene.Size.Width / 2, scene.Size.Height / 2 + yOffset);
                DrawEffectFrameScaled(lib, frame, center, scale);

                if (msg != null)
                {
                    int mx = center.X - (msg.Size.Width / 2);
                    int my = center.Y + 36;
                    if (msgShadow != null) msgShadow.Location = new Point(mx + 1, my + 1);
                    msg.Location = new Point(mx, my);
                }
            };

            var timer = new System.Windows.Forms.Timer { Interval = frameMs };
            timer.Tick += (s, e) =>
            {
                if (frame < END)
                {
                    frame++;
                    scene.Redraw();
                    return;
                }

                if (linger < lingerMs)
                {
                    linger += timer.Interval;
                    return;
                }

                timer.Stop();
                timer.Dispose();
                try { _levelFxHost.Dispose(); } catch { }
                _levelFxHost = null;

                scene.Redraw();
            };
            timer.Start();

            if (!string.IsNullOrWhiteSpace(message))
                GameScene.Scene?.ChatDialog.ReceiveChat(message, ChatType.Hint);
        }

        private void BuildCurrencyUI()
        {
            // --- Stone (Ongseok) ---
            _stoneIcon = new MirImageControl
            {
                Parent = this,
                Library = Libraries.UI_32bit,
                Index = ICON_STONE_IDX,
                Location = CURRENCY_STONE_POS,
                Size = new Size(18, 18),
                Hint = L("Codex_CurrencyStoneHint", STONE_SUBS_FOR),
            };

            _stoneCountLbl = new MirLabel
            {
                Parent = this,
                Location = new Point(CURRENCY_STONE_POS.X + 30, CURRENCY_STONE_POS.Y + 8),
                AutoSize = true,
                ForeColour = Color.White,
                Text = "x0",
                Font = new Font(Settings.FontName, 10f, FontStyle.Bold),
            };

            // --- Jade ---
            _jadeIcon = new MirImageControl
            {
                Parent = this,
                Library = Libraries.UI_32bit,
                Index = ICON_JADE_IDX,
                Location = CURRENCY_JADE_POS,
                Size = new Size(18, 18),
                Hint = L("Codex_CurrencyJadeHint", JADE_SUBS_FOR),
            };

            _jadeCountLbl = new MirLabel
            {
                Parent = this,
                Location = new Point(CURRENCY_JADE_POS.X + 25, CURRENCY_JADE_POS.Y + 8),
                AutoSize = true,
                ForeColour = Color.White,
                Text = "x0",
                Font = new Font(Settings.FontName, 10f, FontStyle.Bold),
            };

            UpdateCurrencyUI();
        }

        private void UpdateCurrencyUI()
        {
            if (_stoneCountLbl != null) _stoneCountLbl.Text = $"x{_stoneCount}";
            if (_jadeCountLbl != null) _jadeCountLbl.Text = $"x{_jadeCount}";
        }
        public void SetCodexCurrencies(int stones, int jades)
        {
            _stoneCount = Math.Max(0, stones);
            _jadeCount = Math.Max(0, jades);
            UpdateCurrencyUI();
        }

        private static bool IsRareOrLegendary(ItemGrade g) => g == ItemGrade.Rare || g == ItemGrade.Legendary;

        private int FindFirstEligibleCurrencyInfoId(RowVM vm)
        {
            if (vm == null) return -1;
            if (!_allowCrossGradeCurrency) return -1;
            if (!IsRareOrLegendary(vm.Rarity)) return -1;

            int stoneId = _itemInfoStone;
            int jadeId = _itemInfoJade;

            if (stoneId > 0 && HasItemInAnyInventory(stoneId)) return stoneId;
            if (jadeId > 0 && HasItemInAnyInventory(jadeId)) return jadeId;

            return -1;
        }

        private void RefreshCurrencyUI()
        {
            if (_stoneCountLbl != null) _stoneCountLbl.Text = $"x{_stoneCount}";
            if (_jadeCountLbl != null) _jadeCountLbl.Text = $"x{_jadeCount}";
        }

        private long ComputeInventorySignature()
        {
            var user = GameScene.User;
            if (user?.Inventory == null) return 0;

            unchecked
            {
                long h = 1469598103934665603L;
                foreach (var it in user.Inventory)
                {
                    int infoIndex = it?.Info?.Index ?? -1;
                    int count = it?.Count ?? 0;
                    ulong uid = it?.UniqueID ?? 0UL;

                    h ^= infoIndex; h *= 1099511628211L;
                    h ^= count; h *= 1099511628211L;
                    h ^= (long)uid; h *= 1099511628211L;
                }
                return h;
            }
        }

        public void InventoryChangedRefresh()
        {
            if (!Visible) return;
            long sig = ComputeInventorySignature();
            if (sig == _lastInvSig) return;
            _lastInvSig = sig;

            ApplySearch(_searchQuery, keepCursor: true);
            RecalcRightCounters();
            UpdateScrollButtons();
            UpdateScrollThumb();
            RefreshHeaderLevelXp();
            RefreshLevelHint();

            RefreshCurrencyUI();
        }

        private void TryUseCurrency(int setId, Currency cur)
        {
            var scene = GameScene.Scene;

            if (cur == Currency.Stone && GameScene.Stone <= 0)
            { scene?.ChatDialog.ReceiveChat(L("Codex_NoStone"), ChatType.Hint); return; }
            if (cur == Currency.Jade && GameScene.Jade <= 0)
            { scene?.ChatDialog.ReceiveChat(L("Codex_NoJade"), ChatType.Hint); return; }

            Network.Enqueue(new C.CodexUseCurrency { RowId = setId, Currency = (byte)cur });
        }

        private static bool IsCurrencyAllowedForRarity(Currency cur, ItemGrade rarity)
        {
            return (rarity == ItemGrade.Rare && cur == Currency.Stone) ||
                   (rarity == ItemGrade.Legendary && cur == Currency.Jade);
        }

        private Currency PickDefaultCurrencyForSet(RowVM data)
        {
            if (data == null) return Currency.None;

            if (data.Rarity == ItemGrade.Legendary && GameScene.Jade > 0) return Currency.Jade;
            if (data.Rarity == ItemGrade.Rare && GameScene.Stone > 0) return Currency.Stone;
            return Currency.None;
        }
    }
}

