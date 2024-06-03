using System;
using System.Collections.Generic;
using System.Linq;
using Interaction_objects;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

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
    Exit,
    Use2
}

public class PlayerControl : MonoBehaviour
{
    public float speed = 350f;
    public float jumpForce = 10f;
    public PlayerState state = PlayerState.grounded;
    public bool isFinished;
    protected Rigidbody2D rb;
    protected PlayerControl otherPlayer;
    protected Transform legs;
    protected Vector2 legsSize;
    protected Animator animator;
    protected AudioSource audio;
    protected HashSet<IInteractable> interactableObjects = new();
    protected Collider2D FinishCollider;
    public bool isChangingMap;

    public static readonly Dictionary<Control, KeyCode> ControlSecond = new()
    {
        { Control.Up, KeyCode.UpArrow },
        { Control.Down, KeyCode.DownArrow },
        { Control.Left, KeyCode.LeftArrow },
        { Control.Right, KeyCode.RightArrow },
        { Control.Use, KeyCode.RightShift },
        { Control.Exit, KeyCode.Slash },
        { Control.Use2, KeyCode.RightControl}
    };

    public static readonly Dictionary<Control, KeyCode> ControlFirst = new()
    {
        { Control.Up, KeyCode.W },
        { Control.Down, KeyCode.S },
        { Control.Left, KeyCode.A },
        { Control.Right, KeyCode.D },
        { Control.Use, KeyCode.E },
        { Control.Exit, KeyCode.Q },
        { Control.Use2, KeyCode.LeftAlt}
    };

    private Dictionary<Control, KeyCode> currentControl;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        otherPlayer = CompareTag("Big player")
            ? GetComponentInParent<GameHandler.GameHandler>().player1
            : GetComponentInParent<GameHandler.GameHandler>().player2;

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
        audio = GetComponent<AudioSource>();

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
        if (otherPlayer.isFinished && isFinished && FinishCollider is not null && !isChangingMap && !otherPlayer.isChangingMap)
            switch (FinishCollider.tag)
            {
                case "1to2":
                    SceneTransition.SwitchToScene("Map 2");
                    isChangingMap = true;
                    break;
                case "2to3":
                    SceneTransition.SwitchToScene("Map 3");
                    isChangingMap = true;
                    break;
                case "EndOfGame":
                    SceneTransition.SwitchToScene("End");
                    isChangingMap = true;
                    break;
            }
            
    }

    protected virtual void MovePlayer()
    {
        var direction = 0;
        if (Input.GetKey(currentControl[Control.Left]) ||
            Input.GetKey(currentControl[Control.Right]))
        {
            if (!audio.isPlaying && !animator.GetBool("isJumping")) audio.Play();
            if (animator.GetBool("isJumping")) audio.Stop();
            direction = Input.GetKey(currentControl[Control.Right]) ? 1 : -1;
            Flip(direction);
            SetAnimationRun(true);
        }
        else
        {
            audio.Stop();
            SetAnimationRun(false);
        }


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
        FinishCollider = Physics2D.OverlapBoxAll(new Vector2(transform.position.x, transform.position.y),
                new Vector2(transform.localScale.x, transform.localScale.y), 0)
            .FirstOrDefault(x => x.CompareTag("1to2") || x.CompareTag("2to3") || x.CompareTag("EndOfGame"));
        isFinished = FinishCollider is not null;
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
                interactable.Interact(this);
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