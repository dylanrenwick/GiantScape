using System.Text;

namespace GiantScape.Common.Net.Packets
{

    public class ClosePacket : NetworkPacket
    {
        public string Reason { get; set; }

        public override PacketType Type => PacketType.Close;

        public override ushort PacketLength => PacketEncoding.GetSingleSize(Reason);

        public ClosePacket() { }
        public ClosePacket(string reason)
        {
            Reason = reason;
        }

        public override void FromBytes(byte[] bytes)
        {
            Reason = PacketEncoding.BytesToString(bytes);
        }

        public override byte[] GetContentBytes()
        {
            return PacketEncoding.StringToBytes(Reason);
        }
    }
}
