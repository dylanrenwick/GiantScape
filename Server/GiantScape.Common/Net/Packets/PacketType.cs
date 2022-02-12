namespace GiantScape.Common.Net.Packets
{
    public enum PacketType : ushort
    {
        None = 0,
        /* Net */
        Close = 1,
        Handshake = 2,
        Ack = 3,
        /* Login */
        LoginRequest = 4,
        Login = 5,
        LoginSuccess = 6,
        LoginFail = 7,
        /* Game */
        Map = 8,
    }
}
