using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject losePanel;
    public GameObject pauseMenu;
    public GameObject helpTextPanel;
    public GameObject endOfDayPanel;
    public ProgressBar progressBar;

    public void Initialize(GameStateSO gameState)
    {
        progressBar.Initialize(gameState);
        
        GameTime.OnTimeUp += () => losePanel.SetActive(true);
        GameManager.OnGameRestart += () => losePanel.SetActive(false);
        GameManager.OnGamePause += DisplayPauseMenu;
        GameManager.OnGameUnpause += () => pauseMenu.SetActive(false);
        GameManager.OnGameStart += () => helpTextPanel.SetActive(false);
        gameState.OnFullfilledHires += DisplayEndOfDayPanel;
    }

    private void DisplayPauseMenu()
    {
        pauseMenu.SetActive(true);
        RectTransform rectTransform = pauseMenu.GetComponent<RectTransform>();
        rectTransform.SetAsLastSibling();
    }

    private void DisplayEndOfDayPanel()
    {
        endOfDayPanel.SetActive(true);
        RectTransform rectTransform = endOfDayPanel.GetComponent<RectTransform>();
        rectTransform.SetAsLastSibling();
    }
}
