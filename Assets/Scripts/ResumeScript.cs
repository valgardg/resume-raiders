using TMPro;
using UnityEngine;

public class ResumeScript : MonoBehaviour
{
    public TMP_Text applicantName;
    public TMP_Text applicantFunFact;
    public Transform proffessionalExperienceContainer;
    public GameObject proffessionalExperiencePrefab;
    public Transform personalProjectsContainer;
    public GameObject personalProjectPrefab;
    public Transform educationContainer;
    public GameObject educationPrefab;

    public void SetResumeData(ApplicantSO resumeData)
    {
        applicantName.text = resumeData.applicantName;
        applicantFunFact.text = resumeData.funFact.factDescription;

        // populate resume sections
        foreach (var jobExperience in resumeData.jobExperiences)
        {
            var jobEntry = Instantiate(proffessionalExperiencePrefab, proffessionalExperienceContainer);
            var jobScript = jobEntry.GetComponent<SectionEntry>();
            jobScript.Initialize(sectionTitleText: jobExperience.jobName, sectionDescriptionText: jobExperience.jobDescription);
        }

        foreach (var personalProject in resumeData.personalProjects)
        {
            var projectGO = Instantiate(personalProjectPrefab, personalProjectsContainer);
            var projectScript = projectGO.GetComponent<SectionEntry>();
            projectScript.Initialize(sectionTitleText: personalProject.projectName, sectionDescriptionText: personalProject.projectDescription);
        }

        foreach (var education in resumeData.education)
        {
            var eduGO = Instantiate(educationPrefab, educationContainer);
            var eduScript = eduGO.GetComponent<SectionEntry>();
            eduScript.Initialize(sectionTitleText: education.degreeName, sectionDescriptionText: education.description, additionalInfoText: education.institutionName);
        }
    }
}