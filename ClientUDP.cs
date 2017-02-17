using System;
using System.Net;
using System.Net.Sockets;
using Terraria.ModLoader;
using System.Runtime;

namespace SocketTest
{
    class ClientUDP : IDisposable
    {
        byte[] m_dataBuffer = new byte[256];
        public Socket socketUDP;
        private IPEndPoint endPoint;

        private SocketTest socketMod;

        private static String targetKey;

        public static void SetTargetKey(String target)
        {
            targetKey = target;
        }

        public ClientUDP(SocketTest mod)
        {
            socketMod = mod;
        }

        public void setEndPointIP(String name)
        {
            IPAddress ip = IPAddress.Parse(name);
            endPoint.Address = ip;
        }

        public void setEndPointPort(int port)
        {
            endPoint.Port = port;
        }

        public void CloseSocket()
        {
            if (socketUDP != null)
            {
                socketUDP.Shutdown(SocketShutdown.Both);
                socketUDP = null;
            }
        }

        public void OpenSocket(String name, int port)
        {
            // Create the socket instance
            socketUDP = new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);

            //// Cet the remote IP address
            IPAddress ip = IPAddress.Parse(name);
            //// Create the end point 
            endPoint = new IPEndPoint(ip, port);
        }

        public void SendMessage(Packet p)
        {
            dynamic d = p;
            SendDynamic(d);
        }

        private void SendDynamic(dynamic p)
        {
            p.target = targetKey;
            SendString(p.serialize());
        }

        private void SendString(String message)
        {
            try
            {
                Object objData = message;
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());
                if (socketUDP != null)
                {
                    socketUDP.SendTo(byData, endPoint);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }

        public void BroadcastMessage(String message)
        {
            try
            {
                Object objData = message;
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());
                if (socketUDP != null)
                {
                    socketUDP.Send(byData);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }

        public void Dispose()
        {
            ((IDisposable)socketUDP).Dispose();
        }
    }
}
