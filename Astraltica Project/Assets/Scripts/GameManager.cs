﻿using System;
using System.Collections.Generic;
using Synty.Interface.Apocalypse.Samples;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private PlayerInputManager playerInputManager;

    [Header("Panels Settings")]
    [SerializeField] private GameObject notePanel;
    [SerializeField] private GameObject pickupText;

    [Header("Escape Panel Settings")]
    [SerializeField] private GameObject escapePanel;

    [Header("Death Screen Panel Settings")]
    [SerializeField] private GameObject deathScreenPanel;

    public static GameManager Instance { get; private set; }

    public bool hasPlayerAlphaKeycard { get; private set; } = false;
    public bool hasPlayerBetaKeycard { get; private set; } = false;
    public bool hasPlayerGammaKeycard { get; private set; } = false;

    private Stargate stargate;
    private EmergencyProtocolManager emergencyProtocolManager;

    private HashSet<KeycardType> placedKeycards = new HashSet<KeycardType>();

    private bool isInventoryShown = false;
    private bool isSettingsShown = false;

    public GameState CurrentState { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        //DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        if (Instance == this)
            Instance = null;
    }

    private void Start()
    {
        InitializeReferences();

        if (playerInputManager != null)
        {
            playerInputManager.OnInventoryChanged += ToggleInventory;
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //Debug.Log("volani onsceneLoaded");
        InitializeReferences();
    }

    private void InitializeReferences()
    {

        //Debug.Log("called InitializeReferences()");

        var canvasTransform = GameObject.Find("CanvasObject")?.transform;
        inventory = canvasTransform?.Find("Canvas/MainInventoryGroup")?.gameObject;

        //var settingsCanvasTransform = GameObject.Find("Canvas")?.transform;
        //settingsPanel = settingsCanvasTransform?.Find("SettingsButton")?.gameObject;

        playerInputManager = FindFirstObjectByType<PlayerInputManager>();

        if (inventory != null)
        {
            isInventoryShown = inventory.activeSelf; 
            if (isInventoryShown)
            {
                inventory.SetActive(false);
                isInventoryShown = false;
            }
        } else
        {
            Debug.LogWarning("[GameManager] MainInventoryGroup not found in CanvasObject.");
        }


        if (escapePanel != null)
        {
            isSettingsShown = escapePanel.activeSelf;
            if (isSettingsShown)
            {
                escapePanel.SetActive(false);
                isSettingsShown = false;
            }
        } else
        {
            Debug.LogWarning("[GameManager] EscapePanel not found in the scene.");
        }

        if(notePanel != null)
        {
            notePanel.SetActive(false);
        }
        else
        {
            Debug.LogWarning("[GameManager] NotePanel not found in the scene.");
        }

        if (pickupText == null)
        {
            Debug.LogWarning("[GameManager] PickupText not found in the scene.");
        }

        if (deathScreenPanel != null)
        {
            if(deathScreenPanel.activeSelf)
            {
                deathScreenPanel.SetActive(false);
            }
        } else
        {
            Debug.LogWarning("[GameManager] DeathScreenPanel not found in the scene.");
        }

        stargate = FindFirstObjectByType<Stargate>();

        if (stargate == null)
        {
            Debug.LogWarning("[GameManager] Stargate not found in the scene.");
        }

        emergencyProtocolManager = FindFirstObjectByType<EmergencyProtocolManager>();

        if (emergencyProtocolManager == null)
        {
            Debug.LogWarning("[GameManager] EmergencyProtocolManager not found in the scene.");
        }

        //if (inventory == null)
        //    Debug.LogWarning("[GameManager] Nenalezen MainInventoryGroup");
        //if (settingsPanel == null)
        //    Debug.LogWarning("[GameManager] Nenalezen SettingsButton");
        //if (playerInputManager == null)
        //    Debug.LogWarning("[GameManager] Nenalezen PlayerInputManager");
    }

    public void KeycardCollected(KeycardType keycardType)
    {
        switch (keycardType)
        {
            case KeycardType.Alpha:
                hasPlayerAlphaKeycard = true;
                break;
            case KeycardType.Beta:
                hasPlayerBetaKeycard = true;
                break;
            case KeycardType.Gamma:
                hasPlayerGammaKeycard = true;
                break;
            default:
                Debug.LogWarning($"Unknown keycard type: {keycardType}");
                break;
        }

        if(hasPlayerAlphaKeycard && hasPlayerBetaKeycard && hasPlayerGammaKeycard)
        {
            Debug.Log("All keycards collected!");
        } else
        {
            Debug.Log($"Keycard collected: {keycardType}. Current state: Alpha={hasPlayerAlphaKeycard}, Beta={hasPlayerBetaKeycard}, Gamma={hasPlayerGammaKeycard}");
        } 
    }

    public void PlacedKeycards(KeycardType keycardType)
    {
        if (keycardType == KeycardType.None)
            return;

        placedKeycards.Add(keycardType);

        int requiredKeycards = Enum.GetValues(typeof(KeycardType)).Length - 1;

        if (placedKeycards.Count == requiredKeycards)
        {
            Debug.Log("Všechny keycards byly umístěny!");
            emergencyProtocolManager.CompleteTask("task_7");
            stargate.ActivateEvacuationVortex();
        }
        else
        {
            Debug.Log($"Umístěna keycard: {keycardType}. Aktuální stav: {string.Join(", ", placedKeycards)}");
        }
    }

    public void Settings()
    {
        isSettingsShown = !isSettingsShown;
        escapePanel?.SetActive(isSettingsShown);

        if (isSettingsShown)
        {
            playerInputManager?.DisableInputs();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            if(notePanel.activeSelf)
            {
                notePanel.SetActive(false);
            }

            if(pickupText.activeSelf)
            {
                pickupText.SetActive(false);
            }

            SetGameState(GameState.Paused);
        }
        else
        {
            playerInputManager?.EnableInputs();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            if(!pickupText.activeSelf)
            {
                pickupText.SetActive(true);
            }

            SetGameState(GameState.Playing);
        }
    }

    public void Quit()
    {
        Debug.Log("Game Hra ukončena!!");

        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }


    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        //Debug.Log($"Game State changed to: {newState}");

        switch (newState)
        {
            case GameState.MainMenu:
                Time.timeScale = 1f;
                break;
            case GameState.Playing:
                Time.timeScale = 1f;
                break;
            case GameState.Paused:
                Time.timeScale = 0f;
                break;
        }
    }

    public void DeathScreen()
    {

        deathScreenPanel.SetActive(true);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        playerInputManager?.DisableInputs();
        SetGameState(GameState.Paused);
    }

    private void ToggleInventory()
    {
        isInventoryShown = !isInventoryShown;
        inventory?.SetActive(isInventoryShown);

        if (isInventoryShown)
        {
            playerInputManager?.DisableInputs(new[] { "Inventory", "Hotbar", "ScrollHotbar"});
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            //Debug.Log("Inventory is shown");
        }
        else
        {
            playerInputManager?.EnableInputs();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}

public enum GameState
{
    MainMenu,
    Playing,
    Paused,
    Respawning
}

public enum KeycardType
{
    None,
    Alpha,
    Beta,
    Gamma
}
