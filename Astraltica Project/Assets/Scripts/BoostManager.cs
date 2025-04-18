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

    [SerializeField]
    private Dictionary<CrystalType, Color> crystalColors = new Dictionary<CrystalType, Color>()
    {
        { CrystalType.Amethyst, new Color(0.6f, 0.4f, 1f) },
        { CrystalType.Cobalt, new Color(0.4f, 0.4f, 1f) },
        { CrystalType.Emerald, new Color(0f, 0.8f, 0.6f) },
        { CrystalType.Honey, new Color(1f, 0.8f, 0.4f) },
        { CrystalType.Ice, new Color(0.9f, 0.9f, 1f) },
        { CrystalType.Monolith, new Color(1f, 0.9f, 0.4f) },
        { CrystalType.Pinkie, new Color(1f, 0.4f, 0.8f) },
        { CrystalType.Ruby, new Color(1f, 0f, 0f) }
    };

    [SerializeField]
    private Dictionary<CrystalType, BoostData> crystalBoosts = new Dictionary<CrystalType, BoostData>()
    {
        { CrystalType.Ice, new BoostData(1.2f, 1.0f) },
        { CrystalType.Ruby, new BoostData(1.0f, 1.15f) },
        { CrystalType.Emerald, new BoostData(1.15f, 1.1f) },
        { CrystalType.Monolith, new BoostData(1.3f, 1.15f) },
        { CrystalType.Amethyst, new BoostData(2f, 1.5f) }
    };

    private const float CRYSTAL_FILL_INCREMENT = 0.25f;
    private const float MAX_FILL_AMOUNT = 1f;

    private float currentFillAmount;
    private float depletionAmount;
    private bool isCrystalActive;
    private CrystalType activeCrystalType;

    public static BoostManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public bool CanAddCrystal(CrystalType crystalType) =>
        !isCrystalActive || activeCrystalType == crystalType;

    public void AddCrystal(CrystalType crystalType)
    {
        if (!isCrystalActive) ActivateNewCrystal(crystalType);
        else if (activeCrystalType == crystalType) AddCrystalValue();
    }

    private void AddCrystalValue()
    {
        currentFillAmount = Mathf.Min(currentFillAmount + CRYSTAL_FILL_INCREMENT, MAX_FILL_AMOUNT);
        UpdateFillAmount();
    }

    private void ActivateNewCrystal(CrystalType crystalType)
    {
        currentFillAmount = CRYSTAL_FILL_INCREMENT;
        activeCrystalType = crystalType;
        UpdateFillAmount();
        UpdateBoostColor();
        isCrystalActive = true;
    }

    private void DepleteCrystal()
    {
        if (isCrystalActive && currentFillAmount > 0)
        {
            currentFillAmount -= depletionAmount;
            UpdateFillAmount();
            if (currentFillAmount <= 0) DeactivateCrystal();
        }
    }

    private void UpdateFillAmount()
    {
        boostFillImage.fillAmount = currentFillAmount;
    }

    private void DeactivateCrystal()
    {
        isCrystalActive = false;
        activeCrystalType = CrystalType.None;
        UpdateFillAmount();
        StopDepleting();
    }

    public BoostData GetCurrentBoostMultiplier()
    {
        if (isCrystalActive && currentFillAmount > 0 &&
            crystalBoosts.TryGetValue(activeCrystalType, out BoostData boost))
        {
            return boost;
        }
        return new BoostData(1.0f, 1.0f);
    }

    private void UpdateBoostColor()
    {
        if (crystalColors.TryGetValue(activeCrystalType, out Color color))
        {
            boostFillImage.color = color;
        }
    }

    public void StartDepleting()
    {
        depletionAmount = fillDepletionRateInSecond * depletionInterval;
        InvokeRepeating(nameof(DepleteCrystal), depletionInterval, depletionInterval);
    }

    private void StopDepleting() => CancelInvoke(nameof(DepleteCrystal));
}
