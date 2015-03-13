namespace Server.MirForms.VisualMapInfo.Control
{
    partial class RespawnEntry
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.MonsterComboBox = new System.Windows.Forms.ComboBox();
            this.Details = new System.Windows.Forms.Label();
            this.Selected = new System.Windows.Forms.CheckBox();
            this.Count = new System.Windows.Forms.TextBox();
            this.Delay = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // MonsterComboBox
            // 
            this.MonsterComboBox.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.MonsterComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MonsterComboBox.FormattingEnabled = true;
            this.MonsterComboBox.Location = new System.Drawing.Point(24, 3);
            this.MonsterComboBox.Name = "MonsterComboBox";
            this.MonsterComboBox.Size = new System.Drawing.Size(222, 21);
            this.MonsterComboBox.TabIndex = 2;
            this.MonsterComboBox.SelectedIndexChanged += new System.EventHandler(this.MonsterComboBox_SelectedIndexChanged);
            this.MonsterComboBox.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
            this.MonsterComboBox.MouseLeave += new System.EventHandler(this.Region_MouseLeave);
            // 
            // Details
            // 
            this.Details.AutoSize = true;
            this.Details.Location = new System.Drawing.Point(3, 30);
            this.Details.Name = "Details";
            this.Details.Size = new System.Drawing.Size(103, 13);
            this.Details.TabIndex = 3;
            this.Details.Text = "C               D            ";
            this.Details.DoubleClick += new System.EventHandler(this.Details_DoubleClick);
            this.Details.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
            this.Details.MouseLeave += new System.EventHandler(this.Region_MouseLeave);
            // 
            // Selected
            // 
            this.Selected.AutoSize = true;
            this.Selected.Location = new System.Drawing.Point(3, 7);
            this.Selected.Name = "Selected";
            this.Selected.Size = new System.Drawing.Size(15, 14);
            this.Selected.TabIndex = 4;
            this.Selected.UseVisualStyleBackColor = true;
            this.Selected.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
            this.Selected.MouseLeave += new System.EventHandler(this.Region_MouseLeave);
            // 
            // Count
            // 
            this.Count.Location = new System.Drawing.Point(19, 27);
            this.Count.Name = "Count";
            this.Count.Size = new System.Drawing.Size(28, 20);
            this.Count.TabIndex = 5;
            this.Count.Text = "0";
            this.Count.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
            this.Count.MouseLeave += new System.EventHandler(this.Region_MouseLeave);
            // 
            // Delay
            // 
            this.Delay.Location = new System.Drawing.Point(70, 27);
            this.Delay.Name = "Delay";
            this.Delay.Size = new System.Drawing.Size(28, 20);
            this.Delay.TabIndex = 6;
            this.Delay.Text = "0";
            this.Delay.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
            this.Delay.MouseLeave += new System.EventHandler(this.Region_MouseLeave);
            // 
            // RespawnEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Delay);
            this.Controls.Add(this.Count);
            this.Controls.Add(this.Selected);
            this.Controls.Add(this.Details);
            this.Controls.Add(this.MonsterComboBox);
            this.Name = "RespawnEntry";
            this.Size = new System.Drawing.Size(249, 50);
            this.Load += new System.EventHandler(this.RespawnEntry_Load);
            this.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Region_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MonsterComboBox;
        private System.Windows.Forms.Label Details;
        public System.Windows.Forms.CheckBox Selected;
        public System.Windows.Forms.TextBox Count;
        public System.Windows.Forms.TextBox Delay;
    }
}
