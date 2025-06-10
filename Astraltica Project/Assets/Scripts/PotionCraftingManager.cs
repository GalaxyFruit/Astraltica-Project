using UnityEngine;
using System.Collections;

public class PotionCraftingManager : MonoBehaviour
{
    [Header("Crystal to Potion Prefabs")]
    [SerializeField] private CrystalType[] crystalTypes;
    [SerializeField] private GameObject[] potionPrefabs;

    [Header("Crafting Slots")]
    [SerializeField] private PotionSlot inputSlot;
    [SerializeField] private PotionSlot outputSlot;

    [Header("Crafting Settings")]
    [SerializeField] private float craftingDuration = 2f;

    public InventoryItem CurrentCraftingCrystal => currentCrystal;

    private Coroutine craftingCoroutine;
    private InventoryItem currentCrystal;

    public void TryCraftPotion()
    {
        if (craftingCoroutine != null)
            return;

        if (inputSlot.transform.childCount <= 1 || outputSlot.transform.childCount > 1)
            return;

        var crystalItem = inputSlot.transform.GetChild(1).GetComponent<InventoryItem>();
        if (crystalItem == null || crystalItem.itemType != ItemType.Crystal)
            return;

        for (int i = 0; i < crystalTypes.Length; i++)
        {
            if (crystalTypes[i] == crystalItem.crystalType && i < potionPrefabs.Length && potionPrefabs[i] != null)
            {
                currentCrystal = crystalItem;
                craftingCoroutine = StartCoroutine(CraftPotionCoroutine(i, crystalItem));
                break;
            }
        }
    }

    private IEnumerator CraftPotionCoroutine(int index, InventoryItem crystalItem)
    {
        yield return new WaitForSeconds(craftingDuration);

        // Check if the crystal is still in the input slot
        if (inputSlot.transform.childCount > 1 && inputSlot.transform.GetChild(1) == crystalItem.transform)
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

        craftingCoroutine = null;
        currentCrystal = null;
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