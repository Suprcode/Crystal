namespace LibraryViewer
{
    partial class LMain
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
            this.components = new System.ComponentModel.Container();
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.LibraryFolderDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.DebugBox = new System.Windows.Forms.TextBox();
            this.checkCenter = new System.Windows.Forms.CheckBox();
            this.checkBackground = new System.Windows.Forms.CheckBox();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.LblHeight = new System.Windows.Forms.Label();
            this.LibNameLabel = new System.Windows.Forms.Label();
            this.LibCountLabel = new System.Windows.Forms.Label();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.LblLibName = new System.Windows.Forms.Label();
            this.LblLibCount = new System.Windows.Forms.Label();
            this.LblWidth = new System.Windows.Forms.Label();
            this.ImageBox = new System.Windows.Forms.PictureBox();
            this.PreviewListView = new CustomFormControl.FixedListView();
            this.ExportImagesButton = new System.Windows.Forms.Button();
            this.MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.Size = new System.Drawing.Size(863, 24);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.fileToolStripMenuItem.Text = "Folder";
            // 
            // openMenuItem
            // 
            this.openMenuItem.Name = "openMenuItem";
            this.openMenuItem.Size = new System.Drawing.Size(103, 22);
            this.openMenuItem.Text = "Open";
            this.openMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // ImageList
            // 
            this.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ImageList.ImageSize = new System.Drawing.Size(64, 64);
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // LibraryFolderDialog
            // 
            this.LibraryFolderDialog.ShowNewFolderButton = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 24);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1MinSize = 150;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.PreviewListView);
            this.splitContainer1.Size = new System.Drawing.Size(863, 507);
            this.splitContainer1.SplitterDistance = 250;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.MinimumSize = new System.Drawing.Size(0, 200);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.ExportImagesButton);
            this.splitContainer2.Panel1.Controls.Add(this.DebugBox);
            this.splitContainer2.Panel1.Controls.Add(this.checkCenter);
            this.splitContainer2.Panel1.Controls.Add(this.checkBackground);
            this.splitContainer2.Panel1.Controls.Add(this.HeightLabel);
            this.splitContainer2.Panel1.Controls.Add(this.LblHeight);
            this.splitContainer2.Panel1.Controls.Add(this.LibNameLabel);
            this.splitContainer2.Panel1.Controls.Add(this.LibCountLabel);
            this.splitContainer2.Panel1.Controls.Add(this.WidthLabel);
            this.splitContainer2.Panel1.Controls.Add(this.LblLibName);
            this.splitContainer2.Panel1.Controls.Add(this.LblLibCount);
            this.splitContainer2.Panel1.Controls.Add(this.LblWidth);
            this.splitContainer2.Panel1MinSize = 150;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.ImageBox);
            this.splitContainer2.Panel2MinSize = 250;
            this.splitContainer2.Size = new System.Drawing.Size(863, 250);
            this.splitContainer2.SplitterDistance = 173;
            this.splitContainer2.TabIndex = 0;
            // 
            // DebugBox
            // 
            this.DebugBox.Location = new System.Drawing.Point(3, 182);
            this.DebugBox.Multiline = true;
            this.DebugBox.Name = "DebugBox";
            this.DebugBox.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.DebugBox.Size = new System.Drawing.Size(168, 65);
            this.DebugBox.TabIndex = 6;
            this.DebugBox.Visible = false;
            // 
            // checkCenter
            // 
            this.checkCenter.AutoSize = true;
            this.checkCenter.Checked = true;
            this.checkCenter.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkCenter.Location = new System.Drawing.Point(12, 106);
            this.checkCenter.Name = "checkCenter";
            this.checkCenter.Size = new System.Drawing.Size(57, 17);
            this.checkCenter.TabIndex = 5;
            this.checkCenter.Text = "Center";
            this.checkCenter.UseVisualStyleBackColor = true;
            this.checkCenter.CheckedChanged += new System.EventHandler(this.checkCenter_CheckedChanged);
            // 
            // checkBackground
            // 
            this.checkBackground.AutoSize = true;
            this.checkBackground.Checked = true;
            this.checkBackground.CheckState = System.Windows.Forms.CheckState.Checked;
            this.checkBackground.Location = new System.Drawing.Point(12, 83);
            this.checkBackground.Name = "checkBackground";
            this.checkBackground.Size = new System.Drawing.Size(84, 17);
            this.checkBackground.TabIndex = 4;
            this.checkBackground.Text = "Background";
            this.checkBackground.UseVisualStyleBackColor = true;
            this.checkBackground.CheckedChanged += new System.EventHandler(this.checkBackground_CheckedChanged);
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.Location = new System.Drawing.Point(56, 58);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(65, 13);
            this.HeightLabel.TabIndex = 3;
            this.HeightLabel.Text = "<No Image>";
            // 
            // LblHeight
            // 
            this.LblHeight.AutoSize = true;
            this.LblHeight.Location = new System.Drawing.Point(12, 58);
            this.LblHeight.Name = "LblHeight";
            this.LblHeight.Size = new System.Drawing.Size(41, 13);
            this.LblHeight.TabIndex = 2;
            this.LblHeight.Text = "Height:";
            // 
            // LibNameLabel
            // 
            this.LibNameLabel.AutoSize = true;
            this.LibNameLabel.Location = new System.Drawing.Point(59, 24);
            this.LibNameLabel.Name = "LibNameLabel";
            this.LibNameLabel.Size = new System.Drawing.Size(80, 13);
            this.LibNameLabel.TabIndex = 1;
            this.LibNameLabel.Text = "<No Selection>";
            // 
            // LibCountLabel
            // 
            this.LibCountLabel.AutoSize = true;
            this.LibCountLabel.Location = new System.Drawing.Point(59, 9);
            this.LibCountLabel.Name = "LibCountLabel";
            this.LibCountLabel.Size = new System.Drawing.Size(13, 13);
            this.LibCountLabel.TabIndex = 1;
            this.LibCountLabel.Text = "0";
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.Location = new System.Drawing.Point(56, 45);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(65, 13);
            this.WidthLabel.TabIndex = 1;
            this.WidthLabel.Text = "<No Image>";
            // 
            // LblLibName
            // 
            this.LblLibName.AutoSize = true;
            this.LblLibName.Location = new System.Drawing.Point(12, 24);
            this.LblLibName.Name = "LblLibName";
            this.LblLibName.Size = new System.Drawing.Size(37, 13);
            this.LblLibName.TabIndex = 0;
            this.LblLibName.Text = "Libfile:";
            // 
            // LblLibCount
            // 
            this.LblLibCount.AutoSize = true;
            this.LblLibCount.Location = new System.Drawing.Point(12, 9);
            this.LblLibCount.Name = "LblLibCount";
            this.LblLibCount.Size = new System.Drawing.Size(51, 13);
            this.LblLibCount.TabIndex = 0;
            this.LblLibCount.Text = "Libcount:";
            // 
            // LblWidth
            // 
            this.LblWidth.AutoSize = true;
            this.LblWidth.Location = new System.Drawing.Point(12, 45);
            this.LblWidth.Name = "LblWidth";
            this.LblWidth.Size = new System.Drawing.Size(38, 13);
            this.LblWidth.TabIndex = 0;
            this.LblWidth.Text = "Width:";
            // 
            // ImageBox
            // 
            this.ImageBox.BackColor = System.Drawing.Color.White;
            this.ImageBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.ImageBox.Location = new System.Drawing.Point(0, 0);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(686, 250);
            this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage;
            this.ImageBox.TabIndex = 0;
            this.ImageBox.TabStop = false;
            // 
            // PreviewListView
            // 
            this.PreviewListView.BackgroundImageTiled = true;
            this.PreviewListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewListView.LargeImageList = this.ImageList;
            this.PreviewListView.Location = new System.Drawing.Point(0, 0);
            this.PreviewListView.Name = "PreviewListView";
            this.PreviewListView.ShowItemToolTips = true;
            this.PreviewListView.Size = new System.Drawing.Size(863, 253);
            this.PreviewListView.TabIndex = 0;
            this.PreviewListView.UseCompatibleStateImageBehavior = false;
            this.PreviewListView.VirtualMode = true;
            this.PreviewListView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.PreviewListView_RetrieveVirtualItem);
            this.PreviewListView.SelectedIndexChanged += new System.EventHandler(this.PreviewListView_SelectedIndexChanged);
            // 
            // ExportImagesButton
            // 
            this.ExportImagesButton.Location = new System.Drawing.Point(12, 130);
            this.ExportImagesButton.Name = "ExportImagesButton";
            this.ExportImagesButton.Size = new System.Drawing.Size(84, 23);
            this.ExportImagesButton.TabIndex = 7;
            this.ExportImagesButton.Text = "Export Images";
            this.ExportImagesButton.UseVisualStyleBackColor = true;
            this.ExportImagesButton.Click += new System.EventHandler(this.ExportImagesButton_Click);
            // 
            // LMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(863, 531);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.MainMenu);
            this.MainMenuStrip = this.MainMenu;
            this.Name = "LMain";
            this.Text = "C# Library Viewer";
            this.MainMenu.ResumeLayout(false);
            this.MainMenu.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openMenuItem;
        private System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.FolderBrowserDialog LibraryFolderDialog;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.CheckBox checkCenter;
        private System.Windows.Forms.CheckBox checkBackground;
        private System.Windows.Forms.Label HeightLabel;
        private System.Windows.Forms.Label LblHeight;
        private System.Windows.Forms.Label LibCountLabel;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.Label LblLibCount;
        private System.Windows.Forms.Label LblWidth;
        private System.Windows.Forms.PictureBox ImageBox;
        private CustomFormControl.FixedListView PreviewListView;
        private System.Windows.Forms.Label LibNameLabel;
        private System.Windows.Forms.Label LblLibName;
        private System.Windows.Forms.TextBox DebugBox;
        private System.Windows.Forms.Button ExportImagesButton;
    }
}

