using System;
using System.Collections.Generic;
using MiniGames;
using UnityEngine;
using UnityEngine.EventSystems;

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
            var missedEdges1 = new List<((int, int), MoveDirection)>()
            {
                ((0, 1), MoveDirection.Right),
                ((1, 0), MoveDirection.Right),
                ((1, 5), MoveDirection.Right),
                ((1, 7), MoveDirection.Right),
                ((2, 1), MoveDirection.Right),
                ((3, 3), MoveDirection.Right),
                ((3, 4), MoveDirection.Right),
                ((3, 7), MoveDirection.Right),
                ((4, 1), MoveDirection.Right),
                ((4, 2), MoveDirection.Right),
                ((4, 4), MoveDirection.Right),
                ((4, 5), MoveDirection.Right),
                ((5, 0), MoveDirection.Right),
                ((5, 3), MoveDirection.Right),
                ((6, 2), MoveDirection.Right),
                ((6, 4), MoveDirection.Right),
                ((6, 6), MoveDirection.Right),
                ((6, 7), MoveDirection.Right),
                
                ((1, 2), MoveDirection.Up),
                ((1, 3), MoveDirection.Up),
                ((1, 5), MoveDirection.Up),
                ((2, 1), MoveDirection.Up),
                ((2, 4), MoveDirection.Up),
                ((3, 4), MoveDirection.Up),
                ((4, 2), MoveDirection.Up),
                ((5, 3), MoveDirection.Up),
                ((6, 2), MoveDirection.Up),
                ((6, 5), MoveDirection.Up),
                ((7, 3), MoveDirection.Up),
            };
            var dots1 = new List<((int, int), MoveDirection)>()
            {
                ((1, 4), MoveDirection.Right),
                ((2, 0), MoveDirection.Right),
                ((3, 2), MoveDirection.Right),
                ((5, 7), MoveDirection.Right),
                ((6, 1), MoveDirection.Right),
                
                ((2, 5), MoveDirection.Up),
                ((4, 4), MoveDirection.Up),
                ((8, 6), MoveDirection.Up),
            };
            var start1 = (0, 2);
            var end1 = (8, 6);

            var missedEdges2 = new List<((int, int), MoveDirection)>()
            {
                ((1, 1), MoveDirection.Right),
                ((1, 2), MoveDirection.Right),
                ((1, 4), MoveDirection.Right),
                ((2, 0), MoveDirection.Right),
                ((2, 1), MoveDirection.Right),
                ((2, 3), MoveDirection.Right),
                ((2, 6), MoveDirection.Right),
                ((3, 2), MoveDirection.Right),
                ((3, 3), MoveDirection.Right),
                ((3, 5), MoveDirection.Right),
                ((4, 1), MoveDirection.Right),
                ((4, 4), MoveDirection.Right),
                ((4, 5), MoveDirection.Right),
                ((4, 7), MoveDirection.Right),
                ((5, 3), MoveDirection.Right),
                ((5, 6), MoveDirection.Right),
                ((6, 0), MoveDirection.Right),

                ((0, 4), MoveDirection.Up),
                ((1, 1), MoveDirection.Up),
                ((1, 4), MoveDirection.Up),
                ((1, 5), MoveDirection.Up),
                ((2, 3), MoveDirection.Up),
                ((2, 5), MoveDirection.Up),
                ((3, 1), MoveDirection.Up),
                ((3, 5), MoveDirection.Up),
                ((4, 0), MoveDirection.Up),
                ((4, 2), MoveDirection.Up),
                ((4, 4), MoveDirection.Up),
                ((5, 1), MoveDirection.Up),
                ((5, 2), MoveDirection.Up),
                ((5, 3), MoveDirection.Up),
                ((5, 5), MoveDirection.Up),
                ((6, 1), MoveDirection.Up),
                ((6, 2), MoveDirection.Up),
                ((6, 4), MoveDirection.Up),
                ((7, 1), MoveDirection.Up),
                ((7, 3), MoveDirection.Up),
                ((7, 6), MoveDirection.Up),
                ((8, 0), MoveDirection.Up),
                ((8, 1), MoveDirection.Up),

            };
            var dots2 = new List<((int, int), MoveDirection)>() { };
            var start2 = (0, 3);
            var end2 = (8, 0);

            game.SetGameState(missedEdges1, dots1, start1, end1, missedEdges2, dots2, start2, end2);
        }
    }
}