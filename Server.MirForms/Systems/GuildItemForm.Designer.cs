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
            SuspendLayout();
            // 
            // GuildItemListView
            // 
            GuildItemListView.Columns.AddRange(new ColumnHeader[] { indexHeader, PlaceHeader, nameHeader, countHeader, DuraHeader });
            GuildItemListView.GridLines = true;
            GuildItemListView.Location = new Point(1, 0);
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
            MemberListView.Location = new Point(547, 12);
            MemberListView.Name = "MemberListView";
            MemberListView.Scrollable = false;
            MemberListView.Size = new Size(291, 323);
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
            DeleteButton.Location = new Point(654, 356);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(75, 32);
            DeleteButton.TabIndex = 2;
            DeleteButton.Text = "Delete";
            DeleteButton.UseVisualStyleBackColor = true;
            DeleteButton.Click += DeleteButton_Click;
            // 
            // GuildItemForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(854, 450);
            Controls.Add(DeleteButton);
            Controls.Add(MemberListView);
            Controls.Add(GuildItemListView);
            Name = "GuildItemForm";
            Text = "GuildItemForm";
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
    }
}