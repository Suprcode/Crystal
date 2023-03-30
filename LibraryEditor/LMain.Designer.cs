using System;

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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(LMain));
            MainMenu = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            newToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            openReferenceFileToolStripMenuItem = new ToolStripMenuItem();
            openReferenceImageToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            saveToolStripMenuItem = new ToolStripMenuItem();
            saveAsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem2 = new ToolStripSeparator();
            closeToolStripMenuItem = new ToolStripMenuItem();
            functionsToolStripMenuItem = new ToolStripMenuItem();
            copyToToolStripMenuItem = new ToolStripMenuItem();
            countBlanksToolStripMenuItem = new ToolStripMenuItem();
            removeBlanksToolStripMenuItem = new ToolStripMenuItem();
            safeToolStripMenuItem = new ToolStripMenuItem();
            convertToolStripMenuItem = new ToolStripMenuItem();
            populateFramesToolStripMenuItem = new ToolStripMenuItem();
            defaultMonsterFramesToolStripMenuItem = new ToolStripMenuItem();
            defaultNPCFramesToolStripMenuItem = new ToolStripMenuItem();
            defaultPlayerFramesToolStripMenuItem = new ToolStripMenuItem();
            autofillFromCodeToolStripMenuItem = new ToolStripMenuItem();
            importShadowsToolStripMenuItem = new ToolStripMenuItem();
            skinToolStripMenuItem = new ToolStripMenuItem();
            splitContainer1 = new SplitContainer();
            splitContainer2 = new SplitContainer();
            numericUpDownY = new NumericUpDown();
            numericUpDownX = new NumericUpDown();
            BulkButton = new Button();
            checkBox1 = new CheckBox();
            groupBox1 = new GroupBox();
            RButtonOverlay = new RadioButton();
            RButtonImage = new RadioButton();
            checkboxRemoveBlackOnImport = new CheckBox();
            nudJump = new NumericUpDown();
            checkBoxPreventAntiAliasing = new CheckBox();
            checkBoxQuality = new CheckBox();
            buttonSkipPrevious = new Button();
            buttonSkipNext = new Button();
            buttonReplace = new Button();
            pictureBox = new PictureBox();
            ZoomTrackBar = new TrackBar();
            ExportButton = new Button();
            InsertImageButton = new Button();
            DeleteButton = new Button();
            AddButton = new Button();
            label10 = new Label();
            label8 = new Label();
            HeightLabel = new Label();
            label6 = new Label();
            WidthLabel = new Label();
            label1 = new Label();
            panel = new Panel();
            ImageBox = new PictureBox();
            tabControl = new TabControl();
            tabImages = new TabPage();
            PreviewListView = new CustomFormControl.FixedListView();
            ImageList = new ImageList(components);
            tabFrames = new TabPage();
            frameGridView = new DataGridView();
            FrameAction = new DataGridViewComboBoxColumn();
            FrameStart = new DataGridViewTextBoxColumn();
            FrameCount = new DataGridViewTextBoxColumn();
            FrameSkip = new DataGridViewTextBoxColumn();
            FrameInterval = new DataGridViewTextBoxColumn();
            FrameEffectStart = new DataGridViewTextBoxColumn();
            FrameEffectCount = new DataGridViewTextBoxColumn();
            FrameEffectSkip = new DataGridViewTextBoxColumn();
            FrameEffectInterval = new DataGridViewTextBoxColumn();
            FrameReverse = new DataGridViewCheckBoxColumn();
            FrameBlend = new DataGridViewCheckBoxColumn();
            OpenLibraryDialog = new OpenFileDialog();
            SaveLibraryDialog = new SaveFileDialog();
            ImportImageDialog = new OpenFileDialog();
            OpenWeMadeDialog = new OpenFileDialog();
            toolTip = new ToolTip(components);
            statusStrip = new StatusStrip();
            toolStripStatusLabel = new ToolStripStatusLabel();
            toolStripProgressBar = new ToolStripProgressBar();
            FolderLibraryDialog = new FolderBrowserDialog();
            FrameAnimTimer = new System.Windows.Forms.Timer(components);
            MainMenu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer1).BeginInit();
            splitContainer1.Panel1.SuspendLayout();
            splitContainer1.Panel2.SuspendLayout();
            splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)splitContainer2).BeginInit();
            splitContainer2.Panel1.SuspendLayout();
            splitContainer2.Panel2.SuspendLayout();
            splitContainer2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)numericUpDownY).BeginInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownX).BeginInit();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)nudJump).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ZoomTrackBar).BeginInit();
            panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)ImageBox).BeginInit();
            tabControl.SuspendLayout();
            tabImages.SuspendLayout();
            tabFrames.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)frameGridView).BeginInit();
            statusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // MainMenu
            // 
            MainMenu.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem, functionsToolStripMenuItem, skinToolStripMenuItem });
            MainMenu.Location = new Point(0, 0);
            MainMenu.Name = "MainMenu";
            MainMenu.Padding = new Padding(7, 2, 0, 2);
            MainMenu.RenderMode = ToolStripRenderMode.Professional;
            MainMenu.Size = new Size(1209, 24);
            MainMenu.TabIndex = 0;
            MainMenu.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { newToolStripMenuItem, openToolStripMenuItem, toolStripMenuItem1, openReferenceFileToolStripMenuItem, openReferenceImageToolStripMenuItem, toolStripSeparator1, saveToolStripMenuItem, saveAsToolStripMenuItem, toolStripMenuItem2, closeToolStripMenuItem });
            fileToolStripMenuItem.Image = (Image)resources.GetObject("fileToolStripMenuItem.Image");
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(53, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // newToolStripMenuItem
            // 
            newToolStripMenuItem.Image = (Image)resources.GetObject("newToolStripMenuItem.Image");
            newToolStripMenuItem.Name = "newToolStripMenuItem";
            newToolStripMenuItem.Size = new Size(194, 22);
            newToolStripMenuItem.Text = "New";
            newToolStripMenuItem.ToolTipText = "New .Lib";
            newToolStripMenuItem.Click += newToolStripMenuItem_Click;
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Image = (Image)resources.GetObject("openToolStripMenuItem.Image");
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(194, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.ToolTipText = "Open Shanda or Wemade files.";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(191, 6);
            // 
            // openReferenceFileToolStripMenuItem
            // 
            openReferenceFileToolStripMenuItem.Name = "openReferenceFileToolStripMenuItem";
            openReferenceFileToolStripMenuItem.Size = new Size(194, 22);
            openReferenceFileToolStripMenuItem.Text = "Open Reference File";
            openReferenceFileToolStripMenuItem.Click += openReferenceFileToolStripMenuItem_Click;
            // 
            // openReferenceImageToolStripMenuItem
            // 
            openReferenceImageToolStripMenuItem.Name = "openReferenceImageToolStripMenuItem";
            openReferenceImageToolStripMenuItem.Size = new Size(194, 22);
            openReferenceImageToolStripMenuItem.Text = "Open Reference Image";
            openReferenceImageToolStripMenuItem.Click += openReferenceImageToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(191, 6);
            // 
            // saveToolStripMenuItem
            // 
            saveToolStripMenuItem.Image = (Image)resources.GetObject("saveToolStripMenuItem.Image");
            saveToolStripMenuItem.Name = "saveToolStripMenuItem";
            saveToolStripMenuItem.Size = new Size(194, 22);
            saveToolStripMenuItem.Text = "Save";
            saveToolStripMenuItem.ToolTipText = "Saves currently open .Lib";
            saveToolStripMenuItem.Click += saveToolStripMenuItem_Click;
            // 
            // saveAsToolStripMenuItem
            // 
            saveAsToolStripMenuItem.Image = (Image)resources.GetObject("saveAsToolStripMenuItem.Image");
            saveAsToolStripMenuItem.Name = "saveAsToolStripMenuItem";
            saveAsToolStripMenuItem.Size = new Size(194, 22);
            saveAsToolStripMenuItem.Text = "Save As";
            saveAsToolStripMenuItem.ToolTipText = ".Lib Only.";
            saveAsToolStripMenuItem.Click += saveAsToolStripMenuItem_Click;
            // 
            // toolStripMenuItem2
            // 
            toolStripMenuItem2.Name = "toolStripMenuItem2";
            toolStripMenuItem2.Size = new Size(191, 6);
            // 
            // closeToolStripMenuItem
            // 
            closeToolStripMenuItem.Image = (Image)resources.GetObject("closeToolStripMenuItem.Image");
            closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            closeToolStripMenuItem.Size = new Size(194, 22);
            closeToolStripMenuItem.Text = "Close";
            closeToolStripMenuItem.ToolTipText = "Exit Application.";
            closeToolStripMenuItem.Click += closeToolStripMenuItem_Click;
            // 
            // functionsToolStripMenuItem
            // 
            functionsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { copyToToolStripMenuItem, countBlanksToolStripMenuItem, removeBlanksToolStripMenuItem, convertToolStripMenuItem, populateFramesToolStripMenuItem, importShadowsToolStripMenuItem });
            functionsToolStripMenuItem.Image = (Image)resources.GetObject("functionsToolStripMenuItem.Image");
            functionsToolStripMenuItem.Name = "functionsToolStripMenuItem";
            functionsToolStripMenuItem.Size = new Size(87, 20);
            functionsToolStripMenuItem.Text = "Functions";
            // 
            // copyToToolStripMenuItem
            // 
            copyToToolStripMenuItem.Image = (Image)resources.GetObject("copyToToolStripMenuItem.Image");
            copyToToolStripMenuItem.Name = "copyToToolStripMenuItem";
            copyToToolStripMenuItem.Size = new Size(162, 22);
            copyToToolStripMenuItem.Text = "Copy To..";
            copyToToolStripMenuItem.ToolTipText = "Copy to a new .Lib or to the end of an exsisting one.";
            copyToToolStripMenuItem.Click += copyToToolStripMenuItem_Click;
            // 
            // countBlanksToolStripMenuItem
            // 
            countBlanksToolStripMenuItem.Image = (Image)resources.GetObject("countBlanksToolStripMenuItem.Image");
            countBlanksToolStripMenuItem.Name = "countBlanksToolStripMenuItem";
            countBlanksToolStripMenuItem.Size = new Size(162, 22);
            countBlanksToolStripMenuItem.Text = "Count Blanks";
            countBlanksToolStripMenuItem.ToolTipText = "Counts the blank images in the .Lib";
            countBlanksToolStripMenuItem.Click += countBlanksToolStripMenuItem_Click;
            // 
            // removeBlanksToolStripMenuItem
            // 
            removeBlanksToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { safeToolStripMenuItem });
            removeBlanksToolStripMenuItem.Image = (Image)resources.GetObject("removeBlanksToolStripMenuItem.Image");
            removeBlanksToolStripMenuItem.Name = "removeBlanksToolStripMenuItem";
            removeBlanksToolStripMenuItem.Size = new Size(162, 22);
            removeBlanksToolStripMenuItem.Text = "Remove Blanks";
            removeBlanksToolStripMenuItem.ToolTipText = "Quick removal of blanks.";
            removeBlanksToolStripMenuItem.Click += removeBlanksToolStripMenuItem_Click;
            // 
            // safeToolStripMenuItem
            // 
            safeToolStripMenuItem.Image = (Image)resources.GetObject("safeToolStripMenuItem.Image");
            safeToolStripMenuItem.Name = "safeToolStripMenuItem";
            safeToolStripMenuItem.Size = new Size(96, 22);
            safeToolStripMenuItem.Text = "Safe";
            safeToolStripMenuItem.ToolTipText = "Use the safe method of removing blanks.";
            safeToolStripMenuItem.Click += safeToolStripMenuItem_Click;
            // 
            // convertToolStripMenuItem
            // 
            convertToolStripMenuItem.Image = (Image)resources.GetObject("convertToolStripMenuItem.Image");
            convertToolStripMenuItem.Name = "convertToolStripMenuItem";
            convertToolStripMenuItem.Size = new Size(162, 22);
            convertToolStripMenuItem.Text = "Converter";
            convertToolStripMenuItem.ToolTipText = "Convert Wil/Wzl/Miz to .Lib";
            convertToolStripMenuItem.Click += convertToolStripMenuItem_Click;
            // 
            // populateFramesToolStripMenuItem
            // 
            populateFramesToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { defaultMonsterFramesToolStripMenuItem, defaultNPCFramesToolStripMenuItem, defaultPlayerFramesToolStripMenuItem, autofillFromCodeToolStripMenuItem });
            populateFramesToolStripMenuItem.Image = (Image)resources.GetObject("populateFramesToolStripMenuItem.Image");
            populateFramesToolStripMenuItem.Name = "populateFramesToolStripMenuItem";
            populateFramesToolStripMenuItem.Size = new Size(162, 22);
            populateFramesToolStripMenuItem.Text = "Populate Frames";
            // 
            // defaultMonsterFramesToolStripMenuItem
            // 
            defaultMonsterFramesToolStripMenuItem.Image = (Image)resources.GetObject("defaultMonsterFramesToolStripMenuItem.Image");
            defaultMonsterFramesToolStripMenuItem.Name = "defaultMonsterFramesToolStripMenuItem";
            defaultMonsterFramesToolStripMenuItem.Size = new Size(200, 22);
            defaultMonsterFramesToolStripMenuItem.Text = "Default Monster Frames";
            defaultMonsterFramesToolStripMenuItem.Click += defaultMonsterFramesToolStripMenuItem_Click;
            // 
            // defaultNPCFramesToolStripMenuItem
            // 
            defaultNPCFramesToolStripMenuItem.Image = (Image)resources.GetObject("defaultNPCFramesToolStripMenuItem.Image");
            defaultNPCFramesToolStripMenuItem.Name = "defaultNPCFramesToolStripMenuItem";
            defaultNPCFramesToolStripMenuItem.Size = new Size(200, 22);
            defaultNPCFramesToolStripMenuItem.Text = "Default NPC Frames";
            defaultNPCFramesToolStripMenuItem.Click += defaultNPCFramesToolStripMenuItem_Click;
            // 
            // defaultPlayerFramesToolStripMenuItem
            // 
            defaultPlayerFramesToolStripMenuItem.Image = (Image)resources.GetObject("defaultPlayerFramesToolStripMenuItem.Image");
            defaultPlayerFramesToolStripMenuItem.Name = "defaultPlayerFramesToolStripMenuItem";
            defaultPlayerFramesToolStripMenuItem.Size = new Size(200, 22);
            defaultPlayerFramesToolStripMenuItem.Text = "Default Player Frames";
            defaultPlayerFramesToolStripMenuItem.Click += defaultPlayerFramesToolStripMenuItem_Click;
            // 
            // autofillFromCodeToolStripMenuItem
            // 
            autofillFromCodeToolStripMenuItem.Image = (Image)resources.GetObject("autofillFromCodeToolStripMenuItem.Image");
            autofillFromCodeToolStripMenuItem.Name = "autofillFromCodeToolStripMenuItem";
            autofillFromCodeToolStripMenuItem.Size = new Size(200, 22);
            autofillFromCodeToolStripMenuItem.Text = "AutoFill From Code";
            autofillFromCodeToolStripMenuItem.Click += autofillNpcFramesToolStripMenuItem_Click;
            // 
            // importShadowsToolStripMenuItem
            // 
            importShadowsToolStripMenuItem.Name = "importShadowsToolStripMenuItem";
            importShadowsToolStripMenuItem.Size = new Size(162, 22);
            importShadowsToolStripMenuItem.Text = "Import Shadows";
            importShadowsToolStripMenuItem.Click += importShadowsToolStripMenuItem_Click;
            // 
            // skinToolStripMenuItem
            // 
            skinToolStripMenuItem.Alignment = ToolStripItemAlignment.Right;
            skinToolStripMenuItem.Image = (Image)resources.GetObject("skinToolStripMenuItem.Image");
            skinToolStripMenuItem.Name = "skinToolStripMenuItem";
            skinToolStripMenuItem.Size = new Size(57, 20);
            skinToolStripMenuItem.Text = "Skin";
            skinToolStripMenuItem.Visible = false;
            // 
            // splitContainer1
            // 
            splitContainer1.BorderStyle = BorderStyle.FixedSingle;
            splitContainer1.Dock = DockStyle.Fill;
            splitContainer1.Location = new Point(0, 24);
            splitContainer1.Margin = new Padding(4, 3, 4, 3);
            splitContainer1.Name = "splitContainer1";
            splitContainer1.Orientation = Orientation.Horizontal;
            // 
            // splitContainer1.Panel1
            // 
            splitContainer1.Panel1.Controls.Add(splitContainer2);
            splitContainer1.Panel1MinSize = 325;
            // 
            // splitContainer1.Panel2
            // 
            splitContainer1.Panel2.Controls.Add(tabControl);
            splitContainer1.Size = new Size(1209, 794);
            splitContainer1.SplitterDistance = 452;
            splitContainer1.SplitterWidth = 5;
            splitContainer1.TabIndex = 1;
            // 
            // splitContainer2
            // 
            splitContainer2.BorderStyle = BorderStyle.FixedSingle;
            splitContainer2.Dock = DockStyle.Fill;
            splitContainer2.FixedPanel = FixedPanel.Panel1;
            splitContainer2.IsSplitterFixed = true;
            splitContainer2.Location = new Point(0, 0);
            splitContainer2.Margin = new Padding(4, 3, 4, 3);
            splitContainer2.Name = "splitContainer2";
            // 
            // splitContainer2.Panel1
            // 
            splitContainer2.Panel1.Controls.Add(numericUpDownY);
            splitContainer2.Panel1.Controls.Add(numericUpDownX);
            splitContainer2.Panel1.Controls.Add(BulkButton);
            splitContainer2.Panel1.Controls.Add(checkBox1);
            splitContainer2.Panel1.Controls.Add(groupBox1);
            splitContainer2.Panel1.Controls.Add(checkboxRemoveBlackOnImport);
            splitContainer2.Panel1.Controls.Add(nudJump);
            splitContainer2.Panel1.Controls.Add(checkBoxPreventAntiAliasing);
            splitContainer2.Panel1.Controls.Add(checkBoxQuality);
            splitContainer2.Panel1.Controls.Add(buttonSkipPrevious);
            splitContainer2.Panel1.Controls.Add(buttonSkipNext);
            splitContainer2.Panel1.Controls.Add(buttonReplace);
            splitContainer2.Panel1.Controls.Add(pictureBox);
            splitContainer2.Panel1.Controls.Add(ZoomTrackBar);
            splitContainer2.Panel1.Controls.Add(ExportButton);
            splitContainer2.Panel1.Controls.Add(InsertImageButton);
            splitContainer2.Panel1.Controls.Add(DeleteButton);
            splitContainer2.Panel1.Controls.Add(AddButton);
            splitContainer2.Panel1.Controls.Add(label10);
            splitContainer2.Panel1.Controls.Add(label8);
            splitContainer2.Panel1.Controls.Add(HeightLabel);
            splitContainer2.Panel1.Controls.Add(label6);
            splitContainer2.Panel1.Controls.Add(WidthLabel);
            splitContainer2.Panel1.Controls.Add(label1);
            splitContainer2.Panel1.ForeColor = Color.Black;
            splitContainer2.Panel1MinSize = 240;
            // 
            // splitContainer2.Panel2
            // 
            splitContainer2.Panel2.Controls.Add(panel);
            splitContainer2.Size = new Size(1209, 452);
            splitContainer2.SplitterDistance = 240;
            splitContainer2.SplitterWidth = 5;
            splitContainer2.TabIndex = 0;
            // 
            // numericUpDownY
            // 
            numericUpDownY.Location = new Point(145, 83);
            numericUpDownY.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDownY.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            numericUpDownY.Name = "numericUpDownY";
            numericUpDownY.Size = new Size(82, 23);
            numericUpDownY.TabIndex = 27;
            numericUpDownY.ValueChanged += numericUpDownY_ValueChanged;
            // 
            // numericUpDownX
            // 
            numericUpDownX.Location = new Point(145, 59);
            numericUpDownX.Maximum = new decimal(new int[] { 1000, 0, 0, 0 });
            numericUpDownX.Minimum = new decimal(new int[] { 1000, 0, 0, int.MinValue });
            numericUpDownX.Name = "numericUpDownX";
            numericUpDownX.Size = new Size(82, 23);
            numericUpDownX.TabIndex = 26;
            numericUpDownX.ValueChanged += numericUpDownX_ValueChanged;
            // 
            // BulkButton
            // 
            BulkButton.Location = new Point(6, 66);
            BulkButton.Name = "BulkButton";
            BulkButton.Size = new Size(77, 23);
            BulkButton.TabIndex = 25;
            BulkButton.Text = "Bulk Offset";
            BulkButton.UseVisualStyleBackColor = true;
            BulkButton.Click += BulkButton_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(89, 109);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(97, 19);
            checkBox1.TabIndex = 24;
            checkBox1.Text = "Apply Offsets";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(RButtonOverlay);
            groupBox1.Controls.Add(RButtonImage);
            groupBox1.Location = new Point(13, 331);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(251, 44);
            groupBox1.TabIndex = 23;
            groupBox1.TabStop = false;
            groupBox1.Text = "View Mode";
            // 
            // RButtonOverlay
            // 
            RButtonOverlay.AutoSize = true;
            RButtonOverlay.Location = new Point(79, 17);
            RButtonOverlay.Margin = new Padding(4, 3, 4, 3);
            RButtonOverlay.Name = "RButtonOverlay";
            RButtonOverlay.Size = new Size(65, 19);
            RButtonOverlay.TabIndex = 1;
            RButtonOverlay.Text = "Overlay";
            RButtonOverlay.UseVisualStyleBackColor = true;
            RButtonOverlay.CheckedChanged += RButtonViewMode_CheckedChanged;
            // 
            // RButtonImage
            // 
            RButtonImage.AutoSize = true;
            RButtonImage.Checked = true;
            RButtonImage.Location = new Point(8, 17);
            RButtonImage.Margin = new Padding(4, 3, 4, 3);
            RButtonImage.Name = "RButtonImage";
            RButtonImage.Size = new Size(58, 19);
            RButtonImage.TabIndex = 0;
            RButtonImage.TabStop = true;
            RButtonImage.Text = "Image";
            RButtonImage.UseVisualStyleBackColor = true;
            RButtonImage.CheckedChanged += RButtonViewMode_CheckedChanged;
            // 
            // checkboxRemoveBlackOnImport
            // 
            checkboxRemoveBlackOnImport.AutoSize = true;
            checkboxRemoveBlackOnImport.Checked = true;
            checkboxRemoveBlackOnImport.CheckState = CheckState.Checked;
            checkboxRemoveBlackOnImport.Location = new Point(13, 385);
            checkboxRemoveBlackOnImport.Margin = new Padding(4, 3, 4, 3);
            checkboxRemoveBlackOnImport.Name = "checkboxRemoveBlackOnImport";
            checkboxRemoveBlackOnImport.Size = new Size(158, 19);
            checkboxRemoveBlackOnImport.TabIndex = 22;
            checkboxRemoveBlackOnImport.Text = "Remove Black On Import";
            checkboxRemoveBlackOnImport.UseVisualStyleBackColor = true;
            // 
            // nudJump
            // 
            nudJump.Location = new Point(90, 253);
            nudJump.Margin = new Padding(4, 3, 4, 3);
            nudJump.Maximum = new decimal(new int[] { 650000, 0, 0, 0 });
            nudJump.Name = "nudJump";
            nudJump.Size = new Size(90, 23);
            nudJump.TabIndex = 21;
            nudJump.ValueChanged += nudJump_ValueChanged;
            nudJump.KeyDown += nudJump_KeyDown;
            // 
            // checkBoxPreventAntiAliasing
            // 
            checkBoxPreventAntiAliasing.AutoSize = true;
            checkBoxPreventAntiAliasing.Location = new Point(111, 412);
            checkBoxPreventAntiAliasing.Margin = new Padding(4, 3, 4, 3);
            checkBoxPreventAntiAliasing.Name = "checkBoxPreventAntiAliasing";
            checkBoxPreventAntiAliasing.Size = new Size(112, 19);
            checkBoxPreventAntiAliasing.TabIndex = 20;
            checkBoxPreventAntiAliasing.Text = "No Anti-aliasing";
            checkBoxPreventAntiAliasing.UseVisualStyleBackColor = true;
            checkBoxPreventAntiAliasing.CheckedChanged += checkBoxPreventAntiAliasing_CheckedChanged;
            // 
            // checkBoxQuality
            // 
            checkBoxQuality.AutoSize = true;
            checkBoxQuality.Location = new Point(13, 412);
            checkBoxQuality.Margin = new Padding(4, 3, 4, 3);
            checkBoxQuality.Name = "checkBoxQuality";
            checkBoxQuality.Size = new Size(87, 19);
            checkBoxQuality.TabIndex = 19;
            checkBoxQuality.Text = "No Blurring";
            checkBoxQuality.UseVisualStyleBackColor = true;
            checkBoxQuality.CheckedChanged += checkBoxQuality_CheckedChanged;
            // 
            // buttonSkipPrevious
            // 
            buttonSkipPrevious.ForeColor = SystemColors.ControlText;
            buttonSkipPrevious.Image = (Image)resources.GetObject("buttonSkipPrevious.Image");
            buttonSkipPrevious.Location = new Point(49, 249);
            buttonSkipPrevious.Margin = new Padding(4, 3, 4, 3);
            buttonSkipPrevious.Name = "buttonSkipPrevious";
            buttonSkipPrevious.Size = new Size(35, 30);
            buttonSkipPrevious.TabIndex = 17;
            buttonSkipPrevious.Tag = "";
            buttonSkipPrevious.TextImageRelation = TextImageRelation.TextBeforeImage;
            buttonSkipPrevious.UseVisualStyleBackColor = true;
            buttonSkipPrevious.Click += buttonSkipPrevious_Click;
            // 
            // buttonSkipNext
            // 
            buttonSkipNext.ForeColor = SystemColors.ControlText;
            buttonSkipNext.Image = (Image)resources.GetObject("buttonSkipNext.Image");
            buttonSkipNext.Location = new Point(186, 249);
            buttonSkipNext.Margin = new Padding(4, 3, 4, 3);
            buttonSkipNext.Name = "buttonSkipNext";
            buttonSkipNext.Size = new Size(35, 30);
            buttonSkipNext.TabIndex = 16;
            buttonSkipNext.Tag = "";
            buttonSkipNext.TextImageRelation = TextImageRelation.TextBeforeImage;
            buttonSkipNext.UseVisualStyleBackColor = true;
            buttonSkipNext.Click += buttonSkipNext_Click;
            // 
            // buttonReplace
            // 
            buttonReplace.ForeColor = SystemColors.ControlText;
            buttonReplace.Image = (Image)resources.GetObject("buttonReplace.Image");
            buttonReplace.ImageAlign = ContentAlignment.MiddleRight;
            buttonReplace.Location = new Point(12, 166);
            buttonReplace.Margin = new Padding(4, 3, 4, 3);
            buttonReplace.Name = "buttonReplace";
            buttonReplace.Size = new Size(122, 30);
            buttonReplace.TabIndex = 15;
            buttonReplace.Tag = "";
            buttonReplace.Text = "Replace Image";
            buttonReplace.TextImageRelation = TextImageRelation.TextBeforeImage;
            buttonReplace.UseVisualStyleBackColor = true;
            buttonReplace.Click += buttonReplace_Click;
            // 
            // pictureBox
            // 
            pictureBox.Image = (Image)resources.GetObject("pictureBox.Image");
            pictureBox.Location = new Point(12, 10);
            pictureBox.Margin = new Padding(4, 3, 4, 3);
            pictureBox.Name = "pictureBox";
            pictureBox.Size = new Size(16, 16);
            pictureBox.SizeMode = PictureBoxSizeMode.AutoSize;
            pictureBox.TabIndex = 14;
            pictureBox.TabStop = false;
            toolTip.SetToolTip(pictureBox, "Switch from Black to White background.");
            pictureBox.Click += pictureBox_Click;
            // 
            // ZoomTrackBar
            // 
            ZoomTrackBar.LargeChange = 1;
            ZoomTrackBar.Location = new Point(49, 286);
            ZoomTrackBar.Margin = new Padding(4, 3, 4, 3);
            ZoomTrackBar.Minimum = 1;
            ZoomTrackBar.Name = "ZoomTrackBar";
            ZoomTrackBar.Size = new Size(172, 45);
            ZoomTrackBar.TabIndex = 4;
            ZoomTrackBar.TickStyle = TickStyle.TopLeft;
            ZoomTrackBar.Value = 1;
            ZoomTrackBar.Scroll += ZoomTrackBar_Scroll;
            // 
            // ExportButton
            // 
            ExportButton.ForeColor = SystemColors.ControlText;
            ExportButton.Image = (Image)resources.GetObject("ExportButton.Image");
            ExportButton.ImageAlign = ContentAlignment.MiddleRight;
            ExportButton.Location = new Point(141, 203);
            ExportButton.Margin = new Padding(4, 3, 4, 3);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(122, 30);
            ExportButton.TabIndex = 3;
            ExportButton.Tag = "";
            ExportButton.Text = "Export Images";
            ExportButton.TextImageRelation = TextImageRelation.TextBeforeImage;
            ExportButton.UseVisualStyleBackColor = true;
            ExportButton.Click += ExportButton_Click;
            // 
            // InsertImageButton
            // 
            InsertImageButton.ForeColor = SystemColors.ControlText;
            InsertImageButton.Image = (Image)resources.GetObject("InsertImageButton.Image");
            InsertImageButton.ImageAlign = ContentAlignment.MiddleRight;
            InsertImageButton.Location = new Point(141, 166);
            InsertImageButton.Margin = new Padding(4, 3, 4, 3);
            InsertImageButton.Name = "InsertImageButton";
            InsertImageButton.Size = new Size(122, 30);
            InsertImageButton.TabIndex = 1;
            InsertImageButton.Tag = "";
            InsertImageButton.Text = "Insert Images";
            InsertImageButton.TextImageRelation = TextImageRelation.TextBeforeImage;
            InsertImageButton.UseVisualStyleBackColor = true;
            InsertImageButton.Click += InsertImageButton_Click;
            // 
            // DeleteButton
            // 
            DeleteButton.ForeColor = SystemColors.ControlText;
            DeleteButton.Image = (Image)resources.GetObject("DeleteButton.Image");
            DeleteButton.ImageAlign = ContentAlignment.MiddleRight;
            DeleteButton.Location = new Point(141, 129);
            DeleteButton.Margin = new Padding(4, 3, 4, 3);
            DeleteButton.Name = "DeleteButton";
            DeleteButton.Size = new Size(122, 30);
            DeleteButton.TabIndex = 2;
            DeleteButton.Tag = "";
            DeleteButton.Text = "Delete Images";
            DeleteButton.TextImageRelation = TextImageRelation.TextBeforeImage;
            DeleteButton.UseVisualStyleBackColor = true;
            DeleteButton.Click += DeleteButton_Click;
            // 
            // AddButton
            // 
            AddButton.ForeColor = SystemColors.ControlText;
            AddButton.Image = (Image)resources.GetObject("AddButton.Image");
            AddButton.ImageAlign = ContentAlignment.MiddleRight;
            AddButton.Location = new Point(12, 129);
            AddButton.Margin = new Padding(4, 3, 4, 3);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(122, 30);
            AddButton.TabIndex = 0;
            AddButton.Tag = "";
            AddButton.Text = "Add Images";
            AddButton.TextImageRelation = TextImageRelation.TextBeforeImage;
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.ForeColor = SystemColors.ControlText;
            label10.Location = new Point(85, 85);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(53, 15);
            label10.TabIndex = 12;
            label10.Text = "OffSet Y:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.ForeColor = SystemColors.ControlText;
            label8.Location = new Point(85, 61);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(53, 15);
            label8.TabIndex = 11;
            label8.Text = "OffSet X:";
            // 
            // HeightLabel
            // 
            HeightLabel.AutoSize = true;
            HeightLabel.ForeColor = SystemColors.ControlText;
            HeightLabel.Location = new Point(144, 35);
            HeightLabel.Margin = new Padding(4, 0, 4, 0);
            HeightLabel.Name = "HeightLabel";
            HeightLabel.Size = new Size(75, 15);
            HeightLabel.TabIndex = 10;
            HeightLabel.Text = "<No Image>";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.ForeColor = SystemColors.ControlText;
            label6.Location = new Point(89, 35);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(46, 15);
            label6.TabIndex = 9;
            label6.Text = "Height:";
            // 
            // WidthLabel
            // 
            WidthLabel.AutoSize = true;
            WidthLabel.ForeColor = SystemColors.ControlText;
            WidthLabel.Location = new Point(144, 14);
            WidthLabel.Margin = new Padding(4, 0, 4, 0);
            WidthLabel.Name = "WidthLabel";
            WidthLabel.Size = new Size(75, 15);
            WidthLabel.TabIndex = 8;
            WidthLabel.Text = "<No Image>";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.ForeColor = SystemColors.ControlText;
            label1.Location = new Point(92, 14);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(42, 15);
            label1.TabIndex = 7;
            label1.Text = "Width:";
            // 
            // panel
            // 
            panel.AutoScroll = true;
            panel.BackColor = Color.Black;
            panel.BorderStyle = BorderStyle.Fixed3D;
            panel.Controls.Add(ImageBox);
            panel.Dock = DockStyle.Fill;
            panel.Location = new Point(0, 0);
            panel.Margin = new Padding(4, 3, 4, 3);
            panel.Name = "panel";
            panel.Size = new Size(962, 450);
            panel.TabIndex = 1;
            // 
            // ImageBox
            // 
            ImageBox.BackColor = Color.Transparent;
            ImageBox.Location = new Point(0, 0);
            ImageBox.Margin = new Padding(4, 3, 4, 3);
            ImageBox.Name = "ImageBox";
            ImageBox.Size = new Size(64, 64);
            ImageBox.SizeMode = PictureBoxSizeMode.AutoSize;
            ImageBox.TabIndex = 0;
            ImageBox.TabStop = false;
            // 
            // tabControl
            // 
            tabControl.Controls.Add(tabImages);
            tabControl.Controls.Add(tabFrames);
            tabControl.Dock = DockStyle.Fill;
            tabControl.Location = new Point(0, 0);
            tabControl.Margin = new Padding(4, 3, 4, 3);
            tabControl.Name = "tabControl";
            tabControl.SelectedIndex = 0;
            tabControl.Size = new Size(1207, 335);
            tabControl.TabIndex = 0;
            tabControl.SelectedIndexChanged += tabControl_SelectedIndexChanged;
            // 
            // tabImages
            // 
            tabImages.Controls.Add(PreviewListView);
            tabImages.Location = new Point(4, 24);
            tabImages.Margin = new Padding(4, 3, 4, 3);
            tabImages.Name = "tabImages";
            tabImages.Padding = new Padding(4, 3, 4, 3);
            tabImages.Size = new Size(1199, 307);
            tabImages.TabIndex = 0;
            tabImages.Text = "Images";
            tabImages.UseVisualStyleBackColor = true;
            // 
            // PreviewListView
            // 
            PreviewListView.Activation = ItemActivation.OneClick;
            PreviewListView.BackColor = Color.GhostWhite;
            PreviewListView.Dock = DockStyle.Fill;
            PreviewListView.ForeColor = Color.FromArgb(142, 152, 156);
            PreviewListView.LargeImageList = ImageList;
            PreviewListView.Location = new Point(4, 3);
            PreviewListView.Margin = new Padding(4, 3, 4, 3);
            PreviewListView.Name = "PreviewListView";
            PreviewListView.Size = new Size(1191, 301);
            PreviewListView.TabIndex = 0;
            PreviewListView.UseCompatibleStateImageBehavior = false;
            PreviewListView.VirtualMode = true;
            PreviewListView.RetrieveVirtualItem += PreviewListView_RetrieveVirtualItem;
            PreviewListView.SelectedIndexChanged += PreviewListView_SelectedIndexChanged;
            PreviewListView.VirtualItemsSelectionRangeChanged += PreviewListView_VirtualItemsSelectionRangeChanged;
            // 
            // ImageList
            // 
            ImageList.ColorDepth = ColorDepth.Depth32Bit;
            ImageList.ImageSize = new Size(64, 64);
            ImageList.TransparentColor = Color.Transparent;
            // 
            // tabFrames
            // 
            tabFrames.Controls.Add(frameGridView);
            tabFrames.Location = new Point(4, 24);
            tabFrames.Margin = new Padding(4, 3, 4, 3);
            tabFrames.Name = "tabFrames";
            tabFrames.Size = new Size(1199, 307);
            tabFrames.TabIndex = 1;
            tabFrames.Text = "Frames";
            tabFrames.UseVisualStyleBackColor = true;
            // 
            // frameGridView
            // 
            frameGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            frameGridView.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            frameGridView.Columns.AddRange(new DataGridViewColumn[] { FrameAction, FrameStart, FrameCount, FrameSkip, FrameInterval, FrameEffectStart, FrameEffectCount, FrameEffectSkip, FrameEffectInterval, FrameReverse, FrameBlend });
            frameGridView.Dock = DockStyle.Fill;
            frameGridView.Location = new Point(0, 0);
            frameGridView.Margin = new Padding(4, 3, 4, 3);
            frameGridView.Name = "frameGridView";
            frameGridView.Size = new Size(1199, 307);
            frameGridView.TabIndex = 2;
            frameGridView.CellValidating += frameGridView_CellValidating;
            frameGridView.DefaultValuesNeeded += frameGridView_DefaultValuesNeeded;
            frameGridView.RowEnter += frameGridView_RowEnter;
            // 
            // FrameAction
            // 
            FrameAction.HeaderText = "Action";
            FrameAction.Name = "FrameAction";
            FrameAction.Resizable = DataGridViewTriState.True;
            FrameAction.SortMode = DataGridViewColumnSortMode.Automatic;
            // 
            // FrameStart
            // 
            FrameStart.HeaderText = "Start";
            FrameStart.Name = "FrameStart";
            // 
            // FrameCount
            // 
            FrameCount.HeaderText = "Count";
            FrameCount.Name = "FrameCount";
            // 
            // FrameSkip
            // 
            FrameSkip.HeaderText = "Skip";
            FrameSkip.Name = "FrameSkip";
            // 
            // FrameInterval
            // 
            FrameInterval.HeaderText = "Interval";
            FrameInterval.Name = "FrameInterval";
            // 
            // FrameEffectStart
            // 
            FrameEffectStart.HeaderText = "EffectStart";
            FrameEffectStart.Name = "FrameEffectStart";
            // 
            // FrameEffectCount
            // 
            FrameEffectCount.HeaderText = "EffectCount";
            FrameEffectCount.Name = "FrameEffectCount";
            // 
            // FrameEffectSkip
            // 
            FrameEffectSkip.HeaderText = "EffectSkip";
            FrameEffectSkip.Name = "FrameEffectSkip";
            // 
            // FrameEffectInterval
            // 
            FrameEffectInterval.HeaderText = "EffectInterval";
            FrameEffectInterval.Name = "FrameEffectInterval";
            // 
            // FrameReverse
            // 
            FrameReverse.HeaderText = "Reverse";
            FrameReverse.Name = "FrameReverse";
            // 
            // FrameBlend
            // 
            FrameBlend.HeaderText = "Blend";
            FrameBlend.Name = "FrameBlend";
            // 
            // OpenLibraryDialog
            // 
            OpenLibraryDialog.Filter = "Library|*.Lib";
            // 
            // SaveLibraryDialog
            // 
            SaveLibraryDialog.Filter = "Library|*.Lib";
            // 
            // ImportImageDialog
            // 
            ImportImageDialog.Filter = "Images (*.BMP;*.JPG;*.GIF;*.PNG)|*.BMP;*.JPG;*.GIF;*.PNG|All files (*.*)|*.*";
            ImportImageDialog.Multiselect = true;
            // 
            // OpenWeMadeDialog
            // 
            OpenWeMadeDialog.Filter = "WeMade|*.Wil;*.Wtl|Shanda|*.Wzl;*.Miz|Lib|*.Lib";
            OpenWeMadeDialog.Multiselect = true;
            // 
            // statusStrip
            // 
            statusStrip.Items.AddRange(new ToolStripItem[] { toolStripStatusLabel, toolStripProgressBar });
            statusStrip.LayoutStyle = ToolStripLayoutStyle.HorizontalStackWithOverflow;
            statusStrip.Location = new Point(0, 818);
            statusStrip.Name = "statusStrip";
            statusStrip.Padding = new Padding(1, 0, 16, 0);
            statusStrip.Size = new Size(1209, 24);
            statusStrip.TabIndex = 2;
            statusStrip.Text = "statusStrip";
            // 
            // toolStripStatusLabel
            // 
            toolStripStatusLabel.Name = "toolStripStatusLabel";
            toolStripStatusLabel.Size = new Size(90, 19);
            toolStripStatusLabel.Text = "Selected Image:";
            // 
            // toolStripProgressBar
            // 
            toolStripProgressBar.Alignment = ToolStripItemAlignment.Right;
            toolStripProgressBar.Name = "toolStripProgressBar";
            toolStripProgressBar.Size = new Size(233, 18);
            toolStripProgressBar.Step = 1;
            toolStripProgressBar.Style = ProgressBarStyle.Continuous;
            // 
            // FolderLibraryDialog
            // 
            FolderLibraryDialog.ShowNewFolderButton = false;
            // 
            // FrameAnimTimer
            // 
            FrameAnimTimer.Tick += FrameAnimTimer_Tick;
            // 
            // LMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1209, 842);
            Controls.Add(splitContainer1);
            Controls.Add(statusStrip);
            Controls.Add(MainMenu);
            DoubleBuffered = true;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MainMenuStrip = MainMenu;
            Margin = new Padding(4, 3, 4, 3);
            MinimumSize = new Size(756, 513);
            Name = "LMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Legend of Mir Library Editor";
            Resize += LMain_Resize;
            MainMenu.ResumeLayout(false);
            MainMenu.PerformLayout();
            splitContainer1.Panel1.ResumeLayout(false);
            splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer1).EndInit();
            splitContainer1.ResumeLayout(false);
            splitContainer2.Panel1.ResumeLayout(false);
            splitContainer2.Panel1.PerformLayout();
            splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)splitContainer2).EndInit();
            splitContainer2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)numericUpDownY).EndInit();
            ((System.ComponentModel.ISupportInitialize)numericUpDownX).EndInit();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)nudJump).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox).EndInit();
            ((System.ComponentModel.ISupportInitialize)ZoomTrackBar).EndInit();
            panel.ResumeLayout(false);
            panel.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)ImageBox).EndInit();
            tabControl.ResumeLayout(false);
            tabImages.ResumeLayout(false);
            tabFrames.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)frameGridView).EndInit();
            statusStrip.ResumeLayout(false);
            statusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
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
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TabPage tabImages;
        private CustomFormControl.FixedListView PreviewListView;
        private System.Windows.Forms.TabPage tabFrames;
        private System.Windows.Forms.DataGridView frameGridView;
        private System.Windows.Forms.ToolStripMenuItem populateFramesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultMonsterFramesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem defaultPlayerFramesToolStripMenuItem;
        private System.Windows.Forms.FolderBrowserDialog FolderLibraryDialog;
        private System.Windows.Forms.ToolStripMenuItem autofillFromCodeToolStripMenuItem;
        private System.Windows.Forms.DataGridViewComboBoxColumn FrameAction;
        private System.Windows.Forms.DataGridViewTextBoxColumn FrameStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn FrameCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn FrameSkip;
        private System.Windows.Forms.DataGridViewTextBoxColumn FrameInterval;
        private System.Windows.Forms.DataGridViewTextBoxColumn FrameEffectStart;
        private System.Windows.Forms.DataGridViewTextBoxColumn FrameEffectCount;
        private System.Windows.Forms.DataGridViewTextBoxColumn FrameEffectSkip;
        private System.Windows.Forms.DataGridViewTextBoxColumn FrameEffectInterval;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FrameReverse;
        private System.Windows.Forms.DataGridViewCheckBoxColumn FrameBlend;
        private System.Windows.Forms.ToolStripMenuItem defaultNPCFramesToolStripMenuItem;
        private System.Windows.Forms.Timer FrameAnimTimer;
        private System.Windows.Forms.CheckBox checkboxRemoveBlackOnImport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RadioButton RButtonOverlay;
        private System.Windows.Forms.RadioButton RButtonImage;
        private ToolStripMenuItem openReferenceFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private CheckBox checkBox1;
        private ToolStripMenuItem importShadowsToolStripMenuItem;
        private ToolStripMenuItem openReferenceImageToolStripMenuItem;
        private Button BulkButton;
        private NumericUpDown numericUpDownY;
        private NumericUpDown numericUpDownX;
    }
}

