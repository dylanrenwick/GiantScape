using System;
using System.Linq;

using UnityEngine;
using UnityEngine.Tilemaps;

using GiantScape.Common.Game.Tilemaps;

namespace GiantScape.Client.Tilemaps
{
    public class Tileset
    {
        public string TilesetName => tilesetData.tilesetName;

        private Tile[] tiles;

        private TilesetData tilesetData;

        public Tileset(TilesetData tilesetData)
        {
            this.tilesetData = tilesetData;
            LoadTileData();
        }

        public void LoadTileData()
        {
            if (tilesetData.tiles.Length == 0 || tiles != null) return;

            string[] tileNames = tilesetData.tiles.Select(t => t.ResourceName).ToArray();

            tiles = new Tile[tileNames.Length];

            tiles[0] = null;
            int failures = 0;
            for (int index = 1; index < tileNames.Length; index++)
            {
                tiles[index] = LoadTile(tileNames[index]);
                if (tiles[index] == null) failures++;
            }
            Debug.Log($"Successfully loaded {tiles.Length - failures}/{tiles.Length} tile resources");
        }

        private Tile LoadTile(string tileName)
        {
            string resourcePath = $"Tilemaps/Tiles/{TilesetName}_{tileName}";

            try
            {
                Tile tile = Resources.Load<Tile>(resourcePath);
                return tile;
            }
            catch (Exception)
            {
                Debug.LogWarning($"Failed to load tile from '{resourcePath}'");
                return null;
            }
        }

        public Tile GetTile(int index)
        {
            if (tiles == null || index < 0 || index >= tiles.Length) return null;

            return tiles[index];
        }
    }
}
