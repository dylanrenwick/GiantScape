using System;

namespace GiantScape.Common.Game.Tilemaps
{
    [Serializable]
    public class TileData
    {
        public string ResourceName { get; set; }

        public TileCollision Collision { get; set; }

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
