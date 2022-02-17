using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

using UnityEngine;

using GiantScape.Client.Net;
using GiantScape.Common.Net.Packets;

namespace GiantScape.Client.Tilemaps
{
    public class TilemapImporter : MonoBehaviour
    {
        [SerializeField]
        private TextAsset jsonFile;

        [SerializeField]
        private TilemapJsonConverter tilemapJsonConverter;

        private NetworkController network;

        public void Import()
        {
            if (tilemapJsonConverter == null) return;

            Debug.Log("Importing from file...");
            tilemapJsonConverter.LoadJson(StripComments(jsonFile.text), Vector2Int.zero);
        }

#if UNITY_EDITOR
        public void Export()
        {
            throw new System.NotImplementedException();
        }
#endif

        public void OnPacketReceived(EventState<NetworkPacket> state)
        {
            var packet = state.State;
            if (packet.Type == PacketType.Map)
            {
                var mapPacket = (BsonPacket)packet;
                LoadMapFromPacket(mapPacket);
                state.Handled = true;
            }
        }

        private void Start()
        {
            network = GameObject.Find("NetworkController").GetComponent<NetworkController>();
            network.PacketReceived.AddListener(OnPacketReceived);
            IEnumerable<BsonPacket> queuedMapPackets = network.PacketBacklog
                .Where(packet => packet.Type == PacketType.Map)
                .Cast<BsonPacket>().ToList();

            foreach (BsonPacket packet in queuedMapPackets)
            {
                LoadMapFromPacket(packet);
                network.PacketBacklog.Remove(packet);
            }
        }

        private void LoadMapFromPacket(BsonPacket packet)
        {
            tilemapJsonConverter.LoadBson(packet.MapBson, Vector2Int.zero);
        }

        private static Regex commentRegex = new Regex("\\/\\*.*?\\*\\/");

        private static string StripComments(string json)
        {
            return commentRegex.Replace(json, string.Empty);
        }
    }
}
