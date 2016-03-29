namespace LibraryViewer
{
    partial class LoadSettings
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
            this.button1 = new System.Windows.Forms.Button();
            this.cbPrefix = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.cbFront = new System.Windows.Forms.CheckBox();
            this.cbManualPrefix = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(136, 36);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 21);
            this.button1.TabIndex = 0;
            this.button1.Text = "开始";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // cbPrefix
            // 
            this.cbPrefix.Enabled = false;
            this.cbPrefix.FormattingEnabled = true;
            this.cbPrefix.Items.AddRange(new object[] {
            "00",
            "000",
            "0000"});
            this.cbPrefix.Location = new System.Drawing.Point(54, 11);
            this.cbPrefix.Name = "cbPrefix";
            this.cbPrefix.Size = new System.Drawing.Size(54, 20);
            this.cbPrefix.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 12);
            this.label1.TabIndex = 2;
            this.label1.Text = "前缀:";
            // 
            // cbFront
            // 
            this.cbFront.AutoSize = true;
            this.cbFront.Location = new System.Drawing.Point(15, 40);
            this.cbFront.Name = "cbFront";
            this.cbFront.Size = new System.Drawing.Size(90, 16);
            this.cbFront.TabIndex = 3;
            this.cbFront.Text = "FrontImages";
            this.cbFront.UseVisualStyleBackColor = true;
            // 
            // cbManualPrefix
            // 
            this.cbManualPrefix.AutoSize = true;
            this.cbManualPrefix.Location = new System.Drawing.Point(124, 13);
            this.cbManualPrefix.Name = "cbManualPrefix";
            this.cbManualPrefix.Size = new System.Drawing.Size(72, 16);
            this.cbManualPrefix.TabIndex = 4;
            this.cbManualPrefix.Text = "手动输入";
            this.cbManualPrefix.UseVisualStyleBackColor = true;
            this.cbManualPrefix.CheckedChanged += new System.EventHandler(this.cbManualPrefix_CheckedChanged);
            // 
            // LoadSettings
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(223, 66);
            this.Controls.Add(this.cbManualPrefix);
            this.Controls.Add(this.cbFront);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cbPrefix);
            this.Controls.Add(this.button1);
            this.Name = "LoadSettings";
            this.Text = "LoadSettings";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ComboBox cbPrefix;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox cbFront;
        private System.Windows.Forms.CheckBox cbManualPrefix;
    }
}