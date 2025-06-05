using System;
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
    [SerializeField] private float multiplier = 1f;

    [SerializeField] private AnimationClip OutAnimClip;

    [Header("Default Task Settings (for quick add)")]
    [SerializeField]
    internal EmergencyTaskData DefaultTask = new EmergencyTaskData
    {
        TaskID = "task_default",
        TaskTitle = "New Task",
        TaskDescription = "This is a new task description.",
        IsCritical = false
    };

    private readonly List<GameObject> taskObjects = new();
    private readonly Dictionary<string, GameObject> activeTasks = new();
    private readonly Queue<EmergencyTaskData> taskQueue = new();

    private bool isActivatingQueue = false;
    private float sceneStartTime;
    private float lengthOfClip;

    private Coroutine activationCoroutine;

    public event Action<EmergencyTaskData> OnTaskAdded;
    public event Action<EmergencyTaskData> OnTaskCompleted;

    private void Awake()
    {
        sceneStartTime = Time.time;
    }

    public bool HasTask(string taskID) => activeTasks.ContainsKey(taskID);

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

        var taskData = new EmergencyTaskData
        {
            TaskID = taskID,
            TaskTitle = taskTitle,
            TaskDescription = taskDescription,
            IsCritical = isCritical
        };

        float elapsed = Time.time - sceneStartTime;
        if (elapsed < activationDelay)
        {
            taskQueue.Enqueue(taskData);

            if (!isActivatingQueue)
            {
                activationCoroutine = StartCoroutine(ActivateTasksFromQueue());
            }
        }
        else
        {
            CreateAndActivateTask(taskData);
        }
    }

    private IEnumerator ActivateTasksFromQueue()
    {
        isActivatingQueue = true;

        float waitTime = activationDelay - (Time.time - sceneStartTime);
        if (waitTime > 0f)
            yield return new WaitForSeconds(waitTime);

        while (taskQueue.Count > 0)
        {
            var task = taskQueue.Dequeue();
            CreateAndActivateTask(task);

            yield return new WaitForSeconds(activationInterval);
        }

        isActivatingQueue = false;
        activationCoroutine = null;
    }

    private void CreateAndActivateTask(EmergencyTaskData taskData)
    {
        GameObject newTask = Instantiate(taskPrefab, taskListParent);
        taskObjects.Add(newTask);
        activeTasks[taskData.TaskID] = newTask;

        TextMeshProUGUI label = newTask.transform.Find("Content/Text/Label_Objective")?.GetComponent<TextMeshProUGUI>();
        TextMeshProUGUI description = newTask.transform.Find("Content/Text/TextDescription/Label_Description")?.GetComponent<TextMeshProUGUI>();

        if (label != null)
        {
            label.text = taskData.IsCritical ? $"<color=#FF0000>[CRITICAL]</color> {taskData.TaskTitle}" : taskData.TaskTitle;
        }
        if (description != null)
        {
            description.text = taskData.TaskDescription;
        }

        Debug.Log($"Task ID: {taskData.TaskID}, GameObject: {newTask.gameObject.name} added and activated.");

        OnTaskAdded?.Invoke(taskData);
    }

    public void CompleteTask(string taskID)
    {
        if (activeTasks.TryGetValue(taskID, out GameObject taskObject))
        {
            var completedTask = GetTaskDataFromObject(taskID, taskObject);
            activationCoroutine = StartCoroutine(PlayTaskCompletionAnimation(taskObject, completedTask));
            activeTasks.Remove(taskID);
        }
        else
        {
            Debug.LogWarning($"Task '{taskID}' not found for completion.");
        }
    }

    private IEnumerator PlayTaskCompletionAnimation(GameObject taskObject, EmergencyTaskData completedTask)
    {
        Animator animator = taskObject.GetComponent<Animator>();
        if (animator == null)
        {
            Debug.LogWarning("No animator attached to the task object.");
            Destroy(taskObject);
            yield break;
        }

        if (OutAnimClip == null)
        {
            Debug.LogWarning("OutAnimClip is not assigned, using default trigger.");
            lengthOfClip = 0.5f;
        }
        else
        {
            lengthOfClip = OutAnimClip.length / multiplier;
        }

        animator.SetTrigger("Out");

        yield return new WaitForSeconds(lengthOfClip + 0.25f);

        Destroy(taskObject);
        taskObjects.Remove(taskObject);

        Debug.Log($"Task '{taskObject.name}' completed and removed from the list.");
        activationCoroutine = null;

        OnTaskCompleted?.Invoke(completedTask);
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

    public void GenerateGrid()
    {
        AddTask(DefaultTask.TaskID, DefaultTask.TaskTitle, DefaultTask.TaskDescription, DefaultTask.IsCritical);
        Debug.Log("New Task Generated via ContextMenu.");
    }

    private EmergencyTaskData GetTaskDataFromObject(string taskID, GameObject taskObject)
    {
        var label = taskObject.transform.Find("Content/Text/Label_Objective")?.GetComponent<TextMeshProUGUI>()?.text ?? "";
        var desc = taskObject.transform.Find("Content/Text/TextDescription/Label_Description")?.GetComponent<TextMeshProUGUI>()?.text ?? "";
        bool isCritical = label.Contains("[CRITICAL]");
        return new EmergencyTaskData
        {
            TaskID = taskID,
            TaskTitle = label,
            TaskDescription = desc,
            IsCritical = isCritical
        };
    }
}