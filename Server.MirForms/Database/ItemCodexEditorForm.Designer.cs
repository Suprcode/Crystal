// Server/MirForms/ItemCodexEditorForm.Designer.cs
using System;
using System.Drawing;
using System.Windows.Forms;
using Shared; // ItemGrade, CodexBucket, Stat

namespace Server.MirForms
{
    partial class ItemCodexEditorForm
    {
        private System.ComponentModel.IContainer components = null;

        // Top strip
        private ToolStrip tsMain;
		private ToolStripButton tsbLoadTxt, tsbSaveTxt, tsbApply, tsbRebuild, tsbSave;

        // Main split: Left (Collections) | Right (Selected + Tabs)
        private SplitContainer splitMain;

        // LEFT: Collections
        private TableLayoutPanel leftLayout;
        private TextBox txtSearchCollections;
        private ComboBox cbBucketFilter;
        private FlowLayoutPanel leftButtons;
        private Button btnAddCollection, btnRemoveCollection, btnDuplicateCollection;
        private DataGridView dgvCollections;
        private DataGridViewTextBoxColumn colId, colName, colCount, colXP;
        private DataGridViewComboBoxColumn colBucket, colRarity;
        private DataGridViewCheckBoxColumn colEnabled;

        // RIGHT split: Top (Selected Items) | Bottom (Tabs)
        private SplitContainer splitRight;

        // Selected items area
        private TableLayoutPanel selectedLayout;
        private FlowLayoutPanel selectedHeader;
        private Label lblAddIndex;
        private TextBox txtAddIndex;
        private Button btnAddItem, btnRemoveItem, btnSortItems;
        private Label lblStageQuick;
        private ComboBox cbStageQuick;
        private Button btnApplyStage;
        private DataGridView dgvItems;
		private DataGridViewTextBoxColumn colItemIndex, colItemName, colItemType;
		private DataGridViewComboBoxColumn colItemStage;

        // Tabs
        private TabControl tabs;
        private TabPage tabOverview, tabRewards, tabAvailable;

        // Overview tab
        private TableLayoutPanel overviewLayout;
        private Label lblId, lblName, lblBucket, lblRarity, lblXP, lblStartUtc, lblEndUtc;
        private TextBox txtName;
        private ComboBox cbBucketDetail, cbRarityDetail;
        private NumericUpDown nudXP;
        private DateTimePicker dtpStartUtc, dtpEndUtc;
        private CheckBox chkEnabled, chkKeepStats;

        // Rewards tab
        private TableLayoutPanel rewardsLayout;
        private DataGridView dgvRewards;
        private DataGridViewComboBoxColumn colRewardStat;
        private DataGridViewTextBoxColumn colRewardValue;
        private FlowLayoutPanel rewardsButtons;
        private Button btnAddReward, btnRemoveReward;

        // Available tab
        private TableLayoutPanel availLayout;
        private FlowLayoutPanel availHeader;
        private Label lblTypeFilter, lblSearchAvail;
        private ComboBox cbTypeFilter;
        private TextBox txtSearchAvail;
        private Button btnAddSelectedAvail;
        private DataGridView dgvAvailable;
        private DataGridViewTextBoxColumn colAvailIndex, colAvailName, colAvailType;

        // Status
        private StatusStrip statusBar;
        private ToolStripStatusLabel statusText;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
                components.Dispose();
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();

            SuspendLayout();
            Text = "Item Codex Editor";
            StartPosition = FormStartPosition.CenterScreen;
            MinimumSize = new Size(1200, 800);
            ClientSize = new Size(1400, 900);
            AutoScaleMode = AutoScaleMode.Font;
            KeyPreview = true;

            // ───────── ToolStrip ─────────
            tsMain = new ToolStrip { GripStyle = ToolStripGripStyle.Hidden, Dock = DockStyle.Top, ImageScalingSize = new Size(20, 20) };
			tsbLoadTxt = new ToolStripButton("Import") { Visible = false };
			tsbSave = new ToolStripButton("Save");
			tsbSaveTxt = new ToolStripButton("Export") { Visible = false };
            tsbApply = new ToolStripButton("Apply to Server");
            tsbRebuild = new ToolStripButton("Rebuild (Auto)");
            tsbLoadTxt.Click += btnLoadTxt_Click;
			tsbSave.Click += btnSave_Click;
            tsbSaveTxt.Click += btnSaveTxt_Click;
            tsbApply.Click += btnApply_Click;
            tsbRebuild.Click += btnRebuild_Click;
			tsMain.Items.Add(tsbSave);
            tsMain.Items.Add(new ToolStripSeparator());
            tsMain.Items.Add(tsbApply);
            tsMain.Items.Add(tsbRebuild);

            // ───────── Status ─────────
            statusBar = new StatusStrip();
            statusText = new ToolStripStatusLabel { Text = "Ready" };
            statusBar.Items.Add(statusText);
            statusBar.Dock = DockStyle.Bottom;

            // ───────── Main Split (Left | Right) ─────────
            splitMain = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Vertical,
                SplitterWidth = 6
            };
            // NOTE: No Panel1MinSize/Panel2MinSize/ SplitterDistance here (set later at runtime).

            // LEFT layout (search + buttons + grid)
            leftLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 4
            };
            leftLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // search
            leftLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // bucket filter
            leftLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));       // buttons
            leftLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));  // grid

            txtSearchCollections = new TextBox
            {
                Dock = DockStyle.Top,
                PlaceholderText = "Search collections (id/name)…",
                Margin = new Padding(6)
            };
            txtSearchCollections.TextChanged += txtSearchCollections_TextChanged;

            cbBucketFilter = new ComboBox
            {
                Dock = DockStyle.Top,
                DropDownStyle = ComboBoxStyle.DropDownList,
                Margin = new Padding(6)
            };

            leftButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Margin = new Padding(6)
            };
            btnAddCollection = new Button { Text = "Add", AutoSize = true };
            btnRemoveCollection = new Button { Text = "Delete", AutoSize = true };
            btnDuplicateCollection = new Button { Text = "Duplicate", AutoSize = true };
            btnAddCollection.Click += btnAddCollection_Click;
            btnRemoveCollection.Click += btnRemoveCollection_Click;
            btnDuplicateCollection.Click += btnDuplicateCollection_Click;
            leftButtons.Controls.AddRange(new System.Windows.Forms.Control[] { btnAddCollection, btnRemoveCollection, btnDuplicateCollection });

            dgvCollections = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                MultiSelect = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.None, // fixed widths
                ScrollBars = ScrollBars.Both                                 // horizontal scroll
            };
            colId = new DataGridViewTextBoxColumn { HeaderText = "ID", DataPropertyName = "Id", Width = 50 };
            colName = new DataGridViewTextBoxColumn { HeaderText = "Name", DataPropertyName = "Name", Width = 160 };
            colBucket = new DataGridViewComboBoxColumn
            {
                HeaderText = "Category",
                DataPropertyName = "Bucket",
                DataSource = Enum.GetValues(typeof(CodexBucket)),
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                FlatStyle = FlatStyle.Standard,
                Width = 100
            };
            colRarity = new DataGridViewComboBoxColumn
            {
                HeaderText = "Rarity",
                DataPropertyName = "Rarity",
                DataSource = Enum.GetValues(typeof(ItemGrade)),
                DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton,
                FlatStyle = FlatStyle.Standard,
                Width = 90
            };
            colEnabled = new DataGridViewCheckBoxColumn { HeaderText = "Enabled", DataPropertyName = "Enabled", Width = 70 };
            colCount = new DataGridViewTextBoxColumn { HeaderText = "Items", DataPropertyName = "RequiredCount", ReadOnly = true, Width = 60 };
            colXP = new DataGridViewTextBoxColumn { HeaderText = "XP", DataPropertyName = "RewardXP", Width = 70 };
            dgvCollections.Columns.AddRange(new DataGridViewColumn[] { colId, colName, colBucket, colRarity, colEnabled, colCount, colXP });
            dgvCollections.SelectionChanged += dgvCollections_SelectionChanged;

            leftLayout.Controls.Add(txtSearchCollections, 0, 0);
            leftLayout.Controls.Add(cbBucketFilter, 0, 1);
            leftLayout.Controls.Add(leftButtons, 0, 2);
            leftLayout.Controls.Add(dgvCollections, 0, 3);

            splitMain.Panel1.Controls.Add(leftLayout);

            // ───────── RIGHT split (Selected | Tabs) ─────────
            splitRight = new SplitContainer
            {
                Dock = DockStyle.Fill,
                Orientation = Orientation.Horizontal,
                SplitterWidth = 6
            };
            // NOTE: No min sizes or distances here either.

            // Selected items layout
            selectedLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            selectedLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            selectedLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            selectedHeader = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(6)
            };
            lblAddIndex = new Label { Text = "Index:", AutoSize = true, Margin = new Padding(6, 10, 6, 6) };
            txtAddIndex = new TextBox { Width = 120 };
            btnAddItem = new Button { Text = "Add by Index", AutoSize = true };
            btnRemoveItem = new Button { Text = "Remove Selected", AutoSize = true };
            btnSortItems = new Button { Text = "Sort by Index", AutoSize = true };
            lblStageQuick = new Label { Text = "Stage:", AutoSize = true, Margin = new Padding(12, 10, 6, 6) };
            cbStageQuick = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 110, Margin = new Padding(0, 6, 0, 6) };
            btnApplyStage = new Button { Text = "Apply Stage", AutoSize = true };
            btnAddItem.Click += btnAddItem_Click;
            btnRemoveItem.Click += btnRemoveItem_Click;
            btnSortItems.Click += btnSortItems_Click;
            btnApplyStage.Click += btnApplyStage_Click;
            selectedHeader.Controls.AddRange(new System.Windows.Forms.Control[] { lblAddIndex, txtAddIndex, btnAddItem, btnRemoveItem, btnSortItems, lblStageQuick, cbStageQuick, btnApplyStage });

            dgvItems = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
			colItemIndex = new DataGridViewTextBoxColumn { HeaderText = "ItemIndex", FillWeight = 18 };
			colItemName = new DataGridViewTextBoxColumn { HeaderText = "Item Name", FillWeight = 52 };
			colItemType = new DataGridViewTextBoxColumn { HeaderText = "Type", FillWeight = 15 };
			colItemStage = new DataGridViewComboBoxColumn { HeaderText = "Stage", FillWeight = 15, FlatStyle = FlatStyle.Standard, DisplayStyle = DataGridViewComboBoxDisplayStyle.DropDownButton };
			colItemIndex.DataPropertyName = "ItemIndex";
			colItemName.DataPropertyName = "ItemName";
			colItemType.DataPropertyName = "ItemType";
			colItemStage.DataPropertyName = "Stage";
			dgvItems.Columns.AddRange(new DataGridViewColumn[] { colItemIndex, colItemName, colItemType, colItemStage });
			dgvItems.CellValueChanged += dgvItems_CellValueChanged;
			dgvItems.CurrentCellDirtyStateChanged += dgvItems_CurrentCellDirtyStateChanged;
			dgvItems.DataError += dgvItems_DataError;

            selectedLayout.Controls.Add(selectedHeader, 0, 0);
            selectedLayout.Controls.Add(dgvItems, 0, 1);

            splitRight.Panel1.Controls.Add(selectedLayout);

            // Tabs
            tabs = new TabControl { Dock = DockStyle.Fill };
            tabOverview = new TabPage("Overview");
            tabRewards = new TabPage("Rewards");
            tabAvailable = new TabPage("Available");

            // Overview layout
            overviewLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Top,
                ColumnCount = 4,
                AutoSize = true,
                Padding = new Padding(10)
            };
            overviewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            overviewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));
            overviewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.AutoSize));
            overviewLayout.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50f));

            lblId = new Label { Text = "ID: -", AutoSize = true, Margin = new Padding(0, 4, 16, 8) };
            lblName = new Label { Text = "Name:", AutoSize = true };
            txtName = new TextBox { Dock = DockStyle.Fill, Margin = new Padding(6) };

            lblBucket = new Label { Text = "Category:", AutoSize = true };
            cbBucketDetail = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill, Margin = new Padding(6) };

            lblRarity = new Label { Text = "Rarity:", AutoSize = true };
            cbRarityDetail = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Dock = DockStyle.Fill, Margin = new Padding(6) };

            lblXP = new Label { Text = "Codex XP:", AutoSize = true };
            nudXP = new NumericUpDown { Maximum = 1000000, Minimum = 0, Increment = 10, Dock = DockStyle.Left, Width = 120, Margin = new Padding(6) };

            lblStartUtc = new Label { Text = "Start (UTC):", AutoSize = true };
            lblEndUtc = new Label { Text = "End (UTC):", AutoSize = true };
            dtpStartUtc = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm",
                ShowCheckBox = true,
                Checked = false,
                Dock = DockStyle.Left,
                Width = 170,
                Margin = new Padding(6)
            };
            dtpEndUtc = new DateTimePicker
            {
                Format = DateTimePickerFormat.Custom,
                CustomFormat = "yyyy-MM-dd HH:mm",
                ShowCheckBox = true,
                Checked = false,
                Dock = DockStyle.Left,
                Width = 170,
                Margin = new Padding(6)
            };

            chkEnabled = new CheckBox { Text = "Enabled", AutoSize = true, Margin = new Padding(6, 8, 6, 8) };
            chkKeepStats = new CheckBox { Text = "Keep stats after expiry", AutoSize = true, Margin = new Padding(6, 8, 6, 8) };

            var topRow = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, Padding = new Padding(10, 8, 10, 0) };
            topRow.Controls.Add(lblId);

            overviewLayout.Controls.Add(lblName, 0, 0);
            overviewLayout.Controls.Add(txtName, 1, 0);
            overviewLayout.Controls.Add(lblBucket, 2, 0);
            overviewLayout.Controls.Add(cbBucketDetail, 3, 0);

            overviewLayout.Controls.Add(lblRarity, 0, 1);
            overviewLayout.Controls.Add(cbRarityDetail, 1, 1);
            overviewLayout.Controls.Add(lblXP, 2, 1);
            overviewLayout.Controls.Add(nudXP, 3, 1);

            overviewLayout.Controls.Add(lblStartUtc, 0, 2);
            overviewLayout.Controls.Add(dtpStartUtc, 1, 2);
            overviewLayout.Controls.Add(lblEndUtc, 2, 2);
            overviewLayout.Controls.Add(dtpEndUtc, 3, 2);

            overviewLayout.Controls.Add(chkEnabled, 0, 3);
            overviewLayout.SetColumnSpan(chkEnabled, 2);
            overviewLayout.Controls.Add(chkKeepStats, 2, 3);
            overviewLayout.SetColumnSpan(chkKeepStats, 2);

            var panelOverview = new Panel { Dock = DockStyle.Fill, AutoScroll = true };
            panelOverview.Controls.Add(overviewLayout);
            panelOverview.Controls.Add(topRow);
            tabOverview.Controls.Add(panelOverview);

            // Rewards layout
            rewardsLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            rewardsLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));
            rewardsLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));

            dgvRewards = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = true,
                AllowUserToDeleteRows = true,
                ReadOnly = false,
                MultiSelect = false,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            colRewardStat = new DataGridViewComboBoxColumn { HeaderText = "Stat", DataPropertyName = "Stat", DataSource = Enum.GetValues(typeof(Stat)), FillWeight = 70 };
            colRewardValue = new DataGridViewTextBoxColumn { HeaderText = "Value", DataPropertyName = "Value", FillWeight = 30 };
            dgvRewards.Columns.AddRange(new DataGridViewColumn[] { colRewardStat, colRewardValue });

            rewardsButtons = new FlowLayoutPanel
            {
                Dock = DockStyle.Bottom,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(10)
            };
            btnAddReward = new Button { Text = "Add Stat", AutoSize = true };
            btnRemoveReward = new Button { Text = "Remove", AutoSize = true };
            btnAddReward.Click += btnAddReward_Click;
            btnRemoveReward.Click += btnRemoveReward_Click;
            rewardsButtons.Controls.AddRange(new System.Windows.Forms.Control[] { btnAddReward, btnRemoveReward });

            rewardsLayout.Controls.Add(dgvRewards, 0, 0);
            rewardsLayout.Controls.Add(rewardsButtons, 0, 1);
            tabRewards.Controls.Add(rewardsLayout);

            // Available layout
            availLayout = new TableLayoutPanel
            {
                Dock = DockStyle.Fill,
                ColumnCount = 1,
                RowCount = 2
            };
            availLayout.RowStyles.Add(new RowStyle(SizeType.AutoSize));
            availLayout.RowStyles.Add(new RowStyle(SizeType.Percent, 100f));

            availHeader = new FlowLayoutPanel
            {
                Dock = DockStyle.Top,
                AutoSize = true,
                FlowDirection = FlowDirection.LeftToRight,
                WrapContents = false,
                Padding = new Padding(10, 8, 10, 4)
            };
            lblTypeFilter = new Label { Text = "Type:", AutoSize = true, Margin = new Padding(0, 6, 6, 0) };
            cbTypeFilter = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList, Width = 160 };
            cbTypeFilter.SelectedIndexChanged += cbTypeFilter_SelectedIndexChanged;

            lblSearchAvail = new Label { Text = "Search:", AutoSize = true, Margin = new Padding(12, 6, 6, 0) };
            txtSearchAvail = new TextBox { Width = 300 };
            txtSearchAvail.TextChanged += txtSearchAvail_TextChanged;

            btnAddSelectedAvail = new Button { Text = "Add Selected →", AutoSize = true, Margin = new Padding(12, 3, 0, 3) };
            btnAddSelectedAvail.Click += btnAddSelectedAvail_Click;

            availHeader.Controls.AddRange(new System.Windows.Forms.Control[] { lblTypeFilter, cbTypeFilter, lblSearchAvail, txtSearchAvail, btnAddSelectedAvail });

            dgvAvailable = new DataGridView
            {
                Dock = DockStyle.Fill,
                AllowUserToAddRows = false,
                AllowUserToDeleteRows = false,
                ReadOnly = true,
                MultiSelect = true,
                SelectionMode = DataGridViewSelectionMode.FullRowSelect,
                RowHeadersVisible = false,
                AutoGenerateColumns = false,
                AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
            };
            colAvailIndex = new DataGridViewTextBoxColumn { HeaderText = "Index", DataPropertyName = "Index", FillWeight = 15 };
            colAvailName = new DataGridViewTextBoxColumn { HeaderText = "Name", DataPropertyName = "Name", FillWeight = 65 };
            colAvailType = new DataGridViewTextBoxColumn { HeaderText = "Type", DataPropertyName = "Type", FillWeight = 20 };
            dgvAvailable.Columns.AddRange(new DataGridViewColumn[] { colAvailIndex, colAvailName, colAvailType });
            dgvAvailable.CellDoubleClick += dgvAvailable_CellDoubleClick;

            availLayout.Controls.Add(availHeader, 0, 0);
            availLayout.Controls.Add(dgvAvailable, 0, 1);
            tabAvailable.Controls.Add(availLayout);

            tabs.TabPages.AddRange(new TabPage[] { tabOverview, tabRewards, tabAvailable });

            splitRight.Panel2.Controls.Add(tabs);
            splitMain.Panel2.Controls.Add(splitRight);

            Controls.Add(splitMain);
            Controls.Add(tsMain);
            Controls.Add(statusBar);

            ResumeLayout(false);
            PerformLayout();
        }

        // wrapper for lambda-free event hook
        private void dgvCollections_SelectionChanged(object sender, EventArgs e) => OnCollectionSelectionChanged();
    }
}
