using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SwitchScene : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            switch (SceneManager.GetActiveScene().name)
            {
                case "Start":
                    SceneTransition.SwitchToScene("Story 1");
                    break;
                case "Story 1":
                    SceneTransition.SwitchToScene("Story 2");
                    break;
                case "Story 2":
                    SceneTransition.SwitchToScene("Map 1");
                    break;
                case "End":
                    SceneTransition.SwitchToScene("Menu");
                    break;
            }
    }
}
