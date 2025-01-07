using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private PlayerInputManager playerInputManager;
    [SerializeField] private GameObject inventory;
    [SerializeField] private GameObject settingsPanel;

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
        DontDestroyOnLoad(gameObject);
    }


    private void Start()
    {
        playerInputManager.OnInventoryChanged += ChangeInventoryUI;
    }

    public void Play()
    {
        SceneManager.LoadScene("MainScene");
        SetGameState(GameState.Playing);
    }

    public void Settings()
    {
        isSettingsShown = !isSettingsShown;
        settingsPanel.SetActive(isSettingsShown);
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Game Quit!");
    }

    private void ChangeInventoryUI()
    {
        isInventoryShown = !isInventoryShown;
        inventory.SetActive(isInventoryShown);

        if (isInventoryShown)
        {
            playerInputManager.DisableInputs();
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            playerInputManager.EnableInputs();
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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

    public void TogglePause()
    {
        if (CurrentState == GameState.Playing)
        {
            SetGameState(GameState.Paused);
        }
        else if (CurrentState == GameState.Paused)
        {
            SetGameState(GameState.Playing);
        }
    }

    private void HandleRespawn()
    {
        Debug.Log("Player is respawning...");
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        Debug.Log("Player respawned!");
        SetGameState(GameState.Playing);
    }
}
