namespace Server.MirForms.VisualMapInfo.Control.Forms
{
    partial class RespawnsDetailForm
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
            this.Spread = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.Y = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.DoneButton = new System.Windows.Forms.Button();
            this.X = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.Delay = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.Count = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.RoutePath = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // Spread
            // 
            this.Spread.Location = new System.Drawing.Point(71, 64);
            this.Spread.Name = "Spread";
            this.Spread.Size = new System.Drawing.Size(53, 20);
            this.Spread.TabIndex = 13;
            this.Spread.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Chk);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(16, 67);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(44, 13);
            this.label3.TabIndex = 12;
            this.label3.Text = "Spread:";
            // 
            // Y
            // 
            this.Y.Location = new System.Drawing.Point(71, 38);
            this.Y.Name = "Y";
            this.Y.Size = new System.Drawing.Size(53, 20);
            this.Y.TabIndex = 11;
            this.Y.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Chk);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(41, 41);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 13);
            this.label2.TabIndex = 10;
            this.label2.Text = "Y:";
            // 
            // DoneButton
            // 
            this.DoneButton.Location = new System.Drawing.Point(174, 89);
            this.DoneButton.Name = "DoneButton";
            this.DoneButton.Size = new System.Drawing.Size(105, 24);
            this.DoneButton.TabIndex = 9;
            this.DoneButton.Text = "Done";
            this.DoneButton.UseVisualStyleBackColor = true;
            this.DoneButton.Click += new System.EventHandler(this.DoneButton_Click);
            // 
            // X
            // 
            this.X.Location = new System.Drawing.Point(71, 12);
            this.X.Name = "X";
            this.X.Size = new System.Drawing.Size(53, 20);
            this.X.TabIndex = 8;
            this.X.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Chk);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(41, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "X:";
            // 
            // Delay
            // 
            this.Delay.Location = new System.Drawing.Point(184, 38);
            this.Delay.Name = "Delay";
            this.Delay.Size = new System.Drawing.Size(53, 20);
            this.Delay.TabIndex = 17;
            this.Delay.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Chk);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(141, 41);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(37, 13);
            this.label4.TabIndex = 16;
            this.label4.Text = "Delay:";
            // 
            // Count
            // 
            this.Count.Location = new System.Drawing.Point(184, 12);
            this.Count.Name = "Count";
            this.Count.Size = new System.Drawing.Size(53, 20);
            this.Count.TabIndex = 15;
            this.Count.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.Chk);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(140, 15);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(38, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "Count:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(243, 41);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(34, 13);
            this.label6.TabIndex = 18;
            this.label6.Text = "(mins)";
            // 
            // RoutePath
            // 
            this.RoutePath.Location = new System.Drawing.Point(184, 63);
            this.RoutePath.Name = "RoutePath";
            this.RoutePath.Size = new System.Drawing.Size(100, 20);
            this.RoutePath.TabIndex = 19;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(141, 66);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(39, 13);
            this.label7.TabIndex = 20;
            this.label7.Text = "Route:";
            // 
            // RespawnsDetailForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(291, 125);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.RoutePath);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.Delay);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.Count);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Spread);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.Y);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.DoneButton);
            this.Controls.Add(this.X);
            this.Controls.Add(this.label1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "RespawnsDetailForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Respawns";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.TextBox Spread;
        private System.Windows.Forms.Label label3;
        public System.Windows.Forms.TextBox Y;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button DoneButton;
        public System.Windows.Forms.TextBox X;
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.TextBox Delay;
        private System.Windows.Forms.Label label4;
        public System.Windows.Forms.TextBox Count;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.TextBox RoutePath;
        private System.Windows.Forms.Label label7;
    }
}