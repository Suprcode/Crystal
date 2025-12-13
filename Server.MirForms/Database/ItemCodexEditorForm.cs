// Server/MirForms/ItemCodexEditorForm.cs
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Text.Json;
using System.Text.Json.Serialization;
using Server.MirDatabase;
using Server.MirEnvir;
using Shared;
using Shared.Data;

namespace Server.MirForms
{
    public partial class ItemCodexEditorForm : Form
    {
        private static ItemCodexEditorForm _instance;

        private readonly Envir _envir;

        // Collections data (master + view)
        private readonly BindingList<CollectionRow> _collectionsMaster = new BindingList<CollectionRow>();
        private readonly BindingList<CollectionRow> _collectionsView = new BindingList<CollectionRow>();
        private CollectionRow _current;

        // Rewards
        private readonly BindingList<RewardRow> _rewards = new BindingList<RewardRow>();

        // Available items
        private readonly BindingList<dynamic> _availItems = new BindingList<dynamic>();
        private ItemType? _filterType = null;
        private string _lastAvailSearch = string.Empty;

        // Stage picker options
        private class StageOption
        {
            public int Value { get; set; }
            public string Text { get; set; } = string.Empty;
            public override string ToString() => Text;
        }

        private readonly BindingList<StageOption> _stageOptions = new BindingList<StageOption>();

        // ItemIndex -> ItemInfo
        private readonly Dictionary<int, ItemInfo> _itemIndexMap = new Dictionary<int, ItemInfo>();

        // Auto-build helpers
        private readonly Regex _reBrackets = new Regex(@"\[[^]]*\]", RegexOptions.Compiled);
        private readonly Regex _reParens = new Regex(@"\([^)]*\)", RegexOptions.Compiled);
        private readonly Regex _reTrailing = new Regex(@"\s+\d+$", RegexOptions.Compiled);

        private bool _suspendRewardSync = false;
        private bool _loadingSelection = false;
        private bool _suspendAutoSave = false;

        // ---------- models ----------
        private class ItemRow
        {
            public int ItemIndex { get; set; }
            public string ItemName { get; set; }
            public string ItemType { get; set; }
			public int Stage { get; set; } = CodexRequirement.AnyStage; // -1 = Any
        }

        private class RewardRow
        {
            public Stat Stat { get; set; }
            public int Value { get; set; }
        }

        private class ItemDto
        {
            public int Index { get; set; }
            public sbyte Stage { get; set; }
        }

        private class CollectionDto
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public List<ItemDto> Items { get; set; }
            public Dictionary<string, int> Reward { get; set; }
            public int XP { get; set; }
            public string Rarity { get; set; }
            public string Bucket { get; set; }
            public bool Enabled { get; set; }
            public string Start { get; set; }
            public string End { get; set; }
            public bool KeepStats { get; set; }
        }

        private class CollectionRow
        {
            public int Id { get; set; }
            public string Name { get; set; } = string.Empty;
            public string RewardText { get; set; } = string.Empty;
            public BindingList<ItemRow> Items { get; } = new BindingList<ItemRow>();
            public int RequiredCount => Items?.Count ?? 0;

            public ItemGrade Rarity { get; set; } = ItemGrade.None;
            public int RewardXP { get; set; } = 0;
            public CodexBucket Bucket { get; set; } = CodexBucket.Character; // 0=Character,1=Limited,2=Event
            public bool Enabled { get; set; } = true;
            public DateTime? StartTimeUtc { get; set; }
            public DateTime? EndTimeUtc { get; set; }
            public bool KeepStatsAfterExpiry { get; set; }
        }

        private IEnumerable<CollectionRow> EnumerateSelectedCollections()
        {
            if (dgvCollections == null)
            {
                if (_current != null) yield return _current;
                yield break;
            }

            if (dgvCollections.SelectedRows.Count == 0)
            {
                if (_current != null) yield return _current;
                yield break;
            }

            foreach (DataGridViewRow row in dgvCollections.SelectedRows)
            {
                if (row?.DataBoundItem is CollectionRow col)
                    yield return col;
            }
        }

        private void ApplyToSelectedCollections(Action<CollectionRow> updater)
        {
            if (updater == null) return;

            bool applied = false;

            foreach (var col in EnumerateSelectedCollections())
            {
                updater(col);
                applied = true;
            }

            if (!applied && _current != null)
                updater(_current);
        }

        private void InitializeStageOptions()
        {
            _stageOptions.Clear();
            _stageOptions.Add(new StageOption { Value = CodexRequirement.AnyStage, Text = "Any Stage" });
            //for (int stage = 0; stage <= TranscendenceInfo.MaxStage; stage++)
            //    _stageOptions.Add(new StageOption { Value = stage, Text = $"Stage {stage}" });
        }

        private void EnsureStageOptionExists(int stage)
        {
            if (_stageOptions.Any(opt => opt.Value == stage)) return;

            string label = stage == CodexRequirement.AnyStage ? "Any Stage" : $"Stage {stage}";
            var option = new StageOption { Value = stage, Text = label };

            int insertIndex = 0;
            while (insertIndex < _stageOptions.Count && _stageOptions[insertIndex].Value < stage)
                insertIndex++;

            _stageOptions.Insert(insertIndex, option);
        }

        private void SyncStageOptionsWithCurrentItems()
        {
            if (_current == null) return;
            foreach (var row in _current.Items)
                EnsureStageOptionExists(row.Stage);
        }

        // ---------- static launchers ----------
        public static void ShowForMain()
        {
            if (Envir.Main == null)
            {
                MessageBox.Show("Envir.Main is not initialized.", "Item Codex Editor",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            ShowForEnvir(Envir.Main);
        }
        public static void ShowForEnvir(Envir envir)
        {
            if (_instance == null || _instance.IsDisposed)
                _instance = new ItemCodexEditorForm(envir);

            _instance.Show();
            _instance.BringToFront();
        }

        // ---------- ctor ----------
        public ItemCodexEditorForm() : this(Envir.Main) { }
        public ItemCodexEditorForm(Envir envir)
        {
            _envir = envir ?? Envir.Main;
            InitializeComponent();

            // Bind collections grid to view list
            dgvCollections.AutoGenerateColumns = false;
            dgvCollections.DataSource = _collectionsView;

            // Selected items grid
            dgvItems.AutoGenerateColumns = false;
            InitializeStageOptions();
            colItemIndex.DataPropertyName = nameof(ItemRow.ItemIndex);
            colItemName.DataPropertyName = nameof(ItemRow.ItemName);
            colItemType.DataPropertyName = nameof(ItemRow.ItemType);
            colItemStage.DataPropertyName = nameof(ItemRow.Stage);
            colItemStage.DisplayMember = nameof(StageOption.Text);
            colItemStage.ValueMember = nameof(StageOption.Value);
            colItemStage.DataSource = _stageOptions;
            colItemStage.DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton;
            colItemStage.FlatStyle = FlatStyle.Standard;
            cbStageQuick.DisplayMember = nameof(StageOption.Text);
            cbStageQuick.ValueMember = nameof(StageOption.Value);
            cbStageQuick.DataSource = _stageOptions;
            if (_stageOptions.Count > 0)
                cbStageQuick.SelectedValue = CodexRequirement.AnyStage;

            // Rewards grid
            dgvRewards.AutoGenerateColumns = false;
            dgvRewards.DataSource = _rewards;
            dgvRewards.CellParsing += (s, e) =>
            {
                if (e.ColumnIndex == 1)
                {
                    var txt = (e.Value?.ToString() ?? "").Trim();
                    if (txt.Length == 0) { e.Value = 0; e.ParsingApplied = true; return; }
                    txt = txt.Replace(",", "");
                    if (int.TryParse(txt, NumberStyles.Integer, CultureInfo.InvariantCulture, out int v))
                    {
                        e.Value = v;
                        e.ParsingApplied = true;
                    }
                }
            };
            dgvRewards.CurrentCellDirtyStateChanged += (s, e) =>
            {
                var cell = dgvRewards.CurrentCell;
                if (cell is DataGridViewComboBoxCell && dgvRewards.IsCurrentCellDirty)
                    dgvRewards.CommitEdit(DataGridViewDataErrorContexts.Commit);
            };
            dgvRewards.DataError += (s, e) => { e.ThrowException = false; };
            dgvRewards.CellValueChanged += (s, e) => SyncRewardsToCurrent();
            _rewards.ListChanged += (s, e) => SyncRewardsToCurrent();

            // Available items grid
            dgvAvailable.AutoGenerateColumns = false;
            dgvAvailable.DataSource = _availItems;

            // Details dropdowns
            cbBucketDetail.Items.Clear();
            foreach (var val in Enum.GetValues(typeof(CodexBucket))) cbBucketDetail.Items.Add(val);
            cbRarityDetail.Items.Clear();
            foreach (var val in Enum.GetValues(typeof(ItemGrade))) cbRarityDetail.Items.Add(val);

            // Bucket filter (left pane)
            if (cbBucketFilter != null)
            {
                cbBucketFilter.Items.Clear();
                cbBucketFilter.Items.Add("All");
                foreach (var val in Enum.GetValues(typeof(CodexBucket))) cbBucketFilter.Items.Add(val);
                cbBucketFilter.SelectedIndex = 0;
                cbBucketFilter.SelectedIndexChanged += (_, __) => ApplyCollectionFilters();
            }

            // Detail change events
            txtName.TextChanged += (_, __) =>
            {
                if (_current == null || _loadingSelection) return;
                var name = txtName.Text?.Trim() ?? string.Empty;
                ApplyToSelectedCollections(col => col.Name = name);
                dgvCollections.Refresh();
				AutoSaveIfPossible();
            };
            cbBucketDetail.SelectedIndexChanged += (_, __) =>
            {
                if (_current == null || cbBucketDetail.SelectedItem == null || _loadingSelection) return;
                var prevBucket = _current.Bucket;
                var bucket = (CodexBucket)cbBucketDetail.SelectedItem;
                ApplyToSelectedCollections(col =>
                {
                    col.Bucket = bucket;
                    if (bucket == CodexBucket.Character)
                    {
                        if ((col.StartTimeUtc.HasValue || col.EndTimeUtc.HasValue || col.KeepStatsAfterExpiry) &&
                            MessageBox.Show("Changing to Character will clear time window and keep-stats. Continue?", "Change Category", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
                        {
                            _loadingSelection = true;
                            cbBucketDetail.SelectedItem = prevBucket;
                            _loadingSelection = false;
                            return;
                        }
                        col.StartTimeUtc = null;
                        col.EndTimeUtc = null;
                        col.KeepStatsAfterExpiry = false;
                    }
                });
                UpdateTimeFieldAvailability();
                dgvCollections.Refresh();
				AutoSaveIfPossible();
            };
            cbRarityDetail.SelectedIndexChanged += (_, __) =>
            {
                if (_current == null || cbRarityDetail.SelectedItem == null || _loadingSelection) return;
                var rarity = (ItemGrade)cbRarityDetail.SelectedItem;
                ApplyToSelectedCollections(col => col.Rarity = rarity);
                dgvCollections.Refresh();
				AutoSaveIfPossible();
            };
            nudXP.ValueChanged += (_, __) =>
            {
                if (_current == null || _loadingSelection) return;
                int xp = (int)nudXP.Value;
                ApplyToSelectedCollections(col => col.RewardXP = xp);
                dgvCollections.Refresh();
				AutoSaveIfPossible();
            };
            chkEnabled.CheckedChanged += (_, __) =>
            {
                if (_current == null || _loadingSelection) return;
                bool enabled = chkEnabled.Checked;
                ApplyToSelectedCollections(col => col.Enabled = enabled);
                dgvCollections.Refresh();
				AutoSaveIfPossible();
            };
            dtpStartUtc.ValueChanged += (_, __) => UpdateTimeFieldsFromUI();
            dtpEndUtc.ValueChanged += (_, __) => UpdateTimeFieldsFromUI();
            chkKeepStats.CheckedChanged += (_, __) =>
            {
                if (_current == null || _loadingSelection) return;
                bool keep = chkKeepStats.Checked;
                ApplyToSelectedCollections(col => col.KeepStatsAfterExpiry = keep);
                dgvCollections.Refresh();
                AutoSaveIfPossible();
            };

            chkEnabled.Checked = true;

            // Build item map, filters, and load data
            RebuildItemIndexMap();
            PopulateTypeFilter();
            RefreshAvailableItems(string.Empty);
            LoadFromServer();

            // Keyboard shortcuts
            KeyDown += ItemCodexEditorForm_KeyDown;

            // SAFELY set splitter constraints & distances *after* layout
            Shown += (_, __) => InitSplittersSafely();
            splitMain.SizeChanged += (_, __) => InitSplittersSafely();
            splitRight.SizeChanged += (_, __) => InitSplittersSafely();
        }

        // Set distances WITHOUT throwing, even during small sizes
        private void InitSplittersSafely()
        {
            // Left/Right
            SafeConfigureSplit(splitMain, desired: 320, min1: 220, min2: 300);
            // Top/Bottom on right (horizontal orientation)
            SafeConfigureSplit(splitRight, desired: 360, min1: 200, min2: 220);
        }

        private static void SafeConfigureSplit(SplitContainer sc, int desired, int min1, int min2)
        {
            try
            {
                int avail = (sc.Orientation == Orientation.Vertical) ? sc.ClientSize.Width : sc.ClientSize.Height;
                int splitter = sc.SplitterWidth;
                int space = avail - splitter;
                if (space <= 0) return; // will run again on SizeChanged

                // Clamp mins so they never exceed available space
                int p1 = Math.Max(0, Math.Min(min1, space));
                int p2 = Math.Max(0, Math.Min(min2, Math.Max(0, space - p1)));

                // Compute a distance within the final legal range
                int maxLeft = space - p2;
                int dist = Math.Max(p1, Math.Min(desired, maxLeft));

                // 1) Set a safe SplitterDistance first (within 0..space) to avoid ApplyPanel*MinSize throwing.
                int distSafe = Math.Max(0, Math.Min(dist, space));
                sc.SplitterDistance = distSafe;

                // 2) Now apply min sizes (current distance is already within the upcoming legal range).
                sc.Panel1MinSize = p1;
                sc.Panel2MinSize = p2;
            }
            catch
            {
                // swallow early layout quirks; next SizeChanged/Shown will fix it
            }
        }

        // ---------- map/helpers ----------
        private void RebuildItemIndexMap()
        {
            _itemIndexMap.Clear();
            if (_envir?.ItemInfoList == null) return;
            foreach (var ii in _envir.ItemInfoList)
            {
                if (ii == null) continue;
                _itemIndexMap[ii.Index] = ii;
            }
        }

        private ItemInfo ItemByIndex(int itemIndex)
        {
            if (_itemIndexMap.TryGetValue(itemIndex, out var info))
                return info;

            var found = _envir?.ItemInfoList?.FirstOrDefault(x => x != null && x.Index == itemIndex);
            if (found != null) _itemIndexMap[itemIndex] = found;
            return found;
        }

        private bool ItemExists(int itemIndex) =>
            _itemIndexMap.ContainsKey(itemIndex) ||
            (_envir?.ItemInfoList?.Any(x => x != null && x.Index == itemIndex) ?? false);

        private static string StatsToText(Stats s)
        {
            if (s == null || s.Values == null || s.Values.Count == 0) return string.Empty;
            return string.Join(",", s.Values.Select(kv => $"{kv.Key}={kv.Value}"));
        }

        private static Stats ParseStats(string txt)
        {
            var s = new Stats();
            if (string.IsNullOrWhiteSpace(txt)) return s;

            var parts = txt.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (var p in parts)
            {
                var kv = p.Split('=');
                if (kv.Length != 2) continue;
                if (!Enum.TryParse<Stat>(kv[0].Trim(), true, out var stat)) continue;
                if (!int.TryParse(kv[1].Trim(), out int val)) continue;
                s[stat] += val;
            }
            return s;
        }

        // ---------- UI events ----------
        private void txtSearchCollections_TextChanged(object sender, EventArgs e) => ApplyCollectionFilters();

        private void btnAddCollection_Click(object sender, EventArgs e)
        {
            int nextId = (_collectionsMaster.Count == 0) ? 1 : _collectionsMaster.Max(c => c.Id) + 1;
            var row = new CollectionRow { Id = nextId, Name = $"Collection {nextId}", Enabled = true };
            _collectionsMaster.Add(row);
            ApplyCollectionFilters();
			AutoSaveIfPossible();

            for (int i = 0; i < _collectionsView.Count; i++)
            {
                if (_collectionsView[i].Id == nextId)
                {
                    dgvCollections.ClearSelection();
                    dgvCollections.Rows[i].Selected = true;
                    dgvCollections.FirstDisplayedScrollingRowIndex = i;
                    break;
                }
            }
        }

        private void btnRemoveCollection_Click(object sender, EventArgs e)
        {
            if (_current == null) return;
            if (MessageBox.Show($"Delete collection '{_current.Name}'?", "Confirm",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes) return;

            int oldId = _current.Id;
            _collectionsMaster.Remove(_current);
            _current = null;
            ApplyCollectionFilters();
			AutoSaveIfPossible();

            if (_collectionsView.Count > 0)
            {
                int idx = _collectionsView.TakeWhile(r => r.Id < oldId).Count();
                idx = Math.Min(idx, _collectionsView.Count - 1);
                dgvCollections.Rows[idx].Selected = true;
            }
        }

        private void btnDuplicateCollection_Click(object sender, EventArgs e)
        {
            if (_current == null) return;
            int nextId = (_collectionsMaster.Count == 0) ? 1 : _collectionsMaster.Max(c => c.Id) + 1;

            var copy = new CollectionRow
            {
                Id = nextId,
                Name = _current.Name + " (Copy)",
                RewardText = _current.RewardText,
                Rarity = _current.Rarity,
                RewardXP = _current.RewardXP,
                Bucket = _current.Bucket,
                Enabled = _current.Enabled,
                StartTimeUtc = _current.StartTimeUtc,
                EndTimeUtc = _current.EndTimeUtc,
                KeepStatsAfterExpiry = _current.KeepStatsAfterExpiry
            };
            foreach (var it in _current.Items)
                copy.Items.Add(new ItemRow { ItemIndex = it.ItemIndex, ItemName = it.ItemName, ItemType = it.ItemType });

            _collectionsMaster.Add(copy);
            ApplyCollectionFilters();
			AutoSaveIfPossible();

            for (int i = 0; i < _collectionsView.Count; i++)
                if (_collectionsView[i].Id == nextId) { dgvCollections.ClearSelection(); dgvCollections.Rows[i].Selected = true; break; }
        }

        private void OnCollectionSelectionChanged()
        {
            if (_current != null)
            {
                try { dgvRewards.EndEdit(); } catch { }
                _suspendRewardSync = true;
                _current.RewardText = RewardsGridToText();
                _suspendRewardSync = false;
            }

            _current = null;
            if (dgvCollections.CurrentRow?.DataBoundItem is CollectionRow r) _current = r;

            _loadingSelection = true;
            if (_current == null)
            {
                dgvItems.DataSource = null;
                lblId.Text = "ID: -";
                txtName.Text = string.Empty;
                cbBucketDetail.SelectedItem = null;
                cbRarityDetail.SelectedItem = null;
                nudXP.Value = 0;
                chkEnabled.Checked = true;
                _suspendRewardSync = true; _rewards.Clear(); _suspendRewardSync = false;
                _loadingSelection = false;
                UpdateTimeFieldAvailability();
                UpdateButtonsEnabled();
                return;
            }

            dgvItems.DataSource = _current.Items;
            SortCurrentItemsByIndex();
            SyncStageOptionsWithCurrentItems();

            lblId.Text = $"ID: {_current.Id}";
            txtName.Text = _current.Name ?? string.Empty;
            cbBucketDetail.SelectedItem = _current.Bucket;
            cbRarityDetail.SelectedItem = _current.Rarity;
            nudXP.Value = Math.Max(nudXP.Minimum, Math.Min(nudXP.Maximum, _current.RewardXP));
            chkEnabled.Checked = _current.Enabled;

            _suspendRewardSync = true;
            _rewards.Clear();
            var stats = ParseStats(_current.RewardText ?? string.Empty);
            if (stats?.Values != null)
                foreach (var kv in stats.Values)
                    _rewards.Add(new RewardRow { Stat = kv.Key, Value = kv.Value });
            _suspendRewardSync = false;

            _loadingSelection = false;
            UpdateTimeFieldAvailability();
            UpdateButtonsEnabled();
        }

        private void UpdateTimeFieldAvailability()
        {
            bool limited = _current != null && (_current.Bucket == CodexBucket.Limited || _current.Bucket == CodexBucket.Event);
            dtpStartUtc.Enabled = limited;
            dtpEndUtc.Enabled = limited;
            chkKeepStats.Enabled = limited;

            if (_current == null)
            {
                dtpStartUtc.Checked = false;
                dtpEndUtc.Checked = false;
                chkKeepStats.Checked = false;
                return;
            }

            _loadingSelection = true;

            if (_current.StartTimeUtc.HasValue)
            {
                var start = _current.StartTimeUtc.Value;
                dtpStartUtc.Value = start < dtpStartUtc.MinDate ? dtpStartUtc.MinDate : start;
                dtpStartUtc.Checked = true;
            }
            else
            {
                dtpStartUtc.Checked = false;
            }

            if (_current.EndTimeUtc.HasValue)
            {
                var end = _current.EndTimeUtc.Value;
                dtpEndUtc.Value = end < dtpEndUtc.MinDate ? dtpEndUtc.MinDate : end;
                dtpEndUtc.Checked = true;
            }
            else
            {
                dtpEndUtc.Checked = false;
            }

            chkKeepStats.Checked = _current.KeepStatsAfterExpiry;

            _loadingSelection = false;
        }

        private void UpdateTimeFieldsFromUI()
        {
            if (_current == null || _loadingSelection) return;

            bool limited = _current.Bucket == CodexBucket.Limited || _current.Bucket == CodexBucket.Event;
            if (!limited)
            {
                dtpStartUtc.Checked = false;
                dtpEndUtc.Checked = false;
                chkKeepStats.Checked = false;
                return;
            }

            DateTime SnapMinutes(DateTime dt, int step)
            {
                if (step <= 0) return dt;
                int minutes = (int)Math.Round(dt.Minute / (double)step) * step;
                if (minutes >= 60) { dt = dt.AddHours(1); minutes = 0; }
                return new DateTime(dt.Year, dt.Month, dt.Day, dt.Hour, minutes, 0);
            }

            DateTime? start = dtpStartUtc.Checked ? (DateTime?)SnapMinutes(dtpStartUtc.Value, 30) : null;
            DateTime? end = dtpEndUtc.Checked ? (DateTime?)SnapMinutes(dtpEndUtc.Value, 30) : null;

            if (start.HasValue && end.HasValue && start.Value >= end.Value)
            {
                MessageBox.Show("Start time must be before End time.", "Time Window", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                dtpEndUtc.Checked = false;
                end = null;
            }

            ApplyToSelectedCollections(col =>
            {
                col.StartTimeUtc = start;
                col.EndTimeUtc = end;
                col.KeepStatsAfterExpiry = chkKeepStats.Checked;
            });

            dgvCollections.Refresh();
            AutoSaveIfPossible();
        }

        private void UpdateButtonsEnabled()
        {
            bool hasSel = _current != null;
            btnAddItem.Enabled = hasSel;
            btnRemoveItem.Enabled = hasSel && dgvItems.CurrentRow != null;
            btnSortItems.Enabled = hasSel;
            btnAddSelectedAvail.Enabled = hasSel;

            tsbSaveTxt.Enabled = true;
            tsbLoadTxt.Enabled = true;
            tsbApply.Enabled = true;
            tsbRebuild.Enabled = true;
        }

        private void AutoSaveIfPossible()
        {
            if (_suspendAutoSave) return;
            try
            {
                // keep current reward row in sync before persisting
                SyncRewardsToCurrent();

                // push changes into the live server environment
                ApplyToServer();

                // keep a text export up to date without prompting
                SaveToTxt(silent: true);

                if (statusText != null)
                    statusText.Text = "Auto-applied to server.";
            }
            catch
            {
                // ignore autosave errors (status bar in Envir will log if needed)
            }
        }

        // Selected items buttons
        private void btnAddItem_Click(object sender, EventArgs e)
        {
            if (_current == null) return;
            if (!int.TryParse(txtAddIndex.Text.Trim(), out int idx) || idx <= 0)
            {
                MessageBox.Show("Enter a valid ItemIndex.", "Add Item",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var info = ItemByIndex(idx);
            if (info == null)
            {
                MessageBox.Show($"No item with Index={idx} exists.", "Add Item",
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

			_current.Items.Add(new ItemRow { ItemIndex = idx, ItemName = info.Name, ItemType = info.Type.ToString(), Stage = CodexRequirement.AnyStage });
            SortCurrentItemsByIndex();
			AutoSaveIfPossible();
        }
        private void btnRemoveItem_Click(object sender, EventArgs e)
        {
            if (_current == null || dgvItems.CurrentRow == null) return;
            if (dgvItems.CurrentRow.DataBoundItem is ItemRow ir)
			{
				_current.Items.Remove(ir);
				AutoSaveIfPossible();
			}
        }
		private void btnSortItems_Click(object sender, EventArgs e) { SortCurrentItemsByIndex(); AutoSaveIfPossible(); }

        private void SortCurrentItemsByIndex()
        {
            if (_current == null) return;
            var sorted = _current.Items.OrderBy(i => i.ItemIndex).ToList();
            _current.Items.RaiseListChangedEvents = false;
            _current.Items.Clear();
            foreach (var it in sorted) _current.Items.Add(it);
            _current.Items.RaiseListChangedEvents = true;
            _current.Items.ResetBindings();
            SyncStageOptionsWithCurrentItems();
        }

        private void dgvItems_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            if (_current == null) return;
            if (e.RowIndex < 0 || e.ColumnIndex < 0) return;

            var column = dgvItems.Columns[e.ColumnIndex];
            if (column == colItemStage)
            {
                if (dgvItems.Rows[e.RowIndex].DataBoundItem is ItemRow row)
                {
                    EnsureStageOptionExists(row.Stage);
                    _current.Items.ResetBindings();
                    SyncStageOptionsWithCurrentItems();
                    AutoSaveIfPossible();
                    if (statusText != null)
                        statusText.Text = row.Stage == CodexRequirement.AnyStage
                            ? "Stage updated: Any Stage."
                            : $"Stage updated: Stage {row.Stage}.";
                }
            }
            else
            {
                AutoSaveIfPossible();
            }
        }

        private void dgvItems_CurrentCellDirtyStateChanged(object sender, EventArgs e)
        {
            if (dgvItems.IsCurrentCellDirty)
                dgvItems.CommitEdit(DataGridViewDataErrorContexts.Commit);
        }

        private void dgvItems_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            if (e.Exception is ArgumentException && e.RowIndex >= 0 && e.ColumnIndex >= 0 &&
                dgvItems.Columns[e.ColumnIndex] == colItemStage)
            {
                if (dgvItems.Rows[e.RowIndex].DataBoundItem is ItemRow row)
                {
                    EnsureStageOptionExists(row.Stage);
                    _current?.Items.ResetBindings();
                    SyncStageOptionsWithCurrentItems();
                    e.ThrowException = false;
                    return;
                }
            }

            e.ThrowException = false;
        }

        private void btnApplyStage_Click(object sender, EventArgs e)
        {
            if (_current == null) return;
            if (cbStageQuick.SelectedItem is not StageOption option) return;

            var rows = dgvItems.SelectedRows.Cast<DataGridViewRow>().ToList();
            if (rows.Count == 0 && dgvItems.CurrentRow != null)
                rows.Add(dgvItems.CurrentRow);

            if (rows.Count == 0) return;

            foreach (var gridRow in rows)
            {
                if (gridRow?.DataBoundItem is ItemRow item)
                {
                    item.Stage = option.Value;
                    EnsureStageOptionExists(item.Stage);
                }
            }

            _current.Items.ResetBindings();
            SyncStageOptionsWithCurrentItems();
            AutoSaveIfPossible();
            if (statusText != null)
                statusText.Text = option.Value == CodexRequirement.AnyStage
                    ? "Applied Any Stage to selected item(s)."
                    : $"Applied Stage {option.Value} to selected item(s).";
        }

        // Rewards
        private void btnAddReward_Click(object sender, EventArgs e)
        {
            _suspendRewardSync = true;
            _rewards.Add(new RewardRow { Stat = Stat.MaxDC, Value = 1 });
            _suspendRewardSync = false;
            SyncRewardsToCurrent();
			AutoSaveIfPossible();
        }
        private void btnRemoveReward_Click(object sender, EventArgs e)
        {
            _suspendRewardSync = true;
            if (dgvRewards.CurrentRow?.DataBoundItem is RewardRow rr)
                _rewards.Remove(rr);
            _suspendRewardSync = false;
            SyncRewardsToCurrent();
			AutoSaveIfPossible();
        }
        private void SyncRewardsToCurrent()
        {
            if (_suspendRewardSync) return;
            if (_current == null) return;
            try { dgvRewards.EndEdit(); } catch { }
            _current.RewardText = RewardsGridToText();
			// don't autosave on every keystroke from grid; handled by callers
        }
        private string RewardsGridToText()
        {
            if (_rewards.Count == 0) return string.Empty;
            return string.Join(",", _rewards.Select(r => $"{r.Stat}={r.Value}"));
        }

        // Available
        private void PopulateTypeFilter()
        {
            var items = new List<object> { new { Text = "All", Value = (ItemType?)null } };
            foreach (var t in Enum.GetValues(typeof(ItemType)).Cast<ItemType>())
                items.Add(new { Text = t.ToString(), Value = (ItemType?)t });

            cbTypeFilter.DisplayMember = "Text";
            cbTypeFilter.ValueMember = "Value";
            cbTypeFilter.DataSource = items;
            cbTypeFilter.SelectedIndex = 0;
        }
        private void cbTypeFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            var sel = cbTypeFilter.SelectedItem;
            if (sel == null) { _filterType = null; return; }
            var pi = sel.GetType().GetProperty("Value");
            _filterType = (ItemType?)pi?.GetValue(sel);
            RefreshAvailableItems(_lastAvailSearch);
        }
        private void txtSearchAvail_TextChanged(object sender, EventArgs e) => RefreshAvailableItems(txtSearchAvail.Text);
        private void btnAddSelectedAvail_Click(object sender, EventArgs e)
        {
            if (_current == null || dgvAvailable.SelectedRows.Count == 0) return;

            var toAdd = new List<dynamic>();
            foreach (DataGridViewRow r in dgvAvailable.SelectedRows)
                if (r?.DataBoundItem != null) toAdd.Add(r.DataBoundItem);

            foreach (var x in toAdd)
            {
                int ix = (int)x.Index;
                var info = ItemByIndex(ix);
                if (info == null) continue;

				_current.Items.Add(new ItemRow { ItemIndex = ix, ItemName = info.Name, ItemType = info.Type.ToString(), Stage = CodexRequirement.AnyStage });
            }
            SortCurrentItemsByIndex();
			AutoSaveIfPossible();
        }
        private void dgvAvailable_CellDoubleClick(object sender, DataGridViewCellEventArgs e) =>
            btnAddSelectedAvail_Click(sender, e);

        private void RefreshAvailableItems(string q)
        {
            if (_envir?.ItemInfoList == null) return;
            q = (q ?? string.Empty).Trim();
            _lastAvailSearch = q;

            var query = _envir.ItemInfoList.Where(ii => ii != null);
            if (_filterType.HasValue) query = query.Where(ii => ii.Type == _filterType.Value);
            if (q.Length > 0) query = query.Where(ii => (ii.Name ?? "").IndexOf(q, StringComparison.OrdinalIgnoreCase) >= 0);

            var list = query.Select(ii => new { Index = ii.Index, Name = ii.Name ?? $"#{ii.Index}", Type = ii.Type.ToString() })
                            .OrderBy(x => x.Index).ToList();

            _availItems.Clear();
            foreach (var x in list) _availItems.Add(x);
        }

        // Top strip actions
        private void btnApply_Click(object sender, EventArgs e)
        {
            SyncRewardsToCurrent();
            ApplyToServer();
            statusText.Text = "Applied to server (in memory).";
            MessageBox.Show("Applied to server (in memory).", "Item Codex",
                            MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void btnSaveTxt_Click(object sender, EventArgs e)
        {
            SyncRewardsToCurrent();
            SaveToTxt();
            statusText.Text = "Exported ItemCodex.ini";
        }
        private void btnLoadTxt_Click(object sender, EventArgs e)
        {
            try
            {
                _suspendAutoSave = true;
                LoadFromTxt();
                statusText.Text = "Imported ItemCodex.ini";
            }
            finally
            {
                _suspendAutoSave = false;
            }
        }
        private void btnRebuild_Click(object sender, EventArgs e)
        {
            RebuildFromItems();
            statusText.Text = "Rebuilt from items.";
        }

        // Server I/O
        private void ApplyToServer()
        {
            var list = new List<Envir.ItemCodexCollection>();
            var byId = new Dictionary<int, Envir.ItemCodexCollection>();

            foreach (var r in _collectionsMaster)
            {
                if (r.Id <= 0) continue;
				var encoded = r.Items
					.Where(i => i.ItemIndex > 0)
					.Select(i =>
					{
						int stage = i.Stage;
						if (stage < sbyte.MinValue) stage = sbyte.MinValue;
						if (stage > sbyte.MaxValue) stage = sbyte.MaxValue;
						return CodexRequirement.Encode(i.ItemIndex, (sbyte)stage);
					})
					.Distinct()
					.OrderBy(req => CodexRequirement.DecodeItemIndex(req))
					.ThenBy(req => CodexRequirement.DecodeStage(req))
					.ToList();
				if (encoded.Count == 0) continue;

                var col = new Envir.ItemCodexCollection
                {
                    Id = r.Id,
                    Name = r.Name ?? $"Collection {r.Id}",
					ItemIndices = encoded,
                    Reward = ParseStats(r.RewardText ?? string.Empty),
                    RewardXP = r.RewardXP,
                    Rarity = r.Rarity,
                    Bucket = (byte)r.Bucket,
                    Enabled = r.Enabled,
                    StartTimeUtc = r.StartTimeUtc,
                    EndTimeUtc = r.EndTimeUtc,
                    KeepStatsAfterExpiry = r.KeepStatsAfterExpiry
                };
                list.Add(col);
                byId[col.Id] = col;
            }

            _envir.ItemCodexCollections = list;
            _envir.ItemCodexById = byId;

			// Auto-save to file as well
			try
			{
				var path = Path.Combine(Settings.ConfigPath, "ItemCodex.json");
				_envir.SaveItemCodexToTxt(path);
			}
			catch { }
        }

        private void LoadFromServer()
        {
            _suspendAutoSave = true;
            try
            {
                var path = EditorTxtPath();
                if (File.Exists(path))
                {
                    var rawLines = File.ReadAllLines(path);
                    if (TryParseIni(rawLines, out var iniRows) && iniRows.Count > 0)
                    {
                        _collectionsMaster.Clear();
                        _collectionsView.Clear();

                        foreach (var row in iniRows.OrderBy(r => r.Id))
                            _collectionsMaster.Add(row);

                        ApplyCollectionFilters();
                        if (_collectionsView.Count > 0)
                            dgvCollections.Rows[0].Selected = true;

                        UpdateButtonsEnabled();

                        // push parsed data into the server environment
                        ApplyToServer();

                        return;
                    }
                }

                _collectionsMaster.Clear();
                _collectionsView.Clear();

                // Try loading from server's in-memory data first
                if (_envir?.ItemCodexCollections != null && _envir.ItemCodexCollections.Count > 0)
                {
                    foreach (var c in _envir.ItemCodexCollections.OrderBy(c => c.Id))
                    {
                        var row = new CollectionRow
                        {
                            Id = c.Id,
                            Name = c.Name ?? string.Empty,
                            RewardText = StatsToText(c.Reward),
                            Rarity = c.Rarity,
                            RewardXP = c.RewardXP,
                            Bucket = (CodexBucket)c.Bucket,
                            Enabled = c.Enabled,
                            StartTimeUtc = c.StartTimeUtc,
                            EndTimeUtc = c.EndTimeUtc,
                            KeepStatsAfterExpiry = c.KeepStatsAfterExpiry
                        };

                        foreach (var req in c.ItemIndices.Distinct())
                        {
                            int ix = CodexRequirement.DecodeItemIndex(req);
                            int stage = CodexRequirement.DecodeStage(req);
                            EnsureStageOptionExists(stage);
                            var info = ItemByIndex(ix);
                            row.Items.Add(new ItemRow
                            {
                                ItemIndex = ix,
                                ItemName = info?.Name ?? $"#{ix}",
                                ItemType = info?.Type.ToString() ?? "?",
                                Stage = stage
                            });
                        }
                        _collectionsMaster.Add(row);
                    }
                }
                else
                {
                    // Fallback: load directly from INI file if server data is empty
                    // This ensures we always show saved data even if server hasn't loaded it yet
                    var iniPath = EditorTxtPath();
                    if (File.Exists(iniPath))
                    {
                        LoadFromTxt();
                        // After loading from INI, sync it to the server's in-memory data
                        ApplyToServer();
                    }
                }

                ApplyCollectionFilters();
                if (_collectionsView.Count > 0)
                    dgvCollections.Rows[0].Selected = true;

                UpdateButtonsEnabled();
            }
            finally
            {
                _suspendAutoSave = false;
                AutoSaveIfPossible();
            }
        }

        private void ApplyCollectionFilters()
        {
            var search = (txtSearchCollections.Text ?? string.Empty).Trim();
            CodexBucket? bucketFilter = null;
            if (cbBucketFilter != null && cbBucketFilter.SelectedItem != null && cbBucketFilter.SelectedIndex > 0)
            {
                if (cbBucketFilter.SelectedItem is CodexBucket b) bucketFilter = b;
            }

            var filtered = _collectionsMaster.Where(r =>
                (search.Length == 0 ||
                 (r.Name?.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0) ||
                 r.Id.ToString().Contains(search)) &&
                (!bucketFilter.HasValue || r.Bucket == bucketFilter.Value))
            .OrderBy(r => r.Id)
            .ToList();

            var selectedId = _current?.Id ?? -1;

            _collectionsView.RaiseListChangedEvents = false;
            _collectionsView.Clear();
            foreach (var r in filtered) _collectionsView.Add(r);
            _collectionsView.RaiseListChangedEvents = true;
            _collectionsView.ResetBindings();

            if (selectedId > 0)
            {
                for (int i = 0; i < _collectionsView.Count; i++)
                {
                    if (_collectionsView[i].Id == selectedId)
                    {
                        dgvCollections.ClearSelection();
                        dgvCollections.Rows[i].Selected = true;
                        break;
                    }
                }
            }
            UpdateButtonsEnabled();
        }

        private string _cachedCodexPath;

        private string EditorTxtPath()
        {
            if (!string.IsNullOrEmpty(_cachedCodexPath))
                return _cachedCodexPath;

            // Primary candidate: Configs relative to server settings (JSON)
            try
            {
                var candidate = Path.GetFullPath(Path.Combine(Settings.ConfigPath, "ItemCodex.json"));
                if (File.Exists(candidate))
                {
                    _cachedCodexPath = candidate;
                    return _cachedCodexPath;
                }

                // If directory exists but file missing, still remember this path for saving later.
                if (!File.Exists(candidate) && Directory.Exists(Path.GetDirectoryName(candidate)))
                {
                    _cachedCodexPath = candidate;
                    return _cachedCodexPath;
                }
            }
            catch
            {
                // ignore and fall through to search
            }

            // Search upwards from the executable directory for a Configs\ItemCodex.json (or .ini fallback)
            try
            {
                var baseDir = new DirectoryInfo(AppDomain.CurrentDomain.BaseDirectory);
                DirectoryInfo current = baseDir;
                int depth = 0;
                while (current != null && depth < 6)
                {
                    var probeJson = Path.Combine(current.FullName, "Configs", "ItemCodex.json");
                    var probeIni = Path.Combine(current.FullName, "Configs", "ItemCodex.ini");
                    if (File.Exists(probeJson))
                    {
                        _cachedCodexPath = probeJson;
                        return _cachedCodexPath;
                    }
                    if (File.Exists(probeIni))
                    {
                        _cachedCodexPath = probeIni;
                        return _cachedCodexPath;
                    }
                    current = current.Parent;
                    depth++;
                }
            }
            catch
            {
                // ignore
            }

            // As a last resort, drop alongside the executable
            var fallback = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Configs", "ItemCodex.json");
            Directory.CreateDirectory(Path.GetDirectoryName(fallback));
            _cachedCodexPath = fallback;
            return _cachedCodexPath;
        }

        private static bool ParseEnabledField(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return true;

            raw = raw.Trim();
            if (bool.TryParse(raw, out var flag)) return flag;
            if (int.TryParse(raw, out var num)) return num != 0;

            raw = raw.ToLowerInvariant();
            return raw switch
            {
                "yes" or "y" => true,
                "no" or "n" => false,
                _ => true
            };
        }

        private static DateTime? ParseUtc(Dictionary<string, string> section, string key)
        {
            if (section == null || !section.TryGetValue(key, out var raw) || string.IsNullOrWhiteSpace(raw)) return null;
            if (DateTime.TryParse(raw.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt))
                return dt;
            return null;
        }

        private void SaveToTxt(bool silent = false)
        {
            var path = EditorTxtPath();
            Directory.CreateDirectory(Path.GetDirectoryName(path));

            // guard: collections must have items
            if (_collectionsMaster.Any(c => c.Items == null || c.Items.Count == 0))
            {
                MessageBox.Show("Each collection must have at least one item before saving.", "Save Codex", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            var payload = _collectionsMaster
                .OrderBy(c => c.Id)
                .Select(r => new CollectionDto
                {
                    Id = r.Id,
                    Name = r.Name,
                    Items = r.Items.Select(i => new ItemDto { Index = i.ItemIndex, Stage = (sbyte)i.Stage }).ToList(),
                    Reward = ParseStats(r.RewardText ?? string.Empty)?.Values?.ToDictionary(kv => kv.Key.ToString(), kv => kv.Value) ?? new Dictionary<string, int>(),
                    XP = r.RewardXP,
                    Rarity = r.Rarity.ToString(),
                    Bucket = r.Bucket.ToString(),
                    Enabled = r.Enabled,
                    Start = r.StartTimeUtc.HasValue ? r.StartTimeUtc.Value.ToString("yyyy-MM-dd HH:mm") : null,
                    End = r.EndTimeUtc.HasValue ? r.EndTimeUtc.Value.ToString("yyyy-MM-dd HH:mm") : null,
                    KeepStats = r.KeepStatsAfterExpiry
                })
                .ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };
            var json = JsonSerializer.Serialize(payload, options);
            File.WriteAllText(path, json, System.Text.Encoding.UTF8);

            if (!silent)
                MessageBox.Show($"Saved to:\n{path}", "Save Codex", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void LoadFromTxt()
        {
            var path = EditorTxtPath();
            if (!File.Exists(path))
            {
                MessageBox.Show($"File not found:\n{path}", "Import Codex", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            _suspendAutoSave = true;
            try
            {
                var raw = File.ReadAllText(path);
                var temp = new List<CollectionRow>();

                if (TryParseJson(raw, out var jsonRows))
                {
                    temp.AddRange(jsonRows);
                }
                else
                {
                    var rawLines = raw.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                    if (TryParseIni(rawLines, out var iniRows))
                    {
                        temp.AddRange(iniRows);
                    }
                    else
                    {
                        var legacyLines = ConvertIniToLegacyLines(rawLines) ?? rawLines.ToList();
                        foreach (var legacy in legacyLines)
                        {
                            var row = ParseLegacyLine(legacy);
                            if (row != null) temp.Add(row);
                        }
                    }
                }

                if (temp.Count == 0) return;

                _collectionsMaster.Clear();
                foreach (var r in temp.OrderBy(r => r.Id)) _collectionsMaster.Add(r);

                ApplyCollectionFilters();
                if (_collectionsView.Count > 0)
                    dgvCollections.Rows[0].Selected = true;

                UpdateButtonsEnabled();
            }
            finally
            {
                _suspendAutoSave = false;
                AutoSaveIfPossible();
            }
        }

        private static DateTime? ParseLocalDate(string raw)
        {
            if (string.IsNullOrWhiteSpace(raw)) return null;
            if (DateTime.TryParse(raw.Trim(), CultureInfo.InvariantCulture, DateTimeStyles.AssumeLocal, out var dt))
                return dt;
            return null;
        }

        private static string RewardDictToText(Dictionary<string, int> reward)
        {
            if (reward == null || reward.Count == 0) return string.Empty;
            return string.Join(",", reward.Select(kv => $"{kv.Key}={kv.Value}"));
        }

        private static TEnum ParseEnumString<TEnum>(string raw, TEnum fallback) where TEnum : struct
        {
            if (!string.IsNullOrWhiteSpace(raw) && Enum.TryParse<TEnum>(raw.Trim(), true, out var val))
                return val;
            return fallback;
        }

        private bool TryParseJson(string raw, out List<CollectionRow> rows)
        {
            rows = null;
            if (string.IsNullOrWhiteSpace(raw)) return false;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var dtoList = JsonSerializer.Deserialize<List<CollectionDto>>(raw, options);
                if (dtoList == null || dtoList.Count == 0) return false;

                var result = new List<CollectionRow>();
                foreach (var dto in dtoList)
                {
                    if (dto == null || dto.Id <= 0) continue;
                    var row = new CollectionRow
                    {
                        Id = dto.Id,
                        Name = dto.Name ?? string.Empty,
                        RewardText = RewardDictToText(dto.Reward),
                        RewardXP = dto.XP,
                        Rarity = ParseEnumString(dto.Rarity, ItemGrade.None),
                        Bucket = ParseEnumString(dto.Bucket, CodexBucket.Character),
                        Enabled = dto.Enabled,
                        StartTimeUtc = ParseLocalDate(dto.Start),
                        EndTimeUtc = ParseLocalDate(dto.End),
                        KeepStatsAfterExpiry = dto.KeepStats
                    };

                    if (dto.Items != null)
                    {
                        foreach (var it in dto.Items)
                        {
                            if (it == null || it.Index <= 0) continue;
                            if (!ItemExists(it.Index)) continue;
                            int stage = it.Stage;
                            if (stage < sbyte.MinValue) stage = sbyte.MinValue;
                            if (stage > sbyte.MaxValue) stage = sbyte.MaxValue;
                            var info = ItemByIndex(it.Index);
                            row.Items.Add(new ItemRow
                            {
                                ItemIndex = it.Index,
                                ItemName = info?.Name ?? $"#{it.Index}",
                                ItemType = info?.Type.ToString() ?? "?",
                                Stage = stage
                            });
                        }
                    }

                    if (row.Items.Count == 0) continue;
                    result.Add(row);
                }

                if (result.Count == 0) return false;
                rows = result;
                return true;
            }
            catch
            {
                rows = null;
                return false;
            }
        }

        private bool TryParseIni(string[] lines, out List<CollectionRow> rows)
        {
            rows = null;
            if (lines == null || lines.Length == 0) return false;

            var result = new List<CollectionRow>();
            Dictionary<string, string> section = null;
            bool sawCollection = false;

            void FinalizeSection()
            {
                if (section == null) return;
                if (!section.TryGetValue("Id", out var idText) || !int.TryParse(idText.Trim(), out int id) || id <= 0)
                {
                    section = null;
                    return;
                }

                var row = new CollectionRow
                {
                    Id = id,
                    Name = section.TryGetValue("Name", out var nameText) ? (nameText ?? string.Empty) : string.Empty,
                    RewardText = section.TryGetValue("Reward", out var rewardText) ? (rewardText ?? string.Empty) : string.Empty,
                    RewardXP = ParseInt(section, "XP", 0),
                    Rarity = ParseEnum(section, "Rarity", ItemGrade.None),
                    Bucket = ParseEnum(section, "Bucket", CodexBucket.Character),
                    Enabled = section.TryGetValue("Enabled", out var enabledText) ? ParseEnabledField(enabledText) : true,
                    StartTimeUtc = ParseUtc(section, "StartUtc"),
                    EndTimeUtc = ParseUtc(section, "EndUtc"),
                    KeepStatsAfterExpiry = section.TryGetValue("KeepStats", out var keepText) ? ParseEnabledField(keepText) : false
                };

                var items = new List<(int index, int stage)>();
                if (section.TryGetValue("Items", out var itemsText) && !string.IsNullOrWhiteSpace(itemsText))
                {
                    foreach (var token in itemsText.Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
                    {
                        var t = token.Trim();
                        if (t.Length == 0) continue;
                        int ix, stage = CodexRequirement.AnyStage;
                        int at = t.IndexOf('@');
                        if (at >= 0)
                        {
                            var left = t.Substring(0, at).Trim();
                            var right = t.Substring(at + 1).Trim();
                            if (!int.TryParse(left, NumberStyles.Integer, CultureInfo.InvariantCulture, out ix)) continue;
                            if (!int.TryParse(right, NumberStyles.Integer, CultureInfo.InvariantCulture, out stage)) continue;
                        }
                        else
                        {
                            if (!int.TryParse(t, NumberStyles.Integer, CultureInfo.InvariantCulture, out ix)) continue;
                        }
                        if (ix > 0) items.Add((ix, stage));
                    }
                }

                var filtered = items
                    .Where(p => ItemExists(p.index))
                    .Distinct()
                    .OrderBy(p => p.index)
                    .ThenBy(p => p.stage)
                    .ToList();

                if (filtered.Count == 0)
                {
                    section = null;
                    return;
                }

                foreach (var pair in filtered)
                {
                    EnsureStageOptionExists(pair.stage);
                    var info = ItemByIndex(pair.index);
                    row.Items.Add(new ItemRow
                    {
                        ItemIndex = pair.index,
                        ItemName = info?.Name ?? $"#{pair.index}",
                        ItemType = info?.Type.ToString() ?? "?",
                        Stage = pair.stage
                    });
                }

                result.Add(row);
                section = null;
            }

            foreach (var raw in lines)
            {
                var line = raw.Trim();
                if (line.Length == 0 || line.StartsWith("#") || line.StartsWith(";")) continue;

                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    FinalizeSection();
                    var header = line.Substring(1, line.Length - 2).Trim();
                    if (header.StartsWith("Collection", StringComparison.OrdinalIgnoreCase))
                    {
                        section = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                        sawCollection = true;
                    }
                    else
                    {
                        section = null;
                    }
                    continue;
                }

                if (section == null) continue;

                int eq = line.IndexOf('=');
                if (eq < 0) continue;

                var key = line.Substring(0, eq).Trim();
                var value = line.Substring(eq + 1).Trim();
                section[key] = value;
            }

            FinalizeSection();

            if (!sawCollection) return false;

            rows = result;
            return true;
        }

        private static int ParseInt(Dictionary<string, string> section, string key, int defaultValue)
        {
            if (section.TryGetValue(key, out var text) && !string.IsNullOrWhiteSpace(text))
            {
                if (int.TryParse(text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var value))
                    return Math.Max(0, value);
            }
            return defaultValue;
        }

        private static T ParseEnum<T>(Dictionary<string, string> section, string key, T defaultValue) where T : struct
        {
            if (section.TryGetValue(key, out var text) && !string.IsNullOrWhiteSpace(text))
            {
                if (Enum.TryParse(text.Trim(), true, out T parsed))
                    return parsed;

                if (byte.TryParse(text.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out var numeric))
                    return (T)Enum.ToObject(typeof(T), numeric);
            }
            return defaultValue;
        }

        private CollectionRow ParseLegacyLine(string raw)
        {
            var line = raw.Trim();
            if (line.Length == 0 || line.StartsWith("#")) return null;

            var parts = line.Split('|');
            if (parts.Length < 3) return null;

            if (!int.TryParse(parts[0].Trim(), out int id) || id <= 0) return null;

            string name = parts[1].Trim();

            var items = new List<(int index, int stage)>();
            foreach (var tok in parts[2].Split(new[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries))
            {
                var t = tok.Trim();
                if (t.Length == 0) continue;
                int ix, stage = CodexRequirement.AnyStage;
                int at = t.IndexOf('@');
                if (at >= 0)
                {
                    var left = t.Substring(0, at).Trim();
                    var right = t.Substring(at + 1).Trim();
                    if (!int.TryParse(left, NumberStyles.Integer, CultureInfo.InvariantCulture, out ix)) continue;
                    if (!int.TryParse(right, NumberStyles.Integer, CultureInfo.InvariantCulture, out stage)) continue;
                }
                else
                {
                    if (!int.TryParse(t, NumberStyles.Integer, CultureInfo.InvariantCulture, out ix)) continue;
                }
                if (ix > 0) items.Add((ix, stage));
            }

            var filtered = items
                .Where(p => ItemExists(p.index))
                .Distinct()
                .OrderBy(p => p.index)
                .ThenBy(p => p.stage)
                .ToList();

            if (filtered.Count == 0) return null;

            string rewardText = parts.Length >= 4 ? parts[3].Trim() : string.Empty;

            int xp = 0;
            if (parts.Length >= 5 && !int.TryParse(parts[4].Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out xp))
                xp = 0;
            if (xp < 0) xp = 0;

            ItemGrade rarity = ItemGrade.None;
            if (parts.Length >= 6)
            {
                var rtxt = parts[5].Trim();
                if (!string.IsNullOrEmpty(rtxt))
                {
                    if (!Enum.TryParse(rtxt, true, out rarity))
                        if (byte.TryParse(rtxt, NumberStyles.Integer, CultureInfo.InvariantCulture, out var b)) rarity = (ItemGrade)b;
                }
            }

            CodexBucket bucket = CodexBucket.Character;
            if (parts.Length >= 7)
            {
                var btxt = parts[6].Trim();
                if (!string.IsNullOrEmpty(btxt))
                {
                    if (!Enum.TryParse(btxt, true, out bucket))
                        if (byte.TryParse(btxt, NumberStyles.Integer, CultureInfo.InvariantCulture, out var bval)) bucket = (CodexBucket)bval;
                }
            }

            bool enabled = true;
            if (parts.Length >= 8)
                enabled = ParseEnabledField(parts[7]);

            var row = new CollectionRow
            {
                Id = id,
                Name = name,
                RewardText = rewardText,
                RewardXP = Math.Max(0, xp),
                Rarity = rarity,
                Bucket = bucket,
                Enabled = enabled
            };

            foreach (var pair in filtered)
            {
                EnsureStageOptionExists(pair.stage);
                var info = ItemByIndex(pair.index);
                row.Items.Add(new ItemRow
                {
                    ItemIndex = pair.index,
                    ItemName = info?.Name ?? $"#{pair.index}",
                    ItemType = info?.Type.ToString() ?? "?",
                    Stage = pair.stage
                });
            }

            return row;
        }

		private static List<string> ConvertIniToLegacyLines(IEnumerable<string> lines)
		{
			if (lines == null) return null;

			var result = new List<string>();
			Dictionary<string, string> current = null;
			string currentSection = null;
			bool sawCollection = false;

			void FinalizeSection()
			{
				if (current == null) return;
				if (!current.TryGetValue("Id", out var idText) || !int.TryParse(idText.Trim(), out _)) { current = null; return; }
				if (!current.TryGetValue("Items", out var itemsText) || string.IsNullOrWhiteSpace(itemsText)) { current = null; return; }

				current.TryGetValue("Name", out var nameText);
				current.TryGetValue("Reward", out var rewardText);
				current.TryGetValue("XP", out var xpText);
				current.TryGetValue("Rarity", out var rarityText);
				current.TryGetValue("Bucket", out var bucketText);
				current.TryGetValue("Enabled", out var enabledText);

				string legacyLine = string.Join("|", new[]
				{
					idText.Trim(),
					(nameText ?? string.Empty).Trim(),
					itemsText.Trim(),
					(rewardText ?? string.Empty).Trim(),
					string.IsNullOrWhiteSpace(xpText) ? "0" : xpText.Trim(),
					string.IsNullOrWhiteSpace(rarityText) ? ItemGrade.None.ToString() : rarityText.Trim(),
					string.IsNullOrWhiteSpace(bucketText) ? CodexBucket.Character.ToString() : bucketText.Trim(),
					string.IsNullOrWhiteSpace(enabledText) ? "True" : enabledText.Trim()
				});

				result.Add(legacyLine);
				current = null;
			}

			foreach (var raw in lines)
			{
				var line = raw.Trim();
				if (line.Length == 0 || line.StartsWith("#") || line.StartsWith(";")) continue;

				if (line.StartsWith("[") && line.EndsWith("]"))
				{
					FinalizeSection();
					currentSection = line.Substring(1, line.Length - 2).Trim();
					if (currentSection.StartsWith("Collection", StringComparison.OrdinalIgnoreCase))
					{
						current = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
						sawCollection = true;
					}
					else
					{
						current = null;
					}
					continue;
				}

				if (current == null) continue;

				int eq = line.IndexOf('=');
				if (eq < 0) continue;

				string key = line.Substring(0, eq).Trim();
				string value = line.Substring(eq + 1).Trim();

				current[key] = value;
			}

			FinalizeSection();

			return sawCollection ? result : null;
		}

        private void RebuildFromItems()
        {
            if (_envir == null) return;

            var candidates = _envir.ItemInfoList.Where(i =>
                i != null && (i.Type == ItemType.Necklace || i.Type == ItemType.Bracelet || i.Type == ItemType.Ring));

            string Key(string raw)
            {
                if (string.IsNullOrWhiteSpace(raw)) return string.Empty;
                string t = _reBrackets.Replace(raw, "");
                t = _reParens.Replace(t, "");
                t = _reTrailing.Replace(t, "");
                return t.Trim();
            }

            var groups = candidates
                .GroupBy(i => new { Base = Key(i.Name), i.Type })
                .Where(g => g.Count() >= 2)
                .OrderBy(g => g.Key.Base);

            int nextId = (_collectionsMaster.Count == 0) ? 1 : _collectionsMaster.Max(c => c.Id) + 1;
            var temp = new List<CollectionRow>();
            foreach (var g in groups)
            {
                var items = g.Select(ii => ii.Index).Distinct().OrderBy(ix => ix).ToList();
                if (items.Count < 2) continue;

                string name = $"{g.Key.Type} Collection: {g.Key.Base}";
                var row = new CollectionRow
                {
                    Id = nextId++,
                    Name = name,
                    RewardText = $"MaxDC={Math.Min(3, items.Count)}",
                    Bucket = CodexBucket.Character,
                    Enabled = true
                };
                foreach (var ix in items)
                {
                    var info = ItemByIndex(ix);
                    row.Items.Add(new ItemRow { ItemIndex = ix, ItemName = info?.Name ?? $"#{ix}", ItemType = info?.Type.ToString() ?? "?" });
                }
                temp.Add(row);
            }

            _collectionsMaster.Clear();
            foreach (var r in temp) _collectionsMaster.Add(r);

            ApplyCollectionFilters();
            if (_collectionsView.Count > 0)
                dgvCollections.Rows[0].Selected = true;

            UpdateButtonsEnabled();
        }

        private void ItemCodexEditorForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.Control && e.KeyCode == Keys.S) { btnSaveTxt_Click(sender, e); e.Handled = true; }
            if (e.Control && e.KeyCode == Keys.L) { btnLoadTxt_Click(sender, e); e.Handled = true; }
            if (e.Control && e.KeyCode == Keys.R) { btnRebuild_Click(sender, e); e.Handled = true; }
            if (e.Control && e.KeyCode == Keys.A) { btnApply_Click(sender, e); e.Handled = true; }
        }

		// explicit save from toolbar
		private void btnSave_Click(object sender, EventArgs e)
		{
			SyncRewardsToCurrent();
			SaveToTxt(silent: false);
			statusText.Text = "Saved ItemCodex.ini";
		}
    }
}
