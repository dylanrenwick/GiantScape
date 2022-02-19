using System;

using GiantScape.Common.Game;
using GiantScape.Server.Accounts;
using GiantScape.Server.Data.Models;
using GiantScape.Server.Net;

namespace GiantScape.Server
{
    internal class PlayerClient
    {
        public PlayerModel Player { get; set; }
        public PlayerEntity Entity { get; set; }
        public NetworkClient Client { get; set; }
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
