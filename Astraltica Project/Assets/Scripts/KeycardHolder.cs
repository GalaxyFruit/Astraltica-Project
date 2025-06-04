using UnityEngine;

public class KeycardHolder : MonoBehaviour, IInteractable
{
    [Header("Keycard Type Required")]
    public KeycardType requiredKeycardType;

    [Header("Placement Transform (optional)")]
    public Transform placementPoint; 

    public string GetInteractionText()
    {
        return $"<b>[E]</b> to place <color=#FFA500>{requiredKeycardType}</color>";
    }

    public void Interact()
    {
        InventoryManager inventory = FindFirstObjectByType<InventoryManager>();
        if (inventory == null)
        {
            Debug.LogError("KeycardHolder: InventoryManager not found!");
            return;
        }

        InventoryItem keycardItem = inventory.RemoveKeycard(requiredKeycardType);
        if (keycardItem == null)
        {
            Debug.Log("KeycardHolder: No matching keycard found in inventory or hotbar.");
            return;
        }
        PlaceKeycardPrefab(keycardItem);
    }

    private void PlaceKeycardPrefab(InventoryItem keycardItem)
    {
        if (keycardItem.itemPrefab != null)
        {
            GameObject placedKeycard = Instantiate(keycardItem.itemPrefab, placementPoint.position, placementPoint.rotation);

            placedKeycard.layer = 0;

            var pickupComponent = placedKeycard.GetComponent<PickupItem>();
            if (pickupComponent != null)
            {
                Destroy(pickupComponent);
            }

            //Destroy(keycardItem.gameObject);

            gameObject.SetActive(false);
        }
        else
        {
            Debug.LogWarning("KeycardHolder: Keycard prefab or placementPoint is missing!");
        }
    }
}