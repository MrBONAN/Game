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

        public override void StartMiniGame()
        {
            game = new Lamps(levelsCount, levelsDifficulty);
            Debug.Log("MemorizingLampsMiniGame started");
        }

        public override MiniGameResult UpdateMiniGame()
        {
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

        public override void OnDestroy()
        {
            Debug.Log("MazeMiniGame destroyed");
            throw new NotImplementedException();
        }
    }
}