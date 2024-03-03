using UnityEngine;

public class PlayerExperience : MonoBehaviour
{
    [SerializeField] private IntVariable experience;
    [SerializeField] private IntVariable level;
    //[SerializeField] private int experience = 0;
    //[SerializeField] private int level = 0;
    [SerializeField] private int baseExperience = 100;
    //[SerializeField] private GameManager gameManager;

    private void Start()
    {
        experience.value = 0;
        level.value = 0;

        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();

        //gameManager.DisplayExperience(experience.value);
        //gameManager.DisplayLevel(level.value);
    }

    public void GainExperience(int gainedExperience)
    {
        experience.value += gainedExperience;
        
        //TODO: Find out why this is not working - GameManager is null but not in editor
        //gameManager.DisplayExperience(experience.value);
        
        ScaleLevel();
    }

    private void ScaleLevel()
    {
        int nextLevelExperience = baseExperience * (int)Mathf.Pow(2, level.value);
        while (experience.value >= nextLevelExperience)
        {
            LevelUp();
            nextLevelExperience = baseExperience * (int)Mathf.Pow(2, level.value);
        }
    }

    private void LevelUp()
    {
        level.value++;
        experience.value -= baseExperience * (int)Mathf.Pow(2, level.value - 1);
        
        //gameManager.DisplayExperience(experience);
        //gameManager.DisplayLevel(level);
    }
}