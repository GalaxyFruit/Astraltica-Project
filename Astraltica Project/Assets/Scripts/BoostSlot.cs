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

        BoostManager.Instance.AddPotion(item.crystalType);
        Destroy(item.gameObject);
        BoostManager.Instance.StartDepleting();
    }
}