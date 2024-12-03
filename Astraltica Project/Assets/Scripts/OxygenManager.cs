using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OxygenManager : MonoBehaviour
{
    public static OxygenManager Instance { get; private set; }

    [SerializeField] private float maxOxygen = 120f;
    [SerializeField] private float depletionAmount = 1f;
    [SerializeField] private Image oxygenBar;

    private float currentOxygen;
    private bool isDepleting = false;
    private bool isInOxygenZone = false;

    public float CurrentOxygen => currentOxygen;
    public float MaxOxygen => maxOxygen;

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
        if (!isDepleting)
        {
            isDepleting = true;
            StartCoroutine(DepleteOxygenCoroutine());
        }
    }

    public void StopOxygenDepletion()
    {
        if (isDepleting)
        {
            isDepleting = false;
            currentOxygen = maxOxygen;
            UpdateOxygenBar();
        }
    }

    private IEnumerator DepleteOxygenCoroutine()
    {
        while (isDepleting)
        {
            if (!isInOxygenZone)
            {
                currentOxygen -= depletionAmount;
                currentOxygen = Mathf.Clamp(currentOxygen, 0f, maxOxygen);
                UpdateOxygenBar();

                if (currentOxygen <= 0)
                {
                    PlayerDies();
                    yield break;
                }
            }
            yield return new WaitForSeconds(1f);
        }
    }

    public void EnterOxygenZone()
    {
        isInOxygenZone = true;
    }

    public void ExitOxygenZone()
    {
        isInOxygenZone = false;
    }

    public void ReplenishOxygen(float amount)
    {
        //Debug.Log("puvodni hodnota: " + currentOxygen + "doplnuji kyslik o " + amount);
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0f, maxOxygen);
        UpdateOxygenBar();
    }

    private void PlayerDies()
    {
        isDepleting = false;
        Time.timeScale = 0f;
        Debug.Log("Player has died due to lack of oxygen!");
    }

    private void UpdateOxygenBar()
    {
        if (oxygenBar != null)
        {
            oxygenBar.fillAmount = currentOxygen / maxOxygen;
        }
    }
}
