using Microsoft.Unity.VisualStudio.Editor;
using TMPro;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public TMP_Text progressText;
    public float currentProgress = 0f;

    private GameStateSO gameState;

    public void Initialize(GameStateSO gameState)
    {
        this.gameState = gameState;
    }
    public void SetProgress()
    {
        float progress = gameState.acceptedApplicants / gameState.requiredApplicants;
        // Clamp the progress value between 0 and 1
        // progress = Mathf.Clamp01(progress);

        // parent width
        float parentWidth = ((RectTransform)transform.parent).rect.width;
        // Set the width of the progress bar based on the progress value
        ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentWidth * progress);

        progressText.text = $"{gameState.acceptedApplicants} / {gameState.requiredApplicants} applications";
    }

    void Update()
    {
        SetProgress();
    }
}
