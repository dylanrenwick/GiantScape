using UnityEngine;

namespace GiantScape.Client
{
    public class ClientController : MonoBehaviour
    {
        [SerializeField]
        private string address;
        [SerializeField]
        private ushort port;

        public GameClient Client { get; private set; }

        public AsyncPromise ConnectAsync()
        {
            return Client.Start();
        }

        private void Start()
        {
            Client = new GameClient(address, port, UnityLogger.Instance.SubLogger("GAMESV"));
        }
    }
}
