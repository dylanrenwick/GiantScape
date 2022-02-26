using System;
using System.Collections;
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

        private Dictionary<string, Tileset> tilesets = new Dictionary<string, Tileset>();
        private Dictionary<string, Tilemap> tilemaps = new Dictionary<string, Tilemap>();
        private Dictionary<string, List<TilemapData>> tilemapsWaitingTileset = new Dictionary<string, List<TilemapData>>();

        public void Import()
        {
            Debug.Log("Importing from file...");
            TilemapData data = Serializer.Deserialize<TilemapData>(jsonFile.text);
        }

#if UNITY_EDITOR
        public void Export()
        {
            throw new NotImplementedException();
        }
#endif

        public void OnPacketReceived(object sender, PacketEventArgs e)
        {
            var packet = e.Packet;
            if (packet.Type == PacketType.Map)
            {
                var mapPacket = (BinaryPacket)packet;
                TilemapData data = Serializer.Deserialize<TilemapData>(mapPacket.Content);
                RegisterTilemap(data, data.TilesetID);
            }
            else if (packet.Type == PacketType.Tileset)
            {
                var tilesetPacket = (BinaryPacket)packet;
                TilesetData data = Serializer.Deserialize<TilesetData>(tilesetPacket.Content);
                RegisterTileset(data);
            }
        }

        public void RegisterTilemap(TilemapData tilemap, string tileset)
        {
            if (tilesets.ContainsKey(tileset))
            {
                RegisterTilemap(tilemap, tilesets[tileset]);
            }
            else
            {
                StartCoroutine(RequestTileset(tileset));
                AddTilemapToWaitlist(tilemap);
            }
        }
        public void RegisterTilemap(TilemapData tilemap, Tileset tileset)
        {
            RegisterTilemap(new Tilemap(tilemap, tileset));
        }
        public void RegisterTilemap(Tilemap tilemap)
        {
            if (tilemaps.ContainsKey(tilemap.Name)) throw new ArgumentException($"Tried to register duplicate tilemap {tilemap.Name}!");
            tilemaps.Add(tilemap.Name, tilemap);
        }

        public void RegisterTileset(TilesetData data)
        {
            var tileset = new Tileset(data);
            RegisterTileset(tileset);
        }
        public void RegisterTileset(Tileset tileset)
        {
            tilesets.Add(tileset.TilesetName, tileset);
            ResolveWaitingTilemaps(tileset);
        }

        public IEnumerator RequestTileset(string tilesetID)
        {
            AsyncPromise<TilesetData> request = client.RequestTileset(tilesetID);
            while (!request.IsDone) yield return null;

            TilesetData data = request.Result;
            if (data == null) throw new Exception($"Could not load Tileset data from server!");

            if (data.TilesetName != tilesetID) Debug.LogWarning($"Tileset ID of {data.TilesetName} does not match requested ID of {tilesetID}");
            RegisterTileset(data);
        }

        private void Start()
        {
            client = GameObject.Find("Controller").GetComponent<ClientController>();
            client.Client.PacketReceived += OnPacketReceived;
        }

        private void ResolveWaitingTilemaps(Tileset tileset)
        {
            string id = tileset.TilesetName;
            if (!tilemapsWaitingTileset.ContainsKey(id)) return;

            foreach (var tilemap in tilemapsWaitingTileset[id])
            {
                RegisterTilemap(tilemap, tileset);
            }
        }

        private void AddTilemapToWaitlist(TilemapData tilemap)
        {
            string tileset = tilemap.TilesetID;
            if (tilemapsWaitingTileset.ContainsKey(tileset))
            {
                tilemapsWaitingTileset[tileset].Add(tilemap);
            }
            else
            {
                var list = new List<TilemapData>();
                list.Add(tilemap);
                tilemapsWaitingTileset.Add(tileset, list);
            }
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
