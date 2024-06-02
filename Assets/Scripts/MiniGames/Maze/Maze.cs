using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MazeMiniGame
{
    public record MazeState
    {
        public (int X, int Y) Start { get; set; }
        public (int X, int Y) End  { get; set; }
        public List<((int X, int Y), MoveDirection)> Missed  { get; set; }
        public List<((int X, int Y), MoveDirection)> Dots  { get; set; }
    }
    
    public class Maze
    {
        private Node[,] maze;
        public int Width => maze.GetLength(1);
        public int Height => maze.GetLength(0);
        private int version = 0;
        public int DotsLeft { get; private set; }

        public Maze(int width, int height, EdgesGenerator edgesGenerator)
        {
            maze = new Node[height, width];
            edgesGenerator.shift = new Vector3(-(width - 1) / 2f, -(height - 1) / 2f);
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    maze[i, j] = new Node(j, i, edgesGenerator);
                }
            }

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    if (j + 1 < width)
                        maze[i, j].Connect(maze[i, j + 1]);
                    if (i + 1 < height)
                        maze[i, j].Connect(maze[i + 1, j]);
                }
            }
        }

        // ReSharper disable Unity.PerformanceAnalysis
        public void MoveInDirection(ref Node curNode, Control direction)
        {
            var otherNode = curNode.GetNeighborNode(direction);
            if (otherNode is not null)
                
            if (otherNode is null ||
                otherNode.visited && otherNode != curNode.prevNode ||
                curNode.GetEdgeBetween(otherNode).Type == EdgeState.Missed)
                return;
            if (otherNode == curNode.prevNode)
            {
                var edge = curNode.GetEdgeBetween(otherNode);
                edge.Visited = false;
                if (edge.Type == EdgeState.Dot) DotsLeft++;
                
                curNode.visited = false;
                curNode.prevNode = null;
                curNode = otherNode;
            }
            else
            {
                var edge = curNode.GetEdgeBetween(otherNode);
                edge.Visited = true;
                if (edge.Type == EdgeState.Dot) DotsLeft--;
                
                otherNode.visited = true;
                otherNode.prevNode = curNode;
                curNode = otherNode;
            }

            //WriteMaze();
        }

        public void SetMazeState(MazeState state)
        {
            foreach (var e in state.Missed) SetEdgeType(e.Item1, e.Item2, EdgeState.Missed);
            foreach (var e in state.Dots) SetEdgeType(e.Item1, e.Item2, EdgeState.Dot);
            DotsLeft = state.Dots.Count;
        }

        public override string ToString()
        {
            var m = new StringBuilder(new string('#', (Width * 2 - 1) * (Height * 2 - 1) + Height * 2 - 1));
            var len = Width * 2;
            for (var y = 0; y < Height; y++)
            {
                for (var x = 0; x < Width; x++)
                {
                    if (x < Width - 1)
                    {
                        var n1 = maze[y, x];
                        var n2 = maze[y, x + 1];
                        var edge = n1.GetEdgeBetween(n2);
                        var edgeCor = y * 2 * len + 2 * x + 1;
                        SetEdgeInBuilder(m, edge, edgeCor);
                        if (y < Height - 1)
                            m[edgeCor + len] = ' ';
                    }

                    if (y < Height - 1)
                    {
                        var n1 = maze[y, x];
                        var n2 = maze[y + 1, x];
                        var edge = n1.GetEdgeBetween(n2);
                        var edgeCor = (y * 2 + 1) * len + 2 * x;
                        SetEdgeInBuilder(m, edge, edgeCor);
                    }
                }

                var endCor = y * 2 * len + len - 1;
                m[endCor] = '\n';
                if (y < Height - 1)
                    m[endCor + len] = '\n';
            }

            return m.ToString() + ++version;
        }

        private void SetEdgeInBuilder(StringBuilder m, Edge edge, int edgeCor)
        {
            if (edge.Visited)
                m[edgeCor] = '+';
            else if (edge.Type is EdgeState.Dot)
                m[edgeCor] = '.';
            else if (edge.Type is EdgeState.Missed)
                m[edgeCor] = ' ';
            else
                m[edgeCor] = '-';
        }

        private void WriteMaze()
        {
            var filePath = @"C:\Users\Admin\Desktop\maze.txt";

            if (!File.Exists(filePath))
            {
                Debug.Log("Created");
                File.Create(filePath);
            }

            try
            {
                File.WriteAllText(filePath, ToString());
            }
            catch (Exception ex)
            {
                Debug.Log("Ошибка при попытке перезаписи файла: " + ex.Message);
            }
        }

        public Node GetNode(int x, int y)
            => maze[y, x];

        public void SetEdgeType(Node n1, Node n2, EdgeState type)
            => n1.GetEdgeBetween(n2).Type = type;

        public void SetEdgeType(Vector2Int n1, Vector2Int n2, EdgeState type)
            => maze[n1.y, n1.x].GetEdgeBetween(maze[n2.y, n2.x]).Type = type;
        
        public void SetEdgeType((int X, int Y) n1, (int X, int Y) n2, EdgeState type)
            => maze[n1.Y, n1.X].GetEdgeBetween(maze[n2.Y, n2.X]).Type = type;
        
        public void SetEdgeType((int X, int Y) n1, MoveDirection direction, EdgeState type)
        {
            var dx  = direction is MoveDirection.Left or MoveDirection.Right ? direction is MoveDirection.Right? 1 : -1 : 0;
            var dy  = direction is MoveDirection.Up or MoveDirection.Down ? direction is MoveDirection.Up? 1 : -1 : 0;
            maze[n1.Y, n1.X].GetEdgeBetween(maze[n1.Y + dy, n1.X + dx]).Type = type;
        }
    }
}