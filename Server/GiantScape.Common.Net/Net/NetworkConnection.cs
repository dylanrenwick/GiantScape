using System;
using System.Net;
using System.Net.Sockets;

using GiantScape.Common.Logging;
using GiantScape.Common.Net.Packets;

namespace GiantScape.Common.Net
{
    public class NetworkConnection : Loggable
    {
        public string ID { get; private set; }

        public event EventHandler<PacketEventArgs> PacketReceived;
        public event EventHandler<NetworkEventArgs> Connected;
        public event EventHandler<NetworkEventArgs> ConnectionLost;

        public bool IsConnected => socket.Connected;

        protected readonly Socket socket;

        protected PacketStream stream;

        public NetworkConnection(Socket socket, Logger log)
            : base(log)
        {
            this.socket = socket;
            if (IsConnected) stream = new PacketStream(socket, log);
            ID = Guid.NewGuid().ToString();
        }

        public void BeginConnect(IPEndPoint endPoint)
        {
            if (IsConnected) return;

            socket.BeginConnect(endPoint, new AsyncCallback(OnConnect), null);
        }
        private void OnConnect(IAsyncResult res)
        {
            socket.EndConnect(res);
            stream = new PacketStream(socket, Log);
            Connected?.Invoke(this, new NetworkEventArgs(this));
        }

        public void Close()
        {
            socket.Close();
        }

        public void SendBytes(byte[] buffer)
        {
            socket.Send(buffer);
        }

        public void SendPacket(NetworkPacket packet)
        {
            Log.Debug($"{this} Sending {packet.Type} packet...");
            SendBytes(packet.GetBytes());
        }

        public void BeginRead()
        {
            Log.Debug($"{this} Listening for packets...");
            stream.BeginRead(OnReadPacket);
        }
        private void OnReadPacket(NetworkPacket packet)
        {
            Log.Debug($"{this} Received {packet.Type} packet");
            PacketReceived?.Invoke(this, new PacketEventArgs(packet));

            if (socket.Connected) BeginRead();
        }

        public override bool Equals(object obj)
        {
            return obj is NetworkConnection connection
                && connection.socket.Equals(socket);
        }

        public override int GetHashCode()
        {
            return socket.GetHashCode();
        }

        public override string ToString()
        {
            return $"{{{ID.Substring(0, 6)}-{ID.Substring(ID.Length - 6)}}}";
        }
    }
}
