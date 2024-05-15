using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace MiniGames
{
    public class Node
    {
        public List<Edge> edges;
        public bool visited;
        public Node prevNode;
        public int X => pos.x;
        public int Y => pos.y;
        private readonly Vector2Int pos;

        public Node(int x, int y)
        {
            pos = new Vector2Int(x, y);
        }

        public void CreateEdge(Node other)
        {
            var edge = new Edge(this, other, EdgeState.Exist);
            edges.Add(edge);
            other.edges.Add(edge);
        }

        public Edge GetEdgeBetween(Node other)
            => edges.Intersect(other.edges).FirstOrDefault();

        public Node GetDirectionNode(MoveDirection direction)
        {
            Vector2Int target = default;
            if (direction == MoveDirection.Up)
                target += Vector2Int.down;
            else if (direction == MoveDirection.Down)
                target += Vector2Int.up;
            else if (direction == MoveDirection.Left)
                target += Vector2Int.left;
            else if (direction == MoveDirection.Right)
                target += Vector2Int.right;
            return edges.Select(e => e.GetOtherNode(this)).FirstOrDefault(n => n.pos == target);
        }
    }

    public enum EdgeState
    {
        Exist,
        Missing,
        Dot
    }

    public class Edge
    {
        public Node from, to;
        public EdgeState state;
        public bool visited;

        public Edge(Node from, Node to, EdgeState state)
        {
            this.from = from;
            this.to = to;
            this.state = state;
        }

        public Node GetOtherNode(Node node)
        {
            if (from != node && to != node)
                return null;
            return from == node ? to : from;
        }
    }

    public class Maze
    {
        public Node[,] maze;
        public int X => maze.GetLength(1);
        public int Y => maze.GetLength(0);

        public Maze(int y, int x)
        {
            maze = new Node[y, x];
            for (var i = 0; i < y; i++)
            {
                for (var j = 0; j < x; j++)
                {
                    maze[i, j] = new Node(i, j);
                }
            }

            for (var i = 0; i < y; i++)
            {
                for (var j = 0; j < x; j++)
                {
                    if (j + 1 < x)
                        maze[i, j].CreateEdge(maze[i, j + 1]);
                    if (i + 1 < y)
                        maze[i, j].CreateEdge(maze[i + 1, j]);
                }
            }
        }

        private void MoveInDirection(ref Node curNode, MoveDirection direction)
        {
            var otherNode = curNode.GetDirectionNode(direction);
            if (otherNode is null)
                return;
            if (curNode.GetEdgeBetween(otherNode).state == EdgeState.Missing)
                return;
            if (otherNode == curNode.prevNode)
            {
                curNode.visited = false;
                curNode.GetEdgeBetween(otherNode).visited = false;
                curNode.prevNode = null;
                curNode = otherNode;
            }
            else
            {
                otherNode.visited = true;
                curNode.GetEdgeBetween(otherNode).visited = true;
                otherNode.prevNode = curNode;
                curNode = otherNode;
            }
        }
    }

    public class MazeMiniGame : MonoBehaviour, IMiniGame
    {
        private Maze maze;
        
        public void StartMiniGame()
        {
            maze = new Maze(5, 5);
        }

        public void FixedUpdate()
        {
            
        }
    }
}