using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ApplicantData : MonoBehaviour
{
    private Dictionary<ApplicantTrait, int> generatedTraitCounts = new Dictionary<ApplicantTrait, int>();

    private List<ApplicantSO> applicantPool = new List<ApplicantSO>();
    public List<ApplicantSO> ApplicantPool => applicantPool;
    private TraitTemplateDatabase traitTemplateDatabase;

    public void Initialize()
    {
        GameManager.OnGameStart += LoadApplicantPool;
        GameManager.OnGameRestart += LoadApplicantPool;
        LoadApplicantPool();
    }

    // Load full trait template data from json
    private void LoadApplicantData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("TraitTemplateDatabase/traitTemplateDB");

        if (jsonFile == null)
        {
            Debug.LogError("JSON file not found!");
            return;
        }

        TraitTemplateDatabase data = ScriptableObject.CreateInstance<TraitTemplateDatabase>();
        JsonUtility.FromJsonOverwrite(jsonFile.text, data);
        traitTemplateDatabase = data; // Store the loaded data in the class variable for later use
    }

    // Generate a pool of applicants based on the trait templates
    private void LoadApplicantPool()
    {
        applicantPool.Clear();
        
        LoadApplicantData();

        for (int i = 0; i < 10; i++)
        {
            ApplicantSO applicant = GenerateApplicant(traitTemplateDatabase);
            applicantPool.Add(applicant);
        }

        Debug.Log("Final Trait Counts in Generated Applicants:");
        foreach (var kvp in generatedTraitCounts)            {
            Debug.Log("Trait: " + kvp.Key + ", Count: " + kvp.Value);
        }
    }

    public ApplicantSO GetNextAppliacnt()
    {
        if (applicantPool.Count == 0)
        {
            Debug.Log("No more applicants in the pool, making one!");
            return GenerateApplicant(traitTemplateDatabase);
        }

        int randomIndex = Random.Range(0, applicantPool.Count);
        ApplicantSO currentApplicantSO = applicantPool[randomIndex];
        applicantPool.RemoveAt(randomIndex);

        return currentApplicantSO;
    }

    // Generate a single applicant from our trait templates
    private ApplicantSO GenerateApplicant(TraitTemplateDatabase data)
    {
        ApplicantSO applicant = ScriptableObject.CreateInstance<ApplicantSO>();
        applicant.applicantName = "Applicant " + Random.Range(1, 1000);
        
        // Job Experiences
        applicant.jobExperiences = new List<ApplicantJobExperience>();

        // Projects
        applicant.personalProjects = new List<ApplicantPersonalProjects>();

        // Education
        applicant.education = new List<ApplicantEducation>();

        for (int j = 0; j < 2; j++)
        {
            TraitTemplate randomTemplate = data.templates[Random.Range(0, data.templates.Length)];
            Debug.Log("Selected trait for job experience: " + randomTemplate.trait);
            
            JobExperienceTemplate randomJobExperienceTemplate = randomTemplate.jobExperienceTemplates[Random.Range(0, randomTemplate.jobExperienceTemplates.Count)];
            applicant.jobExperiences.Add(new ApplicantJobExperience
            {
                trait = randomTemplate.trait,
                jobName = randomJobExperienceTemplate.jobNameTemplates[Random.Range(0, randomJobExperienceTemplate.jobNameTemplates.Count)],
                jobDescription = randomJobExperienceTemplate.jobDescriptionTemplates[Random.Range(0, randomJobExperienceTemplate.jobDescriptionTemplates.Count)]
            });
        }

        for (int j = 0; j < 2; j++)
        {
            TraitTemplate randomTemplate = data.templates[Random.Range(0, data.templates.Length)];
            
            PersonalProjectTemplate randomPersonalProjectTemplate = randomTemplate.personalProjectTemplates[Random.Range(0, randomTemplate.personalProjectTemplates.Count)];
            applicant.personalProjects.Add(new ApplicantPersonalProjects
            {
                trait = randomTemplate.trait,
                projectName = randomPersonalProjectTemplate.projectNames[Random.Range(0, randomPersonalProjectTemplate.projectNames.Count)],
                projectDescription = randomPersonalProjectTemplate.descriptionTemplates[Random.Range(0, randomPersonalProjectTemplate.descriptionTemplates.Count)]
            });
        }

        for (int j = 0; j < 1; j++)
        {
            TraitTemplate randomTemplate = data.templates[Random.Range(0, data.templates.Length)];
            
            EducationTemplate randomEducationTemplate = randomTemplate.educationTemplates[Random.Range(0, randomTemplate.educationTemplates.Count)];
            applicant.education.Add(new ApplicantEducation
            {
                trait = randomTemplate.trait,
                degreeName = randomEducationTemplate.degreeNames[Random.Range(0, randomEducationTemplate.degreeNames.Count)],
                institutionName = randomEducationTemplate.schoolNames[Random.Range(0, randomEducationTemplate.schoolNames.Count)],
                description = randomEducationTemplate.descriptionTemplates[Random.Range(0, randomEducationTemplate.descriptionTemplates.Count)]
            });
        }

        TraitTemplate funFactTemplate = data.templates[Random.Range(0, data.templates.Length)];
        
        applicant.funFact = new ApplicantFunFact
        {
            trait = funFactTemplate.trait,
            factDescription = funFactTemplate.funFactTemplates[Random.Range(0, funFactTemplate.funFactTemplates.Count)]
        };

        Debug.Log("Generated applicant: " + applicant.ToString());

        return applicant;
    }
}
