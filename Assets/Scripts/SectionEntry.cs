using TMPro;
using UnityEngine;

public class SectionEntry : MonoBehaviour
{

    public TMP_Text sectionTitle;
    public TMP_Text sectionDescription;
    public TMP_Text additionalInfo = null;   // optional, only used for fun facts
    public void Initialize(string sectionTitleText, string sectionDescriptionText, string additionalInfoText = null)
    {
        sectionTitle.text = sectionTitleText;
        sectionDescription.text = sectionDescriptionText;
        if (additionalInfo != null && additionalInfoText != null)
        {
            additionalInfo.text = additionalInfoText;
        }
    }
}
