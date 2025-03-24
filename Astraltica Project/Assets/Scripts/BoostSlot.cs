using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BoostSlot : MonoBehaviour, IDropHandler
{
    [Header("UI Elements")]
    [SerializeField] private Image boostFillImage;

    [Header("Boost Settings")]
    [SerializeField] private Color amethystColor = new Color(0.6f, 0.4f, 1f);
    [SerializeField] private Color cobaltColor = new Color(0.4f, 0.4f, 1f);
    [SerializeField] private Color emeraldColor = new Color(0f, 0.8f, 0.6f);
    [SerializeField] private Color honeyColor = new Color(1f, 0.8f, 0.4f);
    [SerializeField] private Color iceColor = new Color(0.9f, 0.9f, 1f);
    [SerializeField] private Color monolithColor = new Color(1f, 0.9f, 0.4f);
    [SerializeField] private Color pinkieColor = new Color(1f, 0.4f, 0.8f);
    [SerializeField] private Color rubyColor = new Color(1f, 0f, 0f);

    [Header("Boost Settings")]
    [SerializeField] private float checkInterval = 0.5f;
    [SerializeField] private float fillDepletionRate = 0.01f;

    private float currentFillAmount = 0f;
    private bool isCrystalActive = false;
    private CrystalType activeCrystalType;
    private GameObject waitingCrystalGO;

    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount > 2)
        {
            Debug.Log("Slot je plný!");
            return;
        }

        InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (item == null || item.itemType != ItemType.Crystal) return;

        if (isCrystalActive)
        {
            // Pokud je stejný typ přičteme hodnotu
            if (activeCrystalType == item.crystalType)
            {
                AddCrystalValue(item.crystalType);
                Destroy(item.gameObject);
            }
            // Pokud je jiný typ čekáme
            else
            {
                item.parentAfterDrag = transform;
                waitingCrystalGO = item.gameObject;
                StartCoroutine(CheckWaitingCrystal());
                Debug.Log($"Krystal {item.crystalType} čeká na uvolnění slotu");
            }
        }
        // Není aktivní žádný krystal
        else
        {
            item.parentAfterDrag = transform;
            ActivateNewCrystal(item.crystalType);
            Destroy(item.gameObject);
        }
    }

    private void AddCrystalValue(CrystalType crystalType)
    {
        currentFillAmount = Mathf.Min(currentFillAmount + 0.25f, 1f);
        boostFillImage.fillAmount = currentFillAmount;
        Debug.Log($"Přidán krystal {crystalType}, nová hodnota: {currentFillAmount}");
    }

    private void ActivateNewCrystal(CrystalType crystalType)
    {
        currentFillAmount = 0.25f;
        activeCrystalType = crystalType;
        boostFillImage.fillAmount = currentFillAmount;
        UpdateBoostColor();
        isCrystalActive = true;
        StartDepleting();
    }

    private System.Collections.IEnumerator CheckWaitingCrystal()
    {
        while (waitingCrystalGO != null)
        {
            Debug.Log("Kontrola čekajícího krystalu");
            yield return new WaitForSeconds(checkInterval);

            // Pokud byl krystal v slotu přesunut
            if (!waitingCrystalGO.transform.IsChildOf(transform))
            {
                waitingCrystalGO = null;
                break;
            }

            // Uvolnění slotu
            if (!isCrystalActive)
            {
                InventoryItem waitingItem = waitingCrystalGO.GetComponent<InventoryItem>();
                if (waitingItem != null)
                {
                    ActivateNewCrystal(waitingItem.crystalType);
                    Destroy(waitingCrystalGO);
                    waitingCrystalGO = null;
                }
                break;
            }
        }
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
        Debug.Log("Krystal vyčerpán");
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

    private void StartDepleting()
    {
        InvokeRepeating(nameof(DepleteCrystal), 0.1f, 0.1f); // Volá každých 0.1 sekundy
    }

    private void StopDepleting()
    {
        CancelInvoke(nameof(DepleteCrystal));
    }
}
