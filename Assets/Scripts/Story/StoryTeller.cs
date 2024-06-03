using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Story
{
    public class StoryTeller : MonoBehaviour
    {
        [SerializeField] private List<Sprite> story = new();
        [SerializeField] private Vector3 positionOutOfBound = new Vector3(0, -10f);
        private SpriteRenderer spriteRenderer;
        private Camera camera;
        private AudioSource _audioSource;
        public float delay = 1f;

        private void Awake()
        {
            camera = GetComponent<Camera>();
            _audioSource = GetComponent<AudioSource>();
            camera.enabled = false;
            camera.rect = new Rect(0.05f, 0, 0.9f, 0.22f);
            camera.orthographicSize = 1.12f;
            spriteRenderer = GetComponentInChildren<SpriteRenderer>();
            
            StartCoroutine(Dialog(story));
        }

        public void PlayOneDialog(Sprite dialog)
        {
            StartCoroutine(Dialog(new () {dialog}));
        }

        private IEnumerator Dialog(List<Sprite> dialogStory)
        {
            yield return new WaitForSeconds(delay);
            camera.enabled = true;
            foreach (var dialog in dialogStory)
            {
                _audioSource.Play();
                spriteRenderer.sprite = dialog;
                while (!Input.GetKeyDown(KeyCode.Space))
                    yield return null;
                while (!Input.GetKeyUp(KeyCode.Space))
                    yield return null;
            }
            camera.enabled = false;
        }
    }
}