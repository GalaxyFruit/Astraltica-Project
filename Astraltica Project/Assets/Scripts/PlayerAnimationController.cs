using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private int animState = 0;

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
            Debug.Log("Volání změny MovementState na : " + animState);
        }
    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
        Debug.Log("Jump Triggered");
    }
}
