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
        [SerializeField] private EdgesGenerator edgesGeneratorPrefab;
        private List<EdgesGenerator> generators = new();

        [SerializeField] private GameObject StartEndNodePrefab;
        private GameObject StartNode1, EndNode1;
        private GameObject StartNode2, EndNode2;
        private Node selected1, selected2;
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

        public MazeState mazeState1 = new() { Start = (0, 0), End = (-1, -1), Missed = new(), Dots = new() };
        public MazeState mazeState2 = new() { Start = (0, 0), End = (-1, -1), Missed = new(), Dots = new() };

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

            EdgesGenerator anchor1, anchor2;
            (maze1, anchor1) = CreateMaze(Width, Height, shift1);
            (maze2, anchor2) = CreateMaze(Width, Height, shift2);
            selected1 = maze1.GetNode(mazeState1.Start.X, mazeState1.Start.Y);
            selected1.visited = true;
            selected2 = maze2.GetNode(mazeState2.Start.X, mazeState2.Start.Y);
            selected2.visited = true;

            (StartNode1, EndNode1) = GetStartEndObjects(mazeState1.Start, mazeState1.End, anchor1);
            (StartNode2, EndNode2) = GetStartEndObjects(mazeState2.Start, mazeState2.End, anchor2);

            maze1.SetMazeState(mazeState1);
            maze2.SetMazeState(mazeState2);

            Debug.Log("MazeMiniGame started");
        }

        private (GameObject, GameObject) GetStartEndObjects((int X, int Y) startNode, (int X, int Y) endNode,
            EdgesGenerator anchor)
        {
            var start = Instantiate(StartEndNodePrefab, anchor.transform);
            var end = Instantiate(StartEndNodePrefab, anchor.transform);
            start.transform.localScale = start.transform.transform.localScale / Edge.edgeSize * MazeScale;
            start.transform.localPosition =
                (Vector3)(new Vector2(startNode.X, startNode.Y) + anchor.shift) * anchor.scale +
                new Vector3(0, 0, -0.1f);
            end.transform.localScale = end.transform.localScale / Edge.edgeSize * MazeScale;
            end.transform.localPosition =
                (Vector3)(new Vector2(endNode.X, endNode.Y) + anchor.shift) * anchor.scale +
                new Vector3(0, 0, -0.1f);

            return (start, end);
        }

        private (Maze, EdgesGenerator) CreateMaze(int width, int height, Vector3 shift)
        {
            var generator = Instantiate(edgesGeneratorPrefab, transform);
            generator.transform.position += shift;
            generator.scale = MazeScale;
            generators.Add(generator);
            return (new Maze(width, height, generator), generator);
        }

        public override MiniGameResult UpdateMiniGame()
        {
            if (CheckExit() is MiniGameResult.Exit) return MiniGameResult.Exit;
            foreach (var (direction, keyCode) in PlayerControl.ControlSecond)
                if (Input.GetKeyDown(keyCode) && 
                    direction is Control.Up or Control.Down or Control.Left or Control.Right)
                    maze1.MoveInDirection(ref selected1, direction);

            foreach (var (direction, keyCode) in PlayerControl.ControlFirst)
                if (Input.GetKeyDown(keyCode) && 
                    direction is Control.Up or Control.Down or Control.Left or Control.Right)
                    maze2.MoveInDirection(ref selected2, direction);

            if (maze1.DotsLeft == 0 && selected1.X == mazeState1.End.X && selected1.Y == mazeState1.End.Y &&
                maze2.DotsLeft == 0 && selected2.X == mazeState2.End.X && selected2.Y == mazeState2.End.Y)
                return MiniGameResult.Win;

            return MiniGameResult.ContinuePlay;
        }
        
        private MiniGameResult CheckExit()
        {
            if (Input.GetKey(PlayerControl.ControlFirst[Control.Exit]) &&
                Input.GetKey(PlayerControl.ControlSecond[Control.Exit]))
                return MiniGameResult.Exit;
            return MiniGameResult.ContinuePlay;
        }

        public override void OnDestroy()
        {
            Debug.Log("MazeMiniGame destroyed");
            foreach (var generator in generators)
                Destroy(generator);
            Destroy(StartNode1);
            Destroy(StartNode2);
            Destroy(EndNode1);
            Destroy(EndNode2);
            generators.Clear();
            //Destroy(gameObject);
        }
    }
}