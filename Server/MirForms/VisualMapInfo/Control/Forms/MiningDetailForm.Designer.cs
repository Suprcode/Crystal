namespace Server.MirForms.VisualMapInfo.Control.Forms
{
    partial class MiningDetailForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Range = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Y = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DoneButton = new System.Windows.Forms.Button();
            this.X = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Range
            // 
            this.Range.Location = new System.Drawing.Point(62, 70);
            this.Range.Name = "Range";
            this.Range.Size = new System.Drawing.Size(72, 20);
            this.Range.TabIndex = 20;
            this.Range.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Insert);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(42, 13);
            this.label3.TabIndex = 19;
            this.label3.Text = "Range:";
            // 
            // Y
            // 
            this.Y.Location = new System.Drawing.Point(62, 41);
            this.Y.Name = "Y";
            this.Y.Size = new System.Drawing.Size(72, 20);
            this.Y.TabIndex = 18;
            this.Y.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Insert);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 44);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 17;
            this.label2.Text = "Y:";
            // 
            // DoneButton
            // 
            this.DoneButton.Location = new System.Drawing.Point(12, 97);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(122, 24);
            this.DoneButton.TabIndex = 16;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // X
            // 
            this.X.Location = new System.Drawing.Point(62, 12);
            this.X.Name = "X";
            this.X.Size = new System.Drawing.Size(72, 20);
            this.X.TabIndex = 15;
            this.X.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Insert);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(37, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 14;
            this.label1.Text = "X:";
            // 
            // MiningDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(146, 133);
            this.Controls.Add(this.Range);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Y);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.X);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "MiningDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Mining";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox Range;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox Y;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DoneButton;
        public System.Windows.Forms.TextBox X;
        private System.Windows.Forms.Label label1;
    }
}