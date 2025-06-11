using UnityEngine;

public class PickupItem : MonoBehaviour, IInteractable
{
    [Header("General Properties")]
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPrefab;
    public bool canEquipToHand = false;

    [Header("Item Type")]
    public ItemType itemType;

    [Header("Keycard Type IF")]
    public KeycardType keycardType;

    [Header("Crystal Type (Only for Crystal Items)")]
    public CrystalType crystalType;

    public void Interact()
    {
        if (FindFirstObjectByType<ItemPickupController>().TryPickup(this))
        {
            Destroy(gameObject);
        }
    }

    public string GetInteractionText()
    {
        return $"<b>[E]</b> to pick up\n<color=#FFA500>{itemName}</color>";
    }
}


public enum ItemType
{
    Crystal,
    Weapon,
    Potion,
    General
}

public enum CrystalType
{
    None,
    Amethyst,
    Cobalt,
    Emerald,
    Honey,
    Ice,
    Monolith,
    Pinkie,
    Ruby
}
