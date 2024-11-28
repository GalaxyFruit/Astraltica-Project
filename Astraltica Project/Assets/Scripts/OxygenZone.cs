using System;
using System.Collections;
using UnityEngine;

public class OxygenZone : MonoBehaviour
{
    [SerializeField] private float regenRate = 1f;

    private bool isPlayerInside = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && OxygenManager.Instance != null)
        {
            isPlayerInside = true;
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
        if (isPlayerInside)
        {
            Debug.Log("Player is in OxygenZone!");
            OxygenManager.Instance.ReplenishOxygen(regenRate);
            IsOxygenFull();
            yield return new WaitForSeconds(1f);
        }
    }

    private void IsOxygenFull()
    {
        if(OxygenManager.Instance.CurrentOxygen == OxygenManager.Instance.MaxOxygen)
        {
            StopCoroutine(RegenerateOxygen());
            //Debug.Log("courotine stopped!");
        }

    }
}
