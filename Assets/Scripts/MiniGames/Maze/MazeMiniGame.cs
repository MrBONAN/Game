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
        private float MazeScale => 15f / Width / 2;
        private int _width = 9, _height = 8;

        public int Width
        {
            get => _width;
            set
            {
                if (value > 0) _width = value;
            }
        }

        public int Height
        {
            get => _height;
            set
            {
                if (value > 0) _height = value;
            }
        }

        public MazeState mazeState1 = new() { Start = (0, 0), End = (-1, -1), Missed = new (), Dots = new()};
        public MazeState mazeState2 = new() { Start = (0, 0), End = (-1, -1), Missed = new (), Dots = new()};

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

        public void SetGameState(
            List<((int X, int Y), MoveDirection)> missed1,
            List<((int X, int Y), MoveDirection)> dots1,
            (int X, int Y) start1, (int X, int Y) end1,
            List<((int X, int Y), MoveDirection)> missed2,
            List<((int X, int Y), MoveDirection)> dots2,
            (int X, int Y) start2, (int X, int Y) end2)
        {
            mazeState1 = new MazeState { Start = start1, End = end1, Missed = missed1, Dots = dots1 };
            mazeState2 = new MazeState { Start = start2, End = end2, Missed = missed2, Dots = dots2 };
        }

        public override void StartMiniGame()
        {
            var distanceFromOrigin = 4.5f;
            var shift1 = new Vector3(distanceFromOrigin, 0);
            var shift2 = new Vector3(-distanceFromOrigin, 0);
            
            maze1 = CreateMaze(Width, Height, shift1);
            maze2 = CreateMaze(Width, Height, shift2);
            selected1 = maze1.GetNode(mazeState1.Start.X, mazeState1.Start.Y);
            selected1.visited = true;
            selected2 = maze2.GetNode(mazeState2.Start.X, mazeState2.Start.Y);
            selected2.visited = true;
            
            maze1.SetMazeState(mazeState1);
            maze2.SetMazeState(mazeState2);
            
            Debug.Log("MazeMiniGame started");
        }

        private Maze CreateMaze(int width, int height, Vector3 shift)
        {
            var generator = Instantiate(edgesGeneratorPrefab, transform);
            generator.transform.position += shift;
            generator.scale = MazeScale;
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

            if (maze1.DotsLeft == 0 && selected1.X == mazeState1.End.X && selected1.Y == mazeState1.End.Y &&
                maze2.DotsLeft == 0 && selected2.X == mazeState2.End.X && selected2.Y == mazeState2.End.Y)
                return MiniGameResult.Win;

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