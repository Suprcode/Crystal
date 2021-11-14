using System.Windows.Forms;

namespace Server.MirForms
{
    partial class GuildItemForm
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
			this.GuildItemListView = new System.Windows.Forms.ListView();
			this.indexHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.PlaceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.nameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.countHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.DuraHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.MemberListView = new System.Windows.Forms.ListView();
			this.Members = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.Rank = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
			this.DeleteButton = new System.Windows.Forms.Button();
			this.SuspendLayout();
			// 
			// GuildItemListView
			// 
			this.GuildItemListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
			this.GuildItemListView.BackColor = System.Drawing.SystemColors.Window;
			this.GuildItemListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.indexHeader,
            this.PlaceHeader,
            this.nameHeader,
            this.countHeader,
            this.DuraHeader});
			this.GuildItemListView.Dock = System.Windows.Forms.DockStyle.Fill;
			this.GuildItemListView.FullRowSelect = true;
			this.GuildItemListView.GridLines = true;
			this.GuildItemListView.HideSelection = false;
			this.GuildItemListView.Location = new System.Drawing.Point(0, 0);
			this.GuildItemListView.MaximumSize = new System.Drawing.Size(520, 490);
			this.GuildItemListView.Name = "GuildItemListView";
			this.GuildItemListView.Size = new System.Drawing.Size(520, 414);
			this.GuildItemListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
			this.GuildItemListView.TabIndex = 2;
			this.GuildItemListView.UseCompatibleStateImageBehavior = false;
			this.GuildItemListView.View = System.Windows.Forms.View.Details;
			// 
			// indexHeader
			// 
			this.indexHeader.Text = "Index";
			this.indexHeader.Width = 71;
			// 
			// PlaceHeader
			// 
			this.PlaceHeader.Text = "Stored By";
			this.PlaceHeader.Width = 128;
			// 
			// nameHeader
			// 
			this.nameHeader.Text = "Name";
			this.nameHeader.Width = 117;
			// 
			// countHeader
			// 
			this.countHeader.Text = "Count";
			this.countHeader.Width = 95;
			// 
			// DuraHeader
			// 
			this.DuraHeader.Text = "Dura";
			this.DuraHeader.Width = 155;
			// 
			// MemberListView
			// 
			this.MemberListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.Members,
            this.Rank});
			this.MemberListView.FullRowSelect = true;
			this.MemberListView.GridLines = true;
			this.MemberListView.HideSelection = false;
			this.MemberListView.Location = new System.Drawing.Point(543, 10);
			this.MemberListView.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.MemberListView.Name = "MemberListView";
			this.MemberListView.Size = new System.Drawing.Size(291, 323);
			this.MemberListView.TabIndex = 3;
			this.MemberListView.UseCompatibleStateImageBehavior = false;
			this.MemberListView.View = System.Windows.Forms.View.Details;
			// 
			// Members
			// 
			this.Members.Text = "Members";
			this.Members.Width = 193;
			// 
			// Rank
			// 
			this.Rank.Text = "Ranks";
			this.Rank.Width = 183;
			// 
			// DeleteButton
			// 
			this.DeleteButton.Location = new System.Drawing.Point(653, 349);
			this.DeleteButton.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
			this.DeleteButton.Name = "DeleteButton";
			this.DeleteButton.Size = new System.Drawing.Size(74, 32);
			this.DeleteButton.TabIndex = 4;
			this.DeleteButton.Text = "Delete";
			this.DeleteButton.UseVisualStyleBackColor = true;
			this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
			// 
			// GuildItemForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(865, 414);
			this.Controls.Add(this.DeleteButton);
			this.Controls.Add(this.MemberListView);
			this.Controls.Add(this.GuildItemListView);
			this.Name = "GuildItemForm";
			this.Text = "GuildItemForm";
			this.ResumeLayout(false);

        }

        #endregion

        public ListView GuildItemListView;
        private System.Windows.Forms.ColumnHeader indexHeader;
        private System.Windows.Forms.ColumnHeader PlaceHeader;
        private System.Windows.Forms.ColumnHeader nameHeader;
        private System.Windows.Forms.ColumnHeader countHeader;
        private System.Windows.Forms.ColumnHeader DuraHeader;
        public ListView MemberListView;
        private ColumnHeader Members;
        private ColumnHeader Rank;
        private Button DeleteButton;
    }
}