﻿using System;
using System.Linq;

using Newtonsoft.Json;

namespace GiantScape.Common.Game.Tilemaps
{
    [Serializable]
    public class TilemapData
    {
        public Vector2Int size { get; set; }
        public LayerData[] layers { get; set; }
        public TilesetData tileset { get; set; }

        public TilemapData Subregion(Vector2Int start, Vector2Int size)
        {
            return new TilemapData
            {
                size = size,
                tileset = tileset,
                layers = layers.Select(l => l.Subregion(start, size, this.size)).ToArray()
            };
        }
    }
}
