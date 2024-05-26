using System;
using System.Collections.Generic;
using MiniGames;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MazeMiniGame
{
    public class WireHandler : CurrentMiniGameHandler
    {
        [SerializeField] private WiresGameObject mazeGamePrefab;
        [SerializeField] private WiresGameObject game;

        public override void StartMiniGame()
        {
            game ??= Instantiate(mazeGamePrefab, transform);
            miniGame = game;
            game.SetGameState();
            base.StartMiniGame();
        }
    }
}