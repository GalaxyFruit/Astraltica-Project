using System;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator animator;
    private bool isGrounded = true;

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

    public void UpdateBlendTree(float speed, float direction)
    {
        animator.SetFloat("Speed", speed); 
        animator.SetFloat("Direction", direction);
        Debug.Log("direction: " + direction + "; Speed: " + speed);
    }


    public void TriggerJump()
    {
        animator.SetTrigger("Jump");
        Debug.Log("Jump Triggered");
    }

    public void ResetToGrounded()
    {
        isGrounded = true;
        //reset to idle anim
        Debug.Log("ResetToGround called");
    }

    public void UpdateGroundedState(bool grounded)
    {
        isGrounded = grounded;
        animator.SetBool("IsGrounded", isGrounded);
        Debug.Log("Změna grounded na: " + grounded.ToString());
    }

    public void SetFalling()
    {
        animator.SetTrigger("Fall");
        Debug.Log("Fall Triggered");
    }
}
