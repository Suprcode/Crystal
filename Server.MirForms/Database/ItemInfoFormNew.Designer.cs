
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            this.itemInfoGridView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupView = new System.Windows.Forms.GroupBox();
            this.rbtnViewAll = new System.Windows.Forms.RadioButton();
            this.rbtnViewBinding = new System.Windows.Forms.RadioButton();
            this.rbtnViewBasic = new System.Windows.Forms.RadioButton();
            this.rbtnViewStats = new System.Windows.Forms.RadioButton();
            this.panel2 = new System.Windows.Forms.Panel();
            this.ItemIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.ItemType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemGrade = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemRequiredType = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemRequiredGender = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemRequiredClass = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemSet = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.ItemRandomStats = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            ((System.ComponentModel.ISupportInitialize)(this.itemInfoGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupView.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // itemInfoGridView
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.itemInfoGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.itemInfoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.itemInfoGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.ItemIndex,
            this.ItemName,
            this.ItemType,
            this.ItemGrade,
            this.ItemRequiredType,
            this.ItemRequiredGender,
            this.ItemRequiredClass,
            this.ItemSet,
            this.ItemRandomStats,
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
            this.ItemPrice});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.itemInfoGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.itemInfoGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.itemInfoGridView.Location = new System.Drawing.Point(0, 0);
            this.itemInfoGridView.Name = "itemInfoGridView";
            this.itemInfoGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.itemInfoGridView.Size = new System.Drawing.Size(956, 421);
            this.itemInfoGridView.TabIndex = 0;
            this.itemInfoGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.itemInfoGridView_CellValidating);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupView);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(956, 59);
            this.panel1.TabIndex = 1;
            // 
            // groupView
            // 
            this.groupView.Controls.Add(this.rbtnViewAll);
            this.groupView.Controls.Add(this.rbtnViewBinding);
            this.groupView.Controls.Add(this.rbtnViewBasic);
            this.groupView.Controls.Add(this.rbtnViewStats);
            this.groupView.Location = new System.Drawing.Point(3, 3);
            this.groupView.Name = "groupView";
            this.groupView.Size = new System.Drawing.Size(263, 50);
            this.groupView.TabIndex = 4;
            this.groupView.TabStop = false;
            this.groupView.Text = "View Mode";
            // 
            // rbtnViewAll
            // 
            this.rbtnViewAll.AutoSize = true;
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
            // panel2
            // 
            this.panel2.Controls.Add(this.itemInfoGridView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 59);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(956, 421);
            this.panel2.TabIndex = 2;
            // 
            // ItemIndex
            // 
            this.ItemIndex.HeaderText = "Index";
            this.ItemIndex.Name = "ItemIndex";
            this.ItemIndex.ReadOnly = true;
            // 
            // ItemName
            // 
            this.ItemName.HeaderText = "Name";
            this.ItemName.Name = "ItemName";
            // 
            // ItemType
            // 
            this.ItemType.HeaderText = "Type";
            this.ItemType.Name = "ItemType";
            // 
            // ItemGrade
            // 
            this.ItemGrade.HeaderText = "Grade";
            this.ItemGrade.Name = "ItemGrade";
            // 
            // ItemRequiredType
            // 
            this.ItemRequiredType.HeaderText = "Required Type";
            this.ItemRequiredType.Name = "ItemRequiredType";
            // 
            // ItemRequiredGender
            // 
            this.ItemRequiredGender.HeaderText = "Required Gender";
            this.ItemRequiredGender.Name = "ItemRequiredGender";
            // 
            // ItemRequiredClass
            // 
            this.ItemRequiredClass.HeaderText = "Required Class";
            this.ItemRequiredClass.Name = "ItemRequiredClass";
            // 
            // ItemSet
            // 
            this.ItemSet.HeaderText = "Set";
            this.ItemSet.Name = "ItemSet";
            // 
            // ItemRandomStats
            // 
            this.ItemRandomStats.HeaderText = "Random Stats";
            this.ItemRandomStats.Name = "ItemRandomStats";
            // 
            // ItemRequiredAmount
            // 
            this.ItemRequiredAmount.HeaderText = "Required Amount";
            this.ItemRequiredAmount.Name = "ItemRequiredAmount";
            // 
            // ItemImage
            // 
            this.ItemImage.HeaderText = "Image";
            this.ItemImage.Name = "ItemImage";
            // 
            // ItemShape
            // 
            this.ItemShape.HeaderText = "Shape";
            this.ItemShape.Name = "ItemShape";
            // 
            // ItemEffect
            // 
            this.ItemEffect.HeaderText = "Effect";
            this.ItemEffect.Name = "ItemEffect";
            // 
            // ItemStackSize
            // 
            this.ItemStackSize.HeaderText = "Stack Size";
            this.ItemStackSize.Name = "ItemStackSize";
            // 
            // ItemSlots
            // 
            this.ItemSlots.HeaderText = "Slots";
            this.ItemSlots.Name = "ItemSlots";
            // 
            // ItemWeight
            // 
            this.ItemWeight.HeaderText = "Weight";
            this.ItemWeight.Name = "ItemWeight";
            // 
            // ItemLightRange
            // 
            this.ItemLightRange.HeaderText = "Light Range";
            this.ItemLightRange.Name = "ItemLightRange";
            // 
            // ItemLightIntensity
            // 
            this.ItemLightIntensity.HeaderText = "Intensity";
            this.ItemLightIntensity.Name = "ItemLightIntensity";
            // 
            // ItemDurability
            // 
            this.ItemDurability.HeaderText = "Durability";
            this.ItemDurability.Name = "ItemDurability";
            // 
            // ItemPrice
            // 
            this.ItemPrice.HeaderText = "Price";
            this.ItemPrice.Name = "ItemPrice";
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
            ((System.ComponentModel.ISupportInitialize)(this.itemInfoGridView)).EndInit();
            this.panel1.ResumeLayout(false);
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
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemName;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemType;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemGrade;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemRequiredType;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemRequiredGender;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemRequiredClass;
        private System.Windows.Forms.DataGridViewComboBoxColumn ItemSet;
        private System.Windows.Forms.DataGridViewTextBoxColumn ItemRandomStats;
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
    }
}