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
		SocketTest mod;
		Socket socket;
		int count = 0;
		float[] positionArray = {0, 0};
		String output;
		
		public override void Initialize()
        {
			mod = (SocketTest)ModLoader.GetMod("SocketTest");
			socket = mod.socket;
				
			if(!mod.playerInitialized)
			{								
				mod.playerInitialized = true;
			}

        }
		
		public override void PostUpdateMiscEffects()
		{
			if(mod.connected)
			{
				EmitPosition();
			}
		}
		
		private void EmitPosition()
		{
			try
			{
				positionArray[0] = player.position.X;
				positionArray[1] = player.position.Y;
				String output = JsonConvert.SerializeObject(positionArray);
				SocketAsync.Send(socket, output);
			} catch (Exception e) {
				count = (count+1)%1000;
				if(count == 0)
					Main.NewText("Cannot connect to server");  
			}
		}
    }
}
