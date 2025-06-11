using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class OxygenManager : MonoBehaviour, IDamageable
{
    public static OxygenManager Instance { get; private set; }

    [SerializeField] private float maxOxygen = 120f;
    [SerializeField] private float depletionAmount = 1f;
    [SerializeField] private Image oxygenBar;
    [SerializeField] private float regenRate = 1f;
    [SerializeField] private float regenRateOverTime = 1f;

    private Coroutine _oxygenCoroutine;
    private GameManager _gameManager;

    private float currentOxygen;
    private bool isDepleting = false;
    private bool isInOxygenZone = false;
    private bool isRegenerationBlocked = false;

    public float CurrentOxygen => currentOxygen;
    public float MaxOxygen => maxOxygen;

    public bool IsDead => currentOxygen <= 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        currentOxygen = maxOxygen;

        if (StormManager.Instance != null)
        {
            StormManager.Instance.OnStormStarted += StartOxygenDepletion;
            StormManager.Instance.OnStormEnded += StopOxygenDepletion;
        }
    }

    private void Start()
    {
        _gameManager = FindFirstObjectByType<GameManager>();
        if (_gameManager == null)
        {
            Debug.LogError("GameManager not found in the scene.");
        }
    }

    private void OnDestroy()
    {
        if (Instance == this)
            Instance = null;
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
            StopCoroutineSafe(ref _oxygenCoroutine);
            StartCoroutineSafe(ref _oxygenCoroutine, DepleteOxygenCoroutine());
        }
    }

    public void StopOxygenDepletion()
    {
        isDepleting = false;
        StopCoroutineSafe(ref _oxygenCoroutine);
        StartCoroutineSafe(ref _oxygenCoroutine, RegenerateOxygen());
    }

    private IEnumerator DepleteOxygenCoroutine()
    {
        while (isDepleting)
        {
            if (!isInOxygenZone)
            {
                currentOxygen = Mathf.Clamp(currentOxygen - depletionAmount, 0f, maxOxygen);
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

    private IEnumerator RegenerateOxygen()
    {
        //Debug.Log("Starting oxygen regeneration coroutine");
        while (currentOxygen < maxOxygen && !isRegenerationBlocked)
        {
            currentOxygen = Mathf.Clamp(currentOxygen + regenRate, 0f, maxOxygen);
            UpdateOxygenBar();
            yield return new WaitForSeconds(regenRateOverTime);

            //Debug.Log($"Regenerating oxygen: {currentOxygen}/{maxOxygen}");
        }
    }

    public void EnterOxygenZone()
    {
        Debug.Log("Entered oxygen zone");
        isInOxygenZone = true;
        StopCoroutineSafe(ref _oxygenCoroutine);
        StartCoroutineSafe(ref _oxygenCoroutine, RegenerateOxygen());
    }

    public void ExitOxygenZone()
    {
        Debug.Log("Exited oxygen zone"); 
        isInOxygenZone = false;
        StopCoroutineSafe(ref _oxygenCoroutine);
        if (isDepleting)
        {
            StartCoroutineSafe(ref _oxygenCoroutine, DepleteOxygenCoroutine());
        } else
        {
            StartCoroutineSafe(ref _oxygenCoroutine, RegenerateOxygen());
        }
    }

    public void TakeDamage(float damage)
    {
        currentOxygen = Mathf.Clamp(currentOxygen - damage, 0f, maxOxygen);
        Debug.Log($"Player took {damage} damage. Current oxygen: {currentOxygen}");
        UpdateOxygenBar();

        if (currentOxygen <= 0)
        {
            PlayerDies();
        }
        else
        {
            BlockOxygenRegeneration(10f);
        }
    }


    private void BlockOxygenRegeneration(float duration)
    {
        Debug.Log($"Blocking oxygen regeneration for {duration} seconds.");
        isRegenerationBlocked = true;
        StopCoroutineSafe(ref _oxygenCoroutine);
        StartCoroutine(BlockRegenerationCoroutine(duration));
    }

    private IEnumerator BlockRegenerationCoroutine(float duration)
    {
        yield return new WaitForSeconds(duration);
        isRegenerationBlocked = false;

        if (!isDepleting && !isInOxygenZone)
        {
            StartCoroutineSafe(ref _oxygenCoroutine, RegenerateOxygen());
        }
    }

    private void PlayerDies()
    {
        isDepleting = false;
        StopCoroutineSafe(ref _oxygenCoroutine);
        Debug.Log("Player has died due to lack of oxygen!");

        _gameManager.DeathScreen();
    }

    private void UpdateOxygenBar()
    {
        if (oxygenBar != null)
        {
            oxygenBar.fillAmount = currentOxygen / maxOxygen;
        }
    }

    private void StartCoroutineSafe(ref Coroutine coroutine, IEnumerator routine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
        }
        coroutine = StartCoroutine(routine);
    }

    private void StopCoroutineSafe(ref Coroutine coroutine)
    {
        if (coroutine != null)
        {
            StopCoroutine(coroutine);
            coroutine = null;
        }
    }
}
