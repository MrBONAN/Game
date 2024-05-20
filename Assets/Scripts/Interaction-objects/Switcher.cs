using UnityEngine;
using UnityEngine.Events;

namespace Interaction_objects
{
    public class Switcher : MonoBehaviour, IInteractable
    {
        private Animator animator;
        public UnityEvent turnOn;
        public UnityEvent turnOff;
        public bool state { get; private set; }

        public void SwitchOnOff()
        {
            state = !state;
            Debug.Log($"Switch state: {state}");
            animator.SetBool("turn-on", state);
            if (state)
                turnOn.Invoke();
            else
                turnOff.Invoke();
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        public void Interact()
        {
            SwitchOnOff();
        }
    }
}