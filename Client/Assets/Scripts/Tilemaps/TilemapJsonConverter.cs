using System.Linq;
using System.Collections;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace GiantScape.Client.Tilemaps
{
    public class TilemapJsonConverter : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int baseOffset;

        [SerializeField]
        private Tilemap[] layers;

        public TilesetData Tileset { get; set; }
        public Vector2Int Size { get; set; }

        private string heldJson;
        private Vector2Int heldOffset;

        public void LoadJson(string json, Vector2Int mapOffset)
        {
            heldJson = json;
            heldOffset = mapOffset;
            UnityMainThreadDispatcher.Instance().Enqueue(LoadData());
        }

        /*public void Update()
        {
            if (enabled) LoadData();
        }*/

        private IEnumerator LoadData()
        {
            var tilemapData = JsonUtility.FromJson<TilemapData>(heldJson);
            Tileset = tilemapData.tileset;
            Tileset.LoadTileData();
            Size = tilemapData.size;

            Debug.Log($"Loaded tilemap of size {{{Size}}} using tileset {Tileset.tilesetName}");
            Debug.Log($"Found {tilemapData.layers.Length} map layers...");

            var layersData = tilemapData.layers.ToList()
                .Zip(layers, (layerData, tilemap) => (layerData, tilemap));

            foreach (var (layerData, tilemap) in layersData)
            {
                LoadDataToTilemap(layerData, tilemap, heldOffset + baseOffset);
                yield return null;
            }

            enabled = false;
            yield return null;
        }

        public string ToJson()
        {
            return string.Empty;
        }

        private void LoadDataToTilemap(LayerData layer, Tilemap tilemap, Vector2Int offset)
        {
            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    var tile = Tileset.GetTile(layer.tiles[y * Size.x + x]);
                    tilemap.SetTile(new Vector3Int(x + offset.x, (Size.y - y) + offset.y, 0), tile);
                }
            }
        }
    }
}