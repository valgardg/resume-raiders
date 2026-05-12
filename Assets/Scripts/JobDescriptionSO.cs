// ============================================================
//  RESUME RUSH — ScriptableObject Data Classes
//  Drop these into a Scripts/Data/ folder in your Unity project.
// ============================================================

using System.Collections.Generic;
using UnityEngine;

// ────────────────────────────────────────────────────────────
//  2. JOB POSTING  (the requirements card shown each round)
// ────────────────────────────────────────────────────────────

[CreateAssetMenu(fileName = "NewJobPosting", menuName ="ResumeRush/JobPosting")]
public class JobPostingSO : ScriptableObject
{
    [Header("Company Info")]
    [Tooltip("e.g. 'Tech Startup Inc.'")]
    public string companyName;
    public Sprite companyLogo;   // optional, but would be fun to have a logo for

    [Header("Identity")]
    [Tooltip("e.g. 'Senior LooksMaxxing Engineer'")]
    public string jobTitle;

    [Header("Requirements — candidate MUST have ALL of these")]
    public List<ApplicantTrait> requiredTraits;        // 2 real + 1 silly

    [Header("Disqualifiers — candidate MUST have NONE of these")]
    public List<ApplicantTrait> disqualifyingTraits;   // 2 per the design doc
}