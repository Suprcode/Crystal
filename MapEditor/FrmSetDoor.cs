using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Map_Editor
{
  public class FrmSetDoor : Form
  {
    private Main.DelSetDoorProperty _delSetDoorProperty;
    private IContainer components = (IContainer) null;
    private CheckBox chkCoreDoor;
    private Label label1;
    private TextBox txtDoorIndex;
    private TextBox txtDoorOffSet;
    private Label label2;
    private Button btnSetDoor;

    public FrmSetDoor() => this.InitializeComponent();

    public FrmSetDoor(Main.DelSetDoorProperty delSetDoorProperty)
    {
      this.InitializeComponent();
      this._delSetDoorProperty = delSetDoorProperty;
    }

    private void btnSetDoor_Click(object sender, EventArgs e)
    {
      if (!(this.txtDoorIndex.Text.Trim() != string.Empty) || !(this.txtDoorOffSet.Text.Trim() != string.Empty))
        return;
      this._delSetDoorProperty(this.chkCoreDoor.Checked, Convert.ToByte(this.txtDoorIndex.Text.Trim()), Convert.ToByte(this.txtDoorOffSet.Text.Trim()));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.chkCoreDoor = new CheckBox();
      this.label1 = new Label();
      this.txtDoorIndex = new TextBox();
      this.txtDoorOffSet = new TextBox();
      this.label2 = new Label();
      this.btnSetDoor = new Button();
      this.SuspendLayout();
      this.chkCoreDoor.AutoSize = true;
      this.chkCoreDoor.Location = new Point(24, 43);
      this.chkCoreDoor.Name = "chkCoreDoor";
      this.chkCoreDoor.Size = new Size(132, 16);
      this.chkCoreDoor.TabIndex = 0;
      this.chkCoreDoor.Text = "实体门 entity door";
      this.chkCoreDoor.UseVisualStyleBackColor = true;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(162, 28);
      this.label1.Name = "label1";
      this.label1.Size = new Size(107, 12);
      this.label1.TabIndex = 1;
      this.label1.Text = "门索引 door index";
      this.txtDoorIndex.Location = new Point(284, 25);
      this.txtDoorIndex.Name = "txtDoorIndex";
      this.txtDoorIndex.Size = new Size(50, 21);
      this.txtDoorIndex.TabIndex = 2;
      this.txtDoorOffSet.Location = new Point(284, 52);
      this.txtDoorOffSet.Name = "txtDoorOffSet";
      this.txtDoorOffSet.Size = new Size(50, 21);
      this.txtDoorOffSet.TabIndex = 4;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(162, 58);
      this.label2.Name = "label2";
      this.label2.Size = new Size(113, 12);
      this.label2.TabIndex = 3;
      this.label2.Text = "门偏移 door offSet";
      this.btnSetDoor.DialogResult = DialogResult.OK;
      this.btnSetDoor.Location = new Point(140, 94);
      this.btnSetDoor.Name = "btnSetDoor";
      this.btnSetDoor.Size = new Size(75, 23);
      this.btnSetDoor.TabIndex = 5;
      this.btnSetDoor.Text = "确定 ok";
      this.btnSetDoor.UseVisualStyleBackColor = true;
      this.btnSetDoor.Click += new EventHandler(this.btnSetDoor_Click);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(400, 145);
      this.Controls.Add((Control) this.btnSetDoor);
      this.Controls.Add((Control) this.txtDoorOffSet);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtDoorIndex);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.chkCoreDoor);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FrmSetDoor);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "SetDoor";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
