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

        public LoginPacket() { }
        public LoginPacket(string username, string passwordHash)
        {
            Username = username;
            PasswordHash = passwordHash;
        }

        public override void FromBytes(byte[] bytes)
        {
            string username = PacketEncoding.BytesToString(bytes);
            int offset = PacketEncoding.GetSingleSize(username);
            string passwordHash = PacketEncoding.BytesToString(bytes, offset);

            Username = username;
            PasswordHash = passwordHash;
        }

        public override byte[] GetContentBytes()
        {
            return PacketEncoding.ContentsToBytes(Username, PasswordHash);
        }
    }
}
