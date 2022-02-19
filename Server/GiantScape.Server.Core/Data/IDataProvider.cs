using System.Collections.Generic;

using GiantScape.Server.Data.Models;

namespace GiantScape.Server.Data
{
    internal interface IDataProvider
    {
        IEnumerable<UserModel> Users { get; }
        IEnumerable<PlayerModel> Players { get; }
        IEnumerable<MapModel> Maps { get; }
        IEnumerable<TilesetModel> Tilesets { get; }
    }
}
