using System;

namespace GiantScape.Common.Net.Packets
{
    public class BinaryPacket : NetworkPacket
    {
        public override PacketType Type => type;

        public override ushort PacketLength => (ushort)content.Length;

        private byte[] content;
        private readonly PacketType type = PacketType.None;

        public override byte[] GetContentBytes() => content;

        public override void FromBytes(byte[] bytes) => content = bytes;

        public BinaryPacket()
        {
            this.content = Array.Empty<byte>();
        }
        public BinaryPacket(PacketType type)
        {
            this.type = type;
            this.content = Array.Empty<byte>();
        }
        public BinaryPacket(byte[] content)
        {
            this.content = content;
        }
        public BinaryPacket(PacketType type, byte[] content)
        {
            this.type = type;
            this.content = content;
        }
    }
}
