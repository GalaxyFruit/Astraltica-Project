using UnityEngine;
using UnityEngine.EventSystems;

public class InventorySlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log($"volame OnDrop, childcount je: ${transform.childCount} pro ${transform.name}");
        if (transform.childCount >= 2) return;
        //Debug.Log("po if");

        InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        item.parentAfterDrag = transform;
    }
}
