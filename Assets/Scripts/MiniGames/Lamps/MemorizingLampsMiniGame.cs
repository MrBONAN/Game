using System;
using UnityEngine;
using MiniGames;

namespace LampsMiniGame
{
    /* Класс LampsObject будет графической интерпретацией обычного Lamps */
    public class LampsObject : MiniGame
    {
        private Lamps game;
        [SerializeField] private int levelsCount = 5;
        [SerializeField] private int levelsDifficulty = 1;
        
        [SerializeField] private Button ButtonPrefab;
        private GameObject firstPlayerButtons;
        private GameObject secondPlayerButtons;
        private Button[,] firstPlayer = new Button[2, 4];
        private Button[,] secondPlayer = new Button[2, 4];

        public override void StartMiniGame()
        {
            game = new Lamps(levelsCount, levelsDifficulty);
            CreateButtons();
            Debug.Log("MemorizingLampsMiniGame started");
        }

        public override MiniGameResult UpdateMiniGame()
        {
            //return MiniGameResult.ContinuePlay;
            foreach (var (direction, keyCode) in PlayerControl.ControlFirst)
                if (Input.GetKeyDown(keyCode) &&
                    direction is Control.Up or Control.Down or Control.Left or Control.Right)
                {
                    game.MoveCursor(ref game.cursor1, direction);
                }

            foreach (var (direction, keyCode) in PlayerControl.ControlFirst)
                if (Input.GetKeyDown(keyCode) &&
                    direction is Control.Up or Control.Down or Control.Left or Control.Right)
                {
                    game.MoveCursor(ref game.cursor2, direction);
                }

            var result = CheckPlayerAction(first: true);
            if (result != MiniGameResult.ContinuePlay) return result;
            result = CheckPlayerAction(first: false);
            return result;
        }

        private MiniGameResult CheckPlayerAction(bool first)
        {
            if (!Input.GetKeyDown(first
                    ? PlayerControl.ControlFirst[Control.Use]
                    : PlayerControl.ControlSecond[Control.Use]))
                return MiniGameResult.ContinuePlay;
            return game.PlayerClicked(first) switch
            {
                Result.Success => SuccessAction(),
                Result.Failure => FailureAction(),
                _ => MiniGameResult.ContinuePlay
            };
        }

        private MiniGameResult SuccessAction()
        {
            game.CurrentLevel++;
            if (game.CurrentLevel == levelsCount)
            {
                OnDestroy();
                return MiniGameResult.Win;
            }

            return MiniGameResult.ContinuePlay;
        }

        private MiniGameResult FailureAction()
        {
            game.Restart();
            return MiniGameResult.ContinuePlay;
        }

        private void CreateButtons()
        {
            var go = new GameObject();
            firstPlayerButtons = Instantiate(go, transform);
            var shift = new Vector3(5, 0);
            firstPlayerButtons.transform.position = transform.position - shift;
            for (var i = 0; i < 8; i++)
            {
                var b = Instantiate(ButtonPrefab, firstPlayerButtons.transform);
                b.SetButton((ButtonContent)((int)ButtonContent.One + i), ButtonContent.Default);
                b.Position = new Vector3(i % 4, i / 4);
                firstPlayer[i / 4, i % 4] = b;
            }
            
            secondPlayerButtons = Instantiate(go, transform);
            secondPlayerButtons.transform.position = transform.position + shift;
            for (var i = 0; i < 8; i++)
            {
                var b = Instantiate(ButtonPrefab, secondPlayerButtons.transform);
                b.ButtonBackground = ButtonContent.Red + i;
                b.SetButtonTextColor(ButtonContent.Red + i);
                b.Position = new Vector3(i % 4, i / 4);
                secondPlayer[i / 4, i % 4] = b;
            }
            
            Destroy(go);
        }

        public override void OnDestroy()
        {
            foreach (var button in firstPlayer)
                Destroy(button);
            foreach (var button in secondPlayer)
                Destroy(button);
            Destroy(firstPlayerButtons);
            Destroy(secondPlayerButtons);
            
            Debug.Log("MemorizingLampsMiniGame destroyed");
        }
    }
}