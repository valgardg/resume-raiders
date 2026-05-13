using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class ResumeManager : MonoBehaviour
{
    private ApplicantSO currentApplicantSO;
    [SerializeField] private JobPostingSO currentJobPostingSO;
    public GameObject resumePrefab;
    public GameObject jobPostingPrefab;
    public Transform canvasContainer;

    private GameTime gameTime;

    private GameObject currentResumeGO;
    private GameObject currentJobPostingGO;
    private ApplicantData applicantData;
    private JobPostingManager jobPostingManager;

    private GameStateSO gameState;

    public void Initialize(GameStateSO gameState, GameTime gameTime, ApplicantData applicantData, JobPostingManager jobPostingManager)
    {
        this.gameState = gameState;
        this.gameTime = gameTime;
        this.applicantData = applicantData;
        this.jobPostingManager = jobPostingManager;
        GameManager.OnGameStart += GetNextResume;
        GameManager.OnGameRestart += GetNextResume;
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
            Debug.LogError("Current Job Posting SO is not set, generating one!");
            currentJobPostingSO = jobPostingManager.GetJobPosting();
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
            gameState.IncrementHires();
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

    private void GetNextResume()
    {
        currentApplicantSO = applicantData.GetNextAppliacnt();

        if (currentApplicantSO != null)
        {
            InitializeApplicantResume();
        }
        else
        {
            Debug.Log("No more applicants available!");
            // Handle end of applicant pool (e.g., show game over screen or restart)
        }
    }
}
