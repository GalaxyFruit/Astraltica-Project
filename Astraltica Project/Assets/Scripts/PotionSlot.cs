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
}
