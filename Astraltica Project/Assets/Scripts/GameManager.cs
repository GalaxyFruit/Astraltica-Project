using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

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
    }

    private void Start()
    {
        //SetGameState(GameState.MainMenu);
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log($"Game State changed to: {newState}");

        switch (newState)
        {
            case GameState.MainMenu:

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
        // TODO : Logika respawn hráče
        RespawnPlayer();
    }

    private void RespawnPlayer()
    {
        // TODO : Logika pro resetování pozice hráče, zdraví atd.
        Debug.Log("Player respawned!");
        SetGameState(GameState.Playing); 
    }
}
