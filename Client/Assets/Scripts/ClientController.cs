using UnityEngine;

using GiantScape.Common;
using GiantScape.Common.Game.Tilemaps;
using GiantScape.Common.Net;
using GiantScape.Common.Net.Packets;

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

        public AsyncPromise<TilemapData> RequestMap()
        {
            var promise = new AsyncPromise<TilemapData>();

            var packet = new BinaryPacket(PacketType.MapRequest);

            AsyncPromise<NetworkPacket> netRequest = Client.SendWithResponse(packet, PacketType.Map);
            netRequest.Done += (_, netPacket) =>
            {
                var bsonPacket = (BinaryPacket)netPacket;
                TilemapData data = Serializer.Deserialize<TilemapData>(bsonPacket.Content);
                promise.Result = data;
                promise.IsDone = true;
            };

            return promise;
        }

        public AsyncPromise<TilesetData> RequestTileset()
        {
            var promise = new AsyncPromise<TilesetData>();

            var packet = new BinaryPacket(PacketType.TilesetRequest);

            AsyncPromise<NetworkPacket> netRequest = Client.SendWithResponse(packet, PacketType.Tileset);
            netRequest.Done += (_, netPacket) =>
            {
                var bsonPacket = (BinaryPacket)netPacket;
                TilesetData data = Serializer.Deserialize<TilesetData>(bsonPacket.Content);
                promise.Result = data;
                promise.IsDone = true;
            };

            return promise;
        }

        private void Start()
        {
            Client = new GameClient(address, port, UnityLogger.Instance.SubLogger("GAMESV"));
        }
    }
}
