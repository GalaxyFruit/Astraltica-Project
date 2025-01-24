using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private float lastAttackDuration = 1.1f; 
    private bool isAttacking = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMoveSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void PlayAttackAnimation()
    {
        Debug.Log("volam PlayAttackAnimation()");

        if(!isAttacking)
        {
            isAttacking = true;
            animator.SetBool("IsAttacking", true);
        }

        int attackType = Random.Range(1, 3); // Vybereme typ útoku (1 nebo 2)
        Debug.Log($"Attack type: {attackType}");
        animator.SetFloat("AttackType", attackType);

        lastAttackDuration = attackType == 1 ? 3.8f : 1.1f;
        Debug.Log($"lastAttackDuration: {lastAttackDuration}");
    }

    public void StopAttackAnimation()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
        Debug.Log($"IsAttacking is changed to: {isAttacking}");
    }

    public bool IsAttacking()
    {
        return isAttacking;
    }

    public float GetCurrentAttackDuration()
    {
        return lastAttackDuration; 
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Death"); 
    }
}
