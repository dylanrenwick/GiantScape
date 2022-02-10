using System;

namespace GiantScape.Common.Net.Packets
{
    public class MiscPacket : NetworkPacket
    {
        public override PacketType Type => type;

        public override ushort PacketLength => (ushort)content.Length;

        private byte[] content;
        private readonly PacketType type = PacketType.None;

        public override byte[] GetContentBytes() => content;

        public override void FromBytes(byte[] bytes) => content = bytes;

        public MiscPacket()
        {
            this.content = Array.Empty<byte>();
        }
        public MiscPacket(PacketType type)
        {
            this.type = type;
            this.content = Array.Empty<byte>();
        }
        public MiscPacket(byte[] content)
        {
            this.content = content;
        }
    }
}
