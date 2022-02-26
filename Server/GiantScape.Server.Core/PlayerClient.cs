using System;

using GiantScape.Common.Game;
using GiantScape.Server.Data.Models;
using GiantScape.Server.DataStores;
using GiantScape.Server.Net;

namespace GiantScape.Server
{
    internal class PlayerClient
    {
        public PlayerModel Player { get; set; }
        public PlayerEntity Entity { get; set; }
        public NetworkClient Client { get; set; }
        public DataStore Data { get; set; }
    }

    internal class PlayerClientEventArgs : EventArgs
    {
        public PlayerClient PlayerClient { get; set; }

        public PlayerClientEventArgs(PlayerClient player)
        {
            PlayerClient = player;
        }
    }
}
