using System.Net.Sockets;

using UnityEngine;

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
        private PacketEvent PacketReceived;

        private NetworkClient client;
        private UnityLogger logger = new UnityLogger();

        public void SendPacket(NetworkPacket packet)
        {
            client.SendPacket(packet);
        }

        private void Start()
        {
            DontDestroyOnLoad(this);

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
            PacketReceived.Invoke(e.Packet);
        }
    }
}
