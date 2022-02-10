using System.IO;

using Newtonsoft.Json;

using GiantScape.Common.Game.Tilemaps;
using GiantScape.Common.Logging;

namespace GiantScape.Common.Game
{
    public class World : Loggable
    {
        private const string filepath = @"Resources/Tilemaps/overworld.json";

        private TilemapData tilemapData;

        public World(Logger log)
            : base(log)
        {
            log.Info($"Reading world data from '{filepath}'...");
            string json = File.ReadAllText(filepath);
            log.Debug("Parsing world json...");
            tilemapData = JsonConvert.DeserializeObject<TilemapData>(json);
            log.Debug($"Loaded {tilemapData.layers.Length} map layers");
        }

        public TilemapData GetMapDataForPlayer(Player player)
        {
            return tilemapData;
        }
    }
}
