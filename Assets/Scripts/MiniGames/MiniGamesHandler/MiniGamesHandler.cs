using System;
using UnityEngine;
using UnityEngine.Events;
using Object = UnityEngine.Object;

namespace MiniGames.MiniGamesZone
{
    public class MiniGamesHandler : MonoBehaviour
    {
        [SerializeField] private MazeMiniGame.MazeObject mazePrefab;

        [SerializeField] private Camera zoneCamera;

        private UnityEvent<MiniGameResult> miniGameEnd;

        private IMiniGame currentMiniGame;
        private MazeMiniGame.MazeObject mazeGame;

        public bool IsMiniGameActive
            => currentMiniGame is not null;

        private void Awake()
        {
            zoneCamera = GetComponentInChildren<Camera>();
            zoneCamera.enabled = false;

            zoneCamera.rect = new Rect(0.1f, 0.1f, 0.8f, 0.8f);
        }

        public void CreateMazeMiniGame(UnityEvent<MiniGameResult> miniGameEndAction = null)
        {
            if (currentMiniGame is not null)
                return;
            miniGameEnd = miniGameEndAction;
            mazeGame = Instantiate(mazePrefab, zoneCamera.transform);
            mazeGame.StartMiniGame();
            currentMiniGame = mazeGame;
            zoneCamera.enabled = true;
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void UpdateMiniGame()
        {
            var result = currentMiniGame.UpdateMiniGame();
            if (result is MiniGameResult.ContinuePlay)
                return;
            switch (result)
            {
                case MiniGameResult.Win:
                    break;
                case MiniGameResult.Lose:
                    break;
                case MiniGameResult.Exit:
                    break;
                default:
                    Debug.Log("Неизвестный результат для миниигры");
                    throw new ArgumentOutOfRangeException();
            }
            miniGameEnd?.Invoke(result);
            CloseMiniGame();
        }

        public void CloseMiniGame()
        {
            if (currentMiniGame is null) return;
            
            miniGameEnd = null;
            Destroy(currentMiniGame as Object);
            currentMiniGame = null;
            zoneCamera.enabled = false;
        }
    }
}