using System;

namespace GiantScape.Common.Game.Tilemaps
{
    [Serializable]
    public class TilesetData
    {
        public string[] tileNames { get; set; }
        public string tilesetName { get; set; }
    }
}
