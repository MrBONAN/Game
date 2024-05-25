using UnityEngine;

public partial class Player2 : PlayerControl
{
    public float isGroundRad = 0.2f;

    protected override void MovePlayer()
    {
        var direction = 0;
        if (Input.GetKey(ControlSecond[Control.Left]) || Input.GetKey(ControlSecond[Control.Right]))
        {
            direction = Input.GetKey(ControlSecond[Control.Right]) ? 1 : -1;
            Flip(direction);
            SetAnimationRun(true);
        }
        else
            SetAnimationRun(false);

        var velocity = new Vector2(direction * speed * Time.fixedDeltaTime, rb.velocity.y);
        if (state == PlayerState.grounded && Input.GetKey(ControlSecond[Control.Up]))
        {
            velocity.y = jumpForce;
            state = PlayerState.jumped;
            SetAnimationJump(true);
        }

        rb.velocity = transform.TransformDirection(velocity);
    }

    protected override void CheckCollisions()
    {
        var colliders = Physics2D.OverlapCircleAll(legs.position, isGroundRad);
        foreach (var c in colliders)
        {
            if (c.gameObject.CompareTag("Ground"))
            {
                state = PlayerState.grounded;
                SetAnimationJump(false);
                break;
            }
        }
    }
    
    // ReSharper disable Unity.PerformanceAnalysis
    protected override void CheckControl()
    {
        if (Input.GetKeyDown(ControlSecond[Control.Use]))
            foreach (var interactable in interactableObjects)
                interactable.Interact();
    }
}