namespace GiantScape.Common.Net.Packets
{
    public enum PacketType : ushort
    {
        None,
        /* Net */
        Close,
        Handshake,
        Ack,
        /* Login */
        LoginRequest,
        Login,
        LoginSuccess,
        LoginFail,
        /* Game */
        Map,
    }
}
