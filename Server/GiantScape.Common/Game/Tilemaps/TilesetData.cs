using System;

namespace GiantScape.Common.Game.Tilemaps
{
    [Serializable]
    public class TilesetData
    {
        public TileData[] tiles { get; set; }
        public string tilesetName { get; set; }
    }
}
