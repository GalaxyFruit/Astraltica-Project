using UnityEngine;
using UnityEngine.EventSystems;

public class BoostSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount > 1)
            return;

        var item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (item == null || item.itemType != ItemType.Potion)
            return;

        var activeType = BoostManager.Instance.GetActiveCrystalType();

        if (activeType != CrystalType.None && activeType != item.crystalType)
            return;

        BoostManager.Instance.AddPotion(item.crystalType);
        Destroy(item.gameObject);
        BoostManager.Instance.StartDepleting();
    }
}