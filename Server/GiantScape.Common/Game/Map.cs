﻿using GiantScape.Common.Game.Tilemaps;

namespace GiantScape.Common.Game
{
    public class Map
    {
        private TilemapData tilemap;
        private TilesetData tileset;

        public Map(TilemapData tilemap, TilesetData tileset)
        {
            this.tilemap = tilemap;
            this.tileset = tileset;
        }
    }
}
