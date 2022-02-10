using System;

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

        private string[] tileNames => tilesetData.tileNames;

        public Tileset(TilesetData tilesetData)
        {
            this.tilesetData = tilesetData;
        }

        public void LoadTileData()
        {
            if (tileNames.Length == 0 || tiles != null) return;

            tiles = new Tile[tileNames.Length];

            tiles[0] = null;
            int failures = 0;
            for (int index = 1; index < tileNames.Length; index++)
            {
                string resourcePath = $"Tilemaps/Tiles/{TilesetName}_{tileNames[index]}";
                Tile tile = null;
                try
                {
                    tile = Resources.Load<Tile>(resourcePath);
                    // TODO: Handle this better
                    if (tile == null) throw new Exception();
                }
                catch (Exception)
                {
                    Debug.LogWarning($"Failed to load tile from '{resourcePath}'");
                    failures++;
                }
                finally
                {
                    tiles[index] = tile;
                }
            }
            Debug.Log($"Successfully loaded {tiles.Length - failures}/{tiles.Length} tile resources");
        }

        public Tile GetTile(int index)
        {
            if (tiles == null || index < 0 || index >= tiles.Length) return null;

            return tiles[index];
        }
    }
}
