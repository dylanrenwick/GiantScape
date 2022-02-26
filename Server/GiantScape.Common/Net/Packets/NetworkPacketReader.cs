using System;
using System.Linq;
using System.Net.Sockets;

using GiantScape.Common.Logging;

using static GiantScape.Common.Net.Constants;

namespace GiantScape.Common.Net.Packets
{
    internal class NetworkPacketReader : Loggable
    {
        private byte[] buffer;

        private readonly NetworkStream stream;

        private PacketType currentPacketType;
        private ushort currentPacketLength;

        public NetworkPacketReader(NetworkStream stream, Logger log)
            : base(log)
        {
            this.stream = stream;

            buffer = new byte[NETWORK_BUFFER_SIZE];
        }

        public NetworkPacket ReadPacket()
        {
            Log.Debug("Reading packet synchronously...");
            stream.Read(buffer, 0, 4);
            PacketType packetType = (PacketType)BitConverter.ToUInt16(buffer, 0);
            ushort packetLength = BitConverter.ToUInt16(buffer, 2);
            Log.Debug($"{packetType} packet of length {packetLength}");

            stream.Read(buffer, 4, packetLength);

            return ConstructPacket(
                packetType,
                buffer.Skip(4).Take(packetLength).ToArray()
            );
        }

        public void BeginReadPacket(Action<NetworkPacket> callback)
        {
            Log.Debug("Reading packet asynchronously...");
            stream.BeginRead(buffer, 0, 4, new AsyncCallback(OnReadPacketHeader), callback);
        }

        private void OnReadPacketHeader(IAsyncResult res)
        {
            stream.EndRead(res);

            currentPacketType = (PacketType)BitConverter.ToUInt16(buffer, 0);
            currentPacketLength = BitConverter.ToUInt16(buffer, 2);
            Log.Debug($"{currentPacketType} packet of length {currentPacketLength}");

            if (currentPacketLength > 0)
            {
                if (currentPacketLength > buffer.Length - 4)
                    Array.Resize(ref buffer, currentPacketLength + 4);
                stream.BeginRead(buffer, 4, currentPacketLength, new AsyncCallback(OnReadPacket), res.AsyncState);
            }
            else
            {
                FinalizePacket(
                    currentPacketType,
                    Array.Empty<byte>(),
                    (Action<NetworkPacket>)res.AsyncState
                );
            }
        }

        private void OnReadPacket(IAsyncResult res)
        {
            stream.EndRead(res);

            FinalizePacket(
                currentPacketType,
                buffer.Skip(4).Take(currentPacketLength).ToArray(),
                (Action<NetworkPacket>)res.AsyncState
            );
        }

        private void FinalizePacket(PacketType type, byte[] packetBytes, Action<NetworkPacket> callback)
        {

            Log.Debug("Packet contents read, constructing packet...");
            NetworkPacket packet = ConstructPacket(type, packetBytes);
            Log.Debug("Packet constructed");

            callback(packet);
        }

        private NetworkPacket ConstructPacket(PacketType type, byte[] packet)
        {
            switch (type)
            {
                case PacketType.None:
                    return new BinaryPacket(packet);
                case PacketType.Handshake:
                    var handshake = new HandshakePacket();
                    handshake.FromBytes(packet);
                    return handshake;
                case PacketType.Login:
                    var login = new LoginPacket();
                    login.FromBytes(packet);
                    return login;
                case PacketType.TilesetRequest:
                case PacketType.Close:
                    var strPacket = new StringPacket(type);
                    strPacket.FromBytes(packet);
                    return strPacket;
                default:
                    var binPacket = new BinaryPacket(type);
                    binPacket.FromBytes(packet);
                    return binPacket;
            }
        }
    }
}
