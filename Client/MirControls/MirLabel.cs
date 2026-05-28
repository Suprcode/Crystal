#if !FNA
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using SlimDX;
using SlimDX.Direct3D9;
#else
using FontStashSharp;
#endif
using Client.MirGraphics;
using Font = System.Drawing.Font;

namespace Client.MirControls
{
    public class MirLabel : MirControl
    {
        #region Auto Size
        private bool _autoSize;
        public bool AutoSize
        {
            get { return _autoSize; }
            set
            {
                if (_autoSize == value)
                    return;
                _autoSize = value;
                OnAutoSizeChanged(EventArgs.Empty);
            }
        }
        public event EventHandler AutoSizeChanged;
        private void OnAutoSizeChanged(EventArgs e)
        {
            TextureValid = false;
            GetSize();
            if (AutoSizeChanged != null)
                AutoSizeChanged.Invoke(this, e);
        }
        #endregion

        #region DrawFormat
        private TextFormatFlags _drawFormat;
        public TextFormatFlags DrawFormat
        {
            get { return _drawFormat; }
            set
            {
                _drawFormat = value;
                OnDrawFormatChanged(EventArgs.Empty);
            }
        }
        public event EventHandler DrawFormatChanged;
        private void OnDrawFormatChanged(EventArgs e)
        {
            TextureValid = false;

            if (DrawFormatChanged != null)
                DrawFormatChanged.Invoke(this, e);
        }
        #endregion

        #region Font
        private Font _font;
        public Font Font
        {
            get { return _font; }
            set
            {
                _font = ScaleFont(value);
                OnFontChanged(EventArgs.Empty);
            }
        }
        public event EventHandler FontChanged;
        private void OnFontChanged(EventArgs e)
        {
            TextureValid = false;

            GetSize();

            if (FontChanged != null)
                FontChanged.Invoke(this, e);
        }
        #endregion

        #region Out Line
        private bool _outLine;
        public bool OutLine
        {
            get { return _outLine; }
            set
            {
                if (_outLine == value)
                    return;
                _outLine = value;
                OnOutLineChanged(EventArgs.Empty);
            }
        }
        public event EventHandler OutLineChanged;
        private void OnOutLineChanged(EventArgs e)
        {
            TextureValid = false;
            GetSize();
            
            if (OutLineChanged != null)
                OutLineChanged.Invoke(this, e);
        }
        #endregion

        #region Out Line Colour
        private Color _outLineColour;
        public Color OutLineColour
        {
            get { return _outLineColour; }
            set
            {
                if (_outLineColour == value)
                    return;
                _outLineColour = value;
                OnOutLineColourChanged();
            }
        }
        public event EventHandler OutLineColourChanged;
        private void OnOutLineColourChanged()
        {
            TextureValid = false;

            if (OutLineColourChanged != null)
                OutLineColourChanged.Invoke(this, EventArgs.Empty);
        }
        #endregion

        #region Size

        private void GetSize()
        {
            if (!AutoSize)
                return;

            if (string.IsNullOrEmpty(_text))
                Size = Size.Empty;
            else
            {
                Size = TextRenderer.MeasureText(CMain.Graphics, Text, Font);
                //Size = new Size(Size.Width, Size.Height + 5);

                if (OutLine && Size != Size.Empty)
                    Size = new Size(Size.Width + 2, Size.Height + 2);
            }
        }
        #endregion

        #region Label
        private string _text;
#if FNA
        private string _wrappedText;
#endif
        public string Text
        {
            get { return _text; }
            set
            {
                if (_text == value)
                    return;

                _text = value;
                OnTextChanged(EventArgs.Empty);
            }
        }
        public event EventHandler TextChanged;
        private void OnTextChanged(EventArgs e)
        {
            DrawControlTexture = !string.IsNullOrEmpty(Text);
            TextureValid = false;
            Redraw();

            GetSize();

            if (TextChanged != null)
                TextChanged.Invoke(this, e);
        }
        #endregion

        public MirLabel()
        {
            DrawControlTexture = true;
            _drawFormat = TextFormatFlags.WordBreak;

            _font = ScaleFont(new Font(Settings.FontName, 8F));
            _outLine = true;
            _outLineColour = Color.Black; 
            _text = string.Empty;

        }
        
        protected override unsafe void CreateTexture()
        {
            if (string.IsNullOrEmpty(Text))
                return;

            if (Size.Width == 0 || Size.Height == 0)
                return;

            if (TextureSize != Size)
                DisposeTexture();

#if !FNA
            if (ControlTexture == null || ControlTexture.Disposed)
            {
                DXManager.ControlList.Add(this);

                ControlTexture = new Texture(DXManager.Device, Size.Width, Size.Height, 1, Usage.None, Format.A8R8G8B8, Pool.Managed);
                TextureSize = Size;
            }

            DataRectangle stream = ControlTexture.LockRectangle(0, LockFlags.Discard);
            using (Bitmap image = new Bitmap(Size.Width, Size.Height, Size.Width * 4, PixelFormat.Format32bppArgb, stream.Data.DataPointer))
            {
                using (Graphics graphics = Graphics.FromImage(image))
                {
                    graphics.SmoothingMode = SmoothingMode.AntiAlias;
                    graphics.TextRenderingHint = TextRenderingHint.AntiAliasGridFit;
                    graphics.CompositingQuality = CompositingQuality.HighQuality;
                    graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
                    graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                    graphics.TextContrast = 0;
                    graphics.Clear(BackColour);


                    if (OutLine)
                    {
                        TextRenderer.DrawText(graphics, Text, Font, new Rectangle(1, 0, Size.Width, Size.Height), OutLineColour, DrawFormat);
                        TextRenderer.DrawText(graphics, Text, Font, new Rectangle(0, 1, Size.Width, Size.Height), OutLineColour, DrawFormat);
                        TextRenderer.DrawText(graphics, Text, Font, new Rectangle(2, 1, Size.Width, Size.Height), OutLineColour, DrawFormat);
                        TextRenderer.DrawText(graphics, Text, Font, new Rectangle(1, 2, Size.Width, Size.Height), OutLineColour, DrawFormat);
                        TextRenderer.DrawText(graphics, Text, Font, new Rectangle(1, 1, Size.Width, Size.Height), ForeColour, DrawFormat);

                        //LinearGradientBrush brush = new LinearGradientBrush(new Rectangle(0, 0, this.Size.Width, this.Size.Height), Color.FromArgb(239, 243, 239), Color.White, LinearGradientMode.Vertical);
                        ////graphics.DrawString(Text, Font, brush, 37, 9);
                        ////graphics.DrawString(this.Text, this.Font, new SolidBrush(Color.Black), 39, 9, StringFormat.GenericDefault);
                    }
                    else
                        TextRenderer.DrawText(graphics, Text, Font, new Rectangle(1, 0, Size.Width, Size.Height), ForeColour, DrawFormat);
                }
            }

            ControlTexture.UnlockRectangle(0);
            DXManager.Sprite.Flush();
            TextureValid = true;
#else
            var font = Client.Platform.FNA.FNAFontManager.GetFont(Font.Size);
            float singleLineHeight = font.MeasureString("A").Y;

            if (!AutoSize && Size.Width > 0 && Size.Height > 0 && !string.IsNullOrEmpty(Text))
            {
                if ((DrawFormat & TextFormatFlags.WordBreak) == TextFormatFlags.WordBreak && Size.Height >= singleLineHeight * 1.5f)
                {
                    _wrappedText = WrapText(font, Text, Size.Width);
                }
                else
                {
                    string[] lines = Text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
                    for (int i = 0; i < lines.Length; i++)
                    {
                        lines[i] = TruncateText(font, lines[i], Size.Width);
                    }
                    _wrappedText = string.Join("\n", lines);
                }

                if (singleLineHeight > 0)
                {
                    string[] wrappedLines = _wrappedText.Split('\n');
                    int maxLines = (int)(Size.Height / singleLineHeight);
                    if (maxLines < wrappedLines.Length)
                    {
                        if (maxLines < 1) maxLines = 1;
                        if (maxLines < wrappedLines.Length)
                        {
                            string[] truncatedLines = new string[maxLines];
                            Array.Copy(wrappedLines, truncatedLines, maxLines);
                            _wrappedText = string.Join("\n", truncatedLines);
                        }
                    }
                }
            }
            else
            {
                _wrappedText = Text;
            }
            TextureValid = true;
#endif
        }

#if FNA
        protected internal override void DrawControl()
        {
            if (string.IsNullOrEmpty(Text))
                return;

            var renderer = DXManager.Renderer as Client.Platform.FNA.FNARenderer;
            if (renderer == null)
                return;

            if (!TextureValid)
                CreateTexture();

            if (BackColour.A > 0 && Size.Width > 0 && Size.Height > 0)
            {
                renderer.DrawRectangle(new Rectangle(DisplayLocation.X, DisplayLocation.Y, Size.Width, Size.Height), BackColour, Opacity);
            }

            var font = Client.Platform.FNA.FNAFontManager.GetFont(Font.Size);
            var xnaForeCol = new Microsoft.Xna.Framework.Color(ForeColour.R, ForeColour.G, ForeColour.B, ForeColour.A) * Opacity;

            string drawText = _wrappedText ?? Text;
            string[] lines = drawText.Replace("\r\n", "\n").Split('\n');

            float singleLineHeight = font.MeasureString("A").Y;
            float lineSpacing = font.MeasureString("A\nA").Y - singleLineHeight;
            if (lineSpacing <= 0) lineSpacing = singleLineHeight;

            float totalHeight = font.MeasureString(drawText).Y;
            float startY = DisplayLocation.Y;

            if ((DrawFormat & TextFormatFlags.VerticalCenter) == TextFormatFlags.VerticalCenter)
            {
                startY += (Size.Height - totalHeight) / 2f;
            }
            else if ((DrawFormat & TextFormatFlags.Bottom) == TextFormatFlags.Bottom)
            {
                startY += Size.Height - totalHeight;
            }

            for (int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                var lineSize = font.MeasureString(line);
                float lineX = DisplayLocation.X;

                if ((DrawFormat & TextFormatFlags.HorizontalCenter) == TextFormatFlags.HorizontalCenter)
                {
                    lineX += (Size.Width - lineSize.X) / 2f;
                }
                else if ((DrawFormat & TextFormatFlags.Right) == TextFormatFlags.Right)
                {
                    lineX += Size.Width - lineSize.X;
                }

                var linePos = new Microsoft.Xna.Framework.Vector2(lineX, startY + i * lineSpacing);

                if (OutLine)
                {
                    var xnaOutCol = new Microsoft.Xna.Framework.Color(OutLineColour.R, OutLineColour.G, OutLineColour.B, OutLineColour.A) * Opacity;
                    
                    renderer.SpriteBatch.DrawString(font, line, linePos + new Microsoft.Xna.Framework.Vector2(1, 0), xnaOutCol);
                    renderer.SpriteBatch.DrawString(font, line, linePos + new Microsoft.Xna.Framework.Vector2(0, 1), xnaOutCol);
                    renderer.SpriteBatch.DrawString(font, line, linePos + new Microsoft.Xna.Framework.Vector2(2, 1), xnaOutCol);
                    renderer.SpriteBatch.DrawString(font, line, linePos + new Microsoft.Xna.Framework.Vector2(1, 2), xnaOutCol);
                    renderer.SpriteBatch.DrawString(font, line, linePos + new Microsoft.Xna.Framework.Vector2(1, 1), xnaForeCol);
                }
                else
                {
                    renderer.SpriteBatch.DrawString(font, line, linePos + new Microsoft.Xna.Framework.Vector2(1, 0), xnaForeCol);
                }
            }

            CleanTime = CMain.Time + Settings.CleanDelay;
        }
#endif

        #region Disposable
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (!disposing) return;
            
            AutoSizeChanged = null;
            _autoSize = false;

            DrawFormatChanged = null;
            _drawFormat = 0;

            FontChanged = null;
            if (_font != null)
            {
                _font.Dispose();
                _font = null;
            }

            OutLineChanged = null;
            _outLine = false;

            OutLineColourChanged = null;
            _outLineColour = Color.Empty;

            TextChanged = null;
            _text = null;
        }
        #endregion

#if FNA
        private string TruncateText(FontStashSharp.SpriteFontBase font, string text, float maxWidth)
        {
            if (maxWidth <= 0 || string.IsNullOrEmpty(text))
                return text;

            if (font.MeasureString(text).X <= maxWidth)
                return text;

            int low = 1;
            int high = text.Length;
            int bestLength = 0;

            while (low <= high)
            {
                int mid = (low + high) / 2;
                string sub = text.Substring(0, mid);
                if (font.MeasureString(sub).X <= maxWidth)
                {
                    bestLength = mid;
                    low = mid + 1;
                }
                else
                {
                    high = mid - 1;
                }
            }

            return text.Substring(0, bestLength);
        }

        private string WrapText(FontStashSharp.SpriteFontBase font, string text, float maxWidth)
        {
            if (maxWidth <= 0)
                return text;

            string[] lines = text.Split(new[] { "\r\n", "\n" }, StringSplitOptions.None);
            List<string> wrappedLines = new List<string>();

            foreach (string line in lines)
            {
                if (font.MeasureString(line).X <= maxWidth)
                {
                    wrappedLines.Add(line);
                    continue;
                }

                System.Text.StringBuilder currentLine = new System.Text.StringBuilder();
                string lastWord = "";

                for (int i = 0; i < line.Length; i++)
                {
                    char c = line[i];
                    bool isCJK = (c >= 0x4E00 && c <= 0x9FFF) || 
                                 (c >= 0x3400 && c <= 0x4DBF) || 
                                 (c >= 0x3000 && c <= 0x303F) || 
                                 (c >= 0x3040 && c <= 0x309F) || 
                                 (c >= 0x30A0 && c <= 0x30FF) || 
                                 (c >= 0xFF00 && c <= 0xFFEF);

                    if (isCJK || char.IsWhiteSpace(c))
                    {
                        if (lastWord.Length > 0)
                        {
                            string test = currentLine.ToString() + lastWord;
                            if (font.MeasureString(test).X > maxWidth)
                            {
                                if (currentLine.Length > 0)
                                {
                                    wrappedLines.Add(currentLine.ToString().TrimEnd());
                                    currentLine.Clear();
                                }
                            }
                            currentLine.Append(lastWord);
                            lastWord = "";
                        }

                        string testChar = currentLine.ToString() + c;
                        if (font.MeasureString(testChar).X > maxWidth)
                        {
                            if (currentLine.Length > 0)
                            {
                                wrappedLines.Add(currentLine.ToString().TrimEnd());
                                currentLine.Clear();
                            }
                            if (!char.IsWhiteSpace(c))
                            {
                                currentLine.Append(c);
                            }
                        }
                        else
                        {
                            currentLine.Append(c);
                        }
                    }
                    else
                    {
                        lastWord += c;
                    }
                }

                if (lastWord.Length > 0)
                {
                    string test = currentLine.ToString() + lastWord;
                    if (font.MeasureString(test).X > maxWidth)
                    {
                        if (currentLine.Length > 0)
                        {
                            wrappedLines.Add(currentLine.ToString().TrimEnd());
                            currentLine.Clear();
                        }
                    }
                    currentLine.Append(lastWord);
                }

                if (currentLine.Length > 0)
                {
                    wrappedLines.Add(currentLine.ToString().TrimEnd());
                }
            }

            return string.Join("\n", wrappedLines);
        }
#endif

    }
}
