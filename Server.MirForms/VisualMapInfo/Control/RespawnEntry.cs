using System;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualBasic.PowerPacks;
using Server.MirForms.VisualMapInfo.Class;
using Server.MirEnvir;
using Server.MirDatabase;
using System.Collections.Generic;

namespace Server.MirForms.VisualMapInfo.Control
{
    public partial class RespawnEntry : UserControl
    {
        public List<int> IndexList = new List<int>();

        public Envir Envir
        {
            get { return SMain.EditEnvir; }
        }

        public RectangleShape RegionHighlight = new RectangleShape();

        public int MonsterIndex;

        public bool RegionHidden = false;

        Point MouseDownLocation;

        public int X, Y;

        public byte Direction;

        public ushort RandomDelay;

        public ushort tempRange;
        public ushort Range
        {
            get
            {
                return (ushort)(tempRange);
            }
            set
            {
                if (value > 0)
                    tempRange = (ushort)(value);
                else
                    tempRange = 1;

                RegionHighlight.Size = new Size(
                    (tempRange * 2) * VisualizerGlobal.ZoomLevel,
                    (tempRange * 2) * VisualizerGlobal.ZoomLevel);

                RegionHighlight.Left = (X - value) * VisualizerGlobal.ZoomLevel;
                RegionHighlight.Top = (Y - value) * VisualizerGlobal.ZoomLevel;


                Details.Text = string.Format("C               D            X: {0} | Y: {1} | Range: {2}", X.ToString(), Y.ToString(), Range.ToString());
            }
        }

        public string RoutePath = string.Empty;

        public RespawnEntry()
        {
            InitializeComponent();
            InitializeRegionHighlight();
        }

        private void InitializeRegionHighlight()
        {
            RegionHighlight.Visible = false;
            RegionHighlight.BorderColor = Color.Lime;
            RegionHighlight.FillColor = Color.FromArgb(30, 200, 0, 200);

            RegionHighlight.BorderWidth = 2;
            RegionHighlight.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Region_MouseMove);
            RegionHighlight.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Region_MouseDown);
            RegionHighlight.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
            RegionHighlight.MouseLeave += new System.EventHandler(this.Region_MouseLeave);
            RegionHighlight.MouseClick += new System.Windows.Forms.MouseEventHandler(RegionHighlight_MouseClick);
            RegionHighlight.MouseHover += RegionHighlight_MouseHover;
        }

        #region "Start RegionHighlight Methods"

        private void Region_MouseLeave(object sender, EventArgs e)
        {
            if (RegionHighlight.Visible)
                this.BackColor = Color.White;
            else
                this.BackColor = Color.Silver;

            RegionHighlight.BorderColor = Color.Lime;
            RegionHighlight.FillStyle = FillStyle.Transparent;
        }

        private void Region_MouseEnter(object sender, EventArgs e)
        {
            if (!RegionHighlight.Visible) return;

            this.BackColor = Color.MediumOrchid;

            RegionHighlight.BorderColor = Color.MediumOrchid;
            RegionHighlight.FillStyle = FillStyle.Solid;
            RegionHighlight.Cursor = VisualizerGlobal.Cursor;
        }

        private void Region_MouseDown(object sender, MouseEventArgs e)
        {
            RegionHighlight.BringToFront();

            this.BackColor = Color.Orange;
            RegionHighlight.BorderColor = Color.Orange;

            if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Select) return;

            MouseDownLocation = e.Location;
        }

        private void Region_MouseMove(object sender, MouseEventArgs e)
        {
            if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Select) return;
            if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Add) return;

            if (e.Button == System.Windows.Forms.MouseButtons.Left)
                if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Move)
                {
                    RegionHighlight.Left = e.X + RegionHighlight.Left - MouseDownLocation.X;
                    RegionHighlight.Top = e.Y + RegionHighlight.Top - MouseDownLocation.Y;

                    X = (RegionHighlight.Left + Range) / VisualizerGlobal.ZoomLevel;
                    Y = (RegionHighlight.Top + Range) / VisualizerGlobal.ZoomLevel;

                    Range = tempRange;
                }
                else if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Resize)
                    Range += (ushort)((MouseDownLocation.X - e.Location.X) / VisualizerGlobal.ZoomLevel);

            if (e.Button == System.Windows.Forms.MouseButtons.Right)
                if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Move)
                    Range += (ushort)((MouseDownLocation.X - e.Location.X) / VisualizerGlobal.ZoomLevel);
                else
                    return;
        }

        private void RegionHighlight_MouseClick(object sender, MouseEventArgs e)
        {
            if (VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Focus)
            {
                VisualizerGlobal.SelectedFocusType = VisualizerGlobal.FocusType.Respawn;
                VisualizerGlobal.FocusRespawnEntry = this;
                VisualizerGlobal.FocusModeActive = true;
            }

            MonsterComboBox.Focus();
        }

        private void RegionHighlight_MouseHover(object sender, EventArgs e)
        {
            RegionHighlight.Cursor = VisualizerGlobal.Cursor;
        }

        #endregion "End RegionHighlight Methods"

        public void UpdateForFocus()
        {
            RegionHighlight.Left = X * VisualizerGlobal.ZoomLevel;
            RegionHighlight.Top = Y * VisualizerGlobal.ZoomLevel;
            Range = tempRange;
        }

        public void RemoveEntry()
        {
            RegionHighlight.Dispose();
            this.Dispose();
        }

        public void HideControl()
        {
            RegionHighlight.Visible = false;
        }

        public void ShowControl()
        {
            RegionHighlight.Visible = true;
        }

        public void HideRegion()
        {
            RegionHighlight.Visible = false;
            this.BackColor = Color.Silver;
            RegionHidden = true;
        }

        public void ShowRegion()
        {
            RegionHighlight.Visible = true;
            this.BackColor = Color.White;
            RegionHidden = false;
        }

        private void RespawnEntry_Load(object sender, EventArgs e)
        {

            for (int i = 0; i < Envir.MonsterInfoList.Count; i++) MonsterComboBox.Items.Add(Envir.MonsterInfoList[i]);

            MonsterComboBox.SelectedIndex = MonsterIndex-1; //-1

            Details.Text = string.Format("C               D            X: {0} | Y: {1} | Range: {2}", X.ToString(), Y.ToString(), Range.ToString());
        }

        private void MonsterComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            MonsterInfo info = MonsterComboBox.SelectedItem as MonsterInfo;
            if (info == null) return;
            MonsterIndex = info.Index;
        }

        private void Details_DoubleClick(object sender, EventArgs e)
        {
            Forms.RespawnsDetailForm RespawnDetail = new Forms.RespawnsDetailForm();

            RespawnDetail.X.Text = X.ToString();
            RespawnDetail.Y.Text = Y.ToString();
            RespawnDetail.Spread.Text = Range.ToString();
            RespawnDetail.Count.Text = Count.Text;
            RespawnDetail.Delay.Text = Delay.Text;
            RespawnDetail.RoutePath.Text = RoutePath;
            RespawnDetail.Direction.Text = Direction.ToString();
            RespawnDetail.RDelay.Text = RandomDelay.ToString();

            RespawnDetail.ShowDialog();

            X = Convert.ToInt16(RespawnDetail.X.Text);
            Y = Convert.ToInt16(RespawnDetail.Y.Text);
            Range = Convert.ToUInt16(RespawnDetail.Spread.Text);
            Count.Text = RespawnDetail.Count.Text;
            Delay.Text = RespawnDetail.Delay.Text;
            RoutePath = RespawnDetail.RoutePath.Text;
            Direction = byte.Parse(RespawnDetail.Direction.Text);
            RandomDelay = ushort.Parse(RespawnDetail.RDelay.Text);

            RespawnDetail.Dispose();
        }
    }
}