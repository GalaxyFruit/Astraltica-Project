using UnityEngine;
using System;
using System.Collections;

public class StormManager : MonoBehaviour
{
    public static StormManager Instance { get; private set; }

    public event Action OnStormStarted;
    public event Action OnStormEnded;

    private bool isStormActive;

    [SerializeField] private float minStormDuration = 30f;
    [SerializeField] private float maxStormDuration = 180f;
    [SerializeField] private float minIntervalBetweenStorms = 120f;
    [SerializeField] private float maxIntervalBetweenStorms = 360f;

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
        StartStorm(); 
        StartCoroutine(StormCycle());
    }

    private IEnumerator StormCycle()
    {
        while (true)
        {
            // náhodný interval před každou bouřkou
            float interval = UnityEngine.Random.Range(minIntervalBetweenStorms, maxIntervalBetweenStorms);
            yield return new WaitForSeconds(interval);

            StartStorm();

            // trvání aktuální bouřky
            float duration = UnityEngine.Random.Range(minStormDuration, maxStormDuration);
            yield return new WaitForSeconds(duration);

            EndStorm();
        }
    }

    private void StartStorm()
    {
        if (isStormActive) return; 
        isStormActive = true;

        Debug.Log("Storm started!");

        OnStormStarted?.Invoke(); // spuštění události
    }

    private void EndStorm()
    {
        if (!isStormActive) return; 
        isStormActive = false;

        OnStormEnded?.Invoke(); // spuštění udalosti
        Debug.Log("Storm ended!");
    }
}
