namespace Server.Systems
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
            GuildItemListView = new ListView();
            indexHeader = new ColumnHeader();
            PlaceHeader = new ColumnHeader();
            nameHeader = new ColumnHeader();
            countHeader = new ColumnHeader();
            DuraHeader = new ColumnHeader();
            MemberListView = new ListView();
            Members = new ColumnHeader();
            Rank = new ColumnHeader();
            DeleteButton = new Button();
            GuildNoticeBox = new RichTextBox();
            MemberCountLabel = new Label();
            BuffListView = new ListView();
            BuffID = new ColumnHeader();
            BuffName = new ColumnHeader();
            BuffActivity = new ColumnHeader();
            BuffTime = new ColumnHeader();
            GuildNoticeGroupBox = new GroupBox();
            GuildStorageGroupBox = new GroupBox();
            GuildMembersGroupBox = new GroupBox();
            GuildEXPLabel = new Label();
            GuildBuffsGroupBox = new GroupBox();
            GuildPointsLabel = new Label();
            GuildNoticeGroupBox.SuspendLayout();
            GuildStorageGroupBox.SuspendLayout();
            GuildMembersGroupBox.SuspendLayout();
            GuildBuffsGroupBox.SuspendLayout();
            SuspendLayout();
            // 
            // GuildItemListView
            // 
            GuildItemListView.Columns.AddRange(new ColumnHeader[] { indexHeader, PlaceHeader, nameHeader, countHeader, DuraHeader });
            GuildItemListView.GridLines = true;
            GuildItemListView.Location = new Point(10, 22);
            GuildItemListView.Name = "GuildItemListView";
            GuildItemListView.Size = new Size(520, 414);
            GuildItemListView.TabIndex = 0;
            GuildItemListView.UseCompatibleStateImageBehavior = false;
            GuildItemListView.View = View.Details;
            // 
            // indexHeader
            // 
            indexHeader.Text = "UID";
            indexHeader.Width = 70;
            // 
            // PlaceHeader
            // 
            PlaceHeader.Text = "Stored By";
            PlaceHeader.Width = 130;
            // 
            // nameHeader
            // 
            nameHeader.Text = "Name";
            nameHeader.Width = 115;
            // 
            // countHeader
            // 
            countHeader.Text = "Count";
            countHeader.Width = 95;
            // 
            // DuraHeader
            // 
            DuraHeader.Text = "Dura";
            DuraHeader.Width = 155;
            // 
            // MemberListView
            // 
            MemberListView.Columns.AddRange(new ColumnHeader[] { Members, Rank });
            MemberListView.GridLines = true;
            MemberListView.Location = new Point(8, 22);
            MemberListView.Name = "MemberListView";
            MemberListView.Scrollable = false;
            MemberListView.Size = new Size(291, 369);
            MemberListView.TabIndex = 1;
            MemberListView.UseCompatibleStateImageBehavior = false;
            MemberListView.View = View.Details;
            // 
            // Members
            // 
            Members.Text = "Members";
            Members.Width = 195;
            // 
            // Rank
            // 
            Rank.Text = "Ranks";
            Rank.Width = 180;
            // 
            // DeleteButton
            // 
            DeleteButton.Location = new Point(115, 397);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(75, 32);
            DeleteButton.TabIndex = 2;
            DeleteButton.Text = "Delete";
            DeleteButton.UseVisualStyleBackColor = true;
            DeleteButton.Click += DeleteButton_Click;
            // 
            // GuildNoticeBox
            // 
            GuildNoticeBox.BorderStyle = BorderStyle.None;
            GuildNoticeBox.Location = new Point(8, 22);
            GuildNoticeBox.Name = "GuildNoticeBox";
            GuildNoticeBox.ReadOnly = true;
            GuildNoticeBox.Size = new Size(399, 219);
            GuildNoticeBox.TabIndex = 3;
            GuildNoticeBox.Text = "";
            // 
            // MemberCountLabel
            // 
            MemberCountLabel.AutoSize = true;
            MemberCountLabel.Location = new Point(6, 398);
            MemberCountLabel.Name = "MemberCountLabel";
            MemberCountLabel.Size = new Size(60, 15);
            MemberCountLabel.TabIndex = 4;
            MemberCountLabel.Text = "Members:";
            // 
            // BuffListView
            // 
            BuffListView.BorderStyle = BorderStyle.None;
            BuffListView.Columns.AddRange(new ColumnHeader[] { BuffID, BuffName, BuffActivity, BuffTime });
            BuffListView.FullRowSelect = true;
            BuffListView.GridLines = true;
            BuffListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            BuffListView.Location = new Point(6, 40);
            BuffListView.Name = "BuffListView";
            BuffListView.Size = new Size(422, 201);
            BuffListView.TabIndex = 5;
            BuffListView.UseCompatibleStateImageBehavior = false;
            BuffListView.View = View.Details;
            // 
            // BuffID
            // 
            BuffID.Text = "ID";
            // 
            // BuffName
            // 
            BuffName.Text = "Name";
            BuffName.Width = 140;
            // 
            // BuffActivity
            // 
            BuffActivity.Text = "Active";
            BuffActivity.Width = 80;
            // 
            // BuffTime
            // 
            BuffTime.Text = "Duration (Mins)";
            BuffTime.Width = 135;
            // 
            // GuildNoticeGroupBox
            // 
            GuildNoticeGroupBox.Controls.Add(GuildNoticeBox);
            GuildNoticeGroupBox.Location = new Point(442, 452);
            GuildNoticeGroupBox.Name = "GuildNoticeGroupBox";
            GuildNoticeGroupBox.Size = new Size(413, 249);
            GuildNoticeGroupBox.TabIndex = 6;
            GuildNoticeGroupBox.TabStop = false;
            GuildNoticeGroupBox.Text = "Notice";
            // 
            // GuildStorageGroupBox
            // 
            GuildStorageGroupBox.Controls.Add(GuildItemListView);
            GuildStorageGroupBox.Location = new Point(2, 0);
            GuildStorageGroupBox.Name = "GuildStorageGroupBox";
            GuildStorageGroupBox.Size = new Size(539, 446);
            GuildStorageGroupBox.TabIndex = 7;
            GuildStorageGroupBox.TabStop = false;
            GuildStorageGroupBox.Text = "Storage";
            // 
            // GuildMembersGroupBox
            // 
            GuildMembersGroupBox.Controls.Add(GuildEXPLabel);
            GuildMembersGroupBox.Controls.Add(MemberListView);
            GuildMembersGroupBox.Controls.Add(DeleteButton);
            GuildMembersGroupBox.Controls.Add(MemberCountLabel);
            GuildMembersGroupBox.Location = new Point(547, 0);
            GuildMembersGroupBox.Name = "GuildMembersGroupBox";
            GuildMembersGroupBox.Size = new Size(308, 446);
            GuildMembersGroupBox.TabIndex = 8;
            GuildMembersGroupBox.TabStop = false;
            GuildMembersGroupBox.Text = "Members/Ranks";
            // 
            // GuildEXPLabel
            // 
            GuildEXPLabel.AutoSize = true;
            GuildEXPLabel.Location = new Point(8, 421);
            GuildEXPLabel.Name = "GuildEXPLabel";
            GuildEXPLabel.Size = new Size(30, 15);
            GuildEXPLabel.TabIndex = 5;
            GuildEXPLabel.Text = "EXP:";
            // 
            // GuildBuffsGroupBox
            // 
            GuildBuffsGroupBox.Controls.Add(GuildPointsLabel);
            GuildBuffsGroupBox.Controls.Add(BuffListView);
            GuildBuffsGroupBox.Location = new Point(2, 452);
            GuildBuffsGroupBox.Name = "GuildBuffsGroupBox";
            GuildBuffsGroupBox.Size = new Size(434, 249);
            GuildBuffsGroupBox.TabIndex = 9;
            GuildBuffsGroupBox.TabStop = false;
            GuildBuffsGroupBox.Text = "Buffs";
            // 
            // GuildPointsLabel
            // 
            GuildPointsLabel.AutoSize = true;
            GuildPointsLabel.Location = new Point(6, 19);
            GuildPointsLabel.Name = "GuildPointsLabel";
            GuildPointsLabel.Size = new Size(43, 15);
            GuildPointsLabel.TabIndex = 6;
            GuildPointsLabel.Text = "Points:";
            // 
            // GuildItemForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(865, 705);
            Controls.Add(GuildBuffsGroupBox);
            Controls.Add(GuildMembersGroupBox);
            Controls.Add(GuildStorageGroupBox);
            Controls.Add(GuildNoticeGroupBox);
            Name = "GuildItemForm";
            Text = "GuildItemForm";
            GuildNoticeGroupBox.ResumeLayout(false);
            GuildStorageGroupBox.ResumeLayout(false);
            GuildMembersGroupBox.ResumeLayout(false);
            GuildMembersGroupBox.PerformLayout();
            GuildBuffsGroupBox.ResumeLayout(false);
            GuildBuffsGroupBox.PerformLayout();
            ResumeLayout(false);
        }

        #endregion
        private ColumnHeader indexHeader;
        private ColumnHeader PlaceHeader;
        private ColumnHeader nameHeader;
        private ColumnHeader countHeader;
        private ColumnHeader DuraHeader;
        private ColumnHeader Members;
        private ColumnHeader Rank;
        private Button DeleteButton;
        public ListView GuildItemListView;
        public ListView MemberListView;
        private RichTextBox GuildNoticeBox;
        private Label MemberCountLabel;
        private ListView BuffListView;
        private ColumnHeader BuffID;
        private ColumnHeader BuffName;
        private ColumnHeader BuffActivity;
        private ColumnHeader BuffTime;
        private GroupBox GuildNoticeGroupBox;
        private GroupBox GuildStorageGroupBox;
        private GroupBox GuildMembersGroupBox;
        private GroupBox GuildBuffsGroupBox;
        private Label GuildPointsLabel;
        private Label GuildEXPLabel;
    }
}