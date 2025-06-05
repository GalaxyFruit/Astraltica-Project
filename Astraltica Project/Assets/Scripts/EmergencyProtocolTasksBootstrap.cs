using UnityEngine;

public class EmergencyProtocolTasksBootstrap : MonoBehaviour
{
    private EmergencyProtocolManager _protocolManager;

    [Header("Tasks")]
    [SerializeField]
    private EmergencyTaskData firstTask = new EmergencyTaskData
    {
        TaskID = "task_1",
        TaskTitle = "Find Shelter Before the Storm",
        TaskDescription = "A storm is approaching, making it impossible to breathe. \nYou have limited oxygen supplies to survive! Locate a safe shelter.",
        IsCritical = true
    };

    [SerializeField]
    private EmergencyTaskData secondTask = new EmergencyTaskData
    {
        TaskID = "task_2",
        TaskTitle = "Explore Your Surroundings",
        TaskDescription = "Signal traces from Shelter A!p#a detected.  Exact coordinates unknown; last data indicates nearby winter location.",
        IsCritical = true
    };

    [SerializeField]
    private EmergencyTaskData thirdTask = new EmergencyTaskData
    {
        TaskID = "task_3",
        TaskTitle = "Access the Terminal in Shelter Alpha",
        TaskDescription = "Interact with the terminal in Shelter Alpha. Retrieve the keycard located on the log table nearby.",
        IsCritical = false
    };

    [SerializeField]
    private EmergencyTaskData fourthTask = new EmergencyTaskData
    {
        TaskID = "task_4",
        TaskTitle = "Access the Terminal in Beta Bunker",
        TaskDescription = "Based on the Alpha log, search for Beta Bunker in the southern forest zone.",
        IsCritical = false
    };

    [SerializeField]
    private EmergencyTaskData fifthTask = new EmergencyTaskData
    {
        TaskID = "task_5",
        TaskTitle = "Locate Gamma Bunker",
        TaskDescription = "According to the Beta log, search for Gamma Bunker located in the marshland.",
        IsCritical = false
    };

    [SerializeField]
    private EmergencyTaskData sixthTask = new EmergencyTaskData
    {
        TaskID = "task_6",
        TaskTitle = "Access the Terminal in Gamma Bunker",
        TaskDescription = "Interact with the terminal in Gamma Bunker and review the final log.",
        IsCritical = false
    };

    [SerializeField]
    private EmergencyTaskData seventhTask = new EmergencyTaskData
    {
        TaskID = "task_7",
        TaskTitle = "Activate the Main Terminal",
        TaskDescription = "Insert all three keycards into the main terminal in Gamma Bunker.",
        IsCritical = false
    };

    [SerializeField]
    private EmergencyTaskData eighthTask = new EmergencyTaskData
    {
        TaskID = "task_8",
        TaskTitle = "Investigate the Stargate",
        TaskDescription = "The Stargate is now active. Investigate it to complete the evacuation.",
        IsCritical = true
    };

    private void Start()
    {
        _protocolManager = FindFirstObjectByType<EmergencyProtocolManager>();

        _protocolManager.AddTask(firstTask.TaskID, firstTask.TaskTitle, firstTask.TaskDescription, firstTask.IsCritical);
        _protocolManager.AddTask(secondTask.TaskID, secondTask.TaskTitle, secondTask.TaskDescription, secondTask.IsCritical);

        _protocolManager.OnTaskCompleted += OnTaskCompleted;
    }

    private void OnDestroy()
    {
        if (_protocolManager != null)
            _protocolManager.OnTaskCompleted -= OnTaskCompleted;
    }

    private void OnTaskCompleted(EmergencyTaskData completedTask)
    {
        if (completedTask.TaskID == firstTask.TaskID)
        {
            _protocolManager.AddTask(thirdTask.TaskID, thirdTask.TaskTitle, thirdTask.TaskDescription, thirdTask.IsCritical);
        }
        else if (completedTask.TaskID == thirdTask.TaskID)
        {
            _protocolManager.AddTask(fourthTask.TaskID, fourthTask.TaskTitle, fourthTask.TaskDescription, fourthTask.IsCritical);
        }
        else if (completedTask.TaskID == fourthTask.TaskID) {
            _protocolManager.AddTask(fifthTask.TaskID, fifthTask.TaskTitle, fifthTask.TaskDescription, fifthTask.IsCritical);
        }
        else if (completedTask.TaskID == fifthTask.TaskID)
        {
            _protocolManager.AddTask(sixthTask.TaskID, sixthTask.TaskTitle, sixthTask.TaskDescription, sixthTask.IsCritical);
        }
        else if (completedTask.TaskID == sixthTask.TaskID)
        {
            _protocolManager.AddTask(seventhTask.TaskID, seventhTask.TaskTitle, seventhTask.TaskDescription, seventhTask.IsCritical);
        }
        else if (completedTask.TaskID == seventhTask.TaskID)
        {
            _protocolManager.AddTask(eighthTask.TaskID, eighthTask.TaskTitle, eighthTask.TaskDescription, eighthTask.IsCritical);
        }
    }
}