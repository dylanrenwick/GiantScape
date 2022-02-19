namespace GiantScape.Server.Data.Models
{
    internal class UserModel : BaseModel
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public string PasswordHash { get; set; }

        public UserLevel Level { get; set; }
    }
}
