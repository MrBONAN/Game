using System;
using System.Collections.Generic;
using MiniGames;
using UnityEngine;

namespace MazeMiniGame
{
    public class MazeHandler : CurrentMiniGameHandler
    {
        // Нах## нужен этот класс? Через него в случае чего можно будет настраивать лабиринт, например, если мы захотим
        // какой-то определённый. Пишем для его создания метод и потом вызываем его. Ну а ещё это своего рода костыль
        [SerializeField] private MazeObject mazeGamePrefab;
        [SerializeField] private MazeObject game;
        [SerializeField] private int mazeType;

        public override void StartMiniGame()
        {
            game ??= Instantiate(mazeGamePrefab, transform);
            miniGame = game;
            if (mazeType == 1) CreateMaze1();
            base.StartMiniGame();
        }

        private void CreateMaze1()
        {
            var missedEdges1 = new List<((int, int), (int, int))>() { };
            var dots1 = new List<((int, int), (int, int))>() { };
            var start1 = (0, 2);
            var end1 = (8, 6);

            var missedEdges2 = new List<((int, int), (int, int))>() { };
            var dots2 = new List<((int, int), (int, int))>() { };
            var start2 = (0, 2);
            var end2 = (8, 6);

            game.SetGameState(missedEdges1, dots1, start1, end1, missedEdges2, dots2, start2, end2);
        }
    }
}