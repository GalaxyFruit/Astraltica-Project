using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private float lastAttackDuration = 1.1f; 

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetMoveSpeed(float speed)
    {
        Debug.Log($"movement enemy je: {speed}");
        animator.SetFloat("Speed", speed);
    }

    public void PlayAttackAnimation()
    {
        animator.SetBool("IsAttacking", true);
        int attackType = Random.Range(1, 3); // 1 = Left Punch, 2 = Right Punch
        animator.SetFloat("AttackType", attackType);

        lastAttackDuration = attackType == 1 ? 2.7f : 1.1f;
    }

    public float GetCurrentAttackDuration()
    {
        return lastAttackDuration;
    }

    private void StopAttack()
    {
        animator.SetBool("IsAttacking", false);
    }

    public void PlayDeathAnimation()
    {
        animator.SetTrigger("Death");
    }
}
