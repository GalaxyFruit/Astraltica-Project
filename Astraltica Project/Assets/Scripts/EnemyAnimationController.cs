using System.Collections;
using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    [SerializeField] private float attack1EventTime = 0.7f;
    [SerializeField] private float attack2EventTime = 0.5f;

    [SerializeField] private int attack1Damage = 50; // Left punch
    [SerializeField] private int attack2Damage = 35; // Right punch

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

        int attackType = Random.Range(1, 3); // 1 = left, 2 = right
        animator.SetFloat("AttackType", attackType);
        Debug.Log($"[EnemyAnimationController] Spouštím animaci útoku typu: {attackType}");

        float animSpeed = animator.GetFloat(attackType == 1 ? "Attack1Speed" : "Attack2Speed");
        float eventTime = attackType == 1 ? attack1EventTime : attack2EventTime;
        float delay = eventTime / (animSpeed > 0 ? animSpeed : 1f);

        int attackDamage = attackType == 1 ? attack1Damage : attack2Damage;
        if (damageCoroutine != null)
            StopCoroutine(damageCoroutine);
        damageCoroutine = StartCoroutine(ApplyDamageCoroutine(delay, attackDamage));
    }

    public void StopAttackAnimation()
    {
        isAttacking = false;
        animator.SetBool("IsAttacking", false);
    }

    // Removed ApplyDamageAfterCurrentAttack()

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
