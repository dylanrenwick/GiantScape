namespace GiantScape.Common.Net.Packets
{
    public enum PacketType : ushort
    {
        None = 0,
        Close = 1,
        Handshake = 2,
        Ack = 3,
        Map = 4,
    }
}
