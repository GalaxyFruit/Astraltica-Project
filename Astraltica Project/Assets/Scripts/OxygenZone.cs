using System.Collections;
using UnityEngine;

public class OxygenZone : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && OxygenManager.Instance != null)
        {
            OxygenManager.Instance.EnterOxygenZone();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && OxygenManager.Instance != null)
        {
            OxygenManager.Instance.ExitOxygenZone();
        }
    }
}
