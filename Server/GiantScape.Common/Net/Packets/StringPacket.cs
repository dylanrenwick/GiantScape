namespace GiantScape.Common.Net.Packets
{
    public class StringPacket : NetworkPacket
    {
        private PacketType type;
        public override PacketType Type => type;

        public override ushort PacketLength => PacketEncoding.GetContentsSize(StringVal);

        public string StringVal { get; private set; }

        public override void FromBytes(byte[] bytes)
        {
            StringVal = PacketEncoding.BytesToString(bytes);
        }

        public override byte[] GetContentBytes()
        {
            return PacketEncoding.StringToBytes(StringVal);
        }

        public StringPacket(PacketType type)
        {
            this.type = type;
        }
        public StringPacket(PacketType type, string str)
        {
            this.type = type;
            StringVal = str;
        }
    }
}
