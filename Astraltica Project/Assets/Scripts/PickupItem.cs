using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName; 
    public Sprite itemIcon;
    public GameObject itemPrefab;
    public ItemType itemType;
    public bool canEquipToHand = false;

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
