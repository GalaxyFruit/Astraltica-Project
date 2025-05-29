using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

public class NoteDisplayController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _noteTextUI;
    [SerializeField] private GameObject _notePanel;
    [SerializeField] private PlayerInputManager _playerInputManager;

    private bool isNoteOpen = false;
    private bool canCloseNote = true;

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

        if (_notePanel == null)
        {
            Debug.LogError("Note Panel is not assigned!");
            return;
        }

        _notePanel.SetActive(true);

        _noteTextUI.text = noteText;
        isNoteOpen = true;

        _playerInputManager.DisableInputs();

        canCloseNote = false;
        StartCoroutine(CloseInputCooldown());
    }

    public void HideNote()
    {
        //if (!isNoteOpen) return;

        _noteTextUI.text = "";
        isNoteOpen = false;

        _notePanel.SetActive(false);

        _playerInputManager.EnableInputs();
        //GameManager.Instance.SetGameState(GameManager.GameState.Playing);
    }

    public void OnCloseNote(InputAction.CallbackContext context)
    {
        if (context.performed && isNoteOpen && canCloseNote)
        {
            HideNote();
        }
    }

    private IEnumerator CloseInputCooldown()
    {
        yield return new WaitForSeconds(0.25f);
        canCloseNote = true;
    }


    public bool IsNoteOpen => isNoteOpen;
}
