using UnityEngine.Events;

namespace MiniGames
{
    public interface IMiniGame
    {
        public void StartMiniGame();
        public void UpdateMiniGame();
        public void OnDestroy();
    }
}