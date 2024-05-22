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
        public override void StartMiniGame()
        {
            game ??= Instantiate(mazeGamePrefab, transform);
            miniGame = game;
            base.StartMiniGame();
        }
    }
}