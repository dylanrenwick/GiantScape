using System.Collections.Generic;

using GiantScape.Common.Net.Packets;

namespace GiantScape.Server.Accounts
{
    internal class LoginManager
    {
        private List<PlayerClient> loginRequested = new List<PlayerClient>();

        public void RequestLogin(PlayerClient player)
        {
            var loginRequestPacket = new MiscPacket(PacketType.LoginRequest);
            player.Client.SendPacket(loginRequestPacket);
            loginRequested.Add(player);
        }

        public void HandlePacket(PlayerClient player, NetworkPacket packet)
        {

        }
    }
}
