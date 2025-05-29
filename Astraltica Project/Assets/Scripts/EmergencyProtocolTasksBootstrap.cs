using UnityEngine;

public class EmergencyProtocolTasksBootstrap : MonoBehaviour
{
    [Header("Reference to EmergencyProtocolManager")]
    private EmergencyProtocolManager _protocolManager;

    [Header("First Two Tasks")]
    [SerializeField]
    private EmergencyTaskData firstTask = new EmergencyTaskData
    {
        TaskID = "task_1",
        TaskTitle = "First Task",
        TaskDescription = "Description for the first task.",
        IsCritical = false
    };

    [SerializeField]
    private EmergencyTaskData secondTask = new EmergencyTaskData
    {
        TaskID = "task_2",
        TaskTitle = "Second Task",
        TaskDescription = "Description for the second task.",
        IsCritical = true
    };

    private void Start()
    {
        _protocolManager = FindFirstObjectByType<EmergencyProtocolManager>();

        _protocolManager.AddTask(firstTask.TaskID, firstTask.TaskTitle, firstTask.TaskDescription, firstTask.IsCritical
        );

        _protocolManager.AddTask(secondTask.TaskID, secondTask.TaskTitle, secondTask.TaskDescription, secondTask.IsCritical
        );
    }
}