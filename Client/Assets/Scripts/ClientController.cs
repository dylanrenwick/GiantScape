﻿using UnityEngine;

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

        public AsyncPromise Login(string username, string passwordHash)
        {
            var packet = new LoginPacket(username, passwordHash);

            AsyncPromise<NetworkPacket> netRequest = Client.SendWithResponse(packet, PacketType.LoginSuccess);
            return netRequest;
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

        public AsyncPromise<TilesetData> RequestTileset(string tilesetID)
        {
            var promise = new AsyncPromise<TilesetData>();

            var packet = new StringPacket(PacketType.TilesetRequest, tilesetID);

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

        private void OnDestroy()
        {
            Client.Close("Client shutting down");
        }
    }
}
