using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class WeaponController : MonoBehaviour
{
    [Header("Weapon Settings")]
    [SerializeField] private Transform weaponHolder;
    [SerializeField] private float maxTiltOffset = 0.05f;

    public bool HasWeapon => hasWeapon;

    private Vector3 originalWeaponPosition;
    private Coroutine weaponAdjustCoroutine;
    private bool hasWeapon = false;
    private Transform equippedWeaponTransform;

    private WeaponShooting weaponShooting;
    private WeaponBobbing weaponBobbing;

    private void Start()
    {
        originalWeaponPosition = weaponHolder.localPosition;
        weaponShooting = GetComponent<WeaponShooting>();
        weaponBobbing = GetComponent<WeaponBobbing>();
    }

    public void SetWeaponState(bool isHolding)
    {
        if (weaponAdjustCoroutine != null) StopCoroutine(weaponAdjustCoroutine);
        if (isHolding) weaponAdjustCoroutine = StartCoroutine(AdjustWeaponTilt());
    }

    public void UpdateWeaponRotation(Transform cameraTransform)
    {
        if (equippedWeaponTransform)
            equippedWeaponTransform.rotation = cameraTransform.rotation;
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

        SetWeaponState(true);
        weaponShooting.SetWeapon(equippedWeaponTransform);
        weaponBobbing.StartBobbing(equippedWeaponTransform);
    }

    public void UnequipWeapon()
    {
        hasWeapon = false;
        SetWeaponState(false);
        weaponShooting.ClearWeapon();
        weaponBobbing.StopBobbing();
        equippedWeaponTransform = null;
    }
}
