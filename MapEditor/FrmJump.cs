using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Map_Editor
{
  public class FrmJump : Form
  {
    private Main.DelJump _delJump;
    private IContainer components = (IContainer) null;
    private Label label1;
    private TextBox txtX;
    private TextBox txtY;
    private Label label2;
    private Button btnJump;

    public FrmJump() => this.InitializeComponent();

    public FrmJump(Main.DelJump delJump)
    {
      this.InitializeComponent();
      this._delJump = delJump;
    }

    private void btnJump_Click(object sender, EventArgs e)
    {
      if (!(this.txtX.Text.Trim() != string.Empty) || !(this.txtY.Text.Trim() != string.Empty))
        return;
      this._delJump(Convert.ToInt32(this.txtX.Text.Trim()), Convert.ToInt32(this.txtY.Text.Trim()));
      this.Dispose();
    }

    private void txtX_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      KeyPressEventArgs e1 = new KeyPressEventArgs(Convert.ToChar((object) Keys.Return));
      this.btnJump_Click(sender, (EventArgs) e1);
    }

    private void txtY_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      KeyPressEventArgs e1 = new KeyPressEventArgs(Convert.ToChar((object) Keys.Return));
      this.btnJump_Click(sender, (EventArgs) e1);
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.label1 = new Label();
      this.txtX = new TextBox();
      this.txtY = new TextBox();
      this.label2 = new Label();
      this.btnJump = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(36, 33);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(17, 17);
      this.label1.TabIndex = 0;
      this.label1.Text = "X";
      this.txtX.Location = new Point(59, 29);
      this.txtX.Margin = new Padding(4, 4, 4, 4);
      this.txtX.Name = "txtX";
      this.txtX.Size = new Size(63, 22);
      this.txtX.TabIndex = 1;
      this.txtX.KeyDown += new KeyEventHandler(this.txtX_KeyDown);
      this.txtY.Location = new Point(59, 65);
      this.txtY.Margin = new Padding(4, 4, 4, 4);
      this.txtY.Name = "txtY";
      this.txtY.Size = new Size(63, 22);
      this.txtY.TabIndex = 3;
      this.txtY.KeyDown += new KeyEventHandler(this.txtY_KeyDown);
      this.label2.AutoSize = true;
      this.label2.Location = new Point(36, 69);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(17, 17);
      this.label2.TabIndex = 2;
      this.label2.Text = "Y";
      this.btnJump.DialogResult = DialogResult.OK;
      this.btnJump.Location = new Point(144, 47);
      this.btnJump.Margin = new Padding(4, 4, 4, 4);
      this.btnJump.Name = "btnJump";
      this.btnJump.Size = new Size(100, 31);
      this.btnJump.TabIndex = 4;
      this.btnJump.Text = "Jump";
      this.btnJump.UseVisualStyleBackColor = true;
      this.btnJump.Click += new EventHandler(this.btnJump_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(281, 113);
      this.Controls.Add((Control) this.btnJump);
      this.Controls.Add((Control) this.txtY);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtX);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.Margin = new Padding(4, 4, 4, 4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FrmJump);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (FrmJump);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
