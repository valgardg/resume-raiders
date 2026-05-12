using TMPro;
using UnityEngine;

public class JobDescriptionScript : MonoBehaviour
{
    public TMP_Text companyNameText;
    public TMP_Text jobTitleText;
    public Transform desiredTraitsContainer;
    public Transform undesiredTraitsContainer;
    public GameObject traitEntryPrefab;

    public void SetJobDescriptionData(JobPostingSO jobDescriptionData)
    {
        companyNameText.text = jobDescriptionData.companyName;
        jobTitleText.text = jobDescriptionData.jobTitle;

        // populate desired traits
        foreach (var trait in jobDescriptionData.requiredTraits)
        {
            var traitGO = Instantiate(traitEntryPrefab, desiredTraitsContainer);
            var traitScript = traitGO.GetComponent<TraitEntry>();
            traitScript.Initialize(trait.ToString());
        }

        // populate undesired traits
        foreach (var trait in jobDescriptionData.disqualifyingTraits)
        {
            var traitGO = Instantiate(traitEntryPrefab, undesiredTraitsContainer);
            var traitScript = traitGO.GetComponent<TraitEntry>();
            traitScript.Initialize(trait.ToString());
        }
    }
}
