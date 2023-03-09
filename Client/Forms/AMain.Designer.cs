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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AMain));
            this.ActionLabel = new System.Windows.Forms.Label();
            this.SpeedLabel = new System.Windows.Forms.Label();
            this.InterfaceTimer = new System.Windows.Forms.Timer(this.components);
            this.Movement_panel = new System.Windows.Forms.Panel();
            this.Name_label = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.Close_pb = new System.Windows.Forms.PictureBox();
            this.Config_pb = new System.Windows.Forms.PictureBox();
            this.Version_label = new System.Windows.Forms.Label();
            this.CurrentFile_label = new System.Windows.Forms.Label();
            this.CurrentPercent_label = new System.Windows.Forms.Label();
            this.TotalPercent_label = new System.Windows.Forms.Label();
            this.Credit_label = new System.Windows.Forms.Label();
            this.ProgTotalEnd_pb = new System.Windows.Forms.PictureBox();
            this.ProgEnd_pb = new System.Windows.Forms.PictureBox();
            this.ProgressCurrent_pb = new System.Windows.Forms.PictureBox();
            this.TotalProg_pb = new System.Windows.Forms.PictureBox();
            this.Launch_pb = new System.Windows.Forms.PictureBox();
            this.Main_browser = new Microsoft.Web.WebView2.WinForms.WebView2();
            this.Movement_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Close_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Config_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgTotalEnd_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgEnd_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressCurrent_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalProg_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Launch_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Main_browser)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionLabel
            // 
            this.ActionLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.ActionLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActionLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.ActionLabel.ForeColor = System.Drawing.Color.Gray;
            this.ActionLabel.Location = new System.Drawing.Point(478, 470);
            this.ActionLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.ActionLabel.Name = "ActionLabel";
            this.ActionLabel.Size = new System.Drawing.Size(126, 21);
            this.ActionLabel.TabIndex = 4;
            this.ActionLabel.Text = "1423MB/2000MB";
            this.ActionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ActionLabel.Visible = false;
            this.ActionLabel.Click += new System.EventHandler(this.ActionLabel_Click);
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.SpeedLabel.BackColor = System.Drawing.Color.Transparent;
            this.SpeedLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.SpeedLabel.ForeColor = System.Drawing.Color.Gray;
            this.SpeedLabel.Location = new System.Drawing.Point(341, 474);
            this.SpeedLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SpeedLabel.Size = new System.Drawing.Size(83, 18);
            this.SpeedLabel.TabIndex = 13;
            this.SpeedLabel.Text = "Speed";
            this.SpeedLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.SpeedLabel.Visible = false;
            // 
            // InterfaceTimer
            // 
            this.InterfaceTimer.Enabled = true;
            this.InterfaceTimer.Interval = 50;
            this.InterfaceTimer.Tick += new System.EventHandler(this.InterfaceTimer_Tick);
            // 
            // Movement_panel
            // 
            this.Movement_panel.BackColor = System.Drawing.Color.Transparent;
            this.Movement_panel.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Movement_panel.Controls.Add(this.Name_label);
            this.Movement_panel.Controls.Add(this.pictureBox1);
            this.Movement_panel.Controls.Add(this.Close_pb);
            this.Movement_panel.Controls.Add(this.Config_pb);
            this.Movement_panel.Location = new System.Drawing.Point(14, 6);
            this.Movement_panel.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Movement_panel.Name = "Movement_panel";
            this.Movement_panel.Size = new System.Drawing.Size(922, 43);
            this.Movement_panel.TabIndex = 21;
            this.Movement_panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Movement_panel_MouseClick);
            this.Movement_panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Movement_panel_MouseClick);
            this.Movement_panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Movement_panel_MouseMove);
            this.Movement_panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Movement_panel_MouseUp);
            // 
            // Name_label
            // 
            this.Name_label.BackColor = System.Drawing.Color.Transparent;
            this.Name_label.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Name_label.ForeColor = System.Drawing.Color.White;
            this.Name_label.Image = global::Client.Resources.Images.server_base;
            this.Name_label.Location = new System.Drawing.Point(289, 11);
            this.Name_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Name_label.Name = "Name_label";
            this.Name_label.Size = new System.Drawing.Size(217, 25);
            this.Name_label.TabIndex = 0;
            this.Name_label.Text = "Crystal Mir 2";
            this.Name_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Name_label.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Client.Resources.Images.server_base;
            this.pictureBox1.Location = new System.Drawing.Point(358, -46);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(217, 23);
            this.pictureBox1.TabIndex = 33;
            this.pictureBox1.TabStop = false;
            // 
            // Close_pb
            // 
            this.Close_pb.BackColor = System.Drawing.Color.Transparent;
            this.Close_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Close_pb.Image = global::Client.Resources.Images.Cross_Base;
            this.Close_pb.Location = new System.Drawing.Point(757, 11);
            this.Close_pb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Close_pb.Name = "Close_pb";
            this.Close_pb.Size = new System.Drawing.Size(22, 23);
            this.Close_pb.TabIndex = 20;
            this.Close_pb.TabStop = false;
            this.Close_pb.Click += new System.EventHandler(this.Close_pb_Click);
            this.Close_pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Close_pb_MouseDown);
            this.Close_pb.MouseEnter += new System.EventHandler(this.Close_pb_MouseEnter);
            this.Close_pb.MouseLeave += new System.EventHandler(this.Close_pb_MouseLeave);
            this.Close_pb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Close_pb_MouseUp);
            // 
            // Config_pb
            // 
            this.Config_pb.BackColor = System.Drawing.Color.Transparent;
            this.Config_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Config_pb.Image = global::Client.Resources.Images.Config_Base;
            this.Config_pb.Location = new System.Drawing.Point(732, 11);
            this.Config_pb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Config_pb.Name = "Config_pb";
            this.Config_pb.Size = new System.Drawing.Size(22, 23);
            this.Config_pb.TabIndex = 32;
            this.Config_pb.TabStop = false;
            this.Config_pb.Click += new System.EventHandler(this.Config_pb_Click);
            this.Config_pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Config_pb_MouseDown);
            this.Config_pb.MouseEnter += new System.EventHandler(this.Config_pb_MouseEnter);
            this.Config_pb.MouseLeave += new System.EventHandler(this.Config_pb_MouseLeave);
            this.Config_pb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Config_pb_MouseUp);
            // 
            // Version_label
            // 
            this.Version_label.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Version_label.BackColor = System.Drawing.Color.Transparent;
            this.Version_label.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Version_label.ForeColor = System.Drawing.Color.Gray;
            this.Version_label.Location = new System.Drawing.Point(648, 534);
            this.Version_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Version_label.Name = "Version_label";
            this.Version_label.Size = new System.Drawing.Size(143, 15);
            this.Version_label.TabIndex = 31;
            this.Version_label.Text = "Version 1.0.0.0";
            this.Version_label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // CurrentFile_label
            // 
            this.CurrentFile_label.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CurrentFile_label.BackColor = System.Drawing.Color.Transparent;
            this.CurrentFile_label.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CurrentFile_label.ForeColor = System.Drawing.Color.Gray;
            this.CurrentFile_label.Location = new System.Drawing.Point(62, 470);
            this.CurrentFile_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CurrentFile_label.Name = "CurrentFile_label";
            this.CurrentFile_label.Size = new System.Drawing.Size(422, 20);
            this.CurrentFile_label.TabIndex = 27;
            this.CurrentFile_label.Text = "Checking Files.";
            this.CurrentFile_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CurrentFile_label.Visible = false;
            // 
            // CurrentPercent_label
            // 
            this.CurrentPercent_label.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.CurrentPercent_label.BackColor = System.Drawing.Color.Transparent;
            this.CurrentPercent_label.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.CurrentPercent_label.ForeColor = System.Drawing.Color.Gray;
            this.CurrentPercent_label.Location = new System.Drawing.Point(619, 488);
            this.CurrentPercent_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.CurrentPercent_label.Name = "CurrentPercent_label";
            this.CurrentPercent_label.Size = new System.Drawing.Size(41, 23);
            this.CurrentPercent_label.TabIndex = 28;
            this.CurrentPercent_label.Text = "100%";
            this.CurrentPercent_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CurrentPercent_label.Visible = false;
            // 
            // TotalPercent_label
            // 
            this.TotalPercent_label.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.TotalPercent_label.BackColor = System.Drawing.Color.Transparent;
            this.TotalPercent_label.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.TotalPercent_label.ForeColor = System.Drawing.Color.Gray;
            this.TotalPercent_label.Location = new System.Drawing.Point(619, 509);
            this.TotalPercent_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.TotalPercent_label.Name = "TotalPercent_label";
            this.TotalPercent_label.Size = new System.Drawing.Size(41, 23);
            this.TotalPercent_label.TabIndex = 29;
            this.TotalPercent_label.Text = "100%";
            this.TotalPercent_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TotalPercent_label.Visible = false;
            // 
            // Credit_label
            // 
            this.Credit_label.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Credit_label.AutoSize = true;
            this.Credit_label.BackColor = System.Drawing.Color.Transparent;
            this.Credit_label.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.Credit_label.ForeColor = System.Drawing.Color.Gray;
            this.Credit_label.Location = new System.Drawing.Point(13, 534);
            this.Credit_label.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.Credit_label.Name = "Credit_label";
            this.Credit_label.Size = new System.Drawing.Size(114, 13);
            this.Credit_label.TabIndex = 30;
            this.Credit_label.Text = "Powered by Crystal M2";
            this.Credit_label.Click += new System.EventHandler(this.Credit_label_Click);
            // 
            // ProgTotalEnd_pb
            // 
            this.ProgTotalEnd_pb.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ProgTotalEnd_pb.BackColor = System.Drawing.Color.Transparent;
            this.ProgTotalEnd_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ProgTotalEnd_pb.Image = global::Client.Resources.Images.NEW_Progress_End__Blue_;
            this.ProgTotalEnd_pb.Location = new System.Drawing.Point(599, 529);
            this.ProgTotalEnd_pb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ProgTotalEnd_pb.Name = "ProgTotalEnd_pb";
            this.ProgTotalEnd_pb.Size = new System.Drawing.Size(5, 17);
            this.ProgTotalEnd_pb.TabIndex = 26;
            this.ProgTotalEnd_pb.TabStop = false;
            // 
            // ProgEnd_pb
            // 
            this.ProgEnd_pb.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ProgEnd_pb.BackColor = System.Drawing.Color.Transparent;
            this.ProgEnd_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ProgEnd_pb.Image = global::Client.Resources.Images.NEW_Progress_End__Green_;
            this.ProgEnd_pb.Location = new System.Drawing.Point(535, 529);
            this.ProgEnd_pb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ProgEnd_pb.Name = "ProgEnd_pb";
            this.ProgEnd_pb.Size = new System.Drawing.Size(5, 17);
            this.ProgEnd_pb.TabIndex = 25;
            this.ProgEnd_pb.TabStop = false;
            // 
            // ProgressCurrent_pb
            // 
            this.ProgressCurrent_pb.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.ProgressCurrent_pb.BackColor = System.Drawing.Color.Transparent;
            this.ProgressCurrent_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ProgressCurrent_pb.Image = global::Client.Resources.Images.Green_Progress;
            this.ProgressCurrent_pb.Location = new System.Drawing.Point(60, 494);
            this.ProgressCurrent_pb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.ProgressCurrent_pb.Name = "ProgressCurrent_pb";
            this.ProgressCurrent_pb.Size = new System.Drawing.Size(557, 17);
            this.ProgressCurrent_pb.TabIndex = 23;
            this.ProgressCurrent_pb.TabStop = false;
            this.ProgressCurrent_pb.SizeChanged += new System.EventHandler(this.ProgressCurrent_pb_SizeChanged);
            // 
            // TotalProg_pb
            // 
            this.TotalProg_pb.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.TotalProg_pb.BackColor = System.Drawing.Color.Transparent;
            this.TotalProg_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.TotalProg_pb.Image = global::Client.Resources.Images.Blue_Progress;
            this.TotalProg_pb.Location = new System.Drawing.Point(60, 511);
            this.TotalProg_pb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.TotalProg_pb.Name = "TotalProg_pb";
            this.TotalProg_pb.Size = new System.Drawing.Size(558, 16);
            this.TotalProg_pb.TabIndex = 22;
            this.TotalProg_pb.TabStop = false;
            this.TotalProg_pb.SizeChanged += new System.EventHandler(this.TotalProg_pb_SizeChanged);
            // 
            // Launch_pb
            // 
            this.Launch_pb.Anchor = System.Windows.Forms.AnchorStyles.Bottom;
            this.Launch_pb.BackColor = System.Drawing.Color.Transparent;
            this.Launch_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Launch_pb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Launch_pb.Image = global::Client.Resources.Images.Launch_Base1;
            this.Launch_pb.Location = new System.Drawing.Point(660, 476);
            this.Launch_pb.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Launch_pb.Name = "Launch_pb";
            this.Launch_pb.Size = new System.Drawing.Size(131, 56);
            this.Launch_pb.TabIndex = 19;
            this.Launch_pb.TabStop = false;
            this.Launch_pb.Click += new System.EventHandler(this.Launch_pb_Click);
            this.Launch_pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launch_pb_MouseDown);
            this.Launch_pb.MouseEnter += new System.EventHandler(this.Launch_pb_MouseEnter);
            this.Launch_pb.MouseLeave += new System.EventHandler(this.Launch_pb_MouseLeave);
            this.Launch_pb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Launch_pb_MouseUp);
            // 
            // Main_browser
            // 
            this.Main_browser.AllowExternalDrop = true;
            this.Main_browser.CausesValidation = false;
            this.Main_browser.CreationProperties = null;
            this.Main_browser.DefaultBackgroundColor = System.Drawing.Color.White;
            this.Main_browser.Location = new System.Drawing.Point(11, 53);
            this.Main_browser.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.Main_browser.MaximumSize = new System.Drawing.Size(782, 403);
            this.Main_browser.Name = "Main_browser";
            this.Main_browser.Size = new System.Drawing.Size(782, 403);
            this.Main_browser.TabIndex = 32;
            this.Main_browser.Visible = false;
            this.Main_browser.ZoomFactor = 1D;
            // 
            // AMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = global::Client.Resources.Images.pfffft;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(804, 558);
            this.Controls.Add(this.Main_browser);
            this.Controls.Add(this.SpeedLabel);
            this.Controls.Add(this.Credit_label);
            this.Controls.Add(this.Version_label);
            this.Controls.Add(this.TotalPercent_label);
            this.Controls.Add(this.CurrentPercent_label);
            this.Controls.Add(this.CurrentFile_label);
            this.Controls.Add(this.ProgTotalEnd_pb);
            this.Controls.Add(this.ProgEnd_pb);
            this.Controls.Add(this.ProgressCurrent_pb);
            this.Controls.Add(this.TotalProg_pb);
            this.Controls.Add(this.Launch_pb);
            this.Controls.Add(this.ActionLabel);
            this.Controls.Add(this.Movement_panel);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(4, 3, 4, 3);
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "AMain";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Launcher";
            this.TransparencyKey = System.Drawing.Color.Black;
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AMain_FormClosed);
            this.Load += new System.EventHandler(this.AMain_Load);
            this.Click += new System.EventHandler(this.AMain_Click);
            this.Movement_panel.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Close_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Config_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgTotalEnd_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgEnd_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressCurrent_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalProg_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Launch_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Main_browser)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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

