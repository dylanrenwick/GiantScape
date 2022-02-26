using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using UnityEngine;
using UnityEngine.SceneManagement;

using GiantScape.Client.Net;
using GiantScape.Client.UI;

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
        private LoadingPanel loadingPanel;
        [SerializeField]
        private ClientController client;

        public void Login(string username, string password)
        {
            StartCoroutine(LoadingCoroutine(username, password));
        }

        private void Start()
        {
            DontDestroyOnLoad(this);
            loadingPanel.Hide();
        }

        private IEnumerator LoadingCoroutine(string username, string password)
        {
            loadingPanel.Show();

            foreach (var stage in LoadingProcedure(username, password))
            {
                loadingPanel.SetState(stage.Text, stage.Stage);
                yield return null;
            }

            yield return null;
        }

        private IEnumerable<LoadingStage> LoadingProcedure(string username, string password)
        {
            yield return new LoadingStage { Text = "Connecting...", Stage = 0 };

            AsyncPromise task = client.ConnectAsync();
            while (!task.IsDone) yield return new LoadingStage { Text = "Connecting...", Stage = 0 };

            yield return new LoadingStage { Text = "Connected", Stage = 0.1f };
            yield return new LoadingStage { Text = "Logging in...", Stage = 0.1f };

            task = login.Login(username, password);
            while (!task.IsDone) yield return new LoadingStage { Text = "Logging in...", Stage = 0.1f };

            yield return new LoadingStage { Text = "Logged in", Stage = 0.2f };
            yield return new LoadingStage { Text = "Loading...", Stage = 0.5f };

            task = ChangeScene();
            while (!task.IsDone) yield return new LoadingStage { Text = "Loading...", Stage = 0.6f };

            yield return new LoadingStage { Text = "Done", Stage = 1 };
        }

        private AsyncPromise ChangeScene()
        {
            var promise = new AsyncPromise();

            SceneManager.activeSceneChanged += (s, e) => promise.IsDone = true;
            SceneManager.LoadSceneAsync("Main");

            return promise;
        }

        private struct LoadingStage
        {
            public string Text;
            public float Stage;
        }
    }
}
