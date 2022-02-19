using System.IO;

using Newtonsoft.Json;

using GiantScape.Common.Game.Tilemaps;
using GiantScape.Common.Logging;

namespace GiantScape.Common.Game
{
    public class World : Loggable
    {
        private const string filepathFormat = "Resources/Tilemaps/{0}.json";
        private const string filename = @"overworld";

        private TilemapData tilemapData;

        public World(Logger log, string mapFilename = filename)
            : base(log)
        {
            string filepath = string.Format(filepathFormat, mapFilename);
            log.Info($"Reading world data from '{filepath}'...");
            string json = File.ReadAllText(filepath);
            log.Debug("Parsing world json...");
            tilemapData = JsonConvert.DeserializeObject<TilemapData>(json);
            log.Debug($"Loaded {tilemapData.Layers.Length} map layers");
        }

        public TilemapData GetMapDataForPlayer(PlayerEntity player)
        {
            return tilemapData;
        }
    }
}
