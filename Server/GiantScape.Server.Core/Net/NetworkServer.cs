using System;
using System.Net;
using System.Collections.Generic;

using GiantScape.Common.Logging;
using GiantScape.Common.Net;

using static GiantScape.Common.Net.Constants;

namespace GiantScape.Server.Net
{
    internal class NetworkServer : Loggable
    {
        public event EventHandler<EventArgs> ConnectionEstablished;

        private readonly TcpServer tcpServer;

        private readonly Dictionary<NetworkConnection, NetworkClient> networkClients;

        public bool IsRunning => tcpServer.IsRunning;

        public NetworkServer(IPAddress address, ushort port, Logger log)
            : base(log)
        {
            tcpServer = new TcpServer(address, port, Log.SubLogger("TCPSRV"));

            networkClients = new Dictionary<NetworkConnection, NetworkClient>();

            tcpServer.NewConnection += OnTcpConnection;
        }

        public void Start()
        {
            Log.Info("Starting server...");
            Log.Info($"protocol version: {NETWORK_PROTOCOL_VER}");
            Log.Debug($"buffer size: {NETWORK_BUFFER_SIZE}");
            tcpServer.Start();
        }

        private void OnTcpConnection(object sender, NetworkEventArgs e)
        {
            NetworkConnection connection = e.Connection;
            if (!networkClients.ContainsKey(connection))
            {
                Log.Info($"{connection} New tcp connection");
                var newClient = new NetworkClient(connection, Log);
                networkClients[connection] = newClient;
                newClient.ConnectionEstablished += OnConnectionEstablished;
                Log.Debug($"{connection} Sending handshake");
                newClient.BeginHandshake();
            }
        }

        private void OnConnectionEstablished(object sender, EventArgs e)
        {
            ConnectionEstablished?.Invoke(sender, e);
        }
    }
}
