using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            "reload", "say", "broadcast", "list", "kick", "blockedips", "player", "ipban", "ipunban", "gm"
        };

        private static readonly string[] ReloadSubcommands = new[] { "npc", "drops", "line", "all" };
        private static readonly string[] ListSubcommands = new[] { "players", "guilds" };
        private static readonly string[] BlockedIpsSubcommands = new[] { "list", "clear", "add", "remove" };
        private static readonly string[] PlayerSubcommands = new[]
        {
            "status", "info", "edit", "message", "kick", "kill", "killpets", "safezone",
            "chatban", "chatunban", "ban", "unban", "flag"
        };
        private static readonly string[] PlayerEditStats = new[] { "level", "gold", "credit", "pk" };

        public static readonly object ConsoleLock = new object();
        public static string CurrentInput { get; set; } = "";
        public static int CursorPosition { get; set; } = 0;
        public static bool IsReadingInput { get; set; } = false;

        public static void ClearInputLine()
        {
            lock (ConsoleLock)
            {
                if (!IsReadingInput) return;
                int length = 2 + (CurrentInput?.Length ?? 0);
                Console.Write("\r" + new string(' ', length) + "\r");
            }
        }

        public static void RedrawInputLine()
        {
            lock (ConsoleLock)
            {
                if (!IsReadingInput) return;
                Console.Write("> " + CurrentInput);
                int backspaces = (CurrentInput?.Length ?? 0) - CursorPosition;
                for (int i = 0; i < backspaces; i++)
                {
                    Console.Write("\b");
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
                const int colWidth = 16;
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
                    Console.Write(option.PadRight(colWidth));
                    lineLen += colWidth;
                }
                Console.WriteLine();
                Console.Write("> " + CurrentInput);
                int backspaces = CurrentInput.Length - CursorPosition;
                for (int i = 0; i < backspaces; i++)
                {
                    Console.Write("\b");
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
                            buffer.Remove(cursor, 1);
                            CurrentInput = buffer.ToString();
                            CursorPosition = cursor;

                            Console.Write("\b \b");
                            if (cursor < buffer.Length)
                            {
                                string remaining = buffer.ToString().Substring(cursor);
                                Console.Write(remaining + " ");
                                for (int i = 0; i <= remaining.Length; i++)
                                {
                                    Console.Write("\b");
                                }
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
                            int currentLength = buffer.Length;
                            for (int i = 0; i < currentLength; i++)
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
                        int currentLength = buffer.Length;
                        for (int i = 0; i < currentLength; i++)
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
                                int currentLength = buffer.Length;
                                for (int i = 0; i < currentLength; i++)
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
                            int currentLength = buffer.Length;
                            for (int i = 0; i < currentLength; i++)
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
                            Console.Write("\b");
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
                            for (int i = 0; i < remaining.Length; i++)
                            {
                                Console.Write("\b");
                            }
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
            }

            return completions;
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
    }
}
