using System;

using GiantScape.Common.Logging;
using GiantScape.Common.Net;
using GiantScape.Common.Net.Packets;

using static GiantScape.Common.Net.Constants;

namespace GiantScape.Client.Net
{
    public class NetworkClient : PacketClient
    {
        public event EventHandler<PacketEventArgs> PacketReceived;

        public bool Connected => state != ClientState.None
            && state != ClientState.Closed
            && state != ClientState.Error;
        public bool Established => state == ClientState.Connected;

        private ClientState state;

        public NetworkClient(NetworkConnection connection, Logger log)
            :base(connection, log)
        {
            connection.Connected += OnConnected;

            state = connection.IsConnected
                ? ClientState.TcpConnected
                : ClientState.Init;
        }

        public override void HandlePacket(NetworkPacket packet)
        {
            switch (state)
            {
                case ClientState.None:
                case ClientState.Closed:
                case ClientState.Error:
                case ClientState.TcpConnected:
                    if (packet.Type != PacketType.Handshake) throw new Exception();
                    var handshake = (HandshakePacket)packet;

                    if (handshake.ProtocolVersion != NETWORK_PROTOCOL_VER) Close();
                    connection.SendPacket(handshake);
                    state = ClientState.HandshakeSent;
                    break;
                case ClientState.HandshakeSent:
                    if (packet.Type != PacketType.Ack) throw new Exception();
                    state = ClientState.Connected;
                    break;
                case ClientState.Connected:
                    PacketReceived?.Invoke(this, new PacketEventArgs(packet));
                    break;
                default:
                    break;
            }
        }

        private void OnConnected(object sender, NetworkEventArgs e)
        {
            if (state != ClientState.Closed && state != ClientState.Init) return;

            Log.Info("Successfully connected");
            state = ClientState.TcpConnected;
            Log.Info("Listening for packets...");
            connection.BeginRead();
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
            AwaitingAuth,
        }
    }
}
