using System;
using UnityEngine;
using UnityEngine.Events;
using Object = System.Object;

namespace MiniGames
{
    public abstract class CurrentMiniGameHandler : MonoBehaviour
    {
        [SerializeField] protected UnityEvent winAction;
        [SerializeField] protected UnityEvent loseAction;
        [SerializeField] protected UnityEvent exitAction;
        [SerializeField] protected UnityEvent continueAction;
        protected MiniGame miniGame;

        public virtual void StartMiniGame()
        {
            miniGame.StartMiniGame();
        }

        public MiniGameResult UpdateMiniGame()
        {
            var result = miniGame.UpdateMiniGame();
            switch (result)
            {
                case MiniGameResult.Win:
                    winAction.Invoke();
                    break;
                case MiniGameResult.Lose:
                    loseAction.Invoke();
                    break;
                case MiniGameResult.Exit:
                    exitAction.Invoke();
                    break;
                case MiniGameResult.ContinuePlay:
                    continueAction.Invoke();
                    break;
                default:
                    Debug.Log("Неизвестный результат для миниигры");
                    throw new ArgumentOutOfRangeException();
            }

            if (result is not MiniGameResult.ContinuePlay)
                CloseGame();
            return result;
        }

        public void OnDestroy()
        {
            CloseGame();
            Destroy(miniGame);
            Destroy(gameObject);
        }

        public void CloseGame()
            => miniGame.OnDestroy();
    }
}