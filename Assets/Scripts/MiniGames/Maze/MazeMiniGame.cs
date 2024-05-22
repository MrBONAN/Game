using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using MiniGames;

namespace MazeMiniGame
{
    /* По заветам Окуловского, отделяю логику игры от её графической реализации
    Класс MazeObject будет графической интерпретацией обычного Maze */
    public class MazeObject : MiniGame
    {
        private Maze maze1, maze2;
        private Node selected1, selected2;
        [SerializeField] private EdgesGenerator edgesGeneratorPrefab;
        private List<EdgesGenerator> generators = new();
        private float mazeScale = 15f / 9 / 2;

        private static Dictionary<MoveDirection, KeyCode> player1Control = new()
        {
            { MoveDirection.Up, KeyCode.UpArrow },
            { MoveDirection.Down, KeyCode.DownArrow },
            { MoveDirection.Left, KeyCode.LeftArrow },
            { MoveDirection.Right, KeyCode.RightArrow }
        };

        private static Dictionary<MoveDirection, KeyCode> player2Control = new()
        {
            { MoveDirection.Up, KeyCode.W },
            { MoveDirection.Down, KeyCode.S },
            { MoveDirection.Left, KeyCode.A },
            { MoveDirection.Right, KeyCode.D }
        };

        public override void StartMiniGame()
        {
            transform.localPosition += new Vector3(0, 0, 1f);
            var distanceFromOrigin = 4.5f;
            var shift1 = new Vector3(distanceFromOrigin, 0);
            var shift2 = new Vector3(-distanceFromOrigin, 0);
            maze1 = CreateMaze(9, 8, shift1);
            maze2 = CreateMaze(9, 8, shift2);
            selected1 = maze1.GetNode(0, 0);
            selected1.visited = true;
            selected2 = maze2.GetNode(0, 0);
            selected2.visited = true;
            Debug.Log("MazeMiniGame started");
        }

        private Maze CreateMaze(int width, int height, Vector3 shift)
        {
            var generator = Instantiate(edgesGeneratorPrefab, transform);
            generator.transform.position += shift;
            generator.scale = mazeScale;
            generators.Add(generator);
            return new Maze(width, height, generator);
        }

        public override MiniGameResult UpdateMiniGame()
        {
            foreach (var (direction, keyCode) in player1Control)
                if (Input.GetKeyDown(keyCode))
                    maze1.MoveInDirection(ref selected1, direction);

            foreach (var (direction, keyCode) in player2Control)
                if (Input.GetKeyDown(keyCode))
                    maze2.MoveInDirection(ref selected2, direction);

            return MiniGameResult.ContinuePlay;
        }

        public override void OnDestroy()
        {
            Debug.Log("MazeMiniGame destroyed");
            foreach (var generator in generators)
                Destroy(generator);
            generators.Clear();
            //Destroy(gameObject);
        }
    }
}