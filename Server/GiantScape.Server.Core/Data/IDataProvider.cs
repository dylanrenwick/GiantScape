using System.Collections.Generic;

using GiantScape.Server.Data.Models;

namespace GiantScape.Server.Data
{
    internal interface IDataProvider
    {
        DbCollection<UserModel> Users { get; }
        DbCollection<PlayerModel> Players { get; }
        DbCollection<MapModel> Maps { get; }
        DbCollection<TilesetModel> Tilesets { get; }


    }
}
