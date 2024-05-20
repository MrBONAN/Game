using System;
using UnityEngine;
using UnityEngine.EventSystems;
using MiniGames;

namespace MazeMiniGame
{
    /* По заветам Окуловского, отделяю логику игры от её графической реализации
    Класс MazeObject будет графической интерпретацией обычного Maze */
    public class MazeObject : MonoBehaviour, IMiniGame
    {
        private Maze maze;
        private Node selected;
        private EdgesGenerator edgesGenerator;

        public void StartMiniGame()
        {
            edgesGenerator = GetComponentInChildren<EdgesGenerator>();
            edgesGenerator.SetParentTransform(transform);
            maze = new Maze(5, 5, edgesGenerator);
            selected = maze.GetNode(0, 0);
            selected.visited = true;
        }

        public void UpdateMiniGame()
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

        public void OnDestroy()
        {
            Destroy(edgesGenerator);
            Destroy(gameObject);
        }
    }
}