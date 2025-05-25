#if UNITY_EDITOR
using UnityEditor;
#endif
using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EmergencyProtocolManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform taskListParent; // Parent object (Objective_List) for placing task prefabs
    [SerializeField] private GameObject taskPrefab; // Prefab for the task item

    [Header("Task Settings")]
    [SerializeField] internal string NewTaskContent = "New Task"; // Default task name for new tasks
    private List<GameObject> taskObjects = new(); // List to track task instances

    public void AddTask(string taskContext)
    {
        if (taskPrefab == null || taskListParent == null)
        {
            Debug.LogError("Task Prefab or Task List Parent is not assigned");
            return;
        }

        // Create a new task instance
        GameObject newTask = Instantiate(taskPrefab, taskListParent);
        taskObjects.Add(newTask);

        // Find the label object and update it with the task name
        TextMeshProUGUI label = newTask.transform.Find("Content/Text/Label_Objective")?.GetComponent<TextMeshProUGUI>();
        if (label != null)
        {
            label.text = taskContext;
        } else
        {
            Debug.LogWarning("Label_Objective not found in the task prefab.");
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
        AddTask(NewTaskContent);
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

        // Add custom buttons
        if (GUILayout.Button("Add Default Task"))
        {
            manager.AddTask(manager.NewTaskContent);
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