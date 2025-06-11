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

    private InventoryItem crystalItem;

    public void TryCraftPotion()
    {
        if (inputSlot.transform.childCount <= 1 || outputSlot.transform.childCount > 1)
            return;

        crystalItem = null;
        for (int i = 0; i < inputSlot.transform.childCount; i++)
        {
            var item = inputSlot.transform.GetChild(i).GetComponent<InventoryItem>();
            if (item != null && item.itemType == ItemType.Crystal)
            {
                crystalItem = item;
                break;
            }
        }
        if (crystalItem == null)
            return;

        StartCoroutine(CraftingCoroutine());
    }

    private IEnumerator CraftingCoroutine()
    {
        yield return new WaitForSeconds(craftingDuration);
        for (int i = 0; i < crystalTypes.Length; i++)
        {
            if (crystalTypes[i] == crystalItem.crystalType && i < potionPrefabs.Length && potionPrefabs[i] != null)
            {
                var potionObj = Instantiate(potionPrefabs[i], outputSlot.transform);
                var potionItem = potionObj.GetComponent<InventoryItem>();
                if (potionItem != null)
                {
                    potionItem.itemType = ItemType.Potion;
                    potionItem.crystalType = crystalItem.crystalType;
                }
                Destroy(crystalItem.gameObject);
                break;
            }
        }
    }
}