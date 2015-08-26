namespace AutoPatcher
{
    partial class AMain
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
            this.components = new System.ComponentModel.Container();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.ImageLinkLabel = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.ActionLabel = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.CancelButton = new System.Windows.Forms.Button();
            this.PlayButton = new System.Windows.Forms.Button();
            this.SizeLabel = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.FileLabel = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBar2 = new System.Windows.Forms.ProgressBar();
            this.InterfaceTimer = new System.Windows.Forms.Timer(this.components);
            this.SourceLinkLabel = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::AutoPatcher.Properties.Resources.C_Mir;
            this.pictureBox1.Location = new System.Drawing.Point(12, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(501, 152);
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // ImageLinkLabel
            // 
            this.ImageLinkLabel.AutoSize = true;
            this.ImageLinkLabel.Location = new System.Drawing.Point(401, 167);
            this.ImageLinkLabel.Name = "ImageLinkLabel";
            this.ImageLinkLabel.Size = new System.Drawing.Size(112, 13);
            this.ImageLinkLabel.TabIndex = 2;
            this.ImageLinkLabel.TabStop = true;
            this.ImageLinkLabel.Text = "Image by DevilsKnight";
            this.ImageLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.ImageLinkLabel_LinkClicked);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(35, 177);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "Action:";
            // 
            // ActionLabel
            // 
            this.ActionLabel.AutoSize = true;
            this.ActionLabel.Location = new System.Drawing.Point(81, 177);
            this.ActionLabel.Name = "ActionLabel";
            this.ActionLabel.Size = new System.Drawing.Size(85, 13);
            this.ActionLabel.TabIndex = 4;
            this.ActionLabel.Text = "Checking Files...";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 219);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(501, 17);
            this.progressBar1.TabIndex = 5;
            // 
            // CancelButton
            // 
            this.CancelButton.Location = new System.Drawing.Point(438, 258);
            this.CancelButton.Name = "CancelButton";
            this.CancelButton.Size = new System.Drawing.Size(75, 23);
            this.CancelButton.TabIndex = 6;
            this.CancelButton.Text = "Cancel";
            this.CancelButton.UseVisualStyleBackColor = true;
            this.CancelButton.Click += new System.EventHandler(this.CancelButton_Click);
            // 
            // PlayButton
            // 
            this.PlayButton.Location = new System.Drawing.Point(12, 258);
            this.PlayButton.Name = "PlayButton";
            this.PlayButton.Size = new System.Drawing.Size(75, 23);
            this.PlayButton.TabIndex = 7;
            this.PlayButton.Text = "Play";
            this.PlayButton.UseVisualStyleBackColor = true;
            this.PlayButton.Click += new System.EventHandler(this.PlayButton_Click);
            // 
            // SizeLabel
            // 
            this.SizeLabel.AutoSize = true;
            this.SizeLabel.Location = new System.Drawing.Point(81, 190);
            this.SizeLabel.Name = "SizeLabel";
            this.SizeLabel.Size = new System.Drawing.Size(24, 13);
            this.SizeLabel.TabIndex = 9;
            this.SizeLabel.Text = "Idle";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 190);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(57, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Total Size:";
            // 
            // FileLabel
            // 
            this.FileLabel.AutoSize = true;
            this.FileLabel.Location = new System.Drawing.Point(81, 203);
            this.FileLabel.Name = "FileLabel";
            this.FileLabel.Size = new System.Drawing.Size(24, 13);
            this.FileLabel.TabIndex = 11;
            this.FileLabel.Text = "Idle";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(12, 203);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(63, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "Current File:";
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.AutoSize = true;
            this.SpeedLabel.Location = new System.Drawing.Point(448, 203);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.Size = new System.Drawing.Size(24, 13);
            this.SpeedLabel.TabIndex = 13;
            this.SpeedLabel.Text = "Idle";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(401, 203);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(41, 13);
            this.label7.TabIndex = 12;
            this.label7.Text = "Speed:";
            // 
            // progressBar2
            // 
            this.progressBar2.Location = new System.Drawing.Point(12, 242);
            this.progressBar2.Name = "progressBar2";
            this.progressBar2.Size = new System.Drawing.Size(501, 10);
            this.progressBar2.TabIndex = 14;
            // 
            // InterfaceTimer
            // 
            this.InterfaceTimer.Enabled = true;
            this.InterfaceTimer.Interval = 50;
            this.InterfaceTimer.Tick += new System.EventHandler(this.InterfaceTimer_Tick);
            // 
            // SourceLinkLabel
            // 
            this.SourceLinkLabel.AutoSize = true;
            this.SourceLinkLabel.Location = new System.Drawing.Point(401, 284);
            this.SourceLinkLabel.Name = "SourceLinkLabel";
            this.SourceLinkLabel.Size = new System.Drawing.Size(117, 13);
            this.SourceLinkLabel.TabIndex = 15;
            this.SourceLinkLabel.TabStop = true;
            this.SourceLinkLabel.Text = "Created by Jamie/Hello";
            this.SourceLinkLabel.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.SourceLinkLabel_LinkClicked);
            // 
            // AMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(525, 307);
            this.Controls.Add(this.SourceLinkLabel);
            this.Controls.Add(this.progressBar2);
            this.Controls.Add(this.SpeedLabel);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.FileLabel);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.SizeLabel);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.PlayButton);
            this.Controls.Add(this.CancelButton);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.ActionLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ImageLinkLabel);
            this.Controls.Add(this.pictureBox1);
            this.Name = "AMain";
            this.Text = "Auto Patcher";
            this.Load += new System.EventHandler(this.AMain_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.LinkLabel ImageLinkLabel;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label ActionLabel;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Button CancelButton;
        private System.Windows.Forms.Button PlayButton;
        private System.Windows.Forms.Label SizeLabel;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label FileLabel;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label SpeedLabel;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar progressBar2;
        private System.Windows.Forms.Timer InterfaceTimer;
        private System.Windows.Forms.LinkLabel SourceLinkLabel;
    }
}

