using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private int animState = 0; // 0 = Idle, 1 = Walking, 2 = Sprinting, 3 = Jumping, 4 = Falling
    private bool isGrounded = true; // New variable to track if the player is grounded

    public static PlayerAnimationController Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        animator = GetComponent<Animator>();
    }

    public void SetMovementState(int state)
    {
        if (animState != state)
        {
            animState = state;
            animator.SetInteger("MovementState", animState);
            Debug.Log("MovementState changed to: " + animState);
        }
    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
        Debug.Log("Jump Triggered");
    }

    public void ResetToGrounded()
    {
        isGrounded = true; 
        SetMovementState(0); 
    }

    public void UpdateGroundedState(bool grounded)
    {
        isGrounded = grounded;
        animator.SetBool("IsGrounded", isGrounded); 
    }
}
