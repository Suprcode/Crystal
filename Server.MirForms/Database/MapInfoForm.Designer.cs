using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Server
{
    partial class MapInfoForm
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
            MapTabs = new TabControl();
            tabPage1 = new TabPage();
            label48 = new Label();
            lstParticles = new ListBox();
            MinimapPreview = new PictureBox();
            label11 = new Label();
            MusicTextBox = new TextBox();
            label33 = new Label();
            MineComboBox = new ComboBox();
            label15 = new Label();
            BigMapTextBox = new TextBox();
            LightsComboBox = new ComboBox();
            label5 = new Label();
            label1 = new Label();
            label4 = new Label();
            MapIndexTextBox = new TextBox();
            MiniMapTextBox = new TextBox();
            label2 = new Label();
            MapNameTextBox = new TextBox();
            FileNameTextBox = new TextBox();
            label3 = new Label();
            tabPage6 = new TabPage();
            NoReincarnation = new CheckBox();
            NoTownTeleportCheckbox = new CheckBox();
            NoFightCheckbox = new CheckBox();
            NeedBridleCheckbox = new CheckBox();
            NoMountCheckbox = new CheckBox();
            label19 = new Label();
            MapDarkLighttextBox = new TextBox();
            NoNamesCheckbox = new CheckBox();
            NoDropMonsterCheckbox = new CheckBox();
            NoDropPlayerCheckbox = new CheckBox();
            NoThrowItemCheckbox = new CheckBox();
            NoPositionCheckbox = new CheckBox();
            NoDrugCheckbox = new CheckBox();
            NoRecallCheckbox = new CheckBox();
            NoEscapeCheckbox = new CheckBox();
            NoRandomCheckbox = new CheckBox();
            LightningTextbox = new TextBox();
            FireTextbox = new TextBox();
            NoReconnectTextbox = new TextBox();
            LightningCheckbox = new CheckBox();
            FireCheckbox = new CheckBox();
            FightCheckbox = new CheckBox();
            NoReconnectCheckbox = new CheckBox();
            NoTeleportCheckbox = new CheckBox();
            tabPage3 = new TabPage();
            RemoveSZButton = new Button();
            AddSZButton = new Button();
            SafeZoneInfoPanel = new Panel();
            label12 = new Label();
            SZYTextBox = new TextBox();
            label14 = new Label();
            SizeTextBox = new TextBox();
            label17 = new Label();
            SZXTextBox = new TextBox();
            StartPointCheckBox = new CheckBox();
            SafeZoneInfoListBox = new ListBox();
            tabPage2 = new TabPage();
            RPasteButton = new Button();
            RCopyButton = new Button();
            RemoveRButton = new Button();
            AddRButton = new Button();
            RespawnInfoListBox = new ListBox();
            RespawnInfoPanel = new Panel();
            chkrespawnsave = new CheckBox();
            chkRespawnEnableTick = new CheckBox();
            Randomtextbox = new TextBox();
            label23 = new Label();
            label34 = new Label();
            RoutePathTextBox = new TextBox();
            label24 = new Label();
            DirectionTextBox = new TextBox();
            label8 = new Label();
            DelayTextBox = new TextBox();
            label7 = new Label();
            MonsterInfoComboBox = new ComboBox();
            label6 = new Label();
            SpreadTextBox = new TextBox();
            label9 = new Label();
            RYTextBox = new TextBox();
            label10 = new Label();
            CountTextBox = new TextBox();
            label13 = new Label();
            RXTextBox = new TextBox();
            tabPage4 = new TabPage();
            RemoveMButton = new Button();
            AddMButton = new Button();
            MovementInfoPanel = new Panel();
            label26 = new Label();
            BigMapIconTextBox = new TextBox();
            ShowBigMapCheckBox = new CheckBox();
            label25 = new Label();
            ConquestComboBox = new ComboBox();
            NeedMoveMCheckBox = new CheckBox();
            NeedHoleMCheckBox = new CheckBox();
            label22 = new Label();
            DestMapComboBox = new ComboBox();
            label18 = new Label();
            DestYTextBox = new TextBox();
            label21 = new Label();
            DestXTextBox = new TextBox();
            label16 = new Label();
            SourceYTextBox = new TextBox();
            label20 = new Label();
            SourceXTextBox = new TextBox();
            MovementInfoListBox = new ListBox();
            tabPage7 = new TabPage();
            MZDeletebutton = new Button();
            MZAddbutton = new Button();
            MineZonepanel = new Panel();
            label27 = new Label();
            MineZoneComboBox = new ComboBox();
            label30 = new Label();
            MZYtextBox = new TextBox();
            label31 = new Label();
            MZSizetextBox = new TextBox();
            label32 = new Label();
            MZXtextBox = new TextBox();
            MZListlistBox = new ListBox();
            RemoveButton = new Button();
            AddButton = new Button();
            MapInfoListBox = new ListBox();
            PasteMapButton = new Button();
            CopyMapButton = new Button();
            ImportMapInfoButton = new Button();
            ExportMapInfoButton = new Button();
            ImportMongenButton = new Button();
            ExportMongenButton = new Button();
            VisualizerButton = new Button();
            toolTip1 = new ToolTip(components);
            MapTabs.SuspendLayout();
            tabPage1.SuspendLayout();
            ((ISupportInitialize)MinimapPreview).BeginInit();
            tabPage6.SuspendLayout();
            tabPage3.SuspendLayout();
            SafeZoneInfoPanel.SuspendLayout();
            tabPage2.SuspendLayout();
            RespawnInfoPanel.SuspendLayout();
            tabPage4.SuspendLayout();
            MovementInfoPanel.SuspendLayout();
            tabPage7.SuspendLayout();
            MineZonepanel.SuspendLayout();
            SuspendLayout();
            // 
            // MapTabs
            // 
            MapTabs.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MapTabs.Controls.Add(tabPage1);
            MapTabs.Controls.Add(tabPage6);
            MapTabs.Controls.Add(tabPage3);
            MapTabs.Controls.Add(tabPage2);
            MapTabs.Controls.Add(tabPage4);
            MapTabs.Controls.Add(tabPage7);
            MapTabs.Location = new Point(241, 72);
            MapTabs.Margin = new Padding(4, 3, 4, 3);
            MapTabs.Name = "MapTabs";
            MapTabs.SelectedIndex = 0;
            MapTabs.Size = new Size(630, 552);
            MapTabs.TabIndex = 8;
            // 
            // tabPage1
            // 
            tabPage1.Controls.Add(label48);
            tabPage1.Controls.Add(lstParticles);
            tabPage1.Controls.Add(MinimapPreview);
            tabPage1.Controls.Add(label11);
            tabPage1.Controls.Add(MusicTextBox);
            tabPage1.Controls.Add(label33);
            tabPage1.Controls.Add(MineComboBox);
            tabPage1.Controls.Add(label15);
            tabPage1.Controls.Add(BigMapTextBox);
            tabPage1.Controls.Add(LightsComboBox);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(label1);
            tabPage1.Controls.Add(label4);
            tabPage1.Controls.Add(MapIndexTextBox);
            tabPage1.Controls.Add(MiniMapTextBox);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(MapNameTextBox);
            tabPage1.Controls.Add(FileNameTextBox);
            tabPage1.Controls.Add(label3);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Margin = new Padding(4, 3, 4, 3);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(4, 3, 4, 3);
            tabPage1.Size = new Size(622, 524);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Info";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // label48
            // 
            label48.AutoSize = true;
            label48.Location = new Point(451, 3);
            label48.Name = "label48";
            label48.Size = new Size(164, 15);
            label48.TabIndex = 20;
            label48.Text = "Weather (Can Select Multiple)";
            // 
            // lstParticles
            // 
            lstParticles.FormattingEnabled = true;
            lstParticles.ItemHeight = 15;
            lstParticles.Location = new Point(454, 21);
            lstParticles.Name = "lstParticles";
            lstParticles.SelectionMode = SelectionMode.MultiSimple;
            lstParticles.Size = new Size(165, 244);
            lstParticles.TabIndex = 0;
            lstParticles.SelectedIndexChanged += lstParticles_SelectedIndexChanged;
            // 
            // MinimapPreview
            // 
            MinimapPreview.Location = new Point(3, 234);
            MinimapPreview.Name = "MinimapPreview";
            MinimapPreview.Size = new Size(357, 287);
            MinimapPreview.SizeMode = PictureBoxSizeMode.StretchImage;
            MinimapPreview.TabIndex = 19;
            MinimapPreview.TabStop = false;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(44, 209);
            label11.Margin = new Padding(4, 0, 4, 0);
            label11.Name = "label11";
            label11.Size = new Size(42, 15);
            label11.TabIndex = 18;
            label11.Text = "Music:";
            // 
            // MusicTextBox
            // 
            MusicTextBox.Location = new Point(96, 205);
            MusicTextBox.Margin = new Padding(4, 3, 4, 3);
            MusicTextBox.Name = "MusicTextBox";
            MusicTextBox.Size = new Size(107, 23);
            MusicTextBox.TabIndex = 17;
            MusicTextBox.TextChanged += MusicTextBox_TextChanged;
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Location = new Point(19, 177);
            label33.Margin = new Padding(4, 0, 4, 0);
            label33.Name = "label33";
            label33.Size = new Size(64, 15);
            label33.TabIndex = 16;
            label33.Text = "Mine Type:";
            // 
            // MineComboBox
            // 
            MineComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MineComboBox.FormattingEnabled = true;
            MineComboBox.Location = new Point(96, 173);
            MineComboBox.Margin = new Padding(4, 3, 4, 3);
            MineComboBox.Name = "MineComboBox";
            MineComboBox.Size = new Size(107, 23);
            MineComboBox.TabIndex = 15;
            MineComboBox.SelectedIndexChanged += MineComboBox_SelectedIndexChanged;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(150, 114);
            label15.Margin = new Padding(4, 0, 4, 0);
            label15.Name = "label15";
            label15.Size = new Size(54, 15);
            label15.TabIndex = 14;
            label15.Text = "Big Map:";
            // 
            // BigMapTextBox
            // 
            BigMapTextBox.Location = new Point(215, 111);
            BigMapTextBox.Margin = new Padding(4, 3, 4, 3);
            BigMapTextBox.MaxLength = 5;
            BigMapTextBox.Name = "BigMapTextBox";
            BigMapTextBox.Size = new Size(42, 23);
            BigMapTextBox.TabIndex = 13;
            BigMapTextBox.TextChanged += BigMapTextBox_TextChanged;
            // 
            // LightsComboBox
            // 
            LightsComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            LightsComboBox.FormattingEnabled = true;
            LightsComboBox.Location = new Point(96, 141);
            LightsComboBox.Margin = new Padding(4, 3, 4, 3);
            LightsComboBox.Name = "LightsComboBox";
            LightsComboBox.Size = new Size(107, 23);
            LightsComboBox.TabIndex = 11;
            LightsComboBox.SelectedIndexChanged += LightsComboBox_SelectedIndexChanged;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(44, 144);
            label5.Margin = new Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new Size(42, 15);
            label5.TabIndex = 12;
            label5.Text = "Lights:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(19, 21);
            label1.Margin = new Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new Size(66, 15);
            label1.TabIndex = 4;
            label1.Text = "Map Index:";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(27, 114);
            label4.Margin = new Padding(4, 0, 4, 0);
            label4.Name = "label4";
            label4.Size = new Size(61, 15);
            label4.TabIndex = 10;
            label4.Text = "Mini Map:";
            // 
            // MapIndexTextBox
            // 
            MapIndexTextBox.Location = new Point(96, 17);
            MapIndexTextBox.Margin = new Padding(4, 3, 4, 3);
            MapIndexTextBox.Name = "MapIndexTextBox";
            MapIndexTextBox.ReadOnly = true;
            MapIndexTextBox.Size = new Size(54, 23);
            MapIndexTextBox.TabIndex = 0;
            // 
            // MiniMapTextBox
            // 
            MiniMapTextBox.Location = new Point(96, 111);
            MiniMapTextBox.Margin = new Padding(4, 3, 4, 3);
            MiniMapTextBox.MaxLength = 5;
            MiniMapTextBox.Name = "MiniMapTextBox";
            MiniMapTextBox.Size = new Size(42, 23);
            MiniMapTextBox.TabIndex = 9;
            MiniMapTextBox.TextChanged += MiniMapTextBox_TextChanged;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(22, 54);
            label2.Margin = new Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new Size(63, 15);
            label2.TabIndex = 6;
            label2.Text = "File Name:";
            // 
            // MapNameTextBox
            // 
            MapNameTextBox.Location = new Point(96, 81);
            MapNameTextBox.Margin = new Padding(4, 3, 4, 3);
            MapNameTextBox.Name = "MapNameTextBox";
            MapNameTextBox.Size = new Size(107, 23);
            MapNameTextBox.TabIndex = 2;
            MapNameTextBox.TextChanged += MapNameTextBox_TextChanged;
            // 
            // FileNameTextBox
            // 
            FileNameTextBox.Location = new Point(96, 51);
            FileNameTextBox.Margin = new Padding(4, 3, 4, 3);
            FileNameTextBox.Name = "FileNameTextBox";
            FileNameTextBox.Size = new Size(54, 23);
            FileNameTextBox.TabIndex = 1;
            FileNameTextBox.TextChanged += FileNameTextBox_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(16, 84);
            label3.Margin = new Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new Size(69, 15);
            label3.TabIndex = 8;
            label3.Text = "Map Name:";
            // 
            // tabPage6
            // 
            tabPage6.Controls.Add(NoReincarnation);
            tabPage6.Controls.Add(NoTownTeleportCheckbox);
            tabPage6.Controls.Add(NoFightCheckbox);
            tabPage6.Controls.Add(NeedBridleCheckbox);
            tabPage6.Controls.Add(NoMountCheckbox);
            tabPage6.Controls.Add(label19);
            tabPage6.Controls.Add(MapDarkLighttextBox);
            tabPage6.Controls.Add(NoNamesCheckbox);
            tabPage6.Controls.Add(NoDropMonsterCheckbox);
            tabPage6.Controls.Add(NoDropPlayerCheckbox);
            tabPage6.Controls.Add(NoThrowItemCheckbox);
            tabPage6.Controls.Add(NoPositionCheckbox);
            tabPage6.Controls.Add(NoDrugCheckbox);
            tabPage6.Controls.Add(NoRecallCheckbox);
            tabPage6.Controls.Add(NoEscapeCheckbox);
            tabPage6.Controls.Add(NoRandomCheckbox);
            tabPage6.Controls.Add(LightningTextbox);
            tabPage6.Controls.Add(FireTextbox);
            tabPage6.Controls.Add(NoReconnectTextbox);
            tabPage6.Controls.Add(LightningCheckbox);
            tabPage6.Controls.Add(FireCheckbox);
            tabPage6.Controls.Add(FightCheckbox);
            tabPage6.Controls.Add(NoReconnectCheckbox);
            tabPage6.Controls.Add(NoTeleportCheckbox);
            tabPage6.Location = new Point(4, 24);
            tabPage6.Margin = new Padding(4, 3, 4, 3);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(4, 3, 4, 3);
            tabPage6.Size = new Size(622, 524);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "Attributes";
            tabPage6.UseVisualStyleBackColor = true;
            // 
            // NoReincarnation
            // 
            NoReincarnation.AutoSize = true;
            NoReincarnation.Location = new Point(18, 228);
            NoReincarnation.Margin = new Padding(4, 3, 4, 3);
            NoReincarnation.Name = "NoReincarnation";
            NoReincarnation.Size = new Size(118, 19);
            NoReincarnation.TabIndex = 45;
            NoReincarnation.Text = "No Reincarnation";
            NoReincarnation.UseVisualStyleBackColor = true;
            NoReincarnation.CheckedChanged += NoReincarnation_CheckedChanged;
            // 
            // NoTownTeleportCheckbox
            // 
            NoTownTeleportCheckbox.AutoSize = true;
            NoTownTeleportCheckbox.Location = new Point(397, 137);
            NoTownTeleportCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoTownTeleportCheckbox.Name = "NoTownTeleportCheckbox";
            NoTownTeleportCheckbox.Size = new Size(115, 19);
            NoTownTeleportCheckbox.TabIndex = 44;
            NoTownTeleportCheckbox.Text = "No TownTeleport";
            NoTownTeleportCheckbox.UseVisualStyleBackColor = true;
            NoTownTeleportCheckbox.CheckedChanged += NoTownTeleportCheckbox_CheckedChanged;
            // 
            // NoFightCheckbox
            // 
            NoFightCheckbox.AutoSize = true;
            NoFightCheckbox.Location = new Point(203, 158);
            NoFightCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoFightCheckbox.Name = "NoFightCheckbox";
            NoFightCheckbox.Size = new Size(72, 19);
            NoFightCheckbox.TabIndex = 43;
            NoFightCheckbox.Text = "No Fight";
            NoFightCheckbox.UseVisualStyleBackColor = true;
            NoFightCheckbox.CheckedChanged += NoFightCheckbox_CheckedChanged;
            // 
            // NeedBridleCheckbox
            // 
            NeedBridleCheckbox.AutoSize = true;
            NeedBridleCheckbox.Location = new Point(203, 185);
            NeedBridleCheckbox.Margin = new Padding(4, 3, 4, 3);
            NeedBridleCheckbox.Name = "NeedBridleCheckbox";
            NeedBridleCheckbox.Size = new Size(87, 19);
            NeedBridleCheckbox.TabIndex = 42;
            NeedBridleCheckbox.Text = "Need Bridle";
            NeedBridleCheckbox.UseVisualStyleBackColor = true;
            NeedBridleCheckbox.CheckedChanged += NeedBridleCheckbox_CheckedChanged;
            // 
            // NoMountCheckbox
            // 
            NoMountCheckbox.AutoSize = true;
            NoMountCheckbox.Location = new Point(203, 130);
            NoMountCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoMountCheckbox.Name = "NoMountCheckbox";
            NoMountCheckbox.Size = new Size(81, 19);
            NoMountCheckbox.TabIndex = 41;
            NoMountCheckbox.Text = "No Mount";
            NoMountCheckbox.UseVisualStyleBackColor = true;
            NoMountCheckbox.CheckedChanged += NoMountCheckbox_CheckedChanged;
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(394, 108);
            label19.Margin = new Padding(4, 0, 4, 0);
            label19.Name = "label19";
            label19.Size = new Size(88, 15);
            label19.TabIndex = 40;
            label19.Text = "Map Dark Light";
            // 
            // MapDarkLighttextBox
            // 
            MapDarkLighttextBox.Location = new Point(517, 105);
            MapDarkLighttextBox.Margin = new Padding(4, 3, 4, 3);
            MapDarkLighttextBox.Name = "MapDarkLighttextBox";
            MapDarkLighttextBox.Size = new Size(56, 23);
            MapDarkLighttextBox.TabIndex = 39;
            MapDarkLighttextBox.TextChanged += MapDarkLighttextBox_TextChanged;
            // 
            // NoNamesCheckbox
            // 
            NoNamesCheckbox.AutoSize = true;
            NoNamesCheckbox.Location = new Point(203, 103);
            NoNamesCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoNamesCheckbox.Name = "NoNamesCheckbox";
            NoNamesCheckbox.Size = new Size(82, 19);
            NoNamesCheckbox.TabIndex = 38;
            NoNamesCheckbox.Text = "No Names";
            NoNamesCheckbox.UseVisualStyleBackColor = true;
            NoNamesCheckbox.CheckedChanged += NoNamesCheckbox_CheckedChanged;
            // 
            // NoDropMonsterCheckbox
            // 
            NoDropMonsterCheckbox.AutoSize = true;
            NoDropMonsterCheckbox.Location = new Point(203, 75);
            NoDropMonsterCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoDropMonsterCheckbox.Name = "NoDropMonsterCheckbox";
            NoDropMonsterCheckbox.Size = new Size(126, 19);
            NoDropMonsterCheckbox.TabIndex = 37;
            NoDropMonsterCheckbox.Text = "No Drop (Monster)";
            NoDropMonsterCheckbox.UseVisualStyleBackColor = true;
            NoDropMonsterCheckbox.CheckedChanged += NoDropMonsterCheckbox_CheckedChanged;
            // 
            // NoDropPlayerCheckbox
            // 
            NoDropPlayerCheckbox.AutoSize = true;
            NoDropPlayerCheckbox.Location = new Point(203, 47);
            NoDropPlayerCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoDropPlayerCheckbox.Name = "NoDropPlayerCheckbox";
            NoDropPlayerCheckbox.Size = new Size(114, 19);
            NoDropPlayerCheckbox.TabIndex = 36;
            NoDropPlayerCheckbox.Text = "No Drop (Player)";
            NoDropPlayerCheckbox.UseVisualStyleBackColor = true;
            NoDropPlayerCheckbox.CheckedChanged += NoDropPlayerCheckbox_CheckedChanged;
            // 
            // NoThrowItemCheckbox
            // 
            NoThrowItemCheckbox.AutoSize = true;
            NoThrowItemCheckbox.Location = new Point(203, 20);
            NoThrowItemCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoThrowItemCheckbox.Name = "NoThrowItemCheckbox";
            NoThrowItemCheckbox.Size = new Size(105, 19);
            NoThrowItemCheckbox.TabIndex = 35;
            NoThrowItemCheckbox.Text = "No Throw Item";
            NoThrowItemCheckbox.UseVisualStyleBackColor = true;
            NoThrowItemCheckbox.CheckedChanged += NoThrowItemCheckbox_CheckedChanged;
            // 
            // NoPositionCheckbox
            // 
            NoPositionCheckbox.AutoSize = true;
            NoPositionCheckbox.Location = new Point(18, 185);
            NoPositionCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoPositionCheckbox.Name = "NoPositionCheckbox";
            NoPositionCheckbox.Size = new Size(88, 19);
            NoPositionCheckbox.TabIndex = 34;
            NoPositionCheckbox.Text = "No Position";
            NoPositionCheckbox.UseVisualStyleBackColor = true;
            NoPositionCheckbox.CheckedChanged += NoPositionCheckbox_CheckedChanged;
            // 
            // NoDrugCheckbox
            // 
            NoDrugCheckbox.AutoSize = true;
            NoDrugCheckbox.Location = new Point(19, 158);
            NoDrugCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoDrugCheckbox.Name = "NoDrugCheckbox";
            NoDrugCheckbox.Size = new Size(71, 19);
            NoDrugCheckbox.TabIndex = 33;
            NoDrugCheckbox.Text = "No Drug";
            NoDrugCheckbox.UseVisualStyleBackColor = true;
            NoDrugCheckbox.CheckedChanged += NoDrugCheckbox_CheckedChanged;
            // 
            // NoRecallCheckbox
            // 
            NoRecallCheckbox.AutoSize = true;
            NoRecallCheckbox.Location = new Point(19, 130);
            NoRecallCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoRecallCheckbox.Name = "NoRecallCheckbox";
            NoRecallCheckbox.Size = new Size(76, 19);
            NoRecallCheckbox.TabIndex = 32;
            NoRecallCheckbox.Text = "No Recall";
            NoRecallCheckbox.UseVisualStyleBackColor = true;
            NoRecallCheckbox.CheckedChanged += NoRecallCheckbox_CheckedChanged;
            // 
            // NoEscapeCheckbox
            // 
            NoEscapeCheckbox.AutoSize = true;
            NoEscapeCheckbox.Location = new Point(19, 103);
            NoEscapeCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoEscapeCheckbox.Name = "NoEscapeCheckbox";
            NoEscapeCheckbox.Size = new Size(81, 19);
            NoEscapeCheckbox.TabIndex = 31;
            NoEscapeCheckbox.Text = "No Escape";
            NoEscapeCheckbox.UseVisualStyleBackColor = true;
            NoEscapeCheckbox.CheckedChanged += NoEscapeCheckbox_CheckedChanged;
            // 
            // NoRandomCheckbox
            // 
            NoRandomCheckbox.AutoSize = true;
            NoRandomCheckbox.Location = new Point(19, 75);
            NoRandomCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoRandomCheckbox.Name = "NoRandomCheckbox";
            NoRandomCheckbox.Size = new Size(90, 19);
            NoRandomCheckbox.TabIndex = 30;
            NoRandomCheckbox.Text = "No Random";
            NoRandomCheckbox.UseVisualStyleBackColor = true;
            NoRandomCheckbox.CheckedChanged += NoRandomCheckbox_CheckedChanged;
            // 
            // LightningTextbox
            // 
            LightningTextbox.Location = new Point(517, 75);
            LightningTextbox.Margin = new Padding(4, 3, 4, 3);
            LightningTextbox.Name = "LightningTextbox";
            LightningTextbox.Size = new Size(56, 23);
            LightningTextbox.TabIndex = 29;
            LightningTextbox.TextChanged += LightningTextbox_TextChanged;
            // 
            // FireTextbox
            // 
            FireTextbox.Location = new Point(517, 47);
            FireTextbox.Margin = new Padding(4, 3, 4, 3);
            FireTextbox.Name = "FireTextbox";
            FireTextbox.Size = new Size(56, 23);
            FireTextbox.TabIndex = 28;
            FireTextbox.TextChanged += FireTextbox_TextChanged;
            // 
            // NoReconnectTextbox
            // 
            NoReconnectTextbox.Location = new Point(138, 45);
            NoReconnectTextbox.Margin = new Padding(4, 3, 4, 3);
            NoReconnectTextbox.Name = "NoReconnectTextbox";
            NoReconnectTextbox.Size = new Size(56, 23);
            NoReconnectTextbox.TabIndex = 27;
            NoReconnectTextbox.TextChanged += NoReconnectTextbox_TextChanged;
            // 
            // LightningCheckbox
            // 
            LightningCheckbox.AutoSize = true;
            LightningCheckbox.Location = new Point(398, 75);
            LightningCheckbox.Margin = new Padding(4, 3, 4, 3);
            LightningCheckbox.Name = "LightningCheckbox";
            LightningCheckbox.Size = new Size(77, 19);
            LightningCheckbox.TabIndex = 26;
            LightningCheckbox.Text = "Lightning";
            LightningCheckbox.UseVisualStyleBackColor = true;
            LightningCheckbox.CheckedChanged += LightningCheckbox_CheckedChanged;
            // 
            // FireCheckbox
            // 
            FireCheckbox.AutoSize = true;
            FireCheckbox.Location = new Point(398, 47);
            FireCheckbox.Margin = new Padding(4, 3, 4, 3);
            FireCheckbox.Name = "FireCheckbox";
            FireCheckbox.Size = new Size(45, 19);
            FireCheckbox.TabIndex = 25;
            FireCheckbox.Text = "Fire";
            FireCheckbox.UseVisualStyleBackColor = true;
            FireCheckbox.CheckStateChanged += FireCheckbox_CheckStateChanged;
            // 
            // FightCheckbox
            // 
            FightCheckbox.AutoSize = true;
            FightCheckbox.Location = new Point(398, 18);
            FightCheckbox.Margin = new Padding(4, 3, 4, 3);
            FightCheckbox.Name = "FightCheckbox";
            FightCheckbox.Size = new Size(53, 19);
            FightCheckbox.TabIndex = 23;
            FightCheckbox.Text = "Fight";
            FightCheckbox.UseVisualStyleBackColor = true;
            FightCheckbox.CheckedChanged += FightCheckbox_CheckedChanged;
            // 
            // NoReconnectCheckbox
            // 
            NoReconnectCheckbox.AutoSize = true;
            NoReconnectCheckbox.Location = new Point(19, 47);
            NoReconnectCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoReconnectCheckbox.Name = "NoReconnectCheckbox";
            NoReconnectCheckbox.Size = new Size(101, 19);
            NoReconnectCheckbox.TabIndex = 22;
            NoReconnectCheckbox.Text = "No Reconnect";
            NoReconnectCheckbox.UseVisualStyleBackColor = true;
            NoReconnectCheckbox.CheckedChanged += NoReconnectCheckbox_CheckedChanged;
            // 
            // NoTeleportCheckbox
            // 
            NoTeleportCheckbox.AutoSize = true;
            NoTeleportCheckbox.Location = new Point(19, 20);
            NoTeleportCheckbox.Margin = new Padding(4, 3, 4, 3);
            NoTeleportCheckbox.Name = "NoTeleportCheckbox";
            NoTeleportCheckbox.Size = new Size(87, 19);
            NoTeleportCheckbox.TabIndex = 21;
            NoTeleportCheckbox.Text = "No Teleport";
            NoTeleportCheckbox.UseVisualStyleBackColor = true;
            NoTeleportCheckbox.CheckedChanged += NoTeleportCheckbox_CheckedChanged;
            // 
            // tabPage3
            // 
            tabPage3.Controls.Add(RemoveSZButton);
            tabPage3.Controls.Add(AddSZButton);
            tabPage3.Controls.Add(SafeZoneInfoPanel);
            tabPage3.Controls.Add(SafeZoneInfoListBox);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Margin = new Padding(4, 3, 4, 3);
            tabPage3.Name = "tabPage3";
            tabPage3.Size = new Size(622, 524);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "Safe Zones";
            tabPage3.UseVisualStyleBackColor = true;
            // 
            // RemoveSZButton
            // 
            RemoveSZButton.Location = new Point(126, 8);
            RemoveSZButton.Margin = new Padding(4, 3, 4, 3);
            RemoveSZButton.Name = "RemoveSZButton";
            RemoveSZButton.Size = new Size(88, 27);
            RemoveSZButton.TabIndex = 8;
            RemoveSZButton.Text = "Remove";
            RemoveSZButton.UseVisualStyleBackColor = true;
            RemoveSZButton.Click += RemoveSZButton_Click;
            // 
            // AddSZButton
            // 
            AddSZButton.Location = new Point(7, 8);
            AddSZButton.Margin = new Padding(4, 3, 4, 3);
            AddSZButton.Name = "AddSZButton";
            AddSZButton.Size = new Size(88, 27);
            AddSZButton.TabIndex = 7;
            AddSZButton.Text = "Add";
            AddSZButton.UseVisualStyleBackColor = true;
            AddSZButton.Click += AddSZButton_Click;
            // 
            // SafeZoneInfoPanel
            // 
            SafeZoneInfoPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            SafeZoneInfoPanel.Controls.Add(label12);
            SafeZoneInfoPanel.Controls.Add(SZYTextBox);
            SafeZoneInfoPanel.Controls.Add(label14);
            SafeZoneInfoPanel.Controls.Add(SizeTextBox);
            SafeZoneInfoPanel.Controls.Add(label17);
            SafeZoneInfoPanel.Controls.Add(SZXTextBox);
            SafeZoneInfoPanel.Controls.Add(StartPointCheckBox);
            SafeZoneInfoPanel.Enabled = false;
            SafeZoneInfoPanel.Location = new Point(220, 40);
            SafeZoneInfoPanel.Margin = new Padding(4, 3, 4, 3);
            SafeZoneInfoPanel.Name = "SafeZoneInfoPanel";
            SafeZoneInfoPanel.Size = new Size(230, 160);
            SafeZoneInfoPanel.TabIndex = 10;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(145, 29);
            label12.Margin = new Padding(4, 0, 4, 0);
            label12.Name = "label12";
            label12.Size = new Size(17, 15);
            label12.TabIndex = 10;
            label12.Text = "Y:";
            // 
            // SZYTextBox
            // 
            SZYTextBox.Location = new Point(172, 25);
            SZYTextBox.Margin = new Padding(4, 3, 4, 3);
            SZYTextBox.MaxLength = 5;
            SZYTextBox.Name = "SZYTextBox";
            SZYTextBox.Size = new Size(42, 23);
            SZYTextBox.TabIndex = 3;
            SZYTextBox.TextChanged += SZYTextBox_TextChanged;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(29, 59);
            label14.Margin = new Padding(4, 0, 4, 0);
            label14.Name = "label14";
            label14.Size = new Size(30, 15);
            label14.TabIndex = 8;
            label14.Text = "Size:";
            // 
            // SizeTextBox
            // 
            SizeTextBox.Location = new Point(71, 55);
            SizeTextBox.Margin = new Padding(4, 3, 4, 3);
            SizeTextBox.MaxLength = 5;
            SizeTextBox.Name = "SizeTextBox";
            SizeTextBox.Size = new Size(42, 23);
            SizeTextBox.TabIndex = 4;
            SizeTextBox.TextChanged += SizeTextBox_TextChanged;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(44, 29);
            label17.Margin = new Padding(4, 0, 4, 0);
            label17.Name = "label17";
            label17.Size = new Size(17, 15);
            label17.TabIndex = 3;
            label17.Text = "X:";
            // 
            // SZXTextBox
            // 
            SZXTextBox.Location = new Point(71, 25);
            SZXTextBox.Margin = new Padding(4, 3, 4, 3);
            SZXTextBox.MaxLength = 5;
            SZXTextBox.Name = "SZXTextBox";
            SZXTextBox.Size = new Size(42, 23);
            SZXTextBox.TabIndex = 2;
            SZXTextBox.TextChanged += SZXTextBox_TextChanged;
            // 
            // StartPointCheckBox
            // 
            StartPointCheckBox.AutoSize = true;
            StartPointCheckBox.Location = new Point(71, 103);
            StartPointCheckBox.Margin = new Padding(4, 3, 4, 3);
            StartPointCheckBox.Name = "StartPointCheckBox";
            StartPointCheckBox.Size = new Size(81, 19);
            StartPointCheckBox.TabIndex = 5;
            StartPointCheckBox.Text = "Start Point";
            StartPointCheckBox.UseVisualStyleBackColor = true;
            StartPointCheckBox.CheckedChanged += StartPointCheckBox_CheckedChanged;
            // 
            // SafeZoneInfoListBox
            // 
            SafeZoneInfoListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            SafeZoneInfoListBox.FormattingEnabled = true;
            SafeZoneInfoListBox.ItemHeight = 15;
            SafeZoneInfoListBox.Location = new Point(7, 40);
            SafeZoneInfoListBox.Margin = new Padding(4, 3, 4, 3);
            SafeZoneInfoListBox.Name = "SafeZoneInfoListBox";
            SafeZoneInfoListBox.SelectionMode = SelectionMode.MultiExtended;
            SafeZoneInfoListBox.Size = new Size(206, 154);
            SafeZoneInfoListBox.TabIndex = 9;
            SafeZoneInfoListBox.SelectedIndexChanged += SafeZoneInfoListBox_SelectedIndexChanged;
            // 
            // tabPage2
            // 
            tabPage2.Controls.Add(RPasteButton);
            tabPage2.Controls.Add(RCopyButton);
            tabPage2.Controls.Add(RemoveRButton);
            tabPage2.Controls.Add(AddRButton);
            tabPage2.Controls.Add(RespawnInfoListBox);
            tabPage2.Controls.Add(RespawnInfoPanel);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Margin = new Padding(4, 3, 4, 3);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(4, 3, 4, 3);
            tabPage2.Size = new Size(622, 524);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "Respawns";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // RPasteButton
            // 
            RPasteButton.Location = new Point(315, 8);
            RPasteButton.Margin = new Padding(4, 3, 4, 3);
            RPasteButton.Name = "RPasteButton";
            RPasteButton.Size = new Size(88, 27);
            RPasteButton.TabIndex = 22;
            RPasteButton.Text = "Paste";
            RPasteButton.UseVisualStyleBackColor = true;
            RPasteButton.Click += RPasteButton_Click;
            // 
            // RCopyButton
            // 
            RCopyButton.Location = new Point(220, 8);
            RCopyButton.Margin = new Padding(4, 3, 4, 3);
            RCopyButton.Name = "RCopyButton";
            RCopyButton.Size = new Size(88, 27);
            RCopyButton.TabIndex = 21;
            RCopyButton.Text = "Copy";
            RCopyButton.UseVisualStyleBackColor = true;
            // 
            // RemoveRButton
            // 
            RemoveRButton.Location = new Point(126, 8);
            RemoveRButton.Margin = new Padding(4, 3, 4, 3);
            RemoveRButton.Name = "RemoveRButton";
            RemoveRButton.Size = new Size(88, 27);
            RemoveRButton.TabIndex = 16;
            RemoveRButton.Text = "Remove";
            RemoveRButton.UseVisualStyleBackColor = true;
            RemoveRButton.Click += RemoveRButton_Click;
            // 
            // AddRButton
            // 
            AddRButton.Location = new Point(7, 8);
            AddRButton.Margin = new Padding(4, 3, 4, 3);
            AddRButton.Name = "AddRButton";
            AddRButton.Size = new Size(88, 27);
            AddRButton.TabIndex = 15;
            AddRButton.Text = "Add";
            AddRButton.UseVisualStyleBackColor = true;
            AddRButton.Click += AddRButton_Click;
            // 
            // RespawnInfoListBox
            // 
            RespawnInfoListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            RespawnInfoListBox.FormattingEnabled = true;
            RespawnInfoListBox.ItemHeight = 15;
            RespawnInfoListBox.Location = new Point(7, 40);
            RespawnInfoListBox.Margin = new Padding(4, 3, 4, 3);
            RespawnInfoListBox.Name = "RespawnInfoListBox";
            RespawnInfoListBox.SelectionMode = SelectionMode.MultiExtended;
            RespawnInfoListBox.Size = new Size(303, 169);
            RespawnInfoListBox.TabIndex = 14;
            RespawnInfoListBox.SelectedIndexChanged += RespawnInfoListBox_SelectedIndexChanged;
            // 
            // RespawnInfoPanel
            // 
            RespawnInfoPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            RespawnInfoPanel.Controls.Add(chkrespawnsave);
            RespawnInfoPanel.Controls.Add(chkRespawnEnableTick);
            RespawnInfoPanel.Controls.Add(Randomtextbox);
            RespawnInfoPanel.Controls.Add(label23);
            RespawnInfoPanel.Controls.Add(label34);
            RespawnInfoPanel.Controls.Add(RoutePathTextBox);
            RespawnInfoPanel.Controls.Add(label24);
            RespawnInfoPanel.Controls.Add(DirectionTextBox);
            RespawnInfoPanel.Controls.Add(label8);
            RespawnInfoPanel.Controls.Add(DelayTextBox);
            RespawnInfoPanel.Controls.Add(label7);
            RespawnInfoPanel.Controls.Add(MonsterInfoComboBox);
            RespawnInfoPanel.Controls.Add(label6);
            RespawnInfoPanel.Controls.Add(SpreadTextBox);
            RespawnInfoPanel.Controls.Add(label9);
            RespawnInfoPanel.Controls.Add(RYTextBox);
            RespawnInfoPanel.Controls.Add(label10);
            RespawnInfoPanel.Controls.Add(CountTextBox);
            RespawnInfoPanel.Controls.Add(label13);
            RespawnInfoPanel.Controls.Add(RXTextBox);
            RespawnInfoPanel.Enabled = false;
            RespawnInfoPanel.Location = new Point(317, 40);
            RespawnInfoPanel.Margin = new Padding(4, 3, 4, 3);
            RespawnInfoPanel.Name = "RespawnInfoPanel";
            RespawnInfoPanel.Size = new Size(296, 237);
            RespawnInfoPanel.TabIndex = 11;
            // 
            // chkrespawnsave
            // 
            chkrespawnsave.AutoSize = true;
            chkrespawnsave.Location = new Point(29, 126);
            chkrespawnsave.Margin = new Padding(4, 3, 4, 3);
            chkrespawnsave.Name = "chkrespawnsave";
            chkrespawnsave.Size = new Size(176, 19);
            chkrespawnsave.TabIndex = 25;
            chkrespawnsave.Text = "Save respawnticks on reboot";
            chkrespawnsave.UseVisualStyleBackColor = true;
            chkrespawnsave.CheckedChanged += chkrespawnsave_CheckedChanged;
            // 
            // chkRespawnEnableTick
            // 
            chkRespawnEnableTick.AutoSize = true;
            chkRespawnEnableTick.Location = new Point(29, 106);
            chkRespawnEnableTick.Margin = new Padding(4, 3, 4, 3);
            chkRespawnEnableTick.Name = "chkRespawnEnableTick";
            chkRespawnEnableTick.Size = new Size(145, 19);
            chkRespawnEnableTick.TabIndex = 24;
            chkRespawnEnableTick.Text = "Use tickbased respawn";
            chkRespawnEnableTick.UseVisualStyleBackColor = true;
            chkRespawnEnableTick.CheckedChanged += chkRespawnEnableTick_CheckedChanged;
            // 
            // Randomtextbox
            // 
            Randomtextbox.Location = new Point(184, 150);
            Randomtextbox.Margin = new Padding(4, 3, 4, 3);
            Randomtextbox.MaxLength = 10;
            Randomtextbox.Multiline = true;
            Randomtextbox.Name = "Randomtextbox";
            Randomtextbox.Size = new Size(42, 22);
            Randomtextbox.TabIndex = 23;
            toolTip1.SetToolTip(Randomtextbox, "Allows random + or - added to each spawn time");
            Randomtextbox.TextChanged += RandomTextBox_TextChanged;
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(158, 153);
            label23.Margin = new Padding(4, 0, 4, 0);
            label23.Name = "label23";
            label23.Size = new Size(17, 15);
            label23.TabIndex = 22;
            label23.Text = "R:";
            // 
            // label34
            // 
            label34.AutoSize = true;
            label34.Location = new Point(26, 183);
            label34.Margin = new Padding(4, 0, 4, 0);
            label34.Name = "label34";
            label34.Size = new Size(41, 15);
            label34.TabIndex = 21;
            label34.Text = "Route:";
            // 
            // RoutePathTextBox
            // 
            RoutePathTextBox.Location = new Point(76, 180);
            RoutePathTextBox.Margin = new Padding(4, 3, 4, 3);
            RoutePathTextBox.Name = "RoutePathTextBox";
            RoutePathTextBox.Size = new Size(151, 23);
            RoutePathTextBox.TabIndex = 20;
            RoutePathTextBox.TextChanged += RoutePathTextBox_TextChanged;
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(26, 213);
            label24.Margin = new Padding(4, 0, 4, 0);
            label24.Name = "label24";
            label24.Size = new Size(25, 15);
            label24.TabIndex = 18;
            label24.Text = "Dir:";
            // 
            // DirectionTextBox
            // 
            DirectionTextBox.Location = new Point(76, 210);
            DirectionTextBox.Margin = new Padding(4, 3, 4, 3);
            DirectionTextBox.MaxLength = 5;
            DirectionTextBox.Name = "DirectionTextBox";
            DirectionTextBox.Size = new Size(42, 23);
            DirectionTextBox.TabIndex = 17;
            DirectionTextBox.TextChanged += DirectionTextBox_TextChanged;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(26, 153);
            label8.Margin = new Padding(4, 0, 4, 0);
            label8.Name = "label8";
            label8.Size = new Size(39, 15);
            label8.TabIndex = 16;
            label8.Text = "Delay:";
            // 
            // DelayTextBox
            // 
            DelayTextBox.Location = new Point(76, 150);
            DelayTextBox.Margin = new Padding(4, 3, 4, 3);
            DelayTextBox.MaxLength = 10;
            DelayTextBox.Multiline = true;
            DelayTextBox.Name = "DelayTextBox";
            DelayTextBox.Size = new Size(72, 22);
            DelayTextBox.TabIndex = 15;
            toolTip1.SetToolTip(DelayTextBox, "if you use tick based spawn: this is ignored!");
            DelayTextBox.TextChanged += DelayTextBox_TextChanged;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(13, 18);
            label7.Margin = new Padding(4, 0, 4, 0);
            label7.Name = "label7";
            label7.Size = new Size(54, 15);
            label7.TabIndex = 14;
            label7.Text = "Monster:";
            // 
            // MonsterInfoComboBox
            // 
            MonsterInfoComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MonsterInfoComboBox.FormattingEnabled = true;
            MonsterInfoComboBox.Location = new Point(76, 15);
            MonsterInfoComboBox.Margin = new Padding(4, 3, 4, 3);
            MonsterInfoComboBox.Name = "MonsterInfoComboBox";
            MonsterInfoComboBox.Size = new Size(151, 23);
            MonsterInfoComboBox.TabIndex = 13;
            MonsterInfoComboBox.SelectedIndexChanged += MonsterInfoComboBox_SelectedIndexChanged;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(126, 80);
            label6.Margin = new Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new Size(46, 15);
            label6.TabIndex = 12;
            label6.Text = "Spread:";
            // 
            // SpreadTextBox
            // 
            SpreadTextBox.Location = new Point(184, 76);
            SpreadTextBox.Margin = new Padding(4, 3, 4, 3);
            SpreadTextBox.MaxLength = 5;
            SpreadTextBox.Name = "SpreadTextBox";
            SpreadTextBox.Size = new Size(42, 23);
            SpreadTextBox.TabIndex = 11;
            SpreadTextBox.TextChanged += SpreadTextBox_TextChanged;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(158, 50);
            label9.Margin = new Padding(4, 0, 4, 0);
            label9.Name = "label9";
            label9.Size = new Size(17, 15);
            label9.TabIndex = 10;
            label9.Text = "Y:";
            // 
            // RYTextBox
            // 
            RYTextBox.Location = new Point(184, 46);
            RYTextBox.Margin = new Padding(4, 3, 4, 3);
            RYTextBox.MaxLength = 5;
            RYTextBox.Name = "RYTextBox";
            RYTextBox.Size = new Size(42, 23);
            RYTextBox.TabIndex = 3;
            RYTextBox.TextChanged += RYTextBox_TextChanged;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(24, 80);
            label10.Margin = new Padding(4, 0, 4, 0);
            label10.Name = "label10";
            label10.Size = new Size(43, 15);
            label10.TabIndex = 8;
            label10.Text = "Count:";
            // 
            // CountTextBox
            // 
            CountTextBox.Location = new Point(76, 76);
            CountTextBox.Margin = new Padding(4, 3, 4, 3);
            CountTextBox.MaxLength = 5;
            CountTextBox.Name = "CountTextBox";
            CountTextBox.Size = new Size(42, 23);
            CountTextBox.TabIndex = 4;
            CountTextBox.TextChanged += CountTextBox_TextChanged;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(49, 50);
            label13.Margin = new Padding(4, 0, 4, 0);
            label13.Name = "label13";
            label13.Size = new Size(17, 15);
            label13.TabIndex = 3;
            label13.Text = "X:";
            // 
            // RXTextBox
            // 
            RXTextBox.Location = new Point(76, 46);
            RXTextBox.Margin = new Padding(4, 3, 4, 3);
            RXTextBox.MaxLength = 5;
            RXTextBox.Name = "RXTextBox";
            RXTextBox.Size = new Size(42, 23);
            RXTextBox.TabIndex = 2;
            RXTextBox.TextChanged += RXTextBox_TextChanged;
            // 
            // tabPage4
            // 
            tabPage4.Controls.Add(RemoveMButton);
            tabPage4.Controls.Add(AddMButton);
            tabPage4.Controls.Add(MovementInfoPanel);
            tabPage4.Controls.Add(MovementInfoListBox);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Margin = new Padding(4, 3, 4, 3);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(4, 3, 4, 3);
            tabPage4.Size = new Size(622, 524);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "Movements";
            tabPage4.UseVisualStyleBackColor = true;
            // 
            // RemoveMButton
            // 
            RemoveMButton.Location = new Point(126, 8);
            RemoveMButton.Margin = new Padding(4, 3, 4, 3);
            RemoveMButton.Name = "RemoveMButton";
            RemoveMButton.Size = new Size(88, 27);
            RemoveMButton.TabIndex = 12;
            RemoveMButton.Text = "Remove";
            RemoveMButton.UseVisualStyleBackColor = true;
            RemoveMButton.Click += RemoveMButton_Click;
            // 
            // AddMButton
            // 
            AddMButton.Location = new Point(7, 8);
            AddMButton.Margin = new Padding(4, 3, 4, 3);
            AddMButton.Name = "AddMButton";
            AddMButton.Size = new Size(88, 27);
            AddMButton.TabIndex = 11;
            AddMButton.Text = "Add";
            AddMButton.UseVisualStyleBackColor = true;
            AddMButton.Click += AddMButton_Click;
            // 
            // MovementInfoPanel
            // 
            MovementInfoPanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MovementInfoPanel.Controls.Add(label26);
            MovementInfoPanel.Controls.Add(BigMapIconTextBox);
            MovementInfoPanel.Controls.Add(ShowBigMapCheckBox);
            MovementInfoPanel.Controls.Add(label25);
            MovementInfoPanel.Controls.Add(ConquestComboBox);
            MovementInfoPanel.Controls.Add(NeedMoveMCheckBox);
            MovementInfoPanel.Controls.Add(NeedHoleMCheckBox);
            MovementInfoPanel.Controls.Add(label22);
            MovementInfoPanel.Controls.Add(DestMapComboBox);
            MovementInfoPanel.Controls.Add(label18);
            MovementInfoPanel.Controls.Add(DestYTextBox);
            MovementInfoPanel.Controls.Add(label21);
            MovementInfoPanel.Controls.Add(DestXTextBox);
            MovementInfoPanel.Controls.Add(label16);
            MovementInfoPanel.Controls.Add(SourceYTextBox);
            MovementInfoPanel.Controls.Add(label20);
            MovementInfoPanel.Controls.Add(SourceXTextBox);
            MovementInfoPanel.Enabled = false;
            MovementInfoPanel.Location = new Point(279, 40);
            MovementInfoPanel.Margin = new Padding(4, 3, 4, 3);
            MovementInfoPanel.Name = "MovementInfoPanel";
            MovementInfoPanel.Size = new Size(299, 237);
            MovementInfoPanel.TabIndex = 14;
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(180, 210);
            label26.Margin = new Padding(4, 0, 4, 0);
            label26.Name = "label26";
            label26.Size = new Size(33, 15);
            label26.TabIndex = 23;
            label26.Text = "Icon:";
            // 
            // BigMapIconTextBox
            // 
            BigMapIconTextBox.Location = new Point(225, 207);
            BigMapIconTextBox.Margin = new Padding(4, 3, 4, 3);
            BigMapIconTextBox.MaxLength = 5;
            BigMapIconTextBox.Name = "BigMapIconTextBox";
            BigMapIconTextBox.Size = new Size(42, 23);
            BigMapIconTextBox.TabIndex = 22;
            BigMapIconTextBox.TextChanged += BigMapIconTextBox_TextChanged;
            // 
            // ShowBigMapCheckBox
            // 
            ShowBigMapCheckBox.AutoSize = true;
            ShowBigMapCheckBox.Location = new Point(16, 210);
            ShowBigMapCheckBox.Margin = new Padding(4, 3, 4, 3);
            ShowBigMapCheckBox.Name = "ShowBigMapCheckBox";
            ShowBigMapCheckBox.Size = new Size(116, 19);
            ShowBigMapCheckBox.TabIndex = 21;
            ShowBigMapCheckBox.Text = "Show on BigMap";
            ShowBigMapCheckBox.UseVisualStyleBackColor = true;
            ShowBigMapCheckBox.CheckedChanged += ShowBigMapCheckBox_CheckedChanged;
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(4, 172);
            label25.Margin = new Padding(4, 0, 4, 0);
            label25.Name = "label25";
            label25.Size = new Size(61, 15);
            label25.TabIndex = 20;
            label25.Text = "Conquest:";
            // 
            // ConquestComboBox
            // 
            ConquestComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            ConquestComboBox.FormattingEnabled = true;
            ConquestComboBox.Location = new Point(75, 168);
            ConquestComboBox.Margin = new Padding(4, 3, 4, 3);
            ConquestComboBox.Name = "ConquestComboBox";
            ConquestComboBox.Size = new Size(212, 23);
            ConquestComboBox.TabIndex = 19;
            ConquestComboBox.SelectedIndexChanged += ConquestComboBox_SelectedIndexChanged;
            // 
            // NeedMoveMCheckBox
            // 
            NeedMoveMCheckBox.AutoSize = true;
            NeedMoveMCheckBox.Location = new Point(16, 142);
            NeedMoveMCheckBox.Margin = new Padding(4, 3, 4, 3);
            NeedMoveMCheckBox.Name = "NeedMoveMCheckBox";
            NeedMoveMCheckBox.Size = new Size(87, 19);
            NeedMoveMCheckBox.TabIndex = 18;
            NeedMoveMCheckBox.Text = "Need Move";
            NeedMoveMCheckBox.UseVisualStyleBackColor = true;
            NeedMoveMCheckBox.CheckedChanged += NeedScriptMCheckBox_CheckedChanged;
            // 
            // NeedHoleMCheckBox
            // 
            NeedHoleMCheckBox.AutoSize = true;
            NeedHoleMCheckBox.Location = new Point(16, 115);
            NeedHoleMCheckBox.Margin = new Padding(4, 3, 4, 3);
            NeedHoleMCheckBox.Name = "NeedHoleMCheckBox";
            NeedHoleMCheckBox.Size = new Size(82, 19);
            NeedHoleMCheckBox.TabIndex = 17;
            NeedHoleMCheckBox.Text = "Need Hole";
            NeedHoleMCheckBox.UseVisualStyleBackColor = true;
            NeedHoleMCheckBox.CheckedChanged += NeedHoleMCheckBox_CheckedChanged;
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(13, 43);
            label22.Margin = new Padding(4, 0, 4, 0);
            label22.Name = "label22";
            label22.Size = new Size(49, 15);
            label22.TabIndex = 16;
            label22.Text = "To Map:";
            // 
            // DestMapComboBox
            // 
            DestMapComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            DestMapComboBox.FormattingEnabled = true;
            DestMapComboBox.Location = new Point(75, 38);
            DestMapComboBox.Margin = new Padding(4, 3, 4, 3);
            DestMapComboBox.Name = "DestMapComboBox";
            DestMapComboBox.Size = new Size(212, 23);
            DestMapComboBox.TabIndex = 15;
            DestMapComboBox.SelectedIndexChanged += DestMapComboBox_SelectedIndexChanged;
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(138, 73);
            label18.Margin = new Padding(4, 0, 4, 0);
            label18.Name = "label18";
            label18.Size = new Size(32, 15);
            label18.TabIndex = 14;
            label18.Text = "To Y:";
            // 
            // DestYTextBox
            // 
            DestYTextBox.Location = new Point(183, 69);
            DestYTextBox.Margin = new Padding(4, 3, 4, 3);
            DestYTextBox.MaxLength = 5;
            DestYTextBox.Name = "DestYTextBox";
            DestYTextBox.Size = new Size(42, 23);
            DestYTextBox.TabIndex = 12;
            DestYTextBox.TextChanged += DestYTextBox_TextChanged;
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(27, 73);
            label21.Margin = new Padding(4, 0, 4, 0);
            label21.Name = "label21";
            label21.Size = new Size(32, 15);
            label21.TabIndex = 13;
            label21.Text = "To X:";
            // 
            // DestXTextBox
            // 
            DestXTextBox.Location = new Point(72, 69);
            DestXTextBox.Margin = new Padding(4, 3, 4, 3);
            DestXTextBox.MaxLength = 5;
            DestXTextBox.Name = "DestXTextBox";
            DestXTextBox.Size = new Size(42, 23);
            DestXTextBox.TabIndex = 11;
            DestXTextBox.TextChanged += DestXTextBox_TextChanged;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(126, 13);
            label16.Margin = new Padding(4, 0, 4, 0);
            label16.Name = "label16";
            label16.Size = new Size(48, 15);
            label16.TabIndex = 10;
            label16.Text = "From Y:";
            // 
            // SourceYTextBox
            // 
            SourceYTextBox.Location = new Point(183, 8);
            SourceYTextBox.Margin = new Padding(4, 3, 4, 3);
            SourceYTextBox.MaxLength = 5;
            SourceYTextBox.Name = "SourceYTextBox";
            SourceYTextBox.Size = new Size(42, 23);
            SourceYTextBox.TabIndex = 3;
            SourceYTextBox.TextChanged += SourceYTextBox_TextChanged;
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(15, 13);
            label20.Margin = new Padding(4, 0, 4, 0);
            label20.Name = "label20";
            label20.Size = new Size(48, 15);
            label20.TabIndex = 3;
            label20.Text = "From X:";
            // 
            // SourceXTextBox
            // 
            SourceXTextBox.Location = new Point(72, 8);
            SourceXTextBox.Margin = new Padding(4, 3, 4, 3);
            SourceXTextBox.MaxLength = 5;
            SourceXTextBox.Name = "SourceXTextBox";
            SourceXTextBox.Size = new Size(42, 23);
            SourceXTextBox.TabIndex = 2;
            SourceXTextBox.TextChanged += SourceXTextBox_TextChanged;
            // 
            // MovementInfoListBox
            // 
            MovementInfoListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            MovementInfoListBox.FormattingEnabled = true;
            MovementInfoListBox.ItemHeight = 15;
            MovementInfoListBox.Location = new Point(7, 40);
            MovementInfoListBox.Margin = new Padding(4, 3, 4, 3);
            MovementInfoListBox.Name = "MovementInfoListBox";
            MovementInfoListBox.SelectionMode = SelectionMode.MultiExtended;
            MovementInfoListBox.Size = new Size(264, 199);
            MovementInfoListBox.TabIndex = 13;
            MovementInfoListBox.SelectedIndexChanged += MovementInfoListBox_SelectedIndexChanged;
            // 
            // tabPage7
            // 
            tabPage7.Controls.Add(MZDeletebutton);
            tabPage7.Controls.Add(MZAddbutton);
            tabPage7.Controls.Add(MineZonepanel);
            tabPage7.Controls.Add(MZListlistBox);
            tabPage7.Location = new Point(4, 24);
            tabPage7.Margin = new Padding(4, 3, 4, 3);
            tabPage7.Name = "tabPage7";
            tabPage7.Padding = new Padding(4, 3, 4, 3);
            tabPage7.Size = new Size(622, 524);
            tabPage7.TabIndex = 6;
            tabPage7.Text = "MineZones";
            tabPage7.UseVisualStyleBackColor = true;
            // 
            // MZDeletebutton
            // 
            MZDeletebutton.Location = new Point(126, 8);
            MZDeletebutton.Margin = new Padding(4, 3, 4, 3);
            MZDeletebutton.Name = "MZDeletebutton";
            MZDeletebutton.Size = new Size(88, 27);
            MZDeletebutton.TabIndex = 12;
            MZDeletebutton.Text = "Remove";
            MZDeletebutton.UseVisualStyleBackColor = true;
            MZDeletebutton.Click += MZDeletebutton_Click;
            // 
            // MZAddbutton
            // 
            MZAddbutton.Location = new Point(7, 8);
            MZAddbutton.Margin = new Padding(4, 3, 4, 3);
            MZAddbutton.Name = "MZAddbutton";
            MZAddbutton.Size = new Size(88, 27);
            MZAddbutton.TabIndex = 11;
            MZAddbutton.Text = "Add";
            MZAddbutton.UseVisualStyleBackColor = true;
            MZAddbutton.Click += MZAddbutton_Click;
            // 
            // MineZonepanel
            // 
            MineZonepanel.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            MineZonepanel.Controls.Add(label27);
            MineZonepanel.Controls.Add(MineZoneComboBox);
            MineZonepanel.Controls.Add(label30);
            MineZonepanel.Controls.Add(MZYtextBox);
            MineZonepanel.Controls.Add(label31);
            MineZonepanel.Controls.Add(MZSizetextBox);
            MineZonepanel.Controls.Add(label32);
            MineZonepanel.Controls.Add(MZXtextBox);
            MineZonepanel.Enabled = false;
            MineZonepanel.Location = new Point(220, 40);
            MineZonepanel.Margin = new Padding(4, 3, 4, 3);
            MineZonepanel.Name = "MineZonepanel";
            MineZonepanel.Size = new Size(281, 160);
            MineZonepanel.TabIndex = 14;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(14, 20);
            label27.Margin = new Padding(4, 0, 4, 0);
            label27.Name = "label27";
            label27.Size = new Size(64, 15);
            label27.TabIndex = 14;
            label27.Text = "Mine Type:";
            // 
            // MineZoneComboBox
            // 
            MineZoneComboBox.DropDownStyle = ComboBoxStyle.DropDownList;
            MineZoneComboBox.FormattingEnabled = true;
            MineZoneComboBox.Location = new Point(91, 16);
            MineZoneComboBox.Margin = new Padding(4, 3, 4, 3);
            MineZoneComboBox.Name = "MineZoneComboBox";
            MineZoneComboBox.Size = new Size(144, 23);
            MineZoneComboBox.TabIndex = 13;
            MineZoneComboBox.SelectedIndexChanged += MineZoneComboBox_SelectedIndexChanged;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(166, 61);
            label30.Margin = new Padding(4, 0, 4, 0);
            label30.Name = "label30";
            label30.Size = new Size(17, 15);
            label30.TabIndex = 10;
            label30.Text = "Y:";
            // 
            // MZYtextBox
            // 
            MZYtextBox.Location = new Point(192, 58);
            MZYtextBox.Margin = new Padding(4, 3, 4, 3);
            MZYtextBox.MaxLength = 5;
            MZYtextBox.Name = "MZYtextBox";
            MZYtextBox.Size = new Size(42, 23);
            MZYtextBox.TabIndex = 3;
            MZYtextBox.TextChanged += MZYtextBox_TextChanged;
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(49, 114);
            label31.Margin = new Padding(4, 0, 4, 0);
            label31.Name = "label31";
            label31.Size = new Size(30, 15);
            label31.TabIndex = 8;
            label31.Text = "Size:";
            // 
            // MZSizetextBox
            // 
            MZSizetextBox.Location = new Point(91, 111);
            MZSizetextBox.Margin = new Padding(4, 3, 4, 3);
            MZSizetextBox.MaxLength = 5;
            MZSizetextBox.Name = "MZSizetextBox";
            MZSizetextBox.Size = new Size(42, 23);
            MZSizetextBox.TabIndex = 4;
            MZSizetextBox.TextChanged += MZSizetextBox_TextChanged;
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(64, 66);
            label32.Margin = new Padding(4, 0, 4, 0);
            label32.Name = "label32";
            label32.Size = new Size(17, 15);
            label32.TabIndex = 3;
            label32.Text = "X:";
            // 
            // MZXtextBox
            // 
            MZXtextBox.Location = new Point(91, 62);
            MZXtextBox.Margin = new Padding(4, 3, 4, 3);
            MZXtextBox.MaxLength = 5;
            MZXtextBox.Name = "MZXtextBox";
            MZXtextBox.Size = new Size(42, 23);
            MZXtextBox.TabIndex = 2;
            MZXtextBox.TextChanged += MZXtextBox_TextChanged;
            // 
            // MZListlistBox
            // 
            MZListlistBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            MZListlistBox.FormattingEnabled = true;
            MZListlistBox.ItemHeight = 15;
            MZListlistBox.Location = new Point(7, 40);
            MZListlistBox.Margin = new Padding(4, 3, 4, 3);
            MZListlistBox.Name = "MZListlistBox";
            MZListlistBox.SelectionMode = SelectionMode.MultiExtended;
            MZListlistBox.Size = new Size(206, 154);
            MZListlistBox.TabIndex = 13;
            MZListlistBox.SelectedIndexChanged += MZListlistBox_SelectedIndexChanged;
            // 
            // RemoveButton
            // 
            RemoveButton.Location = new Point(147, 38);
            RemoveButton.Margin = new Padding(4, 3, 4, 3);
            RemoveButton.Name = "RemoveButton";
            RemoveButton.Size = new Size(88, 27);
            RemoveButton.TabIndex = 6;
            RemoveButton.Text = "Remove";
            RemoveButton.UseVisualStyleBackColor = true;
            RemoveButton.Click += RemoveButton_Click;
            // 
            // AddButton
            // 
            AddButton.Location = new Point(14, 38);
            AddButton.Margin = new Padding(4, 3, 4, 3);
            AddButton.Name = "AddButton";
            AddButton.Size = new Size(88, 27);
            AddButton.TabIndex = 5;
            AddButton.Text = "Add";
            AddButton.UseVisualStyleBackColor = true;
            AddButton.Click += AddButton_Click;
            // 
            // MapInfoListBox
            // 
            MapInfoListBox.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left;
            MapInfoListBox.FormattingEnabled = true;
            MapInfoListBox.ItemHeight = 15;
            MapInfoListBox.Location = new Point(14, 72);
            MapInfoListBox.Margin = new Padding(4, 3, 4, 3);
            MapInfoListBox.Name = "MapInfoListBox";
            MapInfoListBox.SelectionMode = SelectionMode.MultiExtended;
            MapInfoListBox.Size = new Size(220, 529);
            MapInfoListBox.TabIndex = 7;
            MapInfoListBox.SelectedIndexChanged += MapInfoListBox_SelectedIndexChanged;
            // 
            // PasteMapButton
            // 
            PasteMapButton.Location = new Point(336, 38);
            PasteMapButton.Margin = new Padding(4, 3, 4, 3);
            PasteMapButton.Name = "PasteMapButton";
            PasteMapButton.Size = new Size(88, 27);
            PasteMapButton.TabIndex = 24;
            PasteMapButton.Text = "Paste";
            PasteMapButton.UseVisualStyleBackColor = true;
            PasteMapButton.Click += PasteMapButton_Click;
            // 
            // CopyMapButton
            // 
            CopyMapButton.Location = new Point(241, 38);
            CopyMapButton.Margin = new Padding(4, 3, 4, 3);
            CopyMapButton.Name = "CopyMapButton";
            CopyMapButton.Size = new Size(88, 27);
            CopyMapButton.TabIndex = 23;
            CopyMapButton.Text = "Copy";
            CopyMapButton.UseVisualStyleBackColor = true;
            // 
            // ImportMapInfoButton
            // 
            ImportMapInfoButton.Location = new Point(658, 5);
            ImportMapInfoButton.Margin = new Padding(4, 3, 4, 3);
            ImportMapInfoButton.Name = "ImportMapInfoButton";
            ImportMapInfoButton.Size = new Size(102, 27);
            ImportMapInfoButton.TabIndex = 25;
            ImportMapInfoButton.Text = "Import MapInfo";
            ImportMapInfoButton.UseVisualStyleBackColor = true;
            ImportMapInfoButton.Click += ImportMapInfoButton_Click;
            // 
            // ExportMapInfoButton
            // 
            ExportMapInfoButton.Location = new Point(658, 38);
            ExportMapInfoButton.Margin = new Padding(4, 3, 4, 3);
            ExportMapInfoButton.Name = "ExportMapInfoButton";
            ExportMapInfoButton.Size = new Size(102, 27);
            ExportMapInfoButton.TabIndex = 26;
            ExportMapInfoButton.Text = "Export MapInfo";
            ExportMapInfoButton.UseVisualStyleBackColor = true;
            ExportMapInfoButton.Click += ExportMapInfoButton_Click;
            // 
            // ImportMongenButton
            // 
            ImportMongenButton.Location = new Point(766, 3);
            ImportMongenButton.Margin = new Padding(4, 3, 4, 3);
            ImportMongenButton.Name = "ImportMongenButton";
            ImportMongenButton.Size = new Size(100, 27);
            ImportMongenButton.TabIndex = 27;
            ImportMongenButton.Text = "Import Spawns";
            ImportMongenButton.UseVisualStyleBackColor = true;
            ImportMongenButton.Click += ImportMonGenButton_Click;
            // 
            // ExportMongenButton
            // 
            ExportMongenButton.Location = new Point(766, 38);
            ExportMongenButton.Margin = new Padding(4, 3, 4, 3);
            ExportMongenButton.Name = "ExportMongenButton";
            ExportMongenButton.Size = new Size(100, 27);
            ExportMongenButton.TabIndex = 28;
            ExportMongenButton.Text = "Export Spawns";
            ExportMongenButton.UseVisualStyleBackColor = true;
            ExportMongenButton.Click += ExportMonGenButton_Click;
            // 
            // VisualizerButton
            // 
            VisualizerButton.Location = new Point(564, 5);
            VisualizerButton.Margin = new Padding(4, 3, 4, 3);
            VisualizerButton.Name = "VisualizerButton";
            VisualizerButton.Size = new Size(88, 27);
            VisualizerButton.TabIndex = 31;
            VisualizerButton.Text = "Visualizer";
            VisualizerButton.UseVisualStyleBackColor = true;
            VisualizerButton.Click += VisualizerButton_Click;
            // 
            // MapInfoForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(886, 625);
            Controls.Add(VisualizerButton);
            Controls.Add(ExportMongenButton);
            Controls.Add(ImportMongenButton);
            Controls.Add(ExportMapInfoButton);
            Controls.Add(ImportMapInfoButton);
            Controls.Add(PasteMapButton);
            Controls.Add(CopyMapButton);
            Controls.Add(MapTabs);
            Controls.Add(RemoveButton);
            Controls.Add(AddButton);
            Controls.Add(MapInfoListBox);
            Margin = new Padding(4, 3, 4, 3);
            Name = "MapInfoForm";
            Text = "Map Info";
            FormClosed += MapInfoForm_FormClosed;
            MapTabs.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            ((ISupportInitialize)MinimapPreview).EndInit();
            tabPage6.ResumeLayout(false);
            tabPage6.PerformLayout();
            tabPage3.ResumeLayout(false);
            SafeZoneInfoPanel.ResumeLayout(false);
            SafeZoneInfoPanel.PerformLayout();
            tabPage2.ResumeLayout(false);
            RespawnInfoPanel.ResumeLayout(false);
            RespawnInfoPanel.PerformLayout();
            tabPage4.ResumeLayout(false);
            MovementInfoPanel.ResumeLayout(false);
            MovementInfoPanel.PerformLayout();
            tabPage7.ResumeLayout(false);
            MineZonepanel.ResumeLayout(false);
            MineZonepanel.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private TabControl MapTabs;
        private TabPage tabPage3;
        private Button RemoveSZButton;
        private Button AddSZButton;
        private Panel SafeZoneInfoPanel;
        private Label label12;
        private TextBox SZYTextBox;
        private Label label14;
        private TextBox SizeTextBox;
        private Label label17;
        private TextBox SZXTextBox;
        private CheckBox StartPointCheckBox;
        private ListBox SafeZoneInfoListBox;
        private TabPage tabPage2;
        private Button RPasteButton;
        private Button RCopyButton;
        private Button RemoveRButton;
        private Button AddRButton;
        private ListBox RespawnInfoListBox;
        private Panel RespawnInfoPanel;
        private Label label24;
        private TextBox DirectionTextBox;
        private Label label8;
        private TextBox DelayTextBox;
        private Label label7;
        private ComboBox MonsterInfoComboBox;
        private Label label6;
        private TextBox SpreadTextBox;
        private Label label9;
        private TextBox RYTextBox;
        private Label label10;
        private TextBox CountTextBox;
        private Label label13;
        private TextBox RXTextBox;
        private TabPage tabPage4;
        private Button RemoveMButton;
        private Button AddMButton;
        private Panel MovementInfoPanel;
        private Label label22;
        private ComboBox DestMapComboBox;
        private Label label18;
        private TextBox DestYTextBox;
        private Label label21;
        private TextBox DestXTextBox;
        private Label label16;
        private TextBox SourceYTextBox;
        private Label label20;
        private TextBox SourceXTextBox;
        private ListBox MovementInfoListBox;
        private Button RemoveButton;
        private Button AddButton;
        private ListBox MapInfoListBox;
        private Button PasteMapButton;
        private Button CopyMapButton;
        private TabPage tabPage1;
        private Label label15;
        private TextBox BigMapTextBox;
        private ComboBox LightsComboBox;
        private Label label5;
        private Label label1;
        private Label label4;
        private TextBox MapIndexTextBox;
        private TextBox MiniMapTextBox;
        private Label label2;
        private TextBox MapNameTextBox;
        private TextBox FileNameTextBox;
        private Label label3;
        private TabPage tabPage6;
        private CheckBox LightningCheckbox;
        private CheckBox FireCheckbox;
        private CheckBox FightCheckbox;
        private CheckBox NoReconnectCheckbox;
        private CheckBox NoTeleportCheckbox;
        private TextBox LightningTextbox;
        private TextBox FireTextbox;
        private TextBox NoReconnectTextbox;
        private CheckBox NoNamesCheckbox;
        private CheckBox NoDropMonsterCheckbox;
        private CheckBox NoDropPlayerCheckbox;
        private CheckBox NoThrowItemCheckbox;
        private CheckBox NoPositionCheckbox;
        private CheckBox NoDrugCheckbox;
        private CheckBox NoRecallCheckbox;
        private CheckBox NoEscapeCheckbox;
        private CheckBox NoRandomCheckbox;
        private CheckBox NeedHoleMCheckBox;
        private Button ImportMapInfoButton;
        private Button ExportMapInfoButton;
        private Label label19;
        private TextBox MapDarkLighttextBox;
        private TabPage tabPage7;
        private Button MZDeletebutton;
        private Button MZAddbutton;
        private Panel MineZonepanel;
        private Label label30;
        private TextBox MZYtextBox;
        private Label label31;
        private TextBox MZSizetextBox;
        private Label label32;
        private TextBox MZXtextBox;
        private ListBox MZListlistBox;
        private ComboBox MineComboBox;
        private ComboBox MineZoneComboBox;
        private Label label27;
        private Label label33;
        private Button ImportMongenButton;
        private Button ExportMongenButton;
        private Button VisualizerButton;
        private CheckBox NeedBridleCheckbox;
        private CheckBox NoMountCheckbox;
        private Label label34;
        private TextBox RoutePathTextBox;
        private CheckBox NoFightCheckbox;
        private CheckBox NeedMoveMCheckBox;
        private Label label11;
        private TextBox MusicTextBox;
        private Label label23;
        private TextBox Randomtextbox;
        private ToolTip toolTip1;
        private CheckBox chkrespawnsave;
        private CheckBox chkRespawnEnableTick;
        private ComboBox ConquestComboBox;
        private Label label25;
        private CheckBox NoTownTeleportCheckbox;
        private CheckBox NoReincarnation;
        private Label label26;
        private TextBox BigMapIconTextBox;
        private CheckBox ShowBigMapCheckBox;
        private PictureBox MinimapPreview;
        private ListBox lstParticles;
        private Label label48;
    }
}