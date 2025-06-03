#if UNITY_EDITOR
using UnityEditor;
#endif

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

[System.Serializable]
public class EmergencyTaskData
{
    public string TaskID;
    public string TaskTitle;
    public string TaskDescription;
    public bool IsCritical;
}

public class EmergencyProtocolManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private Transform taskListParent;
    [SerializeField] private GameObject taskPrefab;

    [Header("Activation Delay Settings")]
    [SerializeField] private float activationDelay = 2f;
    [Tooltip("Delay between activating each queued task")]
    [SerializeField] private float activationInterval = 0.5f;

    [Header("Default Task Settings (for quick add)")]
    [SerializeField]
    internal EmergencyTaskData DefaultTask = new EmergencyTaskData
    {
        TaskID = "task_default",
        TaskTitle = "New Task",
        TaskDescription = "This is a new task description.",
        IsCritical = false
    };

    private List<GameObject> taskObjects = new();
    private Dictionary<string, GameObject> activeTasks = new();

    private Queue<(string id, string title, string description, bool critical)> taskQueue = new();

    private bool isActivatingQueue = false;
    private float sceneStartTime;

    private Coroutine activationCoroutine;
    private Coroutine activationDelayCoroutine;

    private void Awake()
    {
        sceneStartTime = Time.time;
    }

    public void AddTask(string taskID, string taskTitle, string taskDescription, bool isCritical = false)
    {
        if (taskPrefab == null || taskListParent == null)
        {
            Debug.LogError("Task Prefab or Task List Parent is not assigned.");
            return;
        }

        if (activeTasks.ContainsKey(taskID))
        {
            Debug.LogWarning($"Task with ID {taskID} already exists, skipping add.");
            return;
        }

        float elapsed = Time.time - sceneStartTime;
        if (elapsed < activationDelay)
        {
            taskQueue.Enqueue((taskID, taskTitle, taskDescription, isCritical));

            if (!isActivatingQueue)
            {
                activationCoroutine = StartCoroutine(ActivateTasksFromQueue());
            }
        }
        else
        {
            CreateAndActivateTask(taskID, taskTitle, taskDescription, isCritical);
        }
    }

    private IEnumerator ActivateTasksFromQueue()
    {
        isActivatingQueue = true;

        // Počkat, dokud uplyne activationDelay od startu scény
        float waitTime = activationDelay - (Time.time - sceneStartTime);
        if (waitTime > 0f)
            yield return new WaitForSeconds(waitTime);

        while (taskQueue.Count > 0)
        {
            var task = taskQueue.Dequeue();
            CreateAndActivateTask(task.id, task.title, task.description, task.critical);

            yield return new WaitForSeconds(activationInterval);
        }

        isActivatingQueue = false;
        activationCoroutine = null;
    }

    private void CreateAndActivateTask(string taskID, string taskTitle, string taskDescription, bool isCritical)
    {
        GameObject newTask = Instantiate(taskPrefab, taskListParent);
        taskObjects.Add(newTask);
        activeTasks[taskID] = newTask;

        TextMeshProUGUI label = newTask.transform.Find("Content/Text/Label_Objective")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = newTask.transform.Find("Content/Text/TextDescription/Label_Description")?.GetComponent<TextMeshProUGUI>();

        if (label != null)
        {
            label.text = isCritical ? $"<color=#FF0000>[CRITICAL]</color> {taskTitle}" : taskTitle;
        }
        if (description != null)
        {
            description.text = taskDescription;
        }

        Debug.Log($"Task ID: {taskID}, GameObject: {newTask.gameObject.name} added and activated.");
    }

    public void CompleteTask(string taskID)
    {
        if (activeTasks.TryGetValue(taskID, out GameObject taskObject))
        {
            activationCoroutine = StartCoroutine(PlayTaskCompletionAnimation(taskObject));
            activeTasks.Remove(taskID);
        }
        else
        {
            Debug.LogWarning($"Task '{taskID}' not found for completion.");
        }
    }

    private IEnumerator PlayTaskCompletionAnimation(GameObject taskObject)
    {
        Animator animator = taskObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("No animator attached to the task object.");
            Destroy(taskObject);
            yield break;
        }

        animator.SetTrigger("Complete"); 
        yield return new WaitForSeconds(1f);
        Destroy(taskObject);
        taskObjects.Remove(taskObject);

        Debug.Log($"Task '{taskObject.name}' completed and removed from the list.");
        activationCoroutine = null;
    }

    public void ClearAllTasks()
    {
        foreach (GameObject task in taskObjects)
        {
            Destroy(task);
        }

        taskObjects.Clear();
        activeTasks.Clear();
    }

    // For context menu and editor button
    [ContextMenu("Generate Test Task")]
    public void GenerateGrid()
    {
        AddTask(DefaultTask.TaskID, DefaultTask.TaskTitle, DefaultTask.TaskDescription, DefaultTask.IsCritical);
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
            manager.AddTask(
                manager.DefaultTask.TaskID,
                manager.DefaultTask.TaskTitle,
                manager.DefaultTask.TaskDescription,
                manager.DefaultTask.IsCritical
            );
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