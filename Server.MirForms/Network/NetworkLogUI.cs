using System;
using System.Collections.Concurrent;
using System.Text;
using System.Windows.Forms;
using Server.MirEnvir;

namespace Server
{
    public static class NetworkLogUI
    {
        private static bool subscribed;
        private static readonly ConcurrentQueue<string> goodQueue = new ConcurrentQueue<string>();
        private static readonly ConcurrentQueue<string> blockedQueue = new ConcurrentQueue<string>();
        private static readonly ConcurrentQueue<string> unknownQueue = new ConcurrentQueue<string>();

        public static void Subscribe()
        {
            if (subscribed) return;
            subscribed = true;

            Envir.BlockedIpEvent += (ip, remote, local, reason) =>
            {
                blockedQueue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [BLOCKED] {remote} -> {local} ip={ip} reason={reason}\r\n");
            };
            Envir.TempBlockedIpEvent += (ip, remote, local, reason) =>
            {
                blockedQueue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [TEMP BLOCK] {remote} -> {local} ip={ip} reason={reason}\r\n");
            };
            Envir.GoodConnectionEvent += (ip, remote, local) =>
            {
                goodQueue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [GOOD] {remote} -> {local} ip={ip}\r\n");
            };
            Envir.UnknownConnectionEvent += (ip, remote, local, reason) =>
            {
                unknownQueue.Enqueue($"{DateTime.Now:yyyy-MM-dd HH:mm:ss} [UNKNOWN] {remote} -> {local} ip={ip} reason={reason}\r\n");
            };
        }

        public static void DrainTo(TextBox goodBox, TextBox blockedBox, TextBox unknownBox, Control invoker)
        {
            if (goodBox == null && blockedBox == null && unknownBox == null) return;

            void Append(TextBox box, ConcurrentQueue<string> q)
            {
                if (box == null) return;
                var sb = new StringBuilder();
                int n = 0;
                while (q.TryDequeue(out var line) && n++ < 1000)
                    sb.Append(line);
                if (sb.Length == 0) return;
                if (box.IsDisposed) return;
                try
                {
                    if (invoker != null && invoker.InvokeRequired)
                    {
                        invoker.BeginInvoke(new Action(() => box.AppendText(sb.ToString())));
                    }
                    else
                    {
                        box.AppendText(sb.ToString());
                    }
                }
                catch { }
            }

            Append(goodBox, goodQueue);
            Append(blockedBox, blockedQueue);
            Append(unknownBox, unknownQueue);
        }
    }
}

