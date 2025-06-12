#if UNITY_EDITOR

using UnityEngine;
using UnityEditor;

public class SubmeshChecker : EditorWindow
{
    [MenuItem("Tools/Submesh Checker")]
    public static void ShowWindow()
    {
        GetWindow<SubmeshChecker>("Submesh Checker");
    }

    private void OnGUI()
    {
        if (GUILayout.Button("Check Scene for Submesh Issues"))
        {
            CheckSubmeshes();
        }
    }

    private void CheckSubmeshes()
    {
        GameObject[] allObjects = Object.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (GameObject go in allObjects)
        {
            MeshFilter mf = go.GetComponent<MeshFilter>();
            MeshRenderer mr = go.GetComponent<MeshRenderer>();

            if (mf != null && mr != null)
            {
                Mesh mesh = mf.sharedMesh;
                if (mesh != null)
                {
                    int subMeshCount = mesh.subMeshCount;
                    int materialCount = mr.sharedMaterials.Length;

                    if (materialCount > subMeshCount)
                    {
                        Debug.LogWarning($"GameObject '{go.name}' má {materialCount} materiálů, ale pouze {subMeshCount} submeshů.", go);
                    }
                }
            }
        }
    }
}

#endif
