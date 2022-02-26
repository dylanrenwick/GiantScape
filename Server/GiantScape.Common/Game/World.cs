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

        private HashSet<GameEntity> entities;

        public World(Logger log)
            : base(log)
        {
            tilesets = new Dictionary<string, TilesetData>();
            maps = new Dictionary<string, Map>();
            entities = new HashSet<GameEntity>();
        }

        public void RegisterTileset(string id, TilesetData tileset)
        {
            if (!tilesets.ContainsKey(id)) tilesets.Add(id, tileset);
            else throw new ArgumentException($"Tileset with ID '{id}' already registered!");
        }
        public void LoadTilemap(string id, TilemapData tilemap)
        {
            if (tilesets.ContainsKey(id)) throw new ArgumentException($"Tilemap with ID '{id}' is already loaded!");
            TilesetData tileset = GetTilesetData(tilemap.TilesetID);

            Map map = new Map(tilemap, tileset);
            maps.Add(map.Name, map);
        }

        public TilemapData GetMapDataForPlayer(PlayerEntity player)
        {
            if (!maps.ContainsKey(player.MapID)) throw new ArgumentException($"Player is on non-existant map with ID '{player.MapID}'");
            Map map = maps[player.MapID];
            return map.Tilemap;
        }
        public TilesetData GetTilesetData(string id)
        {
            if (!tilesets.ContainsKey(id)) throw new Exception($"Could not load tileset with ID '{id}'");
            return tilesets[id];
        }
        public TilesetData GetTilesetDataForPlayer(PlayerEntity player)
        {
            if (!maps.ContainsKey(player.MapID)) throw new ArgumentException($"Player is on non-existant map with ID '{player.MapID}'");
            Map map = maps[player.MapID];
            return map.Tileset;
        }

        public void RegisterEntity(GameEntity entity)
        {
            entities.Add(entity);
        }
    }
}
