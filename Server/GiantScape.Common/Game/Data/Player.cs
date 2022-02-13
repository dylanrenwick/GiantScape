using System;

namespace GiantScape.Common.Game.Data
{
    [Serializable]
    public class Player
    {
        public int PlayerID { get; set; }
        public User User { get; set; }
        public int Mpde { get; set; }
        public string DataStoreName { get; set; }
    }
}
