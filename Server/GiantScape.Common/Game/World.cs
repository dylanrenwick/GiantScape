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

        public void RegisterTileset(TilesetData tileset)
        {
            if (!tilesets.ContainsKey(tileset.TilesetName)) tilesets.Add(tileset.TilesetName, tileset);
            else throw new ArgumentException($"Tileset '{tileset.TilesetName}' already registered!");
        }
        public void LoadMap(TilemapData tilemap)
        {
            if (tilesets.ContainsKey(tilemap.Name)) throw new ArgumentException($"Tilemap with name '{tilemap.Name}' is already loaded!");
            string tilesetName = tilemap.TilesetID;
            if (!tilesets.ContainsKey(tilesetName)) throw new Exception($"Could not load tileset '{tilesetName}'");
            TilesetData tileset = tilesets[tilesetName];

            Map map = new Map(tilemap, tileset);
            maps.Add(map.Name, map);
        }

        public TilemapData GetMapDataForPlayer(PlayerEntity player)
        {
            return tilemapData;
        }
    }
}
