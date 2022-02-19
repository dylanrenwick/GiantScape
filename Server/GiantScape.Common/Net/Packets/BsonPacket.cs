namespace GiantScape.Common.Net.Packets
{
    public class BsonPacket : NetworkPacket
    {
        private PacketType type;
        public override PacketType Type => type;

        public override ushort PacketLength => PacketEncoding.GetContentsSize(Bson);

        public byte[] Bson { get; private set; }

        public override void FromBytes(byte[] bytes)
        {
            Bson = bytes;
        }

        public override byte[] GetContentBytes()
        {
            return Bson;
        }

        public BsonPacket(PacketType type)
        {
            this.type = type;
        }
        public BsonPacket(PacketType type, byte[] bson)
        {
            this.type = type;
            Bson = bson;
        }
    }
}
