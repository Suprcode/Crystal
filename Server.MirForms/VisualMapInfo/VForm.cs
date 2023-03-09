using Server.MirForms.VisualMapInfo.Class;
using Server.MirForms.VisualMapInfo.Control;
using Microsoft.VisualBasic.PowerPacks;
using Server.MirEnvir;
using Server.MirDatabase;

namespace Server.MirForms.VisualMapInfo
{
    public partial class VForm : Form
    {
        ShapeContainer Canvas = new ShapeContainer();

        public Envir Envir => SMain.EditEnvir;

        public Point MouseDownLocation;

        public VForm()
        {
            InitializeComponent(); 
        }

        private void VForm_Load(object sender, EventArgs e)
        {
            InitializeMap();
            InitializeMineInfo();
            InitializeRespawnInfo();
            VisualizerGlobal.FocusModeActivated += FocusModeActivated;
        }

        private void VForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            VisualizerGlobal.ZoomLevel = 1;
            VisualizerGlobal.MapInfo.Respawns.Clear();
            VisualizerGlobal.MapInfo.MineZones.Clear();

            for (int i = 0; i < RespawnPanel.Controls.Count; i++)
            {
                try
                {
                    RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                    MirDatabase.RespawnInfo NewRespawnZone = new MirDatabase.RespawnInfo();

                    NewRespawnZone.Location = new Point(RespawnControl.X, RespawnControl.Y);
                    NewRespawnZone.MonsterIndex = RespawnControl.MonsterIndex;
                    NewRespawnZone.Spread = RespawnControl.Range;
                    NewRespawnZone.Count = Convert.ToUInt16(RespawnControl.Count.Text);
                    NewRespawnZone.Delay = Convert.ToUInt16(RespawnControl.Delay.Text);
                    NewRespawnZone.RoutePath = RespawnControl.RoutePath;
                    NewRespawnZone.Direction = RespawnControl.Direction;
                    NewRespawnZone.RandomDelay = RespawnControl.RandomDelay;

                    VisualizerGlobal.MapInfo.Respawns.Add(NewRespawnZone);
                }
                catch (Exception) { continue; }
            }

            for (int i = 0; i < MiningPanel.Controls.Count; i++)
            {
                try
                {
                    MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                    MineZone NewMineZone = new MineZone();

                    NewMineZone.Location = new Point(MineControl.X, MineControl.Y);
                    NewMineZone.Mine = MineControl.MineIndex;
                    NewMineZone.Size = MineControl.Range;

                    VisualizerGlobal.MapInfo.MineZones.Add(NewMineZone);
                }
                catch (Exception) { continue; }
            }
        }

        private void InitializeMap()
        {
            ReadMap readMap = new ReadMap();

            readMap.mapFile = VisualizerGlobal.MapInfo.FileName;
            readMap.Load();

            MapImage.Image = VisualizerGlobal.ClippingMap;

            Canvas.Parent = MapImage;
            Canvas.BringToFront();

            MapDetailsLabel.Text =
                $"Map Name: {VisualizerGlobal.MapInfo.Title}   Width: {VisualizerGlobal.ClippingMap.Width}   Height: {VisualizerGlobal.ClippingMap.Height}";
        }

        private void InitializeMineInfo()
        {
            List<string> miningFilterItems = new() { { "Disabled" } };
            Settings.MineSetList.ForEach(x => miningFilterItems.Add(x.Name));
            miningFilterItems.Add("No Filter");

            MiningFilter.DataSource = miningFilterItems;
            MiningFilter.Text = "No Filter";

            for (int i = 0; i < VisualizerGlobal.MapInfo.MineZones.Count; i++)
            {
                MineEntry MineRegion = new MineEntry();
                MineRegion.Dock = DockStyle.Top;
                MineRegion.MineIndex = VisualizerGlobal.MapInfo.MineZones[i].Mine;
                MineRegion.X = VisualizerGlobal.MapInfo.MineZones[i].Location.X;
                MineRegion.Y = VisualizerGlobal.MapInfo.MineZones[i].Location.Y;
                MineRegion.tempRange = VisualizerGlobal.MapInfo.MineZones[i].Size;
                MineRegion.Range = VisualizerGlobal.MapInfo.MineZones[i].Size;
                MineRegion.ShowControl();

                MiningPanel.Controls.Add(MineRegion);

                MineRegion.RegionHighlight.Parent = Canvas;
            }            
        }

        private void InitializeRespawnInfo()
        {
            for (int i = 0; i < Envir.MonsterInfoList.Count; i++)
                RespawnsFilter.Items.Add(Envir.MonsterInfoList[i]);

            RespawnsFilter.Items.Add("No Filter");
            RespawnsFilter.Text = "No Filter";

            for (int i = 0; i < VisualizerGlobal.MapInfo.Respawns.Count; i++)
            {
                RespawnEntry RespawnRegion = new RespawnEntry();
                RespawnRegion.Dock = DockStyle.Top;
                RespawnRegion.MonsterIndex = VisualizerGlobal.MapInfo.Respawns[i].MonsterIndex;
                RespawnRegion.X = VisualizerGlobal.MapInfo.Respawns[i].Location.X;
                RespawnRegion.Y = VisualizerGlobal.MapInfo.Respawns[i].Location.Y;
                RespawnRegion.Range = VisualizerGlobal.MapInfo.Respawns[i].Spread;
                RespawnRegion.Count.Text = VisualizerGlobal.MapInfo.Respawns[i].Count.ToString();
                RespawnRegion.Delay.Text = VisualizerGlobal.MapInfo.Respawns[i].Delay.ToString();
                RespawnRegion.RoutePath = VisualizerGlobal.MapInfo.Respawns[i].RoutePath;
                RespawnRegion.Direction = VisualizerGlobal.MapInfo.Respawns[i].Direction;
                RespawnRegion.RandomDelay = VisualizerGlobal.MapInfo.Respawns[i].RandomDelay;
                RespawnRegion.HideControl();

                RespawnPanel.Controls.Add(RespawnRegion);

                RespawnRegion.RegionHighlight.Parent = Canvas;
            }
        }

        private void RedrawMap()
        {
            Bitmap Map = new Bitmap(
                VisualizerGlobal.ClippingMap.Width * VisualizerGlobal.ZoomLevel,
                VisualizerGlobal.ClippingMap.Height * VisualizerGlobal.ZoomLevel);

            using (Graphics g = Graphics.FromImage(Map))
            {
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(VisualizerGlobal.ClippingMap, 0, 0, Map.Width, Map.Height);
            }

            MapImage.Image = Map;

            if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Mining)
                VisualizerGlobal.FocusMineEntry.UpdateForFocus(); 
            if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Respawn)
                VisualizerGlobal.FocusRespawnEntry.UpdateForFocus();
        }

        private void FocusModeActivated(object sender, EventArgs e)
        {
            for (int i = MiningPanel.Controls.Count - 1; i > -1; i--)
                try
                {
                    MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];

                    MineControl.Visible = false;
                    MineControl.RegionHighlight.Visible = false;
                }
                catch (Exception)
                {
                    continue;
                }

            for (int i = RespawnPanel.Controls.Count - 1; i > -1; i--)
                try
                {
                    RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];

                    RespawnControl.Visible = false;
                    RespawnControl.RegionHighlight.Visible = false;
                }
                catch (Exception)
                {
                    continue;
                }

            if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Mining)
            {
                VisualizerGlobal.FocusMineEntry.Visible = true;
                VisualizerGlobal.FocusMineEntry.RegionHighlight.Visible = true;
                VisualizerGlobal.FocusMineEntry.UpdateForFocus();
            }
            if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Respawn)
            {
                VisualizerGlobal.FocusRespawnEntry.Visible = true;
                VisualizerGlobal.FocusRespawnEntry.RegionHighlight.Visible = true;
                VisualizerGlobal.FocusRespawnEntry.UpdateForFocus();
            }

            EndFocus.Visible = true;
            FocusBreak.Visible = true;

            ToolSelectedChanged(MoveButton, null);
        }

        private void ToolSelectedChanged(object sender, EventArgs e)
        {
            MapImage.Cursor = Cursors.Arrow;

            ToolStripButton[] ToolButtons = new ToolStripButton[] { SelectButton, AddButton, MoveButton, ResizeButton };

            foreach (var Tool in ToolButtons)
                Tool.Checked = false;

            ToolStripButton ToolSender = (ToolStripButton)sender;
            ToolSender.Checked = true;

            switch (ToolSender.Text)
            {
                case "Select Region":
                    VisualizerGlobal.SelectedTool = VisualizerGlobal.Tool.Select;
                    VisualizerGlobal.Cursor = Cursors.Arrow;
                    break;
                case "Add Region":
                    VisualizerGlobal.SelectedTool = VisualizerGlobal.Tool.Add;
                    VisualizerGlobal.Cursor = Cursors.UpArrow;
                    break;
                case "Move Region":
                    VisualizerGlobal.SelectedTool = VisualizerGlobal.Tool.Move;
                    VisualizerGlobal.Cursor = Cursors.SizeAll;
                    break;
                case "Resize Region":
                    VisualizerGlobal.SelectedTool = VisualizerGlobal.Tool.Resize;
                    VisualizerGlobal.Cursor = Cursors.SizeWE;
                    break;
                default:
                    break;
            }
        }
        
        private void EndFocus_Click(object sender, EventArgs e)
        {
            EndFocus.Visible = false;
            FocusBreak.Visible = false;

            MiningFilter.Enabled = true;
            MiningRemoveSelected.Enabled = true;

            RespawnsFilter.Enabled = true;
            RespawnsRemoveSelected.Enabled = true;

            VisualizerGlobal.ZoomLevel = 1;
            RedrawMap();

            if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Mining)
                MiningFilter_SelectedIndexChanged(MiningFilter, null);
            if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Respawn)
                RespawnsFilter_SelectedIndexChanged(RespawnsFilter, null);

            VisualizerGlobal.FocusMineEntry = null;
            VisualizerGlobal.FocusRespawnEntry = null;
            VisualizerGlobal.SelectedFocusType = VisualizerGlobal.FocusType.None;
        }

        private void MapImage_Click(object sender, EventArgs e)
        {
            if (RegionTabs.SelectedTab.Text == "Mining")
                if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Add)
                {
                    MineEntry MineControl = new MineEntry()
                    {
                        Dock = DockStyle.Top,
                        X = MouseDownLocation.X,
                        Y = MouseDownLocation.Y,
                        Range = 50
                    };

                    MineControl.ShowControl();
                    MineControl.RegionHighlight.Parent = Canvas;

                    MiningPanel.Controls.Add(MineControl);

                    ToolSelectedChanged(MoveButton, e);
                }

            if (RegionTabs.SelectedTab.Text == "Respawns")
                if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Add)
                {
                    RespawnEntry RespawnControl = new RespawnEntry()
                    {
                        Dock = DockStyle.Top,
                        X = MouseDownLocation.X,
                        Y = MouseDownLocation.Y,
                        Range = 50
                    };

                    RespawnControl.ShowControl();
                    RespawnControl.RegionHighlight.Parent = Canvas;

                    RespawnPanel.Controls.Add(RespawnControl);

                    ToolSelectedChanged(MoveButton, e);
                }
        }

        private void RegionTabs_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (RegionTabs.SelectedTab.Text == "Mining")
            {
                for (int i = RespawnPanel.Controls.Count; i > -1; --i)
                    try
                    {
                        RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                        RespawnControl.HideControl();
                    }
                    catch (Exception) { continue; }
                
                MiningFilter_SelectedIndexChanged(MiningFilter, null);
            }
            else if (RegionTabs.SelectedTab.Text == "Respawns")
            {
                for (int i = MiningPanel.Controls.Count; i > -1; --i)
                    try
                    {
                        MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                        MineControl.HideControl();
                    }
                    catch (Exception) { continue; }

                RespawnsFilter_SelectedIndexChanged(RespawnsFilter, null);
            }
        }

        private void MapImage_MouseDown(object sender, MouseEventArgs e)
        {
            if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Select) return;

            MouseDownLocation = e.Location;
        } 

        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams cp = base.CreateParams;
                cp.ExStyle |= 0x02000000;  // Turn on WS_EX_COMPOSITED
                return cp;
            }
        }

        // Quick Keys
        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (keyData == Keys.M)
            {
                ToolSelectedChanged(MoveButton, new EventArgs());

                return true;
            }

            if (keyData == Keys.S)
            {
                ToolSelectedChanged(SelectButton, new EventArgs());

                return true;
            }

            if (keyData == Keys.R)
            {
                ToolSelectedChanged(ResizeButton, new EventArgs());

                return true;
            }

            if (keyData == Keys.A)
            {
                ToolSelectedChanged(AddButton, new EventArgs());

                return true;
            }

            if (keyData == Keys.Add && VisualizerGlobal.FocusModeActive == true)
            {
                if (VisualizerGlobal.ZoomLevel != 6)
                    VisualizerGlobal.ZoomLevel++;

                RedrawMap();

                return true;
            }

            if (keyData == Keys.Subtract && VisualizerGlobal.FocusModeActive == true)
            {
                if (VisualizerGlobal.ZoomLevel != 1)
                    VisualizerGlobal.ZoomLevel--;

                RedrawMap();

                return true;
            }

            if (keyData == Keys.Left && VisualizerGlobal.FocusModeActive == true)
            {
                if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Mining)
                {
                    VisualizerGlobal.FocusMineEntry.X--;
                    VisualizerGlobal.FocusMineEntry.Range = VisualizerGlobal.FocusMineEntry.tempRange;
                }
                else if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Respawn)
                {
                    VisualizerGlobal.FocusRespawnEntry.X--;
                    VisualizerGlobal.FocusRespawnEntry.Range = VisualizerGlobal.FocusRespawnEntry.tempRange;
                }

                return true;
            } 
            
            if (keyData == Keys.Right && VisualizerGlobal.FocusModeActive == true)
            {
                if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Mining)
                {
                    VisualizerGlobal.FocusMineEntry.X++;
                    VisualizerGlobal.FocusMineEntry.Range = VisualizerGlobal.FocusMineEntry.tempRange;
                }
                else if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Respawn)
                {
                    VisualizerGlobal.FocusRespawnEntry.X++;
                    VisualizerGlobal.FocusRespawnEntry.Range = VisualizerGlobal.FocusRespawnEntry.tempRange;
                }

                return true;
            }

            if (keyData == Keys.Up && VisualizerGlobal.FocusModeActive == true)
            {
                if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Mining)
                {
                    VisualizerGlobal.FocusMineEntry.Y--;
                    VisualizerGlobal.FocusMineEntry.Range = VisualizerGlobal.FocusMineEntry.tempRange;
                }
                else if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Respawn)
                {
                    VisualizerGlobal.FocusRespawnEntry.Y--;
                    VisualizerGlobal.FocusRespawnEntry.Range = VisualizerGlobal.FocusRespawnEntry.tempRange;
                }

                return true;
            }

            if (keyData == Keys.Down && VisualizerGlobal.FocusModeActive == true)
            {
                if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Mining)
                {
                    VisualizerGlobal.FocusMineEntry.Y++;
                    VisualizerGlobal.FocusMineEntry.Range = VisualizerGlobal.FocusMineEntry.tempRange;
                }
                else if (VisualizerGlobal.SelectedFocusType == VisualizerGlobal.FocusType.Respawn)
                {
                    VisualizerGlobal.FocusRespawnEntry.Y++;
                    VisualizerGlobal.FocusRespawnEntry.Range = VisualizerGlobal.FocusRespawnEntry.tempRange;
                }

                return true;
            }

            if (keyData == Keys.Escape && VisualizerGlobal.FocusModeActive == true)
            {
                EndFocus_Click(EndFocus, null);

                return true;
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        #region "START Mining Tool Bar"

        private void MiningSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = MiningPanel.Controls.Count - 1; i > -1; i--)
            {
                MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                MineControl.Selected.Checked = true;
            }
        }

        private void MiningSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = MiningPanel.Controls.Count - 1; i > -1; i--)
            {
                MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                MineControl.Selected.Checked = false;
            }
        }

        private void MiningInvertSelection_Click(object sender, EventArgs e)
        {
            for (int i = MiningPanel.Controls.Count - 1; i > -1; i--)
            {
                MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                MineControl.Selected.Checked = !MineControl.Selected.Checked;
            }
        }

        private void MiningRemoveSelected_Click(object sender, EventArgs e)
        {
            if (MiningPanel.Controls.Count == 0) return;

            DialogResult result = MessageBox.Show("Remove selected records?", "", MessageBoxButtons.YesNoCancel);
            if (result != DialogResult.Yes) return;

            for (int i = MiningPanel.Controls.Count; i > -1; --i)
            {
                try
                {
                    MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                    if (MineControl.Selected.Checked == true)
                        MineControl.RemoveEntry();
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void MiningHideRegion_Click(object sender, EventArgs e)
        {
            for (int i = MiningPanel.Controls.Count - 1; i > -1; i--)
            {
                try
                {
                    MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                    if (MineControl.Selected.Checked == true)
                        MineControl.HideRegion();
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void MiningShowRegion_Click(object sender, EventArgs e)
        {
            for (int i = MiningPanel.Controls.Count - 1; i > -1; i--)
            {
                try
                {
                    MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];
                    if (MineControl.Selected.Checked == true)
                        MineControl.ShowRegion();
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void MiningFocusRegion_Click(object sender, EventArgs e)
        {
            VisualizerGlobal.SelectedTool = VisualizerGlobal.Tool.Focus;
            VisualizerGlobal.Cursor = Cursors.Hand;

            MiningFilter.Enabled = false;
            MiningRemoveSelected.Enabled = false;
        }

        private void MiningFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            VisualizerGlobal.ZoomLevel = 1;

            if (MiningFilter.Text == "No Filter")
                for (int i = MiningPanel.Controls.Count - 1; i > -1; i--)
                    try
                    {
                        MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];

                        MineControl.Visible = true;
                        if (!MineControl.RegionHidden)
                            MineControl.RegionHighlight.Visible = true;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
            else
                for (int i = MiningPanel.Controls.Count - 1; i > -1; i--)
                    try
                    {
                        MineEntry MineControl = (MineEntry)MiningPanel.Controls[i];

                        if (MineControl.MineIndex == MiningFilter.SelectedIndex)
                        {
                            MineControl.Visible = true;

                            if (!MineControl.RegionHidden)
                                MineControl.RegionHighlight.Visible = true;
                        }
                        else
                        {
                            MineControl.RegionHighlight.Visible = false;
                            MineControl.Visible = false;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
        }

        #endregion "END Mining Tool Bar"

        #region "START Respawn Tool Bar

        private void RespawnsSelectAll_Click(object sender, EventArgs e)
        {
            for (int i = RespawnPanel.Controls.Count - 1; i > -1; i--)
            {
                RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                RespawnControl.Selected.Checked = true;
            }
        }

        private void RespawnsSelectNone_Click(object sender, EventArgs e)
        {
            for (int i = RespawnPanel.Controls.Count - 1; i > -1; i--)
            {
                RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                RespawnControl.Selected.Checked = false;
            }
        }

        private void RespawnsRemoveSelected_Click(object sender, EventArgs e)
        {
            if (RespawnPanel.Controls.Count == 0) return;

            DialogResult result = MessageBox.Show("Remove selected records?", "", MessageBoxButtons.YesNoCancel);
            if (result != DialogResult.Yes) return;

            for (int i = RespawnPanel.Controls.Count; i > -1; --i)
            {
                try
                {
                    RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                    if (RespawnControl.Selected.Checked == true)
                    {
                        RespawnControl.RemoveEntry();
                    }
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void ResapwnsHideRegion_Click(object sender, EventArgs e)
        {
            for (int i = RespawnPanel.Controls.Count - 1; i > -1; i--)
            {
                try
                {
                    RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                    if (RespawnControl.Selected.Checked == true)
                        RespawnControl.HideRegion();
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void ResapwnsShowRegion_Click(object sender, EventArgs e)
        {
            for (int i = RespawnPanel.Controls.Count - 1; i > -1; i--)
            {
                try
                {
                    RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                    if (RespawnControl.Selected.Checked == true)
                        RespawnControl.ShowRegion();
                }
                catch (Exception)
                {
                    continue;
                }
            }
        }

        private void ResapwnsFocusRegion_Click(object sender, EventArgs e)
        {
            VisualizerGlobal.SelectedTool = VisualizerGlobal.Tool.Focus;
            VisualizerGlobal.Cursor = Cursors.Hand;

            RespawnsFilter.Enabled = false;
            RespawnsRemoveSelected.Enabled = false;
        }
        
        private void RespawnsInvertSelection_Click(object sender, EventArgs e)
        {
            for (int i = RespawnPanel.Controls.Count - 1; i > -1; i--)
            {
                RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];
                RespawnControl.Selected.Checked = !RespawnControl.Selected.Checked;
            }
        }

        private void RespawnsFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            MonsterInfo info = RespawnsFilter.SelectedItem as MonsterInfo;

            VisualizerGlobal.ZoomLevel = 1;

            if (RespawnsFilter.Text == "No Filter")
                for (int i = RespawnPanel.Controls.Count - 1; i > -1; i--)
                    try
                    {
                        RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];

                        RespawnControl.Visible = true;
                        if (!RespawnControl.RegionHidden)
                            RespawnControl.RegionHighlight.Visible = true;
                    }
                    catch (Exception)
                    {
                        continue;
                    }
            else
                for (int i = RespawnPanel.Controls.Count - 1; i > -1; i--)
                    try
                    {
                        RespawnEntry RespawnControl = (RespawnEntry)RespawnPanel.Controls[i];

                        if (RespawnControl.MonsterIndex == info.Index)
                        {
                            RespawnControl.Visible = true;

                            if (!RespawnControl.RegionHidden)
                                RespawnControl.RegionHighlight.Visible = true;
                        }
                        else
                        {
                            RespawnControl.RegionHighlight.Visible = false;
                            RespawnControl.Visible = false;
                        }
                    }
                    catch (Exception)
                    {
                        continue;
                    }
        }

        #endregion "END Respawn Tool Bar

        private void RegionTabs_Selecting(object sender, TabControlCancelEventArgs e)
        {
            if (VisualizerGlobal.SelectedFocusType != VisualizerGlobal.FocusType.None)
                e.Cancel = true;
        }



        

        

    }
}