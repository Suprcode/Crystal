namespace CMirLibraryViewer {
    public partial class LoadSettings : Form {
        public LoadSettings() {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e) {
            Hide();
        }

        public bool GetFrontSide() {
            return cbFront.Checked;
        }

        public string GetPrefix() {
            return cbPrefix.Text;
        }

        public bool GetManualPrefix() {
            return cbManualPrefix.Checked;
        }

        private void cbManualPrefix_CheckedChanged(object sender, EventArgs e) {
            cbPrefix.Enabled = cbManualPrefix.Checked;
            if(cbPrefix.Enabled) {
                cbPrefix.Text = "00";
            } else {
                cbPrefix.Text = "";
            }
        }
    }
}
