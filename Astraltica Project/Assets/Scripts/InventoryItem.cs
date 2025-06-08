using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;
    public Image secondaryImage;

    [HideInInspector] public Transform parentAfterDrag;

    private Canvas canvas;

    [Header("Item Properties")]
    public string itemName;
    public GameObject itemPrefab;
    public ItemType itemType;
    public CrystalType crystalType;
    public KeycardType keycardType;
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
        //Debug.Log("OnBeginDrag called for " + gameObject.name);
        image.raycastTarget = false;

        if (secondaryImage != null && secondaryImage.sprite != null)
        {
            secondaryImage.raycastTarget = false;
        }

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

        if (secondaryImage != null && secondaryImage.sprite != null)
        {
            secondaryImage.raycastTarget = true;
        }

        transform.SetParent(parentAfterDrag);


        if (parentAfterDrag.CompareTag("HotbarSlot"))
        {
            //Debug.Log("hotbar slot se našel");
            Hotbar hotbar = parentAfterDrag.GetComponentInParent<Hotbar>();
            if (hotbar != null)
            {
                StartCoroutine(DelayedEquip(hotbar)); 
            } else
            {
                Debug.LogWarning($"Hotbar nenalezen v parentu hotbar slotu jmeno: ${parentAfterDrag.gameObject.name}!");
            }
        } else
        {
            //Debug.Log("Item was dropped outside of hotbar slot.");
        }
    }

    private IEnumerator DelayedEquip(Hotbar hotbar)
    {
        yield return null; 
        hotbar.EquipSelectedItem();
    }
}
