using UnityEngine;

public class PickupItem : MonoBehaviour
{
    public string itemName; 
    public Sprite itemIcon;

    public void Pickup()
    {
        Destroy(gameObject);
    }
}
