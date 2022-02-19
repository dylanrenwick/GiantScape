using System;

using Newtonsoft.Json;

namespace GiantScape.Common.Game.Tilemaps
{
    [Serializable]
    public class TilesetData
    {
        [JsonProperty("tiles")]
        public TileData[] Tiles { get; set; }
        [JsonProperty("name")]
        public string TilesetName { get; set; }
    }
}
