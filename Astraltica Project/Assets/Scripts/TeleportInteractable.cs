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

    [Header("Interaction Text")]
    public string enterText = "Press E to enter bunker";
    public string exitText = "Press E to exit bunker";

    [Tooltip("Vstup = true; jinak false")]
    public bool isEntrance = true;

    [Header("Bunker Info")]
    public ShelterType shelterType = ShelterType.ShelterAlpha;
    public string CompleteTaskID = "task_1";

    private EmergencyProtocolManager emergencyProtocolManager;
    private TeleportManager teleportManager;
    private bool isOnCooldown = false;

    private void Start()
    {
        emergencyProtocolManager = FindFirstObjectByType<EmergencyProtocolManager>();
        teleportManager = FindFirstObjectByType<TeleportManager>();

        if (emergencyProtocolManager == null)
        {
            Debug.LogError("EmergencyProtocolManager nebyl nalezen ve scéně!");
        }

        if (teleportManager == null)
        {
            Debug.LogError("TeleportManager nebyl nalezen ve scéně!");
        }
    }

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

        Vector3 dir = DirectionToVector(linkedTeleport.direction); 
        Vector3 targetPosition = linkedTeleport.transform.position + dir * linkedTeleport.offset; 
        teleportManager.TeleportPlayer(targetPosition);

        if (isEntrance)
        {
            if (!string.IsNullOrEmpty(CompleteTaskID))
            {
                emergencyProtocolManager.CompleteTask(CompleteTaskID);
            }
        }

        StartCooldown();
        linkedTeleport.StartCooldown();
    }

    public string GetInteractionText()
    {
        return isEntrance ? enterText : exitText;
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

public enum ShelterType
{
    ShelterAlpha,
    ShelterBeta,
    ShelterGamma
}
