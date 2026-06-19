namespace AutoPatcherAdmin
{
    partial class AMain
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AMain));
            ClientTextBox = new TextBox();
            label1 = new Label();
            label2 = new Label();
            HostTextBox = new TextBox();
            TestConnectionButton = new Button();
            label3 = new Label();
            LoginTextBox = new TextBox();
            label4 = new Label();
            PasswordTextBox = new TextBox();
            AbortButton = new Button();
            ProcessButton = new Button();
            ActionLabel = new Label();
            label5 = new Label();
            SpeedLabel = new Label();
            label7 = new Label();
            FileLabel = new Label();
            label8 = new Label();
            ListButton = new Button();
            AllowCleanCheckBox = new CheckBox();
            AllowCleanHintLabel = new Label();
            CompressFilesCheckBox = new CheckBox();
            CompressFilesHintLabel = new Label();
            BrowseClientButton = new Button();
            DownloadExistingButton = new Button();
            ProtocolDropDown = new ComboBox();
            label6 = new Label();
            PortNumericUpDown = new NumericUpDown();
            PortLabel = new Label();
            PortHintLabel = new Label();
            ConnectionHeaderLabel = new Label();
            ActionsHeaderLabel = new Label();
            SummaryHeaderLabel = new Label();
            SummaryAddedLabel = new Label();
            SummaryChangedLabel = new Label();
            SummaryUnchangedLabel = new Label();
            SummaryDeletedLabel = new Label();
            SummaryUploadSizeLabel = new Label();
            ProgressHeaderLabel = new Label();
            PreviewFilterLabel = new Label();
            PreviewActionFilterDropDown = new ComboBox();
            CompareButton = new Button();
            PreviewGrid = new DataGridView();
            PreviewActionColumn = new DataGridViewTextBoxColumn();
            PreviewFileColumn = new DataGridViewTextBoxColumn();
            PreviewSizeColumn = new DataGridViewTextBoxColumn();
            ClearRepositoryButton = new Button();
            OverallProgressBar = new ProgressBar();
            MainStatusStrip = new StatusStrip();
            StatusActionLabel = new ToolStripStatusLabel();
            StatusFileLabel = new ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).BeginInit();
            ((System.ComponentModel.ISupportInitialize)PreviewGrid).BeginInit();
            MainStatusStrip.SuspendLayout();
            SuspendLayout();
            // 
            // ClientTextBox
            // 
            ClientTextBox.Location = new Point(114, 34);
            ClientTextBox.Name = "ClientTextBox";
            ClientTextBox.Size = new Size(572, 23);
            ClientTextBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(25, 38);
            label1.Name = "label1";
            label1.Size = new Size(92, 15);
            label1.TabIndex = 1;
            label1.Text = "Client Directory:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(32, 67);
            label2.Name = "label2";
            label2.Size = new Size(80, 15);
            label2.TabIndex = 4;
            label2.Text = "Host Address:";
            // 
            // HostTextBox
            // 
            HostTextBox.Location = new Point(114, 63);
            HostTextBox.Name = "HostTextBox";
            HostTextBox.Size = new Size(572, 23);
            HostTextBox.TabIndex = 3;
            // 
            // TestConnectionButton
            // 
            TestConnectionButton.Location = new Point(692, 63);
            TestConnectionButton.Name = "TestConnectionButton";
            TestConnectionButton.Size = new Size(56, 23);
            TestConnectionButton.TabIndex = 57;
            TestConnectionButton.Text = "Test";
            TestConnectionButton.UseVisualStyleBackColor = true;
            TestConnectionButton.Click += TestConnectionButton_Click;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(74, 96);
            label3.Name = "label3";
            label3.Size = new Size(40, 15);
            label3.TabIndex = 6;
            label3.Text = "Login:";
            // 
            // LoginTextBox
            // 
            LoginTextBox.Location = new Point(114, 92);
            LoginTextBox.Name = "LoginTextBox";
            LoginTextBox.Size = new Size(634, 23);
            LoginTextBox.TabIndex = 5;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(53, 125);
            label4.Name = "label4";
            label4.Size = new Size(60, 15);
            label4.TabIndex = 8;
            label4.Text = "Password:";
            // 
            // PasswordTextBox
            // 
            PasswordTextBox.Location = new Point(114, 121);
            PasswordTextBox.Name = "PasswordTextBox";
            PasswordTextBox.PasswordChar = '*';
            PasswordTextBox.Size = new Size(634, 23);
            PasswordTextBox.TabIndex = 7;
            // 
            // AbortButton
            // 
            AbortButton.Enabled = false;
            AbortButton.ForeColor = Color.DarkRed;
            AbortButton.Location = new Point(636, 686);
            AbortButton.Name = "AbortButton";
            AbortButton.Size = new Size(112, 23);
            AbortButton.TabIndex = 58;
            AbortButton.Text = "Cancel";
            AbortButton.UseVisualStyleBackColor = true;
            AbortButton.Click += AbortButton_Click;
            // 
            // ProcessButton
            // 
            ProcessButton.Location = new Point(136, 227);
            ProcessButton.Name = "ProcessButton";
            ProcessButton.Size = new Size(118, 28);
            ProcessButton.TabIndex = 9;
            ProcessButton.Text = "Publish Changes";
            ProcessButton.UseVisualStyleBackColor = true;
            ProcessButton.Click += ProcessButton_Click;
            // 
            // ActionLabel
            // 
            ActionLabel.AutoSize = true;
            ActionLabel.Location = new Point(65, 657);
            ActionLabel.MaximumSize = new Size(674, 0);
            ActionLabel.Name = "ActionLabel";
            ActionLabel.Size = new Size(26, 15);
            ActionLabel.TabIndex = 11;
            ActionLabel.Text = "Idle";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(12, 657);
            label5.Name = "label5";
            label5.Size = new Size(45, 15);
            label5.TabIndex = 10;
            label5.Text = "Action:";
            // 
            // SpeedLabel
            // 
            SpeedLabel.AutoSize = true;
            SpeedLabel.Location = new Point(65, 693);
            SpeedLabel.Name = "SpeedLabel";
            SpeedLabel.Size = new Size(26, 15);
            SpeedLabel.TabIndex = 15;
            SpeedLabel.Text = "Idle";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(12, 693);
            label7.Name = "label7";
            label7.Size = new Size(42, 15);
            label7.TabIndex = 14;
            label7.Text = "Speed:";
            // 
            // FileLabel
            // 
            FileLabel.AutoSize = true;
            FileLabel.Location = new Point(65, 677);
            FileLabel.MaximumSize = new Size(674, 0);
            FileLabel.Name = "FileLabel";
            FileLabel.Size = new Size(26, 15);
            FileLabel.TabIndex = 17;
            FileLabel.Text = "Idle";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(33, 677);
            label8.Name = "label8";
            label8.Size = new Size(28, 15);
            label8.TabIndex = 16;
            label8.Text = "File:";
            // 
            // ListButton
            // 
            ListButton.Location = new Point(264, 227);
            ListButton.Name = "ListButton";
            ListButton.Size = new Size(104, 28);
            ListButton.TabIndex = 20;
            ListButton.Text = "PList Only";
            ListButton.UseVisualStyleBackColor = true;
            ListButton.Click += ListButton_Click;
            // 
            // AllowCleanCheckBox
            // 
            AllowCleanCheckBox.AutoSize = true;
            AllowCleanCheckBox.Location = new Point(114, 151);
            AllowCleanCheckBox.Name = "AllowCleanCheckBox";
            AllowCleanCheckBox.Size = new Size(107, 19);
            AllowCleanCheckBox.TabIndex = 22;
            AllowCleanCheckBox.Text = "Allow Clean Up";
            AllowCleanCheckBox.UseVisualStyleBackColor = true;
            // 
            // AllowCleanHintLabel
            // 
            AllowCleanHintLabel.AutoSize = true;
            AllowCleanHintLabel.Font = new Font("Segoe UI", 7.5F);
            AllowCleanHintLabel.ForeColor = SystemColors.GrayText;
            AllowCleanHintLabel.Location = new Point(114, 173);
            AllowCleanHintLabel.MaximumSize = new Size(175, 0);
            AllowCleanHintLabel.Name = "AllowCleanHintLabel";
            AllowCleanHintLabel.Size = new Size(169, 24);
            AllowCleanHintLabel.TabIndex = 53;
            AllowCleanHintLabel.Text = "Removes old server files no longer in the PList after each publish.";
            // 
            // CompressFilesCheckBox
            // 
            CompressFilesCheckBox.AutoSize = true;
            CompressFilesCheckBox.Location = new Point(300, 151);
            CompressFilesCheckBox.Name = "CompressFilesCheckBox";
            CompressFilesCheckBox.Size = new Size(105, 19);
            CompressFilesCheckBox.TabIndex = 54;
            CompressFilesCheckBox.Text = "Compress Files";
            CompressFilesCheckBox.UseVisualStyleBackColor = true;
            // 
            // CompressFilesHintLabel
            // 
            CompressFilesHintLabel.AutoSize = true;
            CompressFilesHintLabel.Font = new Font("Segoe UI", 7.5F);
            CompressFilesHintLabel.ForeColor = SystemColors.GrayText;
            CompressFilesHintLabel.Location = new Point(300, 173);
            CompressFilesHintLabel.MaximumSize = new Size(175, 0);
            CompressFilesHintLabel.Name = "CompressFilesHintLabel";
            CompressFilesHintLabel.Size = new Size(174, 12);
            CompressFilesHintLabel.TabIndex = 55;
            CompressFilesHintLabel.Text = "GZip files before upload. Saves 40-70%";
            // 
            // BrowseClientButton
            // 
            BrowseClientButton.Location = new Point(692, 34);
            BrowseClientButton.Name = "BrowseClientButton";
            BrowseClientButton.Size = new Size(56, 23);
            BrowseClientButton.TabIndex = 56;
            BrowseClientButton.Text = "...";
            BrowseClientButton.UseVisualStyleBackColor = true;
            BrowseClientButton.Click += BrowseClientButton_Click;
            // 
            // DownloadExistingButton
            // 
            DownloadExistingButton.Location = new Point(374, 227);
            DownloadExistingButton.Name = "DownloadExistingButton";
            DownloadExistingButton.Size = new Size(116, 28);
            DownloadExistingButton.TabIndex = 23;
            DownloadExistingButton.Text = "Download Remote";
            DownloadExistingButton.UseVisualStyleBackColor = true;
            DownloadExistingButton.Click += DownloadExistingButton_Click;
            // 
            // ProtocolDropDown
            // 
            ProtocolDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
            ProtocolDropDown.FormattingEnabled = true;
            ProtocolDropDown.Items.AddRange(new object[] { "Ftp", "Sftp", "Http", "Https" });
            ProtocolDropDown.Location = new Point(522, 148);
            ProtocolDropDown.Name = "ProtocolDropDown";
            ProtocolDropDown.Size = new Size(91, 23);
            ProtocolDropDown.TabIndex = 25;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(461, 153);
            label6.Name = "label6";
            label6.Size = new Size(55, 15);
            label6.TabIndex = 26;
            label6.Text = "Protocol:";
            // 
            // PortNumericUpDown
            // 
            PortNumericUpDown.Location = new Point(657, 149);
            PortNumericUpDown.Maximum = new decimal(new int[] { 65535, 0, 0, 0 });
            PortNumericUpDown.Name = "PortNumericUpDown";
            PortNumericUpDown.Size = new Size(91, 23);
            PortNumericUpDown.TabIndex = 48;
            // 
            // PortLabel
            // 
            PortLabel.AutoSize = true;
            PortLabel.Location = new Point(619, 153);
            PortLabel.Name = "PortLabel";
            PortLabel.Size = new Size(32, 15);
            PortLabel.TabIndex = 49;
            PortLabel.Text = "Port:";
            // 
            // PortHintLabel
            // 
            PortHintLabel.AutoSize = true;
            PortHintLabel.Font = new Font("Segoe UI", 7.5F);
            PortHintLabel.ForeColor = SystemColors.GrayText;
            PortHintLabel.Location = new Point(657, 175);
            PortHintLabel.Name = "PortHintLabel";
            PortHintLabel.Size = new Size(54, 12);
            PortHintLabel.TabIndex = 50;
            PortHintLabel.Text = "0 = Default";
            // 
            // ConnectionHeaderLabel
            // 
            ConnectionHeaderLabel.AutoSize = true;
            ConnectionHeaderLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ConnectionHeaderLabel.Location = new Point(12, 10);
            ConnectionHeaderLabel.Name = "ConnectionHeaderLabel";
            ConnectionHeaderLabel.Size = new Size(70, 15);
            ConnectionHeaderLabel.TabIndex = 27;
            ConnectionHeaderLabel.Text = "Connection";
            // 
            // ActionsHeaderLabel
            // 
            ActionsHeaderLabel.AutoSize = true;
            ActionsHeaderLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ActionsHeaderLabel.Location = new Point(12, 205);
            ActionsHeaderLabel.Name = "ActionsHeaderLabel";
            ActionsHeaderLabel.Size = new Size(48, 15);
            ActionsHeaderLabel.TabIndex = 28;
            ActionsHeaderLabel.Text = "Actions";
            // 
            // SummaryHeaderLabel
            // 
            SummaryHeaderLabel.AutoSize = true;
            SummaryHeaderLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            SummaryHeaderLabel.Location = new Point(15, 258);
            SummaryHeaderLabel.Name = "SummaryHeaderLabel";
            SummaryHeaderLabel.Size = new Size(102, 15);
            SummaryHeaderLabel.TabIndex = 29;
            SummaryHeaderLabel.Text = "Publish Summary";
            // 
            // SummaryAddedLabel
            // 
            SummaryAddedLabel.AutoSize = true;
            SummaryAddedLabel.Location = new Point(15, 279);
            SummaryAddedLabel.Name = "SummaryAddedLabel";
            SummaryAddedLabel.Size = new Size(54, 15);
            SummaryAddedLabel.TabIndex = 30;
            SummaryAddedLabel.Text = "Added: 0";
            // 
            // SummaryChangedLabel
            // 
            SummaryChangedLabel.AutoSize = true;
            SummaryChangedLabel.Location = new Point(115, 279);
            SummaryChangedLabel.Name = "SummaryChangedLabel";
            SummaryChangedLabel.Size = new Size(67, 15);
            SummaryChangedLabel.TabIndex = 31;
            SummaryChangedLabel.Text = "Changed: 0";
            // 
            // SummaryUnchangedLabel
            // 
            SummaryUnchangedLabel.AutoSize = true;
            SummaryUnchangedLabel.Location = new Point(231, 279);
            SummaryUnchangedLabel.Name = "SummaryUnchangedLabel";
            SummaryUnchangedLabel.Size = new Size(80, 15);
            SummaryUnchangedLabel.TabIndex = 32;
            SummaryUnchangedLabel.Text = "Unchanged: 0";
            // 
            // SummaryDeletedLabel
            // 
            SummaryDeletedLabel.AutoSize = true;
            SummaryDeletedLabel.Location = new Point(373, 279);
            SummaryDeletedLabel.Name = "SummaryDeletedLabel";
            SummaryDeletedLabel.Size = new Size(59, 15);
            SummaryDeletedLabel.TabIndex = 33;
            SummaryDeletedLabel.Text = "Deleted: 0";
            // 
            // SummaryUploadSizeLabel
            // 
            SummaryUploadSizeLabel.AutoSize = true;
            SummaryUploadSizeLabel.Location = new Point(12, 690);
            SummaryUploadSizeLabel.Name = "SummaryUploadSizeLabel";
            SummaryUploadSizeLabel.Size = new Size(108, 15);
            SummaryUploadSizeLabel.TabIndex = 34;
            SummaryUploadSizeLabel.Text = "Upload: 0 files / 0 B";
            // 
            // ProgressHeaderLabel
            // 
            ProgressHeaderLabel.AutoSize = true;
            ProgressHeaderLabel.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
            ProgressHeaderLabel.Location = new Point(12, 588);
            ProgressHeaderLabel.Name = "ProgressHeaderLabel";
            ProgressHeaderLabel.Size = new Size(55, 15);
            ProgressHeaderLabel.TabIndex = 35;
            ProgressHeaderLabel.Text = "Progress";
            // 
            // PreviewFilterLabel
            // 
            PreviewFilterLabel.AutoSize = true;
            PreviewFilterLabel.Location = new Point(533, 279);
            PreviewFilterLabel.Name = "PreviewFilterLabel";
            PreviewFilterLabel.Size = new Size(75, 15);
            PreviewFilterLabel.TabIndex = 50;
            PreviewFilterLabel.Text = "Show action:";
            // 
            // PreviewActionFilterDropDown
            // 
            PreviewActionFilterDropDown.DropDownStyle = ComboBoxStyle.DropDownList;
            PreviewActionFilterDropDown.FormattingEnabled = true;
            PreviewActionFilterDropDown.Items.AddRange(new object[] { "All", "Added", "Changed", "Deleted", "Unchanged" });
            PreviewActionFilterDropDown.Location = new Point(614, 276);
            PreviewActionFilterDropDown.Name = "PreviewActionFilterDropDown";
            PreviewActionFilterDropDown.Size = new Size(134, 23);
            PreviewActionFilterDropDown.TabIndex = 51;
            PreviewActionFilterDropDown.SelectedIndexChanged += PreviewActionFilterDropDown_SelectedIndexChanged;
            // 
            // CompareButton
            // 
            CompareButton.Location = new Point(12, 227);
            CompareButton.Name = "CompareButton";
            CompareButton.Size = new Size(118, 28);
            CompareButton.TabIndex = 37;
            CompareButton.Text = "Compare";
            CompareButton.UseVisualStyleBackColor = true;
            CompareButton.Click += CompareButton_Click;
            // 
            // PreviewGrid
            // 
            PreviewGrid.AllowUserToAddRows = false;
            PreviewGrid.AllowUserToDeleteRows = false;
            PreviewGrid.AllowUserToResizeRows = false;
            PreviewGrid.ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            PreviewGrid.Columns.AddRange(new DataGridViewColumn[] { PreviewActionColumn, PreviewFileColumn, PreviewSizeColumn });
            PreviewGrid.Location = new Point(12, 305);
            PreviewGrid.MultiSelect = false;
            PreviewGrid.Name = "PreviewGrid";
            PreviewGrid.ReadOnly = true;
            PreviewGrid.RowHeadersVisible = false;
            PreviewGrid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
            PreviewGrid.Size = new Size(736, 369);
            PreviewGrid.TabIndex = 38;
            // 
            // PreviewActionColumn
            // 
            PreviewActionColumn.HeaderText = "Action";
            PreviewActionColumn.Name = "PreviewActionColumn";
            PreviewActionColumn.ReadOnly = true;
            PreviewActionColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            PreviewActionColumn.Width = 90;
            // 
            // PreviewFileColumn
            // 
            PreviewFileColumn.AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill;
            PreviewFileColumn.HeaderText = "File";
            PreviewFileColumn.Name = "PreviewFileColumn";
            PreviewFileColumn.ReadOnly = true;
            PreviewFileColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            // 
            // PreviewSizeColumn
            // 
            PreviewSizeColumn.HeaderText = "Size";
            PreviewSizeColumn.Name = "PreviewSizeColumn";
            PreviewSizeColumn.ReadOnly = true;
            PreviewSizeColumn.SortMode = DataGridViewColumnSortMode.NotSortable;
            PreviewSizeColumn.Width = 90;
            // 
            // ClearRepositoryButton
            // 
            ClearRepositoryButton.ForeColor = Color.DarkRed;
            ClearRepositoryButton.Location = new Point(498, 227);
            ClearRepositoryButton.Name = "ClearRepositoryButton";
            ClearRepositoryButton.Size = new Size(104, 28);
            ClearRepositoryButton.TabIndex = 52;
            ClearRepositoryButton.Text = "Clear Remote";
            ClearRepositoryButton.UseVisualStyleBackColor = true;
            ClearRepositoryButton.Click += ClearRepositoryButton_Click;
            // 
            // OverallProgressBar
            // 
            OverallProgressBar.Location = new Point(12, 676);
            OverallProgressBar.Name = "OverallProgressBar";
            OverallProgressBar.Size = new Size(736, 7);
            OverallProgressBar.Style = ProgressBarStyle.Continuous;
            OverallProgressBar.TabIndex = 61;
            // 
            // MainStatusStrip
            // 
            MainStatusStrip.Items.AddRange(new ToolStripItem[] { StatusActionLabel, StatusFileLabel });
            MainStatusStrip.Location = new Point(0, 716);
            MainStatusStrip.Name = "MainStatusStrip";
            MainStatusStrip.Size = new Size(760, 22);
            MainStatusStrip.SizingGrip = false;
            MainStatusStrip.TabIndex = 62;
            // 
            // StatusActionLabel
            // 
            StatusActionLabel.Name = "StatusActionLabel";
            StatusActionLabel.Size = new Size(465, 17);
            StatusActionLabel.Spring = true;
            StatusActionLabel.Text = "Idle";
            StatusActionLabel.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // StatusFileLabel
            // 
            StatusFileLabel.AutoSize = false;
            StatusFileLabel.Name = "StatusFileLabel";
            StatusFileLabel.Overflow = ToolStripItemOverflow.Never;
            StatusFileLabel.Size = new Size(280, 17);
            StatusFileLabel.TextAlign = ContentAlignment.MiddleRight;
            // 
            // AMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(760, 738);
            Controls.Add(MainStatusStrip);
            Controls.Add(OverallProgressBar);
            Controls.Add(AbortButton);
            Controls.Add(TestConnectionButton);
            Controls.Add(PreviewActionFilterDropDown);
            Controls.Add(PreviewFilterLabel);
            Controls.Add(PreviewGrid);
            Controls.Add(CompareButton);
            Controls.Add(SummaryUploadSizeLabel);
            Controls.Add(SummaryDeletedLabel);
            Controls.Add(SummaryUnchangedLabel);
            Controls.Add(SummaryChangedLabel);
            Controls.Add(SummaryAddedLabel);
            Controls.Add(SummaryHeaderLabel);
            Controls.Add(ActionsHeaderLabel);
            Controls.Add(ConnectionHeaderLabel);
            Controls.Add(PortHintLabel);
            Controls.Add(PortLabel);
            Controls.Add(PortNumericUpDown);
            Controls.Add(label6);
            Controls.Add(ProtocolDropDown);
            Controls.Add(DownloadExistingButton);
            Controls.Add(CompressFilesHintLabel);
            Controls.Add(CompressFilesCheckBox);
            Controls.Add(AllowCleanHintLabel);
            Controls.Add(AllowCleanCheckBox);
            Controls.Add(ClearRepositoryButton);
            Controls.Add(ListButton);
            Controls.Add(ProcessButton);
            Controls.Add(label4);
            Controls.Add(PasswordTextBox);
            Controls.Add(label3);
            Controls.Add(LoginTextBox);
            Controls.Add(label2);
            Controls.Add(HostTextBox);
            Controls.Add(BrowseClientButton);
            Controls.Add(label1);
            Controls.Add(ClientTextBox);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Auto Patcher Admin";
            Load += AMain_Load;
            ((System.ComponentModel.ISupportInitialize)PortNumericUpDown).EndInit();
            ((System.ComponentModel.ISupportInitialize)PreviewGrid).EndInit();
            MainStatusStrip.ResumeLayout(false);
            MainStatusStrip.PerformLayout();
            ResumeLayout(false);
            PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox ClientTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox HostTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox LoginTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox PasswordTextBox;
        private System.Windows.Forms.Button ProcessButton;
        private System.Windows.Forms.Label ActionLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label SpeedLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button ListButton;
        private System.Windows.Forms.CheckBox AllowCleanCheckBox;
        private System.Windows.Forms.Label AllowCleanHintLabel;
        private System.Windows.Forms.CheckBox CompressFilesCheckBox;
        private System.Windows.Forms.Label CompressFilesHintLabel;
        private System.Windows.Forms.Button BrowseClientButton;
        private System.Windows.Forms.Button DownloadExistingButton;
        private System.Windows.Forms.ComboBox ProtocolDropDown;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.NumericUpDown PortNumericUpDown;
        private System.Windows.Forms.Label PortLabel;
        private System.Windows.Forms.Label PortHintLabel;
        private System.Windows.Forms.Label ConnectionHeaderLabel;
        private System.Windows.Forms.Label ActionsHeaderLabel;
        private System.Windows.Forms.Label SummaryHeaderLabel;
        private System.Windows.Forms.Label SummaryAddedLabel;
        private System.Windows.Forms.Label SummaryChangedLabel;
        private System.Windows.Forms.Label SummaryUnchangedLabel;
        private System.Windows.Forms.Label SummaryDeletedLabel;
        private System.Windows.Forms.Label SummaryUploadSizeLabel;
        private System.Windows.Forms.Label ProgressHeaderLabel;
        private System.Windows.Forms.Label PreviewFilterLabel;
        private System.Windows.Forms.ComboBox PreviewActionFilterDropDown;
        private System.Windows.Forms.Button CompareButton;
        private System.Windows.Forms.DataGridView PreviewGrid;
        private System.Windows.Forms.DataGridViewTextBoxColumn PreviewActionColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PreviewFileColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn PreviewSizeColumn;
        private System.Windows.Forms.Button ClearRepositoryButton;
        private System.Windows.Forms.ProgressBar OverallProgressBar;
        private System.Windows.Forms.Button TestConnectionButton;
        private System.Windows.Forms.Button AbortButton;
        private System.Windows.Forms.StatusStrip MainStatusStrip;
        private System.Windows.Forms.ToolStripStatusLabel StatusActionLabel;
        private System.Windows.Forms.ToolStripStatusLabel StatusFileLabel;
    }
}
