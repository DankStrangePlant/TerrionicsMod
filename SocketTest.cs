using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Quobject.SocketIoClientDotNet.Client;

namespace SocketTest
{
    class SocketTest : Mod
	{
        
        public Socket socket;

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
                socket = IO.Socket("http://nerdtaco.com:3005");

                socket.On(Socket.EVENT_CONNECT, () =>
                {
                    socket.Emit("socket-connected", "aloha");
                });

 ///               socket.On("spawn-npc", () =>
 //               {
 //                   socket.Emit("socket-connected", "aloha");
 //               });
            }
        }
    }
}
