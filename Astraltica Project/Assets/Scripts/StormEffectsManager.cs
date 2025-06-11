using System;
using System.Collections;
using UnityEngine;

public class StormEffectsManager : MonoBehaviour
{
    [Header("Effect Parents")]
    [SerializeField] private GameObject rainEffectParent;
    [SerializeField] private GameObject lightingParent;

    [Header("Directional Light")]
    [SerializeField] private Light directionalLight;
    [SerializeField] private float stormLightIntensity = 0.3f;
    private float originalLightIntensity;

    [Header("Storm Settings")]
    [SerializeField] private float transitionDuration = 10f;

    [Header("Rain Particle System")]
    [SerializeField] private ParticleSystem rainParticleSystem;
    [SerializeField] private float maxRainRate = 1500f;

    [Header("Unity Fog Settings")]
    [SerializeField] private FogMode fogMode = FogMode.Linear;
    //[SerializeField] private float startfogDensity = 0.02f;
    [SerializeField][Range(0f, 1f)] private float stormFogDensity = 0.05f;
    private float originalFogDensity;
    [SerializeField] private Color stormFogColor = Color.gray;
    private Color originalFogColor;

    [SerializeField] private SkyDomeController skyDomeController;

    private ParticleSystem.EmissionModule rainEmission;
    private Coroutine stormTransitionCoroutine;
    private bool stormActive = false; 
    //private bool stormEnding = false; 
    private float stormProgress = 0f; 

    public bool IsStormActive => stormActive;

    private void Start()
    {
        if (StormManager.Instance != null)
        {
            StormManager.Instance.OnStormStarted += StartStormEffects;
            StormManager.Instance.OnStormEnded += EndStormEffects;
        }

        if (directionalLight != null)
            originalLightIntensity = directionalLight.intensity;

        if (rainParticleSystem != null)
            rainEmission = rainParticleSystem.emission;

        originalFogDensity = RenderSettings.fogDensity;
        originalFogColor = RenderSettings.fogColor;

        RenderSettings.fogMode = fogMode;

        RenderSettings.fog = false;
        SetStormVisualsActive(false);
        ActivateEffectParent(rainEffectParent, false); 
        SetRateOverTime(rainEmission, 0f);
    }

    private void OnDestroy()
    {
        if (StormManager.Instance != null)
        {
            StormManager.Instance.OnStormStarted -= StartStormEffects;
            StormManager.Instance.OnStormEnded -= EndStormEffects;
        }
    }

    private void StartStormTransition(bool starting)
    {
        if (stormTransitionCoroutine != null)
            StopCoroutine(stormTransitionCoroutine);

        stormTransitionCoroutine = StartCoroutine(StormTransition(starting));
    }

    private IEnumerator StormTransition(bool starting)
    {
        float time = 0f;

        while (time < transitionDuration)
        {
            float t = time / transitionDuration;

            stormProgress = starting ? t : 1f - t;

            SetRateOverTime(rainEmission, Mathf.Lerp(0f, maxRainRate, stormProgress));
            directionalLight.intensity = Mathf.Lerp(originalLightIntensity, stormLightIntensity, stormProgress);
            RenderSettings.fogDensity = Mathf.Lerp(originalFogDensity, stormFogDensity, stormProgress);
            RenderSettings.fogColor = Color.Lerp(originalFogColor, stormFogColor, stormProgress);

            time += Time.deltaTime;
            yield return null;
        }

        stormProgress = starting ? 1f : 0f;

        SetRateOverTime(rainEmission, starting ? maxRainRate : 0f);
        directionalLight.intensity = starting ? stormLightIntensity : originalLightIntensity;
        RenderSettings.fogDensity = starting ? stormFogDensity : originalFogDensity;
        RenderSettings.fogColor = starting ? stormFogColor : originalFogColor;

        if (!starting)
        {
            HideStormEffects();
            rainParticleSystem?.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }
    }

    private void StartStormEffects()
    {
        stormActive = true;
        stormProgress = 0f;

        skyDomeController.SetStormSky();

        ActivateEffectParent(rainEffectParent, true);

        bool isPlayerInside = FindFirstObjectByType<TeleportManager>()?.IsInOxygenZone ?? false;

        if (isPlayerInside)
        {
            HideStormEffects();
        }
        else
        {
            ShowStormEffects();
        }

        StartStormTransition(true);
    }

    private void EndStormEffects()
    {
        stormActive = false;

        skyDomeController.ResetSky();
        ActivateEffectParent(rainEffectParent, false);

        StartStormTransition(false);
    }

    private void SetStormVisualsActive(bool state)
    {
        if (rainParticleSystem != null)
        {
            if (state && !rainParticleSystem.isPlaying)
                rainParticleSystem.Play();
            else if (!state && rainParticleSystem.isPlaying)
                rainParticleSystem.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
        }

        if (lightingParent != null)
            lightingParent.SetActive(state);
    }

    private void SetRateOverTime(ParticleSystem.EmissionModule emission, float rate)
    {
        ParticleSystem.MinMaxCurve curve = emission.rateOverTime;
        curve.constant = rate;
        emission.rateOverTime = curve;
    }

    private void ActivateEffectParent(GameObject effectParent, bool state)
    {
        if (effectParent != null)
        {
            effectParent.SetActive(state);
        }
    }

    public void ShowStormEffects()
    {
        if (!stormActive)
        {
            Debug.Log("Storm is not active. Effects will not be shown.");
            return;
        }

        SetStormVisualsActive(true);
        ActivateEffectParent(lightingParent, true);
        RenderSettings.fog = true;
        Debug.Log("Storm effects shown.");
    }

    public void HideStormEffects()
    {
        RenderSettings.fog = false;
        SetStormVisualsActive(false);
        ActivateEffectParent(lightingParent, false);
        Debug.Log("Storm effects hidden.");
    }
}