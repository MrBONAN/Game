using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace MiniGames.MiniGamesZone
{
    public class MiniGamesHandler : MonoBehaviour
    {
        [SerializeField] private Camera zoneCamera;
        private CurrentMiniGameHandler currentMiniGameHandler;
        private Queue<CurrentMiniGameHandler> miniGamesQueue = new();

        public bool IsMiniGameActive
            => currentMiniGameHandler is not null && currentMiniGameHandler.isMiniGameExist;

        private void Awake()
        {
            zoneCamera = GetComponentInChildren<Camera>();
            zoneCamera.enabled = false;

            zoneCamera.rect = new Rect(0.1f, 0.1f, 0.8f, 0.8f);
        }

        public void CreateMiniGame(CurrentMiniGameHandler miniGameHandler)
        {
            if (currentMiniGameHandler is not null && currentMiniGameHandler.isMiniGameExist) return;
            if (currentMiniGameHandler is not null)
            {
                miniGamesQueue.Enqueue(miniGameHandler);
                return;
            }
            currentMiniGameHandler = miniGameHandler;
            currentMiniGameHandler.transform.position = zoneCamera.transform.position + new Vector3(0, 0, 1);
            currentMiniGameHandler.StartMiniGame();
            zoneCamera.enabled = true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateMiniGame()
        {
            if (currentMiniGameHandler is null || !currentMiniGameHandler.isMiniGameExist) return;
            var result = currentMiniGameHandler.UpdateMiniGame();
            if (result is not MiniGameResult.ContinuePlay) CloseMiniGame();
        }

        public void CloseMiniGame()
        {
            if (currentMiniGameHandler is null) return;
            currentMiniGameHandler.CloseGame();
            currentMiniGameHandler = null;
            if (miniGamesQueue.Count == 0)
            {
                zoneCamera.enabled = false;
                return;
            }
            CreateMiniGame(miniGamesQueue.Dequeue());
        }
    }
}