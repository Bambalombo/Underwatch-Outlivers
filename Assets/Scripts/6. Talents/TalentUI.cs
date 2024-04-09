using System;
using UnityEngine;
using TMPro;

public class TalentUI : MonoBehaviour {
    public TextMeshProUGUI talentNameText;
    public TextMeshProUGUI talentDescriptionText;
    // Reference to TalentManager to notify about talent selection
    public TalentManager talentManager;
    public Sprite talentSprite;
    public GameObject playerGameobject;

    // The talent this UI represents
    private Talent currentTalent;
    public GameObject localTalentPanel; //We use this to disable the panel when selected
    public GameObject doneSelectingPanel; //We can maybe add a gameobject here that we enable to signify that you are done selecting (while waiting for other players)
    

    public void SetTalent(Talent talent) {
        currentTalent = talent; // Store the current talent for reference
        Debug.Log($"Current talent = {currentTalent}");
        talentNameText.text = talent.name;
        talentDescriptionText.text = talent.description;
    }

    public void OnTalentSelected() {
        Debug.Log("Talent selected!");
        if (talentManager == null)
        {
            talentManager = GameObject.Find("TalentMenu").GetComponent<TalentManager>();
        }
        talentManager.TalentSelected(currentTalent, playerGameobject);
        
        localTalentPanel.SetActive(false);
    }

}