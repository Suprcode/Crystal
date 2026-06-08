using System;
using System.Drawing;
using System.Collections.Generic;

namespace Client.Platform.FNA
{
    public static class FNATextRenderer
    {
        public static Size MeasureText(Graphics g, string text, Font font)
        {
            if (string.IsNullOrEmpty(text))
                return Size.Empty;

            var spriteFont = FNAFontManager.GetFont(font.Size);
            var size = spriteFont.MeasureString(text);
            return new Size((int)Math.Ceiling(size.X), (int)Math.Ceiling(size.Y));
        }

        public static Size MeasureText(Graphics g, string text, Font font, Size proposedSize, System.Windows.Forms.TextFormatFlags flags)
        {
            if (string.IsNullOrEmpty(text))
                return Size.Empty;

            if ((flags & System.Windows.Forms.TextFormatFlags.WordBreak) == System.Windows.Forms.TextFormatFlags.WordBreak && proposedSize.Width > 0)
            {
                var spriteFont = FNAFontManager.GetFont(font.Size);
                float singleLineHeight = spriteFont.MeasureString("A").Y;

                if (proposedSize.Height <= 0 || proposedSize.Height >= singleLineHeight * 1.5f)
                {
                    string wrappedText = WrapText(spriteFont, text, proposedSize.Width);
                    var size = spriteFont.MeasureString(wrappedText);
                    return new Size((int)Math.Ceiling(size.X), (int)Math.Ceiling(size.Y));
                }
            }

            return MeasureText(g, text, font);
        }

        private static string WrapText(FontStashSharp.SpriteFontBase font, string text, float maxWidth)
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
    }
}
