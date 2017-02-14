using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Newtonsoft.Json;

using System.Net;  
using System.Net.Sockets;  
using System.Threading;

namespace SocketTest
{
    class SocketTest : Mod
	{
		//public String hostAddress = "127.0.0.1";
		public String hostAddress = "nerdtaco.com";
		public int port = 3005;
        public Socket socket;
		public bool playerInitialized = false;
		public bool connected = false;

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
				socket = SocketAsync.SocketInit(hostAddress, port);
				socket.Blocking = false;
				connected = true;
				
				SocketAsync.Receive(socket);
            }
        }
		
		public override void Unload()
		{
			if (!Main.dedServ)
			{
				if(socket.Connected);
					SocketAsync.SocketClose(socket);
			}
		}
		
		public override void ChatInput(String text, ref bool display)
		{
			if(text.Equals("/ping"))
			{
				SocketAsync.Send(socket, "ping");
				display = false;
			}
		}
    }
}
