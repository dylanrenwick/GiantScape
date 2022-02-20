using System.Net.Sockets;

using GiantScape.Client.Net;
using GiantScape.Common.Game;
using GiantScape.Common.Logging;
using GiantScape.Common.Net;

namespace GiantScape.Client
{
    internal class GameClient : Loggable
    {
        private NetworkClient networkClient;

        private World world;

        private string address;
        private ushort port;

        public GameClient(string address, ushort port, Logger log)
            : base(log)
        {
            this.address = address;
            this.port = port;

            var socket = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var conn = new NetworkConnection(socket, log);
            networkClient = new NetworkClient(conn, log);

            world = new World(log.SubLogger("GAMECL"));
        }

        public void Start()
        {
            networkClient.BeginConnect(address, port);
        }
    }
}
