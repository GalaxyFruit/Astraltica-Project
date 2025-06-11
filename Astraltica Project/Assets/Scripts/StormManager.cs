using UnityEngine;
using System;
using System.Collections;

public class StormManager : MonoBehaviour
{
    public static StormManager Instance { get; private set; }

    public event Action OnStormStarted;
    public event Action OnStormEnded;

    private bool isStormActive;

    [SerializeField] private int minStormDuration = 30; // Minimální délka bouřky v sekundách
    [SerializeField] private int maxStormDuration = 180; // Maximální délka bouřky v sekundách
    [SerializeField] private int minIntervalBetweenStorms = 120; // Minimální interval mezi bouřkami
    [SerializeField] private int maxIntervalBetweenStorms = 360; // Maximální interval mezi bouřkami

    public bool IsStormActive => isStormActive;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        StartCoroutine(StormCycle());
    }


    private IEnumerator StormCycle()
    {
        while (true)
        {
            // Čekání před začátkem bouřky (interval mezi bouřkami)
            int interval = UnityEngine.Random.Range(minIntervalBetweenStorms, maxIntervalBetweenStorms);
            Debug.Log("Interval do další bouřky: " + interval + " sekund");
            yield return new WaitForSeconds(interval);

            StartStorm();

            // Trvání aktuální bouřky
            int duration = UnityEngine.Random.Range(minStormDuration, maxStormDuration);
            Debug.Log("Bouřka bude trvat: " + duration + " sekund");
            yield return new WaitForSeconds(duration);

            EndStorm();
        }
    }

    private void StartStorm()
    {
        if (isStormActive) return;
        isStormActive = true;

        Debug.Log("Bouřka začala!");
        OnStormStarted?.Invoke(); 
    }

    private void EndStorm()
    {
        if (!isStormActive) return; 
        isStormActive = false;

        Debug.Log("Bouřka skončila!");
        OnStormEnded?.Invoke(); 
    }
}
