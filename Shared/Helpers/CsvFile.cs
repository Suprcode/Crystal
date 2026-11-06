using System.Text;

namespace Shared.Helpers
{
    public static class CsvFile
    {
        private static readonly char[] QuoteChars = { ',', '"', '\r', '\n' };

        public static string[][] ReadAllRows(string fileName)
        {
            using var reader = new StreamReader(fileName, Encoding.UTF8, detectEncodingFromByteOrderMarks: true);
            var rows = ReadRows(reader);
            for (int i = 1; i < rows.Length; i++)
            {
                if (!string.IsNullOrWhiteSpace(rows[i][0]) && rows[i].Length != rows[0].Length)
                    throw new FormatException($"Row {i} column count does not match the headers column count.");
            }
            return rows;
        }

        public static void WriteAllRows(string fileName, IEnumerable<string[]> rows)
        {
            using var writer = new StreamWriter(fileName, false, Encoding.UTF8);
            foreach (var row in rows)
                writer.WriteLine(FormatRow(row));
        }

        public static string[] ParseRow(string text)
        {
            using var reader = new StringReader(text);
            var rows = ReadRows(reader);
            if (rows.Length > 1)
                throw new FormatException("Expected a single CSV record.");
            return rows.Length == 0 ? Array.Empty<string>() : rows[0];
        }

        public static string FormatRow(IEnumerable<string> cells)
        {
            return string.Join(",", cells.Select(value =>
            {
                value ??= string.Empty;
                return value.IndexOfAny(QuoteChars) < 0 ? value : "\"" + value.Replace("\"", "\"\"") + "\"";
            }));
        }

        private static string[][] ReadRows(TextReader reader)
        {
            var rows = new List<string[]>();
            var cells = new List<string>();
            var field = new StringBuilder();
            bool inQuotes = false;
            bool quoteClosed = false;
            bool rowStarted = false;

            void EndRow()
            {
                cells.Add(field.ToString());
                rows.Add(cells.ToArray());
                cells.Clear();
                field.Clear();
                rowStarted = false;
                quoteClosed = false;
            }

            int next;
            while ((next = reader.Read()) != -1)
            {
                char c = (char)next;
                if (inQuotes)
                {
                    if (c == '"')
                    {
                        if (reader.Peek() == '"')
                        {
                            field.Append('"');
                            reader.Read();
                        }
                        else
                        {
                            inQuotes = false;
                            quoteClosed = true;
                        }
                    }
                    else
                    {
                        field.Append(c);
                    }
                    continue;
                }

                if (c == ',')
                {
                    cells.Add(field.ToString());
                    field.Clear();
                    quoteClosed = false;
                    rowStarted = true;
                }
                else if (c == '\r' || c == '\n')
                {
                    if (c == '\r' && reader.Peek() == '\n') reader.Read();
                    if (rowStarted) EndRow();
                }
                else
                {
                    if (quoteClosed || (c == '"' && field.Length > 0))
                        throw new FormatException($"Invalid CSV quoting in row {rows.Count}.");

                    if (c == '"') inQuotes = true;
                    else field.Append(c);
                    rowStarted = true;
                }
            }

            if (inQuotes)
                throw new FormatException($"Invalid CSV quoting in row {rows.Count}.");
            if (rowStarted) EndRow();
            return rows.ToArray();
        }
    }
}
