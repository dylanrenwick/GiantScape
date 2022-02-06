using System;

namespace GiantScape.Common.Net
{
    public class NetworkEventArgs : EventArgs
    {
        public NetworkConnection Connection;

        public NetworkEventArgs(NetworkConnection connection)
        {
            Connection = connection;
        }
    }
}
