using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResumeManager : MonoBehaviour
{
    private ApplicantSO currentApplicantSO;
    public JobPostingSO currentJobPostingSO;
    public GameObject resumePrefab;
    public GameObject jobPostingPrefab;
    public Transform canvasContainer;

    private GameTime gameTime;

    private GameObject currentResumeGO;
    private GameObject currentJobPostingGO;

    private Dictionary<ApplicantTrait, int> generatedTraitCounts = new Dictionary<ApplicantTrait, int>();

    private List<ApplicantSO> applicantPool = new List<ApplicantSO>();

    public void Initialize(GameTime gameTime)
    {
        this.gameTime = gameTime;
        GameManager.OnGameStart += InitializeResumes;
        GameManager.OnGameRestart += InitializeResumes;
    }

    private void InitializeResumes()
    {
        LoadApplicantPool();
        GetNextResume();

        Debug.Log("Trait counts in generated applicants:");
        foreach (var traitCount in generatedTraitCounts) {
            Debug.Log(traitCount.Key + ": " + traitCount.Value);
        }
    }

    public void InitializeApplicantResume()
    {
        if (currentApplicantSO == null)
        {
            Debug.LogError("Current Applicant SO is not set!");
            return;
        }

        if (currentResumeGO != null)
        {
            Destroy(currentResumeGO);
        }

        // Instantiate the resume prefab and set the data
        var resumeGO = Instantiate(resumePrefab, canvasContainer);
        var resumeScript = resumeGO.GetComponent<ResumeScript>();
        resumeScript.SetResumeData(currentApplicantSO);
        currentResumeGO = resumeGO;
    }

    public void ToggleJobPosting()
    {
        if (currentJobPostingGO != null)
        {
            currentJobPostingGO.SetActive(!currentJobPostingGO.activeSelf);
        } else
        {
            InitializeJobPosting();
        }
    }

    public void InitializeJobPosting()
    {
        if (currentJobPostingSO == null)
        {
            Debug.LogError("Current Job Posting SO is not set!");
            return;
        }

        // Instantiate the job posting prefab and set the data
        var jobPostingGO = Instantiate(jobPostingPrefab, canvasContainer);
        var jobPostingScript = jobPostingGO.GetComponent<JobDescriptionScript>();
        jobPostingScript.SetJobDescriptionData(currentJobPostingSO);
        currentJobPostingGO = jobPostingGO;
    }

    public void AcceptCurrentApplicant()
    {
        if (currentApplicantSO == null || currentJobPostingSO == null)
        {
            Debug.LogError("Current Applicant SO or Job Posting SO is not set!");
            return;
        }

        ApplicantTrait? firstMissingTrait;
        ApplicantTrait? firstDisqualifyingTrait;

        bool meetsRequirements = MeetsRequirements(currentJobPostingSO, currentApplicantSO, out firstMissingTrait, out firstDisqualifyingTrait);

        if (meetsRequirements)
        {
            Debug.Log("Resume meets requirements!");
        }
        else
        {
            Debug.Log("Resume does not meet requirements! Reason: " + (firstMissingTrait != null ? "Missing required trait: " + firstMissingTrait : "Has disqualifying trait: " + firstDisqualifyingTrait));
            gameTime.ReduceTime(10f); // Reduce time by 10 seconds for incorrect decision
        }

        GetNextResume();
    }

    public void RejectCurrentApplicant()
    {
        if (currentApplicantSO == null || currentJobPostingSO == null)
        {
            Debug.LogError("Current Applicant SO or Job Posting SO is not set!");
            return;
        }

        Debug.Log("Applicant Rejected: " + currentApplicantSO.applicantName);
        bool meetsRequirements = MeetsRequirements(currentJobPostingSO, currentApplicantSO, out _, out _);
        if (meetsRequirements)
        {
            gameTime.ReduceTime(10f); // Reduce time by 10 seconds for incorrect decision
        }

        GetNextResume();
    }

    private List<ApplicantTrait> GetApplicantTraits(ApplicantSO applicant)
    {
        List<ApplicantTrait> traits = new List<ApplicantTrait>();

        foreach (var job in applicant.jobExperiences)
        {
            traits.Add(job.trait);
        }

        foreach (var project in applicant.personalProjects)
        {
            traits.Add(project.trait);
        }

        foreach (var education in applicant.education)
        {
            traits.Add(education.trait);
        }

        traits.Add(applicant.funFact.trait);

        return traits;
    }

    private bool MeetsRequirements(JobPostingSO jobPosting, ApplicantSO applicant,out ApplicantTrait? firstMissingTrait, out ApplicantTrait? firstDisqualifyingTrait)
    {
        firstMissingTrait = null;
        firstDisqualifyingTrait = null;
        List<ApplicantTrait> applicantTraits = GetApplicantTraits(applicant);
        bool meetsRequirements = true;

        // Check if applicant has all required traits
        foreach (var requiredTrait in jobPosting.requiredTraits)
        {
            if (!applicantTraits.Contains(requiredTrait))
            {
                meetsRequirements = false;
                firstMissingTrait = requiredTrait;
                Debug.Log("Applicant is missing required trait: " + requiredTrait);
                break;
            }
        }

        // Check if applicant has any disqualifying traits
        foreach (var disqualifyingTrait in jobPosting.disqualifyingTraits)
        {
            if (applicantTraits.Contains(disqualifyingTrait))
            {
                meetsRequirements = false;
                firstDisqualifyingTrait = disqualifyingTrait;
                Debug.Log("Applicant has disqualifying trait: " + disqualifyingTrait);
                break;
            }
        }

        return meetsRequirements;
    }

    private void LoadApplicantPool()
    {
        applicantPool.Clear();
        
        TraitTemplateDatabase data = LoadApplicantData();

        for (int i = 0; i < 10; i++)
        {
            GenerateApplicant(data);
        }

        Debug.Log("Final Trait Counts in Generated Applicants:");
        foreach (var kvp in generatedTraitCounts)            {
            Debug.Log("Trait: " + kvp.Key + ", Count: " + kvp.Value);
        }
    }

    private TraitTemplateDatabase LoadApplicantData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("TraitTemplateDatabase/traitTemplateDB");

        if (jsonFile == null)
        {
            Debug.LogError("JSON file not found!");
            return null;
        }

        TraitTemplateDatabase data = ScriptableObject.CreateInstance<TraitTemplateDatabase>();
        JsonUtility.FromJsonOverwrite(jsonFile.text, data);

        return data;
    }

    private void GenerateApplicant(TraitTemplateDatabase data)
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

        // Count traits for debugging
        List<ApplicantTrait> traits = GetApplicantTraits(applicant);
        foreach (var trait in traits)        {
            if (generatedTraitCounts.ContainsKey(trait))
            {                generatedTraitCounts[trait]++;
            }
            else            {
                generatedTraitCounts[trait] = 1;
            }
        }

        applicantPool.Add(applicant);
    }

    private void GetNextResume()
    {
        Destroy(currentResumeGO);

        if (applicantPool.Count == 0)
        {
            Debug.Log("No more applicants in the pool!");
            return;
        }

        int randomIndex = Random.Range(0, applicantPool.Count);
        currentApplicantSO = applicantPool[randomIndex];
        applicantPool.RemoveAt(randomIndex);

        InitializeApplicantResume();
    }
}
