using System.Linq;
using System.Text;

namespace GiantScape.Common.Net.Packets
{

    public class HandshakePacket : NetworkPacket
    {
        public ushort UdpPort { get; private set; }
        public string ProtocolVersion { get; private set; }

        public override PacketType Type => PacketType.Handshake;

        public override ushort PacketLength => (ushort)GetSize(ProtocolVersion);

        public HandshakePacket() { }
        public HandshakePacket(string protoVer)
        {
            ProtocolVersion = protoVer;
        }

        public override byte[] GetContentBytes()
        {
            return ContentsToBytes(ProtocolVersion).ToArray();
        }

        public override void FromBytes(byte[] bytes)
        {
            ProtocolVersion = Encoding.ASCII.GetString(bytes, 0, bytes.Length);
        }
    }
}
