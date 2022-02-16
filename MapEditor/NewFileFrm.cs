using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Map_Editor
{
  public class NewFileFrm : Form
  {
    private readonly Main.DelSetMapSize _delSetMapSize;
    private IContainer components = (IContainer) null;
    private Label label1;
    private TextBox txtWidth;
    private TextBox txtHeight;
    private Label label2;
    private Button btnOk;

    public NewFileFrm() => this.InitializeComponent();

    public NewFileFrm(Main.DelSetMapSize delSetMapSize)
    {
      this.InitializeComponent();
      this._delSetMapSize = delSetMapSize;
    }

    private void btnOk_Click(object sender, EventArgs e)
    {
      int int32_1 = Convert.ToInt32(this.txtWidth.Text.Trim());
      int int32_2 = Convert.ToInt32(this.txtHeight.Text.Trim());
      if (int32_1 <= 0 || int32_2 <= 0 || int32_1 >= 1000 || int32_2 >= 1000)
      {
        int num = (int) MessageBox.Show("地图限制1000*1000");
      }
      else
      {
        this._delSetMapSize(int32_1, int32_2);
        this.DialogResult = DialogResult.OK;
        this.Close();
      }
    }

    private void txtWidth_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      KeyPressEventArgs e1 = new KeyPressEventArgs(Convert.ToChar((object) Keys.Return));
      this.btnOk_Click(sender, (EventArgs) e1);
    }

    private void txtHeight_KeyDown(object sender, KeyEventArgs e)
    {
      if (e.KeyCode != Keys.Return)
        return;
      KeyPressEventArgs e1 = new KeyPressEventArgs(Convert.ToChar((object) Keys.Return));
      this.btnOk_Click(sender, (EventArgs) e1);
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
      this.txtWidth = new TextBox();
      this.txtHeight = new TextBox();
      this.label2 = new Label();
      this.btnOk = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Font = new Font("Microsoft YaHei", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label1.Location = new Point(31, 33);
      this.label1.Margin = new Padding(4, 0, 4, 0);
      this.label1.Name = "label1";
      this.label1.Size = new Size(200, 27);
      this.label1.TabIndex = 0;
      this.label1.Text = "地图宽度 map width";
      this.txtWidth.Font = new Font("Microsoft YaHei", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.txtWidth.Location = new Point(261, 29);
      this.txtWidth.Margin = new Padding(4, 4, 4, 4);
      this.txtWidth.Name = "txtWidth";
      this.txtWidth.Size = new Size(132, 34);
      this.txtWidth.TabIndex = 1;
      this.txtWidth.Text = "50";
      this.txtWidth.KeyDown += new KeyEventHandler(this.txtWidth_KeyDown);
      this.txtHeight.Font = new Font("Microsoft YaHei", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.txtHeight.Location = new Point(261, 93);
      this.txtHeight.Margin = new Padding(4, 4, 4, 4);
      this.txtHeight.Name = "txtHeight";
      this.txtHeight.Size = new Size(132, 34);
      this.txtHeight.TabIndex = 3;
      this.txtHeight.Text = "50";
      this.txtHeight.KeyDown += new KeyEventHandler(this.txtHeight_KeyDown);
      this.label2.AutoSize = true;
      this.label2.Font = new Font("Microsoft YaHei", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.label2.Location = new Point(31, 97);
      this.label2.Margin = new Padding(4, 0, 4, 0);
      this.label2.Name = "label2";
      this.label2.Size = new Size(207, 27);
      this.label2.TabIndex = 2;
      this.label2.Text = "地图高度 map height";
      this.btnOk.DialogResult = DialogResult.OK;
      this.btnOk.Font = new Font("Microsoft YaHei", 12f, FontStyle.Regular, GraphicsUnit.Point, (byte) 134);
      this.btnOk.Location = new Point(159, 168);
      this.btnOk.Margin = new Padding(4, 4, 4, 4);
      this.btnOk.Name = "btnOk";
      this.btnOk.Size = new Size(128, 55);
      this.btnOk.TabIndex = 4;
      this.btnOk.Text = "确定 ok";
      this.btnOk.UseVisualStyleBackColor = true;
      this.btnOk.Click += new EventHandler(this.btnOk_Click);
      this.AutoScaleDimensions = new SizeF(8f, 16f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(468, 239);
      this.Controls.Add((Control) this.btnOk);
      this.Controls.Add((Control) this.txtHeight);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtWidth);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedToolWindow;
      this.Margin = new Padding(4, 4, 4, 4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (NewFileFrm);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (NewFileFrm);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
