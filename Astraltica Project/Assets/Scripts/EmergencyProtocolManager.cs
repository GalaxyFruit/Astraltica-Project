#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EmergencyProtocolManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform taskListParent; 
    [SerializeField] private GameObject taskPrefab; 

    [Header("Task Settings")]
    [SerializeField] internal string NewTaskContent = "New Task";
    [SerializeField] internal string NewTaskDescription = "This is a new task description.";
    private List<GameObject> taskObjects = new(); 

    public void AddTask(string taskContext, string taskDescription)
    {
        if (taskPrefab == null || taskListParent == null)
        {
            Debug.LogError("Task Prefab or Task List Parent is not assigned");
            return;
        }


        GameObject newTask = Instantiate(taskPrefab, taskListParent);
        taskObjects.Add(newTask);


        TextMeshProUGUI label = newTask.transform.Find("Content/Text/Label_Objective")?.GetComponent<TextMeshProUGUI>();

        TextMeshProUGUI description = newTask.transform.Find("Content/Text/TextDescription/Label_Description")?.GetComponent<TextMeshProUGUI>();

        if (label != null)
        {
            label.text = taskContext;
        } else
        {
            Debug.LogWarning("Label_Objective not found in the task prefab.");
        }

        if (description != null)
        {
            description.text = taskDescription;
        } else
        {
            Debug.LogWarning("Label_Description not found in the task prefab.");
        }

    }

    public void RemoveTask(GameObject taskObject)
    {
        if (taskObjects.Contains(taskObject))
        {
            taskObjects.Remove(taskObject);
            Destroy(taskObject);
        }
    }

    public void ClearAllTasks()
    {
        foreach (GameObject task in taskObjects)
        {
            Destroy(task);
        }
        taskObjects.Clear();
    }

    [ContextMenu("Generate Test Task")]
    public void GenerateGrid()
    {
        AddTask(NewTaskContent, NewTaskDescription);
        Debug.Log("New Task Generated via ContextMenu.");
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(EmergencyProtocolManager))]
public class EmergencyProtocolManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EmergencyProtocolManager manager = (EmergencyProtocolManager)target;


        if (GUILayout.Button("Add Default Task"))
        {
            manager.AddTask(manager.NewTaskContent, manager.NewTaskDescription);
        }

        if (GUILayout.Button("Clear All Tasks"))
        {
            manager.ClearAllTasks();
        }

        if (GUILayout.Button("Generate Test Task"))
        {
            manager.GenerateGrid();
        }
    }
}
#endif