using System;
using Terraria;
using Terraria.ModLoader;


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
                Packet p = new Packet();
                p["contents"] = "position";
                p["array"] = positionArray;
                socketMod.clientUDP.SendMessage(p);
			} catch (Exception e) {
				count = (count+1)%1000;
				if(count == 0)
					Main.NewText("Cannot send message");  
			}
		}
    }
}
