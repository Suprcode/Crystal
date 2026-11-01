using System.Net;
using System.Net.Sockets;

namespace Server.MirNetwork
{
    public static class NetUtil
    {
        /// <summary>
        /// Converts IPv4-mapped IPv6 addresses (::ffff:1.2.3.4) back to IPv4.
        /// </summary>
        public static IPAddress NormalizeAddress(IPAddress address)
        {
            if (address == null) return null;

            return address.IsIPv4MappedToIPv6 ? address.MapToIPv4() : address;
        }

        /// <summary>
        /// Client address used for logs and account records.
        /// </summary>
        public static string ClientIP(TcpClient client)
        {
            return client?.Client?.RemoteEndPoint is IPEndPoint endPoint
                ? ClientIP(endPoint)
                : "unknown";
        }

        public static string ClientIP(IPEndPoint endPoint)
        {
            return endPoint == null ? "unknown" : NormalizeAddress(endPoint.Address).ToString();
        }

        /// <summary>
        /// Key used for bans and connection limits. IPv6 is grouped by /64 so changing
        /// the host part does not bypass limits. IPv4 keeps the full address.
        /// </summary>
        public static string AccessKey(TcpClient client)
        {
            return client?.Client?.RemoteEndPoint is IPEndPoint endPoint
                ? AccessKey(endPoint.Address)
                : "unknown";
        }

        public static string AccessKey(IPAddress address)
        {
            address = NormalizeAddress(address);

            if (address == null) return "unknown";
            if (address.AddressFamily != AddressFamily.InterNetworkV6) return address.ToString();

            byte[] bytes = address.GetAddressBytes();
            Array.Clear(bytes, 8, 8);

            return new IPAddress(bytes).ToString() + "/64";
        }

        /// <summary>
        /// Starts a listener on the configured address. IPv6Any enables dual mode so the
        /// port accepts both IPv4 and IPv6. Falls back to IPv6-only if unsupported.
        /// </summary>
        public static TcpListener StartListener(IPAddress address, int port)
        {
            TcpListener listener = new TcpListener(address, port);

            if (address.Equals(IPAddress.IPv6Any))
            {
                try
                {
                    listener.Server.DualMode = true;
                }
                catch (Exception ex) when (ex is SocketException or PlatformNotSupportedException or NotSupportedException)
                {
                    // Keep IPv6-only; the actual mode is reported by the startup log.
                }
            }

            listener.Start();
            return listener;
        }
    }
}
