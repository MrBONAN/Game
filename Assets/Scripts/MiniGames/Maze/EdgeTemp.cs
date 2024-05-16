using System;
using UnityEngine;

namespace MazeMiniGame
{
    public class EdgeTemp : MonoBehaviour
    {
        public Node From { get; private set; }
        public Node To { get; private set; }
        private EdgeState _type;
        public EdgeState Type
        {
            get => _type;
            set
            {
                _type = value;
                UpdateTexture();
            }
        }
        private bool _visited;
        public bool Visited
        {
            get => _visited;
            set
            {
                _visited = value;
                animator.SetBool("visited", _visited);
            }
        }
        [SerializeField] private Animator animator;

        public void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void SetEdge(Node from, Node to, EdgeState type)
        {
            From = from;
            To = to;
            Type = type;
            // TODO: добавить установку координат в transpose по координатам нод
        }

        public Node GetOtherNode(Node node)
        {
            if (From != node && To != node)
                return null;
            return From == node ? To : From;
        }

        private void UpdateTexture()
        {
            if (Visited)
                return;
            animator.SetInteger("type", (int)Type);
        }
    }
}