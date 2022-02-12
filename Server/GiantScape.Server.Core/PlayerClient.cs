using GiantScape.Common.Game;
using GiantScape.Server.Accounts;
using GiantScape.Server.Net;

namespace GiantScape.Server
{
    internal class PlayerClient
    {
        public Player Player { get; set; }
        public NetworkClient Client { get; set; }
        public Account Account { get; set; }
    }
}
