using System;

namespace GiantScape.Common.Logging
{
    public class ConsoleLogger : Logger
    {
        public override void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
