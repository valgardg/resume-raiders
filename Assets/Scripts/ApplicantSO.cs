using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewApplicant", menuName ="ResumeRush/Applicant")]
public class ApplicantSO : ScriptableObject
{
    [Header("Identity")]
    [Tooltip("e.g. 'Alice Smith'")]
    public string applicantName;
    public Sprite applicantPhoto;   // optional, but would be fun to have a photo for each applicant
    
    [Header("Resume Sections — each section implies a trait")]
    public List<ApplicantJobExperience> jobExperiences;   // 3 per the design doc
    public List<ApplicantPersonalProjects> personalProjects;   // 3 per the design doc
    public List<ApplicantEducation> education;   // 3 per the design doc
    public ApplicantFunFact funFact;   // 3 per the design doc

    // string override 
    public override string ToString()
    {
        // output name and all traits
        string output = applicantName + ":\n";
        foreach (var job in jobExperiences)
        {
            output += "- Job: " + job.jobName + " (" + job.trait + ")\n";
        }
        foreach (var project in personalProjects)
        {
            output += "- Personal Project: " + project.projectName + " (" + project.trait + ")\n";
        }
        foreach (var edu in education)
        {
            output += "- Education: " + edu.degreeName + " from " + edu.institutionName + " (" + edu.trait + ")\n";
        }
        output += "- Fun Fact: " + funFact.factDescription + " (" + funFact.trait + ")\n";
        return output;
    }
}


// ────────────────────────────────────────────────────────────
//  3. CLUE  (one piece of text inside a resume section)
//     A clue may imply a trait, or be a red-herring.
// ────────────────────────────────────────────────────────────

public enum ApplicantTrait
{
    LockingIn,
    CatHerding,
    GoblinMode,
    LooksMaxxing,
    Streaming,
    Moderating,
}

[System.Serializable]
public class ApplicantJobExperience
{
    public ApplicantTrait trait;
    public string jobName;
    [TextArea]
    public string jobDescription;   // e.g. "Led a team of 5 engineers to successfully launch a new product feature, resulting in a 20% increase in user engagement."
}

[System.Serializable]
public class ApplicantPersonalProjects
{
    public ApplicantTrait trait;
    public string projectName;
    [TextArea]
    public string projectDescription;   // e.g. "Led a team of 5 engineers to successfully launch a new product feature, resulting in a 20% increase in user engagement."
}

[System.Serializable]
public class ApplicantEducation
{
    public ApplicantTrait trait;
    public string degreeName;
    public string institutionName;
    [TextArea]
    public string description;   // e.g. "Graduated with honors, maintaining a
}

[System.Serializable]
public class ApplicantFunFact
{
    public ApplicantTrait trait;
    [TextArea]
    public string factDescription;   // e.g. "Graduated with honors, maintaining a 3.9 GPA while also leading the university's competitive programming team to victory in multiple national competitions."
}