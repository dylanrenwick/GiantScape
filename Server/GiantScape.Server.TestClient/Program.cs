using System;
using System.Net.Sockets;

using GiantScape.Common.Logging;
using GiantScape.Common.Net.Packets;

namespace GiantScape.Server.TestClient
{
    class Program
    {
        private const string address = "127.0.0.1";
        private const ushort port = 17000;

        private static Socket socket;
        private static PacketStream stream;

        private static Logger logger;

        private static void Main()
        {
            logger = new ConsoleLogger();

            logger.Debug($"address: {address}");
            logger.Debug($"port: {port}");

            socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            logger.Info("Press enter to connect.");
            Console.ReadLine();

            logger.Info($"Connecting to {address}:{port}...");
            socket.Connect(address, port);

            stream = new PacketStream(socket, logger);

            try
            {
                Start();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
        }

        private static void Start()
        {
            while (socket.Connected)
            {
                logger.Info("Waiting for incoming data...");
                NetworkPacket packet = stream.ReadPacket();
                logger.Info($"Read {packet.Type} packet");
                HandlePacket(packet);
            }
        }

        private static void HandlePacket(NetworkPacket packet)
        {
            switch (packet.Type)
            {
                case PacketType.Close:
                    HandleClose((StringPacket)packet);
                    break;
                case PacketType.Handshake:
                    HandleHandshake((HandshakePacket)packet);
                    break;
                case PacketType.None:
                default:
                    break;
            }
        }

        private static void HandleHandshake(HandshakePacket packet)
        {
            logger.Info($"Received handshake, protocol version {packet.ProtocolVersion}");
            stream.WritePacket(packet);
        }

        private static void HandleClose(StringPacket packet)
        {
            logger.Info("Received close packet, closing connection");
            logger.Info($"Reason: '{packet.StringVal}'");
            socket.Close();
        }
    }
}
