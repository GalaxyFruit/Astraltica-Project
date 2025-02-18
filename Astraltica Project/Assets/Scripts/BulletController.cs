using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f; 
    [SerializeField] private float damage = 25f;

    private bool isDestroyed = false;

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (isDestroyed) return;

        if (collision.collider.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
            Debug.Log("Damage dealt!");
        }

        DestroyBullet();
    }

    private void DestroyBullet()
    {
        isDestroyed = true;
        Destroy(gameObject);
    }
}
