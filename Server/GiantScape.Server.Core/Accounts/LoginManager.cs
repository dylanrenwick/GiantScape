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
                    HandleLogin(player, (LoginPacket)packet);
                }
            }
        }

        private void HandleLogin(PlayerClient player, LoginPacket packet)
        {
            if (Login(packet.Username, packet.PasswordHash))
            {
                player.Account.Username = packet.Username;
                player.Account.LoggedIn = true;
            }
        }

        private bool Login(string username, string passwordHash)
        {
            return true;
        }
    }
}
