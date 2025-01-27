using UnityEngine;

public class BulletController : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f; 
    [SerializeField] private float damage = 25f;  

    private void Start()
    {
        Destroy(gameObject, lifetime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<IDamageable>(out var damageable))
        {
            damageable.TakeDamage(damage);
            Debug.Log("Damage dealt!");
        }

        //Destroy(gameObject);
    }
}
