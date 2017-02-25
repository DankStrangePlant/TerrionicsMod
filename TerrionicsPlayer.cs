using System;
using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;


namespace TerrionicsMod
{
    public class TerrionicsPlayer : ModPlayer
    {
		TerrionicsMod socketMod;

        public override void Initialize()
        {
			socketMod = (TerrionicsMod)ModLoader.GetMod("SocketTest");
				
			if(!socketMod.playerInitialized)
			{								
				socketMod.playerInitialized = true;
                socketMod.modPlayer = this;

                //socketMod.modActions.Add(new ModAction("position", "/", delegate (ModActionSpecs s) {
                //    Packet p = new Packet();
                //    p["type"] = s.modActionType;
                //    p["route"] = s.modActionRoute;
                //    p["data"] = new float[] { player.position.X, player.position.Y };
                //    socketMod.clientUDP.SendMessage(p);
                //    System.Diagnostics.Debugger.Log(0, "1", playerX + ", " + playerY);
                //}, 10));
            }

        }

        public override void PostUpdateMiscEffects()
        {
            //update tick counter for purposes of handling actions that update every so many ticks
            RequestTimer.update();

            if (socketMod.clientTCP.Connected())
            {
                //Wait for data asynchronously 
                socketMod.clientTCP.WaitForData();
            }
        }
    }
}
