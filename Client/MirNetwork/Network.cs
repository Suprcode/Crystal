using System.Collections.Concurrent;
using System.Net.Sockets;
using Client.MirControls;
using C = ClientPackets;


namespace Client.MirNetwork
{
    static class Network
    {
        private static TcpClient _client;
        public static int ConnectAttempt = 0;
        public static int MaxAttempts = 20;
        public static bool ErrorShown;
        public static bool Connected;
        public static long TimeOutTime, TimeConnected, RetryTime = CMain.Time + 5000;

        private static ConcurrentQueue<Packet> _receiveList;
        private static ConcurrentQueue<Packet> _sendList;

        static byte[] _rawData = new byte[0];
        static readonly byte[] _rawBytes = new byte[8 * 1024];

        public static void Connect()
        {
            if (_client != null)
                Disconnect();

            if (ConnectAttempt >= MaxAttempts)
            {
                if (ErrorShown)
                {
                    return;
                }

                ErrorShown = true;

                MirMessageBox errorBox = new("Error Connecting to Server", MirMessageBoxButtons.Cancel);
                errorBox.CancelButton.Click += (o, e) => Program.Form.Close();
                errorBox.Label.Text = $"Maximum Connection Attempts Reached: {MaxAttempts}" +
                                      $"{Environment.NewLine}Please try again later or check your connection settings.";
                errorBox.Show();
                return;
            }

            ConnectAttempt++;

            try
            {
                _client = new TcpClient { NoDelay = true };
                _client?.BeginConnect(Settings.IPAddress, Settings.Port, Connection, null);
            }
            catch (ObjectDisposedException ex)
            {
                if (Settings.LogErrors) CMain.SaveError(ex.ToString());
                Disconnect();
            }
        }

        private static void Connection(IAsyncResult result)
        {
            try
            {
                _client?.EndConnect(result);

                if ((_client != null &&
                    !_client.Connected) ||
                    _client == null)
                {
                    Connect();
                    return;
                }

                _receiveList = new ConcurrentQueue<Packet>();
                _sendList = new ConcurrentQueue<Packet>();
                _rawData = new byte[0];

                TimeOutTime = CMain.Time + Settings.TimeOut;
                TimeConnected = CMain.Time;

                BeginReceive();
            }
            catch (SocketException)
            {
                Thread.Sleep(100);
                Connect();
            }
            catch (Exception ex)
            {
                if (Settings.LogErrors) CMain.SaveError(ex.ToString());
                Disconnect();
            }
        }

        private static void BeginReceive()
        {
            if (_client == null || !_client.Connected) return;

            try
            {
                _client.Client.BeginReceive(_rawBytes, 0, _rawBytes.Length, SocketFlags.None, ReceiveData, _rawBytes);
            }
            catch
            {
                Disconnect();
            }
        }
        private static void ReceiveData(IAsyncResult result)
        {
            if (_client == null || !_client.Connected) return;

            int dataRead;

            try
            {
                dataRead = _client.Client.EndReceive(result);
            }
            catch
            {
                Disconnect();
                return;
            }

            if (dataRead == 0)
            {
                Disconnect();
            }

            byte[] rawBytes = result.AsyncState as byte[];

            byte[] temp = _rawData;
            _rawData = new byte[dataRead + temp.Length];
            Buffer.BlockCopy(temp, 0, _rawData, 0, temp.Length);
            Buffer.BlockCopy(rawBytes, 0, _rawData, temp.Length, dataRead);

            Packet p;
            List<byte> data = new List<byte>();

            while ((p = Packet.ReceivePacket(_rawData, out _rawData)) != null)
            {
                data.AddRange(p.GetPacketBytes());
                _receiveList.Enqueue(p);
            }

            CMain.BytesReceived += data.Count;

            BeginReceive();
        }

        private static void BeginSend(List<byte> data)
        {
            if (_client == null || !_client.Connected || data.Count == 0) return;
            
            try
            {
                _client.Client.BeginSend(data.ToArray(), 0, data.Count, SocketFlags.None, SendData, null);
            }
            catch
            {
                Disconnect();
            }
        }
        private static void SendData(IAsyncResult result)
        {
            try
            {
                _client.Client.EndSend(result);
            }
            catch
            { }
        }

        public static void Disconnect()
        {
            if (_client == null) return;

            _client?.Close();

            TimeConnected = 0;
            Connected = false;
            _sendList = null;
            _client = null;

            _receiveList = null;
        }

        public static void Process()
        {
            if (_client == null || !_client.Connected)
            {
                if (Connected)
                {
                    while (_receiveList != null && !_receiveList.IsEmpty)
                    {
                        if (!_receiveList.TryDequeue(out Packet p) || p == null) continue;
                        if (!(p is ServerPackets.Disconnect) && !(p is ServerPackets.ClientVersion)) continue;

                        MirScene.ActiveScene.ProcessPacket(p);
                        _receiveList = null;
                        return;
                    }

                    MirMessageBox.Show("Lost connection with the server.", true);
                    Disconnect();
                    return;
                }
                else if (CMain.Time >= RetryTime)
                {
                    RetryTime = CMain.Time + 5000;
                    Connect();
                }
                return;
            }

            if (!Connected && TimeConnected > 0 && CMain.Time > TimeConnected + 5000)
            {
                Disconnect();
                Connect();
                return;
            }



            while (_receiveList != null && !_receiveList.IsEmpty)
            {
                if (!_receiveList.TryDequeue(out Packet p) || p == null) continue;
                MirScene.ActiveScene.ProcessPacket(p);
            }


            if (CMain.Time > TimeOutTime && _sendList != null && _sendList.IsEmpty)
                _sendList.Enqueue(new C.KeepAlive());

            if (_sendList == null || _sendList.IsEmpty) return;

            TimeOutTime = CMain.Time + Settings.TimeOut;

            List<byte> data = new List<byte>();
            while (!_sendList.IsEmpty)
            {
                if (!_sendList.TryDequeue(out Packet p)) continue;
                data.AddRange(p.GetPacketBytes());
            }

            CMain.BytesSent += data.Count;

            BeginSend(data);
        }
        
        public static void Enqueue(Packet p)
        {
            if (_sendList != null && p != null)
                _sendList.Enqueue(p);
        }
    }
}
