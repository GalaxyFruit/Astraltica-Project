using UnityEngine;
using UnityEditor;
using System;

public class ScriptFinder : EditorWindow
{
    private string scriptName = "SampleSceneLoader";

    [MenuItem("Tools/Find First Object With Script")]
    public static void ShowWindow()
    {
        GetWindow<ScriptFinder>("Script Finder");
    }

    void OnGUI()
    {
        GUILayout.Label("Find First Object By Script Name", EditorStyles.boldLabel);
        scriptName = EditorGUILayout.TextField("Script Name:", scriptName);

        if (GUILayout.Button("Find"))
        {
            FindFirstObject(scriptName);
        }
    }

    static void FindFirstObject(string scriptName)
    {
        Type scriptType = Type.GetType(scriptName);

        if (scriptType == null)
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                scriptType = assembly.GetType(scriptName);
                if (scriptType != null)
                    break;
            }
        }

        if (scriptType == null)
        {
            Debug.LogWarning($"Script '{scriptName}' not found.");
            return;
        }

        UnityEngine.Object found = GameObject.FindFirstObjectByType(scriptType);
        if (found != null)
        {
            Debug.Log("Found on: " + ((Component)found).gameObject.name, ((Component)found).gameObject);
            Selection.activeGameObject = ((Component)found).gameObject;
        }
        else
        {
            Debug.LogWarning("No object found with script: " + scriptName);
        }
    }
}
