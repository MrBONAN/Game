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
        public bool isMiniGameExist;

        public virtual void StartMiniGame()
        {
            isMiniGameExist = true;
            miniGame.StartMiniGame();
        }

        public MiniGameResult UpdateMiniGame()
        {
            var result = miniGame.UpdateMiniGame();
            if (result is not MiniGameResult.ContinuePlay)
                CloseGame();
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
            return result;
        }

        public void OnDestroy()
        {
            CloseGame();
            Destroy(miniGame);
            Destroy(gameObject);
        }

        public void CloseGame()
        {
            isMiniGameExist = false;
            miniGame?.OnDestroy();
        }
    }
}