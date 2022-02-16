using System;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Map_Editor
{
  public class FrmAbout : Form
  {
    private IContainer components = (IContainer) null;
    private Label label1;
    private Label label2;
    private Label label3;
    private Button btnclose;

    public FrmAbout() => this.InitializeComponent();

    private void btnclose_Click(object sender, EventArgs e) => this.Close();

    protected override void Dispose(bool disposing)
    {
      if (disposing && this.components != null)
        this.components.Dispose();
      base.Dispose(disposing);
    }

    private void InitializeComponent()
    {
      ComponentResourceManager componentResourceManager = new ComponentResourceManager(typeof (FrmAbout));
      this.label1 = new Label();
      this.label2 = new Label();
      this.label3 = new Label();
      this.btnclose = new Button();
      this.SuspendLayout();
      this.label1.AutoSize = true;
      this.label1.BackColor = Color.White;
      this.label1.Font = new Font("Comic Sans MS", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label1.ForeColor = Color.Black;
      this.label1.Location = new Point(6, 233);
      this.label1.Name = "label1";
      this.label1.Size = new Size(159, 19);
      this.label1.TabIndex = 0;
      this.label1.Text = "Author : xiyue （西月）";
      this.label2.AutoSize = true;
      this.label2.BackColor = Color.White;
      this.label2.Font = new Font("Comic Sans MS", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label2.ForeColor = Color.Black;
      this.label2.Location = new Point(6, 266);
      this.label2.Name = "label2";
      this.label2.Size = new Size(181, 19);
      this.label2.TabIndex = 1;
      this.label2.Text = "Email：xiyue173@163.com";
      this.label3.AutoSize = true;
      this.label3.BackColor = Color.White;
      this.label3.Font = new Font("Comic Sans MS", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.label3.ForeColor = Color.Black;
      this.label3.Location = new Point(6, 199);
      this.label3.Name = "label3";
      this.label3.Size = new Size(99, 19);
      this.label3.TabIndex = 2;
      this.label3.Text = "Version: None";
      this.btnclose.Font = new Font("Comic Sans MS", 10.5f, FontStyle.Regular, GraphicsUnit.Point, (byte) 0);
      this.btnclose.Location = new Point(346, 266);
      this.btnclose.Name = "btnclose";
      this.btnclose.Size = new Size(75, 23);
      this.btnclose.TabIndex = 3;
      this.btnclose.Text = "Close";
      this.btnclose.UseVisualStyleBackColor = true;
      this.btnclose.Click += new EventHandler(this.btnclose_Click);
      this.AutoScaleDimensions = new SizeF(6f, 12f);
      this.AutoScaleMode = AutoScaleMode.Font;
      this.BackgroundImage = (Image) componentResourceManager.GetObject("$this.BackgroundImage");
      this.ClientSize = new Size(423, 293);
      this.Controls.Add((Control) this.btnclose);
      this.Controls.Add((Control) this.label3);
      this.Controls.Add((Control) this.label2);
      this.Controls.Add((Control) this.label1);
      this.FormBorderStyle = FormBorderStyle.None;
      this.Name = nameof (FrmAbout);
      this.StartPosition = FormStartPosition.CenterScreen;
      this.Text = nameof (FrmAbout);
      this.ResumeLayout(false);
      this.PerformLayout();
    }
  }
}
