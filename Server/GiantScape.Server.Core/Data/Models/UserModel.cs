namespace GiantScape.Server.Data.Models
{
    internal class UserModel
    {
        public string ID { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public UserLevel Level { get; set; }
    }
}
