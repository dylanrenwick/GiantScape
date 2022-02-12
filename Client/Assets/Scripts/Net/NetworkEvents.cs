using UnityEngine.Events;

using GiantScape.Common.Net.Packets;

namespace GiantScape.Client.Net
{
    public class PacketEvent : UnityEvent<NetworkPacket> { }
}
