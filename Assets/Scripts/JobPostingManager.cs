using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class JobPostingManager : MonoBehaviour
{
    private GameStateSO gameState;
    private ApplicantData applicantData;

    public void Initialize(GameStateSO gameState, ApplicantData applicantData)
    {
        this.gameState = gameState;
        this.applicantData = applicantData;
    }

    public JobPostingSO GetJobPosting()
    {
        TraitTemplateDatabase currentTraitPool = applicantData.CurrentTraitPool;

        Debug.Log("got currentTraitPool: " + currentTraitPool.templates.Length + " traits");

        JobPostingSO jobPosting = ScriptableObject.CreateInstance<JobPostingSO>();
        jobPosting.requiredTraits = new List<ApplicantTrait>();
        jobPosting.disqualifyingTraits = new List<ApplicantTrait>();

        jobPosting.companyName = "Company " + Random.Range(1, 1000);
        jobPosting.jobTitle = "Job Title " + Random.Range(1, 1000);

         // assign random required and disqualifying traits from current trait pool
        
        for (int i = 0; i < gameState.desiredTraitsPerJobPosting; i++)
        {
            jobPosting.requiredTraits.Add(currentTraitPool.templates[Random.Range(0, currentTraitPool.templates.Length)].trait);
        }
        for (int i = 0; i < gameState.undesiredTraitsPerJobPosting; i++)
        {
            jobPosting.disqualifyingTraits.Add(currentTraitPool.templates[Random.Range(0, currentTraitPool.templates.Length)].trait);
        }
        return jobPosting;
    }
}
