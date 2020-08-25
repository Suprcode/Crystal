using log4net;
using System.Reflection;

namespace Server
{
    public enum LogType
    {
        Server,
        Chat,
        Debug,
        Player,
        Spawn
    }

    public class Logger
    {
        public static ILog GetLogger(LogType type = LogType.Server)
        {
            return LogManager.GetLogger(Assembly.GetEntryAssembly(), type.ToString());
        }
    }
}
