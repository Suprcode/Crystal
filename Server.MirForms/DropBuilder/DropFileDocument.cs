using System.Text;

namespace Server.MirForms.DropBuilder
{
    /// <summary>
    /// One physical line of a drop file together with its original line terminator,
    /// so an untouched document can be written back byte for byte.
    /// </summary>
    public sealed class DropTextLine
    {
        public string Text { get; set; }
        public string Terminator { get; set; }

        public DropTextLine(string text, string terminator)
        {
            Text = text;
            Terminator = terminator;
        }
    }

    /// <summary>
    /// A ";Header" line plus every line that follows it until the next header.
    /// Headers the editor does not understand are kept verbatim.
    /// </summary>
    public sealed class DropSection
    {
        public DropTextLine HeaderLine { get; }
        public List<DropTextLine> Lines { get; } = new List<DropTextLine>();

        public DropSection(DropTextLine headerLine)
        {
            HeaderLine = headerLine;
        }

        public string RawHeader => HeaderLine.Text ?? string.Empty;

        /// <summary>Normalized header: no leading ';', trimmed, lower case.</summary>
        public string Header
        {
            get
            {
                var text = RawHeader.Trim();
                if (text.StartsWith(";")) text = text.Substring(1);
                text = string.Join(" ", text.Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries));
                return text.ToLowerInvariant();
            }
        }

        public bool IsGold => Header == "gold";

        public DropTextLine AddLine(string text, string terminator)
        {
            if (Lines.Count > 0)
            {
                var last = Lines[Lines.Count - 1];
                if (string.IsNullOrEmpty(last.Terminator)) last.Terminator = terminator;
            }

            var line = new DropTextLine(text, terminator);
            Lines.Add(line);
            return line;
        }
    }

    /// <summary>
    /// Lossless model of a drop file: preamble + ordered sections, every line keeps its
    /// own terminator. Rendering an unmodified document reproduces the original bytes;
    /// unknown sections, duplicate sections, several gold lines and comments all survive.
    /// </summary>
    public sealed class DropFileDocument
    {
        byte[] _originalBytes;
        bool _dirty;

        public string Path { get; private set; }
        public List<DropTextLine> Preamble { get; } = new List<DropTextLine>();
        public List<DropSection> Sections { get; } = new List<DropSection>();
        public string NewLine { get; private set; } = "\r\n";
        public Encoding TextEncoding { get; private set; } = new UTF8Encoding(false);
        public bool HasBom { get; private set; }
        public bool IsDirty => _dirty;

        public static DropFileDocument Load(string path)
        {
            var document = new DropFileDocument { Path = path };

            if (string.IsNullOrEmpty(path) || !File.Exists(path))
            {
                document._originalBytes = Array.Empty<byte>();
                document.NewLine = Environment.NewLine;
                return document;
            }

            var bytes = File.ReadAllBytes(path);
            document._originalBytes = bytes;

            bool hasBom;
            int bomLength;
            document.TextEncoding = DetectEncoding(bytes, out hasBom, out bomLength);
            document.HasBom = hasBom;

            var text = document.TextEncoding.GetString(bytes, bomLength, bytes.Length - bomLength);
            document.NewLine = DetectNewLine(text);
            document.ParseLines(SplitLines(text));

            return document;
        }

        public static DropFileDocument FromText(string text, string path, Encoding encoding = null, bool hasBom = false)
        {
            var document = new DropFileDocument { Path = path };
            text = text ?? string.Empty;

            document.TextEncoding = encoding ?? new UTF8Encoding(false);
            document.HasBom = hasBom;
            document.NewLine = DetectNewLine(text);
            document.ParseLines(SplitLines(text));
            document._dirty = true;

            return document;
        }

        public string Render()
        {
            var builder = new StringBuilder();

            foreach (var line in Preamble) AppendLine(builder, line);

            foreach (var section in Sections)
            {
                AppendLine(builder, section.HeaderLine);
                foreach (var line in section.Lines) AppendLine(builder, line);
            }

            return builder.ToString();
        }

        public void Save(string path = null)
        {
            path = path ?? Path;
            if (string.IsNullOrEmpty(path)) return;

            var bytes = (_dirty || _originalBytes == null) ? Encode(Render()) : _originalBytes;

            File.WriteAllBytes(path, bytes);

            Path = path;
            _originalBytes = bytes;
            _dirty = false;
        }

        public DropSection FindSection(params string[] aliases)
        {
            if (aliases == null || aliases.Length == 0) return null;

            foreach (var section in Sections)
                if (aliases.Contains(section.Header)) return section;

            return null;
        }

        public (string Odds, string Amount)? GetGold()
        {
            var section = Sections.FirstOrDefault(s => s.IsGold);
            if (section == null) return null;

            foreach (var line in section.Lines)
            {
                string odds, amount;
                if (TryParseGold(line.Text, out odds, out amount)) return (odds, amount);
            }

            return null;
        }

        /// <summary>Writes odds/amount to the first gold line of the gold section, creating it when missing.</summary>
        public void SetGold(string odds, string amount)
        {
            var section = Sections.FirstOrDefault(s => s.IsGold);

            if (section == null)
            {
                EnsureLastLineTerminated();
                var header = new DropTextLine(";Gold", NewLine);
                section = new DropSection(header);
                Sections.Insert(0, section);
                section.AddLine(string.Format("1/{0} Gold {1}", odds, amount), NewLine);
                section.AddLine(string.Empty, NewLine);
                _dirty = true;
                return;
            }

            foreach (var line in section.Lines)
            {
                string ignoredOdds, ignoredAmount;
                if (!TryParseGold(line.Text, out ignoredOdds, out ignoredAmount)) continue;

                line.Text = string.Format("1/{0} {1} {2}", odds, GoldKeyword(line.Text), amount);
                _dirty = true;
                return;
            }

            section.Lines.Insert(0, new DropTextLine(string.Format("1/{0} Gold {1}", odds, amount), NewLine));
            _dirty = true;
        }

        /// <summary>Removes every gold line, leaving the rest of the gold section untouched.</summary>
        public void RemoveGold()
        {
            var section = Sections.FirstOrDefault(s => s.IsGold);
            if (section == null) return;

            int removed = section.Lines.RemoveAll(line =>
            {
                string odds, amount;
                return TryParseGold(line.Text, out odds, out amount);
            });

            if (removed > 0) _dirty = true;
        }

        /// <summary>
        /// Appends one raw drop line to the first section matching any alias, creating the
        /// canonical section at the end of the file when none exists.
        /// </summary>
        public void AppendItem(string canonicalHeader, string[] aliases, string lineText)
        {
            var section = FindSection(aliases);

            if (section == null)
            {
                EnsureLastLineTerminated();
                section = new DropSection(new DropTextLine(canonicalHeader, NewLine));
                section.AddLine(string.Empty, NewLine);
                Sections.Add(section);
            }

            section.AddLine(lineText, NewLine);
            _dirty = true;
        }

        void EnsureLastLineTerminated()
        {
            DropTextLine last = null;

            if (Sections.Count > 0)
            {
                var section = Sections[Sections.Count - 1];
                last = section.Lines.Count > 0 ? section.Lines[section.Lines.Count - 1] : section.HeaderLine;
            }
            else if (Preamble.Count > 0)
            {
                last = Preamble[Preamble.Count - 1];
            }

            if (last != null && string.IsNullOrEmpty(last.Terminator)) last.Terminator = NewLine;
        }

        static string GoldKeyword(string text)
        {
            var parts = (text ?? string.Empty).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            return parts.Length > 1 ? parts[1] : "Gold";
        }

        public static bool TryParseGold(string text, out string odds, out string amount)
        {
            odds = null;
            amount = null;

            var parts = (text ?? string.Empty).Split(new[] { ' ', '\t' }, StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length < 3) return false;
            if (!parts[0].StartsWith("1/", StringComparison.Ordinal)) return false;
            if (!int.TryParse(parts[0].Substring(2), out _)) return false;
            if (!string.Equals(parts[1], "Gold", StringComparison.OrdinalIgnoreCase)) return false;
            if (!uint.TryParse(parts[2], out _)) return false;

            odds = parts[0].Substring(2);
            amount = parts[2];
            return true;
        }

        void ParseLines(IEnumerable<DropTextLine> lines)
        {
            DropSection current = null;

            foreach (var line in lines)
            {
                if (IsHeader(line.Text))
                {
                    current = new DropSection(line);
                    Sections.Add(current);
                }
                else if (current == null)
                {
                    Preamble.Add(line);
                }
                else
                {
                    current.Lines.Add(line);
                }
            }
        }

        static bool IsHeader(string text)
        {
            var value = (text ?? string.Empty).TrimStart();
            return value.StartsWith(";");
        }

        static List<DropTextLine> SplitLines(string text)
        {
            var lines = new List<DropTextLine>();

            int index = 0;
            while (index < text.Length)
            {
                int newline = text.IndexOf('\n', index);
                if (newline < 0)
                {
                    lines.Add(new DropTextLine(text.Substring(index), string.Empty));
                    break;
                }

                int end = newline;
                string terminator = "\n";
                if (end > index && text[end - 1] == '\r')
                {
                    end--;
                    terminator = "\r\n";
                }

                lines.Add(new DropTextLine(text.Substring(index, end - index), terminator));
                index = newline + 1;
            }

            return lines;
        }

        static string DetectNewLine(string text)
        {
            return text != null && text.IndexOf("\r\n", StringComparison.Ordinal) >= 0 ? "\r\n" : "\n";
        }

        static void AppendLine(StringBuilder builder, DropTextLine line)
        {
            builder.Append(line.Text ?? string.Empty);
            builder.Append(line.Terminator ?? string.Empty);
        }

        byte[] Encode(string text)
        {
            var body = TextEncoding.GetBytes(text);
            if (!HasBom) return body;

            var preamble = TextEncoding.GetPreamble();
            if (preamble.Length == 0) return body;

            var result = new byte[preamble.Length + body.Length];
            Buffer.BlockCopy(preamble, 0, result, 0, preamble.Length);
            Buffer.BlockCopy(body, 0, result, preamble.Length, body.Length);
            return result;
        }

        static Encoding DetectEncoding(byte[] bytes, out bool hasBom, out int bomLength)
        {
            hasBom = true;
            bomLength = 3;

            if (bytes.Length >= 3 && bytes[0] == 0xEF && bytes[1] == 0xBB && bytes[2] == 0xBF)
                return new UTF8Encoding(true);

            if (bytes.Length >= 4 && bytes[0] == 0xFF && bytes[1] == 0xFE && bytes[2] == 0x00 && bytes[3] == 0x00)
            {
                bomLength = 4;
                return new UTF32Encoding(false, true);
            }

            if (bytes.Length >= 4 && bytes[0] == 0x00 && bytes[1] == 0x00 && bytes[2] == 0xFE && bytes[3] == 0xFF)
            {
                bomLength = 4;
                return new UTF32Encoding(true, true);
            }

            if (bytes.Length >= 2 && bytes[0] == 0xFF && bytes[1] == 0xFE)
            {
                bomLength = 2;
                return new UnicodeEncoding(false, true);
            }

            if (bytes.Length >= 2 && bytes[0] == 0xFE && bytes[1] == 0xFF)
            {
                bomLength = 2;
                return new UnicodeEncoding(true, true);
            }

            hasBom = false;
            bomLength = 0;

            try
            {
                new UTF8Encoding(false, true).GetString(bytes);
                return new UTF8Encoding(false);
            }
            catch (DecoderFallbackException)
            {
                // Legacy encoding (typical for Chinese Mir data): read it, but never lose bytes.
                try
                {
                    return Encoding.GetEncoding(54936);
                }
                catch (Exception)
                {
                    return Encoding.Latin1;
                }
            }
        }
    }
}
