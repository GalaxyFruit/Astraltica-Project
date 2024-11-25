using UnityEngine;
using System.Collections;

public class OxygenManager : MonoBehaviour
{
    public static OxygenManager Instance { get; private set; }

    [SerializeField] private float maxOxygen = 120f;
    [SerializeField] private float depletionAmount = 1f;
    private float currentOxygen;

    private bool isDepleting = false;

    public float CurrentOxygen => currentOxygen;

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
        currentOxygen = maxOxygen;

        if (StormManager.Instance != null)
        {
            StormManager.Instance.OnStormStarted += StartOxygenDepletion;
            StormManager.Instance.OnStormEnded += StopOxygenDepletion;
        }
    }

    private void OnDestroy()
    {
        if (StormManager.Instance != null)
        {
            StormManager.Instance.OnStormStarted -= StartOxygenDepletion;
            StormManager.Instance.OnStormEnded -= StopOxygenDepletion;
        }
    }

    public void StartOxygenDepletion()
    {
        Debug.Log("Bouřka začala. Spouštím ubývání kyslíku.");
        if (!isDepleting)
        {
            isDepleting = true;
            StartCoroutine(DepleteOxygenCoroutine());
        }
    }


    public void StopOxygenDepletion()
    {
        Debug.Log("Bouřka skončila.před if");
        if (isDepleting)
        {
            Debug.Log("Bouřka skončila. Obnovuji kyslík na maximum.");
            isDepleting = false;
            currentOxygen = maxOxygen;
            StopCoroutine(DepleteOxygenCoroutine());
        }
    }

    private IEnumerator DepleteOxygenCoroutine()
    {
        while (isDepleting)
        {
            currentOxygen -= depletionAmount;
            currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
            Debug.Log($"Úroveň kyslíku: {currentOxygen}");

            if (currentOxygen <= 0)
            {
                PlayerDies();
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }
    }

    private void PlayerDies()
    {
        isDepleting = false;
        Debug.Log("Hráč zemřel na kyslík!");
        GameManager.Instance.SetGameState(GameManager.GameState.Respawning); 
    }


    public void ReplenishOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0f, maxOxygen);
    }
}
