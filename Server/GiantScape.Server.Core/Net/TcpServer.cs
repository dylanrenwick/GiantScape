using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Collections.Generic;

using GiantScape.Common.Logging;
using GiantScape.Common.Net;

using static GiantScape.Server.Net.Constants;

namespace GiantScape.Server.Net
{
    /// <summary>
    /// Listens for Tcp clients and manages Tcp connections.
    /// Raises events to allow handling packets.
    /// </summary>
    internal class TcpServer : Loggable
    {
        public event EventHandler<NetworkEventArgs> NewConnection;

        protected readonly Socket _socket;
        protected readonly Thread listenThread;

        protected List<NetworkConnection> connections;

        protected IPAddress address;
        public ushort Port { get; private set; }

        public bool IsRunning => _socket.IsBound;

        public TcpServer(IPAddress address, ushort port, Logger logger)
            : base(logger)
        {
            this.address = address;
            Port = port;

            _socket = new Socket(SocketType.Stream, ProtocolType.Tcp);

            connections = new List<NetworkConnection>();
            listenThread = new Thread(new ThreadStart(ThreadLoop))
            {
                IsBackground = true
            };
        }

        public void Start()
        {
            var endPoint = new IPEndPoint(address, Port);
            _socket.Bind(endPoint);

            _socket.Listen(NETWORK_TCP_BACKLOG);

            listenThread.Start();
        }

        private void ThreadLoop()
        {
            _socket.BeginAccept(new AsyncCallback(OnAccept), _socket);
        }
        private void SetupConnection(Socket sock)
        {
            var conn = new NetworkConnection(sock, Log);

            conn.ConnectionLost += OnConnectionLost;

            connections.Add(conn);
            NewConnection?.Invoke(this, new NetworkEventArgs(conn));
            conn.BeginRead();
        }

        private void OnAccept(IAsyncResult state)
        {
            if (state.AsyncState != null)
            {
                Socket sock = (Socket)state.AsyncState;
                Socket otherSocket = sock.EndAccept(state);

                SetupConnection(otherSocket);
            }

            ThreadLoop();
        }

        private void OnConnectionLost(object sender, EventArgs e)
        {
            if (sender == null) return;
            NetworkConnection connection = (NetworkConnection)sender;
            Log.Warn($"{connection} Connection dropped");
            connections.Remove(connection);
        }
    }
}
