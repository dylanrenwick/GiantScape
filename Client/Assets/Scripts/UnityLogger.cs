using GiantScape.Common.Logging;

namespace GiantScape.Client
{
    internal class UnityLogger : Logger
    {
        public override void Log(string message)
        {
            UnityEngine.Debug.Log(message);
        }
    }
}
