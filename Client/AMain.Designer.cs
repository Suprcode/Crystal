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
            this.Config_pb = new System.Windows.Forms.PictureBox();
            this.Close_pb = new System.Windows.Forms.PictureBox();
            this.Version_label = new System.Windows.Forms.Label();
            this.Main_browser = new System.Windows.Forms.WebBrowser();
            this.CurrentFile_label = new System.Windows.Forms.Label();
            this.CurrentPercent_label = new System.Windows.Forms.Label();
            this.TotalPercent_label = new System.Windows.Forms.Label();
            this.Credit_label = new System.Windows.Forms.Label();
            this.ProgTotalEnd_pb = new System.Windows.Forms.PictureBox();
            this.ProgEnd_pb = new System.Windows.Forms.PictureBox();
            this.ProgressCurrent_pb = new System.Windows.Forms.PictureBox();
            this.TotalProg_pb = new System.Windows.Forms.PictureBox();
            this.Launch_pb = new System.Windows.Forms.PictureBox();
            this.Movement_panel.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Config_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Close_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgTotalEnd_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgEnd_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressCurrent_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalProg_pb)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Launch_pb)).BeginInit();
            this.SuspendLayout();
            // 
            // ActionLabel
            // 
            this.ActionLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.ActionLabel.BackColor = System.Drawing.Color.Transparent;
            this.ActionLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ActionLabel.ForeColor = System.Drawing.Color.Gray;
            this.ActionLabel.Location = new System.Drawing.Point(439, 472);
            this.ActionLabel.Name = "ActionLabel";
            this.ActionLabel.Size = new System.Drawing.Size(173, 18);
            this.ActionLabel.TabIndex = 4;
            this.ActionLabel.Text = "1423MB/2000MB";
            this.ActionLabel.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.ActionLabel.Visible = false;
            this.ActionLabel.Click += new System.EventHandler(this.ActionLabel_Click);
            // 
            // SpeedLabel
            // 
            this.SpeedLabel.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.SpeedLabel.BackColor = System.Drawing.Color.Transparent;
            this.SpeedLabel.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.SpeedLabel.ForeColor = System.Drawing.Color.Gray;
            this.SpeedLabel.Location = new System.Drawing.Point(347, 523);
            this.SpeedLabel.Name = "SpeedLabel";
            this.SpeedLabel.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.SpeedLabel.Size = new System.Drawing.Size(265, 16);
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
            this.Movement_panel.Controls.Add(this.Config_pb);
            this.Movement_panel.Controls.Add(this.Close_pb);
            this.Movement_panel.Location = new System.Drawing.Point(5, 7);
            this.Movement_panel.Name = "Movement_panel";
            this.Movement_panel.Size = new System.Drawing.Size(790, 37);
            this.Movement_panel.TabIndex = 21;
            this.Movement_panel.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Movement_panel_MouseClick);
            this.Movement_panel.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Movement_panel_MouseClick);
            this.Movement_panel.MouseMove += new System.Windows.Forms.MouseEventHandler(this.Movement_panel_MouseMove);
            this.Movement_panel.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Movement_panel_MouseUp);
            // 
            // Name_label
            // 
            this.Name_label.BackColor = System.Drawing.Color.Transparent;
            this.Name_label.Font = new System.Drawing.Font("OCR A Std", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Name_label.ForeColor = System.Drawing.Color.White;
            this.Name_label.Image = global::Client.Properties.Resources.server_base;
            this.Name_label.Location = new System.Drawing.Point(307, 8);
            this.Name_label.Name = "Name_label";
            this.Name_label.Size = new System.Drawing.Size(186, 22);
            this.Name_label.TabIndex = 0;
            this.Name_label.Text = "Crystal Mir 2";
            this.Name_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.Name_label.Visible = false;
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::Client.Properties.Resources.server_base;
            this.pictureBox1.Location = new System.Drawing.Point(307, -40);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(186, 19);
            this.pictureBox1.TabIndex = 33;
            this.pictureBox1.TabStop = false;
            // 
            // Config_pb
            // 
            this.Config_pb.BackColor = System.Drawing.Color.Transparent;
            this.Config_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Config_pb.Image = global::Client.Properties.Resources.Config_Base;
            this.Config_pb.Location = new System.Drawing.Point(739, 6);
            this.Config_pb.Name = "Config_pb";
            this.Config_pb.Size = new System.Drawing.Size(19, 19);
            this.Config_pb.TabIndex = 32;
            this.Config_pb.TabStop = false;
            this.Config_pb.Click += new System.EventHandler(this.Config_pb_Click);
            this.Config_pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Config_pb_MouseDown);
            this.Config_pb.MouseEnter += new System.EventHandler(this.Config_pb_MouseEnter);
            this.Config_pb.MouseLeave += new System.EventHandler(this.Config_pb_MouseLeave);
            this.Config_pb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Config_pb_MouseUp);
            // 
            // Close_pb
            // 
            this.Close_pb.BackColor = System.Drawing.Color.Transparent;
            this.Close_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.Close_pb.Image = global::Client.Properties.Resources.Cross_Base;
            this.Close_pb.Location = new System.Drawing.Point(763, 6);
            this.Close_pb.Name = "Close_pb";
            this.Close_pb.Size = new System.Drawing.Size(19, 19);
            this.Close_pb.TabIndex = 20;
            this.Close_pb.TabStop = false;
            this.Close_pb.Click += new System.EventHandler(this.Close_pb_Click);
            this.Close_pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Close_pb_MouseDown);
            this.Close_pb.MouseEnter += new System.EventHandler(this.Close_pb_MouseEnter);
            this.Close_pb.MouseLeave += new System.EventHandler(this.Close_pb_MouseLeave);
            this.Close_pb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Close_pb_MouseUp);
            // 
            // Version_label
            // 
            this.Version_label.BackColor = System.Drawing.Color.Transparent;
            this.Version_label.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Version_label.ForeColor = System.Drawing.Color.Gray;
            this.Version_label.Location = new System.Drawing.Point(672, 530);
            this.Version_label.Name = "Version_label";
            this.Version_label.Size = new System.Drawing.Size(120, 13);
            this.Version_label.TabIndex = 31;
            this.Version_label.Text = "Version 1.0.0.0";
            this.Version_label.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Main_browser
            // 
            this.Main_browser.AllowWebBrowserDrop = false;
            this.Main_browser.IsWebBrowserContextMenuEnabled = false;
            this.Main_browser.Location = new System.Drawing.Point(8, 46);
            this.Main_browser.MinimumSize = new System.Drawing.Size(20, 20);
            this.Main_browser.Name = "Main_browser";
            this.Main_browser.ScriptErrorsSuppressed = true;
            this.Main_browser.ScrollBarsEnabled = false;
            this.Main_browser.Size = new System.Drawing.Size(784, 411);
            this.Main_browser.TabIndex = 24;
            this.Main_browser.Url = new System.Uri("", System.UriKind.Relative);
            this.Main_browser.Visible = false;
            this.Main_browser.WebBrowserShortcutsEnabled = false;
            this.Main_browser.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.Main_browser_DocumentCompleted);
            // 
            // CurrentFile_label
            // 
            this.CurrentFile_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CurrentFile_label.BackColor = System.Drawing.Color.Transparent;
            this.CurrentFile_label.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentFile_label.ForeColor = System.Drawing.Color.Gray;
            this.CurrentFile_label.Location = new System.Drawing.Point(59, 468);
            this.CurrentFile_label.Name = "CurrentFile_label";
            this.CurrentFile_label.Size = new System.Drawing.Size(362, 17);
            this.CurrentFile_label.TabIndex = 27;
            this.CurrentFile_label.Text = "Up to date.";
            this.CurrentFile_label.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.CurrentFile_label.Visible = false;
            // 
            // CurrentPercent_label
            // 
            this.CurrentPercent_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.CurrentPercent_label.BackColor = System.Drawing.Color.Transparent;
            this.CurrentPercent_label.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CurrentPercent_label.ForeColor = System.Drawing.Color.Gray;
            this.CurrentPercent_label.Location = new System.Drawing.Point(616, 486);
            this.CurrentPercent_label.Name = "CurrentPercent_label";
            this.CurrentPercent_label.Size = new System.Drawing.Size(35, 19);
            this.CurrentPercent_label.TabIndex = 28;
            this.CurrentPercent_label.Text = "100%";
            this.CurrentPercent_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.CurrentPercent_label.Visible = false;
            // 
            // TotalPercent_label
            // 
            this.TotalPercent_label.Anchor = System.Windows.Forms.AnchorStyles.Right;
            this.TotalPercent_label.BackColor = System.Drawing.Color.Transparent;
            this.TotalPercent_label.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.TotalPercent_label.ForeColor = System.Drawing.Color.Gray;
            this.TotalPercent_label.Location = new System.Drawing.Point(616, 504);
            this.TotalPercent_label.Name = "TotalPercent_label";
            this.TotalPercent_label.Size = new System.Drawing.Size(35, 19);
            this.TotalPercent_label.TabIndex = 29;
            this.TotalPercent_label.Text = "100%";
            this.TotalPercent_label.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.TotalPercent_label.Visible = false;
            // 
            // Credit_label
            // 
            this.Credit_label.AutoSize = true;
            this.Credit_label.BackColor = System.Drawing.Color.Transparent;
            this.Credit_label.Font = new System.Drawing.Font("Calibri", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Credit_label.ForeColor = System.Drawing.Color.Gray;
            this.Credit_label.Location = new System.Drawing.Point(9, 529);
            this.Credit_label.Name = "Credit_label";
            this.Credit_label.Size = new System.Drawing.Size(114, 13);
            this.Credit_label.TabIndex = 30;
            this.Credit_label.Text = "Powered by Crystal M2";
            this.Credit_label.Click += new System.EventHandler(this.Credit_label_Click);
            // 
            // ProgTotalEnd_pb
            // 
            this.ProgTotalEnd_pb.BackColor = System.Drawing.Color.Transparent;
            this.ProgTotalEnd_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ProgTotalEnd_pb.Image = global::Client.Properties.Resources.NEW_Progress_End__Blue_;
            this.ProgTotalEnd_pb.Location = new System.Drawing.Point(608, 508);
            this.ProgTotalEnd_pb.Name = "ProgTotalEnd_pb";
            this.ProgTotalEnd_pb.Size = new System.Drawing.Size(4, 15);
            this.ProgTotalEnd_pb.TabIndex = 26;
            this.ProgTotalEnd_pb.TabStop = false;
            // 
            // ProgEnd_pb
            // 
            this.ProgEnd_pb.BackColor = System.Drawing.Color.Transparent;
            this.ProgEnd_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ProgEnd_pb.Image = global::Client.Properties.Resources.NEW_Progress_End__Green_;
            this.ProgEnd_pb.Location = new System.Drawing.Point(608, 490);
            this.ProgEnd_pb.Name = "ProgEnd_pb";
            this.ProgEnd_pb.Size = new System.Drawing.Size(4, 15);
            this.ProgEnd_pb.TabIndex = 25;
            this.ProgEnd_pb.TabStop = false;
            // 
            // ProgressCurrent_pb
            // 
            this.ProgressCurrent_pb.BackColor = System.Drawing.Color.Transparent;
            this.ProgressCurrent_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ProgressCurrent_pb.Image = global::Client.Properties.Resources.Green_Progress;
            this.ProgressCurrent_pb.Location = new System.Drawing.Point(58, 490);
            this.ProgressCurrent_pb.Name = "ProgressCurrent_pb";
            this.ProgressCurrent_pb.Size = new System.Drawing.Size(550, 15);
            this.ProgressCurrent_pb.TabIndex = 23;
            this.ProgressCurrent_pb.TabStop = false;
            this.ProgressCurrent_pb.SizeChanged += new System.EventHandler(this.ProgressCurrent_pb_SizeChanged);
            // 
            // TotalProg_pb
            // 
            this.TotalProg_pb.BackColor = System.Drawing.Color.Transparent;
            this.TotalProg_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.TotalProg_pb.Image = global::Client.Properties.Resources.Blue_Progress;
            this.TotalProg_pb.Location = new System.Drawing.Point(58, 508);
            this.TotalProg_pb.Name = "TotalProg_pb";
            this.TotalProg_pb.Size = new System.Drawing.Size(550, 14);
            this.TotalProg_pb.TabIndex = 22;
            this.TotalProg_pb.TabStop = false;
            this.TotalProg_pb.SizeChanged += new System.EventHandler(this.TotalProg_pb_SizeChanged);
            // 
            // Launch_pb
            // 
            this.Launch_pb.BackColor = System.Drawing.Color.Transparent;
            this.Launch_pb.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Launch_pb.Cursor = System.Windows.Forms.Cursors.Hand;
            this.Launch_pb.Image = global::Client.Properties.Resources.Launch_Base1;
            this.Launch_pb.Location = new System.Drawing.Point(658, 472);
            this.Launch_pb.Name = "Launch_pb";
            this.Launch_pb.Size = new System.Drawing.Size(116, 54);
            this.Launch_pb.TabIndex = 19;
            this.Launch_pb.TabStop = false;
            this.Launch_pb.Click += new System.EventHandler(this.Launch_pb_Click);
            this.Launch_pb.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Launch_pb_MouseDown);
            this.Launch_pb.MouseEnter += new System.EventHandler(this.Launch_pb_MouseEnter);
            this.Launch_pb.MouseLeave += new System.EventHandler(this.Launch_pb_MouseLeave);
            this.Launch_pb.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Launch_pb_MouseUp);
            // 
            // AMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.BackgroundImage = global::Client.Properties.Resources.pfffft;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(800, 548);
            this.Controls.Add(this.Credit_label);
            this.Controls.Add(this.Version_label);
            this.Controls.Add(this.TotalPercent_label);
            this.Controls.Add(this.CurrentPercent_label);
            this.Controls.Add(this.CurrentFile_label);
            this.Controls.Add(this.ProgTotalEnd_pb);
            this.Controls.Add(this.ProgEnd_pb);
            this.Controls.Add(this.Main_browser);
            this.Controls.Add(this.ProgressCurrent_pb);
            this.Controls.Add(this.TotalProg_pb);
            this.Controls.Add(this.Launch_pb);
            this.Controls.Add(this.SpeedLabel);
            this.Controls.Add(this.ActionLabel);
            this.Controls.Add(this.Movement_panel);
            this.DoubleBuffered = true;
            this.ForeColor = System.Drawing.SystemColors.ControlText;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
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
            ((System.ComponentModel.ISupportInitialize)(this.Config_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Close_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgTotalEnd_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgEnd_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.ProgressCurrent_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.TotalProg_pb)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Launch_pb)).EndInit();
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
        private System.Windows.Forms.WebBrowser Main_browser;
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
    }
}

