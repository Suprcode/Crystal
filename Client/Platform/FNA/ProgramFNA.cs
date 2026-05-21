using System;
using Client.Platform.FNA;

namespace Client
{
    public static class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {

            // Parse command-line flags
            if (args.Length > 0)
            {
                foreach (var arg in args)
                {
                    if (arg.ToLower() == "-tc") Settings.UseTestConfig = true;
                }
            }

            #if DEBUG
                Settings.UseTestConfig = true;
            #endif

            // Critical: Tell the packet system this is a client, not a server.
            // Without this, received packets are deserialized as client-to-server
            // packets instead of server-to-client packets, causing protocol errors.
            Packet.IsServer = false;

            // Load client configuration (network IP/Port, graphics, sound, etc.)
            Settings.Load();

            // Run cross-platform headless update check before launching the game shell
            if (Settings.P_Patcher)
            {
                Console.WriteLine("[Launcher] Auto-updater is enabled. Initializing headless patch check...");
                try
                {
                    var patcher = new Launcher.HeadlessPatcher();

                    // Synchronously run the headless patcher to completion
                    bool patchSuccess = patcher.RunAsync().GetAwaiter().GetResult();
                    if (!patchSuccess)
                    {
                        Console.WriteLine("[Launcher] Headless patching flow failed. Aborting game execution.");
                        Environment.Exit(1);
                    }
                    Console.WriteLine("[Launcher] Headless patching flow completed successfully.");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"[Launcher] Critical error occurred during patching: {ex}");
                    Environment.Exit(1);
                }
            }

            // 1. Initialize our virtual filesystem mapping (case-insensitivity support)
            var assetResolver = new AssetResolver();
            
            // 2. Register global platform hooks
            Client.MirGraphics.DXManager.AssetResolver = assetResolver;

            // 3. Launch the cross-platform OpenGL game shell
            using (var game = new FNAEntry())
            {
                game.Run();
            }
        }
    }
}

