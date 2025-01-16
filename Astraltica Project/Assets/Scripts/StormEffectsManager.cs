using UnityEngine;

public class StormEffectsManager : MonoBehaviour
{
    [Header("Effect Parents")]
    [SerializeField] private GameObject rainEffectParent;
    [SerializeField] private GameObject lightingParent;
    [SerializeField] private GameObject mistFogParent;

    [Header("Directional Light")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private float stormLightIntensity = 0.3f; 
    private float originalLightIntensity;

    [Header("Storm Settings")]
    [SerializeField] private float transitionDuration = 10f; 

    [Header("Rain Particle System")]
    [SerializeField] private ParticleSystem rainParticleSystem;
    [SerializeField] private float maxRainRate = 1500f; 

    [Header("Fog Particle System")]
    [SerializeField] private ParticleSystem fogParticleSystem;
    [SerializeField] private float maxFogRate = 400f;

    [SerializeField] private SkyDomeController skyDomeController;

    private ParticleSystem.EmissionModule rainEmission;
    private ParticleSystem.EmissionModule fogEmission;

    private bool stormActive = false;
    private bool stormEnding = false; 
    private float stormProgress = 0f;

    private void Start()
    {
        if (StormManager.Instance != null)
        {
            StormManager.Instance.OnStormStarted += StartStormEffects;
            StormManager.Instance.OnStormEnded += EndStormEffects;
        }

        if (directionalLight != null)
        {
            originalLightIntensity = directionalLight.intensity;
        }

        if (rainParticleSystem != null)
            rainEmission = rainParticleSystem.emission;

        if (fogParticleSystem != null)
            fogEmission = fogParticleSystem.emission;

        SetStormParents(false);
        SetRateOverTime(rainEmission, 0f);
        SetRateOverTime(fogEmission, 0f);
    }

    private void OnDestroy()
    {
        if (StormManager.Instance != null)
        {
            StormManager.Instance.OnStormStarted -= StartStormEffects;
            StormManager.Instance.OnStormEnded -= EndStormEffects;
        }
    }

    private void Update()
    {
        if (stormActive || stormEnding)
        {
            stormProgress += (stormActive ? 1 : -1) * Time.deltaTime / transitionDuration;
            stormProgress = Mathf.Clamp01(stormProgress);

            SetRateOverTime(rainEmission, Mathf.Lerp(0f, maxRainRate, stormProgress));
            SetRateOverTime(fogEmission, Mathf.Lerp(0f, maxFogRate, stormProgress));

            directionalLight.intensity = Mathf.Lerp(originalLightIntensity, stormLightIntensity, stormProgress);

            if (stormEnding && stormProgress <= 0f)
            {
                stormEnding = false;
                SetStormParents(false);
                rainParticleSystem?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
                fogParticleSystem?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }


    private void StartStormEffects()
    {
        //Debug.Log("Storm effects started!");
        stormActive = true;
        stormProgress = 0f;
        skyDomeController.SetStormSky(); 
        SetStormParents(true);
        directionalLight.intensity = stormLightIntensity;
    }

    private void EndStormEffects()
    {
        //Debug.Log("Storm effects ended!");
        stormActive = false;
        stormEnding = true;

        skyDomeController.ResetSky();
    }

    private void SetStormParents(bool state)
    {
        if (rainEffectParent != null) rainEffectParent.SetActive(state);
        if (lightingParent != null) lightingParent.SetActive(state);
        if (mistFogParent != null) mistFogParent.SetActive(state);
    }

    private void SetRateOverTime(ParticleSystem.EmissionModule emission, float rate)
    {
        ParticleSystem.MinMaxCurve curve = emission.rateOverTime; 
        curve.constant = rate;                                     
        emission.rateOverTime = curve;
        //Debug.Log($"Set rate over time to {rate} for {emission}");
    }

}