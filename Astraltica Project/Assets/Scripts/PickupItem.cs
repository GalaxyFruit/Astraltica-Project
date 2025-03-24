using UnityEngine;

public class PickupItem : MonoBehaviour
{
    [Header("General Properties")]
    public string itemName;
    public Sprite itemIcon;
    public GameObject itemPrefab;
    public bool canEquipToHand = false;

    [Header("Item Type")]
    public ItemType itemType;

    [Header("Crystal Type (Only for Crystal Items)")]
    public CrystalType crystalType;

    public void Pickup()
    {
        Destroy(gameObject);
    }
}

public enum ItemType
{
    Crystal,
    Weapon,
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
