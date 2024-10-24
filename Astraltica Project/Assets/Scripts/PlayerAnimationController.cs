using UnityEngine;

/// <summary>
/// Class that manages player animations using boolean parameters.
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Set the animation state for idle, walking, and running using booleans
    public void SetIdle(bool isIdle)
    {
        animator.SetBool("IsIdle", isIdle);
        Debug.Log("IsIdle: " + isIdle);
    }

    public void SetWalking(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
        Debug.Log("IsWalking: " + isWalking);
    }

    public void SetRunning(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
        Debug.Log("IsRunning: " + isRunning);
    }

    // Trigger jump
    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
        Debug.Log("Jump Triggered");
    }
}
