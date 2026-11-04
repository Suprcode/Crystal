using System.Text.Encodings.Web;
using System.Text.Json;

/// <summary>
/// A single localizable string. <see cref="Source"/> is the English text collected at runtime,
/// <see cref="Text"/> is the translation. An empty <see cref="Text"/> means the entry has not
/// been translated yet and the source string is displayed instead.
/// </summary>
public sealed class LocalizationEntry
{
    public string Source { get; set; } = string.Empty;
    public string Text { get; set; } = string.Empty;
}

/// <summary>
/// Shared translation table. Keys are either the English template, a stable id (dynamic text,
/// enum values) or a control path prefixed with "ui.". Missing keys are appended on first use
/// and written back to disk shortly after, so the table maintains itself.
/// </summary>
public sealed class LocalizationStore
{
    private const string MetaKey = "__meta";
    private const string SourceKey = "Source";
    private const string TextKey = "Text";
    private const string FormsKey = "forms";
    private const int SaveDelay = 2000;

    private readonly object _sync = new object();
    private readonly List<string> _order = new List<string>();
    private readonly Dictionary<string, LocalizationEntry> _entries = new Dictionary<string, LocalizationEntry>(StringComparer.Ordinal);
    private readonly Dictionary<string, string> _bySource = new Dictionary<string, string>(StringComparer.Ordinal);

    private Timer _saveTimer;
    private volatile bool _dirty;

    public string FilePath { get; private set; }
    public HashSet<string> Forms { get; } = new HashSet<string>(StringComparer.Ordinal);
    public int Count { get { lock (_sync) return _entries.Count; } }

    public int UntranslatedCount
    {
        get
        {
            lock (_sync)
            {
                var count = 0;
                foreach (var entry in _entries.Values)
                {
                    if (string.IsNullOrEmpty(entry.Text)) count++;
                }
                return count;
            }
        }
    }

    /// <summary>
    /// Reads the table from disk. A missing or corrupted file leaves an empty table.
    /// When <paramref name="saveChanges"/> is false the table is read-only: runtime additions stay
    /// in memory and are never written back.
    /// </summary>
    public void Load(string path, bool saveChanges = true)
    {
        FilePath = saveChanges ? path : null;

        {
            lock (_sync)
            {
                _order.Clear();
                _entries.Clear();
                _bySource.Clear();
            }
        }

        if (!File.Exists(path)) return;

        try
        {
            using (var document = JsonDocument.Parse(File.ReadAllText(path)))
            {
                var root = document.RootElement;

                if (root.TryGetProperty(MetaKey, out var meta) && meta.TryGetProperty(FormsKey, out var forms))
                {
                    foreach (var form in forms.EnumerateArray())
                    {
                        var name = form.GetString();
                        if (!string.IsNullOrEmpty(name)) Forms.Add(name);
                    }
                }

                foreach (var property in root.EnumerateObject())
                {
                    if (property.Name == MetaKey) continue;

                    var entry = new LocalizationEntry();

                    if (property.Value.ValueKind == JsonValueKind.Object)
                    {
                        if (property.Value.TryGetProperty(SourceKey, out var source)) entry.Source = source.GetString() ?? string.Empty;
                        if (property.Value.TryGetProperty(TextKey, out var text)) entry.Text = text.GetString() ?? string.Empty;
                    }
                    else if (property.Value.ValueKind == JsonValueKind.String)
                    {
                        entry.Source = property.Value.GetString() ?? string.Empty;
                    }

                    lock (_sync)
                    {
                        _entries[property.Name] = entry;
                        _order.Add(property.Name);

                        if (!string.IsNullOrEmpty(entry.Text) && !string.IsNullOrEmpty(entry.Source))
                        {
                            _bySource.TryAdd(entry.Source, entry.Text);
                        }
                    }
                }
            }
        }
        catch (Exception)
        {
            // Corrupted table: keep running with an empty one instead of blocking startup.
        }
    }

    /// <summary>
    /// Looks up a translation by exact key first, then by source text. Returns null when nothing
    /// matches.
    /// </summary>
    public LocalizationEntry Resolve(string key, string source)
    {
        lock (_sync)
        {
            if (_entries.TryGetValue(key, out var entry)) return entry;

            if (!string.IsNullOrEmpty(source) && _bySource.TryGetValue(source, out var text))
            {
                return new LocalizationEntry { Source = source, Text = text };
            }

            return null;
        }
    }

    /// <summary>
    /// Returns the translation for the key, registering it with the source text on first use.
    /// </summary>
    public string Translate(string key, string source)
    {
        lock (_sync)
        {
            if (_entries.TryGetValue(key, out var existing))
            {
                return string.IsNullOrEmpty(existing.Text) ? source : existing.Text;
            }

            _entries[key] = new LocalizationEntry { Source = source ?? string.Empty, Text = string.Empty };
            _order.Add(key);

            if (!string.IsNullOrEmpty(source)) _bySource.TryAdd(source, string.Empty);

            _dirty = true;
            ScheduleSave();
        }

        return source;
    }

    /// <summary>
    /// Registers a missing key. Returns false when the key already exists; an existing entry is
    /// never modified.
    /// </summary>
    public bool TryAdd(string key, string source)
    {
        lock (_sync)
        {
            if (_entries.ContainsKey(key)) return false;

            _entries[key] = new LocalizationEntry { Source = source ?? string.Empty, Text = string.Empty };
            _order.Add(key);

            if (!string.IsNullOrEmpty(source)) _bySource.TryAdd(source, string.Empty);

            _dirty = true;
            ScheduleSave();
            return true;
        }
    }

    public void MarkCovered(string formName)
    {
        if (string.IsNullOrEmpty(formName)) return;
        if (Forms.Add(formName)) _dirty = true;
    }

    private void ScheduleSave()
    {
        if (_saveTimer == null)
        {
            _saveTimer = new Timer(_ => SaveIfDirty(), null, Timeout.Infinite, Timeout.Infinite);
        }

        _saveTimer.Change(SaveDelay, Timeout.Infinite);
    }

    /// <summary>
    /// Writes the table back to disk when something changed. The file is written to a temporary
    /// path and swapped in; if the swap is not supported (some network shares) it falls back to
    /// overwriting the target. Failures are swallowed.
    /// </summary>
    public bool SaveIfDirty()
    {
        if (!_dirty || string.IsNullOrEmpty(FilePath)) return false;

        try
        {
            var directory = Path.GetDirectoryName(FilePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory)) Directory.CreateDirectory(directory);

            byte[] bytes;
            List<string> order;
            HashSet<string> forms;

            lock (_sync)
            {
                order = new List<string>(_order);
                forms = new HashSet<string>(Forms, StringComparer.Ordinal);

                using (var stream = new MemoryStream())
                {
                    using (var writer = new Utf8JsonWriter(stream, new JsonWriterOptions
                    {
                        Indented = true,
                        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
                    }))
                    {
                        writer.WriteStartObject();
                        writer.WritePropertyName(MetaKey);
                        writer.WriteStartObject();
                        writer.WriteNumber("version", 2);
                        writer.WritePropertyName(FormsKey);
                        writer.WriteStartArray();
                        foreach (var form in forms.OrderBy(x => x, StringComparer.Ordinal)) writer.WriteStringValue(form);
                        writer.WriteEndArray();
                        writer.WriteEndObject();

                        foreach (var key in order)
                        {
                            if (!_entries.TryGetValue(key, out var entry)) continue;

                            writer.WritePropertyName(key);
                            writer.WriteStartObject();
                            writer.WriteString(SourceKey, entry.Source ?? string.Empty);
                            writer.WriteString(TextKey, entry.Text ?? string.Empty);
                            writer.WriteEndObject();
                        }

                        writer.WriteEndObject();
                    }

                    bytes = stream.ToArray();
                }
            }

            var temporary = FilePath + ".tmp";
            File.WriteAllBytes(temporary, bytes);

            if (File.Exists(FilePath))
            {
                try
                {
                    File.Replace(temporary, FilePath, null);
                }
                catch (Exception)
                {
                    File.Copy(temporary, FilePath, true);
                    File.Delete(temporary);
                }
            }
            else
            {
                File.Move(temporary, FilePath);
            }

            _dirty = false;
            return true;
        }
        catch (Exception)
        {
            // Read-only directory, missing permissions or a full disk: keep running untranslated.
            return false;
        }
    }

    /// <summary>
    /// Forces a write to the given path, even when nothing changed.
    /// </summary>
    public void Save(string path)
    {
        FilePath = path;
        _dirty = true;
        SaveIfDirty();
    }
}
