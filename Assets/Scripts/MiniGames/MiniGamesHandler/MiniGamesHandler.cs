using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace MiniGames.MiniGamesZone
{
    public class MiniGamesHandler : MonoBehaviour
    {
        [SerializeField] private Camera zoneCamera;
        private CurrentMiniGameHandler currentMiniGameHandler;

        public bool IsMiniGameActive
            => currentMiniGameHandler is not null;

        private void Awake()
        {
            zoneCamera = GetComponentInChildren<Camera>();
            zoneCamera.enabled = false;

            zoneCamera.rect = new Rect(0.1f, 0.1f, 0.8f, 0.8f);
        }

        public void CreateMiniGame(CurrentMiniGameHandler miniGameHandler)
        {
            if (currentMiniGameHandler is not null) return;
            currentMiniGameHandler = miniGameHandler;
            currentMiniGameHandler.transform.position = zoneCamera.transform.position;
            currentMiniGameHandler.StartMiniGame();
            zoneCamera.enabled = true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateMiniGame()
        {
            if (currentMiniGameHandler is null) return;
            var result = currentMiniGameHandler.UpdateMiniGame();
            if (result is not MiniGameResult.ContinuePlay) CloseMiniGame();
        }

        public void CloseMiniGame()
        {
            if (currentMiniGameHandler is null) return;
            currentMiniGameHandler.CloseGame();
            currentMiniGameHandler = null;
            zoneCamera.enabled = false;
        }
    }
}