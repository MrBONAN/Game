using System;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MazeMiniGame
{
    public class Maze
    {
        private Node[,] maze;
        public int Width => maze.GetLength(1);
        public int Height => maze.GetLength(0);
        private int version = 0;

        public Maze(int width, int height)
        {
            maze = new Node[height, width];
            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    maze[i, j] = new Node(j, i);
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

        public void MoveInDirection(ref Node curNode, MoveDirection direction)
        {
            var otherNode = curNode.GetNeighborNode(direction);
            if (otherNode is null ||
                otherNode.visited && otherNode != curNode.prevNode ||
                curNode.GetEdgeBetween(otherNode).Type == EdgeState.Missing)
                return;
            if (otherNode == curNode.prevNode)
            {
                curNode.visited = false;
                curNode.GetEdgeBetween(otherNode).Visited = false;
                curNode.prevNode = null;
                curNode = otherNode;
            }
            else
            {
                otherNode.visited = true;
                curNode.GetEdgeBetween(otherNode).Visited = true;
                otherNode.prevNode = curNode;
                curNode = otherNode;
            }
            WriteMaze();
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
            else if (edge.Type is EdgeState.Missing)
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

        // TODO перенести это в класс MazeObject
        // private void OnDestroy()
        // {
        //     // Я хз, можно ли удалить объект дважды, поэтому лучше в HashSet записать их
        //     var edges = new HashSet<Edge>();
        //     foreach (var node in maze)
        //         edges.AddRange(node.edges);
        //     foreach (var edge in edges)
        //         Destroy(edge);
        // }
    }
}