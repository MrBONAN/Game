using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    private AudioSource audio;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Tutor 1":
                    SceneTransition.SwitchToScene("Tutor 2");
                    audio.Play();
                    break;
                case "Tutor 2":
                    SceneTransition.SwitchToScene("Tutor 3");
                    audio.Play();
                    break;
                case "Tutor 3":
                    SceneTransition.SwitchToScene("Tutor 4");
                    audio.Play();
                    break;
                case "Tutor 4":
                    SceneTransition.SwitchToScene("Menu");
                    audio.Play();
                    break;
                case "Start":
                    SceneTransition.SwitchToScene("Story 1");
                    audio.Play();
                    break;
                case "Story 1":
                    SceneTransition.SwitchToScene("Story 2");
                    audio.Play();
                    break;
                case "Story 2":
                    SceneTransition.SwitchToScene("Map 1");
                    audio.Play();
                    break;
                case "End":
                    SceneTransition.SwitchToScene("Menu");
                    audio.Play();
                    break;
            }
        }
    }
}
