using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;

using System.Diagnostics;
using Newtonsoft.Json;

namespace TerrionicsMod
{
    class TerrionicsMod : Mod
	{
		public String hostAddress = "127.0.0.1";
		//public String hostAddress = "nerdtaco.com";
		public int port = 3005;
        public ClientTCP clientTCP;
        public ClientUDP clientUDP;

        public TerrionicsPlayer modPlayer;
        public TerrionicsWorld modWorld;

		public bool playerInitialized = false;

        private Stopwatch pingStopWatch = new Stopwatch();

        public TerrionicsMod()
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
                    Packet p = new Packet();
                    p["text"] = "ping";
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
            //Will want to use this to add/remove elements to modActions List
            if (message != null)
            {
                if (message != "\0")
                {
                    message = message.Trim('\0');

                    Packet p = JsonConvert.DeserializeObject<Packet>(message);
                    //TODO Deal with the packet here
                    
                    if(p["type"].Equals("misc"))
                    {
                        if(p["text"].Equals("ping"))
                        {
                            pingStopWatch.Stop();
                            Main.NewText(pingStopWatch.ElapsedMilliseconds + " ms");
                        }
                    }
                    
                }
                else
                {
                    System.Diagnostics.Debugger.Log(0, "1", "\nSERVER SAYS NULL TERMINATOR\n\n");
                }
            }
        }
    }
}
