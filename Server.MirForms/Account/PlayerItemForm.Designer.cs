namespace Server.MirForms
{
    partial class PlayerItemForm
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
            this.PlayersItemListView = new CustomFormControl.ListViewNF();
            this.indexHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.PlaceHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.nameHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.countHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.DuraHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.UniqueIDHeader = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.SuspendLayout();
            // 
            // PlayersItemListView
            // 
            this.PlayersItemListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.PlayersItemListView.BackColor = System.Drawing.SystemColors.Window;
            this.PlayersItemListView.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.indexHeader,
            this.PlaceHeader,
            this.nameHeader,
            this.countHeader,
            this.DuraHeader,
            this.UniqueIDHeader});
            this.PlayersItemListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PlayersItemListView.FullRowSelect = true;
            this.PlayersItemListView.GridLines = true;
            this.PlayersItemListView.Location = new System.Drawing.Point(0, 0);
            this.PlayersItemListView.MaximumSize = new System.Drawing.Size(620, 490);
            this.PlayersItemListView.Name = "PlayersItemListView";
            this.PlayersItemListView.Size = new System.Drawing.Size(619, 461);
            this.PlayersItemListView.Sorting = System.Windows.Forms.SortOrder.Ascending;
            this.PlayersItemListView.TabIndex = 1;
            this.PlayersItemListView.UseCompatibleStateImageBehavior = false;
            this.PlayersItemListView.View = System.Windows.Forms.View.Details;
            // 
            // indexHeader
            // 
            this.indexHeader.Text = "Index";
            this.indexHeader.Width = 71;
            // 
            // PlaceHeader
            // 
            this.PlaceHeader.Text = "Location";
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
            this.countHeader.Width = 94;
            // 
            // DuraHeader
            // 
            this.DuraHeader.Text = "Dura";
            this.DuraHeader.Width = 102;
            // 
            // UniqueIDHeader
            // 
            this.UniqueIDHeader.Text = "UniqueID";
            this.UniqueIDHeader.Width = 103;
            // 
            // PlayerItemForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(619, 461);
            this.Controls.Add(this.PlayersItemListView);
            this.MaximumSize = new System.Drawing.Size(635, 500);
            this.Name = "PlayerItemForm";
            this.Text = "PlayerItemForm";
            this.ResumeLayout(false);

        }

        #endregion

        public CustomFormControl.ListViewNF PlayersItemListView;
        private System.Windows.Forms.ColumnHeader indexHeader;
        private System.Windows.Forms.ColumnHeader nameHeader;
        private System.Windows.Forms.ColumnHeader countHeader;
        private System.Windows.Forms.ColumnHeader DuraHeader;
        private System.Windows.Forms.ColumnHeader PlaceHeader;
        private System.Windows.Forms.ColumnHeader UniqueIDHeader;
    }
}