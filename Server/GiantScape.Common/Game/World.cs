using System;
using System.Collections.Generic;

using GiantScape.Common.Game.Tilemaps;
using GiantScape.Common.Logging;

namespace GiantScape.Common.Game
{
    public class World : Loggable
    {
        private Dictionary<string, TilesetData> tilesets;
        private Dictionary<string, Map> maps;

        public World(Logger log)
            : base(log)
        {
            tilesets = new Dictionary<string, TilesetData>();
            maps = new Dictionary<string, Map>();
        }

        public void RegisterTileset(string id, TilesetData tileset)
        {
            if (!tilesets.ContainsKey(id)) tilesets.Add(id, tileset);
            else throw new ArgumentException($"Tileset with ID '{id}' already registered!");
        }
        public void LoadTilemap(string id, TilemapData tilemap)
        {
            if (tilesets.ContainsKey(id)) throw new ArgumentException($"Tilemap with ID '{id}' is already loaded!");
            string tilesetID = tilemap.TilesetID;
            if (!tilesets.ContainsKey(tilesetID)) throw new Exception($"Could not load tileset with ID '{tilesetID}'");
            TilesetData tileset = tilesets[tilesetID];

            Map map = new Map(tilemap, tileset);
            maps.Add(map.Name, map);
        }

        public TilemapData GetMapDataForPlayer(PlayerEntity player)
        {
            return tilemapData;
        }
    }
}
