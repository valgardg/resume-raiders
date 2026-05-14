using TMPro;
using UnityEngine;
using System;

public class GameTime : MonoBehaviour
{
    public static event Action OnTimeUp;
    public TMP_Text timeText;

    private const float GAME_DURATION = 180f;        // Real seconds for a full workday
    private const int WORK_START_HOUR = 9;           // 9:00 AM
    private const int WORK_END_HOUR = 17;            // 5:00 PM
    private const int WORK_MINUTES = (WORK_END_HOUR - WORK_START_HOUR) * 60; // 480 min

    private float timeLeft = GAME_DURATION;
    private bool timeRunning = false;

    public void Initialize(GameStateSO gameState)
    {
        GameManager.OnGameStart += () => timeRunning = true;
        GameManager.OnGameRestart += RestartTime;
        GameManager.OnGamePause += () => timeRunning = false;
        GameManager.OnGameUnpause += () => timeRunning = true;

        gameState.OnFullfilledHires += () => timeRunning = false;
    }

    private void Update()
    {
        if (!timeRunning) return;

        timeLeft -= Time.deltaTime;

        if (timeLeft <= 0f)
        {
            timeLeft = 0f;
            timeRunning = false;
            OnTimeUp?.Invoke();
        }

        timeText.text = GetWorkdayTime();
    }

    private string GetWorkdayTime()
    {
        // How far through the day are we? (0.0 = 9 AM, 1.0 = 5 PM)
        float progress = 1f - (timeLeft / GAME_DURATION);
        int elapsedWorkMinutes = Mathf.FloorToInt(progress * WORK_MINUTES);

        int hour = WORK_START_HOUR + (elapsedWorkMinutes / 60);
        int minute = elapsedWorkMinutes % 60;

        string period = hour < 12 ? "AM" : "PM";
        int displayHour = hour > 12 ? hour - 12 : hour;

        return $"{displayHour}:{minute:00} {period}";
    }

    public void ReduceTime(float amount)
    {
        timeLeft -= amount;
    }

    private void RestartTime()
    {
        timeLeft = GAME_DURATION;
        timeRunning = true;
    }
}