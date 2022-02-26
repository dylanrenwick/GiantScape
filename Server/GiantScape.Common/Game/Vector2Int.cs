using System;

namespace GiantScape.Common.Game
{
    [Serializable]
    public class Vector2Int
    {
        public int X { get; set; }
        public int Y { get; set; }

        public Vector2Int(int x, int y)
        {
            X = x;
            Y = y;
        }
    }
}
