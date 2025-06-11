using UnityEngine;

public class Note : MonoBehaviour, IInteractable
{
    [TextArea(5, 30)] public string noteText;

    [Header("Optional: Complete this task on interact")]
    public string completeTaskID;

    private NoteDisplayController _noteDisplayController;
    private EmergencyProtocolManager _emergencyProtocolManager;

    public void Interact()
    {
        _noteDisplayController = FindFirstObjectByType<NoteDisplayController>();
        _noteDisplayController.ShowNote(noteText);

        if (!string.IsNullOrEmpty(completeTaskID))
        {
            if (_emergencyProtocolManager == null)
                _emergencyProtocolManager = FindFirstObjectByType<EmergencyProtocolManager>();

            _emergencyProtocolManager?.CompleteTask(completeTaskID);
        }
    }

    public string GetInteractionText()
    {
        return "<b>[E]</b> View/Close Data";
    }
}