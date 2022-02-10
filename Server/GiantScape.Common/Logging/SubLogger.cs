namespace GiantScape.Common.Logging
{
    public class SubLogger : Logger
    {
        private readonly Logger parent;

        public SubLogger(Logger parent, string logLabel = "")
            : base(logLabel)
        {
            this.parent = parent;
        }

        public override void Log(string message) => parent.Log(message);
    }
}
