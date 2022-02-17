namespace GiantScape.Common.Net.Packets
{
    public class BsonPacket : NetworkPacket
    {
        private PacketType type;
        public override PacketType Type => type;

        public override ushort PacketLength => PacketEncoding.GetContentsSize(MapBson);

        public byte[] MapBson { get; private set; }

        public override void FromBytes(byte[] bytes)
        {
            MapBson = bytes;
        }

        public override byte[] GetContentBytes()
        {
            return MapBson;
        }

        public BsonPacket() { }
        public BsonPacket(PacketType type, byte[] bson)
        {
            this.type = type;
            MapBson = bson;
        }
    }
}
