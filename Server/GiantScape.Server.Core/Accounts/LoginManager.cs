using System.Collections.Generic;

using GiantScape.Common.Net.Packets;

namespace GiantScape.Server.Accounts
{
    internal class LoginManager
    {
        private HashSet<PlayerClient> loginRequested = new HashSet<PlayerClient>();

        public void RequestLogin(PlayerClient player)
        {
            var loginRequestPacket = new MiscPacket(PacketType.LoginRequest);
            player.Client.SendPacket(loginRequestPacket);
            loginRequested.Add(player);
        }

        public void HandlePacket(PlayerClient player, NetworkPacket packet)
        {
            if (loginRequested.Contains(player))
            {
                if (packet.Type == PacketType.Login)
                {

                }
            }
        }
    }
}
