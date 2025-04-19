using UnityEngine;

public class Note : MonoBehaviour, IInteractable
{
    public string noteText;

    public void Interact()
    {
        Debug.Log("Reading note: " + noteText);
    }

    public string GetInteractionText()
    {
        return "<b>[E]</b> to read note";
    }
}
