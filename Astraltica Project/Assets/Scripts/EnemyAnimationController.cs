using System.Collections;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;
    private float lastAttackDuration = 1.1f; 
    private bool isAttacking = false;
    private Coroutine damageCoroutine;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogError("Animator component chybí na této komponentě!.");
        }
    }

    public void SetMoveSpeed(float speed)
    {
        animator.SetFloat("Speed", speed);
    }

    public void PlayAttackAnimation()
    {
        if (!isAttacking)
        {
            isAttacking = true;
            animator.SetBool("IsAttacking", true);
        }

        int attackType = Random.Range(1, 3); // Vybereme typ útoku (1 nebo 2)

        animator.SetFloat("AttackType", attackType);
        lastAttackDuration = attackType == 1 ? 3.8f : 1.1f;
    }

    public void StopAttackAnimation()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    public void ApplyDamageAfterCurrentAttack()
    {
        float damageDelay = lastAttackDuration - 0.3f;
        int attackDamage = lastAttackDuration > 3f ? 50 : 35;
        damageCoroutine = StartCoroutine(ApplyDamageCoroutine(damageDelay, attackDamage));
    }

    public void StopDamageCoroutine()
    {
        if (damageCoroutine != null)
        {
            StopCoroutine(damageCoroutine);
            damageCoroutine = null;
            Debug.Log("Poškozovací Coroutine byl zastaven.");
        }
    }

    private IEnumerator ApplyDamageCoroutine(float delay, int damage)
    {
        yield return new WaitForSeconds(delay);

        if (OxygenManager.Instance is IDamageable damageable)
        {
            damageable.TakeDamage(damage);
        }
        else
        {
            Debug.LogWarning("OxygenManager neimplementuje IDamageable nebo není dostupný.");
        }
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
