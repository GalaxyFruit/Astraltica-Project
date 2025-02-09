using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private float maxTiltOffset = 0.05f;

    [Header("Weapon Shooting Settings")]
    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private Transform shootPoint;
    [SerializeField] private float fireRate = 0.5f;
    [SerializeField] private float bulletSpeed = 20f;

    [Header("Weapon Bobbing Settings")]
    [SerializeField] private float bobSpeed = 5f;
    [SerializeField] private float bobAmount = 0.05f;

    public bool HasWeapon => hasWeapon;

    private float nextFireTime = 0f;
    private bool hasWeapon = false;
    private Vector3 originalPosition;
    private PlayerController playerController;
    private Transform equippedWeaponTransform;
    private bool isHoldingWeapon = false;
    private Coroutine weaponAdjustCoroutine, bobbingCoroutine;
    private Vector3 originalWeaponPosition;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
        originalWeaponPosition = weaponHolder.localPosition;
    }

    public void OnShootAction(InputAction.CallbackContext context)
    {
        if (!hasWeapon || Time.time < nextFireTime) return;

        Shoot();
        nextFireTime = Time.time + fireRate;
    }

    private void Shoot()
    {
        if (!bulletPrefab || !shootPoint) return;

        GameObject bullet = Instantiate(bulletPrefab, shootPoint.position, Camera.main.transform.rotation);
        if (bullet.TryGetComponent(out Rigidbody bulletRb))
            bulletRb.AddForce(shootPoint.forward * bulletSpeed, ForceMode.Impulse);
    }

    public void UpdateWeaponRotation(Transform cameraTransform)
    {
        if (equippedWeaponTransform)
            equippedWeaponTransform.rotation = cameraTransform.rotation;
    }

    public void SetWeaponState(bool isHolding)
    {
        isHoldingWeapon = isHolding;

        if (weaponAdjustCoroutine != null) StopCoroutine(weaponAdjustCoroutine);
        if (isHoldingWeapon) weaponAdjustCoroutine = StartCoroutine(AdjustWeaponTilt());
    }

    private IEnumerator AdjustWeaponTilt()
    {
        while (true)
        {
            float tilt = Camera.main.transform.eulerAngles.x;
            if (tilt > 180) tilt -= 360;

            float yOffset = Mathf.Clamp(-tilt * 0.005f, -maxTiltOffset, maxTiltOffset);

            weaponHolder.localPosition = originalWeaponPosition + new Vector3(0, yOffset, 0);

            yield return null;
        }
    }



    public void EquipWeapon(Transform weaponTransform)
    {
        hasWeapon = true;
        equippedWeaponTransform = weaponTransform;
        originalPosition = weaponTransform.localPosition;

        SetWeaponState(true);
        if (bobbingCoroutine == null) bobbingCoroutine = StartCoroutine(WeaponBobbing());
    }

    public void UnequipWeapon()
    {
        hasWeapon = false;
        SetWeaponState(false);

        if (bobbingCoroutine != null) StopCoroutine(bobbingCoroutine);
        bobbingCoroutine = null;
        equippedWeaponTransform = null;
    }

    private IEnumerator WeaponBobbing()
    {
        float timer = 0f;
        while (hasWeapon)
        {
            float speed = playerController.GetCurrentSpeed();
            Vector3 targetPos = originalPosition;

            if (speed > 0.1f)
            {
                timer += Time.deltaTime * bobSpeed * speed;
                targetPos += new Vector3(Mathf.Sin(timer) * bobAmount, Mathf.Cos(timer * 2) * bobAmount, 0);
            }

            equippedWeaponTransform.localPosition = Vector3.Lerp(
                equippedWeaponTransform.localPosition,
                targetPos,
                Time.deltaTime * 5f
            );

            yield return null;
        }
    }
}
