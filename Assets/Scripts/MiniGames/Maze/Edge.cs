using System;
using UnityEngine;

namespace MazeMiniGame
{
    public enum EdgeState
    {
        Unvisited,
        Dot,
        Missing,
    }

    public class Edge
    {
        public readonly Node From;
        public readonly Node To;
        private EdgeObject edgeObject;
        
        private EdgeState _type;
        public EdgeState Type
        {
            get => _type;
            set
            {
                _type = value;
                edgeObject.Type = value;
            }
        }

        private bool _visited;
        public bool Visited
        {
            get => _visited;
            set
            {
                _visited = value;
                edgeObject.Visited = value;
            }
        }

        public Edge(Node from, Node to, EdgeState type, EdgesGenerator edgesGenerator)
        {
            edgeObject = edgesGenerator.GetNewEdge();
            edgeObject.RealEdge = this;
            edgeObject.Pos = new Vector2Int(Math.Min(from.X, to.X), Math.Min(from.Y, to.Y));
            edgeObject.Horizontal = from.X == to.X;
            
            From = from;
            To = to;
            Type = type;
        }

        public Node GetOtherNode(Node node)
        {
            if (From != node && To != node)
                return null;
            return From == node ? To : From;
        }
    }
}