using GiantScape.Common.Logging;

namespace GiantScape.Server.Console
{
    class Program
    {
        static void Main()
        {
            var logger = new ConsoleLogger();
            logger.ShowDebug = false;

            var gameServer = new GameServer("127.0.0.1", 17000, logger.SubLogger("GMESRV"));
            gameServer.Start();
        }
    }
}
