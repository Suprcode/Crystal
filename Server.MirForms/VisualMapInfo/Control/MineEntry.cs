using Microsoft.VisualBasic.PowerPacks;
using Server.Library;
using Server.VisualMapInfo.Class;
using Server.VisualMapInfo.Control.Forms;

namespace Server.VisualMapInfo.Control {
    public partial class MineEntry : UserControl {
        public RectangleShape RegionHighlight = new();

        public byte MineIndex;
        public bool RegionHidden = false;

        private Point MouseDownLocation;

        public int X, Y;

        public ushort tempRange;

        public ushort Range {
            get => (ushort)tempRange;
            set {
                tempRange = 0;

                if(value > 0) {
                    tempRange = (ushort)value;
                }

                RegionHighlight.Size = new Size(
                    tempRange * 2 * VisualizerGlobal.ZoomLevel,
                    tempRange * 2 * VisualizerGlobal.ZoomLevel);

                RegionHighlight.Left = (X - value) * VisualizerGlobal.ZoomLevel;
                RegionHighlight.Top = (Y - value) * VisualizerGlobal.ZoomLevel;

                Details.Text = $"X: {X.ToString()} | Y: {Y.ToString()} | Range: {Range.ToString()}";
            }
        }

        protected override CreateParams CreateParams {
            get {
                CreateParams parms = base.CreateParams;
                parms.Style &= ~0x02000000; // Turn off WS_CLIPCHILDREN
                return parms;
            }
        }

        public MineEntry() {
            InitializeComponent();
            InitializeRegionHighlight();
        }

        private void InitializeRegionHighlight() {
            RegionHighlight.Visible = false;
            RegionHighlight.BorderColor = Color.Lime;
            RegionHighlight.FillColor = Color.FromArgb(30, 200, 0, 200);

            RegionHighlight.BorderWidth = 2;
            RegionHighlight.MouseMove += new MouseEventHandler(Region_MouseMove);
            RegionHighlight.MouseDown += new MouseEventHandler(Region_MouseDown);
            RegionHighlight.MouseEnter += new EventHandler(Region_MouseEnter);
            RegionHighlight.MouseLeave += new EventHandler(Region_MouseLeave);
            RegionHighlight.MouseClick += new MouseEventHandler(RegionHighlight_MouseClick);
            RegionHighlight.MouseHover += RegionHighlight_MouseHover;
        }


        #region "Start RegionHighlight Methods"

        private void Region_MouseLeave(object sender, EventArgs e) {
            if(RegionHighlight.Visible) {
                BackColor = Color.White;
            } else {
                BackColor = Color.Silver;
            }

            RegionHighlight.BorderColor = Color.Lime;
            RegionHighlight.FillStyle = FillStyle.Transparent;
        }

        private void Region_MouseEnter(object sender, EventArgs e) {
            if(!RegionHighlight.Visible) {
                return;
            }

            BackColor = Color.MediumOrchid;

            RegionHighlight.BorderColor = Color.MediumOrchid;
            RegionHighlight.FillStyle = FillStyle.Solid;
            RegionHighlight.Cursor = VisualizerGlobal.Cursor;
        }

        private void Region_MouseDown(object sender, MouseEventArgs e) {
            RegionHighlight.BringToFront();

            BackColor = Color.Orange;
            RegionHighlight.BorderColor = Color.Orange;

            if(VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Select) {
                return;
            }

            MouseDownLocation = e.Location;
        }

        private void Region_MouseMove(object sender, MouseEventArgs e) {
            if(VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Select) {
                return;
            }

            if(VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Add) {
                return;
            }

            if(e.Button == MouseButtons.Left) {
                if(VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Move) {
                    RegionHighlight.Left = e.X + RegionHighlight.Left - MouseDownLocation.X;
                    RegionHighlight.Top = e.Y + RegionHighlight.Top - MouseDownLocation.Y;

                    X = (RegionHighlight.Left + Range) / VisualizerGlobal.ZoomLevel;
                    Y = (RegionHighlight.Top + Range) / VisualizerGlobal.ZoomLevel;

                    Range = tempRange;
                } else if(VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Resize) {
                    Range += (ushort)((MouseDownLocation.X - e.Location.X) / VisualizerGlobal.ZoomLevel);
                }
            }

            if(e.Button == MouseButtons.Right) {
                if(VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Move) {
                    Range += (ushort)((MouseDownLocation.X - e.Location.X) / VisualizerGlobal.ZoomLevel);
                } else {
                    return;
                }
            }
        }

        private void RegionHighlight_MouseClick(object sender, MouseEventArgs e) {
            if(VisualizerGlobal.SelectedTool == VisualizerGlobal.Tool.Focus) {
                VisualizerGlobal.SelectedFocusType = VisualizerGlobal.FocusType.Mining;
                VisualizerGlobal.FocusMineEntry = this;
                VisualizerGlobal.FocusModeActive = true;
            }

            MineComboBox.Focus();
        }

        private void RegionHighlight_MouseHover(object sender, EventArgs e) {
            RegionHighlight.Cursor = VisualizerGlobal.Cursor;
        }

        #endregion "End RegionHighlight Methods"

        public void UpdateForFocus() {
            RegionHighlight.Left = X * VisualizerGlobal.ZoomLevel;
            RegionHighlight.Top = Y * VisualizerGlobal.ZoomLevel;
            Range = tempRange;
        }

        public void RemoveEntry() {
            RegionHighlight.Dispose();
            Dispose();
        }

        public void HideControl() {
            RegionHighlight.Visible = false;
        }

        public void ShowControl() {
            RegionHighlight.Visible = true;
        }

        public void HideRegion() {
            RegionHighlight.Visible = false;
            BackColor = Color.Silver;
            RegionHidden = true;
        }

        public void ShowRegion() {
            RegionHighlight.Visible = true;
            BackColor = Color.White;
            RegionHidden = false;
        }

        private void MineComboBox_SelectedIndexChanged(object sender, EventArgs e) {
            MineIndex = Convert.ToByte(MineComboBox.SelectedIndex);
        }

        private void MineEntry_Load(object sender, EventArgs e) {
            List<string> mineSetItems = new() { { "Disabled" } };
            Settings.MineSetList.ForEach(x => mineSetItems.Add(x.Name));
            MineComboBox.DataSource = mineSetItems;

            MineComboBox.SelectedIndex = MineIndex;

            Details.Text = $"X: {X.ToString()} | Y: {Y.ToString()} | Range: {Range.ToString()}";
        }

        private void Details_DoubleClick(object sender, EventArgs e) {
            MiningDetailForm MiningDetail = new();

            MiningDetail.X.Text = X.ToString();
            MiningDetail.Y.Text = Y.ToString();
            MiningDetail.Range.Text = Range.ToString();

            MiningDetail.ShowDialog();

            X = Convert.ToInt16(MiningDetail.X.Text);
            Y = Convert.ToInt16(MiningDetail.Y.Text);
            Range = Convert.ToUInt16(MiningDetail.Range.Text);

            MiningDetail.Dispose();
        }
    }
}
