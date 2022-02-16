using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Map_Editor
{
  public class FrmSetAnimation : Form
  {
    private Main.DelSetAnimationProperty _delSetAnimationProperty;
    private IContainer components = (IContainer) null;
    private Button btnSetAnimation;
    private TextBox txtAnimationTick;
    private Label label2;
    private TextBox txtAnimationFrame;
    private Label label1;
    private CheckBox chkBlend;

    public FrmSetAnimation() => this.InitializeComponent();

    public FrmSetAnimation(
      Main.DelSetAnimationProperty delSetAnimationProperty)
    {
      this.InitializeComponent();
      this._delSetAnimationProperty = delSetAnimationProperty;
    }

    private void btnSetAnimation_Click(object sender, EventArgs e)
    {
      if (!(this.txtAnimationFrame.Text.Trim() != string.Empty) || !(this.txtAnimationTick.Text.Trim() != string.Empty))
        return;
      this._delSetAnimationProperty(this.chkBlend.Checked, Convert.ToByte(this.txtAnimationFrame.Text.Trim()), Convert.ToByte(this.txtAnimationTick.Text.Trim()));
    }

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      this.btnSetAnimation = new Button();
      this.txtAnimationTick = new TextBox();
      this.label2 = new Label();
      this.txtAnimationFrame = new TextBox();
      this.label1 = new Label();
      this.chkBlend = new CheckBox();
      this.SuspendLayout();
      this.btnSetAnimation.DialogResult = DialogResult.OK;
      this.btnSetAnimation.Location = new Point(99, 86);
      this.btnSetAnimation.Name = "btnSetAnimation";
      this.btnSetAnimation.Size = new Size(75, 23);
      this.btnSetAnimation.TabIndex = 11;
      this.btnSetAnimation.Text = "确定 ok";
      this.btnSetAnimation.UseVisualStyleBackColor = true;
      this.btnSetAnimation.Click += new EventHandler(this.btnSetAnimation_Click);
      this.txtAnimationTick.Location = new Point(264, 49);
      this.txtAnimationTick.Name = "txtAnimationTick";
      this.txtAnimationTick.Size = new Size(50, 21);
      this.txtAnimationTick.TabIndex = 10;
      this.label2.AutoSize = true;
      this.label2.Location = new Point(97, 52);
      this.label2.Name = "label2";
      this.label2.Size = new Size(173, 12);
      this.label2.TabIndex = 9;
      this.label2.Text = "动画间隔 Animation interval ";
      this.txtAnimationFrame.Location = new Point(264, 22);
      this.txtAnimationFrame.Name = "txtAnimationFrame";
      this.txtAnimationFrame.Size = new Size(50, 21);
      this.txtAnimationFrame.TabIndex = 8;
      this.label1.AutoSize = true;
      this.label1.Location = new Point(97, 25);
      this.label1.Name = "label1";
      this.label1.Size = new Size(161, 12);
      this.label1.TabIndex = 7;
      this.label1.Text = "动画帧数 Animation frames ";
      this.chkBlend.AutoSize = true;
      this.chkBlend.Location = new Point(12, 37);
      this.chkBlend.Name = "chkBlend";
      this.chkBlend.Size = new Size(84, 16);
      this.chkBlend.TabIndex = 6;
      this.chkBlend.Text = "混合 blend";
      this.chkBlend.UseVisualStyleBackColor = true;
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(351, 135);
      this.Controls.Add((Control) this.btnSetAnimation);
      this.Controls.Add((Control) this.txtAnimationTick);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.txtAnimationFrame);
      this.Controls.Add((Control) this.label1);
      this.Controls.Add((Control) this.chkBlend);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FrmSetAnimation);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (FrmSetAnimation);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
