using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    private Animator animator;
    private bool isGrounded = true;
    private bool isShowingWatch = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void UpdateBlendTree(float speed, float direction)
    {
        animator.SetFloat("Speed", speed); 
        animator.SetFloat("Direction", direction);
        //Debug.Log("direction: " + direction + "; Speed: " + speed);
    }

    public void OnFootstep()
    {
        if (isGrounded)
        {
            AudioManager.Instance?.PlaySound("GrassStep", transform.position);
            //Debug.Log("Footstep sound played");
        }
    }

    public void ToggleWatch()
    {
        isShowingWatch = !isShowingWatch;
        animator.SetBool("HoldWatch", isShowingWatch);

        if (isShowingWatch)
        {
            animator.SetTrigger("ShowWatch");
            Debug.Log("animator.SetTrigger(\"ShowWatch\");");
        }
        else
        {
            animator.SetTrigger("HideWatch");
            Debug.Log("animator.SetTrigger(\"HideWatch\");");
        }
    }

    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
        //Debug.Log("Jump Triggered");
    }

    public void UpdateGroundedState(bool grounded)
    {
        isGrounded = grounded;
        animator.SetBool("IsGrounded", isGrounded);
        //Debug.Log("Změna grounded na: " + grounded.ToString());
    }

    public void SetFalling()
    {
        animator.SetTrigger("Fall");
        //Debug.Log("Fall Triggered");
    }
}
