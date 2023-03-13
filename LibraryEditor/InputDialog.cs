using System;
using System.Windows.Forms;

public class InputDialog : Form
{
    private Label _label1;
    private Label _label2;
    private NumericUpDown _numericUpDown1;
    private NumericUpDown _numericUpDown2;
    private Button _buttonOk;
    private Button _buttonCancel;

    public InputDialog()
    {
        InitializeComponent();
    }

    private void InitializeComponent()
    {
        _label1 = new Label();
        _label2 = new Label();
        _numericUpDown1 = new NumericUpDown();
        _numericUpDown2 = new NumericUpDown();
        _buttonOk = new Button();
        _buttonCancel = new Button();

        // Label 1
        _label1.Text = "Offset X:";
        _label1.AutoSize = true;
        _label1.Location = new System.Drawing.Point(12, 20);

        // Numeric Up-Down 1
        _numericUpDown1.Location = new System.Drawing.Point(70, 18);
        _numericUpDown1.Width = 120;
        _numericUpDown1.Maximum = 1000;
        _numericUpDown1.Minimum = -1000;

        // Label 2
        _label2.Text = "Offset Y:";
        _label2.AutoSize = true;
        _label2.Location = new System.Drawing.Point(12, 50);

        // Numeric Up-Down 2
        _numericUpDown2.Location = new System.Drawing.Point(70, 48);
        _numericUpDown2.Width = 120;
        _numericUpDown1.Maximum = 1000;
        _numericUpDown2.Minimum = -1000;

        // Button Ok
        _buttonOk.Text = "Ok";
        _buttonOk.DialogResult = DialogResult.OK;
        _buttonOk.Location = new System.Drawing.Point(24, 90);

        // Button Cancel
        _buttonCancel.Text = "Cancel";
        _buttonCancel.DialogResult = DialogResult.Cancel;
        _buttonCancel.Location = new System.Drawing.Point(100, 90);

        // Form
        this.Text = "Input Dialog";
        this.ClientSize = new System.Drawing.Size(200, 130);
        this.Controls.Add(_label1);
        this.Controls.Add(_label2);
        this.Controls.Add(_numericUpDown1);
        this.Controls.Add(_numericUpDown2);
        this.Controls.Add(_buttonOk);
        this.Controls.Add(_buttonCancel);
        this.FormBorderStyle = FormBorderStyle.FixedDialog;
        this.StartPosition = FormStartPosition.CenterParent;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.AcceptButton = _buttonOk;
        this.CancelButton = _buttonCancel;
        this.ShowInTaskbar = false;
    }

    public decimal Value1
    {
        get { return _numericUpDown1.Value; }
    }

    public decimal Value2
    {
        get { return _numericUpDown2.Value; }
    }
}