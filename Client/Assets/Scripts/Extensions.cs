using GiantScape.Common.Game;

using Vector2IntUnity = UnityEngine.Vector2Int;

namespace GiantScape.Client
{
    internal static class Extensions
    {
        public static Vector2IntUnity ToUnity(this Vector2Int self)
        {
            return new Vector2IntUnity(self.x, self.y);
        }
    }
}
