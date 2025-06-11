using UnityEngine;
using UnityEngine.UI;

public class Hotbar : MonoBehaviour
{
    [Header("Hotbar Slots")]
    [SerializeField] private GameObject[] hotbarSlots;

    [Header("Default Selected Slot")]
    [SerializeField] private int defaultSlotIndex = 0;

    [SerializeField] private Transform playerHand;
    [SerializeField] private WeaponController weaponController;
    [SerializeField] private PlayerIKController ikController;

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

            if (slot.childCount > 1)
            {
                GameObject itemToEquip = slot.GetChild(1).gameObject; 
                InventoryItem inventoryItem = itemToEquip.GetComponent<InventoryItem>();

                if (inventoryItem != null && inventoryItem.canEquipToHand)
                {

                    if (equippedItem != null)
                        Destroy(equippedItem);

                    equippedItem = Instantiate(inventoryItem.itemPrefab, playerHand); //Zbran
                    equippedItem.transform.localPosition = Vector3.zero;
                    equippedItem.transform.localRotation = Quaternion.identity;
                    equippedItem.transform.localScale = Vector3.one;

                    Rigidbody rig = equippedItem.GetComponent<Rigidbody>();
                    if (rig != null)
                    {
                        Destroy(rig);
                    }

                    //Transform rightPistolGrip = equippedItem.transform.Find("RightPistolGrip");
                    //ikController.SetRightHandIK(rightPistolGrip);

                    weaponController.EquipWeapon(equippedItem.transform);

                    return;
                }
            }
        }

        if (equippedItem != null)
        {
            Destroy(equippedItem);
            equippedItem = null;
            ikController.ToggleIK(false);
            weaponController.UnequipWeapon();
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
        Transform Content = slot.transform.Find("Content");
        if (Content != null)
        {
            Transform Frame = Content.Find("Frame");
            if (Frame != null)
            {
                Transform Selected = Frame.Find("Selected");
                if(Selected != null)
                {
                    Selected.gameObject.SetActive(highlight);
                }
                else
                {
                    Debug.LogWarning("Selected not found in Frame of slot: " + slot.name);
                }
            }
            else
            {
                Debug.LogWarning("Frame not found in Content of slot: " + slot.name);
            }
        }
        else
        {
            Debug.LogWarning("Content not found in slot: " + slot.name);
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
