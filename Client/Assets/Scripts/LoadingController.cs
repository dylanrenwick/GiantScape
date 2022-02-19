using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;

using GiantScape.Client.Net;

namespace GiantScape.Client
{
    /*
     * Procedure detail:
     * 
     * 1. Send Login to server, wait for LoginSuccess
     * 2. Request Map from server, wait for Map
     * 3. Request Tileset from server, wait for Tileset
     * 4. Load new scene, wait for scene load
     * 5. Pass Map and Tileset to TilemapImporter to be loaded into game.
     * 
     */
    public class LoadingController : MonoBehaviour
    {
        [SerializeField]
        private LoginController login;
        [SerializeField]
        private NetworkController network;

        public void Login(string username, string password)
        {
            login.LoginSuccess.AddListener(OnLoginSuccess);
            login.Login(username, password);
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
        }

        private void OnLoginSuccess()
        {
            network.GetMapData(tilemap =>
            {

            });
        }
    }
}
