using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net.Sockets;
using Godot;
using ClientPackets;
using ServerPackets;

namespace ClientGodot.Scripts
{
    public static class NetworkManager
    {
        private static TcpClient _client;
        public static bool Connected;
        public static long TimeOutTime, TimeConnected;

        private static ConcurrentQueue<Packet> _receiveList;
        private static ConcurrentQueue<Packet> _sendList;

        static byte[] _rawData = new byte[0];
        static readonly byte[] _rawBytes = new byte[8 * 1024];

        // Hardcoded settings for sample
        private const string IPAddress = "127.0.0.1";
        private const int Port = 7000;
        private const int TimeOut = 5000;

        public static void Connect()
        {
            if (_client != null)
                Disconnect();

            GD.Print($"Attempting to connect to {IPAddress}:{Port}...");

            try
            {
                _client = new TcpClient { NoDelay = true };
                _client.BeginConnect(IPAddress, Port, Connection, null);
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Connection exception: {ex}");
                Disconnect();
            }
        }

        private static void Connection(IAsyncResult result)
        {
            try
            {
                if (_client == null) return;
                _client.EndConnect(result);

                if (!_client.Connected)
                {
                    GD.Print("Failed to connect.");
                    return;
                }

                _receiveList = new ConcurrentQueue<Packet>();
                _sendList = new ConcurrentQueue<Packet>();
                _rawData = new byte[0];

                TimeConnected = System.Environment.TickCount64;
                TimeOutTime = TimeConnected + TimeOut;
                Connected = true;

                GD.Print("Connected to server!");

                BeginReceive();
            }
            catch (Exception ex)
            {
                GD.PrintErr($"EndConnect exception: {ex}");
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
                return;
            }

            byte[] rawBytes = result.AsyncState as byte[];

            // Handle data concatenation
            byte[] temp = _rawData;
            _rawData = new byte[dataRead + temp.Length];
            Buffer.BlockCopy(temp, 0, _rawData, 0, temp.Length);
            Buffer.BlockCopy(rawBytes, 0, _rawData, temp.Length, dataRead);

            Packet p;

            try
            {
                while ((p = Packet.ReceivePacket(_rawData, out _rawData)) != null)
                {
                    _receiveList.Enqueue(p);
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"Packet Receive Error: {ex}");
                Disconnect();
                return;
            }

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
            catch { }
        }

        public static void Disconnect()
        {
            if (_client == null) return;
            _client.Close();
            _client = null;
            Connected = false;
            _receiveList = null;
            _sendList = null;
            GD.Print("Disconnected.");
        }

        public static void Process()
        {
            if (_client == null || !_client.Connected)
            {
                return;
            }

            long currentTime = System.Environment.TickCount64;

            while (_receiveList != null && !_receiveList.IsEmpty)
            {
                if (!_receiveList.TryDequeue(out Packet p) || p == null) continue;

                // GD.Print($"Packet Received: {p.GetType().Name} (Index: {p.Index})");

                // Handle Global Packets
                if (p is ServerPackets.Connected)
                {
                    GD.Print("Server: Connected packet received. Sending ClientVersion...");
                    Enqueue(new ClientPackets.ClientVersion { VersionHash = new byte[0] });
                    continue;
                }

                if (p is ServerPackets.ClientVersion)
                {
                    GD.Print("Server: ClientVersion OK.");
                    // In real client, we might reload if version mismatch
                    continue;
                }

                if (p is ServerPackets.Disconnect)
                {
                     Disconnect();
                     return;
                }

                // Forward to Active Scene
                if (SceneManager.ActiveScene != null)
                {
                    SceneManager.ActiveScene.ProcessPacket(p);
                }
            }

            if (currentTime > TimeOutTime)
            {
                if (_sendList != null && _sendList.IsEmpty)
                {
                     Enqueue(new ClientPackets.KeepAlive());
                }
            }

            if (_sendList == null || _sendList.IsEmpty) return;

            TimeOutTime = currentTime + TimeOut;

            List<byte> data = new List<byte>();
            while (!_sendList.IsEmpty)
            {
                if (!_sendList.TryDequeue(out Packet p)) continue;
                data.AddRange(p.GetPacketBytes());
            }

            BeginSend(data);
        }

        public static void Enqueue(Packet p)
        {
            if (_sendList != null && p != null)
                _sendList.Enqueue(p);
        }
    }
}
