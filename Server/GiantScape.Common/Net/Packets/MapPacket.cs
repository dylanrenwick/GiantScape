using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantScape.Common.Net.Packets
{
    public class MapPacket : NetworkPacket
    {
        public override PacketType Type => PacketType.Map;

        public override ushort PacketLength => PacketEncoding.GetSize(MapJson);

        public string MapJson { get; private set; }

        public override void FromBytes(byte[] bytes)
        {
            MapJson = Encoding.ASCII.GetString(bytes);
        }

        public override byte[] GetContentBytes()
        {
            return PacketEncoding.ContentsToBytes(MapJson);
        }

        public MapPacket() { }
        public MapPacket(string json)
        {
            MapJson = json;
        }
    }
}
