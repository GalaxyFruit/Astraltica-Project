using System;
using Synty.Interface.Apocalypse.Samples;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Playables;

public class CutsceneController : MonoBehaviour
{
    [SerializeField] private PlayableDirector timelineDirector;
    [SerializeField] private InputActionAsset _inputActions;

    private InputAction _skipCutsceneAction;
    private SampleSceneLoader _sceneLoader;

    void Start()
    {
        var map = _inputActions.FindActionMap("Cutscene", true);
        _sceneLoader = gameObject.GetComponent<SampleSceneLoader>();

        if(_sceneLoader == null)
        {
            Debug.LogError("SampleSceneLoader component not found on CutsceneController GameObject.");
            return;
        }

        if (timelineDirector == null)
        {
            Debug.LogError("PlayableDirector is not assigned in CutsceneController.");
            return;
        }

        if (map == null)
        {
            Debug.LogError("CutsceneControls action map not found in InputActionAsset.");
            return;
        }

        _skipCutsceneAction = map.FindAction("Skip", true);

        _skipCutsceneAction.performed += OnSkipCutscene;
        _skipCutsceneAction.Enable();
    }

    private void OnDisable()
    {
        _skipCutsceneAction.performed -= OnSkipCutscene;
        _skipCutsceneAction.Disable();
    }

    private void OnSkipCutscene(InputAction.CallbackContext context)
    {
        Debug.Log("Cutscéna byla přeskočena.");
        timelineDirector.Stop();
        MoveToNextScene();
    }

    private void MoveToNextScene()
    {
        _sceneLoader.GoToMainScene();
    }
}
