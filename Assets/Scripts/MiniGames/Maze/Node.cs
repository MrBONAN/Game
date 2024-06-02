using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


namespace MazeMiniGame
{
    public class Node
    {
        public List<Edge> edges = new();
        public bool visited;
        public Node prevNode;
        private readonly EdgesGenerator edgesGenerator;
        public int X => pos.x;
        public int Y => pos.y;
        private readonly Vector2Int pos;

        public Node(int x, int y, EdgesGenerator edgesGenerator)
        {
            pos = new Vector2Int(x, y);
            this.edgesGenerator = edgesGenerator;
        }

        public void Connect(Node other)
        {
            var edge = new Edge(this, other, EdgeState.Unvisited, edgesGenerator);
            edges.Add(edge);
            other.edges.Add(edge);
        }

        public Edge GetEdgeBetween(Node other)
            => edges.Intersect(other.edges).FirstOrDefault();

        public Node GetNeighborNode(Control direction)
        {
            var target = pos + ConvertDirectionToVector(direction);
            return edges.Select(e => e.GetOtherNode(this)).FirstOrDefault(n => n.pos == target);
        }

        public static Vector2Int ConvertDirectionToVector(Control direction)
            => direction switch
            {
                Control.Up => Vector2Int.up,
                Control.Down => Vector2Int.down,
                Control.Left => Vector2Int.left,
                Control.Right => Vector2Int.right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
    }

}