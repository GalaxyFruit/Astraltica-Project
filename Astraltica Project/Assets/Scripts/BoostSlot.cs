using UnityEngine.EventSystems;
using UnityEngine;

public class BoostSlot : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        if (transform.childCount >= 2)
        {
            Debug.Log("Slot je plný!");
            return;
        }

        InventoryItem item = eventData.pointerDrag.GetComponent<InventoryItem>();
        if (item == null || item.itemType != ItemType.Crystal) return;

        if (BoostManager.Instance.CanAddCrystal(item.crystalType))
        {
            BoostManager.Instance.AddCrystal(item.crystalType);
            Destroy(item.gameObject);
            BoostManager.Instance.StartDepleting();
        }
        else
        {
            item.parentAfterDrag = transform;
            StartCoroutine(CheckWaitingCrystal(item.gameObject, item.crystalType));
        }
    }

    private System.Collections.IEnumerator CheckWaitingCrystal(GameObject crystal, CrystalType crystalType)
    {
        while (crystal != null)
        {
            yield return new WaitForSeconds(0.25f);

            if (!crystal.transform.IsChildOf(transform))
            {
                break;
            }

            if (BoostManager.Instance.CanAddCrystal(crystalType))
            {
                BoostManager.Instance.AddCrystal(crystalType);
                Destroy(crystal);
                BoostManager.Instance.StartDepleting();
                break;
            }
        }
    }
}