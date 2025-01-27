using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [Header("Hotbar Slots")]
    [SerializeField] private GameObject[] hotbarSlots;

    [Header("Default Selected Slot")]
    [SerializeField] private int defaultSlotIndex = 0;

    [SerializeField] private Transform playerHand;

    private int currentSlotIndex = -1;
    private GameObject equippedItem;

    private void Start()
    {
        HighlightSlot(defaultSlotIndex);
    }

    public void EquipSelectedItem()
    {
        if (currentSlotIndex >= 0 && currentSlotIndex < hotbarSlots.Length)
        {
            Transform slot = hotbarSlots[currentSlotIndex].transform;

            // Pokud je v slotu item
            if (slot.childCount > 1)
            {
                GameObject itemToEquip = slot.GetChild(1).gameObject; // Předpokládá se, že první dítě je "Border"
                InventoryItem inventoryItem = itemToEquip.GetComponent<InventoryItem>();

                if (inventoryItem != null)
                {
                    PickupItem pickupItem = inventoryItem.GetComponent<PickupItem>();
                    if (pickupItem != null && pickupItem.canEquipToHand)
                    {
                        // Pokud něco už je v ruce, zničíme to
                        if (equippedItem != null)
                            Destroy(equippedItem);

                        // Zobrazíme novou instanci předmětu
                        equippedItem = Instantiate(pickupItem.itemPrefab, playerHand);
                        equippedItem.transform.localPosition = Vector3.zero;
                        equippedItem.transform.localRotation = Quaternion.identity;
                        equippedItem.transform.localScale = Vector3.one;

                        return;
                    }
                }
            }
        }

        // Pokud v aktuálním slotu nic není nebo vybavení nelze vzít do ruky
        if (equippedItem != null)
        {
            Destroy(equippedItem);
            equippedItem = null;
        }
    }


    public void HighlightSlot(int slotIndex)
    {
        int newIndex = slotIndex - 1;

        //Debug.Log("newIndex " + newIndex);

        if (newIndex < 0 || newIndex >= hotbarSlots.Length)
            return;

        if (currentSlotIndex >= 0 && currentSlotIndex < hotbarSlots.Length)
            SetSlotHighlight(hotbarSlots[currentSlotIndex], false);

        SetSlotHighlight(hotbarSlots[newIndex], true);

        currentSlotIndex = newIndex;
        EquipSelectedItem();
    }

    private void SetSlotHighlight(GameObject slot, bool highlight)
    {
        Transform border = slot.transform.Find("Border");

        if (border != null)
        {
            Image borderImage = border.GetComponent<Image>();
            if (borderImage != null)
            {
                borderImage.color = highlight ? Color.cyan : Color.white;
            }
        }
    }

    public void HighlightNextSlot()
    {
        int nextSlotIndex = (currentSlotIndex + 1) % hotbarSlots.Length;
        HighlightSlot(nextSlotIndex + 1);
    }

    public void HighlightPreviousSlot()
    {
        int previousSlotIndex = (currentSlotIndex - 1 + hotbarSlots.Length) % hotbarSlots.Length;
        HighlightSlot(previousSlotIndex + 1);
    }


}
