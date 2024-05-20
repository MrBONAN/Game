using System;
using Unity.VisualScripting;
using UnityEngine;

namespace Interaction_objects
{
    public class Elevator : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        [SerializeField] private Transform start;
        [SerializeField] private Transform end;

        private bool _moveUp;
        public void MoveUp() => moveUp = true;
        public void MoveDown() => moveUp = false;
        public void SwitchDirection() => moveUp = !moveUp;
        public void SetMoveUp(bool moveUp) => this.moveUp = moveUp;

        public bool MovingUp
        {
            get => _moveUp;
        }

        public bool moveUp
        {
            get => _moveUp;
            private set
            {
                _moveUp = value;
                UpdateAnimation();
            }
        }

        public void Start()
        {
            transform.position = start.position;
        }

        public void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position,
                moveUp ? end.position : start.position,
                Time.fixedDeltaTime * speed);
        }

        private void UpdateAnimation()
        {
            //TODO: добавить обработку анимаций
        }
    }
}