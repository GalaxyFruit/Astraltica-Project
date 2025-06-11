// Copyright (c) 2024 Synty Studios Limited. All rights reserved.
//
// Use of this software is subject to the terms and conditions of the Synty Studios End User Licence Agreement (EULA)
// available at: https://syntystore.com/pages/end-user-licence-agreement
//
// Sample scripts are included only as examples and are not intended as production-ready.

using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;


#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.SceneManagement;
#endif

namespace Synty.Interface.Apocalypse.Samples
{
    /// <summary>
    /// A scene loader that uses the Command Pattern for flexible scene transitions.
    /// </summary>
    public class SampleSceneLoader : MonoBehaviour
    {
        [Header("References")]
        public Animator animator;

        [Header("Parameters")]
        public bool showCursor;

        [Header("Player Input Manager")]
        private PlayerInputManager playerInputManager;

        private void Start()
        {
            playerInputManager = FindFirstObjectByType<PlayerInputManager>();
            if (playerInputManager == null)
            {
                Debug.LogWarning("PlayerInputManager not found in the scene.");
            }
        }

        private void OnEnable()
        {
            if (animator)
            {
                animator.gameObject.SetActive(true);
                animator.SetBool("Active", false);
            }

            if (showCursor)
            {
                Cursor.visible = true;
            }
        }

        /// <summary>
        /// Quits the application.
        /// </summary>
        public void QuitApplication()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// Switches to the main menu scene.
        /// </summary>
        public void GoToMainMenu()
        {
            if (playerInputManager != null)
            {
                playerInputManager.EnableInputs();
            }

            ExecuteCommand(new SceneCommand("Synty's_Menu"));
        }

        public void GoToCutsceneScene()
        {
            ExecuteCommand(new SceneCommand("CutsceneScene"));
        }


        /// <summary>
        /// Switches to the main scene.
        /// </summary>
        public void GoToMainScene()
        {
            playerInputManager?.DisableInputs();
            ExecuteCommand(new SceneCommand("MainScene"));
        }

        /// <summary>
        /// Switches to the controls scene.
        /// </summary>
        public void GoToControls()
        {
            ExecuteCommand(new SceneCommand("Controls"));
        }

        /// <summary>
        /// Executes a scene transition command.
        /// </summary>
        private void ExecuteCommand(ISceneCommand command)
        {
            StartCoroutine(C_ExecuteCommand(command));
        }

        /// <summary>
        /// Coroutine to handle scene transitions with animations.
        /// </summary>
        private IEnumerator C_ExecuteCommand(ISceneCommand command)
        {
            if (Time.timeScale != 1f)
                GameManager.Instance?.SetGameState(GameState.Playing);

            if (animator)
            {
                animator.gameObject.SetActive(true);
                animator.SetBool("Active", true);
                yield return new WaitForSeconds(0.5f);
            }

            command.Execute();
        }

        /// <summary>
        /// Interface for scene transition commands.
        /// </summary>
        public interface ISceneCommand
        {
            void Execute();
        }

        /// <summary>
        /// Concrete command for loading scenes.
        /// </summary>
        public class SceneCommand : ISceneCommand
        {
            private readonly string _sceneName;

            public SceneCommand(string sceneName)
            {
                _sceneName = sceneName;
            }

            public void Execute()
            {
#if UNITY_EDITOR
                string path = _sceneName;
                if (path.IndexOf('/') == -1)
                {
                    string guid = AssetDatabase.FindAssets($"{_sceneName} t:scene")[0];
                    path = AssetDatabase.GUIDToAssetPath(guid);
                }
                EditorSceneManager.LoadSceneAsyncInPlayMode(path, new LoadSceneParameters(LoadSceneMode.Single));
#else
                SceneManager.LoadScene(_sceneName);
                //AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(_sceneName);
                //while (!asyncLoad.isDone)
                //{
                //    // Wait until the scene is fully loaded
                //}
#endif
            }
        }
    }
}
