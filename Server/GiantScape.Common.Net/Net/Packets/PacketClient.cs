using System.Net;

using GiantScape.Common.Logging;

namespace GiantScape.Common.Net.Packets
{
    public abstract class PacketClient : Loggable
    {
        protected readonly NetworkConnection connection;

        public PacketClient(NetworkConnection connection, Logger log)
            : base(log)
        {
            this.connection = connection;

            connection.PacketReceived += OnPacketReceived;
        }

        public abstract void HandlePacket(NetworkPacket packet);

        public void BeginConnect(string address, int port)
        {
            connection.BeginConnect(new IPEndPoint(IPAddress.Parse(address), port));
        }

        public void Close(string reason = "", bool sendPacket = true)
        {
            if (sendPacket)
            {
                var packet = new ClosePacket(reason);
                connection.SendPacket(packet);
            }
            connection.Close();
        }

        public void SendPacket(NetworkPacket packet)
        {
            connection.SendPacket(packet);
        }

        public override string ToString()
        {
            return connection.ToString();
        }

        private void OnPacketReceived(object sender, PacketEventArgs e)
        {
            if (e.Packet.Type == PacketType.Close)
            {
                var closePacket = (ClosePacket)e.Packet;
                Close(closePacket.Reason, false);
                return;
            }

            HandlePacket(e.Packet);
        }
    }
}
