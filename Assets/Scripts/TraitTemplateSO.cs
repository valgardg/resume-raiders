using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TraitTemplate
{
    public ApplicantTrait trait;
    public string traitTitle;
    public string traitDescription;
    public List<JobExperienceTemplate> jobExperienceTemplates;
    public List<PersonalProjectTemplate> personalProjectTemplates;
    public List<EducationTemplate> educationTemplates;
    public List<string> funFactTemplates;
}

[System.Serializable]
public class JobExperienceTemplate
{
    public List<string> jobNameTemplates;
    [TextArea]
    public List<string> jobDescriptionTemplates;
}

[System.Serializable]
public class PersonalProjectTemplate
{
    public List<string> projectNames;

    [TextArea]
    public List<string> descriptionTemplates;
}

[System.Serializable]
public class EducationTemplate
{
    public List<string> schoolNames;
    public List<string> degreeNames;

    [TextArea]
    public List<string> descriptionTemplates;
}

[CreateAssetMenu(menuName = "ResumeRush/TraitTemplateDatabase")]
public class TraitTemplateDatabase : ScriptableObject
{
    public TraitTemplate[] templates;
}