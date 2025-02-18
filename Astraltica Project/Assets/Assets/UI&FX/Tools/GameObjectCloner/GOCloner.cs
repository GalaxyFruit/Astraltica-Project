#if UNITY_EDITOR

using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;

class GOCloner : EditorWindow {
    private List<bool> selectedAxis = new List<bool>() { false, false, false };
    private List<bool> selectedDirections = new List<bool>() { true, true, true };
    private List<string> numberOfClonesStr = new List<string>() { "0", "0", "0" };
    private List<string> distanceBetweenClonesStr = new List<string>() { "0.0", "0.0", "0.0" };
    private bool fillIn = true;
    private Stack<List<GameObject>> cloneHistory = new Stack<List<GameObject>>();
    private GUIStyle ImportantNoteStyle = new GUIStyle();

    [MenuItem("Window/GameObject Cloner")]
    static void ShowWindow() {
        EditorWindow.GetWindow(typeof(GOCloner));
    }

    void OnGUI() {
        
        ImportantNoteStyle.normal.textColor = Color.red;
        ImportantNoteStyle.wordWrap = true;

        EditorGUILayout.BeginVertical();
        GUILayout.Label("Select Axis:", EditorStyles.boldLabel);

        EditorGUILayout.BeginHorizontal();
        selectedAxis[0] = GUILayout.Toggle(selectedAxis[0], "X", "Button");
        selectedAxis[1] = GUILayout.Toggle(selectedAxis[1], "Y", "Button");
        selectedAxis[2] = GUILayout.Toggle(selectedAxis[2], "Z", "Button");
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Select Direction:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = selectedAxis[0];
        selectedDirections[0] = GUILayout.Toggle(selectedDirections[0], selectedDirections[0] ? "+ve" : "-ve", "Button");
        GUI.enabled = selectedAxis[1];
        selectedDirections[1] = GUILayout.Toggle(selectedDirections[1], selectedDirections[1] ? "+ve" : "-ve", "Button");
        GUI.enabled = selectedAxis[2];
        selectedDirections[2] = GUILayout.Toggle(selectedDirections[2], selectedDirections[2] ? "+ve" : "-ve", "Button");
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        GUILayout.Label("Number Of Clones:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = selectedAxis[0];
        numberOfClonesStr[0] = GUILayout.TextField(numberOfClonesStr[0]);
        GUI.enabled = selectedAxis[1];
        numberOfClonesStr[1] = GUILayout.TextField(numberOfClonesStr[1]);
        GUI.enabled = selectedAxis[2];
        numberOfClonesStr[2] = GUILayout.TextField(numberOfClonesStr[2]);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();
        

        GUILayout.Label("Distance Between Clones:", EditorStyles.boldLabel);
        EditorGUILayout.BeginHorizontal();
        GUI.enabled = selectedAxis[0];
        distanceBetweenClonesStr[0] = GUILayout.TextField(distanceBetweenClonesStr[0]);
        GUI.enabled = selectedAxis[1];
        distanceBetweenClonesStr[1] = GUILayout.TextField(distanceBetweenClonesStr[1]);
        GUI.enabled = selectedAxis[2];
        distanceBetweenClonesStr[2] = GUILayout.TextField(distanceBetweenClonesStr[2]);
        GUI.enabled = true;
        EditorGUILayout.EndHorizontal();

        GUI.enabled = IsMultipleAxisSelected();
        fillIn = GUILayout.Toggle(fillIn, "Fill Area");

        bool isLocalAndHierarchySelected = IsLocalAndHierarchySelected();
        GUI.enabled = !isLocalAndHierarchySelected;

        if (GUILayout.Button("Clone Selected")) {
            CloneSelected();
        }

        if (isLocalAndHierarchySelected)
        {
            EditorStyles.label.wordWrap = true;
            GUILayout.Label(" Local Space Cloning not supported for hierarchies. Please switch to Global Space mode in the editor.", ImportantNoteStyle);
        }

        GUI.enabled = cloneHistory.Count > 0;
        if (GUILayout.Button("Undo Last Clone")) {
            UndoClone();
        }
        GUI.enabled = true;

        EditorGUILayout.EndVertical();
    }

    private void CloneSelected() {
        List<GameObject> recentCloned = new List<GameObject>();

        UnityEngine.Object[] selected = Selection.objects;

        List<GameObject> currentCloned = new List<GameObject>();
        bool firstAxis = true;

        for (int axis = 0; axis < selectedAxis.Count; axis++)
        {
            if (!selectedAxis[axis])
                continue;

            int numberOfClones = 0;
            Int32.TryParse(numberOfClonesStr[axis], out numberOfClones);

            float distanceBetweenClones = 0;
            float.TryParse(distanceBetweenClonesStr[axis], out distanceBetweenClones);

            if (firstAxis || !fillIn)
            {
                foreach (GameObject g in selected)
                {
                    Transform tile = g.transform;

                    float size = GetSize(tile, axis);
                    List<GameObject> currentAxisCloned = Clone(tile, numberOfClones, distanceBetweenClones, axis, size);
                    currentCloned.AddRange(currentAxisCloned);
                }
            }
            else
            {
                List<GameObject> tempCloned = new List<GameObject>(currentCloned);
                foreach (GameObject g in selected)
                {
                    tempCloned.Add(g);
                }

                foreach (GameObject g in tempCloned)
                {
                    Transform tile = g.transform;
                    float size = GetSize(tile, axis);
                    List<GameObject> currentAxisCloned = Clone(tile, numberOfClones, distanceBetweenClones, axis, size);
                    currentCloned.AddRange(currentAxisCloned);
                }
            }

            firstAxis = false;
            recentCloned.AddRange(currentCloned);
        }

        cloneHistory.Push(recentCloned);
    }

    private List<GameObject> Clone(Transform tile, int numberOfClones, float distanceBetweenClones, int axis, float axisSize)
    {
        List<GameObject> cloned = new List<GameObject>();

        for (int i = 0; i < numberOfClones; i++)
        {
            Transform d = Instantiate(tile, null);
            d.name = tile.name + "_" + i + (selectedDirections[axis] ? "+" : "-") + (axis == 0 ? "x" : axis == 1 ? "y" : "z");
            cloned.Add(d.gameObject);
            
            if (Tools.pivotRotation == PivotRotation.Local)
            {
                d.SetParent(tile);

                int dirMultiplier = selectedDirections[axis] ? 1 : -1;

                float offset = dirMultiplier * ((i + 1) * (distanceBetweenClones + axisSize));
                d.localPosition = new Vector3((axis == 0 ? offset : 0),
                    (axis == 1 ? offset : 0),
                    (axis == 2 ? offset : 0));

                d.localEulerAngles = Vector3.zero;
                d.SetParent(tile.parent);
            }
            else
            {
                d.position = tile.position;
                d.rotation = tile.rotation;
                int dirMultiplier = selectedDirections[axis] ? 1 : -1;

                float offset = dirMultiplier * ((i + 1) * (distanceBetweenClones + axisSize));
                d.position += new Vector3((axis == 0 ? offset : 0),
                    (axis == 1 ? offset : 0),
                    (axis == 2 ? offset : 0));
                d.SetParent(tile.parent);
            }

            d.localScale = tile.localScale;
        }

        return cloned;
    }


    private float GetPos(Transform d, int j)
    {
        float pos = 0;
        if (UnityEditor.Tools.pivotRotation == PivotRotation.Local)
        {
            pos = j == 0 ? d.localPosition.x : j == 1 ? d.localPosition.y : d.localPosition.z;
        }
        else
        {
            var mrenderer = d.GetComponent<Renderer>();
            pos = j == 0 ? mrenderer.bounds.center.x : j == 1 ? mrenderer.bounds.center.y : mrenderer.bounds.center.z;
        }

        return pos;
    }

    private float GetSize(Transform d, int axis)
    {
        float size = 0;
        if (IsHierarchy(d))
        {
            float maxPos = float.MinValue;
            float minPos = float.MaxValue;
            bool hasAClonableChild = false;

            foreach (Transform child in d.GetComponentsInChildren<Transform>())
            {
                if (IsClonable(child))
                {
                    hasAClonableChild = true;
                    float childSize = GetTransformSize(child, axis, true);
                    float childPos = GetPos(child, axis);
                    minPos = Math.Min(minPos, childPos - (childSize / 2));
                    maxPos = Math.Max(maxPos, childPos + (childSize / 2));
                }
            }

            size = hasAClonableChild ? maxPos - minPos : 0f;
        }
        else
        {
            size = GetTransformSize(d, axis, false);
        }

        return size;
    }

    private bool IsLocalAndHierarchySelected()
    {
        bool isLocal = UnityEditor.Tools.pivotRotation == PivotRotation.Local;
        bool isHierarchySelected = false;

        UnityEngine.Object[] selected = Selection.objects;
        foreach (UnityEngine.Object go in selected)
        {
            if (go is GameObject && IsHierarchy(((GameObject)go).transform))
            {
                isHierarchySelected = true;        
                break;
            }
        }

        return isHierarchySelected && isLocal;
    }

    private bool IsHierarchy(Transform d)
    {
        return d.GetComponentsInChildren<Transform>().Length > 1;
    }

    private float GetTransformSize(Transform d, int j, bool trueSize)
    {
        float size = 0;

        if (UnityEditor.Tools.pivotRotation == PivotRotation.Local)
        {
            var mf = d.GetComponent<MeshFilter>();

            if (mf != null && mf.sharedMesh != null)
            {
                Mesh mesh = mf.sharedMesh;
                size = j == 0 ? mesh.bounds.size.x * (trueSize ? d.localScale.x : 1)
                    : j == 1 ? mesh.bounds.size.y * (trueSize ? d.localScale.y : 1)
                    : mesh.bounds.size.z * (trueSize ? d.localScale.z : 1);
            }
        }
        else
        {
            var mrenderer = d.GetComponent<Renderer>();

            if (mrenderer != null)
            {
                size = j == 0 ? mrenderer.bounds.size.x : j == 1 ? mrenderer.bounds.size.y : mrenderer.bounds.size.z;
            }
        }

        return size;
    }

    private bool IsMultipleAxisSelected()
    {
        return (selectedAxis[0] ? 1 : 0) + (selectedAxis[1] ? 1 : 0) + (selectedAxis[2] ? 1 : 0) > 1;
    }

    private bool IsClonable(Transform d)
    {
        var mf = d.GetComponent<MeshFilter>();
        if (UnityEditor.Tools.pivotRotation == PivotRotation.Local)
        {
            return (mf != null && mf.sharedMesh != null);
        }
        else
        {
            var mrenderer = d.GetComponent<Renderer>();
            return mrenderer != null && mf != null && mf.sharedMesh != null;
        }
    }

    private void UndoClone() {
        List<GameObject> latestHistory = cloneHistory.Pop();
        foreach (GameObject g in latestHistory) {
            SafeDestroy(g);
        }

        latestHistory.Clear();
    }

    public T SafeDestroy<T>(T obj) where T : UnityEngine.Object {
        UnityEngine.Object.DestroyImmediate(obj);
        return null;
    }
}

#endif