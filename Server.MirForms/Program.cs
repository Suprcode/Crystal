using System.Reflection;
using log4net;
using log4net.Config;
using log4net.Repository;
using Server.Library;
using Shared;

namespace Server {
    internal static class Program {
        /// <summary>
        ///     The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main() {
            Packet.IsServer = true;

            ILoggerRepository logRepository = LogManager.GetRepository(Assembly.GetEntryAssembly());
            XmlConfigurator.Configure(logRepository, new FileInfo("log4net.config"));

            try {
                Settings.Load();

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new SMain());

                Settings.Save();
            } catch(Exception ex) {
                Logger.GetLogger().Error(ex);
            }
        }
    }
}
