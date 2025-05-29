using UnityEngine;

[RequireComponent(typeof(Collider))]
public class TaskTrigger : MonoBehaviour
{
    [Header("Task Identification")]
    [SerializeField] private string taskID;

    [Tooltip("true -> complete task;  false -> add task")]
    [SerializeField] private bool completeTask = true;

    [Header("Fill for AddTask")]
    [SerializeField] private string taskTitle;
    [TextArea(2, 4)]
    [SerializeField] private string taskDescription;
    [SerializeField] private bool markCritical = false;

    private bool taskTriggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (taskTriggered) return;

        if (other.CompareTag("Player"))
        {
            taskTriggered = true;
            EmergencyProtocolManager protocolManager = FindFirstObjectByType<EmergencyProtocolManager>();
            if (protocolManager != null)
            {
                if (completeTask)
                {
                    protocolManager.CompleteTask(taskID);
                    Debug.Log($"Task '{taskID}' has been completed.");
                }
                else
                {
                    protocolManager.AddTask(taskID, taskTitle, taskDescription, markCritical);
                    Debug.Log($"Task '{taskID}' has been added.");
                }

                gameObject.SetActive(false);
            }
        }
    }
}
