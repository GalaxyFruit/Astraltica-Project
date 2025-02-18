using System.Collections;
using UnityEngine;

public class WeaponBobbing : MonoBehaviour
{
    [SerializeField] private float bobSpeed = 5f;
    [SerializeField] private float bobAmount = 0.05f;

    private Coroutine bobbingCoroutine;
    private Vector3 originalPosition;
    private Transform weapon;
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    public void StartBobbing(Transform weaponTransform)
    {
        weapon = weaponTransform;
        originalPosition = weaponTransform.localPosition;
        if (bobbingCoroutine == null) bobbingCoroutine = StartCoroutine(WeaponBobbingCoroutine());
    }

    public void StopBobbing()
    {
        if (bobbingCoroutine != null) StopCoroutine(bobbingCoroutine);
        bobbingCoroutine = null;
    }

    private IEnumerator WeaponBobbingCoroutine()
    {
        float timer = 0f;
        while (true)
        {
            float speed = playerController.GetCurrentSpeed();
            Vector3 targetPos = originalPosition;

            if (speed > 0.1f)
            {
                timer += Time.deltaTime * bobSpeed * speed;
                targetPos += new Vector3(Mathf.Sin(timer) * bobAmount, Mathf.Cos(timer * 2) * bobAmount, 0);
            }

            weapon.localPosition = Vector3.Lerp(weapon.localPosition, targetPos, Time.deltaTime * 5f);

            yield return null;
        }
    }
}
