using UnityEngine;

public interface IDamageable
{
    public void TakeDamage(float damage);
    public bool IsDead { get; }
}

