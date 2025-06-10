using UnityEngine;
using UnityEngine.EventSystems;

public enum PotionSlotType
{
    Input,
    Output
}

public class PotionSlot : MonoBehaviour, IDropHandler
{
    public PotionSlotType slotType;
    public PotionCraftingManager craftingManager;

    public void OnDrop(PointerEventData eventData)
    {
        if (slotType == PotionSlotType.Input)
        {
            if (transform.childCount > 1) return;
            var item = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (item == null || item.itemType != ItemType.Crystal) return;

            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.zero;

            craftingManager.TryCraftPotion();
        }
    }

    private void OnTransformChildrenChanged()
    {
        if (slotType == PotionSlotType.Input)
        {
            // Check if the currently crafting crystal is still present in the slot
            var craftingCrystal = craftingManager != null ? craftingManager.CurrentCraftingCrystal : null;
            bool found = false;

            if (craftingCrystal != null)
            {
                for (int i = 1; i < transform.childCount; i++)
                {
                    if (transform.GetChild(i) == craftingCrystal.transform)
                    {
                        found = true;
                        break;
                    }
                }
            }

            // If the crafting crystal is not found, cancel crafting
            if (craftingCrystal != null && !found)
            {
                craftingManager.CancelCraftingIfCrystalRemoved(craftingCrystal);
            }
        }
    }
}