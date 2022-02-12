using System;
using System.Linq;
using System.Text;
using System.Net.Sockets;

namespace GiantScape.Common.Net.Packets
{

    public abstract partial class NetworkPacket
    {
        public abstract PacketType Type { get; }
        public abstract ushort PacketLength { get; }

        public abstract byte[] GetContentBytes();

        public abstract void FromBytes(byte[] bytes);

        public byte[] GetBytes()
        {
            return GetHeaderBytes().Concat(GetContentBytes()).ToArray();
        }

        private byte[] GetHeaderBytes()
        {
            byte[] typeBytes = BitConverter.GetBytes((ushort)Type);
            byte[] lengthBytes = BitConverter.GetBytes(PacketLength);

            return typeBytes.Concat(lengthBytes).ToArray();
        }
    }
}
