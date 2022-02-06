using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Sockets;

using GiantScape.Common.Logging;

using static GiantScape.Common.Net.Constants;

namespace GiantScape.Common.Net.Packets
{
    public class PacketStream : Loggable
    {
        private readonly Socket socket;
        private readonly NetworkStream stream;
        private readonly NetworkPacketReader packetReader;

        public PacketStream(Socket sock, Logger log)
            : base(log)
        {
            socket = sock;
            stream = new NetworkStream(socket);
            packetReader = new NetworkPacketReader(stream, log);
        }

        public NetworkPacket ReadPacket() => packetReader.ReadPacket();

        public void BeginRead(Action<NetworkPacket> callback) => packetReader.BeginReadPacket(callback);

        public void WritePacket(NetworkPacket packet)
        {
            byte[] bytes = packet.GetBytes();
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}
