using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;

    [HideInInspector] public Transform parentAfterDrag;
    private Canvas canvas;

    [Header("Item Properties")]
    public string itemName;
    public GameObject itemPrefab; 
    public ItemType itemType;
    public bool canEquipToHand;

    private void Start()
    {
        canvas = GetComponentInParent<Canvas>();
        if (canvas == null)
        {
            Debug.LogError("Canvas nenalezen v InventoryItem scriptu!");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        image.raycastTarget = false;
        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        transform.SetParent(parentAfterDrag);

        if (parentAfterDrag.CompareTag("HotbarSlot"))
        {
            Hotbar hotbar = parentAfterDrag.GetComponentInParent<Hotbar>();
            if (hotbar != null)
            {
                StartCoroutine(DelayedEquip(hotbar)); 
            }
        }
    }

    private IEnumerator DelayedEquip(Hotbar hotbar)
    {
        yield return null; 
        hotbar.EquipSelectedItem();
    }
}
