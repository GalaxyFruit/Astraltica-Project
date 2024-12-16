using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Slots")]
    [SerializeField] private InventorySlot[] inventorySlots;

    [Header("Hotbar Slots")]
    [SerializeField] private GameObject[] hotbarSlots;

    private List<PickupItem> inventoryItems = new List<PickupItem>();

    public bool AddItem(PickupItem item)
    {
        // Nejprve hledáme volné místo v inventáři
        for (int i = 0; i < inventorySlots.Length; i++)
        {
            if (inventorySlots[i].transform.childCount == 0)
            {
                PlaceItemInSlot(inventorySlots[i].transform, item);
                inventoryItems.Add(item);
                return true;
            }
        }

        // Pokud je inventář plný, zkusíme hotbar
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            if (hotbarSlots[i].transform.childCount == 0)
            {
                PlaceItemInSlot(hotbarSlots[i].transform, item);
                return true;
            }
        }

        Debug.Log("Inventory and Hotbar are full!");
        return false;
    }

    private void PlaceItemInSlot(Transform slotTransform, PickupItem item)
    {
        GameObject newItem = new GameObject(item.itemName);
        Image image = newItem.AddComponent<Image>();
        image.sprite = item.itemIcon;
        newItem.transform.SetParent(slotTransform);
        newItem.transform.localScale = Vector3.one;
    }

}
