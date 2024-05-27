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
        public float scale;
        public Vector2 shift = Vector2.zero;

        private void Awake()
            => parent = transform;

        public EdgeObject GetNewEdge()
        {
            var edge = Instantiate(prefab, parent);
            edge.transform.localScale = new Vector3(scale, scale);
            edges.Add(edge);
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