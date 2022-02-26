using System.Collections.Generic;

using UnityEngine;

using GiantScape.Common;
using GiantScape.Common.Game.Tilemaps;
using GiantScape.Common.Net.Packets;

using UnityTilemap = UnityEngine.Tilemaps.Tilemap;

namespace GiantScape.Client.Tilemaps
{
    public class TilemapImporter : MonoBehaviour
    {
        public Vector2Int Size;

        [SerializeField]
        private TextAsset jsonFile;

        private ClientController client;

        private Dictionary<string, Tileset> tilesets = new Dictionary<string,Tileset>();
        private Dictionary<string, TilemapData> tilemapsWaitingTileset = new Dictionary<string, TilemapData>();

        public void Import()
        {
            Debug.Log("Importing from file...");
            TilemapData data = Serializer.Deserialize<TilemapData>(jsonFile.text);
        }

#if UNITY_EDITOR
        public void Export()
        {
            throw new System.NotImplementedException();
        }
#endif

        public void OnPacketReceived(object sender, PacketEventArgs e)
        {
            var packet = e.Packet;
            if (packet.Type == PacketType.Map)
            {
                var mapPacket = (BsonPacket)packet;
                TilemapData data = Serializer.Deserialize<TilemapData>(mapPacket.Bson);
                if (tilesets.ContainsKey(data.TilesetID))
                {
                    Tilemap tilemap = new Tilemap(data, tilesets[data.TilesetID]);
                }
                else
                {
                    RequestTileset(data.TilesetID);
                    tilemapsWaitingTileset.Add(data.TilesetID, data);
                }
            }
            else if (packet.Type == PacketType.Tileset)
            {
                var tilesetPacket = (BsonPacket)packet;
                TilesetData data = Serializer.Deserialize<TilesetData>(tilesetPacket.Bson);
                RegisterTileset(data);
            }
        }

        private void Start()
        {
            client = GameObject.Find("Controller").GetComponent<ClientController>();
            client.Client.PacketReceived += OnPacketReceived;
        }

        public void RegisterTileset(TilesetData data)
        {
            var tileset = new Tileset(data);
            RegisterTileset(tileset);
        }
        public void RegisterTileset(Tileset tileset)
        {
            tilesets.Add(tileset.TilesetName, tileset);
        }

        public void RequestTileset(string tilesetID)
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
