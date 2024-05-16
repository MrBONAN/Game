using UnityEngine;
using UnityEngine.EventSystems;
using MiniGames;

namespace MazeMiniGame
{
    /* По заветам Окуловского, отделяю логику игры от её графической реализации
    Класс MazeObject будет графической интерпретацией обычного Maze */
    public class MazeObject : MonoBehaviour
    {
        
    }
    public class MazeMiniGame : IMiniGame
    {
        private Maze maze;

        public MazeMiniGame(Maze maze)
        {
            this.maze = maze;
        }
        
        public void StartMiniGame()
        {
            
        }
    }
}