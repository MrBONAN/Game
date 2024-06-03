using System.Collections;
using System.Collections.Generic;
using Interaction_objects;
using UnityEditor.Build.Content;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class MenuButton : MonoBehaviour, IInteractable
{
    [SerializeField] public UnityEvent Event;
    
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Interact(PlayerControl player)
    {
        Event.Invoke();
    }
}
