using System;

namespace GiantScape.Common.Game.Tilemaps
{
    [Serializable]
    public class LayerData
    {
        public int[] tiles { get; set; }

        public LayerData Subregion(Vector2Int start, Vector2Int size, Vector2Int outerSize)
        {
            int[] subregionTiles = new int[size.x * size.y];
            for (int y = 0; y < size.y; y++)
            {
                for (int x = 0; x < size.x; x++)
                {
                    subregionTiles[y * size.x + x] = tiles[(y + start.y) * outerSize.x + (x + start.x)];
                }
            }
            return new LayerData() { tiles = subregionTiles };
        }
    }
}
