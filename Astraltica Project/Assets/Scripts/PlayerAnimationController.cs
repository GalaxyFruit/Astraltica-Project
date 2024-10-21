using UnityEngine;

/// <summary>
/// Class that manages player animations
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator; 

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Set walking animation
    public void SetWalking(bool isWalking)
    {
        animator.SetBool("IsWalking", isWalking);
    }

    // Set running animation
    public void SetRunning(bool isRunning)
    {
        animator.SetBool("IsRunning", isRunning);
    }

    // Trigger jump animation
    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
    }
}
