using System;
using System.Collections.Generic;

using GiantScape.Common.Game;
using GiantScape.Common.Game.Tilemaps;
using GiantScape.Common.Logging;
using GiantScape.Server.Data;
using GiantScape.Server.Data.Models;
using GiantScape.Server.Resources;

namespace GiantScape.Server.Game
{
    internal class ServerWorld : World
    {
        private IDataProvider data;

        public ServerWorld(IEnumerable<MapModel> mapsToLoad, IDataProvider data, Logger log)
            : base(log)
        {
            this.data = data;

            foreach (var map in mapsToLoad)
            {
                LoadMap(map);
            }
        }

        private void LoadMap(MapModel mapModel)
        {
            TilemapData tilemap = ResourceCache.LoadMap(mapModel);

            string tilesetID = tilemap.TilesetID;
            var tilesetModel = LoadTilesetModel(tilesetID);

            LoadTileset(tilesetModel);
            LoadTilemap(tilemap);
        }
        private void LoadTileset(TilesetModel tilesetModel)
        {
            TilesetData tileset = ResourceCache.LoadTileset(tilesetModel);

            RegisterTileset(tileset);
        }

        private TilesetModel LoadTilesetModel(string tilesetID)
        {
            TilesetModel tilesetModel = data.Tilesets.Get(tilesetID);
            if (tilesetModel == null) throw new ArgumentException($"Could not load tileset resource with ID '{tilesetID}'");
            return tilesetModel;
        }
    }
}
