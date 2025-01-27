using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName; 
    public Sprite itemIcon;
    public GameObject itemPrefab;
    public bool canEquipToHand = false;

    public void Pickup()
    {
        Destroy(gameObject);
    }
}
