using System;
using System.Collections.Generic;
using System.Drawing;
using Client.MirControls;
using Client.MirGraphics;
using Client.MirScenes;
using Client.MirSounds;
using Client.MirObjects;

namespace Client.MirScenes.Dialogs
{
    public sealed class CurrencyListDialog : MirImageControl
    {
        public static CurrencyListDialog Instance;

        // If you know exact ItemInfo indices, set them at runtime (e.g., after login).
        public static int StoneItemInfoIndex = 0;
        public static int JadeItemInfoIndex = 0;
        public static int PearlItemInfoIndex = 0;
        public static int HwanyangItemInfoIndex = 0;
        public static int MuhanItemInfoIndex = 0;
        public static int BattleItemInfoIndex = 0;
        public static int ChaosItemInfoIndex = 0;

        // Name fallbacks if indices above are 0.
        public static string StoneNameContains = "Stone";
        public static string JadeNameContains = "Jade";
        public static string PearlNameContains = "Pearl";
        public static string HwanyangNameContains = "Hwanyang";
        public static string MuhanNameContains = "Muhan";
        public static string BattleNameContains = "Battle";
        public static string ChaosNameContains = "Chaos";

        private const int RowHeight = 22;
        private const int ArrowWidth = 16; 
        private const int ArrowGap = 4; 

        private readonly MirControl _listHost;
        private readonly MirButton _btnClose;
        private readonly MirButton _btnUp;
        private readonly MirButton _btnDown;

        private readonly List<Row> _rows = new List<Row>();
        private readonly List<Entry> _model = new List<Entry>();

        private int _scrollIndex;
        private int _visibleRows;

        public static bool ShowGuildRowsForEveryone = false;

        public CurrencyListDialog()
        {
            Library = Libraries.Prguse;
            Index = 120;
            Movable = true;
            Sort = true;

            var listRect = new Rectangle(10, 54, Size.Width - 20, Size.Height - 110);

            _listHost = new MirControl
            {
                Parent = this,
                Location = listRect.Location,
                Size = listRect.Size,
                NotControl = true
            };

            _btnClose = new MirButton
            {
                Parent = this,
                Library = Libraries.Prguse2,
                Index = 360,
                HoverIndex = 361,
                PressedIndex = 362,
                Location = new Point(Size.Width - 26, 4),
                Sound = SoundList.ButtonA
            };
            _btnClose.Click += (o, e) => Hide();

            _btnUp = new MirButton
            {
                Parent = this,
                Library = Libraries.Prguse2,
                Index = 197,
                HoverIndex = 198,
                PressedIndex = 199,
                Size = new Size(ArrowWidth, 14),
                Location = new Point(listRect.Right - ArrowWidth, listRect.Top + 1),
                Sound = SoundList.ButtonA
            };
            _btnUp.Click += (o, e) => Scroll(-1);

            _btnDown = new MirButton
            {
                Parent = this,
                Library = Libraries.Prguse2,
                Index = 207,
                HoverIndex = 208,
                PressedIndex = 209,
                Size = new Size(ArrowWidth, 14),
                Location = new Point(listRect.Right - ArrowWidth, listRect.Bottom - 12),
                Sound = SoundList.ButtonA
            };
            _btnDown.Click += (o, e) => Scroll(+1);

            RecalcVisibleRows();
            BuildRows();
            RefreshModel();
            RedrawRows();
        }

        public void Reposition(Point localPointWithinParent) => Location = localPointWithinParent;

        private void RecalcVisibleRows()
        {
            _visibleRows = Math.Max(1, _listHost.Size.Height / RowHeight);
        }

        private void BuildRows()
        {
            _rows.Clear();

            int reservedRight = ArrowWidth + ArrowGap;
            int amountWidth = 75; 
            int amountX = _listHost.Size.Width - reservedRight - amountWidth;
            if (amountX < 60) amountX = 60;

            for (int i = 0; i < _visibleRows; i++)
            {
                var host = new MirControl
                {
                    Parent = _listHost,
                    Location = new Point(0, i * RowHeight),
                    Size = new Size(_listHost.Size.Width, RowHeight)
                };

                var name = new MirLabel
                {
                    Parent = host,
                    Location = new Point(6, 3),
                    Size = new Size(amountX - 10, RowHeight - 6),
                    Font = new Font(Settings.FontName, 8F),
                    NotControl = true
                };

                var amount = new MirLabel
                {
                    Parent = host,
                    Location = new Point(amountX, 3),
                    Size = new Size(amountWidth, RowHeight - 6),
                    DrawFormat = TextFormatFlags.Left | TextFormatFlags.VerticalCenter,
                    Font = new Font(Settings.FontName, 8F),
                    NotControl = true
                };

                _rows.Add(new Row { Host = host, Name = name, Amount = amount });
            }
        }

        private void Scroll(int delta)
        {
            if (_model.Count <= _visibleRows) return;
            _scrollIndex = Math.Max(0, Math.Min(_model.Count - _visibleRows, _scrollIndex + delta));
            RedrawRows();
        }

        public void RefreshModel()
        {
            _model.Clear();

            // Core currencies
            _model.Add(new Entry { Title = "Gold", GetAmount = () => GameScene.Gold });
            _model.Add(new Entry { Title = "Credits", GetAmount = () => GameScene.Credit });
            _model.Add(new Entry { Title = "Pearl", GetAmount = CountPearl });

            // --- Guild (owner/leader only) ---
            if (ShouldShowGuildRows())
            {
                _model.Add(new Entry { Title = "Guild Gold", GetAmount = () => GameScene.Scene.GuildDialog.Gold });
                _model.Add(new Entry { Title = "Guild Points", GetAmount = () => GameScene.Scene.GuildDialog.SparePoints });
            }

            // Codex tokens
            _model.Add(new Entry { Title = "Stone", GetAmount = () => GameScene.Stone });
            _model.Add(new Entry { Title = "Jade", GetAmount = () => GameScene.Jade });

            _model.Sort((a, b) => string.Compare(a.Title, b.Title, StringComparison.Ordinal));
        }

        public void RedrawRows()
        {
            for (int i = 0; i < _rows.Count; i++)
            {
                int idx = _scrollIndex + i;
                var row = _rows[i];

                if (idx >= _model.Count)
                {
                    row.Host.Visible = false;
                    continue;
                }

                var e = _model[idx];
                row.Host.Visible = true;

                row.Name.Text = e.Title;

                long amount = 0;
                try { amount = e.GetAmount?.Invoke() ?? 0; } catch { /* ignore */ }
                row.Amount.Text = amount.ToString("###,###,##0");
            }
        }

        public static void NotifyChanged()
        {
            if (Instance == null || Instance.IsDisposed) return;
            Instance.RefreshModel();
            Instance.RedrawRows();
        }

        private static long CountPearl()
        {
            if (PearlItemInfoIndex > 0) return CountByIndex(PearlItemInfoIndex);
            return CountByName(PearlNameContains);
        }

        private static long CountByIndex(int infoIndex)
        {
            var grid = GameScene.Scene?.InventoryDialog?.Grid;
            if (grid == null) return 0;
            long total = 0;
            foreach (var cell in grid)
            {
                var it = cell?.Item;
                if (it?.Info == null) continue;
                if (it.Info.Index == infoIndex)
                    total += Math.Max(1, (int)it.Count);
            }
            return total;
        }

        private static long CountByName(string contains)
        {
            var grid = GameScene.Scene?.InventoryDialog?.Grid;
            if (grid == null) return 0;
            long total = 0;
            foreach (var cell in grid)
            {
                var it = cell?.Item;
                if (it?.Info == null || string.IsNullOrEmpty(it.Info.Name)) continue;
                if (it.Info.Name.IndexOf(contains, StringComparison.OrdinalIgnoreCase) >= 0)
                    total += Math.Max(1, (int)it.Count);
            }
            return total;
        }
        private static bool ShouldShowGuildRows()
        {
            if (ShowGuildRowsForEveryone) return true;

            var user = MapObject.User;
            var gd = GameScene.Scene?.GuildDialog;
            if (user == null || gd == null) return false;

            // Must actually be in a guild
            if (string.IsNullOrEmpty(user.GuildName)) return false;

            // Owner/leader is rank 0 in your codebase.
            if (GuildDialog.MyRankId != 0) return false;

            // If this hides for real owners at login, remove this line.
            if (string.IsNullOrEmpty(user.GuildRankName)) return false;

            return true;
        }

        private struct Row
        {
            public MirControl Host;
            public MirLabel Name;
            public MirLabel Amount;
        }

        private sealed class Entry
        {
            public string Title;
            public Func<long> GetAmount;
        }
    }
}
