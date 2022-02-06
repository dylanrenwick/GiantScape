using System;

using UnityEngine;

namespace GiantScape.Client.Tilemaps
{
    [Serializable]
    public class TilemapData
    {
        public Vector2Int size;
        public LayerData[] layers;
        public TilesetData tileset;
    }
}
