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
        private Animator animator;
        private AudioSource audio;
        private bool _moveUp;

        public void MoveUp()
        {
            moveUp = true;
            audio.Play();
        }
        public void MoveDown()
        {
            moveUp = false;
            audio.Play();
        }

        public void SwitchDirection() => moveUp = !moveUp;
        public void SetMoveUp(bool moveUp) => this.moveUp = moveUp;

        public bool MovingUp
        {
            get => _moveUp;
        }

        public bool moveUp
        {
            get => _moveUp;
            private set => _moveUp = value;
        }

        public void Start()
        {
            transform.position = start.position;
            animator = GetComponentInChildren<Animator>();
            audio = GetComponentInParent<AudioSource>();
        }

        public void FixedUpdate()
        {
            transform.position = Vector3.MoveTowards(transform.position,
                moveUp ? end.position : start.position,
                Time.fixedDeltaTime * speed);
            
            animator.SetBool("IsWorking",
                !(Math.Abs((transform.position - (moveUp ? end.position : start.position)).sqrMagnitude) < 1e-5));
            if (!animator.GetBool("IsWorking")) audio.Stop();
        }

        private void UpdateAnimation()
        {
            throw new NotImplementedException();
        }
    }
}