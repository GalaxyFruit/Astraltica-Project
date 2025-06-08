using UnityEngine;

public class PotionCraftingManager : MonoBehaviour
{
    [Header("Crystal to Potion Prefabs")]
    [SerializeField] private CrystalType[] crystalTypes;
    [SerializeField] private GameObject[] potionPrefabs;

    [Header("Crafting Slots")]
    [SerializeField] private PotionSlot inputSlot;
    [SerializeField] private PotionSlot outputSlot;

    public void TryCraftPotion()
    {
        if (inputSlot.transform.childCount <= 1 || outputSlot.transform.childCount > 1)
            return;

        var crystalItem = inputSlot.transform.GetChild(1).GetComponent<InventoryItem>();
        if (crystalItem == null || crystalItem.itemType != ItemType.Crystal)
            return;

        for (int i = 0; i < crystalTypes.Length; i++)
        {
            if (crystalTypes[i] == crystalItem.crystalType && i < potionPrefabs.Length && potionPrefabs[i] != null)
            {
                // Spawn potion prefab do output slotu
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