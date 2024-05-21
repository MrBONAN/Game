using System;
using UnityEngine;

namespace Interaction_objects
{
    public class Door : MonoBehaviour
    {
        [SerializeField] private float speed = 1f;
        private float _realSpeed = 0f;
        private Vector3 start;
        private Vector3 end;
        private int IsOpened { get; set; }

        public void Open()
        {
            IsOpened = 1;
            _realSpeed = 0;
            Debug.Log("Жопа");
        }

        public void Close()
        {
            IsOpened = -1;
            _realSpeed = 0;
        }

        public void Start()
        {
            var initTransform = GetComponent<Transform>();
            start = initTransform.position;
            end = initTransform.position + new Vector3(0, initTransform.localScale.y, 0);
            Debug.Log(initTransform.localScale.y);
        }

        public void FixedUpdate()
        {
            var target = transform.position;
            if (IsOpened == 1) target = end;
            if (IsOpened == -1) target = start;
            transform.position = Vector3.MoveTowards(transform.position, target, Time.fixedDeltaTime * _realSpeed);
            if (Math.Abs((transform.position - target).sqrMagnitude) < 1e-5)
            {
                IsOpened = 0;
                _realSpeed = 0;
            }
            else _realSpeed += speed;
        }
    }
}