using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using TMPro;

public class ItemPickupController : MonoBehaviour
{
    [Header("Pickup Settings")]
    [SerializeField] private float pickupRange = 3.0f;
    [SerializeField] private TextMeshProUGUI pickupText;

    private PickupItem lastItem;

    private void Start()
    {
        if (pickupText != null)
            pickupText.gameObject.SetActive(false);
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
    }


    public void OnPickupAction(InputAction.CallbackContext context)
    {
        if (context.performed && lastItem != null)
        {
            Debug.Log("calling OnPickupAction()");
            AddItemToInventoryOrHotbar(lastItem);
            lastItem.Pickup();
            ClearPickupText();
        }
    }

    private void AddItemToInventoryOrHotbar(PickupItem item)
    {
        Debug.Log($"Picked up: {item.itemName}");
    }

    private void ClearPickupText()
    {
        lastItem = null;
        if (pickupText != null)
            pickupText.gameObject.SetActive(false);
    }
}
