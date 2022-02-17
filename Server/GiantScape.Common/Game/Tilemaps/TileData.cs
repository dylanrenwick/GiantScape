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
        /*     */ None = 0,
        /*     */
        /* |X  */ Left,
        /*     */
        /*  X| */ Right,
        /*  _  */
        /*  X  */ Up,
        /*     */
        /*  _  */ Down,
        /*  _  */
        /* |X  */ UpLeft,
        /*  _  */
        /*  X| */ UpRight,
        /*     */
        /*  _| */ DownRight,
        /*     */
        /* |_  */ DownLeft,
        /*     */
        /* |_| */ OpenUp,
        /*  _  */
        /* |_  */ OpenRight,
        /*  _  */
        /* |X| */ OpenDown,
        /*  _  */
        /*  _| */ OpenLeft,
        /*  _  */
        /* |_| */ Full
    }
}
