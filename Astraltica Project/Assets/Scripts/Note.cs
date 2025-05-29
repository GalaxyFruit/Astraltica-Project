using UnityEngine;

public class Note : MonoBehaviour, IInteractable
{
    [TextArea] public string noteText;

    private NoteDisplayController _noteDisplayController;

    public void Interact()
    {
        _noteDisplayController = FindFirstObjectByType<NoteDisplayController>();
        _noteDisplayController.ShowNote(noteText);
    }


    public string GetInteractionText()
    {
        return "<b>[E]</b> View/Close Data";
    }
}
