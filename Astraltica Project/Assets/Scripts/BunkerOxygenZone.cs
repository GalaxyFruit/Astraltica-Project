using System.Collections;
using UnityEngine;
public class BunkerOxygenZone : MonoBehaviour
{
    private StormEffectsManager stormEffectsManager;
    private bool isInsideZone = false;

    private void Start()
    {
        stormEffectsManager = FindFirstObjectByType<StormEffectsManager>();
        if (stormEffectsManager == null)
        {
            Debug.LogWarning("StormEffectsManager not found in the scene.");
        }
    }

    public void Interact()
    {
        if (!isInsideZone)
        {
            isInsideZone = true;
            Debug.Log("Player has entered the Bunker Oxygen Zone.");


            if (OxygenManager.Instance != null)
            {
                OxygenManager.Instance.EnterOxygenZone();
            }
            else
            {
                Debug.LogWarning("OxygenManager instance is missing!");
            }


            if (stormEffectsManager != null)
            {
                stormEffectsManager.HideStormEffects();
            }
        }
        else
        {
            isInsideZone = false;
            Debug.Log("Player has exited the Bunker Oxygen Zone.");


            if (OxygenManager.Instance != null)
            {
                OxygenManager.Instance.ExitOxygenZone();
            }
            else
            {
                Debug.LogWarning("OxygenManager instance is missing!");
            }


            if (stormEffectsManager != null)
            {
                stormEffectsManager.ShowStormEffects();
            }
        }
    }


    public bool IsPlayerInsideZone()
    {
        return isInsideZone;
    }


    public void ForceExitZone()
    {
        if (isInsideZone)
        {
            isInsideZone = false;
            Debug.Log("Player has been forced out of the Ladder Oxygen Zone.");
            if (OxygenManager.Instance != null)
            {
                OxygenManager.Instance.ExitOxygenZone();
            }

            stormEffectsManager.ShowStormEffects();
        }
    }


}