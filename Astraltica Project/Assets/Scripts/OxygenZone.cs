using System;
using System.Collections;
using UnityEngine;

public class OxygenZone : MonoBehaviour
{
    [SerializeField] private float regenRate = 1f;
    [SerializeField] private float regenRepeatTime = 0.1f;

    private bool isPlayerInside = false;
    private bool isRegenerating = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && OxygenManager.Instance != null)
        {
            isPlayerInside = true;
            isRegenerating = true;
            OxygenManager.Instance.EnterOxygenZone();
            StartCoroutine(RegenerateOxygen());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && OxygenManager.Instance != null)
        {
            isPlayerInside = false;
            OxygenManager.Instance.ExitOxygenZone();
            StopCoroutine(RegenerateOxygen());
        }
    }

    private IEnumerator RegenerateOxygen()
    {
        while (isPlayerInside & isRegenerating)
        {
            Debug.Log("Player is in OxygenZone!");
            OxygenManager.Instance.ReplenishOxygen(regenRate);
            IsOxygenFull();
            yield return new WaitForSeconds(regenRepeatTime);
        }
    }

    private void IsOxygenFull()
    {
        if (OxygenManager.Instance.CurrentOxygen == OxygenManager.Instance.MaxOxygen)
        {
            isRegenerating = false;
            StopCoroutine(RegenerateOxygen());
            Debug.Log("courotine stopped!");
        }

    }
}
