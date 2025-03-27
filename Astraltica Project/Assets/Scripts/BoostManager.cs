using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoostManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image boostFillImage;

    [Header("Boost Settings")]
    [SerializeField] private float fillDepletionRateInSecond = 0.01f;
    [SerializeField] private float depletionInterval = 0.05f;
    [Space]
    [SerializeField] private Color amethystColor = new Color(0.6f, 0.4f, 1f);
    [SerializeField] private Color cobaltColor = new Color(0.4f, 0.4f, 1f);
    [SerializeField] private Color emeraldColor = new Color(0f, 0.8f, 0.6f);
    [SerializeField] private Color honeyColor = new Color(1f, 0.8f, 0.4f);
    [SerializeField] private Color iceColor = new Color(0.9f, 0.9f, 1f);
    [SerializeField] private Color monolithColor = new Color(1f, 0.9f, 0.4f);
    [SerializeField] private Color pinkieColor = new Color(1f, 0.4f, 0.8f);
    [SerializeField] private Color rubyColor = new Color(1f, 0f, 0f);

    [Header("Boost Effects")]
    [SerializeField]
    private Dictionary<CrystalType, BoostData> crystalBoosts = new Dictionary<CrystalType, BoostData>()
    {
        { CrystalType.Ice, new BoostData(1.5f, 1.0f) },      // 50% speed boost
        { CrystalType.Ruby, new BoostData(1.0f, 1.3f) },     // 30% jump boost
        { CrystalType.Emerald, new BoostData(1.3f, 1.2f) },  // 30% speed + 20% jump
        { CrystalType.Monolith, new BoostData(1.8f, 1.5f) }, // Tier 2: 80% speed + 50% jump
        { CrystalType.Amethyst, new BoostData(2.0f, 1.7f) }  // Tier 3: 100% speed + 70% jump
    };


    private PlayerController playerController;
    private float currentFillAmount = 0f;
    private float depletionAmount;
    private bool isCrystalActive = false;
    private CrystalType activeCrystalType;

    public static BoostManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        playerController = FindFirstObjectByType<PlayerController>();
    }

    public bool CanAddCrystal(CrystalType crystalType)
    {
        return !isCrystalActive || activeCrystalType == crystalType;
    }

    public void AddCrystal(CrystalType crystalType)
    {
        if (!isCrystalActive)
        {
            ActivateNewCrystal(crystalType);
        }
        else if (activeCrystalType == crystalType)
        {
            AddCrystalValue(crystalType);
        }
    }

    private void AddCrystalValue(CrystalType crystalType)
    {
        currentFillAmount = Mathf.Min(currentFillAmount + 0.25f, 1f);
        boostFillImage.fillAmount = currentFillAmount;
        //Debug.Log($"Přidán krystal {crystalType}, nová hodnota: {currentFillAmount}");
    }

    private void ActivateNewCrystal(CrystalType crystalType)
    {
        currentFillAmount = 0.25f;
        activeCrystalType = crystalType;
        boostFillImage.fillAmount = currentFillAmount;
        UpdateBoostColor();
        isCrystalActive = true;
    }

    private void DepleteCrystal()
    {
        if (isCrystalActive && currentFillAmount > 0)
        {
            currentFillAmount -= depletionAmount;
            boostFillImage.fillAmount = currentFillAmount;
        }
        else
        {
            DeactivateCrystal();
            StopDepleting();
        }
    }

    private void DeactivateCrystal()
    {
        isCrystalActive = false;
        activeCrystalType = CrystalType.None;
        boostFillImage.fillAmount = 0f;
        //Debug.Log("Krystal vyčerpán");
    }

    public BoostData GetCurrentBoostMultiplier()
    {
        if (isCrystalActive && currentFillAmount > 0)
        {
            Debug.Log($"Getting boostData: {crystalBoosts[activeCrystalType].ToString()}");
            return crystalBoosts[activeCrystalType];
        }
        return new BoostData(1.0f, 1.0f);
    }

    private void UpdateBoostColor()
    {
        switch (activeCrystalType)
        {
            case CrystalType.Amethyst:
                boostFillImage.color = amethystColor; // schopnost
                break;
            case CrystalType.Cobalt:
                boostFillImage.color = cobaltColor; // schopnost
                break;
            case CrystalType.Emerald:
                boostFillImage.color = emeraldColor; // schopnost
                break;
            case CrystalType.Honey:
                boostFillImage.color = honeyColor; // schopnost
                break;
            case CrystalType.Ice:
                boostFillImage.color = iceColor; // schopnost
                break;
            case CrystalType.Monolith:
                boostFillImage.color = monolithColor; // schopnost
                break;
            case CrystalType.Pinkie:
                boostFillImage.color = pinkieColor; // schopnost
                break;
            case CrystalType.Ruby:
                boostFillImage.color = rubyColor; // schopnost
                break;
        }
    }

    public void StartDepleting()
    {
        depletionAmount = fillDepletionRateInSecond * depletionInterval;
        InvokeRepeating(nameof(DepleteCrystal), depletionInterval, depletionInterval);
    }

    private void StopDepleting()
    {
        CancelInvoke(nameof(DepleteCrystal));
    }
}
