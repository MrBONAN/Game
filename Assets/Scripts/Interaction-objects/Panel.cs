using System;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction_objects
{
    public class Panel : MonoBehaviour, IInteractable
    {
        [SerializeField] private Panel other;
        private Animator animator;
        private float timeElapsed = 10f;
        public UnityEvent turnOn;
        public bool state { get; private set; }

        public void SwitchOnOff()
        {
            if (state)
                timeElapsed = 10f;
            state = !state;
            animator.SetBool("isWaiting", state);
            if (state && other.state)
                turnOn.Invoke();
        }

        public void Update()
        {
            if (state)
            {
                Debug.Log(timeElapsed);
                timeElapsed -= Time.deltaTime;
                if (timeElapsed < 0)
                    SwitchOnOff();
            }
        }

        private void Start()
        {
            animator = GetComponentInChildren<Animator>();
            state = false;
        }

        public void Interact(PlayerControl player)
        {
            if (player.gameObject.CompareTag("Big player") && gameObject.CompareTag("Big Panel") ||
                player.gameObject.CompareTag("Small player") && gameObject.CompareTag("Small Panel"))
                SwitchOnOff();
        }
    }
}