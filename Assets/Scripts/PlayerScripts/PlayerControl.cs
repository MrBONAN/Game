using System;
using System.Collections.Generic;
using Interaction_objects;
using UnityEngine;

public enum PlayerState
{
    grounded,
    jumped,
}

public class PlayerControl : MonoBehaviour
{
    public float speed = 350f;
    public float jumpForce = 10f;
    public PlayerState state = PlayerState.grounded;
    protected Rigidbody2D rb;
    protected Transform legs;
    protected Animator animator;
    protected HashSet<IInteractable> interactableObjects = new();

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
        Debug.Log(legs?.name);
        animator = GetComponent<Animator>();
    }

    public void UpdateState()
    {
        CheckCollisions();
        MovePlayer();
        UpdateTexture();
    }

    protected void Update()
    {
        CheckControl();
    }

    protected virtual void MovePlayer()
    {
        throw new NotImplementedException();
    }

    protected virtual void UpdateTexture()
    {
        throw new NotImplementedException();
    }

    protected void Flip(int direction)
    {
        transform.localScale =
            new Vector3(direction * Math.Abs(transform.localScale.x),
                transform.localScale.y,
                transform.localScale.z);
    }

    protected virtual void CheckCollisions()
    {
        throw new NotImplementedException();
    }
    
    protected virtual void CheckControl()
    {
        throw new NotImplementedException();
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