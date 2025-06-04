using UnityEngine;

public class KeycardHolder : MonoBehaviour, IInteractable
{
    [Header("Keycard Type Required")]
    public KeycardType requiredKeycardType;

    [Header("Placement Transform (optional)")]
    public Transform placementPoint; 

    public string GetInteractionText()
    {
        return $"<b>[E]</b> to place keycard\n<color=#FFA500></color>";
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
            Vector3 placePos = placementPoint ? placementPoint.position : transform.position;
            Quaternion placeRot = placementPoint ? placementPoint.rotation : transform.rotation;
            Instantiate(keycardItem.itemPrefab, placePos, placeRot);
        }
        else
        {
            Debug.LogWarning("KeycardHolder: Keycard prefab is missing!");
        }
    }
}