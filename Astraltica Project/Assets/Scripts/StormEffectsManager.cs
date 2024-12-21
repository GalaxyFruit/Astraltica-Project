using UnityEngine;

public class StormEffectsManager : MonoBehaviour
{
    [SerializeField] private GameObject rainEffect;    
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

        // Uložení původní intenzity světla
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

        rainEffect.SetActive(true);
        directionalLight.intensity = stormLightIntensity;

    }

    private void DisableStormEffects()
    {
        Debug.Log("Storm effects disabled!");

        rainEffect.SetActive(false);
        directionalLight.intensity = originalLightIntensity;

    }
}
