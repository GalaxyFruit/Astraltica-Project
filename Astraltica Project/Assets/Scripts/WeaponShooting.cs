using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponShooting : MonoBehaviour
{
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float bulletSpeed = 20f;

    private float nextFireTime = 0f;
    private Transform shootPoint;

    public void SetWeapon(Transform weapon)
    {
        shootPoint = weapon.Find("ShootPoint");
    }

    public void ClearWeapon()
    {
        shootPoint = null;
    }

    public void OnShootAction(InputAction.CallbackContext context)
    {
        if (shootPoint == null || Time.time < nextFireTime) return;
        Shoot();
        nextFireTime = Time.time + fireRate;
    }

    private void Shoot()
    {
        if (!bulletPrefab || !shootPoint) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, shootPoint.rotation * Quaternion.Euler(0, 90, 0));
        if (bullet.TryGetComponent(out Rigidbody bulletRb))
            bulletRb.AddForce(shootPoint.forward * bulletSpeed, ForceMode.Impulse);
    }
}
