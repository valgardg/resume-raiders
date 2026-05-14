using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // events
    public static event Action OnGameStart;
    public static event Action OnGameRestart;
    public static event Action OnGamePause;
    public static event Action OnGameUnpause;

    // references
    public GameTime gameTime;
    public ResumeManager resumeManager;
    public ApplicantData applicantData;
    public UIManager uiManager;
    public GameStateSO gameState;
    public JobPostingManager jobPostingManager;

    private void Start()
    {
        uiManager.Initialize(gameState);
        gameTime.Initialize(gameState);
        applicantData.Initialize(gameState);
        jobPostingManager.Initialize(gameState, applicantData);
        resumeManager.Initialize(gameState, gameTime, applicantData, jobPostingManager);
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

    public void StartNewDay()
    {
        RestartGame();
    }
}
