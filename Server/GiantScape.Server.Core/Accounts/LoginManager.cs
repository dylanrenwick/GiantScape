using System;
using System.Collections.Generic;
using System.Linq;

using GiantScape.Common.Logging;
using GiantScape.Common.Net.Packets;
using GiantScape.Server.Data;
using GiantScape.Server.Data.Models;
using GiantScape.Server.DataStores;
using GiantScape.Server.Net;

namespace GiantScape.Server.Accounts
{
    internal class LoginManager : Loggable
    {
        public event EventHandler<PlayerClientEventArgs> PlayerLogin;

        private HashSet<NetworkClient> loginRequested = new HashSet<NetworkClient>();

        private readonly IDataProvider data;

        public LoginManager(IDataProvider data, Logger log)
            : base(log)
        {
            this.data = data;
        }

        public void RequestLogin(NetworkClient client)
        {
            Log.Debug($"{client} Sending login request");
            var loginRequestPacket = new BinaryPacket(PacketType.LoginRequest);
            client.SendPacket(loginRequestPacket);
            loginRequested.Add(client);
        }

        public bool HandlePacket(NetworkClient client, NetworkPacket packet)
        {
            if (loginRequested.Contains(client))
            {
                if (packet.Type == PacketType.Login)
                {
                    Log.Debug($"{client} Received login attempt");
                    HandleLogin(client, (LoginPacket)packet);
                }
                return true;
            }
            return false;
        }

        private void HandleLogin(NetworkClient client, LoginPacket packet)
        {
            if (Login(packet.Username, packet.PasswordHash))
            {
                PlayerClient player = LoadPlayerInfo(client, packet.Username);

                Log.Info($"{client} Login successful");
                client.SendPacket(new BinaryPacket(PacketType.LoginSuccess));
                loginRequested.Remove(client);

                PlayerLogin?.Invoke(client, new PlayerClientEventArgs(player));
            }
            else
            {
                Log.Warn($"{client} Login failed");
                client.SendPacket(new BinaryPacket(PacketType.LoginFail));
            }
        }

        private bool Login(string username, string passwordHash)
        {
            Log.Debug($"Attempting login with username: '{username}'");
            UserModel user = data.Users.Where(u => u.Username == username).FirstOrDefault();
            if (user == null)
            {
                Log.Debug($"No user found with username: '{username}'");
                return false;
            }

            if (passwordHash.Equals(user.PasswordHash))
            {
                Log.Debug($"Login successful");
                return true;
            }
            else
            {
                Log.Debug("Password hash does not match stored hash");
                return false;
            }
        }

        private PlayerClient LoadPlayerInfo(NetworkClient client, string username)
        {
            UserModel user = data.Users.Where(u => u.Username == username).First();
            PlayerModel player = data.Players.Where(p => p.UserID == user.ID).First();

            return new PlayerClient
            {
                Player = player,
                Client = client,
            };
        }
    }
}
