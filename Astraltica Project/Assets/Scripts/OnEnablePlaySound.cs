using UnityEngine;

public class OnEnablePlaySound : MonoBehaviour
{
    [SerializeField]
    private string soundClipName;

    private void OnEnable()
    {
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
