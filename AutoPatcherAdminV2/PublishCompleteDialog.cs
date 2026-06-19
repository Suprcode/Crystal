using System.Drawing;
using System.Windows.Forms;

namespace AutoPatcherAdmin
{
    internal sealed class PublishCompleteDialog : Form
    {
        internal PublishCompleteDialog(
            int filesUploaded, int added, int changed, int removed,
            long origBytes, long compBytes, bool showCompression,
            string timeElapsed)
        {
            SuspendLayout();

            Text = "Publish Complete";
            FormBorderStyle = FormBorderStyle.FixedDialog;
            MaximizeBox = false;
            MinimizeBox = false;
            StartPosition = FormStartPosition.CenterParent;
            ShowInTaskbar = false;
            BackColor = Color.White;
            Font = new Font("Segoe UI", 9f);
            AutoScaleMode = AutoScaleMode.None;

            // Header
            var iconBox = new PictureBox
            {
                Image = SystemIcons.Information.ToBitmap(),
                SizeMode = PictureBoxSizeMode.StretchImage,
                Size = new Size(28, 28),
                Location = new Point(16, 15),
                BackColor = Color.Transparent
            };

            var headerLabel = new Label
            {
                Text = "Publish Complete",
                Font = new Font("Segoe UI", 11f, FontStyle.Bold),
                AutoSize = true,
                Location = new Point(52, 18),
                ForeColor = Color.FromArgb(30, 30, 30),
                BackColor = Color.Transparent
            };

            // Row counts
            const int labelW  = 148;
            const int valueW  = 176;
            const int rowH    = 22;

            int rowCount = 3; // Files Uploaded, Added, Changed
            if (removed > 0) rowCount++;
            rowCount += 1;                             // spacer
            rowCount += showCompression ? 2 : 1;      // size section
            rowCount += 1;                             // spacer
            rowCount += 1;                             // Time Taken

            var table = new TableLayoutPanel
            {
                Location = new Point(16, 66),
                Size = new Size(labelW + valueW, rowCount * rowH),
                ColumnCount = 2,
                RowCount = rowCount,
                CellBorderStyle = TableLayoutPanelCellBorderStyle.None,
                AutoSize = false
            };
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, labelW));
            table.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, valueW));
            for (int i = 0; i < rowCount; i++)
                table.RowStyles.Add(new RowStyle(SizeType.Absolute, rowH));

            int r = 0;
            void AddRow(string label, string value,
                bool boldLabel = false, bool boldValue = false,
                Color? labelFore = null, Color? valueFore = null)
            {
                table.Controls.Add(new Label
                {
                    Text = label,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleLeft,
                    Font = boldLabel ? new Font("Segoe UI", 9f, FontStyle.Bold) : Font,
                    ForeColor = labelFore ?? Color.FromArgb(80, 80, 80),
                    Margin = new Padding(0)
                }, 0, r);
                table.Controls.Add(new Label
                {
                    Text = value,
                    Dock = DockStyle.Fill,
                    TextAlign = ContentAlignment.MiddleRight,
                    Font = boldValue ? new Font("Segoe UI", 9f, FontStyle.Bold) : Font,
                    ForeColor = valueFore ?? Color.FromArgb(30, 30, 30),
                    Margin = new Padding(0, 0, 2, 0)
                }, 1, r);
                r++;
            }
            void AddSpacer()
            {
                table.Controls.Add(new Label { Dock = DockStyle.Fill, Margin = new Padding(0) }, 0, r);
                table.Controls.Add(new Label { Dock = DockStyle.Fill, Margin = new Padding(0) }, 1, r);
                r++;
            }

            // Files section
            AddRow("Files Uploaded", $"{filesUploaded:N0}",
                boldLabel: true, boldValue: true,
                valueFore: Color.FromArgb(0, 128, 0));
            AddRow("  Added",   $"{added:N0}");
            AddRow("  Changed", $"{changed:N0}");
            if (removed > 0)
                AddRow("  Removed", $"{removed:N0}");

            AddSpacer();

            // Size section
            if (showCompression && origBytes > 0)
            {
                long saved = origBytes - compBytes;
                int pct = (int)((double)saved / origBytes * 100);
                AddRow("Original Size",  FormatBytes(origBytes));
                AddRow("Compressed To",  $"{FormatBytes(compBytes)}  ({pct}% saved)",
                    valueFore: Color.FromArgb(0, 128, 0));
            }
            else
            {
                AddRow("Upload Size", FormatBytes(origBytes > 0 ? origBytes : compBytes));
            }

            AddSpacer();
            AddRow("Time Taken", timeElapsed, boldLabel: true, boldValue: true);

            // Layout sizing
            const int padding    = 16;
            int formWidth        = labelW + valueW + padding * 2;
            int tableBottom      = table.Bottom + padding;

            var sep1 = new Panel { BackColor = Color.FromArgb(210, 210, 210), Location = new Point(0, 56), Size = new Size(formWidth, 1) };
            var sep2 = new Panel { BackColor = Color.FromArgb(210, 210, 210), Location = new Point(0, tableBottom), Size = new Size(formWidth, 1) };

            var okButton = new Button
            {
                Text = "OK",
                DialogResult = DialogResult.OK,
                Size = new Size(80, 26),
                FlatStyle = FlatStyle.System,
                Location = new Point(formWidth - 80 - 12, 9)
            };
            var footer = new Panel
            {
                BackColor = Color.FromArgb(245, 245, 245),
                Location = new Point(0, tableBottom + 1),
                Size = new Size(formWidth, 44)
            };
            footer.Controls.Add(okButton);

            ClientSize = new Size(formWidth, tableBottom + 1 + 44);
            AcceptButton = okButton;

            Controls.AddRange(new Control[] { iconBox, headerLabel, sep1, table, sep2, footer });

            ResumeLayout(true);
        }

        private static string FormatBytes(long bytes)
        {
            if (bytes >= 1L << 30) return $"{bytes / (double)(1L << 30):0.##} GB";
            if (bytes >= 1L << 20) return $"{bytes / (double)(1L << 20):0.##} MB";
            if (bytes >= 1L << 10) return $"{bytes / (double)(1L << 10):0.##} KB";
            return $"{bytes} B";
        }
    }
}
