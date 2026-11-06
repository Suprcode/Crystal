using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using Server.MirEnvir;

namespace Server.Database
{
    // One save-time mapping, not a sequence of substitutions while the user edits.
    internal sealed class MonsterNameSynchronization
    {
        private readonly Dictionary<string, string> names = new(StringComparer.OrdinalIgnoreCase);
        private readonly Dictionary<string, string> compactNames = new(StringComparer.OrdinalIgnoreCase);
        private readonly List<FileChange> files = new();
        private readonly List<(string Section, string Key, string OldValue, string NewValue)> settings = new();

        internal MonsterNameSynchronization(IEnumerable<(string OldName, string NewName)> changes)
        {
            lock (Settings.MonsterNameSyncRoot) Prepare(changes);
        }

        private void Prepare(IEnumerable<(string OldName, string NewName)> changes)
        {
            var records = changes.ToArray();
            foreach (var (oldName, newName) in records)
            {
                // A null original name denotes a newly inserted row, not an existing reference.
                if (oldName == null) continue;
                if (string.Equals(oldName, newName, StringComparison.Ordinal)) continue;
                if (string.IsNullOrWhiteSpace(oldName)) throw new IOException("Cannot synchronize references for an empty original monster name.");
                ValidateName(newName);
                if (names.TryGetValue(oldName, out var existing) && existing != newName)
                    throw new IOException($"Original monster name is ambiguous; references cannot be synchronized: {oldName}");
                names[oldName] = newName;
                var compact = oldName.Replace(" ", "");
                if (compactNames.TryGetValue(compact, out var other) && other != newName)
                    throw new IOException($"Monster name is ambiguous after removing spaces; references cannot be synchronized: {oldName}");
                compactNames[compact] = newName;
            }
            if (names.Count == 0) return;

            // Unchanged records also own their names and aliases; do not redirect their references.
            foreach (var group in records.Where(x => x.OldName != null).GroupBy(x => x.OldName, StringComparer.OrdinalIgnoreCase))
                if (group.Count() > 1 && group.Any(x => names.ContainsKey(x.OldName)))
                    throw new IOException($"Original monster name is ambiguous; references cannot be synchronized: {group.Key}");
            foreach (var group in records.Where(x => x.OldName != null).GroupBy(x => x.OldName.Replace(" ", ""), StringComparer.OrdinalIgnoreCase))
                if (group.Count() > 1 && group.Any(x => names.ContainsKey(x.OldName)))
                    throw new IOException($"Monster name is ambiguous after removing spaces; references cannot be synchronized: {group.Key}");
            foreach (var group in records.GroupBy(x => x.NewName.Replace(" ", ""), StringComparer.OrdinalIgnoreCase))
                if (group.Count() > 1 && group.Any(x => x.OldName == null || !string.Equals(x.OldName, x.NewName, StringComparison.Ordinal)))
                    throw new IOException($"Final monster name is ambiguous; references cannot be synchronized: {group.First().NewName}");

            PrepareSettings("Setup.ini", Settings.GetMonsterNameSettings());
            PrepareSettings("FishingSystem.ini", new[] { ("Game", "Monster", Settings.FishingMonster) });
            PrepareScripts(Settings.NPCPath, RewriteNpc);
            PrepareScripts(Settings.QuestPath, RewriteQuest);
        }

        private static void ValidateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name) || name.Any(c => char.IsWhiteSpace(c) || char.IsControl(c) ||
                CharUnicodeInfo.GetUnicodeCategory(c) == UnicodeCategory.Format) ||
                name.IndexOfAny(new[] { '[', ']', '|', '=', ';', '"' }) >= 0)
                throw new IOException($"Monster name contains script delimiters, whitespace or invisible characters: {name}");
        }

        private void PrepareSettings(string fileName, IEnumerable<(string Section, string Key, string Value)> entries)
        {
            var values = entries.ToDictionary(x => (x.Section, x.Key), x => x.Value);
            var seen = new HashSet<(string, string)>();
            var path = Path.Combine(Settings.ConfigPath, fileName);
            bool fishing = fileName == "FishingSystem.ini";
            bool shadowedFish = !fishing && File.Exists(Path.Combine(Settings.ConfigPath, "FishingSystem.ini"));
            var original = File.Exists(path) ? File.ReadAllBytes(path) : null;
            var text = Decode(original ?? Array.Empty<byte>(), path, out var encoding, out var prefix);
            string section = string.Empty;
            var rewritten = RewriteLines(text, line =>
            {
                if (line.StartsWith("[") && line.EndsWith("]"))
                {
                    section = line[1..^1];
                    return line;
                }
                int equals = line.IndexOf('=');
                if (equals < 0) return line;
                var key = line[..equals];
                if (!values.TryGetValue((section, key), out var memory)) return line;
                var value = line[(equals + 1)..];
                var diskName = value.Trim();
                bool diskChanged = names.TryGetValue(diskName, out var diskNew);
                bool shadowed = shadowedFish && key == "FishMonster";
                bool memoryChanged = names.TryGetValue(memory, out var memoryNew) && !shadowed;
                if (!diskChanged && !memoryChanged) return line;
                if (!seen.Add((section, key)))
                    throw new IOException($"Duplicate setting prevents automatic synchronization: {path} [{section}] {key}");
                var finalMemory = memoryChanged ? memoryNew : memory;
                var finalDisk = diskChanged ? diskNew : diskName;
                if (!shadowed && !string.Equals(finalMemory, finalDisk, StringComparison.OrdinalIgnoreCase))
                    throw new IOException($"Monster setting differs between memory and disk. Resolve the conflict first: {path} [{section}] {key}");
                if (memoryChanged) settings.Add((section, fishing ? "FishMonster" : key, memory, memoryNew));
                if (!diskChanged) return line;
                int start = equals + 1 + value.Length - value.TrimStart().Length;
                return line[..start] + diskNew + line[(start + diskName.Length)..];
            });

            foreach (var entry in values)
            {
                if (seen.Contains(entry.Key) || (shadowedFish && entry.Key.Key == "FishMonster") ||
                    !names.TryGetValue(entry.Value, out var renamed)) continue;
                // An absent setting still needs to survive the next restart.
                rewritten = AppendSetting(rewritten, entry.Key.Section, entry.Key.Key, renamed);
                settings.Add((entry.Key.Section, fishing ? "FishMonster" : entry.Key.Key, entry.Value, renamed));
            }
            AddFile(path, original, Encode(rewritten, encoding, prefix), guard: original != null);
        }

        private static string AppendSetting(string text, string section, string key, string value)
        {
            var newline = text.Contains("\r\n") ? "\r\n" : text.Contains('\n') ? "\n" : Environment.NewLine;
            var header = Regex.Match(text, @"(?m)^\[" + Regex.Escape(section) + @"\](?:\r?\n|$)");
            if (header.Success)
            {
                int end = header.Index + header.Length;
                var separator = end > 0 && text[end - 1] != '\n' ? newline : string.Empty;
                return text[..end] + separator + key + "=" + value + newline + text[end..];
            }
            return text + (text.Length > 0 && text[^1] != '\n' ? newline : string.Empty) +
                "[" + section + "]" + newline + key + "=" + value + newline;
        }

        private void PrepareScripts(string directory, Func<string, string> rewrite)
        {
            if (!Directory.Exists(directory)) return;
            foreach (var path in Directory.EnumerateFiles(directory, "*.txt", SearchOption.AllDirectories))
            {
                var original = File.ReadAllBytes(path);
                var text = Decode(original, path, out var encoding, out var prefix);
                var changed = rewrite(text);
                if (changed != text) AddFile(path, original, Encode(changed, encoding, prefix));
            }
        }

        private string ReplaceToken(string line, int index, bool compact = false)
        {
            var tokens = Regex.Matches(line.TrimEnd(), @"[^ ]+");
            if (tokens.Count <= index) return line;
            var token = tokens[index];
            if (!(compact ? compactNames : names).TryGetValue(token.Value, out var renamed)) return line;
            return line[..token.Index] + renamed + line[(token.Index + token.Length)..];
        }

        private string RewriteNpc(string text)
        {
            // Only executable sections contain commands. Dialogue can look like a command too.
            string mode = string.Empty;
            bool page = false;
            return RewriteLines(text, line =>
            {
                if (line.StartsWith('[') && (line.EndsWith(']') || (line.StartsWith("[@") && line.Contains(']'))))
                {
                    page = line.StartsWith("[@");
                    mode = string.Empty;
                    return line;
                }
                if (!page) return line;
                if (line.StartsWith('#'))
                {
                    mode = "#" + line[1..].ToUpperInvariant().Trim().Split(' ')[0];
                    return line;
                }
                var tokens = Regex.Matches(line.TrimEnd(), @"[^ ]+");
                if (tokens.Count == 0 || line.StartsWith(';')) return line;
                string command = tokens[0].Value.ToUpperInvariant();
                if (mode == "#IF" && command == "CHECKEXACTMON" && tokens.Count >= 5)
                    return ReplaceToken(line, 1, compact: true);
                if (mode == "#IF" && command == "CHECKPET" && tokens.Count >= 2)
                    return ReplaceToken(line, 1);
                if (mode == "#ACT" || mode == "#ELSEACT")
                {
                    if ((command == "MONGEN" || command == "GIVEPET") && tokens.Count >= 2)
                        return ReplaceToken(line, 1, compact: true);
                    if (command == "REMOVEPET" && tokens.Count >= 2) return ReplaceToken(line, 1);
                    if (command == "MONCLEAR" && tokens.Count >= 4) return ReplaceToken(line, 3);
                }
                return line;
            });
        }

        private string RewriteQuest(string text)
        {
            bool kills = false;
            return RewriteLines(text, line =>
            {
                if (line.StartsWith('[')) kills = line.Equals("[@KillTasks]", StringComparison.OrdinalIgnoreCase);
                if (line.StartsWith("//")) kills = false;
                if (!kills || string.IsNullOrWhiteSpace(line) || line.StartsWith(';') || line.StartsWith("//")) return line;
                var token = Regex.Match(line, @"^[^ ]+");
                return token.Success && compactNames.TryGetValue(token.Value, out var renamed)
                    ? renamed + line[token.Length..] : line;
            });
        }

        private static string RewriteLines(string text, Func<string, string> rewrite)
        {
            return Regex.Replace(text, @"[^\r\n]+", match => rewrite(match.Value));
        }

        private static string Decode(byte[] bytes, string path, out Encoding encoding, out byte[] prefix)
        {
            encoding = new UTF8Encoding(false, true);
            int length = 0;
            if (bytes.AsSpan().StartsWith(new byte[] { 0xff, 0xfe, 0, 0 })) { encoding = new UTF32Encoding(false, false, true); length = 4; }
            else if (bytes.AsSpan().StartsWith(new byte[] { 0, 0, 0xfe, 0xff })) { encoding = new UTF32Encoding(true, false, true); length = 4; }
            else if (bytes.AsSpan().StartsWith(new byte[] { 0xff, 0xfe })) { encoding = new UnicodeEncoding(false, false, true); length = 2; }
            else if (bytes.AsSpan().StartsWith(new byte[] { 0xfe, 0xff })) { encoding = new UnicodeEncoding(true, false, true); length = 2; }
            else if (bytes.AsSpan().StartsWith(new byte[] { 0xef, 0xbb, 0xbf })) length = 3;
            prefix = bytes[..length];
            try { return encoding.GetString(bytes, length, bytes.Length - length); }
            catch (DecoderFallbackException ex)
            {
                throw new IOException($"File is not valid UTF-8 or BOM-marked Unicode. Refusing to rewrite its encoding: {path}", ex);
            }
        }

        private static byte[] Encode(string text, Encoding encoding, byte[] prefix)
            => prefix.Concat(encoding.GetBytes(text)).ToArray();

        private void AddFile(string path, byte[] original, byte[] updated, bool guard = false)
        {
            if (!guard && original != null && original.SequenceEqual(updated)) return;
            if (original == null && updated.Length == 0) return;
            files.Add(new FileChange(path, original, updated));
        }

        // Open all dirty targets before changing anything; keep writers out until commit/rollback.
        internal void Execute(Envir environment, Action<Action> save)
        {
            lock (Settings.MonsterNameSyncRoot) ExecuteCore(environment, save);
        }

        private void ExecuteCore(Envir environment, Action<Action> save)
        {
            var dragon = environment.DragonInfo;
            var dragonMonsterName = dragon?.MonsterName;
            var dragonBodyName = dragon?.BodyName;
            bool dragonApplied = false;
            var opened = new List<(FileChange Change, FileStream Stream)>();
            var written = new Stack<(FileChange Change, FileStream Stream)>();
            FileStream databaseStream = null;
            byte[] database = null;
            bool databaseExisted = false;
            bool databaseStarted = false;
            bool settingsApplied = false;
            bool committed = false;
            void SaveDatabase()
            {
                // Snapshot only at the actual save boundary and hold the same exclusive handle through rollback.
                if (databaseStream != null) throw new InvalidOperationException("Database save was already requested.");
                databaseExisted = File.Exists(Envir.DatabasePath);
                databaseStream = new FileStream(Envir.DatabasePath, databaseExisted ? FileMode.Open : FileMode.CreateNew,
                    FileAccess.ReadWrite, FileShare.None);
                using var current = new MemoryStream();
                databaseStream.CopyTo(current);
                database = current.ToArray();
                databaseStarted = true;
                environment.SaveDB(databaseStream, databaseExisted);
            }
            try
            {
                foreach (var file in files)
                {
                    var stream = new FileStream(file.Path, file.Original == null ? FileMode.CreateNew : FileMode.Open,
                        file.Dirty ? FileAccess.ReadWrite : FileAccess.Read, FileShare.Read);
                    opened.Add((file, stream));
                    using var current = new MemoryStream();
                    stream.CopyTo(current);
                    if (file.Original != null && !current.ToArray().SequenceEqual(file.Original))
                        throw new IOException($"Referenced file changed after validation. Save was cancelled: {file.Path}");
                }
                Settings.CheckMonsterNameSettings(settings.Select(x => (x.Section, x.Key, x.OldValue)));
                foreach (var item in opened)
                {
                    if (!item.Change.Dirty) continue;
                    written.Push(item);
                    Write(item.Stream, item.Change.Updated);
                }
                Settings.SetMonsterNameSettings(settings.Select(x => (x.Section, x.Key, x.NewValue)));
                settingsApplied = true;
                RefreshSetupCache(opened);
                if (dragon != null)
                {
                    // Dragon.Load uses the same case-insensitive, space-free aliases as GetMonsterInfo.
                    if (dragonMonsterName != null && compactNames.TryGetValue(dragonMonsterName.Replace(" ", ""), out var monsterName))
                        dragon.MonsterName = monsterName;
                    if (dragonBodyName != null && compactNames.TryGetValue(dragonBodyName.Replace(" ", ""), out var bodyName))
                        dragon.BodyName = bodyName;
                    dragonApplied = true;
                }
                save(SaveDatabase);
                committed = true;
            }
            catch (Exception error)
            {
                var errors = new List<Exception> { error };
                if (dragonApplied)
                {
                    dragon.MonsterName = dragonMonsterName;
                    dragon.BodyName = dragonBodyName;
                }
                if (databaseStarted)
                {
                    try { Write(databaseStream, database); }
                    catch (Exception ex) { errors.Add(ex); }
                }
                while (written.TryPop(out var item))
                {
                    try { Write(item.Stream, item.Change.Original ?? Array.Empty<byte>()); }
                    catch (Exception ex) { errors.Add(ex); }
                }
                try
                {
                    if (settingsApplied)
                        Settings.SetMonsterNameSettings(settings.Select(x => (x.Section, x.Key, x.OldValue)));
                    RefreshSetupCache(opened);
                }
                catch (Exception ex) { errors.Add(ex); }
                throw new AggregateException("Monster reference save failed. Settings, scripts and database rollback was attempted. Check the error details before retrying.", errors);
            }
            finally
            {
                if (databaseStream != null)
                {
                    databaseStream.Dispose();
                    if (!committed && !databaseExisted && File.Exists(Envir.DatabasePath) && new FileInfo(Envir.DatabasePath).Length == 0)
                        File.Delete(Envir.DatabasePath);
                }
                foreach (var item in opened)
                {
                    item.Stream.Dispose();
                    if (!committed && item.Change.Original == null &&
                        File.Exists(item.Change.Path) && new FileInfo(item.Change.Path).Length == 0)
                        File.Delete(item.Change.Path);
                }
            }
        }

        private static void RefreshSetupCache(IEnumerable<(FileChange Change, FileStream Stream)> opened)
        {
            var path = Path.GetFullPath(Path.Combine(Settings.ConfigPath, "Setup.ini"));
            var setup = opened.FirstOrDefault(x => Path.GetFullPath(x.Change.Path) == path);
            if (setup.Stream == null)
            {
                Settings.RefreshSetupCache();
                return;
            }
            // Reopening a write-locked file with File.ReadAllLines fails Windows sharing checks.
            setup.Stream.Position = 0;
            using var reader = new StreamReader(setup.Stream, Encoding.UTF8, true, 1024, leaveOpen: true);
            var lines = new List<string>();
            while (reader.ReadLine() is { } line) lines.Add(line);
            Settings.RefreshSetupCache(lines);
        }

        private static void Write(FileStream stream, byte[] bytes)
        {
            stream.Position = 0;
            stream.Write(bytes);
            stream.SetLength(bytes.Length);
            stream.Flush(true);
        }

        private sealed record FileChange(string Path, byte[] Original, byte[] Updated)
        {
            internal bool Dirty => Original == null || !Original.SequenceEqual(Updated);
        }
    }
}
