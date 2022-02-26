using System;

using UnityEngine;
using UnityEngine.Tilemaps;

using GiantScape.Common.Game.Tilemaps;

using UnityTilemap = UnityEngine.Tilemaps.Tilemap;

namespace GiantScape.Client.Tilemaps
{
    public class TilemapController : MonoBehaviour
    {
        public Vector2Int Size;

        public UnityTilemap[] Layers;

        [SerializeField]
        private TilemapImporter tilemapImporter;

        public void LoadTilemap(Tilemap tilemap)
        {
            int layersToLoad = Math.Min(Layers.Length, tilemap.TilemapData.Layers.Length);
            for (int i  = 0; i < layersToLoad; i++)
            {
                LoadDataToTilemap(tilemap.TilemapData.Layers[i], Layers[i], tilemap.Tileset, Vector2Int.zero);
            }
        }

        private void Start()
        {

        }

        private void LoadDataToTilemap(LayerData layer, UnityTilemap tilemap, Tileset tileset, Vector2Int offset)
        {
            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    var tile = tileset.GetTile(layer.Tiles[y * Size.x + x]);
                    var tilemapPos = new Vector3Int(x + offset.x, (Size.y - y) + offset.y, 0);
                    tilemap.SetTile(tilemapPos, tile);
                }
            }
        }
    }
}
