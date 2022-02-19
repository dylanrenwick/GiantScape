using GiantScape.Common.Logging;

namespace GiantScape.Client
{
    internal class UnityLogger : Logger
    {
        public static UnityLogger Instance = new UnityLogger();

        public override void Log(string message)
        {
            UnityEngine.Debug.Log(message);
        }
    }
}
