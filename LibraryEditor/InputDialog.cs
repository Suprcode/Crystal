namespace LibraryEditor {
    public class InputDialog : Form {
        private Label _label1;
        private Label _label2;
        private NumericUpDown _numericUpDown1;
        private NumericUpDown _numericUpDown2;
        private Button _buttonOk;
        private Button _buttonCancel;

        public InputDialog() {
            InitializeComponent();
        }

        private void InitializeComponent() {
            _label1 = new Label();
            _label2 = new Label();
            _numericUpDown1 = new NumericUpDown();
            _numericUpDown2 = new NumericUpDown();
            _buttonOk = new Button();
            _buttonCancel = new Button();

            // Label 1
            _label1.Text = "Offset X:";
            _label1.AutoSize = true;
            _label1.Location = new Point(12, 20);

            // Numeric Up-Down 1
            _numericUpDown1.Location = new Point(70, 18);
            _numericUpDown1.Width = 120;
            _numericUpDown1.Maximum = 1000;
            _numericUpDown1.Minimum = -1000;

            // Label 2
            _label2.Text = "Offset Y:";
            _label2.AutoSize = true;
            _label2.Location = new Point(12, 50);

            // Numeric Up-Down 2
            _numericUpDown2.Location = new Point(70, 48);
            _numericUpDown2.Width = 120;
            _numericUpDown1.Maximum = 1000;
            _numericUpDown2.Minimum = -1000;

            // Button Ok
            _buttonOk.Text = "Ok";
            _buttonOk.DialogResult = DialogResult.OK;
            _buttonOk.Location = new Point(24, 90);

            // Button Cancel
            _buttonCancel.Text = "Cancel";
            _buttonCancel.DialogResult = DialogResult.Cancel;
            _buttonCancel.Location = new Point(100, 90);

            // Form
            Text = "Input Dialog";
            ClientSize = new Size(200, 130);
            Controls.Add(_label1);
            Controls.Add(_label2);
            Controls.Add(_numericUpDown1);
            Controls.Add(_numericUpDown2);
            Controls.Add(_buttonOk);
            Controls.Add(_buttonCancel);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
            MaximizeBox = false;
            MinimizeBox = false;
            AcceptButton = _buttonOk;
            CancelButton = _buttonCancel;
            ShowInTaskbar = false;
        }

        public decimal Value1 => _numericUpDown1.Value;

        public decimal Value2 => _numericUpDown2.Value;
    }
}
