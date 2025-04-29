using UnityEngine;

public enum TeleportDirection
{
    North, South, East, West
}

public class TeleportInteractable : MonoBehaviour, IInteractable
{
    [Header("Teleport Settings")]
    public TeleportInteractable linkedTeleport;
    public TeleportDirection direction = TeleportDirection.North;
    public bool reverseDirection = true;
    public float offset = 0.5f;
    public float cooldownTime = 1f;

    private bool isOnCooldown = false;

    public void Interact()
    {
        if (isOnCooldown)
        {
            Debug.Log($"{name}: Teleport je na cooldownu.");
            return;
        }

        if (linkedTeleport == null)
        {
            Debug.LogWarning($"{name}: Chybí linkedTeleport!");
            return;
        }

        Vector3 dir = DirectionToVector(linkedTeleport.direction); //vezmeme si směr z druhého teleport místa
        Vector3 targetPosition = linkedTeleport.transform.position + dir * linkedTeleport.offset; //transform 2. místa + smer * offset

        TeleportManager teleportManager = FindFirstObjectByType<TeleportManager>();
        if (teleportManager == null)
        {
            Debug.LogError("TeleportManager nebyl nalezen ve scéně!");
            return;
        }
        teleportManager.TeleportPlayer(targetPosition);
        StartCooldown();
        linkedTeleport.StartCooldown();
    }

    public string GetInteractionText()
    {
        return "Press E to use";
    }

    private Vector3 DirectionToVector(TeleportDirection dir)
    {
        return dir switch
        {
            TeleportDirection.North => Vector3.forward,
            TeleportDirection.South => Vector3.back,
            TeleportDirection.East => Vector3.right,
            TeleportDirection.West => Vector3.left,
            _ => Vector3.zero,
        };
    }

    private void StartCooldown()
    {
        isOnCooldown = true;
        Invoke(nameof(ResetCooldown), cooldownTime);
    }

    private void ResetCooldown()
    {
        isOnCooldown = false;
    }
}
