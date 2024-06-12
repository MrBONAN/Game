using System;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D;

namespace GameHandler
{
    public class Arrow : MonoBehaviour
    {
        private Transform _transform;
        private SpriteRenderer _sp;
        [SerializeField] private SpriteAtlas _spriteAtlas;

        private const float DirEps = 17;
        [SerializeField] private float _direction;
        [SerializeField] private float _prevDirection;

        public float Direction
        {
            get => _direction;
            set
            {
                _direction = (value + 360) % 360;
                UpdateArraowDirection();
            }
        }

        private Vector3 _position;

        public Vector3 Position
        {
            get => _position;
            set
            {
                _position = value;
                transform.position = value;
            }
        }

        private void Start()
        {
            _transform = GetComponent<Transform>();
            _sp = GetComponent<SpriteRenderer>();
            UpdateArraowDirection();
        }

        private void UpdateArraowDirection()
        {
            if (GetDiffAngle(Direction, _prevDirection) < DirEps)
                return;
            var newDirection = Enumerable.Range(0, 12).OrderBy(i => GetDiffAngle(i * 30, Direction)).First() * 30;
            _prevDirection = newDirection;
            _sp.sprite = _spriteAtlas.GetSprite(newDirection.ToString());
        }

        private float GetDiffAngle(float a, float b)
            => new[]
            {
                Math.Abs(a - b),
                Math.Abs(a - 360 - b),
                Math.Abs(a + 360 - b)
            }.Min();
    }
}