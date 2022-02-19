using System;
using System.Linq;

using Newtonsoft.Json;

namespace GiantScape.Common.Game.Tilemaps
{
    [Serializable]
    public class TilemapData
    {
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("size")]
        public Vector2Int Size { get; set; }
        [JsonProperty("layers")]
        public LayerData[] Layers { get; set; }
        [JsonProperty("tileset")]
        public string TilesetID { get; set; }

        public TilemapData Subregion(Vector2Int start, Vector2Int size)
        {
            return new TilemapData
            {
                Name = Name,
                Size = size,
                TilesetID = TilesetID,
                Layers = Layers.Select(l => l.Subregion(start, size, this.Size)).ToArray()
            };
        }
    }
}
