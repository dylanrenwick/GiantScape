using System;

namespace GiantScape.Common.Game.Data
{
    [Serializable]
    public class User
    {
        public int UserID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }
        public UserLevel UserLevel { get; set; }
    }
}
