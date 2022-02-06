namespace GiantScape.Common.Logging
{
    public abstract class Loggable
    {
        protected Logger Log;

        public Loggable(Logger log)
        {
            Log = log;
        }
    }
}
