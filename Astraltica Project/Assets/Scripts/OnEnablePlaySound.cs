using UnityEngine;
using System.Collections;

public class OnEnablePlaySound : MonoBehaviour
{
    [SerializeField]
    private string soundClipName;

    [SerializeField]
    private float delay = 0f; 

    private void OnEnable()
    {
        StartCoroutine(PlaySoundWithDelay());
    }

    private IEnumerator PlaySoundWithDelay()
    {
        if (delay > 0f)
        {
            yield return new WaitForSeconds(delay);
        }
        CallMyMethod();
    }

    private void CallMyMethod()
    {
        if (!string.IsNullOrEmpty(soundClipName))
        {
            AudioManager.Instance?.PlaySound(soundClipName, transform.position);
        }
        else
        {
            Debug.LogWarning("Název zvukového klipu není nastaven!");
        }
    }
}
