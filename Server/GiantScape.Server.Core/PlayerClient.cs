using GiantScape.Common.Game;
using GiantScape.Server.Accounts;
using GiantScape.Server.Net;

namespace GiantScape.Server
{
    internal class PlayerClient
    {
        public PlayerEntity Player { get; set; }
        public NetworkClient Client { get; set; }
        public Account Account { get; set; }

        public bool IsLoggedIn => Account?.LoggedIn ?? false;
    }
}
