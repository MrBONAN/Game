using System;
using UnityEngine;

namespace GameHandler
{
    public class GameHandler : MonoBehaviour
    {
        [SerializeField] public PlayerControl player1;
        [SerializeField] public PlayerControl player2;
        
        [SerializeField] private Camera Camera1prefab;
        [SerializeField] private Camera Camera2prefab;
        [SerializeField] private float orthographicSize = 20f;
        
        private Camera camera1;
        private Camera camera2;
        [SerializeField] private MiniGames.MiniGamesZone.MiniGamesHandler miniGamesHandler;

        private bool initialized;

        private void Awake()
        {
            InitializeGame();
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

            var mainCamera = camera1.GetComponent<SplitScrin>();
            mainCamera.splitScreen = true;
            mainCamera.camera1 = camera1;
            mainCamera.camera2 = camera2;
            mainCamera.player1 = player1.transform;
            mainCamera.player2 = player2.transform;
            mainCamera.orthographicSize = orthographicSize;
            mainCamera.Initialize();

            initialized = true;
        }
    }
}