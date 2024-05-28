using MiniGames;
using UnityEngine;

namespace LampsMiniGame
{
    public class LampsHandler : CurrentMiniGameHandler
    {
        // Нах## нужен этот класс? Через него в случае чего можно будет настраивать лабиринт, например, если мы захотим
        // какой-то определённый. Пишем для его создания метод и потом вызываем его. Ну а ещё это своего рода костыль
        [SerializeField] private LampsObject lampsGamePrefab;
        [SerializeField] private LampsObject game;

        public override void StartMiniGame()
        {
            game ??= Instantiate(lampsGamePrefab, transform);
            miniGame = game;
            base.StartMiniGame();
        }
    }
}