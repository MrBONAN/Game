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
}