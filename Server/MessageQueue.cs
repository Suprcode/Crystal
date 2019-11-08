using System;
using System.IO;
using System.Collections.Concurrent;

namespace Server
{
    public class MessageQueue
    {
        private static readonly MessageQueue instance = new MessageQueue();

        public static MessageQueue Instance
        {
            get { return instance; }
        }

        public readonly ConcurrentQueue<string> MessageLog = new ConcurrentQueue<string>();
        public readonly ConcurrentQueue<string> DebugLog = new ConcurrentQueue<string>();
        public readonly ConcurrentQueue<string> ChatLog = new ConcurrentQueue<string>();

        public MessageQueue()
        {

        }

        public void Enqueue(Exception ex)
        {
            if (MessageLog.Count < 100)
                MessageLog.Enqueue(String.Format("[{0}]: {1} - {2}" + Environment.NewLine, DateTime.Now, ex.TargetSite, ex));
            File.AppendAllText(Settings.LogPath + "Log (" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ").txt",
                               String.Format("[{0}]: {1} - {2}" + Environment.NewLine, DateTime.Now, ex.TargetSite, ex));
        }

        public void EnqueueDebugging(string msg)
        {
            if (DebugLog.Count < 100)
                DebugLog.Enqueue(String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
            File.AppendAllText(Settings.LogPath + "DebugLog (" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ").txt",
                               String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
        }

        public void EnqueueChat(string msg)
        {
            if (ChatLog.Count < 100)
                ChatLog.Enqueue(String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
            File.AppendAllText(Settings.LogPath + "ChatLog (" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ").txt",
                               String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
        }

        public void Enqueue(string msg)
        {
            if (MessageLog.Count < 100)
                MessageLog.Enqueue(String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
            File.AppendAllText(Settings.LogPath + "Log (" + DateTime.Now.Date.ToString("dd-MM-yyyy") + ").txt",
                               String.Format("[{0}]: {1}" + Environment.NewLine, DateTime.Now, msg));
        }
    }
}
