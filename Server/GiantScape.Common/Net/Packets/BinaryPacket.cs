using System;

namespace GiantScape.Common.Net.Packets
{
    public class BinaryPacket : NetworkPacket
    {
        public override PacketType Type => type;
        private readonly PacketType type = PacketType.None;

        public override ushort PacketLength => (ushort)Content.Length;

        public byte[] Content { get; private set; }

        public override byte[] GetContentBytes() => Content;

        public override void FromBytes(byte[] bytes) => Content = bytes;

        public BinaryPacket()
        {
            this.Content = Array.Empty<byte>();
        }
        public BinaryPacket(PacketType type)
        {
            this.type = type;
            this.Content = Array.Empty<byte>();
        }
        public BinaryPacket(byte[] content)
        {
            this.Content = content;
        }
        public BinaryPacket(PacketType type, byte[] content)
        {
            this.type = type;
            this.Content = content;
        }
    }
}
