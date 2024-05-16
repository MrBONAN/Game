using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;


namespace MazeMiniGame
{
    public class Node
    {
        public List<Edge> edges = new();
        public bool visited;
        public Node prevNode;
        public int X => pos.x;
        public int Y => pos.y;
        private readonly Vector2Int pos;

        public Node(int x, int y)
        {
            pos = new Vector2Int(x, y);
        }

        public void Connect(Node other)
        {
            var edge = new Edge(this, other, EdgeState.Unvisited);
            edges.Add(edge);
            other.edges.Add(edge);
        }

        public Edge GetEdgeBetween(Node other)
            => edges.Intersect(other.edges).FirstOrDefault();

        public Node GetNeighborNode(MoveDirection direction)
        {
            var target = pos + ConvertDirectionToVector(direction);
            return edges.Select(e => e.GetOtherNode(this)).FirstOrDefault(n => n.pos == target);
        }

        public static Vector2Int ConvertDirectionToVector(MoveDirection direction)
            => direction switch
            {
                MoveDirection.Up => Vector2Int.up,
                MoveDirection.Down => Vector2Int.down,
                MoveDirection.Left => Vector2Int.left,
                MoveDirection.Right => Vector2Int.right,
                _ => throw new ArgumentOutOfRangeException(nameof(direction), direction, null)
            };
    }

}