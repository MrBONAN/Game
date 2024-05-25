using UnityEngine;
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
    public abstract class MiniGame : MonoBehaviour
    {
        public abstract void StartMiniGame();
        public abstract MiniGameResult UpdateMiniGame();
        public abstract void OnDestroy();
    }
}