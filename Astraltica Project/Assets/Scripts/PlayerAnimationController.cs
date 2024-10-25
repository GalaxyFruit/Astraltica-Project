using UnityEngine;

/// <summary>
/// Class that manages player animations using an enum for movement states.
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    private int animState = 0;

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Set movement state in the animator using an enum
    public void SetMovementState(int state)
    {
        if(animState != state)
        {
            animState = state;
            animator.SetInteger("MovementState", animState);
            Debug.Log("MovementState: " + animState);
        }
    }

    // Trigger jump animation
    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
        Debug.Log("Jump Triggered");
    }
}
