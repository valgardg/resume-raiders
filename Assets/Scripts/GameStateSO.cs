using System;
using System.Collections.Generic;
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
    public float timePenalty;

    [Header("Traits")]
    public List<TraitTemplate> traitPool = new();

    public void ResetState()
    {
        acceptedApplicants = 0;

        day = 1;
        timePenalty = 0f;
    }
}