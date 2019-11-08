namespace Server.MirForms.VisualMapInfo.Control
{
    partial class MineEntry
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
            this.MineComboBox = new System.Windows.Forms.ComboBox();
            this.Details = new System.Windows.Forms.Label();
            this.Selected = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // MineComboBox
            // 
            this.MineComboBox.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.MineComboBox.FormattingEnabled = true;
            this.MineComboBox.Location = new System.Drawing.Point(24, 3);
            this.MineComboBox.Name = "MineComboBox";
            this.MineComboBox.Size = new System.Drawing.Size(68, 21);
            this.MineComboBox.TabIndex = 2;
            this.MineComboBox.SelectedIndexChanged += new System.EventHandler(this.MineComboBox_SelectedIndexChanged);
            this.MineComboBox.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
            // 
            // Details
            // 
            this.Details.AutoSize = true;
            this.Details.Location = new System.Drawing.Point(97, 7);
            this.Details.Name = "Details";
            this.Details.Size = new System.Drawing.Size(39, 13);
            this.Details.TabIndex = 3;
            this.Details.Text = "Details";
            this.Details.DoubleClick += new System.EventHandler(this.Details_DoubleClick);
            this.Details.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
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
            // 
            // MineEntry
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.Controls.Add(this.Selected);
            this.Controls.Add(this.Details);
            this.Controls.Add(this.MineComboBox);
            this.Name = "MineEntry";
            this.Size = new System.Drawing.Size(249, 28);
            this.Load += new System.EventHandler(this.MineEntry_Load);
            this.MouseEnter += new System.EventHandler(this.Region_MouseEnter);
            this.MouseLeave += new System.EventHandler(this.Region_MouseLeave);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox MineComboBox;
        private System.Windows.Forms.Label Details;
        public System.Windows.Forms.CheckBox Selected;
    }
}
