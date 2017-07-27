using System;
using System.Net;
using System.Net.Sockets;

namespace Unity
{
    public static class NTP
    {
        public static DateTime GetNetworkTime(string ntp_server)
        {
            string hostNameOrAddress = ntp_server;
            byte[] buffer = new byte[48];
            buffer[0] = 27;
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            socket.Connect(new IPEndPoint(Dns.GetHostEntry(hostNameOrAddress).AddressList[0], 123));
            socket.ReceiveTimeout = 3000;
            socket.Send(buffer);
            socket.Receive(buffer);
            socket.Close();
            return new DateTime(1900, 1, 1, 0, 0, 0, DateTimeKind.Utc).AddMilliseconds((long)(SwapEndianness(BitConverter.ToUInt32(buffer, 40)) * 1000UL + SwapEndianness(BitConverter.ToUInt32(buffer, 44)) * 1000UL / 4294967296UL)).ToLocalTime();
        }

        private static uint SwapEndianness(ulong x)
        {
            return (uint)((ulong)((((long)x & byte.MaxValue) << 24) + (((long)x & 65280L) << 8)) + ((x & 16711680UL) >> 8) + ((x & 4278190080UL) >> 24));
        }
    }
}
