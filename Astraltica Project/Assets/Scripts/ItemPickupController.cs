using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class ItemPickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float pickupRange = 3.0f;
    [SerializeField] private TextMeshProUGUI pickupText;
    [SerializeField] private TextMeshProUGUI FullInventoryText; 

    [Header("References")]
    [SerializeField] private InventoryManager inventoryManager;


    [Header("Layer Mask Settings")]
    [SerializeField] private LayerMask interactableLayer;

    private IInteractable lastInteractable;

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

        if (Physics.Raycast(ray, out RaycastHit hit, pickupRange, interactableLayer))
        {
            if (hit.collider.TryGetComponent<IInteractable>(out var interactable))
            {
                if (lastInteractable != interactable)
                {
                    //Debug.Log("Nový interakční objekt nalezen");
                    lastInteractable = interactable;
                    pickupText.text = interactable.GetInteractionText();
                    pickupText.gameObject.SetActive(true);
                }
                else
                {
                    //Debug.Log("Stále stejný interakční objekt");
                }
            }
        }
        else
        {
            //Debug.Log("Raycast netrefil nic - clear2");
            ClearPickupText();
        }
    }


    public void OnPickupAction(InputAction.CallbackContext context)
    {
        if (context.performed && lastInteractable != null)
        {
            lastInteractable.Interact();
            ClearPickupText();
        }
    }

    public bool TryPickup(PickupItem item)
    {
        bool success = inventoryManager.AddItemToInventoryOrHotbar(item);
        if (!success)
        {
            FullInventoryText.text = "Inventory and Hotbar are full!";
            FullInventoryText.gameObject.SetActive(true);
            Invoke(nameof(HideOutputText), 2f);
        }
        return success;
    }

    private void ClearPickupText()
    {
        if (pickupText != null)
            pickupText.gameObject.SetActive(false);

        lastInteractable = null;
    }

    private void HideOutputText()
    {
        if (FullInventoryText != null)
            FullInventoryText.gameObject.SetActive(false);
    }
}
