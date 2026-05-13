using Microsoft.Unity.VisualStudio.Editor;
using UnityEngine;

public class ProgressBar : MonoBehaviour
{
    public float currentProgress = 0f;
    public void SetProgress(float progress)
    {
        // Clamp the progress value between 0 and 1
        progress = Mathf.Clamp01(progress);

        // parent width
        float parentWidth = ((RectTransform)transform.parent).rect.width;
        // Set the width of the progress bar based on the progress value
        ((RectTransform)transform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, parentWidth * progress);
    }

    void Update()
    {
        SetProgress(currentProgress);
    }
}
