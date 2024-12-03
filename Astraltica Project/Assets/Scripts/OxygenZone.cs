using System.Collections;
using UnityEngine;

public class OxygenZone : MonoBehaviour
{
    [SerializeField] private float regenRate = 1f;
    [SerializeField] private float regenRateOverTime = 1f; 

    private Coroutine regenerateCoroutine;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && OxygenManager.Instance != null)
        {
            OxygenManager.Instance.EnterOxygenZone();
            if (regenerateCoroutine == null)
            {
                regenerateCoroutine = StartCoroutine(RegenerateOxygen());
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && OxygenManager.Instance != null)
        {
            OxygenManager.Instance.ExitOxygenZone();
            if (regenerateCoroutine != null)
            {
                StopCoroutine(regenerateCoroutine);
                regenerateCoroutine = null;
            }
        }
    }

    private IEnumerator RegenerateOxygen()
    {
        while (OxygenManager.Instance.CurrentOxygen < OxygenManager.Instance.MaxOxygen)
        {
            OxygenManager.Instance.ReplenishOxygen(regenRate);
            yield return new WaitForSeconds(regenRateOverTime);
        }
        regenerateCoroutine = null;
    }
}
