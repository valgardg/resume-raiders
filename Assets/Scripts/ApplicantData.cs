using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ApplicantData : MonoBehaviour
{
    private TraitTemplateDatabase traitTemplateDatabase;
    private TraitTemplateDatabase currentTraitPool;
    public TraitTemplateDatabase CurrentTraitPool => currentTraitPool;

    private GameStateSO gameState;

    public void Initialize(GameStateSO gameState)
    {
        this.gameState = gameState;

        LoadApplicantData();
        GameManager.OnGameRestart += LoadApplicantData;
    }

    // Load full trait template data from json and populate initial traiat pool
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

         // set trait pool as only two random traits from full list
        currentTraitPool = ScriptableObject.CreateInstance<TraitTemplateDatabase>();
        currentTraitPool.templates = new TraitTemplate[gameState.traitPoolSize];
        for (int i = 0; i < gameState.traitPoolSize; i++)
        {
            TraitTemplate randomTemplate = traitTemplateDatabase.templates[Random.Range(0, traitTemplateDatabase.templates.Length)];
            while (System.Array.Exists(currentTraitPool.templates, t => t == randomTemplate))
            {
                randomTemplate = traitTemplateDatabase.templates[Random.Range(0, traitTemplateDatabase.templates.Length)];
            }
            currentTraitPool.templates[i] = randomTemplate;
            Debug.Log("Selected trait for current pool: " + randomTemplate.trait);
        }
    }

    public ApplicantSO GetNextAppliacnt()
    {
        return GenerateApplicant(currentTraitPool);
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
