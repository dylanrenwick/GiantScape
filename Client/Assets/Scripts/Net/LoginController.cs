using System.Collections;
using System.Text;
using System.Security.Cryptography;

using UnityEngine;
using UnityEngine.Events;

using GiantScape.Common.Net.Packets;

namespace GiantScape.Client.Net
{
    internal class LoginController : MonoBehaviour
    {
        [SerializeField]
        private NetworkController network;

        public UnityEvent LoginSuccess;

        private HashAlgorithm hash;

        public void Login(string username, string password)
        {
            string passwordHash = HashPassword(password);

            var loginPacket = new LoginPacket(username, passwordHash);
            network.PacketReceived.AddListener(OnPacketReceived);

            network.SendPacket(loginPacket);
        }

        private void Start()
        {
            hash = SHA512.Create();
        }

        private string HashPassword(string password)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] hashBytes = hash.ComputeHash(passwordBytes);
            return ByteArrayToString(hashBytes);
        }

        private void OnPacketReceived(EventState<NetworkPacket> state)
        {
            var packet = state.State;
            if (packet.Type == PacketType.LoginSuccess)
            {
                LoginSuccess.Invoke();
                state.Handled = true;
            }
        }

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
