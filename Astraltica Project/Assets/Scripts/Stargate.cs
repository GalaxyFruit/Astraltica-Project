using UnityEngine;
using UnityEngine.Splines;

public class Stargate : MonoBehaviour
{
    public bool evacuationReady = false;

    private PlayerInputManager playerInputManager;
    public GameObject player;

    private void Start()
    {
        // v budoucnu bude scriptem od jinud
        evacuationReady = true;

        playerInputManager = FindFirstObjectByType<PlayerInputManager>();
    }

    public void DisableInputs()
    {
        CharacterController characterController = player.GetComponent<CharacterController>();
        if (characterController != null)
            characterController.enabled = false;

        playerInputManager.DisableInputs();
    }

    public void EnableInputs()
    {
        playerInputManager.EnableInputs();
    }
}


