using System.Collections;
using UnityEngine;

public class PotionCraftingManager : MonoBehaviour
{
    [Header("Crystal to Potion Prefabs")]
    [SerializeField] private CrystalType[] crystalTypes;
    [SerializeField] private GameObject[] potionPrefabs;

    [Header("Crafting Slots")]
    [SerializeField] private PotionSlot inputSlot;
    [SerializeField] private PotionSlot outputSlot;

    [Header("Crafting Settings")]
    [SerializeField] private float craftingDuration = 1f;

    private Coroutine craftingCoroutine;
    private InventoryItem currentCrystal;

    public InventoryItem CurrentCraftingCrystal => currentCrystal;

    public void TryCraftPotion()
    {
        if (craftingCoroutine != null)
            return;

        if (!IsInputSlotReady() || !IsOutputSlotReady())
            return;

        var crystalItem = GetCrystalFromInputSlot();
        if (crystalItem == null)
            return;

        currentCrystal = crystalItem;
        craftingCoroutine = StartCoroutine(CraftingCoroutine(crystalItem));
    }

    private bool IsInputSlotReady()
    {
        return inputSlot.transform.childCount > 1;
    }

    private bool IsOutputSlotReady()
    {
        return outputSlot.transform.childCount <= 1;
    }

    private InventoryItem GetCrystalFromInputSlot()
    {
        for (int i = 0; i < inputSlot.transform.childCount; i++)
        {
            var item = inputSlot.transform.GetChild(i).GetComponent<InventoryItem>();
            if (item != null && item.itemType == ItemType.Crystal)
                return item;
        }
        return null;
    }

    private int GetPotionIndexForCrystal(CrystalType type)
    {
        for (int i = 0; i < crystalTypes.Length; i++)
        {
            if (crystalTypes[i] == type && i < potionPrefabs.Length && potionPrefabs[i] != null)
                return i;
        }
        return -1;
    }

    private IEnumerator CraftingCoroutine(InventoryItem crystalItem)
    {
        yield return new WaitForSeconds(craftingDuration);

        if (IsCrystalStillInInputSlot(crystalItem))
        {
            int index = GetPotionIndexForCrystal(crystalItem.crystalType);
            if (index != -1)
            {
                CreatePotion(index, crystalItem);
            }
        }

        craftingCoroutine = null;
        currentCrystal = null;
    }

    private bool IsCrystalStillInInputSlot(InventoryItem crystalItem)
    {
        for (int i = 0; i < inputSlot.transform.childCount; i++)
        {
            if (inputSlot.transform.GetChild(i) == crystalItem.transform)
                return true;
        }
        return false;
    }

    private void CreatePotion(int index, InventoryItem crystalItem)
    {
        var potionObj = Instantiate(potionPrefabs[index], outputSlot.transform);
        var potionItem = potionObj.GetComponent<InventoryItem>();
        if (potionItem != null)
        {
            potionItem.itemType = ItemType.Potion;
            potionItem.crystalType = crystalItem.crystalType;
        }
        Destroy(crystalItem.gameObject);
    }

    public void CancelCraftingIfCrystalRemoved(InventoryItem item)
    {
        if (craftingCoroutine != null && currentCrystal == item)
        {
            StopCoroutine(craftingCoroutine);
            craftingCoroutine = null;
            currentCrystal = null;
        }
    }
}
