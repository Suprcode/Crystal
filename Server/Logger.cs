using System.Reflection;
using log4net;

namespace Server.Library {
    public enum LogType {
        Server,
        Chat,
        Debug,
        Player,
        Spawn
    }

    public class Logger {
        public static ILog GetLogger(LogType type = LogType.Server) {
            return LogManager.GetLogger(Assembly.GetEntryAssembly(), type.ToString());
        }
    }
}
