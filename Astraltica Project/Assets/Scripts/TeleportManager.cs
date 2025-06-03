using System.Collections;
using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [Tooltip("Reference na hráče – lze nechat prázdné, automaticky se najde při spuštění.")]
    public Transform player;

    [Header("Animation Settings")]
    public Animator animator;
    public float animationDelay = 0.5f;

    private PlayerInputManager _playerInputManager;
    private StormEffectsManager _stormEffectsManager;
    private bool _isInOxygenZone = false;

    public bool IsInOxygenZone => _isInOxygenZone;

    private void Awake()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
            {
                player = playerObj.transform;
            }
            else
            {
                Debug.LogError("TeleportManager: Hráč s tagem 'Player' nebyl nalezen!");
            }
        }
    }

    private void Start()
    {
        _playerInputManager = FindFirstObjectByType<PlayerInputManager>();
        _stormEffectsManager = FindFirstObjectByType<StormEffectsManager>();

        if (_playerInputManager == null)
        {
            Debug.LogError("TeleportManager: PlayerInputManager nebyl nalezen!");
        }
    }

    public void TeleportPlayer(Vector3 newPosition)
    {
        if (player == null)
        {
            Debug.LogError("TeleportManager: Hráč není nastaven!");
            return;
        }

        if (animator)
        {
            StartCoroutine(TeleportWithAnimation(newPosition));
        }
        else
        {
            PerformTeleport(newPosition);
        }
    }

    private IEnumerator TeleportWithAnimation(Vector3 newPosition)
    {
        animator.SetBool("Active", true);
        _playerInputManager?.DisableInputs();
        yield return new WaitForSeconds(animationDelay);
        PerformTeleport(newPosition);
        _playerInputManager?.EnableInputs();
        animator.SetBool("Active", false);

        HandleOxygenZoneTransition();
    }

    private void HandleOxygenZoneTransition()
    {
        if (!_isInOxygenZone)
        {
            OxygenManager.Instance?.EnterOxygenZone();
            _stormEffectsManager?.HideStormEffects();
            _isInOxygenZone = true;
        }
        else
        {
            OxygenManager.Instance?.ExitOxygenZone();

            if (_stormEffectsManager?.IsStormActive == true)
                _stormEffectsManager?.ShowStormEffects();

            _isInOxygenZone = false;
        }
    }

    private void PerformTeleport(Vector3 newPosition)
    {
        CharacterController characterController = player.GetComponent<CharacterController>();
        if (characterController != null) characterController.enabled = false;

        player.position = newPosition;

        if (characterController != null) characterController.enabled = true;
    }
}