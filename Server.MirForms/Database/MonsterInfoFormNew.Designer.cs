
namespace Server.Database
{
    partial class MonsterInfoFormNew
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
            this.monsterInfoGridView = new System.Windows.Forms.DataGridView();
            this.panel1 = new System.Windows.Forms.Panel();
            this.panel3 = new System.Windows.Forms.Panel();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.groupView = new System.Windows.Forms.GroupBox();
            this.rbtnViewAll = new System.Windows.Forms.RadioButton();
            this.rbtnViewBasic = new System.Windows.Forms.RadioButton();
            this.lblSearch = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.Modified = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MonsterIndex = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterImage = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.MonsterAI = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterEffect = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterLevel = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterLight = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterAttackSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterMoveSpeed = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterViewRange = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterCoolEye = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterExperience = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.MonsterCanPush = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MonsterAutoRev = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MonsterUndead = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MonsterCanTame = new System.Windows.Forms.DataGridViewCheckBoxColumn();
            this.MonsterDropPath = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.monsterInfoGridView)).BeginInit();
            this.panel1.SuspendLayout();
            this.panel3.SuspendLayout();
            this.groupView.SuspendLayout();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // monsterInfoGridView
            // 
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.monsterInfoGridView.AlternatingRowsDefaultCellStyle = dataGridViewCellStyle1;
            this.monsterInfoGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.monsterInfoGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.Modified,
            this.MonsterIndex,
            this.MonsterName,
            this.MonsterImage,
            this.MonsterAI,
            this.MonsterEffect,
            this.MonsterLevel,
            this.MonsterLight,
            this.MonsterAttackSpeed,
            this.MonsterMoveSpeed,
            this.MonsterViewRange,
            this.MonsterCoolEye,
            this.MonsterExperience,
            this.MonsterCanPush,
            this.MonsterAutoRev,
            this.MonsterUndead,
            this.MonsterCanTame,
            this.MonsterDropPath});
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.monsterInfoGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.monsterInfoGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.monsterInfoGridView.Location = new System.Drawing.Point(0, 0);
            this.monsterInfoGridView.Name = "monsterInfoGridView";
            this.monsterInfoGridView.RowTemplate.DefaultCellStyle.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            this.monsterInfoGridView.Size = new System.Drawing.Size(956, 433);
            this.monsterInfoGridView.TabIndex = 0;
            this.monsterInfoGridView.CellValidating += new System.Windows.Forms.DataGridViewCellValidatingEventHandler(this.monsterInfoGridView_CellValidating);
            this.monsterInfoGridView.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.monsterInfoGridView_DataError);
            this.monsterInfoGridView.DefaultValuesNeeded += new System.Windows.Forms.DataGridViewRowEventHandler(this.monsterInfoGridView_DefaultValuesNeeded);
            this.monsterInfoGridView.UserDeletingRow += new System.Windows.Forms.DataGridViewRowCancelEventHandler(this.monsterInfoGridView_UserDeletingRow);
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
            this.panel3.Controls.Add(this.btnExport);
            this.panel3.Controls.Add(this.btnImport);
            this.panel3.Controls.Add(this.groupView);
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
            // groupView
            // 
            this.groupView.Controls.Add(this.rbtnViewAll);
            this.groupView.Controls.Add(this.rbtnViewBasic);
            this.groupView.Enabled = false;
            this.groupView.Location = new System.Drawing.Point(3, 3);
            this.groupView.Name = "groupView";
            this.groupView.Size = new System.Drawing.Size(134, 42);
            this.groupView.TabIndex = 4;
            this.groupView.TabStop = false;
            this.groupView.Text = "View Mode";
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
            // rbtnViewBasic
            // 
            this.rbtnViewBasic.AutoSize = true;
            this.rbtnViewBasic.Location = new System.Drawing.Point(69, 19);
            this.rbtnViewBasic.Name = "rbtnViewBasic";
            this.rbtnViewBasic.Size = new System.Drawing.Size(51, 17);
            this.rbtnViewBasic.TabIndex = 1;
            this.rbtnViewBasic.Text = "Basic";
            this.rbtnViewBasic.UseVisualStyleBackColor = true;
            this.rbtnViewBasic.CheckedChanged += new System.EventHandler(this.rbtnViewBasic_CheckedChanged);
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
            this.panel2.Controls.Add(this.monsterInfoGridView);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(0, 47);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(956, 433);
            this.panel2.TabIndex = 2;
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
            // MonsterIndex
            // 
            this.MonsterIndex.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.AllCells;
            this.MonsterIndex.DataPropertyName = "MonsterIndex";
            this.MonsterIndex.Frozen = true;
            this.MonsterIndex.HeaderText = "Index";
            this.MonsterIndex.Name = "MonsterIndex";
            this.MonsterIndex.ReadOnly = true;
            this.MonsterIndex.Width = 58;
            // 
            // MonsterName
            // 
            this.MonsterName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.MonsterName.DataPropertyName = "MonsterName";
            this.MonsterName.Frozen = true;
            this.MonsterName.HeaderText = "Name";
            this.MonsterName.Name = "MonsterName";
            this.MonsterName.Width = 60;
            // 
            // MonsterImage
            // 
            this.MonsterImage.DataPropertyName = "MonsterImage";
            this.MonsterImage.HeaderText = "Image";
            this.MonsterImage.Name = "MonsterImage";
            this.MonsterImage.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // MonsterAI
            // 
            this.MonsterAI.DataPropertyName = "MonsterAI";
            this.MonsterAI.HeaderText = "AI";
            this.MonsterAI.Name = "MonsterAI";
            // 
            // MonsterEffect
            // 
            this.MonsterEffect.DataPropertyName = "MonsterEffect";
            this.MonsterEffect.HeaderText = "Effect";
            this.MonsterEffect.Name = "MonsterEffect";
            // 
            // MonsterLevel
            // 
            this.MonsterLevel.DataPropertyName = "MonsterLevel";
            this.MonsterLevel.HeaderText = "Level";
            this.MonsterLevel.Name = "MonsterLevel";
            // 
            // MonsterLight
            // 
            this.MonsterLight.DataPropertyName = "MonsterLight";
            this.MonsterLight.HeaderText = "Light";
            this.MonsterLight.Name = "MonsterLight";
            // 
            // MonsterAttackSpeed
            // 
            this.MonsterAttackSpeed.DataPropertyName = "MonsterAttackSpeed";
            this.MonsterAttackSpeed.HeaderText = "Attack Speed";
            this.MonsterAttackSpeed.Name = "MonsterAttackSpeed";
            // 
            // MonsterMoveSpeed
            // 
            this.MonsterMoveSpeed.DataPropertyName = "MonsterMoveSpeed";
            this.MonsterMoveSpeed.HeaderText = "Move Speed";
            this.MonsterMoveSpeed.Name = "MonsterMoveSpeed";
            // 
            // MonsterViewRange
            // 
            this.MonsterViewRange.DataPropertyName = "MonsterViewRange";
            this.MonsterViewRange.HeaderText = "View Range";
            this.MonsterViewRange.Name = "MonsterViewRange";
            // 
            // MonsterCoolEye
            // 
            this.MonsterCoolEye.DataPropertyName = "MonsterCoolEye";
            this.MonsterCoolEye.HeaderText = "Cool Eye";
            this.MonsterCoolEye.Name = "MonsterCoolEye";
            // 
            // MonsterExperience
            // 
            this.MonsterExperience.DataPropertyName = "MonsterExperience";
            this.MonsterExperience.HeaderText = "Experience";
            this.MonsterExperience.Name = "MonsterExperience";
            // 
            // MonsterCanPush
            // 
            this.MonsterCanPush.DataPropertyName = "MonsterCanPush";
            this.MonsterCanPush.HeaderText = "Can Push";
            this.MonsterCanPush.Name = "MonsterCanPush";
            this.MonsterCanPush.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MonsterCanPush.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // MonsterAutoRev
            // 
            this.MonsterAutoRev.DataPropertyName = "MonsterAutoRev";
            this.MonsterAutoRev.HeaderText = "Auto Rev";
            this.MonsterAutoRev.Name = "MonsterAutoRev";
            this.MonsterAutoRev.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MonsterAutoRev.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // MonsterUndead
            // 
            this.MonsterUndead.DataPropertyName = "MonsterUndead";
            this.MonsterUndead.HeaderText = "Undead";
            this.MonsterUndead.Name = "MonsterUndead";
            this.MonsterUndead.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MonsterUndead.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // MonsterCanTame
            // 
            this.MonsterCanTame.DataPropertyName = "MonsterCanTame";
            this.MonsterCanTame.HeaderText = "Can Tame";
            this.MonsterCanTame.Name = "MonsterCanTame";
            this.MonsterCanTame.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.MonsterCanTame.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // MonsterDropPath
            // 
            this.MonsterDropPath.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.DisplayedCells;
            this.MonsterDropPath.DataPropertyName = "MonsterDropPath";
            this.MonsterDropPath.HeaderText = "Drop Path";
            this.MonsterDropPath.Name = "MonsterDropPath";
            this.MonsterDropPath.Width = 80;
            // 
            // MonsterInfoFormNew
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(956, 480);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "MonsterInfoFormNew";
            this.Text = "MonsterInfoFormNew";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.monsterInfoFormNew_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.monsterInfoGridView)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.groupView.ResumeLayout(false);
            this.groupView.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView monsterInfoGridView;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.GroupBox groupView;
        private System.Windows.Forms.RadioButton rbtnViewAll;
        private System.Windows.Forms.RadioButton rbtnViewBasic;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.Label lblSearch;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.DataGridViewCheckBoxColumn Modified;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterIndex;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterName;
        private System.Windows.Forms.DataGridViewComboBoxColumn MonsterImage;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterAI;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterEffect;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterLevel;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterLight;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterAttackSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterMoveSpeed;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterViewRange;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterCoolEye;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterExperience;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MonsterCanPush;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MonsterAutoRev;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MonsterUndead;
        private System.Windows.Forms.DataGridViewCheckBoxColumn MonsterCanTame;
        private System.Windows.Forms.DataGridViewTextBoxColumn MonsterDropPath;
    }
}