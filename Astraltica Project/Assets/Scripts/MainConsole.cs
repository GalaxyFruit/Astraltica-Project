using UnityEngine;

public class MainConsole : MonoBehaviour, IInteractable
{

    public void Interact()
    {
        Debug.Log("MainConsole.Interact() is not implemented yet.");
    }


    public string GetInteractionText()
    {
        return "<b>[E]</b> Insert Keycards";
    }
}
