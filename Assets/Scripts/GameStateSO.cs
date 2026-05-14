using UnityEngine;

[CreateAssetMenu(
    fileName = "GameState",
    menuName = "Game/Game State"
)]
public class GameStateSO : ScriptableObject
{
    [Header("Hiring Goals")]
    public int requiredApplicants;
    public int acceptedApplicants;

    [Header("Time")]
    public int day = 1;

    [Header("Trait related logic")]
    public int traitPoolSize = 3; // trait pool size of applicant traits
    public int desiredTraitsPerJobPosting = 1; // how many traits each job posting requires
    public int undesiredTraitsPerJobPosting = 1; // how many undesired traits each job posting has


    public event System.Action OnFullfilledHires;

    public void ResetState()
    {
        acceptedApplicants = 0;
        day = 1;
        traitPoolSize = 3;
    }

    public void IncrementHires()
    {
        acceptedApplicants++;
        if (acceptedApplicants >= requiredApplicants)
        {
            OnFullfilledHires?.Invoke();
        }
    }
}