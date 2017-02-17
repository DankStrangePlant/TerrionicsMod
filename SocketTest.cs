using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Diagnostics;
using System.Runtime;

namespace SocketTest
{
    class SocketTest : Mod
	{
        delegate void ExecuteDelegate(int interval, String route);

		public String hostAddress = "127.0.0.1";
		//public String hostAddress = "nerdtaco.com";
		public int port = 3005;
        public ClientTCP clientTCP;
        public ClientUDP clientUDP;

		public bool playerInitialized = false;

        private Stopwatch pingStopWatch = new Stopwatch();

        public static Dictionary<String, Delegate> referenceDictionary;

        public SocketTest()
		{
			Properties = new ModProperties()
			{
                Autoload = true,
                AutoloadGores = true,
                AutoloadSounds = true,
                AutoloadBackgrounds = true
            };
        }

        public override void Load()
        {
            if (!Main.dedServ)
            {
                clientTCP = new ClientTCP(this);
                clientTCP.OpenConnection(hostAddress, port);

                clientUDP = new ClientUDP(this);
                clientUDP.OpenSocket(hostAddress, 5000);
            }

            if (referenceDictionary == null)
            {
                //referenceDictionary = new Dictionary<string, Delegate>();
                //referenceDictionary.Add("Player.Position", (x, y) => {DelegateManager.SendPlayerPosition();});
            }
        }
		
		public override void Unload()
		{
			if (!Main.dedServ)
			{
				try{
                    if(clientTCP != null)
                        clientTCP.CloseConnection();
				} catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			}
		}
		
		public override void ChatInput(String text, ref bool display)
		{
			if(text.Equals("/ping"))
			{
				try{
                    pingStopWatch.Restart();
                    dynamic p = new Packet();
                    p.text = "ping";
                    clientTCP.SendMessage(p);
                    display = false;
                } catch (Exception e) {
					Console.WriteLine(e.Message);
				}
			}
		}

        public void GetMessage(String message)
        {
            //Parse read message here!
            if (message != null)
            {
                if (message != "\0")
                {
                    message = message.Trim('\0');

                    for(int i= 0; i < message.Length; i++)
                    {
                        System.Diagnostics.Debugger.Log(0, "1", message[i] + "\n");
                    }

                    if(message == "ping")
                    {
                        pingStopWatch.Stop();
                        Main.NewText(pingStopWatch.ElapsedMilliseconds.ToString() + " ms");
                    }
                    else
                    {
                        System.Diagnostics.Debugger.Log(0, "1", "RECEIVED DATA FROM SERVER: ");

                        Main.NewText("SERVER SAYS: " + message);
                    }
                }
                else
                {
                    System.Diagnostics.Debugger.Log(0, "1", "\nSERVER SAYS NULL TERMINATOR\n\n");
                    Main.NewText("SERVER SAYS NULL TERMINATOR");
                }
            }
        }
    }
}
