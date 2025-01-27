using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float bulletSpeed = 20f;

    private float nextFireTime = 0f;
    private bool hasWeapon = false;

    [Header("Events")]
    public UnityEvent OnShoot;

    public void OnShootAction(InputAction.CallbackContext context)
    {
        if (hasWeapon && Time.time >= nextFireTime)
        {
            Debug.Log($"OnShootAction podminka");
            Shoot();
            nextFireTime = Time.time + fireRate;
        }
    }

    private void Shoot()
    {
        if (bulletPrefab && shootPoint)
        {
            Debug.Log($"Shoot");
            GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation);
            Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();

            if (bulletRb)
            {
                bulletRb.AddForce(shootPoint.forward * bulletSpeed, ForceMode.Impulse);
            }

            OnShoot?.Invoke();
        }
    }

    public void EquipWeapon()
    {
        hasWeapon = true;
    }

    public void UnequipWeapon()
    {
        hasWeapon = false;
    }
}
