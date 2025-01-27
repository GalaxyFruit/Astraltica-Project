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
            if (slot.childCount > 0)
            {
                GameObject itemToEquip = slot.GetChild(1).gameObject;
                PickupItem pickupItem = itemToEquip.GetComponent<PickupItem>();

                if (pickupItem != null && pickupItem.canEquipToHand)
                {
                    if (equippedItem != null) Destroy(equippedItem);

                    equippedItem = Instantiate(pickupItem.itemPrefab, playerHand);
                    equippedItem.transform.localPosition = Vector3.zero;
                    equippedItem.transform.localRotation = Quaternion.identity;
                    equippedItem.transform.localScale = Vector3.one;
                }
            }
            else
            {
                if (equippedItem != null)
                {
                    Destroy(equippedItem);
                    equippedItem = null;
                }
            }
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
