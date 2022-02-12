﻿using System.Collections;
using System.Text;
using System.Security.Cryptography;

using UnityEngine;
using UnityEngine.SceneManagement;

using GiantScape.Common.Net.Packets;

namespace GiantScape.Client.Net
{
    internal class LoginController : MonoBehaviour
    {
        [SerializeField]
        private NetworkController network;

        [SerializeField]
        private string scenePath;

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
            return Encoding.ASCII.GetString(hashBytes);
        }

        private void OnPacketReceived(NetworkPacket packet)
        {
            if (packet.Type == PacketType.LoginSuccess)
                UnityMainThreadDispatcher.Instance().Enqueue(LoadMainScene());
        }

        private IEnumerator LoadMainScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenePath);

            while (!asyncLoad.isDone)
            {
                yield return null;
            }
        }
    }
}
