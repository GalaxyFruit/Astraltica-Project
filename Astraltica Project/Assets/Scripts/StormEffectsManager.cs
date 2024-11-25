using UnityEngine;

public class StormEffectsManager : MonoBehaviour
{
    [SerializeField] private ParticleSystem rainEffect;  
    [SerializeField] private ParticleSystem fogEffect; 
    [SerializeField] private AudioSource stormAudio;     
    [SerializeField] private Light directionalLight;
    [SerializeField] private float stormLightIntensity = 0.3f;
    private float originalLightIntensity;

    /*
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
    */

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

        // Aktivace deště
        if (rainEffect != null)
        {
            rainEffect.Play();
        }

        // Aktivace mlhy
        if (fogEffect != null)
        {
            fogEffect.Play();
        }

        // Přehrání zvuků bouřky
        if (stormAudio != null)
        {
            stormAudio.Play();
        }

        // Změna světla
        if (directionalLight != null)
        {
            directionalLight.intensity = stormLightIntensity;
        }
    }

    private void DisableStormEffects()
    {
        Debug.Log("Storm effects disabled!");

        // Zastavení deště
        if (rainEffect != null)
        {
            rainEffect.Stop();
        }

        // Zastavení mlhy
        if (fogEffect != null)
        {
            fogEffect.Stop();
        }

        // Zastavení zvuků bouřky
        if (stormAudio != null)
        {
            stormAudio.Stop();
        }

        // Obnovení světla
        if (directionalLight != null)
        {
            directionalLight.intensity = originalLightIntensity;
        }
    }
}
