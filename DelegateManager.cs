using Terraria;
using Terraria.ModLoader;


namespace SocketTest
{
    class DelegateManager
    {
        private static SocketTest mod = (SocketTest)ModLoader.GetMod("SocketTest");
        private static SocketPlayer modPlayer = Main.LocalPlayer.GetModPlayer<SocketPlayer>(mod);

        ClientTCP clientTCP = mod.clientTCP;
        ClientUDP clientUDP = mod.clientUDP;

        public int playerPositionInterval;
        public static void SendPlayerPosition()
        {
            
            //clientUDP.SendMessage(output);
        }
    }
}
