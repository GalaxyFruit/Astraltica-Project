using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // Import for UI.Image
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Image))] // Replace GUITexture with Image
public class ForcedReset : MonoBehaviour
{
    private void Update()
    {
        // Check if the "ResetObject" button is pressed
        if (CrossPlatformInputManager.GetButtonDown("ResetObject"))
        {
            // Reload the current scene
            SceneManager.LoadScene(SceneManager.GetSceneAt(0).name);
        }
    }
}
