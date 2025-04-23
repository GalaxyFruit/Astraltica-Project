#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(TeleportInteractable))]
public class TeleportLinkerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TeleportInteractable teleport = (TeleportInteractable)target;

        if (teleport.linkedTeleport != null && teleport.linkedTeleport.linkedTeleport != teleport)
        {
            if (GUILayout.Button("↔ Propojit obousměrně"))
            {
                Undo.RecordObject(teleport.linkedTeleport, "Link zpětně");
                teleport.linkedTeleport.linkedTeleport = teleport;

                if (teleport.reverseDirection)
                {
                    teleport.linkedTeleport.direction = ReverseDirection(teleport.direction);
                }
                else
                {
                    teleport.linkedTeleport.direction = teleport.direction; // Neobracíme směr
                }

                EditorUtility.SetDirty(teleport.linkedTeleport);
            }
        }
    }

    private TeleportDirection ReverseDirection(TeleportDirection dir)
    {
        return dir switch
        {
            TeleportDirection.North => TeleportDirection.South,
            TeleportDirection.South => TeleportDirection.North,
            TeleportDirection.East => TeleportDirection.West,
            TeleportDirection.West => TeleportDirection.East,
            _ => dir
        };
    }
}
#endif
