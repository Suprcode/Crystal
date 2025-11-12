using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Server.Network
{
    partial class BlockListForm
    {
        private IContainer components = null;
        private ListBox tbList;
        private TextBox tbIPCIDRinput;
        private Button btnAdd;
        private Button btnRemove;
        private Button btnImport;
        private Button btnExport;
        private Button btnClose;
        private Label lblFormat;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            tbList = new ListBox();
            tbIPCIDRinput = new TextBox();
            btnAdd = new Button();
            btnRemove = new Button();
            btnImport = new Button();
            btnExport = new Button();
            btnClose = new Button();
            lblFormat = new Label();
            SuspendLayout();
            // 
            // tbList
            // 
            tbList.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbList.FormattingEnabled = true;
            tbList.IntegralHeight = false;
            tbList.ItemHeight = 15;
            tbList.Location = new Point(12, 12);
            tbList.Name = "tbList";
            tbList.SelectionMode = SelectionMode.MultiExtended;
            tbList.Size = new Size(396, 300);
            tbList.TabIndex = 7;
            // 
            // tbIPCIDRinput
            // 
            tbIPCIDRinput.Anchor = AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            tbIPCIDRinput.Location = new Point(12, 323);
            tbIPCIDRinput.Name = "tbIPCIDRinput";
            tbIPCIDRinput.Size = new Size(288, 23);
            tbIPCIDRinput.TabIndex = 6;
            // 
            // btnAdd
            // 
            btnAdd.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnAdd.Location = new Point(306, 322);
            btnAdd.Name = "btnAdd";
            btnAdd.Size = new Size(102, 25);
            btnAdd.TabIndex = 5;
            btnAdd.Text = "Add";
            btnAdd.UseVisualStyleBackColor = true;
            // 
            // btnRemove
            // 
            btnRemove.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnRemove.Location = new Point(12, 354);
            btnRemove.Name = "btnRemove";
            btnRemove.Size = new Size(90, 25);
            btnRemove.TabIndex = 4;
            btnRemove.Text = "Remove";
            btnRemove.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            btnImport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnImport.Location = new Point(108, 354);
            btnImport.Name = "btnImport";
            btnImport.Size = new Size(90, 25);
            btnImport.TabIndex = 3;
            btnImport.Text = "Import";
            btnImport.UseVisualStyleBackColor = true;
            // 
            // btnExport
            // 
            btnExport.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            btnExport.Location = new Point(204, 354);
            btnExport.Name = "btnExport";
            btnExport.Size = new Size(90, 25);
            btnExport.TabIndex = 2;
            btnExport.Text = "Export";
            btnExport.UseVisualStyleBackColor = true;
            // 
            // btnClose
            // 
            btnClose.Anchor = AnchorStyles.Bottom | AnchorStyles.Right;
            btnClose.Location = new Point(318, 354);
            btnClose.Name = "btnClose";
            btnClose.Size = new Size(90, 25);
            btnClose.TabIndex = 1;
            btnClose.Text = "Close";
            btnClose.UseVisualStyleBackColor = true;
            // 
            // lblFormat
            // 
            lblFormat.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;
            lblFormat.AutoSize = true;
            lblFormat.Location = new Point(12, 385);
            lblFormat.Name = "lblFormat";
            lblFormat.Size = new Size(249, 15);
            lblFormat.TabIndex = 0;
            lblFormat.Text = "Format: 36.0.8.0/21 or single IP (e.g., 10.10.0.1)";
            // 
            // BlockListForm
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(420, 410);
            Controls.Add(lblFormat);
            Controls.Add(btnClose);
            Controls.Add(btnExport);
            Controls.Add(btnImport);
            Controls.Add(btnRemove);
            Controls.Add(btnAdd);
            Controls.Add(tbIPCIDRinput);
            Controls.Add(tbList);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "BlockListForm";
            SizeGripStyle = SizeGripStyle.Hide;
            StartPosition = FormStartPosition.CenterParent;
            Text = "View/Edit IP Block List";
            ResumeLayout(false);
            PerformLayout();
        }
    }
}

