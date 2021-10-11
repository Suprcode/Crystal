using System;
using System.Collections.Generic;
using System.Text;

namespace WYMTool
{
    class LogManager
    {
        private static LogManager _instance = null;
        private static object _instanceLock = new object();
        public static LogManager Instance()
        {
            if (_instance == null) //双if +lock
            {
                lock (_instanceLock)
                {
                    if (_instance == null)
                    {
                        _instance = new LogManager();
                    }
                }
            }
            return _instance;
        }

        public void Log(string msg, bool needTag = true)
        {
            // TODO 配置IsDebug
            if (true)
            {
                string path = ConfigManager.Instance().PathConfig + "Log.txt";
                System.IO.File.AppendAllText(@path, (needTag ? "wym---" : "") + msg + "\r\n");
            }
        }
    }
}
