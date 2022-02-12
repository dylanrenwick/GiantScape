using System.Collections.Generic;
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

        public PacketEvent PacketReceived;

        public HashSet<NetworkPacket> PacketBacklog;

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
            PacketBacklog = new HashSet<NetworkPacket>();
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
            var eventState = new EventState<NetworkPacket>(e.Packet);
            PacketReceived.Invoke(eventState);
            if (!eventState.Handled) PacketBacklog.Add(e.Packet);
        }
    }
}
