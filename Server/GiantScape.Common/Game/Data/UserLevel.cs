using System;

namespace GiantScape.Common.Game.Data
{
    [Serializable]
    public class UserLevel
    {
        public int UserLevelID { get; set; }
        public string Name { get; set; }
        public bool IsAdmin { get; set; }
    }
}
