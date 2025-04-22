using UnityEngine;

public class TeleportManager : MonoBehaviour
{
    [Tooltip("Reference na hráče – lze nechat prázdné, automaticky se najde při spuštění.")]
    public Transform player;

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

    public void TeleportPlayer(Vector3 newPosition)
    {
        if (player == null)
        {
            Debug.LogError("TeleportManager: Hráč není nastaven!");
            return;
        }

        CharacterController characterController = player.GetComponent<CharacterController>();

        if (characterController != null)
            characterController.enabled = false;

        player.position = newPosition;

        if (characterController != null)
            characterController.enabled = true;

        Debug.Log($"TeleportManager: Hráč byl teleportován na {newPosition}");
    }
}
