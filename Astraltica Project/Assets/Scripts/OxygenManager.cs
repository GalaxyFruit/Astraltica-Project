using UnityEngine;
using System.Collections;

public class OxygenManager : MonoBehaviour
{
    //TODO : Přidat eventy na bouřku (začatek minus kyslíku); zobrazení kyslíku na hodinkoách a její třídu
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
        isDepleting = false;
        StopCoroutine(DepleteOxygenCoroutine());
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
    }

    public void ReplenishOxygen(float amount)
    {
        currentOxygen = Mathf.Clamp(currentOxygen + amount, 0f, maxOxygen);
    }
}
