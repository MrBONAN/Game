using UnityEngine.Events;

namespace MiniGames
{
    public enum MiniGameResult
    {
        Win,
        Lose,
        Exit,
        ContinuePlay
    }
    public interface IMiniGame
    {
        public void StartMiniGame();
        public MiniGameResult UpdateMiniGame();
        public void OnDestroy();
    }
}