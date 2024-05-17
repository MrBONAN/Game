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
        private Transform parent;

        private void Start()
            => parent = transform;

        public EdgeObject GetNewEdge()
        {
            edges.Add(Instantiate(prefab, parent));
            return edges.Last();
        }

        public void SetParentTransform(Transform parent)
            => this.parent = parent;

        public void OnDestroy()
        {
            foreach (var edge in edges)
                Destroy(edge.gameObject);
            Destroy(gameObject);
        }
    }
}