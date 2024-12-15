using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventoryItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [Header("UI")]
    public Image image;

    private Transform parentAfterDrag;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Upravíme UI pro dragování
        image.raycastTarget = false;
        canvasGroup.alpha = 0.6f; // Průhlednost

        parentAfterDrag = transform.parent; // Uložení původního rodiče
        transform.SetParent(transform.root); // Přesun na nejvyšší úroveň UI
        transform.SetAsLastSibling(); // Zajistí, že je položka nahoře
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Následuje pozici myši
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        image.raycastTarget = true;
        canvasGroup.alpha = 1.0f;

        // Ověříme, zda je položka nad platným slotem
        if (eventData.pointerEnter != null && eventData.pointerEnter.CompareTag("Slot"))
        {
            // Přesun položky do nového slotu
            transform.SetParent(eventData.pointerEnter.transform);
        }
        else
        {
            // Vrácení položky na původní pozici
            transform.SetParent(parentAfterDrag);
        }

        transform.localPosition = Vector3.zero; // Zarovnání na střed slotu
    }
}
