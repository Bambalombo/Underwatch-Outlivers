using System;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject player;
    [SerializeField] private TextMeshProUGUI experienceText;
    [SerializeField] private TextMeshProUGUI levelText;
    [SerializeField] private IntVariable experience;
    [SerializeField] private IntVariable level;
    
    
    // Start is called before the first frame update
    void Awake()
    {
        // Instantiate player at the start of the game
        Instantiate(player, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void FixedUpdate()
    {
        DisplayExperience(experience.value);
        DisplayLevel(level.value);    
    }

    public void DisplayExperience(int exp)
    {
        experienceText.text = "Exp: " + exp;
    }
    
    public void DisplayLevel(int lvl)
    {
        levelText.text = "Lvl: " + lvl;
    }
    
}
