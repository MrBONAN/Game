using Interaction_objects;
using UnityEngine;
using UnityEngine.Events;

public class MenuButton : MonoBehaviour, IInteractable
{
    [SerializeField] public UnityEvent Event;

    private bool wasPressed;
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
        if (!wasPressed)
        {
            wasPressed = true;
            Event.Invoke();
        }
    }
}
