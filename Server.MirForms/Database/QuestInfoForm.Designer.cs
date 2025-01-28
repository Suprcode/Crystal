namespace Server
{
    partial class QuestInfoForm
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
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            QuestInfoPanel = new Panel();
            label5 = new Label();
            TimeLimitTextBox = new TextBox();
            label4 = new Label();
            RequiredMaxLevelTextBox = new TextBox();
            label3 = new Label();
            QFlagTextBox = new TextBox();
            label14 = new Label();
            label12 = new Label();
            label10 = new Label();
            QItemTextBox = new TextBox();
            QKillTextBox = new TextBox();
            QGotoTextBox = new TextBox();
            label9 = new Label();
            label8 = new Label();
            label7 = new Label();
            RequiredClassComboBox = new ComboBox();
            RequiredQuestComboBox = new ComboBox();
            RequiredMinLevelTextBox = new TextBox();
            label2 = new Label();
            QTypeComboBox = new ComboBox();
            label11 = new Label();
            OpenQButton = new Button();
            QFileNameTextBox = new TextBox();
            label29 = new Label();
            QGroupTextBox = new TextBox();
            QNameTextBox = new TextBox();
            label13 = new Label();
            QuestIndexTextBox = new TextBox();
            label1 = new Label();
            RemoveButton = new Button();
            AddButton = new Button();
            QuestInfoListBox = new ListBox();
            PasteMButton = new Button();
            CopyMButton = new Button();
            ExportButton = new Button();
            ImportButton = new Button();
            ExportSelectedButton = new Button();
            QuestSearchBox = new TextBox();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            QuestInfoPanel.SuspendLayout();
            SuspendLayout();
            // 
            // tabControl1
            // 
            tabControl1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Location = new Point(203, 47);
            tabControl1.Margin = new Padding(4, 3, 4, 3);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(702, 331);
            tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(QuestInfoPanel);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new Size(694, 303);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Info";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // QuestInfoPanel
            // 
            QuestInfoPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            QuestInfoPanel.Controls.Add(label5);
            QuestInfoPanel.Controls.Add(TimeLimitTextBox);
            QuestInfoPanel.Controls.Add(label4);
            QuestInfoPanel.Controls.Add(RequiredMaxLevelTextBox);
            QuestInfoPanel.Controls.Add(label3);
            QuestInfoPanel.Controls.Add(QFlagTextBox);
            QuestInfoPanel.Controls.Add(label14);
            QuestInfoPanel.Controls.Add(label12);
            QuestInfoPanel.Controls.Add(label10);
            QuestInfoPanel.Controls.Add(QItemTextBox);
            QuestInfoPanel.Controls.Add(QKillTextBox);
            QuestInfoPanel.Controls.Add(QGotoTextBox);
            QuestInfoPanel.Controls.Add(label9);
            QuestInfoPanel.Controls.Add(label8);
            QuestInfoPanel.Controls.Add(label7);
            QuestInfoPanel.Controls.Add(RequiredClassComboBox);
            QuestInfoPanel.Controls.Add(RequiredQuestComboBox);
            QuestInfoPanel.Controls.Add(RequiredMinLevelTextBox);
            QuestInfoPanel.Controls.Add(label2);
            QuestInfoPanel.Controls.Add(QTypeComboBox);
            QuestInfoPanel.Controls.Add(label11);
            QuestInfoPanel.Controls.Add(OpenQButton);
            QuestInfoPanel.Controls.Add(QFileNameTextBox);
            QuestInfoPanel.Controls.Add(label29);
            QuestInfoPanel.Controls.Add(QGroupTextBox);
            QuestInfoPanel.Controls.Add(QNameTextBox);
            QuestInfoPanel.Controls.Add(label13);
            QuestInfoPanel.Controls.Add(QuestIndexTextBox);
            QuestInfoPanel.Controls.Add(label1);
            QuestInfoPanel.Enabled = false;
            QuestInfoPanel.Location = new Point(4, 7);
            QuestInfoPanel.Margin = new Padding(4, 3, 4, 3);
            QuestInfoPanel.Name = "QuestInfoPanel";
            QuestInfoPanel.Size = new Size(682, 287);
            QuestInfoPanel.TabIndex = 11;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(404, 160);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(122, 15);
            label5.TabIndex = 59;
            label5.Text = "Time Limit (Seconds):";
            // 
            // TimeLimitTextBox
            // 
            TimeLimitTextBox.Location = new Point(532, 157);
            TimeLimitTextBox.Margin = new Padding(4, 3, 4, 3);
            TimeLimitTextBox.Name = "TimeLimitTextBox";
            TimeLimitTextBox.Size = new Size(140, 23);
            TimeLimitTextBox.TabIndex = 58;
            TimeLimitTextBox.TextChanged += TimeLimitTextBox_TextChanged;
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(407, 38);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(112, 15);
            label4.TabIndex = 57;
            label4.Text = "Required Max Level:";
            // 
            // RequiredMaxLevelTextBox
            // 
            RequiredMaxLevelTextBox.Location = new Point(532, 32);
            RequiredMaxLevelTextBox.Margin = new Padding(4, 3, 4, 3);
            RequiredMaxLevelTextBox.MaxLength = 3;
            RequiredMaxLevelTextBox.Name = "RequiredMaxLevelTextBox";
            RequiredMaxLevelTextBox.Size = new Size(140, 23);
            RequiredMaxLevelTextBox.TabIndex = 56;
            RequiredMaxLevelTextBox.TextChanged += RequiredMaxLevelTextBox_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(31, 254);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(56, 15);
            label3.TabIndex = 55;
            label3.Text = "Flag Text:";
            // 
            // QFlagTextBox
            // 
            QFlagTextBox.Location = new Point(102, 250);
            QFlagTextBox.Margin = new Padding(4, 3, 4, 3);
            QFlagTextBox.Name = "QFlagTextBox";
            QFlagTextBox.Size = new Size(209, 23);
            QFlagTextBox.TabIndex = 54;
            QFlagTextBox.TextChanged += QFlagTextBox_TextChanged;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(31, 223);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(58, 15);
            label14.TabIndex = 53;
            label14.Text = "Item Text:";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(40, 192);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(50, 15);
            label12.TabIndex = 52;
            label12.Text = "Kill Text:";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(28, 160);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(60, 15);
            label10.TabIndex = 51;
            label10.Text = "Goto Text:";
            // 
            // QItemTextBox
            // 
            QItemTextBox.Location = new Point(102, 219);
            QItemTextBox.Margin = new Padding(4, 3, 4, 3);
            QItemTextBox.Name = "QItemTextBox";
            QItemTextBox.Size = new Size(209, 23);
            QItemTextBox.TabIndex = 49;
            QItemTextBox.TextChanged += QItemTextBox_TextChanged;
            // 
            // QKillTextBox
            // 
            QKillTextBox.Location = new Point(102, 188);
            QKillTextBox.Margin = new Padding(4, 3, 4, 3);
            QKillTextBox.Name = "QKillTextBox";
            QKillTextBox.Size = new Size(209, 23);
            QKillTextBox.TabIndex = 48;
            QKillTextBox.TextChanged += QKillTextBox_TextChanged;
            // 
            // QGotoTextBox
            // 
            QGotoTextBox.Location = new Point(102, 157);
            QGotoTextBox.Margin = new Padding(4, 3, 4, 3);
            QGotoTextBox.Name = "QGotoTextBox";
            QGotoTextBox.Size = new Size(209, 23);
            QGotoTextBox.TabIndex = 47;
            QGotoTextBox.TextChanged += QGotoTextBox_TextChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(435, 98);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(87, 15);
            label9.TabIndex = 46;
            label9.Text = "Required Class:";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(432, 68);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(91, 15);
            label8.TabIndex = 45;
            label8.Text = "Required Quest:";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(411, 8);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(111, 15);
            label7.TabIndex = 44;
            label7.Text = "Required Min Level:";
            // 
            // RequiredClassComboBox
            // 
            RequiredClassComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            RequiredClassComboBox.FormattingEnabled = true;
            RequiredClassComboBox.Location = new Point(532, 95);
            RequiredClassComboBox.Margin = new Padding(4, 3, 4, 3);
            RequiredClassComboBox.Name = "RequiredClassComboBox";
            RequiredClassComboBox.Size = new Size(140, 23);
            RequiredClassComboBox.TabIndex = 43;
            RequiredClassComboBox.SelectedIndexChanged += RequiredClassComboBox_SelectedIndexChanged;
            // 
            // RequiredQuestComboBox
            // 
            RequiredQuestComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            RequiredQuestComboBox.FormattingEnabled = true;
            RequiredQuestComboBox.Location = new Point(532, 62);
            RequiredQuestComboBox.Margin = new Padding(4, 3, 4, 3);
            RequiredQuestComboBox.Name = "RequiredQuestComboBox";
            RequiredQuestComboBox.Size = new Size(140, 23);
            RequiredQuestComboBox.TabIndex = 42;
            RequiredQuestComboBox.SelectedIndexChanged += RequiredQuestComboBox_SelectedIndexChanged;
            // 
            // RequiredMinLevelTextBox
            // 
            RequiredMinLevelTextBox.Location = new Point(532, 5);
            RequiredMinLevelTextBox.Margin = new Padding(4, 3, 4, 3);
            RequiredMinLevelTextBox.MaxLength = 3;
            RequiredMinLevelTextBox.Name = "RequiredMinLevelTextBox";
            RequiredMinLevelTextBox.Size = new Size(140, 23);
            RequiredMinLevelTextBox.TabIndex = 41;
            RequiredMinLevelTextBox.TextChanged += RequiredMinLevelTextBox_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(55, 98);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(35, 15);
            label2.TabIndex = 32;
            label2.Text = "Type:";
            // 
            // QTypeComboBox
            // 
            QTypeComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            QTypeComboBox.FormattingEnabled = true;
            QTypeComboBox.Location = new Point(102, 95);
            QTypeComboBox.Margin = new Padding(4, 3, 4, 3);
            QTypeComboBox.Name = "QTypeComboBox";
            QTypeComboBox.Size = new Size(209, 23);
            QTypeComboBox.TabIndex = 31;
            QTypeComboBox.SelectedIndexChanged += QTypeComboBox_SelectedIndexChanged;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(28, 129);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(63, 15);
            label11.TabIndex = 23;
            label11.Text = "File Name:";
            // 
            // OpenQButton
            // 
            OpenQButton.Location = new Point(323, 123);
            OpenQButton.Margin = new Padding(4, 3, 4, 3);
            OpenQButton.Name = "OpenQButton";
            OpenQButton.Size = new Size(88, 27);
            OpenQButton.TabIndex = 30;
            OpenQButton.Text = "Open Script";
            OpenQButton.UseVisualStyleBackColor = true;
            OpenQButton.Click += OpenQButton_Click;
            // 
            // QFileNameTextBox
            // 
            QFileNameTextBox.Location = new Point(102, 126);
            QFileNameTextBox.Margin = new Padding(4, 3, 4, 3);
            QFileNameTextBox.Name = "QFileNameTextBox";
            QFileNameTextBox.Size = new Size(209, 23);
            QFileNameTextBox.TabIndex = 22;
            QFileNameTextBox.TextChanged += QFileNameTextBox_TextChanged;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(49, 68);
            label29.Margin = new Padding(4, 0, 4, 0);
            label29.Name = "label29";
            label29.Size = new Size(43, 15);
            label29.TabIndex = 21;
            label29.Text = "Group:";
            // 
            // QGroupTextBox
            // 
            QGroupTextBox.Location = new Point(102, 65);
            QGroupTextBox.Margin = new Padding(4, 3, 4, 3);
            QGroupTextBox.MaxLength = 20;
            QGroupTextBox.Name = "QGroupTextBox";
            QGroupTextBox.Size = new Size(209, 23);
            QGroupTextBox.TabIndex = 20;
            QGroupTextBox.TextChanged += QGroupTextBox_TextChanged;
            // 
            // QNameTextBox
            // 
            QNameTextBox.Location = new Point(102, 35);
            QNameTextBox.Margin = new Padding(4, 3, 4, 3);
            QNameTextBox.MaxLength = 30;
            QNameTextBox.Name = "QNameTextBox";
            QNameTextBox.Size = new Size(209, 23);
            QNameTextBox.TabIndex = 14;
            QNameTextBox.TextChanged += QNameTextBox_TextChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(50, 38);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(42, 15);
            label13.TabIndex = 15;
            label13.Text = "Name:";
            // 
            // QuestIndexTextBox
            // 
            QuestIndexTextBox.Location = new Point(102, 5);
            QuestIndexTextBox.Margin = new Padding(4, 3, 4, 3);
            QuestIndexTextBox.Name = "QuestIndexTextBox";
            QuestIndexTextBox.ReadOnly = true;
            QuestIndexTextBox.Size = new Size(54, 23);
            QuestIndexTextBox.TabIndex = 0;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(16, 8);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(72, 15);
            label1.TabIndex = 4;
            label1.Text = "Quest Index:";
            // 
            // RemoveButton
            // 
            RemoveButton.Location = new Point(108, 14);
            RemoveButton.Margin = new Padding(4, 3, 4, 3);
            RemoveButton.Name = "RemoveButton";
            RemoveButton.Size = new Size(88, 27);
            RemoveButton.TabIndex = 14;
            RemoveButton.Text = "Remove";
            RemoveButton.UseVisualStyleBackColor = true;
            RemoveButton.Click += RemoveButton_Click;
            // 
            // AddButton
            // 
            AddButton.Location = new Point(14, 14);
            AddButton.Margin = new Padding(4, 3, 4, 3);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(88, 27);
            AddButton.TabIndex = 13;
            AddButton.Text = "Add";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // QuestInfoListBox
            // 
            QuestInfoListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            QuestInfoListBox.FormattingEnabled = true;
            QuestInfoListBox.ItemHeight = 15;
            QuestInfoListBox.Location = new Point(13, 70);
            QuestInfoListBox.Margin = new Padding(4, 3, 4, 3);
            QuestInfoListBox.Name = "QuestInfoListBox";
            QuestInfoListBox.SelectionMode = SelectionMode.MultiExtended;
            QuestInfoListBox.Size = new Size(181, 304);
            QuestInfoListBox.TabIndex = 15;
            QuestInfoListBox.SelectedIndexChanged += QuestInfoListBox_SelectedIndexChanged;
            // 
            // PasteMButton
            // 
            PasteMButton.Location = new Point(298, 14);
            PasteMButton.Margin = new Padding(4, 3, 4, 3);
            PasteMButton.Name = "PasteMButton";
            PasteMButton.Size = new Size(88, 27);
            PasteMButton.TabIndex = 22;
            PasteMButton.Text = "Paste";
            PasteMButton.UseVisualStyleBackColor = true;
            PasteMButton.Click += PasteMButton_Click;
            // 
            // CopyMButton
            // 
            CopyMButton.Location = new Point(203, 14);
            CopyMButton.Margin = new Padding(4, 3, 4, 3);
            CopyMButton.Name = "CopyMButton";
            CopyMButton.Size = new Size(88, 27);
            CopyMButton.TabIndex = 21;
            CopyMButton.Text = "Copy";
            CopyMButton.UseVisualStyleBackColor = true;
            // 
            // ExportButton
            // 
            ExportButton.Location = new Point(818, 14);
            ExportButton.Margin = new Padding(4, 3, 4, 3);
            ExportButton.Name = "ExportButton";
            ExportButton.Size = new Size(88, 27);
            ExportButton.TabIndex = 23;
            ExportButton.Text = "Export All";
            ExportButton.UseVisualStyleBackColor = true;
            ExportButton.Click += ExportAllButton_Click;
            // 
            // ImportButton
            // 
            ImportButton.Location = new Point(581, 14);
            ImportButton.Margin = new Padding(4, 3, 4, 3);
            ImportButton.Name = "ImportButton";
            ImportButton.Size = new Size(88, 27);
            ImportButton.TabIndex = 24;
            ImportButton.Text = "Import";
            ImportButton.UseVisualStyleBackColor = true;
            ImportButton.Click += ImportButton_Click;
            // 
            // ExportSelectedButton
            // 
            ExportSelectedButton.Location = new Point(674, 14);
            ExportSelectedButton.Margin = new Padding(4, 3, 4, 3);
            ExportSelectedButton.Name = "ExportSelectedButton";
            ExportSelectedButton.Size = new Size(136, 27);
            ExportSelectedButton.TabIndex = 25;
            ExportSelectedButton.Text = "Export Selected";
            ExportSelectedButton.UseVisualStyleBackColor = true;
            ExportSelectedButton.Click += ExportSelected_Click;
            // 
            // QuestSearchBox
            // 
            QuestSearchBox.Location = new Point(12, 43);
            QuestSearchBox.Name = "QuestSearchBox";
            QuestSearchBox.PlaceholderText = "Search...";
            QuestSearchBox.Size = new Size(182, 23);
            QuestSearchBox.TabIndex = 26;
            QuestSearchBox.TextChanged += QuestSearchBox_TextChanged;
            // 
            // QuestInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(919, 384);
            Controls.Add(QuestSearchBox);
            Controls.Add(ExportSelectedButton);
            Controls.Add(ImportButton);
            Controls.Add(ExportButton);
            Controls.Add(PasteMButton);
            Controls.Add(CopyMButton);
            Controls.Add(tabControl1);
            Controls.Add(RemoveButton);
            Controls.Add(AddButton);
            Controls.Add(QuestInfoListBox);
            Margin = new Padding(4, 3, 4, 3);
            Name = "QuestInfoForm";
            Text = "QuestInfoForm";
            FormClosed += QuestInfoForm_FormClosed;
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            QuestInfoPanel.ResumeLayout(false);
            QuestInfoPanel.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.Panel QuestInfoPanel;
        private System.Windows.Forms.TextBox QuestIndexTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button RemoveButton;
        private System.Windows.Forms.Button AddButton;
        private System.Windows.Forms.Button PasteMButton;
        private System.Windows.Forms.Button CopyMButton;
        private System.Windows.Forms.Button ExportButton;
        private System.Windows.Forms.Button ImportButton;
        private System.Windows.Forms.Button ExportSelectedButton;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button OpenQButton;
        private System.Windows.Forms.TextBox QFileNameTextBox;
        private System.Windows.Forms.Label label29;
        private System.Windows.Forms.TextBox QGroupTextBox;
        private System.Windows.Forms.TextBox QNameTextBox;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.ListBox QuestInfoListBox;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox QTypeComboBox;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox RequiredClassComboBox;
        private System.Windows.Forms.ComboBox RequiredQuestComboBox;
        private System.Windows.Forms.TextBox RequiredMinLevelTextBox;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.TextBox QItemTextBox;
        private System.Windows.Forms.TextBox QKillTextBox;
        private System.Windows.Forms.TextBox QGotoTextBox;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox QFlagTextBox;
        private System.Windows.Forms.TextBox RequiredMaxLevelTextBox;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox TimeLimitTextBox;
        private TextBox QuestSearchBox;
    }
}