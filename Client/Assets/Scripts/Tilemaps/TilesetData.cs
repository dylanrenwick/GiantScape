using System;

using UnityEngine;
using UnityEngine.Tilemaps;

namespace GiantScape.Client.Tilemaps
{
    [Serializable]
    public class TilesetData
    {
        [NonSerialized]
        private Tile[] tiles;

        public string[] tileNames;
        public string tilesetName;

        public void LoadTileData()
        {
            if (tileNames.Length == 0 || tiles != null) return;

            tiles = new Tile[tileNames.Length];

            tiles[0] = null;
            int failures = 0;
            for (int index = 1; index < tileNames.Length; index++)
            {
                Tile tile = null;
                try
                {
                    tile = Resources.Load<Tile>($"Tilemaps/Tiles/{tilesetName}_{tileNames[index]}");
                    if (tile == null) throw new Exception();
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Failed to load tile from Tilemaps/Tiles/{tilesetName}_{tileNames[index]}");
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
