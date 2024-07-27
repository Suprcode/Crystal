using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Timer = System.Windows.Forms.Timer;

namespace Server
{
    partial class SMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private IContainer components = null;

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
            components = new Container();
            MainTabs = new TabControl();
            tabPage1 = new TabPage();
            LogTextBox = new TextBox();
            tabPage2 = new TabPage();
            DebugLogTextBox = new TextBox();
            tabPage3 = new TabPage();
            groupBox1 = new GroupBox();
            GlobalMessageButton = new Button();
            GlobalMessageTextBox = new TextBox();
            ChatLogTextBox = new TextBox();
            tabPage4 = new TabPage();
            PlayersOnlineListView = new CustomFormControl.ListViewNF();
            indexHeader = new ColumnHeader();
            nameHeader = new ColumnHeader();
            levelHeader = new ColumnHeader();
            classHeader = new ColumnHeader();
            genderHeader = new ColumnHeader();
            tabPage5 = new TabPage();
            GuildListView = new CustomFormControl.ListViewNF();
            columnHeader1 = new ColumnHeader();
            columnHeader2 = new ColumnHeader();
            columnHeader3 = new ColumnHeader();
            columnHeader4 = new ColumnHeader();
            columnHeader5 = new ColumnHeader();
            columnHeader6 = new ColumnHeader();
            StatusBar = new StatusStrip();
            PlayersLabel = new ToolStripStatusLabel();
            MonsterLabel = new ToolStripStatusLabel();
            ConnectionsLabel = new ToolStripStatusLabel();
            BlockedIPsLabel = new ToolStripStatusLabel();
            CycleDelayLabel = new ToolStripStatusLabel();
            MainMenu = new MenuStrip();
            controlToolStripMenuItem = new ToolStripMenuItem();
            startServerToolStripMenuItem = new ToolStripMenuItem();
            stopServerToolStripMenuItem = new ToolStripMenuItem();
            rebootServerToolStripMenuItem = new ToolStripMenuItem();
            clearBlockedIPsToolStripMenuItem = new ToolStripMenuItem();
            toolStripMenuItem1 = new ToolStripSeparator();
            toolStripSeparator1 = new ToolStripSeparator();
            closeServerToolStripMenuItem = new ToolStripMenuItem();
            reloadToolStripMenuItem = new ToolStripMenuItem();
            nPCsToolStripMenuItem = new ToolStripMenuItem();
            dropsToolStripMenuItem = new ToolStripMenuItem();
            lineMessageToolStripMenuItem = new ToolStripMenuItem();
            accountToolStripMenuItem = new ToolStripMenuItem();
            databaseFormsToolStripMenuItem = new ToolStripMenuItem();
            mapInfoToolStripMenuItem = new ToolStripMenuItem();
            itemInfoToolStripMenuItem = new ToolStripMenuItem();
            monsterInfoToolStripMenuItem = new ToolStripMenuItem();
            itemNEWToolStripMenuItem = new ToolStripMenuItem();
            monsterExperimentalToolStripMenuItem = new ToolStripMenuItem();
            nPCInfoToolStripMenuItem = new ToolStripMenuItem();
            questInfoToolStripMenuItem = new ToolStripMenuItem();
            magicInfoToolStripMenuItem = new ToolStripMenuItem();
            gameshopToolStripMenuItem = new ToolStripMenuItem();
            configToolStripMenuItem1 = new ToolStripMenuItem();
            serverToolStripMenuItem = new ToolStripMenuItem();
            balanceToolStripMenuItem = new ToolStripMenuItem();
            systemToolStripMenuItem = new ToolStripMenuItem();
            dragonSystemToolStripMenuItem = new ToolStripMenuItem();
            miningToolStripMenuItem = new ToolStripMenuItem();
            guildsToolStripMenuItem = new ToolStripMenuItem();
            fishingToolStripMenuItem = new ToolStripMenuItem();
            mailToolStripMenuItem = new ToolStripMenuItem();
            goodsToolStripMenuItem = new ToolStripMenuItem();
            refiningToolStripMenuItem = new ToolStripMenuItem();
            relationshipToolStripMenuItem = new ToolStripMenuItem();
            mentorToolStripMenuItem = new ToolStripMenuItem();
            gemToolStripMenuItem = new ToolStripMenuItem();
            conquestToolStripMenuItem = new ToolStripMenuItem();
            respawnsToolStripMenuItem = new ToolStripMenuItem();
            heroesToolStripMenuItem = new ToolStripMenuItem();
            monsterTunerToolStripMenuItem = new ToolStripMenuItem();
            dropBuilderToolStripMenuItem = new ToolStripMenuItem();
            CharacterToolStripMenuItem = new ToolStripMenuItem();
            UpTimeLabel = new ToolStripTextBox();
            InterfaceTimer = new Timer(components);
            recipeToolStripMenuItem = new ToolStripMenuItem();
            MainTabs.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            groupBox1.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            StatusBar.SuspendLayout();
            MainMenu.SuspendLayout();
            SuspendLayout();
            // 
            // MainTabs
            // 
            MainTabs.Controls.Add(tabPage1);
            MainTabs.Controls.Add(tabPage2);
            MainTabs.Controls.Add(tabPage3);
            MainTabs.Controls.Add(tabPage4);
            MainTabs.Controls.Add(tabPage5);
            MainTabs.Dock = DockStyle.Fill;
            MainTabs.Location = new Point(0, 24);
            MainTabs.Margin = new Padding(4, 3, 4, 3);
            MainTabs.Name = "MainTabs";
            MainTabs.SelectedIndex = 0;
            MainTabs.Size = new Size(566, 407);
            MainTabs.TabIndex = 5;
            MainTabs.SelectedIndexChanged += MainTabs_SelectedIndexChanged;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(LogTextBox);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new Size(558, 379);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Logs";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // LogTextBox
            // 
            LogTextBox.Dock = DockStyle.Fill;
            LogTextBox.Location = new Point(4, 3);
            LogTextBox.Margin = new Padding(4, 3, 4, 3);
            LogTextBox.Multiline = true;
            LogTextBox.Name = "LogTextBox";
            LogTextBox.ReadOnly = true;
            LogTextBox.ScrollBars = ScrollBars.Vertical;
            LogTextBox.Size = new Size(550, 373);
            LogTextBox.TabIndex = 2;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(DebugLogTextBox);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4, 3, 4, 3);
            tabPage2.Size = new Size(558, 379);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Debug Logs";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // DebugLogTextBox
            // 
            DebugLogTextBox.Dock = DockStyle.Fill;
            DebugLogTextBox.Location = new Point(4, 3);
            DebugLogTextBox.Margin = new Padding(4, 3, 4, 3);
            DebugLogTextBox.Multiline = true;
            DebugLogTextBox.Name = "DebugLogTextBox";
            DebugLogTextBox.ReadOnly = true;
            DebugLogTextBox.ScrollBars = ScrollBars.Vertical;
            DebugLogTextBox.Size = new Size(550, 373);
            DebugLogTextBox.TabIndex = 3;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(groupBox1);
            tabPage3.Controls.Add(ChatLogTextBox);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Margin = new Padding(4, 3, 4, 3);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(4, 3, 4, 3);
            tabPage3.Size = new Size(558, 379);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Chat Logs";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(GlobalMessageButton);
            groupBox1.Controls.Add(GlobalMessageTextBox);
            groupBox1.Dock = DockStyle.Bottom;
            groupBox1.Location = new Point(4, 323);
            groupBox1.Margin = new Padding(4, 3, 4, 3);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new Padding(4, 3, 4, 3);
            groupBox1.Size = new Size(550, 53);
            groupBox1.TabIndex = 7;
            groupBox1.TabStop = false;
            groupBox1.Text = "Send Message";
            // 
            // GlobalMessageButton
            // 
            GlobalMessageButton.Location = new Point(457, 16);
            GlobalMessageButton.Margin = new Padding(4, 3, 4, 3);
            GlobalMessageButton.Name = "GlobalMessageButton";
            GlobalMessageButton.Size = new Size(85, 28);
            GlobalMessageButton.TabIndex = 0;
            GlobalMessageButton.Text = "Send";
            GlobalMessageButton.UseVisualStyleBackColor = true;
            GlobalMessageButton.Click += GlobalMessageButton_Click;
            // 
            // GlobalMessageTextBox
            // 
            GlobalMessageTextBox.Location = new Point(7, 20);
            GlobalMessageTextBox.Margin = new Padding(4, 3, 4, 3);
            GlobalMessageTextBox.Name = "GlobalMessageTextBox";
            GlobalMessageTextBox.Size = new Size(443, 23);
            GlobalMessageTextBox.TabIndex = 0;
            // 
            // ChatLogTextBox
            // 
            ChatLogTextBox.Location = new Point(4, 3);
            ChatLogTextBox.Margin = new Padding(4, 3, 4, 3);
            ChatLogTextBox.Multiline = true;
            ChatLogTextBox.Name = "ChatLogTextBox";
            ChatLogTextBox.ReadOnly = true;
            ChatLogTextBox.ScrollBars = ScrollBars.Vertical;
            ChatLogTextBox.Size = new Size(549, 310);
            ChatLogTextBox.TabIndex = 4;
            // 
            // tabPage4
            // 
            tabPage4.BackColor = SystemColors.Control;
            tabPage4.Controls.Add(PlayersOnlineListView);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Margin = new Padding(4, 3, 4, 3);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(4, 3, 4, 3);
            tabPage4.Size = new Size(558, 379);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Players Online";
            // 
            // PlayersOnlineListView
            // 
            PlayersOnlineListView.Activation = ItemActivation.OneClick;
            PlayersOnlineListView.BackColor = SystemColors.Window;
            PlayersOnlineListView.Columns.AddRange(new ColumnHeader[] { indexHeader, nameHeader, levelHeader, classHeader, genderHeader });
            PlayersOnlineListView.Dock = DockStyle.Fill;
            PlayersOnlineListView.FullRowSelect = true;
            PlayersOnlineListView.GridLines = true;
            PlayersOnlineListView.Location = new Point(4, 3);
            PlayersOnlineListView.Margin = new Padding(4, 3, 4, 3);
            PlayersOnlineListView.Name = "PlayersOnlineListView";
            PlayersOnlineListView.Size = new Size(550, 373);
            PlayersOnlineListView.Sorting = SortOrder.Ascending;
            PlayersOnlineListView.TabIndex = 0;
            PlayersOnlineListView.UseCompatibleStateImageBehavior = false;
            PlayersOnlineListView.View = View.Details;
            PlayersOnlineListView.ColumnWidthChanging += PlayersOnlineListView_ColumnWidthChanging;
            PlayersOnlineListView.DoubleClick += PlayersOnlineListView_DoubleClick;
            // 
            // indexHeader
            // 
            indexHeader.Text = "Index";
            indexHeader.Width = 71;
            // 
            // nameHeader
            // 
            nameHeader.Text = "Name";
            nameHeader.Width = 93;
            // 
            // levelHeader
            // 
            levelHeader.Text = "Level";
            levelHeader.Width = 90;
            // 
            // classHeader
            // 
            classHeader.Text = "Class";
            classHeader.Width = 100;
            // 
            // genderHeader
            // 
            genderHeader.Text = "Gender";
            genderHeader.Width = 98;
            // 
            // tabPage5
            // 
            tabPage5.Controls.Add(GuildListView);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(558, 379);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "Guilds";
            tabPage5.UseVisualStyleBackColor = true;
            // 
            // GuildListView
            // 
            GuildListView.Activation = ItemActivation.OneClick;
            GuildListView.Columns.AddRange(new ColumnHeader[] { columnHeader1, columnHeader2, columnHeader3, columnHeader4, columnHeader5, columnHeader6 });
            GuildListView.Dock = DockStyle.Fill;
            GuildListView.FullRowSelect = true;
            GuildListView.GridLines = true;
            GuildListView.Location = new Point(3, 3);
            GuildListView.Name = "GuildListView";
            GuildListView.Size = new Size(552, 373);
            GuildListView.TabIndex = 1;
            GuildListView.UseCompatibleStateImageBehavior = false;
            GuildListView.View = View.Details;
            GuildListView.DoubleClick += GuildListView_DoubleClick;
            // 
            // columnHeader1
            // 
            columnHeader1.Text = "Index";
            // 
            // columnHeader2
            // 
            columnHeader2.Text = "Name";
            columnHeader2.Width = 115;
            // 
            // columnHeader3
            // 
            columnHeader3.Text = "Leader";
            columnHeader3.Width = 130;
            // 
            // columnHeader4
            // 
            columnHeader4.Text = "Member Count";
            columnHeader4.Width = 100;
            // 
            // columnHeader5
            // 
            columnHeader5.Text = "Level";
            columnHeader5.Width = 75;
            // 
            // columnHeader6
            // 
            columnHeader6.Text = "Gold";
            columnHeader6.Width = 75;
            // 
            // StatusBar
            // 
            StatusBar.Items.AddRange(new ToolStripItem[] { PlayersLabel, MonsterLabel, ConnectionsLabel, BlockedIPsLabel, CycleDelayLabel });
            StatusBar.Location = new Point(0, 431);
            StatusBar.Name = "StatusBar";
            StatusBar.Padding = new Padding(1, 0, 16, 0);
            StatusBar.Size = new Size(566, 24);
            StatusBar.SizingGrip = false;
            StatusBar.TabIndex = 4;
            StatusBar.Text = "statusStrip1";
            // 
            // PlayersLabel
            // 
            PlayersLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            PlayersLabel.Name = "PlayersLabel";
            PlayersLabel.Size = new Size(60, 19);
            PlayersLabel.Text = "Players: 0";
            // 
            // MonsterLabel
            // 
            MonsterLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            MonsterLabel.Name = "MonsterLabel";
            MonsterLabel.Size = new Size(72, 19);
            MonsterLabel.Text = "Monsters: 0";
            // 
            // ConnectionsLabel
            // 
            ConnectionsLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            ConnectionsLabel.Name = "ConnectionsLabel";
            ConnectionsLabel.Size = new Size(90, 19);
            ConnectionsLabel.Text = "Connections: 0";
            // 
            // BlockedIPsLabel
            // 
            BlockedIPsLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            BlockedIPsLabel.Name = "BlockedIPsLabel";
            BlockedIPsLabel.Size = new Size(83, 19);
            BlockedIPsLabel.Text = "Blocked IPs: 0";
            // 
            // CycleDelayLabel
            // 
            CycleDelayLabel.BorderSides = ToolStripStatusLabelBorderSides.Left | ToolStripStatusLabelBorderSides.Top | ToolStripStatusLabelBorderSides.Right | ToolStripStatusLabelBorderSides.Bottom;
            CycleDelayLabel.Name = "CycleDelayLabel";
            CycleDelayLabel.Size = new Size(81, 19);
            CycleDelayLabel.Text = "CycleDelay: 0";
            // 
            // MainMenu
            // 
            MainMenu.BackColor = Color.Transparent;
            MainMenu.Items.AddRange(new ToolStripItem[] { controlToolStripMenuItem, accountToolStripMenuItem, databaseFormsToolStripMenuItem, configToolStripMenuItem1, CharacterToolStripMenuItem, UpTimeLabel });
            MainMenu.Location = new Point(0, 0);
            MainMenu.Name = "MainMenu";
            MainMenu.Padding = new Padding(7, 2, 0, 2);
            MainMenu.Size = new Size(566, 24);
            MainMenu.TabIndex = 3;
            MainMenu.Text = "menuStrip1";
            // 
            // controlToolStripMenuItem
            // 
            controlToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { startServerToolStripMenuItem, stopServerToolStripMenuItem, rebootServerToolStripMenuItem, clearBlockedIPsToolStripMenuItem, toolStripMenuItem1, toolStripSeparator1, closeServerToolStripMenuItem, reloadToolStripMenuItem });
            controlToolStripMenuItem.Name = "controlToolStripMenuItem";
            controlToolStripMenuItem.Size = new Size(59, 20);
            controlToolStripMenuItem.Text = "Control";
            // 
            // startServerToolStripMenuItem
            // 
            startServerToolStripMenuItem.Name = "startServerToolStripMenuItem";
            startServerToolStripMenuItem.Size = new Size(164, 22);
            startServerToolStripMenuItem.Text = "Start Server";
            startServerToolStripMenuItem.Click += startServerToolStripMenuItem_Click;
            // 
            // stopServerToolStripMenuItem
            // 
            stopServerToolStripMenuItem.Name = "stopServerToolStripMenuItem";
            stopServerToolStripMenuItem.Size = new Size(164, 22);
            stopServerToolStripMenuItem.Text = "Stop Server";
            stopServerToolStripMenuItem.Click += stopServerToolStripMenuItem_Click;
            // 
            // rebootServerToolStripMenuItem
            // 
            rebootServerToolStripMenuItem.Name = "rebootServerToolStripMenuItem";
            rebootServerToolStripMenuItem.Size = new Size(164, 22);
            rebootServerToolStripMenuItem.Text = "Reboot Server";
            rebootServerToolStripMenuItem.Click += rebootServerToolStripMenuItem_Click;
            // 
            // clearBlockedIPsToolStripMenuItem
            // 
            clearBlockedIPsToolStripMenuItem.Name = "clearBlockedIPsToolStripMenuItem";
            clearBlockedIPsToolStripMenuItem.Size = new Size(164, 22);
            clearBlockedIPsToolStripMenuItem.Text = "Clear Blocked IPs";
            clearBlockedIPsToolStripMenuItem.Click += clearBlockedIPsToolStripMenuItem_Click;
            // 
            // toolStripMenuItem1
            // 
            toolStripMenuItem1.Name = "toolStripMenuItem1";
            toolStripMenuItem1.Size = new Size(161, 6);
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(161, 6);
            // 
            // closeServerToolStripMenuItem
            // 
            closeServerToolStripMenuItem.Name = "closeServerToolStripMenuItem";
            closeServerToolStripMenuItem.Size = new Size(164, 22);
            closeServerToolStripMenuItem.Text = "Close Server";
            closeServerToolStripMenuItem.Click += closeServerToolStripMenuItem_Click;
            // 
            // reloadToolStripMenuItem
            // 
            reloadToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { nPCsToolStripMenuItem, dropsToolStripMenuItem, lineMessageToolStripMenuItem });
            reloadToolStripMenuItem.Name = "reloadToolStripMenuItem";
            reloadToolStripMenuItem.Size = new Size(164, 22);
            reloadToolStripMenuItem.Text = "Reload";
            // 
            // nPCsToolStripMenuItem
            // 
            nPCsToolStripMenuItem.Name = "nPCsToolStripMenuItem";
            nPCsToolStripMenuItem.Size = new Size(145, 22);
            nPCsToolStripMenuItem.Text = "NPCs";
            nPCsToolStripMenuItem.Click += nPCsToolStripMenuItem_Click;
            // 
            // dropsToolStripMenuItem
            // 
            dropsToolStripMenuItem.Name = "dropsToolStripMenuItem";
            dropsToolStripMenuItem.Size = new Size(145, 22);
            dropsToolStripMenuItem.Text = "Drops";
            dropsToolStripMenuItem.Click += dropsToolStripMenuItem_Click;
            // 
            // lineMessageToolStripMenuItem
            // 
            lineMessageToolStripMenuItem.Name = "lineMessageToolStripMenuItem";
            lineMessageToolStripMenuItem.Size = new Size(145, 22);
            lineMessageToolStripMenuItem.Text = "Line Message";
            lineMessageToolStripMenuItem.Click += lineMessageToolStripMenuItem_Click;
            // 
            // accountToolStripMenuItem
            // 
            accountToolStripMenuItem.Name = "accountToolStripMenuItem";
            accountToolStripMenuItem.Size = new Size(64, 20);
            accountToolStripMenuItem.Text = "Account";
            accountToolStripMenuItem.Click += accountToolStripMenuItem_Click;
            // 
            // databaseFormsToolStripMenuItem
            // 
            databaseFormsToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { mapInfoToolStripMenuItem, itemInfoToolStripMenuItem, monsterInfoToolStripMenuItem, itemNEWToolStripMenuItem, monsterExperimentalToolStripMenuItem, nPCInfoToolStripMenuItem, questInfoToolStripMenuItem, magicInfoToolStripMenuItem, gameshopToolStripMenuItem, recipeToolStripMenuItem });
            databaseFormsToolStripMenuItem.Name = "databaseFormsToolStripMenuItem";
            databaseFormsToolStripMenuItem.Size = new Size(67, 20);
            databaseFormsToolStripMenuItem.Text = "Database";
            // 
            // mapInfoToolStripMenuItem
            // 
            mapInfoToolStripMenuItem.Name = "mapInfoToolStripMenuItem";
            mapInfoToolStripMenuItem.Size = new Size(203, 22);
            mapInfoToolStripMenuItem.Text = "Map";
            mapInfoToolStripMenuItem.Click += mapInfoToolStripMenuItem_Click;
            // 
            // itemInfoToolStripMenuItem
            // 
            itemInfoToolStripMenuItem.Name = "itemInfoToolStripMenuItem";
            itemInfoToolStripMenuItem.ShowShortcutKeys = false;
            itemInfoToolStripMenuItem.Size = new Size(203, 22);
            itemInfoToolStripMenuItem.Text = "Item (OLD- HIDDEN)";
            itemInfoToolStripMenuItem.Visible = false;
            itemInfoToolStripMenuItem.Click += itemInfoToolStripMenuItem_Click;
            // 
            // monsterInfoToolStripMenuItem
            // 
            monsterInfoToolStripMenuItem.Name = "monsterInfoToolStripMenuItem";
            monsterInfoToolStripMenuItem.Size = new Size(203, 22);
            monsterInfoToolStripMenuItem.Text = "Monster(OLD - HIDDEN)";
            monsterInfoToolStripMenuItem.Visible = false;
            monsterInfoToolStripMenuItem.Click += monsterInfoToolStripMenuItem_Click;
            // 
            // itemNEWToolStripMenuItem
            // 
            itemNEWToolStripMenuItem.Name = "itemNEWToolStripMenuItem";
            itemNEWToolStripMenuItem.Size = new Size(203, 22);
            itemNEWToolStripMenuItem.Text = "Item";
            itemNEWToolStripMenuItem.Click += itemNEWToolStripMenuItem_Click;
            // 
            // monsterExperimentalToolStripMenuItem
            // 
            monsterExperimentalToolStripMenuItem.Name = "monsterExperimentalToolStripMenuItem";
            monsterExperimentalToolStripMenuItem.Size = new Size(203, 22);
            monsterExperimentalToolStripMenuItem.Text = "Monster";
            monsterExperimentalToolStripMenuItem.Click += monsterExperimentalToolStripMenuItem_Click;
            // 
            // nPCInfoToolStripMenuItem
            // 
            nPCInfoToolStripMenuItem.Name = "nPCInfoToolStripMenuItem";
            nPCInfoToolStripMenuItem.Size = new Size(203, 22);
            nPCInfoToolStripMenuItem.Text = "NPC";
            nPCInfoToolStripMenuItem.Click += nPCInfoToolStripMenuItem_Click;
            // 
            // questInfoToolStripMenuItem
            // 
            questInfoToolStripMenuItem.Name = "questInfoToolStripMenuItem";
            questInfoToolStripMenuItem.Size = new Size(203, 22);
            questInfoToolStripMenuItem.Text = "Quest";
            questInfoToolStripMenuItem.Click += questInfoToolStripMenuItem_Click;
            // 
            // magicInfoToolStripMenuItem
            // 
            magicInfoToolStripMenuItem.Name = "magicInfoToolStripMenuItem";
            magicInfoToolStripMenuItem.Size = new Size(203, 22);
            magicInfoToolStripMenuItem.Text = "Magic";
            magicInfoToolStripMenuItem.Click += magicInfoToolStripMenuItem_Click;
            // 
            // gameshopToolStripMenuItem
            // 
            gameshopToolStripMenuItem.Name = "gameshopToolStripMenuItem";
            gameshopToolStripMenuItem.Size = new Size(203, 22);
            gameshopToolStripMenuItem.Text = "Gameshop";
            gameshopToolStripMenuItem.Click += gameshopToolStripMenuItem_Click;
            // 
            // configToolStripMenuItem1
            // 
            configToolStripMenuItem1.DropDownItems.AddRange(new ToolStripItem[] { serverToolStripMenuItem, balanceToolStripMenuItem, systemToolStripMenuItem, monsterTunerToolStripMenuItem, dropBuilderToolStripMenuItem });
            configToolStripMenuItem1.Name = "configToolStripMenuItem1";
            configToolStripMenuItem1.Size = new Size(55, 20);
            configToolStripMenuItem1.Text = "Config";
            // 
            // serverToolStripMenuItem
            // 
            serverToolStripMenuItem.Name = "serverToolStripMenuItem";
            serverToolStripMenuItem.Size = new Size(151, 22);
            serverToolStripMenuItem.Text = "Server";
            serverToolStripMenuItem.Click += serverToolStripMenuItem_Click;
            // 
            // balanceToolStripMenuItem
            // 
            balanceToolStripMenuItem.Name = "balanceToolStripMenuItem";
            balanceToolStripMenuItem.Size = new Size(151, 22);
            balanceToolStripMenuItem.Text = "Balance";
            balanceToolStripMenuItem.Click += balanceToolStripMenuItem_Click;
            // 
            // systemToolStripMenuItem
            // 
            systemToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { dragonSystemToolStripMenuItem, miningToolStripMenuItem, guildsToolStripMenuItem, fishingToolStripMenuItem, mailToolStripMenuItem, goodsToolStripMenuItem, refiningToolStripMenuItem, relationshipToolStripMenuItem, mentorToolStripMenuItem, gemToolStripMenuItem, conquestToolStripMenuItem, respawnsToolStripMenuItem, heroesToolStripMenuItem });
            systemToolStripMenuItem.Name = "systemToolStripMenuItem";
            systemToolStripMenuItem.Size = new Size(151, 22);
            systemToolStripMenuItem.Text = "System";
            // 
            // dragonSystemToolStripMenuItem
            // 
            dragonSystemToolStripMenuItem.Name = "dragonSystemToolStripMenuItem";
            dragonSystemToolStripMenuItem.Size = new Size(139, 22);
            dragonSystemToolStripMenuItem.Text = "Dragon";
            dragonSystemToolStripMenuItem.Click += dragonSystemToolStripMenuItem_Click;
            // 
            // miningToolStripMenuItem
            // 
            miningToolStripMenuItem.Name = "miningToolStripMenuItem";
            miningToolStripMenuItem.Size = new Size(139, 22);
            miningToolStripMenuItem.Text = "Mining";
            miningToolStripMenuItem.Click += miningToolStripMenuItem_Click;
            // 
            // guildsToolStripMenuItem
            // 
            guildsToolStripMenuItem.Name = "guildsToolStripMenuItem";
            guildsToolStripMenuItem.Size = new Size(139, 22);
            guildsToolStripMenuItem.Text = "Guilds";
            guildsToolStripMenuItem.Click += guildsToolStripMenuItem_Click;
            // 
            // fishingToolStripMenuItem
            // 
            fishingToolStripMenuItem.Name = "fishingToolStripMenuItem";
            fishingToolStripMenuItem.Size = new Size(139, 22);
            fishingToolStripMenuItem.Text = "Fishing";
            fishingToolStripMenuItem.Click += fishingToolStripMenuItem_Click;
            // 
            // mailToolStripMenuItem
            // 
            mailToolStripMenuItem.Name = "mailToolStripMenuItem";
            mailToolStripMenuItem.Size = new Size(139, 22);
            mailToolStripMenuItem.Text = "Mail";
            mailToolStripMenuItem.Click += mailToolStripMenuItem_Click;
            // 
            // goodsToolStripMenuItem
            // 
            goodsToolStripMenuItem.Name = "goodsToolStripMenuItem";
            goodsToolStripMenuItem.Size = new Size(139, 22);
            goodsToolStripMenuItem.Text = "Goods";
            goodsToolStripMenuItem.Click += goodsToolStripMenuItem_Click;
            // 
            // refiningToolStripMenuItem
            // 
            refiningToolStripMenuItem.Name = "refiningToolStripMenuItem";
            refiningToolStripMenuItem.Size = new Size(139, 22);
            refiningToolStripMenuItem.Text = "Refining";
            refiningToolStripMenuItem.Click += refiningToolStripMenuItem_Click;
            // 
            // relationshipToolStripMenuItem
            // 
            relationshipToolStripMenuItem.Name = "relationshipToolStripMenuItem";
            relationshipToolStripMenuItem.Size = new Size(139, 22);
            relationshipToolStripMenuItem.Text = "Relationship";
            relationshipToolStripMenuItem.Click += relationshipToolStripMenuItem_Click;
            // 
            // mentorToolStripMenuItem
            // 
            mentorToolStripMenuItem.Name = "mentorToolStripMenuItem";
            mentorToolStripMenuItem.Size = new Size(139, 22);
            mentorToolStripMenuItem.Text = "Mentor";
            mentorToolStripMenuItem.Click += mentorToolStripMenuItem_Click;
            // 
            // gemToolStripMenuItem
            // 
            gemToolStripMenuItem.Name = "gemToolStripMenuItem";
            gemToolStripMenuItem.Size = new Size(139, 22);
            gemToolStripMenuItem.Text = "Gem";
            gemToolStripMenuItem.Click += gemToolStripMenuItem_Click;
            // 
            // conquestToolStripMenuItem
            // 
            conquestToolStripMenuItem.Name = "conquestToolStripMenuItem";
            conquestToolStripMenuItem.Size = new Size(139, 22);
            conquestToolStripMenuItem.Text = "Conquest";
            conquestToolStripMenuItem.Click += conquestToolStripMenuItem_Click;
            // 
            // respawnsToolStripMenuItem
            // 
            respawnsToolStripMenuItem.Name = "respawnsToolStripMenuItem";
            respawnsToolStripMenuItem.Size = new Size(139, 22);
            respawnsToolStripMenuItem.Text = "SpawnTick";
            respawnsToolStripMenuItem.Click += respawnsToolStripMenuItem_Click;
            // 
            // heroesToolStripMenuItem
            // 
            heroesToolStripMenuItem.Name = "heroesToolStripMenuItem";
            heroesToolStripMenuItem.Size = new Size(139, 22);
            heroesToolStripMenuItem.Text = "Heroes";
            heroesToolStripMenuItem.Click += heroesToolStripMenuItem_Click;
            // 
            // monsterTunerToolStripMenuItem
            // 
            monsterTunerToolStripMenuItem.Name = "monsterTunerToolStripMenuItem";
            monsterTunerToolStripMenuItem.Size = new Size(151, 22);
            monsterTunerToolStripMenuItem.Text = "Monster Tuner";
            monsterTunerToolStripMenuItem.Click += monsterTunerToolStripMenuItem_Click;
            // 
            // dropBuilderToolStripMenuItem
            // 
            dropBuilderToolStripMenuItem.Name = "dropBuilderToolStripMenuItem";
            dropBuilderToolStripMenuItem.Size = new Size(151, 22);
            dropBuilderToolStripMenuItem.Text = "Drop Builder";
            dropBuilderToolStripMenuItem.Click += dropBuilderToolStripMenuItem_Click;
            // 
            // CharacterToolStripMenuItem
            // 
            CharacterToolStripMenuItem.Name = "CharacterToolStripMenuItem";
            CharacterToolStripMenuItem.Size = new Size(75, 20);
            CharacterToolStripMenuItem.Text = "Characters";
            CharacterToolStripMenuItem.Click += CharacterToolStripMenuItem_Click;
            // 
            // UpTimeLabel
            // 
            UpTimeLabel.Alignment = ToolStripItemAlignment.Right;
            UpTimeLabel.BorderStyle = BorderStyle.None;
            UpTimeLabel.Name = "UpTimeLabel";
            UpTimeLabel.ReadOnly = true;
            UpTimeLabel.Size = new Size(200, 20);
            UpTimeLabel.Text = "Uptime:";
            // 
            // InterfaceTimer
            // 
            InterfaceTimer.Enabled = true;
            InterfaceTimer.Tick += InterfaceTimer_Tick;
            // 
            // recipeToolStripMenuItem
            // 
            recipeToolStripMenuItem.Name = "recipeToolStripMenuItem";
            recipeToolStripMenuItem.Size = new Size(203, 22);
            recipeToolStripMenuItem.Text = "Recipe";
            recipeToolStripMenuItem.Click += recipeToolStripMenuItem_Click;
            // 
            // SMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            AutoSizeMode = AutoSizeMode.GrowAndShrink;
            ClientSize = new Size(566, 455);
            Controls.Add(MainTabs);
            Controls.Add(StatusBar);
            Controls.Add(MainMenu);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            Name = "SMain";
            Text = "Legend of Mir 2 Server";
            FormClosing += SMain_FormClosing;
            Load += SMain_Load;
            MainTabs.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage5.ResumeLayout(false);
            StatusBar.ResumeLayout(false);
            StatusBar.PerformLayout();
            MainMenu.ResumeLayout(false);
            MainMenu.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private TabControl MainTabs;
        private TabPage tabPage1;
        private TextBox LogTextBox;
        private StatusStrip StatusBar;
        private ToolStripStatusLabel PlayersLabel;
        private ToolStripStatusLabel MonsterLabel;
        private ToolStripStatusLabel ConnectionsLabel;
        private MenuStrip MainMenu;
        private ToolStripMenuItem controlToolStripMenuItem;
        private ToolStripMenuItem startServerToolStripMenuItem;
        private ToolStripMenuItem stopServerToolStripMenuItem;
        private ToolStripSeparator toolStripMenuItem1;
        private ToolStripMenuItem closeServerToolStripMenuItem;
        private Timer InterfaceTimer;
        private TabPage tabPage2;
        private TextBox DebugLogTextBox;
        private TabPage tabPage3;
        private TextBox ChatLogTextBox;
        private ToolStripMenuItem accountToolStripMenuItem;
        private ToolStripMenuItem databaseFormsToolStripMenuItem;
        private ToolStripMenuItem mapInfoToolStripMenuItem;
        private ToolStripMenuItem itemInfoToolStripMenuItem;
        private ToolStripMenuItem monsterInfoToolStripMenuItem;
        private ToolStripMenuItem nPCInfoToolStripMenuItem;
        private ToolStripMenuItem questInfoToolStripMenuItem;
        private ToolStripMenuItem configToolStripMenuItem1;
        private ToolStripMenuItem serverToolStripMenuItem;
        private ToolStripMenuItem balanceToolStripMenuItem;
        private ToolStripMenuItem systemToolStripMenuItem;
        private ToolStripMenuItem dragonSystemToolStripMenuItem;
        private ToolStripMenuItem guildsToolStripMenuItem;
        private ToolStripMenuItem miningToolStripMenuItem;
        private ToolStripMenuItem fishingToolStripMenuItem;
        private TabPage tabPage4;
        private GroupBox groupBox1;
        private Button GlobalMessageButton;
        private TextBox GlobalMessageTextBox;
        private CustomFormControl.ListViewNF PlayersOnlineListView;
        private ColumnHeader nameHeader;
        private ColumnHeader levelHeader;
        private ColumnHeader classHeader;
        private ColumnHeader genderHeader;
        private ColumnHeader indexHeader;
        private ToolStripMenuItem mailToolStripMenuItem;
        private ToolStripMenuItem goodsToolStripMenuItem;
        private ToolStripStatusLabel CycleDelayLabel;
        private ToolStripMenuItem magicInfoToolStripMenuItem;
        private ToolStripMenuItem refiningToolStripMenuItem;
        private ToolStripMenuItem relationshipToolStripMenuItem;
        private ToolStripMenuItem mentorToolStripMenuItem;
        private ToolStripMenuItem gameshopToolStripMenuItem;
        private ToolStripMenuItem gemToolStripMenuItem;
        private ToolStripMenuItem conquestToolStripMenuItem;
        private ToolStripMenuItem rebootServerToolStripMenuItem;
        private ToolStripMenuItem respawnsToolStripMenuItem;
        private ToolStripMenuItem monsterTunerToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem itemNEWToolStripMenuItem;
        private ToolStripMenuItem monsterExperimentalToolStripMenuItem;
        private ToolStripMenuItem dropBuilderToolStripMenuItem;
        private ToolStripStatusLabel BlockedIPsLabel;
        private ToolStripMenuItem clearBlockedIPsToolStripMenuItem;
        private ToolStripMenuItem reloadToolStripMenuItem;
        private ToolStripMenuItem nPCsToolStripMenuItem;
        private ToolStripMenuItem dropsToolStripMenuItem;
        private ToolStripMenuItem lineMessageToolStripMenuItem;
        private TabPage tabPage5;
        private CustomFormControl.ListViewNF GuildListView;
        private ColumnHeader columnHeader1;
        private ColumnHeader columnHeader2;
        private ColumnHeader columnHeader3;
        private ColumnHeader columnHeader4;
        private ColumnHeader columnHeader5;
        private ColumnHeader columnHeader6;
        private ToolStripTextBox UpTimeLabel;
        private ToolStripMenuItem heroesToolStripMenuItem;
        private ToolStripMenuItem CharacterToolStripMenuItem;
        private ToolStripMenuItem recipeToolStripMenuItem;
    }
}

