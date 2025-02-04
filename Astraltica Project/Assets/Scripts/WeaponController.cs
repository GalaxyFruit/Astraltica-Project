using System.Collections;
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

    [Header("Weapon Bobbing Settings")]
    [SerializeField] private float bobSpeed = 5f;
    [SerializeField] private float bobAmount = 0.05f;

    private float nextFireTime = 0f;
    private bool hasWeapon = false;
    private Coroutine bobbingCoroutine;
    private Vector3 originalPosition;
    private PlayerController playerController;
    private Transform equippedWeaponTransform;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

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
        }
    }

    public void EquipWeapon(Transform weaponTransform)
    {
        hasWeapon = true;
        equippedWeaponTransform = weaponTransform;
        originalPosition = weaponTransform.localPosition;

        if (bobbingCoroutine == null)
        {
            bobbingCoroutine = StartCoroutine(WeaponBobbing());
        }
    }

    public void UnequipWeapon()
    {
        hasWeapon = false;
        if (bobbingCoroutine != null)
        {
            StopCoroutine(bobbingCoroutine);
            bobbingCoroutine = null;
        }
        equippedWeaponTransform = null;
    }
    private IEnumerator WeaponBobbing()
    {
        float timer = 0f;
        while (hasWeapon)
        {
            float speed = playerController.GetCurrentSpeed();

            if (speed > 0.1f)
            {
                timer += Time.deltaTime * bobSpeed * speed;
                float xBob = Mathf.Sin(timer) * bobAmount;
                float yBob = Mathf.Cos(timer * 2) * bobAmount;
                equippedWeaponTransform.localPosition = originalPosition + new Vector3(xBob, yBob, 0);
            }
            else
            {
                equippedWeaponTransform.localPosition = Vector3.Lerp(equippedWeaponTransform.localPosition, originalPosition, Time.deltaTime * 5f);
            }

            yield return null;
        }
    }
}
