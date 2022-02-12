using System;
using System.Collections.Generic;

using GiantScape.Common.Logging;
using GiantScape.Common.Net.Packets;

namespace GiantScape.Server.Accounts
{
    internal class LoginManager : Loggable
    {
        public event EventHandler<EventArgs> PlayerLogin;

        private HashSet<PlayerClient> loginRequested = new HashSet<PlayerClient>();

        public LoginManager(Logger log)
            : base(log)
        { }

        public void RequestLogin(PlayerClient player)
        {
            Log.Debug($"{player.Client} Sending login request");
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
                    Log.Debug($"{player.Client} Received login attempt");
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

                Log.Info($"{player.Client} Login successful");
                player.Client.SendPacket(new MiscPacket(PacketType.LoginSuccess));
                loginRequested.Remove(player);

                PlayerLogin?.Invoke(player, new EventArgs());
            }
            else
            {
                Log.Warn($"{player.Client} Login failed");
                player.Client.SendPacket(new MiscPacket(PacketType.LoginFail));
            }
        }

        private bool Login(string username, string passwordHash)
        {
            Log.Debug($"Attempting login with username: '{username}'");
            return true;
        }
    }
}
