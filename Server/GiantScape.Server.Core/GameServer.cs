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
    public class GameServer
    {
        private readonly NetworkServer networkServer;

        private readonly Logger logger;

        private readonly World world;

        private readonly List<PlayerClient> players;

        public GameServer(string address, ushort port, Logger logger)
        {
            networkServer = new NetworkServer(IPAddress.Parse(address), port, logger.SubLogger("NETWRK"));
            networkServer.ConnectionEstablished += OnClientConnected;

            this.logger = logger;
            world = new World(logger.SubLogger("WORLD"));
            players = new List<PlayerClient>();
        }

        public void Start()
        {
            logger.Info("Starting game server...");
            networkServer.Start();

            logger.Info("Game server started");
            while (networkServer.IsRunning) { }
        }

        private void OnClientConnected(object sender, EventArgs e)
        {
            NetworkClient client = (NetworkClient)sender;
            logger.Info($"{client} Client connection established");
            players.Add(new PlayerClient
            {
                Client = client,
                Player = new Player(),
                Account = new Account()
            });
        }
    }
}
