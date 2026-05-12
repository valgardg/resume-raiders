using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

public class ApplicantGenerator
{
    private static string[] firstNames = { "Alice", "Bob", "Charlie", "Diana", "Evan", "Fiona" };
    private static string[] lastNames = { "Smith", "Johnson", "Lee", "Brown", "Garcia", "Nguyen" };

    private static string[] jobTitles = { "Software Engineer", "Barista", "Game Dev", "Data Analyst" };
    private static string[] projectNames = { "AI Chatbot", "Weather App", "Indie Game", "Portfolio Website" };
    private static string[] degrees = { "BSc Computer Science", "BA Design", "MSc AI" };
    private static string[] schools = { "MIT", "Stanford", "Local University", "Online Bootcamp" };

    [MenuItem("Tools/Generate Applicants")]
    public static void GenerateApplicants()
    {
        int count = 10;

        for (int i = 0; i < count; i++)
        {
            ApplicantSO applicant = ScriptableObject.CreateInstance<ApplicantSO>();

            // Name
            applicant.applicantName = GetRandom(firstNames) + " " + GetRandom(lastNames);

            // Job Experiences
            applicant.jobExperiences = new List<ApplicantJobExperience>();
            for (int j = 0; j < 3; j++)
            {
                applicant.jobExperiences.Add(new ApplicantJobExperience
                {
                    trait = GetRandomTrait(),
                    jobName = GetRandom(jobTitles),
                    jobDescription = "Did something impressive involving " + Random.value
                });
            }

            // Projects
            applicant.personalProjects = new List<ApplicantPersonalProjects>();
            for (int j = 0; j < 3; j++)
            {
                applicant.personalProjects.Add(new ApplicantPersonalProjects
                {
                    trait = GetRandomTrait(),
                    projectName = GetRandom(projectNames),
                    projectDescription = "Built a project that somehow worked."
                });
            }

            // Education
            applicant.education = new List<ApplicantEducation>();
            for (int j = 0; j < 3; j++)
            {
                applicant.education.Add(new ApplicantEducation
                {
                    trait = GetRandomTrait(),
                    degreeName = GetRandom(degrees),
                    institutionName = GetRandom(schools),
                    description = "Learned things. Some of them useful."
                });
            }

            // Fun Fact
            applicant.funFact = new ApplicantFunFact
            {
                trait = GetRandomTrait(),
                factDescription = "Can fold a fitted sheet. Allegedly."
            };

            // Save asset
            string path = $"Assets/Applicants/Applicant_{i}.asset";
            AssetDatabase.CreateAsset(applicant, path);
        }

        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();

        Debug.Log("Applicants generated!");
    }

    private static T GetRandom<T>(T[] array)
    {
        return array[Random.Range(0, array.Length)];
    }

    private static ApplicantTrait GetRandomTrait()
    {
        var values = System.Enum.GetValues(typeof(ApplicantTrait));
        return (ApplicantTrait)values.GetValue(Random.Range(0, values.Length));
    }
}