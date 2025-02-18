using UnityEngine;

public class EnemyHealth : MonoBehaviour, IDamageable
{
    [SerializeField] private float maxHealth = 100f;
    private float currentHealth;
    public bool IsDead => currentHealth <= 0;
    public event System.Action OnDeath;

    private EnemyAnimationController animationController;

    private void Start()
    {
        currentHealth = maxHealth;
        animationController = GetComponent<EnemyAnimationController>();
    }

    public void TakeDamage(float damage)
    {
        if (IsDead) return;

        currentHealth = Mathf.Max(currentHealth - damage, 0);
        Debug.Log($"Enemy took {damage} damage. Current health: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Enemy has died.");
        animationController?.PlayDeathAnimation();
        OnDeath?.Invoke();
        Destroy(gameObject, 1f); 
    }
}
