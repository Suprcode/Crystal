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
            RefreshNoticeButton = new Button();
            GuildStorageGroupBox = new GroupBox();
            GuildMembersGroupBox = new GroupBox();
            GuildRanksListView = new ListView();
            GuildRank = new ColumnHeader();
            GuildEXPLabel = new Label();
            GuildBuffsGroupBox = new GroupBox();
            GuildPointsLabel = new Label();
            SendGuildMesageBox = new TextBox();
            SendGuildMessageButton = new Button();
            GuildChatGroupBox = new GroupBox();
            GuildChatBox = new RichTextBox();
            GuildNoticeGroupBox.SuspendLayout();
            GuildStorageGroupBox.SuspendLayout();
            GuildMembersGroupBox.SuspendLayout();
            GuildBuffsGroupBox.SuspendLayout();
            GuildChatGroupBox.SuspendLayout();
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
            MemberListView.FullRowSelect = true;
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
            Members.Width = 160;
            // 
            // Rank
            // 
            Rank.Text = "Ranks";
            Rank.Width = 180;
            // 
            // DeleteButton
            // 
            DeleteButton.Location = new Point(104, 402);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(100, 23);
            DeleteButton.TabIndex = 2;
            DeleteButton.Text = "Delete Member";
            DeleteButton.UseVisualStyleBackColor = true;
            DeleteButton.Click += DeleteButton_Click;
            // 
            // GuildNoticeBox
            // 
            GuildNoticeBox.Location = new Point(8, 22);
            GuildNoticeBox.Name = "GuildNoticeBox";
            GuildNoticeBox.Size = new Size(399, 248);
            GuildNoticeBox.TabIndex = 3;
            GuildNoticeBox.Text = "";
            // 
            // MemberCountLabel
            // 
            MemberCountLabel.AutoSize = true;
            MemberCountLabel.Location = new Point(305, 402);
            MemberCountLabel.Name = "MemberCountLabel";
            MemberCountLabel.Size = new Size(60, 15);
            MemberCountLabel.TabIndex = 4;
            MemberCountLabel.Text = "Members:";
            // 
            // BuffListView
            // 
            BuffListView.Columns.AddRange(new ColumnHeader[] { BuffID, BuffName, BuffActivity, BuffTime });
            BuffListView.FullRowSelect = true;
            BuffListView.GridLines = true;
            BuffListView.HeaderStyle = ColumnHeaderStyle.Nonclickable;
            BuffListView.Location = new Point(6, 40);
            BuffListView.Name = "BuffListView";
            BuffListView.Size = new Size(422, 255);
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
            BuffActivity.Text = "Status";
            BuffActivity.Width = 80;
            // 
            // BuffTime
            // 
            BuffTime.Text = "Duration (Mins)";
            BuffTime.Width = 135;
            // 
            // GuildNoticeGroupBox
            // 
            GuildNoticeGroupBox.Controls.Add(RefreshNoticeButton);
            GuildNoticeGroupBox.Controls.Add(GuildNoticeBox);
            GuildNoticeGroupBox.Location = new Point(442, 452);
            GuildNoticeGroupBox.Name = "GuildNoticeGroupBox";
            GuildNoticeGroupBox.Size = new Size(413, 305);
            GuildNoticeGroupBox.TabIndex = 6;
            GuildNoticeGroupBox.TabStop = false;
            GuildNoticeGroupBox.Text = "Notice";
            // 
            // RefreshNoticeButton
            // 
            RefreshNoticeButton.Location = new Point(166, 276);
            RefreshNoticeButton.Name = "RefreshNoticeButton";
            RefreshNoticeButton.Size = new Size(94, 23);
            RefreshNoticeButton.TabIndex = 4;
            RefreshNoticeButton.Text = "Update Notice";
            RefreshNoticeButton.UseVisualStyleBackColor = true;
            RefreshNoticeButton.Click += RefreshNoticeButton_Click;
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
            GuildMembersGroupBox.Controls.Add(GuildRanksListView);
            GuildMembersGroupBox.Controls.Add(GuildEXPLabel);
            GuildMembersGroupBox.Controls.Add(MemberListView);
            GuildMembersGroupBox.Controls.Add(DeleteButton);
            GuildMembersGroupBox.Controls.Add(MemberCountLabel);
            GuildMembersGroupBox.Location = new Point(547, 0);
            GuildMembersGroupBox.Name = "GuildMembersGroupBox";
            GuildMembersGroupBox.Size = new Size(595, 446);
            GuildMembersGroupBox.TabIndex = 8;
            GuildMembersGroupBox.TabStop = false;
            GuildMembersGroupBox.Text = "Members/Ranks";
            // 
            // GuildRanksListView
            // 
            GuildRanksListView.Columns.AddRange(new ColumnHeader[] { GuildRank });
            GuildRanksListView.FullRowSelect = true;
            GuildRanksListView.GridLines = true;
            GuildRanksListView.Location = new Point(305, 22);
            GuildRanksListView.Name = "GuildRanksListView";
            GuildRanksListView.Size = new Size(284, 369);
            GuildRanksListView.TabIndex = 7;
            GuildRanksListView.UseCompatibleStateImageBehavior = false;
            GuildRanksListView.View = View.Details;
            // 
            // GuildRank
            // 
            GuildRank.Text = "Guild Rank";
            GuildRank.Width = 284;
            // 
            // GuildEXPLabel
            // 
            GuildEXPLabel.AutoSize = true;
            GuildEXPLabel.Location = new Point(307, 425);
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
            GuildBuffsGroupBox.Size = new Size(434, 305);
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
            // SendGuildMesageBox
            // 
            SendGuildMesageBox.Location = new Point(6, 277);
            SendGuildMesageBox.Name = "SendGuildMesageBox";
            SendGuildMesageBox.Size = new Size(188, 23);
            SendGuildMesageBox.TabIndex = 10;
            // 
            // SendGuildMessageButton
            // 
            SendGuildMessageButton.Location = new Point(200, 276);
            SendGuildMessageButton.Name = "SendGuildMessageButton";
            SendGuildMessageButton.Size = new Size(75, 23);
            SendGuildMessageButton.TabIndex = 11;
            SendGuildMessageButton.Text = "Send";
            SendGuildMessageButton.UseVisualStyleBackColor = true;
            SendGuildMessageButton.Click += SendGuildMessageButton_Click;
            // 
            // GuildChatGroupBox
            // 
            GuildChatGroupBox.Controls.Add(GuildChatBox);
            GuildChatGroupBox.Controls.Add(SendGuildMessageButton);
            GuildChatGroupBox.Controls.Add(SendGuildMesageBox);
            GuildChatGroupBox.Location = new Point(861, 452);
            GuildChatGroupBox.Name = "GuildChatGroupBox";
            GuildChatGroupBox.Size = new Size(281, 305);
            GuildChatGroupBox.TabIndex = 12;
            GuildChatGroupBox.TabStop = false;
            GuildChatGroupBox.Text = "Guild Chat";
            // 
            // GuildChatBox
            // 
            GuildChatBox.Location = new Point(6, 22);
            GuildChatBox.Name = "GuildChatBox";
            GuildChatBox.ReadOnly = true;
            GuildChatBox.Size = new Size(269, 248);
            GuildChatBox.TabIndex = 12;
            GuildChatBox.Text = "";
            // 
            // GuildItemForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1154, 759);
            Controls.Add(GuildChatGroupBox);
            Controls.Add(GuildBuffsGroupBox);
            Controls.Add(GuildMembersGroupBox);
            Controls.Add(GuildStorageGroupBox);
            Controls.Add(GuildNoticeGroupBox);
            Name = "GuildItemForm";
            Text = "GuildItemForm";
            Load += GuildItemForm_Load;
            GuildNoticeGroupBox.ResumeLayout(false);
            GuildStorageGroupBox.ResumeLayout(false);
            GuildMembersGroupBox.ResumeLayout(false);
            GuildMembersGroupBox.PerformLayout();
            GuildBuffsGroupBox.ResumeLayout(false);
            GuildBuffsGroupBox.PerformLayout();
            GuildChatGroupBox.ResumeLayout(false);
            GuildChatGroupBox.PerformLayout();
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
        private Button RefreshNoticeButton;
        private TextBox SendGuildMesageBox;
        private Button SendGuildMessageButton;
        private ListView GuildRanksListView;
        private ColumnHeader GuildRank;
        private GroupBox GuildChatGroupBox;
        private RichTextBox GuildChatBox;
    }
}