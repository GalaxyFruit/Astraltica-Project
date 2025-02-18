using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ItemPickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float pickupRange = 3.0f;
    [SerializeField] private TextMeshProUGUI pickupText;
    [SerializeField] private TextMeshProUGUI FullInventoryText; 

    [Header("Inventory Manager")]
    [SerializeField] private InventoryManager inventoryManager;

    private PickupItem lastItem;

    private void Start()
    {
        if (pickupText != null)
            pickupText.gameObject.SetActive(false);

        if (FullInventoryText != null)
            FullInventoryText.gameObject.SetActive(false);
    }

    private void Update()
    {
        CheckForItemPickup();
    }

    private void CheckForItemPickup()
    {
        Ray ray = Camera.main.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2));
        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange))
        {
            if (hit.collider.TryGetComponent<PickupItem>(out var item))
            {
                if (lastItem != item)
                {
                    lastItem = item;
                    pickupText.text = $"{item.itemName}";
                    pickupText.gameObject.SetActive(true);
                }
            }
            else
            {
                ClearPickupText();
            }
        }
        else
        {
            ClearPickupText();
        }
    }

    public void OnPickupAction(InputAction.CallbackContext context)
    {
        if (context.performed && lastItem != null)
        {
            bool success = inventoryManager.AddItemToInventoryOrHotbar(lastItem);

            if (success)
            {
                lastItem.Pickup();
                ClearPickupText();
            }
            else // Inventory is full
            {
                FullInventoryText.text = "Inventory and Hotbar are full!";
                FullInventoryText.gameObject.SetActive(true);
                Invoke(nameof(HideOutputText), 2f);
            }
        }
    }

    private void ClearPickupText()
    {
        lastItem = null;
        if (pickupText != null)
            pickupText.gameObject.SetActive(false);
    }

    private void HideOutputText()
    {
        if (FullInventoryText != null)
            FullInventoryText.gameObject.SetActive(false);
    }
}
