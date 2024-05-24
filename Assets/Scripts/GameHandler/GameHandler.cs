using System;
using Photon.Pun;
using UnityEngine;

namespace GameHandler
{
    public class GameHandler : MonoBehaviour
    {
        [SerializeField] private Player1 player1;
        [SerializeField] private Player2 player2;
        
        [SerializeField] private Camera Camera1prefab;
        [SerializeField] private Camera Camera2prefab;
        [SerializeField] private MiniGames.MiniGamesZone.MiniGamesHandler MiniGamesHandlerPrefab;
        
        private Camera camera1;
        private Camera camera2;
        private MiniGames.MiniGamesZone.MiniGamesHandler miniGamesHandler;

        private bool initialized;

        private void Awake()
        {
            InitializeGame();
            //SetViewWhenNewPlayerJoined();
        }

        private void Update()
        {
            if (initialized is false) return;
            
            if (miniGamesHandler.IsMiniGameActive)
                miniGamesHandler.UpdateMiniGame();
            else
            {
                player1.UpdateState();
                player2.UpdateState();
            }
        }

        public void InitializeGame()
        {
            camera1 = Instantiate(Camera1prefab);
            camera2 = Instantiate(Camera2prefab);
            miniGamesHandler = Instantiate(MiniGamesHandlerPrefab);

            var mainCamera = camera1.GetComponent<SplitScrin>();
            mainCamera.splitScreen = true;
            mainCamera.camera1 = camera1;
            mainCamera.camera2 = camera2;
            mainCamera.player1 = player1.transform;
            mainCamera.player2 = player2.transform;
            mainCamera.Initialize();

            initialized = true;
        }
    }
}