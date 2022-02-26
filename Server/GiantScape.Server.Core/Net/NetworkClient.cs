using System;

using GiantScape.Common.Logging;
using GiantScape.Common.Net;
using GiantScape.Common.Net.Packets;

using static GiantScape.Common.Net.Constants;

namespace GiantScape.Server.Net
{
    public class NetworkClient : PacketClient
    {
        public event EventHandler<EventArgs> ConnectionEstablished;
        public event EventHandler<PacketEventArgs> PacketReceived;

        private ClientState state;

        public NetworkClient(NetworkConnection connection, Logger log)
            : base(connection, log)
        {
            connection.Connected += OnConnected;

            state = connection.IsConnected
                ? ClientState.TcpConnected
                : ClientState.Init;
        }

        public void BeginHandshake()
        {
            var packet = new HandshakePacket(NETWORK_PROTOCOL_VER);

            connection.SendPacket(packet);
            state = ClientState.HandshakeSent;
        }

        public override void HandlePacket(NetworkPacket packet)
        {
            switch (state)
            {
                case ClientState.None:
                case ClientState.Closed:
                case ClientState.Error:
                case ClientState.TcpConnected:
                    break;
                case ClientState.HandshakeSent:
                    if (packet.Type != PacketType.Handshake) throw new System.Exception();
                    var handshake = (HandshakePacket)packet;

                    if (handshake.ProtocolVersion != NETWORK_PROTOCOL_VER) Close();
                    else
                    {
                        state = ClientState.Connected;
                        var ack = new BinaryPacket(PacketType.Ack);
                        connection.SendPacket(ack);
                        ConnectionEstablished?.Invoke(this, new EventArgs());
                    }
                    break;
                case ClientState.Connected:
                default:
                    PacketReceived?.Invoke(this, new PacketEventArgs(packet));
                    break;
            }
        }

        private void OnConnected(object sender, NetworkEventArgs e)
        {
            if (state != ClientState.Closed && state != ClientState.Init) return;
            state = ClientState.TcpConnected;
        }

        private enum ClientState
        {
            None,
            Closed,
            Error,
            Init,
            TcpConnected,
            HandshakeSent,
            Connected,
        }
    }
}
