namespace LibraryEditor
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LMain));
            this.MainMenu = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripSeparator();
            this.saveToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveAsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem2 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.functionsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.copyToToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.countBlanksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeBlanksToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.safeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.convertToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.skinToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.nudJump = new System.Windows.Forms.NumericUpDown();
            this.checkBoxPreventAntiAliasing = new System.Windows.Forms.CheckBox();
            this.checkBoxQuality = new System.Windows.Forms.CheckBox();
            this.buttonSkipPrevious = new System.Windows.Forms.Button();
            this.buttonSkipNext = new System.Windows.Forms.Button();
            this.buttonReplace = new System.Windows.Forms.Button();
            this.pictureBox = new System.Windows.Forms.PictureBox();
            this.ZoomTrackBar = new System.Windows.Forms.TrackBar();
            this.ExportButton = new System.Windows.Forms.Button();
            this.InsertImageButton = new System.Windows.Forms.Button();
            this.OffSetYTextBox = new System.Windows.Forms.TextBox();
            this.OffSetXTextBox = new System.Windows.Forms.TextBox();
            this.DeleteButton = new System.Windows.Forms.Button();
            this.AddButton = new System.Windows.Forms.Button();
            this.label10 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.HeightLabel = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.WidthLabel = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.panel = new System.Windows.Forms.Panel();
            this.ImageBox = new System.Windows.Forms.PictureBox();
            this.PreviewListView = new LibraryEditor.FixedListView();
            this.ImageList = new System.Windows.Forms.ImageList(this.components);
            this.OpenLibraryDialog = new System.Windows.Forms.OpenFileDialog();
            this.SaveLibraryDialog = new System.Windows.Forms.SaveFileDialog();
            this.ImportImageDialog = new System.Windows.Forms.OpenFileDialog();
            this.OpenWeMadeDialog = new System.Windows.Forms.OpenFileDialog();
            this.toolTip = new System.Windows.Forms.ToolTip(this.components);
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudJump)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomTrackBar)).BeginInit();
            this.panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).BeginInit();
            this.statusStrip.SuspendLayout();
            this.SuspendLayout();
            // 
            // MainMenu
            // 
            this.MainMenu.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.functionsToolStripMenuItem,
            this.skinToolStripMenuItem});
            this.MainMenu.Location = new System.Drawing.Point(0, 0);
            this.MainMenu.Name = "MainMenu";
            this.MainMenu.RenderMode = System.Windows.Forms.ToolStripRenderMode.Professional;
            this.MainMenu.Size = new System.Drawing.Size(1008, 25);
            this.MainMenu.TabIndex = 0;
            this.MainMenu.Text = "主菜单";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newToolStripMenuItem,
            this.openToolStripMenuItem,
            this.toolStripMenuItem1,
            this.saveToolStripMenuItem,
            this.saveAsToolStripMenuItem,
            this.toolStripMenuItem2,
            this.closeToolStripMenuItem});
            this.fileToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("fileToolStripMenuItem.Image")));
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.fileToolStripMenuItem.Text = "文件";
            // 
            // newToolStripMenuItem
            // 
            this.newToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("newToolStripMenuItem.Image")));
            this.newToolStripMenuItem.Name = "newToolStripMenuItem";
            this.newToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.newToolStripMenuItem.Text = "新建";
            this.newToolStripMenuItem.ToolTipText = "新建 .Lib 文件";
            this.newToolStripMenuItem.Click += new System.EventHandler(this.newToolStripMenuItem_Click);
            // 
            // openToolStripMenuItem
            // 
            this.openToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("openToolStripMenuItem.Image")));
            this.openToolStripMenuItem.Name = "openToolStripMenuItem";
            this.openToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.openToolStripMenuItem.Text = "打开";
            this.openToolStripMenuItem.ToolTipText = "打开盛大或者韩服文件.";
            this.openToolStripMenuItem.Click += new System.EventHandler(this.openToolStripMenuItem_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(118, 6);
            // 
            // saveToolStripMenuItem
            // 
            this.saveToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveToolStripMenuItem.Image")));
            this.saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            this.saveToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveToolStripMenuItem.Text = "保存";
            this.saveToolStripMenuItem.ToolTipText = "保存当前的 .Lib";
            this.saveToolStripMenuItem.Click += new System.EventHandler(this.saveToolStripMenuItem_Click);
            // 
            // saveAsToolStripMenuItem
            // 
            this.saveAsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("saveAsToolStripMenuItem.Image")));
            this.saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            this.saveAsToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.saveAsToolStripMenuItem.Text = "另存为...";
            this.saveAsToolStripMenuItem.ToolTipText = ".Lib Only.";
            this.saveAsToolStripMenuItem.Click += new System.EventHandler(this.saveAsToolStripMenuItem_Click);
            // 
            // toolStripMenuItem2
            // 
            this.toolStripMenuItem2.Name = "toolStripMenuItem2";
            this.toolStripMenuItem2.Size = new System.Drawing.Size(118, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("closeToolStripMenuItem.Image")));
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(121, 22);
            this.closeToolStripMenuItem.Text = "退出";
            this.closeToolStripMenuItem.ToolTipText = "退出程序.";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // functionsToolStripMenuItem
            // 
            this.functionsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.copyToToolStripMenuItem,
            this.countBlanksToolStripMenuItem,
            this.removeBlanksToolStripMenuItem,
            this.convertToolStripMenuItem});
            this.functionsToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("functionsToolStripMenuItem.Image")));
            this.functionsToolStripMenuItem.Name = "functionsToolStripMenuItem";
            this.functionsToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.functionsToolStripMenuItem.Text = "功能";
            // 
            // copyToToolStripMenuItem
            // 
            this.copyToToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("copyToToolStripMenuItem.Image")));
            this.copyToToolStripMenuItem.Name = "copyToToolStripMenuItem";
            this.copyToToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.copyToToolStripMenuItem.Text = "复制到..";
            this.copyToToolStripMenuItem.ToolTipText = "复制到新的 .Lib 文件或现有文件的末尾.";
            this.copyToToolStripMenuItem.Click += new System.EventHandler(this.copyToToolStripMenuItem_Click);
            // 
            // countBlanksToolStripMenuItem
            // 
            this.countBlanksToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("countBlanksToolStripMenuItem.Image")));
            this.countBlanksToolStripMenuItem.Name = "countBlanksToolStripMenuItem";
            this.countBlanksToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.countBlanksToolStripMenuItem.Text = "计算空白";
            this.countBlanksToolStripMenuItem.ToolTipText = "计算 .Lib 文件中的空图片";
            this.countBlanksToolStripMenuItem.Click += new System.EventHandler(this.countBlanksToolStripMenuItem_Click);
            // 
            // removeBlanksToolStripMenuItem
            // 
            this.removeBlanksToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.safeToolStripMenuItem});
            this.removeBlanksToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("removeBlanksToolStripMenuItem.Image")));
            this.removeBlanksToolStripMenuItem.Name = "removeBlanksToolStripMenuItem";
            this.removeBlanksToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.removeBlanksToolStripMenuItem.Text = "移除空白";
            this.removeBlanksToolStripMenuItem.ToolTipText = "快速移除空白图片.";
            this.removeBlanksToolStripMenuItem.Click += new System.EventHandler(this.removeBlanksToolStripMenuItem_Click);
            // 
            // safeToolStripMenuItem
            // 
            this.safeToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("safeToolStripMenuItem.Image")));
            this.safeToolStripMenuItem.Name = "safeToolStripMenuItem";
            this.safeToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.safeToolStripMenuItem.Text = "安全模式";
            this.safeToolStripMenuItem.ToolTipText = "使用安全模式来移除空白图片.";
            this.safeToolStripMenuItem.Click += new System.EventHandler(this.safeToolStripMenuItem_Click);
            // 
            // convertToolStripMenuItem
            // 
            this.convertToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("convertToolStripMenuItem.Image")));
            this.convertToolStripMenuItem.Name = "convertToolStripMenuItem";
            this.convertToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.convertToolStripMenuItem.Text = "转换";
            this.convertToolStripMenuItem.ToolTipText = "转换 Wil/Wzl/Miz 到 .Lib";
            this.convertToolStripMenuItem.Click += new System.EventHandler(this.convertToolStripMenuItem_Click);
            // 
            // skinToolStripMenuItem
            // 
            this.skinToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.skinToolStripMenuItem.Image = ((System.Drawing.Image)(resources.GetObject("skinToolStripMenuItem.Image")));
            this.skinToolStripMenuItem.Name = "skinToolStripMenuItem";
            this.skinToolStripMenuItem.Size = new System.Drawing.Size(60, 21);
            this.skinToolStripMenuItem.Text = "皮肤";
            this.skinToolStripMenuItem.Visible = false;
            // 
            // splitContainer1
            // 
            this.splitContainer1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 25);
            this.splitContainer1.Name = "splitContainer1";
            this.splitContainer1.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.splitContainer2);
            this.splitContainer1.Panel1MinSize = 325;
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.PreviewListView);
            this.splitContainer1.Size = new System.Drawing.Size(1008, 627);
            this.splitContainer1.SplitterDistance = 325;
            this.splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.FixedPanel = System.Windows.Forms.FixedPanel.Panel1;
            this.splitContainer2.IsSplitterFixed = true;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.nudJump);
            this.splitContainer2.Panel1.Controls.Add(this.checkBoxPreventAntiAliasing);
            this.splitContainer2.Panel1.Controls.Add(this.checkBoxQuality);
            this.splitContainer2.Panel1.Controls.Add(this.buttonSkipPrevious);
            this.splitContainer2.Panel1.Controls.Add(this.buttonSkipNext);
            this.splitContainer2.Panel1.Controls.Add(this.buttonReplace);
            this.splitContainer2.Panel1.Controls.Add(this.pictureBox);
            this.splitContainer2.Panel1.Controls.Add(this.ZoomTrackBar);
            this.splitContainer2.Panel1.Controls.Add(this.ExportButton);
            this.splitContainer2.Panel1.Controls.Add(this.InsertImageButton);
            this.splitContainer2.Panel1.Controls.Add(this.OffSetYTextBox);
            this.splitContainer2.Panel1.Controls.Add(this.OffSetXTextBox);
            this.splitContainer2.Panel1.Controls.Add(this.DeleteButton);
            this.splitContainer2.Panel1.Controls.Add(this.AddButton);
            this.splitContainer2.Panel1.Controls.Add(this.label10);
            this.splitContainer2.Panel1.Controls.Add(this.label8);
            this.splitContainer2.Panel1.Controls.Add(this.HeightLabel);
            this.splitContainer2.Panel1.Controls.Add(this.label6);
            this.splitContainer2.Panel1.Controls.Add(this.WidthLabel);
            this.splitContainer2.Panel1.Controls.Add(this.label1);
            this.splitContainer2.Panel1.ForeColor = System.Drawing.Color.Black;
            this.splitContainer2.Panel1MinSize = 240;
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.panel);
            this.splitContainer2.Size = new System.Drawing.Size(1008, 325);
            this.splitContainer2.SplitterDistance = 240;
            this.splitContainer2.TabIndex = 0;
            // 
            // nudJump
            // 
            this.nudJump.Location = new System.Drawing.Point(77, 202);
            this.nudJump.Maximum = new decimal(new int[] {
            650000,
            0,
            0,
            0});
            this.nudJump.Name = "nudJump";
            this.nudJump.Size = new System.Drawing.Size(77, 21);
            this.nudJump.TabIndex = 21;
            this.nudJump.ValueChanged += new System.EventHandler(this.nudJump_ValueChanged);
            this.nudJump.KeyDown += new System.Windows.Forms.KeyEventHandler(this.nudJump_KeyDown);
            // 
            // checkBoxPreventAntiAliasing
            // 
            this.checkBoxPreventAntiAliasing.AutoSize = true;
            this.checkBoxPreventAntiAliasing.Location = new System.Drawing.Point(121, 280);
            this.checkBoxPreventAntiAliasing.Name = "checkBoxPreventAntiAliasing";
            this.checkBoxPreventAntiAliasing.Size = new System.Drawing.Size(72, 16);
            this.checkBoxPreventAntiAliasing.TabIndex = 20;
            this.checkBoxPreventAntiAliasing.Text = "无抗锯齿";
            this.checkBoxPreventAntiAliasing.UseVisualStyleBackColor = true;
            this.checkBoxPreventAntiAliasing.CheckedChanged += new System.EventHandler(this.checkBoxPreventAntiAliasing_CheckedChanged);
            // 
            // checkBoxQuality
            // 
            this.checkBoxQuality.AutoSize = true;
            this.checkBoxQuality.Location = new System.Drawing.Point(42, 280);
            this.checkBoxQuality.Name = "checkBoxQuality";
            this.checkBoxQuality.Size = new System.Drawing.Size(60, 16);
            this.checkBoxQuality.TabIndex = 19;
            this.checkBoxQuality.Text = "无模糊";
            this.checkBoxQuality.UseVisualStyleBackColor = true;
            this.checkBoxQuality.CheckedChanged += new System.EventHandler(this.checkBoxQuality_CheckedChanged);
            // 
            // buttonSkipPrevious
            // 
            this.buttonSkipPrevious.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSkipPrevious.Image = ((System.Drawing.Image)(resources.GetObject("buttonSkipPrevious.Image")));
            this.buttonSkipPrevious.Location = new System.Drawing.Point(42, 199);
            this.buttonSkipPrevious.Name = "buttonSkipPrevious";
            this.buttonSkipPrevious.Size = new System.Drawing.Size(30, 24);
            this.buttonSkipPrevious.TabIndex = 17;
            this.buttonSkipPrevious.Tag = "";
            this.buttonSkipPrevious.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonSkipPrevious.UseVisualStyleBackColor = true;
            this.buttonSkipPrevious.Click += new System.EventHandler(this.buttonSkipPrevious_Click);
            // 
            // buttonSkipNext
            // 
            this.buttonSkipNext.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonSkipNext.Image = ((System.Drawing.Image)(resources.GetObject("buttonSkipNext.Image")));
            this.buttonSkipNext.Location = new System.Drawing.Point(159, 199);
            this.buttonSkipNext.Name = "buttonSkipNext";
            this.buttonSkipNext.Size = new System.Drawing.Size(30, 24);
            this.buttonSkipNext.TabIndex = 16;
            this.buttonSkipNext.Tag = "";
            this.buttonSkipNext.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonSkipNext.UseVisualStyleBackColor = true;
            this.buttonSkipNext.Click += new System.EventHandler(this.buttonSkipNext_Click);
            // 
            // buttonReplace
            // 
            this.buttonReplace.ForeColor = System.Drawing.SystemColors.ControlText;
            this.buttonReplace.Image = ((System.Drawing.Image)(resources.GetObject("buttonReplace.Image")));
            this.buttonReplace.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.buttonReplace.Location = new System.Drawing.Point(10, 133);
            this.buttonReplace.Name = "buttonReplace";
            this.buttonReplace.Size = new System.Drawing.Size(105, 24);
            this.buttonReplace.TabIndex = 15;
            this.buttonReplace.Tag = "";
            this.buttonReplace.Text = "替换图片";
            this.buttonReplace.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.buttonReplace.UseVisualStyleBackColor = true;
            this.buttonReplace.Click += new System.EventHandler(this.buttonReplace_Click);
            // 
            // pictureBox
            // 
            this.pictureBox.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox.Image")));
            this.pictureBox.Location = new System.Drawing.Point(10, 11);
            this.pictureBox.Name = "pictureBox";
            this.pictureBox.Size = new System.Drawing.Size(16, 16);
            this.pictureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.pictureBox.TabIndex = 14;
            this.pictureBox.TabStop = false;
            this.toolTip.SetToolTip(this.pictureBox, "背景切换成白色或黑色.");
            this.pictureBox.Click += new System.EventHandler(this.pictureBox_Click);
            // 
            // ZoomTrackBar
            // 
            this.ZoomTrackBar.LargeChange = 1;
            this.ZoomTrackBar.Location = new System.Drawing.Point(42, 229);
            this.ZoomTrackBar.Minimum = 1;
            this.ZoomTrackBar.Name = "ZoomTrackBar";
            this.ZoomTrackBar.Size = new System.Drawing.Size(147, 45);
            this.ZoomTrackBar.TabIndex = 4;
            this.ZoomTrackBar.TickStyle = System.Windows.Forms.TickStyle.TopLeft;
            this.ZoomTrackBar.Value = 1;
            this.ZoomTrackBar.Scroll += new System.EventHandler(this.ZoomTrackBar_Scroll);
            // 
            // ExportButton
            // 
            this.ExportButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.ExportButton.Image = ((System.Drawing.Image)(resources.GetObject("ExportButton.Image")));
            this.ExportButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ExportButton.Location = new System.Drawing.Point(121, 162);
            this.ExportButton.Name = "ExportButton";
            this.ExportButton.Size = new System.Drawing.Size(105, 24);
            this.ExportButton.TabIndex = 3;
            this.ExportButton.Tag = "";
            this.ExportButton.Text = "导出图片";
            this.ExportButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.ExportButton.UseVisualStyleBackColor = true;
            this.ExportButton.Click += new System.EventHandler(this.ExportButton_Click);
            // 
            // InsertImageButton
            // 
            this.InsertImageButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.InsertImageButton.Image = ((System.Drawing.Image)(resources.GetObject("InsertImageButton.Image")));
            this.InsertImageButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.InsertImageButton.Location = new System.Drawing.Point(121, 133);
            this.InsertImageButton.Name = "InsertImageButton";
            this.InsertImageButton.Size = new System.Drawing.Size(105, 24);
            this.InsertImageButton.TabIndex = 1;
            this.InsertImageButton.Tag = "";
            this.InsertImageButton.Text = "插入图片";
            this.InsertImageButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.InsertImageButton.UseVisualStyleBackColor = true;
            this.InsertImageButton.Click += new System.EventHandler(this.InsertImageButton_Click);
            // 
            // OffSetYTextBox
            // 
            this.OffSetYTextBox.Location = new System.Drawing.Point(121, 70);
            this.OffSetYTextBox.Name = "OffSetYTextBox";
            this.OffSetYTextBox.Size = new System.Drawing.Size(65, 21);
            this.OffSetYTextBox.TabIndex = 6;
            this.OffSetYTextBox.TextChanged += new System.EventHandler(this.OffSetYTextBox_TextChanged);
            // 
            // OffSetXTextBox
            // 
            this.OffSetXTextBox.Location = new System.Drawing.Point(121, 46);
            this.OffSetXTextBox.Name = "OffSetXTextBox";
            this.OffSetXTextBox.Size = new System.Drawing.Size(65, 21);
            this.OffSetXTextBox.TabIndex = 5;
            this.OffSetXTextBox.TextChanged += new System.EventHandler(this.OffSetXTextBox_TextChanged);
            // 
            // DeleteButton
            // 
            this.DeleteButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.DeleteButton.Image = ((System.Drawing.Image)(resources.GetObject("DeleteButton.Image")));
            this.DeleteButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.DeleteButton.Location = new System.Drawing.Point(121, 103);
            this.DeleteButton.Name = "DeleteButton";
            this.DeleteButton.Size = new System.Drawing.Size(105, 24);
            this.DeleteButton.TabIndex = 2;
            this.DeleteButton.Tag = "";
            this.DeleteButton.Text = "删除图片";
            this.DeleteButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.DeleteButton.UseVisualStyleBackColor = true;
            this.DeleteButton.Click += new System.EventHandler(this.DeleteButton_Click);
            // 
            // AddButton
            // 
            this.AddButton.ForeColor = System.Drawing.SystemColors.ControlText;
            this.AddButton.Image = ((System.Drawing.Image)(resources.GetObject("AddButton.Image")));
            this.AddButton.ImageAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.AddButton.Location = new System.Drawing.Point(10, 103);
            this.AddButton.Name = "AddButton";
            this.AddButton.Size = new System.Drawing.Size(105, 24);
            this.AddButton.TabIndex = 0;
            this.AddButton.Tag = "";
            this.AddButton.Text = "添加图片";
            this.AddButton.TextImageRelation = System.Windows.Forms.TextImageRelation.TextBeforeImage;
            this.AddButton.UseVisualStyleBackColor = true;
            this.AddButton.Click += new System.EventHandler(this.AddButton_Click);
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label10.Location = new System.Drawing.Point(68, 73);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(47, 12);
            this.label10.TabIndex = 12;
            this.label10.Text = "偏移 Y:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label8.Location = new System.Drawing.Point(68, 49);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(47, 12);
            this.label8.TabIndex = 11;
            this.label8.Text = "偏移 X:";
            // 
            // HeightLabel
            // 
            this.HeightLabel.AutoSize = true;
            this.HeightLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.HeightLabel.Location = new System.Drawing.Point(121, 28);
            this.HeightLabel.Name = "HeightLabel";
            this.HeightLabel.Size = new System.Drawing.Size(65, 12);
            this.HeightLabel.TabIndex = 10;
            this.HeightLabel.Text = "<No Image>";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label6.Location = new System.Drawing.Point(92, 28);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(23, 12);
            this.label6.TabIndex = 9;
            this.label6.Text = "高:";
            // 
            // WidthLabel
            // 
            this.WidthLabel.AutoSize = true;
            this.WidthLabel.ForeColor = System.Drawing.SystemColors.ControlText;
            this.WidthLabel.Location = new System.Drawing.Point(121, 11);
            this.WidthLabel.Name = "WidthLabel";
            this.WidthLabel.Size = new System.Drawing.Size(65, 12);
            this.WidthLabel.TabIndex = 8;
            this.WidthLabel.Text = "<No Image>";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.ForeColor = System.Drawing.SystemColors.ControlText;
            this.label1.Location = new System.Drawing.Point(92, 11);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(23, 12);
            this.label1.TabIndex = 7;
            this.label1.Text = "宽:";
            // 
            // panel
            // 
            this.panel.AutoScroll = true;
            this.panel.BackColor = System.Drawing.Color.Black;
            this.panel.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.panel.Controls.Add(this.ImageBox);
            this.panel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel.Location = new System.Drawing.Point(0, 0);
            this.panel.Name = "panel";
            this.panel.Size = new System.Drawing.Size(762, 323);
            this.panel.TabIndex = 1;
            // 
            // ImageBox
            // 
            this.ImageBox.BackColor = System.Drawing.Color.Transparent;
            this.ImageBox.Location = new System.Drawing.Point(0, 0);
            this.ImageBox.Name = "ImageBox";
            this.ImageBox.Size = new System.Drawing.Size(64, 64);
            this.ImageBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.AutoSize;
            this.ImageBox.TabIndex = 0;
            this.ImageBox.TabStop = false;
            // 
            // PreviewListView
            // 
            this.PreviewListView.Activation = System.Windows.Forms.ItemActivation.OneClick;
            this.PreviewListView.BackColor = System.Drawing.Color.GhostWhite;
            this.PreviewListView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.PreviewListView.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(142)))), ((int)(((byte)(152)))), ((int)(((byte)(156)))));
            this.PreviewListView.HideSelection = false;
            this.PreviewListView.LargeImageList = this.ImageList;
            this.PreviewListView.Location = new System.Drawing.Point(0, 0);
            this.PreviewListView.Name = "PreviewListView";
            this.PreviewListView.Size = new System.Drawing.Size(1006, 296);
            this.PreviewListView.TabIndex = 0;
            this.PreviewListView.UseCompatibleStateImageBehavior = false;
            this.PreviewListView.VirtualMode = true;
            this.PreviewListView.RetrieveVirtualItem += new System.Windows.Forms.RetrieveVirtualItemEventHandler(this.PreviewListView_RetrieveVirtualItem);
            this.PreviewListView.SelectedIndexChanged += new System.EventHandler(this.PreviewListView_SelectedIndexChanged);
            this.PreviewListView.VirtualItemsSelectionRangeChanged += new System.Windows.Forms.ListViewVirtualItemsSelectionRangeChangedEventHandler(this.PreviewListView_VirtualItemsSelectionRangeChanged);
            // 
            // ImageList
            // 
            this.ImageList.ColorDepth = System.Windows.Forms.ColorDepth.Depth32Bit;
            this.ImageList.ImageSize = new System.Drawing.Size(64, 64);
            this.ImageList.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // OpenLibraryDialog
            // 
            this.OpenLibraryDialog.Filter = "Library|*.Lib";
            // 
            // SaveLibraryDialog
            // 
            this.SaveLibraryDialog.Filter = "Library|*.Lib";
            // 
            // ImportImageDialog
            // 
            this.ImportImageDialog.Filter = "Images (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            this.ImportImageDialog.Multiselect = true;
            // 
            // OpenWeMadeDialog
            // 
            this.OpenWeMadeDialog.Filter = "WeMade|*.Wil;*.Wtl|Shanda|*.Wzl;*.Miz|Lib|*.Lib";
            this.OpenWeMadeDialog.Multiselect = true;
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel,
            this.toolStripProgressBar});
            this.statusStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.statusStrip.Location = new System.Drawing.Point(0, 652);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(1008, 22);
            this.statusStrip.TabIndex = 2;
            this.statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            this.toolStripStatusLabel.Name = "toolStripStatusLabel";
            this.toolStripStatusLabel.Size = new System.Drawing.Size(71, 17);
            this.toolStripStatusLabel.Text = "已选择图片:";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(200, 16);
            this.toolStripProgressBar.Step = 1;
            this.toolStripProgressBar.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // LMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1008, 674);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.MainMenu);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.MainMenu;
            this.MinimumSize = new System.Drawing.Size(650, 418);
            this.Name = "LMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Legend of Mir Library Editor";
            this.Resize += new System.EventHandler(this.LMain_Resize);
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
            ((System.ComponentModel.ISupportInitialize)(this.nudJump)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ZoomTrackBar)).EndInit();
            this.panel.ResumeLayout(false);
            this.panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.ImageBox)).EndInit();
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip MainMenu;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem newToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem saveToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveAsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.SplitContainer splitContainer2;
        private FixedListView PreviewListView;
        private System.Windows.Forms.ImageList ImageList;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Label HeightLabel;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label WidthLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog OpenLibraryDialog;
        private System.Windows.Forms.SaveFileDialog SaveLibraryDialog;
        private System.Windows.Forms.OpenFileDialog ImportImageDialog;
        private System.Windows.Forms.Button DeleteButton;
        private System.Windows.Forms.OpenFileDialog OpenWeMadeDialog;
        private System.Windows.Forms.ToolStripMenuItem functionsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem convertToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem copyToToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeBlanksToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem countBlanksToolStripMenuItem;
        private System.Windows.Forms.TextBox OffSetYTextBox;
        private System.Windows.Forms.TextBox OffSetXTextBox;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button InsertImageButton;
        private System.Windows.Forms.ToolStripMenuItem safeToolStripMenuItem;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.ToolTip toolTip;
        private System.Windows.Forms.TrackBar ZoomTrackBar;
        private System.Windows.Forms.PictureBox ImageBox;
        private System.Windows.Forms.Panel panel;
        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.PictureBox pictureBox;
        private System.Windows.Forms.ToolStripMenuItem skinToolStripMenuItem;
        private System.Windows.Forms.Button buttonReplace;
        private System.Windows.Forms.Button buttonSkipPrevious;
        private System.Windows.Forms.Button buttonSkipNext;
        private System.Windows.Forms.CheckBox checkBoxQuality;
        private System.Windows.Forms.CheckBox checkBoxPreventAntiAliasing;
        private System.Windows.Forms.NumericUpDown nudJump;

    }
}

