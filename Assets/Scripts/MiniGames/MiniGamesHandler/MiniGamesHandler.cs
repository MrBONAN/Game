using UnityEngine;

namespace MiniGames.MiniGamesZone
{
    public class MiniGamesHandler : MonoBehaviour
    {
        [SerializeField] private MazeMiniGame.MazeObject mazePrefab;

        [SerializeField] private Camera zoneCamera;
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

        public void CreateMazeMiniGame()
        {
            if (currentMiniGame is not null)
                return;
            mazeGame = Instantiate(mazePrefab, zoneCamera.transform);
            mazeGame.StartMiniGame();
            currentMiniGame = mazeGame;
            zoneCamera.enabled = true;
        }

        public void UpdateMiniGame()
        {
            currentMiniGame.UpdateMiniGame();
        }

        public void CloseMiniGame()
        {
            Destroy(currentMiniGame as Object);
            currentMiniGame = null;
            zoneCamera.enabled = false;
        }
    }
}