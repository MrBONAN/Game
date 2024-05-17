using UnityEngine;

namespace MiniGames.MiniGamesZone
{
    public class MiniGamesHandler : MonoBehaviour
    {
        [SerializeField] private MazeMiniGame.MazeObject mazePrefab;

        [SerializeField] private Camera zoneCamera;
        private IMiniGame currentMiniGame;
        [SerializeField] private MazeMiniGame.MazeObject mazeGame;

        public bool IsMiniGameActive
            => currentMiniGame is not null;

        private void Awake()
        {
            zoneCamera = GetComponent<Camera>();
            zoneCamera.enabled = false;

            zoneCamera.rect = new Rect(0.1f, 0.1f, 0.9f, 0.9f);
        }

        public void CreateMazeMiniGame()
        {
            if (currentMiniGame is not null)
                return;
            mazeGame = Instantiate(mazePrefab);
            currentMiniGame = mazeGame;
            zoneCamera.enabled = true;
        }

        public void UpdateMiniGame()
        {
            currentMiniGame.UpdateMiniGame();
        }

        public void CloseMiniGame()
        {
            currentMiniGame.OnDestroy();
            currentMiniGame = null;
        }
    }
}