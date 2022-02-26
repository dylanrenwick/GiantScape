using System;
using System.Net.Sockets;

using GiantScape.Client.Net;
using GiantScape.Common.Game;
using GiantScape.Common.Logging;
using GiantScape.Common.Net;
using GiantScape.Common.Net.Packets;

namespace GiantScape.Client
{
    public class GameClient : Loggable
    {
        public event EventHandler<PacketEventArgs> PacketReceived;

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
            networkClient.PacketReceived += (_, e) => PacketReceived?.Invoke(this, e);

            world = new World(log.SubLogger("GAMECL"));
        }

        public AsyncPromise Start()
        {
            var promise = new AsyncPromise();
            networkClient.ConnectionEstablished += (_, _2) => promise.IsDone = true;
            networkClient.BeginConnect(address, port);
            return promise;
        }

        public AsyncPromise<NetworkPacket> SendWithResponse(NetworkPacket packet, params PacketType[] types)
        {
            var promise = new AsyncPromise<NetworkPacket>();
            networkClient.SendPacketWithResponse(packet, types, np =>
            {
                promise.Result = np;
                promise.IsDone = true;

                return true;
            });
            return promise;
        }

        public void Close(string reason)
        {
            networkClient.Close(reason);
        }
    }
}
