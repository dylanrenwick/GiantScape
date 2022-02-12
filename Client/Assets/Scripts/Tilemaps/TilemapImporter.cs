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

        public void OnPacketReceived(NetworkPacket packet)
        {
            if (packet.Type == PacketType.Map)
            {
                var mapPacket = (MapPacket)packet;
                tilemapJsonConverter.LoadJson(mapPacket.MapJson, Vector2Int.zero);
            }
        }

        private void Start()
        {
            network = GameObject.Find("NetworkController").GetComponent<NetworkController>();
            network.PacketReceived.AddListener(OnPacketReceived);
        }

        private static Regex commentRegex = new Regex("\\/\\*.*?\\*\\/");

        private static string StripComments(string json)
        {
            return commentRegex.Replace(json, string.Empty);
        }
    }
}
