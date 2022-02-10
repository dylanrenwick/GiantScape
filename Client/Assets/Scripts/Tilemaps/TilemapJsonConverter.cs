using System.Linq;
using System.Collections;

using UnityEngine;

using GiantScape.Common.Game.Tilemaps;

using UnityTilemap = UnityEngine.Tilemaps.Tilemap;

namespace GiantScape.Client.Tilemaps
{
    public class TilemapJsonConverter : MonoBehaviour
    {
        [SerializeField]
        private Vector2Int baseOffset;

        [SerializeField]
        private UnityTilemap[] layers;

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
            var tilemap = new Tilemap(tilemapData, tilemapData.tileset);
            Size = tilemap.Size;

            Debug.Log($"Loaded tilemap of size {{{Size}}} using tileset {tilemap.Tileset.TilesetName}");
            Debug.Log($"Found {tilemapData.layers.Length} map layers...");

            var layersData = tilemapData.layers.ToList()
                .Zip(layers, (layerData, unityTilemap) => (layerData, unityTilemap));

            foreach (var (layerData, unityTilemap) in layersData)
            {
                LoadDataToTilemap(layerData, unityTilemap, tilemap.Tileset, heldOffset + baseOffset);
                yield return null;
            }

            enabled = false;
            yield return null;
        }

        public string ToJson()
        {
            return string.Empty;
        }

        private void LoadDataToTilemap(LayerData layer, UnityTilemap tilemap, Tileset tileset, Vector2Int offset)
        {
            for (int y = 0; y < Size.y; y++)
            {
                for (int x = 0; x < Size.x; x++)
                {
                    var tile = tileset.GetTile(layer.tiles[y * Size.x + x]);
                    tilemap.SetTile(new Vector3Int(x + offset.x, (Size.y - y) + offset.y, 0), tile);
                }
            }
        }
    }
}