using System.IO;

using GiantScape.Common;
using GiantScape.Common.Game.Tilemaps;
using GiantScape.Server.Data.Models;

namespace GiantScape.Server.Resources
{
    internal class ResourceCache
    {
        private const string RES_ROOT = "Resources/";
        private const string RES_TILEMAP = "Tilemaps/";
        private const string RES_TILESET = "Tilesets/";

        public static TilemapData LoadMap(MapModel map)
        {
            return LoadObject<TilemapData>($"{RES_ROOT}{RES_TILEMAP}{map.Filename}");
        }

        public static TilesetData LoadTileset(TilesetModel tileset)
        {
            return LoadObject<TilesetData>($"{RES_ROOT}{RES_TILESET}{tileset.Filename}");
        }

        private static T LoadObject<T>(string filepath)
        {
            if (TryLoadJson(filepath, out string json))
            {
                return Serializer.Deserialize<T>(json);
            }
            else if (TryLoadJsonc(filepath, out string jsonc))
            {
                return Serializer.Deserialize<T>(jsonc);
            }
            else throw new FileLoadException($"Could not load file at '{filepath}.json' or '{filepath}.jsonc'");
        }

        private static bool TryLoadJson(string filepath, out string json) => TryLoadFile(filepath + ".json", out json);
        private static bool TryLoadJsonc(string filepath, out string json) => TryLoadFile(filepath + ".jsonc", out json);

        private static bool TryLoadFile(string filepath, out string contents)
        {
            if (File.Exists(filepath))
            {
                contents = File.ReadAllText(filepath);
                return true;
            }

            contents = string.Empty;
            return false;
        }
    }
}
