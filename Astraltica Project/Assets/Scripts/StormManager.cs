using UnityEngine;
using System;
using System.Collections;

public class StormManager : MonoBehaviour
{
    public static StormManager Instance { get; private set; }

    public event Action OnStormStarted;
    public event Action OnStormEnded;

    private bool isStormActive;

    [SerializeField] private GameObject rainEffectParent;
    [SerializeField] private GameObject lightingParent;
    [SerializeField] private GameObject windParent;

    [SerializeField] private int minStormDuration = 30;
    [SerializeField] private int maxStormDuration = 180;
    [SerializeField] private int minIntervalBetweenStorms = 120;
    [SerializeField] private int maxIntervalBetweenStorms = 360;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }


    private void Start()
    {
        StartCoroutine(StormCycle());
    }

    private IEnumerator StormCycle()
    {
        while (true)
        {
            // náhodný interval před každou bouřkou
            int interval = UnityEngine.Random.Range(minIntervalBetweenStorms, maxIntervalBetweenStorms);
            Debug.Log("Interval do další Storm je: " + interval + "sec");
            yield return new WaitForSeconds(interval);

            StartStorm();

            // trvání aktuální bouřky
            int duration = UnityEngine.Random.Range(minStormDuration, maxStormDuration);
            Debug.Log("Tato Storm bude trval: " + duration + "sec");
            yield return new WaitForSeconds(duration);

            EndStorm();
        }
    }

    private void StartStorm()
    {
        if (isStormActive) return; 
        isStormActive = true;

        Debug.Log("Storm started!");
        SetStormParents(true);
        OnStormStarted?.Invoke();
    }

    private void EndStorm()
    {
        if (!isStormActive) return; 
        isStormActive = false;

        SetStormParents(false);
        OnStormEnded?.Invoke();
        Debug.Log("Storm ended!");
    }

    private void SetStormParents(bool state)
    {
        rainEffectParent.SetActive(state);
        lightingParent.SetActive(state);
        windParent.SetActive(state);
    }
}
