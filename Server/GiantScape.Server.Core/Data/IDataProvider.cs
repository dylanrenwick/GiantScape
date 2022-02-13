using System;

using GiantScape.Common.Game;
using GiantScape.Common.Game.Tilemaps;

namespace GiantScape.Server.Data
{
    internal interface IDataProvider
    {
        TilemapData GetMap();

        void GetPlayerAsync(string username, Action<Player> callback);
    }
}
