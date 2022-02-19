using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

using GiantScape.Common;
using GiantScape.Common.Game;
using GiantScape.Common.Game.Tilemaps;
using GiantScape.Common.Logging;
using GiantScape.Common.Net.Packets;
using GiantScape.Server.Accounts;
using GiantScape.Server.Data;
using GiantScape.Server.Data.Json;
using GiantScape.Server.Game;
using GiantScape.Server.Net;

namespace GiantScape.Server
{
    public class GameServer : Loggable
    {
        private readonly NetworkServer networkServer;

        private readonly LoginManager loginManager;

        private readonly ServerWorld world;

        private readonly Dictionary<NetworkClient, PlayerClient> players;

        private readonly IDataProvider dataProvider;

        public GameServer(string address, ushort port, Logger log)
            : base(log)
        {
            dataProvider = new JsonDataProvider(@"Resources/database.json");

            networkServer = new NetworkServer(IPAddress.Parse(address), port, Log.SubLogger("NETWRK"));
            networkServer.ConnectionEstablished += OnClientConnected;

            loginManager = new LoginManager(dataProvider, Log.SubLogger("LOGIN"));
            loginManager.PlayerLogin += OnPlayerLogin;

            world = new ServerWorld(dataProvider.Maps, dataProvider, Log.SubLogger("WORLD"));
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

            client.PacketReceived += OnPacketReceived;

            loginManager.RequestLogin(client);
        }

        private void OnPlayerLogin(object sender, PlayerClientEventArgs e)
        {
            NetworkClient client = (NetworkClient)sender;
            PlayerClient player = e.PlayerClient;

            if (players.ContainsKey(client)) players[client] = player;
            else players.Add(client, player);
        }

        private void OnPacketReceived(object sender, PacketEventArgs e)
        {
            NetworkClient client = (NetworkClient)sender;
            if (!players.ContainsKey(client))
            {
                if (!loginManager.HandlePacket(client, e.Packet))
                {
                    Log.Warn($"{client} Unrecognized client, terminating connection");
                    client.Close();
                    if (players.ContainsKey(client)) players.Remove(client);
                }

                return;
            }

            PlayerClient player = players[client];
            switch (e.Packet.Type)
            {
                case PacketType.MapRequest:
                    SendWorldData(player);
                    break;
                default:
                    break;
            }
        }

        private void SendWorldData(PlayerClient player)
        {
            Log.Info($"{player.Client} Sending world data...");
            TilemapData worldData = world.GetMapDataForPlayer(player.Entity);
            var mapPacket =new BsonPacket(PacketType.Map, Serializer.Serialize(worldData));
            player.Client.SendPacket(mapPacket);
        }
    }
}
