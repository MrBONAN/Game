using UnityEngine;
using UnityEngine.EventSystems;
using MiniGames;

namespace MazeMiniGame
{
    /* По заветам Окуловского, отделяю логику игры от её графической реализации
    Класс MazeObject будет графической интерпретацией обычного Maze */
    public class MazeObject : MonoBehaviour
    {
        private Maze maze;
        private Node selected;
        private void Start()
        {
            maze = new Maze(5, 5);
            selected = maze.GetNode(0, 0);
            selected.visited = true;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.UpArrow))
                maze.MoveInDirection(ref selected, MoveDirection.Up);
            else if (Input.GetKeyDown(KeyCode.DownArrow))
                maze.MoveInDirection(ref selected, MoveDirection.Down);
            else if (Input.GetKeyDown(KeyCode.LeftArrow))
                maze.MoveInDirection(ref selected, MoveDirection.Left);
            else if (Input.GetKeyDown(KeyCode.RightArrow))
                maze.MoveInDirection(ref selected, MoveDirection.Right);
        }
    }
    // public class MazeMiniGame : IMiniGame
    // {
    //     private Maze maze;
    //
    //     public MazeMiniGame(Maze maze)
    //     {
    //         this.maze = maze;
    //     }
    //     
    //     public void StartMiniGame()
    //     {
    //         
    //     }
    // }
}