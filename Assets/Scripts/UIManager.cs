using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject losePanel;
    public GameObject pauseMenu;
    public GameObject helpTextPanel;
    public ProgressBar progressBar;

    private GameStateSO gameState;

    public void Initialize(GameStateSO gameState)
    {
        progressBar.Initialize(gameState);
        
        GameTime.OnTimeUp += () => losePanel.SetActive(true);
        GameManager.OnGameRestart += () => losePanel.SetActive(false);
        GameManager.OnGamePause += DisplayPauseMenu;
        GameManager.OnGameUnpause += () => pauseMenu.SetActive(false);
        GameManager.OnGameStart += () => helpTextPanel.SetActive(false);

    }

    private void DisplayPauseMenu()
    {
        pauseMenu.SetActive(true);
        RectTransform rectTransform = pauseMenu.GetComponent<RectTransform>();
        rectTransform.SetAsLastSibling();
    }
}
