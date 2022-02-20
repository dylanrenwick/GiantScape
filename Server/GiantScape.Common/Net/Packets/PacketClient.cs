using System;
using System.Collections.Generic;
using System.Net;

using GiantScape.Common.Logging;

namespace GiantScape.Common.Net.Packets
{
    public abstract class PacketClient : Loggable
    {
        protected readonly NetworkConnection connection;

        protected HashSet<Func<NetworkPacket, bool>> packetCallbacks;

        public PacketClient(NetworkConnection connection, Logger log)
            : base(log)
        {
            this.connection = connection;
            packetCallbacks = new HashSet<Func<NetworkPacket, bool>>();

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

        public void SendPacketWithResponse(NetworkPacket packet, Func<NetworkPacket, bool> callback)
        {
            packetCallbacks.Add(callback);
            SendPacket(packet);
        }
        public void SendPacketWithResponse(NetworkPacket packet, PacketType responseType, Func<NetworkPacket, bool> callback)
        {
            SendPacketWithResponse(packet, np => (np.Type == responseType) && callback(np));
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

            var completed = new HashSet<Func<NetworkPacket, bool>>();
            foreach (var callback in packetCallbacks)
            {
                if (callback(e.Packet)) completed.Add(callback);
            }

            foreach (var complete in completed)
            {
                packetCallbacks.Remove(complete);
            }
        }
    }
}
