namespace GiantScape.Server.Accounts
{
    internal class Account
    {
        public long Id { get; set; }
        public string Username { get; set; }

        public bool LoggedIn { get; set; }
    }
}
