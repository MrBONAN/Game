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
        [SerializeField] private GameObject SelectorPrefab;
        private GameObject firstPlayerButtons;
        private GameObject secondPlayerButtons;
        private Button[,] firstPlayer = new Button[2, 4];
        private Button[,] secondPlayer = new Button[2, 4];
        private GameObject select1, select2;
        private float gameScale = 2f;
        private float buttonScale = 1.5f;

        public override void StartMiniGame()
        {
            game = new Lamps(levelsCount, levelsDifficulty);
            CreateButtons();
            select1 = Instantiate(SelectorPrefab, firstPlayerButtons.transform);
            select2 = Instantiate(SelectorPrefab, secondPlayerButtons.transform);
            select1.transform.localScale = new Vector3(buttonScale, buttonScale);
            select2.transform.localScale = new Vector3(buttonScale, buttonScale);
            UpdateSelectorsPosition();
            Debug.Log("MemorizingLampsMiniGame started");
        }

        public override MiniGameResult UpdateMiniGame()
        {
            foreach (var (direction, keyCode) in PlayerControl.ControlFirst)
                if (Input.GetKeyDown(keyCode) &&
                    direction is Control.Up or Control.Down or Control.Left or Control.Right)
                {
                    game.MoveCursor(ref game.cursor1, direction);
                    UpdateSelectorsPosition();
                }

            foreach (var (direction, keyCode) in PlayerControl.ControlSecond)
                if (Input.GetKeyDown(keyCode) &&
                    direction is Control.Up or Control.Down or Control.Left or Control.Right)
                {
                    game.MoveCursor(ref game.cursor2, direction);
                    UpdateSelectorsPosition();
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
            var shift = new Vector3(4.3f, 0);
            var centerShift = new Vector3(1.5f, 0.75f);
            firstPlayerButtons = Instantiate(go, transform);
            firstPlayerButtons.transform.localScale = new Vector3(gameScale, gameScale);
            firstPlayerButtons.transform.position = transform.position - shift;
            for (var i = 0; i < 8; i++)
            {
                var b = Instantiate(ButtonPrefab, firstPlayerButtons.transform);
                b.SetButton((ButtonContent)((int)ButtonContent.One + i), ButtonContent.Default);
                b.Position = new Vector3(i % 4, -i / 4) - centerShift;
                b.transform.localScale = new Vector3(buttonScale, buttonScale);
                firstPlayer[i / 4, i % 4] = b;
            }
            
            secondPlayerButtons = Instantiate(go, transform);
            secondPlayerButtons.transform.localScale = new Vector3(gameScale, gameScale);
            secondPlayerButtons.transform.position = transform.position + shift;
            for (var i = 0; i < 8; i++)
            {
                var b = Instantiate(ButtonPrefab, secondPlayerButtons.transform);
                b.ButtonBackground = ButtonContent.Red + i;
                b.SetButtonTextColor(ButtonContent.Red + i);
                b.Position = new Vector3(i % 4, -i / 4) - centerShift;
                b.transform.localScale = new Vector3(buttonScale, buttonScale);
                secondPlayer[i / 4, i % 4] = b;
            }
            
            Destroy(go);
        }

        private void UpdateSelectorsPosition()
        {
            var (cursor1, cursor2) = (game.cursor1, game.cursor2);
            select1.transform.localPosition = firstPlayer[cursor1.Y, cursor1.X].Position + new Vector3(0, 0, 0.1f);
            select2.transform.localPosition = secondPlayer[cursor2.Y, cursor2.X].Position + new Vector3(0, 0, 0.1f);
        }

        public override void OnDestroy()
        {
            foreach (var button in firstPlayer)
                Destroy(button);
            foreach (var button in secondPlayer)
                Destroy(button);
            Destroy(firstPlayerButtons);
            Destroy(secondPlayerButtons);
            Destroy(select1);
            Destroy(select2);
            
            Debug.Log("MemorizingLampsMiniGame destroyed");
        }
    }
}