
namespace Server.Database
{
    partial class ItemInfoFormNew
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.itemInfoGridView = new System.Windows.Forms.DataGridView();
            this.Modified = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ItemIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemGrade = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemRequiredType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemRequiredGender = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemRequiredClass = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemSet = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemRandomStatsId = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemRequiredAmount = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemImage = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemShape = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemEffect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemStackSize = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemSlots = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemWeight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemLightRange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemLightIntensity = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemDurability = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemPrice = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemToolTip = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.NeedIdentify = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ShowGroupPickup = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.GlobalDropNotify = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.ClassBased = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.LevelBased = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CanMine = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CanFastRun = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.CanAwakening = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.lblFilterType = new System.Windows.Forms.Label();
            this.groupView = new System.Windows.Forms.GroupBox();
            this.rBtnViewSpecial = new System.Windows.Forms.RadioButton();
            this.rbtnViewAll = new System.Windows.Forms.RadioButton();
            this.rbtnViewBinding = new System.Windows.Forms.RadioButton();
            this.rbtnViewBasic = new System.Windows.Forms.RadioButton();
            this.rbtnViewStats = new System.Windows.Forms.RadioButton();
            this.drpFilterType = new System.Windows.Forms.ComboBox();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Gameshop_button = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.itemInfoGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupView.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // itemInfoGridView
            // 
            dataGridViewCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.itemInfoGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle4;
            this.itemInfoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.itemInfoGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Modified,
            this.ItemIndex,
            this.ItemName,
            this.ItemType,
            this.ItemGrade,
            this.ItemRequiredType,
            this.ItemRequiredGender,
            this.ItemRequiredClass,
            this.ItemSet,
            this.ItemRandomStatsId,
            this.ItemRequiredAmount,
            this.ItemImage,
            this.ItemShape,
            this.ItemEffect,
            this.ItemStackSize,
            this.ItemSlots,
            this.ItemWeight,
            this.ItemLightRange,
            this.ItemLightIntensity,
            this.ItemDurability,
            this.ItemPrice,
            this.ItemToolTip,
            this.NeedIdentify,
            this.ShowGroupPickup,
            this.GlobalDropNotify,
            this.ClassBased,
            this.LevelBased,
            this.CanMine,
            this.CanFastRun,
            this.CanAwakening});
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.itemInfoGridView.DefaultCellStyle = dataGridViewCellStyle6;
            this.itemInfoGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemInfoGridView.Location = new System.Drawing.Point(0, 0);
            this.itemInfoGridView.Name = "itemInfoGridView";
            this.itemInfoGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.itemInfoGridView.Size = new System.Drawing.Size(956, 433);
            this.itemInfoGridView.TabIndex = 0;
            this.itemInfoGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.itemInfoGridView_CellValidating);
            this.itemInfoGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.itemInfoGridView_DataError);
            this.itemInfoGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.itemInfoGridView_DefaultValuesNeeded);
            this.itemInfoGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.itemInfoGridView_UserDeletingRow);
            // 
            // Modified
            // 
            this.Modified.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.ColumnHeader;
            this.Modified.DataPropertyName = "Modified";
            this.Modified.Frozen = true;
            this.Modified.HeaderText = "Modified";
            this.Modified.Name = "Modified";
            this.Modified.ReadOnly = true;
            this.Modified.Width = 53;
            // 
            // ItemIndex
            // 
            this.ItemIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.ItemIndex.DataPropertyName = "ItemIndex";
            this.ItemIndex.Frozen = true;
            this.ItemIndex.HeaderText = "Index";
            this.ItemIndex.Name = "ItemIndex";
            this.ItemIndex.ReadOnly = true;
            this.ItemIndex.Width = 58;
            // 
            // ItemName
            // 
            this.ItemName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.ItemName.DataPropertyName = "ItemName";
            this.ItemName.Frozen = true;
            this.ItemName.HeaderText = "Name";
            this.ItemName.Name = "ItemName";
            this.ItemName.Width = 60;
            // 
            // ItemType
            // 
            this.ItemType.DataPropertyName = "ItemType";
            this.ItemType.HeaderText = "Type";
            this.ItemType.Name = "ItemType";
            this.ItemType.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // ItemGrade
            // 
            this.ItemGrade.DataPropertyName = "ItemGrade";
            this.ItemGrade.HeaderText = "Grade";
            this.ItemGrade.Name = "ItemGrade";
            // 
            // ItemRequiredType
            // 
            this.ItemRequiredType.DataPropertyName = "ItemRequiredType";
            this.ItemRequiredType.HeaderText = "Required Type";
            this.ItemRequiredType.Name = "ItemRequiredType";
            // 
            // ItemRequiredGender
            // 
            this.ItemRequiredGender.DataPropertyName = "ItemRequiredGender";
            this.ItemRequiredGender.HeaderText = "Required Gender";
            this.ItemRequiredGender.Name = "ItemRequiredGender";
            // 
            // ItemRequiredClass
            // 
            this.ItemRequiredClass.DataPropertyName = "ItemRequiredClass";
            this.ItemRequiredClass.HeaderText = "Required Class";
            this.ItemRequiredClass.Name = "ItemRequiredClass";
            // 
            // ItemSet
            // 
            this.ItemSet.DataPropertyName = "ItemSet";
            this.ItemSet.HeaderText = "Set";
            this.ItemSet.Name = "ItemSet";
            // 
            // ItemRandomStatsId
            // 
            this.ItemRandomStatsId.DataPropertyName = "ItemRandomStatsId";
            this.ItemRandomStatsId.HeaderText = "Random Stats";
            this.ItemRandomStatsId.Name = "ItemRandomStatsId";
            // 
            // ItemRequiredAmount
            // 
            this.ItemRequiredAmount.DataPropertyName = "ItemRequiredAmount";
            this.ItemRequiredAmount.HeaderText = "Required Amount";
            this.ItemRequiredAmount.Name = "ItemRequiredAmount";
            // 
            // ItemImage
            // 
            this.ItemImage.DataPropertyName = "ItemImage";
            this.ItemImage.HeaderText = "Image";
            this.ItemImage.Name = "ItemImage";
            // 
            // ItemShape
            // 
            this.ItemShape.DataPropertyName = "ItemShape";
            this.ItemShape.HeaderText = "Shape";
            this.ItemShape.Name = "ItemShape";
            // 
            // ItemEffect
            // 
            this.ItemEffect.DataPropertyName = "ItemEffect";
            this.ItemEffect.HeaderText = "Effect";
            this.ItemEffect.Name = "ItemEffect";
            // 
            // ItemStackSize
            // 
            this.ItemStackSize.DataPropertyName = "ItemStackSize";
            this.ItemStackSize.HeaderText = "Stack Size";
            this.ItemStackSize.Name = "ItemStackSize";
            // 
            // ItemSlots
            // 
            this.ItemSlots.DataPropertyName = "ItemSlots";
            this.ItemSlots.HeaderText = "Slots";
            this.ItemSlots.Name = "ItemSlots";
            // 
            // ItemWeight
            // 
            this.ItemWeight.DataPropertyName = "ItemWeight";
            this.ItemWeight.HeaderText = "Weight";
            this.ItemWeight.Name = "ItemWeight";
            // 
            // ItemLightRange
            // 
            this.ItemLightRange.DataPropertyName = "ItemLightRange";
            this.ItemLightRange.HeaderText = "Light Range";
            this.ItemLightRange.Name = "ItemLightRange";
            // 
            // ItemLightIntensity
            // 
            this.ItemLightIntensity.DataPropertyName = "ItemLightIntensity";
            this.ItemLightIntensity.HeaderText = "Intensity";
            this.ItemLightIntensity.Name = "ItemLightIntensity";
            // 
            // ItemDurability
            // 
            this.ItemDurability.DataPropertyName = "ItemDurability";
            this.ItemDurability.HeaderText = "Durability";
            this.ItemDurability.Name = "ItemDurability";
            // 
            // ItemPrice
            // 
            this.ItemPrice.DataPropertyName = "ItemPrice";
            this.ItemPrice.HeaderText = "Price";
            this.ItemPrice.Name = "ItemPrice";
            // 
            // ItemToolTip
            // 
            this.ItemToolTip.DataPropertyName = "ItemToolTip";
            dataGridViewCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.ItemToolTip.DefaultCellStyle = dataGridViewCellStyle5;
            this.ItemToolTip.HeaderText = "ToolTip";
            this.ItemToolTip.Name = "ItemToolTip";
            this.ItemToolTip.Width = 68;
            // 
            // NeedIdentify
            // 
            this.NeedIdentify.DataPropertyName = "NeedIdentify";
            this.NeedIdentify.HeaderText = "Need Identify";
            this.NeedIdentify.Name = "NeedIdentify";
            // 
            // ShowGroupPickup
            // 
            this.ShowGroupPickup.DataPropertyName = "ShowGroupPickup";
            this.ShowGroupPickup.HeaderText = "Show Group Pickup";
            this.ShowGroupPickup.Name = "ShowGroupPickup";
            // 
            // GlobalDropNotify
            // 
            this.GlobalDropNotify.DataPropertyName = "GlobalDropNotify";
            this.GlobalDropNotify.HeaderText = "Global Drop Notify";
            this.GlobalDropNotify.Name = "GlobalDropNotify";
            // 
            // ClassBased
            // 
            this.ClassBased.DataPropertyName = "ClassBased";
            this.ClassBased.HeaderText = "Class Based";
            this.ClassBased.Name = "ClassBased";
            // 
            // LevelBased
            // 
            this.LevelBased.DataPropertyName = "LevelBased";
            this.LevelBased.HeaderText = "Level Based";
            this.LevelBased.Name = "LevelBased";
            // 
            // CanMine
            // 
            this.CanMine.DataPropertyName = "CanMine";
            this.CanMine.HeaderText = "Can Mine";
            this.CanMine.Name = "CanMine";
            // 
            // CanFastRun
            // 
            this.CanFastRun.DataPropertyName = "CanFastRun";
            this.CanFastRun.HeaderText = "Can FastRun";
            this.CanFastRun.Name = "CanFastRun";
            // 
            // CanAwakening
            // 
            this.CanAwakening.DataPropertyName = "CanAwakening";
            this.CanAwakening.HeaderText = "Can Awakening";
            this.CanAwakening.Name = "CanAwakening";
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.panel3);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(956, 47);
            this.panel1.TabIndex = 1;
            // 
            // panel3
            // 
            this.panel3.Controls.Add(this.Gameshop_button);
            this.panel3.Controls.Add(this.btnExport);
            this.panel3.Controls.Add(this.btnImport);
            this.panel3.Controls.Add(this.lblFilterType);
            this.panel3.Controls.Add(this.groupView);
            this.panel3.Controls.Add(this.drpFilterType);
            this.panel3.Controls.Add(this.lblSearch);
            this.panel3.Controls.Add(this.txtSearch);
            this.panel3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel3.Location = new System.Drawing.Point(0, 0);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(956, 47);
            this.panel3.TabIndex = 5;
            // 
            // btnExport
            // 
            this.btnExport.Location = new System.Drawing.Point(693, 22);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(75, 23);
            this.btnExport.TabIndex = 6;
            this.btnExport.Text = "Export";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.Location = new System.Drawing.Point(611, 22);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(75, 23);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Import";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // lblFilterType
            // 
            this.lblFilterType.AutoSize = true;
            this.lblFilterType.Location = new System.Drawing.Point(334, 9);
            this.lblFilterType.Name = "lblFilterType";
            this.lblFilterType.Size = new System.Drawing.Size(37, 13);
            this.lblFilterType.TabIndex = 3;
            this.lblFilterType.Text = "Type :";
            // 
            // groupView
            // 
            this.groupView.Controls.Add(this.rBtnViewSpecial);
            this.groupView.Controls.Add(this.rbtnViewAll);
            this.groupView.Controls.Add(this.rbtnViewBinding);
            this.groupView.Controls.Add(this.rbtnViewBasic);
            this.groupView.Controls.Add(this.rbtnViewStats);
            this.groupView.Location = new System.Drawing.Point(3, 3);
            this.groupView.Name = "groupView";
            this.groupView.Size = new System.Drawing.Size(325, 41);
            this.groupView.TabIndex = 4;
            this.groupView.TabStop = false;
            this.groupView.Text = "View Mode";
            // 
            // rBtnViewSpecial
            // 
            this.rBtnViewSpecial.AutoSize = true;
            this.rBtnViewSpecial.Location = new System.Drawing.Point(248, 20);
            this.rBtnViewSpecial.Name = "rBtnViewSpecial";
            this.rBtnViewSpecial.Size = new System.Drawing.Size(60, 17);
            this.rBtnViewSpecial.TabIndex = 4;
            this.rBtnViewSpecial.TabStop = true;
            this.rBtnViewSpecial.Text = "Special";
            this.rBtnViewSpecial.UseVisualStyleBackColor = true;
            this.rBtnViewSpecial.CheckedChanged += new System.EventHandler(this.rBtnViewSpecial_CheckedChanged);
            // 
            // rbtnViewAll
            // 
            this.rbtnViewAll.AutoSize = true;
            this.rbtnViewAll.Checked = true;
            this.rbtnViewAll.Location = new System.Drawing.Point(27, 19);
            this.rbtnViewAll.Name = "rbtnViewAll";
            this.rbtnViewAll.Size = new System.Drawing.Size(36, 17);
            this.rbtnViewAll.TabIndex = 0;
            this.rbtnViewAll.TabStop = true;
            this.rbtnViewAll.Text = "All";
            this.rbtnViewAll.UseVisualStyleBackColor = true;
            this.rbtnViewAll.CheckedChanged += new System.EventHandler(this.rbtnViewAll_CheckedChanged);
            // 
            // rbtnViewBinding
            // 
            this.rbtnViewBinding.AutoSize = true;
            this.rbtnViewBinding.Location = new System.Drawing.Point(181, 19);
            this.rbtnViewBinding.Name = "rbtnViewBinding";
            this.rbtnViewBinding.Size = new System.Drawing.Size(60, 17);
            this.rbtnViewBinding.TabIndex = 3;
            this.rbtnViewBinding.TabStop = true;
            this.rbtnViewBinding.Text = "Binding";
            this.rbtnViewBinding.UseVisualStyleBackColor = true;
            this.rbtnViewBinding.CheckedChanged += new System.EventHandler(this.rbtnViewBinding_CheckedChanged);
            // 
            // rbtnViewBasic
            // 
            this.rbtnViewBasic.AutoSize = true;
            this.rbtnViewBasic.Location = new System.Drawing.Point(69, 19);
            this.rbtnViewBasic.Name = "rbtnViewBasic";
            this.rbtnViewBasic.Size = new System.Drawing.Size(51, 17);
            this.rbtnViewBasic.TabIndex = 1;
            this.rbtnViewBasic.TabStop = true;
            this.rbtnViewBasic.Text = "Basic";
            this.rbtnViewBasic.UseVisualStyleBackColor = true;
            this.rbtnViewBasic.CheckedChanged += new System.EventHandler(this.rbtnViewBasic_CheckedChanged);
            // 
            // rbtnViewStats
            // 
            this.rbtnViewStats.AutoSize = true;
            this.rbtnViewStats.Location = new System.Drawing.Point(126, 19);
            this.rbtnViewStats.Name = "rbtnViewStats";
            this.rbtnViewStats.Size = new System.Drawing.Size(49, 17);
            this.rbtnViewStats.TabIndex = 2;
            this.rbtnViewStats.TabStop = true;
            this.rbtnViewStats.Text = "Stats";
            this.rbtnViewStats.UseVisualStyleBackColor = true;
            this.rbtnViewStats.CheckedChanged += new System.EventHandler(this.rbtnViewStats_CheckedChanged);
            // 
            // drpFilterType
            // 
            this.drpFilterType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.drpFilterType.FormattingEnabled = true;
            this.drpFilterType.Location = new System.Drawing.Point(337, 24);
            this.drpFilterType.Name = "drpFilterType";
            this.drpFilterType.Size = new System.Drawing.Size(121, 21);
            this.drpFilterType.TabIndex = 2;
            this.drpFilterType.SelectedIndexChanged += new System.EventHandler(this.drpFilterType_SelectedIndexChanged);
            // 
            // lblSearch
            // 
            this.lblSearch.AutoSize = true;
            this.lblSearch.Location = new System.Drawing.Point(461, 9);
            this.lblSearch.Name = "lblSearch";
            this.lblSearch.Size = new System.Drawing.Size(44, 13);
            this.lblSearch.TabIndex = 1;
            this.lblSearch.Text = "Search:";
            // 
            // txtSearch
            // 
            this.txtSearch.Location = new System.Drawing.Point(464, 25);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(141, 20);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyDown);
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.itemInfoGridView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 47);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(956, 433);
            this.panel2.TabIndex = 2;
            // 
            // Gameshop_button
            // 
            this.Gameshop_button.Location = new System.Drawing.Point(774, 22);
            this.Gameshop_button.Name = "Gameshop_button";
            this.Gameshop_button.Size = new System.Drawing.Size(83, 23);
            this.Gameshop_button.TabIndex = 30;
            this.Gameshop_button.Text = "+ Gameshop";
            this.Gameshop_button.UseVisualStyleBackColor = true;
            this.Gameshop_button.Click += new System.EventHandler(this.Gameshop_button_Click);
            // 
            // ItemInfoFormNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 480);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "ItemInfoFormNew";
            this.Text = "ItemInfoFormNew";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ItemInfoFormNew_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.itemInfoGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupView.ResumeLayout(false);
            this.groupView.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView itemInfoGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupView;
        private System.Windows.Forms.RadioButton rbtnViewAll;
        private System.Windows.Forms.RadioButton rbtnViewBinding;
        private System.Windows.Forms.RadioButton rbtnViewBasic;
        private System.Windows.Forms.RadioButton rbtnViewStats;
        private System.Windows.Forms.RadioButton rBtnViewSpecial;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Label lblFilterType;
        private System.Windows.Forms.ComboBox drpFilterType;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Modified;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemType;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemGrade;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemRequiredType;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemRequiredGender;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemRequiredClass;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemRandomStatsId;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemRequiredAmount;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemShape;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemEffect;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemStackSize;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemSlots;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemWeight;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemLightRange;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemLightIntensity;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemDurability;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemPrice;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemToolTip;
        private System.Windows.Forms.DataGridViewCheckBoxColumn NeedIdentify;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ShowGroupPickup;
        private System.Windows.Forms.DataGridViewCheckBoxColumn GlobalDropNotify;
        private System.Windows.Forms.DataGridViewCheckBoxColumn ClassBased;
        private System.Windows.Forms.DataGridViewCheckBoxColumn LevelBased;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CanMine;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CanFastRun;
        private System.Windows.Forms.DataGridViewCheckBoxColumn CanAwakening;
        private System.Windows.Forms.Button Gameshop_button;
    }
}