using Terraria;
using Terraria.ModLoader;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;

namespace SocketTest
{
    class SocketTest : Mod
	{
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

        /*public override void Load()
        {
            if (!Main.dedServ)
            {

            }
        }*/
    }
}
