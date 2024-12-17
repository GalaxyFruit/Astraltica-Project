using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    [Header("Inventory Slots")]
    [SerializeField] private GameObject[] inventorySlots;

    [Header("Hotbar Slots")]
    [SerializeField] private GameObject[] hotbarSlots;

    [Header("Item Prefab")]
    [SerializeField] private GameObject inventoryItemPrefab;

    public bool AddItemToInventoryOrHotbar(PickupItem pickupItem)
    {
        // 1. Prohledávání inventáře
        foreach (var slot in inventorySlots)
        {
            if (slot.transform.childCount <= 1) 
            {
                CreateItemInSlot(slot.transform, pickupItem);
                return true; // Podařilo se přidat item
            }
        }

        // 2. Prohledávání hotbaru
        foreach (var slot in hotbarSlots)
        {
            if (slot.transform.childCount <= 1)
            {
                CreateItemInSlot(slot.transform, pickupItem);
                return true;
            }
        }

        // 3. Inventář i hotbar jsou plné
        Debug.Log("Inventory and Hotbar are full!");
        return false;
    }

    private void CreateItemInSlot(Transform slotTransform, PickupItem pickupItem)
    {
        // Vytvoření Itemu v přislusnem slotu
        GameObject newItem = Instantiate(inventoryItemPrefab, slotTransform);
        newItem.name = pickupItem.itemName; 

        InventoryItem inventoryItem = newItem.GetComponent<InventoryItem>();
        if (inventoryItem != null)
        {
            inventoryItem.image.sprite = pickupItem.itemIcon; 
        }

        newItem.transform.localScale = Vector3.one;
    }
}
