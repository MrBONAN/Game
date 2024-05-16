using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace MazeMiniGame
{
    public class EdgesGenerator : MonoBehaviour
    {
        public List<EdgeObject> edges = new();
        public EdgeObject prefab;

        public EdgeObject GetNewEdge()
        {
            edges.Add(Instantiate(prefab));
            return edges.Last();
        }

        public void OnDestroy()
        {
            foreach (var edge in edges)
                Destroy(edge);
        }
    }
}