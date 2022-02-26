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
        private ClientController client;

        private HashAlgorithm hash;

        public AsyncPromise Login(string username, string password)
        {
            string passwordHash = HashPassword(password);

            return client.Login(username, passwordHash);
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

        private static string ByteArrayToString(byte[] ba)
        {
            StringBuilder hex = new StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
