using UnityEngine;

public class EnemyAnimationController : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void SetAction(float actionIndex)
    {
        animator.SetFloat("ActionIndex", actionIndex);
    }

    public void Die()
    {
        animator.SetTrigger("Death");
    }
}
