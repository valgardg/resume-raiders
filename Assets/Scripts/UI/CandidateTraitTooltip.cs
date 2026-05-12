using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CandidateTraitTooltip : MonoBehaviour
{
    public GameObject tooltipContainer;
    public RectTransform panelRoot; // Assign the top-most panel that needs to resize

    public TMP_Text traitNameTextUI;
    public TMP_Text traitDescriptionTextUI;

    // trait data to display
    public string traitNameText;
    [TextArea]
    public string traitDescriptionText;

    public void Start()
    {
        traitNameTextUI.text = traitNameText;
        traitDescriptionTextUI.text = traitDescriptionText;
    }

    public void ToggleTooltip()
    {
        tooltipContainer.SetActive(!tooltipContainer.activeSelf);

        // Rebuild from innermost to outermost
        LayoutRebuilder.ForceRebuildLayoutImmediate(tooltipContainer.GetComponent<RectTransform>());
        LayoutRebuilder.ForceRebuildLayoutImmediate(panelRoot);
    }
}