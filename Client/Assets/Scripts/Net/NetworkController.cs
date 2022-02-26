using System;
using System.Collections.Generic;
using System.Net.Sockets;

using UnityEngine;

using GiantScape.Common;
using GiantScape.Common.Game.Tilemaps;
using GiantScape.Common.Net;
using GiantScape.Common.Net.Packets;

using Logger = GiantScape.Common.Logging.Logger;

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
        private Logger logger = UnityLogger.Instance.SubLogger("NETWRK");

        public void GetMapData(Action<TilemapData> callback)
        {
            client.SendPacketWithResponse(
                new BinaryPacket(PacketType.MapRequest),
                PacketType.Map,
                np =>
                {
                    try
                    {
                        var bsonPacket = (BinaryPacket)np;
                        var tilemap = Serializer.Deserialize<TilemapData>(bsonPacket.Content);
                        callback(tilemap);
                        return true;
                    }
                    catch
                    {
                        return false;
                    }
                }
            );
        }

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

            NetworkPacket packet = e.Packet;
            switch (packet.Type)
            {
                case PacketType.Map:
                case PacketType.Tileset:
                    HandleBinaryPacket((BinaryPacket)packet);
                    break;
                default:
                    break;
            }

            if (!eventState.Handled) PacketBacklog.Add(e.Packet);
        }

        private void HandleBinaryPacket(BinaryPacket packet)
        {
            switch (packet.Type)
            {
                case PacketType.Map:
                    TilemapData mapData = Serializer.Deserialize<TilemapData>(packet.Content);
                    break;
                case PacketType.Tileset:
                    break;
                default:
                    break;
            }
        }
    }
}
