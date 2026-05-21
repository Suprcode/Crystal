using System;
using Server;

namespace Server.Headless
{
    public class ConsoleServerHost : IServerHost
    {
        public bool IsRunning { get; private set; } = true;

        public void Log(string msg)
        {
            lock (CommandProcessor.ConsoleLock)
            {
                CommandProcessor.ClearInputLine();
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {msg}");
                CommandProcessor.RedrawInputLine();
            }
        }

        public void UpdatePlayerCount(int count)
        {
            lock (CommandProcessor.ConsoleLock)
            {
                CommandProcessor.ClearInputLine();
                Console.WriteLine($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] Active Players: {count}");
                CommandProcessor.RedrawInputLine();
            }
        }

        public void Shutdown()
        {
            IsRunning = false;
        }
    }
}
