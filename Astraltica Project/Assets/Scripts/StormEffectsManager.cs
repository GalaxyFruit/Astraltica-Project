using UnityEngine;

public class StormEffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject rainEffectParent;
    [SerializeField] private GameObject lightingParent;
    [SerializeField] private GameObject mistFogParent;
    [SerializeField] private Light directionalLight;
    [SerializeField] private float stormLightIntensity = 0.3f;
    private float originalLightIntensity;

    
    private void Start()
    {
        if (StormManager.Instance != null)
        {
            StormManager.Instance.OnStormStarted += EnableStormEffects;
            StormManager.Instance.OnStormEnded += DisableStormEffects;
        }

        if (directionalLight != null)
        {
            originalLightIntensity = directionalLight.intensity;
        }
    }
    

    private void OnDestroy()
    {
        if (StormManager.Instance != null)
        {
            StormManager.Instance.OnStormStarted -= EnableStormEffects;
            StormManager.Instance.OnStormEnded -= DisableStormEffects;
        }
    }

    private void EnableStormEffects()
    {
        Debug.Log("Storm effects enabled!");
        SetStormParents(true);

        directionalLight.intensity = stormLightIntensity;

    }

    private void SetStormParents(bool state)
    {
        rainEffectParent.SetActive(state);
        lightingParent.SetActive(state);
        mistFogParent.SetActive(state);
    }

    private void DisableStormEffects()
    {
        Debug.Log("Storm effects disabled!");
        SetStormParents(false);

        directionalLight.intensity = originalLightIntensity;

    }
}
