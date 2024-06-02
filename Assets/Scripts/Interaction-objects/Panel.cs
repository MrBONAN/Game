using System;
using MiniGames.MiniGamesZone;
using UnityEngine;
using UnityEngine.Events;

namespace Interaction_objects
{
    public class Panel : MonoBehaviour, IInteractable
    {
        [SerializeField] private Panel other;
        private Animator animator;
        private AudioSource audio;
        private float timeElapsed = 10f;
        public UnityEvent turnOn;
        private MiniGamesHandler handler;
        public bool state { get; private set; }

        public void SwitchOnOff()
        {
            if (state)
                timeElapsed = 10f;
            else
                audio.Play();
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
            audio = GetComponentInParent<AudioSource>();
            handler = GetComponentInParent<MiniGamesHandler>();
            state = false;
        }

        public void Interact(PlayerControl player)
        {
            if ((player.gameObject.CompareTag("Big player") && gameObject.CompareTag("Big Panel") ||
                player.gameObject.CompareTag("Small player") && gameObject.CompareTag("Small Panel")) &&
                !handler.HasWin && !handler.IsMiniGameActive) SwitchOnOff();
        }

        public void WinAnimation()
        {
            animator.SetBool("isWin", true);
        }
    }
}