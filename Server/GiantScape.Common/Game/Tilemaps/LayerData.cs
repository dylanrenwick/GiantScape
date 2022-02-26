using System;

using Newtonsoft.Json;

namespace GiantScape.Common.Game.Tilemaps
{
    [Serializable]
    public class LayerData
    {
        [JsonProperty("tiles")]
        public int[] Tiles { get; set; }

        public LayerData Subregion(Vector2Int start, Vector2Int size, Vector2Int outerSize)
        {
            int[] subregionTiles = new int[size.X * size.Y];
            for (int y = 0; y < size.Y; y++)
            {
                for (int x = 0; x < size.X; x++)
                {
                    subregionTiles[y * size.X + x] = Tiles[(y + start.Y) * outerSize.X + (x + start.X)];
                }
            }
            return new LayerData() { Tiles = subregionTiles };
        }
    }
}
