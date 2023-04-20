using Server.MirDatabase;
using Server.MirEnvir;

namespace Server
{
    public partial class MapInfoForm : Form
    {
        public Envir Envir => SMain.EditEnvir;

        private List<MapInfo> _selectedMapInfos;
        private List<SafeZoneInfo> _selectedSafeZoneInfos;
        private List<RespawnInfo> _selectedRespawnInfos;
        private List<MovementInfo> _selectedMovementInfos;
        private List<MineZone> _selectedMineZones;
        private MapInfo _info;

        public MapInfoForm()
        {
            InitializeComponent();

            List<string> mineItems = new(){ { "Disabled" } };
            Settings.MineSetList.ForEach(x => mineItems.Add(x.Name));
            MineComboBox.DataSource = mineItems;

            MineZoneComboBox.DataSource = mineItems;

            LightsComboBox.Items.AddRange(Enum.GetValues(typeof(LightSetting)).Cast<object>().ToArray());

            List<MonsterInfo> monsterInfoItems = new();
            Envir.MonsterInfoList.ForEach(x => monsterInfoItems.Add(x));
            MonsterInfoComboBox.DataSource = monsterInfoItems;

            List<String> conquestItems = new() { { "None" } };
            Envir.ConquestInfoList.ForEach(x => conquestItems.Add(x.Name));
            ConquestComboBox.DataSource = conquestItems;

            UpdateInterface();
        }
        private void MapInfoForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            Envir.SaveDB();
        }


        private void UpdateInterface()
        {
            //Group<MapInfo> orderedMapInfoList = Envir.MapInfoList.OrderBy(m => m.Title).ToList();

            if (MapInfoListBox.Items.Count != Envir.MapInfoList.Count)
            {
                MapInfoListBox.Items.Clear();
                DestMapComboBox.Items.Clear();

                for (int i = 0; i < Envir.MapInfoList.Count; i++)
                {
                    MapInfoListBox.Items.Add(Envir.MapInfoList[i]);
                    DestMapComboBox.Items.Add(Envir.MapInfoList[i]);
                }
            }

            _selectedMapInfos = MapInfoListBox.SelectedItems.Cast<MapInfo>().ToList();

            if (_selectedMapInfos == null || _selectedMapInfos.Count == 0)
            {
                tabPage1.Show();
                MapTabs.Enabled = false;
                MapIndexTextBox.Text = string.Empty;
                FileNameTextBox.Text = string.Empty;
                MapNameTextBox.Text = string.Empty;
                MiniMapTextBox.Text = string.Empty;
                BigMapTextBox.Text = string.Empty;
                LightsComboBox.SelectedItem = null;
                MineComboBox.SelectedItem = null;
                MusicTextBox.Text = string.Empty;

                NoTeleportCheckbox.Checked = false;
                NoReconnectCheckbox.Checked = false;
                NoRandomCheckbox.Checked = false;
                NoEscapeCheckbox.Checked = false;
                NoRecallCheckbox.Checked = false;
                NoDrugCheckbox.Checked = false;

                NoPositionCheckbox.Checked = false;
                NoThrowItemCheckbox.Checked = false;
                NoDropPlayerCheckbox.Checked = false;
                NoDropMonsterCheckbox.Checked = false;
                NoNamesCheckbox.Checked = false;

                FightCheckbox.Checked = false;
                FireCheckbox.Checked = false;
                LightningCheckbox.Checked = false;
                NoReconnectTextbox.Text = string.Empty;
                FireTextbox.Text = string.Empty;
                LightningTextbox.Text = string.Empty;
                MapDarkLighttextBox.Text = string.Empty;
                //MineIndextextBox.Text = string.Empty;
                return;
            }


            MapInfo mi = _selectedMapInfos[0];

            MapTabs.Enabled = true;

            MapIndexTextBox.Text = mi.Index.ToString();
            FileNameTextBox.Text = mi.FileName;
            MapNameTextBox.Text = mi.Title;
            MiniMapTextBox.Text = mi.MiniMap.ToString();
            BigMapTextBox.Text = mi.BigMap.ToString();
            LightsComboBox.SelectedItem = mi.Light;
            MineComboBox.SelectedIndex = mi.MineIndex;
            MusicTextBox.Text = mi.Music.ToString();

            //map attributes
            NoTeleportCheckbox.Checked = mi.NoTeleport;
            NoReconnectCheckbox.Checked = mi.NoReconnect;
            NoReconnectTextbox.Text = mi.NoReconnectMap;
            NoRandomCheckbox.Checked = mi.NoRandom;
            NoEscapeCheckbox.Checked = mi.NoEscape;
            NoRecallCheckbox.Checked = mi.NoRecall;
            NoDrugCheckbox.Checked = mi.NoDrug;
            NoPositionCheckbox.Checked = mi.NoPosition;
            NoThrowItemCheckbox.Checked = mi.NoThrowItem;
            NoDropPlayerCheckbox.Checked = mi.NoDropPlayer;
            NoDropMonsterCheckbox.Checked = mi.NoDropMonster;
            NoNamesCheckbox.Checked = mi.NoNames;
            FightCheckbox.Checked = mi.Fight;
            NoFightCheckbox.Checked = mi.NoFight;
            FireCheckbox.Checked = mi.Fire;
            FireTextbox.Text = mi.FireDamage.ToString();
            LightningCheckbox.Checked = mi.Lightning;
            LightningTextbox.Text = mi.LightningDamage.ToString();
            MapDarkLighttextBox.Text = mi.MapDarkLight.ToString();

            NoMountCheckbox.Checked = mi.NoMount;
            NeedBridleCheckbox.Checked = mi.NeedBridle;
            //MineIndextextBox.Text = mi.MineIndex.ToString();
            NoTownTeleportCheckbox.Checked = mi.NoTownTeleport;
            NoReincarnation.Checked = mi.NoReincarnation;
            for (int i = 1; i < _selectedMapInfos.Count; i++)
            {
                mi = _selectedMapInfos[i];

                if (MapIndexTextBox.Text != mi.Index.ToString()) MapIndexTextBox.Text = string.Empty;
                if (FileNameTextBox.Text != mi.FileName) FileNameTextBox.Text = string.Empty;
                if (MapNameTextBox.Text != mi.Title) MapNameTextBox.Text = string.Empty;
                if (MiniMapTextBox.Text != mi.MiniMap.ToString()) MiniMapTextBox.Text = string.Empty;
                if (BigMapTextBox.Text != mi.BigMap.ToString()) BigMapTextBox.Text = string.Empty;
                if (LightsComboBox.SelectedItem == null || (LightSetting)LightsComboBox.SelectedItem != mi.Light) LightsComboBox.SelectedItem = null;
                if (MineComboBox.SelectedItem == null || MineComboBox.SelectedIndex != mi.MineIndex) MineComboBox.SelectedIndex = 1;
                if (MusicTextBox.Text != mi.Music.ToString()) MusicTextBox.Text = string.Empty;

                //map attributes
                if (NoTeleportCheckbox.Checked != mi.NoTeleport) NoTeleportCheckbox.Checked = false;
                if (NoReconnectCheckbox.Checked != mi.NoReconnect) NoReconnectCheckbox.Checked = false;
                if (NoReconnectTextbox.Text != mi.NoReconnectMap) NoReconnectTextbox.Text = string.Empty;
                if (NoRandomCheckbox.Checked != mi.NoRandom) NoRandomCheckbox.Checked = false;
                if (NoEscapeCheckbox.Checked != mi.NoEscape) NoEscapeCheckbox.Checked = false;
                if (NoRecallCheckbox.Checked != mi.NoRecall) NoRecallCheckbox.Checked = false;
                if (NoDrugCheckbox.Checked != mi.NoDrug) NoDrugCheckbox.Checked = false;
                if (NoPositionCheckbox.Checked != mi.NoPosition) NoPositionCheckbox.Checked = false;
                if (NoThrowItemCheckbox.Checked != mi.NoThrowItem) NoThrowItemCheckbox.Checked = false;
                if (NoDropPlayerCheckbox.Checked != mi.NoDropPlayer) NoDropPlayerCheckbox.Checked = false;
                if (NoDropMonsterCheckbox.Checked != mi.NoDropMonster) NoDropMonsterCheckbox.Checked = false;
                if (NoNamesCheckbox.Checked != mi.NoNames) NoNamesCheckbox.Checked = false;
                if (FightCheckbox.Checked != mi.Fight) FightCheckbox.Checked = false;
                if (NoFightCheckbox.Checked != mi.NoFight) NoFightCheckbox.Checked = false;
                if (FireCheckbox.Checked != mi.Fire) FireCheckbox.Checked = false;
                if (FireTextbox.Text != mi.FireDamage.ToString()) FireTextbox.Text = string.Empty;
                if (LightningCheckbox.Checked != mi.Lightning) LightningCheckbox.Checked = false;
                if (LightningTextbox.Text != mi.LightningDamage.ToString()) LightningTextbox.Text = string.Empty;
                if (MapDarkLighttextBox.Text != mi.MapDarkLight.ToString()) MapDarkLighttextBox.Text = string.Empty;

                if (NoMountCheckbox.Checked != mi.NoMount) NoMountCheckbox.Checked = false;
                if (NeedBridleCheckbox.Checked != mi.NeedBridle) NeedBridleCheckbox.Checked = false;
                if (NoTownTeleportCheckbox.Checked != mi.NoTownTeleport) NoTownTeleportCheckbox.Checked = false;
                if (NoReincarnation.Checked != mi.NoReincarnation) NoReincarnation.Checked = false;
            }

            UpdateSafeZoneInterface();
            UpdateRespawnInterface();
            UpdateMovementInterface();
            UpdateMineZoneInterface();
        }
        private void UpdateSafeZoneInterface()
        {
            if (_selectedMapInfos.Count != 1)
            {
                SafeZoneInfoListBox.Items.Clear();
                if (_selectedSafeZoneInfos != null && _selectedSafeZoneInfos.Count > 0)
                    _selectedSafeZoneInfos.Clear();
                _info = null;

                SafeZoneInfoPanel.Enabled = false;

                SZXTextBox.Text = string.Empty;
                SZYTextBox.Text = string.Empty;
                StartPointCheckBox.CheckState = CheckState.Unchecked;
                return;
            }

            if (_info != _selectedMapInfos[0])
            {
                SafeZoneInfoListBox.Items.Clear();
                _info = _selectedMapInfos[0];
            }

            if (SafeZoneInfoListBox.Items.Count != _info.SafeZones.Count)
            {
                SafeZoneInfoListBox.Items.Clear();
                for (int i = 0; i < _info.SafeZones.Count; i++) SafeZoneInfoListBox.Items.Add(_info.SafeZones[i]);
            }
            _selectedSafeZoneInfos = SafeZoneInfoListBox.SelectedItems.Cast<SafeZoneInfo>().ToList();

            if (_selectedSafeZoneInfos.Count == 0)
            {
                SafeZoneInfoPanel.Enabled = false;

                SZXTextBox.Text = string.Empty;
                SZYTextBox.Text = string.Empty;
                StartPointCheckBox.CheckState = CheckState.Unchecked;
                return;
            }

            SafeZoneInfo info = _selectedSafeZoneInfos[0];
            SafeZoneInfoPanel.Enabled = true;

            SZXTextBox.Text = info.Location.X.ToString();
            SZYTextBox.Text = info.Location.Y.ToString();
            StartPointCheckBox.CheckState = info.StartPoint ? CheckState.Checked : CheckState.Unchecked;
            SizeTextBox.Text = info.Size.ToString();


            for (int i = 1; i < _selectedSafeZoneInfos.Count; i++)
            {
                info = _selectedSafeZoneInfos[i];

                if (SZXTextBox.Text != info.Location.X.ToString()) SZXTextBox.Text = string.Empty;
                if (SZYTextBox.Text != info.Location.Y.ToString()) SZYTextBox.Text = string.Empty;
                if (StartPointCheckBox.Checked != info.StartPoint) StartPointCheckBox.CheckState = CheckState.Indeterminate;
                if (SizeTextBox.Text != info.Size.ToString()) SizeTextBox.Text = string.Empty;
            }
        }
        private void UpdateRespawnInterface()
        {
            if (_selectedMapInfos.Count != 1)
            {
                RespawnInfoListBox.Items.Clear();
                if (_selectedRespawnInfos != null && _selectedRespawnInfos.Count > 0)
                    _selectedRespawnInfos.Clear();
                _info = null;

                RespawnInfoPanel.Enabled = false;

                MonsterInfoComboBox.SelectedItem = null;
                RXTextBox.Text = string.Empty;
                RYTextBox.Text = string.Empty;
                CountTextBox.Text = string.Empty;
                SpreadTextBox.Text = string.Empty;
                DelayTextBox.Text = string.Empty;
                chkrespawnsave.Enabled = false;
                chkRespawnEnableTick.Checked = false;
                chkrespawnsave.Checked = false;
                DirectionTextBox.Text = string.Empty;
                RoutePathTextBox.Text = string.Empty;
                Randomtextbox.Text = string.Empty;
                return;
            }

            if (_info != _selectedMapInfos[0])
            {
                RespawnInfoListBox.Items.Clear();
                _info = _selectedMapInfos[0];
            }

            if (RespawnInfoListBox.Items.Count != _info.Respawns.Count)
            {
                RespawnInfoListBox.Items.Clear();
                for (int i = 0; i < _info.Respawns.Count; i++) RespawnInfoListBox.Items.Add(_info.Respawns[i]);
            }
            _selectedRespawnInfos = RespawnInfoListBox.SelectedItems.Cast<RespawnInfo>().ToList();

            if (_selectedRespawnInfos.Count == 0)
            {
                RespawnInfoPanel.Enabled = false;

                MonsterInfoComboBox.SelectedItem = null;
                RXTextBox.Text = string.Empty;
                RYTextBox.Text = string.Empty;
                CountTextBox.Text = string.Empty;
                SpreadTextBox.Text = string.Empty;
                DelayTextBox.Text = string.Empty;
                chkrespawnsave.Enabled = false;
                chkRespawnEnableTick.Checked = false;
                chkrespawnsave.Checked = false;
                DirectionTextBox.Text = string.Empty;
                RoutePathTextBox.Text = string.Empty;
                Randomtextbox.Text = string.Empty;
                return;
            }

            RespawnInfo info = _selectedRespawnInfos[0];
            RespawnInfoPanel.Enabled = true;

            MonsterInfoComboBox.SelectedItem = Envir.MonsterInfoList.FirstOrDefault(x => x.Index == info.MonsterIndex);
            RXTextBox.Text = info.Location.X.ToString();
            RYTextBox.Text = info.Location.Y.ToString();
            CountTextBox.Text = info.Count.ToString();
            SpreadTextBox.Text = info.Spread.ToString();
            DelayTextBox.Text = info.RespawnTicks == 0 ? info.Delay.ToString() : info.RespawnTicks.ToString();
            chkrespawnsave.Enabled = info.RespawnTicks != 0;
            chkRespawnEnableTick.Checked = info.RespawnTicks != 0;
            chkrespawnsave.Checked = ((info.RespawnTicks != 0) && (info.SaveRespawnTime)) ? true : false;
            DirectionTextBox.Text = info.Direction.ToString();
            RoutePathTextBox.Text = info.RoutePath;
            Randomtextbox.Enabled = info.RespawnTicks == 0;
            Randomtextbox.Text = info.RandomDelay.ToString();

            for (int i = 1; i < _selectedRespawnInfos.Count; i++)
            {
                info = _selectedRespawnInfos[i];

                if (MonsterInfoComboBox.SelectedItem != Envir.MonsterInfoList.FirstOrDefault(x => x.Index == info.MonsterIndex)) MonsterInfoComboBox.SelectedItem = null;
                if (RXTextBox.Text != info.Location.X.ToString()) RXTextBox.Text = string.Empty;
                if (RYTextBox.Text != info.Location.Y.ToString()) RYTextBox.Text = string.Empty;
                if (CountTextBox.Text != info.Count.ToString()) CountTextBox.Text = string.Empty;
                if (SpreadTextBox.Text != info.Spread.ToString()) SpreadTextBox.Text = string.Empty;
                if (chkRespawnEnableTick.Checked != (info.RespawnTicks == 0))
                {
                    DelayTextBox.Text = string.Empty;
                    chkrespawnsave.Enabled = false;
                    chkrespawnsave.Checked = false;
                    Randomtextbox.Text = string.Empty;
                }
                else
                {
                    if (chkRespawnEnableTick.Checked)
                    {
                        if (DelayTextBox.Text != info.RespawnTicks.ToString()) DelayTextBox.Text = string.Empty;
                        if (chkrespawnsave.Checked != info.SaveRespawnTime)
                        {
                            chkrespawnsave.Enabled = false;
                            chkrespawnsave.Checked = false;
                        }
                        Randomtextbox.Enabled = false;
                        Randomtextbox.Text = string.Empty;
                    }
                    else
                    {
                        if (DelayTextBox.Text != info.Delay.ToString()) DelayTextBox.Text = string.Empty;
                        chkrespawnsave.Enabled = false;
                        chkrespawnsave.Checked = false;
                        if (Randomtextbox.Text != info.RandomDelay.ToString()) Randomtextbox.Text = string.Empty;
                    }
                }
                if (DirectionTextBox.Text != info.Direction.ToString()) DirectionTextBox.Text = string.Empty;
                if (RoutePathTextBox.Text != info.RoutePath) RoutePathTextBox.Text = string.Empty;
            }
        }
        private void UpdateMovementInterface()
        {
            if (_selectedMapInfos.Count != 1)
            {
                MovementInfoListBox.Items.Clear();
                if (_selectedMovementInfos != null && _selectedMovementInfos.Count > 0)
                    _selectedMovementInfos.Clear();
                _info = null;

                MovementInfoPanel.Enabled = false;

                SourceXTextBox.Text = string.Empty;
                SourceYTextBox.Text = string.Empty;
                DestMapComboBox.SelectedItem = null;
                ConquestComboBox.SelectedIndex = 0;
                DestXTextBox.Text = string.Empty;
                DestYTextBox.Text = string.Empty;
                BigMapIconTextBox.Text = string.Empty;
                return;
            }

            if (_info != _selectedMapInfos[0])
            {
                MovementInfoListBox.Items.Clear();
                _info = _selectedMapInfos[0];
            }

            if (MovementInfoListBox.Items.Count != _info.Movements.Count)
            {
                MovementInfoListBox.Items.Clear();
                for (int i = 0; i < _info.Movements.Count; i++) MovementInfoListBox.Items.Add(_info.Movements[i]);
            }
            _selectedMovementInfos = MovementInfoListBox.SelectedItems.Cast<MovementInfo>().ToList();

            if (_selectedMovementInfos.Count == 0)
            {
                MovementInfoPanel.Enabled = false;

                SourceXTextBox.Text = string.Empty;
                SourceYTextBox.Text = string.Empty;
                NeedHoleMCheckBox.Checked = false;
                NeedMoveMCheckBox.Checked = false;
                DestMapComboBox.SelectedItem = null;
                ConquestComboBox.SelectedIndex = 0;
                DestXTextBox.Text = string.Empty;
                DestYTextBox.Text = string.Empty;
                BigMapIconTextBox.Text = string.Empty;
                return;
            }

            MovementInfo info = _selectedMovementInfos[0];

            MovementInfoPanel.Enabled = true;

            SourceXTextBox.Text = info.Source.X.ToString();
            SourceYTextBox.Text = info.Source.Y.ToString();
            NeedHoleMCheckBox.Checked = info.NeedHole;
            NeedMoveMCheckBox.Checked = info.NeedMove;
            ShowBigMapCheckBox.Checked = info.ShowOnBigMap;
            DestMapComboBox.SelectedItem = Envir.MapInfoList.FirstOrDefault(x => x.Index == info.MapIndex);
            DestXTextBox.Text = info.Destination.X.ToString();
            DestYTextBox.Text = info.Destination.Y.ToString();
            BigMapIconTextBox.Text = info.Icon.ToString();

            ConquestComboBox.SelectedItem = Envir.ConquestInfoList.FirstOrDefault(x => x.Index == info.ConquestIndex)?.Name;
            if (ConquestComboBox.SelectedItem == null) ConquestComboBox.SelectedIndex = 0;

            for (int i = 1; i < _selectedMovementInfos.Count; i++)
            {
                info = _selectedMovementInfos[i];

                SourceXTextBox.Text = info.Source.X.ToString();
                SourceYTextBox.Text = info.Source.Y.ToString();
                DestMapComboBox.SelectedItem = Envir.MapInfoList.FirstOrDefault(x => x.Index == info.MapIndex);
                DestXTextBox.Text = info.Destination.X.ToString();
                DestYTextBox.Text = info.Destination.Y.ToString();
                ConquestComboBox.SelectedItem = Envir.ConquestInfoList.FirstOrDefault(x => x.Index == info.ConquestIndex)?.Name;
                BigMapIconTextBox.Text = info.Icon.ToString();

                if (SourceXTextBox.Text != info.Source.X.ToString()) SourceXTextBox.Text = string.Empty;
                if (SourceYTextBox.Text != info.Source.Y.ToString()) SourceYTextBox.Text = string.Empty;

                if (DestMapComboBox.SelectedItem != Envir.MapInfoList.FirstOrDefault(x => x.Index == info.MapIndex)) DestMapComboBox.SelectedItem = null;

                if (DestXTextBox.Text != info.Destination.X.ToString()) DestXTextBox.Text = string.Empty;
                if (DestYTextBox.Text != info.Destination.Y.ToString()) DestYTextBox.Text = string.Empty;

                if (BigMapIconTextBox.Text != info.Icon.ToString()) BigMapIconTextBox.Text = string.Empty;
                if (ConquestComboBox.SelectedItem != Envir.ConquestInfoList.FirstOrDefault(x => x.Index == info.ConquestIndex)) ConquestComboBox.SelectedItem = null;
            }

        }

        private void UpdateMineZoneInterface()
        {
            if (_selectedMapInfos.Count != 1)
            {
                MZListlistBox.Items.Clear();

                if (_selectedMineZones != null && _selectedMineZones.Count > 0)
                    _selectedMineZones.Clear();
                _info = null;

                MineZonepanel.Enabled = false;
                MZXtextBox.Text = string.Empty;
                MZYtextBox.Text = string.Empty;
                MineZoneComboBox.SelectedItem = null;
                MZSizetextBox.Text = string.Empty;
                return;
            }

            if (_info != _selectedMapInfos[0])
            {
                MZListlistBox.Items.Clear();
                _info = _selectedMapInfos[0];
            }
            if (MZListlistBox.Items.Count != _info.MineZones.Count)
            {
                MZListlistBox.Items.Clear();
                for (int i = 0; i < _info.MineZones.Count; i++) MZListlistBox.Items.Add(_info.MineZones[i]);
            }
            _selectedMineZones = MZListlistBox.SelectedItems.Cast<MineZone>().ToList();
            if (_selectedMineZones.Count == 0)
            {
                MineZonepanel.Enabled = false;
                MZXtextBox.Text = string.Empty;
                MZYtextBox.Text = string.Empty;
                MineZoneComboBox.SelectedItem = null;
                MZSizetextBox.Text = string.Empty;
                return;
            }
            MineZone info = _selectedMineZones[0];
            MineZonepanel.Enabled = true;

            MZXtextBox.Text = info.Location.X.ToString();
            MZYtextBox.Text = info.Location.Y.ToString();
            MineZoneComboBox.SelectedIndex = info.Mine;
            MZSizetextBox.Text = info.Size.ToString();

            for (int i = 1; i < _selectedMineZones.Count; i++)
            {
                info = _selectedMineZones[i];

                if (MZXtextBox.Text != info.Location.X.ToString()) MZXtextBox.Text = string.Empty;
                if (MZYtextBox.Text != info.Location.Y.ToString()) MZYtextBox.Text = string.Empty;
                if (MineComboBox.SelectedIndex != info.Mine) MineComboBox.SelectedIndex = 1;
                if (MZSizetextBox.Text != info.Size.ToString()) MZSizetextBox.Text = string.Empty;
            }
        }

        private void RefreshMapList()
        {
            MapInfoListBox.SelectedIndexChanged -= MapInfoListBox_SelectedIndexChanged;

            List<bool> selected = new List<bool>();

            for (int i = 0; i < MapInfoListBox.Items.Count; i++) selected.Add(MapInfoListBox.GetSelected(i));
            MapInfoListBox.Items.Clear();
            for (int i = 0; i < Envir.MapInfoList.Count; i++) MapInfoListBox.Items.Add(Envir.MapInfoList[i]);
            for (int i = 0; i < selected.Count; i++) MapInfoListBox.SetSelected(i, selected[i]);

            MapInfoListBox.SelectedIndexChanged += MapInfoListBox_SelectedIndexChanged;
        }
        private void RefreshSafeZoneList()
        {
            SafeZoneInfoListBox.SelectedIndexChanged -= SafeZoneInfoListBox_SelectedIndexChanged;

            List<bool> selected = new List<bool>();

            for (int i = 0; i < SafeZoneInfoListBox.Items.Count; i++) selected.Add(SafeZoneInfoListBox.GetSelected(i));
            SafeZoneInfoListBox.Items.Clear();
            for (int i = 0; i < _info.SafeZones.Count; i++) SafeZoneInfoListBox.Items.Add(_info.SafeZones[i]);
            for (int i = 0; i < selected.Count; i++) SafeZoneInfoListBox.SetSelected(i, selected[i]);

            SafeZoneInfoListBox.SelectedIndexChanged += SafeZoneInfoListBox_SelectedIndexChanged;
        }
        private void RefreshRespawnList()
        {
            RespawnInfoListBox.SelectedIndexChanged -= RespawnInfoListBox_SelectedIndexChanged;

            List<bool> selected = new List<bool>();

            for (int i = 0; i < RespawnInfoListBox.Items.Count; i++) 
                selected.Add(RespawnInfoListBox.GetSelected(i));

            RespawnInfoListBox.Items.Clear();

            for (int i = 0; i < _info.Respawns.Count; i++) 
                RespawnInfoListBox.Items.Add(_info.Respawns[i]);

            for (int i = 0; i < selected.Count; i++) 
                RespawnInfoListBox.SetSelected(i, selected[i]);

            RespawnInfoListBox.SelectedIndexChanged += RespawnInfoListBox_SelectedIndexChanged;
        }
        private void RefreshMovementList()
        {
            MovementInfoListBox.SelectedIndexChanged -= MovementInfoListBox_SelectedIndexChanged;

            List<bool> selected = new List<bool>();

            for (int i = 0; i < MovementInfoListBox.Items.Count; i++) selected.Add(MovementInfoListBox.GetSelected(i));
            MovementInfoListBox.Items.Clear();
            for (int i = 0; i < _info.Movements.Count; i++) MovementInfoListBox.Items.Add(_info.Movements[i]);
            for (int i = 0; i < selected.Count; i++) MovementInfoListBox.SetSelected(i, selected[i]);

            MovementInfoListBox.SelectedIndexChanged += MovementInfoListBox_SelectedIndexChanged;
        }

        private void RefreshMineZoneList()
        {

            MZListlistBox.SelectedIndexChanged -= MZListlistBox_SelectedIndexChanged;

            List<bool> selected = new List<bool>();

            for (int i = 0; i < MZListlistBox.Items.Count; i++) selected.Add(MZListlistBox.GetSelected(i));
            MZListlistBox.Items.Clear();
            for (int i = 0; i < _info.MineZones.Count; i++) MZListlistBox.Items.Add(_info.MineZones[i]);
            for (int i = 0; i < selected.Count; i++) MZListlistBox.SetSelected(i, selected[i]);
            MZListlistBox.SelectedIndexChanged += MZListlistBox_SelectedIndexChanged;
        }

        private void AddButton_Click(object sender, EventArgs e)
        {
            Envir.CreateMapInfo();
            UpdateInterface();
        }
        private void RemoveButton_Click(object sender, EventArgs e)
        {
            if (_selectedMapInfos.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected maps?", "Remove Maps?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++) Envir.Remove(_selectedMapInfos[i]);

            if (Envir.MapInfoList.Count == 0) Envir.MapIndex = 0;

            MapTabs.SelectTab(0);

            UpdateInterface();
        }
        private void MapInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            SafeZoneInfoListBox.Items.Clear();
            RespawnInfoListBox.Items.Clear();
            MovementInfoListBox.Items.Clear();
            MZListlistBox.Items.Clear();
            UpdateInterface();
        }
        private void FileNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].FileName = ActiveControl.Text;
        }
        private void MapNameTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].Title = ActiveControl.Text;

            RefreshMapList();
        }
        private void MiniMapTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].MiniMap = temp;
        }
        private void LightsComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].Light = (LightSetting)LightsComboBox.SelectedItem;
        }


        private void AddSZButton_Click(object sender, EventArgs e)
        {
            if (_info == null) return;

            _info.CreateSafeZone();
            UpdateSafeZoneInterface();
        }
        private void RemoveSZButton_Click(object sender, EventArgs e)
        {
            if (_selectedSafeZoneInfos.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected SafeZones?", "Remove SafeZones?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            for (int i = 0; i < _selectedSafeZoneInfos.Count; i++) _info.SafeZones.Remove(_selectedSafeZoneInfos[i]);

            UpdateSafeZoneInterface();
        }
        private void SafeZoneInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateSafeZoneInterface();
        }
        private void SZXTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedSafeZoneInfos.Count; i++)
                _selectedSafeZoneInfos[i].Location.X = temp;

            RefreshSafeZoneList();
        }
        private void SZYTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedSafeZoneInfos.Count; i++)
                _selectedSafeZoneInfos[i].Location.Y = temp;

            RefreshSafeZoneList();
        }
        private void SizeTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedSafeZoneInfos.Count; i++)
                _selectedSafeZoneInfos[i].Size = temp;
        }
        private void StartPointCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedSafeZoneInfos.Count; i++)
                _selectedSafeZoneInfos[i].StartPoint = StartPointCheckBox.Checked;

            RefreshSafeZoneList();
        }



        private void AddRButton_Click(object sender, EventArgs e)
        {
            if (_info == null) return;

            _info.CreateRespawnInfo();
            UpdateRespawnInterface();
        }
        private void RemoveRButton_Click(object sender, EventArgs e)
        {
            if (_selectedRespawnInfos.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected Respawns?", "Remove Respawns?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            for (int i = 0; i < _selectedRespawnInfos.Count; i++) _info.Respawns.Remove(_selectedRespawnInfos[i]);

            UpdateRespawnInterface();
        }
        private void RespawnInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateRespawnInterface();
        }
        private void MonsterInfoComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            MonsterInfo info = MonsterInfoComboBox.SelectedItem as MonsterInfo;

            if (info == null) return;

            for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                _selectedRespawnInfos[i].MonsterIndex = info.Index;

            RefreshRespawnList();

        }
        private void RXTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                _selectedRespawnInfos[i].Location.X = temp;

            RefreshRespawnList();
        }
        private void RYTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                _selectedRespawnInfos[i].Location.Y = temp;

            RefreshRespawnList();
        }
        private void CountTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                _selectedRespawnInfos[i].Count = temp;

            RefreshRespawnList();
        }
        private void SpreadTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                _selectedRespawnInfos[i].Spread = temp;

            RefreshRespawnList();
        }
        private void DelayTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedRespawnInfos.Count; i++)
            {
                if (chkRespawnEnableTick.Checked)
                {
                    _selectedRespawnInfos[i].RespawnTicks = Math.Max((ushort)1, temp);//you can never have respawnticks set to 0 or it would bug the entire thing really
                    _selectedRespawnInfos[i].Delay = 0;
                }
                else
                {
                    _selectedRespawnInfos[i].Delay = temp;
                    _selectedRespawnInfos[i].RespawnTicks = 0;
                }
            }

            RefreshRespawnList();
        }
        private void DirectionTextBox_TextChanged(object sender, EventArgs e)
        {

            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                _selectedRespawnInfos[i].Direction = temp;

            RefreshRespawnList();
        }

        private void RoutePathTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                _selectedRespawnInfos[i].RoutePath = ActiveControl.Text;

            RefreshRespawnList();
        }

        private void RPasteButton_Click(object sender, EventArgs e)
        {
            if (_info == null) return;

            string data = Clipboard.GetText();

            if (!data.StartsWith("Respawn", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Cannot Paste, Copied data is not Respawn Information.");
                return;
            }


            string[] respawns = data.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);


            for (int i = 1; i < respawns.Length; i++)
            {
                RespawnInfo info = RespawnInfo.FromText(respawns[i]);

                if (info == null) continue;

                _info.Respawns.Add(info);
            }

            UpdateRespawnInterface();
        }
        //RCopy




        private void AddMButton_Click(object sender, EventArgs e)
        {
            if (_info == null) return;

            _info.CreateMovementInfo();
            UpdateMovementInterface();
        }
        private void RemoveMButton_Click(object sender, EventArgs e)
        {
            if (_selectedMovementInfos.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected Movements?", "Remove Movements?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            for (int i = 0; i < _selectedMovementInfos.Count; i++) _info.Movements.Remove(_selectedMovementInfos[i]);

            UpdateMovementInterface();
        }
        private void SourceXTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMovementInfos.Count; i++)
                _selectedMovementInfos[i].Source.X = temp;

            RefreshMovementList();
        }
        private void SourceYTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMovementInfos.Count; i++)
                _selectedMovementInfos[i].Source.Y = temp;

            RefreshMovementList();
        }
        private void DestXTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMovementInfos.Count; i++)
                _selectedMovementInfos[i].Destination.X = temp;

            RefreshMovementList();
        }
        private void DestYTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMovementInfos.Count; i++)
                _selectedMovementInfos[i].Destination.Y = temp;

            RefreshMovementList();
        }
        private void DestMapComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            MapInfo info = DestMapComboBox.SelectedItem as MapInfo;

            if (info == null) return;

            for (int i = 0; i < _selectedMovementInfos.Count; i++)
                _selectedMovementInfos[i].MapIndex = info.Index;

            RefreshMovementList();

        }
        private void NeedHoleMCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMovementInfos.Count; i++)
                _selectedMovementInfos[i].NeedHole = NeedHoleMCheckBox.Checked;
        }

        private void NeedScriptMCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMovementInfos.Count; i++)
                _selectedMovementInfos[i].NeedMove = NeedMoveMCheckBox.Checked;
        }

        private void MovementInfoListBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMovementInterface();
        }

        private void PasteMapButton_Click(object sender, EventArgs e)
        {
            string data = Clipboard.GetText();

            if (!data.StartsWith("Map", StringComparison.OrdinalIgnoreCase))
            {
                MessageBox.Show("Cannot Paste, Copied data is not Map Information.");
                return;
            }


            string[] maps = data.Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);


            for (int i = 1; i < maps.Length; i++)
                MapInfo.FromText(maps[i]);

            UpdateInterface();
        }

        private void BigMapTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].BigMap = temp;
        }



        private void NoTeleportCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoTeleport = NoTeleportCheckbox.Checked;
        }
        private void NoReconnectCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoReconnect = NoReconnectCheckbox.Checked;
        }
        private void NoReconnectTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoReconnectMap = ActiveControl.Text;
        }
        private void NoRandomCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoRandom = NoRandomCheckbox.Checked;
        }
        private void NoEscapeCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoEscape = NoEscapeCheckbox.Checked;
        }
        private void NoRecallCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoRecall = NoRecallCheckbox.Checked;
        }
        private void NoDrugCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoDrug = NoDrugCheckbox.Checked;
        }
        private void NoPositionCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoPosition = NoPositionCheckbox.Checked;
        }
        private void NoThrowItemCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoThrowItem = NoThrowItemCheckbox.Checked;
        }
        private void NoDropPlayerCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoDropPlayer = NoDropPlayerCheckbox.Checked;
        }
        private void NoDropMonsterCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoDropMonster = NoDropMonsterCheckbox.Checked;
        }
        private void NoNamesCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoNames = NoNamesCheckbox.Checked;
        }
        private void FightCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].Fight = FightCheckbox.Checked;
        }
        private void FireCheckbox_CheckStateChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].Fire = FireCheckbox.Checked;
        }
        private void FireTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].FireDamage = temp;
        }
        private void LightningCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].Lightning = LightningCheckbox.Checked;
        }
        private void LightningTextbox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].LightningDamage = temp;
        }

        private void ClearHButton_Click(object sender, EventArgs e)
        {

        }

        private void MapDarkLighttextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if (!byte.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].MapDarkLight = temp;
        }

        private void MZDeletebutton_Click(object sender, EventArgs e)
        {
            if (_selectedMineZones.Count == 0) return;

            if (MessageBox.Show("Are you sure you want to remove the selected MineZones?", "Remove MineZones?", MessageBoxButtons.YesNo) != DialogResult.Yes) return;

            for (int i = 0; i < _selectedMineZones.Count; i++) _info.MineZones.Remove(_selectedMineZones[i]);
            UpdateMineZoneInterface();
        }

        private void MZAddbutton_Click(object sender, EventArgs e)
        {
            if (_info == null) return;

            _info.MineZones.Add(new MineZone());
            UpdateMineZoneInterface();
        }

        private void MZListlistBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            UpdateMineZoneInterface();
        }

        private void MZMineIndextextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            byte temp;

            if ((!byte.TryParse(ActiveControl.Text, out temp)) || (Settings.MineSetList.Count < temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMineZones.Count; i++)
                _selectedMineZones[i].Mine = temp;
            RefreshMineZoneList();
        }

        private void MZXtextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMineZones.Count; i++)
                _selectedMineZones[i].Location.X = temp;
            RefreshMineZoneList();
        }

        private void MZYtextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMineZones.Count; i++)
                _selectedMineZones[i].Location.Y = temp;
            RefreshMineZoneList();
        }

        private void MZSizetextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMineZones.Count; i++)
                _selectedMineZones[i].Size = temp;
            RefreshMineZoneList();
        }

        private void ImportMapInfoButton_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "Text File|*.txt";
            ofd.ShowDialog();

            if (ofd.FileName == string.Empty) return;

            MirForms.ConvertMapInfo.Path = ofd.FileName;
            MirForms.ConvertMapInfo.Start(Envir);

            MirForms.ConvertMapInfo.End();
            UpdateInterface();

        }
        private void ExportMapInfoButton_Click(object sender, EventArgs e)
        {
            if (_selectedMapInfos.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Path.Combine(Application.StartupPath, "Exports");
            sfd.FileName = "MapInfoExport";
            sfd.Filter = "Text File|*.txt";
            sfd.ShowDialog();

            if (sfd.FileName == string.Empty) return;
            for (int i = 0; i < _selectedMapInfos.Count; i++)
            {
                using (StreamWriter sw = File.AppendText(sfd.FileNames[0]))
                {
                    string textOut = string.Empty;
                    textOut += $"[{_selectedMapInfos[i].FileName} {_selectedMapInfos[i].Title.Replace(' ', '*')}]";

                    textOut += " LIGHT(" + _selectedMapInfos[i].Light + ")";
                    textOut += " MINIMAP(" + _selectedMapInfos[i].MiniMap + ")";
                    textOut += " BIGMAP(" + _selectedMapInfos[i].BigMap + ")";
                    textOut += " MAPLIGHT(" + _selectedMapInfos[i].MapDarkLight + ")";
                    textOut += " MINE(" + _selectedMapInfos[i].MineIndex + ")";
                    textOut += " MUSIC(" + _selectedMapInfos[i].Music + ")";
                    textOut += PrintMapAttributes(_selectedMapInfos[i]);
                    sw.WriteLine(textOut);

                    //STARTZONE(0,150,150,50) || SAFEZONE(0,150,150,50)
                    for (int j = 0; j < _selectedMapInfos[i].SafeZones.Count; j++)
                    {
                        string safeZoneOut = _selectedMapInfos[i].SafeZones[j].StartPoint ? "STARTZONE" : "SAFEZONE";
                        safeZoneOut += "(" + _selectedMapInfos[i].FileName + "," +
                            _selectedMapInfos[i].SafeZones[j].Size.ToString() + "," +
                            _selectedMapInfos[i].SafeZones[j].Location.X.ToString() + "," +
                            _selectedMapInfos[i].SafeZones[j].Location.Y.ToString() + ")";
                        sw.WriteLine(safeZoneOut);
                    }
                    for (int j = 0; j < _selectedMapInfos[i].Movements.Count; j++)
                    {
                        try
                        {
                            string movement =
                                $"{_selectedMapInfos[i].FileName} {_selectedMapInfos[i].Movements[j].Source.X + "," + _selectedMapInfos[i].Movements[j].Source.Y} {"->"} {Envir.MapInfoList[_selectedMapInfos[i].Movements[j].MapIndex - 1].FileName} {_selectedMapInfos[i].Movements[j].Destination.X + "," + _selectedMapInfos[i].Movements[j].Destination.Y} {(_selectedMapInfos[i].Movements[j].NeedHole ? "NEEDHOLE " : "") + (_selectedMapInfos[i].Movements[j].NeedMove ? "NEEDMOVE " : "") + (_selectedMapInfos[i].Movements[j].ConquestIndex > 0 ? "NEEDCONQUEST(" + _selectedMapInfos[i].Movements[j].ConquestIndex + ")" : "") + (_selectedMapInfos[i].Movements[j].ShowOnBigMap ? "SHOWONBIGMAP " : "") + (_selectedMapInfos[i].Movements[j].Icon > 0 ? "BIGMAPICON(" + _selectedMapInfos[i].Movements[j].Icon + ")" : "")}";

                            sw.WriteLine(movement);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                    for (int j = 0; j < _selectedMapInfos[i].MineZones.Count; j++)
                    {
                        try
                        {
                            string mineZones =
                                $"MINEZONE {_selectedMapInfos[i].FileName} -> {_selectedMapInfos[i].MineZones[j].Mine.ToString()} {_selectedMapInfos[i].MineZones[j].Location.X.ToString()} {_selectedMapInfos[i].MineZones[j].Location.Y.ToString()} {_selectedMapInfos[i].MineZones[j].Size.ToString()}";
                            sw.WriteLine(mineZones);
                        }
                        catch
                        {
                            continue;
                        }
                    }
                }
            }
            MessageBox.Show("Map Info Export Complete");
        }
        private String PrintMapAttributes(MapInfo map)
        {
            string textOut = string.Empty;
            if (map.NoTeleport) textOut += " NOTELEPORT";
            if (map.NoReconnect) textOut += " NORECONNECT(" + map.NoReconnectMap + ")";
            if (map.NoRandom) textOut += " NORANDOMMOVE";
            if (map.NoEscape) textOut += " NOESCAPE";
            if (map.NoRecall) textOut += " NORECALL";
            if (map.NoDrug) textOut += " NODRUG";
            if (map.NoPosition) textOut += " NOPOSITIONMOVE";
            if (map.NoThrowItem) textOut += " NOTHROWITEM";
            if (map.NoDropPlayer) textOut += " NOPLAYERDROP";
            if (map.NoDropMonster) textOut += " NOMONSTERDROP";
            if (map.NoNames) textOut += " NONAMES";
            if (map.NoMount) textOut += " NOMOUNT";
            if (map.NeedBridle) textOut += " NEEDBRIDLE";
            if (map.NoFight) textOut += " NOFIGHT";
            if (map.Fight) textOut += " FIGHT";
            if (map.Fire) textOut += " FIRE(" + map.FireDamage + ")";
            if (map.Lightning) textOut += " LIGHTNING(" + map.LightningDamage + ")";
            if (map.NoTownTeleport) textOut += " NOTownTeleport";
            return textOut;
        }
        private void ImportMonGenButton_Click(object sender, EventArgs e)
        {
            bool hasImported = false;

            if (Envir.MapInfoList.Count == 0) return;

            MirForms.ConvertMonGenInfo.Start();

            for (int i = 0; i < MirForms.ConvertMonGenInfo.monGenList.Count; i++)
            {
                try
                {
                    int monsterIndex = Envir.MonsterInfoList.Find(a => a.Name == MirForms.ConvertMonGenInfo.monGenList[i].Name).Index;
                    if (monsterIndex == -1) continue;

                    RespawnInfo respawnInfo = new RespawnInfo
                    {
                        MonsterIndex = monsterIndex,
                        Location = new Point(MirForms.ConvertMonGenInfo.monGenList[i].X, MirForms.ConvertMonGenInfo.monGenList[i].Y),
                        Count = (ushort)MirForms.ConvertMonGenInfo.monGenList[i].Count,
                        Spread = (ushort)MirForms.ConvertMonGenInfo.monGenList[i].Range,
                        Delay = (ushort)MirForms.ConvertMonGenInfo.monGenList[i].Delay,
                        Direction = (byte)MirForms.ConvertMonGenInfo.monGenList[i].Direction,
                        RoutePath = MirForms.ConvertMonGenInfo.monGenList[i].RoutePath,
                        RespawnIndex = ++Envir.RespawnIndex
                    };

                    int index = Envir.MapInfoList.FindIndex(a => a.FileName == MirForms.ConvertMonGenInfo.monGenList[i].Map);
                    if (index == -1) continue;

                    Envir.MapInfoList[index].Respawns.Add(respawnInfo);
                    hasImported = true;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            MirForms.ConvertMonGenInfo.Stop();

            if (!hasImported) return;

            UpdateInterface();
            MessageBox.Show("MonGen Import complete");
        }
        private void ExportMonGenButton_Click(object sender, EventArgs e)
        {
            if (_selectedMapInfos.Count == 0) return;

            SaveFileDialog sfd = new SaveFileDialog();
            sfd.InitialDirectory = Path.Combine(Application.StartupPath, "Exports");
            sfd.Filter = "Text File|*.txt";
            sfd.ShowDialog();

            if (sfd.FileName == string.Empty) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
            {
                using (StreamWriter sw = File.AppendText(sfd.FileNames[0]))
                {
                    for (int j = 0; j < _selectedMapInfos[i].Respawns.Count; j++)
                    {
                        MonsterInfo mob = Envir.GetMonsterInfo(_selectedMapInfos[i].Respawns[j].MonsterIndex);

                        if (mob == null) continue;

                        string Output = $"{_selectedMapInfos[i].FileName}" +
                            $",{_selectedMapInfos[i].Respawns[j].Location.X}" +
                            $",{_selectedMapInfos[i].Respawns[j].Location.Y}" +
                            $",{mob.Name}" +
                            $",{_selectedMapInfos[i].Respawns[j].Spread}" +
                            $",{_selectedMapInfos[i].Respawns[j].Count}" +
                            $",{_selectedMapInfos[i].Respawns[j].Delay}" +
                            $",{_selectedMapInfos[i].Respawns[j].Direction}" +
                            $",{_selectedMapInfos[i].Respawns[j].RoutePath}";

                        sw.WriteLine(Output);
                    }
                }
            }
            MessageBox.Show("MonGen Export complete");
        }

        private void VisualizerButton_Click(object sender, EventArgs e)
        {
            if (_selectedMapInfos.Count != 1)
                return;

            MirForms.VisualMapInfo.VForm VForm = new MirForms.VisualMapInfo.VForm();
            MirForms.VisualMapInfo.Class.VisualizerGlobal.MapInfo = _selectedMapInfos[0];
            VForm.FormClosed += VForm_Disposed;

            VForm.ShowDialog();

            _selectedMapInfos[0] = MirForms.VisualMapInfo.Class.VisualizerGlobal.MapInfo;
            UpdateMineZoneInterface();
            UpdateRespawnInterface();
        }

        private void VForm_Disposed(object sender, EventArgs e)
        {
            RefreshRespawnList();
            RefreshMineZoneList();
        }

        private void MineComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].MineIndex = Convert.ToByte(MineComboBox.SelectedIndex);
        }

        private void MineZoneComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMineZones.Count; i++)
                _selectedMineZones[i].Mine = Convert.ToByte(MineZoneComboBox.SelectedIndex);

            RefreshMineZoneList();
        }

        private void NoMountCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoMount = NoMountCheckbox.Checked;
        }

        private void NeedBridleCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NeedBridle = NeedBridleCheckbox.Checked;
        }

        private void NoFightCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoFight = NoFightCheckbox.Checked;
        }

        private void MusicTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return; ushort temp;
            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }

            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].Music = temp;
        }

        private void RandomTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ushort temp;

            if (!ushort.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;


            for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                _selectedRespawnInfos[i].RandomDelay = temp;

            RefreshRespawnList();
        }

        private void chkRespawnEnableTick_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (chkRespawnEnableTick.Checked)
            {
                chkrespawnsave.Enabled = true;
                for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                {
                    _selectedRespawnInfos[i].RespawnTicks = _selectedRespawnInfos[i].Delay;
                    _selectedRespawnInfos[i].Delay = 0;
                }
            }
            else
            {
                chkrespawnsave.Enabled = false;
                for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                {
                    _selectedRespawnInfos[i].Delay = _selectedRespawnInfos[i].RespawnTicks;
                    _selectedRespawnInfos[i].RespawnTicks = 0;
                }
            }
            RefreshRespawnList();

        }

        private void chkrespawnsave_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            if (chkRespawnEnableTick.Checked)
            {
                for (int i = 0; i < _selectedRespawnInfos.Count; i++)
                {
                    _selectedRespawnInfos[i].SaveRespawnTime = chkrespawnsave.Checked;
                }
            }
            RefreshRespawnList();
        }

        private void ConquestComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            ComboBox cmb = sender as ComboBox;

            for (int i = 0; i < _selectedMovementInfos.Count; i++)
            {
                var conquestIndex = 0;

                if (cmb.SelectedIndex >= 0)
                {
                    ConquestInfo info = Envir.ConquestInfoList.FirstOrDefault(x => x.Name == ConquestComboBox.SelectedItem.ToString());

                    if (info != null)
                    {
                        conquestIndex = info.Index;
                    }

                    MovementInfo thisMovement = _info.Movements.FirstOrDefault(x => x.MapIndex == _selectedMovementInfos[i].MapIndex &&
                                        x.Source == _selectedMovementInfos[i].Source &&
                                        x.Destination == _selectedMovementInfos[i].Destination);

                    if (thisMovement != null)
                    {
                        thisMovement.ConquestIndex = conquestIndex;
                    }
                }
            }
                
            RefreshMovementList();
        }

        private void NoTownTeleportCheckbox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoTownTeleport = NoTownTeleportCheckbox.Checked;
        }

        private void NoReincarnation_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;
            for (int i = 0; i < _selectedMapInfos.Count; i++)
                _selectedMapInfos[i].NoReincarnation = NoReincarnation.Checked;
        }

        private void ShowBigMapCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            for (int i = 0; i < _selectedMovementInfos.Count; i++)
                _selectedMovementInfos[i].ShowOnBigMap = ShowBigMapCheckBox.Checked;
        }

        private void BigMapIconTextBox_TextChanged(object sender, EventArgs e)
        {
            if (ActiveControl != sender) return;

            int temp;

            if (!int.TryParse(ActiveControl.Text, out temp))
            {
                ActiveControl.BackColor = Color.Red;
                return;
            }
            ActiveControl.BackColor = SystemColors.Window;

            for (int i = 0; i < _selectedMovementInfos.Count; i++)
                _selectedMovementInfos[i].Icon = temp;

            RefreshMovementList();
        }
    }
}
