using UnityEngine;
using UnityEngine.UI;

namespace UnityStandardAssets.Utility
{
    public class SimpleActivatorMenu : MonoBehaviour
    {
        // Updated to use UnityEngine.UI.Text
        public Text camSwitchButton;
        public GameObject[] objects;

        private int m_CurrentActiveObject;

        private void OnEnable()
        {
            // Start with the first object active
            m_CurrentActiveObject = 0;
            UpdateButtonLabel();
        }

        public void NextCamera()
        {
            // Determine the next active object
            int nextActiveObject = m_CurrentActiveObject + 1 >= objects.Length ? 0 : m_CurrentActiveObject + 1;

            // Update active objects
            for (int i = 0; i < objects.Length; i++)
            {
                objects[i].SetActive(i == nextActiveObject);
            }

            // Update current active object and button label
            m_CurrentActiveObject = nextActiveObject;
            UpdateButtonLabel();
        }

        private void UpdateButtonLabel()
        {
            // Update the button text to reflect the current active object
            if (camSwitchButton != null && m_CurrentActiveObject < objects.Length)
            {
                camSwitchButton.text = objects[m_CurrentActiveObject].name;
            }
        }
    }
}
