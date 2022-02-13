using System.Collections.Generic;

using GiantScape.Server.Data.Models;

namespace GiantScape.Server.Data
{
    internal interface IDataProvider
    {
        IEnumerable<User> Users { get; }
        IEnumerable<Player> Players { get; }
        IEnumerable<Map> Maps { get; }
    }
}
