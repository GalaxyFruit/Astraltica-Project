using UnityEngine;
using UnityEngine.UI;

public class BoostManager : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private Image boostFillImage;

    [Header("Boost Settings")]
    [SerializeField] private float fillDepletionRate = 0.01f;
    [Space]
    [SerializeField] private Color amethystColor = new Color(0.6f, 0.4f, 1f);
    [SerializeField] private Color cobaltColor = new Color(0.4f, 0.4f, 1f);
    [SerializeField] private Color emeraldColor = new Color(0f, 0.8f, 0.6f);
    [SerializeField] private Color honeyColor = new Color(1f, 0.8f, 0.4f);
    [SerializeField] private Color iceColor = new Color(0.9f, 0.9f, 1f);
    [SerializeField] private Color monolithColor = new Color(1f, 0.9f, 0.4f);
    [SerializeField] private Color pinkieColor = new Color(1f, 0.4f, 0.8f);
    [SerializeField] private Color rubyColor = new Color(1f, 0f, 0f);


    private float currentFillAmount = 0f;
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
            currentFillAmount -= fillDepletionRate;
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

    private void UpdateBoostColor()
    {
        switch (activeCrystalType)
        {
            case CrystalType.Amethyst:
                boostFillImage.color = amethystColor;
                break;
            case CrystalType.Cobalt:
                boostFillImage.color = cobaltColor;
                break;
            case CrystalType.Emerald:
                boostFillImage.color = emeraldColor;
                break;
            case CrystalType.Honey:
                boostFillImage.color = honeyColor;
                break;
            case CrystalType.Ice:
                boostFillImage.color = iceColor;
                break;
            case CrystalType.Monolith:
                boostFillImage.color = monolithColor;
                break;
            case CrystalType.Pinkie:
                boostFillImage.color = pinkieColor;
                break;
            case CrystalType.Ruby:
                boostFillImage.color = rubyColor;
                break;
        }
    }

    public void StartDepleting()
    {
        InvokeRepeating(nameof(DepleteCrystal), 0.1f, 0.1f);
    }

    private void StopDepleting()
    {
        CancelInvoke(nameof(DepleteCrystal));
    }
}
