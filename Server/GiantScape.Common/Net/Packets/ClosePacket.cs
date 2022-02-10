using System.Text;

namespace GiantScape.Common.Net.Packets
{

    public class ClosePacket : NetworkPacket
    {
        public string Reason { get; set; }

        public override PacketType Type => PacketType.Close;

        public override ushort PacketLength => (ushort)GetSize(Reason);

        public ClosePacket() { }
        public ClosePacket(string reason)
        {
            Reason = reason;
        }

        public override void FromBytes(byte[] bytes)
        {
            Reason = Encoding.ASCII.GetString(bytes);
        }

        public override byte[] GetContentBytes()
        {
            return Encoding.ASCII.GetBytes(Reason);
        }
    }
}
