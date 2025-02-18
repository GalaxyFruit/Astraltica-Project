using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject settingsPanel;
    [SerializeField] private PlayerInputManager playerInputManager;

    public static GameManager Instance { get; private set; }

    private bool isInventoryShown = false;
    private bool isSettingsShown = false;

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        Respawning
    }

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

        var settingsCanvasTransform = GameObject.Find("Canvas")?.transform;
        settingsPanel = settingsCanvasTransform?.Find("SettingsButton")?.gameObject;

        playerInputManager = FindFirstObjectByType<PlayerInputManager>();

        //if (inventory == null)
        //    Debug.LogWarning("[GameManager] Nenalezen MainInventoryGroup");
        //if (settingsPanel == null)
        //    Debug.LogWarning("[GameManager] Nenalezen SettingsButton");
        //if (playerInputManager == null)
        //    Debug.LogWarning("[GameManager] Nenalezen PlayerInputManager");
    }


    public void Play()
    {
        SceneManager.LoadScene("MainScene");
        SetGameState(GameState.Playing);
    }

    public void Settings()
    {
        isSettingsShown = !isSettingsShown;
        settingsPanel?.SetActive(isSettingsShown);
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
        Debug.Log($"Game State changed to: {newState}");

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
            case GameState.Respawning:
                Time.timeScale = 1f;
                HandleRespawn();
                break;
        }
    }

    private void HandleRespawn()
    {
        Debug.Log("Respawn hráče");
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        Debug.Log("Hráč oživen!");
        SetGameState(GameState.Playing);
    }

    private void ToggleInventory()
    {
        isInventoryShown = !isInventoryShown;
        inventory?.SetActive(isInventoryShown);

        if (isInventoryShown)
        {
            playerInputManager?.DisableInputs();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            playerInputManager?.EnableInputs();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}
