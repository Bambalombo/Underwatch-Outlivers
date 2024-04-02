using System.Collections.Generic;
using UnityEngine;

public class TalentManager : MonoBehaviour
{
    [SerializeField] private GameManager _gameManager;

    public GameObject talentUIParent;
    public List<Talent> allTalents; // Populate this list in the Unity Editor
    public TalentUI[] talentUIElements; // Assign your TalentUI elements here in the Unity Editor

    void Start() {
        //Find the gamemanager
        _gameManager = GameObject.Find("GameManager").GetComponent<GameManager>(); //Ved godt vi laver en gameobject find 2 gange på samme ting men whatever
        


    }

    public void OpenTalentMenu(int newLevel)
    {
        Debug.Log("OPEN TALENT MENU");
        talentUIParent.SetActive(true);
        _gameManager.TogglePause();
        
    }

    public void GenerateRandomTalents() {
        HashSet<Talent> chosenTalents = new HashSet<Talent>();
        while (chosenTalents.Count < 3) {
            int randomIndex = Random.Range(0, allTalents.Count);
            chosenTalents.Add(allTalents[randomIndex]);
        }
        
        int i = 0;
        foreach (Talent talent in chosenTalents) {
            talentUIElements[i].SetTalent(talent);
            i++;
        }
    }

    public void TalentSelected(Talent selectedTalent) {
        // Handle the logic for when a talent is selected
        // For example, applying the talent's effects or upgrading the talent
        //Debug.Log($"Talent selected: {selectedTalent.name}");
        
        // For demonstration purposes, let's just increase the level of the selected talent
        //selectedTalent.level++;
        //Debug.Log($"Talent level increased to: {selectedTalent.level}");
        
        Debug.Log("CLOSE TALENT MENU");
        
        talentUIParent.SetActive(false);
        _gameManager.TogglePause();
        
    }
}