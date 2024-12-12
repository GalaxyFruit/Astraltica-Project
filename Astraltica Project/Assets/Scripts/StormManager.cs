using UnityEngine;
using System;
using System.Collections;

public class StormManager : MonoBehaviour
{
    public static StormManager Instance { get; private set; }

    public event Action OnStormStarted;
    public event Action OnStormEnded;

    private bool isStormActive;

    [SerializeField] private GameObject StormPrefab;

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

        StormPrefab.SetActive(true);
        OnStormStarted?.Invoke(); // spuštění události
    }

    private void EndStorm()
    {
        if (!isStormActive) return; 
        isStormActive = false;

        StormPrefab?.SetActive(false);
        OnStormEnded?.Invoke(); // spuštění udalosti
        Debug.Log("Storm ended!");
    }
}
