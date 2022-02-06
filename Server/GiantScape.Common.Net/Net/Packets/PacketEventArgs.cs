using System;

namespace GiantScape.Common.Net.Packets
{
    public class PacketEventArgs : EventArgs
    {
        public NetworkPacket Packet { get; set; }

        public PacketEventArgs(NetworkPacket packet)
        {
            Packet = packet;
        }
    }
}
