using TMPro;
using UnityEngine;
using System.Collections.Generic;

public class EmergencyProtocol : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject emergencyProtocolUI;
    [SerializeField] private TextMeshProUGUI EPName;
    [SerializeField] private TextMeshProUGUI emergencyProtocolText;

    [Header("Config")]
    [SerializeField] private string emergencyProtocolName = "EMERGENCY PROTOCOL";

    private List<string> activeTasks = new();

    private void Start()
    {
        if (EPName != null)
            EPName.text = emergencyProtocolName;

        UpdateTaskDisplay();
    }

    public void AddTask(string task)
    {
        if (!activeTasks.Contains(task))
        {
            activeTasks.Add(task);
            UpdateTaskDisplay();
        }
    }

    public void RemoveTask(string task)
    {
        if (activeTasks.Contains(task))
        {
            activeTasks.Remove(task);
            UpdateTaskDisplay();
        }
    }

    public void ClearTasks()
    {
        activeTasks.Clear();
        UpdateTaskDisplay();
    }

    private void UpdateTaskDisplay()
    {
        if (emergencyProtocolText != null)
            emergencyProtocolText.text = string.Join("\n", activeTasks);
    }

    public void ToggleUI(bool state)
    {
        if (emergencyProtocolUI != null)
            emergencyProtocolUI.SetActive(state);
    }
}
