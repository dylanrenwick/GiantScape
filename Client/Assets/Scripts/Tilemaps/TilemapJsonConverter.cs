using System.Linq;
using System.Collections;

using UnityEngine;

using GiantScape.Common;
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

        private byte[] heldBson;
        private string heldJson;
        private Vector2Int heldOffset;

        public void LoadJson(string json, Vector2Int mapOffset)
        {
            heldJson = json;
            heldOffset = mapOffset;
            UnityMainThreadDispatcher.Instance().Enqueue(LoadData());
        }

        public void LoadBson(byte[] bson, Vector2Int mapOffset)
        {
            heldJson = string.Empty;
            heldBson = bson;
            heldOffset = mapOffset;
            UnityMainThreadDispatcher.Instance().Enqueue(LoadData());
        }

        private IEnumerator LoadData()
        {
            Tilemap tilemap;

            if (!string.IsNullOrEmpty(heldJson)) tilemap = LoadJsonData();
            else if (heldBson != null && heldBson.Length > 0) tilemap = LoadBsonData();
            else throw new System.Exception("Invalid map data");

            return LoadTilemapData(tilemap);
        }

        private Tilemap LoadJsonData()
        {
            var tilemapData = Serializer.Deserialize<TilemapData>(heldJson);
            return new Tilemap(tilemapData, tilemapData.tileset);
        }

        private Tilemap LoadBsonData()
        {
            var tilemapData = Serializer.Deserialize<TilemapData>(heldBson);
            return new Tilemap(tilemapData, tilemapData.tileset);
        }

        private IEnumerator LoadTilemapData(Tilemap tilemap)
        {
            Size = tilemap.Size;

            Debug.Log($"Loaded tilemap of size {{{Size}}} using tileset {tilemap.Tileset.TilesetName}");
            Debug.Log($"Found {tilemap.TilemapData.layers.Length} map layers...");

            var layersData = tilemap.TilemapData.layers.ToList()
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
                    var tilemapPos = new Vector3Int(x + offset.x, (Size.y - y) + offset.y, 0);
                    tilemap.SetTile(tilemapPos, tile);
                }
            }
        }
    }
}