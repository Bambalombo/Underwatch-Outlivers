using UnityEngine;

public class ExperienceController : MonoBehaviour
{
    [SerializeField] private IntVariable experience;
    [SerializeField] private IntVariable nextLevelExperience;
    [SerializeField] private IntVariable level;
    [SerializeField] private int baseExperience = 100;
    [SerializeField] private float levelScalingFactor = 1.5f;


    private void Start()
    {
        experience.value = 0;
        level.value = 1;
        nextLevelExperience.value = baseExperience;
    }

    public void AddExperience(int gainedExperience)
    {
        experience.value += gainedExperience;
        
        ScaleLevel();
    }

    private void ScaleLevel()
    {
        nextLevelExperience.value = baseExperience * (int)Mathf.Pow(levelScalingFactor, level.value);
        while (experience.value >= nextLevelExperience.value)
        {
            LevelUp();
            nextLevelExperience.value = baseExperience * (int)Mathf.Pow(levelScalingFactor, level.value);
        }
    }

    private void LevelUp()
    {
        level.value++;
        experience.value -= baseExperience * (int)Mathf.Pow(levelScalingFactor, level.value - 1);
    }
}