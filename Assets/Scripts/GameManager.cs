using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnGameStart;
    public static event Action OnGameRestart;
    public static event Action OnGamePause;
    public static event Action OnGameUnpause;
    public GameTime gameTime;
    public ResumeManager resumeManager;

    public UIManager uiManager;

    private void Start()
    {
        gameTime.Initialize();
        resumeManager.Initialize(gameTime);
        uiManager.Initialize();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseGame();
        }
    }

    public void StartGame()
    {
        OnGameStart?.Invoke();
    }

    public void RestartGame()
    {
        OnGameRestart?.Invoke();
    }

    public void PauseGame()
    {
        OnGamePause?.Invoke();
    }

    public void UnpauseGame()
    {
        OnGameUnpause?.Invoke();
    }
}
