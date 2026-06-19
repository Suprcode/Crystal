using System.Drawing;
using System.Windows.Forms;

namespace AutoPatcherAdmin
{
    internal sealed class DgvProgressBarCell : DataGridViewTextBoxCell
    {
        public override Type ValueType => typeof(int);
        public override object DefaultNewRowValue => 0;

        protected override void Paint(
            Graphics g, Rectangle clipBounds, Rectangle cellBounds,
            int rowIndex, DataGridViewElementStates elementState,
            object? value, object? formattedValue, string? errorText,
            DataGridViewCellStyle cellStyle,
            DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            // Draw background + border, skip default text rendering
            base.Paint(g, clipBounds, cellBounds, rowIndex, elementState, value, formattedValue,
                errorText, cellStyle, advancedBorderStyle,
                paintParts & ~DataGridViewPaintParts.ContentForeground);

            int pct = value is int p ? Math.Clamp(p, 0, 100) : 0;

            const int pad = 5;
            const int barH = 7;
            int barY = cellBounds.Y + (cellBounds.Height - barH) / 2;
            int barX = cellBounds.X + pad;
            int barW = cellBounds.Width - pad * 2;

            if (barW <= 0) return;

            // Track — semi-transparent dark overlay so it's readable on any row colour
            using (var trackBrush = new SolidBrush(Color.FromArgb(55, 0, 0, 0)))
                g.FillRectangle(trackBrush, barX, barY, barW, barH);

            // Fill — white with enough alpha to be clearly visible on all status colours
            if (pct > 0)
            {
                int fillW = Math.Max(1, (int)(barW * pct / 100.0));
                using var fillBrush = new SolidBrush(Color.FromArgb(180, 255, 255, 255));
                g.FillRectangle(fillBrush, barX, barY, fillW, barH);
            }

            // Percentage text
            if (pct > 0)
            {
                using var sf = new StringFormat
                {
                    Alignment = StringAlignment.Center,
                    LineAlignment = StringAlignment.Center,
                    Trimming = StringTrimming.None
                };
                using var textBrush = new SolidBrush(cellStyle.ForeColor);
                g.DrawString($"{pct}%", cellStyle.Font ?? SystemFonts.DefaultFont, textBrush, (RectangleF)cellBounds, sf);
            }
        }
    }

    internal sealed class DgvProgressBarColumn : DataGridViewColumn
    {
        public DgvProgressBarColumn() : base(new DgvProgressBarCell())
        {
            HeaderText = "Progress";
            Width = 80;
            MinimumWidth = 50;
            ReadOnly = true;
            SortMode = DataGridViewColumnSortMode.Automatic;
        }
    }
}
