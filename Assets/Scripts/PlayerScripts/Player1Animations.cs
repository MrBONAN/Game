using UnityEngine;

public partial class Player1
{
    protected override void UpdateTexture()
    {
    }

    private void SetAnimationRun(bool on)
    {
        animator.SetBool("isRunning", on);
    }

    private void SetAnimationJump(bool on)
    {
        animator.SetBool("isJumping", on);
    }

    protected override void Flip(int direction)
    {
        transform.localScale = new Vector3(direction, 1, 1);
    }
}