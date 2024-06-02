using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction_objects
{
    public class Switcher : MonoBehaviour, IInteractable
    {
        private Animator animator;
        public UnityEvent turnOn;
        public UnityEvent turnOff;
        private AudioSource[] audio;
        public bool state { get; private set; }

        public void SwitchOnOff()
        {
            state = !state;
            Debug.Log($"Switch state: {state}");
            animator.SetBool("turn-on", state);
            if (state)
            {
                turnOn.Invoke();
                audio.First(x => x.name == "TurnOnSound").Play();
            }
            else
            {
                turnOff.Invoke();
                audio.First(x => x.name == "TurnOffSound").Play();
            }
        }

        private void Start()
        {
            animator = GetComponent<Animator>();
            audio = GetComponentsInChildren<AudioSource>();
        }

        public void Interact(PlayerControl player)
        {
            SwitchOnOff();
        }
    }
}