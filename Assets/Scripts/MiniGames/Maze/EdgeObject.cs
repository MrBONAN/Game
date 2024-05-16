using UnityEngine;
using UnityEngine.UIElements;

namespace MazeMiniGame
{
    public class EdgeObject : MonoBehaviour
    {
        private static float scale = 0.76f;
        public Edge RealEdge { get; set; }
        private Vector2 _pos;

        public Vector2 Pos
        {
            get => _pos;
            set
            {
                _pos = new Vector2(value.x, value.y) * scale;
                transform.localPosition = _pos;
            }
        }

        private bool _horizontal;

        public bool Horizontal
        {
            get => _horizontal;
            set
            {
                _horizontal = value;
                transform.Rotate(0, 0, value? 0 : -90);
            }
        }

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

        private Animator animator;

        public void Awake()
        {
            animator = GetComponentInChildren<Animator>();
        }

        public void UpdateTexture()
        {
            if (Visited)
                return;
            animator.SetInteger("type", (int)Type);
        }
    }
}