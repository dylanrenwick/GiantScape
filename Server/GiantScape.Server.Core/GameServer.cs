using System;
using System.Net;
using System.Collections.Generic;

using Newtonsoft.Json;

using GiantScape.Common.Game;
using GiantScape.Common.Game.Tilemaps;
using GiantScape.Common.Logging;
using GiantScape.Common.Net.Packets;
using GiantScape.Server.Accounts;
using GiantScape.Server.Net;

namespace GiantScape.Server
{
    public class GameServer : Loggable
    {
        private readonly NetworkServer networkServer;

        private readonly LoginManager loginManager;

        private readonly World world;

        private readonly Dictionary<NetworkClient, PlayerClient> players;

        public GameServer(string address, ushort port, Logger log)
            : base(log)
        {
            networkServer = new NetworkServer(IPAddress.Parse(address), port, Log.SubLogger("NETWRK"));
            networkServer.ConnectionEstablished += OnClientConnected;

            loginManager = new LoginManager(Log.SubLogger("LOGIN"));

            world = new World(Log.SubLogger("WORLD"));
            players = new Dictionary<NetworkClient, PlayerClient>();
        }

        public void Start()
        {
            Log.Info("Starting game server...");
            networkServer.Start();

            Log.Info("Game server started");
            while (networkServer.IsRunning) { }
        }

        private void OnClientConnected(object sender, EventArgs e)
        {
            NetworkClient client = (NetworkClient)sender;
            Log.Info($"{client} Client connection established");
            AddPlayerClient(new PlayerClient
            {
                Client = client,
                Player = new Player(),
                Account = new Account()
            });

            client.PacketReceived += OnPacketReceived;
        }

        private void OnPacketReceived(object sender, PacketEventArgs e)
        {
            NetworkClient client = (NetworkClient)sender;
            if (!players.ContainsKey(client))
            {
                Log.Warn($"{client} Unrecognized client, terminating connection");
                client.Close();
                return;
            }

            PlayerClient player = players[client];
            if (!player.IsLoggedIn) loginManager.HandlePacket(player, e.Packet);
        }

        private void AddPlayerClient(PlayerClient player)
        {
            players.Add(player.Client, player);
        }
    }
}
