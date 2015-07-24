using System;
using System.Reflection;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Windows.Forms;

namespace Server.MirNetwork
{
    public class MirStatusConnection
    {
        public readonly string IPAddress;

        public GameStage Stage;

        private TcpClient _client;
        //private ConcurrentQueue<Packet> _receiveList;
        //private Queue<Packet> _sendList, _retryList;
        private long LastSendTime;

        private bool _disconnecting;
        public bool Connected;
        public bool Disconnecting
        {
            get { return _disconnecting; }
            set
            {
                if (_disconnecting == value) return;
                _disconnecting = value;
                TimeOutTime = SMain.Envir.Time + 500;
            }
        }
        public readonly long TimeConnected;
        public long TimeDisconnected, TimeOutTime;

        byte[] _rawData = new byte[0];


        public MirStatusConnection( TcpClient client)
        {
            IPAddress = client.Client.RemoteEndPoint.ToString().Split(':')[0];

            _client = client;
            _client.NoDelay = true;

            TimeConnected = SMain.Envir.Time;
            TimeOutTime = TimeConnected + Settings.TimeOut;
            LastSendTime = SMain.Envir.Time;
            Connected = true;
            BeginReceive();
        }

        private void BeginReceive()
        {
            if (!Connected) return;

            byte[] rawBytes = new byte[8 * 1024];

            try
            {
                _client.Client.BeginReceive(rawBytes, 0, rawBytes.Length, SocketFlags.None, ReceiveData, rawBytes);
            }
            catch
            {
                Disconnecting = true;
            }
        }
        private void ReceiveData(IAsyncResult result)
        {
            if (!Connected) return;

            int dataRead;

            try
            {
                dataRead = _client.Client.EndReceive(result);
            }
            catch
            {
                Disconnecting = true;
                return;
            }

            if (dataRead == 0)
            {
                Disconnecting = true;
                return;
            }
            BeginReceive();
        }
        private void BeginSend(byte[] data)
        {
            if (!Connected || data.Length == 0) return;

            //Interlocked.Add(ref Network.Sent, data.Count);

            try
            {
                _client.Client.BeginSend(data, 0, data.Length, SocketFlags.None, SendData, Disconnecting);
            }
            catch
            {
                Disconnecting = true;
            }
        }
        private void SendData(IAsyncResult result)
        {
            try
            {
                _client.Client.EndSend(result);
            }
            catch
            { }
        }

        public void Process()
        {
            if (_client == null || !_client.Connected)
            {
                Disconnect();
                return;
            }
            TimeOutTime = SMain.Envir.Time + Settings.TimeOut;
            if ((SMain.Envir.Time > TimeOutTime) || Disconnecting)
            {
                Disconnect();
                return;
            }

            //if (_sendList == null || _sendList.Count <= 0) return;

            if (SMain.Envir.Time - LastSendTime > 10 * 1000)
            {
                LastSendTime = SMain.Envir.Time;
                string output = string.Format("c;/{0}/{1}/{2}/{3}//;", "NoName",SMain.Envir.PlayerCount,"CrystalM2", Application.ProductVersion);
                byte[] data = Encoding.ASCII.GetBytes(output);
                BeginSend(data);
            }
        }
        public void Disconnect()
        {
            if (!Connected) return;

            Connected = false;
            Stage = GameStage.Disconnected;
            TimeDisconnected = SMain.Envir.Time;

            lock (SMain.Envir.StatusConnections)
                SMain.Envir.StatusConnections.Remove(this);
            /*
            _sendList = null;
            _receiveList = null;
            _retryList = null;
            */
            _rawData = null;

            if (_client != null) _client.Client.Dispose();
            _client = null;
        }
        public void SendDisconnect()
        {
            Disconnecting = true;
        }
    }
}
