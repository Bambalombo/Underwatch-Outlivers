using UnityEngine;
using System;

public class ExperienceController : MonoBehaviour
{
    [SerializeField] private IntVariable experience;
    [SerializeField] private IntVariable nextLevelExperience;
    [SerializeField] private IntVariable level;
    [SerializeField] private int baseExperience = 100;
    [SerializeField] private float levelScalingFactor = 1.5f;
    private TalentManager _talentManager;
    
    //private UIManager uiManager;


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
        //OK DET HER ER VIRKELIG DUMT, VILLE HAVE LAVET EN EVENT MEN DEN FUCKEDE FKING MEGET MED MIG SÅ JEG WHIPPEDE SPAGETTHI KODEN UD GRR
        if (_talentManager == null)
        {
            _talentManager = GameObject.Find("TalentMenu").GetComponent<TalentManager>();
        }
        //Debug.Log("LEVEL UP");
        level.value++;
        experience.value -= baseExperience * (int)Mathf.Pow(levelScalingFactor, level.value - 1);
        
        // TODO: Could not get the reference from GameManager for some reason
        var uiManager = FindObjectOfType<UIManager>().GetComponent<UIManager>();
        uiManager.UpdateExpUI();
        
        _talentManager.OpenTalentMenu(level.value);

    }
}