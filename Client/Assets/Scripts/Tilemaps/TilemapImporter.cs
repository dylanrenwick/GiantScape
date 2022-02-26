using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GiantScape.Common;
using GiantScape.Common.Game.Tilemaps;

using UnityTilemap = UnityEngine.Tilemaps.Tilemap;

namespace GiantScape.Client.Tilemaps
{
    public class TilemapImporter : MonoBehaviour
    {
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

        public void RegisterTilemap(TilemapData tilemap)
        {
            RegisterTilemap(tilemap, tilemap.TilesetID);
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
        public IEnumerator RequestTilemap()
        {
            AsyncPromise<TilemapData> request = client.RequestMap();
            while (!request.IsDone) yield return null;

            TilemapData data = request.Result;
            if (data == null) throw new Exception($"Could not load Tilemap data from server!");

            RegisterTilemap(data);
        }

        private void Start()
        {
            client = GameObject.Find("Controller").GetComponent<ClientController>();
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
    }
}
