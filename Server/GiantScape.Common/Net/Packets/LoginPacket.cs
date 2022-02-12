using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GiantScape.Common.Net.Packets
{
    public class LoginPacket : NetworkPacket
    {
        public override PacketType Type => PacketType.Login;

        public override ushort PacketLength => PacketEncoding.GetContentsSize(Username, PasswordHash);

        public string Username { get; private set; }
        public string PasswordHash { get; private set; }

        public override void FromBytes(byte[] bytes)
        {
            throw new NotImplementedException();
        }

        public override byte[] GetContentBytes()
        {
            return PacketEncoding.ContentsToBytes(Username, PasswordHash);
        }
    }
}
