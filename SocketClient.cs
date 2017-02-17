using System;
using System.Net;
using System.Net.Sockets;
using Terraria.ModLoader;

namespace SocketTest
{
    class SocketClient
    {
        byte[] m_dataBuffer = new byte[10];
        IAsyncResult m_result;
        public AsyncCallback m_pfnCallBack;
        public Socket m_clientSocket;
        
        private SocketTest socketMod;

        public SocketClient(SocketTest mod)
        {
            socketMod = mod;
        }

        public void CloseConnection()
        {
            if (m_clientSocket != null)
            {
                m_clientSocket.Shutdown(SocketShutdown.Both);
                m_clientSocket = null;
            }
        }

        public void OpenConnection(String hostName, int port)
        {
            try
            {
                // Create the socket instance
                m_clientSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

                // Cet the remote IP address
                IPAddress ip = IPAddress.Parse(hostName);
                // Create the end point 
                IPEndPoint ipEnd = new IPEndPoint(ip, port);
                // Connect to the remote host
                m_clientSocket.Connect(ipEnd);
                if (m_clientSocket.Connected)
                {
                    //Wait for data asynchronously 
                    WaitForData();
                }
            }
            catch (SocketException se)
            {
                string str;
                str = "\nConnection failed, is the server running?\n" + se.Message;
                Console.WriteLine(str);
            }
        }

        public void SendMessage(String message)
        {
            try
            {
                Object objData = message;
                byte[] byData = System.Text.Encoding.ASCII.GetBytes(objData.ToString());
                if (m_clientSocket != null)
                {
                    m_clientSocket.Send(byData);
                }
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }

        public void WaitForData()
        {
            try
            {
                if (m_pfnCallBack == null)
                {
                    m_pfnCallBack = new AsyncCallback(OnDataReceived);
                }
                SocketPacket theSocPkt = new SocketPacket();
                theSocPkt.thisSocket = m_clientSocket;
                // Start listening to the data asynchronously
                if(m_clientSocket != null)
                    m_result = m_clientSocket.BeginReceive(theSocPkt.dataBuffer,
                                                            0, theSocPkt.dataBuffer.Length,
                                                            SocketFlags.None,
                                                            m_pfnCallBack,
                                                            theSocPkt);
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }

        }

        public class SocketPacket
        {
            public System.Net.Sockets.Socket thisSocket;
            public byte[] dataBuffer = new byte[256];
        }

        public void OnDataReceived(IAsyncResult asyn)
        {
            try
            {
                SocketPacket theSockId = (SocketPacket)asyn.AsyncState;
                int iRx = theSockId.thisSocket.EndReceive(asyn);
                char[] chars = new char[iRx + 1];
                System.Text.Decoder d = System.Text.Encoding.UTF8.GetDecoder();
                int charLen = d.GetChars(theSockId.dataBuffer, 0, iRx, chars, 0);
                System.String szData = new System.String(chars);

                socketMod.GetMessage(szData);

                WaitForData();
            }
            catch (ObjectDisposedException)
            {
                System.Diagnostics.Debugger.Log(0, "1", "\nOnDataReceived: Socket has been closed\n");
            }
            catch (SocketException se)
            {
                Console.WriteLine(se.Message);
            }
        }

        public bool Connected()
        {
            return m_clientSocket.Connected;
        }
    }
}
