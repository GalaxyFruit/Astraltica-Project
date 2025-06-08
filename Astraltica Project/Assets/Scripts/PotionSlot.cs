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
            if (transform.childCount > 1) return; // pouze border
            var item = eventData.pointerDrag.GetComponent<InventoryItem>();
            if (item == null || item.itemType != ItemType.Crystal) return;

            // Přesuň crystal do input slotu
            item.transform.SetParent(transform);
            item.transform.localPosition = Vector3.zero;

            // Spusť crafting
            craftingManager.TryCraftPotion();
        }
        // Output slot drop ignorujeme (hráč tam nic nedropuje)
    }
}