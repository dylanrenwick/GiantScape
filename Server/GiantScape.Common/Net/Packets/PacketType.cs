namespace GiantScape.Common.Net.Packets
{
    public enum PacketType : ushort
    {
        None = 0,
        Close = 1,
        Handshake = 2,
        Ack = 3,
        Map = 4,
        LoginRequest = 5,
        Login = 6,
        LoginSuccess = 7,
        LoginFail = 8
    }
}
