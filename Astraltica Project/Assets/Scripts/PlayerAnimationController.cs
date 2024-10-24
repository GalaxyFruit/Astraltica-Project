using UnityEngine;

/// <summary>
/// Class that manages player animations using a movement state int
/// </summary>
public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;

    public enum MovementState
    {
        Idle = 0,
        Walking = 1,
        Running = 2
    }

    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMovementState(MovementState state)
    {
        animator.SetInteger("MovementState", (int)state);
        Debug.Log("MovementState: " + state);

    }

    // Trigger jump
    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
        Debug.Log("Jump Triggered");
    }
}
