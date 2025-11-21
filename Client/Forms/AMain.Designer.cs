using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Net;
using System.Windows.Forms;
using System.Drawing;

namespace Launcher
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
            components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AMain));
            ActionLabel = new Label();
            SpeedLabel = new Label();
            InterfaceTimer = new System.Windows.Forms.Timer(components);
            Movement_panel = new Panel();
            Name_label = new Label();
            pictureBox1 = new PictureBox();
            Close_pb = new PictureBox();
            Config_pb = new PictureBox();
            Version_label = new Label();
            CurrentFile_label = new Label();
            CurrentPercent_label = new Label();
            TotalPercent_label = new Label();
            Credit_label = new Label();
            ProgTotalEnd_pb = new PictureBox();
            ProgEnd_pb = new PictureBox();
            ProgressCurrent_pb = new PictureBox();
            TotalProg_pb = new PictureBox();
            Launch_pb = new PictureBox();
            Main_browser = new Microsoft.Web.WebView2.WinForms.WebView2();
            Movement_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Close_pb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Config_pb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ProgTotalEnd_pb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ProgEnd_pb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)ProgressCurrent_pb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)TotalProg_pb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Launch_pb).BeginInit();
            ((System.ComponentModel.ISupportInitialize)Main_browser).BeginInit();
            SuspendLayout();
            // 
            // ActionLabel
            // 
            ActionLabel.Anchor = AnchorStyles.Bottom;
            ActionLabel.BackColor = Color.Transparent;
            ActionLabel.Font = new Font("Calibri", 8.25F);
            ActionLabel.ForeColor = Color.Gray;
            ActionLabel.Location = new Point(489, 505);
            ActionLabel.Margin = new Padding(4, 0, 4, 0);
            ActionLabel.Name = "ActionLabel";
            ActionLabel.Size = new Size(126, 24);
            ActionLabel.TabIndex = 4;
            ActionLabel.Text = "1423MB/2000MB";
            ActionLabel.TextAlign = ContentAlignment.MiddleRight;
            ActionLabel.Visible = false;
            ActionLabel.Click += ActionLabel_Click;
            // 
            // SpeedLabel
            // 
            SpeedLabel.Anchor = AnchorStyles.Bottom;
            SpeedLabel.BackColor = Color.Transparent;
            SpeedLabel.Font = new Font("Calibri", 8.25F);
            SpeedLabel.ForeColor = Color.Gray;
            SpeedLabel.Location = new Point(426, 507);
            SpeedLabel.Margin = new Padding(4, 0, 4, 0);
            SpeedLabel.Name = "SpeedLabel";
            SpeedLabel.RightToLeft = RightToLeft.No;
            SpeedLabel.Size = new Size(83, 20);
            SpeedLabel.TabIndex = 13;
            SpeedLabel.Text = "Speed";
            SpeedLabel.TextAlign = ContentAlignment.MiddleRight;
            SpeedLabel.Visible = false;
            // 
            // InterfaceTimer
            // 
            InterfaceTimer.Enabled = true;
            InterfaceTimer.Interval = 50;
            InterfaceTimer.Tick += InterfaceTimer_Tick;
            // 
            // Movement_panel
            // 
            Movement_panel.BackColor = Color.Transparent;
            Movement_panel.BackgroundImageLayout = ImageLayout.Center;
            Movement_panel.Controls.Add(Name_label);
            Movement_panel.Controls.Add(pictureBox1);
            Movement_panel.Controls.Add(Close_pb);
            Movement_panel.Controls.Add(Config_pb);
            Movement_panel.Location = new Point(3, 7);
            Movement_panel.Margin = new Padding(4, 3, 4, 3);
            Movement_panel.Name = "Movement_panel";
            Movement_panel.Size = new Size(800, 49);
            Movement_panel.TabIndex = 21;
            Movement_panel.MouseClick += Movement_panel_MouseClick;
            Movement_panel.MouseDown += Movement_panel_MouseClick;
            Movement_panel.MouseMove += Movement_panel_MouseMove;
            Movement_panel.MouseUp += Movement_panel_MouseUp;
            // 
            // Name_label
            // 
            Name_label.BackColor = Color.Transparent;
            Name_label.Font = new Font("Microsoft Sans Serif", 8.25F);
            Name_label.ForeColor = Color.White;
            Name_label.Image = Client.Resources.Images.server_base;
            Name_label.Location = new Point(289, 12);
            Name_label.Margin = new Padding(4, 0, 4, 0);
            Name_label.Name = "Name_label";
            Name_label.Size = new Size(217, 28);
            Name_label.TabIndex = 0;
            Name_label.Text = "Crystal Mir 2";
            Name_label.TextAlign = ContentAlignment.MiddleCenter;
            Name_label.Visible = false;
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Client.Resources.Images.server_base;
            pictureBox1.Location = new Point(358, -52);
            pictureBox1.Margin = new Padding(4, 3, 4, 3);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(217, 26);
            pictureBox1.TabIndex = 33;
            pictureBox1.TabStop = false;
            // 
            // Close_pb
            // 
            Close_pb.BackColor = Color.Transparent;
            Close_pb.BackgroundImageLayout = ImageLayout.Center;
            Close_pb.Image = Client.Resources.Images.Cross_Base;
            Close_pb.Location = new Point(757, 12);
            Close_pb.Margin = new Padding(4, 3, 4, 3);
            Close_pb.Name = "Close_pb";
            Close_pb.Size = new Size(22, 26);
            Close_pb.TabIndex = 20;
            Close_pb.TabStop = false;
            Close_pb.Click += Close_pb_Click;
            Close_pb.MouseDown += Close_pb_MouseDown;
            Close_pb.MouseEnter += Close_pb_MouseEnter;
            Close_pb.MouseLeave += Close_pb_MouseLeave;
            Close_pb.MouseUp += Close_pb_MouseUp;
            // 
            // Config_pb
            // 
            Config_pb.BackColor = Color.Transparent;
            Config_pb.BackgroundImageLayout = ImageLayout.Center;
            Config_pb.Image = Client.Resources.Images.Config_Base;
            Config_pb.Location = new Point(732, 12);
            Config_pb.Margin = new Padding(4, 3, 4, 3);
            Config_pb.Name = "Config_pb";
            Config_pb.Size = new Size(22, 26);
            Config_pb.TabIndex = 32;
            Config_pb.TabStop = false;
            Config_pb.Click += Config_pb_Click;
            Config_pb.MouseDown += Config_pb_MouseDown;
            Config_pb.MouseEnter += Config_pb_MouseEnter;
            Config_pb.MouseLeave += Config_pb_MouseLeave;
            Config_pb.MouseUp += Config_pb_MouseUp;
            // 
            // Version_label
            // 
            Version_label.Anchor = AnchorStyles.Bottom;
            Version_label.BackColor = Color.Transparent;
            Version_label.Font = new Font("Calibri", 8.25F);
            Version_label.ForeColor = Color.Gray;
            Version_label.Location = new Point(648, 571);
            Version_label.Margin = new Padding(4, 0, 4, 0);
            Version_label.Name = "Version_label";
            Version_label.Size = new Size(143, 17);
            Version_label.TabIndex = 31;
            Version_label.Text = "Version 1.0.0.0";
            Version_label.TextAlign = ContentAlignment.TopRight;
            // 
            // CurrentFile_label
            // 
            CurrentFile_label.Anchor = AnchorStyles.Bottom;
            CurrentFile_label.BackColor = Color.Transparent;
            CurrentFile_label.Font = new Font("Calibri", 8.25F);
            CurrentFile_label.ForeColor = Color.Gray;
            CurrentFile_label.Location = new Point(62, 505);
            CurrentFile_label.Margin = new Padding(4, 0, 4, 0);
            CurrentFile_label.Name = "CurrentFile_label";
            CurrentFile_label.Size = new Size(422, 23);
            CurrentFile_label.TabIndex = 27;
            CurrentFile_label.Text = "Checking Files.";
            CurrentFile_label.TextAlign = ContentAlignment.MiddleLeft;
            CurrentFile_label.Visible = false;
            // 
            // CurrentPercent_label
            // 
            CurrentPercent_label.Anchor = AnchorStyles.Bottom;
            CurrentPercent_label.BackColor = Color.Transparent;
            CurrentPercent_label.Font = new Font("Calibri", 8.25F);
            CurrentPercent_label.ForeColor = Color.Gray;
            CurrentPercent_label.Location = new Point(619, 523);
            CurrentPercent_label.Margin = new Padding(4, 0, 4, 0);
            CurrentPercent_label.Name = "CurrentPercent_label";
            CurrentPercent_label.Size = new Size(41, 26);
            CurrentPercent_label.TabIndex = 28;
            CurrentPercent_label.Text = "100%";
            CurrentPercent_label.TextAlign = ContentAlignment.MiddleCenter;
            CurrentPercent_label.Visible = false;
            // 
            // TotalPercent_label
            // 
            TotalPercent_label.Anchor = AnchorStyles.Bottom;
            TotalPercent_label.BackColor = Color.Transparent;
            TotalPercent_label.Font = new Font("Calibri", 8.25F);
            TotalPercent_label.ForeColor = Color.Gray;
            TotalPercent_label.Location = new Point(619, 543);
            TotalPercent_label.Margin = new Padding(4, 0, 4, 0);
            TotalPercent_label.Name = "TotalPercent_label";
            TotalPercent_label.Size = new Size(41, 26);
            TotalPercent_label.TabIndex = 29;
            TotalPercent_label.Text = "100%";
            TotalPercent_label.TextAlign = ContentAlignment.MiddleCenter;
            TotalPercent_label.Visible = false;
            // 
            // Credit_label
            // 
            Credit_label.Anchor = AnchorStyles.Bottom;
            Credit_label.AutoSize = true;
            Credit_label.BackColor = Color.Transparent;
            Credit_label.Font = new Font("Calibri", 8.25F);
            Credit_label.ForeColor = Color.Gray;
            Credit_label.Location = new Point(13, 570);
            Credit_label.Margin = new Padding(4, 0, 4, 0);
            Credit_label.Name = "Credit_label";
            Credit_label.Size = new Size(114, 13);
            Credit_label.TabIndex = 30;
            Credit_label.Text = "Powered by Crystal M2";
            Credit_label.Click += Credit_label_Click;
            // 
            // ProgTotalEnd_pb
            // 
            ProgTotalEnd_pb.Anchor = AnchorStyles.None;
            ProgTotalEnd_pb.BackColor = Color.Transparent;
            ProgTotalEnd_pb.BackgroundImageLayout = ImageLayout.Center;
            ProgTotalEnd_pb.Image = Client.Resources.Images.NEW_Progress_End__Blue_;
            ProgTotalEnd_pb.Location = new Point(610, 550);
            ProgTotalEnd_pb.Margin = new Padding(4, 3, 4, 3);
            ProgTotalEnd_pb.Name = "ProgTotalEnd_pb";
            ProgTotalEnd_pb.Size = new Size(5, 19);
            ProgTotalEnd_pb.TabIndex = 26;
            ProgTotalEnd_pb.TabStop = false;
            // 
            // ProgEnd_pb
            // 
            ProgEnd_pb.Anchor = AnchorStyles.None;
            ProgEnd_pb.BackColor = Color.Transparent;
            ProgEnd_pb.BackgroundImageLayout = ImageLayout.Center;
            ProgEnd_pb.Image = Client.Resources.Images.NEW_Progress_End__Green_;
            ProgEnd_pb.Location = new Point(610, 530);
            ProgEnd_pb.Margin = new Padding(4, 3, 4, 3);
            ProgEnd_pb.Name = "ProgEnd_pb";
            ProgEnd_pb.Size = new Size(5, 19);
            ProgEnd_pb.TabIndex = 25;
            ProgEnd_pb.TabStop = false;
            ProgEnd_pb.Click += ProgEnd_pb_Click;
            // 
            // ProgressCurrent_pb
            // 
            ProgressCurrent_pb.Anchor = AnchorStyles.None;
            ProgressCurrent_pb.BackColor = Color.Transparent;
            ProgressCurrent_pb.BackgroundImageLayout = ImageLayout.Center;
            ProgressCurrent_pb.Image = Client.Resources.Images.Green_Progress;
            ProgressCurrent_pb.Location = new Point(60, 530);
            ProgressCurrent_pb.Margin = new Padding(4, 3, 4, 3);
            ProgressCurrent_pb.Name = "ProgressCurrent_pb";
            ProgressCurrent_pb.Size = new Size(557, 19);
            ProgressCurrent_pb.TabIndex = 23;
            ProgressCurrent_pb.TabStop = false;
            ProgressCurrent_pb.SizeChanged += ProgressCurrent_pb_SizeChanged;
            // 
            // TotalProg_pb
            // 
            TotalProg_pb.Anchor = AnchorStyles.None;
            TotalProg_pb.BackColor = Color.Transparent;
            TotalProg_pb.BackgroundImageLayout = ImageLayout.Center;
            TotalProg_pb.Image = Client.Resources.Images.Blue_Progress;
            TotalProg_pb.Location = new Point(60, 550);
            TotalProg_pb.Margin = new Padding(4, 3, 4, 3);
            TotalProg_pb.Name = "TotalProg_pb";
            TotalProg_pb.Size = new Size(558, 18);
            TotalProg_pb.TabIndex = 22;
            TotalProg_pb.TabStop = false;
            TotalProg_pb.SizeChanged += TotalProg_pb_SizeChanged;
            // 
            // Launch_pb
            // 
            Launch_pb.Anchor = AnchorStyles.Bottom;
            Launch_pb.BackColor = Color.Transparent;
            Launch_pb.BackgroundImageLayout = ImageLayout.Stretch;
            Launch_pb.Cursor = Cursors.Hand;
            Launch_pb.Image = Client.Resources.Images.Launch_Base1;
            Launch_pb.Location = new Point(663, 514);
            Launch_pb.Margin = new Padding(4, 3, 4, 3);
            Launch_pb.Name = "Launch_pb";
            Launch_pb.Size = new Size(117, 54);
            Launch_pb.TabIndex = 19;
            Launch_pb.TabStop = false;
            Launch_pb.Click += Launch_pb_Click;
            Launch_pb.MouseDown += Launch_pb_MouseDown;
            Launch_pb.MouseEnter += Launch_pb_MouseEnter;
            Launch_pb.MouseLeave += Launch_pb_MouseLeave;
            Launch_pb.MouseUp += Launch_pb_MouseUp;
            // 
            // Main_browser
            // 
            Main_browser.AllowExternalDrop = true;
            Main_browser.CausesValidation = false;
            Main_browser.CreationProperties = null;
            Main_browser.DefaultBackgroundColor = Color.White;
            Main_browser.Location = new Point(11, 54);
            Main_browser.Margin = new Padding(4, 3, 4, 3);
            Main_browser.MaximumSize = new Size(782, 457);
            Main_browser.Name = "Main_browser";
            Main_browser.Size = new Size(782, 445);
            Main_browser.TabIndex = 32;
            Main_browser.Visible = false;
            Main_browser.ZoomFactor = 1D;
            // 
            // AMain
            // 
            AutoScaleDimensions = new SizeF(7F, 17F);
            AutoScaleMode = AutoScaleMode.Font;
            BackColor = SystemColors.ActiveCaption;
            BackgroundImage = Client.Resources.Images.pfffft;
            BackgroundImageLayout = ImageLayout.Center;
            ClientSize = new Size(804, 632);
            Controls.Add(Main_browser);
            Controls.Add(SpeedLabel);
            Controls.Add(Credit_label);
            Controls.Add(Version_label);
            Controls.Add(TotalPercent_label);
            Controls.Add(CurrentPercent_label);
            Controls.Add(CurrentFile_label);
            Controls.Add(ProgTotalEnd_pb);
            Controls.Add(ProgEnd_pb);
            Controls.Add(ProgressCurrent_pb);
            Controls.Add(TotalProg_pb);
            Controls.Add(Launch_pb);
            Controls.Add(ActionLabel);
            Controls.Add(Movement_panel);
            DoubleBuffered = true;
            ForeColor = SystemColors.ControlText;
            FormBorderStyle = FormBorderStyle.None;
            Icon = (Icon)resources.GetObject("$this.Icon");
            Margin = new Padding(4, 3, 4, 3);
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "AMain";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Launcher";
            TransparencyKey = Color.Black;
            FormClosed += AMain_FormClosed;
            Load += AMain_Load;
            Click += AMain_Click;
            Movement_panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ((System.ComponentModel.ISupportInitialize)Close_pb).EndInit();
            ((System.ComponentModel.ISupportInitialize)Config_pb).EndInit();
            ((System.ComponentModel.ISupportInitialize)ProgTotalEnd_pb).EndInit();
            ((System.ComponentModel.ISupportInitialize)ProgEnd_pb).EndInit();
            ((System.ComponentModel.ISupportInitialize)ProgressCurrent_pb).EndInit();
            ((System.ComponentModel.ISupportInitialize)TotalProg_pb).EndInit();
            ((System.ComponentModel.ISupportInitialize)Launch_pb).EndInit();
            ((System.ComponentModel.ISupportInitialize)Main_browser).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label ActionLabel;
        private System.Windows.Forms.Label SpeedLabel;
        public System.Windows.Forms.Timer InterfaceTimer;
        public System.Windows.Forms.PictureBox Launch_pb;
        private System.Windows.Forms.PictureBox Close_pb;
        private System.Windows.Forms.Panel Movement_panel;
        private System.Windows.Forms.PictureBox TotalProg_pb;
        private System.Windows.Forms.PictureBox ProgressCurrent_pb;
        private System.Windows.Forms.Label Name_label;
        private System.Windows.Forms.PictureBox ProgEnd_pb;
        private System.Windows.Forms.PictureBox ProgTotalEnd_pb;
        private System.Windows.Forms.Label CurrentFile_label;
        private System.Windows.Forms.Label CurrentPercent_label;
        private System.Windows.Forms.Label TotalPercent_label;
        private System.Windows.Forms.Label Credit_label;
        private System.Windows.Forms.Label Version_label;
        private System.Windows.Forms.PictureBox Config_pb;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Microsoft.Web.WebView2.WinForms.WebView2 Main_browser;
    }
}

