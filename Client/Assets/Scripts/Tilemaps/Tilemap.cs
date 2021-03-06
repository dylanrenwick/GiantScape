using UnityEngine;

using GiantScape.Common.Game.Tilemaps;

namespace GiantScape.Client.Tilemaps
{
    public class Tilemap
    {
        public Vector2Int Size => TilemapData.size.ToUnity();

        public TilemapData TilemapData { get; set; }

        public Tileset Tileset { get; set; }

        public Tilemap(TilemapData tilemapData, TilesetData tilesetData)
        {
            TilemapData = tilemapData;
            Tileset = new Tileset(tilesetData);
        }
    }
}
