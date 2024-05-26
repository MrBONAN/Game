using System;
using System.Collections.Generic;
using System.Linq;
using Interaction_objects;
using UnityEngine;
using UnityEngine.EventSystems;

public enum PlayerState
{
    grounded,
    jumped,
}

public enum Control
{
    Up,
    Down,
    Left,
    Right,
    Use,
    Exit
}

public class PlayerControl : MonoBehaviour
{
    public float speed = 350f;
    public float jumpForce = 10f;
    public PlayerState state = PlayerState.grounded;
    protected Rigidbody2D rb;
    protected Transform legs;
    protected Vector2 legsSize;
    protected Animator animator;
    protected HashSet<IInteractable> interactableObjects = new();

    public static readonly Dictionary<Control, KeyCode> ControlSecond = new()
    {
        { Control.Up, KeyCode.UpArrow },
        { Control.Down, KeyCode.DownArrow },
        { Control.Left, KeyCode.LeftArrow },
        { Control.Right, KeyCode.RightArrow },
        { Control.Use, KeyCode.RightShift },
        { Control.Exit, KeyCode.Slash }
    };

    public static readonly Dictionary<Control, KeyCode> ControlFirst = new()
    {
        { Control.Up, KeyCode.W },
        { Control.Down, KeyCode.S },
        { Control.Left, KeyCode.A },
        { Control.Right, KeyCode.D },
        { Control.Use, KeyCode.E },
        { Control.Exit, KeyCode.Q }
    };

    private Dictionary<Control, KeyCode> currentControl;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        foreach (var component in GetComponentsInChildren<Transform>())
        {
            if (component.name is "Legs")
            {
                legs = component;
                break;
            }
        }

        legsSize = transform.GetComponent<BoxCollider2D>().size * transform.lossyScale.x;
        legsSize = new Vector2(legsSize.x * 0.9f, 0.2f);
        animator = GetComponent<Animator>();

        currentControl = gameObject.CompareTag("Small player") ? ControlFirst : ControlSecond;
    }

    public void UpdateState()
    {
        CheckCollisions();
        MovePlayer();
    }

    protected void Update()
    {
        //rb.velocity = new Vector2(0, rb.velocity.y);
        CheckControl();
    }

    protected virtual void MovePlayer()
    {
        var direction = 0;
        if (Input.GetKey(currentControl[Control.Left]) ||
            Input.GetKey(currentControl[Control.Right]))
        {
            direction = Input.GetKey(currentControl[Control.Right]) ? 1 : -1;
            Flip(direction);
            SetAnimationRun(true);
        }
        else
            SetAnimationRun(false);

        var velocity = new Vector2(direction * speed * Time.fixedDeltaTime, rb.velocity.y);
        if (state == PlayerState.grounded && Input.GetKey(currentControl[Control.Up]))
        {
            velocity.y = jumpForce;
            state = PlayerState.jumped;
            SetAnimationJump(true);
        }

        rb.velocity = transform.TransformDirection(velocity);
    }

    protected void Flip(int direction)
    {
        transform.localScale =
            new Vector3(direction * Math.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
    }

    private void CheckCollisions()
    {
        var colliders = Physics2D.OverlapBoxAll(legs.position, legsSize, 0);
        if (colliders.Any(c => c.gameObject.CompareTag("Ground")))
        {
            state = PlayerState.grounded;
            SetAnimationJump(false);
        }
        else
        {
            state = PlayerState.jumped;
            SetAnimationJump(true);
        }
    }

    // ReSharper disable Unity.PerformanceAnalysis
    protected virtual void CheckControl()
    {
        if (Input.GetKeyDown(currentControl[Control.Use]))
            foreach (var interactable in interactableObjects)
                interactable.Interact();
    }

    protected void OnTriggerEnter2D(Collider2D other)
    {
        var interactable = other.gameObject.GetComponent<IInteractable>();
        if (interactable is null) return;
        interactableObjects.Add(interactable);
    }

    protected void OnTriggerExit2D(Collider2D other)
    {
        var interactable = other.gameObject.GetComponent<IInteractable>();
        if (interactable is null) return;
        interactableObjects.Remove(interactable);
    }

    protected void SetAnimationRun(bool on)
    {
        animator.SetBool("isRunning", on);
    }

    protected void SetAnimationJump(bool on)
    {
        animator.SetBool("isJumping", on);
    }
}