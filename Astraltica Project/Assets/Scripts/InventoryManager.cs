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
                return true;
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
        if (slotTransform == null || pickupItem == null)
        {
            Debug.LogError("CreateItemInSlot: Neplatné vstupní parametry!");
            return;
        }

        if (inventoryItemPrefab == null)
        {
            Debug.LogError("CreateItemInSlot: inventoryItemPrefab není nastaven!");
            return;
        }

        GameObject newItem = Instantiate(inventoryItemPrefab, slotTransform);
        newItem.name = pickupItem.itemName; 

        if (!newItem.TryGetComponent(out InventoryItem inventoryItem))
        {
            Debug.LogError($"Prefab '{inventoryItemPrefab.name}' nemá komponentu InventoryItem!");
            Destroy(newItem);
            return;
        }

        SetInventoryItemProperties(inventoryItem, pickupItem);
        newItem.transform.localScale = Vector3.one;
    }

    private void SetInventoryItemProperties(InventoryItem inventoryItem, PickupItem pickupItem)
    {
        if (pickupItem.itemPrefab == null)
        {
            Debug.LogWarning($"PickupItem '{pickupItem.itemName}' nemá nastavený itemPrefab!");
        }

        if (inventoryItem.image != null)
        {
            inventoryItem.image.sprite = pickupItem.itemIcon;
            inventoryItem.image.preserveAspect = true;
        }
        else
        {
            Debug.LogError($"InventoryItem '{pickupItem.itemName}' nemá nastavenou image komponentu!");
        }

        inventoryItem.itemName = pickupItem.itemName;
        inventoryItem.itemPrefab = pickupItem.itemPrefab;
        inventoryItem.itemType = pickupItem.itemType;
        inventoryItem.crystalType = pickupItem.crystalType;
        inventoryItem.canEquipToHand = pickupItem.canEquipToHand;
    }

}
