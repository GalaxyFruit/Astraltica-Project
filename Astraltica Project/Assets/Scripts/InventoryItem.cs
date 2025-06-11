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
        image.raycastTarget = false;
        if (secondaryImage != null && secondaryImage.sprite != null)
            secondaryImage.raycastTarget = false;

        parentAfterDrag = transform.parent;
        transform.SetParent(canvas.transform);
        transform.SetAsLastSibling();

        PotionSlot parentSlot;
        if (parentAfterDrag != null && parentAfterDrag.TryGetComponent<PotionSlot>(out parentSlot))
        {
            if (parentSlot.slotType == PotionSlotType.Input)
            {
                var craftingManager = parentSlot.craftingManager;
                if (craftingManager != null && craftingManager.CurrentCraftingCrystal == this)
                {
                    craftingManager.CancelCraftingIfCrystalRemoved(this);
                }
            }
        }
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

        if (transform.parent == canvas.transform)
        {
            transform.SetParent(parentAfterDrag);
            transform.localPosition = Vector3.zero;
        }

        if (parentAfterDrag.CompareTag("HotbarSlot"))
        {
            Hotbar hotbar = parentAfterDrag.GetComponentInParent<Hotbar>();
            if (hotbar != null)
            {
                StartCoroutine(DelayedEquip(hotbar));
            }
            else
            {
                Debug.LogWarning($"Hotbar nenalezen v parentu hotbar slotu jmeno: ${parentAfterDrag.gameObject.name}!");
            }
        }
    }


    private IEnumerator DelayedEquip(Hotbar hotbar)
    {
        yield return null; 
        hotbar.EquipSelectedItem();
    }
}
