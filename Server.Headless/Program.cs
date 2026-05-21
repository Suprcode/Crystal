using System;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Threading.Tasks;
using log4net;
using log4net.Config;
using Server;
using Server.MirEnvir;

namespace Server.Headless
{
    class Program
    {
        static async Task Main(string[] args)
        {
            Packet.IsServer = true;

            // Configure log4net
            var logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            var host = new ConsoleServerHost();
            Envir.Initialise(host);

            host.Log("Starting Legend of Mir Crystal Server (Headless Mode)...");

            try
            {
                Settings.Load();

                host.Log("Loading Database...");
                bool dbLoaded = Envir.Edit.LoadDB();
                if (!dbLoaded)
                {
                    host.Log("[CRITICAL] Failed to load server database. Exiting.");
                    return;
                }

                host.Log("Starting Server Environment...");
                Envir.Main.Start();

                host.Log("Server successfully started. Press Ctrl+C or type 'exit' to stop.");

                var cts = new CancellationTokenSource();
                Console.CancelKeyPress += (sender, e) =>
                {
                    e.Cancel = true; // Prevent immediate termination
                    host.Log("Shutdown signal received. Commencing graceful termination...");
                    cts.Cancel();
                };

                var commandProcessor = new CommandProcessor(host, cts);
                await commandProcessor.RunAsync();

                host.Log("Stopping Server Environment...");
                Envir.Main.Stop();

                host.Log("Saving configurations and database state...");
                Settings.Save();

                host.Log("Server shutdown complete.");
            }
            catch (Exception ex)
            {
                Logger.GetLogger().Error("Fatal execution error in Headless server host", ex);
                host.Log($"[FATAL] {ex}");
            }
        }
    }
}
