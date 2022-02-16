using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Map_Editor
{
  public class FrmSetLight : Form
  {
    private readonly Main.DelSetLightProperty _delSetLightProperty;
    private IContainer components = (IContainer) null;
    private Label label1;
    private TextBox txtLight;
    private Button btnSetLight;

    public FrmSetLight() => this.InitializeComponent();

    public FrmSetLight(Main.DelSetLightProperty delSetLightProperty)
    {
      this.InitializeComponent();
      this._delSetLightProperty = delSetLightProperty;
    }

    private void btnSetLight_Click(object sender, EventArgs e)
    {
      if (!(this.txtLight.Text.Trim() != string.Empty))
        return;
      this._delSetLightProperty(Convert.ToByte(this.txtLight.Text.Trim()));
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
      this.txtLight = new TextBox();
      this.btnSetLight = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.Location = new Point(12, 27);
      this.label1.Name = "label1";
      this.label1.Size = new Size(65, 12);
      this.label1.TabIndex = 0;
      this.label1.Text = "亮度 light";
      this.txtLight.Location = new Point(79, 24);
      this.txtLight.Name = "txtLight";
      this.txtLight.Size = new Size(63, 21);
      this.txtLight.TabIndex = 1;
      this.btnSetLight.DialogResult = DialogResult.OK;
      this.btnSetLight.Location = new Point(79, 60);
      this.btnSetLight.Name = "btnSetLight";
      this.btnSetLight.Size = new Size(63, 23);
      this.btnSetLight.TabIndex = 2;
      this.btnSetLight.Text = "确定 ok";
      this.btnSetLight.UseVisualStyleBackColor = true;
      this.btnSetLight.Click += new EventHandler(this.btnSetLight_Click);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.ClientSize = new Size(196, 103);
      this.Controls.Add((Control) this.btnSetLight);
      this.Controls.Add((Control) this.txtLight);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.FixedDialog;
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = nameof (FrmSetLight);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = "SetLight";
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
