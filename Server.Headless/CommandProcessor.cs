using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Server;
using Server.MirEnvir;
using Server.MirDatabase;
using Server.MirObjects;

namespace Server.Headless
{
    public class CommandProcessor
    {
        private readonly ConsoleServerHost _host;
        private readonly CancellationTokenSource _cts;
        private readonly List<string> _history = new List<string>();
        private int _historyIndex = 0;
        private string _tempInput = "";
        private DateTime _lastTabTime = DateTime.MinValue;

        private static readonly string[] PrimaryCommands = new[]
        {
            "help", "status", "start", "stop", "reboot", "restart", "exit", "quit",
            "reload", "say", "broadcast", "list", "kick", "blockedips", "player", "ipban", "ipunban", "gm", "account"
        };

        private static readonly string[] ReloadSubcommands = new[] { "npc", "drops", "line", "all" };
        private static readonly string[] AccountSubcommands = new[] { "show", "get", "set" };
        private static readonly string[] ListSubcommands = new[] { "players", "guilds" };
        private static readonly string[] BlockedIpsSubcommands = new[] { "list", "clear", "add", "remove" };
        private static readonly string[] PlayerSubcommands = new[]
        {
            "status", "info", "edit", "message", "kick", "kill", "killpets", "safezone",
            "chatban", "chatunban", "ban", "unban", "flag"
        };
        private static readonly string[] PlayerEditStats = new[] { "level", "gold", "credit", "pk" };
        private static readonly string[] GMCommands = new[]
        {
            "@LOGIN", "@KILL", "@CHANGEGENDER", "@LEVEL", "@LEVELHERO", "@MAKE", "@CLEARBUFFS", "@CLEARBAG",
            "@SUPERMAN", "@GAMEMASTER", "@OBSERVER", "@ALLOWGUILD", "@RECALL", "@OBSERVE", "@ENABLEGROUPRECALL",
            "@GROUPRECALL", "@RECALLMEMBER", "@RECALLLOVER", "@TIME", "@ROLL", "@MAP", "@BACKUPPLAYER",
            "@ARCHIVEPLAYER", "@LOADPLAYER", "@RESTOREPLAYER", "@MOVE", "@MAPMOVE", "@GOTO", "@MOB",
            "@RECALLMOB", "@RELOADDROPS", "@RELOADNPCS", "@CLEARIPBLOCKS", "@GIVEGOLD", "@GIVEPEARLS",
            "@GIVECREDIT", "@GIVESKILL", "@FIND", "@LEAVEGUILD", "@CREATEGUILD", "@ALLOWTRADE", "@TRIGGER",
            "@RIDE", "@SETFLAG", "@LISTFLAGS", "@CLEARFLAGS", "@CLEARMOB", "@CHANGECLASS", "@DIE", "@HAIR",
            "@DECO", "@ADJUSTPKPOINT", "@AWAKENING", "@REMOVEAWAKENING", "@STARTWAR", "@ADDINVENTORY",
            "@ADDSTORAGE", "@SUMMONHERO", "@ALLOWOBSERVE", "@INFO", "@CLEARQUESTS", "@SETQUEST",
            "@TOGGLETRANSFORM", "@STARTCONQUEST", "@RESETCONQUEST", "@GATES", "@CHANGEFLAG",
            "@CHANGEFLAGCOLOUR", "@REVIVE", "@DELETESKILL", "@SETTIMER", "@SETLIGHT"
        };

        public static readonly object ConsoleLock = new object();
        public static string CurrentInput { get; set; } = "";
        public static int CursorPosition { get; set; } = 0;
        public static bool IsReadingInput { get; set; } = false;

        private static int GetCharWidth(char c)
        {
            if (char.IsControl(c)) return 0;

            int code = c;
            
            // CJK Unified Ideographs & Extension A
            if (code >= 0x4E00 && code <= 0x9FFF) return 2;
            if (code >= 0x3400 && code <= 0x4DBF) return 2;
            
            // Hangul Syllables
            if (code >= 0xAC00 && code <= 0xD7AF) return 2;
            
            // CJK Symbols and Punctuation, Hiragana, Katakana, Bopomofo, Hangul Compatibility Jamo, etc.
            if (code >= 0x3000 && code <= 0x32FF) return 2;
            if (code >= 0x3300 && code <= 0x33FF) return 2;
            
            // Fullwidth Forms (0xFF01 to 0xFF60, 0xFFE0 to 0xFFE6)
            if (code >= 0xFF01 && code <= 0xFF60) return 2;
            if (code >= 0xFFE0 && code <= 0xFFE6) return 2;

            // CJK Compatibility Ideographs
            if (code >= 0xF900 && code <= 0xFAFF) return 2;
            
            // CJK Compatibility Forms
            if (code >= 0xFE30 && code <= 0xFE4F) return 2;

            return 1;
        }

        private static int GetStringWidth(string s)
        {
            if (string.IsNullOrEmpty(s)) return 0;
            int width = 0;
            for (int i = 0; i < s.Length; i++)
            {
                char c = s[i];
                if (char.IsHighSurrogate(c) && i + 1 < s.Length && char.IsLowSurrogate(s[i + 1]))
                {
                    int codePoint = char.ConvertToUtf32(c, s[i + 1]);
                    i++; // skip low surrogate
                    if (codePoint >= 0x20000 && codePoint <= 0x3FFFF)
                    {
                        width += 2; // CJK Extensions B, C, D, E, F, G etc.
                    }
                    else
                    {
                        width += 1;
                    }
                }
                else
                {
                    width += GetCharWidth(c);
                }
            }
            return width;
        }

        public static void ClearInputLine()
        {
            lock (ConsoleLock)
            {
                if (!IsReadingInput) return;
                int length = 2 + GetStringWidth(CurrentInput);
                Console.Write("\r" + new string(' ', length) + "\r");
            }
        }

        public static void RedrawInputLine()
        {
            lock (ConsoleLock)
            {
                if (!IsReadingInput) return;
                Console.Write("> " + CurrentInput);
                if (CurrentInput != null && CursorPosition < CurrentInput.Length)
                {
                    string remaining = CurrentInput.Substring(CursorPosition);
                    int remainingWidth = GetStringWidth(remaining);
                    Console.Write(new string('\b', remainingWidth));
                }
            }
        }

        private void Print(string msg)
        {
            lock (ConsoleLock)
            {
                Console.WriteLine(msg);
            }
        }

        private void PrintTiledCompletions(List<string> options)
        {
            lock (ConsoleLock)
            {
                Console.WriteLine();
                int maxLen = options.Count > 0 ? options.Max(o => GetStringWidth(o)) : 0;
                int colWidth = maxLen + 2;
                if (colWidth < 16) colWidth = 16;
                int windowWidth = 80;
                try { windowWidth = Console.WindowWidth; } catch {}
                int lineLen = 0;
                foreach (var option in options)
                {
                    if (lineLen > 0 && lineLen + colWidth > windowWidth)
                    {
                        Console.WriteLine();
                        lineLen = 0;
                    }
                    int optionWidth = GetStringWidth(option);
                    int padding = colWidth - optionWidth;
                    if (padding < 0) padding = 0;
                    Console.Write(option + new string(' ', padding));
                    lineLen += colWidth;
                }
                Console.WriteLine();
                Console.Write("> " + CurrentInput);
                if (CurrentInput != null && CursorPosition < CurrentInput.Length)
                {
                    string remaining = CurrentInput.Substring(CursorPosition);
                    int remainingWidth = GetStringWidth(remaining);
                    Console.Write(new string('\b', remainingWidth));
                }
            }
        }

        public CommandProcessor(ConsoleServerHost host, CancellationTokenSource cts)
        {
            _host = host;
            _cts = cts;
        }

        public async Task RunAsync()
        {
            _host.Log("Headless CLI Interactive Console Initialized.");
            _host.Log("Type 'help' or '?' for a list of available commands. Tab completion is active.");

            while (!_cts.Token.IsCancellationRequested)
            {
                string line = null;
                try
                {
                    bool isInteractive = !Console.IsInputRedirected && !Console.IsOutputRedirected;
                    if (isInteractive)
                    {
                        lock (ConsoleLock)
                        {
                            Console.CursorVisible = true;
                            IsReadingInput = true;
                            CurrentInput = "";
                            CursorPosition = 0;
                            Console.Write("> ");
                        }
                    }

                    line = await ReadLineWithTabCompletionAsync(_cts.Token);
                }
                catch (TaskCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    lock (ConsoleLock)
                    {
                        IsReadingInput = false;
                    }
                    _host.Log($"Console read error: {ex.Message}");
                    await Task.Delay(1000, _cts.Token);
                    continue;
                }

                lock (ConsoleLock)
                {
                    IsReadingInput = false;
                }

                if (line == null)
                {
                    // EOF or TTY not attached, yield to prevent high CPU utilization
                    await Task.Delay(2000, _cts.Token);
                    continue;
                }

                line = line.Trim();
                if (string.IsNullOrEmpty(line))
                    continue;

                if (line.StartsWith("/"))
                {
                    line = line.Substring(1);
                }

                try
                {
                    ExecuteCommand(line);
                }
                catch (Exception ex)
                {
                    Print($"Error executing command '{line}': {ex.Message}");
                }
            }
        }

        private async Task<string> ReadLineWithTabCompletionAsync(CancellationToken token)
        {
            if (Console.IsInputRedirected || Console.IsOutputRedirected)
            {
                return await Task.Run(() => Console.ReadLine(), token);
            }

            var buffer = new StringBuilder();
            int cursor = 0;
            int activePrefixLength = 0;
            string textBeforeTab = null;

            while (!token.IsCancellationRequested)
            {
                if (!Console.KeyAvailable)
                {
                    await Task.Delay(25, token);
                    continue;
                }

                var keyInfo = Console.ReadKey(true);

                if (keyInfo.Key == ConsoleKey.Enter)
                {
                    lock (ConsoleLock)
                    {
                        Console.WriteLine();
                    }
                    string cmd = buffer.ToString();
                    if (!string.IsNullOrEmpty(cmd))
                    {
                        if (_history.Count == 0 || _history[_history.Count - 1] != cmd)
                        {
                            _history.Add(cmd);
                        }
                    }
                    _historyIndex = _history.Count;
                    return cmd;
                }
                else if (keyInfo.Key == ConsoleKey.Backspace)
                {
                    if (cursor > 0)
                    {
                        lock (ConsoleLock)
                        {
                            cursor--;
                            char deletedChar = buffer[cursor];
                            buffer.Remove(cursor, 1);
                            CurrentInput = buffer.ToString();
                            CursorPosition = cursor;

                            int deletedWidth = GetCharWidth(deletedChar);
                            Console.Write(new string('\b', deletedWidth) + new string(' ', deletedWidth) + new string('\b', deletedWidth));

                            if (cursor < buffer.Length)
                            {
                                string remaining = buffer.ToString().Substring(cursor);
                                Console.Write(remaining + " ");
                                int remainingWidth = GetStringWidth(remaining);
                                Console.Write(new string('\b', remainingWidth + 1));
                            }
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Tab)
                {
                    DateTime now = DateTime.UtcNow;
                    bool isDoubleTab = (now - _lastTabTime).TotalMilliseconds <= 500;
                    if (isDoubleTab)
                    {
                        _lastTabTime = DateTime.MinValue;
                    }
                    else
                    {
                        _lastTabTime = now;
                    }

                    textBeforeTab = buffer.ToString();
                    var completions = GetCompletions(textBeforeTab, out activePrefixLength);

                    if (completions.Count == 1)
                    {
                        lock (ConsoleLock)
                        {
                            string completion = completions[0];
                            int currentVisualWidth = GetStringWidth(buffer.ToString());
                            for (int i = 0; i < currentVisualWidth; i++)
                            {
                                Console.Write("\b \b");
                            }

                            buffer.Clear();
                            string prefix = textBeforeTab.Substring(0, textBeforeTab.Length - activePrefixLength);
                            buffer.Append(prefix);
                            buffer.Append(completion);
                            cursor = buffer.Length;

                            CurrentInput = buffer.ToString();
                            CursorPosition = cursor;

                            Console.Write(buffer.ToString());
                        }
                    }
                    else if (completions.Count > 1)
                    {
                        if (isDoubleTab)
                        {
                            PrintTiledCompletions(completions);
                        }
                        else
                        {
                            Console.Beep();
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    lock (ConsoleLock)
                    {
                        int currentVisualWidth = GetStringWidth(buffer.ToString());
                        for (int i = 0; i < currentVisualWidth; i++)
                        {
                            Console.Write("\b \b");
                        }
                        buffer.Clear();
                        cursor = 0;
                        CurrentInput = "";
                        CursorPosition = 0;
                    }
                }
                else if (keyInfo.Key == ConsoleKey.UpArrow)
                {
                    if (_history.Count > 0)
                    {
                        if (_historyIndex == _history.Count)
                        {
                            _tempInput = buffer.ToString();
                        }

                        if (_historyIndex > 0)
                        {
                            _historyIndex--;
                            lock (ConsoleLock)
                            {
                                int currentVisualWidth = GetStringWidth(buffer.ToString());
                                for (int i = 0; i < currentVisualWidth; i++)
                                {
                                    Console.Write("\b \b");
                                }

                                buffer.Clear();
                                buffer.Append(_history[_historyIndex]);
                                cursor = buffer.Length;

                                CurrentInput = buffer.ToString();
                                CursorPosition = cursor;

                                Console.Write(buffer.ToString());
                            }
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.DownArrow)
                {
                    if (_historyIndex < _history.Count)
                    {
                        _historyIndex++;
                        lock (ConsoleLock)
                        {
                            int currentVisualWidth = GetStringWidth(buffer.ToString());
                            for (int i = 0; i < currentVisualWidth; i++)
                            {
                                Console.Write("\b \b");
                            }

                            buffer.Clear();
                            if (_historyIndex == _history.Count)
                            {
                                buffer.Append(_tempInput);
                            }
                            else
                            {
                                buffer.Append(_history[_historyIndex]);
                            }
                            cursor = buffer.Length;

                            CurrentInput = buffer.ToString();
                            CursorPosition = cursor;

                            Console.Write(buffer.ToString());
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.LeftArrow)
                {
                    if (cursor > 0)
                    {
                        lock (ConsoleLock)
                        {
                            cursor--;
                            CursorPosition = cursor;
                            int charWidth = GetCharWidth(buffer[cursor]);
                            Console.Write(new string('\b', charWidth));
                        }
                    }
                }
                else if (keyInfo.Key == ConsoleKey.RightArrow)
                {
                    if (cursor < buffer.Length)
                    {
                        lock (ConsoleLock)
                        {
                            Console.Write(buffer[cursor]);
                            cursor++;
                            CursorPosition = cursor;
                        }
                    }
                }
                else if (keyInfo.KeyChar != '\0')
                {
                    char c = keyInfo.KeyChar;
                    lock (ConsoleLock)
                    {
                        buffer.Insert(cursor, c);
                        cursor++;
                        CurrentInput = buffer.ToString();
                        CursorPosition = cursor;

                        Console.Write(c);
                        if (cursor < buffer.Length)
                        {
                            string remaining = buffer.ToString().Substring(cursor);
                            Console.Write(remaining);
                            int remainingWidth = GetStringWidth(remaining);
                            Console.Write(new string('\b', remainingWidth));
                        }
                    }
                }
            }

            return null;
        }

        private List<string> GetCompletions(string text, out int prefixLength)
        {
            prefixLength = 0;
            var completions = new List<string>();
            var parts = ParseArgumentsForCompletion(text);
            bool endsWithSpace = text.Length > 0 && text[text.Length - 1] == ' ';

            if (parts.Count == 0 || (parts.Count == 1 && !endsWithSpace))
            {
                string word = parts.Count == 1 ? parts[0] : "";
                prefixLength = word.Length;
                completions.AddRange(PrimaryCommands.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
            }
            else
            {
                string primaryCmd = parts[0].ToLowerInvariant();

                if (primaryCmd == "reload")
                {
                    if (parts.Count == 1 && endsWithSpace)
                    {
                        prefixLength = 0;
                        completions.AddRange(ReloadSubcommands);
                    }
                    else if (parts.Count == 2 && !endsWithSpace)
                    {
                        string word = parts[1];
                        prefixLength = word.Length;
                        completions.AddRange(ReloadSubcommands.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                    }
                }
                else if (primaryCmd == "list")
                {
                    if (parts.Count == 1 && endsWithSpace)
                    {
                        prefixLength = 0;
                        completions.AddRange(ListSubcommands);
                    }
                    else if (parts.Count == 2 && !endsWithSpace)
                    {
                        string word = parts[1];
                        prefixLength = word.Length;
                        completions.AddRange(ListSubcommands.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                    }
                }
                else if (primaryCmd == "blockedips")
                {
                    if (parts.Count == 1 && endsWithSpace)
                    {
                        prefixLength = 0;
                        completions.AddRange(BlockedIpsSubcommands);
                    }
                    else if (parts.Count == 2 && !endsWithSpace)
                    {
                        string word = parts[1];
                        prefixLength = word.Length;
                        completions.AddRange(BlockedIpsSubcommands.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                    }
                }
                else if (primaryCmd == "kick")
                {
                    var onlinePlayerNames = Envir.Main.Players.Select(p => p.Name).ToList();
                    if (parts.Count == 1 && endsWithSpace)
                    {
                        prefixLength = 0;
                        completions.AddRange(onlinePlayerNames);
                    }
                    else if (parts.Count == 2 && !endsWithSpace)
                    {
                        string word = parts[1];
                        prefixLength = word.Length;
                        completions.AddRange(onlinePlayerNames.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                    }
                }
                else if (primaryCmd == "gm")
                {
                    var onlinePlayerNames = Envir.Main.Players.Select(p => p.Name).ToList();
                    var allCharacterNames = Envir.Main.CharacterList.Select(p => p.Name).ToList();

                    if (parts.Count == 1 && endsWithSpace)
                    {
                        prefixLength = 0;
                        completions.AddRange(onlinePlayerNames);
                    }
                    else if (parts.Count == 2 && !endsWithSpace)
                    {
                        string word = parts[1];
                        prefixLength = word.Length;
                        completions.AddRange(onlinePlayerNames.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                    }
                    else
                    {
                        if ((parts.Count == 2 && endsWithSpace) || (parts.Count == 3 && !endsWithSpace))
                        {
                            string word = parts.Count == 3 ? parts[2] : "";
                            prefixLength = word.Length;
                            completions.AddRange(GMCommands.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                        }
                        else if (parts.Count >= 3)
                        {
                            string gmCmd = parts[2].ToUpperInvariant();
                            int argIndex = parts.Count - 3;
                            if (!endsWithSpace)
                            {
                                argIndex--;
                            }

                            if (argIndex >= 0)
                            {
                                string word = (parts.Count > 3 && !endsWithSpace) ? parts[parts.Count - 1] : "";
                                prefixLength = word.Length;

                                List<string> argOptions = GetGMCommandArgCompletions(gmCmd, argIndex, parts, onlinePlayerNames, allCharacterNames);
                                completions.AddRange(argOptions.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                            }
                        }
                    }
                }
                else if (primaryCmd == "player")
                {
                    if (parts.Count == 1 && endsWithSpace)
                    {
                        var onlinePlayerNames = Envir.Main.Players.Select(p => p.Name).ToList();
                        prefixLength = 0;
                        completions.AddRange(onlinePlayerNames);
                    }
                    else if (parts.Count == 2 && !endsWithSpace)
                    {
                        var onlinePlayerNames = Envir.Main.Players.Select(p => p.Name).ToList();
                        string word = parts[1];
                        prefixLength = word.Length;
                        completions.AddRange(onlinePlayerNames.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                    }
                    else if (parts.Count == 2 && endsWithSpace)
                    {
                        prefixLength = 0;
                        completions.AddRange(PlayerSubcommands);
                    }
                    else if (parts.Count == 3 && !endsWithSpace)
                    {
                        string word = parts[2];
                        prefixLength = word.Length;
                        completions.AddRange(PlayerSubcommands.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                    }
                    else if (parts.Count == 3 && endsWithSpace)
                    {
                        string sub = parts[2].ToLowerInvariant();
                        if (sub == "edit")
                        {
                            prefixLength = 0;
                            completions.AddRange(PlayerEditStats);
                        }
                    }
                    else if (parts.Count == 4 && !endsWithSpace)
                    {
                        string sub = parts[2].ToLowerInvariant();
                        if (sub == "edit")
                        {
                            string word = parts[3];
                            prefixLength = word.Length;
                            completions.AddRange(PlayerEditStats.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                        }
                    }
                }
                else if (primaryCmd == "account")
                {
                    if (parts.Count == 1 && endsWithSpace)
                    {
                        prefixLength = 0;
                        completions.AddRange(AccountSubcommands);
                    }
                    else if (parts.Count == 2 && !endsWithSpace)
                    {
                        string word = parts[1];
                        prefixLength = word.Length;
                        completions.AddRange(AccountSubcommands.Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                    }
                    else if (parts.Count == 2 && endsWithSpace)
                    {
                        prefixLength = 0;
                        completions.AddRange(Envir.Main.AccountList.Select(x => x.AccountID));
                    }
                    else if (parts.Count == 3 && !endsWithSpace)
                    {
                        string pathArg = parts[2];
                        if (pathArg.Contains('='))
                        {
                            // Do not autocomplete after equals sign
                        }
                        else
                        {
                            int lastDot = pathArg.LastIndexOf('.');
                            if (lastDot >= 0)
                            {
                                string parentPath = pathArg.Substring(0, lastDot);
                                string segmentPrefix = pathArg.Substring(lastDot + 1);

                                var (nodePath, error) = ResolvePath(parentPath);
                                if (error == null && nodePath != null && nodePath.Count > 0)
                                {
                                    var parentNode = nodePath.Last();
                                    object parentObj = parentNode.Value;
                                    if (parentObj != null)
                                    {
                                        var possibleMembers = GetNextSegmentCompletions(parentObj);
                                        var filtered = possibleMembers
                                            .Where(m => m.StartsWith(segmentPrefix, StringComparison.OrdinalIgnoreCase))
                                            .Select(m => parentPath + "." + m);

                                        prefixLength = pathArg.Length;
                                        completions.AddRange(filtered);
                                    }
                                }
                            }
                            else
                            {
                                string word = pathArg;
                                prefixLength = word.Length;
                                completions.AddRange(Envir.Main.AccountList.Select(x => x.AccountID).Where(c => c.StartsWith(word, StringComparison.OrdinalIgnoreCase)));
                            }
                        }
                    }
                }
            }

            return completions;
        }

        private List<string> GetNextSegmentCompletions(object parentObj)
        {
            var completions = new List<string>();
            if (parentObj == null) return completions;

            Type type = parentObj.GetType();

            // 1. Add fields (excluding byte types)
            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(byte) || field.FieldType == typeof(byte[]) || field.FieldType == typeof(byte?))
                {
                    continue;
                }
                completions.Add(field.Name);
            }

            // 2. Add properties (excluding indexers and byte types)
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.GetIndexParameters().Length > 0) continue;
                if (prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(byte[]) || prop.PropertyType == typeof(byte?))
                {
                    continue;
                }
                completions.Add(prop.Name);
            }

            // 3. Shortcuts: Character names for AccountInfo
            if (parentObj is AccountInfo acc)
            {
                foreach (var character in acc.Characters)
                {
                    if (!string.IsNullOrEmpty(character.Name))
                    {
                        completions.Add(character.Name);
                    }
                }
            }

            // 4. Shortcuts: Stat enum names for UserItem and Stats
            if (parentObj is UserItem || parentObj is Stats)
            {
                foreach (var name in Enum.GetNames(typeof(Stat)))
                {
                    completions.Add(name);
                }
            }

            return completions.Distinct().ToList();
        }

        private List<string> ParseArgumentsForCompletion(string text)
        {
            var list = new List<string>();
            var sb = new StringBuilder();
            bool inQuotes = false;
            for (int i = 0; i < text.Length; i++)
            {
                char c = text[i];
                if (c == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (c == ' ' && !inQuotes)
                {
                    if (sb.Length > 0)
                    {
                        list.Add(sb.ToString());
                        sb.Clear();
                    }
                }
                else
                {
                    sb.Append(c);
                }
            }
            if (sb.Length > 0)
            {
                list.Add(sb.ToString());
            }
            return list;
        }

        private void ExecuteCommand(string line)
        {
            var parts = ParseArguments(line);
            if (parts.Length == 0) return;

            string command = parts[0].ToLowerInvariant();

            switch (command)
            {
                case "help":
                case "?":
                    ShowHelp();
                    break;

                case "status":
                    ShowStatus();
                    break;

                case "start":
                    StartServer();
                    break;

                case "stop":
                    StopServer();
                    break;

                case "reboot":
                case "restart":
                    RebootServer();
                    break;

                case "exit":
                case "quit":
                    ExitServer();
                    break;

                case "reload":
                    ReloadConfig(parts);
                    break;

                case "say":
                case "broadcast":
                    BroadcastAnnouncement(parts);
                    break;

                case "list":
                case "online":
                    ListInfo(parts);
                    break;

                case "kick":
                    KickPlayer(parts);
                    break;

                case "blockedips":
                case "ipban":
                case "ipunban":
                    HandleIPBans(command, parts);
                    break;

                case "player":
                    HandlePlayerCommand(parts);
                    break;

                case "gm":
                    HandleGMCommand(parts);
                    break;

                case "account":
                    HandleAccountCommand(parts);
                    break;

                default:
                    Print($"Unknown command '{command}'. Type 'help' for a list of commands.");
                    break;
            }
        }

        private string[] ParseArguments(string commandLine)
        {
            var parts = new List<string>();
            var currentToken = new StringBuilder();
            bool inQuotes = false;

            for (int i = 0; i < commandLine.Length; i++)
            {
                char c = commandLine[i];

                if (c == '\"')
                {
                    inQuotes = !inQuotes;
                    continue;
                }

                if (c == ' ' && !inQuotes)
                {
                    if (currentToken.Length > 0)
                    {
                        parts.Add(currentToken.ToString());
                        currentToken.Clear();
                    }
                }
                else
                {
                    currentToken.Append(c);
                }
            }

            if (currentToken.Length > 0)
            {
                parts.Add(currentToken.ToString());
            }

            return parts.ToArray();
        }

        private void ShowHelp()
        {
            Print("=== Headless Server Console Commands ===");
            Print("  help / ? - Displays this help message.");
            Print("  status - Displays real-time server metrics (uptime, players, monsters, cycle delays, etc.).");
            Print("  start - Starts the server environment if stopped.");
            Print("  stop - Stops the server environment.");
            Print("  reboot / restart - Reboots the server environment.");
            Print("  exit / quit - Gracefully stops the server and closes the application.");
            Print("  reload <npc | drops | line | all> - Reloads server configuration files.");
            Print("  say / broadcast <message> - Sends an announcement message to all online players.");
            Print("  list <players | guilds> - Lists all online players or registered guilds.");
            Print("  kick <player> [reason] - Kicks the specified player from the server.");
            Print("  blockedips <list | clear | add <ip> <days> | remove <ip>> - Manages IP blocklist.");
            Print("  player <name> <status | edit | message | kick | kill | killpets | safezone | chatban | chatunban | ban | unban | flag> [args...] - Manages a specific player. (Type 'player help' for details)");
            Print("  gm <player> <message> - Simulates a chat box command or message for <player> with temporary GM privileges.");
            Print("  account <show | get | set> [args...] - Manages user accounts and attributes. (Type 'account help' for details)");
        }

        private void ShowPlayerHelp()
        {
            Print("=== Player Management Subcommands ===");
            Print("  player <name> status / info - Shows detailed player information.");
            Print("  player <name> edit <level | gold | credit | pk> <value> - Modifies player stats.");
            Print("  player <name> message <msg> - Sends an announcement chat to the player.");
            Print("  player <name> kick - Disconnects the player.");
            Print("  player <name> kill - Kills the player.");
            Print("  player <name> killpets - Kills all the player's pets.");
            Print("  player <name> safezone - Teleports the player to their safe zone / bind point.");
            Print("  player <name> chatban <minutes> - Bans the player from chatting for N minutes.");
            Print("  player <name> chatunban - Removes the player's chat ban.");
            Print("  player <name> ban <minutes> - Bans the player's account for N minutes.");
            Print("  player <name> unban - Unbans the player's account.");
            Print("  player <name> flag <index> <enable | disable> - Enables or disables a flag on the player.");
        }

        private void ShowStatus()
        {
            var envir = Envir.Main;
            if (envir == null)
            {
                Print("Server Environment is not initialized.");
                return;
            }

            string runningState = envir.Running ? "RUNNING" : "STOPPED";
            var uptime = envir.Stopwatch.Elapsed;
            string uptimeStr = $"{uptime.Days}d:{uptime.Hours}h:{uptime.Minutes}m:{uptime.Seconds}s";
            int playersCount = envir.Players.Count;
            int monstersCount = envir.MonsterCount;
            int connectionsCount = envir.Connections.Count;
            int blockedIpsCount = Envir.IPBlocks.Count(x => x.Value > envir.Now);

            Print($"--- Server Status ({runningState}) ---");
            Print($"  Uptime: {uptimeStr}");
            Print($"  Active Players: {playersCount}");
            Print($"  Active Monsters: {monstersCount}");
            Print($"  TCP Connections: {connectionsCount}");
            Print($"  Blocked IPs: {blockedIpsCount}");

            if (Settings.Multithreaded && envir.MobThreads != null)
            {
                var cycleDelays = $"CycleDelays: {Envir.LastRunTime:0000}";
                for (int i = 0; i < envir.MobThreads.Length; i++)
                {
                    if (envir.MobThreads[i] == null) break;
                    cycleDelays += $"|{envir.MobThreads[i].LastRunTime:0000}";
                }
                Print($"  {cycleDelays}");
            }
            else
            {
                Print($"  CycleDelay: {Envir.LastRunTime}");
            }
        }

        private void StartServer()
        {
            if (Envir.Main.Running)
            {
                Print("Server is already running.");
                return;
            }
            Print("Starting Server Environment...");
            Envir.Main.Start();
        }

        private void StopServer()
        {
            if (!Envir.Main.Running)
            {
                Print("Server is not running.");
                return;
            }
            Print("Stopping Server Environment...");
            Envir.Main.Stop();
            Envir.Main.MonsterCount = 0;
            Print("Server stopped.");
        }

        private void RebootServer()
        {
            Print("Rebooting Server Environment...");
            Envir.Main.Reboot();
        }

        private void ExitServer()
        {
            Print("Shutdown command received. Commencing graceful termination...");
            _cts.Cancel();
        }

        private void ReloadConfig(string[] parts)
        {
            if (parts.Length < 2)
            {
                Print("Usage: reload <npc | drops | line | all>");
                return;
            }

            string target = parts[1].ToLowerInvariant();
            switch (target)
            {
                case "npc":
                    Envir.Main.ReloadNPCs();
                    Print("NPC scripts reloaded.");
                    break;
                case "drops":
                    Envir.Main.ReloadDrops();
                    Print("Drop configs reloaded.");
                    break;
                case "line":
                    Envir.Main.ReloadLineMessages();
                    Print("Line messages reloaded.");
                    break;
                case "all":
                    Envir.Main.ReloadNPCs();
                    Envir.Main.ReloadDrops();
                    Envir.Main.ReloadLineMessages();
                    Print("NPCs, Drops, and Line messages reloaded.");
                    break;
                default:
                    Print($"Unknown reload target '{target}'. Valid targets: npc, drops, line, all");
                    break;
            }
        }

        private void BroadcastAnnouncement(string[] parts)
        {
            if (parts.Length < 2)
            {
                Print("Usage: say <message> or broadcast <message>");
                return;
            }

            string message = string.Join(" ", parts.Skip(1));

            foreach (var player in Envir.Main.Players)
            {
                player.ReceiveChat(message, ChatType.Announcement);
            }

            MessageQueue.Instance.EnqueueChat(message);
            Print($"[Broadcast] {message}");
        }

        private void ListInfo(string[] parts)
        {
            if (parts.Length < 2)
            {
                Print("Usage: list <players | guilds>");
                return;
            }

            string target = parts[1].ToLowerInvariant();
            if (target == "players" || target == "player")
            {
                var players = Envir.Main.Players;
                Print($"--- Online Players ({players.Count}) ---");
                Print(string.Format("{0,-6} | {1,-15} | {2,-5} | {3,-10} | {4,-8} | {5}", "Index", "Name", "Level", "Class", "Gender", "Current Map"));
                Print(new string('-', 75));
                foreach (var p in players)
                {
                    string mapName = MapInfo.GetMapTitleByIndex(p.Info.CurrentMapIndex);
                    Print(string.Format("{0,-6} | {1,-15} | {2,-5} | {3,-10} | {4,-8} | {5}",
                        p.Info.Index, p.Name, p.Level, p.Class, p.Gender, mapName));
                }
            }
            else if (target == "guilds" || target == "guild")
            {
                var guilds = Envir.Main.GuildList;
                Print($"--- Registered Guilds ({guilds.Count}) ---");
                Print(string.Format("{0,-6} | {1,-20} | {2,-15} | {3,-10} | {4,-5} | {5,-10} | {6}",
                    "Index", "Name", "Leader", "Members", "Level", "Gold", "Territory Rent"));
                Print(new string('-', 90));
                foreach (var g in guilds)
                {
                    string leaderName = "DELETED";
                    if (g.Ranks.Count > 0 && g.Ranks[0].Members.Count > 0)
                    {
                        leaderName = g.Ranks[0].Members[0].Name;
                    }

                    Print(string.Format("{0,-6} | {1,-20} | {2,-15} | {3,-10} | {4,-5} | {5,-10} | {6}",
                        g.GuildIndex, g.Name, leaderName, $"{g.Membercount}/{g.MemberCap}", g.Level, g.Gold, g.HasGT ? g.GTRent.ToString() : "None"));
                }
            }
            else
            {
                Print($"Unknown list target '{target}'. Valid targets: players, guilds");
            }
        }

        private void KickPlayer(string[] parts)
        {
            if (parts.Length < 2)
            {
                Print("Usage: kick <player> [reason]");
                return;
            }

            string playerName = parts[1];
            var player = Envir.Main.GetPlayer(playerName);
            if (player == null)
            {
                Print($"Player '{playerName}' not found or is offline.");
                return;
            }

            string reason = parts.Length > 2 ? string.Join(" ", parts.Skip(2)) : "Kicked by administrator.";
            Print($"Kicking player '{player.Name}' (Reason: {reason})");
            player.Connection.SendDisconnect(4); // Reason 4 is KickedByAdmin
        }

        private void HandleGMCommand(string[] parts)
        {
            if (parts.Length < 3)
            {
                Print("Usage: gm <player> <message>");
                return;
            }

            string playerName = parts[1];
            var player = Envir.Main.GetPlayer(playerName);
            if (player == null)
            {
                Print($"Player '{playerName}' not found or is offline.");
                return;
            }

            string message = string.Join(" ", parts.Skip(2));
            bool originalIsGM = player.IsGM;
            player.IsGM = true;
            try
            {
                player.Chat(message);
            }
            finally
            {
                player.IsGM = originalIsGM;
            }
        }

        private void HandleIPBans(string command, string[] parts)
        {
            if (command == "ipban")
            {
                if (parts.Length < 2)
                {
                    Print("Usage: ipban <ip> [days]");
                    return;
                }
                double days = parts.Length > 2 && double.TryParse(parts[2], out double d) ? d : 365;
                Envir.Main.UpdateIPBlock(parts[1], TimeSpan.FromDays(days));
                Print($"IP '{parts[1]}' has been blocked for {days} days.");
                return;
            }
            else if (command == "ipunban")
            {
                if (parts.Length < 2)
                {
                    Print("Usage: ipunban <ip>");
                    return;
                }
                if (Envir.IPBlocks.TryRemove(parts[1], out _))
                {
                    Print($"IP '{parts[1]}' has been unblocked.");
                }
                else
                {
                    Print($"IP '{parts[1]}' was not found in the blocklist.");
                }
                return;
            }

            if (parts.Length < 2)
            {
                Print("Usage: blockedips <list | clear | add <ip> [days] | remove <ip>>");
                return;
            }

            string action = parts[1].ToLowerInvariant();
            switch (action)
            {
                case "list":
                    var activeBlocks = Envir.IPBlocks.Where(x => x.Value > Envir.Main.Now).ToList();
                    Print($"--- Blocked IPs ({activeBlocks.Count}) ---");
                    foreach (var block in activeBlocks)
                    {
                        Print($"  {block.Key} (Expires: {block.Value})");
                    }
                    break;

                case "clear":
                    Envir.IPBlocks.Clear();
                    Print("IP blocklist cleared.");
                    break;

                case "add":
                    if (parts.Length < 3)
                    {
                        Print("Usage: blockedips add <ip> [days]");
                        return;
                    }
                    double addDays = parts.Length > 3 && double.TryParse(parts[3], out double ad) ? ad : 365;
                    Envir.Main.UpdateIPBlock(parts[2], TimeSpan.FromDays(addDays));
                    Print($"IP '{parts[2]}' has been blocked for {addDays} days.");
                    break;

                case "remove":
                    if (parts.Length < 3)
                    {
                        Print("Usage: blockedips remove <ip>");
                        return;
                    }
                    if (Envir.IPBlocks.TryRemove(parts[2], out _))
                    {
                        Print($"IP '{parts[2]}' has been unblocked.");
                    }
                    else
                    {
                        Print($"IP '{parts[2]}' was not found in the blocklist.");
                    }
                    break;

                default:
                    Print($"Unknown blockedips action '{action}'. Valid actions: list, clear, add, remove");
                    break;
            }
        }

        private void HandlePlayerCommand(string[] parts)
        {
            if (parts.Length < 3)
            {
                ShowPlayerHelp();
                return;
            }

            string playerName = parts[1];
            string action = parts[2].ToLowerInvariant();

            CharacterInfo charInfo = Envir.Main.GetCharacterInfo(playerName);
            if (charInfo == null)
            {
                Print($"Character '{playerName}' not found in database.");
                return;
            }

            PlayerObject player = charInfo.Player;

            switch (action)
            {
                case "status":
                case "info":
                    PrintPlayerStatus(charInfo);
                    break;

                case "edit":
                    if (parts.Length < 5)
                    {
                        Print("Usage: player <name> edit <level | gold | credit | pk> <value>");
                        return;
                    }
                    EditPlayerStat(charInfo, parts[3].ToLowerInvariant(), parts[4]);
                    break;

                case "message":
                case "msg":
                    if (player == null)
                    {
                        Print($"Player '{playerName}' is offline.");
                        return;
                    }
                    if (parts.Length < 4)
                    {
                        Print("Usage: player <name> message <msg>");
                        return;
                    }
                    string msg = string.Join(" ", parts.Skip(3));
                    player.ReceiveChat(msg, ChatType.Announcement);
                    Print($"Sent message to '{playerName}': {msg}");
                    break;

                case "kick":
                    if (player == null)
                    {
                        Print($"Player '{playerName}' is offline.");
                        return;
                    }
                    Print($"Kicking player '{playerName}'...");
                    player.Connection.SendDisconnect(4);
                    break;

                case "kill":
                    if (player == null)
                    {
                        Print($"Player '{playerName}' is offline.");
                        return;
                    }
                    Print($"Killing player '{playerName}'...");
                    player.Die();
                    break;

                case "killpets":
                    if (player == null)
                    {
                        Print($"Player '{playerName}' is offline.");
                        return;
                    }
                    Print($"Killing all pets for player '{playerName}'...");
                    for (int i = player.Pets.Count - 1; i >= 0; i--)
                    {
                        player.Pets[i].Die();
                    }
                    break;

                case "safezone":
                    if (player == null)
                    {
                        Print($"Player '{playerName}' is offline.");
                        return;
                    }
                    Print($"Teleporting player '{playerName}' to safe zone...");
                    player.Teleport(Envir.Main.GetMap(charInfo.BindMapIndex), charInfo.BindLocation);
                    break;

                case "chatban":
                    if (parts.Length < 4 || !int.TryParse(parts[3], out int chatBanMins))
                    {
                        Print("Usage: player <name> chatban <minutes>");
                        return;
                    }
                    charInfo.ChatBanned = true;
                    charInfo.ChatBanExpiryDate = Envir.Main.Now.AddMinutes(chatBanMins);
                    Print($"Player '{playerName}' chat-banned for {chatBanMins} minutes (Expires: {charInfo.ChatBanExpiryDate}).");
                    if (player != null)
                    {
                        player.ReceiveChat($"You have been chat-banned by the administrator for {chatBanMins} minutes.", ChatType.System);
                    }
                    break;

                case "chatunban":
                    charInfo.ChatBanned = false;
                    charInfo.ChatBanExpiryDate = DateTime.MinValue;
                    Print($"Player '{playerName}' chat-unbanned.");
                    if (player != null)
                    {
                        player.ReceiveChat("Your chat ban has been removed by the administrator.", ChatType.System);
                    }
                    break;

                case "ban":
                    if (parts.Length < 4 || !int.TryParse(parts[3], out int banMins))
                    {
                        Print("Usage: player <name> ban <minutes>");
                        return;
                    }
                    if (charInfo.AccountInfo.AdminAccount)
                    {
                        Print("Cannot ban an administrator account.");
                        return;
                    }
                    charInfo.AccountInfo.Banned = true;
                    charInfo.AccountInfo.ExpiryDate = Envir.Main.Now.AddMinutes(banMins);
                    Print($"Player '{playerName}' account banned for {banMins} minutes (Expires: {charInfo.AccountInfo.ExpiryDate}).");
                    if (player != null)
                    {
                        player.Connection.SendDisconnect(6);
                    }
                    break;

                case "unban":
                    charInfo.AccountInfo.Banned = false;
                    charInfo.AccountInfo.ExpiryDate = DateTime.MinValue;
                    Print($"Player '{playerName}' account unbanned.");
                    break;

                case "flag":
                    if (parts.Length < 5)
                    {
                        Print("Usage: player <name> flag <index> <enable | disable>");
                        return;
                    }
                    if (!int.TryParse(parts[3], out int flagIndex) || flagIndex < 0 || flagIndex >= charInfo.Flags.Length)
                    {
                        Print($"Invalid flag index. Must be between 0 and {charInfo.Flags.Length - 1}.");
                        return;
                    }
                    string flagValue = parts[4].ToLowerInvariant();
                    if (flagValue == "enable" || flagValue == "true" || flagValue == "active" || flagValue == "1")
                    {
                        charInfo.Flags[flagIndex] = true;
                        Print($"Flag {flagIndex} enabled for '{playerName}'.");
                    }
                    else if (flagValue == "disable" || flagValue == "false" || flagValue == "inactive" || flagValue == "0")
                    {
                        charInfo.Flags[flagIndex] = false;
                        Print($"Flag {flagIndex} disabled for '{playerName}'.");
                    }
                    else
                    {
                        Print("Invalid flag value. Use 'enable' or 'disable'.");
                    }
                    break;

                default:
                    Print($"Unknown player subcommand '{action}'. Type 'player' for details.");
                    break;
            }
        }

        private void PrintPlayerStatus(CharacterInfo charInfo)
        {
            Print($"=== Player Status: {charInfo.Name} ===");
            Print($"  Index: {charInfo.Index}");
            Print($"  Level: {charInfo.Level}");
            Print($"  Class: {charInfo.Class}");
            Print($"  Gender: {charInfo.Gender}");
            Print($"  Gold: {charInfo.AccountInfo.Gold:n0}");
            Print($"  Credit (GameGold): {charInfo.AccountInfo.Credit:n0}");
            Print($"  PK Points: {charInfo.PKPoints}");
            Print($"  Last IP: {charInfo.AccountInfo.LastIP}");
            Print($"  Status: {(charInfo.Player != null ? "ONLINE" : "OFFLINE")}");

            if (charInfo.Player != null)
            {
                var p = charInfo.Player;
                string mapName = MapInfo.GetMapTitleByIndex(charInfo.CurrentMapIndex);
                Print($"  Current Map: {mapName} (X: {p.CurrentLocation.X}, Y: {p.CurrentLocation.Y})");
                double expPct = p.MaxExperience > 0 ? (double)p.Experience / p.MaxExperience : 0;
                Print($"  Experience: {p.Experience} / {p.MaxExperience} ({expPct:P2})");
                Print($"  Stats: AC={p.Stats[Stat.MinAC]}-{p.Stats[Stat.MaxAC]}, MAC={p.Stats[Stat.MinMAC]}-{p.Stats[Stat.MaxMAC]}, DC={p.Stats[Stat.MinDC]}-{p.Stats[Stat.MaxDC]}, MC={p.Stats[Stat.MinMC]}-{p.Stats[Stat.MaxMC]}, SC={p.Stats[Stat.MinSC]}-{p.Stats[Stat.MaxSC]}");
                Print($"  Pets: {p.Pets.Count} active pets.");
            }

            if (charInfo.ChatBanned)
            {
                Print($"  Chat Banned: YES (Expires: {charInfo.ChatBanExpiryDate})");
            }
            if (charInfo.AccountInfo.Banned)
            {
                Print($"  Account Banned: YES (Expires: {charInfo.AccountInfo.ExpiryDate})");
            }
        }

        private void EditPlayerStat(CharacterInfo charInfo, string statName, string rawValue)
        {
            switch (statName)
            {
                case "level":
                    if (byte.TryParse(rawValue, out byte lvl))
                    {
                        charInfo.Level = lvl;
                        Print($"Set '{charInfo.Name}' level to {lvl}.");
                        if (charInfo.Player != null)
                        {
                            charInfo.Player.Level = lvl;
                            charInfo.Player.LevelUp();
                        }
                    }
                    else
                    {
                        Print("Invalid level value. Must be a byte (0-255).");
                    }
                    break;

                case "gold":
                    if (uint.TryParse(rawValue, out uint gold))
                    {
                        charInfo.AccountInfo.Gold = gold;
                        Print($"Set '{charInfo.Name}' account gold to {gold:n0}.");
                    }
                    else
                    {
                        Print("Invalid gold value. Must be a non-negative integer.");
                    }
                    break;

                case "credit":
                case "gamegold":
                    if (uint.TryParse(rawValue, out uint credit))
                    {
                        charInfo.AccountInfo.Credit = credit;
                        Print($"Set '{charInfo.Name}' account credit to {credit:n0}.");
                    }
                    else
                    {
                        Print("Invalid credit value. Must be a non-negative integer.");
                    }
                    break;

                case "pk":
                case "pkpoints":
                    if (int.TryParse(rawValue, out int pk))
                    {
                        charInfo.PKPoints = pk;
                        Print($"Set '{charInfo.Name}' PK points to {pk}.");
                    }
                    else
                    {
                        Print("Invalid PK points value. Must be an integer.");
                    }
                    break;

                default:
                    Print($"Unknown editable stat '{statName}'. Editable stats: level, gold, credit, pk");
                    break;
            }
        }

        private void ShowAccountHelp()
        {
            Print("=== Account Management Commands (OpenWrt uci style) ===");
            Print("  account show [<path>] - Recursively shows attributes. If no path, lists all accounts.");
            Print("  account get <path>    - Displays the value of a specific field.");
            Print("  account set <path>=<value> - Sets the value of a specific field/property.");
            Print("");
            Print("Examples:");
            Print("  account set admin.Gold=999999");
            Print("  account set admin.Characters[0].Inventory[0].Luck=9");
            Print("  account set admin.characterName.Inventory[0].Luck=9");
            Print("  account show admin.characterName");
        }

        private void HandleAccountCommand(string[] parts)
        {
            if (parts.Length < 2)
            {
                ShowAccountHelp();
                return;
            }

            string sub = parts[1].ToLowerInvariant();
            if (sub == "help")
            {
                ShowAccountHelp();
                return;
            }

            if (sub == "show")
            {
                if (parts.Length < 3)
                {
                    var accounts = Envir.Main.AccountList;
                    Print($"--- Accounts List ({accounts.Count}) ---");
                    foreach (var acc in accounts)
                    {
                        Print($"  {acc.AccountID} (Characters: {acc.Characters.Count})");
                    }
                    return;
                }

                string path = parts[2];
                var (nodePath, error) = ResolvePath(path);
                if (error != null)
                {
                    Print($"Error: {error}");
                    return;
                }

                var leaf = nodePath.Last();
                bool isProtected = leaf.Parent is AccountInfo && leaf.Member != null &&
                    (string.Equals(leaf.Member.Name, "Password", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(leaf.Member.Name, "StoragePassword", StringComparison.OrdinalIgnoreCase));

                if (isProtected)
                {
                    Print($"{path}=[Protected]");
                }
                else
                {
                    ShowObject(leaf.Value, path, limitElements: false);
                }
            }
            else if (sub == "get")
            {
                if (parts.Length < 3)
                {
                    Print("Usage: account get <path>");
                    return;
                }

                string path = parts[2];
                var (nodePath, error) = ResolvePath(path);
                if (error != null)
                {
                    Print($"Error: {error}");
                    return;
                }

                var leaf = nodePath.Last();
                bool isProtected = leaf.Parent is AccountInfo && leaf.Member != null &&
                    (string.Equals(leaf.Member.Name, "Password", StringComparison.OrdinalIgnoreCase) ||
                     string.Equals(leaf.Member.Name, "StoragePassword", StringComparison.OrdinalIgnoreCase));

                if (isProtected)
                {
                    Print($"{path}=[Protected]");
                }
                else
                {
                    Print($"{path}={leaf.Value}");
                }
            }
            else if (sub == "set")
            {
                if (parts.Length < 3)
                {
                    Print("Usage: account set <path>=<value>");
                    return;
                }

                string fullArg = string.Join(" ", parts.Skip(2));
                int eqIdx = fullArg.IndexOf('=');
                if (eqIdx < 0)
                {
                    Print("Usage: account set <path>=<value>");
                    return;
                }

                string path = fullArg.Substring(0, eqIdx).Trim();
                string valStr = fullArg.Substring(eqIdx + 1).Trim();

                var (nodePath, error) = ResolvePath(path);
                if (error != null)
                {
                    Print($"Error: {error}");
                    return;
                }

                var leaf = nodePath.Last();
                Type targetType = GetNodeTargetType(leaf);
                if (targetType == null)
                {
                    Print($"Error: Cannot determine type of field/property for '{path}'.");
                    return;
                }

                object parsedVal;
                try
                {
                    parsedVal = ParseValue(valStr, targetType);
                }
                catch (Exception ex)
                {
                    Print($"Error parsing value '{valStr}' to {targetType.Name}: {ex.Message}");
                    return;
                }

                try
                {
                    SetAndPropagate(nodePath, parsedVal);
                }
                catch (Exception ex)
                {
                    Print($"Error setting value: {ex.Message}");
                    return;
                }

                NotifyAndSync(nodePath);

                Print($"{path} set to {parsedVal}");
            }
            else
            {
                Print($"Unknown account subcommand '{sub}'. Type 'account help' for details.");
            }
        }

        private class PathNode
        {
            public object Value { get; set; }
            public object Parent { get; set; }
            public MemberInfo Member { get; set; }
            public int? Index { get; set; }
            public Stat? StatKey { get; set; }
        }

        private (List<PathNode> pathNodes, string error) ResolvePath(string path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return (null, "Path is empty.");

            var segments = path.Split('.');
            if (segments.Length == 0)
                return (null, "Invalid path.");

            string firstSeg = segments[0];
            var (accountID, rootIndex) = ParseSegmentNameAndIndex(firstSeg);

            var account = Envir.Main.GetAccount(accountID);
            if (account == null)
                return (null, $"Account '{accountID}' not found.");

            var pathNodes = new List<PathNode>();

            PathNode current;
            if (rootIndex.HasValue)
            {
                return (null, $"Root account ID '{accountID}' cannot be indexed.");
            }
            else
            {
                current = new PathNode { Value = account };
                pathNodes.Add(current);
            }

            for (int i = 1; i < segments.Length; i++)
            {
                var (segName, index) = ParseSegmentNameAndIndex(segments[i]);
                var next = ResolveSegment(current, segName, index);
                if (next == null)
                {
                    return (null, $"Could not resolve segment '{segments[i]}' on '{GetNodePathString(pathNodes)}'.");
                }
                pathNodes.Add(next);
                current = next;
            }

            return (pathNodes, null);
        }

        private (string name, int? index) ParseSegmentNameAndIndex(string segment)
        {
            segment = segment.Trim();
            int bracketStart = segment.IndexOf('[');
            if (bracketStart >= 0)
            {
                int bracketEnd = segment.IndexOf(']');
                if (bracketEnd > bracketStart)
                {
                    string name = segment.Substring(0, bracketStart).Trim();
                    string idxStr = segment.Substring(bracketStart + 1, bracketEnd - bracketStart - 1).Trim();
                    if (int.TryParse(idxStr, out int idx))
                    {
                        return (name, idx);
                    }
                }
            }
            return (segment, null);
        }

        private string GetNodePathString(List<PathNode> pathNodes)
        {
            var sb = new StringBuilder();
            foreach (var node in pathNodes)
            {
                if (sb.Length > 0) sb.Append(".");
                if (node.Value is AccountInfo acc) sb.Append(acc.AccountID);
                else if (node.Member != null)
                {
                    sb.Append(node.Member.Name);
                    if (node.Index.HasValue) sb.Append($"[{node.Index.Value}]");
                }
                else if (node.StatKey.HasValue)
                {
                    sb.Append(node.StatKey.Value.ToString());
                }
            }
            return sb.ToString();
        }

        private PathNode ResolveSegment(PathNode current, string segmentName, int? index)
        {
            object obj = current.Value;
            if (obj == null) return null;

            Type type = obj.GetType();

            var members = type.GetMember(segmentName, BindingFlags.Public | BindingFlags.Instance | BindingFlags.IgnoreCase);
            var member = members.FirstOrDefault(m => m is FieldInfo || m is PropertyInfo);

            if (member != null)
            {
                object nextVal = GetMemberValue(member, obj);
                if (index.HasValue)
                {
                    if (nextVal is Array arr)
                    {
                        if (index.Value < 0 || index.Value >= arr.Length) return null;
                        return new PathNode
                        {
                            Value = arr.GetValue(index.Value),
                            Parent = arr,
                            Member = member,
                            Index = index.Value
                        };
                    }
                    else if (nextVal is IList list)
                    {
                        if (index.Value < 0 || index.Value >= list.Count) return null;
                        return new PathNode
                        {
                            Value = list[index.Value],
                            Parent = list,
                            Member = member,
                            Index = index.Value
                        };
                    }
                    return null;
                }
                else
                {
                    return new PathNode
                    {
                        Value = nextVal,
                        Parent = obj,
                        Member = member
                    };
                }
            }

            if (obj is AccountInfo acc)
            {
                var characters = acc.Characters;
                var foundChar = characters.FirstOrDefault(c => string.Equals(c.Name, segmentName, StringComparison.OrdinalIgnoreCase));
                if (foundChar != null)
                {
                    if (index.HasValue) return null;
                    return new PathNode
                    {
                        Value = foundChar,
                        Parent = characters,
                        Member = typeof(AccountInfo).GetField("Characters")
                    };
                }
            }

            if (obj is UserItem item)
            {
                if (Enum.TryParse<Stat>(segmentName, true, out var stat))
                {
                    if (index.HasValue) return null;
                    return new PathNode
                    {
                        Value = item.AddedStats[stat],
                        Parent = item.AddedStats,
                        StatKey = stat
                    };
                }
            }

            if (obj is Stats stats)
            {
                if (Enum.TryParse<Stat>(segmentName, true, out var stat))
                {
                    if (index.HasValue) return null;
                    return new PathNode
                    {
                        Value = stats[stat],
                        Parent = stats,
                        StatKey = stat
                    };
                }
            }

            return null;
        }

        private object GetMemberValue(MemberInfo member, object obj)
        {
            if (member is FieldInfo f) return f.GetValue(obj);
            if (member is PropertyInfo p) return p.GetValue(obj);
            return null;
        }

        private Type GetNodeTargetType(PathNode node)
        {
            if (node.StatKey.HasValue)
            {
                return typeof(int);
            }
            if (node.Member != null)
            {
                Type type = (node.Member is FieldInfo f) ? f.FieldType : ((PropertyInfo)node.Member).PropertyType;
                if (node.Index.HasValue)
                {
                    if (type.IsArray)
                    {
                        return type.GetElementType();
                    }
                    else if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(List<>))
                    {
                        return type.GetGenericArguments()[0];
                    }
                }
                else
                {
                    return type;
                }
            }
            return null;
        }

        private object ParseValue(string val, Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            if (type == typeof(string)) return val;
            if (type == typeof(bool))
            {
                val = val.Trim().ToLowerInvariant();
                if (val == "true" || val == "1" || val == "yes" || val == "on" || val == "enable") return true;
                if (val == "false" || val == "0" || val == "no" || val == "off" || val == "disable") return false;
                return bool.Parse(val);
            }
            if (type.IsEnum)
            {
                return Enum.Parse(type, val, true);
            }
            if (type == typeof(Point))
            {
                var parts = val.Split(new char[] { ',', ' ', ';' }, StringSplitOptions.RemoveEmptyEntries);
                if (parts.Length == 2 && int.TryParse(parts[0], out int x) && int.TryParse(parts[1], out int y))
                {
                    return new Point(x, y);
                }
                throw new ArgumentException("Point must be in format 'X,Y'");
            }
            return Convert.ChangeType(val, type, System.Globalization.CultureInfo.InvariantCulture);
        }

        private void ApplyValue(PathNode node, object parsedValue)
        {
            if (node.StatKey.HasValue)
            {
                if (node.Parent is Stats stats)
                {
                    stats[node.StatKey.Value] = (int)parsedValue;
                }
            }
            else if (node.Member != null)
            {
                if (node.Index.HasValue)
                {
                    if (node.Parent is Array arr)
                    {
                        arr.SetValue(parsedValue, node.Index.Value);
                    }
                    else if (node.Parent is IList list)
                    {
                        list[node.Index.Value] = parsedValue;
                    }
                }
                else
                {
                    if (node.Member is FieldInfo f)
                    {
                        f.SetValue(node.Parent, parsedValue);
                    }
                    else if (node.Member is PropertyInfo p)
                    {
                        p.SetValue(node.Parent, parsedValue);
                    }
                }
            }
        }

        private void SetAndPropagate(List<PathNode> path, object parsedValue)
        {
            var leaf = path.Last();
            ApplyValue(leaf, parsedValue);

            for (int i = path.Count - 1; i > 0; i--)
            {
                var current = path[i];
                var parentNode = path[i - 1];

                if (current.Parent != null && current.Parent.GetType().IsValueType)
                {
                    parentNode.Value = current.Parent;
                    ApplyValue(current, current.Parent);
                }
                else
                {
                    break;
                }
            }
        }

        private void NotifyAndSync(List<PathNode> path)
        {
            CharacterInfo charInfo = null;
            UserItem userItem = null;

            foreach (var node in path)
            {
                if (node.Value is CharacterInfo ci)
                {
                    charInfo = ci;
                }
                else if (node.Value is UserItem ui)
                {
                    userItem = ui;
                }
                else if (node.Parent is UserItem uiParent)
                {
                    userItem = uiParent;
                }
            }

            if (charInfo != null && charInfo.Player != null)
            {
                var player = charInfo.Player;

                var levelNode = path.FirstOrDefault(n => n.Member != null && string.Equals(n.Member.Name, "Level", StringComparison.OrdinalIgnoreCase));
                if (levelNode != null && levelNode.Parent == charInfo)
                {
                    player.Level = charInfo.Level;
                    player.LevelUp();
                }

                if (userItem != null)
                {
                    player.Enqueue(new ServerPackets.RefreshItem { Item = userItem });
                }

                player.RefreshStats();
            }

            var accountNode = path.FirstOrDefault(n => n.Value is AccountInfo);
            if (accountNode != null && accountNode.Value is AccountInfo accInfo)
            {
                foreach (var character in accInfo.Characters)
                {
                    if (character.Player != null)
                    {
                        var goldNode = path.FirstOrDefault(n => n.Member != null && (string.Equals(n.Member.Name, "Gold", StringComparison.OrdinalIgnoreCase) || string.Equals(n.Member.Name, "Credit", StringComparison.OrdinalIgnoreCase)));
                        if (goldNode != null && goldNode.Parent == accInfo)
                        {
                            character.Player.GetUserInfo(character.Player.Connection);
                        }
                    }
                }
            }
        }

        private void ShowObject(object obj, string pathPrefix, bool limitElements = true)
        {
            if (obj == null)
            {
                Print($"{pathPrefix}=null");
                return;
            }

            Type type = obj.GetType();

            if (type == typeof(byte) || type == typeof(byte[]) || type == typeof(byte?))
            {
                return;
            }

            if (IsSimpleType(type))
            {
                Print($"{pathPrefix}={obj}");
                return;
            }

            if (obj is Array arr)
            {
                if (type.GetElementType() == typeof(byte))
                {
                    return;
                }
                Print($"{pathPrefix}=Array[{arr.Length}]");
                int printed = 0;
                for (int i = 0; i < arr.Length; i++)
                {
                    var elem = arr.GetValue(i);
                    if (elem != null)
                    {
                        if (!limitElements || printed < 5)
                        {
                            Print($"  {pathPrefix}[{i}]={GetBriefDescription(elem)}");
                            printed++;
                        }
                        else
                        {
                            int remaining = 0;
                            for (int j = i; j < arr.Length; j++)
                            {
                                if (arr.GetValue(j) != null) remaining++;
                            }
                            Print($"  {pathPrefix}[...] (omitting {remaining} elements)");
                            break;
                        }
                    }
                }
                return;
            }

            if (obj is IList list)
            {
                Print($"{pathPrefix}=List[{list.Count}]");
                int printed = 0;
                for (int i = 0; i < list.Count; i++)
                {
                    var elem = list[i];
                    if (elem != null)
                    {
                        if (!limitElements || printed < 5)
                        {
                            Print($"  {pathPrefix}[{i}]={GetBriefDescription(elem)}");
                            printed++;
                        }
                        else
                        {
                            int remaining = 0;
                            for (int j = i; j < list.Count; j++)
                            {
                                if (list[j] != null) remaining++;
                            }
                            Print($"  {pathPrefix}[...] (omitting {remaining} elements)");
                            break;
                        }
                    }
                }
                return;
            }

            var fields = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
            foreach (var field in fields)
            {
                if (field.FieldType == typeof(byte) || field.FieldType == typeof(byte[]) || field.FieldType == typeof(byte?))
                {
                    continue;
                }

                string fieldPath = $"{pathPrefix}.{field.Name}";
                object val = field.GetValue(obj);

                if (val == null)
                {
                    Print($"{fieldPath}=null");
                }
                else if (obj is AccountInfo && (string.Equals(field.Name, "Password", StringComparison.OrdinalIgnoreCase) || string.Equals(field.Name, "StoragePassword", StringComparison.OrdinalIgnoreCase)))
                {
                    Print($"{fieldPath}=[Protected]");
                }
                else if (IsSimpleType(field.FieldType))
                {
                    Print($"{fieldPath}={val}");
                }
                else if (field.FieldType.IsArray)
                {
                    var fieldArr = (Array)val;
                    if (field.FieldType.GetElementType() == typeof(byte))
                    {
                        continue;
                    }
                    Print($"{fieldPath}=Array[{fieldArr.Length}]");
                    int printed = 0;
                    for (int i = 0; i < fieldArr.Length; i++)
                    {
                        var elem = fieldArr.GetValue(i);
                        if (elem != null)
                        {
                            if (printed < 5)
                            {
                                Print($"  {fieldPath}[{i}]={GetBriefDescription(elem)}");
                                printed++;
                            }
                            else
                            {
                                int remaining = 0;
                                for (int j = i; j < fieldArr.Length; j++)
                                {
                                    if (fieldArr.GetValue(j) != null) remaining++;
                                }
                                Print($"  {fieldPath}[...] (omitting {remaining} elements)");
                                break;
                            }
                        }
                    }
                }
                else if (typeof(IList).IsAssignableFrom(field.FieldType))
                {
                    var fieldList = (IList)val;
                    Print($"{fieldPath}=List[{fieldList.Count}]");
                    int printed = 0;
                    for (int i = 0; i < fieldList.Count; i++)
                    {
                        var elem = fieldList[i];
                        if (elem != null)
                        {
                            if (printed < 5)
                            {
                                Print($"  {fieldPath}[{i}]={GetBriefDescription(elem)}");
                                printed++;
                            }
                            else
                            {
                                int remaining = 0;
                                for (int j = i; j < fieldList.Count; j++)
                                {
                                    if (fieldList[j] != null) remaining++;
                                }
                                Print($"  {fieldPath}[...] (omitting {remaining} elements)");
                                break;
                            }
                        }
                    }
                }
                else if (val is Stats stats)
                {
                    Print($"{fieldPath}=Stats");
                    foreach (var kvp in stats.Values)
                    {
                        Print($"  {fieldPath}.{kvp.Key}={kvp.Value}");
                    }
                }
                else
                {
                    Print($"{fieldPath}={GetBriefDescription(val)}");
                }
            }

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            foreach (var prop in props)
            {
                if (prop.GetIndexParameters().Length > 0) continue;
                if (prop.PropertyType == typeof(byte) || prop.PropertyType == typeof(byte[]) || prop.PropertyType == typeof(byte?))
                {
                    continue;
                }

                string propPath = $"{pathPrefix}.{prop.Name}";
                object val;
                try
                {
                    val = prop.GetValue(obj);
                }
                catch
                {
                    continue;
                }

                if (val == null)
                {
                    Print($"{propPath}=null");
                }
                else if (obj is AccountInfo && (string.Equals(prop.Name, "Password", StringComparison.OrdinalIgnoreCase) || string.Equals(prop.Name, "StoragePassword", StringComparison.OrdinalIgnoreCase)))
                {
                    Print($"{propPath}=[Protected]");
                }
                else if (IsSimpleType(prop.PropertyType))
                {
                    Print($"{propPath}={val}");
                }
                else if (prop.PropertyType.IsArray)
                {
                    var propArr = (Array)val;
                    if (prop.PropertyType.GetElementType() == typeof(byte))
                    {
                        continue;
                    }
                    Print($"{propPath}=Array[{propArr.Length}]");
                    int printed = 0;
                    for (int i = 0; i < propArr.Length; i++)
                    {
                        var elem = propArr.GetValue(i);
                        if (elem != null)
                        {
                            if (printed < 5)
                            {
                                Print($"  {propPath}[{i}]={GetBriefDescription(elem)}");
                                printed++;
                            }
                            else
                            {
                                int remaining = 0;
                                for (int j = i; j < propArr.Length; j++)
                                {
                                    if (propArr.GetValue(j) != null) remaining++;
                                }
                                Print($"  {propPath}[...] (omitting {remaining} elements)");
                                break;
                            }
                        }
                    }
                }
                else if (typeof(IList).IsAssignableFrom(prop.PropertyType))
                {
                    var propList = (IList)val;
                    Print($"{propPath}=List[{propList.Count}]");
                    int printed = 0;
                    for (int i = 0; i < propList.Count; i++)
                    {
                        var elem = propList[i];
                        if (elem != null)
                        {
                            if (printed < 5)
                            {
                                Print($"  {propPath}[{i}]={GetBriefDescription(elem)}");
                                printed++;
                            }
                            else
                            {
                                int remaining = 0;
                                for (int j = i; j < propList.Count; j++)
                                {
                                    if (propList[j] != null) remaining++;
                                }
                                Print($"  {propPath}[...] (omitting {remaining} elements)");
                                break;
                            }
                        }
                    }
                }
                else if (val is Stats stats)
                {
                    Print($"{propPath}=Stats");
                    foreach (var kvp in stats.Values)
                    {
                        Print($"  {propPath}.{kvp.Key}={kvp.Value}");
                    }
                }
                else
                {
                    Print($"{propPath}={GetBriefDescription(val)}");
                }
            }
        }

        private bool IsSimpleType(Type type)
        {
            type = Nullable.GetUnderlyingType(type) ?? type;
            return type.IsPrimitive || type == typeof(string) || type.IsEnum || type == typeof(DateTime) || type == typeof(Point);
        }

        private string GetBriefDescription(object elem)
        {
            if (elem == null) return "(null)";
            if (elem is CharacterInfo ci) return $"Character: {ci.Name} (Level: {ci.Level})";
            if (elem is UserItem ui) return ui.Info != null ? ui.FriendlyName : $"Item {ui.ItemIndex}";
            if (elem is AccountInfo acc) return $"Account: {acc.AccountID}";
            return elem.GetType().Name;
        }

        private static List<string> GetGMCommandArgCompletions(string gmCmd, int argIndex, List<string> parts, List<string> onlinePlayers, List<string> allCharacters)
        {
            var options = new List<string>();
            switch (gmCmd)
            {
                case "@MAKE":
                    if (argIndex == 0)
                    {
                        options.AddRange(Envir.Main.ItemInfoList.Select(x => x.Name.Replace(" ", "")).Distinct());
                    }
                    break;

                case "@MOB":
                case "@RECALLMOB":
                    if (argIndex == 0)
                    {
                        options.AddRange(Envir.Main.MonsterInfoList.Select(x => x.Name.Replace(" ", "")).Distinct());
                    }
                    break;

                case "@MAPMOVE":
                case "@CLEARMOB":
                    if (argIndex == 0)
                    {
                        options.AddRange(Envir.Main.MapList.Select(x => x.Info.FileName).Distinct());
                    }
                    break;

                case "@GIVESKILL":
                case "@DELETESKILL":
                    if (argIndex == 0)
                    {
                        options.AddRange(onlinePlayers);
                        options.AddRange(Enum.GetNames(typeof(Spell)));
                    }
                    else if (argIndex == 1)
                    {
                        string firstArg = parts[3];
                        if (onlinePlayers.Contains(firstArg, StringComparer.OrdinalIgnoreCase))
                        {
                            options.AddRange(Enum.GetNames(typeof(Spell)));
                        }
                    }
                    break;

                case "@CHANGECLASS":
                    if (argIndex == 0)
                    {
                        options.AddRange(onlinePlayers);
                        options.AddRange(Enum.GetNames(typeof(MirClass)));
                    }
                    else if (argIndex == 1)
                    {
                        string firstArg = parts[3];
                        if (onlinePlayers.Contains(firstArg, StringComparer.OrdinalIgnoreCase))
                        {
                            options.AddRange(Enum.GetNames(typeof(MirClass)));
                        }
                    }
                    break;

                case "@AWAKENING":
                    if (argIndex == 0)
                    {
                        options.AddRange(Enum.GetNames(typeof(ItemType)));
                    }
                    else if (argIndex == 1)
                    {
                        options.AddRange(Enum.GetNames(typeof(AwakeType)));
                    }
                    break;

                case "@REMOVEAWAKENING":
                    if (argIndex == 0)
                    {
                        options.AddRange(Enum.GetNames(typeof(ItemType)));
                    }
                    break;

                case "@KILL":
                case "@RECALL":
                case "@OBSERVE":
                case "@RECALLMEMBER":
                case "@GOTO":
                case "@GIVEGOLD":
                case "@GIVEPEARLS":
                case "@GIVECREDIT":
                case "@REVIVE":
                case "@ADJUSTPKPOINT":
                    if (argIndex == 0)
                    {
                        options.AddRange(onlinePlayers);
                    }
                    break;

                case "@BACKUPPLAYER":
                case "@ARCHIVEPLAYER":
                case "@LOADPLAYER":
                case "@RESTOREPLAYER":
                case "@CHANGEGENDER":
                    if (argIndex == 0)
                    {
                        options.AddRange(allCharacters);
                    }
                    break;
            }
            return options;
        }
    }
}
