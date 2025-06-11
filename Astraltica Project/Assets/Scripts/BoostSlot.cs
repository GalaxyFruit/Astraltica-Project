using UnityEngine;
using UnityEngine.EventSystems;

public class BoostSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("BoostSlot.OnDrop");
        if (transform.childCount > 1)
            return;
        //Debug.Log("1");
        var item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (item == null || item.itemType != ItemType.Potion)
            return;
        //Debug.Log("2");
        var activeType = BoostManager.Instance.GetActiveCrystalType();
        //Debug.Log("3");
        if (activeType != CrystalType.None && activeType != item.crystalType)
            return;
        //Debug.Log("4");
        BoostManager.Instance.AddPotion(item.crystalType);
        Destroy(item.gameObject);
        BoostManager.Instance.StartDepleting();
    }
}