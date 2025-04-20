using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;

public class NoteDisplayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _noteTextUI;
    [SerializeField] private PlayerInputManager _playerInputManager;

    private bool isNoteOpen = false;

    private void Start()
    {
        _playerInputManager = FindFirstObjectByType<PlayerInputManager>();
    }

    public void ShowNote(string noteText)
    {
        if (isNoteOpen) return;
        if (_noteTextUI == null)
        {
            Debug.LogError("Note Text UI is not assigned!");
            return;
        }

        _noteTextUI.text = noteText;
        isNoteOpen = true;

        _playerInputManager.DisableInputs();
        GameManager.Instance.SetGameState(GameManager.GameState.Paused);
    }

    public void HideNote()
    {
        if (!isNoteOpen) return;

        _noteTextUI.text = "";
        isNoteOpen = false;

        _playerInputManager.EnableInputs();
        GameManager.Instance.SetGameState(GameManager.GameState.Playing);
    }

    public void OnCloseNote(InputAction.CallbackContext context)
    {
        if (context.performed && isNoteOpen)
        {
            HideNote();
        }
    }

    public bool IsNoteOpen => isNoteOpen;
}
