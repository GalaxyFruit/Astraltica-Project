using UnityEngine;
using UnityEngine.Splines;

public class Stargate : MonoBehaviour
{
    [Header("Stargate Settings")]
    [SerializeField] private GameObject Vortex;

    public bool evacuationReady = false;

    private PlayerInputManager playerInputManager;

    private void Start()
    {
        playerInputManager = FindFirstObjectByType<PlayerInputManager>();

        if (playerInputManager == null)
        {
            Debug.LogError("PlayerInputManager not found in the scene.");
        }

        if (Vortex == null)
        {
            Debug.LogError("Vortex GameObject is not assigned in the inspector.");
            Debug.LogWarning($"name: {gameObject.name}.");
        }
        else
        {
            Vortex.SetActive(false);
        }
    }

    public void ActivateEvacuationVortex()
    {
        evacuationReady = true;

        Vortex.SetActive(true);

        //if (!Vortex.TryGetComponent<Animator>(out var animator))
        //{
        //    Debug.LogWarning("No animator attached to the task object.");
        //}
    }

    public void DisableInputs()
    {

        playerInputManager.DisableInputs();
    }
}


