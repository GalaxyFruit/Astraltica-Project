using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        MainMenu,
        Playing,
        Paused,
        GameOver
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
        //SetGameState(GameState.MainMenu);
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
        Debug.Log($"Game State changed to: {newState}");

        switch (newState)
        {
            case GameState.MainMenu:
                // logika hlavního menu
                break;
            case GameState.Playing:
                // spuštění hry
                break;
            case GameState.Paused:
                Time.timeScale = 0f; // Pauza hry
                break;
            case GameState.GameOver:
                // Konec hry
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
            Time.timeScale = 1f;
            SetGameState(GameState.Playing);
        }
    }
}
