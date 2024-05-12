using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public int turnOn;
    private Animator animator;
    public UnityEvent onPressed;
    public UnityEvent onStayPressed;
    public UnityEvent onReleased;
    private string[] whoCanInteract = { "Big player" };

    private void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    private void FixedUpdate()
    {
        animator.SetBool("Pressed", turnOn != 0);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (CheckIfCanInteract(other))
        {
            turnOn += 1;
            onPressed.Invoke();
            Debug.Log("Button pressed");
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (CheckIfCanInteract(other))
            onStayPressed.Invoke();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (CheckIfCanInteract(other))
        {
            turnOn -= 1;
            onReleased.Invoke();
        }
    }

    private bool CheckIfCanInteract(Collider2D other)
        => whoCanInteract.Contains(other.gameObject.tag);
}