using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria;
using Terraria.DataStructures;
using Terraria.ID;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;
using Terraria.GameInput;
using Newtonsoft.Json;

using System.Net;  
using System.Net.Sockets;
using System.Threading;

namespace SocketTest
{
    public class SocketPlayer : ModPlayer
    {
		SocketTest socketMod;
		int count = 0;
		float[] positionArray = {0, 0};
		
		public override void Initialize()
        {
			socketMod = (SocketTest)ModLoader.GetMod("SocketTest");
				
			if(!socketMod.playerInitialized)
			{								
				socketMod.playerInitialized = true;
			}

        }
		
		public override void PostUpdateMiscEffects()
		{
            EmitPosition();
            if (socketMod.clientTCP.Connected())
            {
                //Wait for data asynchronously 
                socketMod.clientTCP.WaitForData();
            }
        }
		
		private void EmitPosition()
		{
			try
			{
				positionArray[0] = player.position.X;
				positionArray[1] = player.position.Y;
                dynamic p = new Packet();
                p.array = positionArray;
                socketMod.clientTCP.SendMessage(p);
			} catch (Exception e) {
				count = (count+1)%1000;
				if(count == 0)
					Main.NewText("Cannot send message");  
			}
		}
    }
}
