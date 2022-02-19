using System;

using Newtonsoft.Json;

namespace GiantScape.Common.Game.Tilemaps
{
    [Serializable]
    public class TileData
    {
        [JsonProperty("name")]
        public string ResourceName { get; set; }

        [JsonProperty("collision")]
        public TileCollision Collision { get; set; }

        [JsonProperty("pathWeight")]
        public float PathingWeight { get; set; }
    }

    public enum TileCollision : byte
    {
        None = 0,
        Left = 1,
        Right = 2,
        Up = 4,
        Down = 8,
    }
}
