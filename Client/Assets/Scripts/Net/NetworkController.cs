using System.Net.Sockets;

using UnityEngine;

using GiantScape.Client.Tilemaps;
using GiantScape.Common.Net;
using GiantScape.Common.Net.Packets;

namespace GiantScape.Client.Net
{
    public class NetworkController : MonoBehaviour
    {
        [SerializeField]
        private string address;
        [SerializeField]
        private int port;

        [SerializeField]
        private TilemapJsonConverter tilemapConverter;

        private NetworkClient client;
        private UnityLogger logger = new UnityLogger();

        private void Start()
        {
            var sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
            var conn = new NetworkConnection(sock, logger);
            client = new NetworkClient(conn, logger);
            client.PacketReceived += OnPacketReceived;

            logger.Info($"Connecting to {address}:{port}...");
            client.BeginConnect(address, port);
        }

        private void OnDestroy()
        {
            client.Close();
        }

        private void OnPacketReceived(object sender, PacketEventArgs e)
        {
            var packet = e.Packet;
            switch (packet.Type)
            {
                case PacketType.Map:
                    var mapPacket = (MapPacket)packet;
                    logger.Info($"Loading map info from server...");
                    tilemapConverter.LoadJson(mapPacket.MapJson, Vector2Int.zero);
                    break;
                case PacketType.None:
                case PacketType.Handshake:
                case PacketType.Ack:
                default:
                    break;
            }
        }
    }
}
