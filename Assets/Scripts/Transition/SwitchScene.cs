using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    private AudioSource audio;

    private bool wasPressed;
    // Start is called before the first frame update
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !wasPressed)
        {
            switch (SceneManager.GetActiveScene().name)
            {
                case "Tutor 1":
                    SceneTransition.SwitchToScene("Tutor 2");
                    wasPressed = true;
                    audio.Play();
                    break;
                case "Tutor 2":
                    SceneTransition.SwitchToScene("Tutor 3");
                    wasPressed = true;
                    audio.Play();
                    break;
                case "Tutor 3":
                    SceneTransition.SwitchToScene("Tutor 4");
                    wasPressed = true;
                    audio.Play();
                    break;
                case "Tutor 4":
                    SceneTransition.SwitchToScene("Menu");
                    wasPressed = true;
                    audio.Play();
                    break;
                case "Start":
                    SceneTransition.SwitchToScene("Story 1");
                    wasPressed = true;
                    audio.Play();
                    break;
                case "Story 1":
                    SceneTransition.SwitchToScene("Story 2");
                    wasPressed = true;
                    audio.Play();
                    break;
                case "Story 2":
                    SceneTransition.SwitchToScene("Map 1");
                    wasPressed = true;
                    audio.Play();
                    break;
                case "End":
                    SceneTransition.SwitchToScene("Menu");
                    wasPressed = true;
                    audio.Play();
                    break;
            }
        }
    }
}
